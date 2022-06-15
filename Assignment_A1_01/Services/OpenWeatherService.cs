using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using Assignment_A1_01.Models;

namespace Assignment_A1_01.Services
{
    public class OpenWeatherService
    {
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "cad5fcb116a44f89fda1cf6556bc21dd"; // Your API Key
        public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
        //public async Task<Forecast> GetForecastAsync()        
        {
            var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={apiKey}";

            //var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat=59.5086798659495&lon=18.265462593297&units=metric&lang=se&appid=54da4dc118007e03cd58c4e98fdb3bf6";

            //Read the response from the WebApi-----------------------------------------------------------
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            WeatherApiData wd = await response.Content.ReadFromJsonAsync<WeatherApiData>();

            Forecast forecast = new Forecast();
            forecast.City = wd.city.name;
            forecast.Items = wd.list.Select(x => new ForecastItem
            {
                //DateTime = UnixTimeStampToDateTime(x.dt_txt),//x.dt_txt,
                //DateTime = x.dt_txt,
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
    }
}
