using HotelAPI_TONE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAPI_TONE.Repository
{
	public class CartRepository
	{
		private readonly HotelAPI_TONEContext _context;
		private readonly ILogger _logger;

		public CartRepository(HotelAPI_TONEContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Cart>> GetCartItems(int userId)
		{
			return await _context.Cart.Where(cart => cart.userId == userId).ToListAsync();
		}

		public async Task<Cart> GetCart(int id)
		{
			var cart = await _context.Cart.FindAsync(id);
			return cart;
		}

		public async Task<bool> EditCart(int id, Cart cart)
		{

			_context.Entry(cart).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CartExists(id))
				{
					return false;
				}
				else
				{
					throw;
				}
			}

			return true;
		}

		public async Task<bool> AddCart(Cart cart)
		{
			_context.Cart.Add(cart);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CartExists(cart.Id))
				{
					return false;
				}
				else
				{
					throw;
				}
			}

			return true;
		}

		public async Task<bool> DeleteCart(int id)
		{

			var cart = await _context.Cart.FindAsync(id);
			if (cart == null)
			{
				return false;
			}

			_context.Cart.Remove(cart);
			await _context.SaveChangesAsync();

			return true;
		}

		private bool CartExists(int id)
		{
			return _context.Cart.Any(e => e.Id == id);
		}


	}
}
