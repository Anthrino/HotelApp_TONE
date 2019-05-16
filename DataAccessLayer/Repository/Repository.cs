using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
	public class Repository<T> : IRepository<T>
	   where T : class
	{
		protected readonly Hotel_TONEDbContext context;

		protected readonly DbSet<T> Entities;

		public Repository()
		{
			this.context = new Hotel_TONEDbContext("<DB_Conn_String>");
			this.Entities = this.context.Set<T>();
		}

		public async Task<ICollection<T>> GetAll()
		{
			var results = await this.Entities.ToListAsync().ConfigureAwait(false);
			return results;
		}

		public async Task<T> Get(params object[] id)
		{
			var results = await this.Entities.FindAsync(id).ConfigureAwait(false);
			return results;
		}

		public async Task<T> Insert(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException();
			}

			var item = await this.Entities.AddAsync(entity).ConfigureAwait(false);
			return item.Entity;
		}

		public async Task Update(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException();
			}

			this.Entities.Update(entity);
		}

		public async Task Delete(T entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException();
			}

			this.Entities.Remove(entity);
		}

		public void SaveChanges()
		{
			this.context.SaveChanges();
		}
	}
}
