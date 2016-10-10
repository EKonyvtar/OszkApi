using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OszkConnector.Models;
using System.Linq;
using OszkConnector.Repository;

namespace OszkApi.Controllers
{
    [Route("api/[controller]")]
    public class AudioBooksController : Controller
    {
        private IAudioBookRepository _audioBookRepository { get; set; }

        public AudioBooksController(IAudioBookRepository audioBookRepository)
        {
            _audioBookRepository = audioBookRepository;
        }

        [HttpGet()]
        public IQueryable<Book> Get([FromQuery] string query = "")
        {
            return _audioBookRepository.Find(query);
        }

        [HttpGet("{id}", Name = "GetAudioBook")]
        public IActionResult GetByCatalogId(string id)
        {
            var book = _audioBookRepository.Get(id);
            if (book == null)
                return NotFound();

            return new ObjectResult(book);
        }

        [HttpGet("{id}/tracks", Name = "GetAudioBookWithTracks")]
        public IActionResult GetAudioBookTrackList(string id)
        {
            var book = _audioBookRepository.Get(id);
            if (book == null)
                return NotFound();

            return new ObjectResult(book);
        }
    }
}
