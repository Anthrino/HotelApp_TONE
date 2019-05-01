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
		[HttpGet("{id}")]
		public IEnumerable<Orders> GetOrders([FromRoute] int userId)
		{
			return new OrderRepository(_context).GetOrders(userId).Result;
		}

		// GET: api/Orders/5
		[Route("order")]
		[HttpGet("{id}")]
		public IActionResult GetOrder([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var order = new OrderRepository(_context).GetOrder(id).Result;

			if (order == null)
			{
				return NotFound();
			}

			return Ok(order);
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
			foreach (var id in itemIds)
			{
				Item item = await _context.Item.FirstAsync(citem => citem.Id == id);
				_logger.LogCritical(item.Id + item.Title);
				items.Add(item);
				cartTotal += item.Price;

				var res = await new OrderRepository(_context).AddOrder(new Orders { userId = userId, itemId = item.Id, price = item.Price, quantity = cartItems.First(citem => citem.itemId == item.Id).quantity, dop = DateTime.UtcNow });
				if (!res)
					return NotFound();
			}
			foreach (var citem in cartItems)
			{
				var res = await new CartRepository(_context).DeleteCart(citem.Id);
				if (!res)
					return NotFound();
			}
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