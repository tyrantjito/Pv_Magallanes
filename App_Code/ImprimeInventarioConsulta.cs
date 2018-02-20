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
using LibPrintTicket;

/// <summary>
/// Descripción breve de ImprimeInventarioConsulta
/// </summary>
public class ImprimeInventarioConsulta
{
    Fechas fechas = new Fechas();
    public string usuario { get; set; }
    public int isla { get; set; }

    private string _nombreIslas;

    public string NombreIslas
    { set { this._nombreIslas = value; } }

    public ImprimeInventarioConsulta()
    {
        usuario = _nombreIslas = string.Empty;
        isla = 0;
    }

    public string imprimeInventario()
    {
        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LETTER.Rotate());
        documento.AddTitle("Inventario");
        documento.AddCreator("E-PuntoVenta");

        string ruta = HttpContext.Current.Server.MapPath("~/Tickets");
        _nombreIslas = _nombreIslas.Replace(' ', '_');
        string archivo = ruta + "\\Inventario_" + _nombreIslas.ToString() + "_" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + ".pdf";

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
        {
            Directory.CreateDirectory(ruta);
        }

        if (archivo.Trim() != "")
        {
            FileInfo fil = new FileInfo(archivo);
            if (fil.Exists)
                fil.Delete();

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
            documento.Add(new Paragraph(" "));

            Paragraph titulo = new Paragraph("INVENTARIO", _standardFont);
            titulo.Alignment = Element.ALIGN_CENTER;
            documento.Add(titulo);

            Paragraph nombre = new Paragraph("NOMBRE DE LA TIENDA: " + _nombreIslas, _standardFont);
            nombre.Alignment = Element.ALIGN_CENTER;
            documento.Add(nombre);
            
            Paragraph fecha = new Paragraph("Fecha: " + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd"), _standardFont1);
            fecha.Alignment = Element.ALIGN_RIGHT;
            documento.Add(fecha);

            Paragraph hora = new Paragraph("Hora: " + fechas.obtieneFechaLocal().ToString("HH:mm:ss"), _standardFont1);
            hora.Alignment = Element.ALIGN_RIGHT;
            documento.Add(hora);

            Paragraph usuariop = new Paragraph("Usuario: " + usuario.ToString().ToUpper(), _standardFont1);
            usuariop.Alignment = Element.ALIGN_RIGHT;
            documento.Add(usuariop);

            documento.AddCreationDate();

            documento.Add(new Paragraph(" "));
            ProductosTicket(documento);

            documento.Close();

        }
        return archivo;
    }
    private void ProductosTicket(Document document)
    {

        // Tipo de Font que vamos utilizar
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        BaseColor colorgris = new BaseColor(Color.LightGray);

        PdfPTable tblProductos = new PdfPTable(10);
        PdfPCell clcla = new PdfPCell(new Phrase("CLAVE", fuente1));
        clcla.BorderWidthBottom = 1;
        clcla.HorizontalAlignment = 1;
        clcla.VerticalAlignment = 1;
        clcla.Padding = 1;
        clcla.BackgroundColor = colorgris;

        PdfPCell tituloProducto = new PdfPCell(new Phrase("PRODUCTO", fuente1));
        tituloProducto.BorderWidthBottom = 1;
        tituloProducto.HorizontalAlignment = 1;
        tituloProducto.VerticalAlignment = 1;
        tituloProducto.Padding = 1;
        tituloProducto.BackgroundColor = colorgris;

        PdfPCell clexist = new PdfPCell(new Phrase("EXISTENCIA", fuente1));
        clexist.BorderWidthBottom = 1;
        clexist.HorizontalAlignment = 1;
        clexist.VerticalAlignment = 1;
        clexist.Padding = 1;
        clexist.BackgroundColor = colorgris;

        PdfPCell clcostun = new PdfPCell(new Phrase("COSTO UNITARIO", fuente1));
        clcostun.BorderWidthBottom = 1;
        clcostun.HorizontalAlignment = 1;
        clcostun.VerticalAlignment = 1;
        clcostun.Padding = 1;
        clcostun.BackgroundColor = colorgris;


        PdfPCell clstmin = new PdfPCell(new Phrase("STOCK MIN", fuente1));
        clstmin.BorderWidthBottom = 1;
        clstmin.HorizontalAlignment = 1;
        clstmin.VerticalAlignment = 1;
        clstmin.Padding = 1;
        clstmin.BackgroundColor = colorgris;

        PdfPCell clstmax = new PdfPCell(new Phrase("STOCK MAX", fuente1));
        clstmax.BorderWidthBottom = 1;
        clstmax.HorizontalAlignment = 1;
        clstmax.VerticalAlignment = 1;
        clstmax.Padding = 1;
        clstmax.BackgroundColor = colorgris;

        PdfPCell clspreven = new PdfPCell(new Phrase("PRECIO VENTA", fuente1));
        clspreven.BorderWidthBottom = 1;
        clspreven.HorizontalAlignment = 1;
        clspreven.VerticalAlignment = 1;
        clspreven.Padding = 1;
        clspreven.BackgroundColor = colorgris;

        PdfPCell cltcosuni = new PdfPCell(new Phrase("TOTAL COSTO UNITARIO", fuente1));
        cltcosuni.BorderWidthBottom = 1;
        cltcosuni.HorizontalAlignment = 1;
        cltcosuni.VerticalAlignment = 1;
        cltcosuni.Padding = 1;
        cltcosuni.BackgroundColor = colorgris;

        PdfPCell cltotalpv = new PdfPCell(new Phrase("TOTAL PRECIO VENTA", fuente1));
        cltotalpv.BorderWidthBottom = 1;
        cltotalpv.HorizontalAlignment = 1;
        cltotalpv.VerticalAlignment = 1;
        cltotalpv.Padding = 1;
        cltotalpv.BackgroundColor = colorgris;

    
        PdfPCell clutild = new PdfPCell(new Phrase("UTILIDAD", fuente1));
        clutild.BorderWidthBottom = 1;
        clutild.HorizontalAlignment = 1;
        clutild.VerticalAlignment = 1;
        clutild.Padding = 1;
        clutild.BackgroundColor = colorgris;

        tblProductos.AddCell(clcla);
        tblProductos.AddCell(tituloProducto);
        tblProductos.AddCell(clexist);
        tblProductos.AddCell(clcostun);
        tblProductos.AddCell(clstmin);
        tblProductos.AddCell(clstmax);
        tblProductos.AddCell(clspreven);
        tblProductos.AddCell(cltcosuni);
        tblProductos.AddCell(cltotalpv);
        tblProductos.AddCell(clutild);

        int tamañodatos = 0;

        Producto datosProducto = new Producto();
        DataSet data = new DataSet();
        data = datosProducto.llenaConsultaAjusteIsla(isla, "");
        
        decimal existencias = 0, totalcu = 0, totalventa = 0, totalutilidad=0, productostot = 0;
        if (data.Tables[0].Rows.Count > 0)
        {  
            foreach (DataRow fila in data.Tables[0].Rows)
            {
                PdfPCell clave = new PdfPCell(new Phrase(fila[2].ToString(), fuente2));
                PdfPCell registroProducto = new PdfPCell(new Phrase(fila[3].ToString(), fuente2));
                PdfPCell existencia = new PdfPCell(new Phrase(fila[4].ToString(), fuente2));
                PdfPCell costounitario = new PdfPCell(new Phrase(Convert.ToDecimal(fila[5].ToString()).ToString("C2"), fuente2));
                PdfPCell stockmin = new PdfPCell(new Phrase(fila[6].ToString(), fuente2));
                PdfPCell stockmax = new PdfPCell(new Phrase(fila[7].ToString(), fuente2));
                PdfPCell precioventa = new PdfPCell(new Phrase(Convert.ToDecimal(fila[8].ToString()).ToString("C2"), fuente2));
                PdfPCell totalcostounitario = new PdfPCell(new Phrase(Convert.ToDecimal(fila[10].ToString()).ToString("C2"), fuente2));
                PdfPCell totalprecioventa = new PdfPCell(new Phrase(Convert.ToDecimal(fila[11].ToString()).ToString("C2"), fuente2));
                PdfPCell utilidad = new PdfPCell(new Phrase(Convert.ToDecimal(fila[12].ToString()).ToString("C2"), fuente2));


                existencias = existencias + Convert.ToDecimal(fila[4].ToString());
                totalcu = totalcu + Convert.ToDecimal(fila[10].ToString());
                totalventa = totalventa + Convert.ToDecimal(fila[11].ToString());
                totalutilidad = totalutilidad + Convert.ToDecimal(fila[12].ToString());
                productostot++;

                clave.BorderWidth = 1;
                clave.HorizontalAlignment = 1;
                clave.VerticalAlignment = 1;
                clave.Padding = 1;

                registroProducto.BorderWidth = 1;
                registroProducto.HorizontalAlignment = 1;
                registroProducto.VerticalAlignment = 1;
                registroProducto.Padding = 1;

                existencia.BorderWidth = 1;
                existencia.HorizontalAlignment = 1;
                existencia.VerticalAlignment = 1;
                existencia.Padding = 1;

                costounitario.BorderWidth = 1;
                costounitario.HorizontalAlignment = 1;
                costounitario.VerticalAlignment = 1;
                costounitario.Padding = 1;

                stockmin.BorderWidth = 1;
                stockmin.HorizontalAlignment = 1;
                stockmin.VerticalAlignment = 1;
                stockmin.Padding = 1;

                stockmax.BorderWidth = 1;
                stockmax.HorizontalAlignment = 1;
                stockmax.VerticalAlignment = 1;
                stockmax.Padding = 1;

                precioventa.BorderWidth = 1;
                precioventa.HorizontalAlignment = 1;
                precioventa.VerticalAlignment = 1;
                precioventa.Padding = 1;

                totalcostounitario.BorderWidth = 1;
                totalcostounitario.HorizontalAlignment = 1;
                totalcostounitario.VerticalAlignment = 1;
                totalcostounitario.Padding = 1;

                totalprecioventa.BorderWidth = 1;
                totalprecioventa.HorizontalAlignment = 1;
                totalprecioventa.VerticalAlignment = 1;
                totalprecioventa.Padding = 1;

                utilidad.BorderWidth = 1;
                utilidad.HorizontalAlignment = 1;
                utilidad.VerticalAlignment = 1;
                utilidad.Padding = 1;

                tblProductos.AddCell(clave);
                tblProductos.AddCell(registroProducto);
                tblProductos.AddCell(existencia);
                tblProductos.AddCell(costounitario);
                tblProductos.AddCell(stockmin);
                tblProductos.AddCell(stockmax);
                tblProductos.AddCell(precioventa);
                tblProductos.AddCell(totalcostounitario);
                tblProductos.AddCell(totalprecioventa);
                tblProductos.AddCell(utilidad);                

                tamañodatos++;
            }
        }

        PdfPCell tclave = new PdfPCell(new Phrase("Totales:", fuente1));
        PdfPCell tregistroProducto = new PdfPCell(new Phrase(productostot.ToString() + " productos.", fuente1));
        PdfPCell texistencia = new PdfPCell(new Phrase(existencias.ToString("F3"), fuente1));
        PdfPCell tcostounitario = new PdfPCell(new Phrase("", fuente1));
        PdfPCell tstockmin = new PdfPCell(new Phrase("", fuente1));
        PdfPCell tstockmax = new PdfPCell(new Phrase("", fuente1));
        PdfPCell tprecioventa = new PdfPCell(new Phrase("", fuente1));
        PdfPCell ttotalcostounitario = new PdfPCell(new Phrase(totalcu.ToString("C2"), fuente1));
        PdfPCell ttotalprecioventa = new PdfPCell(new Phrase(totalventa.ToString("C2"), fuente1));
        PdfPCell tutilidad = new PdfPCell(new Phrase(totalutilidad.ToString("C2"), fuente1));
        tclave.BorderWidth = 1;
        tclave.HorizontalAlignment = 1;
        tclave.VerticalAlignment = 1;
        tclave.Padding = 1;
       
        tclave.BackgroundColor = colorgris;
        

        tregistroProducto.BorderWidth = 1;
        tregistroProducto.HorizontalAlignment = 1;
        tregistroProducto.VerticalAlignment = 1;
        tregistroProducto.Padding = 1;
        tregistroProducto.BackgroundColor = colorgris;

        texistencia.BorderWidth = 1;
        texistencia.HorizontalAlignment = 1;
        texistencia.VerticalAlignment = 1;
        texistencia.Padding = 1;
        texistencia.BackgroundColor = colorgris;

        tcostounitario.BorderWidth = 1;
        tcostounitario.HorizontalAlignment = 1;
        tcostounitario.VerticalAlignment = 1;
        tcostounitario.Padding = 1;
        tcostounitario.BackgroundColor = colorgris;

        tstockmin.BorderWidth = 1;
        tstockmin.HorizontalAlignment = 1;
        tstockmin.VerticalAlignment = 1;
        tstockmin.Padding = 1;
        tstockmin.BackgroundColor = colorgris;

        tstockmax.BorderWidth = 1;
        tstockmax.HorizontalAlignment = 1;
        tstockmax.VerticalAlignment = 1;
        tstockmax.Padding = 1;
        tstockmax.BackgroundColor = colorgris;

        tprecioventa.BorderWidth = 1;
        tprecioventa.HorizontalAlignment = 1;
        tprecioventa.VerticalAlignment = 1;
        tprecioventa.Padding = 1;
        tprecioventa.BackgroundColor = colorgris;


        ttotalcostounitario.BorderWidth = 1;
        ttotalcostounitario.HorizontalAlignment = 1;
        ttotalcostounitario.VerticalAlignment = 1;
        ttotalcostounitario.Padding = 1;
        ttotalcostounitario.BackgroundColor = colorgris;

        ttotalprecioventa.BorderWidth = 1;
        ttotalprecioventa.HorizontalAlignment = 1;
        ttotalprecioventa.VerticalAlignment = 1;
        ttotalprecioventa.Padding = 1;
        ttotalprecioventa.BackgroundColor = colorgris;

        tutilidad.BorderWidth = 1;
        tutilidad.HorizontalAlignment = 1;
        tutilidad.VerticalAlignment = 1;
        tutilidad.Padding = 1;
        tutilidad.BackgroundColor = colorgris;

        tblProductos.AddCell(tclave);
        tblProductos.AddCell(tregistroProducto);
        tblProductos.AddCell(texistencia);
        tblProductos.AddCell(tcostounitario);
        tblProductos.AddCell(tstockmin);
        tblProductos.AddCell(tstockmax);
        tblProductos.AddCell(tprecioventa);
        tblProductos.AddCell(ttotalcostounitario);
        tblProductos.AddCell(ttotalprecioventa);
        tblProductos.AddCell(tutilidad);

        document.Add(tblProductos);
        
    }

}