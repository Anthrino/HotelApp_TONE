using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAPI_TONE.Models
{
	public class Cart
	{
		public int Id { get; set; }
		public int userId { get; set; }
		public int itemId { get; set; }
		public int quantity { get; set; }
	}
}
