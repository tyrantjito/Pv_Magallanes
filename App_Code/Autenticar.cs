using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Autenticar
/// </summary>
public class Autenticar
{
    BaseDatos datos = new BaseDatos();
    string usuario;
    string password;
    string sql;
    
	public Autenticar()
	{
        usuario = string.Empty;
        password = string.Empty;
        sql = string.Empty;
	}

    public string Usuario {
        get { return this.usuario; }
        set { this.usuario = value; }
    }

    public string Password {
        get { return this.password; }
        set { this.password = value; }
    }

    public object[] autenticar() {        
        this.sql = string.Format("Select count(*) from usuarios_PV where usuario='{0}' and password='{1}' and estatus='A'", this.usuario, this.password);
        return datos.intToBool(this.sql);
    }
}