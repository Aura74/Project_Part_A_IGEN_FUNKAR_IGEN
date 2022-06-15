using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using Assignment_A1_02.Models;
using System.Collections.Concurrent;

//  the service should fire an event with a message.


namespace Assignment_A1_02.Services
{
    public class OpenWeatherService
    {
		// cach
		//static ConcurrentDictionary<(int, int), PrimeSuite> _primeNumberCache = new ConcurrentDictionary<(int, int), PrimeSuite>();

        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "cad5fcb116a44f89fda1cf6556bc21dd"; // Your API Key

        public async Task<Forecast> GetForecastAsync(string City)
        {
            var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var uri = $"https://api.openweathermap.org/data/2.5/forecast?q={City}&units=metric&lang={language}&appid={apiKey}";
            Forecast forecast = await ReadWebApiAsync(uri);
            return forecast;
        }
        public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
        {
            var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={apiKey}";
            Forecast forecast = await ReadWebApiAsync(uri);
            return forecast;
        }

        private async Task<Forecast> ReadWebApiAsync(string uri)
        {
            //Read the response from the WebApi---
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            WeatherApiData wd = await response.Content.ReadFromJsonAsync<WeatherApiData>();

            Forecast forecast = new Forecast();
            forecast.City = wd.city.name;
            forecast.Items = wd.list.Select(x => new ForecastItem
            {
                DateTime = Convert.ToDateTime(DateTime.Parse(x.dt_txt).ToString("yyyy-MM-dd HH:mm:ss")),
                Temperature = x.main.temp,
                Description = x.weather.FirstOrDefault().description,
                Icon = x.weather.FirstOrDefault().icon
            }).ToList();
            return forecast;
        }
        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }


        // kopierat från 3an
        public event Action<object, string> WeatherForecastAvailable;
		public void OnWeatherForecastAvailable(object sender, string e)
        {
            WeatherForecastAvailable?.Invoke(sender, e);
			Console.WriteLine("Det fungerade - Va bra" + e);
        }

		//public async Task<Forecast> GetForecastAsync(string city, string country)
		//{
		//	var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
		//	var uri = $"https://api.openweathermap.org/data/2.5/forecast?q={city},{country}&units=metric&lang={language}&appid={apiKey}";
		//	Forecast forecast = await ReadWebApiAsync(uri);
		//	return forecast;
		//}
		public event Action<object, string> NO_WeatherForecastNotAvailable;
		public void NO_WeatherForecastAvailable(object sender, string e)
        {
	        NO_WeatherForecastNotAvailable?.Invoke(sender, e);
        }


		// // Auto-genererat
		//private async Task<Forecast> ReadWebApiAsync(string uri)
		//{
		//    var response = await httpClient.GetAsync(uri);
		//    if (response.StatusCode == HttpStatusCode.OK)
		//    {
		//        var json = await response.Content.ReadAsStringAsync();
		//        return JsonSerializer.Deserialize<Forecast>(json);
		//    }
		//    else
		//    {
		//        return null;
		//    }
		//}


		//public event EventHandler<string> WeatherForecastAvailable;
		//public void OnWeatherForecastAvailable(object sender, string e)
		//{
		//	WeatherForecastAvailable?.Invoke(sender, e);
		//}
	}
}
