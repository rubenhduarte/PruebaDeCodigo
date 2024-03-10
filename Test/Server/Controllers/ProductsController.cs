using Duende.IdentityServer.Extensions;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Server.Data;
using Test.Server.Services;
using Test.Shared.Entities.DataBase;
using Test.Shared.Entities.DTO;

namespace Test.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseProduct>>> GetAllProducts()
        {

            if (_context.Product == null)
            {   
                return NotFound();
            }

            var products = await _context.Product.ToListAsync();
            var producList = products.Adapt<IEnumerable<ResponseProduct>>();

            return Ok(producList);

        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseProduct>> GetProduct(string id)
        {
            if (_context.Product == null)
            {
                return NotFound();
            }
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product.Adapt<ResponseProduct>();
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(string id, Product product)
        {
            if (id != product.Id)
            {
                return NoContent();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product productRequest)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'TestServerContext.Product'  is null.");
            }

            var productToAdd = productRequest.Adapt<Product>();

            var data =  _context.Product.Add(productToAdd);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var productAdded = _context.Product.Where(x => x.Id.ToLower() == productToAdd.Id.ToLower()).FirstOrDefault();
                var productResponse = productToAdd.Adapt<Product>();
                return Ok(productResponse);
            }
            else
            {
                return StatusCode(500, "table could not be created");
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            if (_context.Product == null)
            {
                return NotFound();
            }
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ProductExists(string id)
        {
            return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }

       
    }
}
