using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelAPI_TONE.Models;
using HotelAPI_TONE.Repository;
using Microsoft.Extensions.Logging;

namespace HotelAPI_TONE.Controllers
{
	[Route("api/orders")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly HotelAPI_TONEContext _context;
		private readonly ILogger _logger;

		public OrdersController(HotelAPI_TONEContext context, ILogger<OrdersController> logger)
		{
			_context = context;
			_logger = logger;
		}

		// GET: api/Orders
		[HttpGet("list/{userid}")]
		public async Task<IEnumerable<Orders>> GetOrders([FromRoute] int userId)
		{
			return await new OrderRepository(_context).GetOrders(userId);
		}

		// GET: api/Orders
		[HttpGet("list")]
		public async Task<IEnumerable<Orders>> GetAllOrders()
		{
			return await new OrderRepository(_context).GetOrders(-1);
		}

		// GET: api/Orders/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetOrderItems([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			List<OrderItem> orderItems = new List<OrderItem>();

			var orders = new OrderRepository(_context).GetOrderItems(id).Result;

			if (orders == null)
			{
				return NotFound();
			}
			else
			{
				foreach (var o in orders)
				{
					orderItems.Add(new OrderItem(o, await new ItemRepository(_context).GetItem(o.itemId)));
				}
			}
			_logger.LogCritical(orderItems.Count().ToString());
			return Ok(orderItems);
		}

		// PUT: api/Orders/5
		[HttpPut("{id}")]
		public IActionResult PutOrder([FromRoute] int id, [FromBody] Orders order)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != order.Id)
			{
				return BadRequest();
			}

			if (new OrderRepository(_context).EditOrder(id, order).Result)
				return NoContent();
			else
				return NotFound();
		}

		// POST: api/Orders
		[HttpPost("{userId}")]
		public async Task<IActionResult> PostOrder([FromRoute] int userId)
		{

			_logger.LogCritical(userId.ToString());
			IEnumerable<Cart> cartItems = await new CartRepository(_context).GetCartItems(userId);

			foreach (var i in cartItems)
				_logger.LogCritical(i.Id.ToString());

			List<int> itemIds = cartItems.Select(item => item.itemId).ToList();

			List<Item> items = new List<Item>();
			decimal cartTotal = 0;
			int orderId = await new UserRepository(_context).GetOrderCount(userId);

			foreach (var id in itemIds)
			{
				Item item = await _context.Item.FirstAsync(citem => citem.Id == id);
				_logger.LogCritical(item.Id + item.Title);
				items.Add(item);
				cartTotal += item.Price;

				var res = await new OrderRepository(_context).AddOrder(new Orders { orderId = orderId, userId = userId, itemId = item.Id, price = item.Price, quantity = cartItems.First(citem => citem.itemId == item.Id).quantity, dop = DateTime.UtcNow });
				if (!res)
					return NotFound();
			}

			//Delete items from user cart
			foreach (var citem in cartItems)
			{
				var res = await new CartRepository(_context).DeleteCart(citem.Id);
				if (!res)
					return NotFound();
			}

			//Generate orderItems list for email notification
			List<OrderItem> orderItems = new List<OrderItem>();

			var orders = new OrderRepository(_context).GetOrderItems(orderId).Result;

			if (orders == null)
			{
				return NotFound();
			}
			else
			{
				foreach (var o in orders)
				{
					orderItems.Add(new OrderItem(o, await new ItemRepository(_context).GetItem(o.itemId)));
				}
			}

			_logger.LogCritical(OrderRepository.EmailNotifier("jerinjohn101@outlook.com", orderItems).ToString());

			return Ok();
		}

		// DELETE: api/Orders/5
		[HttpDelete("{id}")]
		public IActionResult DeleteOrder([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (new OrderRepository(_context).DeleteOrder(id).Result)
				return Ok();
			else
				return NotFound();
		}
	}
}