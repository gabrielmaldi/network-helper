using System;
using System.Net;

namespace NetworkHelper.Utilities
{
    public static class IpAddressHelper
    {
        public static bool IsIpAddressValid(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new ArgumentNullException("ipAddress");
            }

            IPAddress parsedIpAddress;
            return IPAddress.TryParse(ipAddress, out parsedIpAddress);
        }

        public static bool AreIpAddressAndMaskValid(string ipAddress, string mask)
        {
            if (string.IsNullOrEmpty(ipAddress))
            {
                throw new ArgumentNullException("ipAddress");
            }

            if (string.IsNullOrEmpty(mask))
            {
                throw new ArgumentNullException("mask");
            }

            bool result;

            int ipAddressBits = BitConverter.ToInt32(IPAddress.Parse(ipAddress).GetAddressBytes(), 0);
            int maskBits = BitConverter.ToInt32(IPAddress.Parse(mask).GetAddressBytes(), 0);
            result = (ipAddressBits & maskBits) == ipAddressBits;

            return result;
        }
    }
}