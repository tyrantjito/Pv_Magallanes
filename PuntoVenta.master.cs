using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;
using System.Configuration;
using System.Data;

public partial class PuntoVenta : System.Web.UI.MasterPage
{
    string usuario, nombre, pv, caja, nom_isla;
    bool[] permisos;
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {
        checaSesiones();
        lblFecha.Text = fechas.obtieneFechaLocal().ToLongDateString();
        lblUsuarioLog.Text = "Bienvenido: " + nombre;
        lblCaja.Text = "Caja:" + caja;
        lblIsla.Text = "Tienda: " + nom_isla;
        if(!IsPostBack)
            lblVersionSis.Text = ConfigurationManager.AppSettings["version"].ToString();
    }
    private void checaSesiones()
    {
        try { usuario = Convert.ToString(Request.QueryString["u"]); nombre = Convert.ToString(Request.QueryString["nu"]); pv = Convert.ToString(Request.QueryString["p"]); caja = Convert.ToString(Request.QueryString["c"]); nom_isla = Convert.ToString(Request.QueryString["np"]); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
        usuario = Request.QueryString["u"];
        nombre = Request.QueryString["nu"];
        pv = Request.QueryString["p"];
        caja = Request.QueryString["c"];
        nom_isla = Request.QueryString["np"];

        ObtienePermisos usuPermisos = new ObtienePermisos();
        usuPermisos.Usuario = usuario;
        usuPermisos.obtienePermisos();
        permisos = usuPermisos.Permisos;
        controlaMenu(permisos);
        CierreCaja cierreCaja = new CierreCaja();
        cierreCaja.Caja = Convert.ToInt32(caja);
        object[] cajasRegistradas = cierreCaja.obtieneCajasExistentes();
        if (Convert.ToBoolean(cajasRegistradas[0]))
        {
            DataSet infoCajas = (DataSet)cajasRegistradas[1];
            if (infoCajas.Tables[0].Rows.Count != 0)
            {
                foreach (DataRow filasCajas in infoCajas.Tables[0].Rows)
                {
                    cierreCaja.Caja = Convert.ToInt32(filasCajas[1]);
                    cierreCaja._anio = Convert.ToInt32(filasCajas[0]);
                    bool cajaCerrada = cierreCaja.cajaActualAbierta();

                    if (cajaCerrada)
                        Response.Redirect("Default.aspx");
                    break;
                }
            }
            else
                Response.Redirect("Default.aspx");
        }
        else
            Response.Redirect("Default.aspx");
    }

    private void controlaMenu(bool[] permisos)
    {
        bool[] autorizados = permisos;
        LinkButton[] opciones = { lknNuevaVenta, lknGastos, lknOrdenCompra, lknCorteParcial, lknFacturacion, lknCierreDia };
        int j = 0;
        for (int i = 0; i < permisos.Length; i++)
        {
            if (i > 27 && i < 34)
            {
                opciones[j].Visible = permisos[i];
                j++;
            }
        }
    }

    protected void lnkSalir_Click(object sender, EventArgs e)
    {
        if (usuario != "")
        {
            CierreCaja cierre = new CierreCaja();
            Usuarios usuarioPv = new Usuarios();
            usuarioPv.Usuario = Request.QueryString["u"];
            usuarioPv.cajaAsignada();
            //usuarioPv.obtienePuntoVenta();
            int caja = usuarioPv.Caja;
            cierre.Acceso = "S";
            cierre.Caja = caja;
            cierre.FechaDia = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
            cierre.Punto = Convert.ToInt32(Request.QueryString["P"]);
            cierre.Usuario = usuarioPv.Usuario;
            cierre.generaCorteCaja();            
        }
        Response.Redirect("Default.aspx");
    }
    
    protected void lknNuevaVenta_Click(object sender, EventArgs e)
    {        
        Response.Redirect("PVenta.aspx?u=" + usuario + "&nu=" + nombre + "&p=" + pv + "&np=" + nom_isla + "&c=" + caja);
    }

    protected void lknGastos_Click(object sender, EventArgs e)
    {        
        Response.Redirect("Gastos_PV.aspx?u=" + usuario + "&nu=" + nombre + "&p=" + pv + "&np=" + nom_isla + "&c=" + caja);
    }

    protected void lknOrdenCompra_Click(object sender, EventArgs e)
    {        
        Response.Redirect("Orden_Compra.aspx?u=" + usuario + "&nu=" + nombre + "&p=" + pv + "&np=" + nom_isla + "&c=" + caja);
    }

    protected void lknCorteParcial_Click(object sender, EventArgs e)
    {        
        Response.Redirect("CorteParcial.aspx?u=" + usuario + "&nu=" + nombre + "&p=" + pv + "&np=" + nom_isla + "&c=" + caja);
    }

    protected void lknFacturacion_Click(object sender, EventArgs e)
    {        
        Response.Redirect("Facturacion_Posterior.aspx?u=" + usuario + "&nu=" + nombre + "&p=" + pv + "&np=" + nom_isla + "&c=" + caja);
    }

    protected void lknCierreDia_Click(object sender, EventArgs e)
    {        
        Response.Redirect("CierreDiario.aspx?u=" + usuario + "&nu=" + nombre + "&p=" + pv + "&np=" + nom_isla + "&c=" + caja);
    }

    protected void lnkPagoServicios_Click(object sender, EventArgs e)
    {
        Response.Redirect("PagoServicios.aspx?u=" + usuario + "&nu=" + nombre + "&p=" + pv + "&np=" + nom_isla + "&c=" + caja);
    }

    protected void lnkCancelaPagos_Click(object sender, EventArgs e)
    {
        Response.Redirect("UtileriasPagoTarjetas.aspx?u=" + usuario + "&nu=" + nombre + "&p=" + pv + "&np=" + nom_isla + "&c=" + caja);
    }

    protected void lnkCancelaDev_Click(object sender, EventArgs e)
    {
        Response.Redirect("Cancelaciones.aspx?u=" + usuario + "&nu=" + nombre + "&p=" + pv + "&np=" + nom_isla + "&c=" + caja);
    }

    protected void lnkReimprimirTicket_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReimprimeTicketPT.aspx?u=" + usuario + "&nu=" + nombre + "&p=" + pv + "&np=" + nom_isla + "&c=" + caja);
    }

    protected void lnkInventario_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConsultaInventarioPV.aspx?u=" + usuario + "&nu=" + nombre + "&p=" + pv + "&np=" + nom_isla + "&c=" + caja);
    }
    protected void lnkentradas_Click(object sender, EventArgs e)
    {
        Response.Redirect("Entradas_PV.aspx?u=" + usuario + "&nu=" + nombre + "&p=" + pv + "&np=" + nom_isla + "&c=" + caja);
    }
}
