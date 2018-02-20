using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using E_Utilities;

/// <summary>
/// Descripción breve de Producto
/// </summary>
public class Producto
{
    BaseDatos datos = new BaseDatos();
    Fechas fechas = new Fechas();
    string claveProducto;
    decimal ventaPublico;
    string sql;
    int pv;
    bool existeEnIsla;
    string descripcionProd;

	public Producto()
	{
        claveProducto = string.Empty;
        sql = descripcionProd = string.Empty;
        ventaPublico = decimal.Zero;
        pv = 0;
        existeEnIsla = false;
	}

    public string ClaveProducto
    {
        get { return this.claveProducto; }
        set { this.claveProducto = value; }
    }

    public decimal MontoVenta
    {
        get { return this.ventaPublico; }        
    }

    public int Isla {
        set { pv = value; }
    }

    public bool ExistenEnIsla {
        get { return existeEnIsla; }
    }

    public string NombrePorducto
    {        
        set { this.descripcionProd = value; }
        get { return this.descripcionProd; }
    }

    public DataSet llenaConsultaAjusteIslas(int isla, string articulo, string fecha)
    {
        DataSet datas = new DataSet();
        //this.sql = "select a.idAlmacen,ca.nombre,a.idArticulo,p.descripcion ,a.cantidadExistencia,(isnull(a.costoUnitario, 0.00)/1.16) as costoUnitario,isnull(a.cantidadMinima, 0) as cantidadMinima,isnull(a.cantidadMaxima, 0) as cantidadMaxima,((select cast(isnull((select TOP 1 ventaUnitaria from precios_venta where idProducto=a.idArticulo and idAlmacen=a.idAlmacen order by fecha DESC,ventaUnitaria desc),0) as decimal(12,2)))/1.16)as ventaUnitaria,(cast(a.idAlmacen as varchar) + ';' + cast(a.idArticulo as varchar)) as idsAlmaArt,((isnull(a.costoUnitario, 0.00)/1.16)*a.cantidadExistencia) as totalCostoUnitario,(((select cast(isnull((select TOP 1 ventaUnitaria from precios_venta where idProducto=a.idArticulo and idAlmacen=a.idAlmacen order by fecha DESC,ventaUnitaria desc),0) as decimal(12,2)))/1.16) * a.cantidadExistencia) as totalVenta,((((select cast(isnull((select TOP 1 ventaUnitaria from precios_venta where idProducto=a.idArticulo and idAlmacen=a.idAlmacen order by fecha DESC,ventaUnitaria desc),0) as decimal(12,2)))/1.16) * a.cantidadExistencia)-((isnull(a.costoUnitario, 0.00)/1.16)*a.cantidadExistencia)) as Utilidad from articulosalmacen a inner join catalmacenes ca on ca.idAlmacen = a.idAlmacen left join catproductos p on p.idProducto=a.idArticulo where a.idAlmacen=" + isla.ToString() + " and (a.idArticulo like '%" + articulo + "%'  OR P.descripcion like '%" + articulo + "%')";
        /*this.sql = "select tabla.idalmacen,tabla.idarticulo,tabla.descripcion,sum(tabla.existencia_ini) as existencia_ini,sum(tabla.existencia_fin) as existencia_fin,sum(tabla.valor_modificado) as valor_modificado from(" +
"select b.idalmacen,b.idarticulo,c.descripcion,b.id_consecutivo,b.existencia_ini,b.existencia_fin,b.valor_modificado "+
"from bitacora_ajuste b inner join catproductos c on c.idproducto = b.idarticulo where b.fecha in((select top 1 bb.fecha from bitacora_ajuste bb where bb.idarticulo = b.idarticulo and bb.idalmacen = b.idalmacen order by bb.id_consecutivo desc)) and b.existencia_fin <> 0 " +
"and b.fecha in((select top 1 bb.fecha from bitacora_ajuste bb where bb.idalmacen = b.idalmacen order by bb.id_consecutivo desc))) as tabla where tabla.idalmacen = " + isla + " and tabla.descripcion like'%" + articulo + "%' or tabla.idarticulo like '%" + articulo + "%' group by tabla.idalmacen,tabla.idarticulo,tabla.descripcion order by tabla.idarticulo ";
        */
        //sql = "select b.idalmacen,b.idarticulo,c.descripcion,b.id_consecutivo,b.existencia_ini,b.existencia_fin,(b.existencia_fin-b.existencia_ini) as valor_modificado,(B.EXISTENCIA_INI*p.ventaunitaria) as inicial,(b.existencia_fin*p.ventaunitaria) as final,(((b.existencia_fin - b.existencia_ini)) * p.ventaunitaria) as UTILIDAD from bitacora_ajuste b inner join catproductos c on b.idarticulo = c.idproducto inner join articulosalmacen a on a.idalmacen=b.idalmacen and a.idarticulo=b.idarticulo left join precios_venta p on p.idproducto = b.idarticulo and p.idalmacen = b.idalmacen and a.idpreciopublico = p.idpreciopublico where b.idalmacen = " + isla + " and b.fecha='" + fecha + "'  and b.existencia_fin <> 0 and(c.descripcion like '%" + articulo + "%' or b.idarticulo like '%" + articulo + "%')";
        sql = "SELECT * FROM (select b.idalmacen,b.idarticulo,c.descripcion,b.id_consecutivo,b.existencia_ini,b.existencia_fin,(b.existencia_fin - b.existencia_ini) as valor_modificado,(B.EXISTENCIA_INI * p.ventaunitaria) as inicial,(b.existencia_fin * p.ventaunitaria) as final,(((b.existencia_fin - b.existencia_ini)) * p.ventaunitaria) as UTILIDAD,b.usuario from bitacora_ajuste b inner join catproductos c on b.idarticulo = c.idproducto inner join articulosalmacen a on a.idalmacen = b.idalmacen and a.idarticulo = b.idarticulo left join precios_venta p on p.idproducto = b.idarticulo and p.idalmacen = b.idalmacen and a.idpreciopublico = p.idpreciopublico where b.idalmacen = " + isla + " and b.fecha = '" + fecha + "'  and b.existencia_fin <> 0 and(c.descripcion like '%" + articulo + "%' or b.idarticulo like '%" + articulo + "%') UNION ALL " +
            "select a.idalmacen,a.idarticulo,c.descripcion,1 as id_consecutivo,a.cantidadExistencia as existencia_ini, a.cantidadExistencia as existencia_fin, 0 as valor_modificado,(a.cantidadExistencia * p.ventaunitaria) as inicial,(a.cantidadExistencia * p.ventaunitaria) as final,0 as UTILIDAD,'' as usuario from articulosalmacen a inner join catproductos c on a.idarticulo = c.idproducto left join precios_venta p on p.idproducto = a.idarticulo and p.idalmacen = a.idalmacen and a.idpreciopublico = p.idpreciopublico where a.idalmacen = " + isla + " and(c.descripcion like '%" + articulo + "%' or a.idarticulo like '%" + articulo + "%') and a.idarticulo not in(select b.idarticulo from bitacora_ajuste b inner join catproductos c on b.idarticulo = c.idproducto where b.idalmacen = a.idalmacen and b.fecha = '" + fecha + "'  and b.existencia_fin <> 0 and(c.descripcion like '%" + articulo + "%' or b.idarticulo like '%" + articulo + "%'))) AS TABLA ORDER BY TABLA.idalmacen, tabla.idarticulo";
        object[] ejecutado = datos.scalarData(sql);
        if ((bool)ejecutado[0])
            datas = (DataSet)ejecutado[1];
        else
            datas = null;
        return datas;
    }

