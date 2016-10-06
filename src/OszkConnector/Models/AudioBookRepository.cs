using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public class AudioBookRepository : IBookRepository
    {
        public IQueryable<BookResult> Find(string query = "")
        {
            var client = new Client();
            return client.FindAudioBook(query).Result;
        }

        public BookResult Get(string UrlId)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BookResult> GetAll()
        {
            return Find();
        }
    }
}
