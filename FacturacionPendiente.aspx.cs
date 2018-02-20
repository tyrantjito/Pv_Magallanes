using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FacturacionPendiente : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try { string usuarioLog = Convert.ToString(Request.QueryString["u"]); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        GridFactPend.DataBind();
    }
    protected void btnActualiza_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        Button boton = (Button)sender;
        string[] argumentos = boton.CommandArgument.ToString().Split(new char[] { ';' });
        string estatus = "A";
        if (argumentos[1] == "A")
            estatus = "F";
        else
            estatus = "A";
        Inventario orden = new Inventario();
        object[] actualizado = orden.actualizaEstatus(Convert.ToInt32(argumentos[0]), Convert.ToInt32(ddlIslas.SelectedValue), estatus);
        if (Convert.ToBoolean(actualizado[0]))
        {
            GridFactPend.DataBind();
            GrdDetalle.DataBind();
        }
        else
        {
            lblError.Text = "Error: " + Convert.ToString(actualizado[1]);
        }
    }
    protected void ddlIslas_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        GridFactPend.DataBind();
        GrdDetalle.DataBind();
    }
    protected void ddlEstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        GridFactPend.DataBind();
        GrdDetalle.DataBind();
    }
    protected void GridFactPend_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        lblError.Text = "";
        GrdDetalle.DataBind();
    }
    protected void GridFactPend_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string estatus = DataBinder.Eval(e.Row.DataItem, "estatus").ToString();
            var btnEstatus = e.Row.Cells[6].Controls[0].FindControl("btnActualiza") as Button;
            if (e.Row.RowState.ToString() == "Normal" || e.Row.RowState.ToString() == "Alternate" || e.Row.RowState.ToString() == "Selected")
            {
                if (estatus == "A")
                    btnEstatus.Text = "Facturado";                
                else 
                {
                    btnEstatus.Text = "Facturado";
                    btnEstatus.CssClass = "btn-default";
                    btnEstatus.Enabled = false;
                }                
            }
        }
    }
}