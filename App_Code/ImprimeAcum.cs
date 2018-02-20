using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.IO;
using E_Utilities;

/// <summary>
/// Descripción breve de ImprimeAcum
/// </summary>
public class ImprimeAcum
{

    string fecha_ini;
    string fecha_fin;
    string _usuario;
    int tot;
    Fechas fechasA = new Fechas();

	public ImprimeAcum()
	{
        fecha_fin = fecha_ini = _usuario = string.Empty;
        
	}

    public string fecha_Ini { set { fecha_ini = value; } }
    public string fecha_Fin { set { fecha_fin = value; } }
    public string Usuario { set { _usuario = value; } }



    public string GenerarTicket()
    {

        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.A4);
        documento.AddTitle("Acumulado");
        documento.AddCreator("E-PuntoVenta");

        string ruta = HttpContext.Current.Server.MapPath("~/Tickets");
        string archivo = ruta + "\\Acumulado_" + fecha_ini.ToString() + "_" 
            + fecha_fin.ToString() + "_"+_usuario+".pdf";

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
        {
            Directory.CreateDirectory(ruta);
        }

        if (archivo.Trim() != "")
        {
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

            Paragraph titulo = new Paragraph("ACUMULADO", _standardFont);
            titulo.Alignment = Element.ALIGN_CENTER;
            documento.Add(titulo);
            
            Paragraph fechas = new Paragraph("DE: "+ fecha_ini + " a " + fecha_fin, _standardFont);
            fechas.Alignment = Element.ALIGN_CENTER;
            documento.Add(fechas);

            Paragraph fecha = new Paragraph("Fecha: "+ fechasA.obtieneFechaLocal().ToString("yyyy-MM-dd"), _standardFont1);
            fecha.Alignment = Element.ALIGN_RIGHT;
            documento.Add(fecha);

            Paragraph hora = new Paragraph("Hora: " + fechasA.obtieneFechaLocal().ToString("HH:mm:ss"), _standardFont1);
            hora.Alignment = Element.ALIGN_RIGHT;
            documento.Add(hora);

            Paragraph usuario = new Paragraph("Usuario: "+_usuario.ToString().ToUpper(),_standardFont1);
            usuario.Alignment = Element.ALIGN_RIGHT;
            documento.Add(usuario);
            
            documento.AddCreationDate();
            
            documento.Add(new Paragraph(" "));
            decimal acumulado = ProductosTicket(documento);

            documento.Add(new Paragraph("---------------------------------------------------------------------------------------------------------------------------------"));

            Paragraph ltotal = new Paragraph("Total: " + acumulado.ToString("C2"), _standardFont);
            ltotal.Alignment = Element.ALIGN_RIGHT;
            documento.Add(ltotal);
            

            documento.Close();

        }
        return archivo;
    }

    private decimal ProductosTicket(Document document)
    {

        // Tipo de Font que vamos utilizar
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        PdfPTable tblProductos = new PdfPTable(13);
        tblProductos.WidthPercentage = 100f;
        PdfPCell clfe = new PdfPCell(new Phrase("FECHA", fuente1));
        clfe.BorderWidthBottom = 1;
        clfe.HorizontalAlignment = 1;
        clfe.VerticalAlignment = 1;
        clfe.Padding = 1;
        PdfPCell clefec = new PdfPCell(new Phrase("EFECTIVO", fuente1));
        clefec.BorderWidthBottom = 1;
        clefec.HorizontalAlignment = 1;
        clefec.VerticalAlignment = 1;
        clefec.Padding = 1;
        PdfPCell cldeb = new PdfPCell(new Phrase("T.DÉBITO", fuente1));
        cldeb.BorderWidthBottom = 1;
        cldeb.HorizontalAlignment = 1;
        cldeb.VerticalAlignment = 1;
        cldeb.Padding = 1;
        PdfPCell clcred = new PdfPCell(new Phrase("T.CRÉDITO", fuente1));
        clcred.BorderWidthBottom = 1;
        clcred.HorizontalAlignment = 1;
        clcred.VerticalAlignment = 1;
        clcred.Padding = 1;

        PdfPCell clpagserv = new PdfPCell(new Phrase("PAGO SERVICIOS", fuente1));
        clpagserv.BorderWidthBottom = 1;
        clpagserv.HorizontalAlignment = 1;
        clpagserv.VerticalAlignment = 1;
        clpagserv.Padding = 1;
        PdfPCell clrecar = new PdfPCell(new Phrase("RECARGAS", fuente1));
        clrecar.BorderWidthBottom = 1;
        clrecar.HorizontalAlignment = 1;
        clrecar.VerticalAlignment = 1;
        clrecar.Padding = 1;

        PdfPCell clgas = new PdfPCell(new Phrase("GASTOS", fuente1));
        clgas.BorderWidthBottom = 1;
        clgas.HorizontalAlignment = 1;
        clgas.VerticalAlignment = 1;
        clgas.Padding = 1;
        PdfPCell clcan = new PdfPCell(new Phrase("CANCELACIONES", fuente1));
        clcan.BorderWidthBottom = 1;
        clcan.HorizontalAlignment = 1;
        clcan.VerticalAlignment = 1;
        clcan.Padding = 1;
        PdfPCell clfon = new PdfPCell(new Phrase("FONDO", fuente1));
        clfon.BorderWidthBottom = 1;
        clfon.HorizontalAlignment = 1;
        clfon.VerticalAlignment = 1;
        clfon.Padding = 1;
        PdfPCell cltal = new PdfPCell(new Phrase("V. TALLER", fuente1));
        cltal.BorderWidthBottom = 1;
        cltal.HorizontalAlignment = 1;
        cltal.VerticalAlignment = 1;
        cltal.Padding = 1;
        PdfPCell clcre = new PdfPCell(new Phrase("V. CREDITO", fuente1));
        clcre.BorderWidthBottom = 1;
        clcre.HorizontalAlignment = 1;
        clcre.VerticalAlignment = 1;
        clcre.Padding = 1;
        PdfPCell cltotal = new PdfPCell(new Phrase("TOTAL", fuente1));
        cltotal.BorderWidthBottom = 1;
        cltotal.HorizontalAlignment = 1;
        cltotal.VerticalAlignment = 1;
        cltotal.Padding = 1;
        PdfPCell clacu = new PdfPCell(new Phrase("ACUM.", fuente1));
        clacu.BorderWidthBottom = 1;
        clacu.HorizontalAlignment = 1;
        clacu.VerticalAlignment = 1;
        clacu.Padding = 1;

        tblProductos.AddCell(clfe);
        tblProductos.AddCell(clefec);
        tblProductos.AddCell(cldeb);
        tblProductos.AddCell(clcred);

        tblProductos.AddCell(clpagserv);
        tblProductos.AddCell(clrecar);

        tblProductos.AddCell(clgas);
        tblProductos.AddCell(clcan);
        tblProductos.AddCell(clfon);
        tblProductos.AddCell(cltal);
        tblProductos.AddCell(clcre);
        tblProductos.AddCell(cltotal);
        tblProductos.AddCell(clacu);

        int tamañodatos = 0;
        BaseDatos data = new BaseDatos();
        string sql = string.Format("select convert(char(10),fecha,120) as fecha,efectivo,debito,credito,gastos,fondo,total,cancelaciones,pagoServicios,recargas,tot_calculado,ventaTaller,ventaCredito from acumulados where usuario='{0}'", _usuario);
        object[] camposcab = data.scalarData(sql);
        decimal acumulado = 0;
        if (Convert.ToBoolean(camposcab[0]))
        {
            DataSet datos = (DataSet)camposcab[1];
           
            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                DateTime fechaC = Convert.ToDateTime(fila[0].ToString());
                PdfPCell fecha = new PdfPCell(new Phrase(fechaC.ToString("dd-MM"), fuente2));
                PdfPCell efect = new PdfPCell(new Phrase(Convert.ToDecimal(fila[1].ToString()).ToString("C2"), fuente2));
                PdfPCell debi = new PdfPCell(new Phrase(Convert.ToDecimal(fila[2].ToString()).ToString("C2"), fuente2));
                PdfPCell cred = new PdfPCell(new Phrase(Convert.ToDecimal(fila[3].ToString()).ToString("C2"), fuente2));
                PdfPCell gast = new PdfPCell(new Phrase(Convert.ToDecimal(fila[4].ToString()).ToString("C2"), fuente2));
                PdfPCell fond = new PdfPCell(new Phrase(Convert.ToDecimal(fila[5].ToString()).ToString("C2"), fuente2));
                PdfPCell tota = new PdfPCell(new Phrase(Convert.ToDecimal(fila[6].ToString()).ToString("C2"), fuente2));
                PdfPCell canc = new PdfPCell(new Phrase(Convert.ToDecimal(fila[7].ToString()).ToString("C2"), fuente2));
                PdfPCell pagSer = new PdfPCell(new Phrase(Convert.ToDecimal(fila[8].ToString()).ToString("C2"), fuente2));
                PdfPCell recar = new PdfPCell(new Phrase(Convert.ToDecimal(fila[9].ToString()).ToString("C2"), fuente2));
                PdfPCell retal = new PdfPCell(new Phrase(Convert.ToDecimal(fila[11].ToString()).ToString("C2"), fuente2));
                PdfPCell recre = new PdfPCell(new Phrase(Convert.ToDecimal(fila[12].ToString()).ToString("C2"), fuente2));
                acumulado = acumulado + Convert.ToDecimal(fila[10].ToString());
                PdfPCell acum = new PdfPCell(new Phrase(acumulado.ToString("C2"), fuente2));

                fecha.BorderWidth = 0;
                fecha.HorizontalAlignment = 2;
                fecha.VerticalAlignment = 1;
                fecha.Padding = 1;
                efect.BorderWidth = 0;
                efect.HorizontalAlignment = 2;
                efect.VerticalAlignment = 1;
                efect.Padding = 1;
                debi.BorderWidth = 0;
                debi.HorizontalAlignment = 2;
                debi.VerticalAlignment = 1;
                debi.Padding = 1;
                cred.BorderWidth = 0;
                cred.HorizontalAlignment = 2;
                cred.VerticalAlignment = 1;
                cred.Padding = 1;
                pagSer.BorderWidth = 0;
                pagSer.HorizontalAlignment = 2;
                pagSer.VerticalAlignment = 1;
                pagSer.Padding = 1;
                recar.BorderWidth = 0;
                recar.HorizontalAlignment = 2;
                recar.VerticalAlignment = 1;
                recar.Padding = 1;
                retal.BorderWidth = 0;
                retal.HorizontalAlignment = 2;
                retal.VerticalAlignment = 1;
                retal.Padding = 1;
                recre.BorderWidth = 0;
                recre.HorizontalAlignment = 2;
                recre.VerticalAlignment = 1;
                recre.Padding = 1;
                gast.BorderWidth = 0;
                gast.HorizontalAlignment = 2;
                gast.VerticalAlignment = 1;
                gast.Padding = 1;                
                canc.BorderWidth = 0;
                canc.HorizontalAlignment = 2;
                canc.VerticalAlignment = 1;
                canc.Padding = 1;
                fond.BorderWidth = 0;
                fond.HorizontalAlignment = 2;
                fond.VerticalAlignment = 1;
                fond.Padding = 1;
                tota.BorderWidth = 0;
                tota.HorizontalAlignment = 2;
                tota.VerticalAlignment = 1;
                tota.Padding = 1;
                acum.BorderWidth = 0;
                acum.HorizontalAlignment = 2;
                acum.VerticalAlignment = 1;
                acum.Padding = 1;
                tblProductos.AddCell(fecha);
                tblProductos.AddCell(efect);
                tblProductos.AddCell(debi);
                tblProductos.AddCell(cred);

                tblProductos.AddCell(pagSer);
                tblProductos.AddCell(recar);

                tblProductos.AddCell(gast);
                tblProductos.AddCell(canc);
                tblProductos.AddCell(fond);
                tblProductos.AddCell(retal);
                tblProductos.AddCell(recre);
                tblProductos.AddCell(tota);
                tblProductos.AddCell(acum);
                tamañodatos++;
            }
           
        }
        document.Add(tblProductos);
        return acumulado;
    }

}
