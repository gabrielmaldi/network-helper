using System;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;

namespace NetworkHelper.Utilities
{
    public static class WinApiRouteManager
    {
        #region Public API

        public static WinApiRouteManagementResult AddRoute(string destinationIpAddress, string mask, string gatewayIpAddress)
        {
            WinApiRouteManagementResult result;

            uint destinationIpAddressWinApiFormat = ParseInternetAddress(destinationIpAddress);

            uint interfaceIndex;
            if (GetBestInterface(destinationIpAddressWinApiFormat, out interfaceIndex) == NO_ERROR)
            {
                uint metric = GetNetworkInterfaceMetric(interfaceIndex);
                if (metric != 0)
                {
                    MIB_IPFORWARDROW route = new MIB_IPFORWARDROW
                    {
                        dwForwardProto = ForwardProtocol.NetMGMT,
                        dwForwardDest = destinationIpAddressWinApiFormat,
                        dwForwardMask = ParseInternetAddress(mask),
                        dwForwardNextHop = ParseInternetAddress(gatewayIpAddress),
                        dwForwardIfIndex = interfaceIndex,
                        dwForwardMetric1 = (int)metric
                    };

                    result = (WinApiRouteManagementResult)CreateIpForwardEntry(ref route);
                }
                else
                {
                    result = WinApiRouteManagementResult.ErrorCouldNotFindNetworkInterfaceMetric;
                }
            }
            else
            {
                result = WinApiRouteManagementResult.ErrorCouldNotFindNetworkInterface;
            }

            return result;
        }

        public static WinApiRouteManagementResult DeleteRoute(string destinationIpAddress, string mask, string gatewayIpAddress)
        {
            WinApiRouteManagementResult result;

            uint destinationIpAddressWinApiFormat = ParseInternetAddress(destinationIpAddress);

            uint interfaceIndex;
            if (GetBestInterface(destinationIpAddressWinApiFormat, out interfaceIndex) == NO_ERROR)
            {
                MIB_IPFORWARDROW route = new MIB_IPFORWARDROW
                {
                    dwForwardProto = ForwardProtocol.NetMGMT,
                    dwForwardDest = destinationIpAddressWinApiFormat,
                    dwForwardMask = ParseInternetAddress(mask),
                    dwForwardNextHop = ParseInternetAddress(gatewayIpAddress),
                    dwForwardIfIndex = interfaceIndex
                };

                result = (WinApiRouteManagementResult)DeleteIpForwardEntry(ref route);
            }
            else
            {
                result = WinApiRouteManagementResult.ErrorCouldNotFindNetworkInterface;
            }

            return result;
        }

        #endregion

        #region Helper functions

        private static uint ParseInternetAddress(string ipAddress)
        {
            return BitConverter.ToUInt32(IPAddress.Parse(ipAddress).GetAddressBytes(), 0);
        }

        private static uint GetNetworkInterfaceMetric(uint interfaceIndex)
        {
            uint result;

            MIB_IPINTERFACE_ROW networkInterface = new MIB_IPINTERFACE_ROW
            {
                Family = AF_INET,
                InterfaceIndex = interfaceIndex
            };

            if (GetIpInterfaceEntry(ref networkInterface) == NO_ERROR)
            {
                result = networkInterface.Metric;

                using (ManagementObjectSearcher query = new ManagementObjectSearcher(string.Format(CultureInfo.InvariantCulture, "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE' AND InterfaceIndex='{0}'", interfaceIndex)))
                using (ManagementObjectCollection queryResult = query.Get())
                {
                    foreach (ManagementObject networkAdapterConfiguration in queryResult)
                    {
                        UInt16[] gatewayCostMetric = networkAdapterConfiguration["GatewayCostMetric"] as UInt16[];
                        if (gatewayCostMetric != null && gatewayCostMetric.Any())
                        {
                            result += gatewayCostMetric.First();
                        }
                    }
                }
            }
            else
            {
                result = 0;
            }

            return result;
        }

