using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelAPI_TONE.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelAPI_TONE.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly HotelAPI_TONEContext _context;

		public UsersController(HotelAPI_TONEContext context)
		{
			_context = context;
		}

		// GET: api/Users
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Users>>> GetUser()
		{
			return await _context.Users.ToListAsync();
		}

		// GET: api/Users/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetUser([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var Users = await _context.Users.FindAsync(id);

			if (Users == null)
			{
				return NotFound();
			}

			return Ok(Users);
		}

		// PUT: api/Users/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] Users Users)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != Users.Id)
			{
				return BadRequest();
			}

			_context.Entry(Users).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserExists(id))
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

		// POST: api/Users
		[HttpPost]
		public async Task<IActionResult> PostUser([FromBody] Users Users)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Users.Add(Users);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetUser", new { id = Users.Id }, Users);
		}

		// DELETE: api/Users/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var Users = await _context.Users.FindAsync(id);
			if (Users == null)
			{
				return NotFound();
			}

			_context.Users.Remove(Users);
			await _context.SaveChangesAsync();

			return Ok(Users);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(Users user)
		{
			if (ModelState.IsValid)
			{

				if (_context.Users.Any(u => u.username == user.username && u.password == user.password))
				{
					user = _context.Users.First(u => u.username == user.username && u.password == user.password);
					//SessionWrapper.UserId = Users.Id;
					//HttpContext.Session.SetInt32("USER_ID", user.USER_ID);
					//TempData.Keep("USER_ID");
					//TempData["USER_ID"] = user.USER_ID;
				}
			}
			return RedirectToAction("Index", "Carts", new { id = user.Id });

		}

		//public async Task<IActionResult> Logout()
		//{
		//	//SessionWrapper.UserId = -1;
		//	return RedirectToAction(nameof(Index));

		//}
	
		private bool UserExists(int id)
		{
			return _context.Users.Any(e => e.Id == id);
		}
	}
}