using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;

public partial class CorteParcial : System.Web.UI.Page
{
    string usuario;
    int pv;
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {
        checaSesiones();
        Label1.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
        GridView1.DataSource = null;
        GridView1.DataBind();
    }

    private void checaSesiones()
    {
        try
        {
            usuario = Convert.ToString(Request.QueryString["u"]);
            pv = Convert.ToInt32(Request.QueryString["p"]);
        }
        catch (Exception) {
            Response.Redirect("PuntoVenta.aspx?u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"] + "&p" + Request.QueryString["p"] + "&np=" + Request.QueryString["np"] + "&c=" + Request.QueryString["c"]);
        }
    }
    protected void btnGenerarCorte_Click(object sender, EventArgs e)
    {
        checaSesiones();
        CortesParciales corte = new CortesParciales();
        corte.Fecha = Label1.Text;
        corte.Usuario = usuario;
        corte.Punto = pv;
        corte.agregaCorteParcial();
        object[] datos = corte.Accion;
        if (Convert.ToBoolean(datos[0]))
        {
            GridView1.DataBind();
        }
        else {
            lblError.Text = "Error: " + Convert.ToString(datos[1]);
            GridView1.DataSource = null;
            GridView1.DataBind();
        }
    }
}