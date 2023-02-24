using OszkConnector;
using OszkConnector.Models;

namespace Tests
{
    public class ConnectorTests
    {
        [Fact]
        public async void FindAudioBook()
        {
            var client = new Client();
            var result = await client.FindAudioBook("gardonyi");
            Assert.NotNull(result);

            Assert.Equal(result.Count(), 15);
        }

        [Fact]
        public async void GetBook()
        {
            var client = new Client();
            var book = await client.GetBook("02900/02965");
            AssertBook(book);

            var book2 = await client.GetBook("2965");
            AssertBook(book2);

            var book3 = await client.GetBook("MEK-2965");
            AssertBook(book3);

            Assert.Equal(book, book2);
            Assert.Equal(book, book3);
        }

        [Fact]
        public async void GetAudioBook()
        {
            var client = new Client();
            var audiobook = await client.GetAudioBook("02900/02965");
            Assert.NotEmpty(audiobook.Id);

            Assert.NotNull(audiobook.Tracks);
            Assert.NotEmpty(audiobook.Tracks);

        }


        public void AssertBook(Book book)
        {
            Assert.NotNull(book);
            Assert.NotEmpty(book.Id);
            Assert.NotEmpty(book.FullTitle);
            Assert.NotEmpty(book.Author);
            Assert.NotEmpty(book.Title);
        }
    }
}
