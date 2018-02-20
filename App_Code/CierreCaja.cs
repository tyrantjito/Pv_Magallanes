using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using E_Utilities;

/// <summary>
/// Descripción breve de CierreCaja
/// </summary>
public class CierreCaja
{
    BaseDatos data = new BaseDatos();
    Fechas fechas = new Fechas();
    string _usuario;
    DateTime _fecha;
    int _caja;
    int _punto;
    string _acceso;
    decimal _efectivo, _tcredito, _tdebito, _tventa;
    string _fechaDia;
    public string _horaDia { get; set; }
    object[] _registrado;
    bool _cierreDia;
    bool _abiertas;
    public int _anio { get; set; }

	public CierreCaja()
	{
        _usuario = _acceso =_fechaDia = string.Empty;
        _caja = _punto = 0;
        _fecha = fechas.obtieneFechaLocal();
        _efectivo = _tcredito = _tdebito = _tventa = 0;
        _cierreDia= _abiertas = false;
        _anio = 0;
    }

    public string Usuario {
        set { _usuario = value; }
    }

    public string Acceso
    {
        set { _acceso = value; }
    }

    public int Caja
    {
        set { _caja = value; }
    }

    public int Punto
    {
        set { _punto = value; }
    }

    public string FechaDia
    {
        set { _fechaDia = value; }
    }

    public object[] Registrado {
        get { return _registrado; }
    }

    public bool cierreDia {
        get { return _cierreDia; }
    }
    public bool Abiertas
    {
        get { return _abiertas; }
    }

    public object[] generaCorteCaja()
    {
        object[] resultado = new object[2];
        _registrado = resultado;
        string sql = "registraCierre";
        string estatus;
        if (_acceso == "E")
            estatus = "A";
        else
            estatus = "I";
        resultado = data.generaCierre(_punto, _usuario, _acceso, estatus, _caja, _fecha, sql);
        _registrado = resultado;
        return resultado;

    }


    public object[] obtieneCajasExistentes()
    {
        string sql = "select * from cajas where id_caja=" + _caja + " and anio<=" + fechas.obtieneFechaLocal().Year.ToString() + " order by anio desc";
        return data.scalarData(sql);
    }

    public bool cajaActualAbierta()
    {
        /*string sql = "select count(*) from cajas where id_cierre!=0 and id_caja="+_caja.ToString()+" and anio="+fechas.obtieneFechaLocal().Year.ToString()+ " or id_caja=" + _caja.ToString() + " and anio=" + fechas.obtieneFechaLocal().Year.ToString() + " and fecha_cierre>'1900-01-01' "+
            "or id_cierre=0 and id_caja=" + _caja.ToString() + " and anio=" + fechas.obtieneFechaLocal().Year.ToString() + " and fecha_cierre is null and cast((cast('"+fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss")+"' as datetime)-cast(fecha_apertura as datetime))as int)>1";*/
        string sql = "select count(*) from cajas where id_cierre!=0 and id_caja=" + _caja.ToString() + " and anio=" + _anio + " or id_caja=" + _caja.ToString() + " and anio=" + _anio + " and fecha_cierre>'1900-01-01' " +
            "or id_cierre=0 and id_caja=" + _caja.ToString() + " and anio=" + _anio + " and fecha_cierre is null and cast((convert(datetime,convert(char(19),'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss") + "',120),120)-convert(datetime,convert(char(19),fecha_apertura,120),120))as int)>1";
        object[] ejecutado = data.intToBool(sql);
        if ((bool)ejecutado[0])
            return Convert.ToBoolean(ejecutado[1]);
        else
            return true;
    }

    public void existeCierreDia()
    {
        object[] datos = new object[2];
        _registrado = datos;
        string sql = string.Format("select count(*) from cierres_diarios where fecha_cierre='{0}' and hora_cierre='{2}' and id_punto_venta={1}", _fechaDia, _punto,_horaDia);
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
            _cierreDia = Convert.ToBoolean(datos[1]);
        else
            _cierreDia = true;
    }

    public string obtieneFechaPrimerCajaAbierta()
    {
        string sql = "select top 1 fecha_apertura from cajas where fecha_cierre is null and anio ="+fechas.obtieneFechaLocal().Year+" order by id_caja asc";
        object[] fechaArr = data.scalarString(sql);
        if (Convert.ToBoolean(fechaArr[0]))
            return fechaArr[1].ToString();
        else
            return fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
    }

    public void generaCierreDia() {
        object[] resultado = new object[2];
        _registrado = resultado;        
        string cajasCerradas="";
        string sql = "select id_caja from cajas where id_cierre=0 and CONVERT(CHAR(10),FECHA_CIERRE,120)+' '+CONVERT(CHAR(8),HORA_CIERRE,120) <= '" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss") + "' and anio<=" + fechas.obtieneFechaLocal().Year.ToString();
        object[] cajasObtenidas = data.scalarData(sql);
        if (Convert.ToBoolean(cajasObtenidas[0]))
        {
            DataSet INFO = (DataSet)cajasObtenidas[1];
            foreach (DataRow valorCaja in INFO.Tables[0].Rows)
            {
                cajasCerradas = cajasCerradas.Trim() + valorCaja[0].ToString() + ",";
            }

            if (cajasCerradas != "")
            {
                cajasCerradas = cajasCerradas.Trim().Substring(0, cajasCerradas.Length - 1);
                sql = "registraCierreDia";
                resultado = data.generaCierreDia(_punto, _usuario, cajasCerradas, _fecha, sql);
                if (Convert.ToBoolean(resultado[0])) {
                    sql = "update cajas set id_cierre=" + resultado[1].ToString() + " where anio=" + fechas.obtieneFechaLocal().Year.ToString() + " and id_caja in(" + cajasCerradas + ") " +
                        "update gastos set id_cierre=" + resultado[1].ToString() + " where id_caja in(" + cajasCerradas + ")" +
                        "update cancelaciones_enc set id_cierre=" + resultado[1].ToString() + " where id_caja in(" + cajasCerradas + ")" +
                        " update pagos_servicios set id_cierre =" + resultado[1].ToString() + " where id_caja in(" + cajasCerradas + ")";
                    object[] acutalizado = data.insertUpdateDelete(sql);
                    if (Convert.ToBoolean(acutalizado[0]))
                    {
                        _registrado[0] = true;
                        _registrado[1] = true;
                    }
                    else
                    {
                        _registrado[0] = false;
                        _registrado[1] = "Error al generar el corte por favor vuelva a intentarlo";
                    }
                }
                else
                    _registrado = resultado;
            }
            else {
                _registrado = new object[2] { false, "No se generaron cajas en el proceso por esta razon es imposible hacer el cierre de dia" };
            }
        }
        else {
            _registrado = new object[2] { false, "No se generaron cajas en el proceso por esta razon es imposible hacer el cierre de dia" };
        }
        
    }

    public void existenCajasAbiertas()
    {
        object[] datos = new object[2];
        _registrado = datos;
        string sql = string.Format("select count(*) from cajas where anio={0} and fecha_apertura='{1}' and fecha_cierre is null and id_punto={2}", _fechaDia.Substring(0, 4), _fechaDia, _punto);
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
            _abiertas = Convert.ToBoolean(datos[1]);
        else
            _abiertas = true;
    }
}