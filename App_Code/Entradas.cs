using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Descripción breve de Entradas
/// </summary>
public class Entradas
{
    BaseDatos ejecuta = new BaseDatos();
    public int entrada { get; set; }
    public int tienda { get; set; }
    public string usuario { get; set; }
    public object[] retorno { get; set; }
    private string sql;

    public Entradas()
    {
        entrada = tienda=0;
        retorno = new object[2] { false, "" };
        sql = usuario = string.Empty;
    }

    public void obtieneEncabezadoEntrada() {
        sql = "select * from entinventarioenc where entfolioid=" + entrada + " and entalmacenid= " + tienda;
        retorno = ejecuta.scalarData(sql);
    }

    public void cargaDetalle()
    {
        sql = "select e.entProductoID,c.descripcion,e.entprodcant,e.entProdCostoUnit,e.entimporte,isnull(p.ventaUnitaria,0) as venta from entinventariodet e inner join catproductos c on c.idProducto = e.entProductoID left join articulosalmacen a on a.idalmacen = e.entAlmacenID and a.idArticulo = e.entProductoID left join precios_venta p on p.idProducto = e.entProductoID and p.idAlmacen = e.entAlmacenID and p.idPrecioPublico = a.idPrecioPublico where e.entfolioid = " + entrada + " and e.entalmacenid = " + tienda;
        retorno = ejecuta.scalarData(sql);
    }

    public void terminarEntrada()
    {
        sql = "declare @entAlmacenID int declare @entrada int declare @existencia as decimal(12,3) declare @articulo as varchar(30) declare @cantidad as decimal(12,3) declare @renglon as int declare @articulos as int declare @costo as decimal(12,2)	declare @importe as decimal(14,2) declare @costoUni as decimal(12,2) declare @IDprecioPublico smallint declare @NvoprecVtaUnitaria as decimal(8,2) declare @precVtaUnitaria as decimal(8,2) declare @entFechaDoc as char(10) declare @usuID as char(15) declare @afectados int declare @registros int " +
            "set @entAlmacenID = " + tienda + " set @entrada = " + entrada + " set @entFechaDoc = (select convert(char(10), entFechaDoc, 120) from entinventarioenc where entfolioid = @entrada and entalmacenid = @entAlmacenID) set @usuID = '" + usuario + "' set @afectados=0 " +
            "set @registros = (select count(*) from entinventariodet where entfolioid = @entrada and entalmacenid = @entAlmacenID) "+
            "create table #tmpDetEnt(entDetID int, entProductoID varchar(30),entProdCant decimal(12,3),entProdCostoUnit decimal(12,2), entPrecVtaUnit decimal(12,2), entImporte  decimal(14,2)) " +
            "insert into #tmpDetEnt select e.entDetID,e.entProductoID,e.entprodcant,e.entProdCostoUnit,isnull(p.ventaUnitaria,0) as venta ,e.entimporte from entinventariodet e inner join catproductos c on c.idProducto = e.entProductoID left join articulosalmacen a on a.idalmacen = e.entAlmacenID and a.idArticulo = e.entProductoID left join precios_venta p on p.idProducto = e.entProductoID and p.idAlmacen = e.entAlmacenID and p.idPrecioPublico = a.idPrecioPublico  and a.idalmacen = e.entAlmacenID and a.idArticulo = e.entProductoID where e.entfolioid = @entrada and e.entalmacenid = @entAlmacenID " +
            "while (exists(select * from #tmpDetEnt)) " +
            "begin " +
                "set @renglon = (select top 1 entDetID from #tmpDetEnt) " +
                "set @articulo = (select top 1 entProductoID from #tmpDetEnt) " +
                "set @cantidad = (select top 1 entProdCant from #tmpDetEnt) " +
                "set @costo = (select top 1 entProdCostoUnit from #tmpDetEnt) " +
                "set @importe = (select top 1 entImporte from #tmpDetEnt) " +
                "set @costoUni = (select top 1 entProdCostoUnit from #tmpDetEnt) " +
                "set @existencia= (select cantidadExistencia from articulosalmacen where idAlmacen = @entAlmacenID and ltrim(rtrim(idArticulo)) = ltrim(rtrim(@articulo))) " +
                "set @articulos = (select count(*) from articulosalmacen where idAlmacen = @entAlmacenID and ltrim(rtrim(idArticulo)) = ltrim(rtrim(@articulo))) " +
                "set @IDprecioPublico = (select cast(isnull((select TOP 1 idPrecioPublico from precios_venta where ltrim(rtrim(idProducto)) = ltrim(rtrim(@articulo)) and idAlmacen = @entAlmacenID order by fecha DESC, ventaUnitaria desc), 0) as smallint)) " +
                "set @NvoprecVtaUnitaria = (select top 1 entPrecVtaUnit from #tmpDetEnt) " +
                "set @precVtaUnitaria = (select isnull((SELECT isnull(pv.ventaUnitaria, 0) FROM  precios_venta pv LEFT JOIN articulosalmacen ar ON pv.idProducto = ar.idArticulo AND pv.idAlmacen = ar.idAlmacen  WHERE ltrim(rtrim(pv.idProducto)) = ltrim(rtrim(@articulo)) AND pv.idAlmacen = @entAlmacenID AND pv.idPrecioPublico = ar.idPrecioPublico),0)) " +
                "if (@articulos > 0) " +
                "begin " +
                    "IF(@precVtaUnitaria <> @NvoprecVtaUnitaria) " +
                    "BEGIN " +
                        "SET @IDprecioPublico = @IDprecioPublico + 1 " +
                        "INSERT INTO precios_venta VALUES(@articulo, @entAlmacenID, @IDprecioPublico, @NvoprecVtaUnitaria,1, CAST(@entFechaDoc AS DATE), @usuID) " +
                    "END " +
                    "UPDATE articulosalmacen SET cantidadExistencia = (@existencia + @cantidad), idPrecioPublico = @IDprecioPublico, costoUnitario = @costoUni where idAlmacen = @entAlmacenID and ltrim(rtrim(idArticulo)) = ltrim(rtrim(@articulo)) " +
                    "set @afectados=@afectados+1 "+
                "end " +
                "else " +
                "begin " +
                    "insert into articulosalmacen(idAlmacen, idArticulo, cantidadExistencia, costoUnitario, idPrecioPublico) values(@entAlmacenID, @articulo, @cantidad, @costo, @IDprecioPublico) " +
                    "INSERT INTO precios_venta VALUES(@articulo, @entAlmacenID, 1, @NvoprecVtaUnitaria, 1, CAST(@entFechaDoc AS DATE), @usuID) " +
                    "set @afectados=@afectados+1 " +
                "end " +
                "delete from #tmpDetEnt where ltrim(rtrim(entProductoID))=ltrim(rtrim(@articulo)) and entDetID=@renglon " +
            "end " +
            "drop table #tmpDetEnt "+
            "if(@afectados>0 and @afectados=@registros) "+
            "begin "+
                "update entinventarioenc set terminado=1 where entfolioid = @entrada and entalmacenid = @entAlmacenID "+
            "end "+
            "select @afectados";
        retorno = ejecuta.scalarString(sql);

    }

