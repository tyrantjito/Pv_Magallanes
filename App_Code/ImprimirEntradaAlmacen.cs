using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using System.Data;
using E_Utilities;
using System.Drawing;

/// <summary>
/// Descripción breve de ImprimirEntradaAlmacen
/// </summary>
public class ImprimirEntradaAlmacen
{
    public ImprimirEntradaAlmacen()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public string imprimeEntrada(string folio)
    {
        Fechas fechas = new Fechas();
        Document documento = new Document(iTextSharp.text.PageSize.B6);
        documento.AddTitle("Inventario");
        documento.AddCreator("E-PuntoVenta");

        string ruta = HttpContext.Current.Server.MapPath("~/Tickets");
        string archivo = ruta + "\\OrdenCompra" + folio + "_" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + ".pdf";

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
        {
            Directory.CreateDirectory(ruta);
        }

        if (archivo.Trim() != "")
        {
            try
            {
                FileInfo fil = new FileInfo(archivo);
                if (fil.Exists)
                    fil.Delete();
            }
            catch (Exception) { }

            FileStream file = new FileStream(archivo,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite);
            PdfWriter.GetInstance(documento, file);
            // Abrir documento.
            documento.Open();

            //Insertar logo o imagen            
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font _standardFont1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            string rutaLogo = HttpContext.Current.Server.MapPath("~/img/logo.png");
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(rutaLogo);
            logo.ScaleToFit(200, 100);
            logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            documento.Add(logo);
            //documento.Add(new Paragraph(" "));
            iTextSharp.text.Font fuente4 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            Paragraph aten = new Paragraph("Compras de contado".ToUpper(),fuente4);
            aten.Alignment = Element.ALIGN_CENTER;
            documento.Add(aten);

            llenaEntrada(documento, folio);

            documento.Close();
        }
        return archivo;
    }

    private void llenaEntrada(Document documento, string folio)
    {              
        object[] ejecutado = OrdenCompra.ordenDatos(folio);
        DataSet data = new DataSet();
        if (Convert.ToBoolean(ejecutado[0]))
            data = (DataSet)ejecutado[1];
        else
            data = null;
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuente3 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente4 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        PdfPTable tablaEntDet = new PdfPTable(3);
        tablaEntDet.WidthPercentage = 60f;
        PdfPCell producto = new PdfPCell();
        PdfPCell productoCantidad = new PdfPCell();
        PdfPCell productoCostoUnit = new PdfPCell();
        PdfPCell productoImporte = new PdfPCell();
        PdfPCell categoria = new PdfPCell();
        PdfPCell totales = new PdfPCell();
        int idCategoria = -1,contador=0;
        string categoriaDesc = "", categoriaDescNew = "";
        decimal monto = 0, total = 0;
        foreach (DataRow row in data.Tables[0].Rows)
        {
            if (idCategoria == 0)
                categoriaDesc = "Sin Categoria";
            else
                categoriaDesc = row[5].ToString();
            if (categoriaDesc == "" || categoriaDesc != categoriaDescNew )
            {
                categoriaDescNew = row[5].ToString();
                try { idCategoria = Convert.ToInt32(row[4]); } catch (Exception) { idCategoria = 0; }
                categoria = new PdfPCell(new Phrase(categoriaDesc.ToUpper(), fuente4));
                categoria.Colspan = 3;
                categoria.Padding = 6;
                categoria.BorderWidth = 0;
                categoria.HorizontalAlignment = 1;
                categoria.VerticalAlignment = 1;
                tablaEntDet.AddCell(categoria);
                contador++;
            }

            producto = new PdfPCell(new Phrase(row[0].ToString()+"   "+row[7].ToString(), fuente3));
            producto.Colspan = 3;
            producto.Padding = 3;
            producto.BorderWidth = 0;
            producto.HorizontalAlignment = 0;
            producto.VerticalAlignment = 1;
            tablaEntDet.AddCell(producto);

            productoCantidad = new PdfPCell(new Phrase(row[1].ToString() + " (" + row[6].ToString() + ")", fuente1));
            productoCantidad.BorderWidth = 0;
            productoCantidad.Padding = 3;
            productoCantidad.HorizontalAlignment = 1;
            productoCantidad.VerticalAlignment = 1;
            tablaEntDet.AddCell(productoCantidad);

            productoCostoUnit = new PdfPCell(new Phrase("$ " + row[2].ToString(), fuente1));
            productoCostoUnit.BorderWidth = 0;
            productoCostoUnit.Padding = 3;
            productoCostoUnit.HorizontalAlignment = 2;
            productoCostoUnit.VerticalAlignment = 1;
            tablaEntDet.AddCell(productoCostoUnit);

            productoImporte = new PdfPCell(new Phrase("$ " + row[3].ToString(), fuente2));
            productoImporte.BorderWidth = 0;
            productoImporte.Padding = 3;
            productoImporte.HorizontalAlignment = 2;
            productoImporte.VerticalAlignment = 1;
            tablaEntDet.AddCell(productoImporte);
            try { monto = Convert.ToDecimal(row[3].ToString()); } catch (Exception) { monto = 0; }
            total += monto;
        }
        for (int filas = 0; filas < 2; filas++)
        {
            totales = new PdfPCell(new Phrase(" ", fuente4));
            totales.Colspan = 3;
            totales.Padding = 3;
            totales.BorderWidth = 0;
            totales.HorizontalAlignment = 1;
            totales.VerticalAlignment = 1;
            tablaEntDet.AddCell(totales);
        }

        totales = new PdfPCell(new Phrase("COSTO DE COMPRAS DE CONTADO:", fuente4));
        totales.Colspan = 3;
        totales.Padding = 3;
        totales.BorderWidth = 0;
        totales.HorizontalAlignment = 1;
        totales.VerticalAlignment = 1;
        tablaEntDet.AddCell(totales);

        totales = new PdfPCell(new Phrase("", fuente4));
        totales.Colspan = 3;
        totales.Padding = 3;
        totales.BorderWidth = 0;
        totales.HorizontalAlignment = 1;
        totales.VerticalAlignment = 1;
        tablaEntDet.AddCell(totales);

        totales = new PdfPCell(new Phrase("$ " + total.ToString("F2"), fuente4));
        totales.Colspan = 3;
        totales.Padding = 3;
        totales.BorderWidth = 0;
        totales.HorizontalAlignment = 1;
        totales.VerticalAlignment = 1;
        tablaEntDet.AddCell(totales);
        documento.Add(tablaEntDet);
    }
}