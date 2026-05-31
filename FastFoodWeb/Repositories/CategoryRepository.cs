using FastFoodWeb.Data;
using FastFoodWeb.Models;

namespace FastFoodWeb.Repositories;

public interface ICategoryRepository : IRepository<Category> { }

public class CategoryRepository
    : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext ctx)
        : base(ctx) { }
}