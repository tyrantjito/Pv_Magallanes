using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


/// <summary>
/// Descripción breve de CatClientes
/// </summary>
public class CatClientes
{
    Ejecuciones ejecuta = new Ejecuciones();
    string sql = "";
	public CatClientes()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public bool insertaCatCliente(string descripcion, string descMo, string descRef, string @prefijo)
    {
        bool resultado = false;
        try
        {
            sql = "insert into Categoria_Cliente (id_cat_cliente,descripcion,desc_mo,desc_ref,prefijo)values(" +
                    "(select isnull(count(cc.id_cat_cliente) + 1, 0) from Categoria_Cliente cc),'" + descripcion + "'," + descMo.ToString() + "," + descRef.ToString() + ",'" + prefijo + "')";
            object[] ejecutado = ejecuta.insertUpdateDelete(sql);
            resultado = Convert.ToBoolean(ejecutado[0]);
            if (resultado)
            {
                resultado = Convert.ToBoolean(ejecutado[1]);
            }
            else
                resultado = false;
        }
        catch (Exception x )
        {
            resultado = false;
        }
        return resultado;
    }

    public object[] insertaActualizaCliente(int id, string tipo, string persona, string rfc, string sexo, string fecha, int edad, string razon, string nombre, string ap, string am, string calle, string numExt, string numInt, string colonia, string cp, string municipio, string estado, string pais, string tel1, string tel2, string tel3, int diaRevision, int diaCobranza, int politica, bool aseguradora, decimal descuento, byte[] logo, string correo, int rgbR, int rgbG, int rgbB, byte[] icono, string localidad, string referencia, int zona, string idSeleccionado, string noProveedor)
    {
        int esAseguradora = 0;
        bool[] tieneImagenes = new bool[2] { false, false };
        if (aseguradora)
            esAseguradora = 1;
        string imagen1 = "";
        if (logo != null)
        {
            imagen1 = "@imagen";
            tieneImagenes[0] = true;
        }
        else
            imagen1 = "null";

        string imagen2 = "";
        if (icono != null)
        {
            imagen2 = "@icono";
            tieneImagenes[1] = true;
        }
        else
            imagen2 = "null";

        if (persona == "F")
            razon = nombre.Trim() + " " + ap.Trim() + " " + am.Trim();
        if (idSeleccionado == "")
            sql = "insert into Cliprov values(isnull((select top 1 id_cliprov from Cliprov c where c.tipo='" + tipo + "' order by id_cliprov desc),0)+1,'" + tipo + "','" + persona + "','" + rfc.ToUpper() + "','" + sexo + "','" + fecha + "'," + edad + ",'" + razon.ToUpper().Trim() + "','" + nombre.ToUpper().Trim() + "','" + ap.ToUpper().Trim() + "','" + am.ToUpper().Trim() + "','" + calle.ToUpper().Trim() + "','" + numExt + "','" + numInt + "','" + colonia.ToUpper().Trim() + "','" + cp.PadLeft(5, '0') + "','" + municipio.ToUpper().Trim() + "','" + estado.ToUpper().Trim() + "','" + pais.ToUpper().Trim() + "','" + tel1 + "','" + tel2 + "','" + tel3 + "'," + diaRevision + "," + diaCobranza + "," + politica + "," + esAseguradora + "," + descuento + "," + imagen1 + ",'A','" + correo + "'," + rgbR + "," + rgbG + "," + rgbB + "," + imagen2 + ",'" + localidad + "','" + referencia + "','"+noProveedor+"'," + zona + ")";
        else
            sql = "update cliprov set persona='" + persona + "',rfc='" + rfc.ToUpper() + "',sexo='" + sexo + "',fecha_nacimiento='" + fecha + "',edad=" + edad + ",razon_social='" + razon.ToUpper().Trim() + "',nombre='" + nombre.ToUpper().Trim() + "',ap_paterno='" + ap.ToUpper().Trim() + "',ap_materno='" + am.ToUpper().Trim() + "',calle='" + calle.ToUpper().Trim() + "',num_ext='" + numExt + "',num_int='" + numInt + "',colonia='" + colonia.ToUpper().Trim() + "',cp='" + cp.PadLeft(5, '0') + "',municipio='" + municipio.ToUpper().Trim() + "',estado='" + estado.ToUpper().Trim() + "',pais='" + pais.ToUpper().Trim() + "',tel_particular='" + tel1 + "',tel_oficina='" + tel2 + "',tel_celular='" + tel3 + "',dia_revision=" + diaRevision + ",dia_cobranza=" + diaCobranza + ",id_politica=" + politica + ",aseguradora=" + esAseguradora + ",porc_descuento=" + descuento + ",logo=" + imagen1 + ",correo='" + correo + "',rgb_r=" + rgbR + ",rgb_g=" + rgbG + ",rgb_b=" + rgbB + ",icono_est=" + imagen2 + ",localidad='" + localidad + "',referencia='" + referencia + "',id_zona=" + zona + ",noProveedor='"+noProveedor+"' where id_cliprov=" + id + " and tipo='" + tipo + "'";
        return ejecuta.insertUpdateDeleteImagenes(sql, tieneImagenes, logo, icono);
    }

