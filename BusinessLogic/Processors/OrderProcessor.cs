using DataAccessLayer.Repository;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BusinessLogic.Processors
{
	public class OrderProcessor : BaseProcessor
	{
		private readonly OrderRepository orderRepo;

		public OrderProcessor()
		{
			orderRepo = new OrderRepository();
			userProcessor = new UserProcessor();
			itemProcessor = new ItemProcessor();
			cartProcessor = new CartProcessor();
		}

		private bool OrderExists(int id)
		{
			return orderRepo.GetAll().Result.Any(o => o.Id == id);
		}

		public async Task<IEnumerable<Orders>> GetOrders(int userId)
		{
			var orderList = await orderRepo.GetAll();
			if (userId != -1)
			{
				return orderList.Where(o => o.userId == userId);
			}
			else
				return orderList;
		}

		public async Task<IEnumerable<OrderItem>> GetOrderItems(int id)
		{
			var orders = orderRepo.GetAll().Result.Where(o => o.orderId == id);
			List<OrderItem> orderItems = new List<OrderItem>();

			foreach (var o in orders)
			{
				orderItems.Add(new OrderItem(o, await itemProcessor.GetItem(o.itemId)));
			}
			return orderItems;
		}

		public async Task<bool> AddOrder(int userId)
		{
			IEnumerable<Cart> cartItems = await cartProcessor.GetCart(userId);

			List<int> itemIds = cartItems.Select(item => item.itemId).ToList();

			List<Item> items = new List<Item>();
			decimal cartTotal = 0;

			int orderId = await userProcessor.GetOrderCount(userId);

			foreach (var id in itemIds)
			{
				Item item = await itemProcessor.GetItem(id);
				items.Add(item);
				cartTotal += item.Price;

				var res = await orderRepo.Insert(new Orders { orderId = orderId, userId = userId, itemId = item.Id, price = item.Price, quantity = cartItems.First(citem => citem.itemId == item.Id).quantity, dop = DateTime.UtcNow });
				if (res != orderId)
					return false;
			}

			//Delete items from user cart
			foreach (var citem in cartItems)
			{
				var res = await cartProcessor.DeleteCart(citem.Id);
				if (!res)
					return false;
			}

			//Generate orderItems list for email notification
			List<OrderItem> orderItems = GetOrderItems(orderId).Result.ToList();

			//TODO: EMAIL
			//OrderRepository.EmailNotifier("jerinjohn101@outlook.com", orderItems).ToString());
			//static bool mailSent = false;
			//private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
			//{
			//	// Get the unique identifier for this asynchronous operation.
			//	String token = (string)e.UserState;

			//	if (e.Cancelled)
			//	{
			//		Console.WriteLine("[{0}] Send canceled.", token);
			//	}
			//	if (e.Error != null)
			//	{
			//		Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
			//	}
			//	else
			//	{
			//		Console.WriteLine("Message sent.");
			//	}
			//	mailSent = true;
			//}

			//public static bool EmailNotifier(string usermail, IEnumerable<OrderItem> orders)
			//{
			//	// Command-line argument must be the SMTP host.
			//	SmtpClient client = new SmtpClient();
			//	{
			//		client.Host = "smtp.outlook.com";
			//		client.Port = 587;
			//		client.EnableSsl = true;

			//		client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
			//		client.Credentials = new NetworkCredential("jerinjohn101@outlook.com", "");
			//		client.UseDefaultCredentials = false;
			//		client.Timeout = 600000;
			//	}

			//	MailAddress from = new MailAddress("jerinjohn101@outlook.com", "MVC Hotel " + (char)0xD8 + " Order", System.Text.Encoding.UTF8);
			//	MailAddress to = new MailAddress(usermail);

			//	MailMessage message = new MailMessage(from, to);
			//	message.Body = "Automated order notification from Hotel.com";

			//	string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
			//	message.Body += Environment.NewLine + someArrows;
			//	message.BodyEncoding = System.Text.Encoding.UTF8;

			//	message.Body += "<table class=\"table\"> < thead > < tr > < th > Order Id# </ th > < th > Title </ th > < th > Price </ th > < th > Quantity </ th > < th ></ th > </ tr > </ thead > < tbody >";
			//	foreach (var item in orders)
			//	{
			//		message.Body += " <tr > < td >" + item.order.orderId + "</ td > < td > " + item.item.Title + " </ td > < td >" + item.item.Price + "</ td > < td >" + item.order.quantity + " </ td ></tr>";
			//	}
			//	message.Body += "</tbody></table>";

			//	message.Subject = "Order Confirmation - Hotel.com" + someArrows;
			//	message.SubjectEncoding = System.Text.Encoding.UTF8;

			//	string userState = "test message1";
			//	client.SendAsync(message, userState);

			//	client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

			//	message.Dispose();

			//	return mailSent;
			//}
			return true;
		}
	}
}
