using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace OszkConnector.Models
{
    [DataContract]
    public class Book
    {

        private static string URL_MEK_THUMBNAIL = "http://mek.oszk.hu/{0}/borito.jpg";

        private string _id = null;
        [DataMember(Order = 0)]
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                Metadata.Id = value;
            }
        }


        private string _fullTitle = null;
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public string FullTitle
        {
            get { return _fullTitle ?? $"{Author}: {Title}"; }
            set { _fullTitle = value; }
        }

        //TODO: eliminate UrlId
        private string _urlId = null;

        public string UrlId
        {
            get { return _urlId ?? $"{Id?.Substring(0, 3)}00/{Id}"; }
            set { _urlId = value; }
        }

        private Uri _ThumbnailUrl = null;
        [DataMember]
        public Uri ThumbnailUrl
        {
            get
            {
                return _ThumbnailUrl ?? new Uri(string.Format(URL_MEK_THUMBNAIL, UrlId));
            }
            set { _ThumbnailUrl = value; }
        }

        [DataMember(Name = "__metadata")]
        public BookMetadata Metadata { get; set; }

        public Book()
        {
            Metadata = new BookMetadata();
        }

        [DataMember(EmitDefaultValue = false)]
        public string Author { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Title { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Urn { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string MekId { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public Uri Source { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Language { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Period { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Contents { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Prologue { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Epilogue { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Summary { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Publisher { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string PublishYear { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string PublishPlace { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Contributor> Creators { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Contributor> Contributors { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<string> Type { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<string> Topics { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<string> Tags { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<string> KeyWords { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public List<Book> Related { get; set; }

        public override string ToString()
        {
            return $"[{Id}] {FullTitle}";
        }

        public override bool Equals(object other)
        {
            try
            {
                var otherBook = (Book)other;
                return (
                    FullTitle == otherBook.FullTitle &&
                    Id == otherBook.Id
                    );
            }
            catch
            {
                //TODO: logging
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public void Copy(Book from)
        {
            this.Author = from.Author;
            this.Contents = from.Contents;
            this.Contributors = from.Contributors;
            this.Creators = from.Creators;
            this.Epilogue = from.Epilogue;
            this.FullTitle = from.FullTitle;
            this.Id = from.Id;
            this.KeyWords = from.KeyWords;
            this.Language = from.Language;
            this.MekId = from.MekId;
            this.Metadata = from.Metadata;
            this.Period = from.Period;
            this.Prologue = from.Prologue;
            this.Publisher = from.Publisher;
            this.PublishPlace = from.PublishPlace;
            this.PublishYear = from.PublishYear;
            this.Related = from.Related;
            this.Source = from.Source;
            this.Summary = from.Summary;
            this.Tags = from.Tags;
            this.ThumbnailUrl = from.ThumbnailUrl;
            this.Title = from.Title;
            this.Type = from.Type;
            this.Topics = from.Topics;
            this.UrlId = from.UrlId;
            this.Urn = from.Urn;
        }
    }
}
