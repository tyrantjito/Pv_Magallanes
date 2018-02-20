using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

/// <summary>
/// Descripción breve de Venta
/// </summary>
public class Venta
{
    private string _clave;
    private string _producto;
    private string _precio;
    private string _cantidad;
    private string _total;
    private string _renglon;
    private float _porc_descuento;
    private float _descuento;


	public Venta()
	{
        _clave = string.Empty;
        _producto = string.Empty;
        _precio = string.Empty;
        _cantidad = string.Empty;
        _total = string.Empty;
        _renglon = string.Empty;
        _porc_descuento = 0.00f;
        _descuento = 0.00f;
	}

    public string clave {
        get { return _clave; }
        set { _clave = value; }
    }
    public string producto
    {
        get { return _producto; }
        set { _producto = value; }
    }
    public string precio
    {
        get { return _precio; }
        set { _precio = value; }
    }
    public string cantidad
    {
        get { return _cantidad; }
        set { _cantidad = value; }
    }
    public string total
    {
        get { return _total; }
        set { _total = value; }
    }

    public string renglon
    {
        get { return _renglon; }
        set { _renglon = value; }
    }

    public float descuento
    {
        get { return _descuento; }
        set { _descuento = value; }
    }
    public float porc_descuento
    {
        get { return _porc_descuento; }
        set { _porc_descuento = value; }
    }
}

public static class appSettingsIva
{
    public static decimal getIva
    {
        get { return decimal.Parse(ConfigurationManager.AppSettings.Get("valIva")); }
    }
}