using Microsoft.EntityFrameworkCore;
using Test.Server.Data;
using Test.Shared.Entities.DataBase;

namespace Test.Server.Services;

public class ProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await _context.Product.ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(string id)
    {
        return await _context.Product.FindAsync(id);
    }

    public async Task CreateProductAsync(Product product)
    {
        _context.Product.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        _context.Product.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(Product product)
    {
        _context.Product.Remove(product);
        await _context.SaveChangesAsync();
    }
    

}
