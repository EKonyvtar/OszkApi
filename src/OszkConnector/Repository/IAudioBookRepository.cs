using OszkConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Repository
{
    public interface IAudioBookRepository
    {
        IQueryable<Book> GetAll();

        IQueryable<Book> Find(string query);

        Book Get(string Id);
    }
}
