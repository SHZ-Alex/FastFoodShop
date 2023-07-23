using System;
using FastFoodShop.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FastFoodShop.Services.CouponAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Coupon> Coupons { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Coupon>().HasData(new Coupon
			{
				Id = 1,
				CouponCode = "10OFF",
				DiscountAmount = 10,
				MinAmount = 20
			});
			
			modelBuilder.Entity<Coupon>().HasData(new Coupon
			{
				Id = 2,
				CouponCode = "15OFF",
				DiscountAmount = 15,
				MinAmount = 30
			});
		}
	}
}

