using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Plantilla : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PlantillaFormat plantilla = new PlantillaFormat();
            plantilla.Plantilla = 1;
            plantilla.obtieneDatos();
            txtEncabezado.Text = plantilla.Encabezado;
            txtNotas.Text = plantilla.Notas;
        }
    }
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        PlantillaFormat plantilla = new PlantillaFormat();
        plantilla.Plantilla = 1;
        plantilla.Encabezado = txtEncabezado.Text;
        plantilla.Notas = txtNotas.Text;
        object[] agregado = plantilla.actualizaPlantilla();
        if (!(bool)agregado[0]) {
            lblError.Text = "Error: " + Convert.ToString(agregado[1]);
        }
    }
}