    public bool insertaClienteBasicos(string rfc, string razon, string nombre, string apPaterno, string apMaterno, bool aseguradora, string tipo,string fecha, string tipoD,string correo)
    {
        bool insertado = false;
        try
        {
            int asegu = 0;
            if (aseguradora)
                asegu = 1;            
            sql = "insert into Cliprov (id_cliprov,tipo,persona,rfc,fecha_nacimiento,razon_social,nombre,ap_paterno,ap_materno,aseguradora,id_politica,correo) " +
            "values(isnull((select top 1 id_cliprov from Cliprov c where c.tipo='"+tipoD+"' order by id_cliprov desc),0)+1,'" + tipoD + "','" + tipo + "','" + rfc + "','" + fecha + "','" + razon + "','" + nombre + "','" + apPaterno + "','" + apMaterno + "'," + asegu + ",0,'" + correo + "')";
            object[] ejecutado = ejecuta.insertUpdateDelete(sql);
            insertado = Convert.ToBoolean(ejecutado[0]);
            if (insertado)
            {
                insertado = Convert.ToBoolean(ejecutado[1]);
            }
            else
                insertado = false;
        }
        catch (Exception x )
        {
            insertado = false;
        }
        return insertado;
    }

    public DataSet llenaDetalle(string id, string tipo)
    {
        DataSet datas = new DataSet();
        sql = "select id_cliprov,tipo,persona,rfc,sexo,fecha_nacimiento,edad,razon_social,nombre,ap_paterno,ap_materno,calle,num_ext,num_int,colonia,cp,municipio,estado,pais,tel_particular,tel_oficina,tel_celular,dia_revision,dia_cobranza,id_politica,aseguradora,porc_descuento,logo,isnull(id_zona,0) as id_zona,correo,noProveedor from cliprov where id_cliprov=" + id + " and tipo='" + tipo + "'";
        object[] valores = ejecuta.dataSet(sql);
        if (Convert.ToBoolean(valores[0]))
        {
            datas = (DataSet)valores[1];
        }
        return datas;
    }

    public bool eliminaLevtoDaños(int idEmpresa, int idTaller, int noOrden, int consecutivo)
    {
        bool resultado = false;
        try
        {
            sql = "delete from mano_obra where id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and no_orden=" + noOrden.ToString() + " and consecutivo=" + consecutivo.ToString();
            object[] ejecutado = ejecuta.insertUpdateDelete(sql);
            if (Convert.ToBoolean(ejecutado[0]))
            {
                resultado = true;
            }
            else
                resultado = false;
        }
        catch (Exception x )
        {
            resultado = false;
        }
        return resultado;
    }

    public bool actualizaLevtoDaños(int idEmpresa, int idTaller, int noOrden, int consecutivo, int idGO, int idOP, int idRef, decimal monto)
    {
        bool resultado = false;
        try
        {
            sql = "update mano_obra set id_grupo_op=" + idGO.ToString() + ",id_operacion=" + idOP.ToString() + ",id_refaccion=" + idRef.ToString() + ",monto_mo=" + monto.ToString() + " where id_empresa=" + idEmpresa.ToString() + " and id_taller=" + idTaller.ToString() + " and no_orden=" + noOrden.ToString() + " and consecutivo=" + consecutivo.ToString();
            object[] ejecutado = ejecuta.insertUpdateDelete(sql);
            if (Convert.ToBoolean(ejecutado[0]))
            {
                resultado = true;
            }
            else
                resultado = false;
        }
        catch (Exception x )
        {
            resultado = false;
        }
        return resultado;
    }

