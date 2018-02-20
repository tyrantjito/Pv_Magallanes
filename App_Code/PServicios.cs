using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Xml;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using System.Threading;
using E_Utilities;
using System.Data;

/// <summary>
/// Descripción breve de PServicios
/// </summary>
public class PServicios
{
    Fechas fechas = new Fechas();
    string servidor;
    string puerto;
    string distribuidor;
    string dispositivo;
    string password;
    public object[] _retorno { get; set; }
    public int _peticion { get; set; }
    public object[] _datos { get; set; }
    public int tipoFront { get; set; }
    string datosPost;
    string valorXML;
    public string _codigo { get; set; }
    public int _punto { get; set; }
    public int _caja { get; set; }
    public string _usuario { get; set; }
    public int operacion;

    public string telefono { get; set; }
    public string referencia_in { get; set; }
    public int catServicio { get; set; }
    public int tipoFont { get; set; }
    public int idServicio { get; set; }
    public int idProducto { get; set; }
    public decimal montoPagar { get; set; }

    public bool esRecarga { get; set; }

    RegPagosServ registroPagos = new RegPagosServ();

    public PServicios()
    {
        _retorno = new object[] { false, "" };
        _peticion = 0;
        _datos = null;
        datosPost = "";
        valorXML = string.Empty;
        tipoFront = 0;
        operacion = 0;
        telefono = referencia_in = "";
        catServicio = tipoFont = idServicio = idProducto = 0;
        montoPagar = 0;
        _punto = _caja = 0;
        _usuario = string.Empty;
        _codigo = string.Empty;
        esRecarga = false;
    }

    public void obtienePagoServicios()
    {
        RegPagosServ registroPagos = new RegPagosServ();
        switch (_peticion)
        {
            case 1:
                // getListaProducto
                obtieneParametros();
                if (servidor == "" || distribuidor == "" || dispositivo == "" || password == "") { _retorno[0] = false; _retorno[1] = _retorno[1]; _codigo = "0"; }
                else
                {
                    obtieneListaProductos();
                    conexion();
                    if (valorXML != null || valorXML != "")
                        lecturaXmlsProductos(valorXML);
                }

                break;
            case 2:
                // abonar
                obtieneParametros();
                if (servidor == "" || distribuidor == "" || dispositivo == "" || password == "") { _retorno[0] = false; _retorno[1] = _retorno[1]; _codigo = "0"; }
                else
                {
                    gerneraAbono();
                    registroPagos.añop = fechas.obtieneFechaLocal().Year;
                    registroPagos.punto = _punto;
                    registroPagos.caja = _caja;
                    registroPagos.usuario = _usuario;
                    registroPagos.telefono = telefono;
                    registroPagos.referencia_in = referencia_in;
                    registroPagos.catServicio = catServicio;
                    registroPagos.tipoFont = tipoFont;
                    registroPagos.idServicio = idServicio;
                    registroPagos.idProducto = idProducto;
                    registroPagos.montoPagar = montoPagar;
                    if (esRecarga)
                        registroPagos.esRecarga = 1;
                    else
                        registroPagos.esRecarga = 0;
                    registroPagos.generaOperacion();
                    object[] idOperacion = registroPagos.retorno;
                    try
                    {
                        if (Convert.ToBoolean(idOperacion[0]))
                            operacion = Convert.ToInt32(idOperacion[1]);
                        else
                            operacion = 0;
                    }
                    catch (Exception) { operacion = 0; }

                    conexion();
                    if (valorXML != null || valorXML != "")
                        lecturaXmlAbono(valorXML);
                }
                break;
            case 3:
                //confirmaTransaccion
                obtieneParametros();
                if (servidor == "" || distribuidor == "" || dispositivo == "" || password == "") { _retorno[0] = false; _retorno[1] = _retorno[1]; _codigo = "0"; }
                else
                {
                    confirmaTransaccion();
                    registroPagos.añop = fechas.obtieneFechaLocal().Year;
                    registroPagos.punto = _punto;
                    registroPagos.caja = _caja;
                    registroPagos.usuario = _usuario;
                    registroPagos.telefono = telefono;
                    registroPagos.referencia_in = referencia_in;
                    registroPagos.catServicio = catServicio;
                    registroPagos.tipoFont = tipoFont;
                    registroPagos.idServicio = idServicio;
                    registroPagos.idProducto = idProducto;
                    registroPagos.montoPagar = montoPagar;
                    if (esRecarga)
                        registroPagos.esRecarga = 1;
                    else
                        registroPagos.esRecarga = 0;
                    registroPagos.generaOperacion();
                    object[] idOperacion = registroPagos.retorno;
                    try
                    {
                        if (Convert.ToBoolean(idOperacion[0]))
                            operacion = Convert.ToInt32(idOperacion[1]);
                        else
                            operacion = 0;
                    }
                    catch (Exception) { operacion = 0; }
                    conexion();
                    if (valorXML != null || valorXML != "")
                        lecturaXmlConfirmacion(valorXML);
                }
                break;
            case 4:
                //identifyMe
                break;
            case 0:
                //Sin peticion
                break;
            default:
                break;
        }
    }

