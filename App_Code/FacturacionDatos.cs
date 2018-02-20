using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;
/// <summary>
/// Descripción breve de FacturacionDatos
/// </summary>
public class FacturacionDatos
{
    BaseDatos ejecuta = new BaseDatos();
    Fechas fechas = new Fechas();
    string sql;
    public FacturacionDatos()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public bool validaFechaTicket(string ticket, int isla)
    {
        try
        {
            sql = "select fecha_venta from venta_enc where ticket = " + ticket + " and id_punto = " + isla.ToString() + " and facturado = 0";
            object[] ejecutado = ejecuta.scalarString(sql);
            if ((bool)ejecutado[0])
            {
                DateTime fechaTicket = Convert.ToDateTime(ejecutado[1].ToString());
                TimeSpan diferenciaDias = fechas.obtieneFechaLocal() - fechaTicket;
                int diasDif = diferenciaDias.Days;
                if (diasDif > 3)
                    return false;
                else
                    return true;
            }
            else
                return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    public bool existeTicket(string ticket, int isla)
    {
        try
        {
        sql = "select count(*) from venta_enc where ticket = " + ticket + " and id_punto = " + isla.ToString() + " and facturado = 0";
        object[] ejecutado = ejecuta.intToBool(sql);
        if ((bool)ejecutado[0])
            return (bool)ejecutado[1];
        else
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public string datosEncabezadoVenta(string sql)
    {
        object[] ejecutado = ejecuta.scalarString(sql);
        if ((bool)ejecutado[0])
            return (string)ejecutado[1];
        else
            return "";
    }

    public int existeCliente(string sql)
    {
        try
        {
            object[] ejecutado = ejecuta.scalarInt(sql);
            if ((bool)ejecutado[0])
                return Convert.ToInt32(ejecutado[1]);
            else
                return 0;
        }
        catch (Exception)
        { return 0; }
    }

    public string obtieneDatosCliente(string sql)
    {
        try
        {
            object[] ejecutado = ejecuta.scalarString(sql);
            if ((bool)ejecutado[0])
                return (string)ejecutado[1];
            else
                return "";
        }
        catch (Exception)
        { return ""; }
    }

    public object[] agregaTicket(int isla, string ticket, string usuario)
    {
        sql = "insert into ticketsfacturar values((select isnull((select top 1 consecutivo from ticketsfacturar where usuario = '" + usuario + "' order by consecutivo desc), 0) + 1),'" + usuario + "'," + ticket + "," + isla + ") ";
        return ejecuta.insertUpdateDelete(sql);
    }

    public void borrar(string usuario)
    {
        sql = "delete from ticketsfacturar where usuario='" + usuario + "'";
        ejecuta.insertUpdateDelete(sql);
    }

    public object[] obtieneFacturar(string usuario)
    {
        sql = "select * from ticketsfacturar where usuario = '" + usuario + "' ";
        return ejecuta.dataSet(sql);
    }
}