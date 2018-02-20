using System;
using System.Collections.Generic;
using System.Web;

using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Descripción breve de Envio_Mail
/// </summary>
public class Envio_Mail
{
	public Envio_Mail()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}
    public bool obtieneDatosServidor(string usuariocorreo, string correo)
    {
        bool enviado=false;
        SqlConnection conexion = new SqlConnection();
        conexion.ConnectionString = ConfigurationManager.ConnectionStrings["PVW"].ToString();
        string sql = "select * from CATEMPRESA WHERE id_empresa=1";
        try
        {
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader lectura = cmd.ExecuteReader();
            string usuariohost="", contrasena="", host="";
            int puerto =0; 
            int ssl=0; 

            while (lectura.Read())
            {
                //perfil = lectura.GetInt16(0);
                usuariohost = lectura.GetString(6);
                contrasena = lectura.GetString(7);
                host = lectura.GetString(3);
                puerto = Convert.ToInt32(lectura.GetString(8));
                bool cifrado = lectura.GetBoolean(9);
                if (cifrado)
                    ssl = 1;
                else
                    ssl = 0;
                
            }
            enviado = EnviarCorreo(usuariocorreo, correo, usuariohost,contrasena, puerto, ssl, host);
        }
        catch (Exception)
        {
            enviado = false;
        }
        conexion.Dispose();
        conexion.Close();
        return enviado;
    }

    private bool EnviarCorreo(string usuariocorreo, string correo, string usuariohost, string contrasena, int puerto, int ssl, string host)
    {
        bool envio = false;
        /*-------------------------MENSAJE DE CORREO----------------------*/
        //Creamos un nuevo Objeto de mensaje
        System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
        //Direccion de correo electronico a la que queremos enviar el mensaje
        mmsg.To.Add(correo);
        //Nota: La propiedad To es una colección que permite enviar el mensaje a más de un destinatario
        //Asunto
       Usuarios datos = new Usuarios();
        string usuario = usuariocorreo;
        datos.Usuario = usuario;
        
        mmsg.Subject = "Recuperacion de Contraseña E-PuntoVenta";// "Asunto del correo";
        mmsg.SubjectEncoding = System.Text.Encoding.UTF8;
        //Cuerpo del Mensaje
        mmsg.Body = "<table><tr><td rowspan='2'></td><td><big><big><big>Su contraseña es: </big></big></big></td></tr><tr><td><strong><big><big><big>" + datos.obtieneContrasena() + "</big></big></big></strong></td></tr><tr><td colspan='2'>Da click <a href='http://epuntoventa.azurewebsites.net' target='_blank'>aqui</a> para iniciar sesi&oacute;n; o dirigiete a la aplicaci&oacute;n para iniciar sesi&oacute;n</td></tr></table>";//"Texto del contenio del mensaje de correo";
        mmsg.BodyEncoding = System.Text.Encoding.UTF8;
        mmsg.IsBodyHtml = true; //Si no queremos que se envíe como HTML
        //Correo electronico desde la que enviamos el mensaje
        mmsg.From = new System.Net.Mail.MailAddress(usuariohost);//"juan@formulasistemas.com");//"micuenta@servidordominio.com");
        /*-------------------------CLIENTE DE CORREO----------------------*/
        //Creamos un objeto de cliente de correo
        System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
        //Hay que crear las credenciales del correo emisor
        cliente.Credentials = new System.Net.NetworkCredential(usuariohost, contrasena);//"juan@formulasistemas.com", "juanFS2014");//"micuenta@servidordominio.com", "micontraseña");
        //Lo siguiente es obligatorio si enviamos el mensaje desde Gmail
        ///*
        cliente.Port = puerto;
        bool ssl_obtenido = false;
        if (ssl == 1)
            ssl_obtenido = true;
        cliente.EnableSsl = ssl_obtenido;
        //*/

        cliente.Host = host; //"mail.formulasistemas.com";// "mail.servidordominio.com"; //Para Gmail "smtp.gmail.com";


        /*-------------------------ENVIO DE CORREO----------------------*/

        try
        {
            //Enviamos el mensaje      
            cliente.Send(mmsg);
            envio = true;
        }
        catch (System.Net.Mail.SmtpException ex)
        {
            envio = false;//Aquí gestionamos los errores al intentar enviar el correo
        }
        return envio;
    }
}