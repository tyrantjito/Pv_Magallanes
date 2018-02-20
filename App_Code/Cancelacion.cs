using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using E_Utilities;

/// <summary>
/// Descripción breve de Cancelacion
/// </summary>
public class Cancelacion
{
    Fechas fechas = new Fechas();
    public int punto { get; set; }
    public int caja { get; set; }
    public int ticket { get; set; }
    public string usuario { get; set; }
    public string requisicion { get; set; }
    public string entrega { get; set; }
    public string rfc { get; set; }
    public int cajaTicket { get; set; }    
    public object[] retorno { get; set; }

	public Cancelacion()
	{
        usuario = string.Empty;
        punto = caja = ticket = cajaTicket = 0;        
        retorno = new object[2] { false, "" };
	}




    public void agregaCancelacionPieza(string[] argumentos)
    {
        BaseDatos ejecuta = new BaseDatos();
        decimal cantidad = 0, precio = 0;
        try { cantidad = Convert.ToDecimal(argumentos[2]); }
        catch (Exception) { cantidad = 0; }

        try { precio = Convert.ToDecimal(argumentos[4]); }
        catch (Exception) { precio = 0; }

        decimal importe = cantidad * precio;
        string sql = "declare @cancelacion int declare @ultimoRenglon int declare @existencia decimal(12,3) declare @existenciaActual decimal(12,3) " +
        "set @cancelacion = (select isnull((select top 1 id_cancelacion from cancelaciones_enc order by id_cancelacion desc),0))+1 " +
        "set @ultimoRenglon = (select isnull((select top 1 renglon from Cancelaciones_det where id_cancelacion=@cancelacion order by renglon desc),0))+1 " +
        "set @existencia = (select cantidadExistencia from articulosalmacen where idArticulo='" + argumentos[1] + "' and idalmacen=" + punto + ") " +
        "set @existenciaActual = @existencia+" + cantidad + " " +
        "insert into Cancelaciones_enc values(@cancelacion," + caja + "," + ticket + "," + punto + ",'" + usuario + "'," + importe.ToString("F2") + "," + cajaTicket + ",0,'" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "') " +
        "insert into Cancelaciones_det values(@cancelacion,@ultimoRenglon,'" + argumentos[1] + "','" + argumentos[3] + "'," + precio.ToString("F2") + "," + cantidad.ToString("F3") + "," + importe.ToString("F2") + ") " +
        "update articulosalmacen set cantidadExistencia=@existenciaActual where idArticulo='" + argumentos[1] + "' and idalmacen=" + punto + " " +
        "update registro_pinturas set ticekt=null,total=0 where ticket=" + ticket + " and total in (select (total+iva)from venta_enc where orden<>0 and ticket=" + ticket + " and id_caja =" + caja + ") " +
        "select @cancelacion";
        retorno = ejecuta.scalarInt(sql);

    }

