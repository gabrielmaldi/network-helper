using System.Collections.Generic;
using System.Xml.Serialization;

namespace NetworkHelper.Classes
{
    public class Vpn
    {
        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public string DnsSuffix { get; set; }

        public string GatewayIpAddress { get; set; }

        public List<Route> Routes { get; set; }

        [XmlIgnore]
        public bool IsValid { get; set; }

        [XmlIgnore]
        public bool IsOnlyTriggeredManually { get; set; }

        public Vpn()
        {
            IsEnabled = true;
            IsValid = true;
        }
    }
}