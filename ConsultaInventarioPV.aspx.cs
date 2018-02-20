using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ConsultaInventarioPV : System.Web.UI.Page
{
    Producto datosProducto = new Producto();
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
            cargaDatos();
    }

    private void cargaDatos()
    {
        try
        {

            int isla = Convert.ToInt32(Request.QueryString["p"]);
            string articulo = txtFiltroArticulo.Text;
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
            else
            {
                DataSet data = new DataSet();
                data = datosProducto.llenaConsultaAjusteIsla(isla, "");
                GridInvetarioProductos.DataSource = data;
                GridInvetarioProductos.DataBind();
            }
        }
        catch (Exception ex) { }
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
            var lblExistencia = e.Row.FindControl("lblCantidadExistencia") as Label;
            decimal existencia = 0;
            try { existencia=Convert.ToDecimal(lblExistencia.Text); } catch (Exception) { existencia = 0; }
            if (existencia<=0)
            {
                lblExistencia.Text = "0.00";
                e.Row.CssClass = "alert-danger";
            }
        }
    }
}