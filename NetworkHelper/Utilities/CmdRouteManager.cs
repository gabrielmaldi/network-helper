using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using NetworkHelper.Classes;
using NetworkHelper.Extensions;

namespace NetworkHelper.Utilities
{
    public static class CmdRouteManager
    {
        #region Constants

        private const int CmdTimeoutInMilliseconds = 1000;

        #endregion

        #region Public API

        public static IEnumerable<CmdRouteManagementResult> AddRoutes(IEnumerable<Route> routes, string gatewayIpAddress)
        {
            return ExecuteCommands(routes.Select(route => GetRouteCommand(route.DestinationIpAddress, route.Mask, gatewayIpAddress, route.Metric, route.InterfaceNumber)));
        }

        public static IEnumerable<CmdRouteManagementResult> DeleteRoutes(IEnumerable<Route> routes)
        {
            return ExecuteCommands(routes.Select(route => GetRouteCommand(route.DestinationIpAddress, route.Mask)));
        }

        #endregion

        #region Helper functions

        private static string GetRouteCommand(string destinationIpAddress, string mask, string gatewayIpAddress, int? metric, int? interfaceNumber)
        {
            string result;

            result = string.Format(CultureInfo.InvariantCulture, "route ADD {0}", destinationIpAddress);
            if (!string.IsNullOrEmpty(mask))
            {
                result += string.Format(CultureInfo.InvariantCulture, " MASK {0}", mask);
            }
            result += string.Format(CultureInfo.InvariantCulture, " {0}", gatewayIpAddress);
            if (metric.HasValue)
            {
                result += string.Format(CultureInfo.InvariantCulture, " METRIC {0}", metric.Value);
            }
            if (interfaceNumber.HasValue)
            {
                result += string.Format(CultureInfo.InvariantCulture, " IF {0}", interfaceNumber.Value);
            }

            return result;
        }

        private static string GetRouteCommand(string destinationIpAddress, string mask)
        {
            string result;

            
            result = string.Format(CultureInfo.InvariantCulture, "route DELETE {0}", destinationIpAddress);
            if (!string.IsNullOrEmpty(mask))
            {
                result += string.Format(CultureInfo.InvariantCulture, " MASK {0}", mask);
            }

            return result;
        }

        private static List<CmdRouteManagementResult> ExecuteCommands(IEnumerable<string> commands)
        {
            List<CmdRouteManagementResult> result = null;

            using (Process cmdProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = string.Format(CultureInfo.InvariantCulture, "/C {0}", string.Join(" & ", commands)),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            })
            using (AutoResetEvent outputAutoResetEvent = new AutoResetEvent(false))
            using (AutoResetEvent errorAutoResetEvent = new AutoResetEvent(false))
            {
                cmdProcess.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        if (!outputAutoResetEvent.SafeWaitHandle.IsClosed)
                        {
                            outputAutoResetEvent.Set();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(e.Data))
                        {
                            if (result == null)
                            {
                                result = new List<CmdRouteManagementResult>();
                            }

                            result.Add(new CmdRouteManagementResult
                            {
                                Code = CmdRouteManagementResultCode.Success,
                                Message = e.Data
                            });
                        }
                    }
                };
                cmdProcess.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        if (!errorAutoResetEvent.SafeWaitHandle.IsClosed)
                        {
                            errorAutoResetEvent.Set();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(e.Data))
                        {
                            CmdRouteManagementResultCode code = CmdRouteManagementResultCode.ErrorUnknown;

                            if (e.Data.Contains("already exists", StringComparison.OrdinalIgnoreCase) || e.Data.Contains("ya existe", StringComparison.OrdinalIgnoreCase))
                            {
                                code = CmdRouteManagementResultCode.ErrorRouteAlreadyExists;
                            }
                            else if (e.Data.Contains("not found", StringComparison.OrdinalIgnoreCase) || e.Data.Contains("no encontrado", StringComparison.OrdinalIgnoreCase))
                            {
                                code = !NetworkInterface.GetIsNetworkAvailable() ? CmdRouteManagementResultCode.ErrorNoActiveNetworkConnection : CmdRouteManagementResultCode.ErrorRouteNotFound;
                            }

                            if (result == null)
                            {
                                result = new List<CmdRouteManagementResult>();
                            }

                            result.Add(new CmdRouteManagementResult
                            {
                                Code = code,
                                Message = e.Data
                            });
                        }
                    }
                };

                cmdProcess.Start();

                cmdProcess.BeginOutputReadLine();
                cmdProcess.BeginErrorReadLine();

                cmdProcess.WaitForExit(CmdTimeoutInMilliseconds);
                outputAutoResetEvent.WaitOne(CmdTimeoutInMilliseconds);
                errorAutoResetEvent.WaitOne(CmdTimeoutInMilliseconds);
            }

            return result;
        }

        #endregion
    }
}