#define UseNewsApiSample  // Remove or undefine to use your own code to read live data

using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Threading.Tasks;

using Assignment_A2_01.Models;
using Assignment_A2_01.ModelsSampleData;
namespace Assignment_A2_01.Services
{
    public class NewsService
    {
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "d318329c40734776a014f9d9513e14ae";
        public async Task<NewsApiData> GetNewsAsync()
        {

#if UseNewsApiSample      
            NewsApiData nd = await NewsApiSampleData.GetNewsApiSampleAsync("sports");

#else
            //https://newsapi.org/docs/endpoints/top-headlines
            var uri = $"https://newsapi.org/v2/top-headlines?country=se&category=sports&apiKey={apiKey}";

            //Your code here to read live data
#endif            
            return nd;
        }
    }
}
