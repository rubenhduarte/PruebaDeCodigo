using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.Drawing;
using Test.Shared.Entities.DataBase;

namespace Test.Server.Services;

public interface IPdfGenerationService
{
    public byte[] GeneratePdf(List<Product> products);
}
public class PdfGenerationService : IPdfGenerationService
{
    public byte[] GeneratePdf(List<Product> products)
    {
        // Crear un nuevo documento PDF
        using (PdfDocument document = new PdfDocument())
        {
            // Añadir una página al documento
            PdfPage page = document.AddPage();

            // Obtener el objeto XGraphics para dibujar en la página
            using (XGraphics gfx = XGraphics.FromPdfPage(page))
            {
                // Definir la fuente y el formato de texto
                XFont font = new XFont("Arial", 12, XFontStyle.Regular);

                // Dibujar texto en la página
                gfx.DrawString("Hola, este es un documento PDF generado dinámicamente.", font, XBrushes.Black, new XPoint(50, 50));
            }

            // Convertir el documento PDF a un array de bytes
            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream, false);
                return stream.ToArray();
            }
        }
    }
}