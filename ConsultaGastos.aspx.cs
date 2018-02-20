using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;

public partial class ConsultaGastos : System.Web.UI.Page
{
    decimal montoTotal;
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            montoTotal = 0;
            txtFechaFin.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
            txtFechaIni.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
            lblError.Text = "";
            lblMontoTotal.Text = 0.ToString("C");
            cargaGrid();
        }
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        cargaGrid();
    }

    private void cargaGrid()
    {
        try
        {
            lblMontoTotal.Text = 0.ToString("C");
            string diaIni = txtFechaIni.Text;
            string diaFin = txtFechaFin.Text;
            DateTime fechaIni = Convert.ToDateTime(diaIni);
            DateTime fechaFin = Convert.ToDateTime(diaFin);
            if (fechaIni <= fechaFin)
            {
                if (fechaIni < fechas.obtieneFechaLocal())
                {
                    if (fechaFin < fechas.obtieneFechaLocal())
                    {
                        string isla = ddlIslas.SelectedValue;
                        string usuario = ddlUsuario.SelectedValue;
                        string islas = obtieneIslas();
                        string whereSlq = "";
                        string condicion = "";
                        if (islas != "")
                            condicion = " and g.idAlmacen in(" + islas + ") ";
                        if (ddlIslas.SelectedValue == "T" && ddlUsuario.SelectedValue == "T")
                            whereSlq = "";
                        else if (ddlIslas.SelectedValue != "T" && ddlUsuario.SelectedValue == "T")
                            whereSlq = "g.idAlmacen = " + ddlIslas.SelectedValue + " and ";
                        else if (ddlIslas.SelectedValue != "T" && ddlUsuario.SelectedValue != "T")
                            whereSlq = "g.idAlmacen = " + ddlIslas.SelectedValue + " and g.usuario = '" + ddlUsuario.SelectedValue + "' and ";
                        else if (ddlIslas.SelectedValue == "T" && ddlUsuario.SelectedValue != "T")
                            whereSlq = "g.usuario = '" + ddlUsuario.SelectedValue + "' and ";
                        string sql = "select g.id_gasto,g.fecha,g.hora,g.idAlmacen,c.nombre,g.usuario,g.referencia,g.justificacion,g.importe,(u.nombre+' '+u.apellido_pat+' '+isnull(u.apellido_mat,'')) as nombreU" +
                                     " from gastos g" +
                                     " inner join catalmacenes c on c.idAlmacen = g.idAlmacen" +
                                     " inner join usuarios_PV u on u.usuario = g.usuario" +
                                     " where " + whereSlq + " g.fecha between '" + diaIni + "' and '" + diaFin + "' " + condicion +
                                     " order by g.id_gasto asc";
                        BaseDatos ejecuta = new BaseDatos();
                        DataSet data = new DataSet();
                        object[] ejecutado = ejecuta.scalarData(sql);
                        if ((bool)ejecutado[0])
                            data = (DataSet)ejecutado[1];
                        GridGastos.DataSource = data;
                        GridGastos.DataBind();
                    }
                    else
                    {
                        txtFechaFin.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                        lblError.Text = "La fecha final no puede ser mayor al dia actual.";
                    }
                }
                else
                {
                    txtFechaIni.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                    lblError.Text = "La fecha inicial no puede ser mayor al dia actual.";
                }
            }
            else
            {
                txtFechaIni.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                txtFechaFin.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                lblError.Text = "La fecha inicial no puede ser mayor a la fecha final.";
            }
        }
        catch (Exception)
        {
            GridGastos.DataSource = null;
            GridGastos.DataBind();
        }
    }

    private string obtieneIslas()
    {
        string islas = "";
        try
        {
            BaseDatos ejecuta = new BaseDatos();
            DataSet data = new DataSet();
            string sql = "select u.id_punto,p.nombre_pv from usuario_puntoventa U inner join punto_venta p on p.id_punto = u.id_punto where U.usuario = '" + Request.QueryString["u"] + "' and U.estatus = 'A'";
            object[] ejecutado = ejecuta.scalarData(sql);
            if (Convert.ToBoolean(ejecutado[0]))
            {                
                data = (DataSet)ejecutado[1];                
                foreach (DataRow r in data.Tables[0].Rows) {
                    islas = islas + r[0].ToString() + ",";                    
                }
                if (islas != "")
                    islas = islas.Substring(0, islas.Length - 1);
            }
        }
        catch (Exception) { islas = ""; }
        return islas;
    }

    protected void GridGastos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.DataItemIndex != -1)
            {
                if (ddlIslas.SelectedValue == "T" && ddlUsuario.SelectedValue == "T")
                    montoTotal += Convert.ToDecimal((e.Row.Cells[6].FindControl("lblImporte") as Label).Text);
                else if (ddlIslas.SelectedValue != "T" && ddlUsuario.SelectedValue == "T")
                {
                    GridGastos.Columns[2].Visible = false;
                    montoTotal += Convert.ToDecimal((e.Row.Cells[5].FindControl("lblImporte") as Label).Text);
                }
                else if (ddlIslas.SelectedValue != "T" && ddlUsuario.SelectedValue != "T")
                {
                    GridGastos.Columns[2].Visible = false;
                    GridGastos.Columns[3].Visible = false;
                    montoTotal += Convert.ToDecimal((e.Row.Cells[4].FindControl("lblImporte") as Label).Text);
                }
                else if (ddlIslas.SelectedValue == "T" && ddlUsuario.SelectedValue != "T")
                {
                    GridGastos.Columns[3].Visible = false;
                    montoTotal += Convert.ToDecimal((e.Row.Cells[5].FindControl("lblImporte") as Label).Text);
                }
                lblMontoTotal.Text = montoTotal.ToString("C");
            }
        }
        catch (Exception ex) { }
    }

    protected void GridGastos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridGastos.PageIndex = e.NewPageIndex;
        cargaGrid();
    }
}