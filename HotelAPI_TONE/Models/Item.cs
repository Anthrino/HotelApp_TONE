using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAPI_TONE.Models
{
	public class Item
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Category { get; set; }
		public string Description { get; set; }
		//public byte[] image { get; set; }
		public decimal Price { get; set; }
	}
}
