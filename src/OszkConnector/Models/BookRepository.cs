using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class BookRepository : IBookRepository
    {
        public IEnumerable<Book> Find(string query = "")
        {
            var client = new Client();
            return client.FindAudioBook(query).Result;
        }

        public Book Get(string UrlId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Book> GetAll()
        {
            return Find();
        }
    }
}
