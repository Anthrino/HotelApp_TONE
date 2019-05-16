using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Models
{
	public class Item
	{
		public int Id { get; set; }
		[DisplayName("Item")]
		[Required(ErrorMessage = "Mandatory Field")] 
		public string Title { get; set; }
		[DisplayName("Category")]
		[Required(ErrorMessage = "Mandatory Field")] 
		public string Category { get; set; }
		[DisplayName("Item Description")]
		[Required(ErrorMessage = "Mandatory Field")] 
		public string Description { get; set; }
		//public byte[] image { get; set; }
		[DataType(DataType.Currency)]
		[Required(ErrorMessage = "Mandatory Field")]
		public decimal Price { get; set; }
	}
}
