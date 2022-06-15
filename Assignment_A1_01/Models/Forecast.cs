using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment_A1_01.Models
{
    public class Forecast
    {
        public string City { get; set; }
        public List<ForecastItem> Items { get; set; }
    }
}
