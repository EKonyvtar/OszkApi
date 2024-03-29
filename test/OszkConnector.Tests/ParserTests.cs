﻿using OszkConnector.Models;

namespace Tests
{
    public class ParserTests
    {

        [Fact]
        public void MekAudioBookPageParseTest()
        {
            string pagecontent = File.ReadAllText(@"./fixtures/mek_page_audiobook.html");
            //throw new NotImplementedException();
        }

        [Fact]
        public void MekIndexParseTest()
        {
            string pagecontent = File.ReadAllText(@"./fixtures/index.xml");
            Assert.NotEmpty(pagecontent);

            var book = MekFactory.CreateBookFromIndex(pagecontent);
            Assert.NotNull(book);
            Assert.NotEmpty(book.Title);
        }

        [Fact]
        public void MekContentsParseTest()
        {
            string pagecontent = File.ReadAllText(@"./fixtures/mek_page_fulszoveg.html");
            Assert.NotEmpty(pagecontent);

            var book = MekFactory.CreateBookFromContentsPage(pagecontent);

            Assert.NotNull(book);
            Assert.NotEmpty(book.Contents);
            Assert.NotEmpty(book.Prologue);
            Assert.NotEmpty(book.Epilogue);
            Assert.NotEmpty(book.Summary);
        }

        [Fact]
        public void MekMp3BookPageParseTest()
        {
            string pagecontent = File.ReadAllText(@"./fixtures/mek_page_mp3.html");
            Assert.NotEmpty(pagecontent);

            var audioBook = MekFactory.CreateAudioBookFromMP3Page("", pagecontent);
            Assert.NotNull(audioBook.Tracks);
            Assert.NotEmpty(audioBook.Tracks);

            //All tracks should be parsed
            Assert.Equal(audioBook.Tracks.Count(), 26);
        }

        [Fact]
        public void MekM3UParseTest()
        {
            string pagecontent = File.ReadAllText(@"./fixtures/mek_audiobook.m3u");
            Assert.NotEmpty(pagecontent);
        }
    }
}
