using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelAPI_TONE.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelAPI_TONE.Repository
{
	public class UserRepository
	{
		private readonly HotelAPI_TONEContext _context;

		public UserRepository(HotelAPI_TONEContext context)
		{
			_context = context;
		}

		public async Task<int> GetOrderCount(int userId)
		{
			Users user = await _context.Users.FindAsync(userId);
			user.orderCount += 1;
			if (await EditUser(userId, user))
				return user.orderCount;
			else
				return -1;
		}	

		public async Task<bool> EditUser(int id, Users user)
		{

			_context.Entry(user).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserExists(id))
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


		private bool UserExists(int id)
		{
			return _context.Orders.Any(e => e.Id == id);
		}
	}
}
