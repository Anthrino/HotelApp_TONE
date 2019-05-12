using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAPI_TONE.Models
{
	public class OrderItem
	{
		public OrderItem(Orders order, Item item)
		{
			this.order = order;
			this.item = item;
		}

		public Orders order { get; set; }
		public Item item { get; set; }
	}
}
