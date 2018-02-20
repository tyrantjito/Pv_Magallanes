<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CorteParcial.aspx.cs" Inherits="CorteParcial" MasterPageFile="~/PuntoVenta.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>
    <div class="ancho95 text-center centrado">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
               <i class="icon-retweet"></i> <asp:Label runat="server" ID="lblTitulo" Text="Corte Parcial " CssClass="alert-success"></asp:Label>               
            </div>
        </div>        
        <br />
        <div class="row">
            <div class="col-lg-12 col-sm-12 center">
                <asp:Button ID="btnGenerarCorte" runat="server" Text="Generar" ToolTip="Generar Corte Parcial" CssClass="btn btn-info" onclick="btnGenerarCorte_Click" OnClientClick="confirm('¿Está seguro de realizar el corte parcial?')" />
                <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>
            </div>
        </div>
        <br /> 
        <div class="row">
            <div class="col-lg-12 col-sm-12 text-center alert-danger">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" CssClass="centrado" />
            </div>
        </div> 
        <br />       
            <div class="row">
                <div class="col-lg-12 col-sm-12 center">
                    <div class="table-responsive">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>                                
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowPaging="true" PageSize="7" AllowSorting="true"
                                    DataSourceID="SqlDataSource1" EmptyDataText="No se ha realizado ningún corte parcial" EmptyDataRowStyle-ForeColor="Red" CssClass="table table-bordered center">
                                    <Columns>
                                        <asp:BoundField DataField="id_corteParcial" HeaderText="id_corteParcial" Visible="false"
                                            SortExpression="id_corteParcial" />
                                        <asp:BoundField DataField="hora" HeaderText="hora" ReadOnly="True" 
                                            SortExpression="hora" />
                                        <asp:BoundField DataField="fondo" HeaderText="fondo" SortExpression="fondo" DataFormatString="{0:C2}" />
                                        <asp:BoundField DataField="efectivo" HeaderText="Venta Efectivo" SortExpression="efectivo" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="debito" HeaderText="Venta T.débito" SortExpression="debito" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="credito" HeaderText="Venta T.crédito" SortExpression="credito" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="gastos" HeaderText="gastos" SortExpression="gastos" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="cancelaciones" HeaderText="cancelaciones" SortExpression="cancelaciones" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="ventaTaller" HeaderText="Venta Taller" SortExpression="ventaTaller" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="ventaCredito" HeaderText="Venta Crédito" SortExpression="ventaCredito" DataFormatString="{0:C2}"/>
                                        <asp:BoundField DataField="total" HeaderText="Venta Total" SortExpression="total" DataFormatString="{0:C2}"/>
                                    </Columns>
                                </asp:GridView>                           
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                    SelectCommand="SELECT id_corteParcial,convert(char(10), hora,108) as hora,fondo,efectivo,debito,credito,gastos,total,cancelaciones,ventaTaller,ventaCredito FROM cortes_parciales where fecha=@fecha and idAlmacen=@idAlmacen and usuario=@usuario order by hora desc">
                                    <SelectParameters>
                                        <asp:ControlParameter Name="fecha" ControlID="Label1" PropertyName="Text" />
                                        <asp:QueryStringParameter Name="idAlmacen" QueryStringField="p" />
                                        <asp:QueryStringParameter Name="usuario" QueryStringField="u" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        
                    </div>
                </div>
            </div>        
    </div>
</asp:Content>
