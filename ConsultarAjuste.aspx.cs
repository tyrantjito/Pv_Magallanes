using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ConsultarAjuste : System.Web.UI.Page
{
    Producto datosProducto = new Producto();
    decimal totalCu, totalVp, totUtilidad, totArticulos;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlIslas.DataBind();
            pnltotales.Visible = false;
        }
    }

    private void cargaDatos()
    {
        lblError.Text = "";
        try
        {
            //quitar filtro de producto o crear uno general para el isposback 
            int isla = Convert.ToInt32(ddlIslas.SelectedValue);
            string articulo = txtFiltroArticulo.Text.Trim();
            string fecha = ddlFechas.SelectedValue;
            string conteo = "1";
            Producto prod = new Producto();
            prod.NombrePorducto = articulo;
            object[] articulos = prod.obtieneIdProducto();
            if (Convert.ToBoolean(articulos[0]))
            {
                prod.ClaveProducto = Convert.ToString(articulos[1]);
                DataSet data = new DataSet();
                data = datosProducto.llenaConsultaAjusteIslas2(isla, articulo, fecha,conteo);
                GridInvetarioProductos.DataSource = data;
                GridInvetarioProductos.DataBind();
            }
            else
            {
                DataSet data = new DataSet();
                data = datosProducto.llenaConsultaAjusteIslas2(isla, "", fecha, conteo);
                GridInvetarioProductos.DataSource = data;
                GridInvetarioProductos.DataBind();
            }
            pnltotales.Visible = true;
            cargaTotales(isla, fecha, conteo);
        }
        catch (Exception ex) { }
    }

    private void cargaTotales(int isla, string fecha, string conteo)
    {
        DataSet tot = new DataSet();
        tot = datosProducto.obtieneTotales2(isla, fecha, conteo);
        decimal inicio = 0, fin = 0, utilidad = 0;
        if (tot.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow r in tot.Tables[0].Rows) {
                try { inicio = Convert.ToDecimal(r[0]); } catch (Exception ex) { inicio = 0; }
                try { fin = Convert.ToDecimal(r[1]); } catch (Exception ex) { fin = 0; }
                try { utilidad = Convert.ToDecimal(r[2]); } catch (Exception ex) { utilidad = 0; }
            }

            colocarColor(inicio, lblInicio);
            colocarColor(fin, lblFinal);
            colocarColor(utilidad, lblUtilidad);

            lblInicio.Text = inicio.ToString("C2");
            lblFinal.Text = fin.ToString("C2");
            lblUtilidad.Text = utilidad.ToString("C2");
        }
        else
        {
            colocarColor(inicio, lblInicio);
            colocarColor(fin, lblFinal);
            colocarColor(utilidad, lblUtilidad);
            lblInicio.Text = inicio.ToString("C2");
            lblFinal.Text = fin.ToString("C2");
            lblUtilidad.Text = utilidad.ToString("C2");
        }

    }

    private void colocarColor(decimal monto, Label etiqueta)
    {
        if (monto < 0)
            etiqueta.CssClass = "text-danger";
        if(monto>0)
            etiqueta.CssClass = "text-success";
        if (monto == 0)
            etiqueta.CssClass = "text-warning";
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

    protected void lnkBuscar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (ddlFechas.SelectedValue == "")
            lblError.Text = "Debe indicar una fecha, si no cuenta con una proceda a realizar un inventario";
        else
            cargaDatos(); 
    }

    protected void GridInvetarioProductos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
    }

    protected void GridInvetarioProductos_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblDiferencia = e.Row.FindControl("lblModificado") as Label;
            
            decimal existencia = 0;            
            try { existencia = Convert.ToDecimal(lblDiferencia.Text); } catch (Exception) { existencia = 0; }
            if (existencia == 0)
                lblDiferencia.CssClass = e.Row.Cells[5].CssClass = "alert-warning";
            else if (existencia < 0)
                lblDiferencia.CssClass = e.Row.Cells[5].CssClass = "alert-danger";
            else if (existencia > 0)
                lblDiferencia.CssClass = e.Row.Cells[5].CssClass = "alert-success-org";
        }
    }

    protected void ddlIslas_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        pnltotales.Visible = false;
        GridInvetarioProductos.DataSource = null;
        GridInvetarioProductos.DataBind();
    }
}