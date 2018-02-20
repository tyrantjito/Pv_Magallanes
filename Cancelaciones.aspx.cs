using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Cancelaciones : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            lblError.Text = "";            
        }
    }
    protected void lnkConsultar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        try {
            int ticket = Convert.ToInt32(radTicket.Value);
            if (ticket != 0)
                cargaInfo();
            else
                lblError.Text = "Debe indicar in ticket";
        }
        catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
    }

    private void cargaInfo()
    {
        try
        {
            BaseDatos ejecuta = new BaseDatos();
            string sql = string.Format(" select renglon, id_refaccion as clave, descripcion as producto, cantidad, venta_unitaria as precio, importe as total from venta_det where ticket={0} and id_punto={1} and id_almacen={1}", radTicket.Value.ToString(), Request.QueryString["p"]);
            object[] datos = ejecuta.scalarData(sql);
            if (Convert.ToBoolean(datos[0]))
            {
                DataSet info = (DataSet)datos[1];
                grvVenta.DataSource = info;
            }
            else
            {
                grvVenta.DataSource = null;
                lblError.Text = "Error al cargar la información del ticket: " + datos[1].ToString();
            }
        }
        catch (Exception ex)
        {
            grvVenta.DataSource = null;
            lblError.Text = "Error al cargar la información del ticket: " + ex.Message;
        }
        finally {
            grvVenta.DataBind();
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        LinkButton btn = (LinkButton)sender;
        string[] argumentos = btn.CommandArgument.Split(new char[] { ';' });
        Producto prd = new Producto();
        prd.ClaveProducto = argumentos[1];

        Cancelacion cancelacion = new Cancelacion();
        cancelacion.punto = Convert.ToInt32(Request.QueryString["p"]);
        cancelacion.caja = Convert.ToInt32(Request.QueryString["c"]);
        cancelacion.ticket = Convert.ToInt32(radTicket.Value);
        cancelacion.usuario = Request.QueryString["u"];
        int caja = 0;
        cancelacion.obtieneCajaTicket();
        try
        {
            if (Convert.ToBoolean(cancelacion.retorno[0]))
                caja = Convert.ToInt32(cancelacion.retorno[1]);
            else
                caja = 0;
        }
        catch (Exception ex) { caja = 0; }
        if (caja != 0)
        {
            cancelacion.cajaTicket = caja;
            decimal cantidad = Convert.ToDecimal(argumentos[2]);
            if (cantidad > 0)
            {
                if (chkCancelacion.Checked)
                {

                }
                else
                {
                    if (cantidad > 1)
                    {
                        lblProductoCancelar.Text = argumentos[1] + " - " + argumentos[3];

                        decimal cantidadPrevia = 0;
                        decimal cantidadFaltante = 0;
                        cancelacion.cantidadCancelacionPrevia(argumentos[1], caja);
                        if (Convert.ToBoolean(cancelacion.retorno[0]))
                        {
                            cantidadPrevia = Convert.ToDecimal(cancelacion.retorno[1]);
                            if (cantidadPrevia >= cantidad)
                            {
                                Session["completado"] = true;
                                lblError.Text = "El producto ya fue cancelado en su totalidad";
                            }
                            else if (cantidadPrevia != 0)
                            {
                                cantidadFaltante = cantidad - cantidadPrevia;
                                RadCantidadCancela.MaxValue = Convert.ToDouble(cantidadFaltante);
                                RadCantidadCancela.Value = Convert.ToDouble(cantidadFaltante);
                                Session["argumentosCancela"] = argumentos;
                                Session["cancelacion"] = cancelacion;
                                PanelMask.Visible = pnlTicket.Visible = true;
                                Session["completado"] = false;
                            }
                            else if (cantidadPrevia == 0)
                            {
                                RadCantidadCancela.MaxValue = Convert.ToDouble(cantidad);
                                RadCantidadCancela.Value = Convert.ToDouble(cantidad);
                                Session["argumentosCancela"] = argumentos;
                                Session["cancelacion"] = cancelacion;
                                PanelMask.Visible = pnlTicket.Visible = true;
                                Session["completado"] = false;
                            }
                             
                        }
                        else
                            lblError.Text = "Error al obtener informacion previa de cancelaciones. " + Convert.ToString(cancelacion.retorno[1]);
                    }
                    else if (cantidad <= 1)
                    {
                        Session["completado"] = true;
                        procesaCancelacionPieza(cancelacion, argumentos);
                    }
                }
            }
            else
                lblError.Text = "No es posible realizar la cancelacion del producto ya que no tiene cantidad registrada";
        }
        else
            lblError.Text = "No se encuentra el ticket indicado";
        
    }

    private void procesaCancelacionPieza(Cancelacion cancelacion, string[] argumentos)
    {
        try
        {
            bool cancelado = false;
            try { cancelado = Convert.ToBoolean(Session["completado"]); }
            catch (Exception ex) { cancelado = true; lblError.Text = "Error: " + ex.Message; }
            
            if (!cancelado)
            {
                cancelacion.agregaCancelacionPieza(argumentos);
                if (Convert.ToBoolean(cancelacion.retorno[0]))
                    lblError.Text = "Cancelacion realizada. No. de Cancelación: " + Convert.ToInt32(cancelacion.retorno[1]);
                else
                    lblError.Text = "No se pudo realizar la cancelación: " + cancelacion.retorno[1].ToString();
            }
            else
                lblError.Text = "Este producto ya fue cancelado con anterioridad";
            
        }
        catch (Exception ex) { lblError.Text = "No se pudo realizar la cancelación: " + ex.Message; }
        finally {
            lblErrorCantidad.Text = "";
            RadCantidadCancela.Value = 0;
            PanelMask.Visible = pnlTicket.Visible = false;
        }
    }


    protected void grvVenta_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void lnkCancelarCant_Click(object sender, EventArgs e)
    {
        lblErrorCantidad.Text = "";
        RadCantidadCancela.Value = 0;
        PanelMask.Visible = pnlTicket.Visible = false;
    }
    protected void lnkAceptarCant_Click(object sender, EventArgs e)
    {
        lblErrorCantidad.Text = "";
        try {
            decimal cantidad = 0;
            Cancelacion cancelacion = (Cancelacion)Session["cancelacion"];
            string[] argumentos = (string[])Session["argumentosCancela"];
            if (RadCantidadCancela.Value == 0)
                lblErrorCantidad.Text = "La cantidad no puede ser cero";
            else {                
                cantidad = Convert.ToDecimal(RadCantidadCancela.Value);                
                argumentos[2] = cantidad.ToString();
                procesaCancelacionPieza(cancelacion, argumentos);
            }
        }
        catch (Exception ex) { lblErrorCantidad.Text = "Error: " + ex.Message; }
    }
    protected void lnkCancelacionVenta_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        try {
            DataTable dt = new DataTable();
            dt.Columns.Add("renglon");
            dt.Columns.Add("id_refacciones");
            dt.Columns.Add("descripcion");
            dt.Columns.Add("cantidad");
            dt.Columns.Add("precio_unitario");
            dt.Columns.Add("importe");
            dt.Columns.Add("cantidad_restante");
            Cancelacion cancelacion = new Cancelacion();
            cancelacion.punto = Convert.ToInt32(Request.QueryString["p"]);
            cancelacion.caja = Convert.ToInt32(Request.QueryString["c"]);
            cancelacion.ticket = Convert.ToInt32(radTicket.Value);
            cancelacion.usuario = Request.QueryString["u"];
            int caja = 0;
            cancelacion.obtieneCajaTicket();
            try
            {
                if (Convert.ToBoolean(cancelacion.retorno[0]))
                    caja = Convert.ToInt32(cancelacion.retorno[1]);
                else
                    caja = 0;
            }
            catch (Exception ex) { caja = 0; }
            if (caja != 0) {
                cancelacion.cajaTicket = caja;
                cancelacion.obtieneInfoTicket();
                if (Convert.ToBoolean(cancelacion.retorno[0]))
                {
                    DataSet info = (DataSet)cancelacion.retorno[1];
                    int i = 0;
                    foreach (DataRow r in info.Tables[0].Rows)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = r[0];
                        dr[1] = r[1];
                        dr[2] = r[2];
                        dr[3] = r[3];
                        dr[4] = r[4];
                        dr[5] = r[7];
                        dr[6] = r[9];
                        dt.Rows.Add(dr);
                        i++;
                    }

                    if (i != 0)
                    {
                        cancelacion.generaCancelacionCompleta(cancelacion, dt);
                        if (Convert.ToBoolean(cancelacion.retorno[0]))
                            lblError.Text = "Se ha realizado la cancelacion de la venta. No. de cancelacion: " + Convert.ToInt32(cancelacion.retorno[1]);
                        else
                            lblError.Text = "No se pudo realizar la cancelacion de la venta. " + Convert.ToString(cancelacion.retorno[1]);
                    }
                    else
                        lblError.Text = "Ya se a realizado la cancelación de la venta con anterioridad";
                }
                else
                    lblError.Text = "Error al intentar hacer la cancelacion. " + cancelacion.retorno[1].ToString();
            } 
            else lblError.Text = "El ticket indicado no existe o no es el correcto";
        }
        catch (Exception ex) { lblError.Text = "Error: " + ex.Message; }
    }
}