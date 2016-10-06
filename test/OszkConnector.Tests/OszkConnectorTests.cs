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

            Assert.Equal(result.Count(), 15);
        }

        [Fact]
        public void MekAudioBookPageParseTest()
        {
            string pagecontent = File.ReadAllText(@"./fixtures/mek_page_audiobook.html");
            Assert.NotEmpty(pagecontent);
        }

        [Fact]
        public void MekMp3BookPageParseTest()
        {
            string pagecontent = File.ReadAllText(@"./fixtures/mek_page_mp3.html");
            Assert.NotEmpty(pagecontent);

            var tracks = MekConverter.ParseMekMP3Page(pagecontent);
            Assert.NotNull(tracks);
            Assert.NotEmpty(tracks);

            //All tracks should be parsed
            Assert.Equal(tracks.Count(), 26);
        }

        [Fact]
        public void MekM3UParseTest()
        {
            string pagecontent = File.ReadAllText(@"./fixtures/mek_audiobook.m3u");
            Assert.NotEmpty(pagecontent);
        }
    }
}
