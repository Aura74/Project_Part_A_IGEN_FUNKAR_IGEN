using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using Assignment_A1_01.Models;
using Assignment_A1_01.Services;

namespace Assignment_A1_01
{
    class Program
    {
        static async Task Main(string[] args)
        {
            double latitude = 59.5086798659495;
            double longitude = 18.2654625932976;

            var t1 = await new OpenWeatherService().GetForecastAsync(latitude, longitude);

            var nytt = t1.Items.Count;
            Console.WriteLine(nytt);

            Console.WriteLine($"I nådens år {t1.City.ToString()}");

            foreach (var item in t1.Items)
            {
                Console.WriteLine($"Datumet {item.DateTime.ToShortDateString()} klockan  {item.DateTime.ToShortTimeString()} är det {item.Description.ToString()} och temperaturen är {item.Temperature.ToString()} Grader Celsius");
                //Console.WriteLine($"och temperaturen är {item.Temperature.ToString()} Grader Celsius");
                Console.WriteLine();
            }

            Console.WriteLine("Martins version");
            Forecast forecast = t1;
            Console.WriteLine($"Weather forecast for {forecast.City}");
            var GroupedList = forecast.Items.GroupBy(item => item.DateTime.Date);
            foreach (var group in GroupedList)
            {
                Console.WriteLine(group.Key.Date.ToShortDateString());
                foreach (var item in group)
                {
                    Console.WriteLine($"   - {item.DateTime.ToShortTimeString()}: {item.Description}, teperature: {item.Temperature} degC, wind: {item.WindSpeed} m/s");
                }
            }


        }
    }
}
