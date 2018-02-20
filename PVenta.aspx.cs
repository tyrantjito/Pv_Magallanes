using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using Telerik.Web.UI;
using iTextSharp.text.pdf;
using System.IO;
using E_Utilities;

public partial class PVenta : System.Web.UI.Page
{
    List<Venta> articulosVenta;
    decimal subtotal, iva, totalVenta;
    decimal porcIva = appSettingsIva.getIva;
    string usuarioLog;
    Fechas fechas = new Fechas();


    protected void Page_Load(object sender, EventArgs e)
    {
        //txtProd.Attributes.Add("onkeypress", "KeyPress()");
        CierreCaja cierre = new CierreCaja();

        cierre.FechaDia = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
        cierre._horaDia = fechas.obtieneFechaLocal().ToString("HH:mm:ss");
        try
        {
            cierre.Punto = Convert.ToInt32(Request.QueryString["p"]);
        }
        catch (Exception)
        {
            string alerta = "alert('Su sesión a caducado, vuelva a ingresar')";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "login", alerta, true);
            Response.Redirect("Default.aspx");
        }
        cierre.existeCierreDia();
        if (cierre.cierreDia)
        {
            string alerta = "alert('Ya se ha realizado el corte del día y no es posible realizar más ventas por el día de hoy')";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "cierre", alerta, true);
        }
        if (cierre.cierreDia)
            Response.Redirect("Default.aspx");

        inactivaPrecioVenta();

        if (!IsPostBack)
        {
            txtProd.Focus();
            lblError.Text = "";
            lblProducto.Text = "";
            //txtReferencia.Text = "";
            ddlBanco.SelectedValue = "";
            ddlTarjeta.SelectedValue = "";
            //txtReferencia.Enabled = false;
            ddlBanco.Enabled = false;
            ddlTarjeta.Enabled = false;
            Label3.Enabled = false;
            Label4.Enabled = false;
            imgProducto.Visible = false;
            Session["venta"] = null;
            Session["idVenta"] = 0;
            grvVenta.DataSource = null;
            grvVenta.DataBind();
            lblSubtotal.Text = "0.00";
            lblTotal.Text = "$0.00";
            lblIva.Text = "0.00";
            //RequiredFieldValidator1.Enabled = false;
            RequiredFieldValidator2.Enabled = false;
            RequiredFieldValidator5.Enabled = false;
            //btnAceptarVenta.Visible = false;
            //btnCancelarVenta.Visible = false;
            btnAgregar.Visible = false;

           /* if (txtProcentaje.Visible)
                txtProcentaje.Text = "1.46";
            else
                txtProcentaje.Text = "0.00";*/
            lblPrecioVentaOriginal.Text = "0.00";
            txtCantidad.Text = "1";
            RadAutoCompleteBox.Entries.Clear();
            RadAutoCompleteBox.TextSettings.SelectionMode = (RadAutoCompleteSelectionMode)Enum.Parse(typeof(RadAutoCompleteSelectionMode), "Single", true);
            Session["agregado"] = false;
                        

            lblMonto.Text = lblCambio.Text = lblRestan.Text = "0.00";
            txtMontoPagado.Text = "";
            btnPagar.Visible = false;

            chkIntereses.Enabled = false;
            chkIntereses.Checked = false;
            ddlParcial.Enabled = false;
            radDiferimiento.Enabled = false;
            radDiferimiento.Value = 0;
            ddlParcial.SelectedValue = "00";

            txtOrden.Text = "";
            ddlTaller.SelectedValue = "-1";
            txtCliente.Text = "MONCAR AZTAHUACAN";
            ddlProv.SelectedValue = "0";
            ddlAreaApp.SelectedIndex = 0;
            chkVtaTaller.Checked = chkVtaCredito.Checked = chkEspecial.Checked = false;
            txtDsctoGral.Text = "0.00";
        }
    }

    protected void ddlFormaPago_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFormaPago.SelectedValue == "A" || ddlFormaPago.SelectedValue == "D")
        {
            //txtReferencia.Text = "";
            ddlBanco.SelectedValue = "";
            ddlTarjeta.SelectedValue = "";
            //txtReferencia.Enabled = true;
            ddlBanco.Enabled = true;
            ddlTarjeta.Enabled = true;
            Label3.Enabled = true;
            Label4.Enabled = true;
            //RequiredFieldValidator1.Enabled = true;
            RequiredFieldValidator2.Enabled = true;
            RequiredFieldValidator5.Enabled = true;
            chkIntereses.Enabled = true;
            chkIntereses.Checked = false;
            ddlParcial.Enabled = false;
            radDiferimiento.Enabled = false;
            radDiferimiento.Value = 0;
            ddlParcial.SelectedValue = "00";
        }
        else
        {
            //txtReferencia.Text = "";
            ddlBanco.SelectedValue = "";
            ddlTarjeta.SelectedValue = "";
            //txtReferencia.Enabled = false;
            ddlBanco.Enabled = false;
            ddlTarjeta.Enabled = false;
            Label3.Enabled = false;
            Label4.Enabled = false;
            //RequiredFieldValidator1.Enabled = false;
            RequiredFieldValidator2.Enabled = false;
            RequiredFieldValidator5.Enabled = false;

            chkIntereses.Enabled = false;
            chkIntereses.Checked = false;
            ddlParcial.Enabled = false;
            radDiferimiento.Enabled = false;
            radDiferimiento.Value = 0;
            ddlParcial.SelectedValue = "00";
        }
    }

    [WebMethod]
    [ScriptMethod]
    public static List<string> obtieneProductos(string prefixText)
    {
        List<string> lista = new List<string>();
        SqlConnection conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
        try
        {
            conexionBD.Open();
            SqlCommand cmd = new SqlCommand("select aa.idarticulo+' / '+c.descripcion as descripcion from articulosalmacen aa inner join catproductos c on c.idproducto=aa.idarticulo where aa.idalmacen=2 ", conexionBD);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            IDataReader lector = cmd.ExecuteReader();
            while (lector.Read())
            {
                lista.Add(lector.GetString(0));
            }
            lector.Close();
        }
        catch (Exception) { }
        finally { conexionBD.Close(); }
        return lista;
    }

    protected void RadAutoCompleteBox1_TextChanged(object sender, AutoCompleteTextEventArgs e)
    {

        lblProducto.Text = "";
        lblError.Text = "";
        imgProducto.Visible = false;
        lblCostoVenta.Text = "0.00";
        //txtCantidad.Text = "1";
        txtVentaAgranel.Text = "";
        lblPrecioVentaOriginal.Text = "0.00";
        lblTotalProducto.Text = "";
        txtCantidad.Enabled = true;
        lblCostoVenta.Enabled = true;
        inactivaPrecioVenta();
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
                    //imgProducto.Visible = true;
                    if (Convert.ToString(valor[1]) != "")
                    {
                        int pv;
                        try { pv = Convert.ToInt32(Request.QueryString["p"]); }
                        catch (Exception) { pv = 0; }
                        if (pv != 0)
                        {
                            produc.Isla = pv;
                            produc.existeIsla();
                            lblProducto.Text = produc.ClaveProducto;
                            imgProducto.ImageUrl = "~/imgProductos.ashx?id=" + produc.ClaveProducto.Trim();
                            object[] venta = produc.obtienePrecioVenta();
                            if (Convert.ToBoolean(venta[0]))
                                lblCostoVenta.Text = Convert.ToDecimal(venta[1]).ToString();
                            else
                                lblCostoVenta.Text = "0.00";

                            lblPrecioVentaOriginal.Text = lblCostoVenta.Text;
                            object[] ventaAgranel = produc.vendeAgranel();
                            if (Convert.ToBoolean(ventaAgranel[0]))
                                txtVentaAgranel.Visible = Convert.ToBoolean(ventaAgranel[1]);
                            else
                                txtVentaAgranel.Visible = false;

                            if (lblCostoVenta.Text == "0.00")
                            {
                                lblProducto.Text = "El producto no esta disponible para su venta";
                                imgProducto.ImageUrl = "img/noDisponible.png";
                                txtCantidad.Enabled = false;
                                txtVentaAgranel.Visible = false;
                                inactivaPrecioVenta();
                              /*  if (txtProcentaje.Visible)
                                    txtProcentaje.Text = "1.46";
                                else
                                    txtProcentaje.Text = "0.00";*/
                                lblCostoVenta.Enabled = false;
                            }
                            else {

                                decimal precio = 0;
                                decimal porcentaje = 0;
                                try { precio = Convert.ToDecimal(lblPrecioVentaOriginal.Text); } catch (Exception) { precio = 0; }
                               /* try { porcentaje = Convert.ToDecimal(txtProcentaje.Text); } catch (Exception) { porcentaje = 0; }*/

                                if (porcentaje != 0)
                                {
                                    precio = precio * (1 + (porcentaje / 100));
                                    lblCostoVenta.Text = txtVentaAgranel.Text = precio.ToString("F2");
                                }

                                if (!txtVentaAgranel.Visible)
                                {
                                    lblTotalProducto.Text = calculaTotal(lblCostoVenta.Text, txtCantidad.Text).ToString("C2");
                                    //lblMonto.Text = calculaTotal(lblCostoVenta.Text, txtCantidad.Text).ToString("F2");
                                }
                                else
                                {
                                    lblTotalProducto.Text = txtVentaAgranel.Text = calculaImporteGranel(lblCostoVenta.Text, txtCantidad.Text, txtVentaAgranel.Text, 1);
                                    //lblMonto.Text = calculaImporteGranel(lblCostoVenta.Text, txtCantidad.Text, txtVentaAgranel.Text, 1);
                                }
                            }
                        }
                        else
                            lblError.Text = "Su sesión a caducado por favor vuelva a ingresar";
                    }
                    else
                    {
                        lblProducto.Text = "El producto no existe";
                        imgProducto.ImageUrl = "img/noDisponible.png";
                    }
                }
                else
                {
                    lblProducto.Text = "";
                    imgProducto.Visible = false;
                    lblError.Text = Convert.ToString(valor[1]);
                }
            }
            else
            {
                lblProducto.Text = "";
                imgProducto.Visible = false;
            }

            txtCantidad.Focus();
        }
        catch (Exception ex) { lblProducto.Text = ""; lblError.Text = ex.Message; imgProducto.Visible = false; }


    }

    protected void txtProducto_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtCantidad_TextChanged(object sender, EventArgs e)
    {
        decimal precio = 0;
        decimal porcentaje = 0;
        try { precio = Convert.ToDecimal(lblCostoVenta.Text); } catch (Exception) { precio = 0; }
       /* try { porcentaje = Convert.ToDecimal(txtProcentaje.Text); } catch (Exception) { porcentaje = 0; }*/

        if (porcentaje != 0)
        {
            precio = precio * (1 + (porcentaje / 100));
            lblCostoVenta.Text = txtVentaAgranel.Text = precio.ToString("F2");
        }
        if (!txtVentaAgranel.Visible)
            lblTotalProducto.Text = calculaTotal(lblCostoVenta.Text, txtCantidad.Text).ToString("C2");
        else
            lblTotalProducto.Text = txtVentaAgranel.Text = calculaImporteGranel(lblCostoVenta.Text, txtCantidad.Text, txtVentaAgranel.Text, 1);
    }

    private string calculaImporteGranel(string costo, string cantidad, string monto, int calculo)
    {
        string valorRet = "0";
        decimal total = 0;
        string valor = "";
        for (int j = 0; j < costo.Length; j++)
        {
            if (char.IsDigit(costo[j]))
                valor = valor.Trim() + costo[j];
            else
            {
                if (costo[j] == '.')
                    valor = valor.Trim() + costo[j];
            }
        }
        string venta = valor.Trim();

        try
        {
            decimal valorUnit = 0;
            decimal cant = 0;
            decimal importe = 0;

            if (calculo == 1)
            {
                valorUnit = Convert.ToDecimal(venta);
                cant = Convert.ToDecimal(cantidad);
                importe = cant * valorUnit;
                valorRet = importe.ToString("F2");
                total = importe;
            }
            else
            {
                valorUnit = Convert.ToDecimal(venta);
                importe = Convert.ToDecimal(monto);
                cant = importe / valorUnit;
                valorRet = cant.ToString("F3");
                total = importe;
            }
        }
        catch (Exception ex) { valor = "0"; }

        if (total == 0)
            btnAgregar.Visible = false;
        else
            btnAgregar.Visible = true;

        return valorRet;
    }

    protected void lblCostoVenta_TextChanged(object sender, EventArgs e)
    {
        Producto produc = new Producto();
        produc.ClaveProducto = lblProducto.Text;
        int pv;
        try { pv = Convert.ToInt32(Request.QueryString["p"]); }
        catch (Exception)
        {
            pv = 0;
        }
        produc.Isla = pv;
        object[] precioVta = produc.obtienePrecioVenta();
        if ((bool)precioVta[0])
        {
            decimal valor = (decimal)precioVta[1];
            decimal precioV = Convert.ToDecimal(lblCostoVenta.Text);
            /*if (precioV > valor)
            {
                lblError.Text = "El precio de venta no puede ser mayor al último precio de venta vigente";
                lblCostoVenta.Text = "0.00";
            }*/
        }

        decimal precio = 0;
        decimal porcentaje = 0;
        try { precio = Convert.ToDecimal(lblCostoVenta.Text); } catch (Exception) { precio = 0; }
       /* try { porcentaje = Convert.ToDecimal(txtProcentaje.Text); } catch (Exception) { porcentaje = 0; }*/

        if (porcentaje != 0)
        {
            precio = precio * (1 + (porcentaje / 100));
            lblCostoVenta.Text = txtVentaAgranel.Text = precio.ToString("F2");
        }
        if (!txtVentaAgranel.Visible)
            lblTotalProducto.Text = calculaTotal(lblCostoVenta.Text, txtCantidad.Text).ToString("F2");
        else
            lblTotalProducto.Text = txtVentaAgranel.Text = calculaImporteGranel(lblCostoVenta.Text, txtCantidad.Text, txtVentaAgranel.Text, 1);
    }

    protected void txtVentaAgranel_TextChanged(object sender, EventArgs e)
    {
        decimal precio = 0;
        decimal porcentaje = 0;
        try { precio = Convert.ToDecimal(lblCostoVenta.Text); } catch (Exception) { precio = 0; }
     /*   try { porcentaje = Convert.ToDecimal(txtProcentaje.Text); } catch (Exception) { porcentaje = 0; }*/

        if (porcentaje != 0)
        {
            precio = precio * (1 + (porcentaje / 100));
            lblCostoVenta.Text = txtVentaAgranel.Text = precio.ToString("F2");
        }
        if (!txtVentaAgranel.Visible)
            lblTotalProducto.Text = calculaTotal(lblCostoVenta.Text, txtCantidad.Text).ToString("F2");
        else
            lblTotalProducto.Text = txtVentaAgranel.Text = calculaImporteGranel(lblCostoVenta.Text, txtCantidad.Text, txtVentaAgranel.Text, 1);
    }

    private decimal calculaTotal(string venta, string cantidad)
    {
        decimal total = 0;
        decimal precioVenta = 0;
        int cantidadProd = 0;
        string valor = "";
        for (int j = 0; j < venta.Length; j++)
        {
            if (char.IsDigit(venta[j]))
                valor = valor.Trim() + venta[j];
            else
            {
                if (venta[j] == '.')
                    valor = valor.Trim() + venta[j];
            }
        }
        venta = valor.Trim();
        if (venta == "" || cantidad == "" || venta == "0" || cantidad == "0")
            total = 0;
        else
        {
            try
            {
                precioVenta = Convert.ToDecimal(venta);
                cantidadProd = Convert.ToInt32(cantidad);
            }
            catch (Exception) { }
            finally
            {
                total = precioVenta * cantidadProd;
            }
        }
        if (total == 0)
            btnAgregar.Visible = false;
        else
            btnAgregar.Visible = true;

        return total;
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        int registro = 0;
        try { registro = Convert.ToInt32(Session["idVenta"].ToString()); }
        catch (Exception) { registro = 0; }
        if (Session["venta"] != null)
            articulosVenta = (List<Venta>)Session["venta"];
        else
            articulosVenta = new List<Venta>();

        int pv;
        try { pv = Convert.ToInt32(Request.QueryString["p"]); }
        catch (Exception) { pv = 0; }
        if (pv != 0)
        {

            Producto produc = new Producto();
            decimal existencia = produc.obtieneExistencia(lblProducto.Text, pv.ToString());
            if (existencia <= 0)
                lblError.Text = "El producto no cuenta con existencia";
            else
            {                
                bool existePrevio = false;

                foreach (Venta vnt in articulosVenta) {
                    if (vnt.clave == lblProducto.Text)
                    {
                        existePrevio = true;
                        break;
                    }
                }

                if (existePrevio)
                {
                    try
                    {
                        foreach (Venta vnt in articulosVenta)
                        {
                            if (vnt.clave == lblProducto.Text)
                            {
                                decimal precio = Convert.ToDecimal(vnt.precio);
                                decimal cantidad = Convert.ToDecimal(vnt.cantidad);
                                decimal nuevaCantidad = cantidad + Convert.ToDecimal(txtCantidad.Text);
                                decimal totalNuevo = nuevaCantidad * precio;
                                vnt.cantidad = nuevaCantidad.ToString();
                                vnt.total = totalNuevo.ToString();
                            }
                        }
                    }
                    catch (Exception) { }
                    finally {
                        btnAgregar.Visible = false;
                        UpdatePanel3.Update();
                        if (articulosVenta != null)
                            grvVenta.DataSource = articulosVenta;
                        else
                            grvVenta.DataSource = null;
                        //txtProducto.Text = "";
                        txtCantidad.Text = "1";
                        lblProducto.Text = "";
                        txtVentaAgranel.Text = "";
                        txtVentaAgranel.Visible = false;
                        lblTotalProducto.Text = "";
                        lblCostoVenta.Text = "";
                        imgProducto.ImageUrl = null;
                        imgProducto.Visible = false;
                        grvVenta.DataBind();
                        grvVenta.PageIndex = grvVenta.PageCount;

                        btnPagar.Visible = true;
                        obtieneTotales(articulosVenta);
                        RadAutoCompleteBox.Entries.Clear();
                        RadAutoCompleteBox.TextSettings.SelectionMode = (RadAutoCompleteSelectionMode)Enum.Parse(typeof(RadAutoCompleteSelectionMode), "Single", true);
                    }
                }
                else
                {

                    string[] argumentos = RadAutoCompleteBox.Text.ToString().Split(new char[] { '/' });
                    string[] producto = argumentos[1].Trim().Split(new char[] { ';' });
                    Venta articulo = new Venta();
                    articulo.clave = lblProducto.Text;
                    articulo.producto = producto[0].Trim();
                    decimal costo = 0;
                    try { costo = Convert.ToDecimal(lblCostoVenta.Text); } catch (Exception) { costo = 0; }
                    articulo.precio = costo.ToString("F2");
                    articulo.cantidad = txtCantidad.Text;
                    articulo.total = lblTotalProducto.Text;
                    articulo.renglon = (registro + 1).ToString();
                    try
                    {
                        articulosVenta.Add(articulo);
                        //btnAceptarVenta.Visible = true;

                        Session["venta"] = articulosVenta;
                        Session["idVenta"] = registro + 1;
                    }
                    catch (Exception) { }
                    finally
                    {
                        btnAgregar.Visible = false;
                        UpdatePanel3.Update();
                        if (articulosVenta != null)
                            grvVenta.DataSource = articulosVenta;
                        else
                            grvVenta.DataSource = null;
                        //txtProducto.Text = "";
                        txtCantidad.Text = "1";
                        lblProducto.Text = "";
                        txtVentaAgranel.Text = "";
                        txtVentaAgranel.Visible = false;
                        lblTotalProducto.Text = "";
                        lblCostoVenta.Text = "";
                        imgProducto.ImageUrl = null;
                        imgProducto.Visible = false;
                        grvVenta.DataBind();

                        btnPagar.Visible = true;
                        //btnCancelarVenta.Visible = true;
                        grvVenta.PageIndex = grvVenta.PageCount;
                        obtieneTotales(articulosVenta);
                        RadAutoCompleteBox.Entries.Clear();
                        RadAutoCompleteBox.TextSettings.SelectionMode = (RadAutoCompleteSelectionMode)Enum.Parse(typeof(RadAutoCompleteSelectionMode), "Single", true);
                    }
                }

                txtProd.Focus();
                radTabOpciones.SelectedIndex = 0;
            }
        }
        else
            Response.Redirect("Default.aspx");
    }

    protected void grvVenta_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        articulosVenta = (List<Venta>)Session["venta"];
        grvVenta.PageIndex = e.NewPageIndex;
        grvVenta.DataSource = articulosVenta;
        grvVenta.DataBind();
        UpdatePanel3.Update();
    }

    protected void grvVenta_RowEditing(object sender, GridViewEditEventArgs e)
    {
        articulosVenta = (List<Venta>)Session["venta"];
        grvVenta.EditIndex = e.NewEditIndex;
        Session["venta"] = articulosVenta;
        grvVenta.DataSource = articulosVenta;
        grvVenta.DataBind();
        grvVenta.PageIndex = grvVenta.PageCount;
        obtieneTotales(articulosVenta);
        UpdatePanel3.Update();
    }

    protected void grvVenta_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        articulosVenta = (List<Venta>)Session["venta"];
        grvVenta.EditIndex = -1;
        Session["venta"] = articulosVenta;
        grvVenta.DataSource = articulosVenta;
        grvVenta.DataBind();
        grvVenta.PageIndex = grvVenta.PageCount;
        obtieneTotales(articulosVenta);
        UpdatePanel3.Update();
    }

    protected void txtModCantidad_TextChanged(object sender, EventArgs e)
    {
        articulosVenta = (List<Venta>)Session["venta"];
        Label lblArticulo = (Label)grvVenta.Rows[grvVenta.EditIndex].FindControl("lblIdArticulo");
        TextBox txtCantidadMod = (TextBox)grvVenta.Rows[grvVenta.EditIndex].FindControl("txtModCantidad");
        Label lblTotalArt = (Label)grvVenta.Rows[grvVenta.EditIndex].FindControl("lblTotalMod");
        Label lblPrecioMod = (Label)grvVenta.Rows[grvVenta.EditIndex].FindControl("lblPrecioMod");
        lblTotalArt.Text = calculaTotal(lblPrecioMod.Text, txtCantidadMod.Text).ToString("C2");
        
        string cant = txtCantidadMod.Text;
        string tot = lblTotalArt.Text;
        int id = Convert.ToInt32(lblArticulo.Text);
        if (cant != "0" && cant != "")
        {
            foreach (Venta item in articulosVenta)
            {
                if (item.renglon.Equals(id))
                {
                    item.cantidad = cant;
                    item.total = tot;
                }
            }
        }
        Session["venta"] = articulosVenta;
        grvVenta.DataSource = articulosVenta;
        grvVenta.DataBind();
        obtieneTotales(articulosVenta);
    }

    protected void grvVenta_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        TextBox txtCantidadMod = (TextBox)grvVenta.Rows[grvVenta.EditIndex].FindControl("txtModCantidad");
        Label lblTotalArt = (Label)grvVenta.Rows[grvVenta.EditIndex].FindControl("lblTotalMod");
        Label lblIdArticulo = (Label)grvVenta.Rows[grvVenta.EditIndex].FindControl("lblIdArticulo");
        articulosVenta = (List<Venta>)Session["venta"];
        string id = lblIdArticulo.Text;
        string cant = txtCantidadMod.Text;
        string tot = lblTotalArt.Text;
        if (cant != "0" && cant != "")
        {
            foreach (Venta item in articulosVenta)
            {
                if (item.renglon.Equals(id))
                {
                    item.cantidad = cant;
                    item.total = tot;
                }
            }
        }
        grvVenta.EditIndex = -1;
        Session["venta"] = articulosVenta;
        grvVenta.DataSource = articulosVenta;
        grvVenta.DataBind();
        grvVenta.PageIndex = grvVenta.PageCount;
        obtieneTotales(articulosVenta);
        UpdatePanel3.Update();
    }

    protected void grvVenta_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string id = grvVenta.DataKeys[e.RowIndex].Value.ToString();
        articulosVenta = (List<Venta>)Session["venta"];
        foreach (Venta item in articulosVenta)
        {
            if (item.renglon.Equals(id))
            {
                articulosVenta.Remove(item);
                break;
            }
        }
        Session["venta"] = articulosVenta;
        grvVenta.DataSource = articulosVenta;
        grvVenta.DataBind();
        grvVenta.PageIndex = grvVenta.PageCount;
        obtieneTotales(articulosVenta);
        UpdatePanel3.Update();
    }

    private void obtieneTotales(List<Venta> articulosVendidos)
    {
        subtotal = 0;
        iva = 0;
        totalVenta = 0;
        decimal importe = 0;
        if (articulosVendidos != null)
        {
            foreach (Venta item in articulosVendidos)
            {
                string venta = item.total;
                if (venta != "")
                {
                    venta = convierteMontos(venta);
                }
                if (venta == "")
                    importe = 0;
                else
                {
                    try { importe = Convert.ToDecimal(venta); }
                    catch (Exception) { importe = 0; }
                }

                subtotal += importe;
            }

            subtotal = subtotal / Convert.ToDecimal(1.16);

            if (articulosVendidos.Count > 0)
            {
                //btnAceptarVenta.Visible = true;
                //btnCancelarVenta.Visible = true;
            }
            else
            {
                //btnAceptarVenta.Visible = false;
                //btnCancelarVenta.Visible = true;
            }
        }
        else
        {
            //btnAceptarVenta.Visible = false;
            //btnCancelarVenta.Visible = false;
        }
        lblSubtotal.Text = lblTotal.Text = subtotal.ToString("F2");
        calculaDsctoGral(); //Alx 27-10/2016
        //txtMontoPagado.Text = "";
        lblCambio.Text = lblRestan.Text = "0.00";        
    }

    private string convierteMontos(string importe)
    {
        if (importe != "")
        {
            importe = importe.Replace('$', ',');
            importe = importe.Replace(',', ' ');
            string valor = "";
            for (int j = 0; j < importe.Length; j++)
            {
                if (char.IsDigit(importe[j]))
                    valor = valor.Trim() + importe[j];
                else
                {
                    if (importe[j] == '.')
                        valor = valor.Trim() + importe[j];
                }
            }
            importe = valor.Trim();
        }
        else
            importe = "0";
        return importe;
    }

    protected void btnAceptarVenta_Click(object sender, EventArgs e)
    {
        generaVentaNueva();
    }

    private void registraNotificaciones(List<Venta> articulosVenta, int isla, int caja, string usuario, string fecha, string hora, string ticketGenerado)
    {
        int notificaciones = 0;
        foreach (Venta item in articulosVenta)
        {
            Producto produc = new Producto();
            produc.ClaveProducto = item.clave;
            produc.Isla = isla;
            //Notificacion para precios de venta menores 

            object[] venta = produc.obtienePrecioVenta();
            decimal precioVenta = 0;
            if (Convert.ToBoolean(venta[0]))
            {
                precioVenta = Convert.ToDecimal(venta[1]);
                if (Convert.ToDecimal(item.precio) < precioVenta)
                {
                    if (Convert.ToDecimal(item.precio) >= 0)
                    {
                        Notificaciones notifi = new Notificaciones();
                        notifi.Articulo = item.clave;
                        notifi.Punto = isla;
                        notifi.Caja = caja;
                        notifi.Usuario = usuario;
                        notifi.Fecha = Convert.ToDateTime(fecha);
                        notifi.Entrada = Convert.ToInt32(ticketGenerado);
                        notifi.Estatus = "P";
                        notifi.Clasificacion = 1;
                        notifi.Extra = item.precio.ToString();
                        notifi.Origen = "V";
                        notifi.armaNotificacion();
                        notifi.agregaNotificacion();
                        if ((bool)notifi.Retorno[0])
                            notificaciones++;
                    }
                }
            }

            //Notificacion para existencias negativas

            decimal existenciaActual = produc.obtieneExistencia(produc.ClaveProducto, isla.ToString());
            if (existenciaActual < 0)
            {
                Notificaciones notifi = new Notificaciones();
                notifi.Articulo = item.clave;
                notifi.Punto = isla;
                notifi.Usuario = usuario;
                notifi.Fecha = Convert.ToDateTime(fecha);
                notifi.Estatus = "P";
                notifi.Clasificacion = 3;
                notifi.Extra = existenciaActual.ToString();
                notifi.Origen = "V";
                notifi.armaNotificacion();
                notifi.agregaNotificacion();
                if ((bool)notifi.Retorno[0])
                    notificaciones++;
            }
        }

        if (notificaciones != 0)
        {
            lblError.Text = "Se notificó al administrador del sistema posibles fallas de la aplicación";
        }

    }
    protected void btnCancelarVenta_Click(object sender, EventArgs e)
    {
        cancelaDatos();
    }
    protected void btnImprimir_Click(object sender, EventArgs e)
    {
        Imprime();
    }
    protected void btnCancelarImpresion_Click(object sender, EventArgs e)
    {
        Session["venta"] = null;
        Session["idVenta"] = 0;
        pnlTicket.Visible = false;
        PanelMask.Visible = false;
        cancelaDatos();
    }

    private void cancelaDatos()
    {
        lblError.Text = "";
        //txtReferencia.Text = "";
        ddlBanco.SelectedValue = "";
        ddlTarjeta.SelectedValue = "";
        //txtReferencia.Enabled = false;
        ddlBanco.Enabled = false;
        ddlTarjeta.Enabled = false;
        Label3.Enabled = false;
        Label4.Enabled = false;
        imgProducto.Visible = false;
        Session["venta"] = null;
        Session["idVenta"] = 0;
        grvVenta.EditIndex = -1;
        grvVenta.DataSource = null;
        grvVenta.DataBind();
        lblSubtotal.Text = "0.00";
        lblTotal.Text = "$0.00";
        lblIva.Text = "0.00";
        //RequiredFieldValidator1.Enabled = false;
        RequiredFieldValidator2.Enabled = false;
        RequiredFieldValidator5.Enabled = false;
        //btnAceptarVenta.Visible = false;
        //btnCancelarVenta.Visible = false;
        btnAgregar.Visible = false;
        ddlFormaPago.SelectedValue = "E";
        obtieneTotales((List<Venta>)Session["venta"]);
        lblTicket.Text = "";
        //Response.Redirect("PuntoVenta.aspx?u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"] + "&p=" + Request.QueryString["p"] + "&np=" + Request.QueryString["np"] + "&c=" + Request.QueryString["c"]);
        Response.Redirect("PVenta.aspx?u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"] + "&p=" + Request.QueryString["p"] + "&np=" + Request.QueryString["np"] + "&c=" + Request.QueryString["c"]);
    }

    protected void chkFacturacionPend_CheckedChanged(object sender, EventArgs e)
    {
        if (chkFacturacionPend.Checked)
        {
            Label34.Visible = true;
            txtRFCVerifica.Visible = true;
            btnRevisaRFC.Visible = true;
            lblRazon.Visible = true;
        }
        else
        {
            Label34.Visible = false;
            lblRazon.Visible = false;
            txtRFC.Text = "";
            lblRazon.Text = "";
            txtRFCVerifica.Visible = false;
            btnRevisaRFC.Visible = false;
        }
    }

    protected void rbtnPersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        string tipoPersona = rbtnPersona.SelectedValue;
        if (tipoPersona == "M")
        {
            Label20.Visible = false;
            Label19.Visible = txtArchivarNombre.Visible = true;
            txtNombre.Visible = txtPaterno.Visible = txtMaterno.Visible = false;
            if (txtRFC.Text.Trim().Length != 12)
                lblErrorFacCliente.Text = "El R.F.C. No coincide con el tipo de persona seleccionado, verifique sus datos.";
        }
        else if (tipoPersona == "F")
        {
            Label20.Visible = true;
            Label19.Visible = txtArchivarNombre.Visible = false;
            txtNombre.Visible = txtPaterno.Visible = txtMaterno.Visible = true;
            if (txtRFC.Text.Trim().Length != 13)
                lblErrorFacCliente.Text = "El R.F.C. No coincide con el tipo de persona seleccionado, verifique sus datos.";
        }
    }

    protected void btnNuevoCliente_Click(object sender, EventArgs e)
    {
        if (rbtnPersona.SelectedValue == "M" && txtRFC.Text.Trim().Length == 12 || rbtnPersona.SelectedValue == "F" && txtRFC.Text.Trim().Length == 13)
        {
            ClientesDatos datosCli = new ClientesDatos();
            string tipoPersona = rbtnPersona.SelectedValue;
            lblErrorFacCliente.Text = "";
            string nombre = "";
            string paterno = "";
            string materno = "";
            string razon = "";
            string cp = "";
            string rfc = "";
            string fechaRFC = "";
            string edad = "";
            string fechaNacimiento = "";
            bool vacios = false;
            if (tipoPersona == "M")
            {
                razon = txtArchivarNombre.Text;
                if (razon.Trim() == "")
                {
                    vacios = true;
                    lblErrorFacCliente.Text = "Necesita colocar la razón social.";
                }
                rfc = txtRFC.Text.ToUpper();
                if (rfc.Length != 12)
                {
                    vacios = true;
                    lblErrorFacCliente.Text = "El R.F.C. No corresponde con el tipo de persona.";
                }
                fechaRFC = rfc.Substring(3, 6);
            }
            else if (tipoPersona == "F")
            {
                nombre = txtNombre.Text;
                paterno = txtPaterno.Text;
                materno = txtMaterno.Text;
                if (nombre.Trim() == "" || paterno.Trim() == "")
                {
                    vacios = true;
                    lblErrorFacCliente.Text = "Necesita colocar el nombre y el apellido paterno.";
                }
                razon = nombre + " " + paterno + " " + materno;
                rfc = txtRFC.Text.ToUpper();
                if (rfc.Length != 13)
                {
                    vacios = true;
                    lblErrorFacCliente.Text = "El R.F.C. No corresponde con el tipo de persona.";
                }
                int inicio = 0;
                if (rbtnPersona.SelectedValue == "M")
                    inicio = 0;
                else if (rbtnPersona.SelectedValue == "F")
                    inicio = 1;
                fechaRFC = rfc.Substring(4 + inicio, 6 + inicio);
            }
            try
            {
                cp = txtCP.Text;
                if (cp.Length != 5)
                {
                    lblErrorFacCliente.Text = "El cÓdigo postal debe ser de 5 dígitos.";
                    vacios = true;
                }
            }
            catch (Exception ex)
            {
                lblErrorFacCliente.Text = "El codigo postal debe ser de 5 dígitos.";
                vacios = true;
            }
            try
            {
                string año = fechaRFC.Substring(0, 2);
                string mes = fechaRFC.Substring(2, 2);
                string dia = fechaRFC.Substring(4, 2);
                DateTime fec = fechas.obtieneFechaLocal(); 
                try
                {
                    fec = Convert.ToDateTime(dia + "/" + mes + "/" + año);
                }
                catch (Exception)
                {
                    lblErrorFacCliente.Text = "El R.F.C. No coincide con el tipo de persona seleccionado, verifique sus datos.";
                }
                TimeSpan edadTS = fechas.obtieneFechaLocal() - fec;
                int dias = Convert.ToInt32(edadTS.TotalDays);
                int años = Convert.ToInt32(dias / 365);
                edad = Convert.ToString(años);
                fechaNacimiento = fec.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                lblErrorFacCliente.Text = "Hubo un error al procesar el R.F.C. verifique sus datos. " + ex.Message;
                vacios = true;
            }
            if (!vacios)
            {
                string sexo = "";
                string calle = txtCalle.Text;
                string numero = txtNumero.Text;
                string colonia = txtColonia.Text;
                string ciudad = txtCiudad.Text;
                string estado = txtEstado.Text;
                string correo = txtEMail.Text;
                string numeroInt = txtNumeroInt.Text;
                string delegacion = txtDelegacion.Text;
                string pais = "México";
                string referncia = txtReferenciaFacCliente.Text;

                bool existe = datosCli.existeClienteRFC(rfc);
                if (!existe)
                {
                    bool insertado = datosCli.agregaCliente(rfc, razon, calle, numero, colonia, cp, ciudad, estado, tipoPersona, paterno, materno, nombre, correo, sexo, edad, fechaNacimiento, numeroInt, delegacion, referncia, pais);
                    if (!insertado)
                        lblErrorFacCliente.Text = "Hubo un problema al insertar el cliente, verifique su conexión e intentelo nuevamente.";
                }
                else
                {
                    int id = datosCli.obtieneIdClienteRFC(rfc);
                    bool insertado = datosCli.actualizaCliente(rfc, razon, calle, numero, colonia, cp, ciudad, estado, tipoPersona, paterno, materno, nombre, correo, sexo, edad, fechaNacimiento, numeroInt, delegacion, referncia, pais, id);
                    if (!insertado)
                        lblErrorFacCliente.Text = "Hubo un problema al actualizar el cliente, verifique su conexión e intentelo nuevamente.";

                }
            }
        }
        lblErrorFacCliente.Text = "El formato del R.F.C. No coincide con el tipo de persona seleccionado, verifique su información.";
    }

    protected void btnCancelarCliente_Click(object sender, EventArgs e)
    {
        try
        {
            ClientesDatos datosCli = new ClientesDatos();
            int idCliente = datosCli.obtieneIdClienteRFC(txtRFC.Text.ToUpper());
            string isla = Request.QueryString["p"];
            string ticket = lblNumTicket.Text;
            bool actualizaVentEncab = datosCli.actualizaVentaEncabezado(idCliente, isla, ticket, 1, fechas.obtieneFechaLocal().ToString("yyyy-MM-dd"), fechas.obtieneFechaLocal().ToString("HH:mm:ss"), ticket.ToString());
            if (actualizaVentEncab)
            {
                pnlTicket.Visible = true;
                btnCancelarCliente.Enabled = false;
                chkFacturacionPend.Enabled = false;
                pnlCliente.Enabled = false;
                btnRevisaRFC.Enabled = false;
                txtRFCVerifica.Enabled = false;
                lblErrorFacCliente.Text = "Su factura será enviada al correo electrónico proporcionado.";
            }
            else
            {
                lblErrorFacCliente.Text = "Se produjo un error al solicitar la factura, profavor intente más tarde.";
            }
        }
        catch (Exception) { }
    }

    protected void btnRevisaRFC_Click(object sender, EventArgs e)
    {
        ClientesDatos datosCli = new ClientesDatos();
        string rfc = txtRFCVerifica.Text.ToUpper();
        chkFacturacionPend.Enabled = false;
        bool existe = false;
        if (rfc.Trim() != "")
        {
            pnlCliente.Visible = true;
            existe = datosCli.existeClienteRFC(rfc);
            if (existe)
            {
                lblErrorFacCliente.Text = "";
                int idCliente = datosCli.obtieneIdClienteRFC(rfc);
                ClientesDatos cliente = new ClientesDatos();
                object[] datos = cliente.obtieneDatosCliente(rfc.ToUpper());
                if (Convert.ToBoolean(datos[0]))
                {
                    DataSet valores = (DataSet)datos[1];
                    foreach (DataRow fila in valores.Tables[0].Rows)
                    {
                        rbtnPersona.SelectedValue = fila[7].ToString();
                        txtRFC.Text = rfc;
                        txtArchivarNombre.Text = fila[0].ToString();
                        txtNombre.Text = fila[10].ToString();
                        txtPaterno.Text = fila[8].ToString();
                        txtMaterno.Text = fila[9].ToString();
                        txtCalle.Text = fila[1].ToString();
                        txtNumero.Text = fila[2].ToString();
                        txtNumeroInt.Text = fila[3].ToString();
                        txtColonia.Text = fila[12].ToString();
                        txtDelegacion.Text = fila[13].ToString();
                        txtEstado.Text = fila[6].ToString();
                        txtCiudad.Text = fila[5].ToString();
                        txtCP.Text = fila[4].ToString().PadLeft(5, '0');
                        txtEMail.Text = fila[11].ToString();
                        //txtReferencia.Text = fila[14].ToString();
                    }
                }
                else
                    lblErrorFacCliente.Text = "Error: " + Convert.ToString(datos[1]);
            }
            else
            {
                lblErrorFacCliente.Text = "El cliente indicado no se encuentra, agregue su información";
                lblRazon.Text = "";
                rbtnPersona.SelectedValue = "M";
                txtArchivarNombre.Text = "";
                txtNombre.Text = "";
                txtPaterno.Text = "";
                txtMaterno.Text = "";
                txtCalle.Text = "";
                txtNumero.Text = "";
                txtNumeroInt.Text = "";
                txtColonia.Text = "";
                txtDelegacion.Text = "";
                txtEstado.Text = "";
                txtCiudad.Text = "";
                txtCP.Text = "";
                txtEMail.Text = "";
                //txtReferencia.Text = "";
                txtRFC.Text = rfc;
                if (rfc.Length == 12)
                {
                    rbtnPersona.SelectedValue = "M";
                    Label20.Visible = false;
                    Label19.Visible = txtArchivarNombre.Visible = true;
                    txtNombre.Visible = txtPaterno.Visible = txtMaterno.Visible = false;
                }
                else if (rfc.Length == 13)
                {
                    rbtnPersona.SelectedValue = "F";
                    Label20.Visible = true;
                    Label19.Visible = txtArchivarNombre.Visible = false;
                    txtNombre.Visible = txtPaterno.Visible = txtMaterno.Visible = true;
                }
            }
        }
        else
        {
            pnlCliente.Visible = false;
            lblErrorFacCliente.Text = "Debe introducir un R.F.C.";
        }

    }
    
    private void inactivaPrecioVenta()
    {
        bool[] _permisos = new bool[36];
        for (int i = 0; i < _permisos.Length; i++)
        {
            _permisos[i] = false;
        }
        try { usuarioLog = Convert.ToString(Request.QueryString["u"]); }
        catch (Exception)
        {
            string alerta = "alert('Su sesión a caducado, por favor vuelva a ingresar')";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "cierre", alerta, true);
        }
        if (usuarioLog == "")
            Response.Redirect("Default.aspx");
        else
        {
            ObtienePermisos permisos = new ObtienePermisos();
            permisos.Usuario = usuarioLog;
            permisos.obtienePermisos();
            _permisos = permisos.Permisos;
            permisos.PermisoBuscado = 35;
            permisos.cuentaPermiso();
            lblCostoVenta.Enabled = permisos.Permitido;
          /*  Label13.Visible = permisos.Permitido;
            txtProcentaje.Visible = permisos.Permitido;*/
        }
    }
    protected void btnValidar_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtProd.Text != "")
            {
                //Timer1.Enabled = false;

                string producto = txtProd.Text;//.PadLeft(15, '0');
                Producto produc = new Producto();

                produc.ClaveProducto = Convert.ToString(producto.Trim());
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
                            produc.Isla = pv;
                            produc.existeIsla();
                            lblProducto.Text = produc.ClaveProducto;
                            decimal precioVenta = 0;
                            object[] venta = produc.obtienePrecioVenta();
                            if (Convert.ToBoolean(venta[0]))
                                precioVenta = Convert.ToDecimal(venta[1]);

                            if (precioVenta == 0)
                                lblError.Text = "El producto indicado no cuenta con precio";
                            else
                            {
                                int registro = 0;
                                try { registro = Convert.ToInt32(Session["idVenta"].ToString()); }
                                catch (Exception) { registro = 0; }
                                if (Session["venta"] != null)
                                    articulosVenta = (List<Venta>)Session["venta"];
                                else
                                    articulosVenta = new List<Venta>();

                                Venta articulo = new Venta();
                                articulo.clave = producto;
                                articulo.producto = Convert.ToString(valor[1]);
                                articulo.precio = precioVenta.ToString();
                                articulo.cantidad = "1";
                                articulo.total = (precioVenta * 1).ToString();
                                articulo.renglon = (registro + 1).ToString();
                                try
                                {
                                    articulosVenta.Add(articulo);
                                    //btnAceptarVenta.Visible = true;
                                   
                                    Session["venta"] = articulosVenta;
                                    Session["idVenta"] = registro + 1;
                                }
                                catch (Exception) { }
                                finally
                                {
                                    UpdatePanel3.Update();
                                    if (articulosVenta != null)
                                        grvVenta.DataSource = articulosVenta;
                                    else
                                        grvVenta.DataSource = null;

                                    grvVenta.DataBind();
                                    grvVenta.PageIndex = grvVenta.PageCount;
                                    
                                    producto = txtProd.Text = "";
                                    precioVenta = 0;
                                    obtieneTotales(articulosVenta);
                                    lblError.Text = "";
                                    txtProd.Focus();
                                    //Timer1.Enabled = true;
                                }
                            }
                            //btnCancelarVenta.Visible = true;
                        }
                        else
                            lblError.Text = "Su sesión a caducado por favor vuelva a ingresar";
                    }
                    else
                        lblError.Text = "El producto no existe";
                }
                else
                    lblError.Text = Convert.ToString(valor[1]);
            }
        }
        catch (Exception ex) { lblError.Text = ex.Message; }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        
            txtProd.Focus();
            try
            {
                if (txtProd.Text != "")
                {
                    //Timer1.Enabled = false;

                    string producto = txtProd.Text;//.PadLeft(15, '0');
                    Producto produc = new Producto();

                    produc.ClaveProducto = Convert.ToString(producto.Trim());
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
                                produc.Isla = pv;
                                produc.existeIsla();
                                lblProducto.Text = produc.ClaveProducto;
                                decimal precioVenta = 0;
                                object[] venta = produc.obtienePrecioVenta();
                                if (Convert.ToBoolean(venta[0]))
                                    precioVenta = Convert.ToDecimal(venta[1]);

                                if (precioVenta == 0)
                                    lblError.Text = "El producto indicado no cuenta con precio";
                                else
                                {
                                    int registro = 0;
                                    try { registro = Convert.ToInt32(Session["idVenta"].ToString()); }
                                    catch (Exception) { registro = 0; }
                                    if (Session["venta"] != null)
                                        articulosVenta = (List<Venta>)Session["venta"];
                                    else
                                        articulosVenta = new List<Venta>();

                                    Venta articulo = new Venta();
                                    articulo.clave = producto;
                                    articulo.producto = Convert.ToString(valor[1]);
                                    articulo.precio = precioVenta.ToString();
                                    articulo.cantidad = "1";
                                    articulo.total = (precioVenta * 1).ToString();
                                    articulo.renglon = (registro + 1).ToString();
                                    try
                                    {
                                        articulosVenta.Add(articulo);
                                        //btnAceptarVenta.Visible = true;
                                        
                                        Session["venta"] = articulosVenta;
                                        Session["idVenta"] = registro + 1;
                                    }
                                    catch (Exception) { }
                                    finally
                                    {
                                        UpdatePanel3.Update();
                                        if (articulosVenta != null)
                                            grvVenta.DataSource = articulosVenta;
                                        else
                                            grvVenta.DataSource = null;

                                        grvVenta.DataBind();
                                        grvVenta.PageIndex = grvVenta.PageCount;
                                        
                                        producto = txtProd.Text = "";
                                        precioVenta = 0;
                                        obtieneTotales(articulosVenta);
                                        lblError.Text = "";
                                        //Timer1.Enabled = true;
                                    }
                                }
                                //btnCancelarVenta.Visible = true;
                            }
                            else
                                lblError.Text = "Su sesión a caducado por favor vuelva a ingresar";
                        }
                        else
                            lblError.Text = "El producto no existe";
                    }
                    else
                        lblError.Text = Convert.ToString(valor[1]);
                }
            }
            catch (Exception ex) { lblError.Text = ex.Message; }
        
    }

    protected void txtProd_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtProd.Text != "")
            {
                //Timer1.Enabled = false;

                bool agregado;
                try { agregado = Convert.ToBoolean(Session["agregado"]); }
                catch (Exception) { agregado = false; }


                string producto = txtProd.Text;//.PadLeft(15, '0');
                Producto produc = new Producto();

                produc.ClaveProducto = Convert.ToString(producto.Trim());
                //object[] existencia = produc.hayExistencia(,Convert.ToString(producto.Trim()));
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
                            produc.Isla = pv;
                            produc.existeIsla();
                            lblProducto.Text = produc.ClaveProducto;
                            decimal precioVenta = 0;
                            object[] venta = produc.obtienePrecioVenta();
                            if (Convert.ToBoolean(venta[0]))
                                precioVenta = Convert.ToDecimal(venta[1]);

                            if (precioVenta == 0)
                                lblError.Text = "El producto indicado no cuenta con precio";
                            else
                            {
                                decimal existencia = Convert.ToDecimal(produc.obtieneExistencia(produc.ClaveProducto, pv.ToString()));
                                if (existencia <= 0)
                                    lblError.Text = "El producto no cuenta con existencia";
                                else
                                {

                                    int registro = 0;
                                    try { registro = Convert.ToInt32(Session["idVenta"].ToString()); }
                                    catch (Exception) { registro = 0; }
                                    if (Session["venta"] != null)
                                        articulosVenta = (List<Venta>)Session["venta"];
                                    else
                                        articulosVenta = new List<Venta>();


                                    bool existePrevio = false;

                                    foreach (Venta vnt in articulosVenta)
                                    {
                                        if (vnt.clave == producto)
                                        {
                                            existePrevio = true;
                                            break;
                                        }
                                    }

                                    if (existePrevio && !agregado || existePrevio && agregado)
                                    {

                                        try
                                        {
                                            foreach (Venta vnt in articulosVenta)
                                            {
                                                if (vnt.clave == producto)
                                                {
                                                    decimal precio = Convert.ToDecimal(vnt.precio);
                                                    decimal cantidad = Convert.ToDecimal(vnt.cantidad);
                                                    int cantidadNew = 0;
                                                    decimal nuevaCantidad = 0;
                                                    try { cantidadNew = Convert.ToInt32(txtCantidadNew.Text); } catch (Exception) { cantidadNew = 0; }
                                                    nuevaCantidad = cantidad + cantidadNew;
                                                    decimal totalNuevo = nuevaCantidad * precio;
                                                    vnt.cantidad = nuevaCantidad.ToString();
                                                    vnt.total = totalNuevo.ToString();
                                                    Session["agregado"] = true;
                                                    txtCantidadNew.Value = 1;
                                                }
                                            }
                                        }
                                        catch (Exception) { }
                                        finally
                                        {
                                            UpdatePanel3.Update();
                                            if (articulosVenta != null)
                                                grvVenta.DataSource = articulosVenta;

                                            else
                                                grvVenta.DataSource = null;

                                            grvVenta.DataBind();

                                            btnPagar.Visible = true;
                                            //btnCancelarVenta.Visible = true;

                                            grvVenta.PageIndex = grvVenta.PageCount;
                                            producto = txtProd.Text = "";
                                            precioVenta = 0;
                                            obtieneTotales(articulosVenta);
                                            lblError.Text = "";
                                            txtCantidadNew.Text = "1";
                                            txtProd.Focus();
                                            //Timer1.Enabled = true;
                                        }

                                    }
                                    else if (!existePrevio && !agregado)
                                    {
                                        Venta articulo = new Venta();
                                        articulo.clave = producto;
                                        articulo.producto = Convert.ToString(valor[1]);
                                        articulo.precio = precioVenta.ToString("F2");
                                        int cantidadNew = 1;
                                        try { cantidadNew = Convert.ToInt32(txtCantidadNew.Text); } catch (Exception) { cantidadNew = 1; }
                                        articulo.cantidad = cantidadNew.ToString();
                                        articulo.total = (precioVenta * cantidadNew).ToString("F2");
                                        articulo.renglon = (registro + 1).ToString();
                                        try
                                        {
                                            articulosVenta.Add(articulo);
                                            //btnAceptarVenta.Visible = true;

                                            Session["venta"] = articulosVenta;
                                            Session["idVenta"] = registro + 1;
                                            Session["agregado"] = true;
                                        }
                                        catch (Exception) { Session["agregado"] = false; }
                                        finally
                                        {
                                            UpdatePanel3.Update();
                                            if (articulosVenta != null)
                                                grvVenta.DataSource = articulosVenta;
                                            else
                                                grvVenta.DataSource = null;

                                            grvVenta.DataBind();
                                            grvVenta.PageIndex = grvVenta.PageCount;
                                            btnPagar.Visible = true;
                                            //btnCancelarVenta.Visible = true;

                                            producto = txtProd.Text = "";
                                            precioVenta = 0;
                                            obtieneTotales(articulosVenta);
                                            lblError.Text = "";
                                            txtProd.Focus();

                                            //Timer1.Enabled = true;
                                        }
                                    }
                                    else if (!existePrevio && agregado)
                                    {
                                        Venta articulo = new Venta();
                                        articulo.clave = producto;
                                        articulo.producto = Convert.ToString(valor[1]);
                                        articulo.precio = precioVenta.ToString("F2");
                                        int cantidadNew = 1;
                                        try { cantidadNew = Convert.ToInt32(txtCantidadNew.Text); } catch (Exception) { cantidadNew = 1; }
                                        articulo.cantidad = cantidadNew.ToString();
                                        articulo.total = (precioVenta * cantidadNew).ToString("F2");
                                        articulo.renglon = (registro + 1).ToString();
                                        try
                                        {
                                            articulosVenta.Add(articulo);
                                            //btnAceptarVenta.Visible = true;

                                            Session["venta"] = articulosVenta;
                                            Session["idVenta"] = registro + 1;
                                            Session["agregado"] = true;
                                        }
                                        catch (Exception) { Session["agregado"] = false; }
                                        finally
                                        {
                                            UpdatePanel3.Update();
                                            if (articulosVenta != null)
                                                grvVenta.DataSource = articulosVenta;
                                            else
                                                grvVenta.DataSource = null;

                                            grvVenta.DataBind();
                                            grvVenta.PageIndex = grvVenta.PageCount;
                                            btnPagar.Visible = true;
                                            //btnCancelarVenta.Visible = true;

                                            producto = txtProd.Text = "";
                                            precioVenta = 0;
                                            obtieneTotales(articulosVenta);
                                            lblError.Text = "";
                                            txtProd.Focus();

                                            //Timer1.Enabled = true;
                                        }
                                    }
                                    else
                                    {
                                        UpdatePanel3.Update();
                                        if (articulosVenta != null)
                                            grvVenta.DataSource = articulosVenta;
                                        else
                                            grvVenta.DataSource = null;

                                        grvVenta.DataBind();
                                        grvVenta.PageIndex = grvVenta.PageCount;

                                        btnPagar.Visible = true;
                                        //btnCancelarVenta.Visible = true;

                                        producto = txtProd.Text = "";
                                        precioVenta = 0;
                                        obtieneTotales(articulosVenta);
                                        lblError.Text = "";
                                        txtProd.Focus();
                                        Session["agregado"] = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblError.Text = "Su sesión a caducado por favor vuelva a ingresar";
                            txtProd.Text = "";
                            RadAutoCompleteBox.Focus();
                        }
                    }
                    else
                    {
                        lblError.Text = "El producto no existe";
                        txtProd.Text = "";
                        RadAutoCompleteBox.Focus();
                    }
                }
                else
                {
                    lblError.Text = Convert.ToString(valor[1]);
                    txtProd.Text = "";
                    RadAutoCompleteBox.Focus();
                }
                txtCantidadNew.Value = 1;
                txtProd.Focus();
            }
        }
        catch (Exception ex) { lblError.Text = ex.Message; }
        finally { txtProd.Focus(); txtCantidadNew.Value = 1; }
    }

    protected void btnEliminar_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;        
        string id = btn.CommandArgument;//grvVenta.DataKeys[e.RowIndex].Value.ToString();
        articulosVenta = (List<Venta>)Session["venta"];
        foreach (Venta item in articulosVenta)
        {
            if (item.renglon.Equals(id))
            {
                articulosVenta.Remove(item);
                break;
            }
        }
        Session["venta"] = articulosVenta;
        grvVenta.DataSource = articulosVenta;
        grvVenta.DataBind();
        grvVenta.PageIndex = grvVenta.PageCount;
        obtieneTotales(articulosVenta);
        UpdatePanel3.Update();
        //btnCancelarVenta.Visible = true;
    }
    protected void txtMontoPagado_TextChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        try
        {
            decimal montoPagar = Convert.ToDecimal(lblMonto.Text);
            decimal pagado = Convert.ToDecimal(txtMontoPagado.Text);
            if (montoPagar != 0)
            {
                if ((pagado - montoPagar) < 0)
                    lblCambio.Text = "0.00";
                else
                    lblCambio.Text = (pagado - montoPagar).ToString("F2");
                if ((montoPagar - pagado) < 0)
                    lblRestan.Text = "0.00";
                else
                    lblRestan.Text = (montoPagar - pagado).ToString("F2");
            }
            else
                lblError.Text = "El monto a pagar debe ser mayor a cero.";
        }
        catch (Exception ex) {
            lblError.Text = "El monto indicado no es correcto, favor de verificar.";
        }
    }

    protected void btnPagar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        decimal montoPagado = 0;
        decimal restan = 0;
        try { montoPagado = Convert.ToDecimal(txtMontoPagado.Text); }
        catch (Exception ex) { montoPagado = 0; }
        if (chkVtaCredito.Checked || (!chkVtaCredito.Checked & montoPagado != 0))
        {
            /*ALX: Validaciones para guardar venta a taller en Registro_Pinturas*/
            if (chkVtaTaller.Checked && (string.IsNullOrEmpty(txtOrden.Text.Trim()) || ddlTaller.SelectedValue == "-1" || ddlRegistro.SelectedValue=="-1"))
            {
                lblError.Text = "";
            }
            else
            {
                if (chkVtaTaller.Checked && buscaOrdenes() == 0)
                {
                    lblError.Text = "";
                }
                else
                {
                    if (txtCliente.Text == "")
                        lblError.Text = "";
                    else {

                        try { restan = Convert.ToDecimal(lblRestan.Text); }
                        catch (Exception) { restan = Convert.ToDecimal(lblMonto.Text); }
                        if (restan != 0)
                        {
                            btnPagar.Visible = true;
                            procesaPagos();
                        }
                        else
                        {
                            procesaPagos();
                        }
                    }
                }

            }
        }
        else
            lblError.Text = "";
        Cancelacion act = new Cancelacion();
        act.ticket = Convert.ToInt32(lblTicket.Text);
        act.requisicion = txtrequi.Text;
        act.entrega = txtentrega.Text;
        act.actualizaReqEnt();
        grvVenta.Visible = false;
        grvVenta.DataSource = null;
        grvVenta.DataBind();

    }

    private int buscaOrdenes()
    {
        string sqlExOrd = "SELECT COUNT(no_orden) FROM Ordenes_Reparacion WHERE no_orden=" + txtOrden.Text.Trim();
        BaseDatos ejec = new BaseDatos();
        object[] exOrd = ejec.scalarToInt(sqlExOrd);
        return int.Parse(exOrd[1].ToString());
    }
    private void procesaPagos()
    {
        decimal importe=0;
        decimal pago =0;
        decimal resta=0;
        decimal cambio=0;
        int ticket = 0;
        string requi = "";
        string entrega = "";


        try{

            importe = Convert.ToDecimal(lblMonto.Text);
            pago = Convert.ToDecimal(txtMontoPagado.Text);
            resta = Convert.ToDecimal(lblRestan.Text);
            cambio = Convert.ToDecimal(lblCambio.Text);
            requi = txtrequi.Text;
            entrega = txtentrega.Text;

            if (ddlFormaPago.SelectedValue == "E")
            {
                if (chkVtaCredito.Checked) {
                    generaVentaNueva();
                    try { ticket = Convert.ToInt32(lblNumTicket.Text); }
                    catch (Exception) { ticket = 0; }
                    Pagos pagoTicket = new Pagos();
                    pagoTicket.ticket = ticket;
                    pagoTicket.caja = Convert.ToInt32(Request.QueryString["c"]);
                    pagoTicket.punto = Convert.ToInt32(Request.QueryString["p"]);
                    pagoTicket.formaPago = ddlFormaPago.SelectedValue;
                    pagoTicket.referencia = "";// txtReferencia.Text;
                    pagoTicket.banco = ddlBanco.SelectedValue;
                    pagoTicket.tarjeta = ddlTarjeta.SelectedValue;
                    pagoTicket.monto = Convert.ToDecimal(txtMontoPagado.Text);
                    pagoTicket.cambio = Convert.ToDecimal(lblCambio.Text);
                    pagoTicket.restan = Convert.ToDecimal(txtMontoPagado.Text);
                    pagoTicket.registraPago();
                    object[] datos = pagoTicket.retorno;
                    if (!Convert.ToBoolean(datos[0]))
                        lblError.Text = "";
                }
                else
                {

                    if (resta == 0)
                    {
                        generaVentaNueva();
                        try { ticket = Convert.ToInt32(lblNumTicket.Text); }
                        catch (Exception) { ticket = 0; }
                        Pagos pagoTicket = new Pagos();
                        pagoTicket.ticket = ticket;
                        pagoTicket.caja = Convert.ToInt32(Request.QueryString["c"]);
                        pagoTicket.punto = Convert.ToInt32(Request.QueryString["p"]);
                        pagoTicket.formaPago = ddlFormaPago.SelectedValue;
                        pagoTicket.referencia = "";// txtReferencia.Text;
                        pagoTicket.banco = ddlBanco.SelectedValue;
                        pagoTicket.tarjeta = ddlTarjeta.SelectedValue;
                        pagoTicket.monto = Convert.ToDecimal(txtMontoPagado.Text);
                        pagoTicket.cambio = Convert.ToDecimal(lblCambio.Text);
                        pagoTicket.restan = Convert.ToDecimal(lblRestan.Text);
                        pagoTicket.registraPago();
                        object[] datos = pagoTicket.retorno;
                        if (!Convert.ToBoolean(datos[0]))
                            lblError.Text = "";

                        //lblError.Text = "Venta pagada en su totalidad";
                    }
                    else
                        lblError.Text = "";
                }
                if (Convert.ToDecimal(resta) == 0)
                        btnPagar.Visible = false;
                    else
                        btnPagar.Visible = true;
                
            }
            else
            {
                if (resta == 0)
                    generaPagoTarjetaCompleto();
                else
                    generaPagoTarjeta();

                
            }
        }
        catch (Exception ex) { lblError.Text = ""; }
    }

    private void generaPagoTarjeta()
    {
        lblError.Text = "";
        int terminal = 0;
        string clave = "";
        int intentos = 0;
        int ticket = 0;
        try
        {
            Islas pv = new Islas();
            pv.Almacen = Convert.ToInt32(Request.QueryString["p"]);
            object[] datos_pv = pv.obtieneParametrosTarjeta();
            if (Convert.ToBoolean(datos_pv[0]))
            {
                try
                {
                    DataSet info = (DataSet)datos_pv[1];
                    foreach (DataRow fila in info.Tables[0].Rows)
                    {
                        terminal = Convert.ToInt32(fila[0].ToString());
                        clave = fila[1].ToString();
                        intentos = Convert.ToInt32(fila[2].ToString());
                    }
                }
                catch (Exception) { terminal = 0; }
            }

            if (terminal == 0)
            {
                lblError.Text = "No es posible realizar pagos con tarjetas ya que no tiene una terminal definida";
                btnPagar.Visible = true;
            }
            else
            {
                if (clave == "")
                {
                    lblError.Text = "No es posible realizar pagos con tarjetas ya que no se cuenta con la clave proporcionada por el proveedor de servicios";
                    btnPagar.Visible = true;
                }
                else
                {

                    decimal importe = 0;
                    try { importe = Convert.ToDecimal(txtMontoPagado.Text); }
                    catch (Exception ex) { importe = 0; }

                    decimal totalPagar = 0;
                    try { totalPagar = Convert.ToDecimal(lblMonto.Text); }
                    catch (Exception) { totalPagar = 0; }

                    if (totalPagar != 0)
                    {
                        if (importe > totalPagar)
                            lblError.Text = "El valor pagar no puede ser mayor al valor del total de la venta";
                        else
                        {

                            PagosPinPad pagos = new PagosPinPad();
                            if (chkIntereses.Checked)
                                pagos.opcion = "03";
                            else
                                pagos.opcion = "00";

                            pagos.terminal = terminal;
                            pagos.clave = clave;
                            pagos.ticket = 0;
                            pagos.caja = Convert.ToInt32(Request.QueryString["c"]);
                            pagos.punto = Convert.ToInt32(Request.QueryString["p"]);
                            pagos.usuario = Request.QueryString["u"];

                            if (importe != 0)
                            {
                                pagos.importe = importe;
                                pagos.nombre = Request.QueryString["np"];//txtNombrePago.Text;
                                pagos.concepto = "Compra de Productos";//txtConcepto.Text;
                                pagos.correo = "e-apps@outlook.com";//txtCorreoPago.Text;
                                pagos.referencia = "";// txtReferencia.Text;
                                pagos.folio = "";
                                pagos.parcializacion = ddlParcial.SelectedValue;
                                pagos.diferimiento = radDiferimiento.Text.PadLeft(2, '0');
                                pagos.iteraciones = intentos;
                                DateTime fecha = fechas.obtieneFechaLocal();
                                pagos.fecha = fecha.ToString("ddMMyyyy");
                                pagos.ejecutaPeticion();
                                object[] respuesta = pagos._retorno;
                                if (Convert.ToBoolean(respuesta[0]))
                                {
                                    lblError.Text = Convert.ToString(respuesta[1]);
                                }
                                else
                                    lblError.Text = Convert.ToString(respuesta[1]);
                            }
                            else
                                lblError.Text = "El importe a pagar debe ser mayor a cero y/o indique un monto válido";
                            string[] codigos = pagos.codigos;
                            int codigo = Convert.ToInt32(codigos[0]);
                            if (codigo > 99 && codigo < 116)
                                lblError.Text = "No se pudo realizar el pago. " + codigos[1];
                            else
                            {
                                if (codigo == 2)
                                {
                                    if (Convert.ToDecimal(lblRestan.Text) == 0)
                                        generaVentaNueva();

                                    try { ticket = Convert.ToInt32(lblNumTicket.Text); }
                                    catch (Exception) { ticket = 0; }
                                    Pagos pagoTicket = new Pagos();
                                    pagoTicket.caja = Convert.ToInt32(Request.QueryString["c"]);
                                    pagoTicket.punto = Convert.ToInt32(Request.QueryString["p"]);
                                    if (ticket == 0)
                                    {
                                        pagoTicket.ticket = ticket;
                                        pagoTicket.formaPago = ddlFormaPago.SelectedValue;
                                        pagoTicket.referencia = "";// txtReferencia.Text;
                                        pagoTicket.banco = ddlBanco.SelectedValue;
                                        pagoTicket.tarjeta = ddlTarjeta.SelectedValue;
                                        pagoTicket.monto = Convert.ToDecimal(txtMontoPagado.Text);
                                        pagoTicket.cambio = Convert.ToDecimal(lblCambio.Text);
                                        pagoTicket.restan = Convert.ToDecimal(lblRestan.Text);
                                        pagoTicket.registraPago();
                                        object[] datos = pagoTicket.retorno;
                                        if (!Convert.ToBoolean(datos[0]))
                                            lblError.Text = "Error: " + datos[1].ToString();
                                    }
                                    else
                                    {
                                        pagoTicket.ticket = ticket;
                                        pagoTicket.actualizaTickets();
                                        pagoTicket.formaPago = ddlFormaPago.SelectedValue;
                                        pagoTicket.referencia = "";// txtReferencia.Text;
                                        pagoTicket.banco = ddlBanco.SelectedValue;
                                        pagoTicket.tarjeta = ddlTarjeta.SelectedValue;
                                        pagoTicket.monto = Convert.ToDecimal(txtMontoPagado.Text);
                                        pagoTicket.cambio = Convert.ToDecimal(lblCambio.Text);
                                        pagoTicket.restan = Convert.ToDecimal(lblRestan.Text);
                                        pagoTicket.registraPago();
                                        object[] datos = pagoTicket.retorno;
                                        if (!Convert.ToBoolean(datos[0]))
                                            lblError.Text = "Error: " + datos[1].ToString();

                                        pagos.ticket = ticket;
                                        pagos.caja = Convert.ToInt32(Request.QueryString["c"]);
                                        pagos.punto = Convert.ToInt32(Request.QueryString["p"]);
                                        pagos.usuario = Request.QueryString["u"];
                                        pagos.actualizaDatos();
                                    }

                                    if (Convert.ToDecimal(lblRestan.Text) == 0)
                                        btnPagar.Visible = false;
                                    else
                                    {
                                        txtMontoPagado.Text = lblRestan.Text;
                                        lblRestan.Text = "0.00";
                                        btnPagar.Visible = true;
                                    }
                                }
                                else
                                    btnPagar.Visible = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error: " + ex.Message;
        }
    }
    protected void chkIntereses_CheckedChanged(object sender, EventArgs e)
    {
        if (chkIntereses.Checked)
        {
            ddlParcial.Enabled = true;
            radDiferimiento.Enabled = true;
            radDiferimiento.Value = 0;
            ddlParcial.SelectedValue = "00";
        }
        else {
            ddlParcial.Enabled = false;
            radDiferimiento.Enabled = false;
            radDiferimiento.Value = 0;
            ddlParcial.SelectedValue = "00";
        }
    }

    private void generaVentaNueva() {
        if (Request.QueryString["p"] != null && Request.QueryString["u"] != null && Request.QueryString["c"] != null)
        {
            object[] ticket = new object[25];
            ticket[0] = ticket[1] = Request.QueryString["p"];
            ticket[2] = Request.QueryString["c"];
            ticket[3] = Request.QueryString["u"];
            ticket[4] = convierteMontos(lblSubtotal.Text);
            ticket[5] = convierteMontos(lblIva.Text);
            ticket[6] = convierteMontos(lblTotal.Text);//lblMonto se usaba antes de desglozar el iva, lblTotal tiene el total con descuentos sin iva
            ticket[7] = porcIva;
            ticket[8] = fechas.obtieneFechaLocal();
            ticket[9] = fechas.obtieneFechaLocal().ToString("HH:mm:ss");
            ticket[10] = ddlFormaPago.SelectedValue;
            ticket[11] = "";// txtReferencia.Text;
            ticket[12] = ddlBanco.Text;
            ticket[13] = "";
            ticket[14] = Session["venta"];
            ticket[15] = ddlTarjeta.SelectedValue;
            ticket[16] = txtOrden.Text;
            ticket[17] = ddlTaller.SelectedValue;
            ticket[18] = ddlAreaApp.SelectedValue;
            ticket[19] = txtDsctoGral.Text;
            ticket[20] = lblMtoDcto.Text;
            ticket[21] = ddlProv.SelectedValue;
            ticket[22] = txtCliente.Text;
            ticket[23] = chkVtaCredito.Checked;
            ticket[24] = chkVtaTaller.Checked;

            GeneraVenta genera = new GeneraVenta();
            object[] generado = genera.generaVenta(ticket);
            if (Convert.ToBoolean(generado[0]))
            {
                

                Session["venta"] = null;
                Session["idVenta"] = 0;
                lblTicket.Text = lblNumTicket.Text = generado[1].ToString();
                lblErrorFacCliente.Text = "";
                txtCP.Text = "";
                txtArchivarNombre.Text = "";
                txtNombre.Text = "";
                txtPaterno.Text = "";
                txtMaterno.Text = "";
                txtRFC.Text = "";
                txtCalle.Text = "";
                txtNumero.Text = "";
                txtColonia.Text = "";
                txtCiudad.Text = "";
                txtEstado.Text = "";
                txtEMail.Text = "";
                txtNumeroInt.Text = "";
                txtDelegacion.Text = "";
                txtReferenciaFacCliente.Text = "";
                txtRFCVerifica.Text = "";
                rbtnPersona.SelectedValue = "M";
                chkFacturacionPend.Checked = false;                
                pnlTicket.Visible = false;
                PanelMask.Visible = false;
                articulosVenta = (List<Venta>)ticket[14];
                registraNotificaciones(articulosVenta, Convert.ToInt32(ticket[0]), Convert.ToInt32(ticket[2]), Convert.ToString(ticket[3]), Convert.ToString(ticket[8]), Convert.ToString(ticket[9]), lblNumTicket.Text);
                /*Alx : Inserta en Registro_pinturas si es Venta a taller */
                if (chkVtaTaller.Checked)
                {
                    object[] actualizado = genera.actualizarTicketRegistro(txtOrden.Text, ddlTaller.SelectedValue, ddlRegistro.SelectedValue, lblNumTicket.Text);
                }
                Imprime();
                lblError.Text = "";
                //txtReferencia.Text = "";
                ddlBanco.SelectedValue = "";
                ddlTarjeta.SelectedValue = "";
                //txtReferencia.Enabled = false;
                ddlBanco.Enabled = false;
                ddlTarjeta.Enabled = false;
                Label3.Enabled = false;
                Label4.Enabled = false;
                imgProducto.Visible = false;


                txtOrden.Text = "";
                ddlTaller.SelectedValue = "-1";
                txtCliente.Text = "BUGOS MAGALLANES";
                ddlProv.SelectedValue = "0";
                ddlAreaApp.SelectedIndex = 0;                
                chkVtaTaller.Checked = chkVtaCredito.Checked = chkEspecial.Checked = false;
                txtDsctoGral.Text = "0.00";

                Session["venta"] = null;
                Session["idVenta"] = 0;
                grvVenta.EditIndex = -1;
                grvVenta.DataSource = null;
                grvVenta.DataBind();
                lblSubtotal.Text = "0.00";
                lblTotal.Text = "$0.00";
                lblIva.Text = "0.00";
                //RequiredFieldValidator1.Enabled = false;
                RequiredFieldValidator2.Enabled = false;
                RequiredFieldValidator5.Enabled = false;
                //btnAceptarVenta.Visible = false;
                //btnCancelarVenta.Visible = false;
                btnAgregar.Visible = false;
                ddlFormaPago.SelectedValue = "E";
                
                obtieneTotales((List<Venta>)Session["venta"]);
                lblTicket.Text = "";
                grvVenta.DataSource = null;
                grvVenta.DataBind();
                UpdatePanel3.Update();
                txtProd.Focus();
            }
            else
            {
                lblTicket.Text = lblNumTicket.Text = "";
                pnlTicket.Visible = false;
                PanelMask.Visible = false;
                lblError.Text = "Error al generar la venta: " + generado[1].ToString();
            }
        }
        else
        {
            btnAgregar.Visible = false;
            //btnAceptarVenta.Visible = false;
            //btnCancelarVenta.Visible = true;
            lblError.Text = "Su sesión a caducado, vuelva a ingresar para realizar de nuevo la venta";
        }
      
    }

    private void generaPagoTarjetaCompleto() {
        lblError.Text = "";
        int terminal = 0;
        string clave = "";
        int intentos = 0;
        int ticket = 0;
        try
        {
            Islas pv = new Islas();
            pv.Almacen = Convert.ToInt32(Request.QueryString["p"]);
            object[] datos_pv = pv.obtieneParametrosTarjeta();
            if (Convert.ToBoolean(datos_pv[0]))
            {
                try
                {
                    DataSet info = (DataSet)datos_pv[1];
                    foreach (DataRow fila in info.Tables[0].Rows)
                    {
                        terminal = Convert.ToInt32(fila[0].ToString());
                        clave = fila[1].ToString();
                        intentos = Convert.ToInt32(fila[2].ToString());
                    }
                }
                catch (Exception) { terminal = 0; }
            }

            if (terminal == 0)
            {
                lblError.Text = "No es posible realizar pagos con tarjetas ya que no tiene una terminal definida";
                btnPagar.Visible = true;
            }
            else
            {
                if (clave == "")
                {
                    lblError.Text = "No es posible realizar pagos con tarjetas ya que no se cuenta con la clave proporcionada por el proveedor de servicios";
                    btnPagar.Visible = true;
                }
                else
                {
                    PagosPinPad pagos = new PagosPinPad();
                    if (chkIntereses.Checked)
                        pagos.opcion = "03";
                    else
                        pagos.opcion = "00";

                    pagos.terminal = terminal;
                    pagos.clave = clave;
                    pagos.ticket = 0;
                    pagos.caja = Convert.ToInt32(Request.QueryString["c"]);
                    pagos.punto = Convert.ToInt32(Request.QueryString["p"]);
                    pagos.usuario = Request.QueryString["u"];
                    decimal importe = 0;
                    try { importe = Convert.ToDecimal(txtMontoPagado.Text); }
                    catch (Exception ex) { importe = 0; }
                    if (importe != 0)
                    {
                        pagos.importe = importe;
                        pagos.nombre = Request.QueryString["np"];//txtNombrePago.Text;
                        pagos.concepto = "Compra de Productos";//txtConcepto.Text;
                        pagos.correo = "e-apps@outlook.com";//txtCorreoPago.Text;
                        pagos.referencia = "";// txtReferencia.Text;
                        pagos.folio = "";
                        pagos.parcializacion = ddlParcial.SelectedValue;
                        pagos.diferimiento = radDiferimiento.Text.PadLeft(2, '0');
                        pagos.iteraciones = intentos;
                        DateTime fecha = fechas.obtieneFechaLocal();
                        pagos.fecha = fecha.ToString("ddMMyyyy");
                        pagos.ejecutaPeticion();
                        object[] respuesta = pagos._retorno;
                        if (Convert.ToBoolean(respuesta[0]))
                        {
                            lblError.Text = Convert.ToString(respuesta[1]);
                        }
                        else
                            lblError.Text = Convert.ToString(respuesta[1]);
                    }
                    else
                        lblError.Text = "El importe a pagar debe ser mayor a cero y/o indique un monto válido";
                    string[] codigos = pagos.codigos;
                    int codigo = Convert.ToInt32(codigos[0]);
                    if (codigo > 99 && codigo < 116)
                    {
                        lblError.Text = "No se pudo realizar el pago. " + codigos[1];
                        btnPagar.Visible = true;
                    }
                    else
                    {
                        if (codigo == 2)
                        {
                            generaVentaNueva();
                            try { ticket = Convert.ToInt32(lblNumTicket.Text); }
                            catch (Exception) { ticket = 0; }
                            Pagos pagoTicket = new Pagos();
                            pagoTicket.ticket = ticket;
                            pagoTicket.caja = Convert.ToInt32(Request.QueryString["c"]);
                            pagoTicket.punto = Convert.ToInt32(Request.QueryString["p"]);
                            pagoTicket.formaPago = ddlFormaPago.SelectedValue;
                            pagoTicket.referencia = "";//txtReferencia.Text;
                            pagoTicket.banco = ddlBanco.SelectedValue;
                            pagoTicket.tarjeta = ddlTarjeta.SelectedValue;
                            pagoTicket.monto = Convert.ToDecimal(txtMontoPagado.Text);
                            pagoTicket.cambio = Convert.ToDecimal(lblCambio.Text);
                            pagoTicket.restan = Convert.ToDecimal(lblRestan.Text);
                            pagoTicket.registraPago();
                            object[] datos = pagoTicket.retorno;
                            if (!Convert.ToBoolean(datos[0]))
                                lblError.Text = "Error: " + datos[1].ToString();

                            pagos.ticket = ticket;
                            pagos.caja = Convert.ToInt32(Request.QueryString["c"]);
                            pagos.punto = Convert.ToInt32(Request.QueryString["p"]);
                            pagos.usuario = Request.QueryString["u"];
                            pagos.actualizaDatos();
                            if (Convert.ToDecimal(lblRestan.Text) == 0)
                                btnPagar.Visible = false;
                            else
                                btnPagar.Visible = true;
                        }
                        else
                            btnPagar.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Error: " + ex.Message;
        }
    
    }


    private void Imprime() {
        TicketPv impTicket = new TicketPv();
        impTicket.Curp = txtRfcCap.Text;
        impTicket.Cliente = txtRazonNew.Text;
        impTicket.Correo = TextBox2.Text +" "+ txtNoExt.Text+" " + txtLocalidad.Text;
        impTicket.Requi = txtrequi.Text;
        impTicket.Entrada = txtentrega.Text;
        impTicket.PuntoVenta = Convert.ToInt32(Request.QueryString["p"]);
        impTicket.Ticket = Convert.ToInt32(lblNumTicket.Text);
        impTicket.Caja = Convert.ToInt32(Request.QueryString["c"]);
        string Archivo = impTicket.GenerarTicket(chkVtaTaller.Checked);
        if (Archivo != "")
        {
            try
            {
                System.IO.FileInfo filename = new System.IO.FileInfo(Archivo);
                if (filename.Exists)
                {
                    string url = "TicketPdf.aspx?a=" + filename.Name;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "pdfs", "window.open('" + url + "', 'nuevo', 'directories=no, location=no, menubar=no, scrollbars=yes, statusbar=no, titlebar=no, width=856px, height=550px');", true);
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        else
            lblError.Text = "Error al imprimir el ticket: " + lblNumTicket.Text + ", vuelva intentarlo";
    }

    protected void ddlPais_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlEstado.Text = "";
        ddlEstado.SelectedIndex = -1;
        ddlMunicipio.Text = "";
        ddlMunicipio.SelectedIndex = -1;
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        ddlCodigo.Text = "";
        ddlCodigo.SelectedIndex = -1;
    }
    protected void PreventErrorOnbinding(object sender, EventArgs e)
    {
        RadComboBox cb = sender as RadComboBox;
        cb.DataBinding -= new EventHandler(PreventErrorOnbinding);
        cb.AppendDataBoundItems = true;

        try
        {
            cb.DataBind();
            cb.Items.Clear();
        }
        catch (ArgumentOutOfRangeException)
        {
            cb.Items.Clear();
            cb.ClearSelection();
            RadComboBoxItem cbI = new RadComboBoxItem("", "0");
            cbI.Selected = true;
            cb.Items.Add(cbI);
        }
    }

    protected void ddlEstado_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlMunicipio.Text = "";
        ddlMunicipio.SelectedIndex = -1;
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        ddlCodigo.Text = "";
        ddlCodigo.SelectedIndex = -1;
    }

    protected void ddlMunicipio_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        ddlCodigo.Text = "";
        ddlCodigo.SelectedIndex = -1;
    }

    protected void ddlColonia_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ddlCodigo.Text = "";
        ddlCodigo.SelectedIndex = -1;
    }


    private int[] obtieneSesiones()
    {
        int[] sesiones = new int[6] { 0, 0, 1, 0, 0, 0 };
        try
        {
            //sesiones[0] = Convert.ToInt32(Request.QueryString["u"]);
            //sesiones[1] = Convert.ToInt32(Request.QueryString["p"]);
            //sesiones[2] = Convert.ToInt32(Request.QueryString["e"]);
            sesiones[3] = Convert.ToInt32(ddlTaller.SelectedValue);
            sesiones[4] = Convert.ToInt32(txtOrden.Text);
            //sesiones[5] = Convert.ToInt32(Request.QueryString["f"]);
        }
        catch (Exception)
        {
            sesiones = new int[4] { 0, 0, 0, 0 };
            Session["paginaOrigen"] = "Ordenes.aspx";
            Session["errores"] = "Su sesión a expirado vuelva a iniciar Sesión";
            Response.Redirect("AppErrorLog.aspx");
        }
        return sesiones;
    }

    
    protected void txtDescto_TextChanged(object sender, EventArgs e)
    {
        TextBox txtDscto = (TextBox)sender;
        float fPrecio = float.Parse(((Label)txtDscto.Parent.FindControl("lblPrecio")).Text);        
        float fCant = float.Parse(((Label)txtDscto.Parent.FindControl("lblCant")).Text);
        float fPorc_Dscto = float.Parse(txtDscto.Text);
        float fDescto = fPrecio * (fPorc_Dscto / 100);
        float fImp = fPrecio - fDescto;
        fImp = fImp * fCant;
        string strCveProd = ((Label)txtDscto.Parent.FindControl("lblClaveProd")).Text;
        articulosVenta = (List<Venta>)Session["venta"];
        articulosVenta.Find(x => x.clave == strCveProd).total = fImp.ToString("0.00");
        articulosVenta.Find(x => x.clave == strCveProd).porc_descuento = fPorc_Dscto;
        articulosVenta.Find(x => x.clave == strCveProd).descuento = fDescto;
        Session["venta"] = articulosVenta;
        grvVenta.DataSource = articulosVenta;
        grvVenta.DataBind();

        decimal decTotal = 0;
        foreach (Venta vnt in articulosVenta)
        {
            decTotal = decTotal + decimal.Parse(convierteMontos(vnt.total));
        }
        decTotal = decTotal / Convert.ToDecimal(1.16);

        subtotal = decTotal;
        
        lblSubtotal.Text = lblTotal.Text = decTotal.ToString("0.00");
        calculaDsctoGral();
        UpdatePanel3.Update();
    }

    protected void txtDsctoGral_TextChanged(object sender, EventArgs e)
    {
        calculaDsctoGral();
    }

    private void calculaDsctoGral()
    {
        if (!string.IsNullOrEmpty(txtDsctoGral.Text.Trim()))
        {
            decimal DsctoGral = decimal.Parse(txtDsctoGral.Text);
            //lblSubtotal tiene el total de los articulos sin descuento Gral.
            decimal Monto = decimal.Parse(lblSubtotal.Text);
            decimal decMtoDcto = (Monto * DsctoGral) / 100;
            lblMtoDcto.Text = decMtoDcto.ToString("F2");
            Monto -= decMtoDcto;
            //lblTotal tendra el monto con el descuento gral. SIN IVA
            lblTotal.Text = Monto.ToString("0.00");
            subtotal = Monto;
        }
        else
            txtDsctoGral.Text = lblMtoDcto.Text = "0.00";
        calcIva();
    }

    private void calcIva()
    {
        iva = subtotal * (porcIva / 100);
        totalVenta = subtotal + iva;
        lblIva.Text = iva.ToString("F2");
        lblMonto.Text = txtMontoPagado.Text = totalVenta.ToString("F2");
    }

    protected void ddlTaller_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlRegistro.DataBind();
    }

    protected void txtOrden_TextChanged(object sender, EventArgs e)
    {
        ddlRegistro.DataBind();
    }

    protected void txtProcentaje_TextChanged(object sender, EventArgs e)
    {
        decimal precio = 0;
        decimal porcentaje = 0;
        try { precio = Convert.ToDecimal(lblCostoVenta.Text); } catch (Exception) { precio = 0; }
      /*  try { porcentaje = Convert.ToDecimal(txtProcentaje.Text); } catch (Exception) { porcentaje = 0; }*/

        if (porcentaje != 0)
        {
            precio = precio * (1 + (porcentaje / 100));
            lblCostoVenta.Text = txtVentaAgranel.Text = precio.ToString("F2");
        }
        

        if (!txtVentaAgranel.Visible)
            lblTotalProducto.Text = calculaTotal(lblCostoVenta.Text, txtCantidad.Text).ToString("C2");
        else
            lblTotalProducto.Text = txtVentaAgranel.Text = calculaImporteGranel(lblCostoVenta.Text, txtCantidad.Text, txtVentaAgranel.Text, 1);
    }

    protected void chkVtaTaller_CheckedChanged(object sender, EventArgs e)
    {
        if (chkVtaTaller.Checked)
            txtDsctoGral.Text = "5.00";
        else
            txtDsctoGral.Text = "0.00";

        calculaDsctoGral();
    }

    protected void chkEspecial_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEspecial.Checked)
            txtDsctoGral.Text = "5.00";
        else
            txtDsctoGral.Text = "0.00";

        calculaDsctoGral();
    }

   



    protected void btnActualizar_Click(object sender, EventArgs e)
    {
        lblErroresFolio.Text = "";
        lblErrorFacCliente.Text = "";
        if (TextBox1.Text != "")
        {
            string rfc = TextBox1.Text;
            if (rfc.Length > 11 && rfc.Length < 14)
            {
                Cancelacion rec = new Cancelacion();
                rec.rfc = rfc;
                rec.recuperaDatos();
                if (Convert.ToBoolean(rec.retorno[0]))
                {
                    DataSet ds = (DataSet)rec.retorno[1];

                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        txtRfcCap.Text = r[1].ToString();
                        txtRazonNew.Text = r[3].ToString();
                        TextBox2.Text = r[7].ToString();
                        txtNoExt.Text = r[8].ToString();
                        txtNoIntMod.Text = r[9].ToString();
                        ddlPais.SelectedValue = r[12].ToString();
                       // ddlPais.DataBind();
                        ddlEstado.SelectedValue = r[13].ToString();
                       // ddlEstado.DataBind();
                        ddlMunicipio.SelectedValue = r[14].ToString();
                       // ddlMunicipio.DataBind();
                        ddlColonia.SelectedValue = r[15].ToString();
                        //ddlColonia.DataBind();
                        ddlCodigo.SelectedValue = r[16].ToString();
                      //  ddlCodigo.DataBind();
                        txtLocalidad.Text = r[10].ToString();
                        txtReferenciaMod.Text = r[11].ToString();
                        txtCorreo.Text = r[4].ToString();
                        txtCorreoCC.Text = r[5].ToString();
                        txtCorreoCCO.Text = r[6].ToString();
                        btnAgregaCli.Visible = false;
                        btnActualiza.Visible = true;
                    }

                }
                txtRfcCap.Text = rfc;
                txtRFC.ReadOnly=true;
                    }
            else 
            lblErrorFacCliente.Text = "Debe indicar el RFC del cliente";
            }
        }

    protected void btnMultiFacturacion_Click(object sender, EventArgs e)
    {
        Response.Redirect("MultiFacturacion.aspx?u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"] + "&p=" + Request.QueryString["p"] + "&np=" + Request.QueryString["np"] + "&c=" + Request.QueryString["c"]);
    }
    private void deshabilitaCampos()
    {
        txtRfcCap.Enabled = txtRazonNew.Enabled = txtCalle.Enabled = txtNoExt.Enabled = txtNoIntMod.Enabled = txtLocalidad.Enabled = txtReferenciaMod.Enabled = txtCorreo.Enabled = txtCorreoCC.Enabled = txtCorreoCCO.Enabled = ddlPais.Enabled = ddlEstado.Enabled = ddlMunicipio.Enabled = ddlColonia.Enabled = ddlCodigo.Enabled = rbtnPersona.Enabled = false;
    }

   
    private void vaciaDatos()
    {
        ddlPais.Text = ddlEstado.Text = ddlMunicipio.Text = ddlColonia.Text = ddlCodigo.Text = txtRazonNew.Text = txtLocalidad.Text = txtReferenciaMod.Text = txtCalle.Text = txtNoExt.Text = txtNoIntMod.Text = txtCorreo.Text = txtCorreoCC.Text = txtCorreoCCO.Text = "";
        ddlPais.SelectedIndex = ddlEstado.SelectedIndex = ddlMunicipio.SelectedIndex = ddlColonia.SelectedIndex = ddlCodigo.SelectedIndex = -1;
        //btnActualizaCliente.Visible = false;
        btnNuevoCliente.Visible = true;
    }


    protected void btnAgregaCli_Click(object sender, EventArgs e)
    {
        Clien agrega = new Clien();
        agrega.rfc = txtRfcCap.Text;
        agrega.razon = txtRazonNew.Text;
        agrega.calle = TextBox2.Text;
        agrega.numero = txtNoExt.Text;
        agrega.numeroint = txtNoIntMod.Text;
        agrega.pais = Convert.ToInt32(ddlPais.SelectedValue);
        agrega.estado = Convert.ToInt32(ddlEstado.SelectedValue);
        agrega.municipio = Convert.ToInt32(ddlMunicipio.SelectedValue);
        agrega.colonia = Convert.ToInt32(ddlColonia.SelectedValue);
        agrega.cp = Convert.ToInt32(ddlCodigo.SelectedValue);
        agrega.localidad = txtLocalidad.Text;
        agrega.referencia = txtReferenciaMod.Text;
        agrega.correo = txtCorreo.Text;
        agrega.correocc = txtCorreoCC.Text;
        agrega.correocco = txtCorreoCCO.Text;
        agrega.ingresaCliente();
        lblError.Text = "Se Agrego Correvtamente";

    }

    protected void btnActualiza_Click(object sender, EventArgs e)
    {
        Clien agrega = new Clien();
        agrega.rfc = txtRfcCap.Text;
        agrega.razon = txtRazonNew.Text;
        agrega.calle = TextBox2.Text;
        agrega.numero = txtNoExt.Text;
        agrega.numeroint = txtNoIntMod.Text;
        agrega.pais = Convert.ToInt32(ddlPais.SelectedValue);
        agrega.estado = Convert.ToInt32(ddlEstado.SelectedValue);
        agrega.municipio = Convert.ToInt32(ddlMunicipio.SelectedValue);
        agrega.colonia = Convert.ToInt32(ddlColonia.SelectedValue);
        agrega.cp = Convert.ToInt32(ddlCodigo.SelectedValue);
        agrega.localidad = txtLocalidad.Text;
        agrega.referencia = txtReferenciaMod.Text;
        agrega.correo = txtCorreo.Text;
        agrega.correocc = txtCorreoCC.Text;
        agrega.correocco = txtCorreoCCO.Text;
        agrega.actualizaCliente();
        lblError.Text = "Se Actualizo Correvtamente";
    }

    protected void ddlPais_SelectedIndexChanged1(object sender, EventArgs e)
    {

        ddlEstado.Text = "";
        ddlEstado.SelectedIndex = -1;
        ddlMunicipio.Text = "";
        ddlMunicipio.SelectedIndex = -1;
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        ddlCodigo.Text = "";
        ddlCodigo.SelectedIndex = -1;
    }

    protected void ddlEstado_SelectedIndexChanged1(object sender, EventArgs e)
    {
        ddlMunicipio.Text = "";
        ddlMunicipio.SelectedIndex = -1;
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        ddlCodigo.Text = "";
        ddlCodigo.SelectedIndex = -1;
    }

    protected void ddlMunicipio_SelectedIndexChanged1(object sender, EventArgs e)
    {
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        ddlCodigo.Text = "";
        ddlCodigo.SelectedIndex = -1;
    }

    
}

