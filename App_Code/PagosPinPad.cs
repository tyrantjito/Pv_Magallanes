using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Threading;
using System.Configuration;
using System.Xml;
using System.Data;

/// <summary>
/// Descripción breve de PagosPinPad
/// </summary>
public class PagosPinPad
{

    string servidor;    
    public int terminal { get; set; }
    public string clave { get; set; }
    string valorXML;
    string cadena;
    string cadenaPost;

    public object[] _retorno { get; set; }
    
    public string opcion {get; set;}
    public decimal importe {get; set;}
    public string estatus { get; set; }
    public string fecha { get; set; }

    public string aprobacion {get; set;} 
    public string tarjeta {get; set;}
    public string nombre {get; set;}
    public string concepto {get; set;}
    public string referencia {get; set;} 
    public string correo {get; set;}
    public string folio {get; set;}
    public string parcializacion {get; set;}
    public string diferimiento {get; set;}
    public string estatusComentario {get; set;}
    public string causaDenegada {get; set;}
    public string descripcion {get; set;}
    public string corte {get; set;}
    public string cadenaEncriptada {get; set;}
    public string[] codigos { get; set; }

    long idSolicitud;
    public int iteraciones { get; set; }
    int intentos;
    int proceso;
    string[] respuesta;
    long transaccion;

    public int ticket { get; set; }
    public int caja { get; set; }
    public int punto { get; set; }
    public string usuario { get; set; }

    object[] datos;

	public PagosPinPad()
	{
        terminal = intentos = iteraciones = 0;
        idSolicitud = 0;
        importe = 0;
        clave = cadena = cadenaPost = estatus = fecha = aprobacion = tarjeta = nombre = concepto = referencia = correo = folio = parcializacion = diferimiento = estatusComentario = causaDenegada = descripcion = corte = cadenaEncriptada = string.Empty;
        respuesta = new string[18];
        _retorno = new object[2];
        datos = new object[2];
        codigos = new string[2];
        transaccion = 0;

        ticket = caja = punto = 0;
        usuario = string.Empty;
	}


