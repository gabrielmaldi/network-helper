using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace NetworkHelper.Utilities
{
    public static class Serializer
    {
        public static string XmlSerialize<T>(T serializableObject)
        {
            return XmlSerialize(typeof(T), serializableObject);
        }

        public static T XmlDeserialize<T>(string serializedObject)
        {
            return (T)XmlDeserialize(typeof(T), serializedObject);
        }

        private static string XmlSerialize(Type type, object serializableObject)
        {
            string result;

            using (StringWriter stringWriter = new Utf8StringWriter())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                xmlSerializer.Serialize(stringWriter, serializableObject);

                result = stringWriter.ToString();
            }

            return result;
        }

        private static object XmlDeserialize(Type type, string serializedObject)
        {
            object result;

            using (StringReader stringReader = new StringReader(serializedObject))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                result = xmlSerializer.Deserialize(stringReader);
            }

            return result;
        }

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get
                {
                    return Encoding.UTF8;
                }
            }
        }
    }
}
