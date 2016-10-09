using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    public interface IAudioBookRepository
    {
        IQueryable<BookResult> GetAll();

        IQueryable<BookResult> Find(string query);

        Book Get(string Id);
    }
}
