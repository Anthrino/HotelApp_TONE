using DataAccessLayer.Repository;
using Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Processors
{
	public class UserProcessor : BaseProcessor
	{
		private readonly UserRepository userRepo;

		public UserProcessor()
		{
			userRepo = new UserRepository();
		}
		private bool UserExists(int id)
		{
			if (userRepo.Get(id).Result != null)
				return true;
			else
				return false;
		}

		public async Task<int> GetOrderCount(int userId)
		{
			Users user = await GetUser(userId);
			user.orderCount += 1;
			await EditUser(userId, user);

			if (GetUser(userId).Result.orderCount == user.orderCount + 1)
				return user.orderCount;
			else
				return -1;
		}

		public async Task<Users> GetUser(int id)
		{
			Users user = await userRepo.Get(id);
			return user;
		}

		public async Task<IEnumerable<Users>> GetUsers()
		{
			return await userRepo.GetAll();
		}

		public async Task EditUser(int id, Users user)
		{
			await userRepo.Update(user);
		}


		public async Task<bool> InsertUser(Users user)
		{
			await userRepo.Insert(user);
			if (!UserExists(user.Id))
			{
				return false;
			}
			return true;
		}

		public async Task<bool> DeleteUser(int id)
		{
			await userRepo.Delete(GetUser(id).Result);
			if (!UserExists(id))
			{
				return true;
			}
			return false;
		}
		public async Task<Users> Authenticate(Users user)
		{
			var userList = await userRepo.GetAll();
			if (userList.Any(u => u.username == user.username))
			{
				user = userList.FirstOrDefault(u => u.username == user.username);
			}
			return user;
		}
	}
}
