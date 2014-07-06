using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using NetworkHelper.Classes;
using BaseClassLibraryConfigurationManager = System.Configuration.ConfigurationManager;

namespace NetworkHelper.Utilities
{
    public static class ConfigurationManager
    {
        private const string _DefaultConfigurationFilePath = "NetworkHelperConfiguration.xml";

        private static readonly Lazy<string> ConfigurationFilePath = new Lazy<string>(() =>
        {
            string result;

            result = BaseClassLibraryConfigurationManager.AppSettings["ConfigurationFilePath"];
            if (string.IsNullOrEmpty(result))
            {
                result = _DefaultConfigurationFilePath;
            }
            if (!Path.IsPathRooted(result))
            {
                result = Path.Combine(WinFormsHelper.ExecutableDirectory, result);
            }

            return result;
        });

        public static Configuration Configuration { get; private set; }

        public static void LoadConfiguration()
        {
            if (!File.Exists(ConfigurationFilePath.Value))
            {
                InitializeAndValidateConfigurationAndCreateExampleIfNeeded();
                SaveConfiguration();
            }
            else
            {
                Configuration = Serializer.XmlDeserialize<Configuration>(File.ReadAllText(ConfigurationFilePath.Value, Encoding.UTF8));
                InitializeAndValidateConfigurationAndCreateExampleIfNeeded();
            }
        }

        public static void SaveConfiguration()
        {
            File.WriteAllText(ConfigurationFilePath.Value, Serializer.XmlSerialize(Configuration), Encoding.UTF8);
        }

        private static void InitializeAndValidateConfigurationAndCreateExampleIfNeeded()
        {
            if (Configuration == null)
            {
                Configuration = new Configuration();
            }

            if (Configuration.Vpns == null)
            {
                Configuration.Vpns = new List<Vpn>
                {
                    new Vpn
                    {
                        Name = "My VPN name",
                        IsEnabled = false,
                        DnsSuffix = "some.suffix.com",
                        Routes = new List<Route>
                        {
                            new Route
                            {
                                Description = "A database server",
                                DestinationIpAddress = "192.168.44.33"
                            },
                            new Route
                            {
                                Description = "A range of workstations",
                                DestinationIpAddress = "192.168.45.0",
                                Mask = "255.255.255.0"
                            }
                        }
                    }
                };
            }
            else
            {
                HashSet<string> vpnNames = new HashSet<string>(StringComparer.Ordinal);
                int unnamedVpnNameCount = 0;

                foreach (Vpn vpn in Configuration.Vpns.Where(vpn => vpn.IsEnabled))
                {
                    if (string.IsNullOrEmpty(vpn.Name))
                    {
                        vpn.IsOnlyTriggeredManually = true;
                        vpn.Name = string.Format(CultureInfo.InvariantCulture, "#Unnamed_{0}", ++unnamedVpnNameCount);

                        Logger.Instance.Log(LogLevel.Warning, "An unamed VPN has been assigned the name \"{0}\" and will only be triggered manually.", vpn.Name);
                    }
                    else if (vpnNames.Contains(vpn.Name))
                    {
                        vpn.IsValid = false;

                        Logger.Instance.Log(LogLevel.Error, "Duplicate VNP \"{0}\", only the one specified first in the configuration file will work.", vpn.Name);
                    }

                    if (vpn.IsValid)
                    {
                        vpnNames.Add(vpn.Name);

                        if (!string.IsNullOrEmpty(vpn.GatewayIpAddress) && !IpAddressHelper.IsIpAddressValid(vpn.GatewayIpAddress))
                        {
                            vpn.GatewayIpAddress = null;

                            Logger.Instance.Log(LogLevel.Error, "VPN \"{0}\" has an invalid \"GatewayIpAddress\" ({1}), you will have to specify it when adding routes manually.", vpn.Name, vpn.GatewayIpAddress);
                        }

                        #region Routes validation

                        if (vpn.Routes == null)
                        {
                            vpn.Routes = new List<Route>();
                        }
                        else if (vpn.Routes.Any())
                        {
                            foreach (Route route in vpn.Routes.Where(route => route.IsEnabled))
                            {
                                Lazy<string> routeRepresentation = new Lazy<string>(() => route.ToString());

                                Action<string, string> validateIpAddress = (ipAddress, propertyName) =>
                                {
                                    if (route.IsValid)
                                    {
                                        if (!IpAddressHelper.IsIpAddressValid(ipAddress))
                                        {
                                            route.IsValid = false;

                                            if (!string.IsNullOrEmpty(routeRepresentation.Value))
                                            {
                                                Logger.Instance.Log(LogLevel.Error, "Route {0} has an invalid \"{1}\" and will not be processed.", routeRepresentation.Value, propertyName);
                                            }
                                            else
                                            {
                                                Logger.Instance.Log(LogLevel.Error, "A route with an invalid \"{0}\" will not be processed.", propertyName);
                                            }
                                        }
                                    }
                                };

                                if (string.IsNullOrEmpty(route.DestinationIpAddress))
                                {
                                    route.IsValid = false;

                                    if (!string.IsNullOrEmpty(routeRepresentation.Value))
                                    {
                                        Logger.Instance.Log(LogLevel.Error, "Route {0} has no \"DestinationIpAddress\" and will not be processed.", routeRepresentation.Value);
                                    }
                                    else
                                    {
                                        Logger.Instance.Log(LogLevel.Error, "A route with no \"DestinationIpAddress\" will not be processed.");
                                    }
                                }

                                validateIpAddress(route.DestinationIpAddress, "DestinationIpAddress");
                                if (!string.IsNullOrEmpty(route.Mask))
                                {
                                    validateIpAddress(route.Mask, "Mask");
                                }

                                if (route.IsValid && !string.IsNullOrEmpty(route.Mask))
                                {
                                    if (!IpAddressHelper.AreIpAddressAndMaskValid(route.DestinationIpAddress, route.Mask))
                                    {
                                        route.IsValid = false;

                                        if (!string.IsNullOrEmpty(routeRepresentation.Value))
                                        {
                                            Logger.Instance.Log(LogLevel.Error, "Route {0} has an invalid combination of \"DestinationIpAddress\" and \"Mask\" and will not be processed (\"DestinationIpAddress\" & \"Mask\" must be = \"DestinationIpAddress\").", routeRepresentation.Value);
                                        }
                                        else
                                        {
                                            Logger.Instance.Log(LogLevel.Error, "A route with an invalid combination of \"DestinationIpAddress\" and \"Mask\" and will not be processed (\"DestinationIpAddress\" & \"Mask\" must be = \"DestinationIpAddress\").");
                                        }
                                    }
                                }
                            }
                        }

                        #endregion
                    }
                }
            }
        }
    }
}
