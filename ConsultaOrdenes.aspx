<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConsultaOrdenes.aspx.cs" Inherits="ConsultaOrdenes" MasterPageFile="~/Administracion.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="ancho95 text-center centrado">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
               <i class="icon-folder-open"></i> <asp:Label runat="server" ID="lblTitulo" Text="Consulta Ordenes de Compra" CssClass="alert-success"></asp:Label>
            </div>
        </div>
        <div class="row marTop">
            <div class="col-lg-12 col-sm-12 center">  
                <div class="col-lg-6 col-sm-6 center">                          
                    <asp:Label ID="Label8" runat="server" Text="Tiendas:" />
                    <asp:DropDownList ID="ddlIslas" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2"
                        DataTextField="nombre" DataValueField="idAlmacen" OnSelectedIndexChanged="ddlIslas_SelectedIndexChanged" CssClass="dropdown">
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select u.id_punto as idAlmacen,p.nombre_pv as nombre from usuario_puntoventa U inner join punto_venta p on p.id_punto=u.id_punto where U.usuario=@usuario and U.estatus='A'">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="usuario" QueryStringField="u" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <div class="col-lg-6 col-sm-6 center">                          
                    <asp:Label ID="Label1" runat="server" Text="Estatus:" />
                    <asp:DropDownList ID="ddlEstatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlEstatus_SelectedIndexChanged" CssClass="dropdown">
                        <asp:ListItem Selected="True" Value="A">Pendientes</asp:ListItem>
                        <asp:ListItem Value="E">Enviados</asp:ListItem>
                        <asp:ListItem Value="V">Procesando</asp:ListItem>
                    </asp:DropDownList>                    
                </div>
            </div>
        </div>
        <br />  
        <div class="row marTop">
            <div class="col-lg-12 col-sm-12 center alert-danger negritas">  
                <asp:Label ID="lblError" runat="server" ></asp:Label>
            </div>
        </div>
        <br />      
        <div class="row">
            <div class="col-lg-12 col-sm-12 center">
                <div class="table-responsive">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="GridOrdenes" ShowHeaderWhenEmpty="True"
                                AutoGenerateColumns="False" DataKeyNames="no_orden" AllowSorting="True"
                                EmptyDataText="No existen Ordenes de Compra registradas" 
                                EmptyDataRowStyle-ForeColor="Red" AllowPaging="True" PageSize="5"
                                CssClass="table table-bordered" 
                                DataSourceID="SqlDataSource1" OnRowDataBound="GridOrdenes_RowDataBound"
                                onpageindexchanging="GridOrdenes_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField DataField="no_orden" HeaderText="No. Orden" SortExpression="no_orden" />
                                    <asp:BoundField DataField="obsevaciones" HeaderText="Nota" SortExpression="obsevaciones" />
                                    <asp:BoundField DataField="usuario" HeaderText="Usuario" SortExpression="usuario" />
                                    <asp:BoundField DataField="fecha" HeaderText="Fecha" ReadOnly="True" SortExpression="fecha" />
                                    <asp:BoundField DataField="hora" HeaderText="Hora" ReadOnly="True" SortExpression="hora" />
                                    <asp:BoundField DataField="estatus" HeaderText="estatus" SortExpression="estatus" Visible="false" />
                                    <asp:BoundField DataField="estatusOrden" HeaderText="Estatus" ReadOnly="True" SortExpression="estatusOrden" />
                                    <asp:CommandField ButtonType="Button" ShowSelectButton="True" ControlStyle-CssClass="btn-success" SelectText="Seleccionar" >
                                    <ControlStyle CssClass="btn-success" />
                                    </asp:CommandField>                                    
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnActualiza" runat="server" Text="Estatus" 
                                                CssClass="btn-warning" OnClientClick="confirm('¿Esta seguro de cambiar el estatus de la orden de compra?')"
                                                CommandArgument='<%# Eval("no_orden")+";"+Eval("estatus") %>' 
                                                onclick="btnActualiza_Click"></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>                                
                                <EmptyDataRowStyle ForeColor="Red" />
                                <SelectedRowStyle CssClass="alert-success-org" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                SelectCommand="select no_orden,obsevaciones,usuario,convert(char(10),fecha,126) as fecha,convert(char(10),hora,108) as hora,estatus,case estatus when 'A' then 'Pendiente' when 'E' then 'Enviada' when 'V' then 'Procesando' else '' end as estatusOrden from orden_compra_encabezado_PV where idAlmacen=@idAlmacen and estatus=@estatus order by fecha desc, hora desc">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlIslas" Name="idAlmacen" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="ddlEstatus" Name="estatus" PropertyName="SelectedValue" />
                                </SelectParameters>
                            </asp:SqlDataSource>                                                      
                        </ContentTemplate>
                    </asp:UpdatePanel>                        
                </div>
            </div>
        </div>        
        <br />
        <div class="row">
            <div class="col-lg-12 col-sm-12 center">
                <div class="table-responsive">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="GrdDetalleOrden" ShowHeaderWhenEmpty="True"
                                AutoGenerateColumns="False" AllowSorting="True"
                                EmptyDataText="Seleccione una orden de compra para ver su detalle" 
                                EmptyDataRowStyle-ForeColor="Red" 
                                CssClass="table table-bordered" 
                                DataSourceID="SqlDataSource3">
                                <Columns>
                                    <asp:BoundField DataField="no_orden" Visible="false" SortExpression="no_orden" />
                                    <asp:BoundField DataField="idAlmacen" Visible="false" SortExpression="idAlmacen" />
                                    <asp:BoundField DataField="no_detalle" Visible="false" SortExpression="no_detalle" />
                                    <asp:BoundField DataField="idArticulo" HeaderText="Codigo" SortExpression="idArticulo" />
                                    <asp:BoundField DataField="descripcion" HeaderText="Articulo" SortExpression="descripcion" />
                                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" />
                                    <asp:BoundField DataField="descripcion_categoria" HeaderText="Categoria" SortExpression="descripcion_categoria" />
                                </Columns>
                                <EmptyDataRowStyle ForeColor="Red" />
                                <SelectedRowStyle CssClass="alert-success-org" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                SelectCommand="select no_orden,idAlmacen,no_detalle,idArticulo,descripcion,cantidad,descripcion_categoria from orden_compra_detalle_PV where no_orden=@no_orden and idAlmacen=@idAlmacen order by descripcion_categoria asc">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="GridOrdenes" Name="no_orden" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="ddlIslas" Name="idAlmacen" PropertyName="SelectedValue" />
                                </SelectParameters>
                            </asp:SqlDataSource>                                                            
                        </ContentTemplate>
                    </asp:UpdatePanel>                        
                </div>
            </div>
        </div>        
    </div>
</asp:Content>


