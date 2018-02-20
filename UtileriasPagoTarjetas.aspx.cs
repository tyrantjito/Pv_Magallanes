using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using E_Utilities;
using System.Data;

public partial class UtileriasPagoTarjetas : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lnkCancelarPago_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        int terminal = 0;
        string clave = "";
        int intentos = 0;
        try
        {
            Islas pv = new Islas();
            pv.Almacen = Convert.ToInt32(Request.QueryString["p"]);
            object[] datos_pv = pv.obtieneParametrosTarjeta();
            if (Convert.ToBoolean(datos_pv[0]))
            {
                try
                {
                    DataSet info = (DataSet)datos_pv[1];
                    foreach (DataRow fila in info.Tables[0].Rows)
                    {
                        terminal = Convert.ToInt32(fila[0].ToString());
                        clave = fila[1].ToString();
                        intentos = Convert.ToInt32(fila[2].ToString());
                    }
                }
                catch (Exception) { terminal = 0; }
            }

            if (terminal == 0)
                lblError.Text = "No es posible realizar pagos y/o cancelaciones con tarjetas ya que no tiene una terminal definida";
            else
            {
                if (clave == "")
                    lblError.Text = "No es posible realizar pagos y/o cancelaciones con tarjetas ya que no se cuenta con la clave proporcionada por el proveedor de servicios";
                else
                {
                    if (txtRefeCancelar.Text != "")
                    {
                        string[] valores = txtRefeCancelar.Text.Split(new char[] { '-' });
                        int operacion = Convert.ToInt32(valores[3]);
                        PagosPinPad pagos = new PagosPinPad();

                        string[] valoresOperacion = new string[6] { "", "", "0", "0", "","00" };
                        pagos.ticket = operacion;
                        pagos.obtieneOperacion();
                        object[] info = pagos._retorno;
                        if (Convert.ToBoolean(info[0]))
                        {
                            DataSet datos = (DataSet)info[1];
                            foreach (DataRow filas in datos.Tables[0].Rows)
                            {
                                valoresOperacion = new string[6] { filas[0].ToString(), filas[1].ToString(), filas[2].ToString(), filas[3].ToString(), filas[4].ToString(), filas[5].ToString() };
                            }

                            if (valoresOperacion[5] == "16")
                                lblError.Text = "La Operación ya fue cancelada con anterioridad";
                            else if (valoresOperacion[5] == "17")
                                lblError.Text = "La operacion ya fue devuelta con anterioridad";
                            else if (valoresOperacion[5] == "20" || valoresOperacion[5] == "21")
                                lblError.Text = "Debe entrar a la opcion reimpresión de tickets de pagados con tarjeta para continuar";
                            else
                            {

                                string cadFecha = valoresOperacion[4].Substring(0, 10);
                                DateTime fechaCom = Convert.ToDateTime(cadFecha);
                                if (fechaCom.Date < fechas.obtieneFechaLocal().Date)
                                    lblError.Text = "La Operacion indicada no corresponde a una operación del día de hoy por lo cual no es posible hacer la cancelación";
                                else
                                {
                                    if (valoresOperacion[3] != "0")
                                    {
                                        pagos.opcion = "16";
                                        pagos.terminal = terminal;
                                        pagos.clave = clave;
                                        pagos.ticket = 0;
                                        pagos.caja = Convert.ToInt32(Request.QueryString["c"]);
                                        pagos.punto = Convert.ToInt32(Request.QueryString["p"]);
                                        pagos.usuario = Request.QueryString["u"];
                                        decimal importe = 0;
                                        try { importe = Convert.ToDecimal(valoresOperacion[2]); }
                                        catch (Exception ex) { importe = 0; }
                                        if (importe != 0)
                                        {
                                            pagos.importe = importe;
                                            pagos.nombre = Request.QueryString["np"];//txtNombrePago.Text;
                                            pagos.concepto = "Compra de Productos";//txtConcepto.Text;
                                            pagos.correo = "e-apps@outlook.com";//txtCorreoPago.Text;
                                            pagos.referencia = "";
                                            pagos.folio = valoresOperacion[3];
                                            pagos.parcializacion = "00";
                                            pagos.diferimiento = "00";
                                            pagos.iteraciones = intentos;
                                            DateTime fecha = fechas.obtieneFechaLocal();
                                            pagos.fecha = fecha.ToString("ddMMyyyy");
                                            pagos.ejecutaPeticion();
                                            object[] respuesta = pagos._retorno;
                                            if (Convert.ToBoolean(respuesta[0]))
                                            {
                                                lblError.Text = Convert.ToString(respuesta[1]);
                                            }
                                            else
                                                lblError.Text = Convert.ToString(respuesta[1]);
                                        }
                                        else
                                            lblError.Text = "El importe a pagar debe ser mayor a cero y/o indique un monto válido";
                                    }
                                    else
                                        lblError.Text = "No se encontro el folio indicado por la transaccion";
                                }
                            }
                        }
                        else
                            lblError.Text = "La Operación no puede ser cancelada debido a que no existe registro de la misma";
                    }
                    else
                        lblError.Text = "Debe indicar la referencia del pago realizado previamente";
                }
            }
        }
        catch (Exception ex) { lblError.Text = "Error al cancelar pago: " + ex.Message; }
        finally
        {

        }
    }
}