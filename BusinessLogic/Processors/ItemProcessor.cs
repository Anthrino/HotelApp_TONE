using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Repository;
using Shared.Models;

namespace BusinessLogic.Processors
{
	public class ItemProcessor : BaseProcessor
	{
		private readonly ItemRepository itemRepo;

		public ItemProcessor()
		{
			itemRepo = new ItemRepository();
		}

		private bool ItemExists(int id)
		{
			if (itemRepo.Get(id).Result != null)
				return true;
			else
				return false;
		}

		public async Task<Item> GetItem(int id)
		{
			Item item = await itemRepo.Get(id);
			return item;
		}

		public async Task<IEnumerable<Item>> GetItems()
		{
			return await itemRepo.GetAll();
		}

		public async Task EditItem(int id, Item item)
		{
			await itemRepo.Update(item);
		}

		public async Task<bool> InsertItem(Item item)
		{
			await itemRepo.Insert(item);
			if (!ItemExists(item.Id))
			{
				return false;
			}
			return true;
		}

		public async Task<bool> DeleteItem(int id)
		{
			await itemRepo.Delete(GetItem(id).Result);
			if (!ItemExists(id))
			{
				return true;
			}
			return false;
		}
	}
}
