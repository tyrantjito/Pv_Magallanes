using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using E_Utilities;

public partial class VentasUsuarios : System.Web.UI.Page
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
            //llenaGridCajasUusario();
            //obtieneTotales();
            lblCaja.Text = "Seleccione una caja para mostrar su información de venta";
            lblTicket.Text = "Seleccione un ticket para mostrar su información";
            lblEfectivo.Text = Convert.ToDecimal("0").ToString("C2");
            lblCredito.Text = Convert.ToDecimal("0").ToString("C2");
            lblDebito.Text = Convert.ToDecimal("0").ToString("C2");
            lblRecargas.Text = Convert.ToDecimal("0").ToString("C2");
            lblPagoServicios.Text = Convert.ToDecimal("0").ToString("C2");
            lblGastos.Text = Convert.ToDecimal("0").ToString("C2");
            lblCancelaciones.Text = Convert.ToDecimal("0").ToString("C2");
            lblTotalUsuario.Text = Convert.ToDecimal("0").ToString("C2");
        }
    }

    private void obtieneTotales()
    {
        try
        {
            string punto = "";

            if (ddlIslas.SelectedValue != "")
            {
                if (ddlIslas.SelectedValue != "0")
                    punto = " and c.id_punto=" + ddlIslas.SelectedValue;
            }
            string vendedores2="";
            int selecccionados = 0;
            string users = "";
            int usuariosLista = chkListUsuarios.Items.Count;

            foreach (ListItem user in chkListUsuarios.Items)
            {
                if (user.Selected)
                {
                    selecccionados++;
                    users = users + "'" + user.Value.ToString() + "',";
                }
            }

            if (selecccionados > 0)
            {
                users = users.Substring(0, users.Length - 1);                
                vendedores2 = " and upper(usuario) in (" + users + ")";
            }
           
            BaseDatos data = new BaseDatos();
            string sql = string.Format("select sum(tabla.efectivo) as efectivo,sum(tabla.t_credito) as credito, sum(tabla.t_debito) as debito,sum(tabla.pagoServicios) as servicios, sum(tabla.recargas) as recargas,sum(tabla.gastos) as gastos,sum(tabla.cancelaciones) as cancelaciones from(" +
"select c.anio, c.id_caja, Upper(c.usuario) as usuario, Convert(char(10), c.fecha_apertura, 126) as fecha_apertura, Convert(char(10), c.hora_apertura, 108) as hora_apertura," +
"Convert(char(10), c.fecha_cierre, 126) as fecha_cierre, Convert(char(10), c.hora_cierre, 108) as hora_cierre, c.efectivo, c.t_credito, c.t_debito, c.saldo_corte, c.id_punto, p.nombre_pv" +
", isnull((select sum(isnull(cast(monto as decimal), 0)) from pagos_servicios where recarga = 1 and id_punto = c.id_punto AND ID_CAJA = c.id_caja and codigo = '01'), 0) as recargas" +
", isnull((select sum(isnull(cast(monto as decimal), 0)) from pagos_servicios where recarga = 0 and id_punto = c.id_punto AND ID_CAJA = c.id_caja and codigo = '01'),0)as pagoServicios" +
",isnull((select sum(importe) from gastos where id_caja = c.id_caja and idalmacen = c.id_punto),0) as gastos" +
",isnull((select sum(ce.total) from cancelaciones_enc ce where ce.id_caja = c.id_caja and ce.id_punto = c.id_punto),0) as cancelaciones " +
"from cajas c  left join punto_venta p on p.id_punto = c.id_punto  where c.fecha_apertura between '" + fechaIni.ToString("yyyy-MM-dd") + "' and '" + fechaFin.ToString("yyyy-MM-dd") + "' " + vendedores2 + " " + punto + ") as tabla");

            object[] datos = data.scalarData(sql);
            if (Convert.ToBoolean(datos[0]))
            {                
                DataSet valores = (DataSet)datos[1];
                foreach (DataRow fila in valores.Tables[0].Rows) {
                    lblEfectivo.Text = Convert.ToDecimal(fila[0]).ToString("C2");
                    lblCredito.Text = Convert.ToDecimal(fila[1]).ToString("C2");
                    lblDebito.Text = Convert.ToDecimal(fila[2]).ToString("C2");
                    lblGastos.Text = Convert.ToDecimal(fila[5]).ToString("C2");
                    lblCancelaciones.Text = Convert.ToDecimal(fila[6]).ToString("C2");
                    lblRecargas.Text = Convert.ToDecimal(fila[4]).ToString("C2");
                    lblPagoServicios.Text = Convert.ToDecimal(fila[3]).ToString("C2");
                    decimal total = Convert.ToDecimal(fila[0]) + Convert.ToDecimal(fila[1]) + Convert.ToDecimal(fila[2]) + Convert.ToDecimal(fila[3]) + Convert.ToDecimal(fila[4]) - Convert.ToDecimal(fila[5]) - Convert.ToDecimal(fila[6]);
                    lblTotalUsuario.Text = total.ToString("C2");
                }
            }
            else
            {
                lblEfectivo.Text = Convert.ToDecimal("0").ToString("C2");
                lblCredito.Text = Convert.ToDecimal("0").ToString("C2");
                lblDebito.Text = Convert.ToDecimal("0").ToString("C2");
                lblRecargas.Text = Convert.ToDecimal("0").ToString("C2");
                lblPagoServicios.Text = Convert.ToDecimal("0").ToString("C2");
                lblGastos.Text = Convert.ToDecimal("0").ToString("C2");
                lblCancelaciones.Text = Convert.ToDecimal("0").ToString("C2");
                lblTotalUsuario.Text = Convert.ToDecimal("0").ToString("C2");
            }
        }
        catch (Exception)
        {
            lblEfectivo.Text = Convert.ToDecimal("0").ToString("C2");
            lblCredito.Text = Convert.ToDecimal("0").ToString("C2");
            lblDebito.Text = Convert.ToDecimal("0").ToString("C2");
            lblRecargas.Text = Convert.ToDecimal("0").ToString("C2");
            lblPagoServicios.Text = Convert.ToDecimal("0").ToString("C2");
            lblGastos.Text = Convert.ToDecimal("0").ToString("C2");
            lblCancelaciones.Text = Convert.ToDecimal("0").ToString("C2");
            lblTotalUsuario.Text = Convert.ToDecimal("0").ToString("C2");
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
            llenaGridCajasUusario(0);
            obtieneTotales();
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

    protected void ddlEstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        llenaGridCajas();
    }

    private void llenaGridCajas()
    {
        string[] argumentos = null;
        try
        {
            argumentos = (string[])Session["cajas"];
            BaseDatos data = new BaseDatos();
            lblCaja.Text = "Caja: " + argumentos[0];
            string sql = string.Format("select ticket,id_caja,id_punto,subtotal,iva,total, Convert(char(10),hora_venta,108) as hora_venta,usuario,case forma_pago when 'E' then 'Efectivo' when 'D' THEN 'T. Débito' when 'A' then 'T. Crédito' else '' end as forma_pago,case estatus when 'A' then 'Abierto' when 'F' then 'Facturado' when 'C' then 'Cancelado' else '' end as estatus from venta_enc where id_punto={0} and id_caja={2} and estatus='{3}'", argumentos[2], argumentos[1], argumentos[0], ddlEstatus.SelectedValue);
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
        catch (Exception)
        {
            GridView2.EmptyDataText = "No existen ventas para la caja: " + argumentos[0];
            lblTicket.Text = "Seleccione un ticket para mostrar su información";
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
        catch (Exception)
        {
            GridView3.EmptyDataText = "No existen artículos vendidos para mostrar";
            GridView3.DataSource = null;
            GridView3.DataBind();
        }
    }

    private void llenaGridCajasUusario(int index)
    {        
        try
        {
            string punto = "";

            if (ddlIslas.SelectedValue != "")
            {
                if (ddlIslas.SelectedValue != "0")
                    punto = " and c.id_punto=" + ddlIslas.SelectedValue;
            }
            string vendedores = "";
            int selecccionados =0;
            string users = "";
            int usuariosLista = chkListUsuarios.Items.Count;

            foreach (ListItem user in chkListUsuarios.Items)
            {
                if (user.Selected)
                {
                    selecccionados++;                   
                    users = users +"'"+ user.Value.ToString() + "',";                    
                }
            }

            if (selecccionados > 0)
            {
                users = users.Substring(0, users.Length - 1);
                vendedores = " and upper(c.usuario) in (" + users + ")";
            }
            


            BaseDatos data = new BaseDatos();
            string sql = string.Format("select tabla.anio,tabla.id_caja,tabla.usuario,tabla.fecha_apertura,tabla.hora_apertura,tabla.fecha_cierre,tabla.hora_cierre,tabla.efectivo,tabla.t_credito,tabla.t_debito,tabla.id_punto,tabla.nombre_pv,tabla.recargas,tabla.pagoServicios,tabla.gastos,tabla.cancelaciones,cast((tabla.efectivo+tabla.t_credito+tabla.t_debito+tabla.recargas+tabla.pagoServicios-tabla.gastos-tabla.cancelaciones) as decimal(15,2)) as saldo_corte from(" +
                "select c.anio,c.id_caja,Upper(c.usuario) as usuario,Convert(char(10),c.fecha_apertura,126) as fecha_apertura,Convert(char(10),c.hora_apertura,108) as hora_apertura," +
  "Convert(char(10),c.fecha_cierre,126) as fecha_cierre,Convert(char(10),c.hora_cierre,108) as hora_cierre,c.efectivo,c.t_credito,c.t_debito,c.saldo_corte,c.id_punto,p.nombre_pv " +
  ",isnull((select sum(isnull(cast(monto as decimal(15,2)),0)) from pagos_servicios where recarga=1 and id_punto=c.id_punto AND ID_CAJA=c.id_caja and codigo='01'),0)as recargas" +
  ",isnull((select sum(isnull(cast(monto as decimal(15,2)),0)) from pagos_servicios where recarga=0 and id_punto=c.id_punto AND ID_CAJA=c.id_caja and codigo='01'),0)as pagoServicios " +
  ",isnull(( select sum(importe) from gastos where id_caja =c.id_caja and idalmacen=c.id_punto),0) as gastos " +
", isnull((select sum(ce.total) from cancelaciones_enc ce where ce.id_caja = c.id_caja and ce.id_punto = c.id_punto), 0) as cancelaciones " +
  "from cajas c  left join punto_venta p on p.id_punto=c.id_punto  where c.fecha_apertura between '" + txtFechaIni.Text + "' and '" + txtFechaFin.Text + "' " + vendedores + "  " + punto + " ) as tabla order by tabla.id_caja desc");


            SqlDataSource1.SelectCommand = sql;
            GridView1.DataSourceID = "SqlDataSource1";
            GridView1.PageIndex = index;         
            GridView1.DataBind();

            lblCaja.Text = "Seleccione una caja para mostrar su información de venta";
            lblTicket.Text = "Seleccione un ticket para mostrar su información";
            GridView2.SelectedIndex = -1;
            GridView2.DataSource = null;
            GridView2.DataBind();
            GridView3.DataSource = null;
            GridView3.DataBind();
        }
        catch (Exception)
        {
            GridView1.DataSource = null;
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

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //GridView1.PageIndex = e.NewPageIndex;
        llenaGridCajasUusario(e.NewPageIndex);
    }


    

    protected void ddlIslas_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblEfectivo.Text = Convert.ToDecimal("0").ToString("C2");
        lblCredito.Text = Convert.ToDecimal("0").ToString("C2");
        lblDebito.Text = Convert.ToDecimal("0").ToString("C2");
        lblRecargas.Text = Convert.ToDecimal("0").ToString("C2");
        lblPagoServicios.Text = Convert.ToDecimal("0").ToString("C2");
        lblGastos.Text = Convert.ToDecimal("0").ToString("C2");
        lblCancelaciones.Text = Convert.ToDecimal("0").ToString("C2");
        lblTotalUsuario.Text = Convert.ToDecimal("0").ToString("C2");
        lblCaja.Text = "Seleccione una caja para mostrar su información de venta";
        lblTicket.Text = "Seleccione un ticket para mostrar su información";
        GridView1.EmptyDataText = "Seleccione una Tienda e indique que usuario y/o usuario quiere consultar";
        GridView1.DataSource = null;
        GridView1.DataBind();
        GridView3.EmptyDataText = "No existen artículos vendidos para mostrar";
        GridView2.EmptyDataText= "Seleccione una caja para mostrar su información de venta";
        GridView2.SelectedIndex = -1;
        GridView2.DataSource = null;
        GridView2.DataBind();
        GridView3.DataSource = null;
        GridView3.DataBind();
    }
}