using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelAPI_TONE.Data;
using Microsoft.AspNetCore.Authorization;
using Shared.Models;

namespace HotelAPI_TONE.Controllers
{
	//[Authorize]
	[Route("api/items")]
	[ApiController]
	public class ItemsController : ControllerBase
	{
		private readonly ItemProcessor itemProcessor;

		public ItemsController()
		{
			itemProcessor = new ItemProcessor();
		}

		// GET: api/Items
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Item>>> GetItems()
		{
			return await itemProcessor.GetItems();
		}

		// GET: api/Items/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetItem([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var item = await itemProcessor.GetItem(id);

			if (item == null)
			{
				return NotFound();
			}
			return Ok(item);
		}

		// PUT: api/Items/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutItem([FromRoute] int id, [FromBody] Item item)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (id != item.Id)
			{
				return BadRequest();
			}

			await itemProcessor.EditItem(id, item);
			return Ok(await itemProcessor.GetItem(id));
		}

		// POST: api/Items
		[HttpPost]
		public async Task<IActionResult> PostItem([FromBody] Item item)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			await itemProcessor.InsertItem(item);
			return CreatedAtAction("GetItem", new { id = item.Id }, item);
		}

		// DELETE: api/Items/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteItem([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var item = await itemProcessor.GetItem(id);
			if (item == null)
			{
				return NotFound();
			}

			if (await itemProcessor.DeleteItem(id))
				return Ok(item);
			else
				return BadRequest();
		}
	}
}