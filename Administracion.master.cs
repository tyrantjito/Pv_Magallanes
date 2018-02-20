using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;
using System.Configuration;

public partial class Administracion : System.Web.UI.MasterPage
{
    Fechas fechas = new Fechas();
    string usuario, nombre, pv, caja="Cajas Activas";
    bool[] permisos;
    protected void Page_Load(object sender, EventArgs e)
    {
        checaSesiones();
        lblFechaActual.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-d");
        lblFecha.Text = fechas.obtieneFechaLocal().ToLongDateString();
        lblUsuarioLog.Text = "Bienvenido: " + nombre;
        lblCaja.Text = caja;
        Notificaciones noti = new Notificaciones();
        noti.Fecha = Convert.ToDateTime(lblFechaActual.Text);
        noti.Estatus = "P";
        noti.obtieneNotificacionesPendientes();
        object[] pendientes = noti.Retorno;
        if (Convert.ToBoolean(pendientes[0]))
            lblNotifi.Text = Convert.ToString(pendientes[1]);
        else
            lblNotifi.Text = "0";
        if (!IsPostBack) {
            lblVersionSis.Text = ConfigurationManager.AppSettings["version"].ToString();
        }
    }
    private void checaSesiones()
    {
        try {
            usuario = Request.QueryString["u"].ToString();
            nombre = Request.QueryString["nu"].ToString();
            ObtienePermisos usuPermisos = new ObtienePermisos();
            usuPermisos.Usuario = usuario;
            usuPermisos.obtienePermisos();
            permisos = usuPermisos.Permisos;
            controlaMenu(permisos);

            Usuarios usuariosDat = new Usuarios();
            usuariosDat.Usuario = usuario;
            DateTime fechaIngreso = usuariosDat.fechaAccesosPV();
            DateTime fechaHoy = fechas.obtieneFechaLocal();
            TimeSpan fechaDife = fechaHoy - fechaIngreso;
            int diasDife = fechaDife.Days;
            if (diasDife != 0)
                Response.Redirect("Default.aspx");
        }
        catch (Exception) { 
            Response.Redirect("Default.aspx");
        }
    }

    private void controlaMenu(bool[] permisos)
    {
        bool[] autorizados = permisos;
        LinkButton[] opciones = { lnkAdmon, lnkCatalogos, lnkEntradas, lnkConsulInv, lnkConsultas, lnkReportes, lnkVenta,
                                lnkcatUsu, lnkUsuPer,lnkEmpresas,lnkUnidades,lnkFamilias,lnkLineas,lnkIslas,lnkUsuIsla,
                                lnkUsuFact,lnkProvee,lnkProductos,lnkPlantilla,lnkConsulOrden,lnkConsulVenta,lnkConsulGastos,
                                lnkConsulPendientes,lnkNotificaciones,lnkEnvio,lnkAcumulado,lnkCortes,lnkPersonal,not};
        for (int i = 0; i < permisos.Length; i++) {
            if (i < 28)
                opciones[i].Visible = permisos[i];
            else if (i == 35)
                opciones[28].Visible = permisos[i];
        }
    }

    protected void lnkSalir_Click(object sender, EventArgs e)
    {
        if (usuario != "")
        {
            Accesos salida = new Accesos();
            salida.Acceso = "S";
            salida.Punto = 0;
            salida.Usuario = usuario;
            salida.registraIngreso();            
        }
        Response.Redirect("Default.aspx");
    }
    
    protected void lnkNotificacion_Click(object sender, EventArgs e)
    {
        LinkButton btnNot = (LinkButton)sender;
        int alerta = Convert.ToInt32(btnNot.CommandArgument.ToString());
        Notificaciones noti = new Notificaciones();
        noti.Fecha =Convert.ToDateTime( lblFechaActual.Text);
        noti.Estatus = "V";
        noti.Entrada = alerta;
        noti.actualizaEstado();
        DataList2.DataBind();
        noti.Estatus = "P";
        noti.obtieneNotificacionesPendientes();
        object[] pendientes = noti.Retorno;
        if (Convert.ToBoolean(pendientes[0]))
            lblNotifi.Text = Convert.ToString(pendientes[1]);
        else
            lblNotifi.Text = "0";
    }
    protected void lnkcatUsu_Click(object sender, EventArgs e)
    {        
        Response.Redirect("CatUsuarios.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkUsuPer_Click(object sender, EventArgs e)
    {
        Response.Redirect("Usuarios_Permisos.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkEntradas_Click(object sender, EventArgs e)
    {
        Response.Redirect("EntradasAlmacen.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkConsulInv_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConsultaInventario.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkVenta_Click(object sender, EventArgs e)
    {
        Response.Redirect("VentaPublico.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkEmpresas_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatEmpresa.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkUnidades_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatUnidades.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkFamilias_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatFamilias.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkLineas_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatLineas.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkIslas_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatIslas.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkUsuIsla_Click(object sender, EventArgs e)
    {
        Response.Redirect("UsuariosPunto.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkUsuFact_Click(object sender, EventArgs e)
    {
        Response.Redirect("UsuariosFacturacion.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkProvee_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatProveedores.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkProductos_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatProductos.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkPlantilla_Click(object sender, EventArgs e)
    {
        Response.Redirect("Plantilla.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkNotificaciones_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConsultaNotificaciones.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkConsulPendientes_Click(object sender, EventArgs e)
    {
        Response.Redirect("FacturacionPendiente.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkConsulGastos_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConsultaGastos.aspx?u=" + usuario + "&nu=" + nombre);
    }
    
    protected void lnkConsulCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConsultaCancelaciones.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkConsulVenta_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConsultaVentas.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkConsulVentaUser_Click(object sender, EventArgs e)
    {
        Response.Redirect("VentasUsuarios.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkConsulOrden_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConsultaOrdenes.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkEnvio_Click(object sender, EventArgs e)
    {
        Response.Redirect("EnvioIsla.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkAcumulado_Click(object sender, EventArgs e)
    {
        Response.Redirect("Acumulado.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkCortes_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConsultaCierres.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkPersonal_Click(object sender, EventArgs e)
    {
        Response.Redirect("Personal.aspx?u=" + usuario + "&nu=" + nombre);
    }
    protected void lnkTodas_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConsultaNotificaciones.aspx?u=" + usuario + "&nu=" + nombre);
    }

    protected void lnkCategorias_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatCategoria.aspx?u=" + usuario + "&nu=" + nombre);
    }

    protected void lnkAjusteInventario_Click(object sender, EventArgs e)
    {
        Response.Redirect("AjusteInventario.aspx?u=" + usuario + "&nu=" + nombre);
    }

    protected void lnkConsultaAjuste_Click(object sender, EventArgs e)
    {
        Response.Redirect("ConsultarAjuste.aspx?u=" + usuario + "&nu=" + nombre);
    }
    
        protected void lnkCentros_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatCentros.aspx?u=" + usuario + "&nu=" + nombre);
    }
}
