using FastFoodWeb.Data;
using FastFoodWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace FastFoodWeb.Repositories;

// Interface mở rộng thêm method riêng của Product
public interface IProductRepository : IRepository<Product>
{
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<Product>> GetFeaturedAsync();
    Task<IEnumerable<Product>> SearchAsync(string keyword);
}

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext ctx) : base(ctx) { }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        => await _context.Products
            .Include(p => p.Category)
            .Where(p => p.CategoryId == categoryId && p.IsAvailable)
            .ToListAsync();

    public async Task<IEnumerable<Product>> GetFeaturedAsync()
        => await _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsFeatured && p.IsAvailable)
            .ToListAsync();

    public async Task<IEnumerable<Product>> SearchAsync(string keyword)
        => await _context.Products
            .Include(p => p.Category)
            .Where(p => p.Name.Contains(keyword) && p.IsAvailable)
            .ToListAsync();
}