    private void lecturaXmlConfirmacion(string xml)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);

        string codigo, mensaje;
        string[] respuesta = new string[1] { "0" };
        string[] respuestaHijos = new string[2] { "", "" };

        foreach (XmlNode nodos in doc.DocumentElement.ChildNodes)
        {
            bool tieneHijos = false;
            string nodosHijo = "#";
            try
            {
                nodosHijo = nodos.ChildNodes[0].Name;
                if (nodosHijo.Substring(0, 1) != "#")
                    tieneHijos = true;
                else
                    tieneHijos = false;
            }
            catch (Exception) { tieneHijos = false; }
            if (tieneHijos)
            {
                foreach (XmlNode nodoHijo in nodos.ChildNodes)
                {
                    switch (nodoHijo.Name)
                    {
                        case "CODIGO":
                            respuestaHijos[0] = nodoHijo.InnerText;
                            break;
                        case "TEXTO":
                            respuestaHijos[1] = nodoHijo.InnerText;
                            break;
                        default: break;
                    }
                }
            }
            else
            {
                switch (nodos.Name)
                {
                    case "NUM_AUTORIZACION":
                        respuesta[0] = nodos.InnerText;
                        break;
                    default: break;
                }
            }
        }


        registroPagos.añop = fechas.obtieneFechaLocal().Year;
        registroPagos.punto = _punto;
        registroPagos.caja = _caja;
        registroPagos.usuario = _usuario;
        registroPagos.telefono = telefono;
        registroPagos.referencia_in = referencia_in;
        registroPagos.catServicio = catServicio;
        registroPagos.tipoFont = tipoFont;
        registroPagos.idServicio = idServicio;
        registroPagos.idProducto = idProducto;
        registroPagos.montoPagar = montoPagar;
        registroPagos.operacion = operacion;
        registroPagos.codigo = respuestaHijos[0];
        registroPagos.texto = respuestaHijos[1];
        registroPagos.num_autorizacion = respuesta[0];
        registroPagos.xml = xml;
        registroPagos.actualizaOperacion();


        codigo = respuestaHijos[0];
        mensaje = respuestaHijos[1];
        ErroresServicios erroresServicios = new ErroresServicios();
        erroresServicios._codigo = codigo;
        erroresServicios._mensaje = mensaje;
        erroresServicios.obtieneCodigo();
        if (Convert.ToBoolean(erroresServicios._retorno[0]))
        {
            if (erroresServicios._codigo == "06")
            {
                if (respuesta[0] != "" && Convert.ToInt32(respuesta[0]) > 0)
                {
                    _retorno[0] = true;
                    _retorno[1] = erroresServicios._mensaje.Trim() + ". No. Operación: " + operacion + ". No. Autorización:" + respuesta[0];
                }
                else
                {
                    _retorno[0] = true;
                    _retorno[1] = "Error " + erroresServicios._codigo.Trim() + ": " + erroresServicios._mensaje.Trim() + Environment.NewLine + "Detalle:" + mensaje.ToUpper().Trim() + Environment.NewLine + " No. Operación: " + operacion;
                }
            }
            else
            {
                _retorno[0] = true;
                _retorno[1] = "Error " + erroresServicios._codigo.Trim() + ": " + erroresServicios._mensaje.Trim() + Environment.NewLine + "Detalle:" + mensaje.ToUpper().Trim() + Environment.NewLine + " No. Operación: " + operacion;
            }
        }
    }

    public void actualizaRecarga()
    {
        BaseDatos ejectuta = new BaseDatos();
        string sql = "declare @recarga int set @recarga =select isnull((select count(ps.id_operacion) from pagos_servicios ps inner join prod_pago_serv pp on pp.idproducto = ps.idproducto and pp.idservicio = ps.idservicio where ps.id_operacion = " + operacion.ToString() + " and(pp.producto like '%recarga%' or pp.servicio like '%recarga%')),0) " +
                     "update pagos_servicios set recarga = @recarga where id_operacion = " + operacion.ToString();
        _retorno = ejectuta.scalarData(sql);
    }

    private void confirmaTransaccion()
    {
        servidor = servidor + "confirmaTransaccion.do";

        if (tipoFront == 2)
        {
            if (Convert.ToString(_datos[5]) != "")
                _datos[6] = _datos[6].ToString() + "_" + _datos[5].ToString();
            datosPost = "codigoDispositivo=" + dispositivo + "&password=" + password + "&idDistribuidor=" + distribuidor + "&telefono=" + _datos[4].ToString() + "&idServicio=" + _datos[2].ToString() + "&idProducto=" + _datos[3].ToString() + "&referencia=" + _datos[6].ToString() + "&montoPago=" + _datos[7].ToString();
        }
        else if (tipoFront == 3)
            datosPost = "codigoDispositivo=" + dispositivo + "&password=" + password + "&idDistribuidor=" + distribuidor + "&telefono=" + _datos[4].ToString() + "&idServicio=" + _datos[2].ToString() + "&idProducto=" + _datos[3].ToString() + "&referencia=" + _datos[6].ToString();
        else
            datosPost = "codigoDispositivo=" + dispositivo + "&password=" + password + "&idDistribuidor=" + distribuidor + "&telefono=" + _datos[4].ToString() + "&idServicio=" + _datos[2].ToString() + "&idProducto=" + _datos[3].ToString();
    }

    private void lecturaXmlAbono(string xml)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);

        string codigo, mensaje;//, referencia, saldoCliente;
        string[] respuesta = new string[8] { "", "", "", "", "", "", "", "" };
        string[] respuestaHijos = new string[4] { "", "", "", "" };

        foreach (XmlNode nodos in doc.DocumentElement.ChildNodes)
        {
            bool tieneHijos = false;
            string nodosHijo = "#";
            try
            {
                nodosHijo = nodos.ChildNodes[0].Name;
                string valor = nodosHijo.Substring(0, 1);
                if (nodosHijo.Substring(0, 1) != "#")
                    tieneHijos = true;
                else
                    tieneHijos = false;
            }
            catch (Exception) { tieneHijos = false; }
            if (tieneHijos)
            {
                foreach (XmlNode nodoHijo in nodos.ChildNodes)
                {
                    switch (nodoHijo.Name)
                    {
                        case "CODIGO":
                            respuestaHijos[0] = nodoHijo.InnerText;
                            break;
                        case "TEXTO":
                            respuestaHijos[1] = nodoHijo.InnerText;
                            break;
                        case "SALDOCLIENTE":
                            respuestaHijos[2] = nodoHijo.InnerText;
                            break;
                        case "REFERENCIA":
                            respuestaHijos[3] = nodoHijo.InnerText;
                            break;
                        default: break;
                    }
                }
            }
            else
            {
                switch (nodos.Name)
                {
                    case "ID_TX":
                        respuesta[0] = nodos.InnerText;
                        break;
                    case "NUM_AUTORIZACION":
                        respuesta[1] = nodos.InnerText;
                        break;
                    case "SALDO":
                        respuesta[2] = nodos.InnerText;
                        break;
                    case "COMISION":
                        respuesta[3] = nodos.InnerText;
                        break;
                    case "SALDO_F":
                        respuesta[4] = nodos.InnerText;
                        break;
                    case "COMISION_F":
                        respuesta[5] = nodos.InnerText;
                        break;
                    case "FECHA":
                        respuesta[6] = nodos.InnerText;
                        break;
                    case "MONTO":
                        respuesta[7] = nodos.InnerText;
                        break;
                    default: break;
                }
            }
        }

        registroPagos.añop = fechas.obtieneFechaLocal().Year;
        registroPagos.punto = _punto;
        registroPagos.caja = _caja;
        registroPagos.usuario = _usuario;
        registroPagos.telefono = telefono;
        registroPagos.referencia_in = referencia_in;
        registroPagos.catServicio = catServicio;
        registroPagos.tipoFont = tipoFont;
        registroPagos.idServicio = idServicio;
        registroPagos.idProducto = idProducto;
        registroPagos.montoPagar = montoPagar;
        registroPagos.operacion = operacion;
        registroPagos.codigo = respuestaHijos[0];
        registroPagos.texto = respuestaHijos[1];
        registroPagos.saldo_cliente = respuestaHijos[2];
        registroPagos.referencia = respuestaHijos[3];
        registroPagos.id_txt = respuesta[0];
        registroPagos.num_autorizacion = respuesta[1];
        registroPagos.saldo = respuesta[2];
        registroPagos.comision = respuesta[3];
        registroPagos.saldo_f = respuesta[4];
        registroPagos.comision_f = respuesta[5];
        registroPagos.fecha = respuesta[6];
        registroPagos.monto = respuesta[7];
        registroPagos.xml = "";
        registroPagos.actualizaOperacion();

        codigo = respuestaHijos[0];
        mensaje = respuestaHijos[1];
        _codigo = codigo;
        ErroresServicios erroresServicios = new ErroresServicios();
        erroresServicios._codigo = codigo;
        erroresServicios._mensaje = mensaje;
        erroresServicios.obtieneCodigo();
        if (Convert.ToBoolean(erroresServicios._retorno[0]))
        {
            if (erroresServicios._codigo == "01")
            {
                if (respuesta[1] != "" && Convert.ToInt32(respuesta[1]) > 0)
                {
                    _retorno[0] = true;
                    if (tipoFront == 4)
                    {
                        _retorno[1] = erroresServicios._mensaje.Trim() + ". No. de Operación: " + operacion + ". No. Autorización:" + respuesta[1] + ". Saldo:" + respuestaHijos[2];

                    }
                    else
                    {
                        _retorno[1] = erroresServicios._mensaje.Trim() + ". No. de Operación: " + operacion + ". No. Autorización:" + respuesta[1];
                    }
                }
                else
                {
                    _retorno[0] = true;
                    _retorno[1] = "Error " + erroresServicios._codigo.Trim() + ": " + erroresServicios._mensaje.Trim() + Environment.NewLine + "Detalle:" + mensaje.ToUpper().Trim() + Environment.NewLine + ". No. de Operación: " + operacion;
                }
            }
            else if (codigo == "03" || codigo == "82")
            {
                _peticion = 3;
                Thread.Sleep(2000);
                obtienePagoServicios();
            }
            else
            {
                _retorno[0] = true;
                _retorno[1] = "Error " + erroresServicios._codigo.Trim() + ": " + erroresServicios._mensaje.Trim() + Environment.NewLine + "Detalle:" + mensaje.ToUpper().Trim() + Environment.NewLine + ". No. de Operación: " + operacion;
            }
        }
    }

    private void obtieneListaProductos()
    {
        servidor = servidor + "getListaProducto.do";
        datosPost = "codigoDispositivo=" + dispositivo + "&password=" + password + "&idDistribuidor=" + distribuidor;
    }

    private void gerneraAbono()
    {
        servidor = servidor + "abonar.do";
        if (tipoFront == 2)
        {
            if (Convert.ToString(_datos[5]) != "")
                _datos[6] = _datos[6].ToString() + "_" + _datos[5].ToString();
            datosPost = "codigoDispositivo=" + dispositivo + "&password=" + password + "&idDistribuidor=" + distribuidor + "&telefono=" + _datos[4].ToString() + "&idServicio=" + _datos[2].ToString() + "&idProducto=" + _datos[3].ToString() + "&referencia=" + _datos[6].ToString() + "&montoPago=" + _datos[7].ToString();
        }
        else if (tipoFront == 3)
            datosPost = "codigoDispositivo=" + dispositivo + "&password=" + password + "&idDistribuidor=" + distribuidor + "&telefono=" + _datos[4].ToString() + "&idServicio=" + _datos[2].ToString() + "&idProducto=" + _datos[3].ToString() + "&referencia=" + _datos[6].ToString();
        else
            datosPost = "codigoDispositivo=" + dispositivo + "&password=" + password + "&idDistribuidor=" + distribuidor + "&telefono=" + _datos[4].ToString() + "&idServicio=" + _datos[2].ToString() + "&idProducto=" + _datos[3].ToString();

    }

    private void conexion()
    {
        try
        {
            valorXML = "";
            //Verifica las credenciales de servicio https
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

            //Genera conexion con servidor de ws
            var request = (HttpWebRequest)WebRequest.Create(servidor);

            //Datos a enviar por metodo POST
            var postData = datosPost;

            //Encriptacion de datos
            var data = Encoding.ASCII.GetBytes(postData);

            //request.Timeout = 60000;

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;




            //Consumo de ws
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }



            //Valor de retorno
            var response = (HttpWebResponse)request.GetResponse();

            //Xml de retorno
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            valorXML = responseString;



        }
        catch (Exception ex)
        {
            _retorno[0] = false; _retorno[1] = ex.Message; valorXML = ""; _codigo = "70";
            _peticion = 3;
            Thread.Sleep(2000);
            obtienePagoServicios();
        }
    }

    private void lecturaXmlsProductos(string xml)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);

        string codigo = doc.DocumentElement.ChildNodes[0].ChildNodes[0].InnerText;
        string mensaje = doc.DocumentElement.ChildNodes[0].ChildNodes[1].InnerText.ToUpper();
        _codigo = codigo;
        ErroresServicios erroresServicios = new ErroresServicios();
        erroresServicios._codigo = codigo;
        erroresServicios._mensaje = mensaje;
        erroresServicios.obtieneCodigo();
        if (Convert.ToBoolean(erroresServicios._retorno[0]))
        {
            if (erroresServicios._codigo == "01")
            {
                if (doc.DocumentElement.ChildNodes[1].ChildNodes.Count > 0)
                {
                    int i = 0;
                    int actualizados = 0;
                    foreach (XmlNode nodo in doc.DocumentElement.ChildNodes[1].ChildNodes)
                    {
                        int idCatTipoServicio = Convert.ToInt32(nodo.Attributes["idCatTipoServicio"].Value);
                        int idServicio = Convert.ToInt32(nodo.Attributes["idServicio"].Value);
                        int idProducto = Convert.ToInt32(nodo.Attributes["idProducto"].Value);
                        int tipoFront = Convert.ToInt32(nodo.Attributes["tipoFront"].Value);
                        string servicio = nodo.Attributes["servicio"].Value;
                        string productos = nodo.Attributes["producto"].Value;
                        decimal precio = Convert.ToDecimal(nodo.Attributes["precio"].Value);
                        bool hasDigitoVerificador = Convert.ToBoolean(nodo.Attributes["hasDigitoVerificador"].Value);
                        bool showAyuda = Convert.ToBoolean(nodo.Attributes["showAyuda"].Value);

                        object[] actualizado = producto(idCatTipoServicio, idServicio, idProducto, tipoFront, servicio, productos, precio, hasDigitoVerificador, showAyuda);
                        if (Convert.ToBoolean(actualizado[0]))
                            actualizados++;
                        i++;

                    }
                    _retorno[0] = true;
                    if (actualizados == 0)
                        _retorno[1] = "No se registraron modificaciones a los productos";
                    else
                        _retorno[1] = "Se actualizaron " + actualizados + " productos de " + i;
                }
                else
                {
                    _retorno[0] = true;
                    _retorno[1] = "No se encontraron productos por parte del proveedor de servicios, Contactelo para que le brinde mas información";
                }
            }
            else
            {
                _retorno[0] = true;
                _retorno[1] = "Error " + erroresServicios._codigo.Trim() + ": " + erroresServicios._mensaje.Trim() + Environment.NewLine + "Detalle:" + mensaje.ToUpper().Trim();
            }
        }


    }

    private static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    private void obtieneParametros()
    {
        try
        {
            string[] parametros = obtieneParametrosPagoServicios();
            servidor = "https://" + parametros[0].Trim() + ":" + parametros[1].Trim() + "/BuyTime/service/";
            distribuidor = parametros[2].Trim();
            dispositivo = parametros[3].Trim();
            password = parametros[4].Trim();
        }
        catch (Exception ex)
        {
            servidor = distribuidor = dispositivo = password = "";
            _retorno[1] = ex.Message;
        }
    }

    private string[] obtieneParametrosPagoServicios()
    {
        string[] valores = new string[5] { "", "", "", "", "" };
        BaseDatos datos = new BaseDatos();
        string sql = "select servidor,puerto,distribuidor,dispositivo,contraPS from punto_venta where id_punto=" + _punto;
        object[] info = datos.scalarData(sql);
        try
        {
            if (Convert.ToBoolean(info[0]))
            {
                DataSet infoDatos = (DataSet)info[1];
                foreach (DataRow r in infoDatos.Tables[0].Rows) {
                    valores[0] = r[0].ToString();
                    valores[1] = r[1].ToString();
                    valores[2] = r[2].ToString();
                    valores[3] = r[3].ToString();
                    valores[4] = r[4].ToString();
                }
            }
            else { valores = new string[5] { "", "", "", "", "" }; }
        }
        catch (Exception) { valores = new string[5] { "", "", "", "", "" }; }
        return valores;
    }

    private object[] producto(int idCatTipoServicio, int idServicio, int idProducto, int tipoFont, string servicio, string productos, decimal precio, bool hasDigito, bool ayuda)
    {
        object[] verificaExisteProd = existeProducto(idCatTipoServicio, idServicio, idProducto);
        object[] retorno = new object[] { false, "" };
        if (Convert.ToBoolean(verificaExisteProd[0]))
        {
            if (Convert.ToInt32(verificaExisteProd[1]) > 0)
                retorno = insertaActualizaProducto(2, idCatTipoServicio, idServicio, idProducto, tipoFont, servicio, productos, precio, hasDigito, ayuda);
            else
                retorno = insertaActualizaProducto(1, idCatTipoServicio, idServicio, idProducto, tipoFont, servicio, productos, precio, hasDigito, ayuda);
        }
        return retorno;
    }

    private object[] existeProducto(int idCatTipoServicio, int idServicio, int idProducto)
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "select count(*) from Prod_Pago_Serv where idCatTipoServicio=" + idCatTipoServicio.ToString() + " and idServicio=" + idServicio.ToString() + " and idProducto=" + idProducto.ToString();
        return ejecuta.scalarInt(sql);
    }

    private object[] insertaActualizaProducto(int opcion, int idCatTipoServicio, int idServicio, int idProducto, int tipoFont, string servicio, string producto, decimal precio, bool hasDigito, bool ayuda)
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "";

        decimal[] montos = new decimal[2] { 0, 0 };
        if (producto.IndexOf('$') > 0)
            montos = obtieneValoresPrecioComision(producto, precio);
        else
            montos = new decimal[2] { precio, 0 };

        string[] parametros = obtieneParametrosPagoServicios();


        string urlLogo = "https://" + parametros[0].Trim() + ":" + parametros[1].Trim() + "/BuyTime/assets/images/logos/" + idServicio.ToString() + ".jpg";
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
        WebClient wc = new WebClient();
        byte[] imagen = null;
        try { imagen = wc.DownloadData(urlLogo); }
        catch (Exception) { imagen = null; }

        string urlRef = "https://" + parametros[0].Trim() + ":" + parametros[1].Trim() + "/BuyTime/assets/images/references/" + idServicio.ToString() + ".jpg";
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
        WebClient wcf = new WebClient();
        byte[] imagenRef = null;
        try { imagenRef = wcf.DownloadData(urlRef); }
        catch (Exception) { imagenRef = null; }

        if (opcion == 1)
            sql = "insert into Prod_Pago_Serv values(" + idCatTipoServicio.ToString() + "," + idServicio.ToString() + "," + idProducto.ToString() + ",'" + servicio.Trim().ToUpper() + "','" + producto.Trim().ToUpper() + "'," + precio.ToString("F2") + "," + tipoFont.ToString() + "," + ConvierteBoolInt(hasDigito).ToString() + "," + ConvierteBoolInt(ayuda).ToString() + "," + montos[0].ToString("F2") + ",0," + montos[1].ToString("F2") + ")";
        else
            sql = "update Prod_Pago_Serv set servicio='" + servicio.Trim().ToUpper() + "', producto='" + producto.Trim().ToUpper() + "', precio=" + precio.ToString("F2") + ",tipoFont=" + tipoFont.ToString() + ",hasDigitoVerificador=" + ConvierteBoolInt(hasDigito).ToString() + ",showAyuda=" + ConvierteBoolInt(ayuda).ToString() + ", precio_tienda=" + montos[0].ToString("F2") + ", comision=" + montos[1].ToString("F2") + " where idCatTipoServicio=" + idCatTipoServicio.ToString() + " and idServicio=" + idServicio.ToString() + " and idProducto=" + idProducto.ToString();
        object[] retorno = ejecuta.insertUpdateDelete(sql);

        if (opcion == 1)
            sql = "insert into imagenes_pago_serv values(" + idCatTipoServicio.ToString() + "," + idServicio.ToString() + ",@imagenLogo,@imagenRef)";
        else
            sql = "update imagenes_pago_serv set logo=@imagenLogo, referencia=@imagenRef where idCatTipoServicio=" + idCatTipoServicio.ToString() + " and idServicio=" + idServicio.ToString();

        ejecuta.insertUpdateDeleteImagenes(sql, imagen, imagenRef);

        return retorno;

    }

    private decimal[] obtieneValoresPrecioComision(string producto, decimal precio)
    {
        decimal[] retorno = new decimal[2] { 0, 0 };
        try
        {
            decimal monto = 0, comision = 0;
            int posicion = producto.IndexOf('$');
            int longitud = producto.Length;
            string cadenaProceso = producto.Substring(posicion + 1);
            if (cadenaProceso.IndexOf(',') > 0)
                cadenaProceso = cadenaProceso.Remove(cadenaProceso.IndexOf(','), 1);
            monto = Convert.ToDecimal(cadenaProceso);
            comision = precio - monto;
            retorno[0] = monto;
            retorno[1] = comision;
        }
        catch (Exception ex) { retorno[0] = precio; retorno[1] = 0; }
        return retorno;

    }

    private int ConvierteBoolInt(bool valor)
    {
        if (valor)
            return 1;
        else
            return 0;
    }

    public void obtieneTipoFont()
    {
        BaseDatos ejectuta = new BaseDatos();
        string sql = "select DISTINCT P.tipoFront,T.descripcion from  Prod_Pago_Serv P INNER JOIN TipoFont T ON T.tipoFont=P.tipoFront";
        _retorno = ejectuta.scalarData(sql);
    }

    public void obtieneInformacionProducto()
    {
        BaseDatos ejectuta = new BaseDatos();
        string sql = string.Format("select * from  Prod_Pago_Serv where tipofront={0} and idCatTipoServicio={1} and idServicio={2} and idProducto={3}", Convert.ToString(_datos[0]), Convert.ToString(_datos[1]), Convert.ToString(_datos[2]), Convert.ToString(_datos[3]));
        _retorno = ejectuta.scalarData(sql);
    }

}