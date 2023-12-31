﻿// <auto-generated />
using FastFood.Services.ProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FastFood.Services.ProductAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FastFood.Services.ProductAPI.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageLocalPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("ProductId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            CategoryName = "Хлеб",
                            Description = " Самса от дяди васи.<br/> Приготовлена в горах Сванетии. 2+1 акция.",
                            ImageUrl = "https://gotovim-doma.ru/images/recipe/3/50/350f7e02367db6ab8bc271bc7a836495.jpg",
                            Name = "Самса",
                            Price = 15.0
                        },
                        new
                        {
                            ProductId = 2,
                            CategoryName = "Шаурма",
                            Description = " Та самая шаурма на средном.<br/> По рецепту НиНо. С тем же соусом, та самая.",
                            ImageUrl = "https://media-cdn.tripadvisor.com/media/photo-s/16/50/30/86/caption.jpg",
                            Name = "Шаурма",
                            Price = 19.989999999999998
                        },
                        new
                        {
                            ProductId = 3,
                            CategoryName = "Дессерт",
                            Description = " Просто топ. Ой, картина не та",
                            ImageUrl = "https://siapress.ru/images/news/main/118312-v-hanti-mansiyske-nashli-samiy-gigantskiy-muraveynik-v-rf.jpg",
                            Name = "Муравейник",
                            Price = 10.99
                        },
                        new
                        {
                            ProductId = 4,
                            CategoryName = "Хлеб",
                            Description = " Куплена на втором этаже легендарного магазина в Казахстане.<br/> Жаренная сосиска в тесте. Саше возьмите 2 штуки.",
                            ImageUrl = "https://cdn.lifehacker.ru/wp-content/uploads/2021/10/Depositphotos_446841358_XL_1633610444-scaled-e1633610551746-1280x640.jpg",
                            Name = "Сосиска в тесте",
                            Price = 250.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
