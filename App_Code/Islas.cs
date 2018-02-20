using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de Islas
/// </summary>
public class Islas
{
    BaseDatos data = new BaseDatos();
    int _almacen;
    bool _relacionado;
    bool _existe;
    string _nombre;
    string _ubicacion;
    DataSet _islas;
	public Islas()
	{
        _almacen = 0;
        _relacionado = _existe = false;
        _nombre = string.Empty;
        _ubicacion = string.Empty;
        _islas = new DataSet();
	}

    public int Almacen
    {
        get { return _almacen; }
        set { _almacen = value; }
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
        get { return _nombre; }
    }

    public string Ubicacion {
        get { return _ubicacion; }
    }

    public DataSet IslasAgregar {
        get { return _islas; }
    }

    public void verificaRelacion()
    {
        object[] datos = new object[2];
        string sql = string.Format("select sum(tabla.registros) from(select count(*) as registros from articulosalmacen where idAlmacen={0} union all select COUNT(*) as registros from venta_enc where id_almacen={0}) as tabla", _almacen);
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
            _relacionado = Convert.ToBoolean(datos[1]);
        else
            _relacionado = true;
    }

    public void verificaExiste()
    {
        object[] datos = new object[2];
        string sql = string.Format("select count(*) from catalmacenes where Upper(nombre)='{0}'", _nombre.ToUpper());
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
            _existe = Convert.ToBoolean(datos[1]);
        else
            _existe = true;
    }

    public void obtieneUbicacion()
    {
        object[] datos = new object[2];
        string sql = string.Format("select ubicacion from catalmacenes where idAlmacen = {0} ", _almacen);
        datos = data.scalarString(sql);
        if (Convert.ToBoolean(datos[0]))
            _ubicacion = Convert.ToString(datos[1]);
        else
            _ubicacion = "";
    }

    public void obtieneNombre()
    {
        object[] datos = new object[2];
        string sql = string.Format("select nombre from catalmacenes where idAlmacen = {0} ", _almacen);
        datos = data.scalarString(sql);
        if (Convert.ToBoolean(datos[0]))
            _nombre = Convert.ToString(datos[1]);
        else
            _nombre = "";
    }

    public void obtieneIslas() {
        object[] datos = new object[2];
        string sql = "select idAlmacen from catalmacenes where estatus='A'";
        datos = data.scalarData(sql);
        if (Convert.ToBoolean(datos[0]))
            _islas = (DataSet)datos[1];
        else
            _islas = null;
    }

    public object[] obtieneParametrosTarjeta()
    {
        string sql = "select terminal,clave,intentos from punto_venta where id_punto=" + _almacen.ToString();
        return data.scalarData(sql);
    }

    public void agregaAlamacen(string islaAgregar)
    {
        string sql = "insert into articulosalmacen select " + islaAgregar + ",idproducto,0,0,1,1,'...',1,null,null,null,null from catproductos where idproducto not in((select idarticulo from articulosalmacen where idalmacen = " + islaAgregar + "))";
         data.insertUpdateDelete(sql);
    }
}