using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CatEmpresa : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Empresas empresa = new Empresas();
            empresa.Empresa = 1;
            empresa.obtieneDatos();
            txtRfc.Text = empresa.Rfc;
            txtRazon.Text = empresa.Razon;
            txtEntrante.Text = empresa.Entrada;
            txtSaliente.Text = empresa.Salida;
            txtUsuario.Text = empresa.Usuario;
            txtContraseña.Text = empresa.Contraseña;
            txtPuerto.Text = empresa.Puerto;
            ddlTipo.SelectedValue = empresa.Tipo;
            if (empresa.Ssl == 1)
                ckhSSL.Checked = true;
            else
                ckhSSL.Checked = false;
        }
    }
    protected void btnActualizar_Click(object sender, EventArgs e)
    {

        Empresas empresa = new Empresas();
        empresa.Empresa = 1;
        empresa.obtieneDatos();
        empresa.Rfc = txtRfc.Text;
        empresa.Razon=txtRazon.Text;
        empresa.Entrada=txtEntrante.Text;
        empresa.Salida=txtSaliente.Text;
        empresa.Usuario=txtUsuario.Text;
        empresa.Contraseña=txtContraseña.Text ;
        empresa.Puerto = txtPuerto.Text;
        empresa.Tipo = ddlTipo.SelectedValue;
        if (ckhSSL.Checked)
            empresa.Ssl = 1;
        else
            empresa.Ssl = 0;        
        object[] agregado = empresa.actualizaEmpresa();
        if (!(bool)agregado[0])
        {
            lblError.Text = "Error: " + Convert.ToString(agregado[1]);
        }
    }
}