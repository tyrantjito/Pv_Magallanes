using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de CortesParciales
/// </summary>
public class CortesParciales
{
    Fechas fechas = new Fechas();
    string _usuario;
    string _fecha;
    int _pv;
    object[] _accion;
	public CortesParciales()
	{
        _usuario = _fecha = string.Empty;
        _pv = 0;
        _accion = new object[2];
	}

    public string Usuario {
        set { _usuario = value; }
    }

    public string Fecha
    {
        set { _fecha = value; }
    }

    public int Punto
    {
        set { _pv = value; }
    }

    public object[] Accion {
        get { return _accion; }
    }

    public void agregaCorteParcial() {
        string sql = string.Format("declare @ultimoCorte int "+
"declare @saldoIni decimal(15, 2) "+
"declare @efectivo decimal(15, 2) " +
"declare @credito decimal(15, 2) " +
"declare @debito decimal(15, 2) " +
"declare @gastos decimal(15, 2) " +
"declare @cancelaciones decimal(15, 2) " +
"declare @ventaTaller decimal(15, 2) " +
"declare @ventaCredito decimal(15, 2) " +
"declare @saldo decimal(15, 2) " +
"set @ultimoCorte = (select isnull((select top 1 id_corteParcial from cortes_parciales where fecha = '{0}' order by id_corteParcial desc), 0) + 1) " +
"set @saldoIni = (select isnull(sum(saldo_inicial_pv), 0) from punto_venta where id_punto ={1}) " +
"set @efectivo = (select isnull(sum(total), 0) from venta_enc where id_almacen ={1} and fecha_venta = '{0}' AND forma_pago = 'E' and usuario='{2}') " +
"set @debito = (select isnull(sum(total), 0) from venta_enc where id_almacen ={1} and fecha_venta = '{0}' AND forma_pago = 'D' and usuario='{2}') " +
"set @credito = (select isnull(sum(total), 0) from venta_enc where id_almacen ={1} and fecha_venta = '{0}' AND forma_pago = 'A' and usuario='{2}') " +
"set @ventaTaller= (select sum(tabla.total) from(SELECT isnull(total,0) as total,isnull(Orden,0) as orden from venta_enc where id_punto={1} and fecha_venta = '{0}' and usuario='{2}')as tabla where tabla.orden>0) "+
"set @ventaCredito = (select sum(tabla.total) from(SELECT isnull(total, 0) as total, isnull(venta_credito, 0) as venta_credito,isnull(Orden,0) as orden from venta_enc where id_punto={1} and fecha_venta = '{0}' and usuario='{2}') as tabla where tabla.venta_credito = 1 AND tabla.orden=0) " +
"set @gastos = (select isnull(sum(importe), 0) from gastos where fecha = '{0}' and idAlmacen = {1} and usuario='{2}') " +
"set @cancelaciones = (select isnull(sum(total), 0) from cancelaciones_enc where fecha = '{0}' and id_punto = {1} and usuario='{2}') " +
"set @saldo = @saldoIni + @efectivo + @debito + @credito - @gastos - @cancelaciones " +
"insert into cortes_parciales values(@ultimoCorte,'{0}',{1},'{2}','{3}',@saldoIni,@efectivo,@debito,@credito,@gastos,@saldo,@cancelaciones,@ventaTaller,@ventaCredito)", _fecha, _pv, _usuario, fechas.obtieneFechaLocal().ToString("HH:mm:ss"));
        BaseDatos data = new BaseDatos();
        _accion = data.insertUpdateDelete(sql);
    }

}