        #endregion

        #region Windows API definitions

        private const uint AF_INET = 2;
        private const int NO_ERROR = 0;

        [DllImport("iphlpapi")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern int CreateIpForwardEntry(ref MIB_IPFORWARDROW pRoute);

        [DllImport("iphlpapi")]
        [return: MarshalAs(UnmanagedType.U4)]
        private static extern int DeleteIpForwardEntry(ref MIB_IPFORWARDROW pRoute);

        [DllImport("iphlpapi", SetLastError = true)]
        private static extern int GetBestInterface(uint destAddr, out uint bestIfIndex);

        [DllImport("iphlpapi")]
        private static extern uint GetIpInterfaceEntry(ref MIB_IPINTERFACE_ROW pRoute);

        private enum ForwardProtocol
        {
            Other = 1,
            Local = 2,
            NetMGMT = 3,
            ICMP = 4,
            EGP = 5,
            GGP = 6,
            Hello = 7,
            RIP = 8,
            IS_IS = 9,
            ES_IS = 10,
            CISCO = 11,
            BBN = 12,
            OSPF = 13,
            BGP = 14,
            NT_AUTOSTATIC = 10002,
            NT_STATIC = 10006,
            NT_STATIC_NON_DOD = 10007
        }

        private enum ForwardType
        {
            Other = 1,
            Invalid = 2,
            Direct = 3,
            Indirect = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MIB_IPFORWARDROW
        {
            public uint dwForwardDest;
            public uint dwForwardMask;
            public int dwForwardPolicy;
            public uint dwForwardNextHop;
            public uint dwForwardIfIndex;
            public ForwardType dwForwardType;
            public ForwardProtocol dwForwardProto;
            public int dwForwardAge;
            public int dwForwardNextHopAS;
            public int dwForwardMetric1;
            public int dwForwardMetric2;
            public int dwForwardMetric3;
            public int dwForwardMetric4;
            public int dwForwardMetric5;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MIB_IPINTERFACE_ROW
        {
            public uint Family;
            public ulong InterfaceLuid;
            public uint InterfaceIndex;
            public uint MaxReassemblySize;
            public ulong InterfaceIdentifier;
            public uint MinRouterAdvertisementInterval;
            public uint MaxRouterAdvertisementInterval;
            public byte AdvertisingEnabled;
            public byte ForwardingEnabled;
            public byte WeakHostSend;
            public byte WeakHostReceive;
            public byte UseAutomaticMetric;
            public byte UseNeighborUnreachabilityDetection;
            public byte ManagedAddressConfigurationSupported;
            public byte OtherStatefulConfigurationSupported;
            public byte AdvertiseDefaultRoute;
            public uint RouterDiscoveryBehavior;
            public uint DadTransmits;
            public uint BaseReachableTime;
            public uint RetransmitTime;
            public uint PathMtuDiscoveryTimeout;
            public uint LinkLocalAddressBehavior;
            public uint LinkLocalAddressTimeout;
            public uint ZoneIndice0;
            public uint ZoneIndice1;
            public uint ZoneIndice2;
            public uint ZoneIndice3;
            public uint ZoneIndice4;
            public uint ZoneIndice5;
            public uint ZoneIndice6;
            public uint ZoneIndice7;
            public uint ZoneIndice8;
            public uint ZoneIndice9;
            public uint ZoneIndice10;
            public uint ZoneIndice11;
            public uint ZoneIndice12;
            public uint ZoneIndice13;
            public uint ZoneIndice14;
            public uint ZoneIndice15;
            public uint SitePrefixLength;
            public uint Metric;
            public uint NlMtu;
            public byte Connected;
            public byte SupportsWakeUpPatterns;
            public byte SupportsNeighborDiscovery;
            public byte SupportsRouterDiscovery;
            public uint ReachableTime;
            public byte TransmitOffload;
            public byte ReceiveOffload;
            public byte DisableDefaultRoutes;
        }

        #endregion
    }
}