    public object[] ajustaEntrada(string tienda, string usuario, string productoAnt, string productoNuevo, string cantidad, string proceso, int entrada, int detalle)
    {
        sql = "declare @punto as int declare @entrada as int declare @detalle  as int declare @producto as varchar(30) declare @existencia as decimal(15,3) declare @existenciaReal as decimal(15,3) declare @cantEntrada as decimal(15,3) declare @cantEntradaAnt as decimal(15,3) " +
"declare @subtotal  as decimal(15,2) declare @importe  as decimal(15,2) declare @total  as decimal(15,2) declare @impuesto  as decimal(15,2) declare @precioUnit  as decimal(15,2) declare @totProd as decimal(15,3) " +
"set @punto = " + tienda + " set @entrada = " + entrada + " set @detalle = " + detalle + " set @producto = '" + productoAnt + "' set @cantEntrada = " + cantidad + " " +
"set @existencia = (select isnull((select cantidadExistencia from articulosalmacen where idAlmacen = @punto and idArticulo = @producto),0)) " +
"set @cantEntradaAnt = (select isnull((select entProdCant from entinventariodet where entAlmacenID = @punto and entFolioID = @entrada and entDetID = @detalle),0)) " +
"set @precioUnit = (select isnull((select entProdCostoUnit from entinventariodet where entAlmacenID = @punto and entFolioID = @entrada and entDetID = @detalle),0)) " +
"set @existenciaReal = @existencia - @cantEntradaAnt + @cantEntrada " +
"set @importe = @precioUnit * @cantEntrada " +
"set @subtotal = (select isnull((select SUM(entImporte) from entinventariodet where entAlmacenID = @punto and entFolioID = @entrada and entDetID <> @detalle),0))+@importe " +
"set @totProd = (select isnull((select SUM(entProdCant) from entinventariodet where entAlmacenID = @punto and entFolioID = @entrada and entDetID <> @detalle),0))+@cantEntrada " +
"set @impuesto = @subtotal * 0.16 " +
"set @total = @subtotal + @impuesto " +
"update entinventariodet set entProdCant = @cantEntrada, entImporte = @importe where entAlmacenID = @punto and entFolioID = @entrada and entDetID = @detalle " +
"update entinventarioenc set entSubtotal = @subtotal, entImpuesto = @impuesto, entTotal = @total, entSumaProductos = @totProd where entAlmacenID = @punto and entFolioID = @entrada " +
"update articulosalmacen set cantidadExistencia = @existenciaReal where idAlmacen = @punto and idArticulo = @producto " +
"insert into bitacora_movimientos values(@punto,(select isnull((select top 1 consecutivo from bitacora_movimientos where idalmacen = @punto order by consecutivo desc), 0)+1),@producto,@producto,@existencia,@existenciaReal,@cantEntrada,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "','" + usuario + "','" + proceso + "') ";
        return datos.insertUpdateDelete(sql);
    }

