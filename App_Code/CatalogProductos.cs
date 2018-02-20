using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de CatalogProductos
/// </summary>
public class CatalogProductos
{
	BaseDatos data = new BaseDatos();
    string _producto;
    bool _relacionado;
    bool _existe;
    string _nombre;
    public CatalogProductos()
	{
        _producto =string.Empty;
        _relacionado = _existe = false;
        _nombre = string.Empty;
	}

    public string Producto
    {
        get { return _producto; }
        set { _producto = value; }
    }

    public bool Relacionado
    {
        get { return _relacionado; }
    }

    public bool Existe
    {
        get { return _existe; }
    }

    public string Nombre {
        set { _nombre = value; }
    }

    public void verificaRelacion()
    {
        object[] datos = new object[2];
        string sql = string.Format("select sum(tabla.registros) from(select count(*) as registros from articulosalmacen where idArticulo='{0}' and cantidadExistencia<>0 union all select COUNT(*) as registros from venta_det where id_refaccion='{0}' union all select COUNT(*) as registros from entinventariodet where entProductoID='{0}') as tabla", _producto);
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
            _relacionado = Convert.ToBoolean(datos[1]);
        else
            _relacionado = true;
    }

    public void verificaExiste()
    {
        object[] datos = new object[2];
        string sql = string.Format("select count(*) from catproductos where Upper(idProducto)='{0}'", _producto.ToUpper());
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
            _existe = Convert.ToBoolean(datos[1]);
        else
            _existe = true;
    }

    public object[] obtieneCategorias()
    {
        string sql = string.Format("select distinct tabla.id_categoria,tabla.descripcion_categoria from (select case id_categoria when 0 then '' else cast(id_categoria as char(10))end as id_categoria,descripcion_categoria from catcategorias)as tabla order by 1");
        return data.scalarData(sql);
    }

    public object[] obtieneexistenciaTotal()
    {
        string sql = string.Format("select sum(cantidad) as existencias from (select isnull((select isnull(cantidadExistencia, 0) as cantidad from articulosalmacen where idarticulo = '{0}'), 0) as cantidad) as tabla", _producto.ToUpper());
        return data.scalarToDecimal(sql);
    }

    public void actualizaUltimoCosto(int isla)
    {
        string sql = "update articulosalmacen set idpreciopublico=(select isnull((select top 1 idpreciopublico from precios_venta where idproducto='" + _producto + "' and idalmacen=" + isla + " order by idpreciopublico desc),0)) where idArticulo='" + _producto + "' and idalmacen=" + isla;
        data.insertUpdateDelete(sql);
    }
}