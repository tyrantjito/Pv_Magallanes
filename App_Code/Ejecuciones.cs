using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using E_Utilities;

/// <summary>
/// Descripción breve de Ejecuciones
/// </summary>
public class Ejecuciones
{

    SqlConnection conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
    SqlCommand cmd;
    DataSet ds;
    SqlDataAdapter da;
    SqlDataReader dr;    
    Fechas fechas = new Fechas();
	public Ejecuciones()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public object[] scalarToInt(string sql) {
        object[] valor = new object[2] { false, "0" };
        try
        {
            int retorno = 0;
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            retorno = Convert.ToInt32(cmd.ExecuteScalar());
            valor[0] = true;
            valor[1] = retorno.ToString();

        }
        catch (Exception x )
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


    public object[] scalarToDecimal(string sql)
    {
        object[] valor = new object[2] { false, "0" };
        try
        {
            decimal retorno = 0;
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            retorno = Convert.ToDecimal(cmd.ExecuteScalar());
            valor[0] = true;
            valor[1] = retorno.ToString();

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


    public object[] scalarToBool(string sql)
    {
        object[] valor = new object[2] { false, false };
        try
        {
            int retorno = 0;
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            retorno = Convert.ToInt32(cmd.ExecuteScalar());

            valor[0] = true;
            valor[1] = retornaLogico(retorno);

        }
        catch (Exception x )
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

    internal object[] scalarToBoolLog(string sql)
    {
        object[] valor = new object[2] { false, false };
        try
        {
            int retorno = 0;
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            retorno = Convert.ToInt32(cmd.ExecuteScalar());

            valor[0] = true;
            valor[1] = retornaLogico(retorno);

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

    private bool retornaLogico(int valor) {
        if (valor > 0)
            return true;
        else
            return false;
    }

    public object[] dataSet(string sql)
    {
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
        catch (Exception x )
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

    internal object[] insertUpdateDeleteImagenes(string sql, bool[] tieneImagenes, byte[] logo, byte[] icono)
    {
        object[] retorno = new object[2];
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            if(tieneImagenes[0])
                cmd.Parameters.AddWithValue("imagen", logo);
            if(tieneImagenes[1])
                cmd.Parameters.AddWithValue("icono", icono);
            cmd.ExecuteNonQuery();
            retorno[0] = true;
            retorno[1] = true;
        }
        catch (Exception x) { retorno[0] = false; retorno[1] = x.Message; }
        finally
        {
            //conexionBD.Dispose();
            conexionBD.Close();
        }
        return retorno;
    }

    internal object[] exeStoredOrden(string sql, object[] datosOrden)
    {
        object[] retorno = new object[2] { false, "" };
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idEmpresa", datosOrden[0]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@idTaller", datosOrden[1]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@placa", datosOrden[2]).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@tOrden", datosOrden[3]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@cliente", datosOrden[4]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@tServicio", datosOrden[5]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@tValuacion", datosOrden[6]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@tAsegurado", datosOrden[7]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@idEmpleado", datosOrden[8]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@idMarca", datosOrden[9]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@idTvehiculo", datosOrden[10]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@idTUnidad", datosOrden[11]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@idVehiculo", datosOrden[12]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@idUsuario", datosOrden[13]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@localizacion", datosOrden[14]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@perfil", datosOrden[15]).SqlDbType = SqlDbType.Int;
            DateTime fechaActual = fechas.obtieneFechaLocal();
            cmd.Parameters.AddWithValue("@fechaActual", fechaActual).SqlDbType = SqlDbType.DateTime;

            SqlParameter ordenObten = new SqlParameter("@respuesta", 0);
            ordenObten.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ordenObten);
            cmd.ExecuteNonQuery();
            string ordenRetornada = cmd.Parameters["@respuesta"].Value.ToString();
            retorno[0] = true;
            retorno[1] = ordenRetornada;
        }
        catch (Exception x ) { retorno[0] = false; retorno[1] = "Detalle: " + x.Message; }
        finally
        {
            //conexionBD.Dispose();
            conexionBD.Close();
        }
        return retorno;
    }

    internal object[] insertUpdateDelete(string sql)
    {
        object[] retorno= new object[2];
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.ExecuteNonQuery();
            retorno[0] = true;
            retorno[1] = true;
        }
        catch (Exception x ) { retorno[0] = false; retorno[1] = x.Message; }
        finally
        {
            //conexionBD.Dispose();
            conexionBD.Close();
        }
        return retorno;
    }

    public object[] scalarToString(string sql)
    {
        object[] valor = new object[2] { false, "" };
        try
        {
            string retorno = "";
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            retorno = Convert.ToString(cmd.ExecuteScalar());
            valor[0] = true;
            valor[1] = retorno.ToString();

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

    internal string scalarToStringSimple(string sql)
    {
        string valor = "";
        try
        {            
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            valor = Convert.ToString(cmd.ExecuteScalar());
        }
        catch (Exception x )
        {
            valor = "";
        }
        finally
        {
            conexionBD.Close();
        }
        return valor;
    }

    internal object[] insertAdjuntos(string sql, byte[] imagen)
    {
        object[] retorno = new object[2];
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.Parameters.AddWithValue("imagen", imagen);
            cmd.ExecuteNonQuery();
            retorno[0] = true;
            retorno[1] = true;
        }
        catch (Exception x) { retorno[0] = false; retorno[1] = x.Message; }
        finally
        {
            //conexionBD.Dispose();
            conexionBD.Close();
        }
        return retorno;
    }


    public object[] exeStoredCotizacion(string sql, int empresa, int taller, int orden, int cotizacion, int proveedor, string folio, DateTime fechaLocal)
    {
        object[] retorno = new object[2] { false, "" };
        string cotizacionRetorno = "";
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idEmpresa", empresa);
            cmd.Parameters.AddWithValue("@idTaller", taller);            
            cmd.Parameters.AddWithValue("@orden", orden);
            cmd.Parameters.AddWithValue("@proveedor", proveedor);
            cmd.Parameters.AddWithValue("@cotizacion", cotizacion);
            cmd.Parameters.AddWithValue("@fechaActual", fechaLocal);
            cmd.Parameters.AddWithValue("@folio", folio);

            SqlParameter ordenObten = new SqlParameter("@respuesta", 0);
            ordenObten.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ordenObten);
            cmd.ExecuteNonQuery();
            cotizacionRetorno = cmd.Parameters["@respuesta"].Value.ToString();
            retorno[0] = true;
            retorno[1] = cotizacionRetorno;
        }
        catch (Exception x) { 
            retorno[0] = false;
            if (cotizacionRetorno == "")
                retorno[1] = "Error: " + x.Message;
            else
                retorno[1] = cotizacionRetorno; }
        finally
        {
            //conexionBD.Dispose();
            conexionBD.Close();
        }
        return retorno;
    }

    internal object[] exeStoredOrdenCompra(int[] sesiones, string sql, int proveedor, int autoriza, int acceso, DateTime fecha)
    {
        object[] retorno = new object[2] { false, "" };
        string cotizacionRetorno = "";
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@idEmpresa", sesiones[2]);
            cmd.Parameters.AddWithValue("@idTaller", sesiones[3]);
            cmd.Parameters.AddWithValue("@orden", sesiones[4]);
            cmd.Parameters.AddWithValue("@idCotizacion", sesiones[6]);
            cmd.Parameters.AddWithValue("@proveedor", proveedor);
            cmd.Parameters.AddWithValue("@acceso", acceso);
            cmd.Parameters.AddWithValue("@fechaActual", fecha.ToString("yyyy-MM-dd HH:mm:ss"));
            SqlParameter ordenObten = new SqlParameter("@respuesta", 0);//("@respuesta", 0);
            ordenObten.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(ordenObten);
            cmd.ExecuteNonQuery();
            cotizacionRetorno = cmd.Parameters["@respuesta"].Value.ToString();
            retorno[0] = true;
            retorno[1] = cotizacionRetorno;
        }
        catch (Exception x)
        {
            retorno[0] = false;
            if (cotizacionRetorno == "")
                retorno[1] = "Error: " + x.Message;
            else
                retorno[1] = cotizacionRetorno;
        }
        finally
        {            
            conexionBD.Close();
        }
        return retorno;
    }

