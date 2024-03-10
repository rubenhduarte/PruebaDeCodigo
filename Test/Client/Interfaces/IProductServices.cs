using Test.Shared.Entities;
using Test.Shared.Entities.DataBase;

namespace Test.Client.Interfaces;

public interface IProductServices
{
    Task<List<Product>> GetAllProduct();
    Task<Product> GetProductById(string id);
    Task <AppResult> DeleteProduct(string id);
    Task <AppResult> AddProduct(Product product);
    Task <AppResult> UpdateProduct(Product product);
    Task<Byte[]> ExportToPdf(string NombreDeArchivo, List<Product> products);
}
