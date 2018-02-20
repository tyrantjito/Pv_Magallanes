using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CatProveedores : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        txtRFC.Focus();
        if (IsPostBack)
        {

        }
    }
    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView grdProvs = (GridView)sender;
        int index = -1;
        string provID = string.Empty;
        string strEstatusMsg = string.Empty;
        Proveedores provCat = new Proveedores();
        if (e.CommandName == "Elimina/Inactiva")
        {
            index = grdProvs.SelectedIndex;
            string[] valores = e.CommandArgument.ToString().Split(new char[] { ';' });
            provID = valores[0];
            string estatus = valores[1];
            
            provCat.Proveedor = provID;
            provCat.existeEntinventarioenc();
            bool existeEntinvent = provCat.Existe;
            if (existeEntinvent)
            {
                string result = provCat.cambiaStatus(provID, estatus);
                if (result == "A" || result == "B")
                    strEstatusMsg = string.Format("Proveedor actualizado a {0}.", result == "A" ? "Inactivo" : "Activo");
                else
                    strEstatusMsg = string.Format("Error al cambiar el Estatus del Proveedor {0}: {1}.", provID, result);
            }
            else
            {
                SqlDSProvs.DeleteCommand = "DELETE FROM [clienteproveedor] WHERE [clave]=" + provID;
                SqlDSProvs.Delete();
                strEstatusMsg = "Proveedor Eliminado.";
            }
            GridView1.DataBind();
            GridView1.SelectedIndex = -1;
            lblErrores.Text = strEstatusMsg;
        }
        if(e.CommandName == "EditarProv")
        {
            provID = e.CommandArgument.ToString();
            DataSet ds = provCat.selectProv(provID) ;
            foreach(DataRow ro in ds.Tables[0].Rows)
            {
                txtRazonSocial.Text =  ro["razonSocial"].ToString();
                txtRFC.Text = ro["RFC"].ToString();
                string persFisc = ro["personaFiscal"].ToString();
                if (persFisc == "M")
                {
                    lblRazon.Visible = txtRazonSocial.Visible = true;
                    lblnombre.Visible = lblAM.Visible = lblAP.Visible = false;
                    txtNombre.Visible = txtAP.Visible = txtAM.Visible = false;
                    rfvRazSoc.Enabled = true;
                    rfvNombre.Enabled = false;
                    rfvApPat.Enabled = false;
                    rbtPersona.SelectedIndex = 0;
                }
                else if (persFisc == "F")
                {
                    lblRazon.Visible = txtRazonSocial.Visible = false;
                    lblnombre.Visible = lblAM.Visible = lblAP.Visible = true;
                    txtNombre.Visible = txtAP.Visible = txtAM.Visible = true;
                    rfvRazSoc.Enabled = rfvRazSoc.Visible = false;
                    rfvNombre.Enabled = true;
                    rfvApPat.Enabled = true;
                    rbtPersona.SelectedIndex = 1;
                }
                txtNombre.Text = ro["nombres"].ToString();
                txtAP.Text = ro["apPat"].ToString();
                txtAM.Text = ro["apMat"].ToString();
                txtCalle.Text = ro["calle"].ToString();
                TxtNE.Text = ro["numExt"].ToString();
                txtNI.Text = ro["numInt"].ToString();
                txtCol.Text = ro["colonia"].ToString();
                txtCP.Text = ro["cp"].ToString();
                txtCiudad.Text = ro["ciudad"].ToString();
                txtEstado.Text = ro["estado"].ToString();
                txtPais.Text = ro["pais"].ToString();
                txtCorreo.Text = ro["email"].ToString();
                txtTel.Text = ro["telPart"].ToString();
                txtCel.Text = ro["telCel"].ToString();
                txtcontact.Text = ro["contacto"].ToString();
                btnAgrAct.Text = "Actualizar";
                btnAgrAct.CommandName = "Actualiza";
                btnAgrAct.CommandArgument = provID;
                btnCancelar.Visible = true;
            }
        }   
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridView grdProvs = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int filaEdit = GridView1.EditIndex;
            if (filaEdit >= 0 && int.Parse(GridView1.Rows[e.Row.RowIndex].ToString()) == filaEdit)
            {
                string strTipo = DataBinder.Eval(e.Row.DataItem, "personaFiscal").ToString();
                RadioButtonList rbtTipoPer = (RadioButtonList)e.Row.FindControl("rbtTipoPer");
                if (rbtTipoPer != null)
                {
                    if (strTipo == "F")
                        rbtTipoPer.SelectedIndex = 0;
                    else
                        rbtTipoPer.SelectedIndex = 1;
                }
            }
            else
            {
                string strEstatus = DataBinder.Eval(e.Row.DataItem, "estatus").ToString();
                string provID = DataBinder.Eval(e.Row.DataItem, "clave").ToString();
                string strRFC = DataBinder.Eval(e.Row.DataItem, "RFC").ToString();
                int intCampos = e.Row.Cells.Count;
                Button objBoton;
                Button btnInactva = (Button)e.Row.FindControl("btnEliminar");
                foreach(TableCell campo in e.Row.Cells)
                {
                    if(campo.Controls[0].GetType().Name == "DataControlButton")
                    {
                        objBoton = (Button)campo.Controls[0];
                        if (objBoton.CommandName == "Edit" && strEstatus == "B") 
                        {
                            objBoton.Enabled = false;
                            //DataControlFieldCell dcfc = (DataControlFieldCell)objBoton.Parent;
                            //dcfc.CssClass = "btn-default";
                            objBoton.CssClass = "btn-default";
                            objBoton.Style.Add("color", "#333");
                            objBoton.Style.Add("background-color", "#fff");
                            objBoton.Style.Add("border-color", "#ccc");
                        }
                    }
                }
                if (btnInactva != null)
                {
                    if (strEstatus == "A")
                    {
                        btnInactva.Text = "Inactiva";
                    }
                    else
                    {
                        btnInactva.Text = "Reactivar";
                        btnInactva.CssClass = "btn-success ancho50px";
                    }
                }
            }
        }
    }

    protected void rbtPersona_SelectedIndexChanged(object sender, EventArgs e)
    {
        string tipoPersona = rbtPersona.SelectedValue;
        if (tipoPersona == "M")
        {
            lblRazon.Visible = txtRazonSocial.Visible = true;
            lblnombre.Visible = lblAM.Visible = lblAP.Visible = false;
            txtNombre.Visible = txtAP.Visible = txtAM.Visible = false;
            rfvRazSoc.Enabled = true;
            rfvNombre.Enabled = false;
            rfvApPat.Enabled = false;
        }
        else if (tipoPersona == "F")
        {
            lblRazon.Visible = txtRazonSocial.Visible = false;
            lblnombre.Visible = lblAM.Visible = lblAP.Visible = true;
            txtNombre.Visible = txtAP.Visible = txtAM.Visible = true;
            rfvRazSoc.Enabled = rfvRazSoc.Visible = false;
            rfvNombre.Enabled = true;
            rfvApPat.Enabled = true;
        }
    }
    protected void rbtTipoPer_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rbtpersonas = (RadioButtonList)sender;
        string tipoPersona = rbtpersonas.SelectedValue;
        if (tipoPersona == "M")
        {
        }
        else if (tipoPersona == "F")
        {

        }
    }
    protected void btnAgrAct_Click(object sender, EventArgs e)
    {
        lblErrores.Text = "";
        Button btnAgAc = (Button)sender;
        if (btnAgAc.CommandName.Equals("Inserta"))
        {
            Proveedores provCat = new Proveedores();
            provCat.Proveedor = txtRFC.Text.ToUpper();
            provCat.verificaExiste();
            bool existeProv = provCat.Existe;
            if (!existeProv)
            {
                try
                {
                    if (rbtPersona.SelectedValue == "M")
                    {
                        SqlDSProvs.InsertParameters["razonSocial"].DefaultValue = txtRazonSocial.Text.Trim().ToUpper();
                        SqlDSProvs.InsertParameters["nombreComercial"].DefaultValue = txtRazonSocial.Text.Trim().ToUpper();
                    }
                    else
                    {
                        string strApMat = string.IsNullOrEmpty(txtAM.Text.Trim()) ? "" : " " + txtAM.Text.Trim();
                        SqlDSProvs.InsertParameters["razonSocial"].DefaultValue = txtNombre.Text.Trim().ToUpper() + " " + txtAP.Text.Trim() + strApMat;
                        SqlDSProvs.InsertParameters["nombreComercial"].DefaultValue = txtNombre.Text.Trim().ToUpper() + " " + txtAP.Text.Trim();
                    }
                    SqlDSProvs.InsertParameters["RFC"].DefaultValue = txtRFC.Text.ToUpper();
                    SqlDSProvs.InsertParameters["pais"].DefaultValue = txtPais.Text.Trim();
                    SqlDSProvs.InsertParameters["estado"].DefaultValue = txtEstado.Text.Trim();
                    SqlDSProvs.InsertParameters["ciudad"].DefaultValue = txtCiudad.Text.Trim();
                    SqlDSProvs.InsertParameters["calle"].DefaultValue = txtCalle.Text.Trim();
                    SqlDSProvs.InsertParameters["numExt"].DefaultValue = TxtNE.Text.Trim();
                    SqlDSProvs.InsertParameters["numInt"].DefaultValue = txtNI.Text.Trim();
                    SqlDSProvs.InsertParameters["colonia"].DefaultValue = txtCol.Text.Trim();
                    SqlDSProvs.InsertParameters["telefonoPart"].DefaultValue = txtTel.Text.Trim();
                    SqlDSProvs.InsertParameters["telefonoCel"].DefaultValue = txtCel.Text.Trim();
                    SqlDSProvs.InsertParameters["codigoPostal"].DefaultValue = txtCP.Text.Trim();
                    SqlDSProvs.InsertParameters["apellidoPaterno"].DefaultValue = txtAP.Text.Trim();
                    SqlDSProvs.InsertParameters["apellidoMaterno"].DefaultValue = txtAM.Text.Trim();
                    SqlDSProvs.InsertParameters["nombres"].DefaultValue = txtNombre.Text.Trim();
                    SqlDSProvs.InsertParameters["email"].DefaultValue = txtCorreo.Text.Trim();
                    SqlDSProvs.InsertParameters["contacto"].DefaultValue = txtcontact.Text.Trim();
                    SqlDSProvs.InsertParameters["personaFiscal"].DefaultValue = rbtPersona.SelectedValue;
                    SqlDSProvs.Insert();
                    limpiaTextBox();
                    lblErrores.Text = "Proveedor Guardado";
                    GridView1.DataBind();
                }
                catch (Exception ex)
                {
                    lblErrores.Text = "Error al agregar el Proveedor " + txtRFC.Text + " - " + txtRazonSocial.Text + ": " + ex.Message;
                }
            }
            else
                lblErrores.Text = "El proveedor a ingresar ya se encuentra registrado";
        }
        else if(btnAgAc.CommandName.Equals("Actualiza"))
        {
            string provID = btnAgAc.CommandArgument;
            if (rbtPersona.SelectedValue == "M")
            {
                SqlDSProvs.UpdateParameters["razonSocial"].DefaultValue = txtRazonSocial.Text.Trim().ToUpper();
                SqlDSProvs.UpdateParameters["nombreComercial"].DefaultValue = txtRazonSocial.Text.Trim().ToUpper();
            }
            else
            {
                string strApMat = string.IsNullOrEmpty(txtAM.Text.Trim()) ? "" : " " + txtAM.Text.Trim();
                SqlDSProvs.UpdateParameters["razonSocial"].DefaultValue = txtNombre.Text.Trim().ToUpper() + " " + txtAP.Text.Trim() + strApMat.ToUpper();
                SqlDSProvs.UpdateParameters["nombreComercial"].DefaultValue = txtNombre.Text.Trim().ToUpper() + " " + txtAP.Text.Trim();
            }
            SqlDSProvs.UpdateParameters["clave"].DefaultValue = provID;
            SqlDSProvs.UpdateParameters["RFC"].DefaultValue = txtRFC.Text.ToUpper();
            SqlDSProvs.UpdateParameters["pais"].DefaultValue = txtPais.Text.Trim();
            SqlDSProvs.UpdateParameters["estado"].DefaultValue = txtEstado.Text.Trim();
            SqlDSProvs.UpdateParameters["ciudad"].DefaultValue = txtCiudad.Text.Trim();
            SqlDSProvs.UpdateParameters["calle"].DefaultValue = txtCalle.Text.Trim();
            SqlDSProvs.UpdateParameters["numExt"].DefaultValue = TxtNE.Text.Trim();
            SqlDSProvs.UpdateParameters["numInt"].DefaultValue = txtNI.Text.Trim();
            SqlDSProvs.UpdateParameters["colonia"].DefaultValue = txtCol.Text.Trim();
            SqlDSProvs.UpdateParameters["telefonoPart"].DefaultValue = txtTel.Text.Trim();
            SqlDSProvs.UpdateParameters["telefonoCel"].DefaultValue = txtCel.Text.Trim();
            SqlDSProvs.UpdateParameters["codigoPostal"].DefaultValue = txtCP.Text.Trim();
            SqlDSProvs.UpdateParameters["apellidoPaterno"].DefaultValue = txtAP.Text.Trim();
            SqlDSProvs.UpdateParameters["apellidoMaterno"].DefaultValue = txtAM.Text.Trim();
            SqlDSProvs.UpdateParameters["nombres"].DefaultValue = txtNombre.Text.Trim();
            SqlDSProvs.UpdateParameters["email"].DefaultValue = txtCorreo.Text.Trim();
            SqlDSProvs.UpdateParameters["contacto"].DefaultValue = txtcontact.Text.Trim();
            SqlDSProvs.UpdateParameters["personaFiscal"].DefaultValue = rbtPersona.SelectedValue;
            SqlDSProvs.Update();
            limpiaTextBox();
            btnAgrAct.Text = "Agregar";
            btnAgrAct.CommandName = "Inserta";
            lblErrores.Text = "Proveedor Actualizado";
            btnCancelar.Visible = false;
            GridView1.DataBind();
        }
    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            RadioButtonList rbtTipoPer = (RadioButtonList)GridView1.Rows[e.RowIndex].Cells[0].FindControl("rbtTipoPer");
            e.NewValues.Add("personaFiscal", rbtTipoPer.SelectedValue);
            if (rbtPersona.SelectedValue == "M")
            {
                e.NewValues.Add("nombreComercial", e.NewValues["razonSocial"].ToString());
            }
            else
            {
                string strApMat = string.IsNullOrEmpty(e.NewValues["apellidoMaterno"].ToString()) ? "" : " " + e.NewValues["apellidoMaterno"].ToString();
                e.NewValues["razonSocial"] = e.NewValues["nombres"].ToString() + " " + e.NewValues["apellidoPaterno"].ToString() + strApMat;
                e.NewValues.Add("nombreComercial", e.NewValues["nombres"].ToString() + " " + e.NewValues["apellidoPaterno"].ToString());
            }
        }
        catch (Exception ex)
        {
            string proveedor = GridView1.DataKeys[e.RowIndex].Value.ToString();
            lblErrores.Text = "Error al actualizar el proveedor " + proveedor + ": " + ex.Message;
        }
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        limpiaTextBox();
        btnCancelar.Visible = false;
        btnAgrAct.Text = "Agregar";
        btnAgrAct.CommandName = "Inserta";
    }

    private void limpiaTextBox()
    {
        txtRFC.Text = txtRazonSocial.Text = txtNombre.Text = txtAP.Text = txtAM.Text = txtCalle.Text = txtCol.Text = txtCiudad.Text = "";
        txtEstado.Text = TxtNE.Text = txtNI.Text = txtTel.Text = txtCel.Text = txtCorreo.Text = txtcontact.Text = txtCP.Text = "";
    }
}