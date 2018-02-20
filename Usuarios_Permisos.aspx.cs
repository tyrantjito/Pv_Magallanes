using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Usuarios_Permisos : System.Web.UI.Page
{
    PermisosDatos datos = new PermisosDatos();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ddlUsuarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            GridPermisos.DataBind();
            GridUsuariosPermisos.DataBind();
        }
        catch (Exception ex) {
            GridPermisos.DataSource = GridUsuariosPermisos.DataSource = null;
            GridPermisos.DataBind();
            GridUsuariosPermisos.DataBind();
            lblError.Text = "Error: " + ex.Message;
        }
    }
    
    protected void GridPermisos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "permiso")
            {
                int idPermiso = Convert.ToInt32(e.CommandArgument);
                string usuario = ddlUsuarios.SelectedValue.ToString();
                bool agregado = datos.daPermiso(idPermiso, usuario);
                if(agregado)
                {
                    GridPermisos.DataBind();
                    GridUsuariosPermisos.DataBind();
                }
                else
                {
                    lblError.Text = "El permiso no se logro asignar al usuario con exito, verifique su conexión e intentelo nuevamente.";
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error: " + ex.Message;
        }
    }

    protected void GridUsuariosPermisos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "quitaPermiso")
            {
                int idPermiso = Convert.ToInt32(e.CommandArgument);
                string usuario = ddlUsuarios.SelectedValue.ToString();
                bool agregado = datos.quitaPermiso(idPermiso, usuario);
                if (agregado)
                {
                    GridPermisos.DataBind();
                    GridUsuariosPermisos.DataBind();
                }
                else
                {
                    lblError.Text = "El permiso no se quitar al usuario con exito, verifique su conexión e intentelo nuevamente.";
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error: " + ex.Message;
        }
    }
}