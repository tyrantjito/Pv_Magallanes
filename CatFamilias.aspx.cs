using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CatFamilias : System.Web.UI.Page
{
    string usuarioLog;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                GridView1.DataBind();
            }
            catch (Exception)
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }
        checaSesiones();
    }

    private void checaSesiones()
    {
        try { usuarioLog = Convert.ToString(Session["usuario"]); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
    }


    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView grilla = (GridView)sender;
        int index = -1;
        string clave = "";
        if (e.CommandName == "Update")
        {
            index = grilla.EditIndex;
            clave = grilla.DataKeys[index].Value.ToString();
            var descripcion = grilla.Rows[index].FindControl("txtDescMod") as TextBox;

            SqlDataSource1.UpdateCommand = "update catfamilias set descripcionFamilia=@descripcionFamilia where idFamilia=@idFamilia";
            SqlDataSource1.UpdateParameters.Add("idFamilia", clave);
            SqlDataSource1.UpdateParameters.Add("descripcionFamilia", descripcion.Text);
            try
            {                
                grilla.EditIndex = -1;
                grilla.DataBind();
            }
            catch (Exception ex)
            {
                lblErrores.Text = "Error al actualizar la familia " + clave + ": " + ex.Message;
            }

        }
        else if (e.CommandName == "Delete")
        {
            clave = e.CommandArgument.ToString();
            try
            {
                string sql = string.Format("delete from catfamilias where rtrim(ltrim(idFamilia))='{0}'", clave);
                SqlDataSource1.DeleteCommand = sql;
                grilla.DataBind();
            }
            catch (Exception ex)
            {
                lblErrores.Text = "Error al eliminar la familia " + clave + ": " + ex.Message;
            }
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            checaSesiones();
            if (usuarioLog != null)
            {
                if (e.Row.RowState.ToString() == "Normal" || e.Row.RowState.ToString() == "Alternate")
                {
                    var btnEliminar = e.Row.Cells[2].Controls[1].FindControl("btnEliminar") as Button;
                    string clave = DataBinder.Eval(e.Row.DataItem, "idFamilia").ToString();
                    Familias familia = new Familias();
                    familia.Unidad = clave;
                    familia.verificaRelacion();                    
                    if (familia.Relacionado)
                    {
                        btnEliminar.Text = "Elimina";
                        btnEliminar.Enabled = false;
                        btnEliminar.CssClass = "btn-default ancho50px";
                    }
                    else
                    {
                        btnEliminar.OnClientClick = "return confirm('¿Está seguro de eliminar la Familia " + clave + "?')";
                        btnEliminar.Text = "Elimina";
                        btnEliminar.Enabled = true;
                        btnEliminar.CssClass = "btn-danger ancho50px";
                    }
                }
            }
            else
                Response.Redirect("Default.aspx");
        }
    }
    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        lblErrores.Text = "";
        Familias familia = new Familias();
        familia.Unidad = txtClave.Text;
        familia.verificaExiste();
        bool existeFamilia = familia.Existe;
        if (!existeFamilia)
        {
            SqlDataSource1.InsertCommand = "insert into catfamilias(idFamilia,descripcionFamilia) values(@idFamilia,@descripcionFamilia)";
            SqlDataSource1.InsertParameters.Add("idFamilia", txtClave.Text);
            SqlDataSource1.InsertParameters.Add("descripcionFamilia", txtDescripcion.Text);
            try
            {
                SqlDataSource1.Insert();
                GridView1.DataBind();
                txtClave.Text = txtDescripcion.Text = "";
            }
            catch (Exception ex)
            {
                lblErrores.Text = "Error al agregar la familia " + txtClave.Text + ": " + ex.Message;
            }
        }
        else
            lblErrores.Text = "La familia a ingresar ya se encuentra registrada";

    }
}