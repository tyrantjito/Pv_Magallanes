using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;

public partial class CierreDiario : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblDia.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
        if (!IsPostBack)
        {
            CierreCaja cierres = new CierreCaja();
            string ultimaFechaApertura = cierres.obtieneFechaPrimerCajaAbierta();            
            lblDiaApertura.Text = Convert.ToDateTime(ultimaFechaApertura).ToString("yyyy-MM-dd");
            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView2.Visible = false;
            GridView2.DataSource = null;
            GridView2.DataBind();
        }
    }

    protected void btnGenerarCorte_Click(object sender, EventArgs e)
    {
        int pv = 0, caja = 0;
        string usuarioLog = "";
        try
        {
            usuarioLog = Convert.ToString(Request.QueryString["u"]);
            pv = Convert.ToInt32(Request.QueryString["p"]);
            caja = Convert.ToInt32(Request.QueryString["c"]);
        }
        catch (Exception)
        {
            Response.Redirect("Default.aspx");
        }


        if (pv != 0 && caja != 0 && usuarioLog != "")
        {
            CierreCaja cierre = new CierreCaja();
            Usuarios usuario = new Usuarios();
            usuario.Usuario = Request.QueryString["u"];
            usuario.cajaAsignada();
            cierre.FechaDia = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
            //usuario.obtienePuntoVenta();
            cierre.Punto = Convert.ToInt32(Request.QueryString["p"]);
            cierre._horaDia = fechas.obtieneFechaLocal().ToString("HH:mm:ss");
            cierre.existeCierreDia();
            if (!cierre.cierreDia)
            {
                if (usuario.Caja > 0)
                {
                    int cajaAsig = usuario.Caja;
                    cierre.Acceso = "S";
                    cierre.Caja = cajaAsig;
                    cierre.Usuario = usuario.Usuario;
                    cierre.generaCorteCaja();
                    if (!Convert.ToBoolean(cierre.Registrado[0]))
                    {
                        lblError.Text = "Se produjo un error al intentar hacer el cierre de la caja no cerrada.: " + cierre.Registrado[1].ToString();
                    }
                    else
                    {
                        lblError.Text = "Se cerro exitosamente la caja #" + caja.ToString() + "; por favor proceda a volver a iniciar sesión";
                    }
                }
                cierre.existenCajasAbiertas();
                if (!cierre.Abiertas)
                {
                    //cierre.FechaDia = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                    
                    cierre.FechaDia = cierre.obtieneFechaPrimerCajaAbierta();
                    cierre.Usuario = Request.QueryString["u"];
                    cierre.generaCierreDia();
                    object[] cerrado = cierre.Registrado;
                    if (!Convert.ToBoolean(cerrado[0]))
                        lblError.Text = "Se produjo un error al intentar hacer el cierre del día: " + cerrado[1].ToString();
                    else
                    {
                        lblError.Text = "El cierre del día se realizó existósamente. Proceda a cerrar sesión";
                        btnGenerarCorte.Enabled = false;
                        GridView2.Visible = true;
                        GridView1.DataBind();
                        GridView2.DataBind();
                    }
                }
                else
                    lblError.Text = "No se puede cerrar el dia ya que existen cajas abiertas.";
            }
            else
                lblError.Text = "Ya se ha realizó el cierre del día.";

        }
    }
}