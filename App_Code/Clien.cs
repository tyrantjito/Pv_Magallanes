using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Clien
/// </summary>
public class Clien
{

    Ejecuciones ejecutar = new Ejecuciones();
    BaseDatos ejecuta = new BaseDatos();

    public string rfc { get; set; }
    public string razon { get; set; }
    public string calle { get; set; }
    public string numero { get; set; }
    public string numeroint { get; set; }
    public int pais { get; set; }
    public int idmatriz { get; set; }
    public string localidad { get; set; }
    public string correo { get; set; }
    public string rec { get; set; }
    public string correocc { get; set; }
    public string correocco { get; set; }
    public object[] retorno { get; set; }
    public string referencia { get; set; }
    public int colonia { get; set; }
    public int municipio { get; set; }
    public int estado { get; set; }
    public int cp { get; set; }


    public Clien()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }


    public void ingresaCliente()
    {
        string sql = "insert into Receptores_f values((select isnull((select top 1 IdRecep from Receptores_f order by IdRecep desc),0)+1),'" + rfc+ "',(select isnull((select top 1 idMatriz from Receptores_f order by idMatriz desc),0)+1),'" + razon + "','"+correo+"','"+correocc+"','"+correocco+"','"+calle+"','"+numero+"','"+numeroint+"','"+localidad+"','"+referencia+"',"+pais+","+estado+","+municipio+","+colonia+","+cp+",'"+rec+"')";
        retorno = ejecuta.insertUpdateDelete(sql);
    }

    public void actualizaCliente()
    {
        string sql = "update receptores_f set reRFC='"+rfc+"',ReNombre='"+razon+"',ReCorreo='"+correo+"',ReCorreoCC='"+correocc+"',ReCorreoCCO='"+correocco+"',ReCalle='"+calle+"',ReNoExt='"+numero+"',ReNoInt='"+numeroint+"',ReLocalidad='"+localidad+"',ReReferencia='"+referencia+"',Recve_pais="+pais+" ,Re_ID_Estado="+estado+" ,Re_ID_Del_Mun="+municipio+" ,ReID_Cod_Pos="+cp+" where ReRFC='"+rfc+"'";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
}