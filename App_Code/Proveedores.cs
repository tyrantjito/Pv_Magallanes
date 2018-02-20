using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for Proveedores
/// </summary>
public class Proveedores
{
   BaseDatos data = new BaseDatos();
    string _proveedor;
    
    bool _existe;
    string _nombre;
    public Proveedores()
	{
        _proveedor =string.Empty;
       
        _nombre = string.Empty;
	}

    public string Proveedor
    {
        get { return _proveedor; }
        set { _proveedor = value; }
    }


    public bool Existe
    {
        get { return _existe; }
    }

    public string Nombre {
        set { _nombre = value; }
    }

    public void existeEntinventarioenc()
    {
        object[] datos = new object[2];
        string sql = string.Format("SELECT COUNT(*) FROM entinventarioenc WHERE [claveProveedor]='{0}'", _proveedor.ToUpper());
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
            _existe = Convert.ToBoolean(datos[1]);
        else
            _existe = true;
    }

    public void verificaExiste()
    {
        object[] datos = new object[2];
        string sql = string.Format("SELECT COUNT(*) FROM clienteproveedor WHERE [RFC]='{0}'", _proveedor.ToUpper());
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
            _existe = Convert.ToBoolean(datos[1]);
        else
            _existe = true;
    }

    public string cambiaStatus(string provID, string status)
    {
        object[] datos = new object[2];
        if(string.IsNullOrEmpty(status))
        {
            object[] objEstatus = new object[2];
            string qryEst = "SELECT estatus FROM clienteproveedor WHERE clave=" + provID;
            objEstatus = data.scalarString(qryEst);
            status = objEstatus[1].ToString();
        }
        string qryCambEstProv = string.Format("UPDATE clienteproveedor SET estatus = '{0}' WHERE clave = {1}", status == "A" ? "B" : "A", provID);
        datos = data.insertUpdateDelete(qryCambEstProv);
        string result;
        if (Convert.ToBoolean(datos[0]))
            result = status;
        else
            result = datos[1].ToString();
        return result;
    }

    public DataSet selectProv(string provID)
    {
        object[] datos = new object[2];
        string sql = "SELECT [RFC], [razonSocial], nombres, ISNULL(apellidoPaterno, '') AS apPat, ISNULL(apellidoMaterno, '') AS apMat, ISNULL(calle, '') As calle, ISNULL(numExt,'') AS numExt, ISNULL(numInt, '') AS numInt, ISNULL(colonia,'')AS colonia, ISNULL(ciudad, '') AS ciudad, ISNULL(estado, '') AS estado, ISNULL(pais, '') AS pais, " +
                     "ISNULL(telefonoPart,'') AS telPart, ISNULL(telefonoCel,'') AS telCel, ISNULL(codigoPostal,'') AS cp, ISNULL(email,'') AS email, ISNULL(contacto,'') AS contacto, personaFiscal FROM [clienteproveedor] WHERE clave=" + provID;
        datos = data.scalarData(sql);
        DataSet ds = null;
        if(Convert.ToBoolean(datos[0]))
        { 
            ds = (DataSet)datos[1];
        }
        return ds;
    }

    public DataSet obtieneInfo(string proveedor)
    {
        object[] datos = new object[2] { false, "" };
        string sql = "select * from clienteproveedor where clave=" + proveedor;
        datos = data.scalarData(sql);
        DataSet ds = null;
        if (Convert.ToBoolean(datos[0]))
        {
            ds = (DataSet)datos[1];
        }
        return ds;
    }
}
