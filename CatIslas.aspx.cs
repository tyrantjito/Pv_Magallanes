using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CatIslas : System.Web.UI.Page
{
    string usuarioLog;
    protected void Page_Load(object sender, EventArgs e)
    {
        checaSesiones();
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
        int almacen = 0;
        if (e.CommandName == "Update")
        {
            index = grilla.EditIndex;
            almacen = Convert.ToInt32(grilla.DataKeys[index].Value.ToString());
            var nombre = grilla.Rows[index].FindControl("txtNomMod") as TextBox;
            var direccion = grilla.Rows[index].FindControl("txtDireccionMod") as TextBox;
            var encargado = grilla.Rows[index].FindControl("ddlEncargadoMod") as DropDownList;
            var fondo = grilla.Rows[index].FindControl("txtFondoMod") as TextBox;

            decimal fondoM = 0;
            try { fondoM = Convert.ToDecimal(fondo.Text); } catch (Exception) { fondoM = 0; }

            SqlDataSource1.UpdateCommand = "update catalmacenes set nombre=@nombre, ubicacion=@ubicacion,userEncargado=@userEncargado,nombreEncargado=@nombreEncargado where idAlmacen=@idAlmacen "
                +"update punto_venta set saldo_inicial_pv=@fondo where id_punto=@idAlmacen and id_almacen=@idAlmacen";
            SqlDataSource1.UpdateParameters.Add("idAlmacen", almacen.ToString());
            SqlDataSource1.UpdateParameters.Add("nombre", nombre.Text);           
            SqlDataSource1.UpdateParameters.Add("userEncargado", encargado.SelectedValue);
            SqlDataSource1.UpdateParameters.Add("nombreEncargado", encargado.SelectedItem.Text);
            SqlDataSource1.UpdateParameters.Add("ubicacion", direccion.Text);
            SqlDataSource1.UpdateParameters.Add("fondo", fondoM.ToString("F2"));
            try
            {
                SqlDataSource1.Update();
                grilla.EditIndex = -1;
                grilla.DataBind();
            }
            catch (Exception ex)
            {
                lblErrores.Text = "Error al actualizar la Tienda " + almacen + ": " + ex.Message;
            }
           
        }
        else if (e.CommandName == "Delete")
        {
            string[] valores = e.CommandArgument.ToString().Split(new char[] { ';' });
            almacen = Convert.ToInt32(valores[0]);
            string estatus = valores[1];
            if (estatus == "A")
                SqlDataSource1.DeleteCommand = "update catalmacenes set estatus='I' where idAlmacen=@idAlmacen";
            else
                SqlDataSource1.DeleteCommand = "update catalmacenes set estatus='A' where idAlmacen=@idAlmacen";
            SqlDataSource1.DeleteParameters.Add("idAlmacen", almacen.ToString());
            try
            {
                SqlDataSource1.Delete();
                grilla.EditIndex = -1;
                grilla.DataBind();
            }
            catch (Exception ex)
            {
                lblErrores.Text = "Error al actualizar la Tienda " + almacen + ": " + ex.Message;
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
                    var btnEliminar = e.Row.Cells[5].Controls[1].FindControl("btnEliminar") as Button;
                    string status = DataBinder.Eval(e.Row.DataItem, "estatus").ToString();
                    int almacen =Convert.ToInt32( DataBinder.Eval(e.Row.DataItem, "idAlmacen").ToString());
                    string nomAlamacen = DataBinder.Eval(e.Row.DataItem, "nombre").ToString();
                    Islas isla = new Islas();
                    isla.Almacen = almacen;
                    isla.verificaRelacion();
                    if (status == "A")
                    {
                        btnEliminar.OnClientClick = "return confirm('¿Está seguro de inactivar la Tienda " + nomAlamacen + "?')";
                        btnEliminar.Text = "Inactiva";
                        if (isla.Relacionado)
                            btnEliminar.CssClass = "btn-default ancho50px";
                        else
                            btnEliminar.CssClass = "btn-danger ancho50px";
                    }
                    else
                    {
                        btnEliminar.OnClientClick = "return confirm('¿Está seguro de reactivar la Tienda " + nomAlamacen + "?')";
                        btnEliminar.Text = "Reactiva";
                        if (isla.Relacionado)
                            btnEliminar.CssClass = "btn-default ancho50px";
                        else
                            btnEliminar.CssClass = "btn-success ancho50px";
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
        decimal fondo = 0;
        try { fondo = Convert.ToDecimal(txtFondo.Text); } catch (Exception) { fondo = 0; }
        Islas isla = new Islas();
        isla.Nombre = txtNombre.Text;
        isla.verificaExiste();
        bool existeUsuario = isla.Existe;
        if (!existeUsuario)
        {
            SqlDataSource1.InsertCommand = "insert into catalmacenes(idAlmacen,nombre,estatus,ubicacion,userEncargado,nombreEncargado) " +
                "values((select (case (Select count(*) from catalmacenes) when 0 then 1 else (select top 1 idAlmacen from catalmacenes order by idAlmacen desc) end)+1),@nombre,'A',@ubicacion,@userEncargado,@nombreEncargado) "+
                "insert into punto_venta(id_punto,id_almacen,nombre_pv,encargado,nom_encargado, estatus,id_emisor,tipo_documento,prefijo_pv,num_cajas_pv,saldo_inicial_pv,saldo_max_retiro) "+
                "values((select (case (Select count(*) from catalmacenes) when 0 then 1 else (select top 1 idAlmacen from catalmacenes order by idAlmacen desc) end)),(select (case (Select count(*) from catalmacenes) when 0 then 1 else (select top 1 idAlmacen from catalmacenes order by idAlmacen desc) end)),@nombre,@userEncargado,@nombreEncargado,'A',0,'','',0,@fondo,0)";                
            SqlDataSource1.InsertParameters.Add("nombre", txtNombre.Text);
            SqlDataSource1.InsertParameters.Add("ubicacion", txtDireccion.Text);
            SqlDataSource1.InsertParameters.Add("userEncargado", ddlEncargado.SelectedValue);
            SqlDataSource1.InsertParameters.Add("nombreEncargado", ddlEncargado.SelectedItem.Text);
            SqlDataSource1.InsertParameters.Add("fondo", fondo.ToString("F2"));
            try
            {
                SqlDataSource1.Insert();
                GridView1.DataBind();
                txtNombre.Text = txtDireccion.Text = "";
                ddlEncargado.Items.Clear();                    
                ddlEncargado.DataBind();
            }
            catch (Exception ex)
            {
                lblErrores.Text = "Error al agregar la Tienda " + txtNombre.Text + ": " + ex.Message;
            }
        }
        else
            lblErrores.Text = "La Tienda a ingresar ya se encuentra registrada";        
    }
}