using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using E_Utilities;

public partial class Orden_Compra : System.Web.UI.Page
{
    OrdenCompra datos = new OrdenCompra();
    List<OrdenCompraDetalle> ordenCompraList;
    string usuario;
    int idAlmacen;
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            RadAutoCompleteBox.Entries.Clear();
            txtCantidad.Text = "";
            ddlCategoria.SelectedValue = "-1";
            lnkAgregarProd.Visible = false;
            txtCantidad.Enabled = false;
            RadAutoCompleteBox.Enabled = false;
            lblError.Text = "Los datos iniciales sugeridos son generados en base a la existencia del producto, su stock minimo y maximo.";
            Session["ordenes"] = null;
            cargaQuery();
            //cargaVariables();
            //GridOrdenDetalle.Visible = false;
        }
    }

    private void cargaQuery()
    {
        DataSet data = null;
        int idAlmacen = Convert.ToInt32(Request.QueryString["p"]);
        data = datos.llenaOrdenCompra(idAlmacen);
        ordenCompraList = new List<OrdenCompraDetalle>();
        foreach (DataRow dr in data.Tables[0].Rows)
        {
            OrdenCompraDetalle ordenD = new OrdenCompraDetalle();
            ordenD.Cantidad = Convert.ToDecimal(dr[7].ToString());
            ordenD.IdArticulo = dr[1].ToString();
            ordenD.Producto = dr[3].ToString();
            ordenD.Descripcion_categoria = dr[2].ToString();
            ordenCompraList.Add(ordenD);
        }
        GridOrdenCompra.DataSource = ordenCompraList;
        GridOrdenCompra.DataBind();
        Session["listaOrden"] = ordenCompraList;
        Session["listaOriginal"] = ordenCompraList;
        if (ordenCompraList.Count > 0)
            lnkGenerarOrden.Visible = true;
        else
            lnkGenerarOrden.Visible = false;
    }

    private void cargaVariables()
    {
        try
        {
            usuario = Request.QueryString["u"];
            idAlmacen = Convert.ToInt32(Request.QueryString["p"]);
            lblUsuario.Text = usuario;
            lblIsla.Text = idAlmacen.ToString();
            txtRequerimiento.Visible = true;
            btnAceptar.Visible = true;
            GridTempDetalle.Visible = false;
            GridTempDetalle.DataSource = null;
            GridTempDetalle.DataBind();
            btnNuevo.Visible = false;
            btnCancelarNuevo.Visible = false;
        }
        catch (Exception)
        {
            lblUsuario.Text = "0";
            lblIsla.Text = "0";
        }
        
    }
    
    protected void btnAceptar_Click(object sender, EventArgs e)
    {
        lblErrores.Text = "";
        if (txtRequerimiento.Text.Trim() != "")
        {
            try
            {
                OrdenCompra ordenCompra = new OrdenCompra();
                List<OrdenCompra> solicitud = new List<OrdenCompra>();
                if (Session["ordenes"] != null)
                    solicitud = (List<OrdenCompra>)Session["ordenes"];
                ordenCompra.texto = txtRequerimiento.Text;
                int renglon = 0;
                if (solicitud == null)
                    renglon = 1;
                else
                {
                    foreach (OrdenCompra registro in solicitud)
                    {
                        renglon = registro.renglon;
                    }
                    renglon++;
                }
                ordenCompra.renglon = renglon;
                solicitud.Add(ordenCompra);
                Session["ordenes"] = solicitud;
                GridTempDetalle.Visible = true;
                GridTempDetalle.DataSource = solicitud;
                GridTempDetalle.DataBind();
                btnNuevo.Visible = true;
                btnCancelarNuevo.Visible = true;
                txtRequerimiento.Text = "";
            }
            catch (Exception)
            {
                GridTempDetalle.DataSource = null;
                GridTempDetalle.DataBind();
            }
        }
        else
            lblErrores.Text = "Debe introducir el texto para la orden de compra.";
    }

    protected void rbtnOrdenVista_SelectedIndexChanged(object sender, EventArgs e)
    {
        char vista = Convert.ToChar(rbtnOrdenVista.SelectedValue);
        if (vista == 'N')
        {
            txtRequerimiento.Visible = true;
            btnAceptar.Visible = true;
            GridTempDetalle.Visible = true;
            GridTempDetalle.DataSource = null;
            GridTempDetalle.DataBind();
            btnNuevo.Visible = true;
            btnCancelarNuevo.Visible = true;
        }
        if (vista == 'O')
        {
            txtRequerimiento.Visible = false;
            btnAceptar.Visible = false;
            GridTempDetalle.Visible = false;
            btnNuevo.Visible = false;
            btnCancelarNuevo.Visible = false;
        }
    }

    protected void btnNuevo_Click(object sender, EventArgs e)
    {
        OrdenCompra ordenNueva = new OrdenCompra();
        List<OrdenCompra> nuevaSolicitud = new List<OrdenCompra>();
        nuevaSolicitud = (List<OrdenCompra>)Session["ordenes"];
        object[] agregar = new object[4];
        agregar[0] = lblIsla.Text;
        agregar[1] = lblUsuario.Text;
        agregar[2] = fechas.obtieneFechaLocal();
        agregar[3] = nuevaSolicitud;
        object[] agregado = ordenNueva.agregaNuevaOrden(agregar);
        if ((bool)agregado[0])
        {
            lblErrores.Text = "";
            txtRequerimiento.Text = "";
            GridTempDetalle.Visible = false;
            btnNuevo.Visible = false;
            btnCancelarNuevo.Visible = false;

            //Agrega notificaicon de nueva orden
            Notificaciones notifi = new Notificaciones();
            notifi.Punto = Convert.ToInt32(agregar[0]);
            notifi.Usuario = Convert.ToString(agregar[1]);
            notifi.Fecha =Convert.ToDateTime(agregar[2]);
            notifi.Entrada = Convert.ToInt32(agregado[1]);
            notifi.Estatus = "P";
            notifi.Clasificacion = 2;            
            notifi.Origen = "V";
            notifi.armaNotificacion();
            notifi.agregaNotificacion();
            if ((bool)notifi.Retorno[0])
                lblErrores.Text = "Se ha notificado que se le surta la orden generada";
        }
        else
            lblErrores.Text = "Hubo un problema al insertar la Orden de Compra, verifique su conexión e intentelo nuevamente.";
    }

    protected void btnCancelarNuevo_Click(object sender, EventArgs e)
    {
        List<OrdenCompra> solicitud = new List<OrdenCompra>();
        Session["ordenes"] = null;
        solicitud = (List<OrdenCompra>)Session["ordenes"];
        GridTempDetalle.Visible = false;
        btnNuevo.Visible = false;
        btnCancelarNuevo.Visible = false;
    }

    protected void GridTempDetalle_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        lblErrores.Text = "";
        try
        {
            Label lblRenglonGrid = GridTempDetalle.Rows[e.RowIndex].FindControl("lblRenglonGrid") as Label;
            int renglon = Convert.ToInt32(lblRenglonGrid.Text);
            List<OrdenCompra> solicitud = (List<OrdenCompra>)Session["ordenes"];
            solicitud.RemoveAt(renglon-1);
            Session["ordenes"] = solicitud;
            GridTempDetalle.DataSource = solicitud;
            GridTempDetalle.DataBind();
        }
        catch (Exception ex)
        {
            lblErrores.Text = "No se logro eliminar la solicitud, verifique su conexión e intentelo nuevamente.";
        }
        
    }



    protected void lnkAgregarProd_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        try
        {
            if (lblProducto.Text != "")
            {
                if (Session["listaOrden"] != null)
                    ordenCompraList = (List<OrdenCompraDetalle>)Session["listaOrden"];
                else
                {
                    cargaQuery();
                    lblError.Text = "Su sesión expiro, la orden de compra se reinicio a la lista de articulos sugeridos.";
                }
                if (txtCantidad.Text != "")
                {
                    bool existe = false;
                    string idArticulo = lblIdArticulo.Text;
                    string descArticulo = lblProducto.Text;
                    string categoria = lblCategoria.Text;
                    foreach (OrdenCompraDetalle lista in ordenCompraList) {
                        if (lista.IdArticulo == lblIdArticulo.Text) {
                            existe = true;
                            break;
                        }
                    }
                    if (existe)
                        lblError.Text = "El producto indicado ya esta registrado en esta solicitud de compra";
                    else
                    {
                        decimal cantidad = Convert.ToDecimal(txtCantidad.Text);
                        if (cantidad > 0)
                        {
                            OrdenCompraDetalle ordenD = new OrdenCompraDetalle();
                            ordenD.Cantidad = cantidad;
                            ordenD.IdArticulo = idArticulo;
                            ordenD.Producto = descArticulo;
                            ordenD.Descripcion_categoria = categoria;
                            ordenCompraList.Add(ordenD);
                            GridOrdenCompra.DataSource = ordenCompraList;
                            GridOrdenCompra.DataBind();
                            Session["listaOrden"] = ordenCompraList;
                            Session["listaOriginal"] = ordenCompraList;
                            lnkGenerarOrden.Visible = true;
                            limpiaNuevo();
                        }
                        else
                            lblError.Text = "La cantidad no debe ser menor a 1.";
                    }
                }
                else
                    lblError.Text = "La cantidad no debe estar vacia.";
            }
            else
                lblError.Text = "Necesita colocar un articulo para agregarlo a la orden de compra.";
        }
        catch (Exception)
        {
            txtCantidad.Text = "";
            RadAutoCompleteBox.Entries.Clear();
            lblError.Text = "Hubo un error al agregar el articulo a la orden de compra, verifique que los datos insertados sean correctos.";
        }
    }

    private void limpiaNuevo()
    {
        lblError.Text = "";
        txtCantidad.Text = "";
        lblProducto.Text = "";
        lblIdArticulo.Text = "";
        lblCategoria.Text = "";
        RadAutoCompleteBox.Entries.Clear();
    }

    protected void RadAutoCompleteBox_TextChanged(object sender, Telerik.Web.UI.AutoCompleteTextEventArgs e)
    {
        lblProducto.Text = "";
        lblError.Text = "";
        try
        {
            if (RadAutoCompleteBox.Text != "")
            {
                string[] argumentos = RadAutoCompleteBox.Text.ToString().Split(new char[] { '/' });
                string[] producto = argumentos[1].Trim().Split(new char[] { ';' });
                Producto produc = new Producto();
                produc.NombrePorducto = producto[0].Trim();
                produc.ClaveProducto = Convert.ToString(argumentos[0].Trim());
                object[] valor = produc.obtieneProducto();
                if (Convert.ToBoolean(valor[0]))
                {
                    if (Convert.ToString(valor[1]) != "")
                    {
                        int pv;
                        try { pv = Convert.ToInt32(Request.QueryString["p"]); }
                        catch (Exception) { pv = 0; }
                        if (pv != 0)
                        {
                            string idProducto = Convert.ToString(argumentos[0].Trim());
                            string categoria = datos.obtieneDescCategoriaProducto(idProducto);
                            if (categoria != "")
                            {
                                lblIdArticulo.Text = idProducto;
                                lblCategoria.Text = categoria;
                                produc.Isla = pv;
                                produc.existeIsla();
                                lblProducto.Text = Convert.ToString(argumentos[1].Trim());
                                txtCantidad.Focus();
                            }
                            else
                                lblError.Text = "Su sesión a caducado por favor vuelva a ingresar";
                        }
                        else
                            lblError.Text = "Su sesión a caducado por favor vuelva a ingresar";
                    }
                    else
                    {
                        lblProducto.Text = "El producto no existe";
                        RadAutoCompleteBox.Focus();
                    }
                }
                else
                {
                    lblProducto.Text = "";
                    lblError.Text = Convert.ToString(valor[1]);
                    RadAutoCompleteBox.Focus();
                }
            }
            else
            {
                lblProducto.Text = "";
                RadAutoCompleteBox.Focus();
            }
        }
        catch (Exception ex) { lblProducto.Text = ""; lblError.Text = ex.Message; }
    }

    protected void GridOrdenCompra_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        ordenCompraList = (List<OrdenCompraDetalle>)Session["listaOrden"];
        GridOrdenCompra.EditIndex = -1;
        Session["listaOrden"] = ordenCompraList; 
        Session["listaOriginal"] = ordenCompraList;
        GridOrdenCompra.DataSource = ordenCompraList;
        GridOrdenCompra.DataBind();
    }

    protected void GridOrdenCompra_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id = GridOrdenCompra.DataKeys[e.RowIndex].Value.ToString();
        ordenCompraList = (List<OrdenCompraDetalle>)Session["listaOrden"];
        foreach (OrdenCompraDetalle item in ordenCompraList)
        {
            if (item.IdArticulo.Equals(id))
            {
                ordenCompraList.Remove(item);
                break;
            }
        }
        Session["listaOrden"] = ordenCompraList;
        Session["listaOriginal"] = ordenCompraList;
        GridOrdenCompra.DataSource = ordenCompraList;
        GridOrdenCompra.DataBind();
    }

    protected void GridOrdenCompra_RowEditing(object sender, GridViewEditEventArgs e)
    {
        ordenCompraList = (List<OrdenCompraDetalle>)Session["listaOrden"];
        GridOrdenCompra.EditIndex = e.NewEditIndex;
        Session["listaOrden"] = ordenCompraList;
        Session["listaOriginal"] = ordenCompraList;
        GridOrdenCompra.DataSource = ordenCompraList;
        GridOrdenCompra.DataBind();
    }

    protected void GridOrdenCompra_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        RadNumericTextBox txtCantidadEdit = (RadNumericTextBox)GridOrdenCompra.Rows[GridOrdenCompra.EditIndex].FindControl("txtCantidadEdit");
        Label lblIdArticuloEdit = (Label)GridOrdenCompra.Rows[GridOrdenCompra.EditIndex].FindControl("lblIdArticuloEdit");
        ordenCompraList = (List<OrdenCompraDetalle>)Session["listaOrden"];
        string id = lblIdArticuloEdit.Text;
        decimal cant = 0;
        try
        {
            cant = Convert.ToDecimal(txtCantidadEdit.Text);
            if (cant > 0)
            {
                foreach (OrdenCompraDetalle item in ordenCompraList)
                {
                    if (item.IdArticulo.Equals(id))
                    {
                        item.Cantidad = cant;
                        break;
                    }
                }
                GridOrdenCompra.EditIndex = -1;
                Session["listaOrden"] = ordenCompraList;
                Session["listaOriginal"] = ordenCompraList;
                GridOrdenCompra.DataSource = ordenCompraList;
                GridOrdenCompra.DataBind();
                lblError.Text = "";
            }
            else
            {
                lblError.Text = "La cantidad no debe quedar en 0.";
                Session["listaOrden"] = ordenCompraList;
                Session["listaOriginal"] = ordenCompraList;
                GridOrdenCompra.DataSource = ordenCompraList;
                GridOrdenCompra.DataBind();
            }
        }
        catch (Exception) { }
        
    }
    ///////////////////////////////////////////////////////////////////////////////////
    protected void lnkGenerarOrden_Click(object sender, EventArgs e)
    {
        try
        {
            ordenCompraList = (List<OrdenCompraDetalle>)Session["listaOrden"];
            object[] agregar = new object[4];
            agregar[0] = Request.QueryString["p"];
            agregar[1] = Request.QueryString["u"];
            agregar[2] = fechas.obtieneFechaLocal();
            agregar[3] = ordenCompraList;
            object[] agregado = datos.agregaNuevaOrden(agregar);
            if ((bool)agregado[0])
            {
                lblError.Text = "";
                //Agrega notificaicon de nueva orden
                Notificaciones notifi = new Notificaciones();
                notifi.Punto = Convert.ToInt32(agregar[0]);
                notifi.Usuario = Convert.ToString(agregar[1]);
                notifi.Fecha = Convert.ToDateTime(agregar[2]);
                notifi.Entrada = Convert.ToInt32(agregado[1]);
                notifi.Estatus = "P";
                notifi.Clasificacion = 2;
                notifi.Origen = "V";
                notifi.armaNotificacion();
                notifi.agregaNotificacion();
                if ((bool)notifi.Retorno[0])
                {
                    lblError.Text = "Se ha generado la orden de compra correctamente.";
                    GridOrdenCompra.Visible = false;
                    lnkGenerarOrden.Visible = false;
                    lnkAgregarProd.Visible = false;
                }
            }
            else
                lblError.Text = "Ocurrio un problema inesperado al generar la Orden de Compra.<br/> Error: " + agregado[1].ToString() + ".";
        }
        catch (Exception ex)
        {
            lblError.Text = "Ocurrio un problema inesperado al generar la Orden de Compra.<br/> Error: " + ex.Message.ToString() + ".";
        }
    }
    protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        try
        {
            if (ddlCategoria.SelectedValue != "-1")
            {
                if (Session["listaOrden"] != null)
                {
                    ordenCompraList = (List<OrdenCompraDetalle>)Session["listaOrden"];
                    List<OrdenCompraDetalle> ordenRange = new List<OrdenCompraDetalle>();
                    foreach (OrdenCompraDetalle lista in ordenCompraList)
                    {
                        if (lista.Descripcion_categoria.ToUpper().Trim() == ddlCategoria.SelectedItem.Text.ToUpper().Trim())
                            ordenRange.Add(lista);
                    }
                    GridOrdenCompra.DataSource = ordenRange;
                    GridOrdenCompra.DataBind();
                }
                RadAutoCompleteBox.Enabled = true;
                RadAutoCompleteBox.Entries.Clear();
                txtCantidad.Enabled = true;
                txtCantidad.Text = "";
                lnkAgregarProd.Visible = true;
            }
            else {
                if (Session["listaOrden"] != null)
                {
                    ordenCompraList = (List<OrdenCompraDetalle>)Session["listaOrden"];                    
                    GridOrdenCompra.DataSource = ordenCompraList;
                    GridOrdenCompra.DataBind();
                }
                RadAutoCompleteBox.Enabled = false;
                txtCantidad.Enabled = false;
                lnkAgregarProd.Visible = false;
                RadAutoCompleteBox.Entries.Clear();
                txtCantidad.Text = "";
            }

            if (GridOrdenCompra.Rows.Count > 0)
                lnkGenerarOrden.Visible = true;
            else
                lnkGenerarOrden.Visible = false;
        }
        catch (Exception ex) {
            lblError.Text = "Error al cargar la información: " + ex.Message;
        }
    }
}