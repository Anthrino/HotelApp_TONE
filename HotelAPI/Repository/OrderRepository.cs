using HotelAPI_TONE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HotelAPI_TONE.Repository
{
	public class OrderRepository
	{
		private readonly HotelAPI_TONEContext _context;

		public OrderRepository(HotelAPI_TONEContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Orders>> GetOrders(int userId)
		{
			if (userId != -1)
				return await _context.Orders.Where(order => order.userId == userId).ToListAsync();
			else
				return await _context.Orders.ToListAsync();

		}

		public async Task<IEnumerable<Orders>> GetOrderItems(int id)
		{
			var orderItems = await _context.Orders.Where(o => o.orderId == id).ToListAsync();
			return orderItems;
		}

		public async Task<bool> EditOrder(int id, Orders order)
		{

			_context.Entry(order).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrderExists(id))
				{
					return false;
				}
				else
				{
					throw;
				}
			}

			return true;
		}

		public async Task<bool> AddOrder(Orders order)
		{
			_context.Orders.Add(order);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!OrderExists(order.Id))
				{
					return false;
				}
				else
				{
					throw;
				}
			}

			return true;
		}

		public async Task<bool> DeleteOrder(int id)
		{

			var order = await _context.Orders.FindAsync(id);
			if (order == null)
			{
				return false;
			}

			_context.Orders.Remove(order);
			await _context.SaveChangesAsync();

			return true;
		}

		private bool OrderExists(int id)
		{
			return _context.Orders.Any(e => e.Id == id);
		}

		static bool mailSent = false;
		private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
		{
			// Get the unique identifier for this asynchronous operation.
			String token = (string)e.UserState;

			if (e.Cancelled)
			{
				Console.WriteLine("[{0}] Send canceled.", token);
			}
			if (e.Error != null)
			{
				Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
			}
			else
			{
				Console.WriteLine("Message sent.");
			}
			mailSent = true;
		}

		public static bool EmailNotifier(string usermail, IEnumerable<OrderItem> orders)
		{
			// Command-line argument must be the SMTP host.
			SmtpClient client = new SmtpClient();
			{
				client.Host = "smtp.outlook.com";
				client.Port = 587;
				client.EnableSsl = true;

				client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
				client.Credentials = new NetworkCredential("jerinjohn101@outlook.com", "");
				client.UseDefaultCredentials = false;
				client.Timeout = 600000;
			}

			MailAddress from = new MailAddress("jerinjohn101@outlook.com", "MVC Hotel " + (char)0xD8 + " Order", System.Text.Encoding.UTF8);
			MailAddress to = new MailAddress(usermail);

			MailMessage message = new MailMessage(from, to);
			message.Body = "Automated order notification from Hotel.com";

			string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
			message.Body += Environment.NewLine + someArrows;
			message.BodyEncoding = System.Text.Encoding.UTF8;

			message.Body += "<table class=\"table\"> < thead > < tr > < th > Order Id# </ th > < th > Title </ th > < th > Price </ th > < th > Quantity </ th > < th ></ th > </ tr > </ thead > < tbody >";
			foreach (var item in orders)
			{
				message.Body += " <tr > < td >" + item.order.orderId + "</ td > < td > " + item.item.Title + " </ td > < td >" + item.item.Price + "</ td > < td >" + item.order.quantity + " </ td ></tr>";
			}
			message.Body += "</tbody></table>";

			message.Subject = "Order Confirmation - Hotel.com" + someArrows;
			message.SubjectEncoding = System.Text.Encoding.UTF8;

			string userState = "test message1";
			client.SendAsync(message, userState);

			client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

			message.Dispose();

			return mailSent;
		}
	}
}
