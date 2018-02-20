using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de Usuarios
/// </summary>
public class Usuarios
{
    BaseDatos data = new BaseDatos();
    string _usuario;
    string _nombre;
    int _perfil;
    bool _existe;
    int _pv;
    bool _sesionPrevia;
    bool _ultimaCaja;
    bool _registrado;
    int _caja;

	public Usuarios()
	{
        _usuario = string.Empty;
        _nombre = string.Empty;
        _perfil = _caja = _pv = 0;
        _existe = _sesionPrevia =_ultimaCaja = _registrado = false;
	}

    public string Usuario {
        get { return _usuario; }
        set { _usuario = value; }
    }

    public string Nombre {
        get { return _nombre; }
    }

    public int Perfil
    {
        get { return _perfil; }
    }

    public bool Existe {
        get { return _existe; }
    }

    public int Punto {
        set { _pv = value; }
        get { return _pv; }
    }

    public bool SesionPrevia
    {
        get { return _sesionPrevia; }
    }

    public bool UltimaCaja
    {
        get { return _ultimaCaja; }
    }

    public bool Registrado
    {
        get { return _registrado; }
    }

    public int Caja
    {
        get { return _caja; }
    }

    public void obtieneNombreUsuario() {
        object[] datos = new object[2];
        string sql = string.Format("select (rtrim(ltrim(nombre))+' '+rtrim(ltrim(apellido_pat))+' '+rtrim(ltrim(isnull(apellido_mat,'')))) as nombre from usuarios_PV where usuario='{0}'", _usuario);
        datos = data.scalarString(sql);
        if (Convert.ToBoolean(datos[0]))
            _nombre = Convert.ToString(datos[1]);
        else
            _nombre = "";
    }

    public DateTime fechaAccesosPV()
    {
        Fechas fechas = new Fechas();
        DateTime fechaDate = fechas.obtieneFechaLocal().AddDays(10);
        string sql = "select top 1 cast(fecha as varchar) from accesos_pv where usuario='" + Usuario + "' order by fecha desc";
        object[] ejecutado = data.scalarString(sql);
        if (Convert.ToBoolean(ejecutado[0]))
            fechaDate= Convert.ToDateTime(ejecutado[1]);
        return fechaDate;
    }

    public void obtienePerfilUsuario()
    {
        object[] datos = new object[2];
        string sql = string.Format("select perfil from usuarios_PV where usuario='{0}'", _usuario);
        datos = data.scalarInt(sql);
        if (Convert.ToBoolean(datos[0]))
            _perfil = Convert.ToInt32(datos[1]);
        else
            _perfil = 0;
    }

    public void existeUsuario()
    {
        object[] datos = new object[2];
        string sql = string.Format("select count(*) from usuarios_PV where usuario='{0}'", _usuario);
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
            _existe = Convert.ToBoolean(datos[1]);
        else
            _existe = false;
    }

    public void obtienePuntoVenta() {
        object[] datos = new object[2];
        string sql = string.Format("select id_punto from usuario_puntoventa where usuario='{0}'", _usuario);
        datos = data.scalarInt(sql);
        if (Convert.ToBoolean(datos[0]))
        {
            try
            {
                _pv = Convert.ToInt32(datos[1].ToString());
            }
            catch (Exception) {
                _pv = 0;
            }
        }
        else
            _pv = 0;
    }

    public DataSet obtienePuntos() {
        object[] datos = new object[2];
        string sql = string.Format("select u.id_punto,isnull(c.nombre,'Desconocida') as nombre  from usuario_puntoventa u left join catalmacenes c on c.idAlmacen=u.id_punto where u.usuario='{0}'", _usuario);
        datos = data.scalarData(sql);
        if (Convert.ToBoolean(datos[0]))
            return (DataSet)datos[1];
        else
            return null;
    }

    public void existeSessionPrevia()
    {
        object[] datos = new object[2];
        string sql = string.Format("select count(*) from usuarios_PV where usuario='{0}' and estatus_dia='A'", _usuario);
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
            _sesionPrevia = Convert.ToBoolean(datos[1]);
        else
            _sesionPrevia = true;
    }

    public void tieneCajaAsignada()
    {
        object[] datos = new object[2];
        string sql = string.Format("select count(*) from usuarios_PV where usuario='{0}' and caja_asignada<>0", _usuario);
        datos = data.intToBool(sql);
        if (Convert.ToBoolean(datos[0]))
        {
            _ultimaCaja = Convert.ToBoolean(datos[1]);
        }
        else
            _ultimaCaja = true;
    }

    public void registraAccesoAdmin()
    {
        object[] datos = new object[2];
        Accesos acceso = new Accesos();
        acceso.Punto = _pv;
        acceso.Usuario = _usuario;
        acceso.Acceso = "E";
        acceso.registraIngreso();
        datos = acceso.Registrado;
        if (Convert.ToBoolean(datos[0]))
            _registrado = Convert.ToBoolean(datos[1]);
        else
            _registrado = false;
    }

    public void cajaAsignada()
    {
        object[] datos = new object[2];
        string sql = string.Format("select caja_asignada from usuarios_PV where usuario='{0}'", _usuario);
        datos = data.scalarInt(sql);
        if (Convert.ToBoolean(datos[0]))
        {
            _caja = Convert.ToInt32(datos[1]);
        }
        else
            _caja = 0;
    }


    public string obtieneCorreo()
    {
        object[] datos = new object[2];
        string sql = string.Format("select correo from usuarios_PV where usuario='{0}'", _usuario);
        datos = data.scalarString(sql);
        if (Convert.ToBoolean(datos[0]))
            return Convert.ToString(datos[1]);
        else
            return "";
    }

    internal string obtieneContrasena()
    {
        object[] datos = new object[2];
        string sql = string.Format("select password from usuarios_PV where usuario='{0}'", _usuario);
        datos = data.scalarString(sql);
        if (Convert.ToBoolean(datos[0]))
            return Convert.ToString(datos[1]);
        else
            return "Error al intentar recuperar su contrase&ntilde;a, por favor contacte al administrador del sistema para que le sea proporcionada su contrase&ntilde;a.";
    }
}