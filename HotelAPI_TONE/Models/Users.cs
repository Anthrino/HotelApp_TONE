using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelAPI_TONE.Models
{
	public class Users
	{
		public int Id { get; set; }
		public string username { get; set; }
		public string password { get; set; }
		public int role { get; set; }
		public int orderCount { get; set; }
		[NotMapped]
		public string token { get; set; }
	}
}
