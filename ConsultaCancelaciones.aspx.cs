using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using E_Utilities;

public partial class ConsultaCancelaciones : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    DateTime fechaIni, fechaFin;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fechaIni = Convert.ToDateTime(fechas.obtieneFechaLocal().Year.ToString() + "-" + fechas.obtieneFechaLocal().Month + "-01");
            fechaFin = fechaIni.AddMonths(1);
            fechaFin = fechaFin.AddDays(-1);
            txtFechaIni.Text = fechaIni.ToString("yyyy-MM-dd");
            txtFechaFin.Text = fechaFin.ToString("yyyy-MM-dd");
            lblCaja.Text = "Seleccione una caja para mostrar sus cancelaciones";
            lblTicket.Text = "Seleccione una cancelacion para mostrar sus productos";
        }
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        fechaIni = Convert.ToDateTime(txtFechaIni.Text);
        fechaFin = Convert.ToDateTime(txtFechaFin.Text);
        DateTime fechaActual = Convert.ToDateTime(fechas.obtieneFechaLocal().ToString("yyyy-MM-dd"));
        if (fechaFin < fechaIni)
            lblError.Text = "La fecha final no puede ser menor a la inicial";
        else if (fechaIni > fechaFin)
            lblError.Text = "La fecha inicial no puede ser superior a la inicial";
        else
        {
            GridView1.SelectedIndex = -1;
            GridView1.DataBind();
            lblCaja.Text = "Seleccione una caja para mostrar sus cancelaciones";
            lblTicket.Text = "Seleccione una cancelacion para mostrar sus productos";
            GridView2.SelectedIndex = -1;
            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView3.DataSource = null;
            GridView3.DataBind();
        }
    }
    protected void lnkCaja_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        string[] argumentos = lnk.CommandArgument.ToString().Split(new char[] { ';' });
        Session["cajas"] = argumentos;
        llenaGridCajas();
    }
    protected void lnkTicket_Click(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        string[] argumentos = lnk.CommandArgument.ToString().Split(new char[] { ';' });
        Session["tickets"] = argumentos;
        llenaTickets();

    }
    protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView2.PageIndex = e.NewPageIndex;
        llenaGridCajas();
    }

    protected void GridView3_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView3.PageIndex = e.NewPageIndex;
        llenaTickets();
    }

    private void llenaGridCajas()
    {
        string[] argumentos = null;
        try
        {
            argumentos = (string[])Session["cajas"];
            BaseDatos data = new BaseDatos();
            lblCaja.Text = "Caja: " + argumentos[0];
            string sql = string.Format("select id_cancelacion,id_punto,id_caja,ticket,id_caja_ticket,fecha,hora,usuario,total from cancelaciones_enc where id_caja={1} and id_punto={0}", argumentos[2], argumentos[0]);
            object[] datos = data.scalarData(sql);
            if (Convert.ToBoolean(datos[0]))
            {
                GridView2.EmptyDataText = "No existen cancelaciones para la caja: " + argumentos[0];
                DataSet valores = (DataSet)datos[1];
                GridView2.SelectedIndex = -1;
                GridView2.DataSource = valores;
                lblTicket.Text = "Seleccione una cancelacion para mostrar sus productos";
                GridView2.DataBind();
                GridView3.DataSource = null;
                GridView3.EmptyDataText = "No existen artículos cancelados para mostrar";
                GridView3.DataBind();
            }
            else
            {
                GridView2.EmptyDataText = "No existen cancelaciones para la caja: " + argumentos[0];
                lblTicket.Text = "Seleccione una cancelacion para mostrar sus productos";
                GridView2.DataSource = null;
                GridView2.DataBind();
                GridView3.DataSource = null;
                GridView3.DataBind();
            }
        }
        catch (Exception)
        {
            GridView2.EmptyDataText = "No existen cancelaciones para la caja: " + argumentos[0];
            lblTicket.Text = "Seleccione una cancelacion para mostrar sus productos";
            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView3.DataSource = null;
            GridView3.DataBind();
        }

    }

    private void llenaTickets()
    {
        string[] argumentos = null;
        try
        {
            argumentos = (string[])Session["tickets"];
            BaseDatos data = new BaseDatos();
            lblTicket.Text = "Cancelación: " + argumentos[0];
            string sql = string.Format("select renglon,cantidad,idProducto,descripcion,precio_unitario,importe from Cancelaciones_det where id_cancelacion={0}", argumentos[0]);
            object[] datos = data.scalarData(sql);
            if (Convert.ToBoolean(datos[0]))
            {
                GridView3.EmptyDataText = "No existen artículos en la cancelación: " + argumentos[0];
                DataSet valores = (DataSet)datos[1];
                GridView3.DataSource = valores;
                GridView3.DataBind();
            }
            else
            {
                GridView3.EmptyDataText = "No existen artículos cancelados para mostrar";
                GridView3.DataSource = null;
                GridView3.DataBind();
            }
        }
        catch (Exception)
        {
            GridView3.EmptyDataText = "No existen artículos cancelados para mostrar";
            GridView3.DataSource = null;
            GridView3.DataBind();
        }
    }
   
    protected void ddlIslas_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }

}