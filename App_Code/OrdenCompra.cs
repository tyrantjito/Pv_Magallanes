using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de OrdenCompra
/// </summary>
public class OrdenCompra
{
    string _texto;
    int _renglon;
    public OrdenCompra()
    {
        _texto = string.Empty;        
        _renglon = 0;
    }

    public string texto
    {
        set {  _texto = value; }
        get { return _texto; }
    }

    public int renglon
    {
        set { _renglon = value; }
        get { return _renglon; }
    }   

    public object[] agregaNuevaOrden(object[] _nuevo) {
        object[] retorno = new object[2];
        string sql = "nuevaOrden";//nuevaOrden
        string[] argmentosBasicos = new string[3] { _nuevo[0].ToString(), _nuevo[1].ToString(), _nuevo[2].ToString()};
        BaseDatos data = new BaseDatos();
        List<OrdenCompraDetalle> lista = (List<OrdenCompraDetalle>)_nuevo[3];
        retorno = data.generaOrden(sql, argmentosBasicos, lista);
        return retorno;
    }

    public DataSet llenaOrdenCompra(int idAlmacen)
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "select tabla.id_categoria,tabla.idArticulo,tabla.descripcion_categoria,tabla.descripcion,tabla.existencia,tabla.minima,tabla.maxima," +
            "((tabla.minima) - (case when tabla.existencia < 0 then 0 else tabla.existencia end) + ((tabla.maxima - tabla.minima) / 2))as recomendado " +
            "from(" +
            "select c.id_categoria,cc.descripcion_categoria,a.idArticulo,c.descripcion," +
            "isnull(a.cantidadexistencia, 0) as existencia," +
            "isnull(a.cantidadMinima, 0) as minima," +
            "isnull(a.cantidadMaxima, 0) as maxima " +
            "from articulosalmacen a " +
            "inner join catproductos c on c.idproducto = a.idArticulo " +
            "inner join CatCategorias cc on cc.id_categoria = c.id_categoria " +
            "where a.idalmacen = " + idAlmacen.ToString() + " and c.estatus = 'A' and a.cantidadexistencia <= a.cantidadMinima) as tabla";
        object[] insertado = ejecuta.scalarData(sql);
        if ((bool)insertado[0])
            return (DataSet)insertado[1];
        else
            return null;
    }

    public DataSet llenaOrdenDetalle(int noOrden, int idAlmacen)
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "select solicitud from orden_compra_detalle_PV where no_orden=" + noOrden.ToString() + " and idAlmacen=" + idAlmacen.ToString();
        object[] insertado = ejecuta.scalarData(sql);
        if ((bool)insertado[0])
            return (DataSet)insertado[1];
        else
            return null;
    }

    public object[] actualizaEstatus(int noOrden, int idAlmacen,string status)
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = string.Format("update orden_compra_encabezado_PV set estatus='{0}' where no_orden={1} and idAlmacen={2}", status, noOrden, idAlmacen);
        object[] insertado = ejecuta.insertUpdateDelete(sql);
        return insertado;
    }

    internal static object[] ordenDatos(string folio)
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "select c.descripcion,e.entprodcant,e.entprodcostounit,e.entimporte,c.id_categoria,cc.descripcion_categoria,c.idmedida,c.idproducto " +
                     "from entinventariodet e " +
                     "inner join catproductos c on c.idproducto = e.entproductoid " +
                     "inner join catcategorias cc on cc.id_categoria = c.id_categoria " +
                     "where e.entfolioid = " + folio + " order by cc.descripcion_categoria desc,c.descripcion asc";
        return ejecuta.scalarData(sql);
    }

    public string obtieneDescCategoriaProducto(string idProducto)
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "select cc.descripcion_categoria from catproductos c inner join CatCategorias cc on cc.id_categoria = c.id_categoria where c.idproducto = '" + idProducto + "'";
        object[] ejecutado = ejecuta.scalarString(sql);
        if ((bool)ejecutado[0])
            return ejecutado[1].ToString();
        else
            return "";

    }

    public object[] generaorden(object[] orden)
    {
        BaseDatos data = new BaseDatos();
        object[] resultado = new object[2];
        string sql = "generaOrden";
        //resultado = data.generaOrden(orden, sql);
        return resultado;
    }

    public DataSet obtieneInfoDetalle(int ordenCompra, int isla)
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "select o.idArticulo,o.descripcion,o.cantidad,(select isnull((select costoUnitario from articulosalmacen where idAlmacen=o.idAlmacen and idArticulo=o.idArticulo),0)) as costo,(o.cantidad * (select isnull((select costoUnitario from articulosalmacen where idAlmacen=o.idAlmacen and idArticulo=o.idArticulo),0))) as importe from orden_compra_detalle_PV o where o.no_orden=" + ordenCompra.ToString() + " and o.idAlmacen=" + isla.ToString();
        object[] insertado = ejecuta.scalarData(sql);
        if ((bool)insertado[0])
            return (DataSet)insertado[1];
        else
            return null;
    }
}