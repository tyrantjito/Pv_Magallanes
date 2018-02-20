using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CatUnidades : System.Web.UI.Page
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

            SqlDataSource1.UpdateCommand = "update catunidmedida set descripcion=@descripcion where idMedida=@idMedida";
                SqlDataSource1.UpdateParameters.Add("idMedida", clave);
                SqlDataSource1.UpdateParameters.Add("descripcion", descripcion.Text);                
                try
                {
                    //SqlDataSource1.Update();
                    grilla.EditIndex = -1;
                    grilla.DataBind();
                }
                catch (Exception ex)
                {
                    lblErrores.Text = "Error al actualizar la unidad " + clave + ": " + ex.Message;
                }
            
        }
        else if (e.CommandName == "Delete")
        {   
            clave = e.CommandArgument.ToString();
            try
            {
                string sql = string.Format("delete from catunidmedida where rtrim(ltrim(idMedida))='{0}'", clave);
                SqlDataSource1.DeleteCommand = sql;
                grilla.DataBind();               
            }
            catch (Exception ex)
            {
                lblErrores.Text = "Error al eliminar la unidad " + clave + ": " + ex.Message;
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
                    string clave = DataBinder.Eval(e.Row.DataItem, "idMedida").ToString();
                    UnidadesMedida unidades = new UnidadesMedida();
                    unidades.Unidad = clave;
                    unidades.verificaRelacion();
                    if (unidades.Relacionado)
                    {                       
                        btnEliminar.Text = "Elimina";
                        btnEliminar.Enabled = false;
                        btnEliminar.CssClass = "btn-default ancho50px";                        
                    }
                    else
                    {
                        btnEliminar.OnClientClick = "return confirm('¿Está seguro de eliminar la unidad " + clave + "?')";
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
            UnidadesMedida unidades = new UnidadesMedida();
            unidades.Unidad = txtClave.Text;
            unidades.verificaExiste();
            bool existeUnidad = unidades.Existe;
            if (!existeUnidad)
            {
                SqlDataSource1.InsertCommand = "insert into catunidmedida values(@idMedida,@descripcion)";
                SqlDataSource1.InsertParameters.Add("idMedida", txtClave.Text);
                SqlDataSource1.InsertParameters.Add("descripcion", txtDescripcion.Text);                
                try
                {
                    SqlDataSource1.Insert();
                    GridView1.DataBind();
                    txtClave.Text = txtDescripcion.Text = "";
                }
                catch (Exception ex)
                {
                    lblErrores.Text = "Error al agregar la unidad " + txtClave.Text + ": " + ex.Message;
                }
            }
            else
                lblErrores.Text = "La unidad a ingresar ya se encuentra registrada";
        
    }
}