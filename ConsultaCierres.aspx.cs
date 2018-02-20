using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;

public partial class ConsultaCierres : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {            
        GridCierre.EmptyDataText = "No hay cierres de caja para mostrar.";
        if (!IsPostBack)
        {
            txtFechaFin.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
            txtFechIni.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
            lblError.Text = "";

            cargaGrid();

            GridDesglose.DataSource = null;
            GridDesglose.DataBind();
            GridDesglose.Visible = false;
            lblCierreSelect.Text = "Seleccione No. Cierre";
            lblError.Text = "";
        }
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        cargaGrid();
        GridDesglose.DataSource = null;
        GridDesglose.DataBind();
        GridDesglose.Visible = false;
        lblCierreSelect.Text = "Seleccione No. Cierre";
        lblError.Text = "";
    }

    private void cargaGrid()
    {
        try
        {
            lblError.Text = "";
            

            string diaIni = txtFechIni.Text;
            string diaFin = txtFechaFin.Text;
            DateTime fechaIni = Convert.ToDateTime(diaIni) ;
            DateTime fechaFin = Convert.ToDateTime(diaFin);
            if (fechaIni <= fechaFin)
            {
                if (fechaIni < fechas.obtieneFechaLocal())
                {
                    if (fechaFin < fechas.obtieneFechaLocal())
                    {
                        string isla = ddlIslas.SelectedValue;

                        int selecccionados = 0;
                        string users = "";
                        int usuariosLista = chkListUsuarios.Items.Count;
                        string condicion = "";

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
                            condicion = "upper(c.usuario_cierre) in (" + users + ") AND ";                            
                        }

                                               
                        string islaNom = ddlIslas.SelectedItem.Text;
                        string whereSlq = "";
                        if (ddlIslas.SelectedValue == "T")
                            whereSlq = "";
                        else if (ddlIslas.SelectedValue != "T") 
                            whereSlq = "c.id_punto_venta = " + ddlIslas.SelectedValue + " and ";                        
                        string sql = "select c.id_cierre," +
                                     " CONVERT(char(10), c.fecha_cierre, 126) + ' ' + CONVERT(char(10), c.hora_cierre, 108) as cierre," +
                                     " c.usuario_cierre,g.nombre," +
                                     " (u.nombre + ' ' + u.apellido_pat + ' ' + isnull(u.apellido_mat, '')) as nombreU," +
                                     " fondo,efectivo,debito,credito,gastos,cancelaciones,total,(c.usuario_cierre+';'+cast(c.id_punto_venta as varchar)+';'+CONVERT(char(10), c.fecha_cierre,126))as datosComand" +
                                     " ,c.pagoServicios,c.recargas,c.ventaTaller,c.ventaCredito from cierres_diarios c" +
                                     " inner join usuarios_PV u on u.usuario = c.usuario_cierre" +
                                     " inner join catalmacenes g on c.id_punto_venta = g.idAlmacen" +
                                     " where " + whereSlq + " "+condicion+"  c.fecha_cierre between '" + diaIni + "' and '" + diaFin + "'";
                        BaseDatos ejecuta = new BaseDatos();
                        DataSet data = new DataSet();
                        object[] ejecutado = ejecuta.scalarData(sql);
                        if ((bool)ejecutado[0])
                            data = (DataSet)ejecutado[1];
                        
                        if (data.Tables[0].Rows.Count == 0)
                        {
                            string msgEmptyData = "No hay cierres de caja para mostrar entre el " + fechaIni.ToString("d") + " y " + fechaFin.ToString("d");                            
                            GridCierre.EmptyDataText = msgEmptyData;
                        }
                        GridCierre.DataSource = data;
                        GridCierre.DataBind();
                    }
                    else
                    {
                        txtFechaFin.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                        lblError.Text = "La fecha final no puede ser mayor al dia actual.";
                    }
                }
                else
                {
                    txtFechIni.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                    lblError.Text = "La fecha inicial no puede ser mayor al dia actual.";
                }
            }
            else
            {
                txtFechIni.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                txtFechaFin.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                lblError.Text = "La fecha inicial no puede ser mayor a la fecha final.";
            }
        }
        catch (Exception)
        {
            GridCierre.DataSource = null;
            GridCierre.DataBind();
        }
    }

    protected void GridCierre_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        lblError.Text = "";
        try
        {
            if (e.Row.DataItemIndex != -1)
            {
                if (ddlIslas.SelectedValue != "T")
                    GridCierre.Columns[2].Visible = false;               
            }
        }
        catch (Exception ex) { }
    }

    protected void GridCierre_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        lblError.Text = "";
        GridCierre.PageIndex = e.NewPageIndex;
        cargaGrid();
        GridDesglose.DataSource = null;
        GridDesglose.DataBind();
        GridDesglose.Visible = false;
        lblCierreSelect.Text = "Seleccione No. Cierre";
    }
    /*
    protected void GridCierre_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName== "Desglose")
        {
            Label lblImporte = GridCierre.FindControl("lblImporte") as Label;
            string[] argumentos = lblImporte.Text.ToString().Split(';');

        }
    }*/

    protected void lknCierre_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        try
        {
            LinkButton lnkCierre = (LinkButton)sender;
            string[] argumentos = lnkCierre.CommandArgument.ToString().Split(';');
            string usuario = argumentos[0];
            string isla = argumentos[1];
            string fecha = argumentos[2];
            string cierreSelect = lnkCierre.Text;
            Session["fechaDesglose"] = fecha;
            Session["islaDesglose"] = isla;
            Session["cierreSelectDesglose"] = cierreSelect;
            llenaDesglose(isla,fecha, cierreSelect);
        }
        catch (Exception ex)
        {
            GridDesglose.Visible = false;
            lblCierreSelect.Text = "Seleccione No. Cierre";
            lblError.Text = "Hubo un error al cargar el Desglose. " + ex.Message;
        }
    }

    private void llenaDesglose(string isla, string fecha, string cierreSelect)
    {
        string sql = "select c.usuario,count(c.id_caja)as id_cajaS," +
                     " (u.nombre + ' ' + u.apellido_pat + ' ' + isnull(u.apellido_mat, '')) as nombre," +
                     " sum(c.efectivo) as efectivoS ,sum(c.t_credito) as t_creditoS,sum(c.t_debito) as t_debitoS,sum(c.t_gastos) as t_gastosS,sum(c.t_cancelacion) as t_cancelacion," +
                     " (sum(c.efectivo) + sum(c.t_credito) + sum(c.t_debito) - sum(c.t_gastos) - sum(c.t_cancelacion)) as total,c.recargas,c.pagoServicios,c.ventaTaller,c.ventaCredito " +
                     " from cajas c" +
                     " inner join usuarios_PV u on u.usuario = c.usuario" +
                     " where c.id_cierre = " + cierreSelect + " and c.id_punto = " + isla+
                     " GROUP BY c.usuario,u.nombre,u.apellido_pat,u.apellido_mat,c.recargas,c.pagoServicios";
        BaseDatos ejecuta = new BaseDatos();
        DataSet data = new DataSet();
        object[] ejecutado = ejecuta.scalarData(sql);
        if ((bool)ejecutado[0])
            data = (DataSet)ejecutado[1];
        else
            data = null;
        lblCierreSelect.Text = "No. Cierre: " + cierreSelect;
        GridDesglose.Visible = true;
        GridDesglose.DataSource = data;
        GridDesglose.DataBind();
    }

    protected void GridDesglose_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            GridDesglose.PageIndex = e.NewPageIndex;
            string fecha = Session["fechaDesglose"].ToString();
            string isla = Session["islaDesglose"].ToString();
            string cierreSelect = Session["cierreSelectDesglose"].ToString();
            llenaDesglose(isla, fecha, cierreSelect);
        }
        catch(Exception)
        {
            cargaGrid();
            GridDesglose.DataSource = null;
            GridDesglose.DataBind();
            GridDesglose.Visible = false;
            lblCierreSelect.Text = "Seleccione No. Cierre";
        }
    }
    protected void btnGenPDF_Click(object sender, EventArgs e)
    {
        ImprimeCierres impTicket = new ImprimeCierres();
        impTicket.fecha_Ini = txtFechIni.Text;
        impTicket.fecha_Fin = txtFechaFin.Text;
        impTicket.Usuario = Convert.ToString(Request.QueryString["u"]);
        impTicket.Isla = ddlIslas.SelectedValue;

        int selecccionados = 0;
        string users = "";
        int usuariosLista = chkListUsuarios.Items.Count;
        string condicion = "";

        foreach (ListItem user in chkListUsuarios.Items)
        {
            if (user.Selected)
            {
                selecccionados++;
                users = users + "'" + user.Value.ToString() + "',";
            }
        }

        if (selecccionados > 0)
            users = users.Substring(0, users.Length - 1);

        impTicket.UsuSelec = users;
        impTicket.NombreIsla = ddlIslas.SelectedItem.Value;
        string Archivo = impTicket.GenerarTicket();
        if (Archivo != "")
        {
            //if (GridView1.Rows.Count != 0)
            //{
                try
                {
                    System.IO.FileInfo filename = new System.IO.FileInfo(Archivo);
                    if (filename.Exists)
                    {
                        string url = "TicketPdf.aspx?a=" + filename.Name;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "pdfs", "window.open('" + url + "', 'nuevo', 'directories=no, location=no, menubar=no, scrollbars=yes, statusbar=no, titlebar=no, width=856px, height=550px');", true);

                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                }
        }
        else
            lblError.Text = "No existe información para imprimir";
        //}
        /*else
            lblError.Text = "Error al imprimir el cierre, vuelva intentarlo";
         */
    }
}