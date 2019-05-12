using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelMVC_TONE.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http.Headers;

namespace HotelMVC_TONE.Controllers
{
	public class OrdersController : Controller
	{

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (SessionWrapper.userId == -1)
			{
				RedirectToAction("Index");
			}
		}

		// GET: Orders
		public async Task<IActionResult> Index()
		{
			IEnumerable<Orders> orderItems = null;

			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				using (var client = new HttpClient(httpClientHandler))
				{
					//HTTP GET item list
					//client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);
					client.DefaultRequestHeaders.Add("Authorization", "Bearer " + SessionWrapper.token);

					string path;
					if (SessionWrapper.role != 1)
						path = "http://localhost:5000/api/orders/list/" + SessionWrapper.userId;
					else
						path = "http://localhost:5000/api/orders/list";

					var responseTask = client.GetAsync(path);
					responseTask.Wait();

					var result = responseTask.Result;
					if (result.IsSuccessStatusCode)
					{
						var readTask = result.Content.ReadAsAsync<IList<Orders>>();
						readTask.Wait();

						orderItems = readTask.Result;
					}
					else
					{
						orderItems = Enumerable.Empty<Orders>();

						ModelState.AddModelError(string.Empty, "Server error.");
					}

					Dictionary<int, decimal> cartTotals = new Dictionary<int, decimal>();
					List<Tuple<int, string>> orders = new List<Tuple<int, string>>();

					foreach (Orders o in orderItems)
					{
						if (cartTotals.ContainsKey(o.orderId))
						{
							cartTotals[o.orderId] += o.price * o.quantity;
						}
						else
						{
							cartTotals[o.orderId] = o.price * o.quantity;
						}
						orders.Add(new Tuple<int, string>(o.orderId, o.dop.ToLongDateString()));
					}
					return View(new Tuple<List<Tuple<int, string>>, Dictionary<int, decimal>>(orders.Distinct().ToList(), cartTotals));
				}
			}
		}

		// GET: Orders/Details/5

		public async Task<IActionResult> Details(int? id)
		{
			IEnumerable<OrderItem> orderItems = null;

			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				using (var client = new HttpClient(httpClientHandler))
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

					var responseTask = client.GetAsync("http://localhost:5000/api/orders/" + id);
					responseTask.Wait();

					var result = responseTask.Result;
					if (result.IsSuccessStatusCode)
					{
						var readTask = result.Content.ReadAsAsync<IEnumerable<OrderItem>>();
						readTask.Wait();

						orderItems = readTask.Result;
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Server error.");
					}
				}
			}
			return View(orderItems);
		}

		// POST: Orders/Edit/5

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,orderId,userId,itemId,price,quantity")] Orders order)
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

						var response = await client.PutAsJsonAsync<Orders>("orders/" + id, order);
						if (response.IsSuccessStatusCode)
						{
							var retVal = await response.Content.ReadAsAsync<Orders>();
							ViewBag.Message = "Profile updated";
							return RedirectToAction("Details", new { id = retVal.Id });
						}
						else
						{
							ModelState.AddModelError(string.Empty, "Server error.");
							return View(order);
						}
					}
				}
			}
			catch
			{
				return View();
			}
		}

		// GET: Orders/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			Orders order = null;

			if (SessionWrapper.role != 1)
			{
				ModelState.AddModelError(string.Empty, "Access Denied. Only available to admin");
				return View(order);
			}

			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				using (var client = new HttpClient(httpClientHandler))
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

					var responseTask = client.GetAsync("http://localhost:5000/api/orders/" + id);
					responseTask.Wait();

					var result = responseTask.Result;
					if (result.IsSuccessStatusCode)
					{
						var readTask = result.Content.ReadAsAsync<Orders>();
						readTask.Wait();

						order = readTask.Result;
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Server error.");
					}
				}
			}
			return View(order);
		}

		// POST: Orders/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(int id, [Bind("Id,orderId,userId,itemId,price,quantity")] Orders order)
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

						var response = await client.DeleteAsync("http://localhost:5000/api/orders/" + id);
						if (response.IsSuccessStatusCode)
						{
							var retVal = await response.Content.ReadAsAsync<Orders>();
							return RedirectToAction("Index");
						}
						else
						{
							ModelState.AddModelError(string.Empty, "Server error.");
						}
					}
					return View(order);
				}
			}
			catch
			{
				return View();
			}
		}

		//private bool OrderExists(int id)
		//{
		//	return _context.Order.Any(e => e.Id == id);
		//}
	}
}
