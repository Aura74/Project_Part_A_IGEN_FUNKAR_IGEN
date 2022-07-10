using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_A1_03.Services
{
    internal class MyEvent : EventArgs
	{
		public decimal oldPrice { get; set; }
		public decimal newPrice { get; set; }

		public string oldText1 { get; set; }
		public string newText1 { get; set; }
	}
}
