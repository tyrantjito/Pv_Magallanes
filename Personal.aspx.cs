using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;

public partial class Personal : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtFechaFin.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
            txtFechaIni.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
            lblError.Text = "";
            GridPersonal.DataSource = null;
            GridPersonal.DataBind();
        }
    }

    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        cargaGrid();
    }

    private void cargaGrid()
    {
        try
        {
            string diaIni = txtFechaIni.Text;
            string diaFin = txtFechaFin.Text;
            DateTime fechaIni = Convert.ToDateTime(diaIni);
            DateTime fechaFin = Convert.ToDateTime(diaFin);
            if (fechaIni <= fechaFin)
            {
                if (fechaIni < fechas.obtieneFechaLocal())
                {
                    if (fechaFin < fechas.obtieneFechaLocal())
                    {
                        string isla = ddlIslas.SelectedValue;
                        string usuario = ddlUsuario.SelectedValue;
                        string whereSlq = "";
                        if (ddlIslas.SelectedValue == "T" && ddlUsuario.SelectedValue == "T")
                            whereSlq = "";
                        else if (ddlIslas.SelectedValue != "T" && ddlUsuario.SelectedValue == "T")
                            whereSlq = "c.id_punto = " + ddlIslas.SelectedValue + " and ";
                        else if (ddlIslas.SelectedValue != "T" && ddlUsuario.SelectedValue != "T")
                            whereSlq = "c.id_punto = " + ddlIslas.SelectedValue + " and c.usuario = '" + ddlUsuario.SelectedValue + "' and ";
                        else if (ddlIslas.SelectedValue == "T" && ddlUsuario.SelectedValue != "T")
                            whereSlq = "c.usuario = '" + ddlUsuario.SelectedValue + "' and ";
                        string sql = "select c.usuario,(u.nombre+' '+u.apellido_pat+' '+isnull(u.apellido_mat,'')) as nombreU,"+
                                     " CONVERT(char(10), c.fecha_apertura, 126) + ' ' + CONVERT(char(10), c.hora_apertura, 108) as ingreso,"+
                                     " CONVERT(char(10), isnull(c.fecha_cierre,'1900-01-01'), 126) + ' ' + CONVERT(char(10), isnull(c.hora_cierre,'00:00:00'), 126) as salida," +
                                     " g.nombre,c.id_caja" +
                                     " from cajas c" +
                                     " inner join usuarios_PV u on u.usuario = c.usuario" +
                                     " inner join catalmacenes g on c.id_punto = g.idAlmacen" +
                                     " where " + whereSlq + " c.fecha_apertura between '" + diaIni + "' and '" + diaFin + "'" +
                                     " order by c.usuario asc,c.fecha_apertura asc, c.hora_apertura asc";
                        BaseDatos ejecuta = new BaseDatos();
                        DataSet data = new DataSet();
                        object[] ejecutado = ejecuta.scalarData(sql);
                        if ((bool)ejecutado[0])
                            data = (DataSet)ejecutado[1];
                        GridPersonal.DataSource = data;
                        GridPersonal.DataBind();
                    }
                    else
                    {
                        txtFechaFin.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                        lblError.Text = "La fecha final no puede ser mayor al dia actual.";
                    }
                }
                else
                {
                    txtFechaIni.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                    lblError.Text = "La fecha inicial no puede ser mayor al dia actual.";
                }
            }
            else
            {
                txtFechaIni.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                txtFechaFin.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                lblError.Text = "La fecha inicial no puede ser mayor a la fecha final.";
            }
        }
        catch (Exception)
        {
            GridPersonal.DataSource = null;
            GridPersonal.DataBind();
        }
    }

    protected void GridPersonal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (ddlIslas.SelectedValue != "T" && ddlUsuario.SelectedValue == "T")
        {
            GridPersonal.Columns[0].Visible = false;
        }
        else if (ddlIslas.SelectedValue != "T" && ddlUsuario.SelectedValue != "T")
        {
            GridPersonal.Columns[0].Visible = false;
            GridPersonal.Columns[1].Visible = false;
        }
        else if (ddlIslas.SelectedValue == "T" && ddlUsuario.SelectedValue != "T")
        {
            GridPersonal.Columns[1].Visible = false;
        }
    }

    protected void GridPersonal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridPersonal.PageIndex = e.NewPageIndex;
        cargaGrid();
    }
    protected void btnImprimir_Click(object sender, EventArgs e)
    {
        ImprimePersonal impTicket = new ImprimePersonal();
        impTicket.Fecha_Ini = txtFechaIni.Text;
        impTicket.Fecha_Fin = txtFechaFin.Text;
        impTicket.UsuarioLog = Convert.ToString(Request.QueryString["u"]);
        if (ddlUsuario.SelectedValue == "T")
        {
            impTicket.UsuarioFiltro = "";
            impTicket.NombreUsuario = "";
        }
        else
        {
            impTicket.UsuarioFiltro = ddlUsuario.SelectedValue;
            impTicket.NombreUsuario = ddlUsuario.SelectedItem.Text;
        }
        if (ddlIslas.SelectedValue == "T")
        {
            impTicket.Isla = "0";
            impTicket.NombreIsla = "";
        }
        else {
            impTicket.Isla = ddlIslas.SelectedValue;
            impTicket.NombreIsla = ddlIslas.SelectedItem.Text;
        }


        string Archivo = impTicket.GenerarTicket();
        if (Archivo != "")
        {
            if (GridPersonal.Rows.Count != 0)
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
            lblError.Text = "Error al imprimir el reporte de personal, vuelva intentarlo";
    }
}