using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelMVC_TONE.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HotelMVC_TONE.Controllers
{
	public class CartsController : Controller
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (SessionWrapper.userId == -1)
			{
				filterContext.Result = RedirectToAction("Index", "Users");
			}
		}

		public IActionResult SetOrderuser(IFormCollection form)
		{
			SessionWrapper.orderUserId = int.Parse(form["orderUserId"]);
			return RedirectToAction(nameof(Index));
		}

		// GET: Carts
		public async Task<ViewResult> Index()
		{
			IEnumerable<Cart> cart = null;
			IEnumerable<Tuple<Item, Cart>> cartItems = new List<Tuple<Item, Cart>>();

			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				using (var client = new HttpClient(httpClientHandler))
				{
					//HTTP GET item list
					//client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);
					client.DefaultRequestHeaders.Add("Authorization", "Bearer " + SessionWrapper.token);

					var responseTask = client.GetAsync("http://localhost:5000/api/carts/" + SessionWrapper.orderUserId);
					responseTask.Wait();

					var result = responseTask.Result;
					if (result.IsSuccessStatusCode)
					{
						var readTask = result.Content.ReadAsAsync<IList<Cart>>();
						readTask.Wait();

						cart = readTask.Result;
					}
					else
					{
						cart = Enumerable.Empty<Cart>();

						ModelState.AddModelError(string.Empty, "Server error.");
					}

					foreach (Cart c in cart)
					{
						var response = await client.GetAsync("http://localhost:5000/api/items/" + c.itemId);
						if (response.IsSuccessStatusCode)
						{
							var readTask = response.Content.ReadAsAsync<Item>();
							readTask.Wait();

							cartItems = cartItems.Append(new Tuple<Item, Cart>(readTask.Result, c));
						}
						else
						{
							ModelState.AddModelError(string.Empty, "Server error.");
						}
					}

					ViewBag.role = SessionWrapper.role;

					IEnumerable<Users> users = null;


					responseTask = client.GetAsync("http://localhost:5000/api/users/");
					responseTask.Wait();

					result = responseTask.Result;
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

					ViewBag.orderUser = users.FirstOrDefault(u => u.Id == SessionWrapper.orderUserId).username;

					return View(new Tuple<IEnumerable<Tuple<Item, Cart>>, IEnumerable<SelectListItem>>(cartItems, users.Where(u => u.role != 1 && u.Id != SessionWrapper.orderUserId).Select(u => new SelectListItem { Text = u.username, Value = u.Id.ToString() })));
				}
			}

		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(IFormCollection form)
		{

			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				using (var client = new HttpClient(httpClientHandler))
				{
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

					HttpResponseMessage response;
					int itemId = int.Parse(form["Id"]);
					Cart cart = GetCartItem(itemId).Result;
					if (cart == null)
					{
						response = await client.PostAsJsonAsync<Cart>("http://localhost:5000/api/carts/", new Cart { userId = SessionWrapper.orderUserId, itemId = itemId, quantity = 1 });
					}
					else
					{
						response = await client.PutAsJsonAsync<Cart>("http://localhost:5000/api/carts/" + cart.Id, new Cart { Id = cart.Id, userId = SessionWrapper.orderUserId, itemId = itemId, quantity = cart.quantity + 1 });
					}
					if (response.IsSuccessStatusCode)
					{
						var retVal = await response.Content.ReadAsAsync<Item>();
						return RedirectToAction("Index");
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Server error.");
					}
				}
				return RedirectToAction(nameof(Index));
			}
		}
		// GET: Carts/Edit/5
		[HttpPost]
		public async Task<IActionResult> Edit(IFormCollection form)
		{
			if (ModelState.IsValid)
			{
				using (var httpClientHandler = new HttpClientHandler())
				{
					httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

					using (var client = new HttpClient(httpClientHandler))
					{
						client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
						client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

						Cart cart = GetCartItem(int.Parse(form["item.Item1.Id"])).Result;
						cart.quantity = int.Parse(form["item.Item2.quantity"]);
						var response = await client.PutAsJsonAsync<Cart>("http://localhost:5000/api/carts/" + cart.Id, cart);

					}
				}
				return RedirectToAction(nameof(Index));

			}
			else
			{
				ModelState.AddModelError(string.Empty, "Server error.");
				return View();

			}
		}

		public async Task<IActionResult> Order()
		{
			if (ModelState.IsValid)
			{
				using (var httpClientHandler = new HttpClientHandler())
				{
					httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

					using (var client = new HttpClient(httpClientHandler))
					{
						client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
						client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

						var response = await client.PostAsync("http://localhost:5000/api/orders/" + SessionWrapper.orderUserId, null);

					}
				}
				ViewBag.message = "Order successful";
				return RedirectToAction(nameof(Index));
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Server error.");
				return View();
			}
		}

		//public async Task<IActionResult> Clear()
		//{
		//	//TODO: Clear Cart
		//}

		
		// GET: Carts/Delete/5
		public IActionResult Delete(int id)
		{
			Cart cart = GetCartItem(id).Result;
			return View(cart);
		}

		// POST: Carts/Delete/id
		[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int Id)
		{
			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				using (var client = new HttpClient(httpClientHandler))
				{

					//HTTP GET item list
					Cart cart = null;
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

					var response = await client.DeleteAsync("http://localhost:5000/api/carts/" + Id);
					if (response.IsSuccessStatusCode)
					{
						var readTask = response.Content.ReadAsAsync<Cart>();
						readTask.Wait();

						cart = readTask.Result;
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Server error.");
					}
					return RedirectToAction(nameof(Index));
				}
			}
		}

		public async Task<Cart> GetCartItem(int itemId)
		{
			using (var httpClientHandler = new HttpClientHandler())
			{
				httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

				HttpResponseMessage response;

				using (var client = new HttpClient(httpClientHandler))
				{

					//HTTP GET item list
					Cart cart = null;
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

					response = await client.PostAsJsonAsync<Cart>("http://localhost:5000/api/carts/item", new Cart { userId = SessionWrapper.orderUserId, itemId = itemId, quantity = 0 });
					if (response.IsSuccessStatusCode)
					{
						var readTask = response.Content.ReadAsAsync<Cart>();
						readTask.Wait();

						cart = readTask.Result;
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Server error.");
					}
					return cart;
				}
			}
		}


		//private async Task<bool> CartExists(Cart cart)
		//{
		//	using (var httpClientHandler = new HttpClientHandler())
		//	{
		//		httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

		//		using (var client = new HttpClient(httpClientHandler))
		//		{
		//			client.BaseAddress = new Uri("http://localhost:5000/api/carts/");
		//			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", SessionWrapper.token);

		//			var response = await client.PostAsJsonAsync<Cart>("exists", cart);

		//			if (response.IsSuccessStatusCode)
		//			{
		//				return await response.Content.ReadAsAsync<bool>();
		//			}
		//			else
		//			{
		//				return false;
		//			}
		//		}
		//	}
		//}
	}
}
