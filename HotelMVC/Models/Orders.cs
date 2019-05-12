using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HotelMVC_TONE.Models
{
	public class Orders
	{
		public int Id { get; set; }
		[DisplayName("Order Id#")]
		public int orderId { get; set; }
		public int userId { get; set; }
		public int itemId { get; set; }
		[DataType(DataType.Currency)]
		public decimal price { get; set; }
		[Range(0, 20, ErrorMessage = "Please select Number within (0,20)")]
		public int quantity { get; set; }
		public DateTime dop { get; set; }

	}
}
