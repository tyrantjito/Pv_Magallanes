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
/// Descripción breve de ImprimePersonal
/// </summary>
public class ImprimePersonal
{

    string _fecha_ini;
    string _fecha_fin;
    string _usuarioFiltro;
    string _isla;
    string _nombreUsu;
    string _nomIsla;
    string _usuario;
    DataSet _valrores;
    Fechas fechasA = new Fechas();

	public ImprimePersonal()
	{
        _fecha_fin = _fecha_ini = _usuario = _usuarioFiltro = _nombreUsu = _nomIsla = _isla = string.Empty;
        _valrores = new DataSet();
	}

    public string Fecha_Ini { set { _fecha_ini = value; } }
    public string Fecha_Fin { set { _fecha_fin = value; } }
    public string UsuarioLog { set { _usuario = value; } }
    public string Isla { set { _isla = value; } }
    public string UsuarioFiltro { set { _usuarioFiltro = value; } }
    public string NombreIsla { set { _nomIsla = value; } }
    public string NombreUsuario { set { _nombreUsu = value; } }
    public DataSet Datos { set { _valrores = value; } }

    public string GenerarTicket()
    {

        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.A4);
        documento.AddTitle("Reporte de Personal");
        documento.AddCreator("E-PuntoVenta");

        string ruta = HttpContext.Current.Server.MapPath("~/Tickets");
        string archivo = ruta + "\\Personal_" + _fecha_ini.ToString() + "_"
            + _fecha_fin.ToString() + "_" + _usuario + ".pdf";

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

            Paragraph titulo = new Paragraph("REPORTE DE PERSONAL", _standardFont);
            titulo.Alignment = Element.ALIGN_CENTER;
            documento.Add(titulo);

            Paragraph fechas = new Paragraph("DE: " + _fecha_ini + " A " + _fecha_fin, _standardFont);
            fechas.Alignment = Element.ALIGN_CENTER;
            documento.Add(fechas);


            if (_isla != "0")
            {
                Paragraph isla = new Paragraph("TIENDA: " + _nomIsla, _standardFont);
                isla.Alignment = Element.ALIGN_CENTER;
                documento.Add(isla);
            }

            if (_usuarioFiltro != "") {
                Paragraph user = new Paragraph("USUARIO: " + _nombreUsu, _standardFont);
                user.Alignment = Element.ALIGN_CENTER;
                documento.Add(user);
            }


            Paragraph fecha = new Paragraph("Fecha: " + fechasA.obtieneFechaLocal().ToString("yyyy-MM-dd"), _standardFont1);
            fecha.Alignment = Element.ALIGN_RIGHT;
            documento.Add(fecha);

            Paragraph hora = new Paragraph("Hora: " + fechasA.obtieneFechaLocal().ToString("HH:mm:ss"), _standardFont1);
            hora.Alignment = Element.ALIGN_RIGHT;
            documento.Add(hora);

            Paragraph usuario = new Paragraph("Usuario: " + _usuario.ToString().ToUpper(), _standardFont1);
            usuario.Alignment = Element.ALIGN_RIGHT;
            documento.Add(usuario);

            documento.AddCreationDate();

