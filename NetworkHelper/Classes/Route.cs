using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace NetworkHelper.Classes
{
    public class Route
    {
        public string Description { get; set; }

        public bool IsEnabled { get; set; }
        
        public string DestinationIpAddress { get; set; }
        
        public string Mask { get; set; }

        public int? InterfaceNumber { get; set; }

        public int? Metric { get; set; }

        [XmlIgnore]
        public bool IsValid { get; set; }

        public Route()
        {
            IsEnabled = true;
            IsValid = true;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrEmpty(DestinationIpAddress))
            {
                result.AppendFormat(CultureInfo.InvariantCulture, "{0}", DestinationIpAddress);
                if (!string.IsNullOrEmpty(Mask))
                {
                    result.AppendFormat(CultureInfo.InvariantCulture, "/{0}", Mask);
                }
                if (!string.IsNullOrEmpty(Description))
                {
                    result.AppendFormat(CultureInfo.InvariantCulture, " (\"{0}\")", Description);
                }
            }
            else if (!string.IsNullOrEmpty(Description))
            {
                result.AppendFormat(CultureInfo.InvariantCulture, "\"{0}\"", Description);
            }

            return result.ToString();
        }
    }
}