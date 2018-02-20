using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de GastosDatos
/// </summary>
public class GastosDatos
{
    BaseDatos ejecuta = new BaseDatos();
    Fechas fechas = new Fechas();
    string sql;
    public GastosDatos()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public bool agregaGasto(int idAlmacen, int idCaja, string usuario, decimal importe, string justificacion, string referencia)
    {
        sql = "INSERT INTO gastos VALUES((select isnull(count(*),0)+1 from gastos g where g.idAlmacen=" + idAlmacen.ToString() + " and g.id_caja=" + idCaja.ToString() + ")," +
        idAlmacen.ToString() + "," + idCaja.ToString() + ",'" + usuario + "'," + importe.ToString() + ",'" + justificacion + "','" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "','" + referencia + "',0)";
        object[] insertado = ejecuta.insertUpdateDelete(sql);
        if ((bool)insertado[0])
            return (bool)insertado[1];
        else
            return false;
    }
}