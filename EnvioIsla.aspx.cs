using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;

public partial class EnvioIsla : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    DateTime fechaIni;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {            
            txtFechaIni.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");            
        }
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        fechaIni = Convert.ToDateTime(txtFechaIni.Text);        
        DateTime fechaActual = Convert.ToDateTime(fechas.obtieneFechaLocal().ToString("yyyy-MM-dd"));
        if (fechaIni > fechaActual)
            lblError.Text = "La fecha inicial no puede ser superior a la inicial";
        else
        {
            GridView1.SelectedIndex = -1;
            GridView1.DataBind();            
        }
    }

    protected void btnImprimir_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        fechaIni = Convert.ToDateTime(txtFechaIni.Text);
        DateTime fechaActual = Convert.ToDateTime(fechas.obtieneFechaLocal().ToString("yyyy-MM-dd"));
        if (fechaIni > fechaActual)
            lblError.Text = "La fecha inicial no puede ser superior a la inicial";
        else
        {
            if (GridView1.Rows.Count != 0)
            {

                ImprimeEnvio impTicket = new ImprimeEnvio();
                impTicket.fecha_Ped = txtFechaIni.Text;
                impTicket.Nom_isla = ddlIslas.SelectedItem.Text;
                impTicket.Id_isla = ddlIslas.SelectedValue;
                impTicket.Usuario = Convert.ToString(Request.QueryString["u"]);

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

                            //Proceso para Imprimir Documento
                            /*Process proc = new Process();
                            proc.StartInfo.FileName = Archivo;
                            proc.Start();
                            myPDF.Attributes.Add("src", Archivo);
                            */

                            //Response.Write("<script type='text/javascript'>window.open('TicketPdf.aspx?a=" + Archivo + "','','status=yes, directories=no, location=no, menubar=no, resizable=no, scrollbars=no, titlebar=no, toolbar=no,width=700,height=800,left=0,top=0');</script>");

                            // Impresion directa
                            /*proc.StartInfo.UseShellExecute = true;
                            proc.StartInfo.CreateNoWindow = true;
                            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            proc.StartInfo.Verb = "print";*/

                            // abre pdf
                            /*proc.Start();
                            proc.WaitForExit();
                            proc.Close();*/
                        }
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = ex.Message;
                    }
                }
                else
                    lblError.Text = "Error al imprimir el envio de Tienda, vuelva intentarlo";
            }
            else
                lblError.Text = "No existen registros para imprimir";
        }
    }    
}