using SendGrid;
using SendGrid.Helpers.Mail;
using Shared.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Shared.Helpers
{
	public class EmailService
	{
		public async Task<bool> EmailNotifier(string apiKey, Users user, IEnumerable<OrderItem> orders)
		{
			var client = new SendGridClient(apiKey);
			var from = new EmailAddress("hoteltone@outlook.com", "Hotel Admin");
			List<EmailAddress> tos = new List<EmailAddress>
			{
			  new EmailAddress(user.username, user.displayName),
			};

			var subject = "TONE Hotel - Order Confirmation";
			var htmlContent = await System.IO.File.ReadAllTextAsync("Shared/Templates/orderConfMailTemplate.html");

			var body = htmlContent.Split("<tbody>")[1].Split("</tbody>")[0];
			string orderList = "";

			foreach (var item in orders)
			{
				orderList += body.Replace("orderId", item.order.orderId.ToString()).Replace("Title", item.item.Title.ToString()).Replace("Price", item.item.Price.ToString()).Replace("quantity", item.order.quantity.ToString());
			}

			htmlContent = htmlContent.Replace(body, orderList);

			var displayRecipients = false; // set this to true if you want recipients to see each others mail id 
			var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, "", htmlContent, false);
			var response = await client.SendEmailAsync(msg);
			if (response.StatusCode == HttpStatusCode.OK)
				return true;
			return false;
		}
	}
}
