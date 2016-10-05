using OszkConnector;
using OszkConnector.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Tests
{
    public class OszkConnectorTests
    {
        [Fact]
        public async void MekCatalog()
        {
            var client = new Client();
            var result = await client.FindAudioBook("gardonyi");
            Assert.NotNull(result);
        }

        [Fact]
        public void MekMp3BookPageParse()
        {
            string pagecontent = File.ReadAllText(@"./fixtures/mek_page_mp3.html");
            Assert.NotEmpty(pagecontent);

            var tracks = Client.ParseMekAudioBookHtml(pagecontent);
            Assert.NotNull(tracks);
            Assert.NotEmpty(tracks);
        }
    }
}
