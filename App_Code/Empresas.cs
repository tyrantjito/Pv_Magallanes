using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de Empresas
/// </summary>
public class Empresas
{
    int _empresa;
    string _rfc, _razon,_entrante, _saliente, _puerto, _tipo, _usuario, _contraseña;
    int _ssl;    
    BaseDatos data = new BaseDatos();

	public Empresas()
	{
        _rfc = _razon = _entrante = _saliente = _puerto = _tipo = _usuario = _contraseña = string.Empty;
        _empresa=_ssl  = 0;        
	}


    public string Rfc {
        get { return _rfc; }
        set { _rfc = value; }
    }
    public string Razon
    {
        get { return _razon; }
        set { _razon = value; }
    }
    public string Entrada
    {
        get { return _entrante; }
        set { _entrante = value; }
    }
    public string Salida
    {
        get { return _saliente; }
        set { _saliente = value; }
    }
    public string Puerto
    {
        get { return _puerto; }
        set { _puerto = value; }
    }
    public string Tipo
    {
        get { return _tipo; }
        set { _tipo = value; }
    }
    public string Usuario
    {
        get { return _usuario; }
        set { _usuario = value; }
    }
    public string Contraseña
    {
        get { return _contraseña; }
        set { _contraseña = value; }
    }
    public int Ssl
    {
        get { return _ssl; }
        set { _ssl = value; }
    }
    public int Empresa
    {
        get { return _empresa; }
        set { _empresa = value; }
    }

    private object[] existeEmpresa()
    {
        return data.intToBool(string.Format("select count(*) from catempresa where id_empresa={0}", _empresa));
    }

    public object[] actualizaEmpresa()
    {
        object[] existencia = existeEmpresa();
        object[] datos = new object[2];
        if ((bool)existencia[0])
        {
            string sql;
            if (Convert.ToBoolean(existencia[1]))
                sql = string.Format("update catempresa set rfc='{0}', razon_social='{1}', servidor_entrante='{2}',servidor_saliente='{3}',tipo_servicio='{4}',usuario='{5}',contrasena='{6}',puerto='{7}',ssl={8} where id_empresa={9}", _rfc, _razon, _entrante, _saliente, _tipo, _usuario, _contraseña, _puerto,_ssl, _empresa);
            else
                sql = string.Format("insert into catempresa values({0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9})", _empresa, _rfc, _razon, _entrante, _saliente, _tipo, _usuario, _contraseña, _puerto, _ssl);

            datos = data.insertUpdateDelete(sql);
        }
        return datos;
    }

    public void obtieneDatos()
    {
        string sql = string.Format("select rfc,razon_social,servidor_entrante,servidor_saliente,tipo_servicio,usuario,contrasena,puerto,ssl from catempresa where id_empresa={0}", _empresa);
        object[] datos = data.scalarData(sql);
        if (Convert.ToBoolean(datos[0]))
        {
            DataSet info = (DataSet)datos[1];
            foreach (DataRow fila in info.Tables[0].Rows)
            {
                _rfc =fila[0].ToString(); _razon = fila[1].ToString();
                _entrante =fila[2].ToString(); _saliente =fila[3].ToString(); _puerto = fila[7].ToString(); _tipo = fila[4].ToString();
                _usuario = fila[5].ToString(); _contraseña = fila[6].ToString();
                bool cifrado = false;
                try { cifrado = Convert.ToBoolean(fila[8].ToString()); }
                catch (Exception) { cifrado = false; }
                if (cifrado)
                    _ssl = 1;
                else
                    _ssl = 0;
            }
        }
        else
        {
            _rfc = _razon = _entrante = _saliente = _puerto = _tipo = _usuario = _contraseña = "";
            _ssl = 0; 
        }
    }

}