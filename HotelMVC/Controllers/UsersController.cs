using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HotelMVC_TONE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HotelMVC_TONE.Controllers
{
	public class UsersController : Controller
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (SessionWrapper.userId == -1)
			{
				RedirectToAction("Index");
			}
		}
		// GET: Users
		public IActionResult Index()
		{
			if (SessionWrapper.role != 1)
			{
				ModelState.AddModelError(string.Empty, "Access Denied. Only available to admin");
				return RedirectToAction("Index", "Carts");
			}

			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				IEnumerable<Users> users = null;

				using (var client = new HttpClient(httpClientHandler))
				{
					//HTTP GET item list
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);
					var responseTask = client.GetAsync("http://localhost:5000/api/users/");
					responseTask.Wait();

					var result = responseTask.Result;
					if (result.IsSuccessStatusCode)
					{
						var readTask = result.Content.ReadAsAsync<IList<Users>>();
						readTask.Wait();

						users = readTask.Result;
					}
					else
					{
						users = Enumerable.Empty<Users>();
						ModelState.AddModelError(string.Empty, "Server error.");
					}
					return View(users);
				}
			}
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login([Bind("username, password")] Users user)
		{
			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				using (var client = new HttpClient(httpClientHandler))
				{
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

					var response = await client.PostAsJsonAsync<Users>("http://localhost:5000/api/users/login", user);
					if (response.IsSuccessStatusCode)
					{
						var retEnt = await response.Content.ReadAsAsync<Users>();
						//HttpContext.Session.SetInt32("userid", user.Id);
						SessionWrapper.userId = retEnt.Id;
						SessionWrapper.orderUserId = retEnt.Id;
						SessionWrapper.role = retEnt.role;
						SessionWrapper.token = retEnt.token;

						return RedirectToAction("Index", "Carts", new { id = retEnt.Id });
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Login Credentials Error");
						return View(user);
					}
				}
			}

		}

		public async Task<IActionResult> Logout()
		{
			//HttpContext.Session.SetInt32("userid", -1);
			SessionWrapper.userId = -1;
			SessionWrapper.orderUserId = -1;
			SessionWrapper.role = -1;
			SessionWrapper.token = "";
			return RedirectToAction(nameof(Login));
		}

		//[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		//public IActionResult Error()
		//{
		//	return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		//}

		// GET: Users/Details/5
		public ActionResult Details(Users user)
		{
			return View(user);
		}

		// GET: Users/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Users/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Create([Bind("username,password,roleType")] Users user)
		{
			try
			{
				using (var httpClientHandler = new HttpClientHandler())
				{
					httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

					using (var client = new HttpClient(httpClientHandler))
					{
						client.BaseAddress = new Uri("http://localhost:5000/api/");
						client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
						client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

						var response = await client.PostAsJsonAsync<Users>("users", user);

						if (response.IsSuccessStatusCode)
						{
							var retVal = await response.Content.ReadAsAsync<Users>();
							return RedirectToAction("Details", user);
						}
						else
						{
							ModelState.AddModelError(string.Empty, "Server error.");
						}
					}
					return RedirectToAction(nameof(Index));
				}
			}
			catch
			{
				return View();
			}
		}

		// GET: Users/Edit/5
		public ActionResult Edit(int id)
		{
			Users user = null;

			if (SessionWrapper.role != 1)
			{
				ModelState.AddModelError(string.Empty, "Access Denied. Only available to admin");
				return View(user);
			}

			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				using (var client = new HttpClient(httpClientHandler))
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

					var responseTask = client.GetAsync("http://localhost:5000/api/users/" + id);
					responseTask.Wait();

					var result = responseTask.Result;
					if (result.IsSuccessStatusCode)
					{
						var readTask = result.Content.ReadAsAsync<Users>();
						readTask.Wait();

						user = readTask.Result;
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Server error.");
					}
				}
			}
			return View(user);
		}

		// POST: Users/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Edit(int id, [Bind("Id,username,password,role")] Users user)
		{
			try
			{
				if (SessionWrapper.role != 1)
				{
					ModelState.AddModelError(string.Empty, "Access Denied. Only available to admin");
					return RedirectToAction(nameof(Index));
				}
				using (var httpClientHandler = new HttpClientHandler())
				{
					httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

					using (var client = new HttpClient(httpClientHandler))
					{
						client.BaseAddress = new Uri("http://localhost:5000/api/");
						client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
						client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

						var response = await client.PutAsJsonAsync<Users>("users/" + id, user);
						if (response.IsSuccessStatusCode)
						{
							var retVal = await response.Content.ReadAsAsync<Users>();
							ViewBag.Message = "Profile updated";
							SessionWrapper.role = retVal.role;
							return RedirectToAction("Details", new { id = retVal.Id });
						}
						else
						{
							ModelState.AddModelError(string.Empty, "Server error.");
							return View(user);
						}
					}
				}
			}
			catch
			{
				return View();
			}
		}

		// GET: Users/Delete/5
		public ActionResult Delete(int id)
		{
			Users user = null;

			if (SessionWrapper.role != 1)
			{
				ModelState.AddModelError(string.Empty, "Access Denied. Only available to admin");
				return View(user);
			}

			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				using (var client = new HttpClient(httpClientHandler))
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

					var responseTask = client.GetAsync("http://localhost:5000/api/users/" + id);
					responseTask.Wait();

					var result = responseTask.Result;
					if (result.IsSuccessStatusCode)
					{
						var readTask = result.Content.ReadAsAsync<Users>();
						readTask.Wait();

						user = readTask.Result;
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Server error.");
					}
				}
			}
			return View(user);
		}

		// POST: Users/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(int id, [Bind("username,password,role")] Users user)
		{
			try
			{
				if (SessionWrapper.role != 1)
				{
					ModelState.AddModelError(string.Empty, "Access Denied. Only available to admin");
					return RedirectToAction(nameof(Index));

				}

				using (var httpClientHandler = new HttpClientHandler())
				{
					httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

					using (var client = new HttpClient(httpClientHandler))
					{
						client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
						client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

						var response = await client.DeleteAsync("http://localhost:5000/api/users/" + id);
						if (response.IsSuccessStatusCode)
						{
							var retVal = await response.Content.ReadAsAsync<Users>();
							return RedirectToAction("Index");
						}
						else
						{
							ModelState.AddModelError(string.Empty, "Server error.");
						}
					}
					return View(user);
				}
			}
			catch
			{
				return View();
			}
		}
	}
}