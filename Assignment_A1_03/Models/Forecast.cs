using System;
using System.Collections.Generic;

namespace Assignment_A1_03.Models
{
    public class Forecast
    {
        public string City { get; set; }
        public List<ForecastItem> Items { get; set; }
    }

    //public class GroupedForecast
    //{
    //    public string City { get; set; }
    //    public IEnumerable<IGrouping<DateTime, ForecastItem>> Items { get; set; }
    //}
}
