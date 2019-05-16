using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace DataAccessLayer.Data
{
	public class Hotel_TONEDbContext : DbContext
	{
		public Hotel_TONEDbContext(DbContextOptions<Hotel_TONEDbContext> options)
			: base(options)
		{
		}
		public Hotel_TONEDbContext(string connectionString)
		   : base(GetOptions(connectionString))
		{
		}
		public DbSet<Item> Item { get; set; }

		public DbSet<Cart> Cart { get; set; }

		public DbSet<Users> Users { get; set; }

		public DbSet<Orders> Orders { get; set; }

		private static DbContextOptions GetOptions(string connectionString)
		{
			return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
		}
	}
}
