using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelMVC_TONE.Models
{
	public class Users
	{
		public int Id { get; set; }
		[DisplayName("Username")]
		[Required(ErrorMessage = "Email address is required")]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string username { get; set; }
		[DisplayName("<DB_Conn_String>")]
		public string password { get; set; }
		public int orderCount { get; set; }
		public string token { get; set; }
		[Required]
		public int role {
			get
			{
				return (int)this.roleType;
			}
			set
			{
				roleType = (roleTypes)value;
			}
		}
		[NotMapped]
		[EnumDataType(typeof(roleTypes))]
		public roleTypes roleType { get; set; }
		
	}

	public enum roleTypes
	{
		Admin = 1,
		Vendor = 2,
		Customer = 3
	}
}

  