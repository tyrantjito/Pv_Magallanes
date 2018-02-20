using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using Telerik.Web.UI;
using E_Utilities;

namespace pvAccom
{
    public partial class EntradasAlmacen : System.Web.UI.Page
    {
        Fechas fechas = new Fechas();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["contador"] = 1;
                lnkImprime.Visible = false;
                btnAgrEnt.Visible = false;
            }
            checasesiones();
        }

        private void checasesiones()
        {
            try
            {
                string usuario = Request.QueryString["u"];
                string nombre = Request.QueryString["nu"];                
            }
            catch (Exception)
            {
                Response.Redirect("Default.aspx");
            }
        }

        protected void btnAgrEnt_OnClick(object sender, EventArgs e)
        {
            grdDetProductos.Columns[7].Visible = true;
            btnOrdenes.Visible = btnGuardaEnt.Visible = RadAutoCompleteBox.Visible = txtCant.Visible = txtCosto.Visible = txtPrVta.Visible = txtImporte.Visible = lnkFFin.Visible = btnAgrProd.Visible = true;
            ddlProve.Enabled = txtDocu.Enabled = ddlTipoDoc.Enabled = true;
            lblErrorPop.Text = "";
            lblEntrada.Text = "0";
            pnlPopupNvoDoc.GroupingText = "Crea Documento";
            Session["lstEntProd"] = null;
            Session["contador"] = 1;
            pnlMask.Visible = true;
            btnOrdenes.Visible = true;
            pnlPopupNvoDoc.Visible = true;
            txtCant.Text = "";
            txtPrVta.Text = "";
            txtCosto.Text = "";
            txtImporte.Text = "";
            //txtProducto.Text = "";
            CargaGrdVacio();
            //ddlIsla.DataBind();
            //ddlIsla.SelectedValue = ddlTiendas.SelectedValue;            
            txtFechaDoc.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
            //calFechaDoc.SelectedDate = fechas.obtieneFechaLocal();
            txtDocu.Text = "";
            ddlProve.SelectedIndex = -1;
            ddlTipoDoc.SelectedIndex = 0;
            lblSubT.Text = lblIva.Text = lblTotIva.Text = lblTotal.Text = lblTotalVenta.Text = Convert.ToDecimal("0").ToString("C2");
            lblSumaProds.Text = "Total Productos: 0";
            RadAutoCompleteBox.Entries.Clear();
            RadAutoCompleteBox.TextSettings.SelectionMode = (RadAutoCompleteSelectionMode)Enum.Parse(typeof(RadAutoCompleteSelectionMode), "Single", true);
        }

        protected void grdEntradas_SelectedIndexChanged(object sender, EventArgs e)
        {
            //recargaDetalle();
        }

        #region :Código del panel para captura de entradas de productos que se despliega como una ventana modal:.
        protected void grdDetProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("AgrNvo"))
            {

            }
        }

        protected void btnGuardaEnt_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                List<pvAccom.Clases.EntradaProd> lstEntProd = (List<pvAccom.Clases.EntradaProd>)Session["lstEntProd"];
                if (lstEntProd != null)
                {
                    try
                    {
                        short intIsla = Convert.ToInt16(RadGrid1.SelectedValues["id_punto"]);
                        string strDoc = txtDocu.Text;
                        DateTime strFechaDoc = Convert.ToDateTime(txtFechaDoc.Text);
                        int intProveedor = int.Parse(ddlProve.SelectedValue);
                        string strTipoDoc = ddlTipoDoc.SelectedValue;
                        float floSubTot = 0;
                        float floTotIva = 0;
                        decimal intSumaProds = 0;
                        float iva = 0.16F; //Sacar este valor de BD o WebConfig
                        foreach (pvAccom.Clases.EntradaProd ep in lstEntProd)
                        {
                            floSubTot = floSubTot + ep.entImporte;
                            intSumaProds += Convert.ToDecimal(ep.entCant);
                        }
                        floTotIva = (floSubTot * iva);
                        float floTotal = floSubTot + floTotIva;
                        string intEntFolioID = pvAccom.Clases.EntradaProd.guardaEntEncabezado(intIsla, strDoc, strFechaDoc, intProveedor, strTipoDoc, floSubTot, floTotIva, floTotal, intSumaProds, Request.QueryString["u"], lstEntProd, lblEntrada.Text);
                        int folio = 0;
                        try { folio = Convert.ToInt32(intEntFolioID); }
                        catch (Exception) { folio = 0; }
                        if (folio != 0)
                        {
                            Session.Remove("lstEntProd");
                            //ddlIsla.SelectedIndex = 0;
                            ddlProve.SelectedIndex = 0;
                            txtDocu.Text = "";
                            ddlTipoDoc.SelectedIndex = 0;
                            txtFechaDoc.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                            CargaGrdVacio();
                            //grdDetProductos.DataBind();
                            RadGrid2.Rebind();

                            lblSubT.Text = lblIva.Text = lblTotIva.Text = lblTotal.Text = lblTotalVenta.Text = Convert.ToDecimal("0").ToString("C2");
                            lblSumaProds.Text = "Total Productos: 0";
                            btnOrdenes.Visible = false;
                            if (lblEntrada.Text != "0")
                            {
                                pnlMask.Visible = false;
                                pnlPopupNvoDoc.Visible = false;
                            }
                            lblEntrada.Text = "0";
                        }

                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Se presento un error al intentar generar la entrada, por favor refresque la ventana y vuelva a intentarlo. Detalle: " + ex.Message + "')", true);
                    }
                }
                else
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Se presento un error al intentar generar la entrada, la lista de productos ingresados se ha perdido por limite de tiempo de sesión, por favor refresque la ventana y vuelva a intentarlo')", true);
            }
            else
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", "alert('Se presento un error al intentar generar la entrada los datos son erroneos, por favor refresque la ventana y vuelva a intentarlo')", true);
        }
        protected void grdDetProductos_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdDetProductos.EditIndex = e.NewEditIndex;
            List<pvAccom.Clases.EntradaProd> lstEntProd = (List<pvAccom.Clases.EntradaProd>)Session["lstEntProd"];
            grdDetProductos.ShowFooter = false;
            grdDetProductos.DataSource = lstEntProd;
            grdDetProductos.DataBind();
        }

        protected void grdDetProductos_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            pvAccom.Clases.EntradaProd entProd = new pvAccom.Clases.EntradaProd();
            List<pvAccom.Clases.EntradaProd> lstEntProd = (List<pvAccom.Clases.EntradaProd>)Session["lstEntProd"];
            int intNoFila = int.Parse(e.Keys["entID"].ToString());
            entProd.entID = intNoFila;
            entProd.entProducto = e.NewValues["entProducto"].ToString();
            entProd.entProdDesc = e.NewValues["entProdDesc"].ToString();
            entProd.entCant = int.Parse(e.NewValues["entCant"].ToString());
            entProd.entCosto = float.Parse(e.NewValues["entCosto"].ToString());
            entProd.entImporte = float.Parse((entProd.entCant * entProd.entCosto).ToString());
            Producto prd = new Producto();
            prd.ClaveProducto = entProd.entProducto;
            prd.Isla = Convert.ToInt32(RadGrid1.SelectedValues["id_punto"].ToString());
            object[] valor = prd.obtieneUltimoCosto();
            if (Convert.ToBoolean(valor[0]))
            {
                prd.obtienePrecioVtaActivo();                               
                entProd.entPrecVtaUnit = float.Parse(prd.MontoVenta.ToString());
            }
            int index = 0;
            foreach (pvAccom.Clases.EntradaProd prod in lstEntProd) {
                if (prod.entID == intNoFila)
                    break;
                index++;
            }
            
                lstEntProd.RemoveAt(index);
                lstEntProd.Insert(index, entProd);
            
            
            Session["lstEntProd"] = lstEntProd;
            grdDetProductos.EditIndex = -1;
            grdDetProductos.ShowFooter = false;
            grdDetProductos.DataSource = lstEntProd;
            grdDetProductos.DataBind();
            ActualizaSumas(lstEntProd);
        }

        protected void grdDetProductos_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            List<pvAccom.Clases.EntradaProd> lstEntProd = (List<pvAccom.Clases.EntradaProd>)Session["lstEntProd"];
            int intEntPInd = lstEntProd.FindIndex(lsEP => lsEP.entID == int.Parse(e.Keys["entID"].ToString()));
            lstEntProd.RemoveAt(intEntPInd);
            if (lstEntProd.Count > 0)
            {
                Session["lstEntProd"] = lstEntProd;
                grdDetProductos.DataSource = lstEntProd;
                grdDetProductos.DataBind();
            }
            else
                CargaGrdVacio();
            ActualizaSumas(lstEntProd);
        }

        protected void grdDetProductos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdDetProductos.EditIndex = -1;
            grdDetProductos.DataSource = Session["lstEntProd"];
            grdDetProductos.DataBind();
        }

        protected void ActualizaSumas(List<pvAccom.Clases.EntradaProd> lstEP)
        {
            float subT = 0;
            float totIva = 0;
            float intSumaProds = 0;
            float totVenta = 0;
            float iva = 0.16F; //Sacar esta  variable de BD o WebConfig
            foreach (pvAccom.Clases.EntradaProd ep in lstEP)
            {
                subT = subT + ep.entImporte;
                totVenta = totVenta + (ep.entPrecVtaUnit * ep.entCant);
                intSumaProds += ep.entCant;
            }
            if (intSumaProds == 0)
                lblSumaProds.Visible = false;
            else
            {
                lblSumaProds.Visible = true;
                lblSumaProds.Text = "Total Productos: " + intSumaProds.ToString();
            }
            totIva = (subT * iva);
            lblSubT.Text = subT.ToString("C2");
            lblIva.Text = iva.ToString("P2");
            //lblTotIva.Text = totIva.ToString("C2");
            //lblTotal.Text = (subT + totIva).ToString("C2");
            lblTotal.Text = lblSubT.Text;
            lblTotalVenta.Text = totVenta.ToString("C2");
            if (intSumaProds > 0 && !string.IsNullOrEmpty(txtFechaDoc.Text) && !string.IsNullOrEmpty(txtDocu.Text))
                btnGuardaEnt.Enabled = true;
        }

        protected void CargaGrdVacio()
        {
            grdDetProductos.DataSource = null;
            grdDetProductos.DataBind();
        }

        #endregion

        protected void btnAgrProd_Click(object sender, EventArgs e)
        {
            lblErrorPop.Text = "";
            try
            {
                if (ddlProve.SelectedIndex != -1)
                    //if (ddlIsla.SelectedIndex != 0)
                    if (txtFechaDoc.Text != "")
                        if (RadAutoCompleteBox.Text != "")
                            if (txtCant.Text != "")
                                if (txtCosto.Text != "")
                                {
                                    short intAlmacen = short.Parse(RadGrid1.SelectedValues["id_punto"].ToString());
                                    Producto prod = new Producto();
                                    pvAccom.Clases.EntradaProd EntProd = new pvAccom.Clases.EntradaProd();
                                    List<pvAccom.Clases.EntradaProd> lstEntProd = new List<pvAccom.Clases.EntradaProd>();
                                    if (Session["lstEntProd"] != null)
                                        lstEntProd = (List<pvAccom.Clases.EntradaProd>)Session["lstEntProd"];
                                    int ultimoValor = 0;
                                    foreach (pvAccom.Clases.EntradaProd articulo in lstEntProd)
                                    {
                                        ultimoValor = articulo.entID;
                                    }

                                    EntProd.entID = ultimoValor + 1;
                                    string[] argumentos = RadAutoCompleteBox.Text.ToString().Split(new char[] { '/' });

                                    EntProd.entProducto = prod.ClaveProducto = argumentos[0].Trim(); //txtProdDesc.Text;
                                    EntProd.entProdAlm = intAlmacen;
                                    prod.Isla = intAlmacen;
                                    string[] producto = argumentos[1].Trim().Split(new char[] { ';' });
                                    EntProd.entProdDesc = producto[0].Trim();//txtProducto.Text;
                                    EntProd.entCant = float.Parse(txtCant.Text);
                                    EntProd.entCosto = float.Parse(txtCosto.Text, System.Globalization.NumberStyles.Currency);
                                    prod.obtienePrecioVtaActivo();
                                    if (!decimal.Parse(txtPrVta.Text.Trim()).Equals(prod.MontoVenta))
                                        EntProd.entPrecVtaUnit = float.Parse(txtPrVta.Text.Trim());
                                    else
                                        EntProd.entPrecVtaUnit = (float)prod.MontoVenta;
                                    EntProd.entImporte = EntProd.entCant * EntProd.entCosto;//  float.Parse(txtImporte.Text, System.Globalization.NumberStyles.Currency);
                                    lstEntProd.Add(EntProd);
                                    grdDetProductos.DataSource = lstEntProd;
                                    grdDetProductos.DataBind();
                                    Session["lstEntProd"] = lstEntProd;
                                    ActualizaSumas(lstEntProd);
                                    txtCant.Text = "";
                                    txtPrVta.Text = "";
                                    txtCosto.Text = "";
                                    txtImporte.Text = "";
                                    //txtProducto.Text = "";
                                    RadAutoCompleteBox.Entries.Clear();
                                    RadAutoCompleteBox.TextSettings.SelectionMode = (RadAutoCompleteSelectionMode)Enum.Parse(typeof(RadAutoCompleteSelectionMode), "Single", true);
                                }
                                else
                                    lblErrorPop.Text = "Necesita colocar un C. Unitario";
                            else
                                lblErrorPop.Text = "Necesita colocar una cantidad";
                        else
                            lblErrorPop.Text = "Necesita colocar un producto";
                    else
                        lblErrorPop.Text = "Necesita colocar una fecha de entrada";
                /*else
                    lblErrorPop.Text = "Necesita seleccionar una Tienda";*/
                else
                    lblErrorPop.Text = "Nesecita ingresar Proveedores al catalogo para realizar una entrada en almacen";
            }
            catch (Exception ex)
            {
                lblErrorPop.Text = "Ocurrio un error inesperado: " + ex.Message;
            }
        }

        /*protected void grdEntDet_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Producto datosProducto = new Producto();
            decimal newImporte = 0;
            TextBox txtCostoUnit = grdEntDet.Rows[e.RowIndex].FindControl("TextBox22") as TextBox;
            TextBox txtPrecioLista = grdEntDet.Rows[e.RowIndex].FindControl("TextBox31") as TextBox;
            Label lblCantidad = grdEntDet.Rows[e.RowIndex].FindControl("Label12") as Label;
            Label lblEntDetID = grdEntDet.Rows[e.RowIndex].FindControl("Label31") as Label;
            decimal costoUnit = Convert.ToDecimal(txtCostoUnit.Text);
            decimal Cantidad = Convert.ToDecimal(lblCantidad.Text);
            newImporte = Cantidad * costoUnit;
            int folioId = Convert.ToInt32(grdEntDet.DataKeys[e.RowIndex].Value.ToString());
            int idDetEnt = Convert.ToInt32(lblEntDetID.Text);
            string sql = "UPDATE entinventariodet SET entProdCostoUnit=" + costoUnit.ToString() + ", entImporte=" + newImporte.ToString() + " WHERE entFolioID=" + folioId.ToString() + " and entDetID=" + idDetEnt.ToString();
            bool actualizado = datosProducto.actualizaDetalleEntrada(sql);
            if (actualizado)
            {
                e.Cancel = true;
                bool actualizaEnc = datosProducto.actualizaEncabezado(folioId);
                grdEntDet.EditIndex = -1;
                grdEntDet.DataBind();
                if (actualizaEnc)
                    grdEntradas.DataBind();
            }
            else
                lblError.Text = "Hubo un error en la actualización, verifique su conexón e intentelo nuevamente mas tarde.";
        }*/

        protected void txtProducto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (RadAutoCompleteBox.Text != "")
                {
                    Producto produc = new Producto();
                    string[] argumentos = RadAutoCompleteBox.Text.ToString().Split(new char[] { '/' });
                    produc.NombrePorducto = argumentos[0].Trim();//txtProducto.Text;
                    object[] nomProd = produc.obtieneIdProducto();
                    if (Convert.ToBoolean(nomProd[0]))
                    {
                        produc.ClaveProducto = Convert.ToString(nomProd[1]);
                        object[] valor = produc.obtieneProducto();
                        if (Convert.ToBoolean(valor[0]))
                        {
                            if (Convert.ToString(valor[1]) != "")
                            {
                                //txtProdDesc.Text = produc.ClaveProducto;
                                txtCosto.Text = "0.00";
                                txtImporte.Text = "0.00";
                            }
                            else
                            {
                                txtCosto.Text = "";
                                txtImporte.Text = "";
                                //txtProdDesc.Text = "El producto no existe";
                            }
                        }
                        else
                        {
                            //txtProdDesc.Text = "";
                            lblError.Text = Convert.ToString(valor[1]);
                        }
                    }
                }
                else
                {
                    //txtProdDesc.Text = "";
                }
            }
            catch (Exception ex) { //txtProdDesc.Text = ""; 
                lblError.Text = ex.Message; }
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
                SqlCommand cmd = new SqlCommand("select descripcion from catproductos where descripcion like '%" + prefixText + "%'", conexionBD);
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

        protected void grdDetProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            pvAccom.Clases.EntradaProd EntProd = new pvAccom.Clases.EntradaProd();
            List<pvAccom.Clases.EntradaProd> lstEntProd = new List<pvAccom.Clases.EntradaProd>();
            if (Session["lstEntProd"] != null)
                lstEntProd = (List<pvAccom.Clases.EntradaProd>)Session["lstEntProd"];
            grdDetProductos.PageIndex = e.NewPageIndex;
            grdDetProductos.DataSource = lstEntProd;
            grdDetProductos.DataBind();
            Session["lstEntProd"] = lstEntProd;
        }

        protected void txtCosto_TextChanged(object sender, EventArgs e)
        {
            if (txtCant.Text != "")
                txtImporte.Text = (Convert.ToDecimal(txtCant.Text) * Convert.ToDecimal(txtCosto.Text)).ToString("C2");
            else if (txtCant.Text == "")
                txtCant.Focus();
            else
                btnAgrProd.Focus();
        }

        protected void txtCant_TextChanged(object sender, EventArgs e)
        {
            if (txtCosto.Text != "")
            {
                if (Convert.ToDecimal(txtCant.Text) != 0)
                    txtImporte.Text = (Convert.ToDecimal(txtCant.Text) * Convert.ToDecimal(txtCosto.Text)).ToString("C2");
                else
                    txtImporte.Text = "0.00";
            }
            txtCosto.Focus();
        }

        protected void txtFechaDoc_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaIngresada = Convert.ToDateTime(txtFechaDoc.Text);
                if (fechaIngresada > fechas.obtieneFechaLocal())
                {
                    txtFechaDoc.Text = "";
                    lblErrorDoc.Text = "La fecha ingresada no puede ser mayor a la fecha actual.";
                }
                else
                    lblErrorDoc.Text = "";
            }
            catch (Exception)
            {
                txtFechaDoc.Text = "";
                lblErrorDoc.Text = "Formato de fecha incorrecto (AAAA-MM-DD).";
            }
        }
        
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            pnlMask.Visible = false;
            pnlPopupNvoDoc.Visible = false;
        }
       

        

        protected void GridOrdenes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdDetalleOrden.DataBind();
        }

        protected void btnOrdenes_Click(object sender, EventArgs e)
        {
            lblErrorDoc.Text = "";
            if (RadGrid1.SelectedValues["id_punto"].ToString() != "-1")
            {
                GridOrdenes.DataBind();
                pnlPopupNvoDoc.Visible = false;
                pnlOrdenesCompra.Visible = true;
            }
            else
                lblErrorDoc.Text = "Para poder seleccionar una orden debe indicar primero la Tienda";
        }

        protected void btnAceptarOrden_Click(object sender, EventArgs e)
        {
            try
            {
                int ordenCompra = Convert.ToInt32(GridOrdenes.DataKeys[0].Value.ToString());
                pvAccom.Clases.EntradaProd EntProd = new pvAccom.Clases.EntradaProd();
                List<pvAccom.Clases.EntradaProd> lstEntProd = new List<pvAccom.Clases.EntradaProd>();
                if (Session["lstEntProd"] != null)
                    lstEntProd = (List<pvAccom.Clases.EntradaProd>)Session["lstEntProd"];
                OrdenCompra ordenPrevia = new OrdenCompra();
                DataSet infoOrden = ordenPrevia.obtieneInfoDetalle(ordenCompra, Convert.ToInt32(RadGrid1.SelectedValues["id_punto"]));
                OrdenCompraDetalle orden = new OrdenCompraDetalle();

                int ultimoValor = 0;
                foreach (pvAccom.Clases.EntradaProd ent in lstEntProd) {
                    ultimoValor = ent.entID;
                }

                short intAlmacen = short.Parse(RadGrid1.SelectedValues["id_punto"].ToString());

                foreach (DataRow fila in infoOrden.Tables[0].Rows) {
                    EntProd = new pvAccom.Clases.EntradaProd();
                    EntProd.entID = ultimoValor + 1;
                    EntProd.entProducto = fila[0].ToString();
                    EntProd.entProdAlm = intAlmacen;
                    string[] producto = fila[1].ToString().Trim().Split(new char[] { ';' });
                    EntProd.entProdDesc = producto[0].Trim();//txtProducto.Text;
                    EntProd.entCant = float.Parse(fila[2].ToString());
                    EntProd.entCosto = float.Parse(fila[3].ToString(), System.Globalization.NumberStyles.Currency);
                    EntProd.entImporte = float.Parse(fila[4].ToString(), System.Globalization.NumberStyles.Currency);
                    lstEntProd.Add(EntProd);
                    ultimoValor++;
                }

                grdDetProductos.DataSource = lstEntProd;
                grdDetProductos.DataBind();
                Session["lstEntProd"] = lstEntProd;
                ActualizaSumas(lstEntProd);
                pnlPopupNvoDoc.Visible = true;
                pnlOrdenesCompra.Visible = false;
            }
            catch (Exception ex) { }
        }

        protected void btnCancelarOrden_Click(object sender, EventArgs e)
        {            
            pnlPopupNvoDoc.Visible = true;
            pnlOrdenesCompra.Visible = false;
        }

        protected void RadAutoCompleteBox1_TextChanged(object sender, AutoCompleteTextEventArgs e)
        {
            if (RadAutoCompleteBox.Text != "")
            {
                try
                {
                    string[] argumentos = RadAutoCompleteBox.Text.ToString().Split(new char[] { '/' });
                    string idProducto = argumentos[0];
                    Producto prd = new Producto();
                    prd.ClaveProducto = idProducto;
                    prd.Isla = Convert.ToInt32(RadGrid1.SelectedValues["id_punto"].ToString());
                    object[] valor = prd.obtieneUltimoCosto();
                    if (Convert.ToBoolean(valor[0]))
                    {
                        prd.obtienePrecioVtaActivo();
                        decimal costo = Convert.ToDecimal(valor[1]);
                        txtCosto.Text = costo.ToString("F2");
                        txtPrVta.Text = prd.MontoVenta.ToString("F2");
                    }
                    else
                        txtCosto.Text = txtPrVta.Text = "";
                }
                catch (Exception ex)
                {
                    txtCosto.Text = txtPrVta.Text = "";
                }
                finally {
                    txtCant.Focus();
                }
            }
        }

        protected void lnkImprime_Click(object sender, EventArgs e)
        {
            ImprimirEntradaAlmacen imprimir = new ImprimirEntradaAlmacen();
            string folio = lnkImprime.CommandArgument;
            string Archivo = imprimir.imprimeEntrada(folio);
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
                lblError.Text = "No se logro generar la impresion de la entrada seleccionada";
        }

        protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAgrEnt.Visible = true;
        }

        protected void lnkEntrada_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            try
            {
                grdDetProductos.Columns[7].Visible = true;
                btnOrdenes.Visible = btnGuardaEnt.Visible = RadAutoCompleteBox.Visible = txtCant.Visible = txtCosto.Visible = txtPrVta.Visible = txtImporte.Visible = lnkFFin.Visible = btnAgrProd.Visible = true;
                ddlProve.Enabled = txtDocu.Enabled = ddlTipoDoc.Enabled = true;

                btnOrdenes.Visible = false;
                LinkButton btn = (LinkButton)sender;
                string id = btn.CommandArgument.ToString();
                lblEntrada.Text = id;
                pnlPopupNvoDoc.GroupingText = "Modifica Documento #" + id;
                Entradas entradas = new Entradas();
                entradas.entrada = Convert.ToInt32(id);
                entradas.tienda = Convert.ToInt32(RadGrid1.SelectedValues["id_punto"].ToString());
                entradas.obtieneEncabezadoEntrada();
                if (Convert.ToBoolean(entradas.retorno[0]))
                {
                    DataSet infoEnc = (DataSet)entradas.retorno[1];
                    ddlProve.Items.Clear();
                    ddlProve.DataBind();
                    foreach (DataRow info in infoEnc.Tables[0].Rows)
                    {
                        ddlProve.SelectedValue = Convert.ToString(info[5]);
                        txtDocu.Text = Convert.ToString(info[2]);
                        ddlTipoDoc.SelectedValue = Convert.ToString(info[3]);
                        txtFechaDoc.Text = Convert.ToDateTime(info[4]).ToString("yyyy-MM-dd");
                    }
                    entradas.cargaDetalle();
                    pvAccom.Clases.EntradaProd EntProd = new pvAccom.Clases.EntradaProd();
                    List<pvAccom.Clases.EntradaProd> lstEntProd = new List<pvAccom.Clases.EntradaProd>();
                    int ultimoValor = 0;
                    Session["lstEntProd"] = null;
                    if (Convert.ToBoolean(entradas.retorno[0]))
                    {
                        DataSet infoOrden = (DataSet)entradas.retorno[1];
                        foreach (DataRow fila in infoOrden.Tables[0].Rows)
                        {
                            EntProd = new pvAccom.Clases.EntradaProd();
                            EntProd.entID = ultimoValor + 1;
                            EntProd.entProducto = fila[0].ToString();
                            EntProd.entProdAlm = (short)entradas.tienda;
                            EntProd.entProdDesc = fila[1].ToString();//txtProducto.Text;
                            EntProd.entCant = float.Parse(fila[2].ToString());
                            EntProd.entCosto = float.Parse(fila[3].ToString(), System.Globalization.NumberStyles.Currency);
                            EntProd.entPrecVtaUnit = float.Parse(fila[5].ToString(), System.Globalization.NumberStyles.Currency);
                            EntProd.entImporte = float.Parse(fila[4].ToString(), System.Globalization.NumberStyles.Currency);
                            lstEntProd.Add(EntProd);
                            ultimoValor++;
                        }
                    }
                    grdDetProductos.DataSource = lstEntProd;
                    grdDetProductos.DataBind();
                    Session["lstEntProd"] = lstEntProd;
                    ActualizaSumas(lstEntProd);
                    lblErrorPop.Text = "";
                    Session["contador"] = 1;
                    pnlMask.Visible = true;
                    pnlPopupNvoDoc.Visible = true;
                    txtCant.Text = "";
                    txtPrVta.Text = "";
                    txtCosto.Text = "";
                    txtImporte.Text = "";
                    RadAutoCompleteBox.Entries.Clear();
                    RadAutoCompleteBox.TextSettings.SelectionMode = (RadAutoCompleteSelectionMode)Enum.Parse(typeof(RadAutoCompleteSelectionMode), "Single", true);
                }
                else
                    lblError.Text = "Error al cargar la entrada. Detalle: " + Convert.ToString(entradas.retorno[1]);
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al cargar la entrada. Detalle: " + ex.Message;
            }
        }

        protected void lnkTerminar_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            try
            {
                LinkButton btn = (LinkButton)sender;
                string id = btn.CommandArgument.ToString();
                Entradas entradas = new Entradas();
                entradas.entrada = Convert.ToInt32(id);
                entradas.tienda = Convert.ToInt32(RadGrid1.SelectedValues["id_punto"].ToString());
                entradas.terminarEntrada();
                if (Convert.ToBoolean(entradas.retorno[0]))
                {
                    decimal monto = 0;
                    string factura = "";
                    int idProveedor = 0;
                    int proveedor = 0;
                    string rfc = "";
                    string nombrePorvee = "";

                    entradas.obtieneEncabezadoEntrada();
                    object[] infoEnt = entradas.retorno;
                    if (Convert.ToBoolean(infoEnt[0])) {
                        DataSet datos = (DataSet)infoEnt[1];
                        foreach (DataRow r in datos.Tables[0].Rows) {
                            monto = Convert.ToDecimal(r[8]);
                            factura = Convert.ToString(r[2]);
                            proveedor = Convert.ToInt32(r[5]);
                            break;
                        }
                    }
                    Proveedores proveerod = new Proveedores();
                    DataSet infoProv = proveerod.obtieneInfo(proveedor.ToString());
                    foreach (DataRow r in infoProv.Tables[0].Rows)
                    {
                        rfc = Convert.ToString(r[2]).ToUpper().Trim();
                        nombrePorvee = Convert.ToString(r[3]).ToUpper().Trim();
                    }

                    CatClientes clientes = new CatClientes();
                    
                    

                    lblError.Text = "Entrada Concluida Existosamente";
                    Facturas facturas = new Facturas();
                    facturas.folio = Convert.ToInt32(id);
                    facturas.tipoCuenta = "PA";
                    facturas.factura =factura;



                    object[] existe = clientes.existeCliente(rfc);
                    
                    if (Convert.ToBoolean(existe[0])) {
                        if (!Convert.ToBoolean(existe[1])) 
                            clientes.insertaClienteBasicos(rfc, nombrePorvee, "", "", "", true, "M", "1900-01-01", "P", "");
                        
                    }
                    
                   
                    object[] info = clientes.obtieneIdProveedor(rfc);
                    if (Convert.ToBoolean(info[0]))
                        idProveedor = Convert.ToInt32(info[1]);
                    else
                        idProveedor = 0;

                    if (idProveedor != 0)
                    {
                        
                        string politica = clientes.obtieneClavePolitica(Convert.ToDecimal(idProveedor));
                        facturas.id_cliprov = Convert.ToInt32(idProveedor);
                        facturas.politica = politica;
                        facturas.estatus = "PEN";
                        facturas.empresa = 1;
                        facturas.taller = Convert.ToInt16(RadGrid1.SelectedValues["id_punto"]);
                        facturas.tipoCargo = "C";                                                
                        facturas.Importe = monto;
                        facturas.montoPagar = monto;
                        facturas.orden = Convert.ToInt32(id);
                        facturas.razon_social = nombrePorvee.ToUpper().Trim();
                        facturas.monto = monto;
                        facturas.existeFactura();
                        if (Convert.ToBoolean(facturas.retorno[0]))
                        {
                            if (Convert.ToInt32(facturas.retorno[1]) == 0)
                                facturas.generaFactura();
                        }
                    }
                    RadGrid2.Rebind();
                }
                else
                    lblError.Text = "Error al terminar la entrada. Detalle: " + Convert.ToString(entradas.retorno[1]);
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al terminar la entrada. Detalle: " + ex.Message;
            }
        }
                
        protected void lnkEliminarEntrada_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            try
            {
                LinkButton btn = (LinkButton)sender;
                string id = btn.CommandArgument.ToString();
                Entradas entradas = new Entradas();
                entradas.entrada = Convert.ToInt32(id);
                entradas.tienda = Convert.ToInt32(RadGrid1.SelectedValues["id_punto"].ToString());
                entradas.eliminarEntrada();
                if (Convert.ToBoolean(entradas.retorno[0]))
                {
                    lblError.Text = "Entrada Eliminada Existosamente";
                    RadGrid2.Rebind();
                }
                else
                    lblError.Text = "Error al eliminar la entrada. Detalle: " + Convert.ToString(entradas.retorno[1]);
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al eliminar la entrada. Detalle: " + ex.Message;
            }
        }

        protected void RadGrid2_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                bool terminado = false;
                try { terminado = Convert.ToBoolean(DataBinder.Eval(e.Item.DataItem, "terminado").ToString()); } catch (Exception) { terminado = false; }
                var botonTerminado = e.Item.FindControl("lnkTerminar") as LinkButton;
                var botonEDitar = e.Item.FindControl("lnkEntrada") as LinkButton;
                if (terminado)
                    botonEDitar.Visible = botonTerminado.Visible = false;
            }
        }

        protected void lnkSeleccionar_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            try
            {
                LinkButton btn = (LinkButton)sender;
                string id = btn.CommandArgument.ToString();
                lblEntrada.Text = id;
                pnlPopupNvoDoc.GroupingText = "Documento #" + id;
                Entradas entradas = new Entradas();
                entradas.entrada = Convert.ToInt32(id);
                entradas.tienda = Convert.ToInt32(RadGrid1.SelectedValues["id_punto"].ToString());
                entradas.obtieneEncabezadoEntrada();
                if (Convert.ToBoolean(entradas.retorno[0]))
                {
                    DataSet infoEnc = (DataSet)entradas.retorno[1];
                    ddlProve.Items.Clear();
                    ddlProve.DataBind();
                    foreach (DataRow info in infoEnc.Tables[0].Rows)
                    {
                        ddlProve.SelectedValue = Convert.ToString(info[5]);
                        txtDocu.Text = Convert.ToString(info[2]);
                        ddlTipoDoc.SelectedValue = Convert.ToString(info[3]);
                        txtFechaDoc.Text = Convert.ToDateTime(info[4]).ToString("yyyy-MM-dd");
                    }
                    entradas.cargaDetalle();
                    pvAccom.Clases.EntradaProd EntProd = new pvAccom.Clases.EntradaProd();
                    List<pvAccom.Clases.EntradaProd> lstEntProd = new List<pvAccom.Clases.EntradaProd>();
                    int ultimoValor = 0;
                    Session["lstEntProd"] = null;
                    if (Convert.ToBoolean(entradas.retorno[0]))
                    {
                        DataSet infoOrden = (DataSet)entradas.retorno[1];
                        foreach (DataRow fila in infoOrden.Tables[0].Rows)
                        {
                            EntProd = new pvAccom.Clases.EntradaProd();
                            EntProd.entID = ultimoValor + 1;
                            EntProd.entProducto = fila[0].ToString();
                            EntProd.entProdAlm = (short)entradas.tienda;
                            EntProd.entProdDesc = fila[1].ToString();//txtProducto.Text;
                            EntProd.entCant = float.Parse(fila[2].ToString());
                            EntProd.entCosto = float.Parse(fila[3].ToString(), System.Globalization.NumberStyles.Currency);
                            EntProd.entPrecVtaUnit = float.Parse(fila[5].ToString(), System.Globalization.NumberStyles.Currency);
                            EntProd.entImporte = float.Parse(fila[4].ToString(), System.Globalization.NumberStyles.Currency);
                            lstEntProd.Add(EntProd);
                            ultimoValor++;
                        }
                    }
                    grdDetProductos.DataSource = lstEntProd;
                    grdDetProductos.DataBind();
                    Session["lstEntProd"] = lstEntProd;
                    ActualizaSumas(lstEntProd);
                    lblErrorPop.Text = "";
                    Session["contador"] = 1;
                    pnlMask.Visible = true;
                    pnlPopupNvoDoc.Visible = true;
                    txtCant.Text = "";
                    txtPrVta.Text = "";
                    txtCosto.Text = "";
                    txtImporte.Text = "";
                    RadAutoCompleteBox.Entries.Clear();
                    RadAutoCompleteBox.TextSettings.SelectionMode = (RadAutoCompleteSelectionMode)Enum.Parse(typeof(RadAutoCompleteSelectionMode), "Single", true);
                    grdDetProductos.Columns[7].Visible = false;
                    btnOrdenes.Visible = btnGuardaEnt.Visible = RadAutoCompleteBox.Visible = txtCant.Visible = txtCosto.Visible = txtPrVta.Visible = txtImporte.Visible = lnkFFin.Visible = btnAgrProd.Visible = false;
                    ddlProve.Enabled = txtDocu.Enabled = ddlTipoDoc.Enabled = false;
                }
                else
                    lblError.Text = "Error al cargar la entrada. Detalle: " + Convert.ToString(entradas.retorno[1]);
            }
            catch (Exception ex)
            {
                lblError.Text = "Error al cargar la entrada. Detalle: " + ex.Message;
            }
        }
    }

    

}