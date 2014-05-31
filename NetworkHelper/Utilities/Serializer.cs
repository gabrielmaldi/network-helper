using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace NetworkHelper.Utilities
{
    /// <summary>
    /// Helper class to serialize and deserialize in various ways
    /// </summary>
    public static class Serializer
    {
        #region XmlSerialization

        #region Non-generic methods

        #region Serialization methods

        /// <summary>
        /// Serialize from object to string using XmlSerializer
        /// </summary>
        /// <param name="type">Type of object</param>
        /// <param name="serializableObject">Instance of type</param>
        /// <param name="includeXmlDeclaration">Specifies whether to include the XML declaration in the serialization</param>
        /// <param name="knownTypes">Types that may be present in the object graph</param>
        /// <returns>A string with the serialized object</returns>
        public static string XmlSerialize(Type type, object serializableObject, bool includeXmlDeclaration, params Type[] knownTypes)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (serializableObject == null)
            {
                throw new ArgumentNullException("serializableObject");
            }

            string result;

            using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            {
                XmlSerializer xmlSerializer = GetXmlSerializer(type, knownTypes);
                xmlSerializer.Serialize(stringWriter, serializableObject);

                if (includeXmlDeclaration)
                {
                    result = stringWriter.ToString();
                }
                else
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(stringWriter.ToString());
                    result = xmlDocument.FirstChild.NextSibling.OuterXml;
                }
            }

            return result;
        }

        /// <summary>
        /// Serialize from object to string using XmlSerializer
        /// </summary>
        /// <param name="type">Type of object</param>
        /// <param name="serializableObject">Instance of type</param>
        /// <param name="includeXmlDeclaration">Specifies whether to include the XML declaration in the serialization</param>
        /// <returns>A string with the serialized object</returns>
        public static string XmlSerialize(Type type, object serializableObject, bool includeXmlDeclaration)
        {
            return XmlSerialize(type, serializableObject, includeXmlDeclaration, null);
        }

        /// <summary>
        /// Serialize from object to string using XmlSerializer, including the XML declaration
        /// </summary>
        /// <param name="type">Type of object</param>
        /// <param name="serializableObject">Instance of type</param>
        /// <param name="knownTypes">Types that may be present in the object graph</param>
        /// <returns>A string with the serialized object</returns>
        public static string XmlSerialize(Type type, object serializableObject, params Type[] knownTypes)
        {
            return XmlSerialize(type, serializableObject, true, knownTypes);
        }

        /// <summary>
        /// Serialize from object to string using XmlSerializer, including the XML declaration
        /// </summary>
        /// <param name="type">Type of object</param>
        /// <param name="serializableObject">Instance of type</param>
        /// <returns>A string with the serialized object</returns>
        public static string XmlSerialize(Type type, object serializableObject)
        {
            return XmlSerialize(type, serializableObject, true, null);
        }

        #endregion

        #region Deserialization methods

        /// <summary>
        /// Deserialize from string to object using XmlSerializer
        /// </summary>
        /// <param name="type">Type of serialized object</param>
        /// <param name="serializedObject">Instance of serialized object</param>
        /// <param name="knownTypes">Types that may be present in the object graph</param>
        /// <returns>An instance of object type</returns>
        public static object XmlDeserialize(Type type, string serializedObject, params Type[] knownTypes)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (serializedObject == null)
            {
                throw new ArgumentNullException("serializedObject");
            }

            object result;

            using (StringReader stringReader = new StringReader(serializedObject))
            {
                XmlSerializer xmlSerializer = GetXmlSerializer(type, knownTypes);
                result = xmlSerializer.Deserialize(stringReader);
            }

            return result;
        }

        /// <summary>
        /// Deserialize from string to object using XmlSerializer
        /// </summary>
        /// <param name="type">Type of serialized object</param>
        /// <param name="serializedObject">Instance of serialized object</param>
        /// <returns>An instance of object type</returns>
        public static object XmlDeserialize(Type type, string serializedObject)
        {
            return XmlDeserialize(type, serializedObject, null);
        }

        #endregion

        #endregion

        #region Generic methods

        #region Serialization methods

        /// <summary>
        /// Serialize from object to string using XmlSerializer
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="serializableObject">Instance of T</param>
        /// <param name="includeXmlDeclaration">Specifies whether to include the XML declaration in the serialization</param>
        /// <param name="knownTypes">Types that may be present in the object graph</param>
        /// <returns>A string with the serialized object</returns>
        public static string XmlSerialize<T>(T serializableObject, bool includeXmlDeclaration, params Type[] knownTypes)
        {
            return XmlSerialize(typeof(T), serializableObject, includeXmlDeclaration, knownTypes);
        }

        /// <summary>
        /// Serialize from object to string using XmlSerializer, including the XML declaration
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="serializableObject">Instance of T</param>
        /// <param name="knownTypes">Types that may be present in the object graph</param>
        /// <returns>A string with the serialized object</returns>
        public static string XmlSerialize<T>(T serializableObject, params Type[] knownTypes)
        {
            return XmlSerialize(typeof(T), serializableObject, true, knownTypes);
        }

        /// <summary>
        /// Serialize from object to string using XmlSerializer
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="serializableObject">Instance of T</param>
        /// <param name="includeXmlDeclaration">Specifies whether to include the XML declaration in the serialization</param>
        /// <returns>A string with the serialized object</returns>
        public static string XmlSerialize<T>(T serializableObject, bool includeXmlDeclaration)
        {
            return XmlSerialize(typeof(T), serializableObject, includeXmlDeclaration, null);
        }

        /// <summary>
        /// Serialize from object to string using XmlSerializer, including the XML declaration
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="serializableObject">Instance of T</param>
        /// <returns>A string with the serialized object</returns>
        public static string XmlSerialize<T>(T serializableObject)
        {
            return XmlSerialize(typeof(T), serializableObject, true, null);
        }

        #endregion

        #region Deserialization methods

        /// <summary>
        /// Deserialize from string to object using XmlSerializer
        /// </summary>
        /// <typeparam name="T">Type of serialized object</typeparam>
        /// <param name="serializedObject">Instance of serialized object</param>
        /// <param name="knownTypes">Types that may be present in the object graph</param>
        /// <returns>An instance of object T</returns>
        public static T XmlDeserialize<T>(string serializedObject, params Type[] knownTypes)
        {
            return (T)XmlDeserialize(typeof(T), serializedObject, knownTypes);
        }

        /// <summary>
        /// Deserialize from string to object using XmlSerializer
        /// </summary>
        /// <typeparam name="T">Type of serialized object</typeparam>
        /// <param name="serializedObject">Instance of serialized object</param>
        /// <returns>An instance of object T</returns>
        public static T XmlDeserialize<T>(string serializedObject)
        {
            return (T)XmlDeserialize(typeof(T), serializedObject, null);
        }

        #endregion

        #endregion

        #endregion

        #region Private members

        private static XmlSerializer GetXmlSerializer(Type type, Type[] knownTypes)
        {
            XmlSerializer result;

            if (knownTypes != null && knownTypes.Any())
            {
                result = new XmlSerializer(type, knownTypes);
            }
            else
            {
                result = new XmlSerializer(type);
            }

            return result;
        }

        #endregion
    }
}