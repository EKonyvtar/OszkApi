using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class MekConvert
    {
        public const string REGEX_AUDIOBOOK = @"\[Hangoskönyv\]|\[MVGYOSZ hangoskönyvek\]";
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

        public static string ClearText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            //TODO: Unescape text
            //text = HttpUtility.HtmlDecode(text);

            // Fix text
            text = text.
                Replace(STR_NBSP, " ").
                Replace("û", "ű").
                Replace("õ", "ő");

            //Multiline Trim
            var middle = new Regex(REGEX_AUDIOBOOK);
            var front = new Regex(@"^\s+", RegexOptions.Multiline);
            var back = new Regex(@"\s+$", RegexOptions.Multiline);
            text = front.Replace(text, "");
            text = middle.Replace(text, "");
            text = back.Replace(text, "");
            return text;
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
    }
}
