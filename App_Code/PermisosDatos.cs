using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de PermisosDatos
/// </summary>
public class PermisosDatos
{
    BaseDatos ejecuta = new BaseDatos();
    string sql = "";
    public PermisosDatos()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public bool agregaPermiso(string permiso)
    {
        sql = "insert into permisos_PV values((isnull((select TOP 1(p.id_permiso)+1 from permisos_PV p order by id_permiso desc),1)),'" + permiso + "')";
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if ((bool)ejecutado[0])
            return (bool)ejecutado[1];
        else
            return false;
    }

    public bool daPermiso(int idPermiso, string usuario)
    {
        sql = "insert into usuarios_permisos_PV values(" + idPermiso.ToString() + ",'" + usuario + "')";
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if ((bool)ejecutado[0])
            return (bool)ejecutado[1];
        else
            return false;
    }

    public bool quitaPermiso(int idPermiso, string usuario)
    {
        sql = "delete from usuarios_permisos_PV where id_permiso=" + idPermiso.ToString() + " and usuario='" + usuario + "'";
        object[] ejecutado = ejecuta.insertUpdateDelete(sql);
        if ((bool)ejecutado[0])
            return (bool)ejecutado[1];
        else
            return false;
    }
}