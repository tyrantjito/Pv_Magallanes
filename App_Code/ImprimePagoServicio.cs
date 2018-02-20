using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data;
using E_Utilities;

/// <summary>
/// Descripción breve de ImprimePagoServicio
/// </summary>
public class ImprimePagoServicio
{
    BaseDatos ejecuta = new BaseDatos();
    Fechas fechas = new Fechas();
    int _ticket;
    private string _fecha;
    private string _hora;
    int _pv;
    int _caja;
    string _usuario, _forma, _refe, _ban, _encabezado, _notas, _notasTicket;
    decimal _sub, _tot, _iva, _porcIva;

	public ImprimePagoServicio()
	{
        _ticket = 0;
        _pv = 0;
        _caja = 0;
        _sub = _tot = _iva = _porcIva = 0;
        _usuario = _refe = _forma = _ban = _fecha = _hora = _encabezado = _notas = _notasTicket = string.Empty;
	}

    public int Operacion
    {
        set { _ticket = value; }
    }

    public int PuntoVenta
    {
        set { _pv = value; }
    }

    public int Caja
    {
        set { _caja = value; }
    }

    public string GenerarTicket()
    {

        iTextSharp.text.Font fuenteT = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuenteS = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.B6);
        documento.AddTitle("Ticket de Compra Pago Servicios");
        documento.AddCreator("E-PuntoVenta");

        string ruta = HttpContext.Current.Server.MapPath("~/Tickets");
        string archivo = ruta + "\\PagoServicio_" + _pv.ToString() + "_" + _ticket.ToString() + ".pdf";

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
            string rutaLogo = HttpContext.Current.Server.MapPath("~/img/logo.png");
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(rutaLogo);
            logo.ScaleToFit(100, 50);
            logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            documento.Add(logo);
            documento.Add(new Paragraph(" "));
            PlantillaFormat plantilla = new PlantillaFormat();
            plantilla.Plantilla = 1;
            plantilla.obtieneDatos();
            _encabezado = plantilla.Encabezado;
            _notas = plantilla.Notas;

            RegPagosServ pagosServ = new RegPagosServ();
            pagosServ.punto=_pv;
            pagosServ.caja=_caja;
            pagosServ.operacion=_ticket;
            pagosServ.añop=fechas.obtieneFechaLocal().Year;
            pagosServ.obtieneInformacion();

