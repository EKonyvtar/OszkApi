using OszkConnector;
using OszkConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OszkApiExamples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Magyar Elektronikus Könyvtár API - Ákos Muráti");



            Console.ReadLine();
            var client = new Client();
            var books = client.FindAudioBook("gardonyi").Result;
            foreach (var b in books)
                Console.WriteLine(b.ToString());

            Console.ReadLine();
        }
    }
}
