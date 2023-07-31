using FastFood.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Services.ProductAPI.Data;
public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                Name = "Самса",
                Price = 15,
                Description = " Самса от дяди васи.<br/> Приготовлена в горах Сванетии. 2+1 акция.",
                ImageUrl = "https://gotovim-doma.ru/images/recipe/3/50/350f7e02367db6ab8bc271bc7a836495.jpg",
                CategoryName = "Хлеб"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 2,
                Name = "Шаурма",
                Price = 19.99,
                Description = " Та самая шаурма на средном.<br/> По рецепту НиНо. С тем же соусом, та самая.",
                ImageUrl = "https://media-cdn.tripadvisor.com/media/photo-s/16/50/30/86/caption.jpg",
                CategoryName = "Шаурма"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 3,
                Name = "Муравейник",
                Price = 10.99,
                Description =
                    " Просто топ. Ой, картина не та",
                ImageUrl = "https://siapress.ru/images/news/main/118312-v-hanti-mansiyske-nashli-samiy-gigantskiy-muraveynik-v-rf.jpg",
                CategoryName = "Дессерт"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 4,
                Name = "Сосиска в тесте",
                Price = 250,
                Description =
                    " Куплена на втором этаже легендарного магазина в Казахстане.<br/> Жаренная сосиска в тесте. Саше возьмите 2 штуки.",
                ImageUrl =
                    "https://cdn.lifehacker.ru/wp-content/uploads/2021/10/Depositphotos_446841358_XL_1633610444-scaled-e1633610551746-1280x640.jpg",
                CategoryName = "Хлеб"
            });
        }
    }

