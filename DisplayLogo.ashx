<%@ WebHandler Language="C#" Class="DisplayLogo" %>

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class DisplayLogo : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Clear();
        if (!String.IsNullOrEmpty(context.Request.QueryString["id"]))
        {
            string[] argumentos = context.Request.QueryString["id"].ToString().Split(new char[] { ';' });
            Image imagen =  GetImagen(argumentos[0], argumentos[1], argumentos[2]);
            context.Response.ContentType = "image/jpeg";
            if (imagen != null)
                imagen.Save(context.Response.OutputStream, ImageFormat.Jpeg);
        }
        else
        {
            context.Response.ContentType = "text/html";
            context.Response.Write("&nbsp;");
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private Image GetImagen(string cat, string servicio, string opcion)
    {
        Image img = null;
        MemoryStream memoryStream = new MemoryStream();
        SqlConnection conexion = new SqlConnection();
        conexion.ConnectionString = ConfigurationManager.ConnectionStrings["PVW"].ToString();
        string sql = "";
        if (opcion == "1")
            sql = "select logo from imagenes_pago_serv where idCatTipoServicio=" + cat + " and idServicio=" + servicio;
        else
            sql = "select referencia from imagenes_pago_serv where idCatTipoServicio=" + cat + " and idServicio=" + servicio;
        try
        {
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader lectura = cmd.ExecuteReader();
            if (lectura.HasRows)
            {
                lectura.Read();
                byte[] imagen = (byte[])lectura[0];
                memoryStream = new MemoryStream(imagen, false);
            }
        }
        catch (Exception)
        {

        }
        finally
        {
            conexion.Dispose();
            conexion.Close();
        }
        try
        {
            img = Image.FromStream(memoryStream);
        }
        catch (Exception)
        {
            img = null;
        }
        return img;
    }
}