using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using E_Utilities;

public partial class MultiFacturacion : System.Web.UI.Page
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


    protected void btnCargaDatos_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (txtRFC.Text != "")
        {
            string rfc = txtRFC.Text;
            if (rfc.Length > 11 && rfc.Length < 14)
            {
                txtRfcCap.Text = rfc.ToUpper().Trim();
                txtRfcCap.Enabled = false;
                
                cargaInfoReceptor();
                facDatos.borrar(Request.QueryString["u"]);
            }
            else
                lblError.Text = "El R.F.C. Tiene un formato incorrecto.";
        }
        else
            lblError.Text = "Debe indicar el RFC del cliente";
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

    private void limpiaCampos()
    {
        lblError.Text = "";
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
        facDatos.borrar(Request.QueryString["u"]);
        RadGrid1.Rebind();
        RadGrid2.Rebind();
        // lblUsuario.Text = lblTotal.Text = lblSubTotal.Text = lblIVA.Text = lblHora.Text = lblFecha.Text = lblNeto.Text = lblDesceunto.Text = lblDescuentoPorc.Text = "";
    }

    private void habilitaCampos()
    {
        txtRfcCap.Enabled = txtRazonNew.Enabled = txtCalle.Enabled = txtNoExt.Enabled = txtNoIntMod.Enabled = txtLocalidad.Enabled = txtReferenciaMod.Enabled = txtCorreo.Enabled = txtCorreoCC.Enabled = txtCorreoCCO.Enabled = ddlPais.Enabled = ddlEstado.Enabled = ddlMunicipio.Enabled = ddlColonia.Enabled = ddlCodigo.Enabled = rbtnPersona.Enabled = true;
    }

    private void deshabilitaCampos()
    {
        txtRfcCap.Enabled = txtRazonNew.Enabled = txtCalle.Enabled = txtNoExt.Enabled = txtNoIntMod.Enabled = txtLocalidad.Enabled = txtReferenciaMod.Enabled = txtCorreo.Enabled = txtCorreoCC.Enabled = txtCorreoCCO.Enabled = ddlPais.Enabled = ddlEstado.Enabled = ddlMunicipio.Enabled = ddlColonia.Enabled = ddlCodigo.Enabled = rbtnPersona.Enabled = false;
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
            lblError.Text = "Error: " + ex.Message;
        }
    }
    private void vaciaDatos()
    {
        ddlPais.Text = ddlEstado.Text = ddlMunicipio.Text = ddlColonia.Text = ddlCodigo.Text = txtRazonNew.Text = txtLocalidad.Text = txtReferenciaMod.Text = txtCalle.Text = txtNoExt.Text = txtNoIntMod.Text = txtCorreo.Text = txtCorreoCC.Text = txtCorreoCCO.Text = "";
        ddlPais.SelectedIndex = ddlEstado.SelectedIndex = ddlMunicipio.SelectedIndex = ddlColonia.SelectedIndex = ddlCodigo.SelectedIndex = -1;
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

    protected void btnCancelarCliente_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        limpiaCampos();        
    }

    protected void txtRFC_TextChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        string rfc = txtRfcCap.Text;
        if (rfc.Length > 11 && rfc.Length < 14)
        {            
            cargaInfoReceptor();
            facDatos.borrar(Request.QueryString["u"]);
        }
        else
            lblError.Text = "El R.F.C. Tiene un formato incorrecto.";
    }

    protected void btnActualizaCliente_Click(object sender, EventArgs e)
    {
        string tipoPersona = rbtnPersona.SelectedValue;
        lblError.Text = "";
        Receptores Receptor = new Receptores();
        Receptor.existeReceptor(txtRfcCap.Text.ToUpper());
        object[] existeReceptor = Receptor.info;
        if (Convert.ToBoolean(existeReceptor[0]))
        {
            int existe = Convert.ToInt32(existeReceptor[1]);
            Receptor.agregarActualizarReceptor(txtRfcCap.Text, txtRazonNew.Text, txtCalle.Text, txtNoExt.Text, txtNoIntMod.Text, txtLocalidad.Text, txtReferenciaMod.Text, txtCorreo.Text, ddlPais.SelectedValue, ddlEstado.SelectedValue, ddlMunicipio.SelectedValue, ddlColonia.SelectedValue, ddlCodigo.SelectedValue, txtCorreoCC.Text, txtCorreoCCO.Text);
            if (!Convert.ToBoolean(Receptor.info[0]))
                lblError.Text = "Error al actualizar los datos. " + Receptor.info[1].ToString();
        }
        else
            lblError.Text = "Error al actualizar los datos del cliente. " + existeReceptor[1].ToString();
    }

    protected void btnBuscarFolio_Click(object sender, EventArgs e)
    {
        if (txtFolio.Text.Trim() != "")
        {
            string ticket = txtFolio.Text;
            int isla = Convert.ToInt32(Request.QueryString["p"]);
            bool existeTicket = facDatos.existeTicket(ticket, isla);
            if (existeTicket)
            {
                object[] agregado = facDatos.agregaTicket(isla, ticket, Request.QueryString["u"]);
                txtFolio.Text = "";
                RadGrid1.Rebind();
                RadGrid2.Rebind();
            }
            else
                lblError.Text = "El ticket indicado no existe";
        }
    }

    protected void btnGuardarFacturar_Click(object sender, EventArgs e)
    {
        string tipoPersona = rbtnPersona.SelectedValue;
        string fecha = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
        string hora = fechas.obtieneFechaLocal().ToString("HH:mm:ss");
        lblError.Text = "";
        Receptores Receptor = new Receptores();
        Receptor.existeReceptor(txtRfcCap.Text.ToUpper());
        object[] existeReceptor = Receptor.info;
        if (Convert.ToBoolean(existeReceptor[0]))
        {
            int existe = Convert.ToInt32(existeReceptor[1]);
            Receptor.agregarActualizarReceptor(txtRfcCap.Text, txtRazonNew.Text, txtCalle.Text, txtNoExt.Text, txtNoIntMod.Text, txtLocalidad.Text, txtReferenciaMod.Text, txtCorreo.Text, ddlPais.SelectedValue, ddlEstado.SelectedValue, ddlMunicipio.SelectedValue, ddlColonia.SelectedValue, ddlCodigo.SelectedValue, txtCorreoCC.Text, txtCorreoCCO.Text);            
        }

        ClientesDatos datosCli = new ClientesDatos();
        string isla = Request.QueryString["p"];
        string ticket = txtFolio.Text;
        string rfc = txtRfcCap.Text;
        Receptores receptor = new Receptores();
        receptor.obtieneIdReceptor(rfc);
        int idCliente = 0;
        if (Convert.ToBoolean(receptor.info[0]))
            idCliente = Convert.ToInt32(receptor.info[1]);

        int desglosado = 0;
        if (chkDesglosado.Checked)
            desglosado = 1;

        if (idCliente != 0)
        {
            object[] info = facDatos.obtieneFacturar(Request.QueryString["u"]);
            if (Convert.ToBoolean(info[0])) {
                DataSet dat = (DataSet)info[1];
                string tickets = "";
                int i = 1;
                foreach (DataRow fila in dat.Tables[0].Rows)
                {
                    if (i == 1)
                        tickets = fila[2].ToString() + ";";
                    else
                        tickets = tickets + fila[2].ToString() + ";";
                    i++;
                }

                tickets = tickets.Substring(0, tickets.Length - 1);


                foreach (DataRow fila in dat.Tables[0].Rows)
                {
                    datosCli.actualizaVentaEncabezado(idCliente, isla, fila[2].ToString(), desglosado, fecha, hora, tickets);
                }
                lblError.Text = "Los tickets indicados han sido enviados para su facturación.";                
                limpiaCampos();
                RadGrid1.Rebind();
                RadGrid2.Rebind();
            }
        }
        else
            lblError.Text = "El cliente no a sido dado de alta o actualizado.";
    }
}