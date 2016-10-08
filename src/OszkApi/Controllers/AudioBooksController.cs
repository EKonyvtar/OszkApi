using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OszkConnector.Models;

namespace OszkApi.Controllers
{
    [Route("api/[controller]")]
    public class AudioBooksController : Controller
    {
        private IBookRepository _bookRepository { get; set; }

        public AudioBooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet()]
        public IEnumerable<BookResult> Get([FromQuery] string query = "")
        {
            return _bookRepository.Find(query);
        }

        [HttpGet("{id}", Name = "GetAudioBook")]
        public IActionResult GetByUrlId(int id)
        {
            var book = _bookRepository.Get(id);
            if (book == null)
                return NotFound();

            return new ObjectResult(book);
        }
    }
}
