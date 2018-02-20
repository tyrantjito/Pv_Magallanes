<%@ WebHandler Language="C#" Class="imgProductos" %>

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class imgProductos : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.Clear();
        if (!String.IsNullOrEmpty(context.Request.QueryString["id"]))
        {
            string claveProducto = context.Request.QueryString["id"].ToString();
            Image imagen = GetImagen(claveProducto);
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

    private Image GetImagen(string claveProducto)
    {
        Image img = null;
        MemoryStream memoryStream = new MemoryStream();
        SqlConnection conexion = new SqlConnection();
        conexion.ConnectionString = ConfigurationManager.ConnectionStrings["PVW"].ToString();
        string sql = "select imagenMueble from catproductos where idproducto='" + claveProducto + "'";
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