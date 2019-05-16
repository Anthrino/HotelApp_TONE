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
	public class ItemRepository
	{
		private IRepository<Item> itemRepo { get; set; }

		public ItemRepository()
		{
			this.itemRepo = new Repository<Item>();
		}

		public async Task Delete(Item entity)
		{
			await this.itemRepo.Delete(entity).ConfigureAwait(false);
			this.itemRepo.SaveChanges();
		}

		public async Task<Item> Get(int id)
		{
			var result = await this.itemRepo.Get(id).ConfigureAwait(false);
			return result;
		}

		public async Task<ICollection<Item>> GetAll()
		{
			var results = await this.itemRepo.GetAll().ConfigureAwait(false);
			return results;
		}

		public async Task<int> Insert(Item entity)
		{
			var item = await this.itemRepo.Insert(entity).ConfigureAwait(false);
			this.itemRepo.SaveChanges();
			return item.Id;
		}

		public async Task Update(Item entity)
		{
			await this.itemRepo.Update(entity).ConfigureAwait(false);
			this.itemRepo.SaveChanges();
		}
	}
}