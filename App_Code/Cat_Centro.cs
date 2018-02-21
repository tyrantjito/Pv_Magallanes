using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Cat_Centro
/// </summary>
public class Cat_Centro
{
    BaseDatos data = new BaseDatos();
    string _unidad;
    string Nombre { set; get; }
    bool _relacionado;
    bool _existe;
    bool _borrado;
    public Cat_Centro()
    {
        _unidad = string.Empty;
        _relacionado = false;
        _existe = false;
        _borrado = false;
    }

    public string Unidad
    {
        get { return _unidad; }
        set { _unidad = value; }
    }

    public bool Relacionado
    {
        get { return _relacionado; }
    }

    public bool Existe
    {
        get { return _existe; }
    }

    public bool Borrado
    {
        get { return _borrado; }
    }

    public void verificaRelacion()
    {
        object[] datos = new object[2];
        string sql = string.Format("select count(*) from PV_Centros where IDCentro='{0}'", _unidad);
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
            _relacionado = Convert.ToBoolean(datos[1]);
        else
            _relacionado = true;
    }

    public void verificaExiste()
    {
        object[] datos = new object[2];
        string sql = string.Format("select count(*) from PV_Centros where IDCentro='{0}'", _unidad);
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
            _existe = Convert.ToBoolean(datos[1]);
        else
            _existe = true;
    }

    /*public void borraUnidad()
    {
        object[] datos = new object[2];
        string sql = string.Format("delete from PV_Centros where IDCentro='{0}' and Nom_Centro= '{1}'", _unidad,Nombre);
        datos = data.insertUpdateDelete(sql);
        if (Convert.ToBoolean(datos[0]))
            _borrado = Convert.ToBoolean(datos[1]);
        else
            _borrado = false;
    }*/
}