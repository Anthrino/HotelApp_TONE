using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelMVC_TONE.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HotelMVC_TONE.Controllers
{
	public class ItemsController : Controller
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (SessionWrapper.userId == -1)
			{
				filterContext.Result = RedirectToAction("Index", "Users");
			}
		}

		// GET: Items
		public ViewResult Index()
		{
			IEnumerable<Item> items = null;
			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				using (var client = new HttpClient(httpClientHandler))
				{
					client.BaseAddress = new Uri("http://localhost:5000/api/");
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

					//HTTP GET item list
					var responseTask = client.GetAsync("items");
					responseTask.Wait();

					var result = responseTask.Result;
					if (result.IsSuccessStatusCode)
					{
						var readTask = result.Content.ReadAsAsync<IList<Item>>();
						readTask.Wait();

						items = readTask.Result;
					}
					else
					{
						items = Enumerable.Empty<Item>();

						ModelState.AddModelError(string.Empty, "Server error.");
					}
				}
			}
			return View(items);
		}

		// GET: Items/Details/id
		public ActionResult Details(int? id)
		{
			Item item = null;
			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				using (var client = new HttpClient(httpClientHandler))
				{

					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

					//HTTP GET item list
					var responseTask = client.GetAsync("http://localhost:5000/api/items/" + id);
					responseTask.Wait();

					var result = responseTask.Result;
					if (result.IsSuccessStatusCode)
					{
						var readTask = result.Content.ReadAsAsync<Item>();
						readTask.Wait();

						item = readTask.Result;
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Server error.");
					}
				}

				return View(item);
			}
		}

		// GET: Items/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Items/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Title,Category,Description,Price")] Item item)
		{
			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				using (var client = new HttpClient(httpClientHandler))
				{
					client.BaseAddress = new Uri("http://localhost:5000/api/");
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

					var response = await client.PostAsJsonAsync<Item>("items", item);

					if (response.IsSuccessStatusCode)
					{
						var retVal = await response.Content.ReadAsAsync<Item>();
						return RedirectToAction("Details", new { id = retVal.Id });
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Server error.");
					}
				}
				return RedirectToAction(nameof(Index));
			}
		}

		// GET: Items/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{

			Item item = null;

			if (SessionWrapper.role != 1)
			{
				ModelState.AddModelError(string.Empty, "Access Denied. Only available to admin");
				return View(item);
			}

			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				using (var client = new HttpClient(httpClientHandler))
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

					var responseTask = client.GetAsync("http://localhost:5000/api/items/" + id);
					responseTask.Wait();

					var result = responseTask.Result;
					if (result.IsSuccessStatusCode)
					{
						var readTask = result.Content.ReadAsAsync<Item>();
						readTask.Wait();

						item = readTask.Result;
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Server error.");
					}
				}
			}
			return View(item);
		}

		// POST: Items/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Category,Description,Price")] Item item)
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

					var response = await client.PutAsJsonAsync<Item>("http://localhost:5000/api/items/" + id, item);
					if (response.IsSuccessStatusCode)
					{
						var retVal = await response.Content.ReadAsAsync<Item>();
						return RedirectToAction("Details", new { id = retVal.Id });
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Server error.");
					}
				}
				return View(item);
			}
		}

		// GET: Items/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			Item item = null;
			if (SessionWrapper.role != 1)
			{
				ModelState.AddModelError(string.Empty, "Access Denied. Only available to admin");
				return View(item);
			}

			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				using (var client = new HttpClient(httpClientHandler))
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

					var responseTask = client.GetAsync("http://localhost:5000/api/items/" + id);
					responseTask.Wait();

					var result = responseTask.Result;
					if (result.IsSuccessStatusCode)
					{
						var readTask = result.Content.ReadAsAsync<Item>();
						readTask.Wait();

						item = readTask.Result;
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Server error.");
					}
				}
			}
			return View(item);
		}

		// POST: Items/Delete/5
		[AllowAnonymous]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
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

					var response = await client.DeleteAsync("items/" + id);

					if (response.IsSuccessStatusCode)
					{
						var retVal = await response.Content.ReadAsAsync<Item>();
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Server error.");
					}
				}
			};
			return RedirectToAction(nameof(Index));
		}
	}
}
