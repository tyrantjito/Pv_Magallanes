using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Inventario
/// </summary>
public class Inventario
{
	public Inventario()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public object[] actualizaEstatus(int ticket, int punto, string estatus)
    {
        BaseDatos ejecuta = new BaseDatos();
        string sql = string.Format("update venta_enc set estatus='{0}',facturado=1 where ticket={1} and id_punto={2}", estatus, ticket, punto);
        object[] insertado = ejecuta.insertUpdateDelete(sql);
        return insertado;
    }
}