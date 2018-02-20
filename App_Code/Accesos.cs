using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de Accesos
/// </summary>
public class Accesos
{
    Fechas fechas = new Fechas();
    BaseDatos data = new BaseDatos();
    string _usuario;
    int _pv, _caja;
    string _acceso;
    object[] _registrado;
	public Accesos()
	{
        _usuario = _acceso = string.Empty;
        _pv =_caja = 0;
        _registrado = null;
	}

    public string Usuario {
        set { _usuario = value; }
    }

    public string Acceso
    {
        set { _acceso = value; }
    }

    public int Punto
    {
        set { _pv = value; }
    }
    public int Caja
    {
        set { _caja = value; }
    }

    public object[] Registrado {
        get { return _registrado; }
    }

    public void registraIngreso(){

        object[] datos = new object[2];
        _registrado = datos;
        DateTime fecha = fechas.obtieneFechaLocal();
        string sql = "registraIngresoAdm";
        string estatus;
        if (_acceso == "E")
            estatus = "A";
        else
            estatus = "I";
        datos = data.generaMovimiento(_pv, _usuario, fecha, _acceso, estatus, sql);
        _registrado = datos;        
    }
}