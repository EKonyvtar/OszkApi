using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class MekConverter
    {
        public const string STR_AUDIOBOOK = "[Hangoskönyv]";
        public const string STR_NBSP = "&nbsp;";

        private static Regex REGEX_TITLE = new Regex($"^(.*){STR_NBSP}(.*)$");

        public static string ToUtf8(byte[] byteArray)
        {
            //TODO: Implement legacy conversion from ISO-8859-2
            var iso = Encoding.GetEncoding("ISO-8859-1");
            var utf8 = Encoding.UTF8;
            var utf8Bytes = Encoding.Convert(iso, utf8, byteArray);

            return utf8.GetString(utf8Bytes);
        }

        public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePair(string postData)
        {
            var cc = new List<KeyValuePair<string, string>>();
            foreach (var field in postData.Split('&'))
            {
                var key = field.Split('=')[0];
                var value = field.Split('=')[1];
                cc.Add(new KeyValuePair<string, string>(key, value?.Replace('+', ' ')));
            }
            return cc;
        }

        public static string ToFullTitle(string text)
        {
            return text.
                Replace(STR_AUDIOBOOK, "").
                Replace(STR_NBSP, " ").
                Trim();
        }

        public static string ToAuthor(string text)
        {
            if (text.Contains(":"))
                return ToFullTitle(text).Split(':')[0]?.Trim();
            return null;
        }

        public static string ToTitle(string text)
        {
            if (text.Contains(":"))
                return ToFullTitle(text).Split(':')[1]?.Trim();
            return text;

        }
    }
}
