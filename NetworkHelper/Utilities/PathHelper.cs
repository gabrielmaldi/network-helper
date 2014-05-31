using System;
using System.IO;

namespace NetworkHelper.Utilities
{
    public static class PathHelper
    {
        public static bool ArePathsEqual(string pathA, string pathB)
        {
            bool result;

            if (object.ReferenceEquals(pathA, pathB) || (string.IsNullOrEmpty(pathA) && string.IsNullOrEmpty(pathB)) || string.Equals(pathA, pathB, StringComparison.OrdinalIgnoreCase))
            {
                result = true;
            }
            else
            {
                if (string.IsNullOrEmpty(pathA) || string.IsNullOrEmpty(pathB))
                {
                    result = false;
                }
                else
                {
                    result = string.Equals(Path.GetFullPath(pathA).TrimEnd('\\'), Path.GetFullPath(pathB).TrimEnd('\\'), StringComparison.OrdinalIgnoreCase);
                }
            }

            return result;
        }
    }
}