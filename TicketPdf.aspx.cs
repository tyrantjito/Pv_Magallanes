using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class TicketPdf : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string ruta = Server.MapPath("~/Tickets/");
        ShowPdf1.FilePath = "Tickets/" + Request.QueryString["a"];

        /*
        string archivo = HttpContext.Current.Request.QueryString["a"];

        string ruta = Server.MapPath("~/Tickets/");
        ruta = ruta + archivo;
        System.IO.FileInfo filename = new System.IO.FileInfo(ruta);
        if (filename.Exists)
        {
            Response.Redirect("~/Tickets/" + filename.Name);
        }
        else
            Response.Redirect("PuntoVenta.aspx");*/
    }
}