    public object[] cambiaCodigos(string tienda, string usuario, string productoCorrecto, string productoErrone, string proceso)
    {
        sql = "declare @existProdCorr decimal(8,3) declare @existProdErr decimal(8, 3) " +
            "set @existProdCorr = (select cantidadExistencia from articulosalmacen where idarticulo = '" + productoCorrecto + "' and idalmacen = " + tienda + ") " +
            "set @existProdErr = (select cantidadExistencia from articulosalmacen where idarticulo = '" + productoErrone + "' and idalmacen = " + tienda + ") " +
            "update articulosalmacen set cantidadexistencia = 0 where idarticulo = '" + productoErrone + "' and idalmacen = " + tienda +
            " update articulosalmacen set cantidadexistencia = @existProdCorr + @existProdErr where idarticulo = '" + productoCorrecto + "' and idalmacen = " + tienda +
            " insert into bitacora_movimientos values(" + tienda + ",(select isnull((select top 1 consecutivo from bitacora_movimientos where idalmacen=" + tienda + " order by consecutivo desc),0)+1),'" + productoErrone + "','" + productoCorrecto + "',@existProdErr,@existProdCorr,@existProdErr,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "','" + usuario + "','" + proceso + "')";
        return datos.insertUpdateDelete(sql);
    }

    public object[] cambiaInventario(string tienda, string usuario, string productoAnt, string productoNuevo, string cantidad, string proceso, int entrada, int detalle)
    {
        sql = "declare @existProdAnt decimal(12,3) declare @existProdNuevo decimal(12, 3) declare @cantidad decimal(12, 3) declare @cantidadEnt decimal(12,3) declare @importeEnt decimal(12,2) " +
"set @existProdAnt = (select cantidadExistencia from articulosalmacen where idarticulo = '" + productoAnt + "' and idalmacen = " + tienda + ") " +
"set @existProdNuevo = (select cantidadExistencia from articulosalmacen where idarticulo = '" + productoNuevo + "' and idalmacen = " + tienda + ") " +
"set @cantidad = " + cantidad +
"set @cantidadEnt = (select entProdCant from entinventariodet where entFolioId=" + entrada + " and entDetID=" + detalle + " and entalmacenid=" + tienda + " ) " +
"set @importeEnt = (select entProdCostoUnit from entinventariodet where entFolioId=" + entrada + " and entDetID=" + detalle + " and entalmacenid=" + tienda + " ) " +
"if(@cantidad=@cantidadEnt) begin update entinventariodet set entProductoId='" + productoNuevo + "' where entFolioId=" + entrada + " and entDetID=" + detalle + " and entalmacenid=" + tienda + " end " +
"else begin update entinventariodet set entProdCant=@cantidadEnt-@cantidad,entImporte=((@cantidadEnt-@cantidad)*@importeEnt) where entFolioId=" + entrada + " and entDetID=" + detalle + " and entalmacenid=" + tienda + " end " +
" update articulosalmacen set cantidadexistencia = @existProdAnt - @cantidad where idarticulo = '" + productoAnt + "' and idalmacen = " + tienda +
" update articulosalmacen set cantidadexistencia = @existProdNuevo + @cantidad where idarticulo = '" + productoNuevo + "' and idalmacen = " + tienda +
" insert into bitacora_movimientos values(" + tienda + ",(select isnull((select top 1 consecutivo from bitacora_movimientos where idalmacen=" + tienda + " order by consecutivo desc),0)+1),'" + productoAnt + "','" + productoNuevo + "',@existProdAnt,(@existProdNuevo + @cantidad),@cantidad,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "','" + usuario + "','" + proceso + "')";
        return datos.insertUpdateDelete(sql);
    }

    public DataSet llenaConsultaAjusteIslas2(int isla, string articulo, string fecha, string ajuste)
    {
        DataSet datas = new DataSet();

        sql = "select tabla.idArticulo,tabla.descripcion,tabla.precio,tabla.existenciaIni,(tabla.existenciaIni*tabla.precio) as inicial,tabla.existenciaFin,(tabla.existenciaFin*tabla.precio) as final,(abs(tabla.existenciaFin*tabla.precio)-abs(tabla.existenciaIni*tabla.precio)) as utilidad,tabla.usuario,(abs(tabla.existenciaFin)-abs(tabla.existenciaIni)) as valor_modificado from(" +
"select i.idArticulo,c.descripcion,i.cantidadExistencia as existenciaIni,i.existenciaFin,isnull(i.precio, 0) as precio,i.usuario,i.fecha from inventario_inicial i left join catproductos c on c.idproducto = i.idarticulo " +
"where i.idalmacen = " + isla + " and i.fecha='" + fecha + "') as tabla where(tabla.idarticulo like '%" + articulo + "%' or tabla.descripcion like '%" + articulo + "%')";
        object[] ejecutado = datos.scalarData(sql);
        if ((bool)ejecutado[0])
            datas = (DataSet)ejecutado[1];
        else
            datas = null;
        return datas;
    }

    public DataSet obtieneTotaLES(int isla, string fecha)
    {
        DataSet datas = new DataSet();
        sql = "select sum(isnull(tabla.inicial,0)) as inicial,sum(isnull(tabla.final,0)) as final,sum(tabla.utilidad) as utilidad from(select(b.existencia_ini * p.ventaunitaria) as inicial,(b.existencia_fin * p.ventaunitaria) as final,((b.existencia_fin - b.existencia_ini) * p.ventaunitaria) as utilidad from bitacora_ajuste b inner join catproductos c on b.idarticulo = c.idproducto inner join articulosalmacen a on a.idalmacen = b.idalmacen and a.idarticulo = b.idarticulo left join precios_venta p on p.idproducto = b.idarticulo and p.idalmacen = b.idalmacen and a.idpreciopublico = p.idpreciopublico where b.idalmacen = " + isla + " and b.fecha='" + fecha + "'  and b.existencia_fin <> 0 UNION ALL " +
              "select(a.cantidadExistencia * p.ventaunitaria) as inicial,(a.cantidadExistencia * p.ventaunitaria) as final,0 as UTILIDAD from articulosalmacen a inner join catproductos c on a.idarticulo = c.idproducto left join precios_venta p on p.idproducto = a.idarticulo and p.idalmacen = a.idalmacen and a.idpreciopublico = p.idpreciopublico where a.idalmacen = " + isla + " and a.idarticulo not in(select b.idarticulo from bitacora_ajuste b inner join catproductos c on b.idarticulo = c.idproducto where b.idalmacen = a.idalmacen and b.fecha = '" + fecha + "'  and b.existencia_fin <> 0 )) as tabla";
        object[] ejecutado = datos.scalarData(sql);
        if ((bool)ejecutado[0])
            datas = (DataSet)ejecutado[1];
        else
            datas = null;
        return datas;
    }

