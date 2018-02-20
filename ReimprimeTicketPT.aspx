<%@ Page Title="" Language="C#" MasterPageFile="~/PuntoVenta.master" AutoEventWireup="true" CodeFile="ReimprimeTicketPT.aspx.cs" Inherits="ReimprimeTicketPT" Culture="es-Mx" UICulture="es-Mx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"/>    
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <div class="venta">
        <div class="row centrado">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
                <i class="icon-print"></i>
                <asp:Label runat="server" ID="lblTitulo" Text="Reimpresión de Ticket Pago Tarjeta" CssClass="alert-success"></asp:Label>
            </div>
        </div>                    
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="row center marTop">
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:Label ID="lblError" runat="server" CssClass="validaciones"></asp:Label>
                    </div>
                </div>
                <div class="row marTop text-center">
                    <div class="col-lg-12 col-sm-12">
                        <asp:Label ID="Label55" runat="server" Text="Referencia:"></asp:Label>
                        <asp:TextBox ID="txtRefeCancelar" runat="server" MaxLength="200" CssClass="input-medium" placeholder="Referencia"></asp:TextBox>
                    </div>
                </div>
                <div class="row marTop text-center">
                    <div class="col-lg-6 col-sm-6">
                        <asp:LinkButton ID="lnkCancelarPago" OnClick="lnkCancelarPago_Click" runat="server" CssClass="btn btn-info"><i class="icon-print"></i>&nbsp;<span>Reimprimir Ticket Pago Tarjeta</span></asp:LinkButton>
                    </div>
                    <div class="col-lg-6 col-sm-6">
                        <asp:LinkButton ID="lnkImprime" OnClick="lnkImprime_Click" runat="server" CssClass="btn btn-info"><i class="icon-print"></i>&nbsp;<span>Reimprimir Ticket de Venta</span></asp:LinkButton>
                    </div>
                </div>


                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                    <ProgressTemplate>
                        <asp:Panel ID="pnlMaskLoad1" runat="server" CssClass="maskLoad"></asp:Panel>
                        <asp:Panel ID="pnlCargando1" runat="server" CssClass="pnlPopUpLoad padding8px">
                            <asp:Image ID="imgLoad1" runat="server" ImageUrl="~/img/loading.gif" Width="80%" />
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

