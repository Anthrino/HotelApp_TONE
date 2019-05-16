using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.ComponentModel;
using HotelAPI_TONE.Repository;
using Microsoft.Extensions.Logging;
using Shared.Models;
using HotelAPI_TONE.Data;

namespace HotelAPI_TONE.Controllers
{
	//[Authorize]
	[Route("api/carts")]
	[ApiController]
	public class CartsController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly CartProcessor cartProcessor;


		public CartsController(ILogger<CartsController> logger)
		{
			_logger = logger;
			cartProcessor = new CartProcessor();
		}

		// GET: api/Carts
		[HttpGet("{id}")]
		public async Task<ActionResult<IEnumerable<Cart>>> GetCart([FromRoute] int? id)
		{
			return Ok(await cartProcessor.GetCart(id));
		}

		// GET: api/Carts/id
		[Route("item")]
		[HttpPost]
		public async Task<IActionResult> GetCartItem([FromBody] Cart cart)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var cartItem = cartProcessor.GetCartItem(cart.userId, cart.ItemId);
			if (cartItem == null)
			{
				return NotFound();
			}

			return Ok(cartItem);
		}

		// PUT: api/Carts/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutCart([FromRoute] int id, [FromBody] Cart cart)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != cart.Id)
			{
				return BadRequest();
			}

			await cartProcessor.EditCart(id, cart);
			return NoContent();
		}

		// POST: api/Carts
		[HttpPost]
		public async Task<IActionResult> PostCart([FromBody] Cart cart)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			await cartProcessor.InsertCart(cart);
			return CreatedAtAction("GetCart", new { id = cart.Id }, cart);
		}

		// DELETE: api/Carts/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCart([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var cart = await cartProcessor.GetCartItem(id);
			if (cart == null)
			{
				return NotFound();
			}

			if (await cartProcessor.DeleteCart(id))
				return Ok(cart);
			else
				return BadRequest();
		}
	}

}