using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SixLabors.Fonts;
using System.Drawing;
using Test.Shared.Entities.DataBase;

namespace Test.Server.Services;

public interface IPdfGenerationService
{
    Task<byte[]> GeneratePdf(List<Product> products);
}
public class PdfGenerationService : IPdfGenerationService
{
    public async Task<byte[]> GeneratePdf(List<Product> products)
    {
        // Crear un nuevo documento PDF
        using (PdfDocument document = new PdfDocument())
        {
            // Añadir una página al documento
            PdfPage page = document.AddPage();

            // Obtener el objeto XGraphics para dibujar en la página
            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Definir la fuente y el formato para el texto
            XFont font = new XFont("Verdana", 20, XFontStyle.Regular);

            int yPosition = 50;
            foreach (var product in products)
            {
                gfx.DrawString($"{product.Name}: {product.Description}", font, XBrushes.Black,
                    new XRect(50, yPosition, page.Width.Point - 100, page.Height.Point - 100),
                    XStringFormats.TopLeft);
                yPosition += 20;
            }

            byte[] pdfBytes;

            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream, false);
                pdfBytes = stream.ToArray();
            }

            return pdfBytes;
        }
    }
}