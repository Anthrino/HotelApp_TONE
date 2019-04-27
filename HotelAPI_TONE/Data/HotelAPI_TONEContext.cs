using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HotelAPI_TONE.Models;

namespace HotelAPI_TONE.Models
{
	public class HotelAPI_TONEContext : DbContext
	{
		public HotelAPI_TONEContext(DbContextOptions<HotelAPI_TONEContext> options)
			: base(options)
		{
		}

		public DbSet<HotelAPI_TONE.Models.Item> Item { get; set; }

		public DbSet<HotelAPI_TONE.Models.Cart> Cart { get; set; }

		public DbSet<HotelAPI_TONE.Models.Users> Users { get; set; }
	}
}
