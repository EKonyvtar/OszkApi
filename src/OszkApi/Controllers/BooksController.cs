using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OszkConnector.Models;
using System.Linq;
using OszkConnector.Repository;

namespace OszkApi.Controllers
{
    [Route("api/[controller]")]
    public class BooksController : Controller
    {
        private IBookRepository _bookRepository { get; set; }

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet()]
        public IQueryable<BookResult> Get([FromQuery] string query = "")
        {
            return _bookRepository.Find(query);
        }

        [HttpGet("{id}", Name = "GetBook")]
        public IActionResult GetByCatalogId(string id)
        {
            var book = _bookRepository.Get(id);
            if (book == null)
                return NotFound();

            return new ObjectResult(book);
        }
    }
}
