using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;

public partial class Acumulado : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    DateTime fechaIni, fechaFin;
    decimal acumulado = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            fechaIni = Convert.ToDateTime(fechas.obtieneFechaLocal().Year.ToString() + "-" + fechas.obtieneFechaLocal().Month + "-01");
            fechaFin = fechaIni.AddMonths(1);
            fechaFin = fechaFin.AddDays(-1);
            txtFechaIni.Text = fechaIni.ToString("yyyy-MM-dd");
            txtFechaFin.Text = fechaFin.ToString("yyyy-MM-dd");
            GridView1.DataSource = null;
            GridView1.DataBind();
            btn_imprimir.Visible = false;
        }
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        fechaIni = Convert.ToDateTime(txtFechaIni.Text);
        fechaFin = Convert.ToDateTime(txtFechaFin.Text);
        DateTime fechaActual = Convert.ToDateTime(fechas.obtieneFechaLocal().ToString("yyyy-MM-dd"));
        if (fechaFin < fechaIni)
            lblError.Text = "La fecha final no puede ser menor a la inicial";
        else if (fechaIni > fechaFin)
            lblError.Text = "La fecha inicial no puede ser superior a la inicial";
        else
        {
            if (ddlIslas.SelectedValue != "0")
            {
                GridView1.SelectedIndex = -1;
                GridView1.DataBind();
                btn_imprimir.Visible = true;
            }else
                lblError.Text = "Debe seleccionar una Tienda";
        }
    }
    protected void ddlIslas_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        GridView1.DataSource = null;
        GridView1.DataBind();
        btn_imprimir.Visible = false;
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string total = DataBinder.Eval(e.Row.DataItem, "tot_calculado").ToString();
            var lblAcum = e.Row.Cells[14].Controls[0].FindControl("lblAcum") as Label;
            acumulado = acumulado + Convert.ToDecimal(total);
            if (e.Row.RowState.ToString() == "Normal" || e.Row.RowState.ToString() == "Alternate" || e.Row.RowState.ToString() == "Selected")
            {
                lblAcum.Text = acumulado.ToString("C2");
                lblAcum.Font.Bold = true;
            }
        }
    }
    protected void btn_imprimir_Click(object sender, EventArgs e)
    {
        ImprimeAcum impTicket = new ImprimeAcum();
        impTicket.fecha_Ini = txtFechaIni.Text;
        impTicket.fecha_Fin = txtFechaFin.Text;
        impTicket.Usuario = Convert.ToString(Request.QueryString["u"]);
        string Archivo = impTicket.GenerarTicket();
        if (Archivo != "")
        {
            if (GridView1.Rows.Count != 0)
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
                lblError.Text = "No existe información para imprimir";
        }
        else
            lblError.Text = "Error al imprimir el acumulado, vuelva intentarlo";
    }
}