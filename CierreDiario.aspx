<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CierreDiario.aspx.cs" Inherits="CierreDiario" MasterPageFile="~/PuntoVenta.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>
    <div class="ancho95 text-center centrado">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
               <i class="icon-briefcase"></i> <asp:Label runat="server" ID="lblTitulo" Text="Cierre del Día " CssClass="alert-success"></asp:Label>
                <br />
               Del: <asp:Label ID="lblDiaApertura" runat="server"></asp:Label> Al:
               <asp:Label ID="lblDia" runat="server"></asp:Label>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-lg-12 col-sm-12 center">
                <asp:Button ID="btnGenerarCorte" runat="server" Text="Generar Cierre" ToolTip="Generar Cierre del Día" CssClass="btn btn-info" onclick="btnGenerarCorte_Click" OnClientClick="confirm('¿Está seguro de realizar el cierre del dia, tome en cuenta que no podra efectuar otra venta por el dia de hoy?')" />                
            </div>
        </div>                
        <br />        
            <div class="row">
                <div class="col-lg-12 col-sm-12 center">
                    <div class="table-responsive">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>                                
                                <div class="row">
                                    <div class="col-lg-12 col-sm-12 text-center alert-danger">
                                        <asp:Label ID="lblError" runat="server" ForeColor="Red" CssClass="centrado" />
                                    </div>
                                </div>
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                                    AllowPaging="True" PageSize="3" AllowSorting="True"
                                    DataSourceID="SqlDataSource1" DataKeyNames="id_cierre"
                                    EmptyDataText="No se ha realizado corte de día" 
                                    EmptyDataRowStyle-ForeColor="Red" CssClass="table table-bordered center">
                                    <Columns>
                                        <asp:BoundField DataField="id_cierre" HeaderText="id_cierre" SortExpression="id_cierre" Visible="false" />
                                        <asp:BoundField DataField="hora_cierre" HeaderText="hora cierre" SortExpression="hora_cierre" />
                                        <asp:BoundField DataField="usuario_cierre" HeaderText="usuario cierre" SortExpression="usuario_cierre" />
                                        <asp:BoundField DataField="fondo" HeaderText="fondo" SortExpression="fondo" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="efectivo" HeaderText="Venta Efectivo" SortExpression="efectivo" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="debito" HeaderText="Venta T. débito" SortExpression="debito" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="credito" HeaderText="Venta T. crédito" SortExpression="credito" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="pagoServicios" HeaderText="Venta Pago Servicios" SortExpression="pagoServicios" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="recargas" HeaderText="Venta Recargas" SortExpression="recargas" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="gastos" HeaderText="Total Gastos" SortExpression="gastos" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="cancelaciones" HeaderText="Total Cancelaciones" SortExpression="cancelaciones" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="total" HeaderText="Venta Total" SortExpression="total" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="ventaTaller" HeaderText="Venta Taller" SortExpression="ventaTaller" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="ventaCredito" HeaderText="Venta Crédito" SortExpression="ventaCredito" DataFormatString="{0:C2}"/>
                                    </Columns>
                                    <EmptyDataRowStyle ForeColor="Red" />                                    
                                </asp:GridView>                           
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                    SelectCommand="select tabla.id_cierre,tabla.hora_cierre,tabla.usuario_cierre,tabla.fondo,tabla.debito,tabla.credito,
                                        tabla.recargas,tabla.pagoServicios,
                                        tabla.efectivo,
                                        tabla.gastos,tabla.total,tabla.cancelaciones,tabla.ventaTaller,tabla.ventaCredito
                                        from (
                                        (select c.id_cierre,c.hora_cierre,c.usuario_cierre,c.fondo,c.efectivo,c.debito,c.credito,
                                        c.gastos,c.total,c.cancelaciones,c.recargas,c.pagoServicios,c.ventaTaller,c.ventaCredito
                                        from cierres_diarios c
                                        where fecha_cierre=@fecha and id_punto_venta=@id_punto_venta)
                                        )as tabla">
                                    <SelectParameters>
                                        <asp:ControlParameter Name="fecha" ControlID="lblDia" PropertyName="Text" />
                                        <asp:QueryStringParameter QueryStringField="p" Name="id_punto_venta" />
                                    </SelectParameters>
                                </asp:SqlDataSource>                                    
                                <br />
                                <div class="col-lg-6 col-sm-6 text-center">
                                    <div class="table-responsive">
                                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" EmptyDataText="No se han registrado movimientos el día de hoy" 
                                            EmptyDataRowStyle-ForeColor="Red" CssClass="table table-bordered center"  AllowPaging="true" AllowSorting="true" PageSize="8"
                                            DataSourceID="SqlDataSource2">
                                            <Columns>
                                                <asp:BoundField DataField="usuario" HeaderText="usuario" SortExpression="usuario" />
                                                <asp:BoundField DataField="saldo_inicial" HeaderText="Fondo" SortExpression="saldo_inicial" Visible="false" />
                                                <asp:BoundField DataField="efe" HeaderText="Efectivo" ReadOnly="True" SortExpression="efe" />
                                                <asp:BoundField DataField="cred" HeaderText="T. Crédito" ReadOnly="True" SortExpression="cred" />
                                                <asp:BoundField DataField="deb" HeaderText="T. Débito" ReadOnly="True" SortExpression="deb" />
                                                <asp:BoundField DataField="gastos" HeaderText="Gastos" ReadOnly="True" SortExpression="gastos" />
                                                <asp:BoundField DataField="cancelaciones" HeaderText="Cancelaciones" ReadOnly="True" SortExpression="cancelaciones" />
                                                <asp:BoundField DataField="recargas" HeaderText="Recargas" ReadOnly="True" SortExpression="recargas" />
                                                <asp:BoundField DataField="pagoServicios" HeaderText="pagoServicios" ReadOnly="True" SortExpression="pagoServicios" />
                                                <asp:BoundField DataField="ventaTaller" HeaderText="Venta Taller" ReadOnly="True" SortExpression="ventaTaller" />
                                                <asp:BoundField DataField="ventaCredito" HeaderText="Venta Crédito" ReadOnly="True" SortExpression="ventaCredito" />
                                                <asp:BoundField DataField="saldo" HeaderText="Saldo" ReadOnly="True" SortExpression="saldo" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select usuario,saldo_inicial,sum(efectivo) as efe,sum(t_credito) as cred, sum(t_debito) as deb, sum(t_gastos) as gastos,sum(t_cancelacion) as cancelaciones, sum(saldo_corte) as saldo, sum(recargas) as recargas, sum(pagoServicios) as pagoServicios, sum(ventaTaller) as ventaTaller, sum(ventaCredito) as ventaCredito
from cajas where fecha_apertura = @fecha_apertura and id_punto=@id_punto group by usuario,saldo_inicial">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="lblDia" Name="fecha_apertura" PropertyName="Text" />                                                
                                                <asp:QueryStringParameter Name="id_punto" QueryStringField="p" />                                                
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                </div>  
                                                       
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        
                    </div>
                </div>
            </div>        
    </div>
</asp:Content>
