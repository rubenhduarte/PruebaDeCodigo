using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Test.Server.Data;

namespace Test.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExcelController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ExportarTodosLosProductos")]
        public async Task<IActionResult> ExportExcel1()
        {

            try
            {
                var products = await _context.Product.ToListAsync();

                DataTable table = new DataTable();

                table.Columns.Add("Modelo");
                table.Columns.Add("Nombre");
                table.Columns.Add("Descripcion");
                table.Columns.Add("Precio");


                foreach (var product in products)
                {

                    DataRow fila = table.NewRow();
                    fila["Modelo"] = product.Model;
                    fila["Nombre"] = product.Name;
                    fila["Descripcion"] = product.Description;
                    fila["Precio"] = product.Price;

                    table.Rows.Add(fila);

                }

                using var libro = new XLWorkbook();
                table.TableName = "Productos";

                var hoja = libro.Worksheets.Add(table);

                hoja.ColumnsUsed().AdjustToContents();

                using var memoria = new MemoryStream();
                libro.SaveAs(memoria);
                var nombreExcel = "Reporte.xlsx";
                var archivo = File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                return archivo;
            }
            catch (Exception)
            {
                throw;

            }

        }

    }
}
