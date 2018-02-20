<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="FacturacionPendiente.aspx.cs" Inherits="FacturacionPendiente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    
    <div class="ancho95 centrado">
        <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
            <i class="icon-list-ul">&nbsp;</i><asp:Label runat="server" ID="lblTitulo" Text="Pendientes por facturar" CssClass="alert-success"></asp:Label>
        </div>
        <div class="row marTop">
            <div class="col-lg-12 col-sm-12 center">  
                <div class="col-lg-6 col-sm-6 text-center">                          
                    <asp:Label ID="Label8" runat="server" Text="Tiendas:" />
                    <asp:DropDownList ID="ddlIslas" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2"
                        DataTextField="nombre" DataValueField="idAlmacen" OnSelectedIndexChanged="ddlIslas_SelectedIndexChanged" CssClass="dropdown">
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select 0 as idAlmacen, 'Seleccione Tienda' as nombre union all select u.idAlmacen,c.nombre from usuarios_facturacion u left join catalmacenes c on c.idAlmacen=u.idAlmacen where u.usuario=@usuario order by 1">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="usuario" QueryStringField="u" />                            
                        </SelectParameters>
                    </asp:SqlDataSource>                               
                </div>
                <div class="col-lg-6 col-sm-6 text-center">                          
                    <asp:Label ID="Label1" runat="server" Text="Estatus:" />
                    <asp:DropDownList ID="ddlEstatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlEstatus_SelectedIndexChanged" CssClass="dropdown">
                        <asp:ListItem Selected="True" Value="A">Pendientes</asp:ListItem>
                        <asp:ListItem Value="F">Facturados</asp:ListItem>                        
                    </asp:DropDownList>                    
                </div>                
            </div>            
        </div>
        <div class="row marTop">
            <div class="col-lg-12 col-sm-12 center alert-danger negritas">  
                <asp:Label ID="lblError" runat="server" ></asp:Label>
            </div>
        </div>
        <div class="row">            
            <div class="col-lg-2 col-sm-2 center"></div>
            <div class="col-lg-8 col-sm-8 center">
                <div class="table-responsive">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Timer ID="Timer1" runat="server" Interval="30000" OnTick="Timer1_Tick"></asp:Timer>
                            <br />
                            <asp:GridView ID="GridFactPend" runat="server" AutoGenerateColumns="False"
                                CssClass="table table-bordered center" EmptyDataText="No existen facturas pendientes" EmptyDataRowStyle-ForeColor="Red"
                                DataKeyNames="ticket" AllowPaging="True" PageSize="5" OnPageIndexChanging="GridFactPend_PageIndexChanging"
                                AllowSorting="True" DataSourceID="SqlDataSource1" OnRowDataBound="GridFactPend_RowDataBound">
                                <Columns>                                                                        
                                    <asp:BoundField DataField="ticket" HeaderText="Ticket" ReadOnly="True" SortExpression="ticket"></asp:BoundField>
                                    <asp:TemplateField HeaderText="Fecha Venta" SortExpression="fecha_venta">
                                        <EditItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("fecha_venta") %>' ID="Label1"></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("fecha_venta", "{0:yyyy-MM-dd}") %>' ID="Label1"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Email" HeaderText="Correo Electrónico" SortExpression="Email"></asp:BoundField>
                                    <asp:BoundField DataField="id_cliente" Visible="false" HeaderText="id_cliente" SortExpression="id_cliente"></asp:BoundField>
                                    <asp:BoundField DataField="archivarNombre" HeaderText="Razon Social" SortExpression="archivarNombre"></asp:BoundField>
                                    <asp:CommandField ButtonType="Button" ShowSelectButton="True" ControlStyle-CssClass="btn-success" SelectText="Seleccionar" >
                                    <ControlStyle CssClass="btn-success" />
                                    </asp:CommandField>                                    
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnActualiza" runat="server" Text="Facturado" CssClass="btn-warning" CommandArgument='<%# Eval("ticket")+";"+Eval("estatus") %>' onclick="btnActualiza_Click" OnClientClick="confirm('¿Esta seguro de cambiar a facturado el ticket?')"></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                </Columns>
                                <SelectedRowStyle CssClass="alert-success-org" />
                            </asp:GridView>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                                SelectCommand="select v.ticket,v.fecha_venta,cli.Email,v.id_cliente,cli.archivarNombre,v.estatus from venta_enc v left join catAlmacenes c on c.idAlmacen=v.id_punto inner join clientes cli on cli.clave = v.id_cliente where v.factura_Posterior=1 and v.estatus=@estatus and id_cliente is not null and v.id_punto=@id_punto">
                                <SelectParameters>
                                    <asp:ControlParameter Name="id_punto" ControlID="ddlIslas" PropertyName="SelectedValue" />
                                    <asp:ControlParameter Name="estatus" ControlID="ddlEstatus" PropertyName="SelectedValue" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div class="col-lg-2 col-sm-2 center"></div>
        </div>
        <div class="row">
            <div class="col-lg-2 col-sm-2 center"></div>
            <div class="col-lg-8 col-sm-8 center">
                <div class="table-responsive">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="GrdDetalle" ShowHeaderWhenEmpty="True"
                                AutoGenerateColumns="False" AllowSorting="True"
                                EmptyDataText="No existen movimientos registrados" 
                                EmptyDataRowStyle-ForeColor="Red" AllowPaging="True" PageSize="5"
                                CssClass="table table-bordered" 
                                DataSourceID="SqlDataSource3">
                                <Columns>
                                    <asp:BoundField DataField="renglon" HeaderText="renglon" SortExpression="renglon" Visible="false" />
                                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" />
                                    <asp:BoundField DataField="id_refaccion" HeaderText="Clave" SortExpression="id_refaccion" />
                                    <asp:BoundField DataField="descripcion" HeaderText="Producto" SortExpression="descripcion" />
                                    <asp:BoundField DataField="venta_unitaria" HeaderText="Precio Venta" SortExpression="venta_unitaria" DataFormatString="{0:C2}" />
                                    <asp:BoundField DataField="importe" HeaderText="Importe" SortExpression="importe" DataFormatString="{0:C2}"/>
                                </Columns>
                                <EmptyDataRowStyle ForeColor="Red" />
                                <SelectedRowStyle CssClass="alert-success-org" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="SELECT V.renglon,V.cantidad,V.id_refaccion,V.descripcion,V.venta_unitaria,V.importe FROM venta_det V WHERE V.id_punto=@id_punto AND V.ticket=@ticket">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlIslas" Name="id_punto" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="GridFactPend" Name="ticket" PropertyName="SelectedValue" />
                                </SelectParameters>
                            </asp:SqlDataSource>                                                            
                        </ContentTemplate>
                    </asp:UpdatePanel>                        
                </div>
            </div>
            <div class="col-lg-2 col-sm-2 center"></div>
        </div>
    </div>
</asp:Content>

