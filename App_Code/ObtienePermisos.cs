using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de ObtienePermisos
/// </summary>
public class ObtienePermisos
{
    string _usuario;
    bool[] _permisos;
    int _permisoBusca;
    bool _permite;
	public ObtienePermisos()
	{
        _usuario = string.Empty;
        _permisos = new bool[37];
        _permisoBusca = 0;
        _permite=false;
	}

    public string Usuario {
        set { _usuario = value; }
    }

    public int PermisoBuscado {
        set { _permisoBusca = value; }
    }

    public bool Permitido{
        get{return _permite;}
    }

    public bool[] Permisos {
        get { return _permisos; }
        set { _permisos = value; }
    }

    public void obtienePermisos() {
        for (int i = 0; i < _permisos.Length; i++) {
            _permisos[i] = false;
        }
        BaseDatos data = new BaseDatos();
        string sql = string.Format("select id_permiso from usuarios_permisos_PV where usuario='{0}'", _usuario);
        object[] datos = data.scalarData(sql);
        try {
            if (Convert.ToBoolean(datos[0])) {
                DataSet valores = (DataSet)datos[1];
                foreach (DataRow filaValor in valores.Tables[0].Rows) {
                    int posicion = Convert.ToInt32(filaValor[0].ToString());
                    posicion = posicion - 1;
                    _permisos[posicion] = true;
                }
            }
        }
        catch (Exception) {
            for (int i = 0; i < _permisos.Length; i++)
            {
                _permisos[i] = false;
            }
        }

    }

    public void cuentaPermiso() {
        for (int i = 0; i < _permisos.Length; i++)
        {
            if (_permisoBusca - 1 == i)
                _permite = _permisos[i];
        }
    }
}