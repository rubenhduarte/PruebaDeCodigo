using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Test.Client.Interfaces;
using Test.Shared.Entities;
using Test.Shared.Entities.DataBase;
namespace Test.Client.Services;

public class ProductServices : IProductServices
{
    public ProductServices(HttpClient http, NavigationManager navigationManager)
    {
        Http = http;
        NavigationManager = navigationManager;
    }
    public HttpClient Http { get; }
    public NavigationManager NavigationManager { get; }

    public async Task<AppResult> AddProduct(Product product)
    {
        AppResult result = new AppResult();
        if (product == null)
        {
            result.Result = AppResultStatus.Failed;
            result.Message = "Product is null";
            return result;
        }
        try
        {
            product.Id = Guid.NewGuid().ToString();
            var response = await Http.PostAsJsonAsync("api/Products", product);
            string content = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                result.Result = AppResultStatus.Ok;
                result.Message = "created succcessfully";
            }
        }
        catch (Exception ex)
        {
            result.Result = AppResultStatus.InternalError;
            result.Message = $"the product could not be created {ex}";
        }
        return result;
    }

    public async Task<AppResult> DeleteProduct(string id)
    {
        AppResult deletedResult = new AppResult();
        var existingProduct = GetProductById(id);
        if (existingProduct == null)
        {
            deletedResult.Result = AppResultStatus.InternalError;
            deletedResult.Message = "product is not fount";
            return deletedResult;
        }
        var response = await Http.DeleteAsync($"api/Products/{id}");
        var result = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            NavigationManager.NavigateTo("/logout");
            return null;
        }
        try
        {
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("product can deleted");
                deletedResult.Result = AppResultStatus.Ok;
                deletedResult.Message = "delete successfully";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("product can deleted");
            deletedResult.Result = AppResultStatus.InternalError;
            deletedResult.Message = $"delete is not Succesful {ex}";
        }
        return deletedResult;

    }
    public async Task<Product> GetProductById(string id)
    {
        var response = await Http.GetAsync($"api/Products/{id}");
        string content = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            NavigationManager.NavigateTo("/logout");
            return null;
        }
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            var product = JsonSerializer.Deserialize<Product>(content, options);
            return product;
        }
        catch (Exception)
        {

            return null;
        }
    }
    public async Task<AppResult> UpdateProduct(Product product)
    {
        AppResult updatedResult = new AppResult();
        var existingProduct = await GetProductById(product?.Id);
        if (existingProduct == null)
        {
            updatedResult.Result = AppResultStatus.InternalError;
            updatedResult.Message = "product is not fount";
            return updatedResult;
        }

        var response = await Http.PutAsJsonAsync($"api/Products/{product.Id}", product);
        string content = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            updatedResult.Result = AppResultStatus.InternalError;
            updatedResult.Message = "unauthorized";
            NavigationManager.NavigateTo("/logout");
            return updatedResult;
        }
        try
        {
            if (response.IsSuccessStatusCode)
            {
                updatedResult.Result = AppResultStatus.Ok;
                updatedResult.Message = "created succcessfully";
            }
        }
        catch (Exception ex)
        {
            updatedResult.Result = AppResultStatus.InternalError;
            updatedResult.Message = $"the product could not be created {ex}";
        }
        return updatedResult;
    }

    public async Task<List<Product>> GetAllProduct()
    {
        var list = new List<Product>();
        try
        {
            if (!Http.DefaultRequestHeaders.Authorization.Parameter.Contains("ey") || Http.DefaultRequestHeaders.Authorization == null)
            {
                await Task.Delay(1000);
            }
            var response = await Http.GetAsync($"api/Products");
            string content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                NavigationManager.NavigateTo("/logout");
                return list;
            }
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var obj = JsonSerializer.Deserialize<IEnumerable<Product>>(content, options);
            foreach (var item in obj)
            {
                list.Add(item);
            }
        }
        catch (Exception ex)
        {

        }

        return list;
    }

    public async Task<byte[]> ExportToPdf(string NombreDeArchivo)
    {
        try
        {
            HttpResponseMessage response = await Http.GetAsync($"api/Products/ExportToPdf?nombreDeArchivo={NombreDeArchivo}");

            // Verificar si la solicitud fue exitosa
            response.EnsureSuccessStatusCode();

            // Leer el contenido del archivo PDF como un array de bytes
            byte[] pdfBytes = await response.Content.ReadAsByteArrayAsync();

            //// Devolver el array de bytes del PDF

            //if (response.IsSuccessStatusCode)
            //{
            //    try
            //    {
            //        File.WriteAllBytes(NombreDeArchivo, pdfBytes);
            //    }
            //    catch (Exception ex)
            //    { 
                
            //    }
            //}
            //else
            //{
            //    Console.WriteLine($"Error al generar el archivo PDF: {response.StatusCode}");
            //}
            return pdfBytes;


        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar el archivo PDF: {ex.Message}");
            return null;
        }
    }
}
