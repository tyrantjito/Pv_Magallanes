using System;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.io;
using System.Diagnostics;
using System.IO;

/// <summary>
/// Descripción breve de TicketPv
/// </summary>
public class TicketPv
{
    BaseDatos ejecuta = new BaseDatos();
    ImprimeTicket detTicket = new ImprimeTicket();
    int _ticket;
    private string _fecha;
    private string _hora;
    int _pv;
    int _caja;
    string _usuario, _forma, _refe, _ban, _encabezado, _notas, _notasTicket,_cliente,_correo,_curp,_requi,_entrada;
    decimal _sub, _tot, _porc_dctoGral, _iva, _porcIva,_montoDecto;
    bool _esVtaCredito;

	public TicketPv()
	{
        _ticket = 0;
        _pv = 0;
        _caja = 0;
        _sub = _tot = _porc_dctoGral = _iva = _porcIva = _montoDecto = 0;
        _usuario = _refe = _forma = _ban = _fecha = _hora = _encabezado = _notas = _notasTicket = string.Empty;
        _esVtaCredito = false;
	}

    public int Ticket{        
        set { _ticket = value; }
        get { return _ticket; }
    }

    public string Curp
    {
        set { _curp = value; }
        get { return _curp; }
    }
    public string Requi
    {
        set { _requi = value; }
        get { return _requi; }
    }
    public string Entrada
    {
        set { _entrada = value; }
        get { return _entrada; }
    }
    public string Cliente
    {
        set { _cliente = value; }
        get { return _cliente; }
    }

    public string Correo
    {
        set { _correo = value; }
        get { return _correo; }
    }

    public int PuntoVenta
    {
        set { _pv = value; }
        get { return _pv; }
    }

    public int Caja
    {
        set { _caja = value; }
        get { return _caja; }
    }

    

