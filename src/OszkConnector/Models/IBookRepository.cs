using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public interface IBookRepository
    {
        IQueryable<BookResult> GetAll();

        IQueryable<BookResult> Find(string query);

        BookResult Get(string UrlId);
    }
}
