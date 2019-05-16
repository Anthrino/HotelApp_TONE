using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
	public interface IRepository<T>
		where T : class
	{
		Task<ICollection<T>> GetAll();

		Task<T> Get(params object[] id);

		Task<T> Insert(T entity);

		Task Update(T entity);

		Task Delete(T entity);

		void SaveChanges();
	}
}
