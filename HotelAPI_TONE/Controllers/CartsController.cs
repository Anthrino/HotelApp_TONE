using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelAPI_TONE.Models;

namespace HotelAPI_TONE.Controllers
{
	[Route("api/carts")]
	[ApiController]
	public class CartsController : ControllerBase
	{
		private readonly HotelAPI_TONEContext _context;

		public CartsController(HotelAPI_TONEContext context)
		{
			_context = context;
		}

		// GET: api/Carts
		[HttpGet("{id}")]
		public async Task<ActionResult<IEnumerable<Cart>>> GetCart([FromRoute] int? id)
		{
			return await _context.Cart.Where(citem => citem.userId == id).ToListAsync();
		}

		// GET: api/Carts/id
		[Route("item")]
		[HttpPost]
		public async Task<IActionResult> GetCartItem([FromBody] Cart cart)
		{
			Console.WriteLine("CART:" + cart.Id);
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			try
			{
				var cartItem = await _context.Cart.FirstAsync(citem => citem.userId == cart.userId && citem.itemId == cart.itemId);

				if (cartItem == null)
				{
					return NotFound();
				}

				return Ok(cartItem);

			}
			catch (Exception)
			{
				throw;
			}
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

			_context.Entry(cart).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CartExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

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

			_context.Cart.Add(cart);
			await _context.SaveChangesAsync();

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

			var cart = await _context.Cart.FindAsync(id);
			if (cart == null)
			{
				return NotFound();
			}

			_context.Cart.Remove(cart);
			await _context.SaveChangesAsync();

			return Ok(cart);
		}

		[Route("/exists")]
		[HttpGet("{id}")]
		private bool CartExists([FromRoute] int id)
		{
			return _context.Cart.Any(e => e.Id == id);
		}
	}
}