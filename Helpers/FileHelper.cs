using System;
using System.Text;

namespace DotNetApisForAngularProjects.Helpers
{
    public static class FileHelper
    {
        public static byte[] EncodeString(string unicodeString) {
            Encoding unicode = Encoding.Unicode;
            byte[] unicodeBytes = unicode.GetBytes(unicodeString);
            return unicodeBytes;
        }

        public static string DecodeString(byte[] unicodeBytes) {
            Encoding unicode = Encoding.Unicode;
            string unicodeString = unicode.GetString(unicodeBytes);
            return unicodeString;
        }

    }
}