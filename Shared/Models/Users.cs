using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Models
{
	public class Users
	{
		public int Id { get; set; }
		public string displayName { get; set; }
		public string username { get; set; }
		public int orderCount { get; set; }
		public string token { get; set; }
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

  