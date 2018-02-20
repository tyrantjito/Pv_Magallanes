using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;
using System.Threading;

public partial class _Default : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            username.Text = "";
            password.Text = "";
        }
    }
    protected void btnIngresar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        //try
        //{
            if (password.Text.Length > 2)
            {
                Autenticar acceso = new Autenticar();
                acceso.Usuario = username.Text;
                acceso.Password = password.Text;
                object[] valido = acceso.autenticar();
                if (Convert.ToBoolean(valido[0]))
                {
                    if (Convert.ToBoolean(valido[1]))
                    {
                        Usuarios usuario = new Usuarios();
                        usuario.Usuario = username.Text;
                        usuario.obtienePerfilUsuario();
                        if (usuario.Perfil != 0)
                        {
                            usuario.obtieneNombreUsuario();
                            if (usuario.Perfil == 1)
                            {
                                usuario.existeSessionPrevia();
                                if (!usuario.SesionPrevia)
                                {
                                    usuario.registraAccesoAdmin();
                                    if (usuario.Registrado)
                                    {
                                        /*Session["nombreUsuario"] = usuario.Nombre.Trim();
                                        Session["usuario"] = usuario.Usuario;*/
                                        Response.Redirect("Administracion.aspx?u=" + usuario.Usuario + "&nu=" + usuario.Nombre);
                                    }
                                    else
                                    {
                                        lblError.Text = "Se produjo un error al intentar accesar, contacte al administrador del sistema para que le solucione su acceso.";
                                    }
                                }
                                else {
                                    btnCierreSesion.Visible = true;
                                    btnIngresar.Enabled = false;
                                    lblError.Text = "Aún cuenta con una sesión activa; de clic en el siguiente botón para realizar el cierre de sesión correspondiente.";
                                }
                            }
                            else
                            {
                                DataSet islas = usuario.obtienePuntos();
                                if (islas != null)
                                {
                                    if (islas.Tables[0].Rows.Count != 0)
                                    {
                                        CierreCaja cierre = new CierreCaja();
                                        cierre.FechaDia = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                                        cierre._horaDia = fechas.obtieneFechaLocal().ToString("HH:mm:ss");
                                        cierre.Punto = usuario.Punto;
                                        cierre.existeCierreDia();
                                        if (cierre.cierreDia)
                                        {
                                            string alerta = "alert('Ya se ha realizado el corte del día y no es posible realizar más ventas por el día de hoy')";
                                            ScriptManager.RegisterStartupScript(this, typeof(Page), "cierre", alerta, true);
                                        }
                                        else
                                        {
                                            usuario.existeSessionPrevia();
                                            if (!usuario.SesionPrevia)
                                            {
                                                if (islas.Tables[0].Rows.Count == 1)
                                                {
                                                    usuario.obtienePuntoVenta();
                                                    usuario.tieneCajaAsignada();
                                                    Cajas caja = new Cajas();
                                                    if (!usuario.UltimaCaja)
                                                    {
                                                        caja.Usuario = usuario.Usuario;
                                                        caja.Acceso = "E";
                                                        caja.Punto = usuario.Punto;
                                                        caja.generaCaja();
                                                        object[] cajaAsignada = caja.Valores;
                                                        if (Convert.ToBoolean(cajaAsignada[0]))
                                                        {

                                                            Islas isla = new Islas();
                                                            isla.Almacen = usuario.Punto;
                                                            isla.obtieneNombre();

                                                            //Notificaicon de mas de 2 accesos al sistema
                                                            object[] accesos = caja.cajasDelDia();
                                                            if (Convert.ToBoolean(accesos[0]))
                                                            {
                                                                int registros = Convert.ToInt32(Convert.ToString(accesos[1]));
                                                                if (registros > 2)
                                                                {
                                                                    Notificaciones notifi = new Notificaciones();
                                                                    notifi.Punto = isla.Almacen;
                                                                    notifi.Usuario = usuario.Usuario;
                                                                    notifi.Fecha = fechas.obtieneFechaLocal();
                                                                    notifi.Estatus = "P";
                                                                    notifi.Extra = registros.ToString();
                                                                    notifi.Clasificacion = 4;
                                                                    notifi.Origen = "V";
                                                                    notifi.armaNotificacion();
                                                                    notifi.agregaNotificacion();
                                                                }
                                                            }
                                                        /*Session["nomPv"] = isla.Nombre;
                                                        Session["caja"] = caja.Caja;
                                                        Session["nombreUsuario"] = usuario.Nombre.Trim();
                                                        Session["usuario"] = usuario.Usuario;*/
                                                        //string href = "PuntoVenta.aspx?u=" + usuario.Usuario + "&nu=" + usuario.Nombre + "&p=" + usuario.Punto + "&np=" + isla.Nombre + "&c=" + caja.Caja;
                                                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.location.href('"+ href+ "');", true);
                                                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( '"+href+"','_blank','height=600px,width=600px,scrollbars=1');", true);
                                                        
                                                        try
                                                        {
                                                            Response.Redirect("PuntoVenta.aspx?u=" + usuario.Usuario + "&nu=" + usuario.Nombre + "&p=" + usuario.Punto + "&np=" + isla.Nombre + "&c=" + caja.Caja, false);
                                                            Context.ApplicationInstance.CompleteRequest();
                                                        }
                                                        catch (ThreadAbortException ex)
                                                        {
                                                            Thread.ResetAbort();
                                                            Response.Redirect("PuntoVenta.aspx?u=" + usuario.Usuario + "&nu=" + usuario.Nombre + "&p=" + usuario.Punto + "&np=" + isla.Nombre + "&c=" + caja.Caja, false);
                                                        }
                                                    }
                                                        else
                                                            lblError.Text = "Se produjo un error al accesar:" + Convert.ToString(cajaAsignada[1]);
                                                    }
                                                    else
                                                    {
                                                        lblError.Text = "No realizó su corte de caja, contacte al administrador para realizar el corte correspondiente.";
                                                    }

                                                }
                                                else {
                                                    SqlDataSourceIslas.DataBind();
                                                    lblErrorIsla.Text = "";
                                                    pnlMask.Visible = true;
                                                    pnlIslas.Visible = true;
                                                }
                                            }
                                            else
                                            {
                                                btnCierreCaja.Visible = true;
                                                btnIngresar.Enabled = false;
                                                lblError.Text = "Aún cuenta con una sesión activa o no realizó su corte de caja; de clic en el siguiente botón para realizar el cierre correspondiente.";
                                            }
                                        }
                                    }
                                    else
                                        lblError.Text = "No cuenta con una Tienda asignada, por favor contacte al administrador del sistema para que le asigne una.";
                                }
                                else
                                    lblError.Text = "No cuenta con una Tienda asignada, por favor contacte al administrador del sistema para que le asigne una.";
                            }
                        }
                        else
                            lblError.Text = "No cuenta con el perfil necesario para accesar a la aplicación, contacte a su administrador del sistema.";

                    }
                    else
                        lblError.Text = "Usuario y/o Contraseña Incorrectos.";
                }
                else
                    lblError.Text = Convert.ToString(valido[1]);
            }
            else
                lblError.Text = "La contraseña debe contener entre 5 y 15 caracteres.";

        //}
        //catch (Exception ex)
        //{
        //    lblError.Text = ex.Message;
        //}
    }
    protected void btnCierreSesion_Click(object sender, EventArgs e)
    {
        Accesos acceso = new Accesos();
        acceso.Punto = 0;
        acceso.Usuario = username.Text;
        acceso.Acceso = "S";
        acceso.registraIngreso();
        if (Convert.ToBoolean(acceso.Registrado[0]))
        {
            btnCierreSesion.Visible = false;
            btnIngresar.Enabled = true;
            lblError.Text = "Se ha cerrado la sesión existósamente, por favor vuelva a ingresar";
        }
        else
        {
            btnCierreSesion.Visible = false;
            btnIngresar.Enabled = true;
            lblError.Text = "No se pudo cerrar la sesión previa, contacte al administrador del sistema";
        }
    }
    protected void btnCierreCaja_Click(object sender, EventArgs e)
    {
        CierreCaja cierre = new CierreCaja();
        Usuarios usuario = new Usuarios();
        usuario.Usuario = username.Text;
        usuario.cajaAsignada();
        usuario.obtienePuntoVenta();
        int caja = usuario.Caja;
        cierre.Acceso = "S";
        cierre.Caja = caja;
        cierre.FechaDia = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
        cierre.Punto = usuario.Punto;
        cierre.Usuario = usuario.Usuario;
        cierre.generaCorteCaja();
        if (!Convert.ToBoolean(cierre.Registrado[0]))
        {
            lblError.Text = "Se produjo un error al intentar hacer el cierre de la caja no cerrada.: " + cierre.Registrado[1].ToString();
        }
        else
        {
            lblError.Text = "Se cerro existosamente la caja #" + caja.ToString() + "; por favor proceda a volver a iniciar sesión";
            btnCierreCaja.Visible = false;
            btnIngresar.Enabled = true;
        }
    }
    protected void btnSeleccionar_Click(object sender, EventArgs e)
    {
        if (ddlIsla.SelectedValue != "0")
        {
            CierreCaja cierre = new CierreCaja();
            cierre.FechaDia = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
            cierre._horaDia = fechas.obtieneFechaLocal().ToString("HH:mm:ss");
            cierre.Punto = Convert.ToInt32(ddlIsla.SelectedValue);
            cierre.existeCierreDia();
            if (cierre.cierreDia)
            {
                string alerta = "alert('Ya se ha realizado el corte del día y no es posible realizar más ventas por el día de hoy')";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "cierre", alerta, true);
            }
            else
            {
                Usuarios usuario = new Usuarios();
                usuario.Usuario = username.Text;
                usuario.Punto = Convert.ToInt32(ddlIsla.SelectedValue);
                usuario.obtieneNombreUsuario();
                usuario.tieneCajaAsignada();
                Cajas caja = new Cajas();
                if (!usuario.UltimaCaja)
                {
                    caja.Usuario = usuario.Usuario;
                    caja.Acceso = "E";
                    caja.Punto = usuario.Punto;
                    caja.generaCaja();
                    object[] cajaAsignada = caja.Valores;
                    if (Convert.ToBoolean(cajaAsignada[0]))
                    {
                        //Session["pv"] = usuario.Punto;
                        Islas isla = new Islas();
                        isla.Almacen = usuario.Punto;
                        isla.obtieneNombre();

                        //Notificaicon de mas de 2 accesos al sistema
                        object[] accesos = caja.cajasDelDia();
                        if (Convert.ToBoolean(accesos[0]))
                        {
                            int registros = Convert.ToInt32(Convert.ToString(accesos[1]));
                            if (registros > 2)
                            {
                                Notificaciones notifi = new Notificaciones();
                                notifi.Punto = isla.Almacen;
                                notifi.Usuario = usuario.Usuario;
                                notifi.Fecha = fechas.obtieneFechaLocal();
                                notifi.Estatus = "P";
                                notifi.Extra = registros.ToString();
                                notifi.Clasificacion = 4;
                                notifi.Origen = "V";
                                notifi.armaNotificacion();
                                notifi.agregaNotificacion();
                            }
                        }


                        /*Session["nomPv"] = isla.Nombre;
                        Session["caja"] = caja.Caja;
                        Session["nombreUsuario"] = usuario.Nombre.Trim();
                        Session["usuario"] = usuario.Usuario;*/
                        Response.Redirect("PuntoVenta.aspx?u=" + usuario.Usuario + "&nu=" + usuario.Nombre + "&p=" + usuario.Punto + "&np=" + isla.Nombre + "&c=" + caja.Caja);
                    }
                    else
                        lblErrorIsla.Text = "Se produjo un error al accesar:" + Convert.ToString(cajaAsignada[1]);
                }
                else
                {
                    lblErrorIsla.Text = "No realizó su corte de caja, contacte al administrador para realizar el corte correspondiente.";
                }
            }
        }
        else
        {
            lblErrorIsla.Text = "Debe seleccionar una Tienda";
        }
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        pnlMask.Visible = false;
        pnlIslas.Visible = false;
        password.Text = "";
    }
    protected void lnkOlvido_Click(object sender, EventArgs e)
    {
        Usuarios usuario = new Usuarios();
        usuario.Usuario = username.Text;
        usuario.existeUsuario();
        if (usuario.Existe)
        {
            string correo = usuario.obtieneCorreo();
            if (correo != "")
            {
                Envio_Mail mail = new Envio_Mail();
                bool envidado = mail.obtieneDatosServidor(username.Text, correo);
                if (envidado)
                    lblError.Text = "Se ha enviado su contraseña vía correo electronico";
                else
                    lblError.Text = "Se produjo un error al enviar la contraseña vía correo, intente de nuevo. Si persiste el error por favor contacte al administrador del sistema para que le proporcione su contraseña.";
            }
            else
            {
                lblError.Text = "No cuenta con un correo registrado, por favor contacte al administrador del sistema para que le proporcione su contraseña.";
            }
        }
        else
            lblError.Text = "El usuario indicado no existe, favor de verificar sus datos";
    }
}