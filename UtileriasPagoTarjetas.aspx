<%@ Page Title="" Language="C#" MasterPageFile="~/PuntoVenta.master" AutoEventWireup="true" CodeFile="UtileriasPagoTarjetas.aspx.cs" Culture="es-Mx" UICulture="es-Mx" Inherits="UtileriasPagoTarjetas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"/>    
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <div class="venta">
        <div class="row centrado">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
                <i class="icon-ban-circle"></i>
                <asp:Label runat="server" ID="lblTitulo" Text="Cancelación Pagos Tarjeta" CssClass="alert-success"></asp:Label>
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
                    <div class="col-lg-8 col-sm-8">
                        <asp:Label ID="Label55" runat="server" Text="Referencia a Cancelar:"></asp:Label>
                        <asp:TextBox ID="txtRefeCancelar" runat="server" MaxLength="200" CssClass="input-medium" placeholder="Referencia a Cancelar"></asp:TextBox>
                    </div>
                    <div class="col-lg-4 col-sm-4">
                        <asp:LinkButton ID="lnkCancelarPago" OnClick="lnkCancelarPago_Click" runat="server" CssClass="btn btn-warning"><i class="icon-ban-circle"></i>&nbsp;<span>Cancelar Pago</span></asp:LinkButton>
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

