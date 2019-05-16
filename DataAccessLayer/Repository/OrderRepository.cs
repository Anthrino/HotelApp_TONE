using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace DataAccessLayer.Repository
{
	public class OrderRepository
	{
		private IRepository<Orders> orderRepo { get; set; }

		public OrderRepository()
		{
			this.orderRepo = new Repository<Orders>();
		}

		public async Task Delete(Orders entity)
		{
			await this.orderRepo.Delete(entity).ConfigureAwait(false);
			this.orderRepo.SaveChanges();
		}

		public async Task<Orders> Get(int id)
		{
			var result = await this.orderRepo.Get(id).ConfigureAwait(false);
			return result;
		}

		public async Task<ICollection<Orders>> GetAll()
		{
			var results = await this.orderRepo.GetAll().ConfigureAwait(false);
			return results;
		}

		public async Task<int> Insert(Orders entity)
		{
			var order = await this.orderRepo.Insert(entity).ConfigureAwait(false);
			this.orderRepo.SaveChanges();
			return order.Id;
		}

		public async Task Update(Orders entity)
		{
			await this.orderRepo.Update(entity).ConfigureAwait(false);
			this.orderRepo.SaveChanges();
		}
	}
}