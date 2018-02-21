using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CatCentros : System.Web.UI.Page
{
    string usuarioLog;
    string Nomenclatura = "";
    string Nombre = "";
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
            string nombre= (grilla.Rows[index].FindControl("txtNomT") as TextBox).Text;
            string Nclatura = (grilla.Rows[index].FindControl("txtClaT") as TextBox).Text;

            SqlDataSource1.UpdateCommand = "update PV_Centros set Nom_Centro='" + nombre + "', NomenclaturaCentro='" + Nclatura + "' where IDCentro='" + clave + "'";
            try
            {
                //SqlDataSource1.Update();
                grilla.EditIndex = -1;
                grilla.DataBind();
            }
            catch (Exception ex)
            {
                lblErrores.Text = "Error al actualizar el Centro " + nombre + ": " + ex.Message;
            }

        }
        else if (e.CommandName == "Delete")
        {
            clave = e.CommandArgument.ToString();
            try
            {
                string sql = string.Format("delete from PV_Centros where rtrim(ltrim(IDCentro))='{0}'", clave);
                SqlDataSource1.DeleteCommand = sql;
                grilla.DataBind();
            }
            catch (Exception ex)
            {
                lblErrores.Text = "Error al eliminar la unidad " + txtNombreCentro.Text + ": " + ex.Message;
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
                    string clave = DataBinder.Eval(e.Row.DataItem, "IDCentro").ToString();
                    string nom = DataBinder.Eval(e.Row.DataItem, "Nom_Centro").ToString();
                    Cat_Centro centros = new Cat_Centro();
                    centros.Unidad = clave;
                    centros.verificaRelacion();
                    if (!centros.Relacionado)
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
        Cat_Centro centros = new Cat_Centro();
        centros.Unidad = txtNombreCentro.Text;
        string Nomenclatura = txtNomen.Text;
        centros.verificaExiste();

        if (centros.Existe) {
            SqlDataSource1.InsertCommand = "insert into PV_Centros (IDCentro,Nom_Centro,NomenclaturaCentro)"
                + "values ((select isnull((select top 1 IDCentro from PV_Centros order by cast(IDCentro as int)desc),0)+1),'"
                +txtNombreCentro.Text+"','"+txtNomen.Text+"')";
            try
            {
                SqlDataSource1.Insert();
                GridView1.DataBind();
                txtNombreCentro.Text = txtNomen.Text = "";
            }
            catch (Exception es) {
                lblErrores.Text = "Error al agrear el centro " + txtNombreCentro.Text + ": " + es.Message;
            }
        }

        /*lblErrores.Text = "";
        UnidadesMedida unidades = new UnidadesMedida();
        unidades.Unidad = txtNombreCentro.Text;
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
            */
    }
}