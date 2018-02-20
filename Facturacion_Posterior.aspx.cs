using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;
using Telerik.Web.UI;
using System.Data;

public partial class Facturacion_Posterior : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    FacturacionDatos facDatos = new FacturacionDatos();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int isla = Convert.ToInt32(Request.QueryString["p"]);
            if (isla == 0)
                Response.Redirect("Default.aspx");
        }
    }

    protected void btnBuscarFolio_Click(object sender, EventArgs e)
    {
        lblErroresFolio.Text = "";
        lblErrorFacCliente.Text = "";
        txtRFC.Text = "";
        limpiaCampos();
        habilitaCampos();
        txtRFC.Visible = false;
        btnActualizar.Visible = false;
        btnCancelarCliente.Visible = false;
        PanelDetalleVenta.Visible = false;
        if (txtFolio.Text.Trim() != "")
        {
            string ticket = txtFolio.Text;
            int isla = Convert.ToInt32(Request.QueryString["p"]);
            bool existeTicket = facDatos.existeTicket(ticket, isla);
            if (existeTicket)
            {
                bool fechaValida = true;// facDatos.validaFechaTicket(ticket, isla);
                if (fechaValida)
                {
                    string sql = "select CONVERT(varchar(12), fecha_venta, 126)+';' + CONVERT(varchar(8), hora_venta, 108) + ';' + cast(subtotal as varchar) + ';' + cast(iva as varchar) + ';' + cast(total as varchar) + ';' + usuario+';'+cast(porc_iva as varchar)+';'+cast(porc_descuento as varchar)+';'+cast(descuento as varchar) as encabezado from venta_enc where ticket = " + ticket.ToString() + " and id_punto = " + isla.ToString();
                    string datosEncabezado = facDatos.datosEncabezadoVenta(sql);
                    if (datosEncabezado != "")
                    {
                        string[] encabezadoVenta = datosEncabezado.Split(';');
                        lblFecha.Text = encabezadoVenta[0];
                        lblHora.Text = encabezadoVenta[1];
                        lblNeto.Text = "Neto: " + encabezadoVenta[2];
                        lblDescuentoPorc.Text = "Descuento: " + encabezadoVenta[7] + "% ";
                        lblDesceunto.Text = encabezadoVenta[8];
                        decimal subtotal = 0;
                        try { subtotal = Convert.ToDecimal(encabezadoVenta[2]) - Convert.ToDecimal(encabezadoVenta[8]); } catch (Exception) { subtotal = 0; }
                        lblSubTotal.Text = "Subtotal: " + subtotal.ToString("F2");
                        lblIVA.Text = "IVA: " + encabezadoVenta[6] + "% " + encabezadoVenta[3];
                        decimal total = 0;
                        try { total = subtotal + Convert.ToDecimal(encabezadoVenta[3]); } catch (Exception) { total = subtotal; }
                        lblTotal.Text = "Total: " + total;
                        lblUsuario.Text = "Atendio por: " + encabezadoVenta[5];
                        txtRFC.Visible = true;
                        btnActualizar.Visible = true;
                        btnCancelarCliente.Visible = true;
                        if (rbtnPersona.SelectedValue == "M" && txtRfcCap.Text.Trim().Length == 12)
                            rbtnPersona.SelectedValue = "M";
                        else if (rbtnPersona.SelectedValue == "F" && txtRfcCap.Text.Trim().Length == 13)
                            rbtnPersona.SelectedValue = "F";
                    }
                    else
                        lblErroresFolio.Text = "La fecha del ticket es anterior a la de hoy.";
                }
                else
                {
                    limpiaCampos();
                    lblErroresFolio.Text = "Hubo un problema al cargar los datos, verifique su información e intentelo nuevamente.";
                }
            }
            else
                lblErroresFolio.Text = "El ticket no existe, verifique sus datos y/o la Tienda sean correctos.";
        }
        else
            lblErroresFolio.Text = "Necesita colocar un numero de ticket.";
    }
        

    protected void btnNuevoCliente_Click(object sender, EventArgs e)
    {
        if (rbtnPersona.SelectedValue == "M" && txtRfcCap.Text.Trim().Length == 12 || rbtnPersona.SelectedValue == "F" && txtRfcCap.Text.Trim().Length == 13)
        {
            string tipoPersona = rbtnPersona.SelectedValue;
            lblErrorFacCliente.Text = "";
            Receptores Receptor = new Receptores();
            Receptor.existeReceptor(txtRfcCap.Text.ToUpper());
            object[] existeReceptor = Receptor.info;
            if (Convert.ToBoolean(existeReceptor[0]))
            {
                int existe = Convert.ToInt32(existeReceptor[1]);
                Receptor.agregarActualizarReceptor(txtRfcCap.Text, txtRazonNew.Text, txtCalle.Text, txtNoExt.Text, txtNoIntMod.Text, txtLocalidad.Text, txtReferenciaMod.Text, txtCorreo.Text, ddlPais.SelectedValue, ddlEstado.SelectedValue, ddlMunicipio.SelectedValue, ddlColonia.SelectedValue, ddlCodigo.SelectedValue, txtCorreoCC.Text, txtCorreoCCO.Text);
                if (Convert.ToBoolean(Receptor.info[0]))
                    btnFacturar.Visible = true;
                else
                    lblErrorActuraliza.Text = "Error al actualizar los datos. " + Receptor.info[1].ToString();
            }
            else
                lblErrorActuraliza.Text = "Error al actualizar los datos del cliente. " + existeReceptor[1].ToString();
        }
        else
            lblErrorFacCliente.Text = "El formato del R.F.C. No coincide con el tipo de persona seleccionado, verifique su información.";
    }

    protected void btnCancelarCliente_Click(object sender, EventArgs e)
    {
        lblErroresFolio.Text = "";
        lblErrorFacCliente.Text = "";
        limpiaCampos();
        PanelDetalleVenta.Visible = false;
        txtRFC.Visible = false;
        btnCancelarCliente.Visible = false;
        btnActualizar.Visible = false;
    }

    protected void txtRFC_TextChanged(object sender, EventArgs e)
    {
        lblErroresFolio.Text = "";
        lblErrorFacCliente.Text = "";
        string rfc = txtRfcCap.Text;
        if (rfc.Length > 11 && rfc.Length < 14)
        {
            PanelDetalleVenta.Visible = true;
            cargaInfoReceptor();
        }
        else
            lblErrorFacCliente.Text = "El R.F.C. Tiene un formato incorrecto.";
    }

    private void cargaInfoReceptor()
    {
        try
        {
            Receptores Receptor = new Receptores();
            Receptor.existeReceptor(txtRfcCap.Text.Trim().ToUpper());
            if (Convert.ToBoolean(Receptor.info[0]))
            {
                if (Convert.ToBoolean(Receptor.info[1]))
                {
                    Receptor.obtieneIdReceptor(txtRfcCap.Text.Trim().ToUpper());
                    if (Convert.ToBoolean(Receptor.info[0]))
                    {
                        Receptor.idReceptor = Convert.ToInt32(Receptor.info[1]);
                        Receptor.obtieneInfoReceptor();
                        object[] info = Receptor.info;
                        if (Convert.ToBoolean(info[0]))
                        {
                            DataSet valores = (DataSet)info[1];
                            if (valores.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow fila in valores.Tables[0].Rows)
                                {
                                    for (int i = 0; i < fila.ItemArray.Length; i++)
                                    {
                                        if (fila[i].ToString() == "")
                                            fila[i] = "...";
                                    }

                                    if (txtRfcCap.Text.Length == 12)
                                        rbtnPersona.SelectedValue = "M";
                                    else
                                        rbtnPersona.SelectedValue = "F";

                                    if (rbtnPersona.SelectedValue == "M")
                                        txtRfcCap.MaxLength = 12;
                                    else
                                        txtRfcCap.MaxLength = 13;

                                    txtRazonNew.Text = fila[2].ToString();
                                    try { ddlPais.SelectedValue = fila[6].ToString(); }
                                    catch (Exception)
                                    {
                                        ddlPais.Items.Add(new RadComboBoxItem(fila[6].ToString(), fila[6].ToString()));
                                        ddlPais.SelectedValue = fila[6].ToString();
                                    }
                                    try
                                    {
                                        ddlEstado.SelectedValue = fila[8].ToString();
                                    }
                                    catch (Exception)
                                    {
                                        ddlEstado.Items.Add(new RadComboBoxItem(fila[8].ToString(), fila[8].ToString()));
                                        ddlEstado.SelectedValue = fila[8].ToString();
                                    }
                                    try
                                    {
                                        ddlMunicipio.SelectedValue = fila[10].ToString();
                                    }
                                    catch (Exception)
                                    {
                                        ddlMunicipio.Items.Add(new RadComboBoxItem(fila[10].ToString(), fila[10].ToString()));
                                        ddlMunicipio.SelectedValue = fila[10].ToString();
                                    }
                                    try { ddlColonia.SelectedValue = fila[12].ToString(); }
                                    catch (Exception)
                                    {
                                        ddlColonia.Items.Add(new RadComboBoxItem(fila[12].ToString(), fila[12].ToString()));
                                        ddlColonia.SelectedValue = fila[12].ToString();
                                    }
                                    try { ddlCodigo.SelectedValue = fila[14].ToString(); }
                                    catch (Exception)
                                    {
                                        ddlCodigo.Items.Add(new RadComboBoxItem(fila[14].ToString(), fila[14].ToString()));
                                        ddlCodigo.SelectedValue = fila[14].ToString();
                                    }
                                    txtCalle.Text = fila[3].ToString();
                                    txtNoExt.Text = fila[4].ToString();
                                    txtNoIntMod.Text = fila[5].ToString();
                                    txtLocalidad.Text = fila[15].ToString();
                                    txtReferenciaMod.Text = fila[16].ToString();
                                    txtCorreo.Text = fila[17].ToString();
                                    txtCorreoCC.Text = fila[18].ToString();
                                    txtCorreoCCO.Text = fila[19].ToString();
                                    btnActualizaCliente.Visible = true;
                                    btnNuevoCliente.Visible = false;
                                }
                            }
                            else vaciaDatos();
                        }
                        else vaciaDatos();
                    }
                    else vaciaDatos();
                }
                else vaciaDatos();
            }
            else vaciaDatos();
        }
        catch (Exception ex)
        {
            lblErrorFacCliente.Text = "Error: " + ex.Message;
        }
    }

    private void vaciaDatos()
    {
        ddlPais.Text = ddlEstado.Text = ddlMunicipio.Text = ddlColonia.Text = ddlCodigo.Text = txtRazonNew.Text = txtLocalidad.Text = txtReferenciaMod.Text = txtCalle.Text = txtNoExt.Text = txtNoIntMod.Text = txtCorreo.Text = txtCorreoCC.Text = txtCorreoCCO.Text = "";
        ddlPais.SelectedIndex = ddlEstado.SelectedIndex = ddlMunicipio.SelectedIndex = ddlColonia.SelectedIndex = ddlCodigo.SelectedIndex = -1;
        btnActualizaCliente.Visible = false;
        btnNuevoCliente.Visible = true;
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

    protected void btnActualizaCliente_Click(object sender, EventArgs e)
    {
        string tipoPersona = rbtnPersona.SelectedValue;
        lblErrorFacCliente.Text = "";
        Receptores Receptor = new Receptores();
        Receptor.existeReceptor(txtRfcCap.Text.ToUpper());
        object[] existeReceptor = Receptor.info;
        if (Convert.ToBoolean(existeReceptor[0]))
        {
            int existe = Convert.ToInt32(existeReceptor[1]);
            Receptor.agregarActualizarReceptor(txtRfcCap.Text, txtRazonNew.Text, txtCalle.Text, txtNoExt.Text, txtNoIntMod.Text, txtLocalidad.Text, txtReferenciaMod.Text, txtCorreo.Text, ddlPais.SelectedValue, ddlEstado.SelectedValue, ddlMunicipio.SelectedValue, ddlColonia.SelectedValue, ddlCodigo.SelectedValue, txtCorreoCC.Text, txtCorreoCCO.Text);
            if (Convert.ToBoolean(Receptor.info[0]))
                btnFacturar.Visible = true;
            else
                lblErrorActuraliza.Text = "Error al actualizar los datos. " + Receptor.info[1].ToString();
        }
        else
            lblErrorActuraliza.Text = "Error al actualizar los datos del cliente. " + existeReceptor[1].ToString();
    }

    private void limpiaCampos()
    {
        lblErroresFolio.Text = "";
        lblErrorFacCliente.Text = "";

        ddlPais.Text = "";
        ddlPais.SelectedIndex = -1;
        ddlEstado.Text = "";
        ddlEstado.SelectedIndex = -1;
        ddlMunicipio.Text = "";
        ddlMunicipio.SelectedIndex = -1;
        ddlColonia.Text = "";
        ddlColonia.SelectedIndex = -1;
        ddlCodigo.Text = "";
        ddlCodigo.SelectedIndex = -1;
        txtRazonNew.Text = "";
        txtLocalidad.Text = "";
        txtReferenciaMod.Text = "";
        txtRfcCap.Text = "";        
        txtCalle.Text = "";
        txtNoExt.Text = "";
        txtNoIntMod.Text = "";
        txtCorreo.Text = "";
        txtCorreoCC.Text = "";
        txtCorreoCCO.Text = "";
        lblUsuario.Text = lblTotal.Text = lblSubTotal.Text = lblIVA.Text = lblHora.Text = lblFecha.Text = lblNeto.Text = lblDesceunto.Text = lblDescuentoPorc.Text = "";
    }

    private void habilitaCampos()
    {
        txtRfcCap.Enabled = txtRazonNew.Enabled = txtCalle.Enabled = txtNoExt.Enabled = txtNoIntMod.Enabled = txtLocalidad.Enabled = txtReferenciaMod.Enabled = txtCorreo.Enabled = txtCorreoCC.Enabled = txtCorreoCCO.Enabled = ddlPais.Enabled = ddlEstado.Enabled = ddlMunicipio.Enabled = ddlColonia.Enabled = ddlCodigo.Enabled = rbtnPersona.Enabled = true;        
    }

    private void deshabilitaCampos()
    {
        txtRfcCap.Enabled = txtRazonNew.Enabled = txtCalle.Enabled = txtNoExt.Enabled = txtNoIntMod.Enabled = txtLocalidad.Enabled = txtReferenciaMod.Enabled = txtCorreo.Enabled = txtCorreoCC.Enabled = txtCorreoCCO.Enabled = ddlPais.Enabled = ddlEstado.Enabled = ddlMunicipio.Enabled = ddlColonia.Enabled = ddlCodigo.Enabled = rbtnPersona.Enabled = false;
    }


    protected void btnFacturar_Click(object sender, EventArgs e)
    {
        ClientesDatos datosCli = new ClientesDatos();
        string isla = Request.QueryString["p"];
        string ticket = txtFolio.Text;
        string rfc = txtRfcCap.Text;
        Receptores receptor = new Receptores();
        receptor.obtieneIdReceptor(rfc);
        int idCliente = 0;
        if (Convert.ToBoolean(receptor.info[0])) 
            idCliente = Convert.ToInt32(receptor.info[1]);
                
        if (idCliente != 0)
        {
            bool actualizaVentEncab = datosCli.actualizaVentaEncabezado(idCliente, isla, ticket, 1, fechas.obtieneFechaLocal().ToString("yyyy-MM-dd"), fechas.obtieneFechaLocal().ToString("HH:mm:ss"), ticket);
            deshabilitaCampos();
            lblErrorFacCliente.Text = "Su factura será enviada al correo electrónico proporcionado.";
        }
        else
            lblErrorFacCliente.Text = "El cliente no a sido dado de alta o actualizado.";
    }

    protected void rbtnPersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        string persona = rbtnPersona.SelectedValue.ToString();
        if (persona == "M")
            txtRfcCap.MaxLength = 12;
        else if (persona == "F")
            txtRfcCap.MaxLength = 13;
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

    protected void btnActualizar_Click(object sender, EventArgs e)
    {
        lblErroresFolio.Text = "";
        lblErrorFacCliente.Text = "";
        if (txtRFC.Text != "")
        {
            string rfc = txtRFC.Text;
            if (rfc.Length > 11 && rfc.Length < 14)
            {
                txtRfcCap.Text = rfc.ToUpper().Trim();
                txtRfcCap.Enabled = false;
                PanelDetalleVenta.Visible = true;
                cargaInfoReceptor();
                ClientesDatos datosCli = new ClientesDatos();
                string isla = Request.QueryString["p"];
                string ticket = txtFolio.Text;
                bool previamenteSolicitado = datosCli.ticketSolicitado(isla, ticket);
                if (previamenteSolicitado) {
                    lblErrorFacCliente.Text = "Este ticket ya ha sido enviado para su facturación";
                    deshabilitaCampos();
                    btnActualizaCliente.Visible = btnNuevoCliente.Visible = btnFacturar.Visible = false;
                }
            }
            else
                lblErrorFacCliente.Text = "El R.F.C. Tiene un formato incorrecto.";
        }
        else
            lblErrorFacCliente.Text = "Debe indicar el RFC del cliente";
    }

    protected void btnMultiFacturacion_Click(object sender, EventArgs e)
    {
        Response.Redirect("MultiFacturacion.aspx?u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"] + "&p=" + Request.QueryString["p"] + "&np=" + Request.QueryString["np"] + "&c=" + Request.QueryString["c"]);
    }
}