    public DataSet obtieneTotales2(int isla, string fecha, string ajuste)
    {
        DataSet datas = new DataSet();
        sql = "select sum((tabla.existenciaIni*tabla.precio)) as inicial,sum((tabla.existenciaFin*tabla.precio)) as final,sum(abs(tabla.existenciaFin*tabla.precio)-(tabla.existenciaIni*tabla.precio)) as utilidad from(" +
"select i.idArticulo,c.descripcion,i.cantidadExistencia as existenciaIni,i.existenciaFin,isnull(i.precio, 0) as precio,i.usuario,i.fecha from inventario_inicial i left join catproductos c on c.idproducto = i.idarticulo " +
"where i.idalmacen = " + isla + " and i.fecha='" + fecha + "') as tabla";
        object[] ejecutado = datos.scalarData(sql);
        if ((bool)ejecutado[0])
            datas = (DataSet)ejecutado[1];
        else
            datas = null;
        return datas;
    }

    public object[] obtieneProducto()
    {
        this.sql = string.Format("Select descripcion from catproductos where idproducto='{0}'", this.claveProducto);
        return datos.scalarString(this.sql);
    }
    public object[] hayExistencia(int almacen,string articulo)
    {
        this.sql = string.Format("select count(*) from articulosalmacen where idAlmacen="+ almacen +" and idArticulo='"+ articulo +"' and cantidadExistencia>0");
        return datos.scalarString(this.sql);
    }

    public object[] obtieneIdProducto()
    {
        this.sql = string.Format("Select idProducto from catproductos where descripcion like '%{0}%'", this.descripcionProd);
        return datos.scalarString(this.sql);
    }

    public object[] obtienePrecioVenta()
    {
        this.sql = string.Format("select cast(isnull((select TOP 1 ventaUnitaria from precios_venta where idProducto='{0}' and idAlmacen={1} order by fecha DESC,ventaUnitaria desc),0) as decimal(12,2))", this.claveProducto, this.pv);
        return datos.scalarToDecimal(this.sql);
    }

    public decimal obtieneExistencia(string idProducto, string isla)
    {
        string query = "select cantidadExistencia from articulosalmacen where idAlmacen=" + isla + " and idArticulo='" + idProducto + "'";
        object[] valores = datos.scalarToDecimal(query);
        if (Convert.ToBoolean(valores[0]))
        {
            return Convert.ToDecimal(valores[1]);
        }
        else
            return 0;
    }

    public bool descuentaInventario(string idProducto, string isla, int existenciaReal)
    {
        string query = "update articulosalmacen set cantidadExistencia=" + existenciaReal.ToString() + " where idAlmacen=" + isla + " and idArticulo='" + idProducto + "'";
        object[] valores = datos.insertUpdateDelete(query);
        if (Convert.ToBoolean(valores[0]))
        {
            return Convert.ToBoolean(valores[1]);
        }
        else
            return false;
    }

    public bool actualizaDetalleEntrada(string sql)
    {
        bool actualizado = false;
        object[] ejecutado = datos.insertUpdateDelete(sql);
        if ((bool)ejecutado[0])
            actualizado = (bool)ejecutado[1];
        else
            actualizado = false;
        return actualizado;
    }

