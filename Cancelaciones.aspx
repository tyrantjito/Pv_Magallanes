<%@ Page Title="" Language="C#" MasterPageFile="~/PuntoVenta.master" AutoEventWireup="true" CodeFile="Cancelaciones.aspx.cs" Inherits="Cancelaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"/>    
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <div class="venta">
        <div class="row centrado">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
                <i class="icon-ban-circle"></i>
                <asp:Label runat="server" ID="lblTitulo" Text="Cancelaciones y/o Devoluciones" CssClass="alert-success"></asp:Label>
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
                    <div class="col-lg-3 col-sm-3">
                        <asp:Label ID="Label1" runat="server" Text="Ticket:"></asp:Label>
                        <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radTicket" Skin="MetroTouch" CssClass="input-mini" Value="0" MinValue="1" ShowSpinButtons="true" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                    </div>
                    <div class="col-lg-3 col-sm-3">
                        <asp:LinkButton ID="lnkConsultar" OnClick="lnkConsultar_Click" runat="server" CssClass="btn btn-success"><i class="icon-search"></i>&nbsp;<span>Buscar Ticket</span></asp:LinkButton>
                    </div>
                    <div class="col-lg-3 col-sm-3">
                        <asp:CheckBox ID="chkCancelacion" runat="server" Text="Cancelacion Completa del o los productos" Visible="false" /></div>
                    <div class="col-lg-3 col-sm-3">
                        <asp:LinkButton ID="lnkCancelacionVenta" runat="server" 
                            CssClass="btn btn-primary" onclick="lnkCancelacionVenta_Click" OnClientClick="return confirm('¿Está seguro de realizar la cancelación de la venta?')"><i class="icon-ban-circle"></i>&nbsp;<span>Cancelar Venta</span></asp:LinkButton>
                    </div>
                    
                </div>

                <div class="row marTop">
                    <div class="col-lg-12 col-sm-12 center pad1em">
                        <asp:Panel runat="server" ID="pnlProd" CssClass="maxAlto500px" ScrollBars="auto">
                            <div class="table-responsive">                                
                                <asp:GridView ID="grvVenta" runat="server" CssClass="table table-bordered center textcentrado"
                                    AutoGenerateColumns="False" EmptyDataText="No existen Productos " AllowPaging="false"
                                    DataKeyNames="renglon" onrowdeleting="grvVenta_RowDeleting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="No." Visible="false">
                                            <EditItemTemplate>
                                                <asp:Label ID="lblIdArticulo" runat="server" Text='<%# Eval("renglon") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label10" runat="server" Text='<%# Bind("renglon") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="clave" HeaderText="Clave" ReadOnly="true" />
                                        <asp:BoundField DataField="producto" HeaderText="Producto" ReadOnly="true" />
                                        <asp:TemplateField HeaderText="Cantidad">
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" runat="server" Text='<%# Bind("cantidad") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Precio">
                                            <ItemTemplate>
                                                <asp:Label ID="Label9" runat="server" Text='<%# Bind("precio") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Importe">
                                            <ItemTemplate>
                                                <asp:Label ID="Label8" runat="server" Text='<%# Bind("total") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEliminar" runat="server" CausesValidation="False" OnClientClick="return confirm('¿Está seguro de cancelar la venta de este producto?')" CommandArgument='<%# Eval("renglon")+";"+Eval("clave")+";"+Eval("cantidad")+";"+Eval("producto")+";"+Eval("precio") %>'
                                                    CommandName="Delete" ToolTip="Cancelar" CssClass="btn btn-danger alineados" OnClick="btnCancelar_Click"><i class="icon-ban-circle"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>                                    
                            </div>
                        </asp:Panel>
                    </div>
                </div>


                <asp:Panel ID="PanelMask" runat="server" Visible="false" CssClass="mask"/>
                <asp:Panel ID="pnlTicket" runat="server" Visible="false" CssClass="popupw pading8px">
                    <asp:Panel ID="Panel1" runat="server" CssClass="text-center">
                        <div class="col-lg-12 col-sm-12 text-center ticket">
                            <asp:Label ID="Label15" runat="server" Text="Producto:"></asp:Label>
                            <asp:Label ID="lblProductoCancelar" runat="server" CssClass="text-info"></asp:Label>
                        </div>
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="Label34" runat="server" Text="Cantidad: " />
                            <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="RadCantidadCancela" Skin="MetroTouch" CssClass="input-mini" Value="0" MinValue="0" ShowSpinButtons="true" NumberFormat-DecimalDigits="3"></telerik:RadNumericTextBox>
                        </div>                       
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="lblErrorCantidad" runat="server" CssClass="errores" />
                        </div>
                        <div class="col-lg-6 col-sm-6 text-center">
                             <asp:LinkButton ID="lnkAceptarCant" runat="server" CssClass="btn btn-success" 
                                 onclick="lnkAceptarCant_Click"><i class="icon-check"></i>&nbsp;<span>Aceptar</span></asp:LinkButton>
                        </div>
                        <div class="col-lg-6 col-sm-6 text-center">
                             <asp:LinkButton ID="lnkCancelarCant" runat="server" CssClass="btn btn-danger" 
                                 onclick="lnkCancelarCant_Click"><i class="icon-remove"></i>&nbsp;<span>Cancelar</span></asp:LinkButton>
                        </div>
                    </asp:Panel>
                </asp:Panel>


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

