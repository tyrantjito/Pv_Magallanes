using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using E_Utilities;

/// <summary>
/// Descripción breve de Facturas
/// </summary>
public class Facturas
{
    Fechas fechas = new Fechas();
    private SqlConnection conexionBD;
    private SqlCommand cmd;
    private SqlDataAdapter da;
    private DataSet ds;

    public int renglon { get; set; }
    public string factura { get; set; }
    public DateTime fechaRevision { get; set; }
    public DateTime fechaProgPago { get; set; }
    public DateTime fechaPago { get; set; }
    public string formaPago { get; set; }
    public string referenciaPago { get; set; }
    public string clvBanco { get; set; }
    public string observaciones { get; set; }
    public int id_cliprov { get; set; }
    public string tipoCuenta { get; set; }
    public string estatus { get; set; }
    public int folio { get; set; }
    public int taller { get; set; }
    public int empresa { get; set; }
    public string tipoCargo { get; set; }
    public decimal Importe { get; set; }
    public decimal porcentajePP { get; set; }
    public decimal importePP { get; set; }
    public decimal montoPagar { get; set; }
    public string concepto { get; set; }
    public decimal impDescuento { get; set; }
    public decimal porcentajeDesc { get; set; }
    public int orden { get; set; }
    public string estatusFactura { get; set; }
    public string politica { get; set; }
    public int idCfd { get; set; }
    public object[] retorno { get; set; }
    public bool pagado { get; set; }
    public string razon_social { get; set; }
    public decimal monto { get; set; }

    public Facturas()
	{
        renglon = id_cliprov = folio = taller = empresa = orden = 0;
        factura = formaPago = referenciaPago = clvBanco = observaciones = tipoCuenta = estatus = tipoCargo = concepto = estatusFactura = politica = razon_social = string.Empty;
        fechaRevision = fechaProgPago = fechaPago = fechas.obtieneFechaLocal();
        Importe = porcentajePP = importePP = montoPagar = impDescuento = porcentajeDesc = monto = 0;
        pagado = false;
        retorno = new object[2] { false, "" };
	}

    public void generaFactura() {
        string sql = "Declare @renglon int set @renglon=(select isnull((select top 1 renglon from Facturas_f where TipoCuenta='" + tipoCuenta + "' and folio=" + orden + " order by renglon desc),0))+1 " +
                    "insert into Facturas_f (Renglon,TipoCuenta,Folio,Factura,id_cliprov,clv_politica,Estatus,id_empresa,id_taller,no_orden,TipoCargo,importe,porcentaje_pp,importe_pp,monto_pagar,razon_social,monto) values(@renglon,'" + tipoCuenta + "'," + orden + ",'" + factura + "'," + id_cliprov + ",'" + politica + "','" + estatus + "'," + empresa + "," + taller + "," + orden + ",'C'," + Importe + "," + porcentajePP + "," + importePP + "," + montoPagar + ",'" + razon_social + "'," + monto + ")";
        retorno = insertUpdateDelete(sql);
        if (estatus == "PAG") {
            string fecha = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
            sql = "update facturas_f set fecharevision='" + fecha + "', fechaprogpago='" + fecha + "',fechapago='" + fecha + "' where factura='" + factura + "' and tipocuenta='" + tipoCuenta + "' AND FOLIO=" + orden + " AND ID_EMPRESA=" + empresa + " AND ID_TALLER=" + taller + " AND ID_CLIPROV=" + id_cliprov;
            insertUpdateDelete(sql);
        }
    }
    