    public DataSet llenaConsultaAjusteIsla(int isla, string articulo)
    {
        DataSet datas = new DataSet();
        //this.sql = "select a.idAlmacen,ca.nombre,a.idArticulo,p.descripcion ,a.cantidadExistencia,(isnull(a.costoUnitario, 0.00)/1.16) as costoUnitario,isnull(a.cantidadMinima, 0) as cantidadMinima,isnull(a.cantidadMaxima, 0) as cantidadMaxima,((select cast(isnull((select TOP 1 ventaUnitaria from precios_venta where idProducto=a.idArticulo and idAlmacen=a.idAlmacen order by fecha DESC,ventaUnitaria desc),0) as decimal(12,2)))/1.16)as ventaUnitaria,(cast(a.idAlmacen as varchar) + ';' + cast(a.idArticulo as varchar)) as idsAlmaArt,((isnull(a.costoUnitario, 0.00)/1.16)*a.cantidadExistencia) as totalCostoUnitario,(((select cast(isnull((select TOP 1 ventaUnitaria from precios_venta where idProducto=a.idArticulo and idAlmacen=a.idAlmacen order by fecha DESC,ventaUnitaria desc),0) as decimal(12,2)))/1.16) * a.cantidadExistencia) as totalVenta,((((select cast(isnull((select TOP 1 ventaUnitaria from precios_venta where idProducto=a.idArticulo and idAlmacen=a.idAlmacen order by fecha DESC,ventaUnitaria desc),0) as decimal(12,2)))/1.16) * a.cantidadExistencia)-((isnull(a.costoUnitario, 0.00)/1.16)*a.cantidadExistencia)) as Utilidad from articulosalmacen a inner join catalmacenes ca on ca.idAlmacen = a.idAlmacen left join catproductos p on p.idProducto=a.idArticulo where a.idAlmacen=" + isla.ToString() + " and (a.idArticulo like '%" + articulo + "%'  OR P.descripcion like '%" + articulo + "%')";
        this.sql = "select a.idAlmacen,ca.nombre,a.idArticulo,p.descripcion ,isnull((case when a.cantidadExistencia=0 then (select top 1 cantidadExistencia from inventario_inicial where idalmacen=a.idalmacen and idarticulo=a.idarticulo order by consecutivo desc) else a.cantidadExistencia end),0) as cantidadExistencia,(isnull(a.costoUnitario, 0.00)) as costoUnitario,isnull(a.cantidadMinima, 0) as cantidadMinima,isnull(a.cantidadMaxima, 0) as cantidadMaxima,((select cast(isnull((select ventaUnitaria from precios_venta where idProducto=a.idArticulo and idAlmacen=a.idAlmacen and idPrecioPublico=a.idPrecioPublico),0) as decimal(12,2))))as ventaUnitaria,(cast(a.idAlmacen as varchar) + ';' + cast(a.idArticulo as varchar)) as idsAlmaArt,((isnull(a.costoUnitario, 0.00))*a.cantidadExistencia) as totalCostoUnitario,(((select cast(isnull((select ventaUnitaria from precios_venta where idProducto=a.idArticulo and idAlmacen=a.idAlmacen and idPrecioPublico=a.idPrecioPublico),0) as decimal(12,2)))) * a.cantidadExistencia) as totalVenta,((((select cast(isnull((select ventaUnitaria from precios_venta where idProducto=a.idArticulo and idAlmacen=a.idAlmacen and idPrecioPublico=a.idPrecioPublico),0) as decimal(12,2)))) * a.cantidadExistencia)-((isnull(a.costoUnitario, 0.00))*a.cantidadExistencia)) as Utilidad from articulosalmacen a left join catalmacenes ca on ca.idAlmacen = a.idAlmacen left join catproductos p on p.idProducto=a.idArticulo where a.idAlmacen=" + isla.ToString() + " and (ltrim(rtrim(a.idArticulo)) like '%" + articulo + "%'  OR ltrim(rtrim(P.descripcion)) like '%" + articulo + "%')";
        object[] ejecutado = datos.scalarData(sql);
        if ((bool)ejecutado[0])
            datas = (DataSet)ejecutado[1];
        else
            datas = null;
        return datas;
    }

    public DataSet llenaConsultaInventarioIsla(int isla, string articulo)
    {
        DataSet datas = new DataSet();
        //this.sql = "select a.idAlmacen,ca.nombre,a.idArticulo,p.descripcion ,a.cantidadExistencia,(isnull(a.costoUnitario, 0.00)/1.16) as costoUnitario,isnull(a.cantidadMinima, 0) as cantidadMinima,isnull(a.cantidadMaxima, 0) as cantidadMaxima,((select cast(isnull((select TOP 1 ventaUnitaria from precios_venta where idProducto=a.idArticulo and idAlmacen=a.idAlmacen order by fecha DESC,ventaUnitaria desc),0) as decimal(12,2)))/1.16)as ventaUnitaria,(cast(a.idAlmacen as varchar) + ';' + cast(a.idArticulo as varchar)) as idsAlmaArt,((isnull(a.costoUnitario, 0.00)/1.16)*a.cantidadExistencia) as totalCostoUnitario,(((select cast(isnull((select TOP 1 ventaUnitaria from precios_venta where idProducto=a.idArticulo and idAlmacen=a.idAlmacen order by fecha DESC,ventaUnitaria desc),0) as decimal(12,2)))/1.16) * a.cantidadExistencia) as totalVenta,((((select cast(isnull((select TOP 1 ventaUnitaria from precios_venta where idProducto=a.idArticulo and idAlmacen=a.idAlmacen order by fecha DESC,ventaUnitaria desc),0) as decimal(12,2)))/1.16) * a.cantidadExistencia)-((isnull(a.costoUnitario, 0.00)/1.16)*a.cantidadExistencia)) as Utilidad from articulosalmacen a inner join catalmacenes ca on ca.idAlmacen = a.idAlmacen left join catproductos p on p.idProducto=a.idArticulo where a.idAlmacen=" + isla.ToString() + " and (a.idArticulo like '%" + articulo + "%'  OR P.descripcion like '%" + articulo + "%')";
        this.sql = "select a.idAlmacen,ca.nombre,a.idArticulo,p.descripcion ,a.cantidadExistencia,(isnull(a.costoUnitario, 0.00)) as costoUnitario,isnull(a.cantidadMinima, 0) as cantidadMinima,isnull(a.cantidadMaxima, 0) as cantidadMaxima,((select cast(isnull((select ventaUnitaria from precios_venta where idProducto=a.idArticulo and idAlmacen=a.idAlmacen and idPrecioPublico=a.idPrecioPublico),0) as decimal(12,2))))as ventaUnitaria,(cast(a.idAlmacen as varchar) + ';' + cast(a.idArticulo as varchar)) as idsAlmaArt,((isnull(a.costoUnitario, 0.00))*a.cantidadExistencia) as totalCostoUnitario,(((select cast(isnull((select ventaUnitaria from precios_venta where idProducto=a.idArticulo and idAlmacen=a.idAlmacen and idPrecioPublico=a.idPrecioPublico),0) as decimal(12,2)))) * a.cantidadExistencia) as totalVenta,((((select cast(isnull((select ventaUnitaria from precios_venta where idProducto=a.idArticulo and idAlmacen=a.idAlmacen and idPrecioPublico=a.idPrecioPublico),0) as decimal(12,2)))) * a.cantidadExistencia)-((isnull(a.costoUnitario, 0.00))*a.cantidadExistencia)) as Utilidad from articulosalmacen a left join catalmacenes ca on ca.idAlmacen = a.idAlmacen left join catproductos p on p.idProducto=a.idArticulo where a.idAlmacen=" + isla.ToString() + " and (ltrim(rtrim(a.idArticulo)) like '%" + articulo + "%'  OR ltrim(rtrim(P.descripcion)) like '%" + articulo + "%')";
        object[] ejecutado = datos.scalarData(sql);
        if ((bool)ejecutado[0])
            datas = (DataSet)ejecutado[1];
        else
            datas = null;
        return datas;
    }