    public void ejecutaPeticion() {
        servidor = ConfigurationManager.AppSettings["peticion"].ToString();
        //terminal = 31339;
        //clave = "FSI724960";
        
        switch (opcion)
        {
            case "00":
                //00 Venta Normal

                cadenaPost = "tipoPlan=" + opcion.Trim() + "&terminal=" + terminal.ToString() + "&importe=" + importe.ToString("F2").Trim();                
                cadena = opcion + terminal.ToString().Trim() + importe.ToString("F2").Trim();

                generaTransaccionInterna();
                if (transaccion != 0)
                {
                    if (nombre != "")
                    {
                        cadenaPost = cadenaPost.Trim() + "&nombre=" + nombre.Trim();
                        cadena = cadena.Trim() + nombre.Trim();
                    }

                    if (concepto != "")
                    {
                        cadenaPost = cadenaPost.Trim() + "&concepto=" + concepto.Trim();
                        cadena = cadena.Trim() + concepto.Trim();
                    }

                    cadenaPost = cadenaPost.Trim() + "&referencia=" + referencia.Trim();
                    cadena = cadena.Trim() + referencia.Trim();

                    if (correo != "")
                    {
                        cadenaPost = cadenaPost.Trim() + "&correo=" + correo.Trim();
                        cadena = cadena.Trim() + correo.Trim();
                    }

                    cadena = cadena.Trim() + clave.Trim();
                    cadenaEncriptada = encripta(cadena).ToLower();

                    cadenaPost = cadenaPost.Trim() + "&cadenaEncriptada=" + cadenaEncriptada.Trim();

                    proceso = 1;
                    conexion();

                    try
                    {
                        string valor = valorXML;
                        idSolicitud = Convert.ToInt64(valor);

                        intentos = 0;
                        if (idSolicitud != 0)
                        {
                            actualizaDatos();
                            if (intentos == 0)
                                Thread.Sleep(10000);
                            else
                                Thread.Sleep(3000);
                            llamaValidador();
                        }
                    }
                    catch (Exception ex)
                    {
                        _retorno[0] = false;
                        _retorno[1] = "Error al intentar generar la transacción de venta normal. Id Solicitud: " + idSolicitud.ToString() + ". Detalle de error: " + ex.Message;
                        codigos[0] = "100";
                        codigos[1] = "Error al intentar generar la transacción de venta normal. Id Solicitud: " + idSolicitud.ToString() + ". Detalle de error: " + ex.Message;
                    }
                }
                else {
                    _retorno[0] = false;
                    _retorno[1] = "Error al intentar generar la transacción de venta normal";
                    codigos[0] = "101";
                    codigos[1] = "Error al intentar generar la transacción de venta normal";
                }
                break;
            case "03":
                //03 Venta con Promición
                cadenaPost = "tipoPlan=" + opcion.Trim() + "&terminal=" + terminal.ToString() + "&importe=" + importe.ToString("F2").Trim();                
                cadena = opcion + terminal.ToString().Trim() + importe.ToString("F2").Trim();
                generaTransaccionInterna();
                if (transaccion != 0)
                {
                    if (nombre != "")
                    {
                        cadenaPost = cadenaPost.Trim() + "&nombre=" + nombre.Trim();
                        cadena = cadena.Trim() + nombre.Trim();
                    }

                    if (concepto != "")
                    {
                        cadenaPost = cadenaPost.Trim() + "&concepto=" + concepto.Trim();
                        cadena = cadena.Trim() + concepto.Trim();
                    }

                    cadenaPost = cadenaPost.Trim() + "&referencia=" + referencia.Trim();
                    cadena = cadena.Trim() + referencia.Trim();

                    if (correo != "")
                    {
                        cadenaPost = cadenaPost.Trim() + "&correo=" + correo.Trim();
                        cadena = cadena.Trim() + correo.Trim();
                    }


                    cadenaPost = cadenaPost.Trim() + "&parcializacion=" + parcializacion.Trim();
                    cadena = cadena.Trim() + parcializacion.Trim();

                    cadenaPost = cadenaPost.Trim() + "&diferimiento=" + diferimiento.Trim();
                    cadena = cadena.Trim() + diferimiento.Trim();

                    cadena = cadena.Trim() + clave.Trim();
                    cadenaEncriptada = encripta(cadena).ToLower();

                    cadenaPost = cadenaPost.Trim() + "&cadenaEncriptada=" + cadenaEncriptada.Trim();

                    proceso = 1;

                    conexion();

                    try
                    {
                        string valor = valorXML;
                        idSolicitud = Convert.ToInt64(valor);

                        intentos = 0;
                        if (idSolicitud != 0)
                        {
                            actualizaDatos();
                            if (intentos == 0)
                                Thread.Sleep(10000);
                            else
                                Thread.Sleep(3000);
                            llamaValidador();
                        }
                    }
                    catch (Exception ex)
                    {
                        _retorno[0] = false;
                        _retorno[1] = "Error al intentar generar la transacción de venta con promoción. Id Solicitud: " + idSolicitud.ToString() + ". Detalle de error: " + ex.Message;
                        codigos[0] = "102";
                        codigos[1] = "Error al intentar generar la transacción de venta con promoción. Id Solicitud: " + idSolicitud.ToString() + ". Detalle de error: " + ex.Message;
                    }
                }
                else
                {
                    _retorno[0] = false;
                    _retorno[1] = "Error al intentar generar la transacción de venta con promoción";
                    codigos[0] = "103";
                    codigos[1] = "Error al intentar generar la transacción de venta con promoción";
                }
                break;
            case "16":
                //16 Cancelación
                cadenaPost = "tipoPlan=" + opcion.Trim() + "&terminal=" + terminal.ToString() + "&importe=" + importe.ToString("F2").Trim();                
                cadena = opcion + terminal.ToString().Trim() + importe.ToString("F2").Trim();
                
                generaTransaccionInterna();
                if (transaccion != 0)
                {

                    cadenaPost = cadenaPost.Trim() + "&referencia=" + referencia.Trim();
                    cadena = cadena.Trim() + referencia.Trim();

                    cadenaPost = cadenaPost.Trim() + "&folio=" + folio.Trim();
                    cadena = cadena.Trim() + folio.Trim();

                    cadena = cadena.Trim() + clave.Trim();
                    cadenaEncriptada = encripta(cadena).ToLower();

                    cadenaPost = cadenaPost.Trim() + "&cadenaEncriptada=" + cadenaEncriptada.Trim();

                    proceso = 1;

                    conexion();

                    try
                    {
                        string valor = valorXML;
                        idSolicitud = Convert.ToInt64(valor);

                        intentos = 0;
                        if (idSolicitud != 0)
                        {
                            actualizaDatos();
                            if (intentos == 0)
                                Thread.Sleep(10000);
                            else
                                Thread.Sleep(3000);
                            llamaValidador();
                        }
                    }
                    catch (Exception ex)
                    {
                        _retorno[0] = false;
                        _retorno[1] = "Error al intentar generar la transacción de cancelación. Id Solicitud: " + idSolicitud.ToString() + ". Detalle de error: " + ex.Message;
                        codigos[0] = "104";
                        codigos[1] = "Error al intentar generar la transacción de cancelación. Id Solicitud: " + idSolicitud.ToString() + ". Detalle de error: " + ex.Message;
                    }
                }
                else
                {
                    _retorno[0] = false;
                    _retorno[1] = "Error al intentar generar la transacción de cancelación";
                    codigos[0] = "105";
                    codigos[1] = "Error al intentar generar la transacción de cancelación";
                }
                break;
            case "17":
                //17 Devolución
                cadenaPost = "tipoPlan=" + opcion.Trim() + "&terminal=" + terminal.ToString() + "&importe=" + importe.ToString("F2").Trim();                
                cadena = opcion + terminal.ToString().Trim() + importe.ToString("F2").Trim();
                
                generaTransaccionInterna();
                if (transaccion != 0)
                {
                    cadenaPost = cadenaPost.Trim() + "&referencia=" + referencia.Trim();
                    cadena = cadena.Trim() + referencia.Trim();

                    cadenaPost = cadenaPost.Trim() + "&folio=" + folio.Trim();
                    cadena = cadena.Trim() + folio.Trim();

                    cadena = cadena.Trim() + clave.Trim();
                    cadenaEncriptada = encripta(cadena).ToLower();

                    cadenaPost = cadenaPost.Trim() + "&cadenaEncriptada=" + cadenaEncriptada.Trim();

                    proceso = 1;

                    conexion();

                    try
                    {
                        string valor = valorXML;
                        idSolicitud = Convert.ToInt64(valor);

                        intentos = 0;
                        if (idSolicitud != 0)
                        {
                            actualizaDatos();
                            if (intentos == 0)
                                Thread.Sleep(10000);
                            else
                                Thread.Sleep(3000);
                            llamaValidador();
                        }
                    }
                    catch (Exception ex)
                    {
                        _retorno[0] = false;
                        _retorno[1] = "Error al intentar generar la transacción de devoluación. Id Solicitud: " + idSolicitud.ToString() + ". Detalle de error: " + ex.Message;
                        codigos[0] = "106";
                        codigos[1] = "Error al intentar generar la transacción de devoluación. Id Solicitud: " + idSolicitud.ToString() + ". Detalle de error: " + ex.Message;
                    }
                }
                else
                {
                    _retorno[0] = false;
                    _retorno[1] = "Error al intentar generar la transacción de devolución";
                    codigos[0] = "107";
                    codigos[1] = "Error al intentar generar la transacción de devolución";
                }
                break;
            case "20":
                //20 Reimprimir Tickets
                cadenaPost = "tipoPlan=" + opcion.Trim() + "&terminal=" + terminal.ToString();                
                cadena = opcion + terminal.ToString().Trim();
                
                generaTransaccionInterna();
                if (transaccion != 0)
                {

                    cadenaPost = cadenaPost.Trim() + "&folio=" + folio.Trim();
                    cadena = cadena.Trim() + folio.Trim();

                    cadena = cadena.Trim() + clave.Trim();
                    cadenaEncriptada = encripta(cadena).ToLower();

                    cadenaPost = cadenaPost.Trim() + "&cadenaEncriptada=" + cadenaEncriptada.Trim();

                    proceso = 1;

                    conexion();

                    try
                    {
                        string valor = valorXML;
                        idSolicitud = Convert.ToInt64(valor);

                        intentos = 0;
                        if (idSolicitud != 0)
                        {
                            actualizaDatos();
                            if (intentos == 0)
                                Thread.Sleep(10000);
                            else
                                Thread.Sleep(3000);
                            llamaValidador();
                        }
                    }
                    catch (Exception ex)
                    {
                        _retorno[0] = false;
                        _retorno[1] = "Error al intentar generar la transacción de reimpresión de tickets. Id Solicitud: " + idSolicitud.ToString() + ". Detalle de error: " + ex.Message;
                        codigos[0] = "108";
                        codigos[1] = "Error al intentar generar la transacción de reimpresión de tickets. Id Solicitud: " + idSolicitud.ToString() + ". Detalle de error: " + ex.Message;
                    }
                }
                else
                {
                    _retorno[0] = false;
                    _retorno[1] = "Error al intentar generar la transacción de reimpresión de tickets";
                    codigos[0] = "109";
                    codigos[1] = "Error al intentar generar la transacción de reimpresión de tickets";
                }
                break;
            case "21":
                //21 Reporte de Detalles
                cadenaPost = "tipoPlan=" + opcion.Trim() + "&terminal=" + terminal.ToString();                                
                cadenaPost = cadenaPost.Trim() + "&fecha=" + fecha.Trim();
                generaTransaccionInterna();
                if (transaccion != 0)
                {
                    proceso = 1;
                    conexion();
                    try
                    {
                        string valor = valorXML;
                        idSolicitud = Convert.ToInt64(valor);

                        intentos = 0;
                        if (idSolicitud != 0)
                        {
                            actualizaDatos();
                            if (intentos == 0)
                                Thread.Sleep(10000);
                            else
                                Thread.Sleep(3000);
                            llamaValidador();
                        }
                    }
                    catch (Exception ex)
                    {
                        _retorno[0] = false;
                        _retorno[1] = "Error al intentar generar la transacción de reporte de detalles. Id Solicitud: " + idSolicitud.ToString() + ". Detalle de error: " + ex.Message;
                        codigos[0] = "110";
                        codigos[1] = "Error al intentar generar la transacción de reporte de detalles. Id Solicitud: " + idSolicitud.ToString() + ". Detalle de error: " + ex.Message;
                    }
                }
                else
                {
                    _retorno[0] = false;
                    _retorno[1] = "Error al intentar generar la transacción de reporte de detalles";
                    codigos[0] = "111";
                    codigos[1] = "Error al intentar generar la transacción de reporte de detalles";
                }
                break;
            default:
                break;

        }

        
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
            var postData = cadenaPost;// datosPost;

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

            if (proceso == 1)
                valorXML = responseString;
            else {
                while (intentos <= iteraciones)
                {
                    int codigo;
                    try
                    {
                        codigo = Convert.ToInt32(responseString);
                        intentos++;
                        Thread.Sleep(3000);
                        llamaValidador();
                    }
                    catch (Exception ex) { valorXML = responseString; procesaXml(); break; }
                }
                if (intentos>iteraciones) {

                    object[] info = obtieneEstatusTransaccion();
                    if (Convert.ToBoolean(info[0]))
                    {
                        string valorEstatus = Convert.ToString(info[1]);

                        _retorno[0] = true;
                        string estatus = "Incompleta. Compruebe que este conectado el dispositivo y/o que prosepago se este ejecutando";

                        if (valorEstatus == "" || valorEstatus == null)
                        {
                            _retorno[0] = false;
                            _retorno[1] = valorXML;
                            codigos[0] = "112";
                            codigos[1] = "Detalle de Error: " + valorXML;
                        }
                        else
                        {

                            switch (valorEstatus)
                            {
                                case "0":
                                    estatus = "Incompleta. El proceso de la transacción no ha sido completada.";
                                    codigos[0] = valorEstatus;                                    
                                    break;
                                case "1":
                                    estatus = "Denegada. La operación es rechazada por el banco.";
                                    break;
                                case "2":
                                    estatus = "Autorizada. Operación existosa.";
                                    break;
                                case "3":
                                    estatus = "Liquidada. Ya se llevó a cabo el depósito por parte de Prosepago. ";
                                    break;
                                case "4":
                                    estatus = "En Proceso. Está en Proceso de liquidación. Detalle: ";
                                    break;
                                case "5":
                                    estatus = "Esperando autorización. La domiciliación está pendiente de autorización.";
                                    break;
                                case "6":
                                    estatus = "Cancelada. Una operación autorizada es cancelada con éxito.";
                                    break;
                                case "7":
                                    estatus = "Devuelta. Una operación autorizada es devuelta con éxito.";
                                    break;
                                case "8":
                                    estatus = "Inválida.";
                                    break;
                                case "10":
                                    estatus = "En aclaración. Depósito es retenido por el banco.";
                                    break;
                                default:
                                    estatus = "Incompleta. El proceso de la transacción no ha sido completada.";
                                    break;
                            }

                            codigos[0] = valorEstatus;
                        }

                        string estatusCOmentario = "";
                        if (respuesta[14] == null)
                            estatusCOmentario = "";
                        else
                            estatusCOmentario = respuesta[14];
                        string casuaDenegada = "";
                        if (respuesta[15] == null)
                            casuaDenegada = "";
                        else
                            casuaDenegada = respuesta[15];
                        string mensaje = "Id Solicitud: " + idSolicitud.ToString() + "." + Environment.NewLine + "Id transacción: " + transaccion.ToString() + Environment.NewLine + "Información adicional y/o estatus de pago: " + estatus + " " + estatusCOmentario.Trim() + " " + casuaDenegada.Trim();
                        codigos[1] = mensaje.Replace(Environment.NewLine, "<br/>");
                        _retorno[1] = mensaje.Replace(Environment.NewLine, "<br/>");

                    }
                    else
                    {
                        _retorno[0] = false;
                        _retorno[1] = "Error al intentar resolver el estatus de su transacción, por favor vuelva a intenar. No. Solicitud:" + idSolicitud.ToString();
                        codigos[0] = "113";
                        codigos[1] = "Error al intentar resolver el estatus de su transacción, por favor vuelva a intenar. No. Solicitud:" + idSolicitud.ToString();
                    }
                }
                else if (intentos <= iteraciones) {
                    _retorno[0] = true;
                    string estatus = "Incompleta. Compruebe que este conectado el dispositivo y/o que prosepago se este ejecutando";

                    if (respuesta[3] == "" || respuesta[3] == null)
                    {
                        _retorno[0] = false;
                        _retorno[1] = valorXML;
                        codigos[0] = "114";
                        codigos[1] = "Detalle de Error: " + valorXML;
                    }
                    else
                    {

                        switch (respuesta[3])
                        {
                            case "0":
                                estatus = "Incompleta. El proceso de la transacción no ha sido completada.";
                                break;
                            case "1":
                                estatus = "Denegada. La operación es rechazada por el banco.";
                                break;
                            case "2":
                                estatus = "Autorizada. Operación existosa.";
                                break;
                            case "3":
                                estatus = "Liquidada. Ya se llevó a cabo el depósito por parte de Prosepago. ";
                                break;
                            case "4":
                                estatus = "En Proceso. Está en Proceso de liquidación. Detalle: ";
                                break;
                            case "5":
                                estatus = "Esperando autorización. La domiciliación está pendiente de autorización.";
                                break;
                            case "6":
                                estatus = "Cancelada. Una operación autorizada es cancelada con éxito.";
                                break;
                            case "7":
                                estatus = "Devuelta. Una operación autorizada es devuelta con éxito.";
                                break;
                            case "8":
                                estatus = "Inválida.";
                                break;
                            case "10":
                                estatus = "En aclaración. Depósito es retenido por el banco.";
                                break;
                            default:
                                estatus = "Incompleta. El proceso de la transacción no ha sido completada.";
                                break;
                        }
                        codigos[0] = respuesta[3];
                    }

                    string estatusCOmentario = "";
                    if (respuesta[14] == null)
                        estatusCOmentario = "";
                    else
                        estatusCOmentario = respuesta[14];
                    string casuaDenegada = "";
                    if (respuesta[15] == null)
                        casuaDenegada = "";
                    else
                        casuaDenegada = respuesta[15];
                    string mensaje = "Id Solicitud: " + idSolicitud.ToString() + "." + Environment.NewLine + "Id transacción: " + transaccion.ToString() + Environment.NewLine + "Información adicional y/o estatus de pago: " + estatus + " " + estatusCOmentario.Trim() + " " + casuaDenegada.Trim();
                    codigos[1] = mensaje.Replace(Environment.NewLine, "<br/>");
                    _retorno[1] = mensaje.Replace(Environment.NewLine, "<br/>");                     
                }
            }
        }
        catch (Exception ex)
        {
            _retorno[0] = false;
            _retorno[1] = "Error al intentar resolver el estatus de su transacción, por favor vuelva a intenar. No. Solicitud:" + idSolicitud.ToString() + " Detalle de error:" + ex.Message;
            codigos[0] = "115";
            codigos[1] = "Error al intentar resolver el estatus de su transacción, por favor vuelva a intenar. No. Solicitud:" + idSolicitud.ToString() + " Detalle de error:" + ex.Message;
        }
    }

    

    private void procesaXml()
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(valorXML);

        foreach (XmlNode nodos in doc.DocumentElement.ChildNodes)
        {
            if (nodos.Name == "terminal")
                respuesta[0] = nodos.InnerText;
            if (nodos.Name == "tipoPlan")
                respuesta[1] = nodos.InnerText;
            if (nodos.Name == "importe")
                respuesta[2] = nodos.InnerText;
            if (nodos.Name == "status")
                respuesta[3] = nodos.InnerText;
            if (nodos.Name == "aprobacion")
                respuesta[4] = nodos.InnerText;
            if (nodos.Name == "tarjeta")
                respuesta[5] = nodos.InnerText;
            if (nodos.Name == "nombre")
                respuesta[6] = nodos.InnerText;
            if (nodos.Name == "concepto")
                respuesta[7] = nodos.InnerText;
            if (nodos.Name == "referencia")
                respuesta[8] = nodos.InnerText;
            if (nodos.Name == "correo")
                respuesta[9] = nodos.InnerText;
            if (nodos.Name == "folio")
                respuesta[10] = nodos.InnerText;
            if (nodos.Name == "parcializacion")
                respuesta[11] = nodos.InnerText;
            if (nodos.Name == "diferimiento")
                respuesta[12] = nodos.InnerText;
            if (nodos.Name == "fecha")
                respuesta[13] = nodos.InnerText;
            if (nodos.Name == "statusComentario")
                respuesta[14] = nodos.InnerText;
            if (nodos.Name == "causaDenegada")
                respuesta[15] = nodos.InnerText;
            if (nodos.Name == "descripcion")
                respuesta[16] = nodos.InnerText;
            if (nodos.Name == "corte")
                respuesta[17] = nodos.InnerText;            
        }
        if (respuesta.Length > 0) {
            actualizaInformacion();
        }
    }

    

    private void llamaValidador()
    {
        try {
            servidor = ConfigurationManager.AppSettings["resultado"].ToString();

            cadena = idSolicitud.ToString() + clave.Trim();
            string encriptado = encripta(cadena).ToLower();
            cadenaPost = "idsolicitud=" + idSolicitud.ToString() + "&cadenaEncriptada=" + encriptado.ToString();
            proceso = 2;
            conexion();
        }
        catch (Exception ex) { }
    }

    private static bool ValidateServerCertificate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    public static string encripta(string cadena)
    {
        SHA1 sha1 = SHA1Managed.Create();
        ASCIIEncoding encodiing = new ASCIIEncoding();
        byte[] stream = null;
        StringBuilder sb = new StringBuilder();
        stream = sha1.ComputeHash(encodiing.GetBytes(cadena));
        for (int i = 0; i < stream.Length; i++) {
            sb.AppendFormat("{0:x2}", stream[i]);
        }

        return sb.ToString();       
    }

    private void generaTransaccionInterna()
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "declare @transaccion bigint " +
"declare @referencia varchar(200) " +
"set @transaccion = (select isnull((select top 1 transaccion from pagos_Tarjetas order by transaccion desc),0))+1 " +
"set @referencia = ltrim(rtrim(cast(" + punto.ToString() + " as char(10))))+'-'+ltrim(rtrim(CAST(" + terminal + " as char(7))))+'-'+'" + opcion + "'+'-'+ltrim(rtrim(CAST(@transaccion as char(20)))) " +
"insert into Pagos_Tarjetas (transaccion,terminal,tipoPlan,referencia) values (@transaccion," + terminal + ",'" + opcion + "',@referencia) " +
"Select @transaccion,@referencia";
        datos = ejecuta.scalarData(sql);
        try
        {
            if (Convert.ToBoolean(datos[0]))
            {
                DataSet info = (DataSet)datos[1];
                foreach (DataRow fila in info.Tables[0].Rows)
                {
                    transaccion = Convert.ToInt64(fila[0]);
                    referencia = Convert.ToString(fila[1]);
                }
            }
            else
            {
                transaccion = 0;
                referencia = "0";
            }

        }
        catch (Exception ex)
        {
            transaccion = 0;
            referencia = "0";
        }

    }

    private void actualizaInformacion()
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "update pagos_tarjetas set importe=" + respuesta[2] + ",status=" + respuesta[3] + ", aprobacion='" + respuesta[4] + "',tarjeta='" + respuesta[5] + "',nombre='" + respuesta[6] + "',concepto='" + respuesta[7] + "',correo='" +
            respuesta[9] + "',folio=" + respuesta[10] + ",parcializacion='" + respuesta[11] + "',diferimiento='" + respuesta[12] + "',fecha='" + respuesta[13] + "',statusComentario='" + respuesta[14] + "',causaDenegada='" +
            respuesta[15] + "',descripcion=" + respuesta[16] + ",corte='" + respuesta[17] + "',cadenaEncriptada='" + cadenaEncriptada + "',ticket=" + ticket.ToString() + ",id_caja=" + caja.ToString() + ",id_punto=" + punto.ToString() + ",usuario='" + usuario + "' where transaccion=" + transaccion;
        datos = ejecuta.insertUpdateDelete(sql);
    }

    public void actualizaDatos()
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "update pagos_tarjetas set ticket=" + ticket.ToString() + ",id_caja=" + caja.ToString() + ",id_punto=" + punto.ToString() + ",usuario='" + usuario + "',id_solicitud=" + idSolicitud.ToString() + " where transaccion=" + transaccion;
        datos = ejecuta.insertUpdateDelete(sql);
    }

    private object[] obtieneEstatusTransaccion()
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "select status from Pagos_Tarjetas where transaccion=" + transaccion;
        return ejecuta.scalarString(sql);
    }


    public void obtieneOperacion()
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "select transaccion,referencia,importe,folio,fecha,tipoPlan from pagos_tarjetas where transaccion=" + ticket;
        _retorno = ejecuta.scalarData(sql);
    }
}