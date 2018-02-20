using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UsuariosPunto : System.Web.UI.Page
{
    string usuarioLog;
    UsuPv datosUsu = new UsuPv();
    protected void Page_Load(object sender, EventArgs e)
    {        
        checaSesiones();
    }

    private void checaSesiones()
    {
        try { usuarioLog = Convert.ToString(Session["usuario"]); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
    }
              
    
    protected void ddlUsuario_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrores.Text = "";
        grdIslas.DataBind();
        GridView1.DataBind();
    }

    protected void lnkIsla_Click(object sender, EventArgs e)
    {
        LinkButton boton = (LinkButton)sender;
        string pv = boton.CommandArgument;
        int isla = 0;
        try { isla = Convert.ToInt32(pv); }
        catch (Exception) { isla = 0; }
        if (isla != 0)
        {
            string usuario = ddlUsuario.SelectedValue;
            UsuPv usuarioIsla = new UsuPv();
            usuarioIsla.Usuario = usuario;
            usuarioIsla.Punto = isla;
            usuarioIsla.agregaIslaUsuario();
            if (usuarioIsla.Error != "")
                lblErrores.Text = usuarioIsla.Error;
            else
            {
                grdIslas.DataBind();
                GridView1.DataBind();
            }
        }
        else
            lblErrores.Text = "Seleccione un usuario y una Tienda para realizar la asignación";
    }
    protected void lnkIslaDel_Click(object sender, EventArgs e)
    {
        LinkButton boton = (LinkButton)sender;
        string pv = boton.CommandArgument;
        int isla = 0;
        try { isla = Convert.ToInt32(pv); }
        catch (Exception) { isla = 0; }
        if (isla != 0)
        {
            string usuario = ddlUsuario.SelectedValue;
            UsuPv usuarioIsla = new UsuPv();
            usuarioIsla.Usuario = usuario;
            usuarioIsla.Punto = isla;
            usuarioIsla.eliminaIslaUsuario();
            if (usuarioIsla.Error != "")
                lblErrores.Text = usuarioIsla.Error;
            else
            {
                grdIslas.DataBind();
                GridView1.DataBind();                
            }
        }
        else
            lblErrores.Text = "Seleccione un usuario y una Tienda para realizar la desasignación";
    }
    protected void chkAgregaTodas_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAgregaTodas.Checked) {
            UsuPv usuIs = new UsuPv();
            usuIs.Usuario = ddlUsuario.SelectedValue;
            DataSet islas = usuIs.llenaIsla();
            int agregados = 0, numIslas = 0;

            if (islas != null) {
                numIslas = islas.Tables[0].Rows.Count;
                if (numIslas != 0)
                {
                    foreach (DataRow fila in islas.Tables[0].Rows)
                    {
                        usuIs.Punto = Convert.ToInt32(fila[0].ToString());
                        usuIs.agregaIslaUsuario();
                        if (usuIs.Error == "")
                            agregados++;
                    }
                    lblErrores.Text = "Se asignaron " + agregados.ToString() + " de " + numIslas.ToString() + " Tiendas disponibles";
                    chkAgregaTodas.Checked = false;
                    GridView1.DataBind();
                    grdIslas.DataBind();
                }
            }
        }

    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBox1.Checked) {
            UsuPv usuIs = new UsuPv();
            usuIs.Usuario = ddlUsuario.SelectedValue;
            DataSet islas = usuIs.llenaIslasUsuario();
            int agregados = 0, numIslas = 0;

            if (islas != null)
            {
                numIslas = islas.Tables[0].Rows.Count;
                if (numIslas != 0)
                {
                    foreach (DataRow fila in islas.Tables[0].Rows)
                    {
                        usuIs.Punto = Convert.ToInt32(fila[0].ToString());
                        usuIs.eliminaIslaUsuario();
                        if (usuIs.Error == "")
                            agregados++;
                    }
                    lblErrores.Text = "Se desasignaron " + agregados.ToString() + " de " + numIslas.ToString() + " Tiendas disponibles";
                    CheckBox1.Checked = false;
                    GridView1.DataBind();
                    grdIslas.DataBind();
                }
            }
        }
    }
}