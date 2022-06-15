using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Text.Json;
using System.Threading;
using Assignment_A1_03.Models;

namespace Assignment_A1_03.Services
{
    public class OpenWeatherService : EventArgs // tror inte den ska va där
    {
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "cad5fcb116a44f89fda1cf6556bc21dd"; // Your API Key

        // part of your event and cache code here
        static ConcurrentDictionary<(string, string), Forecast> _finnsDetCache = new ConcurrentDictionary<(string, string), Forecast>();

        // BROADCASTER/EVENT-DELEN 1  --  Broadcaster eventet
        public static event EventHandler<string> WrittenToFile, WrittenToFile2;// WrittenToFile är Event/deligate-variabel

        // BROADCASTER/EVENT-DELEN 2  --  AVFYRA/INVOKERA EVENTET WrittenToFile  
        public void OnWrittenToFile(string e) // OnWrittenToFile metoden, hittas av service.??
        {
            WrittenToFile?.Invoke(this, e);
        }
        public void OnWrittenToFile2(string e) // OnWrittenToFile en metod
        {
            WrittenToFile2?.Invoke(this, e);
            if (_finnsDetCache.Count == 0)
            {
                Console.WriteLine("Det finns inget i CACHE:n OOooOOooOOooOOooOOooOOooOO...");
            }
            else
            {
                Console.WriteLine("NU FINNS DET I CACH 22222222222222222222222222...");
            }

        }

        // City-delen
        public async Task<Forecast> GetForecastAsync(string City)
        {
            //part of cache code here
            
            Forecast forecast = null;
            var key = (DateTime.Now.ToString("yyyy-MM-dd HH:mm"), City);

            // BROADCASTER/EVENT-DELEN 3  --  Anropar eventet
            OnWrittenToFile("Från OnWrittenToFile alldeles innan cachen i CITY-delen");
            OnWrittenToFile2("Meddelande från OnWrittenToFile2 innan cachen i CITY-delen  ");

            if (!_finnsDetCache.TryGetValue(key, out forecast))
            {
                var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                var uri = $"https://api.openweathermap.org/data/2.5/forecast?q={City}&units=metric&lang={language}&appid={apiKey}";
                
                //int milliseconds = 1000;
                //Thread.Sleep(milliseconds);
                
                forecast = await ReadWebApiAsync(uri);
                
                //part of event and cache code here
                //generate an event with different message if cached data
                _finnsDetCache[key] = forecast;

                OnWrittenToFile("CITY Hämtat från NÄTET/API inte från cachen.... DETTA ÄR IFRÅN ETT EVENT MEASSGE från - GetForecastAsync(string City)");
            }
            OnWrittenToFile("ALLDELES OVAN RETURN City-delen MIAMI ");
            return forecast;
        }

        // Longitude Latitude delen
        public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
        {
            //part of cache code here

            double value1 = latitude;
            double value2 = longitude;
            //Console.WriteLine(value1.ToString());
            string stringV1 = value1.ToString();

            //Console.WriteLine(value2.ToString());
            string stringV2 = value2.ToString();

            string str = $"{stringV1}{stringV2}";
            //Console.WriteLine(str);

            Forecast forecast = null;
            var key = (DateTime.Now.ToString("yyyy-MM-dd HH:mm"), str);
            
            OnWrittenToFile("OnWrittenToFile alldeles innan cachen i LONG LAT - delen");
            if (!_finnsDetCache.TryGetValue(key, out forecast))
            {
                var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={apiKey}";

                //int milliseconds = 1000;
                //Thread.Sleep(milliseconds);

                forecast = await ReadWebApiAsync(uri);
                
                //part of event and cache code here
                //generate an event with different message if cached data

                OnWrittenToFile("Hämtat från NÄTET/API inte cachen.... DEN HÄR ÄR FRÅN LONG OCH LATTITUDE-delen");
                _finnsDetCache[key] = forecast; // var _finnsDetCache2 förut
            }
            OnWrittenToFile("ALLDELES OVAN LONG LAT-delens RETURN ");
            return forecast;
            
            //part of event and cache code here
            //generate an event with different message if cached data
        }

        // För både lat och city
        private async Task<Forecast> ReadWebApiAsync(string uri)
        {
            //Read the response from the WebApi-----------------------------
            // part of your read web api code here

            // part of your data transformation to Forecast here
            //generate an event with different message if cached data
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            WeatherApiData wd = await response.Content.ReadFromJsonAsync<WeatherApiData>();

            // Här kan en eventdel vara 
            OnWrittenToFile($"FRÅN ReadWebApiAsync ----  OnWrittenToFile{wd.city.name}");
            
            //int milliseconds = 1000;
            //Thread.Sleep(milliseconds);

            Forecast forecast = new Forecast();
            forecast.City = wd.city.name;
            forecast.Items = wd.list.Select(x => new ForecastItem
            {
                DateTime = Convert.ToDateTime(DateTime.Parse(x.dt_txt).ToString("yyyy-MM-dd HH:mm:ss")),
                Temperature = x.main.temp,
                Description = x.weather.FirstOrDefault().description,
                Icon = x.weather.FirstOrDefault().icon
            }).ToList();
            OnWrittenToFile("DEN HÄR ÄR FRÅN ReadWebApiAsync(string uri) ANVÄNDS AV BÅDE AV CITY OCH LONG LAT");
            return forecast;
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
