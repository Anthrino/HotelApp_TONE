using DataAccessLayer.Repository;
using Shared.Helpers;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Processors
{
	public class OrderProcessor : BaseProcessor
	{
		private readonly OrderRepository orderRepo;

		public OrderProcessor()
		{
			orderRepo = new OrderRepository();
			userProcessor = new UserProcessor();
			itemProcessor = new ItemProcessor();
			cartProcessor = new CartProcessor();
		}

		private bool OrderExists(int id)
		{
			return orderRepo.GetAll().Result.Any(o => o.Id == id);
		}

		public async Task<IEnumerable<Orders>> GetOrders(int userId)
		{
			var orderList = await orderRepo.GetAll();
			if (userId != -1)
			{
				return orderList.Where(o => o.userId == userId);
			}
			else
				return orderList;
		}

		public async Task<IEnumerable<OrderItem>> GetOrderItems(int id)
		{
			var orders = orderRepo.GetAll().Result.Where(o => o.orderId == id);
			List<OrderItem> orderItems = new List<OrderItem>();

			foreach (var o in orders)
			{
				orderItems.Add(new OrderItem(o, await itemProcessor.GetItem(o.itemId)));
			}
			return orderItems;
		}

		public async Task<bool> AddOrder(int userId, string apiKey)
		{
			IEnumerable<Cart> cartItems = await cartProcessor.GetCart(userId);

			List<int> itemIds = cartItems.Select(item => item.itemId).ToList();

			List<Item> items = new List<Item>();
			decimal cartTotal = 0;

			int orderId = await userProcessor.GetOrderCount(userId);

			foreach (var id in itemIds)
			{
				Item item = await itemProcessor.GetItem(id);
				items.Add(item);
				cartTotal += item.Price;

				var res = await orderRepo.Insert(new Orders { orderId = orderId, userId = userId, itemId = item.Id, price = item.Price, quantity = cartItems.First(citem => citem.itemId == item.Id).quantity, dop = DateTime.UtcNow });
				if (res != orderId)
					return false;
			}

			//Delete items from user cart
			foreach (var citem in cartItems)
			{
				var res = await cartProcessor.DeleteCart(citem.Id);
				if (!res)
					return false;
			}

			//Generate orderItems list for email notification
			List<OrderItem> orderItems = GetOrderItems(orderId).Result.ToList();

			Users user = await userProcessor.GetUser(userId);

			//Call Sendgrid Email Notification Service
			return await new EmailService().EmailNotifier(apiKey, user, orderItems);
		}
	}
}
