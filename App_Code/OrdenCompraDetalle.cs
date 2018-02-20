using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de OrdenCompraDetalle
/// </summary>
public class OrdenCompraDetalle
{
    private decimal _cantidad;
    private string _idArticulo;
    private string _producto;
    private string _descripcion_categoria;
    public int Renglon { get; set; }

    public OrdenCompraDetalle()
    {
        _cantidad = 0;
        _producto= string.Empty;
        _idArticulo = string.Empty;
        _descripcion_categoria = string.Empty;
        Renglon = 1;
    }

    public decimal Cantidad
    {
        get { return _cantidad; }
        set { _cantidad = value; }
    }

    public string IdArticulo
    {
        get { return _idArticulo; }
        set { _idArticulo = value; }
    }

    public string Descripcion_categoria
    {
        get { return _descripcion_categoria; }
        set { _descripcion_categoria = value; }
    }

    public string Producto
    {
        get { return _producto; }
        set { _producto = value; }
    }
}