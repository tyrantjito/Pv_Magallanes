using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;

public partial class ConsultaNotificaciones : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {            
            txtFechaIni.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
            GridView1.DataBind();
        }
    }
    protected void chkLeido_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox check = (CheckBox)sender;
        int valor = Convert.ToInt32(check.ToolTip.ToString());
        int notificacion =valor;        
        Notificaciones noti = new Notificaciones();
        noti.Fecha = Convert.ToDateTime(txtFechaIni.Text);
        noti.Estatus = "V";
        noti.Entrada = notificacion;
        noti.actualizaEstado();
        GridView1.DataBind();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string estatus = DataBinder.Eval(e.Row.DataItem, "estatus").ToString();
            if (e.Row.RowState.ToString() == "Normal" || e.Row.RowState.ToString() == "Alternate")
            {
                var check = e.Row.Cells[7].Controls[1].FindControl("chkLeido") as CheckBox;
                if (estatus == "P")
                {
                    check.Checked = false;
                    check.Visible = true;
                    e.Row.CssClass = "alert-success";
                }
                else {
                    check.Checked = true;
                    check.Visible = false;
                }
            }
        }
    }
    
    protected void btnConsultar_Click(object sender, EventArgs e)
    {
        GridView1.DataBind();
    }
}