    public object[] existeRFCcleinte(string rfc, string tipo)
    {
        sql = "Select count(*) from cliprov where tipo='" + tipo + "' and upper(rfc)='" + rfc.Trim() + "'";
        return ejecuta.scalarToBool(sql);
    }
    public object[] existeRFCcleinteModifica(string rfc, string tipo, int id)
    {
        sql = "Select count(*) from cliprov where tipo='" + tipo + "' and upper(rfc)='" + rfc.Trim() + "' and id_cliprov<>" + id;
        return ejecuta.scalarToBool(sql);
    }

    public object[] tieneRelacion(int cliente, string tipo)
    {
        string sql = "select sum(tabla.registros) as relaciones from(SELECT count(*) as registros FROM Ordenes_Reparacion WHERE id_cliprov=" + cliente + " and tipo_cliprov='"+tipo+"' union all select count(*) as registros from Ordenes_reparacion where id_cliprov_aseguradora=" + cliente + " and tipo_cliprov_aseguradora='"+tipo+"') as tabla";
        return ejecuta.scalarToBool(sql);
    }

    public bool altaBajaCliprov(int idCliprov, string estatus, string tipo)
    {
        string sql = "update Cliprov set estatus='" + estatus + "' where tipo='"+ tipo + "' and id_cliprov=" + idCliprov.ToString();
        object[] ejecutado = ejecuta.scalarToBool(sql);
        bool existe = Convert.ToBoolean(ejecutado[0]);
        if (existe)
            return true;
        else
            return false;
    }

    public string obtieneNumeroProveedor(int idCliprov, string tipo)
    {
        string sql = "select isnull(noProveedor,'') as numero from cliprov where tipo='" + tipo + "' and id_cliprov= " + idCliprov;
        return Convert.ToString(ejecuta.scalarToStringSimple(sql));
    }

    public string obtieneEstatusCliprov(int idCliprov, string tipo)
    {
        string resultado = "A";
        string sql = "select estatus from Cliprov where tipo='" + tipo + "' and id_cliprov=" + idCliprov.ToString();
        resultado = Convert.ToString(ejecuta.scalarToStringSimple(sql));
        return resultado;
    }

    public object[] tieneRelacionProv(int cliente, string tipo)
    {
        string sql = "select sum(tabla.registros) as relaciones from (SELECT count(*) as registros FROM refacciones_orden " +
                     "WHERE refProveedor = " + cliente + " union all select count(*) as registros from Cotizacion_Detalle where id_cliprov = " + cliente +
                     " union all select count(*) as registros from orden_compra_encabezado where id_cliprov = " + cliente + " ) as tabla";
        return ejecuta.scalarToBool(sql);
    }

    public void actualizaColores(int idCliprov, Telerik.Web.UI.RadNumericTextBox rgb_r, Telerik.Web.UI.RadNumericTextBox rgb_g, Telerik.Web.UI.RadNumericTextBox rgb_b)
    {
        string sql = "update Cliprov set rgb_r=" + rgb_r.Value.ToString() + ",rgb_g=" + rgb_g.Value.ToString() + ",rgb_b=" + rgb_b.Value.ToString() + " where tipo='C' and id_cliprov=" + idCliprov.ToString();
        object[] actualizado = ejecuta.insertUpdateDelete(sql);
    }

    public void actualizaImagen(int idCliprov, byte[] imagen)
    {
        if (imagen != null)
        {
            string sql = "update Cliprov set icono_est=@imagen where tipo='C' and id_cliprov=" + idCliprov.ToString();
            object[] actualizado = ejecuta.insertAdjuntos(sql, imagen);
        }
    }

    public void actualizaDireccion(int idCliprov, Telerik.Web.UI.RadComboBoxItem ddlPais, Telerik.Web.UI.RadComboBoxItem ddlEstado, Telerik.Web.UI.RadComboBoxItem ddlMunicipio, Telerik.Web.UI.RadComboBoxItem ddlColonia, Telerik.Web.UI.RadComboBoxItem ddlCodigo)
    {
        string sql = string.Format("update Cliprov set pais='{0}',estado='{1}',municipio='{2}',colonia='{3}',cp='{4}' where tipo='C' and id_cliprov={5}", ddlPais.Text, ddlEstado.Text, ddlMunicipio.Text, ddlColonia.Text, ddlCodigo.Text.PadLeft(5, '0'), idCliprov);
        object[] actualizado = ejecuta.insertUpdateDelete(sql);
    }