    public void eliminarEntrada()
    {
        sql = "declare @entrada int declare @tienda int declare @terminado int declare @entradaBorrar int declare @tiendaBorrar int set @entradaBorrar = " + entrada + " set @tiendaBorrar = " + tienda + " set @terminado = (select count(*) from entinventarioenc where entfolioid = @entradaBorrar and entalmacenid = @tiendaBorrar and terminado = 1) " +
                "if (@terminado > 0) " +
                "begin " +
                    "declare entradas cursor for " +
                    "select entfolioid, entalmacenid from entinventarioenc where entfolioid = @entradaBorrar and entalmacenid = @tiendaBorrar " +
                    "open entradas fetch next from entradas into @entrada, @tienda " +
                    "while @@fetch_status = 0 " +
                    "begin " +
                        "declare @idArticulo varchar(30) declare @cantEntrada decimal(12, 3) declare @cantExiste decimal(12, 3) declare articulos cursor for " +
                        "select e.entproductoid, e.entprodcant from entinventariodet e inner join articulosalmacen a on a.idalmacen = e.entalmacenid and a.idarticulo = e.entproductoid where e.entfolioid in(@entrada)and e.entalmacenid = @tienda " +
                        "open articulos fetch next from articulos into @idArticulo, @cantEntrada " +
                        "while @@fetch_status = 0 " +
                        "begin " +
                            "update articulosalmacen set cantidadExistencia = cantidadExistencia - @cantEntrada where idalmacen = @tienda and idarticulo = @idArticulo " +
                            "fetch next from articulos into @idArticulo,@cantEntrada " +
                        "end close articulos deallocate articulos fetch next from entradas into @entrada,@tienda " +
                    "end  " +
                    "close entradas deallocate entradas " +
                    "delete from entinventarioenc where entfolioid = @entradaBorrar and entalmacenid = @tiendaBorrar " +
                    "delete from entinventariodet where entfolioid = @entradaBorrar and entalmacenid = @tiendaBorrar " +
                "end " +
                "else  " +
                "begin " +
                    "delete from entinventarioenc where entfolioid = @entradaBorrar and entalmacenid = @tiendaBorrar " +
                    "delete from entinventariodet where entfolioid = @entradaBorrar and entalmacenid = @tiendaBorrar " +
                "end";
        retorno = ejecuta.insertUpdateDelete(sql);
    }
}