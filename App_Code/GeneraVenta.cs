using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de GeneraVenta
/// </summary>
public class GeneraVenta
{
    BaseDatos data = new BaseDatos();
	public GeneraVenta()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public object[] generaVenta(object[] ticket) {
        object[] resultado = new object[2];
        string sql = "generaVenta";
        resultado = data.generaTicket(ticket,sql);
        return resultado;
    }

    public object[] actualizarTicketRegistro(string orden, string taller, string registro, string ticket)
    {
        object[] resultado = new object[2];
        string sql = "update registro_pinturas set ticket=" + ticket + " where id_taller=" + taller + " and no_orden=" + orden + " and id_solicitud=" + registro;
        resultado = data.insertUpdateDelete(sql);
        return resultado;
    }
}