using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using E_Utilities;

public partial class ConsultaVentas : System.Web.UI.Page
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
            lblCaja.Text = "Seleccione una caja para mostrar su información de venta";
            lblTicket.Text = "Seleccione un ticket para mostrar su información";
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
            lblCaja.Text = "Seleccione una caja para mostrar su información de venta";
            lblTicket.Text = "Seleccione un ticket para mostrar su información";
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

    private void llenaGridCajas() {
        string[] argumentos = null;
        try { 
            argumentos = (string[])Session["cajas"];
            BaseDatos data = new BaseDatos();
            lblCaja.Text = "Caja: " + argumentos[0];
            string sql = string.Format("select ticket,id_caja,id_punto,subtotal,iva,total, Convert(char(10),hora_venta,108) as hora_venta,usuario,case forma_pago when 'E' then 'Efectivo' when 'D' THEN 'T. Débito' when 'A' then 'T. Crédito' else '' end as forma_pago,case estatus when 'A' then 'Abierto' when 'F' then 'Facturado' when 'C' then 'Cancelado' else '' end as estatus,cast((case when orden>0 then 1 else 0 end) as bit) as ventaTaller,cast(isnull(venta_credito,0) as bit) as ventaCredito  from venta_enc where id_punto={0} and id_caja={2} and estatus='{3}'", argumentos[2], argumentos[1], argumentos[0], ddlEstatus.SelectedValue);
            object[] datos = data.scalarData(sql);
            if (Convert.ToBoolean(datos[0]))
            {
                GridView2.EmptyDataText = "No existen ventas para la caja: " + argumentos[0];
                DataSet valores = (DataSet)datos[1];
                GridView2.SelectedIndex = -1;
                GridView2.DataSource = valores;
                lblTicket.Text = "Seleccione un ticket para mostrar su información";
                GridView2.DataBind();
                GridView3.DataSource = null;
                GridView3.EmptyDataText = "No existen artículos vendidos para mostrar";
                GridView3.DataBind();
            }
            else
            {
                GridView2.EmptyDataText = "No existen ventas para la caja: " + argumentos[0];
                lblTicket.Text = "Seleccione un ticket para mostrar su información";
                GridView2.DataSource = null;
                GridView2.DataBind();
                GridView3.DataSource = null;
                GridView3.DataBind();
            }
        }
        catch (Exception) {
            GridView2.EmptyDataText = "No existen ventas para la caja: " + argumentos[0];
            lblTicket.Text = "Seleccione un ticket para mostrar su información";
            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView3.DataSource = null;
            GridView3.DataBind();
        }
        
    }

    private void llenaTickets() {
         string[] argumentos = null;
         try
         {
             argumentos = (string[])Session["tickets"];
             BaseDatos data = new BaseDatos();
             lblTicket.Text = "Ticket: " + argumentos[0];
             string sql = string.Format("SELECT V.renglon,V.cantidad,V.id_refaccion,V.descripcion,V.venta_unitaria,V.importe FROM venta_det V WHERE V.id_punto={0} AND V.ticket={1}", argumentos[1], argumentos[0]);
             object[] datos = data.scalarData(sql);
             if (Convert.ToBoolean(datos[0]))
             {
                 GridView3.EmptyDataText = "No existen artículos en el ticket: " + argumentos[0];
                 DataSet valores = (DataSet)datos[1];
                 GridView3.DataSource = valores;
                 GridView3.DataBind();
             }
             else
             {
                 GridView3.EmptyDataText = "No existen artículos vendidos para mostrar";
                 GridView3.DataSource = null;
                 GridView3.DataBind();
             }
         }
         catch (Exception) {
             GridView3.EmptyDataText = "No existen artículos vendidos para mostrar";
             GridView3.DataSource = null;
             GridView3.DataBind();
         }
    }
    protected void ddlEstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        llenaGridCajas();
    }
    protected void ddlIslas_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridView1.DataBind();
        lblCaja.Text = "Seleccione una caja para mostrar su información de venta";
        lblTicket.Text = "Seleccione un ticket para mostrar su información";
        GridView2.EmptyDataText = "Seleccione una caja para mostrar su información de venta";
        GridView3.EmptyDataText = "No existen artículos vendidos para mostrar";
        GridView2.SelectedIndex = -1;
        GridView2.DataSource = null;
        GridView2.DataBind();
        GridView3.DataSource = null;
        GridView3.DataBind();
    }



    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void SqlDataSource2_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            Label lblSaldoCorte = e.Row.FindControl("lblSaldoCorte") as Label;
            Label lblSaldoCorteBind = e.Row.FindControl("lblSaldoCorteBind") as Label;
            Label lblRecargas = e.Row.FindControl("lblRecargas") as Label;
            Label lblPagoServicios = e.Row.FindControl("lblPagoServicios") as Label;
            decimal saldoCorte = Convert.ToDecimal(lblSaldoCorteBind.Text);
            decimal recargas = Convert.ToDecimal(lblRecargas.Text);
            decimal pagoServicios = Convert.ToDecimal(lblPagoServicios.Text);
            lblSaldoCorte.Text = (saldoCorte + recargas + pagoServicios).ToString("F2");
        }
        catch (Exception)
        {
            try
            {
                Label lblSaldoCorte = e.Row.FindControl("lblSaldoCorte") as Label;
                Label lblSaldoCorteBind = e.Row.FindControl("lblSaldoCorteBind") as Label;
                lblSaldoCorte.Text = lblSaldoCorteBind.Text;
            }
            catch (Exception) { }
        }
    }
}