    internal object[] exeStoredRemisionesSS(string sql, int[] sesiones, DataTable dt, decimal totalManoObra, decimal totalRefacciones, decimal totalTotal, string tipo, DateTime fecha, string comentarios)
    {
        object[] retorno = new object[2] { false, "" };
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(sql, conexionBD);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@empresa", sesiones[2]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@taller", sesiones[3]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@orden", sesiones[4]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@tipo", tipo).SqlDbType = SqlDbType.Char;
            cmd.Parameters.AddWithValue("@fecha", fecha).SqlDbType = SqlDbType.DateTime;
            cmd.Parameters.AddWithValue("@totalMo", totalManoObra).SqlDbType = SqlDbType.Decimal;
            cmd.Parameters.AddWithValue("@totalRef", totalRefacciones).SqlDbType = SqlDbType.Decimal;
            cmd.Parameters.AddWithValue("@total", totalTotal).SqlDbType = SqlDbType.Decimal;
            cmd.Parameters.AddWithValue("@usuario", sesiones[0]).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@comentarios", comentarios).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@detalle",dt);            

            SqlParameter remision = new SqlParameter("@respuesta", 0);
            remision.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(remision);
            cmd.ExecuteNonQuery();
            string valoresRetorno = cmd.Parameters["@respuesta"].Value.ToString();
            retorno[0] = true;
            retorno[1] = valoresRetorno;
        }
        catch (Exception x) { retorno[0] = false; retorno[1] = x.Message; }
        finally
        {
            //conexionBD.Dispose();
            conexionBD.Close();
        }
        return retorno;
    }

}