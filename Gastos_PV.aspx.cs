using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Gastos_PV : System.Web.UI.Page
{
    GastosDatos datos = new GastosDatos();
    string usuario;
    int idAlmacen;
    int idCaja;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            cargaVariables();
    }

    private void cargaVariables()
    {
        try
        {
            /*
            usuario = Request.QueryString["usuario"];
            idAlmacen = Convert.ToInt32(Request.QueryString["isla"]);
            idCaja = Convert.ToInt32(Request.QueryString["caja"]);
            lblUsuario.Text = usuario;
            lblCaja.Text = idCaja.ToString();
            lblIsla.Text = idAlmacen.ToString();
            GridGastos.DataBind();*/
            usuario = Request.QueryString["u"];// Session["usuario"].ToString();
            idAlmacen = Convert.ToInt32(Request.QueryString["p"]);
            idCaja = Convert.ToInt32(Request.QueryString["c"]);
            lblUsuario.Text = usuario;
            lblCaja.Text = idCaja.ToString();
            lblIsla.Text = idAlmacen.ToString();
            GridGastos.DataBind();
        }
        catch (Exception ex)
        {
            lblUsuario.Text = "0";
            lblCaja.Text = "0";
            lblIsla.Text = "0";
            GridGastos.DataSource = null;
            GridGastos.DataBind();
        }
    }

    protected void btnNuevoGasto_Click(object sender, EventArgs e)
    {
        try
        {
            usuario = lblUsuario.Text;
            idAlmacen = Convert.ToInt32(lblIsla.Text);
            idCaja = Convert.ToInt32(lblCaja.Text);
            decimal importe = Convert.ToDecimal(txtMonto.Text);
            string justificacion = txtMotivo.Text;
            string referencia = txtReferencia.Text;
            bool nuevoGasto = datos.agregaGasto(idAlmacen, idCaja, usuario, importe, justificacion, referencia);
            if (nuevoGasto)
            {
                lblErrores.Text = "";
                txtMonto.Text = "";
                txtMotivo.Text = "";
                txtReferencia.Text = "";
                GridGastos.DataBind();
            }
            else
                lblErrores.Text = "No se logro agregar el gasto, verifique su conexión e intentelo nuevamente mas tarde.";
        }
        catch (Exception)
        {

        }
    }
}