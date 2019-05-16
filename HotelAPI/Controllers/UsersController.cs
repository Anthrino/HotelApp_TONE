using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using HotelAPI_TONE.Helpers;
using Shared.Models;
using BusinessLogic.Processors;

namespace HotelAPI_TONE.Controllers
{
	[Authorize]
	[Route("api/users")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly UserProcessor userProcessor;

		public UsersController(IConfiguration configuration)
		{
			_configuration = configuration;
			userProcessor = new UserProcessor();
		}

		// GET: api/Users
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
		{
			return await userProcessor.GetUsers();
		}

		// GET: api/Users/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetUser([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var user = await userProcessor.GetUser(id);

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

			userProcessor.EditUser(id, user);
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

			await userProcessor.InsertUser(user);

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

			var user = userProcessor.GetUser(id);
			if (user == null)
			{
				return NotFound();
			}

			if(await userProcessor.DeleteUser(id))
				return Ok(user);
			else
				return BadRequest();
		}

		[AllowAnonymous]
		[Route("login")]
		[HttpPost]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> Authenticate([FromBody] Users user)
		{
			if (ModelState.IsValid)
			{
				user = userProcessor.Authenticate(user);
				return Ok(user);
			}
			return BadRequest(ModelState);
		}
	}
}