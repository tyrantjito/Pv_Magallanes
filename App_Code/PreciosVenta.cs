using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de PreciosVenta
/// </summary>
public class PreciosVenta
{
    BaseDatos data = new BaseDatos();
    Fechas fechas = new Fechas();
    decimal _precio;
    string _producto;
    private bool _existe;
    bool _agregado;
    string _usuario;
    int _almacen;

	public PreciosVenta()
	{
        _producto = _usuario = string.Empty;
        _precio = 0;
        _existe = false;
        _agregado = false;
        _almacen = 0;
	}

    public decimal Precio {
        set { _precio = value; }
    }

    public int Almacen {
        set { _almacen = value; }
    }

    public string Producto {
        set { _producto = value; }
    }

    public string Usuario
    {
        set { _usuario = value; }
    }

    public bool Agregado{
        get{return _agregado;}
    }

    private bool existePrecio() {
        object[] datos = new object[2];
        string sql = string.Format("select count(*) from precios_venta where ventaUnitaria={0} and upper(idProducto)='{1}' and idAlmacen={2}", _precio, _producto.ToUpper(), _almacen);
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
            _existe = Convert.ToBoolean(datos[1]);
        else
            _existe = true;
        return _existe;
    }

    public void agregaPrecioVenta() {
        existePrecio();
        if (!_existe)
        {
            object[] datos = new object[2];
            string sql = string.Format("insert into precios_venta (idProducto,idAlmacen,idPrecioPublico,ventaUnitaria,idMoneda,fecha,usuario) values('{0}',{2},( select case (select count(*) from precios_venta where idProducto='{0}' and idAlmacen={2}) when 0 then 1 else (select top 1 idPrecioPublico from precios_venta where idProducto='{0}' and idAlmacen={2} order by idPrecioPublico desc)+1 end),{1},1,'{3}','{4}')", _producto.ToUpper(), _precio, _almacen, fechas.obtieneFechaLocal().ToString("yyyy-MM-dd"), _usuario.ToUpper());
            datos = data.insertUpdateDelete(sql);
            if (Convert.ToBoolean(datos[0]))
                _agregado = Convert.ToBoolean(datos[1]);
            else
                _agregado = false;            
        }
        else
            _agregado = false;
    }
}