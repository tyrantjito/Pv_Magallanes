using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de Pagos
/// </summary>
public class Pagos
{

    public int ticket { get; set; }
    public int caja { get; set; }
    public int punto { get; set; }
    public int almacen { get; set; }
    public int pago { get; set; }
    public string formaPago { get; set; }
    public string referencia { get; set; }
    public string banco { get; set; }
    public string tarjeta { get; set; }
    public string temrinacion { get; set; }
    public decimal monto { get; set; }
    public decimal cambio { get; set; }
    public decimal restan { get; set; }
    public object[] retorno { get; set; }


	public Pagos()
	{
        ticket = caja = punto = almacen = pago = 0;
        formaPago = referencia = banco = tarjeta = temrinacion = "";
        monto = cambio = restan = 0;
        retorno = new object[2] { false, "" };
	}

    public void registraPago() {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "insert into pagos_ticket values(" + ticket.ToString() + "," + caja.ToString() + "," + punto.ToString() + "," +
            punto.ToString() + ",(select isnull((select top 1 pagos from pagos_ticket where ticket=" + ticket.ToString() + " and id_caja=" + caja.ToString() + " and id_punto=" + punto.ToString() + " and id_almacen=" + punto.ToString() + " order by pagos desc),0)+1),'" + formaPago + "','" + referencia + "','" + banco + "','" + tarjeta + "','" + temrinacion + "'," + monto.ToString("F2") + "," + cambio.ToString("F2") + "," + restan.ToString("F2") + ")";
        retorno = ejecuta.insertUpdateDelete(sql);
    }



    public void actualizaTickets()
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "update pagos_ticket set ticket=" + ticket + " where id_caja=" + caja.ToString() + " and id_punto=" + punto.ToString() + " and id_almacen=" + punto.ToString() + " and ticket=0";
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    internal void obtienePagos()
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "select forma_pago,(case forma_pago when 'E' then 'Efectivo' else 'Tarjeta' end ) as desc_FormaPago,monto,cambio,restan,tarjeta,terminacion_tarjeta,referencia_pago from pagos_ticket where ticket=" + ticket.ToString() + " and id_caja=" + caja.ToString() + " and id_punto=" + punto.ToString() + " and id_almacen=" + punto.ToString();
        retorno = ejecuta.scalarData(sql);
    }
}