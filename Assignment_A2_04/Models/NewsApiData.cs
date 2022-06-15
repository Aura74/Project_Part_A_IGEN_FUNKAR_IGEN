using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assignment_A2_04.Models
{
    public class Source
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class Article
    {
        public Source Source { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Content { get; set; }
    }
    [XmlRoot("NewsApiData", Namespace = "http://mynamespace/test/")] //This line needed only for the SampleData
    public class NewsApiData
    {
        public string Status { get; set; }
        public int TotalResults { get; set; }
        public List<Article> Articles { get; set; }

        public static void Serialize(NewsApiData news, string fname)
        {
            var _locker = new object();
            lock (_locker)
            {
                var xs = new XmlSerializer(typeof(NewsApiData));
                using (Stream s = File.Create(fname))
                    xs.Serialize(s, news);
            }
        }
        public static NewsApiData Deserialize(string fname)
        {
            var _locker = new object();
            lock (_locker)
            {
                NewsApiData news;
                var xs = new XmlSerializer(typeof(NewsApiData));

                using (Stream s = File.OpenRead(fname))
                    news = (NewsApiData)xs.Deserialize(s);

                return news;
            }
        }

    }
}
