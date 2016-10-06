using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public interface IBookRepository
    {
        IQueryable<Book> GetAll();

        IQueryable<Book> Find(string query);

        Book Get(string UrlId);
    }
}
