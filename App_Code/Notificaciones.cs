using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de Notificaciones
/// </summary>
public class Notificaciones
{
    DateTime _fecha;
    string _usuario;
    string _origen;
    int _clasificacion;
    string _notificacion;
    object[] _retorno;
    string _estatus;
    int _punto;
    string _articulo;
    int _entrada;
    int _caja;
    string _extra;
    Fechas fechas = new Fechas();
    
	public Notificaciones()
	{
        _usuario = _notificacion =_origen = _estatus = _articulo =_extra = string.Empty;
        _clasificacion = _punto = _caja = _entrada = 0;
        _fecha = fechas.obtieneFechaLocal();
        _retorno = new object[2];
	}

    public DateTime Fecha { set {_fecha=value;} }
    public string Usuario { set {_usuario=value;} }
    public string Articulo { set { _articulo = value; } }
    public string Origen { set { _origen = value; } }
    public int Clasificacion { set { _clasificacion = value; } }
    public int Punto { set { _punto = value; } }
    public int Caja { set { _caja = value; } }
    public int Entrada { set { _entrada = value; } }  
    public object[] Retorno { get { return _retorno; } }
    public string Estatus { set { _estatus = value; } }
    public string Extra { set { _extra = value; } }


    public void agregaNotificacion() {
        BaseDatos data = new BaseDatos();
        object[] datos = new object[2];
        string sql = string.Format("insert into notificaciones_PV values((select isnull((select top 1 id_notificacion from notificaciones_PV  where fecha='{0}' order by id_notificacion desc),0))+1,'{0}','{1}','{2}','{3}',{4},'{5}','{6}')", _fecha.ToString("yyyy-MM-dd"), _fecha.ToString("HH:mm:ss"), _usuario, _origen, _clasificacion, _notificacion, _estatus);
        _retorno = data.insertUpdateDelete(sql);
    }


    public void armaNotificacion() {
        Islas isla = new Islas();
        isla.Almacen = _punto;
        isla.obtieneNombre();
        string nombrePv = isla.Nombre;

        Producto articulo = new Producto();
        articulo.ClaveProducto = _articulo;
        articulo.Isla = _punto;        
        object[] nomProd = articulo.obtieneProducto();
        string descripProd="";
        if (Convert.ToBoolean(nomProd[0]))
            descripProd = Convert.ToString(nomProd[1]);
        else
            descripProd = "inexistente";
        switch (_clasificacion) { 
            case 1:
                //Precio de Venta menor o igual a 0 al ultimo registrado                
                _notificacion = "Se efectúo venta a menor precio del establecido: Producto " + descripProd + " con Clave " + _articulo + ", Tienda " + nombrePv+" (" + _punto.ToString() + "), Ticket #" + _entrada.ToString() + ", Caja #" + _caja.ToString() + ", Usuario " + _usuario.ToUpper() + ", Precio Vendido " + Convert.ToDecimal(_extra).ToString("C2");
                break;
            case 2:
                //Nueva Orden de Compra Solicitada                
                _notificacion = "Se ha generado una nueva orden de compra: Tienda " + nombrePv + " (" + _punto.ToString() + "), Usuario " + _usuario + ", Orden " + _entrada.ToString();
                break;
            case 3:
                //Inventario Negativo
                _notificacion = "Se ha generado un inventario negativo: Tienda " + nombrePv + " (" + _punto.ToString() + "), Producto " + descripProd + " con Clave " + _articulo + ", Existencia " + _extra;
                break;
            case 4:
                //Mas de dos Cajas Día
                _notificacion = "Se ha ingresado más de dos ocasiones: Tienda " + nombrePv + " (" + _punto.ToString() + "), Usuario " + _usuario + ", Intentos " + _extra;
                break;
            default:
                _notificacion = "";
                break;
        }
    }


    public void actualizaEstado() {
        BaseDatos data = new BaseDatos();
        object[] datos = new object[2];
        string sql = string.Format("update notificaciones_PV set estatus='{0}' where fecha='{1}' and id_notificacion={2}", _estatus, _fecha.ToString("yyyy-MM-dd"), _entrada);
        _retorno = data.insertUpdateDelete(sql);
    }

    public void obtieneNotificacionesPendientes() {
        BaseDatos data = new BaseDatos();
        object[] datos = new object[2];
        string sql = string.Format("select count(*) from notificaciones_PV where fecha='{1}' and estatus='{0}'", _estatus, _fecha.ToString("yyyy-MM-dd"));
        _retorno = data.scalarInt(sql);
    }
}