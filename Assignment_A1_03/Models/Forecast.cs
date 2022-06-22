using System;
using System.Collections.Generic;

namespace Assignment_A1_03.Models
{
    public class Forecast
    {
        public string City { get; set; }
        public string City2 { get; set; }// bara för test
        public List<ForecastItem> Items { get; set; }
    }

}