    public object[] obtieneRfc(int idCliprov)
    {
        string sql = string.Format("select rtrim(ltrim(rfc)) from cliprov where tipo='C' and id_cliprov={0}", idCliprov);
        return ejecuta.scalarToString(sql);
    }

    public string obtieneClavePolitica(decimal proveedor)
    {
        string sql = string.Format("select isnull(p.clv_politica,'') as clv_politica from Cliprov c left join Politica_Pago p on p.id_politica=c.id_politica where c.tipo='P' and c.id_cliprov={0}", proveedor);
        object[] resultado = ejecuta.scalarToString(sql);
        try
        {
            if (Convert.ToBoolean(resultado[0]))
                return resultado[1].ToString();
            else
                return "";
        }
        catch (Exception ex) { return ""; }
         
    }

    public int obtieneDiasPolitica(string proveedor)
    {
        string sql = string.Format("select isnull(dias_plazo,0) as dias from Cliprov c left join Politica_Pago p on p.id_politica=c.id_politica where c.tipo='C' and c.id_cliprov={0}", proveedor);
        object[] resultado = ejecuta.scalarToString(sql);
        try
        {
            if (Convert.ToBoolean(resultado[0]))
                return Convert.ToInt32(resultado[1].ToString());
            else
                return 0;
        }
        catch (Exception ex) { return 0; }
    }

    public string obtieneClavePoliticaCliente(string proveedor)
    {
        string sql = string.Format("select isnull(p.clv_politica,'') as clv_politica from Cliprov c left join Politica_Pago p on p.id_politica=c.id_politica where c.tipo='C' and c.id_cliprov={0}", proveedor);
        object[] resultado = ejecuta.scalarToString(sql);
        try
        {
            if (Convert.ToBoolean(resultado[0]))
                return resultado[1].ToString();
            else
                return "";
        }
        catch (Exception ex) { return ""; }
    }

    public string obtieneClavePoliticaPago(string politica)
    {
        string sql = string.Format("select isnull(clv_politica,'') as clv_politica from Politica_Pago where id_politica={0}", politica);
        object[] resultado = ejecuta.scalarToString(sql);
        try
        {
            if (Convert.ToBoolean(resultado[0]))
                return resultado[1].ToString();
            else
                return "";
        }
        catch (Exception ex) { return ""; }
    }

    public string obtieneIdPoliticaPago(string politica)
    {
        string sql = string.Format("select id_politica from Politica_Pago where clv_politica='{0}'", politica);
        object[] resultado = ejecuta.scalarToString(sql);
        try
        {
            if (Convert.ToBoolean(resultado[0]))
                return resultado[1].ToString();
            else
                return "0";
        }
        catch (Exception ex) { return "0"; }
    }

    public string obtieneIdPoliticaCliprov(int idProv)
    {
        string sql = string.Format("select id_politica from Cliprov where id_cliprov={0} and tipo='P'", idProv);
        object[] resultado = ejecuta.scalarToString(sql);
        try
        {
            if (Convert.ToBoolean(resultado[0]))
                return resultado[1].ToString();
            else
                return "0";
        }
        catch (Exception ex) { return "0"; }
    }

    public object[] eliminaClienteProveedor(int id, string tipo)
    {
        string sql = string.Format("delete from Cliprov where id_cliprov={0} and tipo='{1}'", id, tipo);
        return ejecuta.insertUpdateDelete(sql);
    }

    public string obtieneRfcNombre(string nombre)
    {
        string sql = string.Format("select rfc from cliprov where tipo='P' and razon_social='{0}'", nombre.Trim());
        return ejecuta.scalarToStringSimple(sql);
             
    }

    public object[] obtieneIdProveedor(string rfc)
    {
        string sql = "select id_cliprov from cliprov where tipo='P' and rfc='" + rfc.Trim().ToUpper() + "'";
        return ejecuta.scalarToInt(sql);
    }

    public object[] existeCliente(string rfc)
    {
        string sql = "select count(*) from cliprov where tipo='P' and rfc='" + rfc.Trim().ToUpper() + "'";
        return ejecuta.scalarToBoolLog(sql);
    }
}