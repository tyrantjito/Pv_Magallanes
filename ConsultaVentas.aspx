<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConsultaVentas.aspx.cs" Inherits="ConsultaVentas" MasterPageFile="~/Administracion.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <center>
    <div class="ancho95 centrado center">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
               <i class="icon-search"></i> <asp:Label runat="server" ID="lblTitulo" Text="Ventas" CssClass="alert-success"></asp:Label><i class="icon-shopping-cart"></i>
            </div>
        </div>
        <br />
         <div class="row center centrado " >
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
             <ContentTemplate>
                <div class="col-lg-1 col-sm-1 center"></div>          
                <div class="col-lg-10 col-sm-10 center">
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:Label ID="Label1" runat="server" Text="Fecha Incial:"></asp:Label>
                            <asp:TextBox ID="txtFechaIni" runat="server" CssClass="input-medium" MaxLength="10" Enabled="false"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" PopupButtonID="lnkFini"
                                BehaviorID="txtFechaIni_CalendarExtender" TargetControlID="txtFechaIni" Format="yyyy-MM-dd">
                            </cc1:CalendarExtender>
                            <asp:LinkButton ID="lnkFini" runat="server" CssClass="btn btn-info"><i class="icon-calendar"></i></asp:LinkButton>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:Label ID="Label2" runat="server" Text="Fecha Final:"></asp:Label>
                            <asp:TextBox ID="txtFechaFin" runat="server" CssClass="input-medium" MaxLength="10" Enabled="false"></asp:TextBox>                        
                            <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" PopupButtonID="lnkFFin"
                                BehaviorID="txtFechaFin_CalendarExtender" TargetControlID="txtFechaFin" Format="yyyy-MM-dd">
                            </cc1:CalendarExtender>
                            <asp:LinkButton ID="lnkFFin" runat="server" CssClass="btn btn-info"><i class="icon-calendar"></i></asp:LinkButton>
                        </div>
                        <div class="col-lg-3 col-sm-3 text-left">
                             <asp:Label ID="Label7" runat="server" Text="Tienda:"></asp:Label>
                            <asp:DropDownList ID="ddlIslas" runat="server" DataSourceID="SqlDataSource2" CssClass="input-medium"  AppendDataBoundItems="true"
                                 DataTextField="nombre" DataValueField="idAlmacen" AutoPostBack="true" OnSelectedIndexChanged="ddlIslas_SelectedIndexChanged">
                                <asp:ListItem Text="--Seleccione Tienda--" Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                             <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select u.id_punto as idAlmacen,p.nombre_pv as nombre from usuario_puntoventa U inner join punto_venta p on p.id_punto=u.id_punto where U.usuario=@usuario and U.estatus='A'">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="usuario" QueryStringField="u" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                        <div class="col-lg-1 col-sm-1 text-center">
                            <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="btn-info" ValidationGroup="busca" onclick="btnBuscar_Click"  />
                        </div>
                    </div>                    
                    <div class="row">
                        <div class="col-lg-10 col-sm-10 center alert-danger">
                            <asp:Label ID="lblError" runat="server" CssClass="errores negritas"></asp:Label>
                        </div>
                    </div>               
                </div>  
                <div class="col-lg-1 col-sm-1 center"></div>  
                <br /><br />
                <div class="ancho95 centrado center">
                    <div class="table-responsive">
                        <asp:GridView ID="GridView1" runat="server" 
                            EmptyDataRowStyle-CssClass="errores" EmptyDataText="No existen ventas registradas"
                            CssClass="table table-bordered center" AutoGenerateColumns="False"  
                            AllowPaging="True" AllowSorting="True" PageSize="5"
                            DataSourceID="SqlDataSource1" OnRowDataBound="GridView1_RowDataBound" >
                            <Columns>
                                <asp:BoundField DataField="anio" HeaderText="anio" ReadOnly="True" Visible="false" SortExpression="anio" />
                                <asp:TemplateField HeaderText="Caja" SortExpression="id_caja">
                                    <EditItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("id_caja") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCaja" runat="server" CssClass="alert-info negritas" 
                                            CommandArgument='<%# Eval("id_caja")+";"+Eval("fecha_apertura")+";"+Eval("id_punto") %>' 
                                            onclick="lnkCaja_Click"><asp:Label ID="Label4" runat="server" Text='<%# Bind("id_caja") %>'></asp:Label></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="nombre_pv" HeaderText="Tienda" SortExpression="nombre_pv" />
                                <asp:BoundField DataField="usuario" HeaderText="Usuario" SortExpression="usuario" />
                                <asp:BoundField DataField="fecha_apertura" HeaderText="Fecha Apertura" SortExpression="fecha_apertura" />
                                <asp:BoundField DataField="hora_apertura" HeaderText="Hora Apertura" SortExpression="hora_apertura" />
                                <asp:BoundField DataField="fecha_cierre" HeaderText="Fecha Cierre" SortExpression="fecha_cierre" />
                                <asp:BoundField DataField="hora_cierre" HeaderText="Hora Cierre" SortExpression="hora_cierre" />
                                <asp:BoundField DataField="efectivo" HeaderText="Efectivo" SortExpression="efectivo" DataFormatString="{0:C2}" />
                                <asp:BoundField DataField="t_credito" HeaderText="T. Crédito" SortExpression="t_credito" DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="t_debito" HeaderText="T. Débito" SortExpression="t_debito" DataFormatString="{0:C2}"/>

                                <asp:BoundField DataField="recargas" HeaderText="Recargas" SortExpression="recargas" DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="pagoServicios" HeaderText="Pago Servicios" SortExpression="pagoServicios" DataFormatString="{0:C2}"/>
                                
                                <asp:TemplateField HeaderText="Total" SortExpression="saldo_corte">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSaldoCorte" runat="server" />
                                        <asp:Label ID="lblSaldoCorteBind" runat="server" Visible="false" Text='<%# Bind("saldo_corte") %>' />
                                        <asp:Label ID="lblRecargas" runat="server" Visible="false" Text='<%# Bind("recargas") %>' />
                                        <asp:Label ID="lblPagoServicios" runat="server" Visible="false" Text='<%# Bind("pagoServicios") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="id_punto" HeaderText="id_punto" Visible="false" SortExpression="id_punto" />
                                <asp:BoundField DataField="ventaTaller" HeaderText="Venta Taller" SortExpression="ventaTaller" DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="ventaCredito" HeaderText="Venta Crédito" SortExpression="ventaCredito" DataFormatString="{0:C2}"/>
                            </Columns>
                            <SelectedRowStyle CssClass="alert-success-org" />
                            <EmptyDataRowStyle CssClass="errores" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                            SelectCommand=" select c.anio,c.id_caja,Upper(c.usuario) as usuario,Convert(char(10),c.fecha_apertura,126) as fecha_apertura,Convert(char(10),c.hora_apertura,108) as hora_apertura,
                                Convert(char(10),c.fecha_cierre,126) as fecha_cierre,Convert(char(10),c.hora_cierre,108) as hora_cierre,c.efectivo,c.t_credito,c.t_debito,c.saldo_corte,c.id_punto,p.nombre_pv
                                ,c. recargas,c.pagoServicios,c.ventaTaller,c.ventaCredito
                                from cajas c
                                left join punto_venta p on p.id_punto=c.id_punto
                                where c.fecha_apertura between @fechaIni and @fechaFin and  c.id_punto=@id_punto order by id_caja desc">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtFechaIni" Name="fechaIni" PropertyName="Text" />
                                <asp:ControlParameter ControlID="txtFechaFin" PropertyName="Text" Name="fechaFin"></asp:ControlParameter>
                                <asp:ControlParameter ControlID="ddlIslas" PropertyName="SelectedValue" Name="id_punto" DbType="Int32"></asp:ControlParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>                            
                 <div class="ancho95 centrado center">
                    <div class="row">
                        <div class="alert-info col-lg-6 col-sm-6 center negritas font-14 pad1em">
                           <asp:Label runat="server" ID="lblCaja" CssClass="alert-info"></asp:Label>
                        </div>                                                
                        <div class="alert-info col-lg-6 col-sm-6 center negritas font-14 pad1em">
                           <asp:Label runat="server" ID="lblTicket" CssClass="alert-info"></asp:Label>
                        </div>                    
                    </div>
                    <div class="row">
                        <div class="table-responsive col-lg-6 col-sm-6">
                            <asp:Label runat="server" ID="Label6" Text="Estatus:"></asp:Label>
                            <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="input-medium" 
                                AutoPostBack="true" onselectedindexchanged="ddlEstatus_SelectedIndexChanged">
                                <asp:ListItem Text="Abiertos" Value="A"></asp:ListItem>
                                <asp:ListItem Text="Facturados" Value="F"></asp:ListItem>                                
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                    <div class="table-responsive col-lg-6 col-sm-6">
                        <asp:GridView ID="GridView2" runat="server" 
                            EmptyDataRowStyle-CssClass="errores" EmptyDataText="Seleccione una caja para mostrar sus ventas"
                            CssClass="table table-bordered center" AutoGenerateColumns="False"  
                            AllowPaging="True" AllowSorting="True" PageSize="5" onpageindexchanging="GridView2_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Ticket" SortExpression="ticket">
                                     <ItemTemplate>
                                        <asp:LinkButton ID="lnkTicket" runat="server" CssClass="alert-info negritas" CommandArgument='<%# Eval("ticket")+";"+Eval("id_punto") %>' onclick="lnkTicket_Click"><asp:Label ID="Label5" runat="server" Text='<%# Eval("ticket") %>'></asp:Label></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="subtotal" HeaderText="Subtotal" SortExpression="subtotal" DataFormatString="{0:C2}" />                                
                                <asp:BoundField DataField="iva" HeaderText="I.V.A." SortExpression="iva" DataFormatString="{0:C2}" />
                                <asp:BoundField DataField="total" HeaderText="Total" SortExpression="total" DataFormatString="{0:C2}" />
                                <asp:BoundField DataField="hora_venta" HeaderText="Hora Venta" SortExpression="hora_venta" />    
                                <asp:BoundField DataField="usuario" HeaderText="Usuario Venta" SortExpression="usuario" />    
                                <asp:BoundField DataField="forma_pago" HeaderText="Forma de Pago" SortExpression="forma_pago" />                                    
                                <asp:BoundField DataField="estatus" HeaderText="Estatus" SortExpression="estatus" />
                                <asp:CheckBoxField DataField="ventaTaller" HeaderText="Venta a Taller" SortExpression="ventaTaller" />
                                <asp:CheckBoxField DataField="ventaCredito" HeaderText="Venta a Crédito" SortExpression="credito" />
                            </Columns>
                            <SelectedRowStyle CssClass="alert-success-org" />
                            <EmptyDataRowStyle CssClass="errores" />
                        </asp:GridView>
                    </div>
                   
                    <div class="table-responsive col-lg-6 col-sm-6">
                        <asp:GridView ID="GridView3" runat="server" 
                            EmptyDataRowStyle-CssClass="errores" EmptyDataText="Seleccione una ticket para mostrar sus ventas"
                            CssClass="table table-bordered center" AutoGenerateColumns="false"  
                            AllowPaging="true" AllowSorting="true" PageSize="5" 
                            onpageindexchanging="GridView3_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="renglon" HeaderText="Renglon" SortExpression="renglon" Visible="false" />
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" />
                                <asp:BoundField DataField="id_refaccion" HeaderText="Clave" SortExpression="id_refaccion" />
                                <asp:BoundField DataField="descripcion" HeaderText="Artículo" SortExpression="descripcion" />
                                <asp:BoundField DataField="venta_unitaria" HeaderText="C. Unitario" SortExpression="venta_unitaria" DataFormatString="{0:C2}" />
                                <asp:BoundField DataField="importe" HeaderText="Importe" SortExpression="importe" DataFormatString="{0:C2}"/>                                
                            </Columns>
                        </asp:GridView>
                    </div>
                    </div>
                </div>
             </ContentTemplate>
             </asp:UpdatePanel>   
                
        </div>
        
    </div>   
    </center>  
</asp:Content>