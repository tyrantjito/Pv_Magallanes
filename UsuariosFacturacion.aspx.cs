using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UsuariosFacturacion : System.Web.UI.Page
{
    BaseDatos ejecuta = new BaseDatos();
    string sql;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            lblError.Text = "";
    }

    protected void ddlUsuarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridPermisos.DataBind();
    }

    protected void GridPermisos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "permiso")
            {
                int idIsla = Convert.ToInt32(e.CommandArgument);
                string usuario = ddlUsuarios.SelectedValue.ToString();
                sql = "insert into Usuarios_Facturacion values(" + idIsla.ToString() + ",'" + usuario + "')";
                object[] ejecutado = ejecuta.insertUpdateDelete(sql);
                bool agregado = false;
                if ((bool)ejecutado[0])
                    agregado = (bool)ejecutado[1];
                else
                    agregado = false;
                if (agregado)
                {
                    GridPermisos.DataBind();
                    GridUsuariosPermisos.DataBind();
                }
                else
                {
                    lblError.Text = "La asignación de la Tienda no se logro efectuar con exito, verifique su conexión e intentelo nuevamente.";
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Hubo un error inesperado: " + ex.Message;
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
                bool eliminado = false;
               sql = "delete from Usuarios_Facturacion where idAlmacen=" + idPermiso.ToString() + " and usuario='" + usuario + "'";
                object[] ejecutado = ejecuta.insertUpdateDelete(sql);
                if ((bool)ejecutado[0])
                    eliminado = (bool)ejecutado[1];
                else
                    eliminado = false;
                if (eliminado)
                {
                    GridPermisos.DataBind();
                    GridUsuariosPermisos.DataBind();
                }
                else
                {
                    lblError.Text = "La Tienda no se logro quitar al usuario con exito, verifique su conexión e intentelo nuevamente.";
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Hubo un error inesperado: " + ex.Message;
        }
    }
}