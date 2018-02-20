using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Descripción breve de PlantillaFormat
/// </summary>
public class PlantillaFormat
{
    string _encabezado;
    string _nota;
    int _plantilla;
    BaseDatos data = new BaseDatos();

	public PlantillaFormat()
	{
        _encabezado = _nota = string.Empty;
        _plantilla = 0;
	}

    public string Encabezado { set { _encabezado = value; } get{return _encabezado;}}
    public string Notas { set { _nota = value; } get{return _nota;} }
    public int Plantilla { set { _plantilla = value; } get { return _plantilla; } }

    private object[] existePlantilla() {        
        return data.intToBool(string.Format("select count(*) from plantilla_ticket where id_plantilla={0}", _plantilla));
    }

    public object[] actualizaPlantilla() {
        object[] existencia = existePlantilla();
        object[] datos = new object[2];
        if ((bool)existencia[0]) {
            string sql;
            if (Convert.ToBoolean(existencia[1]))
                sql = string.Format("update plantilla_ticket set encabezado='{0}', notas='{1}' where id_plantilla={2}", _encabezado, _nota, _plantilla);
            else
                sql = string.Format("insert into plantilla_ticket values({0},'{1}','{2}')", _plantilla, _encabezado, _nota);

            datos = data.insertUpdateDelete(sql);
        }
        return datos;
    }

    public void obtieneDatos() {
        string sql = string.Format("select encabezado,notas from plantilla_ticket where id_plantilla={0}", _plantilla);
        object[] datos = data.scalarData(sql);
        if (Convert.ToBoolean(datos[0]))
        {
            DataSet info = (DataSet)datos[1];
            foreach (DataRow fila in info.Tables[0].Rows)
            {
                _encabezado = fila[0].ToString();
                _nota = fila[1].ToString();
            }
        }
        else {
            _encabezado = _nota = "";
        }
    }
}