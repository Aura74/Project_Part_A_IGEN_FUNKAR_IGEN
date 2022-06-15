using System;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;

using Assignment_A2_01.Models;
namespace Assignment_A2_01.ModelsSampleData
{
    public static class NewsApiSampleData
    {
        #region News Test Data as News Service only allows 100 calls per day
        static public async Task<NewsApiData> GetNewsApiSampleAsync(string category)
        {
            Task<NewsApiData> t = Task.Run(() =>
            {
                NewsApiData n = Deserialize(fname($"sample {category}.xml"));
                return n;
            });

            return await t;

            static NewsApiData Deserialize(string fname)
            {
                var _locker = new object();
                lock (_locker)
                {
                    NewsApiData newsapi;
                    var xs = new XmlSerializer(typeof(NewsApiData));

                    using (Stream s = File.OpenRead(fname))
                        newsapi = (NewsApiData)xs.Deserialize(s);

                    return newsapi;
                }
            }
            static string fname(string name)
            {
                return Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\ModelsSampleData", name);
            }
        }
        #endregion
    }
}
