using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class ConsultaInventario : System.Web.UI.Page
{
    Producto datosProducto = new Producto();
    decimal totalCu, totalVp, totUtilidad, totArticulos;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //PanelPrincipal.CssClass = "ancho60 centrado";
            ddlIslas.DataBind();
            cargaDatos();
        }
    }

    private void cargaDatos()
    {
        try
        {
            int isla = Convert.ToInt32(ddlIslas.SelectedValue);
            string articulo = txtFiltroArticulo.Text.Trim();
            Producto prod = new Producto();
            prod.NombrePorducto = articulo;
            object[] articulos = prod.obtieneIdProducto();
            if (Convert.ToBoolean(articulos[0]))
            {
                prod.ClaveProducto = Convert.ToString(articulos[1]);
                DataSet data = new DataSet();                
                data = datosProducto.llenaConsultaAjusteIsla(isla, articulo);
                GridInvetarioProductos.DataSource = data;
                GridInvetarioProductos.DataBind();
            }
            else {
                DataSet data = new DataSet();
                data = datosProducto.llenaConsultaAjusteIsla(isla, "");
                GridInvetarioProductos.DataSource = data;
                GridInvetarioProductos.DataBind();
            }
        }
        catch (Exception ex) { }
    }

    protected void ddlIslas_SelectedIndexChanged(object sender, EventArgs e)
    {
        cargaDatos();
    }

    protected void GridInvetarioProductos_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //PanelPrincipal.CssClass = "ancho80 centrado";        
        GridInvetarioProductos.EditIndex = e.NewEditIndex;
        cargaDatos();
    }

    protected void GridInvetarioProductos_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        //PanelPrincipal.CssClass = "ancho60 centrado";
        e.Cancel = true;
        GridInvetarioProductos.EditIndex = -1;
        cargaDatos();
    }

    protected void GridInvetarioProductos_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //PanelPrincipal.CssClass = "ancho60 centrado";
        string idIsla = e.Keys[0].ToString();
        string idArticulo = e.Keys[1].ToString();
        TextBox txtMin = GridInvetarioProductos.Rows[e.RowIndex].FindControl("txtMin") as TextBox;
        TextBox txtMax = GridInvetarioProductos.Rows[e.RowIndex].FindControl("txtMax") as TextBox;
        decimal min = Convert.ToDecimal(txtMin.Text);
        decimal max = Convert.ToDecimal(txtMax.Text);
        if (min != max)
        {
            if (min < max)
            {

                bool actualizado = datosProducto.actualizaMinMax(idIsla, idArticulo, min, max);
                if (actualizado)
                {
                    GridInvetarioProductos.EditIndex = -1;
                    e.Cancel = true;
                    lblError.Text = "";
                }
                else
                {
                    lblError.Text = "Hubo un problema en la actualización, verifique su conexión e intentelo nuevamente mas tarde.";
                    GridInvetarioProductos.EditIndex = -1;
                    e.Cancel = true;
                }
                cargaDatos();
            }
            else
                lblError.Text = "El stock mínimo no puede ser mayor al stock máximo.";
        }
        else
            lblError.Text = "El stock mínimo y máximo no pueden ser iguales.";
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

    protected void txtFiltroArticulo_TextChanged(object sender, EventArgs e)
    {
        cargaDatos();
    }

    protected void GridInvetarioProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridInvetarioProductos.PageIndex = e.NewPageIndex;
        cargaDatos();
    }

    protected void GridInvetarioProductos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string utilidad = DataBinder.Eval(e.Row.DataItem, "Utilidad").ToString();
            string existencia = DataBinder.Eval(e.Row.DataItem, "cantidadExistencia").ToString();
            string totCu = DataBinder.Eval(e.Row.DataItem, "totalCostoUnitario").ToString();
            string totoVp = DataBinder.Eval(e.Row.DataItem, "totalVenta").ToString();            
            
            if (e.Row.RowState.ToString() == "Normal" || e.Row.RowState.ToString() == "Alternate")
            {
                var lblUtilidad = e.Row.Cells[2].Controls[1].FindControl("Label73") as Label;
                try
                {
                    totalCu = totalCu + Convert.ToDecimal(totCu);
                    totalVp = totalVp + Convert.ToDecimal(totoVp);
                    totUtilidad = totUtilidad + Convert.ToDecimal(utilidad);
                    if (Convert.ToDecimal(existencia) > 0)
                        totArticulos = totArticulos + Convert.ToDecimal(existencia);
                }
                catch (Exception) {
                    totalCu = totalVp = totUtilidad = totArticulos = 0;
                }

                if (Convert.ToDecimal(utilidad) <= 0)
                {
                    lblUtilidad.Font.Bold = true;
                    lblUtilidad.ForeColor = Color.Red;
                }
                else
                {
                    lblUtilidad.Font.Bold = true;
                    lblUtilidad.ForeColor = Color.ForestGreen;
                }
            }
            else if (e.Row.RowState.ToString() == "Edit")
            {
                var lblUtilidad = e.Row.Cells[2].Controls[1].FindControl("Label63") as Label;                
                if (Convert.ToDecimal(utilidad) <= Convert.ToDecimal("0"))
                {
                    lblUtilidad.Font.Bold = true;
                    lblUtilidad.ForeColor = Color.Red;
                }
                else
                {
                    lblUtilidad.Font.Bold = true;
                    lblUtilidad.ForeColor = Color.ForestGreen;
                }
            }
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            var lblTotArt = e.Row.Cells[2].Controls[1].FindControl("lblTotArti") as Label;
            var lblTotCu = e.Row.Cells[7].Controls[0].FindControl("lblTotCu") as Label;
            var lblTotVp = e.Row.Cells[8].Controls[0].FindControl("lblTotVp") as Label;
            var lblTotUtil = e.Row.Cells[9].Controls[0].FindControl("lblTotUtil") as Label;

            lblTotArt.Font.Bold = lblTotCu.Font.Bold = lblTotVp.Font.Bold = lblTotUtil.Font.Bold = true;

            lblTotArt.ForeColor = lblTotCu.ForeColor = lblTotVp.ForeColor = Color.Black;
            lblTotArt.Text = totArticulos.ToString();
            lblTotCu.Text = totalCu.ToString("C2");
            lblTotVp.Text = totalVp.ToString("C2");
            if (totUtilidad <= 0)
                lblTotUtil.ForeColor = Color.Red;
            else
                lblTotUtil.ForeColor = Color.ForestGreen;
            lblTotUtil.Text = totUtilidad.ToString("C2");
        }
    }

    protected void lnkImprime_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        int idIsla = Convert.ToInt32(ddlIslas.SelectedValue);
        string usuario = Request.QueryString["u"].ToString();
        ImprimeInventarioConsulta imprimir = new ImprimeInventarioConsulta();
        imprimir.usuario = usuario;
        imprimir.isla = idIsla;
        imprimir.NombreIslas = ddlIslas.SelectedItem.Text;
        string Archivo = imprimir.imprimeInventario();
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
}

