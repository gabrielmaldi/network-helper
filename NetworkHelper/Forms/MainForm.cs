using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;
using DotRas;
using Microsoft.Win32.TaskScheduler;
using NetworkHelper.Classes;
using NetworkHelper.Extensions;
using NetworkHelper.Utilities;

namespace NetworkHelper.Forms
{
    public partial class MainForm : Form
    {
        #region Constants

        private static readonly Color DarkYellow = Color.FromArgb(200, 200, 0);

        #endregion

        #region Instance variables

        private bool _isExiting;
        private bool _isChangingAutoStartToolStripMenuItemCheckedProgrammatically;
        private int _lastDeactivatedTickCount;
        private RasConnectionWatcher _rasConnectionWatcher;

        #endregion

        #region Form and controls methods and events

        public MainForm()
        {
            InitializeComponent();

            Initialize();
        }

        private void Initialize()
        {
            Logger.Instance.Logged += Logger_Logged;

            Logger.Instance.Log(LogLevel.Info, "NetworkHelper started.");

            UpdateAutoStartTaskPath();
            LoadConfigurationAndUpdateConnectionsSettings();
            InitializeConnectionWatcher();
            CreateManualRouteProcessingMenuItems();
        }

        protected override void WndProc(ref Message message)
        {
            if (message.Msg == SingleApplicationHelper.WM_NEWINSTANCE)
            {
                Visible = true;
                Activate();
            }
            else
            {
                base.WndProc(ref message);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isExiting && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Visible = false;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _rasConnectionWatcher.Dispose();
            Logger.Instance.Logged -= Logger_Logged;
        }
        
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Visible = false;
            }
        }

        protected override void SetVisibleCore(bool value)
        {
            if (!IsHandleCreated)
            {
                CreateHandle();
                value = false;
            }

            base.SetVisibleCore(value);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            base.OnDeactivate(e);

            _lastDeactivatedTickCount = Environment.TickCount;
        }

