using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using System.Data;
using E_Utilities;

/// <summary>
/// Descripción breve de ImprimeEnvio
/// </summary>
public class ImprimeEnvio
{
     string nom_isla;
    string fecha_ped;
    string usuario;
    string id_isla;
    Fechas fechasA = new Fechas();

	public ImprimeEnvio()
	{
        nom_isla = fecha_ped = usuario = id_isla = string.Empty;
        
	}

    public string Nom_isla { set { nom_isla = value; } }
    public string fecha_Ped { set { fecha_ped = value; } }
    public string Usuario { set { usuario = value;  } }
    public string Id_isla { set { id_isla = value; } }



    public string GenerarTicket()
    {

        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.A4);
        documento.AddTitle("Envio de Tienda");
        documento.AddCreator("E-PuntoVenta");

        string ruta = HttpContext.Current.Server.MapPath("~/Tickets");
        nom_isla = nom_isla.Replace(' ', '_');
        string archivo = ruta + "\\Envio_Tienda_" + nom_isla.ToString() + "_" + fecha_ped.ToString() + ".pdf";

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

            Paragraph titulo = new Paragraph("ENVIO TIENDA", _standardFont);
            titulo.Alignment = Element.ALIGN_CENTER;
            documento.Add(titulo);
            
            Paragraph nombre = new Paragraph("NOMBRE DE LA TIENDA: "+ nom_isla, _standardFont);
            nombre.Alignment = Element.ALIGN_CENTER;
            documento.Add(nombre);

            Paragraph fechas = new Paragraph("FECHA: " + fecha_ped , _standardFont);
            fechas.Alignment = Element.ALIGN_CENTER;
            documento.Add(fechas);

            Paragraph fecha = new Paragraph("Fecha: " + fechasA.obtieneFechaLocal().ToString("yyyy-MM-dd"), _standardFont1);
            fecha.Alignment = Element.ALIGN_RIGHT;
            documento.Add(fecha);

            Paragraph hora = new Paragraph("Hora: " + fechasA.obtieneFechaLocal().ToString("HH:mm:ss"), _standardFont1);
            hora.Alignment = Element.ALIGN_RIGHT;
            documento.Add(hora);

            Paragraph usuariop = new Paragraph("Usuario: " + usuario.ToString().ToUpper(), _standardFont1);
            usuariop.Alignment = Element.ALIGN_RIGHT;
            documento.Add(usuariop);

            documento.AddCreationDate();
            
            documento.Add(new Paragraph(" "));            
            decimal cantid = ProductosTicket(documento);

            documento.Add(new Paragraph("-----------------------------------------------------------------------------------------------------------------------"));

            Paragraph ltotal = new Paragraph("Total: " + cantid.ToString(), _standardFont);
            ltotal.Alignment = Element.ALIGN_RIGHT;
            documento.Add(ltotal);

            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));
            Paragraph lfirm = new Paragraph("________________________________________________");
            lfirm.Alignment = Element.ALIGN_CENTER;
            documento.Add(lfirm);
            Paragraph lfirm1 = new Paragraph("Firma y Nombre");
            lfirm1.Alignment = Element.ALIGN_CENTER;
            documento.Add(lfirm1);

            documento.Close();

        }
        return archivo;
    }

    private decimal ProductosTicket(Document document)
    {

        // Tipo de Font que vamos utilizar
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        PdfPTable tblProductos = new PdfPTable(4);
        PdfPCell clcla = new PdfPCell(new Phrase("CLAVE", fuente1));
        clcla.BorderWidthBottom = 1;
        clcla.HorizontalAlignment = 1;
        clcla.VerticalAlignment = 1;
        clcla.Padding = 1;
        PdfPCell clpro = new PdfPCell(new Phrase("PRODUCTO", fuente1));
        clpro.BorderWidthBottom = 1;
        clpro.HorizontalAlignment = 1;
        clpro.VerticalAlignment = 1;
        clpro.Padding = 1;
        PdfPCell clcan = new PdfPCell(new Phrase("CANTIDAD", fuente1));
        clcan.BorderWidthBottom = 1;
        clcan.HorizontalAlignment = 1;
        clcan.VerticalAlignment = 1;
        clcan.Padding = 1;
        PdfPCell clcuadrito = new PdfPCell(new Phrase("REVISIÓN", fuente1));
        clcuadrito.BorderWidthBottom = 1;
        clcuadrito.HorizontalAlignment = 1;
        clcuadrito.VerticalAlignment = 1;
        clcuadrito.Padding = 1;
                
        tblProductos.AddCell(clcla);
        tblProductos.AddCell(clpro);
        tblProductos.AddCell(clcan);
        tblProductos.AddCell(clcuadrito);
        

        int tamañodatos = 0;
        BaseDatos data = new BaseDatos();
        string sql = string.Format("select d.entProductoID,c.descripcion,cast(isnull(sum(d.entProdCant),0) as varchar(50)) as cantidad from entinventariodet d left join catproductos c on c.idProducto=d.entProductoID where d.entAlmacenID={0} and d.entFolioID in (select e.entFolioID from entinventarioenc e where e.entAlmacenID=d.entAlmacenID and CONVERT(CHAR(10),e.entFechaDoc,126)='{1}') group by d.entProductoID,c.descripcion", id_isla, fecha_ped);
        string lineas = "|___|      Notas:______________________________________________";
        object[] camposcab = data.scalarData(sql);
        decimal cantid = 0;
        if (Convert.ToBoolean(camposcab[0]))
        {
            DataSet datos = (DataSet)camposcab[1];
           
            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                PdfPCell cla = new PdfPCell(new Phrase(fila[0].ToString(), fuente2));
                PdfPCell prod = new PdfPCell(new Phrase(fila[1].ToString(), fuente2));
                PdfPCell canti = new PdfPCell(new Phrase(fila[2].ToString(), fuente2));
                cantid = cantid + Convert.ToDecimal(fila[2].ToString());
                PdfPCell cuadrito = new PdfPCell(new Phrase(lineas, fuente2));
               
                cla.BorderWidth = 0;
                cla.HorizontalAlignment = 1;
                cla.VerticalAlignment = 1;
                cla.Padding = 1;
                prod.BorderWidth = 0;
                prod.HorizontalAlignment = 1;
                prod.VerticalAlignment = 1;
                prod.Padding = 1;
                canti.BorderWidth = 0;
                canti.HorizontalAlignment = 1;
                canti.VerticalAlignment = 1;
                canti.Padding = 1;                
                cuadrito.BorderWidth = 0;
                cuadrito.HorizontalAlignment = 1;
                cuadrito.VerticalAlignment = 1;
                cuadrito.Padding = 1;

                tblProductos.AddCell(cla);
                tblProductos.AddCell(prod);
                tblProductos.AddCell(canti);
                tblProductos.AddCell(cuadrito);

                tamañodatos++;
            }
           
        }
        document.Add(tblProductos);
        return cantid;
    }

}