<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="ConsultaCancelaciones.aspx.cs" Inherits="ConsultaCancelaciones" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <center>
    <div class="ancho95 centrado center">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
               <i class="icon-ban-circle"></i> <asp:Label runat="server" ID="lblTitulo" Text="Cancelaciones" CssClass="alert-success"></asp:Label>
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
                            <asp:DropDownList ID="ddlIslas" runat="server" DataSourceID="SqlDataSource2" CssClass="input-medium" 
                                 DataTextField="nombre" DataValueField="idAlmacen" AutoPostBack="true" 
                                 onselectedindexchanged="ddlIslas_SelectedIndexChanged" ></asp:DropDownList>
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
                            EmptyDataRowStyle-CssClass="errores" EmptyDataText="No existen cancelaciones registradas"
                            CssClass="table table-bordered center" AutoGenerateColumns="False"  
                            AllowPaging="True" AllowSorting="True" PageSize="5"
                            DataSourceID="SqlDataSource1" >
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
                                <asp:BoundField DataField="t_cancelacion" HeaderText="Cancelaciones" SortExpression="t_cancelacion" DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="id_punto" HeaderText="id_punto" Visible="false" SortExpression="id_punto" />
                            </Columns>
                            <SelectedRowStyle CssClass="alert-success-org" />
                            <EmptyDataRowStyle CssClass="errores" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand=" select c.anio,c.id_caja,Upper(c.usuario) as usuario,Convert(char(10),c.fecha_apertura,126) as fecha_apertura,Convert(char(10),c.hora_apertura,108) as hora_apertura,
  Convert(char(10),c.fecha_cierre,126) as fecha_cierre,Convert(char(10),c.hora_cierre,108) as hora_cierre,c.t_cancelacion,c.id_punto,p.nombre_pv
  from cajas c
  left join punto_venta p on p.id_punto=c.id_punto
  where c.fecha_apertura between @fechaIni and @fechaFin and c.id_punto=@id_punto and c.id_caja in (select cc.id_caja from Cancelaciones_enc cc where cc.id_punto=c.id_punto and cc.id_caja=c.id_caja) order by id_caja desc">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtFechaIni" Name="fechaIni" PropertyName="Text" />
                                <asp:ControlParameter ControlID="txtFechaFin" Name="fechaFin" PropertyName="Text" />
                                <asp:ControlParameter ControlID="ddlIslas" Name="id_punto" PropertyName="SelectedValue" />
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
                        <asp:GridView ID="GridView2" runat="server" 
                            EmptyDataRowStyle-CssClass="errores" EmptyDataText="Seleccione una caja para mostrar sus cancelaciones"
                            CssClass="table table-bordered center" AutoGenerateColumns="False"  
                            AllowPaging="True" AllowSorting="True" PageSize="5" onpageindexchanging="GridView2_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Cancelacion" SortExpression="id_cancelacion">
                                     <ItemTemplate>
                                        <asp:LinkButton ID="lnkTicket" runat="server" CssClass="alert-info negritas" CommandArgument='<%# Eval("id_cancelacion") %>' onclick="lnkTicket_Click"><asp:Label ID="Label5" runat="server" Text='<%# Eval("id_cancelacion") %>'></asp:Label></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ticket" HeaderText="Ticket" SortExpression="ticket" />                                
                                <asp:BoundField DataField="id_caja_ticket" HeaderText="Caja Ticket" SortExpression="id_caja_ticket" />
                                <asp:BoundField DataField="fecha" HeaderText="Fecha" SortExpression="fecha" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="hora" HeaderText="Hora" SortExpression="hora" DataFormatString="{0:HH:mm:ss}"/>    
                                <asp:BoundField DataField="usuario" HeaderText="Usuario" SortExpression="usuario" />    
                                <asp:BoundField DataField="total" HeaderText="Total" SortExpression="total" DataFormatString="{0:C2}" />                                                                                         
                            </Columns>
                            <SelectedRowStyle CssClass="alert-success-org" />
                            <EmptyDataRowStyle CssClass="errores" />
                        </asp:GridView>
                    </div>
                   
                    <div class="table-responsive col-lg-6 col-sm-6">
                        <asp:GridView ID="GridView3" runat="server" 
                            EmptyDataRowStyle-CssClass="errores" EmptyDataText="Seleccione una cancelacion para mostrar sus productos"
                            CssClass="table table-bordered center" AutoGenerateColumns="false"  
                            AllowPaging="true" AllowSorting="true" PageSize="5" 
                            onpageindexchanging="GridView3_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="renglon" HeaderText="Renglon" SortExpression="renglon" Visible="false" />
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" />
                                <asp:BoundField DataField="idProducto" HeaderText="Clave" SortExpression="idProducto" />
                                <asp:BoundField DataField="descripcion" HeaderText="Artículo" SortExpression="descripcion" />
                                <asp:BoundField DataField="precio_unitario" HeaderText="P. Unitario" SortExpression="precio_unitario" DataFormatString="{0:C2}" />
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

