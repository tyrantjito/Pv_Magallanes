using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de ClientesDatos
/// </summary>
public class ClientesDatos
{
    string sql;
    BaseDatos ejecuta = new BaseDatos();
    Fechas fechas = new Fechas();
    public ClientesDatos()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public bool agregaCliente(string rfc, string razon, string calle, string numero, string colonia, string cp , string ciudad, string estado, string tipoPersona, string paterno, string materno, string nombre, string correo, string sexo, string edad, string fechaNacimiento, string numeroInt, string delegacion, string referncia, string pais)
    {
        sql = "INSERT INTO clientes" +
            "(Clave, RFC, ArchivarNombre, Calle, NumeroCalle, Colonia, CodigoPostal, Ciudad" +
            ", Estado, PersonaFisicaMoral, FechaAlta, ApellidoPaterno, ApellidoMaterno, Nombres" +
            ", Email, DomicilioCompleto, Sexo, Edad, FechaNacimiento, Num_int, Del_Municip" +
            ", Referencia, Pais) VALUES" +
            "  ((select isnull((select top 1 Clave from clientes order by clave desc),0)+1)" +
            ",'" + rfc + "','" + razon + "','" + calle + "','" + numero + "','" + colonia + "'," + cp.ToString() + ",'" + ciudad + "'" +
            ",'" + estado + "','" + tipoPersona + "','" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + paterno + "'" +
            ",'" + materno + "','" + nombre + "','" + correo + "','" 
            + (calle + " " + numero + " " + numeroInt + " " + colonia + " " + cp + " " + delegacion + " " + ciudad + " " + estado + " " + pais) + "'" +
            ",'" + sexo + "','" + edad + "','" + fechaNacimiento + "','" + numeroInt + "','" + delegacion + "'" +
            ",'" + referncia + "','" + pais + "')";
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if ((bool)ejecutado[0])
            return (bool)ejecutado[1];
        else
            return false;
    }

    public bool existeClienteRFC(string rfc)
    {
        sql = "select isnull(count(*),0) from clientes where RFC='" + rfc + "'";
        object[] ejecutado = ejecuta.intToBool(sql);
        if ((bool)ejecutado[0])
            return (bool)ejecutado[1];
        else
            return false;
    }

    public bool actualizaVentaEncabezado(int idCliente, string isla, string ticket, int desglosado, string fecha, string hora, string tickets)
    {
        sql = "update venta_enc set factura_posterior=1, id_cliente=" + idCliente.ToString() + ", desglosado=" + desglosado + ", fecha='" + fecha + "', hora='" + hora + "', tickets='" + tickets + "'  where ticket=" + ticket + " and id_punto=" + isla;
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if ((bool)ejecutado[0])
            return (bool)ejecutado[1];
        else
            return false;
    }

    public int obtieneIdClienteRFC(string rfc)
    {
        sql = "select isnull((select Clave from clientes where rfc='" + rfc + "'),0)";
        object[] ejecutado = ejecuta.scalarString(sql);
        if ((bool)ejecutado[0])
            return Convert.ToInt32(ejecutado[1]);
        else
            return 0;
    }

    public object[] obtieneDatosCliente(string rfc)
    {
        sql = "select ArchivarNombre,Calle,NumeroCalle,Num_int,CodigoPostal,Ciudad,Estado,PersonaFisicaMoral,ApellidoPaterno,ApellidoMaterno,Nombres,Email,Colonia,Del_Municip,Referencia from clientes where rfc='" + rfc + "'";
        object[] ejecutado = ejecuta.scalarData(sql);
        return ejecutado;
    }

    public bool actualizaCliente(string rfc, string razon, string calle, string numero, string colonia, string cp, string ciudad, string estado, string tipoPersona, string paterno, string materno, string nombre, string correo, string sexo, string edad, string fechaNacimiento, string numeroInt, string delegacion, string referncia, string pais, int id)
    {
        if (id > 0)
        {
            sql = "update clientes " +
                "set RFC='" + rfc + "', ArchivarNombre='" + razon + "', Calle='" + calle + "', NumeroCalle='" + numero + "', Colonia='" + colonia + "', CodigoPostal=" + cp.ToString() + ", Ciudad='" + ciudad + "', " +
                "Estado='" + estado + "', PersonaFisicaMoral='" + tipoPersona + "', ApellidoPaterno='" + paterno + "', ApellidoMaterno='" + materno + "', Nombres='" + nombre + "', Email='" + correo + "', " +
                "DomicilioCompleto='" + ("Calle: " + calle + " No.: " + numero + "  Int.: " + numeroInt + ", Colonia: " + colonia + " ,C.P.: " + cp + ", Deleg. o Municip.: " + delegacion + ", Ciudad: " + ciudad + ", Estado; " + estado + ", País: " + pais) + "', " +
                "Edad=" + edad + ", FechaNacimiento='" + fechaNacimiento + "', Num_int='" + numeroInt + "', Del_Municip='" + delegacion + "', Referencia='" + referncia + "', Pais='" + pais + "',sexo='" + sexo + "' where clave=" + id.ToString();
        }
        else {
           sql= "INSERT INTO clientes" +
                        "(Clave, RFC, ArchivarNombre, Calle, NumeroCalle, Colonia, CodigoPostal, Ciudad" +
                        ", Estado, PersonaFisicaMoral, FechaAlta, ApellidoPaterno, ApellidoMaterno, Nombres" +
                        ", Email, DomicilioCompleto, Sexo, Edad, FechaNacimiento, Num_int, Del_Municip" +
                        ", Referencia, Pais) VALUES" +
                        "  ((select isnull((select top 1 Clave from clientes order by clave desc),0)+1)" +
                        ",'" + rfc + "','" + razon + "','" + calle + "','" + numero + "','" + colonia + "'," + cp.ToString() + ",'" + ciudad + "'" +
                        ",'" + estado + "','" + tipoPersona + "','" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + paterno + "'" +
                        ",'" + materno + "','" + nombre + "','" + correo + "','"
                        + (calle + " " + numero + " " + numeroInt + " " + colonia + " " + cp + " " + delegacion + " " + ciudad + " " + estado + " " + pais) + "'" +
                        ",'" + sexo + "','" + edad + "','" + fechaNacimiento + "','" + numeroInt + "','" + delegacion + "'" +
                        ",'" + referncia + "','" + pais + "')";
        }
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if ((bool)ejecutado[0])
            return (bool)ejecutado[1];
        else
            return false;
    }

    public bool ticketSolicitado(string isla, string ticket)
    {
        sql = "Select count(*) from venta_enc where ticket=" + ticket + " and id_punto=" + isla + " and factura_posterior=1";
        object[] ejecutado = ejecuta.intToBool(sql);
        if ((bool)ejecutado[0])
            return (bool)ejecutado[1];
        else
            return true;
    }
}