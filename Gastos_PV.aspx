<%@ Page Title="" Language="C#" MasterPageFile="~/PuntoVenta.master" AutoEventWireup="true" CodeFile="Gastos_PV.aspx.cs" Inherits="Gastos_PV" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="row">
        <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
            <i class="icon-dollar"></i>&nbsp;&nbsp;<asp:Label runat="server" ID="lblTitulo" Text="Gastos" CssClass="alert-success"></asp:Label>&nbsp;&nbsp;<i class="icon-dollar"></i>
        </div>
    </div>
    <br />
    <center>
        <table>
            <tr>
                <td>
                    <asp:TextBox ID="txtMonto" CssClass="alto30px" runat="server" placeholder="Monto" MaxLength="7" />
                    <ajaxToolkit:FilteredTextBoxExtender BehaviorID="txtMonto_FilteredTextBoxExtender" 
                        ID="txtMonto_FilteredTextBoxExtender" FilterType="Numbers, Custom"
                        ValidChars="." runat="server" TargetControlID="txtMonto" />
                    <asp:RequiredFieldValidator ControlToValidate="txtMonto" Text="*" CssClass="errores" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Necesita colocar un Monto." ValidationGroup="agrega" />
                </td>
                <td>
                    <asp:TextBox ID="txtMotivo" CssClass="alto30px" runat="server" placeholder="Justificación" MaxLength="500" />
                    <asp:RequiredFieldValidator ControlToValidate="txtMotivo" Text="*" CssClass="errores" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Necesita colocar una Justificación." ValidationGroup="agrega" />
                </td>
                <td>
                    <asp:TextBox ID="txtReferencia" runat="server" CssClass="alto30px" placeholder="Referencia" MaxLength="150" />
                </td>
                <td class="text-right">
                    <asp:Button ID="btnNuevoGasto" runat="server" Text="Agregar" OnClick="btnNuevoGasto_Click" CssClass="btn-success" ValidationGroup="agrega"/></td>
            </tr>
            <tr>
                <td colspan="4" class="text-center">
                    <asp:Label ID="lblErrores" CssClass="errores" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="4" class="text-center">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errores" />
                </td>
            </tr>
        </table>
        <div class="ancho95 text-center centrado">
            <asp:GridView ID="GridGastos" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
                EmptyDataText="No cuenta con ningun gasto registrado el dia de hoy en la actual caja." 
                CssClass="table table-bordered center" AllowSorting="true" >
                <Columns>
                    <asp:BoundField DataField="id_caja" HeaderText="Caja" SortExpression="id_caja"></asp:BoundField>
                    <asp:BoundField DataField="usuario" HeaderText="Usuario" SortExpression="usuario"></asp:BoundField>
                    <asp:BoundField DataField="importe" HeaderText="Importe" SortExpression="importe"></asp:BoundField>
                    <asp:TemplateField HeaderText="Fecha" SortExpression="fecha">
                        <EditItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("fecha", "{0:yyyy-MM-dd}") %>' ID="TextBox1"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Bind("fecha", "{0:yyyy-MM-dd}") %>' ID="Label1"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:BoundField DataField="hora" HeaderText="Hora" SortExpression="hora"></asp:BoundField>
                    <asp:BoundField DataField="referencia" HeaderText="Referencia" SortExpression="referencia"></asp:BoundField>
                </Columns>
                <EmptyDataRowStyle CssClass="errores" />
            </asp:GridView>
            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                SelectCommand="select id_caja,usuario,importe,fecha,hora,referencia from gastos where idAlmacen=@idAlmacen and id_caja=@id_caja and usuario=@usuario">
                <SelectParameters>
                    <asp:QueryStringParameter Name="usuario" QueryStringField="u" />
                    <asp:QueryStringParameter Name="idAlmacen" QueryStringField="p" />
                    <asp:QueryStringParameter Name="id_caja" QueryStringField="c" />                    
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </center>
    <asp:Panel ID="Panel1" runat="server" Visible="false" >
        <asp:Label ID="lblUsuario" runat="server" />
        <asp:Label ID="lblIsla" runat="server" />
        <asp:Label ID="lblCaja" runat="server" />
    </asp:Panel>
</asp:Content>

