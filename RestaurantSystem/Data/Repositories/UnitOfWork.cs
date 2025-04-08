using RestaurantSystem.Data.Context;
using RestaurantSystem.Models;

namespace RestaurantSystem.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        public IRepository<Ingredient>? _ingredientRepo;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<Ingredient> IngredientRepo => _ingredientRepo ??= new Repository<Ingredient>(_context);

        public Task CommitAsync() => _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
