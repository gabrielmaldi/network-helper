using System;
using System.Globalization;

namespace NetworkHelper.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source.IndexOf(value, comparisonType) >= 0;
        }

        public static bool ContainsAny(this string source, params char[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            return source.IndexOfAny(values) != -1;
        }

        public static bool ContainsAny(this string source, string values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            return source.ContainsAny(values.ToCharArray());
        }

        public static string Substring(this string source, int startIndex, string endString)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (endString == null)
            {
                throw new ArgumentNullException("endString");
            }

            return source.Substring(startIndex, source.IndexOf(endString, startIndex) - startIndex);
        }

        public static string Substring(this string source, string startString, string endString)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (startString == null)
            {
                throw new ArgumentNullException("startString");
            }

            if (endString == null)
            {
                throw new ArgumentNullException("endString");
            }

            return source.Substring(source.IndexOf(startString) + startString.Length, endString);
        }

        public static string[] Split(this string source, char separator, int count, StringSplitOptions options)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return source.Split(new char[] { separator }, count, options);
        }

        public static string[] Split(this string source, char separator, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return source.Split(new char[] { separator }, count);
        }

        public static string[] Split(this string source, char separator, StringSplitOptions options)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return source.Split(new char[] { separator }, options);
        }

        public static bool TryToBoolean(this string source, out bool result)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            bool convertionSuccessful;

            convertionSuccessful = bool.TryParse(source, out result);
            if (!convertionSuccessful)
            {
                int intValue;
                convertionSuccessful = int.TryParse(source, out intValue);
                if (convertionSuccessful)
                {
                    if (intValue == 0)
                    {
                        result = false;
                    }
                    else if (intValue == 1)
                    {
                        result = true;
                    }
                    else
                    {
                        convertionSuccessful = false;
                    }
                }
            }

            return convertionSuccessful;
        }

        public static bool ToBoolean(this string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            bool convertedValue;

            if (!source.TryToBoolean(out convertedValue))
            {
                throw new FormatException(string.Format(CultureInfo.InvariantCulture, "Cannot convert \"{0}\" to \"{1}\".", source, typeof(bool).FullName));
            }

            return convertedValue;
        }
    }
}