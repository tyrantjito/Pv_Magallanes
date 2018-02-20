using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de Cajas
/// </summary>
public class Cajas
{
    BaseDatos data = new BaseDatos();
    Fechas fechas = new Fechas();
    string _usuario,_acceso;
    int _caja,_pv;
    object[] _valores;
	public Cajas()
	{
        _usuario =_acceso = string.Empty;
        _caja= _pv = 0;
        _valores=null;
	}

    public string Usuario {
        set { _usuario = value; }
    }

    public string Acceso
    {
        set { _acceso = value; }
    }

    public int Caja {
        set { _caja = value; }
        get { return _caja; }
    }

    public int Punto
    {
        set { _pv = value; }        
    }

    public object[] Valores{
        get{return _valores;}
    }

    public void generaCaja() { 
        object[] datos = new object[2];
        DateTime fecha = fechas.obtieneFechaLocal(); 
        string sql = "generaCaja";
        datos = data.generaCaja(_pv, _usuario, fecha, _acceso, sql);
        _valores = datos;
        if (Convert.ToBoolean(datos[0]))
        {
            try { _caja = Convert.ToInt32(datos[1].ToString()); }
            catch (Exception) { _caja = 0; }
        }
        else
            _caja = 0;        
    }

    public void obtieneCajaAsignada()
    {
        object[] datos = new object[2];        
        string sql = string.Format("select caja_asignada from usuarios_PV where usuario='{0}'", _usuario);
        datos = data.scalarInt(sql);
        if (Convert.ToBoolean(datos[0]))
        {
            try { _caja = Convert.ToInt32(datos[1].ToString()); }
            catch (Exception) { _caja = 0; _valores[1] = datos[1]; }
        }
        else
        {
            _valores[1] = Convert.ToString(datos[1]);
            _caja = 0; 
        }
        _valores[0] = datos[0];
    }

    public void obtieneUltimaCajaAsignadaUsuario()
    {
        object[] datos = new object[2];
        string sql = string.Format("select top 1 id_caja from cajas where anio={0}", fechas.obtieneFechaLocal().Year);
        datos = data.scalarInt(sql);
        if (Convert.ToBoolean(datos[0]))
        {
            try { _caja = Convert.ToInt32(datos[1].ToString()); }
            catch (Exception) { _caja = 0; _valores[1] = datos[1]; }
        }
        else
        {
            _valores[1] = Convert.ToString(datos[1]);
            _caja = 0;
        }
        _valores[0] = datos[0];
    }

    public object[] cajasDelDia()
    {
        string sql = string.Format("select count(*) from cajas where anio={0} and fecha_apertura='{1}' and usuario='{2}'", fechas.obtieneFechaLocal().Year, fechas.obtieneFechaLocal().ToString("yyyy-MM-dd"), _usuario);
        return data.scalarInt(sql);        
    }
}