using OszkConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Repository
{
    public class AudioBookRepository : IAudioBookRepository
    {
        public IQueryable<Book> Find(string query = "")
        {
            var client = new Client();
            return client.FindAudioBook(query).Result;
        }

        public Book Get(string Id)
        {
            var client = new Client();
            return client.GetBook(Id).Result;
        }

        public IQueryable<Book> GetAll()
        {
            return Find();
        }
    }
}
