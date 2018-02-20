using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Lineas
/// </summary>
public class Lineas
{
	BaseDatos data = new BaseDatos();
    string _unidad;
    bool _relacionado;
    bool _existe;    
	public Lineas()
	{
        _unidad = string.Empty;
        _relacionado = false;
        _existe = false;        
	}

    public string Unidad {
        get { return _unidad; }
        set { _unidad = value; }
    }

    public bool Relacionado {
        get { return _relacionado; }        
    }

    public bool Existe {
        get { return _existe; }
    }

   public void verificaRelacion() {
        object[] datos = new object[2];
        string sql = string.Format("select count(*) from catproductos where idLinea='{0}'", _unidad);
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
            _relacionado = Convert.ToBoolean(datos[1]);
        else
            _relacionado = true;
    }

    public void verificaExiste()
    {
        object[] datos = new object[2];
        string sql = string.Format("select count(*) from catlineas where idLinea='{0}'", _unidad);
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
            _existe = Convert.ToBoolean(datos[1]);
        else
            _existe = true;
    }
}