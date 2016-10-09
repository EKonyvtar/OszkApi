using OszkConnector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Repository
{
    public interface IBookRepository
    {
        IQueryable<BookResult> GetAll();

        IQueryable<BookResult> Find(string query);

        Book Get(string Id);
    }
}
