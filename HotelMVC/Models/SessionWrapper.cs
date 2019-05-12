using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelMVC_TONE.Models
{
	public static class SessionWrapper
	{
		public static int userId { get; set; }
		public static int orderUserId { get; set; }
		public static int role { get; set; }
		public static string token { get; set; }
	}
}