    public object[] dataSet(string sql)
    {
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
        object[] valor = new object[2] { false, false };
        ds = new DataSet();
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            valor[0] = true;
            valor[1] = ds;

        }
        catch (Exception x)
        {
            valor[0] = false;
            valor[1] = x.Message;
        }
        finally
        {
            conexionBD.Close();
        }
        return valor;
    }

    internal object[] insertUpdateDelete(string sql)
    {
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
        object[] retorno = new object[2];
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.ExecuteNonQuery();
            retorno[0] = true;
            retorno[1] = true;
        }
        catch (Exception x) { retorno[0] = false; retorno[1] = x.Message; }
        finally
        {            
            conexionBD.Close();
        }
        return retorno;
    }

    public object[] scalarInt(string query)
    {
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
        object[] valor = new object[2] { false, "" };
        try
        {
            int retorno = -10;
            conexionBD.Open();
            cmd = new SqlCommand(query, conexionBD);
            retorno = Convert.ToInt32(cmd.ExecuteScalar());
            valor[0] = true;
            valor[1] = retorno;

        }
        catch (Exception ex)
        {
            valor[0] = false;
            valor[1] = ex.Message;
        }
        finally
        {
            conexionBD.Close();
        }
        return valor;
    }

    public object[] scalarDecimal(string query)
    {
        conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
        object[] valor = new object[2] { false, "" };
        try
        {
            decimal retorno = 0;
            conexionBD.Open();
            cmd = new SqlCommand(query, conexionBD);
            retorno = Convert.ToDecimal(cmd.ExecuteScalar());
            valor[0] = true;
            valor[1] = retorno;

        }
        catch (Exception ex)
        {
            valor[0] = false;
            valor[1] = ex.Message;
        }
        finally
        {
            conexionBD.Close();
        }
        return valor;
    }
   
    public void generaFacturaCC()
    {
        string sql = "Declare @renglon int set @renglon=(select isnull((select top 1 renglon from Facturas_f where TipoCuenta='" + tipoCuenta + "' and folio=" + folio + " order by renglon desc),0))+1 " +
                    "insert into Facturas_f (Renglon,TipoCuenta,Folio,Factura,id_cliprov,clv_politica,Estatus,id_empresa,id_taller,no_orden,TipoCargo,fechaRevision,fechaprogpago,formapago,importe,idCfd,razon_social,monto) " +
                    "values(@renglon,'" + tipoCuenta + "'," + folio + ",'" + factura + "'," + id_cliprov + ",'" + politica + "','" + estatus + "'," + empresa + "," + taller + "," + orden + ",'" + tipoCargo + "','" + fechaRevision.ToString("yyyy-MM-dd") + "','" + fechaProgPago.ToString("yyyy-MM-dd") + "','" + formaPago + "'," + Importe + "," + idCfd + ",'" + razon_social + "'," + monto + ")";
        retorno = insertUpdateDelete(sql);
    }

    public void obtieneInfoFactura()
    {
        string sql = "select FechaRevision,FechaProgPago,FechaPago,FormaPago,ReferenciaPago," +
              "clvBanco,Observaciones,id_cliprov,clv_politica,concepto,porcentaje_pp,Porcentaje_desc,importe_pp,monto_pagar,id_nota_credito,firmante,razon_social" +
              " from facturas_f where TipoCuenta = '" + tipoCuenta + "' and factura = '" + factura + "' and folio = " + folio + " and id_cliprov=" + id_cliprov + " and id_empresa = " + empresa + " and id_taller = " + taller + " and no_orden=" + orden; //+ " and renglon=" + renglon;
        retorno = dataSet(sql);
    }

    public void actualizaFacturaCC()
    {
        string sql = "update Facturas_f set id_cliprov=" + id_cliprov + ",clv_politica='" + politica + "',importe=" + Importe + ", razon_social='" + razon_social + "' where tipoCuenta='" + tipoCuenta + "' and factura='" + factura + "' and folio=" + folio + " and idCfd=" + idCfd;
        retorno = insertUpdateDelete(sql);
    }

    public void actualizaFacturas(string sql, int idCfd)
    {
        string condicion = "";
        if(idCfd!=0)
        {
            condicion = " and idcfd=" + idCfd.ToString();
        }
        sql = sql + condicion;
        retorno = insertUpdateDelete(sql);

    }

    public void existeFactura()
    {
        string sql = "select count(*) from facturas_f where folio=" + folio + " and TipoCuenta='" + tipoCuenta + "' and no_orden=" + orden + " and id_empresa=" + empresa + " and id_taller=" + taller + " and Factura='" + factura + "'";
        retorno = scalarInt(sql);
    }

    public void agregaPagoAnticipado(string sql)
    {        
        retorno = insertUpdateDelete(sql);
    }

    public void obtieneUltimoResto()
    {
        string sql = "select isnull((select top 1 monto_restante from detallefacturas_f where renglon=" + renglon + " and factura='" + factura + "' and tipocuenta='" + tipoCuenta + "' and folio =" + folio + " order by pago desc),0) as restante";
        retorno = scalarDecimal(sql);
    }

    public void actualizaTotalFactura()
    {
        string sql = "update facturas_f set importe="+Importe+", monto_pagar="+montoPagar+" where factura='" + factura + "' and tipocuenta='" + tipoCuenta + "' AND FOLIO=" + orden + " AND ID_EMPRESA=" + empresa + " AND ID_TALLER=" + taller + " AND ID_CLIPROV=" + id_cliprov;
        insertUpdateDelete(sql);
    }

    public void obtieneFolioFactura()
    {
        string sql = "select folio from facturas_f where TipoCuenta='" + tipoCuenta + "' and no_orden=" + orden + " and id_empresa=" + empresa + " and id_taller=" + taller + " and Factura='" + factura + "'";
        retorno = scalarInt(sql);        
    }
}