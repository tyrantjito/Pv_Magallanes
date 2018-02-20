using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de ErroresServicios
/// </summary>
public class ErroresServicios
{
    public object[] _retorno { get; set; }
    public string _codigo { get; set; }
    public string _mensaje { get; set; }
    string sql;
    BaseDatos datos = new BaseDatos();
	public ErroresServicios()
	{
        _retorno = new object[] { false, "" };
        _codigo = "";
        sql = string.Empty;
	}

    public void obtieneCodigo() {
        existeCodigo();
        if (Convert.ToBoolean(_retorno[0]))
        {
            if (Convert.ToInt32(_retorno[1]) > 0)
            {
                sql = "Select mensaje from Codigos_error_ps where codigo='" + _codigo + "'";
                _retorno = datos.scalarString(sql);
                if (Convert.ToBoolean(_retorno[0]))
                    _mensaje = Convert.ToString(_retorno[1]);
                else
                {
                    _mensaje = Convert.ToString(_retorno[1]);
                    _codigo = "";
                }
            }
            else 
                insertaCodigo();            
        }        
    }

    private void existeCodigo() {
        sql = "Select count(*) from Codigos_error_ps where codigo='" + _codigo + "'";
        _retorno = datos.scalarInt(sql);
    }

    private void insertaCodigo() {
        sql = "insert into Codigos_error_ps values('" + _codigo + "','" + _mensaje + "')";
        _retorno = datos.insertUpdateDelete(sql);
    }
}