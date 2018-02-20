using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;

public partial class CatUsuarios : System.Web.UI.Page
{
    string usuarioLog;
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            checaSesiones();
            try
            {                
                GridView1.DataBind();
            }
            catch (Exception) {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }
    }

    private void checaSesiones()
    {
        try { usuarioLog = Request.QueryString["u"].ToString(); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
    }


    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView grilla = (GridView)sender;
        int index = -1;
        string usuario = "";
        if (e.CommandName == "Update") {
            index = grilla.EditIndex;
            usuario = grilla.DataKeys[index].Value.ToString();
            var nombre = grilla.Rows[index].FindControl("txtNomMod") as TextBox;
            var ap = grilla.Rows[index].FindControl("txtApPatMod") as TextBox;
            var am = grilla.Rows[index].FindControl("txtApMatMod") as TextBox;
            var fechaNacimiento = grilla.Rows[index].FindControl("txtFechaMod") as TextBox;
            var direccion = grilla.Rows[index].FindControl("txtDireccionMod") as TextBox;
            var telefono = grilla.Rows[index].FindControl("txtTelefonoMod") as TextBox;
            var contasena = grilla.Rows[index].FindControl("txtPasswordMod") as TextBox;
            var puesto = grilla.Rows[index].FindControl("ddlPuestoMod") as DropDownList;
            var perfil = grilla.Rows[index].FindControl("ddlPerfilMod") as DropDownList;
            var correo = grilla.Rows[index].FindControl("txtCorreoMod") as TextBox;
            string fechaProceso;
            if (fechaNacimiento.Text == "")
                fechaProceso = "1900-01-01";
            else
                fechaProceso = fechaNacimiento.Text;

            string apellidoMaterno = "";
            try { apellidoMaterno = Convert.ToString(am.Text); } catch (Exception) { apellidoMaterno = ""; }
            bool fechaValida = validaFecha(fechaProceso);


            if (fechaValida)
            {
                SqlDataSource1.UpdateCommand = "update usuarios_PV set nombre=@nombre, apellido_pat=@apellido_pat,apellido_mat=@apellido_mat,f_nacimiento=@f_nacimiento,puesto=@puesto,perfil=@perfil,direccion=@direccion,telefono=@telefono,password=@password,correo=@correo where usuario=@usuario";
                SqlDataSource1.UpdateParameters.Add("usuario", usuario);
                SqlDataSource1.UpdateParameters.Add("nombre", nombre.Text);
                SqlDataSource1.UpdateParameters.Add("apellido_pat", ap.Text);
                SqlDataSource1.UpdateParameters.Add("apellido_mat", apellidoMaterno);
                SqlDataSource1.UpdateParameters.Add("f_nacimiento", fechaProceso);
                SqlDataSource1.UpdateParameters.Add("puesto", puesto.SelectedValue);
                SqlDataSource1.UpdateParameters.Add("perfil", perfil.SelectedValue);
                SqlDataSource1.UpdateParameters.Add("direccion", direccion.Text);
                SqlDataSource1.UpdateParameters.Add("telefono", telefono.Text);
                SqlDataSource1.UpdateParameters.Add("password", contasena.Text);
                SqlDataSource1.UpdateParameters.Add("correo", correo.Text);
                try
                {
                    SqlDataSource1.Update();
                    grilla.EditIndex = -1;
                    grilla.DataBind();
                }
                catch (Exception ex) {
                    lblErrores.Text = "Error al actualizar al usuario " + usuario + ": " + ex.Message;
                }
            }
            else
                lblErrores.Text = "Debe indicar una fecha de nacimiento válida y menor a la fecha actual";
        }
        else if (e.CommandName == "Delete") {
            string[] valores = e.CommandArgument.ToString().Split(new char[] { ';' });
            usuario = valores[0];
            string estatus = valores[1];
            if (estatus == "A")
                SqlDataSource1.DeleteCommand = "update usuarios_PV set estatus='B' where usuario=@usuario";
            else
                SqlDataSource1.DeleteCommand = "update usuarios_PV set estatus='A' where usuario=@usuario";
            SqlDataSource1.DeleteParameters.Add("usuario", usuario);
            try
            {
                SqlDataSource1.Delete();
                grilla.EditIndex = -1;
                grilla.DataBind();
            }
            catch (Exception ex)
            {
                lblErrores.Text = "Error al actualizar al usuario " + usuario + ": " + ex.Message;
            }
        }
    }

    private bool validaFecha(string fechaS) {
        DateTime fecha, fechaActual;
        bool retorno = false;
        fechaActual=Convert.ToDateTime(fechas.obtieneFechaLocal().ToString("yyyy-MM-dd"));
        try
        {
            fecha = Convert.ToDateTime(fechaS);
            if (fecha >= fechaActual)
                retorno = false;
            else
                retorno = true;
        }
        catch (Exception)
        {
            retorno = false;
        }
        return retorno;        
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
                    var btnEliminar = e.Row.Cells[9].Controls[1].FindControl("btnEliminar") as Button;
                    string status = DataBinder.Eval(e.Row.DataItem, "estatus").ToString();
                    string usuario = DataBinder.Eval(e.Row.DataItem, "usuario").ToString();
                    if (usuario.ToUpper() == usuarioLog.ToUpper())
                        btnEliminar.Enabled = false;
                    else
                        btnEliminar.Enabled = true;
                    if (status == "A")
                    {
                        btnEliminar.OnClientClick = "return confirm('¿Está seguro de inactivar el usuario " + usuario + "?')";
                        btnEliminar.Text = "Inactiva";
                        if (usuario.ToUpper() == usuarioLog.ToUpper())
                            btnEliminar.CssClass = "btn-default ancho50px";
                        else
                            btnEliminar.CssClass = "btn-danger ancho50px";
                    }
                    else
                    {
                        btnEliminar.OnClientClick = "return confirm('¿Está seguro de reactivar el usuario " + usuario + "?')";
                        btnEliminar.Text = "Reactiva";
                        if (usuario.ToUpper() == usuarioLog.ToUpper())
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
        string fechaProceso;
        if (txtFecha.Text == "")
            fechaProceso = "1900-01-01";
        else
            fechaProceso = txtFecha.Text;
        bool verificaFecha = validaFecha(fechaProceso);
        if (verificaFecha)
        {
            string apellidoMaterno = "";
            try { apellidoMaterno = Convert.ToString(txtApMat.Text); } catch (Exception) { apellidoMaterno = ""; }
            Usuarios usuario = new Usuarios();
            usuario.Usuario = txtUsuario.Text;
            usuario.existeUsuario();
            bool existeUsuario = usuario.Existe;
            if (!existeUsuario)
            {
                SqlDataSource1.InsertCommand = "insert into usuarios_PV(usuario,nombre,apellido_pat,apellido_mat,f_nacimiento,puesto,perfil,direccion,telefono,password,no_empleado,estatus_dia,caja_asignada,estatus,correo) values(@usuario,@nombre,@apellido_pat,@apellido_mat,@f_nacimiento,@puesto,@perfil,@direccion,@telefono,@password,'','I',0,'A',@correo)";
                SqlDataSource1.InsertParameters.Add("usuario", txtUsuario.Text);
                SqlDataSource1.InsertParameters.Add("nombre", txtNombre.Text);
                SqlDataSource1.InsertParameters.Add("apellido_pat", txtApPat.Text);
                SqlDataSource1.InsertParameters.Add("apellido_mat", apellidoMaterno);
                SqlDataSource1.InsertParameters.Add("f_nacimiento", fechaProceso);
                SqlDataSource1.InsertParameters.Add("puesto", ddlPuesto.SelectedValue);
                SqlDataSource1.InsertParameters.Add("perfil", ddlPerfil.SelectedValue);
                SqlDataSource1.InsertParameters.Add("direccion", txtDireccion.Text);
                SqlDataSource1.InsertParameters.Add("telefono", txtTelefono.Text);
                SqlDataSource1.InsertParameters.Add("password", txtContraseña.Text);
                SqlDataSource1.InsertParameters.Add("correo", txtCorreo.Text);
                try
                {
                    SqlDataSource1.Insert();
                    GridView1.DataBind();
                    txtUsuario.Text = txtNombre.Text = txtApPat.Text = txtApMat.Text = txtFecha.Text = txtDireccion.Text = txtTelefono.Text = txtContraseña.Text = txtCorreo.Text= "";
                    ddlPuesto.Items.Clear();
                    ddlPerfil.Items.Clear();
                    ddlPerfil.DataBind();
                    ddlPuesto.DataBind();

                }
                catch (Exception ex)
                {
                    lblErrores.Text = "Error al agregar al usuario " + txtUsuario.Text + ": " + ex.Message;
                }
            }
            else
                lblErrores.Text = "El usuario a ingresar ya se encuentra registrado";
        }
        else
            lblErrores.Text = "Debe indicar una fecha de nacimiento válida y menor a la fecha actual";
    }
}