    public void existeCancelacionPrevia(string idPorducto)
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "select count(*) as registros from Cancelaciones_enc c, Cancelaciones_det d where c.ticket=" + ticket + " and c.id_punto=" + punto + " and d.idProducto='" + idPorducto + "'";
        retorno = ejecuta.intToBool(sql);
    }
    public void recuperaDatos()
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "select * from receptores_f where ReRFC='"+rfc+"'";
        retorno = ejecuta.dataSet(sql);
    }

    public void cantidadCancelacionPrevia(string idPorducto, int cajaTicket)
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "select isnull(sum(cantidad),0) as cantidad from Cancelaciones_enc c, Cancelaciones_det d where c.ticket=" + ticket + " and c.id_punto=" + punto + " and d.idProducto='" + idPorducto + "' and c.id_cancelacion=d.id_cancelacion and c.id_caja_ticket=" + cajaTicket;
        retorno = ejecuta.scalarToDecimal(sql);
    }

    public void obtieneCajaTicket()
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "select isnull((select id_caja from venta_enc where ticket=" + ticket + " and id_punto=" + punto + " and id_almacen=" + punto + "),0)";
        retorno = ejecuta.scalarToInt(sql);
    }
    public void actualizaReqEnt()
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = "update  venta_enc set requisicion='"+requisicion+"',entrega='"+entrega+"' where ticket="+ticket;
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void obtieneInfoTicket()
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = " select tabla.renglon,tabla.id_refaccion,tabla.descripcion,tabla.cantidad,tabla.venta_unitaria," +
"tabla.porc_descuento,tabla.valor_descuento,tabla.importe,tabla.cantidadCancelada,(tabla.cantidad-tabla.cantidadCancelada) as cantFaltante from(" +
"select vd.renglon,vd.id_refaccion,vd.descripcion,vd.cantidad,vd.venta_unitaria," +
"vd.porc_descuento,vd.valor_descuento,vd.importe," +
"(select isnull(sum(d.cantidad),0) from Cancelaciones_enc c, Cancelaciones_det d " +
"where c.ticket=ve.ticket and c.id_punto=ve.id_punto and d.idProducto=vd.id_refaccion " +
"and c.id_cancelacion=d.id_cancelacion and c.id_caja_ticket=ve.id_caja) as cantidadCancelada " +
"from venta_enc ve,venta_det vd " +
"where ve.ticket="+ticket+" and ve.id_punto="+punto+" and ve.id_almacen="+punto+" and ve.id_caja="+cajaTicket+" " +
"and ve.ticket=vd.ticket and ve.id_almacen=vd.id_almacen and ve.id_punto=vd.id_punto) " +
"as tabla where tabla.cantidad<>tabla.cantidadCancelada";
        retorno = ejecuta.scalarData(sql);
    }

    protected static SqlConnection ConeccSql()
    {
        SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ConnectionString);
        return sqlConn;
    }

    public void generaCancelacionCompleta(Cancelacion cancelacion, System.Data.DataTable dtd)
    {

        int entFolioID = -1;
        string respuesta = "";
        try
        {
            SqlConnection sqlConn = ConeccSql();
            using (sqlConn)
            {
                sqlConn.Open();
                using (SqlCommand sqlComm = new SqlCommand("cancelaciones", sqlConn))
                {
                    sqlComm.CommandType = CommandType.StoredProcedure;                    
                    sqlComm.Parameters.AddWithValue("@idAlmacen", cancelacion.punto).DbType = DbType.Int16;
                    sqlComm.Parameters.AddWithValue("@idPunto", cancelacion.punto).DbType = DbType.Int16;
                    sqlComm.Parameters.AddWithValue("@caja", cancelacion.caja).DbType = DbType.Int32;
                    sqlComm.Parameters.AddWithValue("@ticket", cancelacion.ticket).DbType = DbType.Int32;                    
                    sqlComm.Parameters.AddWithValue("@usuario", cancelacion.usuario).DbType = DbType.String;
                    sqlComm.Parameters.AddWithValue("@cajaTicket", cancelacion.cajaTicket).DbType = DbType.Int32;
                    sqlComm.Parameters.AddWithValue("@fecha", fechas.obtieneFechaLocal().ToString("yyyy-MM-dd HH:mm:ss") ).DbType = DbType.String;
                    sqlComm.Parameters.AddWithValue("@datos", dtd);
                    sqlComm.Parameters.Add("@respuesta", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;
                    sqlComm.ExecuteNonQuery();
                    try
                    {
                        entFolioID = Convert.ToInt32(sqlComm.Parameters["@respuesta"].Value);
                        respuesta = entFolioID.ToString();
                        retorno[0] = true;
                        retorno[1] = respuesta;
                    }
                    catch (Exception) { 
                        respuesta = Convert.ToString(sqlComm.Parameters["@respuesta"].Value);
                        retorno[0] = false;
                        retorno[1] = respuesta;
                    }
                    
                    
                }
            }
            sqlConn.Close();
        }
        catch (Exception e)
        {
            respuesta =  e.Message;
            retorno[0] = false;
            retorno[1] = respuesta;            
        }        
    }
}