            decimal comision = 0;
            decimal monto = 0;
            object[] datosTicket = pagosServ.retorno;
            if (Convert.ToBoolean(datosTicket[0]))
            {
                DataSet valores = (DataSet)datosTicket[1];
                DateTime fecha;
                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                foreach (DataRow fila in valores.Tables[0].Rows)
                {
                    try
                    {
                        fecha = Convert.ToDateTime(fila[15].ToString());
                        _fecha = fecha.ToString("yyyy-MM-dd");
                        _hora = fecha.ToString("HH:mm:ss");
                    }
                    catch (Exception)
                    {
                        fecha = fechas.obtieneFechaLocal();
                        _fecha = fecha.ToString("yyyy-MM-dd");
                        _hora = fecha.ToString("HH:mm:ss");
                    }

                    _usuario = fila[18].ToString();                    
                    _tot = Convert.ToDecimal(fila[16].ToString());
                    monto = Convert.ToDecimal(fila[28].ToString());
                    comision = Convert.ToDecimal(fila[29].ToString());
                                        
                    Encabezado(documento);
                    Paragraph aten = new Paragraph("Atendio: " + _usuario.ToString(), fuente);
                    aten.Alignment = Element.ALIGN_CENTER;
                    documento.Add(aten);
                    documento.Add(new Paragraph(" "));
                    Paragraph serv = new Paragraph(Convert.ToString(fila[26]) + " " + fila[27].ToString(), fuenteB);
                    serv.Alignment = Element.ALIGN_CENTER;
                    documento.Add(serv);
                    documento.Add(new Paragraph(" "));                    
                    string dato = "";
                    if (fila[19].ToString().Trim() == "1111111111")
                    {
                        dato = "Referencia: " + fila[20].ToString().Trim();
                    }
                    else if (fila[20].ToString().Trim() == "")
                    {
                        dato = "Teléfono: " + fila[19].ToString().Trim();
                    }
                    else
                        dato = "Teléfono: " + fila[19].ToString() + " Ref: " + fila[20].ToString().Trim();

                    Paragraph tel = new Paragraph(dato, fuenteT);
                    tel.Alignment = Element.ALIGN_CENTER;
                    documento.Add(tel);
                    documento.Add(new Paragraph(" "));
                  
                    Paragraph aut = new Paragraph("Autorización: " + fila[10].ToString().Trim() , fuenteT);
                    aut.Alignment = Element.ALIGN_CENTER;
                    documento.Add(aut);
                    documento.Add(new Paragraph(" "));
                }


                documento.AddCreationDate();
                documento.Add(new Paragraph(" "));
                 /*               
                Paragraph lin = new Paragraph("--------------------------------------------------------", fuente);
                lin.Alignment = Element.ALIGN_CENTER;
               */
                
                if (comision != 0) {
                    Paragraph com = new Paragraph("Comision: " + comision.ToString("C2"), fuenteT);
                    com.Alignment = Element.ALIGN_CENTER;
                    documento.Add(com);
                }


                Paragraph ltotal = new Paragraph("Total: " + _tot.ToString("C2"), fuenteT);
                ltotal.Alignment = Element.ALIGN_CENTER;
                documento.Add(ltotal);


                Pagos pagosTicket = new Pagos();
                pagosTicket.ticket = _ticket;
                pagosTicket.caja = _caja;
                pagosTicket.punto = _pv;
                pagosTicket.obtienePagos();
                object[] datosTpagos = pagosTicket.retorno;
                if (Convert.ToBoolean(datosTpagos[0]))
                {
                    DataSet infoPago = (DataSet)datosTpagos[1];
                    foreach (DataRow fila1 in infoPago.Tables[0].Rows)
                    {
                        Paragraph pagf = new Paragraph("Forma Pago: " + fila1[1].ToString(), fuente);
                        pagf.Alignment = Element.ALIGN_CENTER;
                        documento.Add(pagf);
                        if (fila1[0].ToString() != "E" && fila1[0].ToString() != "")
                        {
                            Paragraph refef = new Paragraph("T.: " + fila1[5] + " Referencia Pago: " + fila1[7].ToString(), fuente);
                            refef.Alignment = Element.ALIGN_CENTER;
                            documento.Add(refef);
                        }

                        Paragraph lpago = new Paragraph("Pago: " + Convert.ToDecimal(fila1[2]).ToString("C2"), fuenteB);
                        lpago.Alignment = Element.ALIGN_CENTER;
                        documento.Add(lpago);
                        Paragraph lcambio = new Paragraph("Cambio: " + Convert.ToDecimal(fila1[3]).ToString("C2"), fuente);
                        lcambio.Alignment = Element.ALIGN_CENTER;
                        documento.Add(lcambio);
                    }
                }
                documento.Add(new Paragraph(" "));                
                //importe con letra
                ConvertirNumerosLetras conversion = new ConvertirNumerosLetras();
                conversion.IMporte = _tot.ToString();
                string textoLetras = conversion.enletras();
                Paragraph importeLetra = new Paragraph(textoLetras, fuente);
                importeLetra.Alignment = Element.ALIGN_CENTER;
                documento.Add(importeLetra);                
                documento.Add(new Paragraph(" "));
                documento.Add(new Paragraph(" "));              
                Paragraph not = new Paragraph(_notas, fuenteS);
                not.Alignment = Element.ALIGN_CENTER;
                documento.Add(not);
            }
            documento.Close();

        }
        return archivo;
    }

    private void Encabezado(Document document)
    {

        // FUENTE
        iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuenteT = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuenteS = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        PdfPTable tblCab = new PdfPTable(1);

        Paragraph plan = new Paragraph(_encabezado, fuente);
        plan.Alignment = Element.ALIGN_CENTER;
        document.Add(plan);

        document.Add(new Paragraph(""));
        document.Add(new Paragraph(""));
        Islas isla = new Islas();
        isla.Almacen = _pv;
        isla.obtieneUbicacion();
        string ubicacion = isla.Ubicacion;
        if (ubicacion == "")
            ubicacion = "Terminal: " + _pv.ToString() + ": Sin Ubicación";
        else
            ubicacion = "Terminal: " + _pv.ToString() + ": " + ubicacion;

        Paragraph ubica = new Paragraph(ubicacion, fuente);
        ubica.Alignment = Element.ALIGN_CENTER;
        document.Add(ubica);
        Paragraph fech = new Paragraph("Fecha: " + _fecha + "          Hora: " + _hora, fuente);
        fech.Alignment = Element.ALIGN_CENTER;
        document.Add(fech);
        Paragraph caj = new Paragraph("Caja: " + _caja.ToString() + "          Operacion: #" + _ticket, fuente);
        caj.Alignment = Element.ALIGN_CENTER;
        document.Add(caj);
    }

    private void ProductosTicket(Document document)
    {
        /*
        iTextSharp.text.Font fuenteT = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuenteS = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        PdfPTable tblProductos = new PdfPTable(4);
        PdfPCell cllcan = new PdfPCell(new Phrase("CANT", fuenteB));
        cllcan.BorderWidthBottom = 1;
        cllcan.HorizontalAlignment = 1;
        cllcan.VerticalAlignment = 1;
        cllcan.Padding = 1;
        PdfPCell cldesc = new PdfPCell(new Phrase("DESCRIPCIÓN", fuenteB));
        cldesc.BorderWidthBottom = 1;
        cldesc.HorizontalAlignment = 1;
        cldesc.VerticalAlignment = 1;
        cldesc.Padding = 1;
        PdfPCell clpre = new PdfPCell(new Phrase("PRECIO", fuenteB));
        clpre.BorderWidthBottom = 1;
        clpre.HorizontalAlignment = 1;
        clpre.VerticalAlignment = 1;
        clpre.Padding = 1;
        PdfPCell cltotal = new PdfPCell(new Phrase("TOTAL", fuenteB));
        cltotal.BorderWidthBottom = 1;
        cltotal.HorizontalAlignment = 1;
        cltotal.VerticalAlignment = 1;
        cltotal.Padding = 1;

        tblProductos.AddCell(cllcan);
        tblProductos.AddCell(cldesc);
        tblProductos.AddCell(clpre);
        tblProductos.AddCell(cltotal);

        int tamañodatos = 0;
        object[] camposcab = detTicket.obtieneDetalle();
        if (Convert.ToBoolean(camposcab[0]))
        {
            DataSet datos = (DataSet)camposcab[1];
            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                PdfPCell cant = new PdfPCell(new Phrase(fila[0].ToString(), fuenteS));
                PdfPCell desc = new PdfPCell(new Phrase(fila[1].ToString(), fuenteS));
                PdfPCell prec = new PdfPCell(new Phrase(fila[2].ToString(), fuenteS));
                PdfPCell tot = new PdfPCell(new Phrase(fila[3].ToString(), fuenteS));
                cant.BorderWidth = 0;
                cant.HorizontalAlignment = 1;
                cant.VerticalAlignment = 1;
                cant.Padding = 1;
                desc.BorderWidth = 0;
                desc.HorizontalAlignment = 1;
                desc.VerticalAlignment = 1;
                desc.Padding = 1;
                prec.BorderWidth = 0;
                prec.HorizontalAlignment = 1;
                prec.VerticalAlignment = 1;
                prec.Padding = 1;
                tot.BorderWidth = 0;
                tot.HorizontalAlignment = 1;
                tot.VerticalAlignment = 1;
                tot.Padding = 1;
                tblProductos.AddCell(cant);
                tblProductos.AddCell(desc);
                tblProductos.AddCell(prec);
                tblProductos.AddCell(tot);
                tamañodatos++;
            }
        }
        document.Add(tblProductos);
        */
    }
}