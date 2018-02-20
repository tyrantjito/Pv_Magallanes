using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de ImprimeCierres
/// </summary>
public class ImprimeCierres
{
    string fecha_ini;
    string fecha_fin;
    string _usuario;
    string _usuSelec;
    string _isla;
    string _nomIsla;
    Fechas fechasA = new Fechas();

    int tot;

	public ImprimeCierres()
	{
        fecha_fin = fecha_ini = _usuario = string.Empty;
	}

    public string fecha_Ini { set { fecha_ini = value; } }
    public string fecha_Fin { set { fecha_fin = value; } }
    public string Usuario { set { _usuario = value; } }
    public string UsuSelec { set { _usuSelec = value; } }
    public string Isla { set { _isla = value; } }
    public string NombreIsla { set { _nomIsla = value; } }

    public string GenerarTicket()
    {
        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.A4);
        documento.AddTitle("Cierres Diarios");
        documento.AddCreator("E-PuntoVenta");

        string ruta = HttpContext.Current.Server.MapPath("~/Tickets");
        string archivo = ruta + "\\Cierre_" + fecha_ini.ToString() + "_"
            + fecha_fin.ToString() + "_" + _usuario + ".pdf";

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

            Paragraph titulo = new Paragraph("CIERRES DIARIOS", _standardFont);
            titulo.Alignment = Element.ALIGN_CENTER;
            documento.Add(titulo);

            if (_isla != "T")
            {
                Paragraph isla = new Paragraph("TIENDA: " + _nomIsla, _standardFont);
                isla.Alignment = Element.ALIGN_CENTER;
                documento.Add(isla);
            }

            if (_usuSelec != "T")
            {
                Paragraph user = new Paragraph("USUARIO(S): " + _usuSelec, _standardFont);
                user.Alignment = Element.ALIGN_CENTER;
                documento.Add(user);
            }


            Paragraph fechas = new Paragraph("DE " + fecha_ini + " A " + fecha_fin, _standardFont);
            fechas.Alignment = Element.ALIGN_CENTER;
            documento.Add(fechas);            

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
            decimal acumulado = GeneraReporte(documento);