        private void notifyIconMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (Visible && Environment.TickCount - _lastDeactivatedTickCount < 1000)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Visible = false;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    toggleLogVisibilityToolStripMenuItem.Tag = false;
                    toggleLogVisibilityToolStripMenuItem.Text = "Hide log";
                }
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    Visible = true;
                    Activate();
                }
                else if (e.Button == MouseButtons.Right)
                {
                    toggleLogVisibilityToolStripMenuItem.Tag = true;
                    toggleLogVisibilityToolStripMenuItem.Text = "Show log";
                }
            }
        }

        private void contextMenuStripMain_Opening(object sender, CancelEventArgs e)
        {
            using (TaskService taskService = new TaskService())
            {
                _isChangingAutoStartToolStripMenuItemCheckedProgrammatically = true;
                autoStartToolStripMenuItem.Checked = taskService.GetTask(GetAutoStartTaskName()) != null;
                _isChangingAutoStartToolStripMenuItemCheckedProgrammatically = false;
            }
        }

        private void contextMenuStripMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Tuple<bool, Vpn> tag = e.ClickedItem.Tag as Tuple<bool, Vpn>;
            if (tag != null)
            {
                Route[] validAndEnabledRoutes;
                if (HasValidAndEnabledRoutes(tag.Item1, tag.Item2, out validAndEnabledRoutes))
                {
                    if (tag.Item1)
                    {
                        if (!string.IsNullOrEmpty(tag.Item2.GatewayIpAddress))
                        {
                            ProcessRoutes(tag.Item1, tag.Item2.Name, validAndEnabledRoutes, tag.Item2.GatewayIpAddress);
                        }
                        else
                        {
                            RasConnection activeConnection = RasConnection.GetActiveConnections().SingleOrDefault(connection => connection.EntryName.Equals(tag.Item2.Name, StringComparison.OrdinalIgnoreCase));
                            if (activeConnection != null)
                            {
                                ProcessRoutes(tag.Item1, tag.Item2.Name, validAndEnabledRoutes, ((RasIPInfo)activeConnection.GetProjectionInfo(RasProjectionType.IP)).IPAddress.ToString());
                            }
                            else
                            {
                                IpAddressInputForm ipAddressInputForm = new IpAddressInputForm(tag.Item2);
                                ipAddressInputForm.Completed += (completedSender, completedArgs) =>
                                {
                                    if (completedArgs.DialogResult == DialogResult.OK)
                                    {
                                        ProcessRoutes(tag.Item1, tag.Item2.Name, validAndEnabledRoutes, completedArgs.IpAddress);
                                    }
                                };
                                ipAddressInputForm.Show();
                            }
                        }
                    }
                    else
                    {
                        ProcessRoutes(tag.Item1, tag.Item2.Name, validAndEnabledRoutes, null);
                    }
                }
            }
        }

        private void toggleLogVisibilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool? makeVisible = toggleLogVisibilityToolStripMenuItem.Tag as bool?;
            if (!makeVisible.HasValue || makeVisible.Value)
            {
                Visible = true;
                Activate();
            }
            else
            {
                Visible = false;
            }
        }

        private void autoStartToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (!_isChangingAutoStartToolStripMenuItemCheckedProgrammatically)
            {
                using (TaskService taskService = new TaskService())
                {
                    if (autoStartToolStripMenuItem.Checked)
                    {
                        CreateAutoStartTask(taskService);

                        Logger.Instance.Log(LogLevel.Info, "Created auto-start task in Task Scheduler.");
                    }
                    else
                    {
                        DeleteAutoStartTask(taskService);

                        Logger.Instance.Log(LogLevel.Info, "Deleted auto-start task from Task Scheduler.");
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _isExiting = true;
            Close();
        }

        private void _rasConnectionWatcher_Connected(object sender, RasConnectionEventArgs e)
        {
            ProcessRoutes(true, e);
        }

        private void _rasConnectionWatcher_Disconnected(object sender, RasConnectionEventArgs e)
        {
            ProcessRoutes(false, e);
        }

        private void Logger_Logged(object sender, LoggedEventArgs e)
        {
            richTextBoxLog.InvokeIfRequired(log => log.AppendLine(e.Message, LogLevelToColor(e.LogLevel)));
        }

        #endregion

        #region Instance helper methods

        private void UpdateAutoStartTaskPath()
        {
            using (TaskService taskService = new TaskService())
            {
                Task task = taskService.GetTask(GetAutoStartTaskName());
                if (task != null)
                {
                    ExecAction action = (ExecAction)task.Definition.Actions.Single();

                    bool isTaskScheduler2OrLater = taskService.HighestSupportedVersion.Minor >= 2;

                    if (!PathHelper.ArePathsEqual(action.Path, Application.ExecutablePath) || (isTaskScheduler2OrLater && !PathHelper.ArePathsEqual(action.WorkingDirectory, WinFormsHelper.ExecutableDirectory)))
                    {
                        if (isTaskScheduler2OrLater)
                        {
                            action.Path = Application.ExecutablePath;
                            action.WorkingDirectory = WinFormsHelper.ExecutableDirectory;
                            task.RegisterChanges();
                        }
                        else
                        {
                            DeleteAutoStartTask(taskService);
                            CreateAutoStartTask(taskService);
                        }

                        Logger.Instance.Log(LogLevel.Warning, "Updated path of executable in auto-start task in TaskScheduler.");
                    }
                }
            }
        }

        private void LoadConfigurationAndUpdateConnectionsSettings()
        {
            ConfigurationManager.LoadConfiguration();

            foreach (Vpn vpn in ConfigurationManager.Configuration.Vpns.Where(vpn => vpn.IsValid))
            {
                Logger.Instance.Log(LogLevel.Info, "Loaded \"{0}\" from configuration. Number of routes: {1}. {2}.", vpn.Name, vpn.Routes.Where(route => route.IsValid && route.IsEnabled).Count(), vpn.IsEnabled ? "Enabled" : "Not enabled");
            }

            IEnumerable<RasEntry> phoneBookConnections;

            using (RasPhoneBook userPhoneBook = new RasPhoneBook())
            using (RasPhoneBook systemPhoneBook = new RasPhoneBook())
            {
                userPhoneBook.Open(RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User));
                systemPhoneBook.Open(RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.AllUsers));

                phoneBookConnections = userPhoneBook.Entries.Where(entry => entry.EntryType == RasEntryType.Vpn).Concat(systemPhoneBook.Entries.Where(entry => entry.EntryType == RasEntryType.Vpn));
            }

            foreach (var phoneBookConnectionAndVpn in phoneBookConnections.Join(ConfigurationManager.Configuration.Vpns.Where(vpn => vpn.IsValid && vpn.IsEnabled && !vpn.IsOnlyTriggeredManually), entry => entry.Name, vpn => vpn.Name, (entry, vpn) => new { PhoneBookConnection = entry, Vpn = vpn }, (entryName, vpnName) => entryName.Equals(vpnName, StringComparison.OrdinalIgnoreCase), name => name.GetHashCode()))
            {
                Logger.Instance.Log(LogLevel.Info, "Matched VPN \"{0}\" from configuration to Windows VPN.", phoneBookConnectionAndVpn.Vpn.Name);

                if (phoneBookConnectionAndVpn.PhoneBookConnection.Device != null)
                {
                    bool update = false;

                    if (phoneBookConnectionAndVpn.PhoneBookConnection.Options.RemoteDefaultGateway)
                    {
                        phoneBookConnectionAndVpn.PhoneBookConnection.Options.RemoteDefaultGateway = false;
                        update = true;

                        Logger.Instance.Log(LogLevel.Info, "Updated \"{0}\" to not use default gateway on remote network, in order to allow internet access using local connection.", phoneBookConnectionAndVpn.Vpn.Name);
                    }

                    if (!phoneBookConnectionAndVpn.PhoneBookConnection.Options.DoNotUseRasCredentials)
                    {
                        phoneBookConnectionAndVpn.PhoneBookConnection.Options.DoNotUseRasCredentials = true;
                        update = true;

                        Logger.Instance.Log(LogLevel.Info, "Updated \"{0}\" to not use RAS credentials, in order to allow access to local resources using Windows Integrated Security.", phoneBookConnectionAndVpn.Vpn.Name);
                    }

                    if (!string.IsNullOrEmpty(phoneBookConnectionAndVpn.Vpn.DnsSuffix) && phoneBookConnectionAndVpn.PhoneBookConnection.DnsSuffix != phoneBookConnectionAndVpn.Vpn.DnsSuffix)
                    {
                        phoneBookConnectionAndVpn.PhoneBookConnection.DnsSuffix = phoneBookConnectionAndVpn.Vpn.DnsSuffix;
                        update = true;

                        Logger.Instance.Log(LogLevel.Info, "Updated the DNS suffix of \"{0}\" to \"{1}\".", phoneBookConnectionAndVpn.Vpn.Name, phoneBookConnectionAndVpn.Vpn.DnsSuffix);
                    }

                    if (update)
                    {
                        phoneBookConnectionAndVpn.PhoneBookConnection.Update();
                    }
                }
                else
                {
                    phoneBookConnectionAndVpn.Vpn.IsOnlyTriggeredManually = false;

                    Logger.Instance.Log(LogLevel.Error, "VPN \"{0}\" has no associated device and will only be triggered manually (this can be fixed by deleting the VPN and creating it again).", phoneBookConnectionAndVpn.Vpn.Name);
                }
            }
        }

        private void InitializeConnectionWatcher()
        {
            _rasConnectionWatcher = new RasConnectionWatcher();
            _rasConnectionWatcher.Connected += _rasConnectionWatcher_Connected;
            _rasConnectionWatcher.Disconnected += _rasConnectionWatcher_Disconnected;
            _rasConnectionWatcher.EnableRaisingEvents = true;
        }

        private void CreateManualRouteProcessingMenuItems()
        {
            Func<Vpn, bool, ToolStripMenuItem> createMenuItem = (vpn, addRoutes) => new ToolStripMenuItem(string.Format(CultureInfo.InvariantCulture, "{0} routes for \"{1}\"", addRoutes ? "Add" : "Remove", vpn.Name), addRoutes ? manuallyAddRoutesTemplateToolStripMenuItem.Image : manuallyRemoveRoutesTemplateToolStripMenuItem.Image) { Tag = Tuple.Create(addRoutes, vpn) };

            bool wasAtLeastOneMenuItemAdded = false;

            int index = 0;
            foreach (Vpn vpn in ConfigurationManager.Configuration.Vpns.Where(vpn => vpn.IsValid && vpn.IsEnabled && HasValidAndEnabledRoutes(vpn)))
            {
                if (!wasAtLeastOneMenuItemAdded)
                {
                    contextMenuStripMain.Items.Insert(index, new ToolStripSeparator());
                    wasAtLeastOneMenuItemAdded = true;
                }

                contextMenuStripMain.Items.Insert(index, createMenuItem(vpn, true));
                contextMenuStripMain.Items.Insert(index * 2 + 1, createMenuItem(vpn, false));
                index++;
            }

            if (wasAtLeastOneMenuItemAdded)
            {
                contextMenuStripMain.Items.Insert(index, new ToolStripSeparator());
            }
        }

        #endregion

        #region Class helper methods

        private static void ProcessRoutes(bool isConnected, RasConnectionEventArgs connectionEventInfo)
        {
            if (connectionEventInfo.Connection.Device.DeviceType == RasDeviceType.Vpn)
            {
                string gatewayIpAddress;

                if (isConnected)
                {
                    gatewayIpAddress = ((RasIPInfo)connectionEventInfo.Connection.GetProjectionInfo(RasProjectionType.IP)).IPAddress.ToString();

                    Logger.Instance.Log(LogLevel.Info, "Connected to \"{0}\". Client IP: {1}.", connectionEventInfo.Connection.EntryName, gatewayIpAddress);
                }
                else
                {
                    gatewayIpAddress = null;

                    Logger.Instance.Log(LogLevel.Info, "Disconnected from \"{0}\".", connectionEventInfo.Connection.EntryName);
                }

                Vpn configuredVpn = ConfigurationManager.Configuration.Vpns.SingleOrDefault(vpn => !vpn.IsOnlyTriggeredManually && vpn.IsValid && vpn.Name.Equals(connectionEventInfo.Connection.EntryName, StringComparison.OrdinalIgnoreCase));
                if (configuredVpn != null)
                {
                    if (configuredVpn.IsEnabled)
                    {
                        Route[] validAndEnabledRoutes;
                        if (HasValidAndEnabledRoutes(isConnected, configuredVpn, out validAndEnabledRoutes))
                        {
                            ProcessRoutes(isConnected, connectionEventInfo.Connection.EntryName, validAndEnabledRoutes, gatewayIpAddress);
                        }
                    }
                    else
                    {
                        Logger.Instance.Log(LogLevel.Info, "No routes will be {0} for \"{1}\" because it is not enabled.", isConnected ? "added" : "deleted", connectionEventInfo.Connection.EntryName);
                    }
                }
                else
                {
                    Logger.Instance.Log(LogLevel.Info, "No routes will be {0} for \"{1}\" because it has not been configured.", isConnected ? "added" : "deleted", connectionEventInfo.Connection.EntryName);
                }
            }
        }

        private static void ProcessRoutes(bool addRoutes, string vpnName, Route[] validAndEnabledRoutes, string gatewayIpAddress)
        {
            int routeIndex = 0;
            foreach (CmdRouteManagementResult routeManagementResult in addRoutes ? CmdRouteManager.AddRoutes(validAndEnabledRoutes, gatewayIpAddress) : CmdRouteManager.DeleteRoutes(validAndEnabledRoutes))
            {
                Route route = validAndEnabledRoutes[routeIndex];

                switch (routeManagementResult.Code)
                {
                    case CmdRouteManagementResultCode.Success:
                        Logger.Instance.Log(LogLevel.Info, "{0} route {1} for \"{2}\"{3}.", addRoutes ? "Added" : "Deleted", route, vpnName, addRoutes ? string.Format(CultureInfo.InvariantCulture, " ({0})", gatewayIpAddress) : string.Empty);
                        break;
                    case CmdRouteManagementResultCode.ErrorNoActiveNetworkConnection:
                        Logger.Instance.Log(LogLevel.Warning, "Could not {0} route {1} for \"{2}\" because there are no active network connections.", addRoutes ? "add" : "delete", route, vpnName);
                        break;
                    case CmdRouteManagementResultCode.ErrorRouteAlreadyExists:
                        Logger.Instance.Log(LogLevel.Warning, "Route {0} already exists for \"{1}\".", route, vpnName);
                        break;
                    case CmdRouteManagementResultCode.ErrorRouteNotFound:
                        Logger.Instance.Log(LogLevel.Warning, "Route {0} not found for \"{1}\". Keep in mind that Windows, almost always, deletes routes on disconnection.", route, vpnName);
                        break;
                    default:
                        Logger.Instance.Log(LogLevel.Error, "Could not {0} route {1} for \"{2}\". Reason: \"{3}\".", addRoutes ? "add" : "delete", route, vpnName, routeManagementResult.Message);
                        break;
                }

                routeIndex++;
            }
        }

        private static bool HasValidAndEnabledRoutes(bool addRoutes, Vpn vpn, out Route[] validAndEnabledRoutes)
        {
            bool result;

            validAndEnabledRoutes = vpn.Routes != null ? vpn.Routes.Where(route => route.IsValid && route.IsEnabled).ToArray() : null;
            result = validAndEnabledRoutes != null && validAndEnabledRoutes.Any();
            if (!result)
            {
                Logger.Instance.Log(LogLevel.Info, "No routes will be {0} for \"{1}\" because no valid and enabled routes have been configured.", addRoutes ? "added" : "deleted", vpn.Name);
            }

            return result;
        }

        private static bool HasValidAndEnabledRoutes(Vpn vpn)
        {
            return vpn.Routes != null && vpn.Routes.Any(route => route.IsValid && route.IsEnabled);
        }

        private static void CreateAutoStartTask(TaskService taskService)
        {
            WindowsIdentity currentUser = WindowsIdentity.GetCurrent();

            TaskDefinition taskDefinition = taskService.NewTask();

            taskDefinition.RegistrationInfo.Description = string.Format(CultureInfo.InvariantCulture, "Starts NetworkHelper when \"{0}\" logs on.", currentUser.Name);
            taskDefinition.RegistrationInfo.Author = "NetworkHelper";
            if (taskService.HighestSupportedVersion.Minor >= 2)
            {
                taskDefinition.RegistrationInfo.Date = DateTime.Now;
                taskDefinition.Principal.RunLevel = TaskRunLevel.Highest;
            }
            taskDefinition.Settings.DisallowStartIfOnBatteries = false;
            taskDefinition.Triggers.Add(new LogonTrigger { UserId = currentUser.User.Value });
            taskDefinition.Actions.Add(new ExecAction { Path = Application.ExecutablePath, WorkingDirectory = WinFormsHelper.ExecutableDirectory });

            taskService.RootFolder.RegisterTaskDefinition(GetAutoStartTaskName(), taskDefinition);
        }

        private static void DeleteAutoStartTask(TaskService taskService)
        {
            taskService.RootFolder.DeleteTask(GetAutoStartTaskName());
        }

        private static string GetAutoStartTaskName()
        {
            return "NetworkHelper_" + WindowsIdentity.GetCurrent().Name.Replace('\\', '-');
        }

        private static Color LogLevelToColor(LogLevel logLevel)
        {
            Color result;

            if (logLevel == LogLevel.Debug)
            {
                result = Color.LightGray;
            }
            else if (logLevel == LogLevel.Info)
            {
                result = Color.Black;
            }
            else if (logLevel == LogLevel.Warning)
            {
                result = DarkYellow;
            }
            else if (logLevel == LogLevel.Error)
            {
                result = Color.OrangeRed;
            }
            else if (logLevel == LogLevel.Fatal)
            {
                result = Color.Red;
            }
            else
            {
                throw new InvalidEnumArgumentException("logLevel", (int)logLevel, typeof(LogLevel));
            }

            return result;
        }

        #endregion
    }
}