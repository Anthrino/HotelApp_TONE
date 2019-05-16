using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace DataAccessLayer.Repository
{
	public class UserRepository
	{
		private IRepository<Users> userRepo { get; set; }

		public UserRepository()
		{
			this.userRepo = new Repository<Users>();
		}

		public async Task Delete(Users entity)
		{
			await this.userRepo.Delete(entity).ConfigureAwait(false);
			this.userRepo.SaveChanges();
		}

		public async Task<Users> Get(int id)
		{
			var result = await this.userRepo.Get(id).ConfigureAwait(false);
			return result;
		}

		public async Task<ICollection<Users>> GetAll()
		{
			var results = await this.userRepo.GetAll().ConfigureAwait(false);
			return results;
		}

		public async Task<int> Insert(Users entity)
		{
			var item = await this.userRepo.Insert(entity).ConfigureAwait(false);
			this.userRepo.SaveChanges();
			return item.Id;
		}

		public async Task Update(Users entity)
		{
			await this.userRepo.Update(entity).ConfigureAwait(false);
			this.userRepo.SaveChanges();
		}
	}
}