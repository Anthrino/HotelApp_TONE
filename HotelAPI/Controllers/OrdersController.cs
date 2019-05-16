using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelAPI_TONE.Data;
using HotelAPI_TONE.Repository;
using Microsoft.Extensions.Logging;
using Shared.Models;

namespace HotelAPI_TONE.Controllers
{
	[Authorize]
	[Route("api/orders")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly OrderProcessor orderProcessor;

		public OrdersController(ILogger<OrdersController> logger)
		{
			_logger = logger;
			orderProcessor = new OrderProcessor();
		}

		// GET: api/Orders
		[HttpGet("list/{userid}")]
		public async Task<IEnumerable<Orders>> GetOrders([FromRoute] int userId)
		{
			return await orderProcessor.GetOrders(userId);
		}

		// GET: api/Orders
		[HttpGet("list")]
		public async Task<IEnumerable<Orders>> GetAllOrders()
		{
			return await orderProcessor.GetOrders(-1);
		}

		// GET: api/Orders/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetOrderItems([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var orderItems = await orderProcessor.GetOrderItems(id);

			if (orderItems == null)
			{
				return NotFound();
			}

			_logger.LogCritical(orderItems.Count().ToString());
			return Ok(orderItems);
		}

		// POST: api/Orders
		[HttpPost("{userId}")]
		public async Task<IActionResult> PostOrder([FromRoute] int userId)
		{
			if (orderProcessor.AddOrder(userId))
				return Ok();
			else
				return NotFound();
		}
	}
}