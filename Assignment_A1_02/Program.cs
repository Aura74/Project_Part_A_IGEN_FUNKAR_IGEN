using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

using Assignment_A1_02.Models;
using Assignment_A1_02.Services;

namespace Assignment_A1_02

// Exception handling should be implemented
// so incase a forecast is not available for a city an appropriate message should be print
// Program.Main should be a subscriber of the event from OpenWeatherService and print out
// the message.
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Register the event
            OpenWeatherService service = new OpenWeatherService();
            service.WeatherForecastAvailable += ReportWeatherDataAvailable;
            
            Task<Forecast> t1 = null, t2 = null;
            Exception exception = null;
            try
            {

                double latitude = 59.5086798659495;
                double longitude = 18.2654625932976;

                //Create the two tasks and wait for comletion
                t1 = service.GetForecastAsync(latitude, longitude);
                t2 = service.GetForecastAsync("Miami");


				Task.WaitAll(t1, t2);
            }
            catch (Exception ex)
            {
                //if exception write the message later
                exception = ex;
            }


            // Ovanåkersdelen
            Console.WriteLine();
            Console.WriteLine("----Ovanåkers kommun-------------");
            if (t1?.Status == TaskStatus.RanToCompletion)
            {
                Forecast forecast = t1.Result;
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
            else
            {
                Console.WriteLine($"Geolocation weather service error.");
                Console.WriteLine($"Error: {exception.Message}");
            }


            try
            {
                // Miami
                Console.WriteLine();
                Console.WriteLine("-------MIAMI DELEN ----------");
                if (t2.Status == TaskStatus.RanToCompletion)
                {
                    Forecast forecast = t2.Result;
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
                else
                {
                    Console.WriteLine($"City weather service error");
                    Console.WriteLine($"Error: {exception.Message}");
                }
            }
            catch
            {
                Console.WriteLine("Inget väder i Miami antar jag");
            }


            
        }
        static void ReportWeatherDataAvailable(object sender, string message)// Samma som alarm
        {
            Console.WriteLine($"Event message from weather service: {message}");
        }

    }
}