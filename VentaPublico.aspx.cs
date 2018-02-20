using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using E_Utilities;

public partial class VentaPublico : System.Web.UI.Page
{
    string usuarioLog;
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {
        try { usuarioLog = Convert.ToString(Request.QueryString["u"]); }
        catch (Exception) { usuarioLog = ""; }
        if (usuarioLog != "")
        {
            Usuarios usuario = new Usuarios();
            usuario.Usuario = usuarioLog;
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

                                        /*
                                        Session["nomPv"] = isla.Nombre;
                                        Session["caja"] = caja.Caja;
                                        Session["nombreUsuario"] = usuario.Nombre.Trim();
                                        Session["usuario"] = usuario.Usuario;*/
                                        Response.Redirect("PuntoVenta.aspx?u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"] + "&p=" + usuario.Punto + "&np=" + isla.Nombre + "&c=" + caja.Caja);
                                    }
                                    else
                                        lblError.Text = "Se produjo un error al accesar:" + Convert.ToString(cajaAsignada[1]);
                                }
                                else
                                {
                                    CierreCaja cierreCaja = new CierreCaja();
                                    usuario.cajaAsignada();
                                    usuario.obtienePuntoVenta();
                                    cierreCaja.Acceso = "S";
                                    cierreCaja.Caja = usuario.Caja;
                                    cierreCaja.FechaDia = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                                    cierreCaja.Punto = usuario.Punto;
                                    cierreCaja.Usuario = usuario.Usuario;
                                    cierreCaja.generaCorteCaja();
                                    if (!Convert.ToBoolean(cierreCaja.Registrado[0]))
                                    {
                                        lblError.Text = "Se produjo un error al intentar hacer el cierre de la caja no cerrada.: " + cierreCaja.Registrado[1].ToString();
                                    }
                                    else
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

                                            /*
                                            Session["nomPv"] = isla.Nombre;
                                            Session["caja"] = caja.Caja;
                                            Session["nombreUsuario"] = usuario.Nombre.Trim();
                                            Session["usuario"] = usuario.Usuario;*/
                                            Response.Redirect("PuntoVenta.aspx?u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"] + "&p=" + usuario.Punto + "&np=" + isla.Nombre + "&c=" + caja.Caja);
                                        }
                                        else
                                            lblError.Text = "Se produjo un error al accesar:" + Convert.ToString(cajaAsignada[1]);

                                    }    
                                }

                            }
                            else
                            {
                                lblError.Text = "";
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
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        if (ddlIslas.SelectedValue != "0")
        {
            try { usuarioLog = Convert.ToString(Request.QueryString["u"]); }
            catch (Exception) { usuarioLog = ""; }
            if (usuarioLog != "")
            {
                CierreCaja cierre = new CierreCaja();
                cierre.FechaDia = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                cierre.Punto = Convert.ToInt32(ddlIslas.SelectedValue);
                cierre._horaDia = fechas.obtieneFechaLocal().ToString("HH:mm:ss");
                cierre.existeCierreDia();
                if (cierre.cierreDia)
                {
                    string alerta = "alert('Ya se ha realizado el corte del día y no es posible realizar más ventas por el día de hoy')";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "cierre", alerta, true);
                }
                else
                {
                    Usuarios usuario = new Usuarios();
                    usuario.Usuario = usuarioLog;
                    usuario.Punto = Convert.ToInt32(ddlIslas.SelectedValue);
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
                            Response.Redirect("PuntoVenta.aspx?u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"] + "&p=" + usuario.Punto + "&np=" + isla.Nombre + "&c=" + caja.Caja);
                        }
                        else
                            lblError.Text = "Se produjo un error al accesar:" + Convert.ToString(cajaAsignada[1]);
                    }
                    else
                    {
                        CierreCaja cierreCaja = new CierreCaja();
                        usuario.cajaAsignada();
                        cierreCaja.Acceso = "S";
                        cierreCaja.Caja = usuario.Caja;
                        cierreCaja.FechaDia = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                        cierreCaja.Punto = usuario.Punto;
                        cierreCaja.Usuario = usuario.Usuario;
                        cierreCaja.generaCorteCaja();
                        if (!Convert.ToBoolean(cierreCaja.Registrado[0]))
                        {
                            lblError.Text = "Se produjo un error al intentar hacer el cierre de la caja no cerrada.: " + cierreCaja.Registrado[1].ToString();
                        }
                        else
                        {
                            lblError.Text = "Se cerro existosamente la caja #" + usuario.Caja.ToString() + ", por favor vuelve a dar click en la opción consulta para comenzar nueva venta";
                        }
                    }
                }
            }
        }
        else
            lblError.Text = "Debe seleccionar la Tienda a la que desea ingresar";
    }
}