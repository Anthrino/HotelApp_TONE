using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAPI_TONE.Models
{
	public class Orders
	{
		public int Id { get; set; }
		public int orderId { get; set; }
		public int userId { get; set; }
		public int itemId { get; set; }
		public decimal price { get; set; }
		public int quantity { get; set; }
		public DateTime dop { get; set; }
	}
}
