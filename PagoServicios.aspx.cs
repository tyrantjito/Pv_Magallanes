using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using System.IO;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;

public partial class PagoServicios : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {            
            imgLogo.ImageUrl = "";
            lblError.Text = "";                        
            txtTelefono.Text = txtReferencia.Text = txtMonto.Text = "";
            Label1.Visible = Label2.Visible = Label4.Visible = Label5.Visible = Label6.Visible = false;            
            txtTelefono.Visible = txtTelefonoConfirm.Visible = txtDigito.Visible = txtReferencia.Visible = txtMonto.Visible = false;
            lnkAbonar.Visible = false;
            lblOperacion.Text = lblIdCatTipoServicio.Text = lblTipoFront.Text = lblIdServicio.Text = lblIdProducto.Text = "0";
            lblReferenciaInfo.Text = "";
            lblDescripcion.Text = "";
            imgReference.Visible = false;
        }
    }
    
    protected void lnkListaProductos_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        PServicios pagos = new PServicios();
        pagos._peticion = 1;
        pagos.obtienePagoServicios();
        object[] pago = pagos._retorno;
        lblError.Text = Convert.ToString(pago[1]);        
        lblIdCatTipoServicio.Text = "0";        
        lblIdServicio.Text = "0";        
    }
    /*
    protected void lnkServicio_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        LinkButton btn = (LinkButton)sender;
        string[] argumnetos = btn.CommandArgument.Split(new char[] { ';' });
        int idCatTipoServicio = Convert.ToInt32(argumnetos[0]);
        int tipoFront = Convert.ToInt32(lblTipoFront.Text);
        string tipoServ = argumnetos[1];        
        lblIdCatTipoServicio.Text = idCatTipoServicio.ToString();
        lblServicioIndicado.Text = tipoServ;
        lblIdServicio.Text = "0";
        lblServicio.Text = "Seleccione un Servicio";
        ddlProducto.Items.Clear();
        ddlProducto.DataBind();
       
        txtReferencia.Text = txtMonto.Text = txtTelefono.Text = "";
        Label1.Visible = Label2.Visible = Label3.Visible = Label4.Visible = Label5.Visible = Label6.Visible = false;
        ddlProducto.Visible = false;
        txtTelefono.Visible = txtTelefonoConfirm.Visible = txtDigito.Visible = txtReferencia.Visible = txtMonto.Visible = false;
        lnkAbonar.Visible = false;
        lnkImprimir.Visible = false;        
    }

    protected void lnkServ_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        LinkButton btn = (LinkButton)sender;
        string[] argumnetos = btn.CommandArgument.Split(new char[] { ';' });
        int idServicio = Convert.ToInt32(argumnetos[0]);
        int tipoFront = Convert.ToInt32(lblTipoFront.Text);
        string tipoServ = argumnetos[1];
        lblIdServicio.Text = idServicio.ToString();        
        lblServicio.Text = tipoServ;
        ddlProducto.Items.Clear();
        ddlProducto.DataBind();
        DataList1.DataBind();
        if (lblTipoFront.Text == "2")
        {
            txtTelefono.Text = txtTelefonoConfirm.Text = "1111111111";
            txtTelefono.Enabled = txtTelefonoConfirm.Enabled = false;
            RequiredFieldValidator2.Enabled = false;
        }
        else {
            txtTelefono.Text = txtTelefonoConfirm.Text = "";
            txtTelefono.Enabled = txtTelefonoConfirm.Enabled = true;
            RequiredFieldValidator2.Enabled = true;
        }
        txtReferencia.Text = txtMonto.Text = "";
        Label1.Visible = Label2.Visible = Label3.Visible = Label4.Visible = Label5.Visible = Label6.Visible = true;
        ddlProducto.Visible = true;
        txtTelefono.Visible = txtTelefonoConfirm.Visible = txtDigito.Visible = txtReferencia.Visible = txtMonto.Visible = true;
        lnkAbonar.Visible = true;
        habilitaDigito();
        lnkImprimir.Visible = false;
        lnkAbonar.Visible = true;
    }
    */
    protected void lnkAbonar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        LinkButton btn = (LinkButton)sender;
        string confirmacion = "";
        try
        {
            bool valido = esValido();
            if (valido)
            {
                if (lblTipoFront.Text == "2")
                    confirmacion = txtTelefono.Text;
                else
                    confirmacion = txtTelefonoConfirm.Text;

                if (txtTelefono.Text == confirmacion)
                {
                    string[] argumentos = new string[] { lblTipoFront.Text, lblIdCatTipoServicio.Text, lblIdServicio.Text, lblIdProducto.Text, txtTelefono.Text, txtDigito.Text, txtReferencia.Text, txtMonto.Text };
                    PServicios pagos = new PServicios();
                    pagos._peticion = 2;
                    pagos._datos = argumentos;
                    pagos._caja = Convert.ToInt32(Request.QueryString["c"]);
                    pagos._punto = Convert.ToInt32(Request.QueryString["p"]);
                    pagos._usuario = Request.QueryString["u"];
                    pagos.tipoFront = Convert.ToInt32(lblTipoFront.Text);
                    if (pagos.tipoFront == 3)
                        argumentos = new string[] { lblTipoFront.Text, lblIdCatTipoServicio.Text, lblIdServicio.Text, lblIdProducto.Text, txtTelefono.Text, txtDigito.Text, lblServicio.Text, txtMonto.Text };
                    pagos.telefono = txtTelefono.Text;
                    pagos.referencia_in = txtReferencia.Text;
                    pagos.catServicio = Convert.ToInt32(lblIdCatTipoServicio.Text);
                    pagos.tipoFont = Convert.ToInt32(lblTipoFront.Text);
                    pagos.idServicio = Convert.ToInt32(lblIdServicio.Text);
                    pagos.idProducto = Convert.ToInt32(lblIdProducto.Text);
                    string valor = lblDescripcion.Text.ToUpper();
                    int posicion = -1;
                    try
                    {
                        posicion = valor.IndexOf("RECARGA");
                    }
                    catch (Exception EX) { posicion = -1; }
                    if (posicion != -1)
                        pagos.esRecarga = true;
                    else
                        pagos.esRecarga = false;

                    try
                    {
                        pagos.montoPagar = Convert.ToDecimal(txtMonto.Text);
                    }
                    catch (Exception) { pagos.montoPagar = 0; }

                    pagos.obtienePagoServicios();
                    object[] pago = pagos._retorno;
                    if (pagos._codigo == "01")
                    {
                        lblOperacion.Text = pagos.operacion.ToString();
                        Session["operacion"] = pagos.operacion;
                        if (lblTipoFront.Text == "2")
                            txtTelefono.Text = "1111111111";
                        else
                            txtTelefono.Text = "";
                        txtReferencia.Text = txtMonto.Text = "";
                        Label1.Visible = Label2.Visible = Label4.Visible = Label5.Visible = Label6.Visible = true;
                        txtTelefono.Visible = txtTelefonoConfirm.Visible = txtDigito.Visible = txtReferencia.Visible = txtMonto.Visible = true;
                        lnkAbonar.Visible = true;
                        habilitaDigito();
                        if (Convert.ToInt32(argumentos[0]) == 2)
                        {
                            RequiredFieldValidator5.Enabled = RequiredFieldValidator6.Enabled = true;
                            txtReferencia.Enabled = txtMonto.Enabled = true;
                        }
                        else
                        {
                            if (Convert.ToInt32(argumentos[0]) == 3)
                            {
                                RequiredFieldValidator6.Enabled = true;
                                txtMonto.Enabled = true;
                            }
                            else
                            {
                                RequiredFieldValidator5.Enabled = RequiredFieldValidator6.Enabled = false;
                                txtReferencia.Enabled = txtMonto.Enabled = false;
                            }
                        }

                        if (pagos.operacion != 0)
                        {
                            ImprimePagoServicio impTicket = new ImprimePagoServicio();
                            impTicket.PuntoVenta = Convert.ToInt32(Request.QueryString["p"]);
                            impTicket.Operacion = Convert.ToInt32(pagos.operacion.ToString());
                            impTicket.Caja = Convert.ToInt32(Request.QueryString["c"]);
                            //pagos.actualizaRecarga();
                            string Archivo = impTicket.GenerarTicket();
                            if (Archivo != "")
                            {
                                try
                                {
                                    System.IO.FileInfo filename = new System.IO.FileInfo(Archivo);
                                    if (filename.Exists)
                                    {
                                        string url = "TicketPdf.aspx?a=" + filename.Name;
                                        lblOperacion.Text = "0";
                                        lnkImprimir.Visible = false;
                                        lnkAbonar.Visible = true;
                                        lblError.Text = "";

                                        ScriptManager.RegisterStartupScript(this, typeof(Page), "pdfs", "window.open('" + url + "', 'nuevo', 'directories=no, location=no, menubar=no, scrollbars=yes, statusbar=no, titlebar=no, width=856px, height=550px');", true);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    lblError.Text = ex.Message;
                                    lnkImprimir.Visible = true;
                                    lnkAbonar.Visible = false;
                                }
                            }
                            else
                            {
                                lnkImprimir.Visible = true;
                                lnkAbonar.Visible = false;
                                lblError.Text = "Error al imprimir el ticket de la operacion: " + pagos.operacion.ToString() + ", vuelva intentarlo";
                            }
                        }
                        else
                        {
                            lnkImprimir.Visible = false;
                            lnkAbonar.Visible = true;
                            lblError.Text = "Error al imprimir el ticket de la operacion: " + pagos.operacion.ToString() + ", vuelva intentarlo";
                        }
                        lnkImprimir.Visible = true;
                    }
                    else
                    {
                        lblError.Text = "Error " + pagos._codigo + ": " + pago[1].ToString();
                        lnkImprimir.Visible = false;
                        lnkAbonar.Visible = true;
                    }
                    try { lblError.Text = Convert.ToString(pago[1]).Replace(Environment.NewLine, "<br/>"); }
                    catch (Exception ex) { lblError.Text = pago[1].ToString() + " Execepcion:" + ex.Message; }
                }
                else
                    lblError.Text = "Error: el teléfono con su confirmación no coinciden, por favor verificar";
            }
        }
        catch (Exception ex) { lblError.Text = "Error al procesar: " + ex.Message; }
    }

    private bool esValido()
    {
        bool valido = false;
        if (lblIdCatTipoServicio.Text == "0")
        {
            valido = false;
            lblError.Text = "Debe seleccionar un Pago";
        }
        else
        {
            if (lblIdServicio.Text == "0")
            {
                valido = false;
                lblError.Text = "Debe seleccionar un Servicio";
            }
            else {
                if (lblIdProducto.Text == "0" || lblTipoFront.Text == "0")
                {
                    valido = false;
                    lblError.Text = "Debe seleccionar un Producto";
                }
                else
                {
                    valido = true;
                    lblError.Text = "";
                }
            }
        }

        return valido;
    }
    /*
    protected void btn_Click(object sender, EventArgs e)
    {
        RadButton btn = (RadButton)sender;
        string[] argumentos = btn.CommandArgument.Split(new char[] { ';' });
        if (Convert.ToInt32(argumentos[0]) == 2)
        {
            RequiredFieldValidator5.Enabled = RequiredFieldValidator6.Enabled = true;
            txtReferencia.Enabled = txtMonto.Enabled = true;
        }
        else
        {
            if (Convert.ToInt32(argumentos[0]) == 3)
            {
                RequiredFieldValidator6.Enabled = true;
                txtMonto.Enabled = true;
            }
            else
            {
                RequiredFieldValidator5.Enabled = RequiredFieldValidator6.Enabled = false;
                txtReferencia.Enabled = txtMonto.Enabled = false;
            }
        }
        habilitaDigito();
        lblTipoFront.Text = argumentos[0];
        lblTipoFronValor.Text = btn.Text.ToUpper();
        DataList1.DataBind();
        lblServicioIndicado.Text = "Seleccione una opción";
        lblIdCatTipoServicio.Text = "0";
        lblIdServicio.Text = "0";
        lblServicio.Text = "Seleccione un Servicio";
        ddlProducto.Items.Clear();
        ddlProducto.DataBind();
        txtTelefono.Text = txtReferencia.Text = txtMonto.Text = "";
        Label1.Visible = Label2.Visible = Label3.Visible = Label4.Visible = Label5.Visible = Label6.Visible = false;
        ddlProducto.Visible = false;
        txtTelefono.Visible = txtTelefonoConfirm.Visible = txtDigito.Visible = txtReferencia.Visible = txtMonto.Visible = false;
        lnkAbonar.Visible = false;
    }

    protected void ddlProducto_SelectedIndexChanged(object sender, EventArgs e)
    {
        habilitaDigito();   
    }
    */
    private void habilitaDigito() {
        object[] informacion = new object[] { lblTipoFront.Text, lblIdCatTipoServicio.Text, lblIdServicio.Text, lblIdProducto.Text };
        PServicios servicios = new PServicios();
        servicios._datos = informacion;
        servicios.obtieneInformacionProducto();
        object[] datos = servicios._retorno;
        if (Convert.ToBoolean(datos[0]))
        {
            DataSet info = (DataSet)datos[1];
            foreach (DataRow fila in info.Tables[0].Rows)
            {
                txtDigito.Enabled = RequiredFieldValidator4.Enabled = Convert.ToBoolean(fila[7]);
            }
        }
        else
            txtDigito.Enabled = RequiredFieldValidator4.Enabled = false;
    }

    protected void lnkImprimir_Click(object sender, EventArgs e)
    {
        ImprimePagoServicio impTicket = new ImprimePagoServicio();
        impTicket.PuntoVenta = Convert.ToInt32(Request.QueryString["p"]);
        int operacionImprime = 0;
        try { operacionImprime = Convert.ToInt32(Session["operacion"].ToString()); }
        catch (Exception) { operacionImprime = Convert.ToInt32(lblOperacion.Text); }

        impTicket.Operacion = operacionImprime;
        impTicket.Caja = Convert.ToInt32(Request.QueryString["c"]);
        string Archivo = impTicket.GenerarTicket();
        if (Archivo != "")
        {
            try
            {
                System.IO.FileInfo filename = new System.IO.FileInfo(Archivo);
                if (filename.Exists)
                {

                    string url = "TicketPdf.aspx?a=" + filename.Name;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "pdfs", "window.open('" + url + "', 'nuevo', 'directories=no, location=no, menubar=no, scrollbars=yes, statusbar=no, titlebar=no, width=856px, height=550px');", true);

                    /* impresion directa con crystal reports
                    string ruta = HttpContext.Current.Server.MapPath("~/PagoServicios/TMP");
                    string archivoCopia = ruta + "\\" + filename.Name;

                    //si no existe la carpeta temporal la creamos 
                    if (!(Directory.Exists(ruta)))
                        Directory.CreateDirectory(ruta);

                    PdfReader reader = new PdfReader(Archivo);
                    PdfStamper stamper = new PdfStamper(reader, new FileStream(archivoCopia, FileMode.Create));
                    AcroFields fields = stamper.AcroFields;
                    stamper.JavaScript = "this.print(true);\r";
                    stamper.FormFlattening = true;
                    stamper.Close();
                    reader.Close();

                    FileInfo copia = new FileInfo(archivoCopia);

                    Response.Redirect("TicketPdf.aspx?a=" + copia.Name);


                    */
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        else
            lblError.Text = "Error al imprimir el ticket de la operacion: " + lblOperacion.Text + ", vuelva intentarlo";

    }

    protected void Referencias(object sender, EventArgs e)
    {
        imgRef = imgReference;
        //imgRef.ImageUrl = string.Format("DisplayLogo.ashx?id={0};{1};2", lblIdCatTipoServicio.Text, lblIdServicio.Text);
        ScriptManager.RegisterStartupScript(this, typeof(Page), "imagen", "abreWinEmi()", true);
    }
    
    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid2.Rebind();            
            RadGrid3.Rebind();
            lblError.Text = "";
            object valor = "0"; try { valor = RadGrid2.SelectedValues["idServicio"]; }
            catch (Exception) { valor = "0"; }
            //imgLogo.ImageUrl = imgReference.ImageUrl = string.Format("DisplayLogo.ashx?id={0};{1};1", RadGrid1.SelectedValue, valor.ToString());
            
            lblIdCatTipoServicio.Text = RadGrid1.SelectedValue.ToString();
            lblIdServicio.Text = lblTipoFront.Text = lblIdProducto.Text = "0";
            lblDescripcion.Text = "";
            txtTelefono.Text = txtReferencia.Text = txtMonto.Text = "";
            Label1.Visible = Label2.Visible = Label4.Visible = Label5.Visible = Label6.Visible = false;            
            txtTelefono.Visible = txtTelefonoConfirm.Visible = txtDigito.Visible = txtReferencia.Visible = txtMonto.Visible = false;
            lnkAbonar.Visible = false;

            lblReferenciaInfo.Text = "";
            imgReference.Visible = false;

        }
        catch (Exception) { lblIdCatTipoServicio.Text = lblIdServicio.Text = lblTipoFront.Text = lblIdProducto.Text = "0"; lblServicio.Text = ""; }
    }

    protected void RadGrid2_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {   
            RadGrid3.Rebind();
            lblError.Text = "";
            object valor = RadGrid2.SelectedValues["idServicio"];            
            object valorProd = RadGrid2.SelectedValues["servicio"];  
            imgLogo.ImageUrl = string.Format("DisplayLogo.ashx?id={0};{1};1", RadGrid1.SelectedValue, valor.ToString());
            imgReference.ImageUrl = string.Format("DisplayLogo.ashx?id={0};{1};2", RadGrid1.SelectedValue, valor.ToString());
            lblIdCatTipoServicio.Text = RadGrid1.SelectedValue.ToString();
            lblIdServicio.Text = valor.ToString();
            lblServicio.Text = valorProd.ToString();
            lblTipoFront.Text = lblIdProducto.Text = "0";
            txtTelefono.Text = txtReferencia.Text = txtMonto.Text = "";
            Label1.Visible = Label2.Visible = Label4.Visible = Label5.Visible = Label6.Visible = false;            
            txtTelefono.Visible = txtTelefonoConfirm.Visible = txtDigito.Visible = txtReferencia.Visible = txtMonto.Visible = false;
            lnkAbonar.Visible = false;            
            lblReferenciaInfo.Text = "";
            lblDescripcion.Text = "";
            imgReference.Visible = false;
        }
        catch (Exception) { lblIdServicio.Text = lblTipoFront.Text = lblIdProducto.Text = "0"; lblServicio.Text = ""; }
    }

    protected void RadGrid3_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try {
            object valor = RadGrid3.SelectedValues["tipoFront"];
            lblTipoFront.Text = valor.ToString();
            object valorProd = RadGrid3.SelectedValues["idProducto"];
            object valorDescProd = RadGrid3.SelectedValues["producto"];
            lblIdProducto.Text = valorProd.ToString();
            lblDescripcion.Text = valorDescProd.ToString();
            lblError.Text = "";

            if (lblTipoFront.Text == "2")
            {
                txtTelefono.Text = txtTelefonoConfirm.Text = "1111111111";
                txtTelefono.Enabled = txtTelefonoConfirm.Enabled = false;
                RequiredFieldValidator2.Enabled = false;
                RequiredFieldValidator5.Enabled = RequiredFieldValidator6.Enabled = true;
                txtReferencia.Enabled = txtMonto.Enabled = true;
                lblReferenciaInfo.Text = "¿Donde puedo ubicar la referencia?";
                imgReference.Visible = true;
            }
            else
            {
                if (lblTipoFront.Text == "3")
                {
                    RequiredFieldValidator6.Enabled = true;
                    txtMonto.Enabled = true;
                }
                else
                {
                    RequiredFieldValidator5.Enabled = RequiredFieldValidator6.Enabled = false;
                    txtReferencia.Enabled = txtMonto.Enabled = false;
                }
                txtTelefono.Text = txtTelefonoConfirm.Text = "";
                txtTelefono.Enabled = txtTelefonoConfirm.Enabled = true;
                RequiredFieldValidator2.Enabled = true;

                lblReferenciaInfo.Text = "Ayuda del Servicio";
                imgReference.Visible = true;

            }
            txtReferencia.Text = txtMonto.Text = "";
            Label1.Visible = Label2.Visible = Label4.Visible = Label5.Visible = Label6.Visible = true;            
            txtTelefono.Visible = txtTelefonoConfirm.Visible = txtDigito.Visible = txtReferencia.Visible = txtMonto.Visible = true;            
            habilitaDigito();
            lnkImprimir.Visible = false;
            lnkAbonar.Visible = true;
        }
        catch (Exception) { lblTipoFront.Text = lblIdProducto.Text = "0"; }
    }

}