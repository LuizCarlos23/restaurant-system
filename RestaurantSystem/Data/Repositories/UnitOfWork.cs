using RestaurantSystem.Data.Context;
using RestaurantSystem.Data.Repositories.Food;
using RestaurantSystem.Data.Repositories.Ingredient;
using RestaurantSystem.Data.Repositories.Order;
using RestaurantSystem.Models;

namespace RestaurantSystem.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        public IIngredientRepository? _ingredientRepo;
        public IFoodRepository? _foodRepo;
        public IOrderRepository? _orderRepo;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IIngredientRepository IngredientRepo => _ingredientRepo ??= new IngredientRepository(_context);
        public IFoodRepository FoodRepo => _foodRepo ??= new FoodRepository(_context);
        public IOrderRepository OrderRepo => _orderRepo ??= new OrderRepository(_context);

        public async Task CommitAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
