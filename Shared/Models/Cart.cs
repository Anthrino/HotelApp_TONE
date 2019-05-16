﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Models
{
	public class Cart
	{
		public int Id { get; set; }
		public int userId { get; set; }
		public int itemId { get; set; }
		[Range(0, 20, ErrorMessage = "Please select Number within (0,20)")]
		[DisplayName("Quantity")]
		public int quantity { get; set; }
	}
}
