using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace DataAccessLayer.Repository
{
	public class CartRepository
	{
		private IRepository<Cart> cartRepo { get; set; }

		public CartRepository()
		{
			this.cartRepo = new Repository<Cart>();
		}

		public async Task Delete(Cart entity)
		{
			await this.cartRepo.Delete(entity).ConfigureAwait(false);
			this.cartRepo.SaveChanges();
		}

		public async Task<Cart> Get(int id)
		{
			var result = await this.cartRepo.Get(id).ConfigureAwait(false);
			return result;
		}

		public async Task<ICollection<Cart>> GetAll()
		{
			var results = await this.cartRepo.GetAll().ConfigureAwait(false);
			return results;
		}

		public async Task<int> Insert(Cart entity)
		{
			var item = await this.cartRepo.Insert(entity).ConfigureAwait(false);
			this.cartRepo.SaveChanges();
			return item.Id;
		}

		public async Task Update(Cart entity)
		{
			await this.cartRepo.Update(entity).ConfigureAwait(false);
			this.cartRepo.SaveChanges();
		}
	}
}