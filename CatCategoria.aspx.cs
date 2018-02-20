using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CatCategoria : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lnkAceptarNew_Click(object sender, EventArgs e)
    {
        try
        {
            SqlDataSource1.Insert();
            GridCategoria.DataBind();
        }
        catch (Exception ex)
        {
            lblErrores.Text = "Ocurrio un error insensperado: " + ex.Message;
        }
    }
}