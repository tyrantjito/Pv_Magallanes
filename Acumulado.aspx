<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Acumulado.aspx.cs" Inherits="Acumulado" MasterPageFile="~/Administracion.master" Culture="es-Mx" UICulture="es-Mx"%>

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
               <i class="icon-file"></i>&nbsp;&nbsp;<asp:Label runat="server" ID="lblTitulo" Text="Acumulado" CssClass="alert-success"></asp:Label>&nbsp;&nbsp;<i class="icon-signal"></i>
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
                                 DataTextField="nombre" DataValueField="idAlmacen" AutoPostBack="True" OnSelectedIndexChanged="ddlIslas_SelectedIndexChanged" ></asp:DropDownList>
                             <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="SELECT 0 AS idAlmacen,'Seleccione una Tienda' AS nombre union all select idAlmacen,nombre from catalmacenes where estatus='A' order by 1"></asp:SqlDataSource>
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
                             AllowSorting="True" OnRowDataBound="GridView1_RowDataBound"
                            DataSourceID="SqlDataSource1" DataKeyNames="usuario,fecha">
                            <Columns>
                                <asp:BoundField DataField="usuario" HeaderText="Usuario" ReadOnly="True" SortExpression="usuario" Visible="false" />
                                <asp:BoundField DataField="fecha" HeaderText="Fecha" SortExpression="fecha" ReadOnly="True" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:BoundField DataField="tot_calculado" HeaderText="Tot. Calculado" SortExpression="tot_calculado" Visible="false" />
                                <asp:BoundField DataField="efectivo" HeaderText="Efectivo" SortExpression="efectivo" DataFormatString="{0:C2}" />
                                <asp:BoundField DataField="debito" HeaderText="T. Débito" SortExpression="debito"  DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="credito" HeaderText="T. Crédito" SortExpression="credito" DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="pagoServicios" HeaderText="Pago Servicios" SortExpression="pagoServicios" DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="recargas" HeaderText="Recargas" SortExpression="recargas" DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="gastos" HeaderText="Gastos" SortExpression="gastos" DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="cancelaciones" HeaderText="Cancelaciones" SortExpression="cancelaciones" DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="fondo" HeaderText="Fondo" SortExpression="fondo" DataFormatString="{0:C2}"/>                                
                                <asp:BoundField DataField="ventaTaller" HeaderText="Venta Taller" SortExpression="ventaTaller" DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="ventaCredito" HeaderText="Venta Credito" SortExpression="ventaCredito" DataFormatString="{0:C2}"/>
                                <asp:BoundField DataField="total" HeaderText="Total" SortExpression="total" DataFormatString="{0:C2}"/>
                                <asp:TemplateField HeaderText="Acumulado">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAcum" runat="server" ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <SelectedRowStyle CssClass="alert-success-org" />
                            <EmptyDataRowStyle CssClass="errores" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                            SelectCommand="generaAcumulado" SelectCommandType="StoredProcedure">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtFechaIni" Name="fechaIni" PropertyName="Text" />
                                <asp:ControlParameter ControlID="txtFechaFin" Name="fechaFin" PropertyName="Text" />
                                <asp:ControlParameter ControlID="ddlIslas" Name="idAlmacen" PropertyName="SelectedValue" Type="Int16" />
                                <asp:QueryStringParameter Name="usuario" QueryStringField="u" Type="String" />                                
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
                
                <div class="row">
                        <div class="col-lg-12 col-sm-12 center">
                            <asp:Button ID="btn_imprimir" runat="server" Text="Imprimir" CssClass="btn btn-info" 
                                onclick="btn_imprimir_Click" />
                        </div>
                    </div>
                         
                         <br /><br /><br />                                    
             </ContentTemplate>
             </asp:UpdatePanel>   
                
        </div>
        
    </div>   
    </center>  
</asp:Content>