            documento.Close();

        }
        return archivo;
    }

    private decimal GeneraReporte(Document document)
    {

        // Tipo de Font que vamos utilizar
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        PdfPTable tblCierres = new PdfPTable(11);
        tblCierres.WidthPercentage = 100f;
        PdfPCell clfe = new PdfPCell(new Phrase("FECHA", fuente1));
        clfe.BorderWidthBottom = 1;
        clfe.HorizontalAlignment = 1;
        clfe.VerticalAlignment = 1;
        clfe.Padding = 1;
        PdfPCell clIsla = new PdfPCell(new Phrase("TIENDA", fuente1));
        clIsla.BorderWidthBottom = 1;
        clIsla.HorizontalAlignment = 1;
        clIsla.VerticalAlignment = 1;
        clIsla.Padding = 1;
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
        clcred.BorderWidthBottom = 1;
        clcred.HorizontalAlignment = 1;
        clcred.VerticalAlignment = 1;
        clcred.Padding = 1;
        PdfPCell clrecar = new PdfPCell(new Phrase("RECARGAS", fuente1));
        clcred.BorderWidthBottom = 1;
        clcred.HorizontalAlignment = 1;
        clcred.VerticalAlignment = 1;
        clcred.Padding = 1;

        PdfPCell clgas = new PdfPCell(new Phrase("GASTOS", fuente1));
        clgas.BorderWidthBottom = 1;
        clgas.HorizontalAlignment = 1;
        clgas.VerticalAlignment = 1;
        clgas.Padding = 1;
        PdfPCell clcan = new PdfPCell(new Phrase("CANCELACION", fuente1));
        clcan.BorderWidthBottom = 1;
        clcan.HorizontalAlignment = 1;
        clcan.VerticalAlignment = 1;
        clcan.Padding = 1;
        PdfPCell clfon = new PdfPCell(new Phrase("FONDO", fuente1));
        clfon.BorderWidthBottom = 1;
        clfon.HorizontalAlignment = 1;
        clfon.VerticalAlignment = 1;
        clfon.Padding = 1;
        PdfPCell cltotal = new PdfPCell(new Phrase("TOTAL", fuente1));
        cltotal.BorderWidthBottom = 1;
        cltotal.HorizontalAlignment = 1;
        cltotal.VerticalAlignment = 1;
        cltotal.Padding = 1;

        tblCierres.AddCell(clfe);
        tblCierres.AddCell(clIsla);
        tblCierres.AddCell(clefec);
        tblCierres.AddCell(cldeb);
        tblCierres.AddCell(clcred);

        tblCierres.AddCell(clpagserv);
        tblCierres.AddCell(clrecar);

        tblCierres.AddCell(clgas);
        tblCierres.AddCell(clcan);
        tblCierres.AddCell(clfon);
        tblCierres.AddCell(cltotal);

        int tamañodatos = 0;
        BaseDatos data = new BaseDatos();

        string sqlCierres = string.Format("SELECT CONVERT(char(10), c.fecha_cierre, 126) AS fecha, c.efectivo, c.debito, c.credito, c.gastos, c.fondo, c.total, g.nombre, c.id_punto_venta, c.usuario_cierre,c.id_cierre, c.cancelaciones,c.pagoservicios,c.recargas " +
                    "FROM cierres_diarios AS c " +
                    "INNER JOIN usuarios_PV AS u ON u.usuario = c.usuario_cierre " +
                    "INNER JOIN catalmacenes AS g ON c.id_punto_venta = g.idAlmacen " +
                    "WHERE (fecha_cierre BETWEEN '{0}' AND '{1}')", fecha_ini, fecha_fin);
        if (_usuSelec != "")
        {
            sqlCierres = string.Format(sqlCierres + " AND usuario_cierre IN({0})", _usuSelec);
        }

        sqlCierres = sqlCierres + " ORDER BY fecha_cierre";
                
        object[] camposcab = data.scalarData(sqlCierres);
        decimal acumulado = 0;
        if (Convert.ToBoolean(camposcab[0]))
        {
            DataSet datos = (DataSet)camposcab[1];

            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                PdfPCell fecha = new PdfPCell(new Phrase(fila[0].ToString(), fuente2));
                PdfPCell isla = new PdfPCell(new Phrase(fila[7].ToString(), fuente2));
                PdfPCell efect = new PdfPCell(new Phrase(Convert.ToDecimal(fila[1].ToString()).ToString("C2"), fuente2));
                PdfPCell debi = new PdfPCell(new Phrase(Convert.ToDecimal(fila[2].ToString()).ToString("C2"), fuente2));
                PdfPCell cred = new PdfPCell(new Phrase(Convert.ToDecimal(fila[3].ToString()).ToString("C2"), fuente2));
                PdfPCell gast = new PdfPCell(new Phrase(Convert.ToDecimal(fila[4].ToString()).ToString("C2"), fuente2));
                PdfPCell canc = new PdfPCell(new Phrase(Convert.ToDecimal(fila[11].ToString()).ToString("C2"), fuente2));
                PdfPCell recar = new PdfPCell(new Phrase(Convert.ToDecimal(fila[12].ToString()).ToString("C2"), fuente2));
                PdfPCell pagServ = new PdfPCell(new Phrase(Convert.ToDecimal(fila[13].ToString()).ToString("C2"), fuente2));
                decimal fodos = 0;
                try { fodos = Convert.ToDecimal(fila[5].ToString()); }
                catch (Exception) { fodos = 0; }
                PdfPCell fond = new PdfPCell(new Phrase(fodos.ToString("C2"), fuente2));
                PdfPCell tota = new PdfPCell(new Phrase(Convert.ToDecimal(fila[6].ToString()).ToString("C2"), fuente2));

                fecha.BorderWidth = 0;
                fecha.HorizontalAlignment = 1;
                fecha.VerticalAlignment = 1;
                fecha.Padding = 1;
                isla.BorderWidth = 0;
                isla.HorizontalAlignment = 1;
                isla.VerticalAlignment = 1;
                isla.Padding = 1;
                efect.BorderWidth = 0;
                efect.HorizontalAlignment = 1;
                efect.VerticalAlignment = 1;
                efect.Padding = 1;
                debi.BorderWidth = 0;
                debi.HorizontalAlignment = 1;
                debi.VerticalAlignment = 1;
                debi.Padding = 1;
                cred.BorderWidth = 0;
                cred.HorizontalAlignment = 1;
                cred.VerticalAlignment = 1;
                cred.Padding = 1;
                gast.BorderWidth = 0;
                gast.HorizontalAlignment = 1;
                gast.VerticalAlignment = 1;
                gast.Padding = 1;
                canc.BorderWidth = 0;
                canc.HorizontalAlignment = 1;
                canc.VerticalAlignment = 1;
                canc.Padding = 1;

                pagServ.BorderWidth = 0;
                pagServ.HorizontalAlignment = 1;
                pagServ.VerticalAlignment = 1;
                pagServ.Padding = 1;

                recar.BorderWidth = 0;
                recar.HorizontalAlignment = 1;
                recar.VerticalAlignment = 1;
                recar.Padding = 1;

                fond.BorderWidth = 0;
                fond.HorizontalAlignment = 1;
                fond.VerticalAlignment = 1;
                fond.Padding = 1;
                tota.BorderWidth = 0;
                tota.HorizontalAlignment = 1;
                tota.VerticalAlignment = 1;
                tota.Padding = 1;

                tblCierres.AddCell(fecha);
                tblCierres.AddCell(isla);
                tblCierres.AddCell(efect);
                tblCierres.AddCell(debi);
                tblCierres.AddCell(cred);

                tblCierres.AddCell(pagServ);
                tblCierres.AddCell(recar);

                tblCierres.AddCell(gast);
                tblCierres.AddCell(canc);
                tblCierres.AddCell(fond);
                tblCierres.AddCell(tota);
                
                document.Add(tblCierres);
                document.Add(new Paragraph(" "));
                obtieneUsuarios(document, fila[0].ToString(), fila[8].ToString(), Convert.ToInt32(fila[10].ToString()));
                tblCierres.DeleteLastRow();

                tamañodatos++;
            }
        }
        
        return acumulado;
    }
    
    private void obtieneUsuarios(Document documento, string fecha, string isla, int cierre)
    {
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

        string sql = "SELECT Upper(c.usuario) AS usuario,upper((u.nombre+' '+u.apellido_pat+' '+isnull(u.apellido_mat,''))) AS nombreU, c.id_cierre "  +
                    "FROM cajas AS c INNER JOIN usuarios_PV u on u.usuario = c.usuario "+
                    "WHERE c.id_cierre =" + cierre.ToString() + " and c.id_punto=" + isla +
                    " GROUP BY c.usuario,u.nombre,u.apellido_pat,u.apellido_mat,c.id_cierre " +
                    "ORDER BY u.apellido_pat,u.apellido_mat,u.nombre";
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
                obtieneDatos(documento, fecha, isla, fila[0].ToString(), fila[2].ToString());
            }
        }
    }

    private void obtieneDatos(Document documento, string fecha, string isla, string usuario, string cierre)
    {
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        string sql = "SELECT efectivo, t_debito, t_credito, t_gastos, saldo_corte, t_cancelacion,recargas,pagoservicios " +
                    "FROM cajas c INNER JOIN usuarios_PV u on u.usuario = c.usuario INNER JOIN catalmacenes g on c.id_punto = g.idAlmacen "+
                    "WHERE c.id_punto = " + isla + " and c.usuario='" + usuario + "' and c.id_cierre = " + cierre + " "+
                    "GROUP BY c.fecha_apertura,c.id_punto,c.usuario, c.efectivo, c.t_credito, c.t_debito, c.t_gastos, c.saldo_corte, c.id_caja, c.t_cancelacion,c.recargas,c.pagoservicios " +
                    "ORDER BY c.fecha_apertura";

        PdfPTable tblProductos = new PdfPTable(8);
        tblProductos.WidthPercentage = 100f;
        PdfPCell clefec = new PdfPCell(new Phrase("EFECTIVO", fuente1));
        clefec.BorderWidthBottom = 1;
        clefec.HorizontalAlignment = 1;
        clefec.VerticalAlignment = 1;
        clefec.Padding = 1;
        PdfPCell cldebi = new PdfPCell(new Phrase("DÉBITO", fuente1));
        cldebi.BorderWidthBottom = 1;
        cldebi.HorizontalAlignment = 1;
        cldebi.VerticalAlignment = 1;
        cldebi.Padding = 1;
        PdfPCell clcred = new PdfPCell(new Phrase("CRÉDITO", fuente1));
        clcred.BorderWidthBottom = 1;
        clcred.HorizontalAlignment = 1;
        clcred.VerticalAlignment = 1;
        clcred.Padding = 1;

        PdfPCell clpagserv = new PdfPCell(new Phrase("PAGO SERVICIOS", fuente1));
        clcred.BorderWidthBottom = 1;
        clcred.HorizontalAlignment = 1;
        clcred.VerticalAlignment = 1;
        clcred.Padding = 1;
        PdfPCell clrecar = new PdfPCell(new Phrase("RECARGAS", fuente1));
        clcred.BorderWidthBottom = 1;
        clcred.HorizontalAlignment = 1;
        clcred.VerticalAlignment = 1;
        clcred.Padding = 1;

        PdfPCell clGast = new PdfPCell(new Phrase("GASTOS", fuente1));
        clGast.BorderWidthBottom = 1;
        clGast.HorizontalAlignment = 1;
        clGast.VerticalAlignment = 1;
        clGast.Padding = 1;
        PdfPCell clcAN = new PdfPCell(new Phrase("CANCELACION", fuente1));
        clcAN.BorderWidthBottom = 1;
        clcAN.HorizontalAlignment = 1;
        clcAN.VerticalAlignment = 1;
        clcAN.Padding = 1;
        PdfPCell clTot = new PdfPCell(new Phrase("TOTAL", fuente1));
        clTot.BorderWidthBottom = 1;
        clTot.HorizontalAlignment = 1;
        clTot.VerticalAlignment = 1;
        clTot.Padding = 1;

        tblProductos.AddCell(clefec);
        tblProductos.AddCell(cldebi);
        tblProductos.AddCell(clcred);

        tblProductos.AddCell(clpagserv);
        tblProductos.AddCell(clrecar);

        tblProductos.AddCell(clGast);
        tblProductos.AddCell(clcAN);
        tblProductos.AddCell(clTot);

        BaseDatos ejecuta = new BaseDatos();
        object[] val = ejecuta.scalarData(sql);
        if (Convert.ToBoolean(val[0]))
        {
            DataSet datosSql = (DataSet)val[1];
            foreach (DataRow fila in datosSql.Tables[0].Rows)
            {
                PdfPCell efe = new PdfPCell(new Phrase(Convert.ToDecimal(fila[0].ToString()).ToString("C2"), fuente2));
                PdfPCell deb = new PdfPCell(new Phrase(Convert.ToDecimal(fila[1].ToString()).ToString("C2"), fuente2));
                PdfPCell cre = new PdfPCell(new Phrase(Convert.ToDecimal(fila[2].ToString()).ToString("C2"), fuente2));

                PdfPCell serv = new PdfPCell(new Phrase(Convert.ToDecimal(fila[7].ToString()).ToString("C2"), fuente2));
                PdfPCell rec = new PdfPCell(new Phrase(Convert.ToDecimal(fila[6].ToString()).ToString("C2"), fuente2));

                PdfPCell gas = new PdfPCell(new Phrase(Convert.ToDecimal(fila[3].ToString()).ToString("C2"), fuente2));
                PdfPCell can = new PdfPCell(new Phrase(Convert.ToDecimal(fila[5].ToString()).ToString("C2"), fuente2));
                PdfPCell tot = new PdfPCell(new Phrase(Convert.ToDecimal(fila[4].ToString()).ToString("C2"), fuente2));

                efe.BorderWidth = serv.BorderWidth = rec.BorderWidth = deb.BorderWidth = cre.BorderWidth = gas.BorderWidth = tot.BorderWidth = can.BorderWidth = 0;
                efe.HorizontalAlignment = serv.HorizontalAlignment = rec.HorizontalAlignment = deb.HorizontalAlignment = cre.HorizontalAlignment = gas.HorizontalAlignment = tot.HorizontalAlignment = can.HorizontalAlignment = 1;
                efe.VerticalAlignment = serv.VerticalAlignment = rec.VerticalAlignment = deb.VerticalAlignment = cre.VerticalAlignment = gas.VerticalAlignment = tot.VerticalAlignment = can.VerticalAlignment = 1;
                efe.Padding = serv.Padding = rec.Padding = deb.Padding = cre.Padding = gas.Padding = tot.Padding = can.Padding = 1 ;
                tblProductos.AddCell(efe);
                tblProductos.AddCell(deb);
                tblProductos.AddCell(cre);

                tblProductos.AddCell(serv);
                tblProductos.AddCell(rec);

                tblProductos.AddCell(gas);
                tblProductos.AddCell(can);
                tblProductos.AddCell(tot);
            }
        }
        documento.Add(tblProductos);
        documento.Add(new Paragraph(" "));        
    }
}