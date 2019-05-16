using DataAccessLayer.Repository;
using Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Processors
{
	public class CartProcessor : BaseProcessor
	{
		private readonly CartRepository cartRepo;

		public CartProcessor()
		{
			cartRepo = new CartRepository();
		}
		private bool CartExists(int id)
		{
			if (cartRepo.Get(id).Result != null)
				return true;
			else
				return false;
		}

		public async Task<IEnumerable<Cart>> GetCart(int userId)
		{
			var cartList = await cartRepo.GetAll();
			return cartList.Where(c => c.userId == userId);
		}

		public async Task<Cart> GetCartItem(int userId, int itemId)
		{
			var cartList = await cartRepo.GetAll();
			return cartList.FirstOrDefault(c => c.userId == userId && c.itemId == itemId);
		}

		public async Task EditCart(int id, Cart cart)
		{
			await cartRepo.Update(cart);
		}

		public async Task<bool> InsertCart(Cart cart)
		{
			await cartRepo.Insert(cart);
			if (!CartExists(cart.Id))
			{
				return false;
			}
			return true;
		}

		public async Task<bool> DeleteCart(int id)
		{
			await cartRepo.Delete(GetCart(id).Result.FirstOrDefault(c=> c.Id == id));
			if (!CartExists(id))
			{
				return true;
			}
			return false;
		}

	}
}
