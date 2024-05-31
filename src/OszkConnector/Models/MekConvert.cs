using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;


namespace OszkConnector.Models
{
    public class MekConvert
    {
        public const string REGEX_AUDIOBOOK = @"\[Hangoskönyv\]|\[MVGYOSZ hangoskönyvek\]";
        public const string STR_NBSP = "&nbsp;";

        private static Regex REGEX_TITLE = new Regex($"^(.*){STR_NBSP}(.*)$");

        public static string ToIso88592(byte[] byteArray)
        {
            var iso = Encoding.GetEncoding("ISO-8859-1");
            string isoString = iso.GetString(byteArray);
            return isoString;
        }

        public static string ToUtf8(byte[] byteArray)
        {
            string utf8String = Encoding.UTF8.GetString(byteArray);
            return utf8String;
        }

        public static string Trim(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;

            //Multiline Trim
            var front = new Regex(@"^\s+", RegexOptions.Multiline);
            var back = new Regex(@"\s+$", RegexOptions.Multiline);
            text = front.Replace(text, "");
            text = back.Replace(text, "");
            return text;
        }

        public static string ClearText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            // Fix text
            text = text.
                Replace(STR_NBSP, " ").
                Replace("û", "ű").
                Replace("Õ", "Ő").
                Replace("õ", "ő").
                Replace("õ", "ő").
                Replace("&amp;", "&").
                Replace("&#194;", "ja"). //Â"). // 13799
                Replace("&#269;", "cs"). //č"). // TODO: Fix char encoding
                Replace("&#251;", "ju"); // û");

            // TODO: Unescape text
            text = HttpUtility.HtmlDecode(text);
            

            //Multiline Trim
            var middle = new Regex(REGEX_AUDIOBOOK);
            text = middle.Replace(text, "");
            text = Trim(text);
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