    public bool actualizaMinMax(string idIsla, string idArticulo, decimal min, decimal max)
    {
        bool actualziado = false;
        string sql = "update articulosalmacen set cantidadMinima = " + min.ToString() + " , cantidadMaxima = " + max.ToString() + " where idAlmacen = " + idIsla.ToString() + " and idArticulo = '" + idArticulo + "'";
        object[] ejecutado = datos.insertUpdateDelete(sql);
        if ((bool)ejecutado[0])
            actualziado = (bool)ejecutado[1];
        else
            actualziado = false;
        return actualziado;
    }

    public object[] obtieneConteos(string isla, string idArticulo)
    {
        BaseDatos ejecuta = new BaseDatos();
        sql = "select valor_modificado from bitacora_ajuste where idalmacen=" + isla + " and idarticulo='" + idArticulo + "' and concluido=0 and existencia_fin=0";
        return ejecuta.scalarData(sql);
    }

    public void existeIsla()
    {
        BaseDatos data = new BaseDatos();
        object[] datos = new object[2];
        string sql = string.Format("select count(*) from articulosalmacen where idArticulo='{0}' and idAlmacen={1} and cantidadExistencia>0", claveProducto, pv);
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[1]))
            existeEnIsla = Convert.ToBoolean(datos[1]);
        else
            existeEnIsla = false;
    }

    public bool actualizaEncabezado(int folioId)
    {
        bool actualziado = false;
        string sql = "select isnull(sum(entImporte),0) from entinventariodet where entFolioID=" + folioId.ToString();
        BaseDatos data = new BaseDatos();
        object[] datos = new object[2];
        datos = data.scalarToDecimal(sql);
        if (Convert.ToBoolean(datos[0]))
        {
            decimal subtotal = Convert.ToDecimal(datos[1]);
            decimal iva = subtotal * Convert.ToDecimal(0.16);
            decimal total = subtotal + iva;
            sql = "update entinventarioenc set entSubtotal = " + subtotal.ToString() + " , entImpuesto = " + iva.ToString() + ", entTotal=" + total.ToString() + " where entFolioID = " + folioId.ToString();
            object[] ejecutado = data.insertUpdateDelete(sql);
            if ((bool)ejecutado[0])
                actualziado = (bool)ejecutado[1];
            else
                actualziado = false;
        }
        else
            actualziado = false;
        return actualziado;
    }

    public object[] vendeAgranel()
    {
        BaseDatos data = new BaseDatos();        
        string sql = string.Format("select granel from catproductos where ltrim(rtrim(idProducto))=ltrim(rtrim('{0}'))", claveProducto);
        return data.intToBool(sql);        
    }

    public object[] obtieneInfo()
    {
        BaseDatos data = new BaseDatos();
        string sql = "select tabla.idarticulo, tabla.descripcion,tabla.idprecio,tabla.precio from(" +
"select aa.idarticulo,c.descripcion,(select top 1 idPrecioPublico from precios_venta where idProducto=aa.idArticulo and idAlmacen=aa.idAlmacen order by idPrecioPublico desc) as idprecio," +
"(select top 1 ventaUnitaria from precios_venta where idProducto=aa.idArticulo and idAlmacen=aa.idAlmacen order by idPrecioPublico desc) as precio " +
"from articulosalmacen aa inner join catproductos c on c.idproducto=aa.idarticulo where aa.idalmacen=" + pv.ToString() + ") as tabla " +
"where tabla.idArticulo like '%" + descripcionProd + "%' or tabla.descripcion like '%" + descripcionProd + "%' or tabla.precio like '%" + descripcionProd + "%'";
        return data.scalarData(sql);
    }

    public object[] productosTerminarInventario(string isla, string usuario, string fecha)
    {
        BaseDatos data = new BaseDatos();
        string sql = "select idarticulo,case existenciaFin when 0 then 0 else existenciaFin end as existenciareal from inventario_inicial where idalmacen=" + isla + " and isnull(terminado,0)=0 and fecha='" + fecha + "'";
        return data.scalarData(sql);
    }

    public object[] actuializaInventario(string isla, string producto, decimal existencia, string fecha)
    {
        BaseDatos data = new BaseDatos();
        string sql = "update articulosalmacen set cantidadExistencia=" + existencia.ToString("F3") + " where idAlmacen=" + isla + " and idArticulo='" + producto + "' update Inventario_Inicial set terminado = 1 where idAlmacen = " + isla + " and idArticulo = '" + producto + "' and fecha = '" + fecha + "' and isnull(terminado,0)= 0";
        return data.insertUpdateDelete(sql);
    }

    public object[] obtieneUltimoCosto()
    {
        BaseDatos data = new BaseDatos();
        string sql = "select isnull((select costoUnitario from articulosalmacen where idAlmacen=" + pv.ToString() + " and idArticulo='" + claveProducto + "'),0)";
        return data.scalarToDecimal(sql);
    }

    public void obtienePrecioVtaActivo()
    {
        BaseDatos data = new BaseDatos();
        string sqlPrcVta = "select isnull((select p.ventaunitaria from precios_venta p left join articulosalmacen a on a.idalmacen=p.idalmacen and a.idarticulo=p.idproducto where p.idalmacen=" + pv + " and p.idproducto='" + claveProducto + "' and p.idpreciopublico=a.idpreciopublico),0)";
        object[] obj = data.scalarToDecimal(sqlPrcVta);
        ventaPublico = decimal.Parse(obj[1].ToString());
    }

    public object[] actualizaExistencia(decimal existenciaFinal, string idAlmacen, string idArticulo, decimal existenciaIni, decimal valorMod, string usuario)
    {
        sql = "update articulosalmacen set cantidadExistencia=(select cantidadexistencia+" + valorMod.ToString() + " from articulosalmacen where idalmacen=" + idAlmacen.ToString() + " and idarticulo='" + idArticulo + "') where idalmacen=" + idAlmacen + " and idarticulo='" + idArticulo + "' " +
              " " +
              "insert into bitacora_ajuste values(" + idAlmacen + ",'" + idArticulo + "'," +
              "(select isnull((select top 1 id_consecutivo from bitacora_ajuste where idalmacen=" + idAlmacen + " and idArticulo ='" + idArticulo + "' order by id_consecutivo desc), 0) + 1)," +
              existenciaIni.ToString() + "," + existenciaFinal.ToString() + "," + valorMod.ToString() + ",'" + usuario + "','" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "')";
        return datos.insertUpdateDelete(sql);
    }

    public object[] generaHistorico(string isla, string clave, decimal ajuste, string usuario)
    {
        sql = "declare @existencia decimal(15,3) set @existencia = (select cantidadExistencia from articulosalmacen where idAlmacen="+isla+" and idarticulo='"+clave+"') "+
              "insert into bitacora_ajuste values(" + isla + ",'" + clave + "'," +
              "(select isnull((select top 1 id_consecutivo from bitacora_ajuste where idalmacen=" + isla + " and idArticulo ='" + clave + "' order by id_consecutivo desc), 0) + 1),@existencia,0," + ajuste.ToString("F3") + ",'" + usuario + "','" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "',0)";
        return datos.insertUpdateDelete(sql);
    }

    public object[] obtieneDetalleInventario(string articulo, string idAlmacen)
    {
        string sql = "select isnull(( " +
                "select top 1 existencia_fin from bitacora_ajuste where idarticulo = '" + articulo + "' and idalmacen = " + idAlmacen + " order by id_consecutivo desc " +
                "),0)as ajuste, " +
                "isnull((select sum(vd.entprodcant) " +
                "from entinventariodet vd " +
                "left join entinventarioenc ve on ve.entfolioid = vd.entfolioid " +
                "where vd.entproductoid = '" + articulo + "' and vd.entalmacenid = " + idAlmacen + " " +
                "and ve.entfechadoc >= ((select top 1 fecha from bitacora_ajuste where idarticulo = '" + articulo + "' and idalmacen = " + idAlmacen + " order by id_consecutivo desc)) " +
                "),0)as entrada, " +
                "isnull((select sum(vd.cantidad) " +
                "from venta_det vd " +
                "inner join venta_enc ve on ve.ticket = vd.ticket " +
                "where vd.id_refaccion = '" + articulo + "' and vd.id_almacen = " + idAlmacen + " and ve.fecha_venta >= ((select top 1 fecha from bitacora_ajuste where idarticulo = '" + articulo + "' and idalmacen = " + idAlmacen + " order by id_consecutivo desc))),0)as venta";
        return datos.scalarData(sql);
    }

    public object[] concluyeArticulo(string isla, string clave, string usuario)
    {
        sql = "declare @existencia decimal(15,3) declare @ajuste decimal(15,3) set @existencia = (select cantidadExistencia from articulosalmacen where idAlmacen=" + isla + " and idarticulo='" + clave + "') " +
            "set @ajuste = (select isnull((select sum(valor_modificado) from bitacora_ajuste where idalmacen=" + isla + " and idarticulo='" + clave + "' and concluido=0 and existencia_fin=0),0)) " +
            "update bitacora_ajuste set concluido=1 where idalmacen=" + isla + " and idarticulo='" + clave + "' and concluido=0 and existencia_fin=0 " +
            "update articulosalmacen set cantidadExistencia=@ajuste where idalmacen=" + isla + " and idarticulo='" + clave + "' " +
            "update inventario_inicial set existenciaFin=@ajuste,usuario='" + usuario.ToUpper() + "' where idalmacen=" + isla + " and idarticulo='" + clave + "' and fecha ='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "' and terminado=0 " +
             "insert into bitacora_ajuste values(" + isla + ",'" + clave + "'," +
             "(select isnull((select top 1 id_consecutivo from bitacora_ajuste where idalmacen=" + isla + " and idArticulo ='" + clave + "' order by id_consecutivo desc), 0) + 1),@existencia,@ajuste,(@ajuste-@existencia),'" + usuario + "','" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "',1)";
        return datos.insertUpdateDelete(sql);
    }

    /* public object[] concluirTodo(string isla, string usuario)
    {
        BaseDatos ejecuta = new BaseDatos();
        object[] retono = new object[] { false, "" };
        sql = "select distinct idArticulo from bitacora_ajuste where concluido = 0 and existencia_fin = 0 and idalmacen = " + isla;
        object[] articulos = ejecuta.scalarData(sql);
        if (Convert.ToBoolean(articulos[0]))
        {
            DataSet infoArt = (DataSet)articulos[1];
            int articulosAct = 0;
            foreach(DataRow r in infoArt.Tables[0].Rows)
            {
                string clave = r[0].ToString();
                sql = "declare @existencia decimal(15,3) declare @ajuste decimal(15,3) set @existencia = (select cantidadExistencia from articulosalmacen where idAlmacen=" + isla + " and idarticulo='" + clave + "') " +
           "set @ajuste = (select isnull((select sum(valor_modificado) from bitacora_ajuste where idalmacen=" + isla + " and idarticulo='" + clave + "' and concluido=0 and existencia_fin=0),0)) " +
           "update bitacora_ajuste set concluido=1 where idalmacen=" + isla + " and idarticulo='" + clave + "' and concluido=0 and existencia_fin=0 " +
           "update articulosalmacen set cantidadExistencia=@ajuste where idalmacen=" + isla + " and idarticulo='" + clave + "' " +
            "insert into bitacora_ajuste values(" + isla + ",'" + clave + "'," +
            "(select isnull((select top 1 id_consecutivo from bitacora_ajuste where idalmacen=" + isla + " and idArticulo ='" + clave + "' order by id_consecutivo desc), 0) + 1),@existencia,@ajuste,(@ajuste-@existencia),'" + usuario + "','" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "',1)";
                object[] actualizado = ejecuta.insertUpdateDelete(sql);
                if (Convert.ToBoolean(actualizado[0]))
                    articulosAct++;
            }
            retono = new object[] { true, articulosAct };
        }
        else {
            retono = articulos;
        }
        return retono;
    }*/

    public object[] existeInventarioInicialDia(string isla, string fecha)
    {
        sql = "select count(*) from inventario_inicial where idalmacen=" + isla + " and fecha='" + fecha + "' and terminado=0";
        return datos.intToBool(sql);
    }

    public object[] iniciaInventario(string isla)
    {
        sql = "insert into inventario_inicial select a.idAlmacen,a.idArticulo,(select isnull((select top 1 i.consecutivo from Inventario_inicial i where i.idalmacen=a.idalmacen and i.idArticulo=a.idarticulo order by i.consecutivo desc),0)+1) as consecutivo,a.cantidadExistencia,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "',0,isnull((SELECT ventaUnitaria FROM precios_venta P WHERE p.idproducto=a.idarticulo and p.idalmacen=a.idalmacen and p.idpreciopublico=a.idpreciopublico),0) as precio,'',0 from articulosalmacen a where a.idalmacen=" + isla + " update articulosalmacen set cantidadExistencia = 0 where idalmacen = " + isla;
        return datos.insertUpdateDelete(sql);
    }

    public object[] concluirTodo(string isla, string usuario)
    {
        BaseDatos ejecuta = new BaseDatos();
        object[] retono = new object[] { false, "" };
        sql = "select distinct idArticulo from bitacora_ajuste where concluido = 0 and existencia_fin = 0 and idalmacen = " + isla;
        object[] articulos = ejecuta.scalarData(sql);
        if (Convert.ToBoolean(articulos[0]))
        {
            DataSet infoArt = (DataSet)articulos[1];
            int articulosAct = 0;
            foreach (DataRow r in infoArt.Tables[0].Rows)
            {
                string clave = r[0].ToString();
                sql = "declare @existencia decimal(15,3) declare @ajuste decimal(15,3) set @existencia = (select cantidadExistencia from articulosalmacen where idAlmacen=" + isla + " and idarticulo='" + clave + "') " +
           "set @ajuste = (select isnull((select sum(valor_modificado) from bitacora_ajuste where idalmacen=" + isla + " and idarticulo='" + clave + "' and concluido=0 and existencia_fin=0),0)) " +
           "update bitacora_ajuste set concluido=1 where idalmacen=" + isla + " and idarticulo='" + clave + "' and concluido=0 and existencia_fin=0 " +
           "update articulosalmacen set cantidadExistencia=@ajuste where idalmacen=" + isla + " and idarticulo='" + clave + "' " +
            "update inventario_inicial set existenciaFin=@ajuste,usuario='" + usuario.ToUpper() + "' where idalmacen=" + isla + " and idarticulo='" + clave + "' and fecha ='" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "' and terminado=0 " +
            "insert into bitacora_ajuste values(" + isla + ",'" + clave + "'," +
            "(select isnull((select top 1 id_consecutivo from bitacora_ajuste where idalmacen=" + isla + " and idArticulo ='" + clave + "' order by id_consecutivo desc), 0) + 1),@existencia,@ajuste,(@ajuste-@existencia),'" + usuario + "','" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "',1)";
                object[] actualizado = ejecuta.insertUpdateDelete(sql);
                if (Convert.ToBoolean(actualizado[0]))
                    articulosAct++;
            }
            retono = new object[] { true, articulosAct };
        }
        else
        {
            retono = articulos;
        }
        return retono;
    }
}