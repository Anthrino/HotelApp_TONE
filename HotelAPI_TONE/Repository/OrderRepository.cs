using HotelAPI_TONE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAPI_TONE.Repository
{
	public class OrderRepository
	{
		private readonly HotelAPI_TONEContext _context;

		public OrderRepository(HotelAPI_TONEContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Orders>> GetOrders(int userId)
		{
			return await _context.Orders.Where(order => order.userId == userId).ToListAsync();
		}

		public async Task<Orders> GetOrder(int id)
		{
			var order = await _context.Orders.FindAsync(id);
			return order;
		}

		public async Task<bool> EditOrder(int id, Orders order)
		{

			_context.Entry(order).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrderExists(id))
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

		public async Task<bool> AddOrder(Orders order)
		{
			_context.Orders.Add(order);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrderExists(order.Id))
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

		public async Task<bool> DeleteOrder(int id)
		{

			var order = await _context.Orders.FindAsync(id);
			if (order == null)
			{
				return false;
			}

			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();

			return true;
		}

		private bool OrderExists(int id)
		{
			return _context.Orders.Any(e => e.Id == id);
		}


	}
}
