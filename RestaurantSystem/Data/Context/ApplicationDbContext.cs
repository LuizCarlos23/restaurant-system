using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Models;

namespace RestaurantSystem.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<OrderItem> OrderedItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Food>()
                .HasMany(f => f.OptionalIngredients)
                .WithMany(i => i.OptionalForFoods)
                .UsingEntity(j => j.ToTable("FoodOptionalIngredients"));

            builder.Entity<Food>()
                .HasMany(f => f.ExclusiveIngredients)
                .WithMany(i => i.ExclusiveForFoods)
                .UsingEntity(j => j.ToTable("FoodExclusiveIngredients"));

            builder.Entity<OrderItem>()
                .HasMany(f => f.OptionalIngredientsSelected)
                .WithMany()
                .UsingEntity(j => j.ToTable("OrderItemOptinalIngredients"));
        }
    }
}
