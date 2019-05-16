using DataAccessLayer.Repository;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BusinessLogic.Processors
{
	public class OrderProcessor
	{
		private readonly OrderRepository orderRepo;

		public OrderProcessor()
		{
			orderRepo = new OrderRepository();
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

		public async Task<IEnumerable<Orders>> GetOrderItems(int id)
		{
			var orderItems = orderRepo.GetAll().Result.Where(o => o.orderId == id);
			return orderItems;
		}

		public async Task<bool> AddOrder(Orders order)
		{
			await orderRepo.Insert(order);
			if (!OrderExists(order.Id))
			{
				return false;
			}
			return true;
		}
	}
}
