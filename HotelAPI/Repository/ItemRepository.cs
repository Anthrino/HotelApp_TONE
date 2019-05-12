using HotelAPI_TONE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAPI_TONE.Repository
{
	public class ItemRepository
	{
		private HotelAPI_TONEContext _context;

		public ItemRepository(HotelAPI_TONEContext context)
		{
			_context = context;
		}

		public async Task<Item> GetItem(int id)
		{
			var item = await _context.Item.FindAsync(id);
			return item;
		}
	}
}