    public string GenerarTicket(bool esVtaTaller)
    {

        iTextSharp.text.Font fuenteT = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteBb = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.RED);
        iTextSharp.text.Font fuente = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuenteS = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 5, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuente6 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.LETTER);
        documento.AddTitle("Ticket de Compra");
        documento.AddCreator("E-PuntoVenta");

        string ruta = HttpContext.Current.Server.MapPath("~/Tickets");
        string archivo = ruta + "\\Ticket_" + _pv.ToString() + "_" + _ticket.ToString() + ".pdf";

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
        {
            Directory.CreateDirectory(ruta);
        }


        if (archivo.Trim() != "")
        {
            FileInfo info = new FileInfo(archivo);
            if (info.Exists)
                info.Delete();

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
            logo.Alignment = iTextSharp.text.Image.ALIGN_LEFT;

            PdfPTable enc1 = new PdfPTable(3);
            enc1.DefaultCell.Border = 0;
            enc1.WidthPercentage = 100f;
            int[] enc1cellwidth = { 20, 40, 40 };
            enc1.SetWidths(enc1cellwidth);


            PdfPCell img1 = new PdfPCell(logo);
            img1.Border = 0;
            img1.HorizontalAlignment = Element.ALIGN_CENTER;
            enc1.AddCell(img1);


            PdfPCell enca = new PdfPCell(new Phrase("\n COMERCIALIZADORA \n BURGOS MAGALLANES \n S. DE R.L. DE C.V. \n ", fuente));
            enca.HorizontalAlignment = Element.ALIGN_CENTER;

            enca.Border = 0;
            enc1.AddCell(enca);


            ImprimeTicket imp = new ImprimeTicket();
            imp.ticketc = _ticket;
            object[] camposcab = detTicket.obtieneenca(_ticket);
            DateTime fechav;

            if (Convert.ToBoolean(camposcab[0]))
            {
                DataSet datos = (DataSet)camposcab[1];


                foreach (DataRow fila in datos.Tables[0].Rows)
                {
                    fechav = Convert.ToDateTime(fila[0]);

                    PdfPCell enca2 = new PdfPCell(new Phrase("\n\n REMISION No."+_ticket+"    \n Requisicion : "+_requi.ToUpper()+"\n Entrada : "+_entrada.ToUpper() , fuente6));
                    enca2.HorizontalAlignment = Element.ALIGN_LEFT;
                    enca2.Border = 0;
                    enc1.AddCell(enca2);

                }
            }

            documento.Add(enc1);
            documento.Add(new Paragraph(" "));

            PdfPTable enc2 = new PdfPTable(1);
            enc2.DefaultCell.Border = 0;
            enc2.WidthPercentage = 100f;
            int[] enc2cellwidth = { 50};
            enc2.SetWidths(enc2cellwidth);

            PdfPCell enca3 = new PdfPCell(new Phrase("Cliente:   GOBIERNO DEL DISTRITO FEDERAL / SUBSECRETARIA DEL SISTEMA PENITENCIARIO \n                PLAZAS DE LA CONSTITUCION S/N CENTRO DE LA CIUDAD DE MEXICO C.P. 06000 AREA 1 \n                DISTRITO FEDERAL GDF-971205-4NA", fuente6));
            enca3.HorizontalAlignment = Element.ALIGN_LEFT;
            enca3.Border = 0;
            enc2.AddCell(enca3);

         

            documento.Add(enc2);


            string noOrden = "";
                    string nomTaller = "";
                    string folio = "";
                    string cliente = "";
                    if (esVtaTaller)
                    {
                        object[] datosVtaTaller = detTicket.datosVtaTaller();
                        DataSet valVtaTaller = (DataSet)datosVtaTaller[1];
                        foreach (DataRow f in valVtaTaller.Tables[0].Rows)
                        {
                            folio = f[0].ToString();
                            noOrden = f[1].ToString();
                            nomTaller = f[2].ToString();
                            cliente = f[3].ToString();
                        }
                    }

                    object[] datosTicket = detTicket.datosTicket();
                    if (Convert.ToBoolean(datosTicket[0]))
                    {
                        DataSet valores = (DataSet)datosTicket[1];
                        DateTime fecha;
                        iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 13, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                        foreach (DataRow fila in valores.Tables[0].Rows)
                        {
                            try
                            {
                                fecha = Convert.ToDateTime(fila[0].ToString() + " " + fila[1].ToString());
                                _fecha = fecha.ToString("yyyy-MM-dd");
                                _hora = fecha.ToString("HH:mm:ss");
                            }
                            catch (Exception)
                            {
                                _fecha = fila[0].ToString();
                                _hora = fila[1].ToString().Substring(0, 10);
                            }

                            _usuario = fila[3].ToString();
                            _forma = fila[8].ToString();
                            _refe = fila[9].ToString();
                            _ban = fila[11].ToString();
                            _sub = Convert.ToDecimal(fila[4].ToString());
                            _iva = Convert.ToDecimal(fila[5].ToString());
                            _tot = Convert.ToDecimal(fila[6].ToString());
                            _porc_dctoGral = Convert.ToDecimal(fila[13].ToString()) / 100;
                            _montoDecto = Convert.ToDecimal(fila[14].ToString());
                            _porcIva = Convert.ToDecimal(fila[7].ToString());
                            _notasTicket = fila[12].ToString();
                            try { _esVtaCredito = bool.Parse(fila[15].ToString()); } catch (Exception) { _esVtaCredito = false; }
                            Encabezado(documento);

                            if (esVtaTaller)
                            {
                                Paragraph vtaTall = new Paragraph("Folio: " + folio.Trim() + " - Orden: " + noOrden + " - Cliente: " + cliente + " - Taller: " + nomTaller, fuente);
                                vtaTall.Alignment = Element.ALIGN_CENTER;
                                documento.Add(vtaTall);
                            }

                            Paragraph aten = new Paragraph("Atendio: " + _usuario.ToString(), fuente);
                            aten.Alignment = Element.ALIGN_CENTER;
                            documento.Add(aten);

                            if (_esVtaCredito)
                            {
                                Paragraph vtaCred = new Paragraph("-Venta a Crédito-", fuente);
                                vtaCred.Alignment = Element.ALIGN_CENTER;
                                documento.Add(vtaCred);
                            }

                            Paragraph pag = new Paragraph("Forma Pago: " + _forma.ToString(), fuente);
                            pag.Alignment = Element.ALIGN_CENTER;
                            documento.Add(pag);
                            if (_forma != "Efectivo" && _forma != "")
                            {
                                /*Paragraph refe = new Paragraph("Referencia Pago: " + _refe.ToString(), fuente);
                                refe.Alignment = Element.ALIGN_CENTER;
                                documento.Add(refe);*/
                                Paragraph banc = new Paragraph("Banco: " + _ban.ToString(), fuente);
                                banc.Alignment = Element.ALIGN_CENTER;
                                documento.Add(banc);
                            }
                        }


                        documento.AddCreationDate();
                        documento.Add(new Paragraph(" "));
                        documento.Add(new Paragraph(" "));
                        ProductosTicket(documento);

                        Paragraph lin = new Paragraph("", fuente);
                        lin.Alignment = Element.ALIGN_CENTER;
                        //documento.Add(lin);                
                        Paragraph subtotal = new Paragraph(" ", fuenteBb);
                        subtotal.Alignment = Element.ALIGN_CENTER;

                        Paragraph liva = new Paragraph("", fuenteB);
                        liva.Alignment = Element.ALIGN_CENTER;

                        documento.Add(new Paragraph(" "));
                        documento.Add(new Paragraph(" "));
                        Paragraph ltotal = new Paragraph("", fuenteT);
                        ltotal.Alignment = Element.ALIGN_CENTER;
                        Paragraph dctoGral = new Paragraph("HUEHUETAN MZA. 171 LOTE 14 COL. HEROES DE PADIERNA 14200 TLALPAN DISTRITO FEDERAL RFC. CMB140917L5A", fuenteT);
                        dctoGral.Alignment = Element.ALIGN_CENTER;
                        documento.Add(dctoGral);
                        documento.Add(subtotal);
                        documento.Add(liva);
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
                            foreach (DataRow fila in infoPago.Tables[0].Rows)
                            {

                                Paragraph pagf = new Paragraph("", fuente);
                                pagf.Alignment = Element.ALIGN_CENTER;
                                documento.Add(pagf);
                                /*if (fila[0].ToString() != "E")
                                {
                                    Paragraph refef = new Paragraph( "T.: "+fila[5]+  " Referencia Pago: " + fila[7].ToString(), fuente);
                                    refef.Alignment = Element.ALIGN_CENTER;
                                    documento.Add(refef);                           
                                }*/

                                Paragraph lpago = new Paragraph("", fuenteB);
                                lpago.Alignment = Element.ALIGN_CENTER;
                                documento.Add(lpago);
                                Paragraph lcambio = new Paragraph("", fuente);
                                lcambio.Alignment = Element.ALIGN_CENTER;
                                documento.Add(lcambio);
                            }
                        }

                        documento.Add(new Paragraph(" "));
                        //importe con letra
                        ConvertirNumerosLetras conversion = new ConvertirNumerosLetras();
                        conversion.IMporte = (_tot + _iva).ToString();
                        string textoLetras = conversion.enletras();
                        Paragraph importeLetra = new Paragraph("", fuente);
                        importeLetra.Alignment = Element.ALIGN_CENTER;
                        documento.Add(importeLetra);

                        if (_notasTicket != "")
                        {
                            documento.Add(new Paragraph(" "));
                            Paragraph notTi = new Paragraph("", fuente);
                            notTi.Alignment = Element.ALIGN_CENTER;
                            documento.Add(notTi);
                            documento.Add(new Paragraph(" "));
                            Paragraph notTciket = new Paragraph("", fuenteS);
                            notTciket.Alignment = Element.ALIGN_CENTER;
                            documento.Add(notTciket);
                        }

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
        Paragraph fech = new Paragraph("Fecha: "+ _fecha+"          Hora: "+_hora, fuente);
        fech.Alignment = Element.ALIGN_CENTER;
        document.Add(fech);
        Paragraph caj = new Paragraph("Caja: " + _caja.ToString() + "          Ticket: #" + _ticket, fuente);
        caj.Alignment = Element.ALIGN_CENTER;
        document.Add(caj);
    }

    private void ProductosTicket(Document document)
    {

        iTextSharp.text.Font fuenteT = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenteB = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuenten = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        iTextSharp.text.Font fuenter = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.RED);
        iTextSharp.text.Font fuenteS = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        PdfPTable tblProductos = new PdfPTable(3);

        PdfPCell cllcan = new PdfPCell(new Phrase("CANT", fuenteB));
        cllcan.BorderWidthBottom = 1;
        cllcan.HorizontalAlignment = Element.ALIGN_MIDDLE;
        cllcan.VerticalAlignment = Element.ALIGN_MIDDLE;
        cllcan.BackgroundColor = BaseColor.LIGHT_GRAY;
        cllcan.Padding = 1;
        PdfPCell cldesc = new PdfPCell(new Phrase("UNIDAD MEDIDA", fuenteB));
        cldesc.BorderWidthBottom = 1;
        cldesc.VerticalAlignment = Element.ALIGN_MIDDLE;
        cldesc.HorizontalAlignment = Element.ALIGN_MIDDLE;
        cldesc.VerticalAlignment = 1;
        cldesc.BackgroundColor = BaseColor.LIGHT_GRAY;
        cldesc.Padding = 1;
        PdfPCell clpre = new PdfPCell(new Phrase("DESCRIPCIÓN", fuenteB));
        clpre.BorderWidthBottom = 1;
        clpre.HorizontalAlignment = Element.ALIGN_MIDDLE;
        clpre.VerticalAlignment = Element.ALIGN_MIDDLE;
        clpre.BackgroundColor = BaseColor.LIGHT_GRAY;
        clpre.Padding = 1;
        

       

        tblProductos.AddCell(cllcan);
        tblProductos.AddCell(cldesc);
        tblProductos.AddCell(clpre);


        ImprimeTicket imp = new  ImprimeTicket();

        imp.ticketc = _ticket;


        int tamañodatos= 0;
        int cantidad;
        string medidas;
        string descripcion;
        object[] camposcab = detTicket.obtieneDetalle(_ticket); 
        if (Convert.ToBoolean(camposcab[0]))
        {
            DataSet datos = (DataSet)camposcab[1];
         

            foreach (DataRow fila in datos.Tables[0].Rows)
            {
                cantidad = Convert.ToInt32( fila[0]);
                medidas = Convert.ToString(fila[1]);
                descripcion = Convert.ToString(fila[2]);
               

               
                    PdfPCell canti = new PdfPCell(new Phrase("" + cantidad, fuenteS));
                    canti.HorizontalAlignment = Element.ALIGN_CENTER;
                    canti.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tblProductos.AddCell(canti);

               
                    PdfPCell deta = new PdfPCell(new Phrase("" + medidas, fuenteS));
                    deta.HorizontalAlignment = Element.ALIGN_CENTER;
                    deta.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tblProductos.AddCell(deta);

                    PdfPCell desc = new PdfPCell(new Phrase("" + descripcion, fuenteS));
                    desc.HorizontalAlignment = Element.ALIGN_CENTER;
                    desc.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tblProductos.AddCell(desc);

                  
                }
               

                }
        

       


        document.Add(tblProductos);
        document.Add(new Paragraph(" "));
        document.Add(new Paragraph(" "));
     
    }

}