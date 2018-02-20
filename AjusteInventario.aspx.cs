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
using Telerik.Web.UI;

public partial class AjusteInventario : System.Web.UI.Page
{
    Producto datosProducto = new Producto();
    decimal totalCu, totalVp, totUtilidad, totArticulos;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlIslas.DataBind();
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

    private void cargaDatosGral()
    {
        try
        {
            //quitar filtro de producto o crear uno general para el isposback 
            int isla = Convert.ToInt32(ddlIslas.SelectedValue);
            string articulo = txtFiltroArticulo.Text;
            Producto prod = new Producto();
            DataSet data = new DataSet();
            data = datosProducto.llenaConsultaAjusteIsla(isla, "");
            GridInvetarioProductos.DataSource = data;
            GridInvetarioProductos.DataBind();
        }
        catch (Exception ex) { }
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
                    lblError.Text = "Hubo un probla en la actualización, verifique su conexión e intentelo nuevamente mas tarde.";
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
    
    protected void GridInvetarioProductos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblTotal = e.Row.FindControl("lblTotal") as Label;
            Label lblExistencia = e.Row.FindControl("lblExistenciaN") as Label;
            Label lblConteos = e.Row.FindControl("lblConteos") as Label;
            Label lblPrecioVenta = e.Row.FindControl("lblPrecioVenta") as Label;
            LinkButton lnkArticulo = e.Row.FindControl("lnkIdArticulo") as LinkButton;

            decimal existencia = 0, precioVenta = 0;
            try { existencia = Convert.ToDecimal(lblExistencia.Text); } catch (Exception ex) { }
            try { precioVenta = Convert.ToDecimal(lblPrecioVenta.Text.Trim(new char[] { '$' })); } catch (Exception ex) { }
            lblTotal.Text = (existencia * precioVenta).ToString("C2");

            try { existencia = Convert.ToDecimal(lblExistencia.Text); } catch (Exception) { existencia = 0; }
            if (existencia == 0)
            {
                lblExistencia.ForeColor = Color.DarkGoldenrod;
                e.Row.Cells[2].CssClass = "alert-warning";
            }
            else if (existencia < 0)
            {
                lblExistencia.ForeColor = Color.Red;
                e.Row.Cells[2].CssClass = "alert-danger";
            }
            else if (existencia > 0)
            {
                lblExistencia.ForeColor = Color.Green;
            }
            object[] cont = datosProducto.obtieneConteos(ddlIslas.SelectedValue, lnkArticulo.Text);
            if (Convert.ToBoolean(cont[0]))
            {
                lblConteos.Text = "";
                decimal total = 0;
                DataSet info = (DataSet)cont[1];
                foreach(DataRow r in info.Tables[0].Rows)
                {
                    lblConteos.Text = lblConteos.Text + Convert.ToDecimal(r[0]).ToString("F3") + "+";
                    total = total + Convert.ToDecimal(r[0]);
                }
                if (info.Tables[0].Rows.Count != 0)
                    lblConteos.Text = lblConteos.Text.Substring(0, lblConteos.Text.Length - 1) + "=" + total;
            }
            else { lblConteos.Text = ""; }

        }
    }

    protected void lnkBuscar_Click(object sender, EventArgs e)
    {
        cargaDatos();
    }

    protected void txtAjusteN_TextChanged(object sender, EventArgs e)
    {
        try
        {
            GridViewRow currentRow = (GridViewRow)((TextBox)sender).Parent.Parent;
            Label lblIdArticulo = currentRow.FindControl("lblIdArticulo") as Label;
            Label lblDescripcion = currentRow.FindControl("lblDescripcion") as Label;
            Label lblExistenciaFinal = currentRow.FindControl("lblExistenciaFinalN") as Label;
            Label lblExistencia = currentRow.FindControl("lblExistenciaN") as Label;
            RadNumericTextBox txtAjusteNR = currentRow.FindControl("txtAjusteN") as RadNumericTextBox;
            decimal existencia = 0, existenciaFinal = 0, ajuste = 0;
            string usuario = Request.QueryString["U"]; ;
            try { existencia = Convert.ToDecimal(lblExistencia.Text); } catch (Exception) { existencia = 0; }
            try { existenciaFinal = Convert.ToDecimal(lblExistenciaFinal.Text); } catch (Exception) { existenciaFinal = 0; }
            try { ajuste = Convert.ToDecimal(txtAjusteNR.Text); } catch (Exception) { ajuste = 0; }
            existenciaFinal = (existencia) + (ajuste);
            if (existenciaFinal > -1)
            {

                lblExistenciaFinal.Text = existenciaFinal.ToString("F2");
                object[] actualizaExist = datosProducto.actualizaExistencia(existenciaFinal, ddlIslas.SelectedValue, lblIdArticulo.Text, existencia, ajuste, usuario);
                if (Convert.ToBoolean(actualizaExist[0]))
                {
                    if (Convert.ToBoolean(actualizaExist[1]))
                        lblError.Text = lblDescripcion.Text + " actualizado exitosamente";
                    else
                        lblError.Text = "Ocurrio un error inesperado al ajustar el articulo.";
                    cargaDatos();
                }
                else
                {
                    lblError.Text = "Ocurrio un error inesperado al ajustar el articulo: " + actualizaExist[1].ToString();
                    cargaDatos();
                }
            }
            else
            {
                lblError.Text = "El ajuste no puede generar negativos en la existencia final.";
                lblExistenciaFinal.Text = "0.00";
                txtAjusteNR.Text = "0";
            }
        }
        catch (Exception ex)
        {
            lblError.Text = "Ocurrio un error al ejecutar el ajuste: " + ex.Message.ToArray();
        }
    }

    protected void lnkOK_Click(object sender, EventArgs e)
    {
        try
        {
            GridViewRow currentRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
            LinkButton lblIdArticulo = currentRow.FindControl("lnkIdArticulo") as LinkButton;
            Label lblDescripcion = currentRow.FindControl("lblDescripcion") as Label;
            Label lblExistenciaFinal = currentRow.FindControl("lblExistenciaFinalN") as Label;
            Label lblExistencia = currentRow.FindControl("lblExistenciaN") as Label;
            RadNumericTextBox txtAjusteNR = currentRow.FindControl("txtAjusteNR") as RadNumericTextBox;
            decimal existencia = 0, existenciaFinal = 0, ajuste = 0;
            string usuario = Request.QueryString["U"];
            try { ajuste = Convert.ToDecimal(txtAjusteNR.Text); } catch (Exception) { ajuste = 0; }
            /* try { existencia = Convert.ToDecimal(lblExistencia.Text); } catch (Exception) { existencia = 0; }
             try { existenciaFinal = Convert.ToDecimal(lblExistenciaFinal.Text); } catch (Exception) { existenciaFinal = 0; }

             existenciaFinal = (existencia) + (ajuste);
             if (existenciaFinal > -1)
             {

                 lblExistenciaFinal.Text = existenciaFinal.ToString("F2");*/
            object[] actualizaExist = datosProducto.generaHistorico(ddlIslas.SelectedValue, lblIdArticulo.Text, ajuste, usuario);
                //object[] actualizaExist = datosProducto.actualizaExistencia(existenciaFinal, ddlIslas.SelectedValue, lblIdArticulo.Text, existencia, ajuste, usuario);
                if (Convert.ToBoolean(actualizaExist[0]))
                {
                    if (Convert.ToBoolean(actualizaExist[1]))
                        lblError.Text = lblDescripcion.Text + " actualizado exitosamente";
                    else
                        lblError.Text = "Ocurrio un error inesperado al ajustar el articulo.";
                    cargaDatos();
                }
                else
                {
                    lblError.Text = "Ocurrio un error inesperado al ajustar el articulo: " + actualizaExist[1].ToString();
                    cargaDatos();
                }
           /* }
            else
            {
                lblError.Text = "El ajuste no puede generar negativos en la existencia final.";
                lblExistenciaFinal.Text = "0.00";
                txtAjusteNR.Text = "0";
            }*/
        }
        catch (Exception ex)
        {
            lblError.Text = "Ocurrio un error al ejecutar el ajuste: " + ex.Message.ToArray();
        }
    }

    protected void lnkIdArticulo_Click(object sender, EventArgs e)
    {
        PanelPopDetalle.Visible = true;
        PanelMask.Visible = true;
        GridViewRow currentRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        LinkButton lnkIdArticulo = currentRow.FindControl("lnkIdArticulo") as LinkButton;
        string[] argumentos = lnkIdArticulo.CommandArgument.Split(';');
        lbltituPop.Text = "Movimientos de "+argumentos[0]+" "+argumentos[1];
        DataSet sumas = new DataSet();
        object[] ejecutado = datosProducto.obtieneDetalleInventario(argumentos[0],ddlIslas.SelectedValue);
        if(Convert.ToBoolean(ejecutado[0]))
        {
            sumas = (DataSet)ejecutado[1];
            foreach(DataRow fila in sumas.Tables[0].Rows)
            {
                decimal ajusDec = 0,entrDec=0,ventDec=0;
                try { ajusDec= Convert.ToDecimal(fila[0].ToString()); } catch (Exception) { }
                try { entrDec= Convert.ToDecimal(fila[1].ToString()); } catch (Exception) { }
                try { ventDec= Convert.ToDecimal(fila[2].ToString()); } catch (Exception) { }
                lblAjusteDet.Text = ajusDec.ToString("F2");
                lblEntradaDet.Text = entrDec.ToString("F2");
                lblVentaDet.Text = ventDec.ToString("F2");
                lblExistenciaDet.Text = (ajusDec+entrDec-ventDec).ToString("F2");
            }
        }
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        PanelPopDetalle.Visible = false;
        PanelMask.Visible = false;
    }

    protected void lnkConcluir_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)sender;
            string[] argumentos = btn.CommandArgument.ToString().Split(new char[] { ';' });
            string idArticulo = argumentos[0];
            string descripcion = argumentos[1];
            string isla = ddlIslas.SelectedValue;
            string usuario = Request.QueryString["U"];
            object[] actualizaExist = datosProducto.concluyeArticulo(isla, idArticulo, usuario);
            //object[] actualizaExist = datosProducto.actualizaExistencia(existenciaFinal, ddlIslas.SelectedValue, lblIdArticulo.Text, existencia, ajuste, usuario);
            if (Convert.ToBoolean(actualizaExist[0]))
            {
                if (Convert.ToBoolean(actualizaExist[1]))
                    lblError.Text = "El producto " + descripcion + " ha sido concluido en su conteo";
                else
                    lblError.Text = "Ocurrio un error inesperado al concluir el producto.";
                cargaDatos();
            }
            else
            {
                lblError.Text = "Ocurrio un error inesperado al concluir el producto: " + actualizaExist[1].ToString();
                cargaDatos();
            }
        }
        catch (Exception ex) { lblError.Text = "Error al concluir el producto: " + ex.Message; }
    }

    protected void lnkConcluirTodo_Click(object sender, EventArgs e)
    {
        string isla = ddlIslas.SelectedValue;
        string usuario = Request.QueryString["U"];
        object[] concluidos = datosProducto.concluirTodo(isla, usuario);
        if (Convert.ToBoolean(concluidos[0]))
        {
            lblError.Text = "Se han dado por concluido " + Convert.ToInt32(concluidos[1]).ToString() + " productos modificados";
        }
        else {
            lblError.Text = "No fue posible concluir los productos: " + concluidos[1].ToString();
        }
        cargaDatos();
    }
}

