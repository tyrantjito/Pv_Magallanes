using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CatProductos : System.Web.UI.Page
{
    string usuarioLog;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            checaSesiones();
            try
            {
                llenaGrid();
            }
            catch (Exception)
            {
                llenaGrid();
            }
        }
    }

    private void checaSesiones()
    {
        try { usuarioLog = Convert.ToString(Request.QueryString["u"]); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
    }


    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView grilla = (GridView)sender;
        int index = -1;
        string producto = "";
        if (e.CommandName == "Update")
        {
            index = grilla.EditIndex;
            producto = grilla.DataKeys[index].Value.ToString();
            var descripcion = grilla.Rows[index].FindControl("txtDescripcionMod") as TextBox;
            var unidad = grilla.Rows[index].FindControl("ddlUnidadMod") as DropDownList;
            var familia = grilla.Rows[index].FindControl("ddlFamiliaMod") as DropDownList;
            var linea = grilla.Rows[index].FindControl("ddlLineaMod") as DropDownList;
            var detalles = grilla.Rows[index].FindControl("txtDetallesMod") as TextBox;
            var ddlCategoriaMod = grilla.Rows[index].FindControl("ddlCategoriaMod") as DropDownList; 
            var observaciones = grilla.Rows[index].FindControl("txtObservacionesMod") as TextBox;
            var agranel = grilla.Rows[index].FindControl("chkGranelMod") as CheckBox;
            var aIVA = grilla.Rows[index].FindControl("chkIVAMod") as CheckBox;


            string granel = "0";
            if (agranel.Checked)
                granel = "1";
            string IVA = "0";
            if (aIVA.Checked)
                IVA = "1";

            SqlDataSource1.UpdateCommand = "update catproductos set id_categoria=@id_categoria, descripcion=@descripcion, idMedida=@idMedida,idFamilia=@idFamilia,idLinea=@idLinea,detalles=@detalles,observaciones=@observaciones,granel=@granel,IVA=@IVA where idProducto=@idProducto";
            SqlDataSource1.UpdateParameters.Add("idProducto", producto);
            SqlDataSource1.UpdateParameters.Add("descripcion", descripcion.Text);
            SqlDataSource1.UpdateParameters.Add("id_categoria", ddlCategoriaMod.SelectedValue);
            SqlDataSource1.UpdateParameters.Add("idMedida", unidad.SelectedValue);
            SqlDataSource1.UpdateParameters.Add("idFamilia", familia.SelectedValue);
            SqlDataSource1.UpdateParameters.Add("idLinea", linea.SelectedValue);
            SqlDataSource1.UpdateParameters.Add("detalles", detalles.Text);
            SqlDataSource1.UpdateParameters.Add("observaciones", observaciones.Text);
            SqlDataSource1.UpdateParameters.Add("granel", granel);
            SqlDataSource1.UpdateParameters.Add("IVA", IVA);
            try
            {
                SqlDataSource1.Update();
                grilla.EditIndex = -1;
                grilla.DataBind();
                llenaGrid();
            }
            catch (Exception ex)
            {
                lblErrores.Text = "Error al actualizar el producto " + producto + ": " + ex.Message;
            }

        }
        else if (e.CommandName == "Delete")
        {
            string[] valores = e.CommandArgument.ToString().Split(new char[] { ';' });
            producto = valores[0];
            string estatus = valores[1];
            if (estatus == "A")
                SqlDataSource1.DeleteCommand = "update catproductos set estatus='B' where idProducto=@idProducto " +
                    "update articulosalmacen set cantidadExistencia=0 where idArticulo=@idProducto";
            else
                SqlDataSource1.DeleteCommand = "update catproductos set estatus='A' where idProducto=@idProducto";
            SqlDataSource1.DeleteParameters.Add("idProducto", producto);
            try
            {
                SqlDataSource1.Delete();
                grilla.EditIndex = -1;
                grilla.DataBind();
            }
            catch (Exception ex)
            {
                lblErrores.Text = "Error al cambiar de estatus el producto " + producto + ": " + ex.Message;
            }
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            checaSesiones();
            if (usuarioLog != null)
            {

                string tipo = e.Row.RowState.ToString();
                string[] valores = null;
                if (tipo.IndexOf(',') > 0)
                    valores = tipo.Split(new char[] { ',' });
                else
                    valores = new string[] { tipo };
                                
                bool edicion = false;
                foreach (string valor in valores) {
                    if (valor.Trim() == "Edit")
                    {
                        edicion = true;
                        break;
                    }
                }

                if (!edicion)
                {
                    var btnEliminar = e.Row.Cells[9].Controls[1].FindControl("btnEliminar") as Button;
                    string status = DataBinder.Eval(e.Row.DataItem, "estatus").ToString();
                    string producto = DataBinder.Eval(e.Row.DataItem, "idProducto").ToString();

                    CatalogProductos productoCat = new CatalogProductos();
                    productoCat.Producto = producto;
                    productoCat.verificaRelacion();

                    if (productoCat.Relacionado)
                        btnEliminar.Enabled = false;
                    else
                    {
                        object[] existencias = productoCat.obtieneexistenciaTotal();
                        if (Convert.ToBoolean(existencias[0]))
                        {                            
                            if (Convert.ToDecimal(existencias[1]) == 0)
                                btnEliminar.Enabled = true;
                            else
                                btnEliminar.Enabled = false;
                        }
                        else
                            btnEliminar.Enabled = false;

                    }
                    if (status == "A")
                    {
                        btnEliminar.OnClientClick = "return confirm('¿Está seguro de inactivar el producto " + producto + "?')";
                        btnEliminar.Text = "Inactiva";
                        if (productoCat.Relacionado)
                            btnEliminar.CssClass = "btn-default ancho50px";
                        else
                            btnEliminar.CssClass = "btn-danger ancho50px";
                    }
                    else
                    {
                        btnEliminar.OnClientClick = "return confirm('¿Está seguro de reactivar el producto " + producto + "?')";
                        btnEliminar.Text = "Reactiva";
                        if (productoCat.Relacionado)
                            btnEliminar.CssClass = "btn-default ancho50px";
                        else
                            btnEliminar.CssClass = "btn-success ancho50px";
                    }
                }
                else if (edicion) {
                    var ddlCategoriaMod = e.Row.Cells[3].Controls[1].FindControl("ddlCategoriaMod") as DropDownList;
                    string cat = DataBinder.Eval(e.Row.DataItem, "id_categoria").ToString();
                    ddlCategoriaMod.DataBind();
                    ddlCategoriaMod.SelectedValue = cat;
                }
            }
            else
                Response.Redirect("Default.aspx");
        }
    }

    protected void btnAgregar_Click(object sender, EventArgs e)
    {
        lblErrores.Text = "";

        CatalogProductos productoCat = new CatalogProductos();
        productoCat.Producto = txtClave.Text.ToUpper();
        productoCat.verificaExiste();
        bool existeUsuario = productoCat.Existe;
        if (!existeUsuario)
        {
            decimal precio = -1;
            if (txtPrecioVenta.Text == "")
                precio = 0;
            else
            {
                try { precio = Convert.ToDecimal(txtPrecioVenta.Text); }
                catch (Exception)
                {
                    precio = -1;
                }
                if (precio != -1)
                {
                    try
                    {
                        string sql = "insert into catproductos(idProducto, descripcion, idMedida, idFamilia, idLinea, detalles, observaciones, estatus, porCosto, id_categoria, granel,IVA) values(@clave, @descripcion, @idMedida, @idFamilia, @idLinea, @detalles, @observaciones, 'A', 0, @id_categoria, @granel,@IVA) ";
                        Islas tiendas = new Islas();
                        tiendas.obtieneIslas();
                        DataSet infoTiendas = tiendas.IslasAgregar;
                        foreach (DataRow fila in infoTiendas.Tables[0].Rows) {
                            sql = sql + " insert into articulosalmacen select " + fila[0].ToString() + ",idproducto,0,0,1,1,'...',1,null,null,null,null from catproductos where idproducto not in((select idarticulo from articulosalmacen where idalmacen =  " + fila[0].ToString() + ")) ";
                        }

                        SqlDataSource1.InsertCommand = sql;
                        SqlDataSource1.InsertParameters.Add("clave", txtClave.Text.ToUpper());
                        SqlDataSource1.InsertParameters.Add("descripcion", txtDescripcion.Text);
                        SqlDataSource1.InsertParameters.Add("idMedida", ddlUnidad.SelectedValue);
                        SqlDataSource1.InsertParameters.Add("id_categoria", ddlCategoria.SelectedValue);
                        SqlDataSource1.InsertParameters.Add("idFamilia", ddlFamilia.SelectedValue);
                        SqlDataSource1.InsertParameters.Add("idLinea", ddlLinea.SelectedValue);
                        SqlDataSource1.InsertParameters.Add("detalles", txtDetalles.Text);
                        SqlDataSource1.InsertParameters.Add("observaciones", txtObservaciones.Text);
                        string granel = "0";
                        if (chkGranel.Checked)
                            granel = "1";
                        SqlDataSource1.InsertParameters.Add("granel", granel);
                        string IVA = "0";
                        if (chkGranel.Checked)
                            IVA = "1";
                        SqlDataSource1.InsertParameters.Add("IVA", IVA);

                        SqlDataSource1.Insert();
                        Islas islas = new Islas();
                        islas.obtieneIslas();
                        DataSet islasPrecios = new DataSet();
                        islasPrecios = islas.IslasAgregar;

                        try { usuarioLog = Convert.ToString(Request.QueryString["u"]); }
                        catch (Exception) { usuarioLog = ""; }
                        if (usuarioLog != "")
                        {
                            int agregados, noAgregados;
                            agregados = noAgregados = 0;
                            foreach (DataRow fila in islasPrecios.Tables[0].Rows)
                            {
                                int islaReg = Convert.ToInt32(fila[0].ToString());
                                PreciosVenta precioVenta = new PreciosVenta();
                                precioVenta.Producto = txtClave.Text.ToUpper();
                                precioVenta.Precio = precio;
                                precioVenta.Usuario = usuarioLog;
                                precioVenta.Almacen = islaReg;

                                precioVenta.agregaPrecioVenta();
                                if (!precioVenta.Agregado)
                                    noAgregados++;
                                else
                                    agregados++;
                            }
                            lblErrores.Text = "Se agregó el precio de venta a " + agregados.ToString() + " islas de " + (agregados + noAgregados).ToString() + ".";

                            islas.obtieneIslas();
                            DataSet islasAlta = islas.IslasAgregar;
                            foreach (DataRow r in islasAlta.Tables[0].Rows) {
                                islas.agregaAlamacen(r[0].ToString());
                            }

                            llenaGrid();
                            txtClave.Text = txtDescripcion.Text = txtDetalles.Text = txtObservaciones.Text = txtPrecioVenta.Text = "";
                            ddlUnidad.Items.Clear();
                            ddlCategoria.Items.Clear();
                            ddlFamilia.Items.Clear();
                            ddlLinea.Items.Clear();
                            ddlUnidad.DataBind();
                            ddlCategoria.DataBind();
                            ddlFamilia.DataBind();
                            ddlLinea.DataBind();
                            ddlUnidad.SelectedValue = "";
                            ddlCategoria.SelectedIndex = 0;
                            ddlFamilia.SelectedValue = "";
                            ddlLinea.SelectedValue = "";
                        }
                        else {
                            lblErrores.Text = "Su sesión a caducado, por favor vuelva a ingresar.";
                        }
                    }
                    catch (Exception ex)
                    {
                        lblErrores.Text = "Error al agregar el Producto " + txtClave.Text + " - " + txtDescripcion.Text + ": " + ex.Message;
                    }
                }
                else
                    lblErrores.Text = "El precio de venta no es correcto, verifique";
            }
        }
        else
            lblErrores.Text = "El producto a ingresar ya se encuentra registrado";
    }

    protected void lnkProducto_Click(object sender, EventArgs e)
    {
        lblErrores.Text = "";
        LinkButton lnk = (LinkButton)sender;
        string[] valores = lnk.CommandArgument.ToString().Split(new char[] { ';' });
        lblClaveProductoP.Text = valores[0];
        lblDescripProd.Text = valores[1];
        txtVenta.Text = "";
        chkTodos.Checked = false;
        ddlIsla.Enabled = true;
        PanelMask.Visible = true;
        pnlPrecios.Visible = true;
    }
    protected void btnCerrarPrecios_Click(object sender, EventArgs e)
    {
        lblErrores.Text = "";
        lblClaveProductoP.Text = "";
        lblDescripProd.Text = "";
        PanelMask.Visible = false;
        pnlPrecios.Visible = false;
    }
    protected void btnAgregaPrecio_Click(object sender, EventArgs e)
    {
        lblErrorNuevo.Text = "";
        try
        {
            PreciosVenta precio = new PreciosVenta();
            precio.Producto = lblClaveProductoP.Text;
            decimal precioVenta = 0;
            try { 
                precioVenta = Convert.ToDecimal(txtVenta.Text);
                precio.Precio = precioVenta;
                if (!chkTodos.Checked)
                {
                    try { usuarioLog = Convert.ToString(Request.QueryString["u"]); }
                        catch (Exception) { usuarioLog = ""; }
                    if (usuarioLog != "")
                    {
                        precio.Usuario = usuarioLog;
                        precio.Almacen = Convert.ToInt32(ddlIsla.SelectedValue);
                        precio.agregaPrecioVenta();
                        if (precio.Agregado)
                        {
                            CatalogProductos productoCat = new CatalogProductos();
                            productoCat.Producto = lblClaveProductoP.Text.ToUpper();
                            productoCat.actualizaUltimoCosto(Convert.ToInt32(ddlIsla.SelectedValue));


                            txtVenta.Text = "";
                            chkTodos.Checked = false;
                            ddlIsla.Enabled = true;
                            GridView2.SelectedIndex = -1;
                            GridView2.DataBind();
                            GridView3.DataBind();
                        }
                        else
                            lblErrorNuevo.Text = "El precio ingresado ya existe o se produjo un error al intentar agregar el precio";
                    }
                    else 
                        lblErrorNuevo.Text = "Su sesión a caducado por favor vuelva a ingresar";
                }
                else {
                    try { usuarioLog = Convert.ToString(Request.QueryString["u"]); }
                        catch (Exception) { usuarioLog = ""; }
                    if (usuarioLog != "")
                    {
                        DataSet islasProd = new DataSet();
                        Islas islas = new Islas();
                        islas.obtieneIslas();
                        islasProd = islas.IslasAgregar;

                        int agregados, noAgregados;
                        agregados = noAgregados = 0;
                        foreach (DataRow fila in islasProd.Tables[0].Rows)
                        {
                            int islaReg = Convert.ToInt32(fila[0].ToString());
                            precio.Usuario = usuarioLog;
                            precio.Almacen = islaReg;

                            precio.agregaPrecioVenta();
                            if (!precio.Agregado)
                                noAgregados++;
                            else 
                                agregados++;
                            CatalogProductos productoCat = new CatalogProductos();
                            productoCat.Producto = lblClaveProductoP.Text.ToUpper();
                            productoCat.actualizaUltimoCosto(islaReg);
                        }
                        lblErrorNuevo.Text = "Se agregó el precio de venta a " + agregados.ToString() + " Tiendas de " + (agregados + noAgregados).ToString() + ".";
                        txtVenta.Text = "";
                        chkTodos.Checked = false;
                        ddlIsla.Enabled = true;
                        GridView2.SelectedIndex = -1;
                        GridView2.DataBind();
                        GridView3.DataBind();
                    }
                    else {
                        lblErrorNuevo.Text = "Su sesión a caducado por favor vuelva a ingresar";
                    }
                }
            }
            catch (Exception) { lblErrorNuevo.Text = "El precio de venta no es un monto válido"; }            
        }
        catch (Exception ex) {
            lblErrorNuevo.Text = "Error al agregar precio de venta: " + ex.Message;
        }
    }
    protected void chkTodos_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTodos.Checked)
        {
            try { ddlIsla.SelectedIndex = 0; }
            catch (Exception) { ddlIsla.SelectedIndex = -1; }
            ddlIsla.Enabled = false;
        }
        else {
            ddlIsla.Enabled = true;
        }
    }
    protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrores.Text = "";
        GridView3.DataBind();
    }
    protected void GridView2_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        lblErrores.Text = "";
        CatalogProductos productoCat = new CatalogProductos();
        productoCat.Producto = lblClaveProductoP.Text.ToUpper();
        productoCat.actualizaUltimoCosto(Convert.ToInt32(ddlIsla.SelectedValue));
        GridView3.DataBind();
    }

    protected void btnFiltro_Click(object sender, EventArgs e)
    {
        llenaGrid();
    }

    private void llenaGrid()
    {
        lblErrores.Text = "";
        try
        {
            BaseDatos datos = new BaseDatos();
            string filtro = txtFiltro.Text;
            string sql = "select p.idProducto,isnull(p.descripcion,'') as descripcionProducto,isnull(p.idMedida,'') as idMedida," +
                             " isnull(u.descripcion, '') as descripcionMedida,isnull(p.idFamilia, '') as idFamilia,isnull(f.descripcionFamilia, '') as descripcionFamilia," +
                             " isnull(p.idLinea, '') as idLinea,isnull(l.descripcionLinea, '') as descripcionLinea,p.detalles,p.observaciones,p.estatus,cat.descripcion_categoria,cat.id_categoria,isnull(p.IVA,0) as IVA,isnull(p.granel,0) as granel" +
                             " from catproductos p" +
                             " left join catunidmedida u on u.idMedida = p.idMedida" +
                             " left join catfamilias f on f.idFamilia = p.idFamilia" +
                             " left join catlineas l on l.idLinea = p.idLinea" +
                             " left join catcategorias cat on cat.id_categoria=p.id_categoria" +
                             " where (p.idProducto like '%" + filtro + "%' or cat.descripcion_categoria like '%" + filtro + "%' or p.descripcion like '%" + filtro + "%' or p.idfamilia like '%" + filtro + "%' or l.descripcionLinea like '%" + filtro + "%')";
            object[] ejecutado = datos.scalarData(sql);
            if ((bool)ejecutado[0])
            {
                DataSet data = new DataSet();
                data = (DataSet)ejecutado[1];
                GridView1.DataSource = data;               
                GridView1.DataBind();
            }
            else
            {
                GridView1.DataBind();
            }
        }
        catch (Exception ex)
        {
            string cc = ex.Message;
        }
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        llenaGrid();
    }

    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        llenaGrid();
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        llenaGrid();
    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView1.EditIndex = -1;
        llenaGrid();
    }

    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        llenaGrid();
    }
}
