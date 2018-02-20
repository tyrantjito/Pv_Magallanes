using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using E_Utilities;

/// <summary>
/// Descripción breve de Receptores
/// </summary>
public class Receptores
{
    
    SqlConnection conexionBD = new SqlConnection(ConfigurationManager.ConnectionStrings["PVW"].ToString());
    SqlCommand cmd;
    DataSet ds;
    SqlDataAdapter da;
    SqlDataReader dr;
    private string sql;
    public int idReceptor { get; set; }
    public object[] info { get; set; }
    public Receptores()
    {
        sql = string.Empty;
        idReceptor = 0;
        info = new object[2] { false, "" };
    }
    
    public void obtieneInfoReceptor()
    {        
        sql = "select * from Receptores_F  where IdRecep=" + idReceptor.ToString();
        info = dataSet(sql);
    }

    public void agregarActualizarReceptor(string rfc, string nombre, string calle, string ext, string inte, string localidad, string referencia, string correo, string pais, string estado, string municipio, string colonia, string cp, string cc, string cco)
    {        
        existeReceptor(rfc);
        if (Convert.ToBoolean(info[0]))
        {
            int existe = Convert.ToInt32(info[1]);
            if (existe > 0)
            {
                obtieneIdReceptor(rfc);
                if (Convert.ToBoolean(info[0]))
                {
                    int id = Convert.ToInt32(info[1]);
                    sql = string.Format("update receptores_f set reRFC='{0}', reNombre='{1}', reCorreo='{2}',reCalle='{3}',reNoExt='{4}',reNoInt='{5}', reLocalidad='{6}',reReferencia='{7}',Recve_pais={8} ,Re_ID_Estado={9},Re_ID_Del_Mun={10},ReID_Colonia={11},ReID_Cod_Pos='{12}',ReCorreoCC='{14}',ReCorreoCCO='{15}' where idRecep={13}", rfc.ToUpper().Trim(), nombre.Trim().ToUpper(), correo.Trim().ToLower(), calle.ToUpper().Trim(), ext.Trim().ToUpper(), inte.ToUpper().Trim(), localidad.Trim().ToUpper(), referencia.ToUpper().Trim(), pais, estado, municipio, colonia, cp, id, cc.Trim().ToLower(), cco.Trim().ToLower());
                    info = insertUpdateDelete(sql);
                }
            }
            else
            {
                sql = string.Format("insert into Receptores_f (ReRFC,ReNombre,ReCorreo,ReCalle,ReNoExt,ReNoInt,ReLocalidad,ReReferencia,Recve_pais,Re_ID_Estado,Re_ID_Del_Mun,ReID_Colonia,ReID_Cod_Pos,recorreocc,recorreocco) values" +
                    "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},{9},{10},{11},'{12}','{13}','{14}')",
                    rfc.ToUpper().Trim(), nombre.Trim().ToUpper(), correo.Trim().ToLower(), calle.ToUpper().Trim(), ext.Trim().ToUpper(), inte.ToUpper().Trim(), localidad.Trim().ToUpper(), referencia.ToUpper().Trim(), pais, estado, municipio, colonia, cp, cc.Trim().ToLower(), cco.Trim().ToLower());
                info = insertUpdateDelete(sql);
            }
        }
    }

    public void existeReceptor(string rfc)
    {        
        sql = string.Format("select count(*) from Receptores_f where RTRIM(LTRIM(reRFC))='{0}'", rfc.ToUpper().Trim());
        info = scalarToInt(sql);
    }

    public void obtieneIdReceptor(string rfc)
    {        
        sql = string.Format("select idRecep from Receptores_f where RTRIM(LTRIM(reRFC))='{0}'", rfc.Trim().ToUpper());
        info = scalarToInt(sql);
    }

    public void tieneRelacion()
    {        
        sql = string.Format("select count(*) from enccfd_f where idrecep={0}", idReceptor);
        info = scalarToInt(sql);
    }

    public void eliminar()
    {        
        sql = string.Format("delete from Receptores_f where idrecep={0}", idReceptor);
        info = insertUpdateDelete(sql);
    }

    private object[] scalarToInt(string query)
    {
        object[] valor = new object[2] { false, 0 };
        try
        {
            int retorno = 0;
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

    private object[] dataSet(string sql)
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
    private object[] insertUpdateDelete(string query)
    {
        object[] retorno = new object[2];
        try
        {
            conexionBD.Open();
            cmd = new SqlCommand(query, conexionBD);
            cmd.ExecuteNonQuery();
            retorno[0] = true;
            retorno[1] = true;
        }
        catch (Exception ex) { retorno[0] = false; retorno[1] = ex.Message; }
        finally
        {
            conexionBD.Close();
        }
        return retorno;
    }
}