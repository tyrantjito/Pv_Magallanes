using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de ImprimeTicket
/// </summary>
public class ImprimeTicket
{
    public int ticketc { get; set; }
    int _pv;
    int _ticket;
    BaseDatos data = new BaseDatos();
    

    public string rfc { get; set; }
    public int pais { get; set; }
    public object[] retorno { get; set; }
    public ImprimeTicket()
	{
        _pv = 0;
        _ticket = 0;
	}

    public int PuntoVenta {
        set { _pv = value; }
    }

    public int Ticket {
        set { _ticket = value; }
    }
    public void obtieneCliente()
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "select * from Receptores_f  where ReRFC='" + rfc + "'";
        retorno = ejecuta.dataSet(sql);
    }
    public object[] obtieneDetalle(int ticket ) { 
        string  sql= "Select v.cantidad,p.descripcion,p.idMedida from catproductos p left join venta_det v on p.idproducto = v.id_refaccion where v.ticket=" + ticket;
        return data.scalarData(sql);
    }
    public object[] obtieneenca(int ticket)
    {
        string sql = "select fecha_venta from venta_enc where ticket=" + ticket;
        return data.scalarData(sql);
    }

    public object[] datosTicket() {
        string sql = string.Format("select convert(char(10),v.fecha_venta,126) as fecha_venta,v.hora_venta,v.usuario,(ltrim(rtrim(u.nombre)) +' '+ltrim(rtrim(u.apellido_pat))+' '+ltrim(rtrim(isnull(u.apellido_mat,'')))) as nombre,v.subtotal,v.iva,v.total,v.porc_iva,case v.forma_pago when 'E' then 'Efectivo' when 'D' then 'Tarjeta Débito' when 'A' then 'Tarjeta de Crédito' else '' end as firma_pago,v.referencia_pago,v.cve_Banco,isnull(b.nombreBanco,'') as nombreBanco,v.notas,porc_descuento,descuento,venta_credito " +
            "FROM venta_enc v left join usuarios_PV u on u.usuario=v.usuario left join catbancos b on b.clvbanco=v.cve_Banco where v.ticket={1} and v.id_punto={0}", _pv, _ticket);
        return data.scalarData(sql);
    }

    public object[] datosVtaTaller() {
        string sql = string.Format("SELECT isnull(rp.folio_solicitud,'') as folio_solicitud, orp.no_orden, t.nombre_taller, v.cliente, v.ticket FROM VENTA_ENC V LEFT JOIN Talleres AS t ON V.division_taller = t.id_taller LEFT JOIN Ordenes_Reparacion orp ON v.orden = orp.no_orden and orp.id_taller = v.division_taller left join registro_pinturas rp on rp.ticket = v.ticket WHERE V.TICKET = {0}", _ticket);
        return data.scalarData(sql);
    }

    public bool esVentaTaller()
    {
        string sql = string.Format("select count(*) from venta_enc where id_punto={0} and ticket={1} and ((not orden is null and orden<>0))", _pv, _ticket);
        object[] valor = data.intToBool(sql);
        if (Convert.ToBoolean(valor[0]))
            return Convert.ToBoolean(valor[1]);
        else
            return false;
    }
}