            documento.Add(new Paragraph(" "));
            obtieneFechas(documento);

            
            documento.Close();

        }
        return archivo;
    }

    private void obtieneFechas(Document documento)
    {

        // Tipo de Font que vamos utilizar
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

        string condicion = "";
        if (_isla != "0")
            condicion = condicion.Trim() + " c.id_punto=" + _isla + " and ";
        if (_usuarioFiltro != "")
            condicion = condicion + " c.usuario='" + _usuarioFiltro + "' and ";

        string sql = "select CONVERT(char(10), c.fecha_apertura, 126) as ingreso from cajas c inner join usuarios_PV u on u.usuario = c.usuario inner join catalmacenes g on c.id_punto = g.idAlmacen where " + condicion + " c.fecha_apertura between '" + _fecha_ini + "' and '" + _fecha_fin + "' group by c.fecha_apertura order by c.fecha_apertura ";
        BaseDatos ejecuta = new BaseDatos();
        object[] val = ejecuta.scalarData(sql);
        if (Convert.ToBoolean(val[0]))
        {
            DataSet fechasSql = (DataSet)val[1];
            foreach (DataRow fila in fechasSql.Tables[0].Rows) {
                Paragraph fech = new Paragraph(fila[0].ToString(), fuente1);
                fech.Alignment = Element.ALIGN_LEFT;
                documento.Add(fech);
                obtieneIslas(documento, fila[0].ToString());
            }
        }
    }

    private void obtieneIslas(Document documento, string fecha)
    {
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

        string condicion = "";
        if (_isla != "0")
            condicion = condicion.Trim() + " c.id_punto=" + _isla + " and ";
        if (_usuarioFiltro != "")
            condicion = condicion + " c.usuario='" + _usuarioFiltro + "' and ";

        string sql = "select c.id_punto,g.nombre from cajas c inner join catalmacenes g on c.id_punto = g.idAlmacen where " + condicion + " c.fecha_apertura = '" + fecha + "' group by c.id_punto,g.nombre order by c.id_punto ";
        BaseDatos ejecuta = new BaseDatos();
        object[] val = ejecuta.scalarData(sql);
        if (Convert.ToBoolean(val[0]))
        {
            DataSet islasSql = (DataSet)val[1];
            foreach (DataRow fila in islasSql.Tables[0].Rows)
            {
                Paragraph islasDat = new Paragraph(fila[1].ToString(), fuente1);
                islasDat.Alignment = Element.SECTION;
                documento.Add(islasDat);
                obtieneUsuarios(documento, fecha, fila[0].ToString());
            }
        }
    }

    private void obtieneUsuarios(Document documento, string fecha, string isla)
    {
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                
        string sql = "select Upper(c.usuario) as usuario,upper((u.nombre+' '+u.apellido_pat+' '+isnull(u.apellido_mat,''))) as nombreU from cajas c inner join usuarios_PV u on u.usuario = c.usuario where c.fecha_apertura ='" + fecha + "' and c.id_punto=" + isla + " group by c.usuario,u.nombre,u.apellido_pat,u.apellido_mat order by u.apellido_pat,u.apellido_mat,u.nombre";
        BaseDatos ejecuta = new BaseDatos();
        object[] val = ejecuta.scalarData(sql);
        if (Convert.ToBoolean(val[0]))
        {
            DataSet usuariosSql = (DataSet)val[1];
            foreach (DataRow fila in usuariosSql.Tables[0].Rows)
            {
                Paragraph islasDat = new Paragraph(fila[1].ToString(), fuente1);
                islasDat.Alignment = Element.SECTION;
                documento.Add(islasDat);
                documento.Add(new Paragraph(" "));
                obtieneDatos(documento, fecha, isla, fila[0].ToString());
            }
        }
    }

    private void obtieneDatos(Document documento, string fecha, string isla, string usuario)
    {
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        string sql = "select CONVERT(char(10), c.hora_apertura, 108) as apertura,CONVERT(char(10), isnull(c.fecha_cierre,'1900-01-01'), 126) as fecha_salida,CONVERT(char(10), isnull(c.hora_cierre,'00:00:00'), 126) as salida from cajas c inner join usuarios_PV u on u.usuario = c.usuario inner join catalmacenes g on c.id_punto = g.idAlmacen where c.id_punto = " + isla + " and c.usuario='" + usuario + "' and c.fecha_apertura = '" + fecha + "' group by c.fecha_apertura,c.id_punto,c.usuario,c.hora_apertura,c.fecha_cierre,c.hora_cierre,g.nombre,u.nombre,u.apellido_pat,u.apellido_mat order by c.fecha_apertura ";


        PdfPTable tblProductos = new PdfPTable(3);
        PdfPCell clfe = new PdfPCell(new Phrase("HORA ENTRADA", fuente1));
        clfe.BorderWidthBottom = 1;
        clfe.HorizontalAlignment = 1;
        clfe.VerticalAlignment = 1;
        clfe.Padding = 1;
        PdfPCell clefec = new PdfPCell(new Phrase("FECHA SALIDA", fuente1));
        clefec.BorderWidthBottom = 1;
        clefec.HorizontalAlignment = 1;
        clefec.VerticalAlignment = 1;
        clefec.Padding = 1;
        PdfPCell cldeb = new PdfPCell(new Phrase("HORA SALIDA", fuente1));
        cldeb.BorderWidthBottom = 1;
        cldeb.HorizontalAlignment = 1;
        cldeb.VerticalAlignment = 1;
        cldeb.Padding = 1;

        tblProductos.AddCell(clfe);
        tblProductos.AddCell(clefec);
        tblProductos.AddCell(cldeb);


        BaseDatos ejecuta = new BaseDatos();
        object[] val = ejecuta.scalarData(sql);
        if (Convert.ToBoolean(val[0]))
        {
            DataSet datosSql = (DataSet)val[1];
            foreach (DataRow fila in datosSql.Tables[0].Rows)
            {
                PdfPCell he = new PdfPCell(new Phrase(Convert.ToDateTime(fila[0].ToString()).ToString("HH:mm:ss"), fuente2));
                PdfPCell fs = new PdfPCell(new Phrase(Convert.ToDateTime(fila[1].ToString()).ToString("yyyy-MM-dd"), fuente2));
                PdfPCell hs = new PdfPCell(new Phrase(Convert.ToDateTime(fila[2].ToString()).ToString("HH:mm:ss"), fuente2));

                he.BorderWidth = fs.BorderWidth = hs.BorderWidth = 0;
                he.HorizontalAlignment = fs.HorizontalAlignment = hs.HorizontalAlignment = 1;
                he.VerticalAlignment = fs.VerticalAlignment = hs.VerticalAlignment = 1;
                he.Padding = fs.Padding = hs.Padding = 1;
                tblProductos.AddCell(he);
                tblProductos.AddCell(fs);
                tblProductos.AddCell(hs);
            }
            
        }
        documento.Add(tblProductos);
        documento.Add(new Paragraph(" "));
    }
}