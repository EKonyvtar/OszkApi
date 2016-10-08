using OszkConnector;
using OszkConnector.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

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
        public async void GetAudioBookByUrlId()
        {
            var client = new Client();
            var book = await client.GetBook("02900/02965");
            Assert.NotNull(book);
            Assert.NotEmpty(book.FullTitle);
            Assert.NotEmpty(book.Author);
            Assert.NotEmpty(book.Title);
        }

        [Fact]
        public async void GetAudioBookById()
        {
            var client = new Client();
            var book = await client.GetBook(2965);
            Assert.NotNull(book);
            Assert.NotEmpty(book.FullTitle);
            Assert.NotEmpty(book.Author);
            Assert.NotEmpty(book.Title);
        }
    }
}
