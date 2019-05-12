using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelAPI_TONE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using HotelAPI_TONE.Helpers;

namespace HotelAPI_TONE.Controllers
{
	//[Authorize]
	[Route("api/users")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly HotelAPI_TONEContext _context;
		private readonly IConfiguration _configuration;

		public UsersController(HotelAPI_TONEContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
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

			var user = await _context.Users.FindAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			return Ok(user);
		}

		// PUT: api/Users/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] Users user)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != user.Id)
			{
				return BadRequest();
			}

			_context.Entry(user).State = EntityState.Modified;

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
		public async Task<IActionResult> PostUser([FromBody] Users user)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetUser", new { id = user.Id }, user);
		}

		// DELETE: api/Users/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			return Ok(user);
		}

		[AllowAnonymous]
		[Route("login")]
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> Authenticate([FromBody] Users user)
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

					var appSettings =_configuration.GetSection("AppSettings").Get<AppSettings>();

					user.token = new TokenController(appSettings).GenerateToken(user.username);
					return Ok(user);
				}
			}
			return RedirectToAction("Index", "Carts", new { id = user.Id });



		}

		private bool UserExists(int id)
		{
			return _context.Users.Any(e => e.Id == id);
		}
	}
}