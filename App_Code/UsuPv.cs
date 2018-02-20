using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de UsuPv
/// </summary>
public class UsuPv
{
    BaseDatos ejecuta = new BaseDatos();
    string sql = "";
    string _usuario;
    int _pv;
    string _error;

    public UsuPv()
    {
        _usuario =_error = string.Empty;
        _pv = 0;
    }

    public string Usuario {
        set { _usuario = value; }
    }

    public int Punto {
        set { _pv = value; }
    }


    public string Error {
        get { return _error; }
    }

    public void agregaIslaUsuario() {
        sql = string.Format("Insert into usuario_puntoventa values('{0}',{1},'A')", _usuario, _pv);
        object[] agregado = ejecuta.insertUpdateDelete(sql);
        if (Convert.ToBoolean(agregado[0]))
            _error = "";
        else
            _error = Convert.ToString(agregado[1]);
    }

    public void eliminaIslaUsuario()
    {
        sql = string.Format("delete from usuario_puntoventa where usuario='{0}' and id_punto={1}", _usuario, _pv);
        object[] eliminado = ejecuta.insertUpdateDelete(sql);
        if (Convert.ToBoolean(eliminado[0]))
            _error = "";
        else
            _error = Convert.ToString(eliminado[1]);
    }



    public DataSet llenaIslasUsuario()
    {
        sql = string.Format("select u.id_punto from usuario_puntoventa u where u.usuario='{0}'", _usuario);
        DataSet data = new DataSet();
        object[] ejecutado = ejecuta.scalarData(sql);
        if ((bool)ejecutado[0])
            data = (DataSet)ejecutado[1];
        else
            data = null;
        return data;
    }

    public DataSet llenaIsla()
    {
        sql = String.Format("select idAlmacen from catalmacenes where estatus='A' AND idAlmacen not in(select id_punto from usuario_puntoventa where usuario='{0}')", _usuario);
        DataSet data = new DataSet();
        object[] ejecutado = ejecuta.scalarData(sql);
        if ((bool)ejecutado[0])
            data = (DataSet)ejecutado[1];
        else
            data = null;
        return data;
    }

    public bool existeUsuarioAsignado(string usuario)
    {
        sql = "select count(*) from usuario_puntoventa where usuario='" + usuario + "'";
        object[] ejecutado = ejecuta.intToBool(sql);
        if ((bool)ejecutado[0])
            return (bool)ejecutado[1];
        else
            return false;
    }
}