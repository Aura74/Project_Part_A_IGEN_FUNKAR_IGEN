using Assignment_A1_03.Models;
using Assignment_A1_03.Services;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment_A1_03
{
    class Program 
    {
        static async Task Main(string[] args)// Main är subscriber av event
        {
            //Register the event
            OpenWeatherService service = new OpenWeatherService();
            //service.WeatherForecastAvailable += ReportWeatherDataAvailable;

            // ASSAIGNA EVENTHANDLERN - SUBSCRIBER-DEL 2 -- kan vara vartsomhelst,
            OpenWeatherService.WeatherForecastAvailable += AppLogWrittenHandler;
            OpenWeatherService.WeatherForecastAvailable2 += AppLogWrittenHandler;
            // Från katan
            //HiltonMembers.ListSortedEvent += ListSortedEventHandler;

            //OpenWeatherService.WeatherForecastAvailable += ReportWeatherDataAvailable;
            //OpenWeatherService.WeatherForecastAvailable2 += ReportWeatherDataAvailable;

            Task <Forecast> t1 = null, t2 = null, t3 = null, t4 = null;
            Exception exception = null;
            try
            {
                double latitude = 59.5086798659495;
                double longitude = 18.2654625932976;

                t1 = service.GetForecastAsync(latitude, longitude);
                t2 = service.GetForecastAsync("Miami");

                Task.WaitAll(t1, t2);

                Console.WriteLine("Task 1 an 2 completed\n");
                Console.WriteLine();

                t3 = service.GetForecastAsync(latitude, longitude);
                t4 = service.GetForecastAsync("Miami");


                //Wait and confirm we get an event showing cahced data avaialable
                Task.WaitAll(t3, t4);
                Console.WriteLine("Task 3 an 4 completed\n");
            }
            catch (Exception ex)
            {
                //if exception write the message later
                Console.WriteLine("Exception: " + ex.Message);
                exception = ex;
            }

            try
            {
                Console.WriteLine("*******************************************************");
                Console.WriteLine("------t1-----------");
                Console.WriteLine("*******************************************************\n");
                if (t1?.Status == TaskStatus.RanToCompletion)
                {
                    Forecast forecast = t1.Result;
                    
                    Console.WriteLine($"Weather forecast for {forecast.City}");
                    Console.WriteLine($"Weather forecast for {forecast.City2}");
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

                Console.WriteLine();
                Console.WriteLine("*******************************************************");
                Console.WriteLine("------t2-----------");
                Console.WriteLine("*******************************************************");
                if (t2?.Status == TaskStatus.RanToCompletion)
                {
                    Forecast forecast = t2.Result;
                    Console.WriteLine($"Weather forecast for {forecast.City}");
                    Console.WriteLine($"Weather forecast for {forecast.City2}");
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

                Console.WriteLine();
                Console.WriteLine("*******************************************************");
                Console.WriteLine("------t3-----------");
                Console.WriteLine("*******************************************************");

                    if (t3?.Status == TaskStatus.RanToCompletion)
                    {
                        Forecast forecast = t3.Result;
                        Console.WriteLine($"Weather forecast for {forecast.City}");
                        //Console.WriteLine($"Weather forecast for {forecast.City2}");
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
                        //Console.WriteLine($"Error: {exception.Message}");
                    }

                    Console.WriteLine();
                    Console.WriteLine("*******************************************************");
                    Console.WriteLine("------t4-----------");
                    Console.WriteLine("*******************************************************");

                    if (t4?.Status == TaskStatus.RanToCompletion)// fel
                    {

                    Forecast forecast = t4.Result;
                        Console.WriteLine($"Weather forecast for {forecast.City}");
                        //Console.WriteLine($"Weather forecast for {forecast.City2}");
                        var GroupedList = forecast.Items.GroupBy(item => item.DateTime.Date);
                        foreach (var group in GroupedList)
                        {
                            Console.WriteLine(group.Key.Date.ToShortDateString());
                            foreach (var item in group)
                            {
                                Console.WriteLine(
                                    $"   - {item.DateTime.ToShortTimeString()}: {item.Description}, teperature: {item.Temperature} degC, wind: {item.WindSpeed} m/s");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"City weather service error");
                        //Console.WriteLine($"Error: {exception.Message}");
                    }
            }
            catch (Exception ex)
            {
                //if exception write the message later
                Console.WriteLine("NÅGOT GICK FEL Exception: " + ex.Message);
                exception = ex;
            }

        }



        // EVENTHANDLER - SUBSCRIBER-DELEN 1 -- 
        static void AppLogWrittenHandler(object sender, string wd)// object sender this
        {
            Console.WriteLine($"Event-Meddelandet LYDER: {wd}");
        }

        static void ReportWeatherDataAvailable(object sender, string wd)// vill inte funka
        {
            Console.WriteLine($"NYTT NYTT NYTT NYTT: {wd}");
        }

        // Från katan
        static void ListSortedEventHandler(object? sender, int NrOfItems)
        {
            Console.WriteLine($"#### {NrOfItems} sorted");
        }

    }
}
