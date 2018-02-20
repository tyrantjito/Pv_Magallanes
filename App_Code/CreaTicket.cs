using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using E_Utilities;

/// <summary>
/// Descripción breve de CreaTicket
/// </summary>
public class CreaTicket
{
    private int _ticket;
    private int _punto;
    private string _impresora;
    private string _contenido;
    int max, cort;
    string parte1, parte2;

    public CreaTicket()
	{
        _ticket = 0;
        _punto = 0;
        _impresora = string.Empty;
        _contenido = string.Empty;
	}

    public int ticket {
        get { return _ticket; }
        set { _ticket = value; }
    }

    public int punto {
        set { _punto = value; }
    }

    public string impresora {
        set { _impresora = value; }
    }

    public void LineasGuion() {
        _contenido = "----------------------------------------\n";   // agrega lineas separadoras -
        RawPrinterHelper.RawPrinterHelperI.SendStringToPrinter(_impresora, _contenido); // imprime linea
    }


    public void LineasAsterisco()
    {
        _contenido = "****************************************\n";   // agrega lineas separadoras *
        RawPrinterHelper.RawPrinterHelperI.SendStringToPrinter(_impresora, _contenido); // imprime linea
    }
    public void LineasIgual()
    {
        _contenido = "========================================\n";   // agrega lineas separadoras =
        RawPrinterHelper.RawPrinterHelperI.SendStringToPrinter(_impresora, _contenido); // imprime linea
    }
    public void LineasTotales()
    {
        _contenido = "                             -----------\n"; ;   // agrega lineas de total
        RawPrinterHelper.RawPrinterHelperI.SendStringToPrinter(_impresora, _contenido); // imprime linea
    }
    public void EncabezadoVenta()
    {
        _contenido = "Producto       Cant    P.Unit    Importe\n";   // agrega lineas de  encabezados
        RawPrinterHelper.RawPrinterHelperI.SendStringToPrinter(_impresora, _contenido); // imprime texto
    }
    public void TextoIzquierda(string par1)                          // agrega texto a la izquierda
    {
        max = par1.Length;
        if (max > 40)                                 // **********
        {
            cort = max - 40;
            parte1 = par1.Remove(40, cort);        // si es mayor que 40 caracteres, lo corta
        }
        else { parte1 = par1; }                      // **********
        _contenido = parte1 + "\n";
        RawPrinterHelper.RawPrinterHelperI.SendStringToPrinter(_impresora, _contenido); // imprime texto
    }
    public void TextoDerecha(string par1)
    {
        _contenido = "";
        max = par1.Length;
        if (max > 40)                                 // **********
        {
            cort = max - 40;
            parte1 = par1.Remove(40, cort);           // si es mayor que 40 caracteres, lo corta
        }
        else { parte1 = par1; }                      // **********
        max = 40 - par1.Length;                     // obtiene la cantidad de espacios para llegar a 40
        for (int i = 0; i < max; i++)
        {
            _contenido += " ";                          // agrega espacios para alinear a la derecha
        }
        _contenido += parte1 + "\n";                    //Agrega el texto
        RawPrinterHelper.RawPrinterHelperI.SendStringToPrinter(_impresora, _contenido); // imprime texto
    }
    public void TextoCentro(string par1)
    {
        _contenido = "";
        max = par1.Length;
        if (max > 40)                                 // **********
        {
            cort = max - 40;
            parte1 = par1.Remove(40, cort);          // si es mayor que 40 caracteres, lo corta
        }
        else { parte1 = par1; }                      // **********
        max = (int)(40 - parte1.Length) / 2;         // saca la cantidad de espacios libres y divide entre dos
        for (int i = 0; i < max; i++)                // **********
        {
            _contenido += " ";                           // Agrega espacios antes del texto a centrar
        }                                            // **********
        _contenido += parte1 + "\n";
        RawPrinterHelper.RawPrinterHelperI.SendStringToPrinter(_impresora, _contenido); // imprime texto
    }
    public void TextoExtremos(string par1, string par2)
    {
        max = par1.Length;
        if (max > 18)                                 // **********
        {
            cort = max - 18;
            parte1 = par1.Remove(18, cort);          // si par1 es mayor que 18 lo corta
        }
        else { parte1 = par1; }                      // **********
        _contenido = parte1;                             // agrega el primer parametro
        max = par2.Length;
        if (max > 18)                                 // **********
        {
            cort = max - 18;
            parte2 = par2.Remove(18, cort);          // si par2 es mayor que 18 lo corta
        }
        else { parte2 = par2; }
        max = 40 - (parte1.Length + parte2.Length);
        for (int i = 0; i < max; i++)                 // **********
        {
            _contenido += " ";                            // Agrega espacios para poner par2 al final
        }                                             // **********
        _contenido += parte2 + "\n";                     // agrega el segundo parametro al final
        RawPrinterHelper.RawPrinterHelperI.SendStringToPrinter(_impresora, _contenido); // imprime texto
    }
    public void AgregaTotales(string par1, double total)
    {
        max = par1.Length;
        if (max > 25)                                 // **********
        {
            cort = max - 25;
            parte1 = par1.Remove(25, cort);          // si es mayor que 25 lo corta
        }
        else { parte1 = par1; }                      // **********
        _contenido = parte1;
        parte2 = total.ToString("c");
        max = 40 - (parte1.Length + parte2.Length);
        for (int i = 0; i < max; i++)                // **********
        {
            _contenido += " ";                           // Agrega espacios para poner el valor de moneda al final
        }                                            // **********
        _contenido += parte2 + "\n";
        RawPrinterHelper.RawPrinterHelperI.SendStringToPrinter(_impresora, _contenido); // imprime texto
    }
    public void AgregaArticulo(string par1, int cant, double precio, double total)
    {
        if (cant.ToString().Length <= 3 && precio.ToString("c").Length <= 10 && total.ToString("c").Length <= 11) // valida que cant precio y total esten dentro de rango
        {
            max = par1.Length;
            if (max > 16)                                 // **********
            {
                cort = max - 16;
                parte1 = par1.Remove(16, cort);          // corta a 16 la descripcion del articulo
            }
            else { parte1 = par1; }                      // **********
            _contenido = parte1;                             // agrega articulo
            max = (3 - cant.ToString().Length) + (16 - parte1.Length);
            for (int i = 0; i < max; i++)                // **********
            {
                _contenido += " ";                           // Agrega espacios para poner el valor de cantidad
            }
            _contenido += cant.ToString();                   // agrega cantidad
            max = 10 - (precio.ToString("c").Length);
            for (int i = 0; i < max; i++)                // **********
            {
                _contenido += " ";                           // Agrega espacios
            }                                            // **********
            _contenido += precio.ToString("c"); // agrega precio
            max = 11 - (total.ToString().Length);
            for (int i = 0; i < max; i++)                // **********
            {
                _contenido += " ";                           // Agrega espacios
            }                                            // **********
            _contenido += total.ToString("c") + "\n"; // agrega precio
            RawPrinterHelper.RawPrinterHelperI.SendStringToPrinter(_impresora, _contenido); // imprime texto
        }
        else
        {
            //MessageBox.Show("Valores fuera de rango");
            RawPrinterHelper.RawPrinterHelperI.SendStringToPrinter(_impresora, "Error, valor fuera de rango\n"); // imprime texto
        }
    }
    public void CortaTicket()
    {
        string corte = "\x1B" + "m";                  // caracteres de corte
        string avance = "\x1B" + "d" + "\x09";        // avanza 9 renglones
        RawPrinterHelper.RawPrinterHelperI.SendStringToPrinter(_impresora, avance); // avanza
        RawPrinterHelper.RawPrinterHelperI.SendStringToPrinter(_impresora, corte); // corta
    }
    public void AbreCajon()
    {
        string cajon0 = "\x1B" + "p" + "\x00" + "\x0F" + "\x96";                  // caracteres de apertura cajon 0
        //string cajon1 = "\x1B" + "p" + "\x01" + "\x0F" + "\x96";                 // caracteres de apertura cajon 1
        RawPrinterHelper.RawPrinterHelperI.SendStringToPrinter(_impresora, cajon0); // abre cajon0
        //RawPrinterHelper.RawPrinterHelper.SendStringToPrinter(_impresora, cajon1); // abre cajon1
    }
}