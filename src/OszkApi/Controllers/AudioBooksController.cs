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
        public IActionResult GetAudioBook(string id)
        {
            var audiobook = _audioBookRepository.Get(id);
            if (audiobook == null)
                return NotFound();

            return new ObjectResult(audiobook);
        }
    }
}
