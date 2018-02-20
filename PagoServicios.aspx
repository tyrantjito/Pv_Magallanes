<%@ Page Title="" Language="C#" MasterPageFile="~/PuntoVenta.master" AutoEventWireup="true"
    CodeFile="PagoServicios.aspx.cs" Inherits="PagoServicios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script type="text/javascript">
        function abreWinEmi() {
            var oWnd = $find("<%=modalEmisores.ClientID%>");
            oWnd.setUrl('');
            oWnd.show();
        }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid3">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>           
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="MetroTouch"></telerik:RadAjaxLoadingPanel>

    <asp:Panel ID="pnlVenta" runat="server" CssClass="venta">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
                <i class="icon-cogs"></i>
                <asp:Label runat="server" ID="lblTitulo" Text="Pago de Servicios" CssClass="alert-success"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-4 col-sm-4">
                <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="true" PageSize="20" DataSourceID="SqlDataSource5"
                    OnItemCommand="RadGrid1_ItemCommand" Skin="Metro">
                    <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <MasterTableView DataKeyNames="idCatTipoServicio" Width="100%" AutoGenerateColumns="False">
                        <Columns>
                            <telerik:GridBoundColumn DataField="idCatTipoServicio" DataType="System.Int32" HeaderText="idCatTipoServicio"
                                ReadOnly="True" SortExpression="idCatTipoServicio" UniqueName="idCatTipoServicio" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="TipoServicio" DataType="System.String" HeaderText="Pagos"
                                ReadOnly="True" SortExpression="TipoServicio" UniqueName="TipoServicio" >
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                </telerik:RadGrid>
            </div>
            <div class="col-lg-4 col-sm-4">
                <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid2" ShowStatusBar="true" runat="server" AllowPaging="True"
                    PageSize="20" DataSourceID="SqlDataSource6" OnItemCommand="RadGrid2_ItemCommand" Skin="Metro">
                    <MasterTableView Width="100%" AutoGenerateColumns="False" DataKeyNames="idServicio,servicio"
                        DataSourceID="SqlDataSource6">
                        <Columns>
                            <telerik:GridBoundColumn DataField="idServicio" DataType="System.Int32" HeaderText="idServicio"
                                ReadOnly="True" SortExpression="idServicio" UniqueName="idServicio" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="servicio" DataType="System.String" HeaderText="Servicios"
                                ReadOnly="True" SortExpression="servicio" UniqueName="servicio" >
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                </telerik:RadGrid>
            </div>                    
            <div class="col-lg-4 col-sm-4">
                <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid3" ShowStatusBar="true" runat="server" AllowPaging="True"
                    PageSize="20" DataSourceID="SqlDataSource7" Skin="Metro" OnItemCommand="RadGrid3_ItemCommand">
                    <MasterTableView Width="100%" AutoGenerateColumns="False" DataKeyNames="tipoFront,idProducto,producto"
                        DataSourceID="SqlDataSource7">
                        <Columns>
                            <telerik:GridBoundColumn DataField="tipoFront" HeaderText="tipoFront" SortExpression="tipoFront"
                                UniqueName="tipoFront" Visible="false">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="idProducto" HeaderText="idProducto" SortExpression="idProducto" Visible="false"
                                UniqueName="idProducto">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="producto" HeaderText="Producto" SortExpression="producto"
                                UniqueName="producto">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="precio_tienda" HeaderText="Costo" SortExpression="precio_tienda"
                                UniqueName="precio_tienda">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="comision" HeaderText="Comisión" SortExpression="comision"
                                UniqueName="comision">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="precio" HeaderText="Importe" SortExpression="precio"
                                UniqueName="precio">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                    </ClientSettings>
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                </telerik:RadGrid>
            </div>
            <asp:SqlDataSource ID="SqlDataSource5" ConnectionString="<%$ ConnectionStrings:PVW %>" ProviderName="System.Data.SqlClient" SelectCommand="SELECT idCatTipoServicio, upper(TipoServicio) as TipoServicio FROM CatTipoServicio" runat="server"></asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource6" ConnectionString="<%$ ConnectionStrings:PVW %>" ProviderName="System.Data.SqlClient" SelectCommand="select idServicio,servicio from prod_pago_serv WHERE idCatTipoServicio=@idCatTipoServicio GROUP BY idServicio,servicio order by servicio" runat="server">
                <SelectParameters>
                    <asp:ControlParameter ControlID="RadGrid1" DefaultValue="0" Name="idCatTipoServicio" PropertyName="SelectedValue" Type="String"></asp:ControlParameter>
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource7" ConnectionString="<%$ ConnectionStrings:PVW %>" ProviderName="System.Data.SqlClient" SelectCommand="select tipoFront,idProducto,producto,precio_tienda,comision,precio from Prod_Pago_Serv where idCatTipoServicio=@idCatTipoServicio and idServicio=@idServicio group by tipoFront,idProducto,producto,precio_tienda,comision,precio order by idProducto" runat="server">
                <SelectParameters>
                    <asp:ControlParameter ControlID="RadGrid1" DefaultValue="0" Name="idCatTipoServicio" PropertyName="SelectedValue" Type="String"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="RadGrid2" DefaultValue="0" Name="idServicio" PropertyName="SelectedValue" Type="String"></asp:ControlParameter>
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
        <div class="row marTop">
            <div class="col-lg-12 col-sm-12 text-center alert-info">
                <h4>Captura de Informaci&oacute;n</h4>
            </div>
            </div>

        <div class="row marTop">
            <div class="col-lg-2 col-sm-2 text-center"></div>
            <div class="col-lg-4 col-sm-4">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate> 
                        <div class="row">
                            <div class="col-lg-12 col-sm-12">
                                <asp:Label runat="server" ID="lblError" CssClass="errores"></asp:Label>
                                <asp:Label runat="server" ID="lblOperacion" CssClass="errores" Visible="false"></asp:Label>                              
                                <asp:Label ID="lblIdCatTipoServicio" runat="server"  Visible="false"></asp:Label>
                                <asp:Label ID="lblIdServicio" runat="server"  Visible="false"></asp:Label>
                                <asp:Label ID="lblTipoFront" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblIdProducto" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblServicio" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblDescripcion" runat="server" Visible="false"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12">                                                
                                    <asp:Label ID="Label1" runat="server" Text="Teléfono:"></asp:Label>&nbsp;
                                    <asp:TextBox ID="txtTelefono" runat="server" MaxLength="20" CssClass="input-large"></asp:TextBox>                                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el número de teléfono" ControlToValidate="txtTelefono" Text="*" CssClass="errores" ValidationGroup="abonar"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12">
                                    <asp:Label ID="Label2" runat="server" Text="Confirmar:"></asp:Label>&nbsp;
                                    <asp:TextBox ID="txtTelefonoConfirm" runat="server" MaxLength="20" TextMode="Password" CssClass="input-large"></asp:TextBox>                                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe confirmar el número de teléfono" ControlToValidate="txtTelefonoConfirm" Text="*" CssClass="errores" ValidationGroup="abonar"></asp:RequiredFieldValidator>
                                                
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12">
                                    <asp:Label ID="Label4" runat="server" Text="Dígito Verificador:"></asp:Label>&nbsp;
                                    <asp:TextBox ID="txtDigito" runat="server" MaxLength="20" TextMode="Password" CssClass="input-small"></asp:TextBox>                                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar el dígito verificador" ControlToValidate="txtDigito" Text="*" CssClass="errores" ValidationGroup="abonar"></asp:RequiredFieldValidator>
                                                
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12">
                                    <asp:Label ID="Label5" runat="server" Text="Referencia:"></asp:Label>&nbsp;
                                    <asp:TextBox ID="txtReferencia" runat="server" MaxLength="20" CssClass="input-large"></asp:TextBox>                                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar la referencia" ControlToValidate="txtReferencia" Text="*" CssClass="errores" ValidationGroup="abonar"></asp:RequiredFieldValidator>
                                                
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12 col-sm-12">
                                    <asp:Label ID="Label6" runat="server" Text="Monto a Pagar:"></asp:Label>&nbsp;
                                    <asp:TextBox ID="txtMonto" runat="server" MaxLength="20" CssClass="input-medium"></asp:TextBox>                                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe indicar el monto a pagar" ControlToValidate="txtMonto" Text="*" CssClass="errores" ValidationGroup="abonar"></asp:RequiredFieldValidator>
                                                
                            </div>
                        </div>
                        <div class="row marTop">
                            <div class="col-lg-12 col-sm-12 text-center">                                
                                <asp:Image ID="imgLogo" runat="server" Width="100%"  />
                            </div>                            
                        </div>
                        <div class="row marTop">
                            <div class="col-lg-12 col-sm-12 text-center">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="abonar" CssClass="errores" DisplayMode="List" />
                            </div>
                            <div class="col-lg-12 col-sm-12 text-center">
                                <asp:LinkButton ID="lnkAbonar" runat="server" CssClass="btn btn-primary font-13" OnClick="lnkAbonar_Click" ValidationGroup="abonar"><i class="icon-check"></i>&nbsp;Abonar</asp:LinkButton>
                                <asp:LinkButton ID="lnkImprimir" runat="server" CssClass="btn btn-success font-13" OnClick="lnkImprimir_Click" Visible="false" ><i class="icon-print"></i>&nbsp;Imprimir</asp:LinkButton>
                            </div>
                        </div>

                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                            <ProgressTemplate>
                                <asp:Panel ID="pnlMaskLoad" runat="server" CssClass="maskLoad">
                                </asp:Panel>
                                <asp:Panel ID="pnlCargando" runat="server" CssClass="pnlPopUpLoad padding8px">
                                    <asp:Image ID="imgLoad" runat="server" ImageUrl="~/img/loading.gif" Width="80%" />
                                </asp:Panel>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>           
            
            <div class="col-lg-4 col-sm-4 text-center">
                 <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate> 
                                <asp:Label ID="lblReferenciaInfo" runat="server" Text="¿Donde lo encuentro?"></asp:Label><br />
                                <asp:Image ID="imgReference" runat="server" Width="100%" style="max-height:500px" />
                  </ContentTemplate>
                </asp:UpdatePanel>           
            </div>
            <div class="col-lg-2 col-sm-2 text-center"></div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate> 
                <div class="row pad1em marTop">
                    <div class="col-lg-12 col-sm-12 text-center">
                        <asp:LinkButton ID="lnkListaProductos" runat="server" CssClass="btn btn-primary" OnClick="lnkListaProductos_Click"><i class="icon-briefcase"></i><span>Lista de Productos</span></asp:LinkButton>
                    </div>
                </div>
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel4">
                            <ProgressTemplate>
                                <asp:Panel ID="pnlMaskLoad1" runat="server" CssClass="maskLoad">
                                </asp:Panel>
                                <asp:Panel ID="pnlCargando1" runat="server" CssClass="pnlPopUpLoad padding8px">
                                    <asp:Image ID="imgLoad1" runat="server" ImageUrl="~/img/loading.gif" Width="80%" />
                                </asp:Panel>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <telerik:RadWindow RenderMode="Lightweight" ID="modalEmisores" Title="Info" EnableShadow="true"
        Behaviors="Close" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true"
        Animation="Fade" runat="server" Modal="true" Width="790px" Height="590px" Style="z-index: 1000;">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-12 col-sm-12">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <asp:Image ID="imgRef" runat="server" Width="500px" Height="500px" />
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>
</asp:Content>
