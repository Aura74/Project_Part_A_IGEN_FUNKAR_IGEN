using Assignment_A1_03.Models;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Threading.Tasks;

namespace Assignment_A1_03.Services
{
    public class OpenWeatherService //: EventArgs // tror inte EventArgs ska vara där???
    {
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "cad5fcb116a44f89fda1cf6556bc21dd";

        // part of your event and cache code here
        
        static ConcurrentDictionary<(string, string), Forecast> _finnsDetCache = new ConcurrentDictionary<(string, string), Forecast>();

        // PUBLISHER/BROADCASTER/EVENT-DEL 1  --  Broadcaster, DETTA ÄR eventet, Tar en string som parameter
        public static event EventHandler<string> WeatherForecastAvailable, WeatherForecastAvailable2;// WeatherForecastAvailable är en Event/deligate-variabel




        ////Event handler and On... Method that fires the event, Från katan
        //public EventHandler<int> ListSortedEvent;
        //public void OnListSorted(int NrOfItems) => ListSortedEvent?.Invoke(this, NrOfItems);

        ////Modified Sort that invokes the ListSorted event
        //public void Sort()
        //{
        //    _members.Sort();
        //    OnListSorted(_members.Count);
        //}






        // PUBLISHER/BROADCASTER/EVENT-DEL 2  --  Syftet med denna: AVFYRAR/INVOKERA EVENTET -WeatherForecastAvailable  
        public void OnWrittenToFile(string e) // OnWrittenToFile metoden, hittas av service.??
        { 
            WeatherForecastAvailable?.Invoke(this, e);
        }
        
        public void OnWrittenToFile2(string e) // OnWrittenToFile en metod
        {
            WeatherForecastAvailable2?.Invoke(this, e);
            if (_finnsDetCache.Count == 0)
            {
                Console.WriteLine("Det finns INGET i CACHEN, den är tom just nu.\n");
            }
            else
            {
                Console.WriteLine("NU FINNS DET DATA I CACHEN ATT VISA UPP.\n");
            }
        }



        // City-delen
        public async Task<Forecast> GetForecastAsync(string City)
        {
            //part of cache code here
            

            Forecast forecast = null;
            var key = (DateTime.Now.ToString("yyyy-MM-dd HH:mm"), City);
            //Console.WriteLine(key);
            // BROADCASTER/EVENT-DELEN 3  --  AVFYRNINGEN AV eventet
            ////OnWrittenToFile($"Från OnWrittenToFile1 alldeles innan cachen i CITY-delen\n ska visas både om det finns data i cachen eller inte. Har nu fått info från Program.cs att City: {City}\n");

            ////OnWrittenToFile2("Meddelande från OnWrittenToFile2, är placerad innan cachen i CITY-delen.");

            if (!_finnsDetCache.IsEmpty) 
            //if (_finnsDetCache.ContainsKey(key))
            //if (_finnsDetCache.Count == 0)
			{
                //OnWrittenToFile2("Från IF-Satsen Forecast hämtas nu från Nätet - openweather api");
                OnWrittenToFile2("Från IF-Satsen Forecast hämtas nu från CACHEN");
            }
            else
            {
                //OnWrittenToFile2("Från IF-Satsen Forecast hämtas nu från CACHEN");
                OnWrittenToFile2("Från IF-Satsen Forecast hämtas nu från Nätet - openweather api");
            }

            _finnsDetCache.Keys.ToList().ForEach(x =>
            {
                Console.WriteLine($"Key: {x}");
            });

            
            _finnsDetCache.ContainsKey(key).ToString();


            if (!_finnsDetCache.TryGetValue(key, out forecast))
            {
                var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                var uri = $"https://api.openweathermap.org/data/2.5/forecast?q={City}&units=metric&lang={language}&appid={apiKey}";

                forecast = await ReadWebApiAsync(uri);
                //part of event and cache code here
                //generate an event with different message if cached data

                _finnsDetCache[key] = forecast;

                OnWrittenToFile($"***{_finnsDetCache.TryGetValue(key, out forecast)}*** CITY Hämtat från NÄTET/API inte från cachen.\n DETTA ÄR IFRÅN ETT EVENT MEASSGE från - GetForecastAsync(string City), BORDE BARA VISAS NÄR CACH ÄR TOM. \n uri: {uri}\n");

                OnWrittenToFile2("nya cach nya cach *-*-/-*-*-/-*-*/-.");
            }
            OnWrittenToFile("ALLDELES OVAN return forecast i City-delen MIAMI\nOnce a Forecast is received, either by GeoLocation or by City, the service should fire an event with a message.\n");
            return forecast;
        }


        // Longitude Latitude delen
        public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
        {
            //part of cache code here
            
            string str = $"{latitude.ToString()}{longitude.ToString()}"; // räcker med denna

            Forecast forecast = null;
            var key = (DateTime.Now.ToString("yyyy-MM-dd HH:mm"), str);
            ////OnWrittenToFile2("Meddelande från OnWrittenToFile2, är placerad innan cachen i Long Lat-delen");
            Console.WriteLine(key);
            ////OnWrittenToFile("OnWrittenToFile alldeles innan cachen i LONG LAT - delen");
            if (!_finnsDetCache.TryGetValue(key, out forecast))
            {
                var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={apiKey}";

                forecast = await ReadWebApiAsync(uri);

                //part of event and cache code here
                //generate an event with different message if cached data

                _finnsDetCache[key] = forecast; // var _finnsDetCache2 förut
            }
            OnWrittenToFile($"***{_finnsDetCache.TryGetValue(key, out forecast)}*** ALLDELES OVAN LONG LAT-delens return forecast, borde visas vid båda tillfällerna\n Once a Forecast is received, either by GeoLocation or by City, the service should fire an event with a message.\n");
            return forecast;

        }// slutet på public async Task<Forecast> GetForecastAsync

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
            OnWrittenToFile($"DEN HÄR TEXTEN SER DU BARA NÄR DATAN LÄSES IN FRÅN INTERNET OCH INTE CACH, denna text återfinns i ReadWebApiAsync ca rad 136 i skrivandets stund ---- just nu hämtas datan till: {wd.city.name}\n");

            Forecast forecast = new Forecast();//ny
            forecast.City = wd.city.name;//ny
            forecast.Items = wd.list.Select(x => new ForecastItem
            {
                DateTime = Convert.ToDateTime(DateTime.Parse(x.dt_txt).ToString("yyyy-MM-dd HH:mm:ss")),
                Temperature = x.main.temp,
                Description = x.weather.FirstOrDefault().description,
                Icon = x.weather.FirstOrDefault().icon
            }).ToList();
            OnWrittenToFile("SER DU DENNA TEXT ÄR DATAT INTE FRÅN CACHEN, DEN HÄR ÄR FRÅN ReadWebApiAsync(string uri)\n");

            return forecast;
        }

        //private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        //{
        //    DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        //    dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        //    return dateTime;
        //}
    }
}
