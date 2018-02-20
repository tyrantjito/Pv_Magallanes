using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de RegPagosServ
/// </summary>
public class RegPagosServ
{
    public int añop { get; set; }
    public int punto { get; set; }
    public int caja { get; set; }
    public int ticket { get; set; }
    public string codigo { get; set;}
    public string texto { get; set; }
    public string saldo_cliente { get; set; }
    public string referencia { get; set; }
    public string id_txt { get; set; }
    public string num_autorizacion { get; set; }
    public string saldo { get; set; }
    public string comision { get; set; }
    public string saldo_f { get; set; }
    public string comision_f { get; set; }
    public string fecha { get; set; }
    public string monto { get; set; }
    public string xml { get; set; }
    public string usuario { get; set; }
    public object[] retorno { get; set; }
    public int operacion { get; set; }

    public string telefono { get; set; }
    public string referencia_in { get; set; }
    public int catServicio { get; set; }
    public int tipoFont { get; set; }
    public int idServicio { get; set; }
    public int idProducto { get; set; }
    public decimal montoPagar { get; set; }

    public int esRecarga { get; set; }


	public RegPagosServ()
	{
        añop = punto = caja = ticket = operacion = 0;
        codigo = texto = saldo_cliente = referencia = id_txt = num_autorizacion = saldo = comision = saldo_f = comision_f = fecha = monto = xml = usuario = "";
        retorno = new object[2] { false, "" };
        telefono = referencia_in = "";
        catServicio = tipoFont = idServicio = idProducto = 0;
        montoPagar = 0;
        esRecarga = 0;
	}

    public void generaOperacion() {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "declare @operacion int set @operacion = (SELECT ISNULL((SELECT TOP 1 id_operacion FROM PAGOS_SERVICIOS WHERE anio=" + añop.ToString() + " ORDER BY id_operacion DESC),0))+1 " +
                     "insert into pagos_servicios(anio,id_operacion,id_punto,id_caja, usuario,telefono,referencia_in,idCatServicio,tipoFont,idServicio,idProducto,montoPagar,recarga) values("
                     + añop.ToString() + ",@operacion," + punto.ToString() + "," + caja.ToString() + ",'" + usuario + "','" + telefono + "','" + referencia_in + "'," + catServicio + "," + tipoFont + "," + idServicio + "," + idProducto + "," + montoPagar + "," + esRecarga + ") select @operacion";
        retorno = ejecuta.scalarInt(sql);        
    }

    internal void actualizaOperacion()
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "update pagos_servicios set codigo='" + codigo + "',texto='" + texto + "',saldo_cliente='" + saldo_cliente + "',referencia='" + referencia + "',id_tx='" + id_txt + "',num_autorizacion='" + num_autorizacion + "',saldo='" + saldo + "',comision='" + comision + "',saldo_f='" + saldo_f + "',comision_f='" + comision_f + "',fecha='" + fecha + "',monto='" + monto + "',xml='" + xml + "' where anio=" + añop.ToString() + " and id_operacion=" + operacion.ToString();
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    internal void obtieneInformacion()
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "select anio,id_operacion,id_punto,id_caja,ticket,codigo,texto,saldo_cliente,referencia,id_tx,num_autorizacion,saldo,p.comision,saldo_f,comision_f,fecha,monto,xml,usuario," +
"telefono,referencia_in,idCatServicio,p.tipoFont,p.idServicio,p.idProducto,montoPagar,pp.servicio,pp.producto,pp.precio_tienda,pp.comision as comisonVenta  from pagos_servicios p left join CatTipoServicio c on c.idCatTipoServicio=idCatServicio " +
 "left join TipoFont t on t.tipoFont=p.tipoFont left join Prod_Pago_Serv pp on pp.idCatTipoServicio=idCatServicio and pp.tipoFront=t.tipoFont and pp.idServicio=p.idServicio and pp.idProducto=p.idProducto where anio=" + añop.ToString() + " and id_operacion=" + operacion.ToString();
        retorno = ejecuta.scalarData(sql);
    }
}