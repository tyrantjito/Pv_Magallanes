<%@ Page Title="Entradas de Almacen" Culture="es-MX" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="EntradasAlmacen.aspx.cs" Inherits="pvAccom.EntradasAlmacen" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
            function onEntryAdded(sender, eventArgs) {
                var lblProdEdit = document.getElementById( $find("<%= grdDetProductos.ClientID %>");
                alert("VAl-" + eventArgs.get_entry().get_text());
                lblProdEdit.value = eventArgs.get_entry().get_text();
            }
        </script>
        <script type="text/javascript">
            function entryAdded(sender, eventArgs) {
                var cboProd = sender;
                alert(cboProd.get_text());
                //alert("An item with Text='" + eventArgs.get_entry().get_text() + "' has just been selected.");
        }
    </script>
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="ancho95 centrado">
                <div class="row">
                    <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
                        <i class="icon-inbox"></i>
                        <asp:Label runat="server" ID="lblTitulo" Text="Entrada" CssClass="alert-success"></asp:Label>
                    </div>
                </div>
                <div class="row">                    
                    <div class="col-lg-12 col-sm-12 center">
                        <asp:Label ID="lblError" runat="server" CssClass="text-alert"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-sm-12 center">
                        <div class="row">
                    <div class="col-lg-12 col-sm-12 text-right">
                        <asp:Button runat="server" ID="btnAgrEnt" CssClass="btn-info" Text="Nueva Entrada" OnClick="btnAgrEnt_OnClick" />
                    </div>
                    <div class="col-lg-2 col-sm-2">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" EnableAJAX="true">
                            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="WebBlue" OnSelectedIndexChanged="RadGrid1_SelectedIndexChanged"
                                EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="SqlDataSource1" AllowSorting="true" GroupingEnabled="false" PageSize="100">
                                <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="False" DataKeyNames="id_punto">
                                    <Columns>                               
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="nombre_pv" FilterControlAltText="Filtro Tienda" HeaderText="Tienda" SortExpression="nombre_pv" UniqueName="nombre_pv" Resizable="true"/>
                                    </Columns>
                                    <NoRecordsTemplate>
                                        <asp:Label runat="server" ID="lblVacio" Text="No se han encontrado Tiendas" CssClass="text-danger"></asp:Label>
                                    </NoRecordsTemplate>
                                </MasterTableView>
                                <ClientSettings EnablePostBackOnRowClick="true">
                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>
                                    <Selecting AllowRowSelect="true" /> 
                            </ClientSettings>                        
                            <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select u.id_punto,p.nombre_pv from usuario_puntoventa U inner join punto_venta p on p.id_punto=u.id_punto where U.usuario=@usuario and U.estatus='A'">
                            <SelectParameters>
                                <asp:QueryStringParameter QueryStringField="u"  Name="usuario"></asp:QueryStringParameter>
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <div class="col-lg-10 col-sm-10">
                        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" EnableAJAX="true">
                            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid2" AllowFilteringByColumn="true" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="WebBlue"  OnItemDataBound="RadGrid2_ItemDataBound"
                                EnableHeaderContextFilterMenu="true" AllowPaging="True" PagerStyle-AlwaysVisible="true" DataSourceID="sqlDsrEntInventario" AllowSorting="true" GroupingEnabled="false" PageSize="100">
                                <MasterTableView DataSourceID="sqlDsrEntInventario" AutoGenerateColumns="False" DataKeyNames="entFolioID">
                                    <Columns>                               
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="entFolioID" HeaderStyle-Width="150px" FilterControlWidth="100px" FilterControlAltText="Filtro Entrada" HeaderText="No." SortExpression="entFolioID" UniqueName="entFolioID" Resizable="true"/>
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="entDocumento" HeaderStyle-Width="200px" FilterControlWidth="150px" FilterControlAltText="Filtro Documento" HeaderText="Documento" SortExpression="entDocumento" UniqueName="entDocumento" Resizable="true"/>
                                        <telerik:GridBoundColumn FilterCheckListEnableLoadOnDemand="true" DataField="entFechaDoc" HeaderStyle-Width="100px" FilterControlWidth="80px" FilterControlAltText="Filtro Fecha" HeaderText="Fecha" SortExpression="entFechaDoc" UniqueName="entFechaDoc" Resizable="true" DataFormatString="{0:d}"/>
                                        <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="entSumaProductos" HeaderStyle-Width="100px" FilterControlWidth="80px" FilterControlAltText="Filtro Suma" HeaderText="Productos" SortExpression="entSumaProductos" UniqueName="entSumaProductos" Resizable="true"/>
                                        <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="entSubtotal" HeaderStyle-Width="100px" FilterControlWidth="80px" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Compra" HeaderText="Total Compra" SortExpression="entSubtotal" UniqueName="entSubtotal" Resizable="true" DataFormatString="{0:C2}"/>
                                        <telerik:GridNumericColumn FilterCheckListEnableLoadOnDemand="true" DataField="compra" HeaderStyle-Width="100px" FilterControlWidth="80px" ItemStyle-CssClass="text-right" FilterControlAltText="Filtro Venta" HeaderText="Total Venta" SortExpression="compra" UniqueName="compra" Resizable="true" DataFormatString="{0:C2}"/>
                                        <telerik:GridCheckBoxColumn FilterCheckListEnableLoadOnDemand="true" DataField="terminado" HeaderStyle-Width="80px" FilterControlWidth="50px" ItemStyle-CssClass="text-center" FilterControlAltText="Filtro Terminado" HeaderText="Terminado" SortExpression="terminado" UniqueName="terminado" Resizable="true" />
                                        <telerik:GridTemplateColumn HeaderText="Editar" HeaderStyle-Width="50px" FilterControlWidth="0px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEntrada" runat="server" CommandArgument='<%# Bind("entFolioID") %>' OnClick="lnkEntrada_Click" CssClass="btn btn-warning"><i class="icon icon-edit"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn  HeaderText="Terminar" HeaderStyle-Width="50px" FilterControlWidth="0px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkTerminar" runat="server" CommandArgument='<%# Bind("entFolioID") %>' OnClick="lnkTerminar_Click" OnClientClick="return confirm('¿Está seguro de dar como terminada la entrada?. Una vez realizado el proceso se afectara la existencia de los articulos indicados en la entrada')" CssClass="btn btn-primary"><i class="icon icon-check"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn  HeaderText="Eliminar" HeaderStyle-Width="50px" FilterControlWidth="0px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEliminarEntrada" runat="server" CommandArgument='<%# Bind("entFolioID") %>' OnClick="lnkEliminarEntrada_Click" CssClass="btn btn-danger" OnClientClick="return confirm('¿Está seguro de eliminar la entrada?')"><i class="icon icon-trash"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn> 
                                        <telerik:GridTemplateColumn HeaderText="Seleccionar" HeaderStyle-Width="50px" FilterControlWidth="0px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSeleccionar" runat="server" CommandArgument='<%# Bind("entFolioID") %>' OnClick="lnkSeleccionar_Click" CssClass="btn btn-success"><i class="icon icon-plus"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <NoRecordsTemplate>
                                        <asp:Label runat="server" ID="lblVacio" Text="No se han encontrado Entradas registradas" CssClass="text-danger"></asp:Label>
                                    </NoRecordsTemplate>
                                </MasterTableView>
                                <ClientSettings>
                                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="1"></Scrolling>                                     
                            </ClientSettings>                        
                            <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                            </telerik:RadGrid>
                        </telerik:RadAjaxPanel>
                        <asp:SqlDataSource ID="sqlDsrEntInventario" runat="server"
                            ConnectionString="<%$ ConnectionStrings:PVW %>"
                            InsertCommand="INSERT INTO [entinventarioenc] ([documento], [entAlmacenID], [claveProveedor], [fechaDocumento], [subtotal], [impuesto], [total], [entSumaProductos], [descuentoProveedor]) VALUES (@documento, @entAlmacenID, @claveProveedor, @fechaDocumento, @subtotal, @impuesto, @total, @descuentoProveedor, @sumaProductos)"
                            SelectCommand="SELECT e.entFolioID, e.entAlmacenID,c.nombre, e.entDocumento, Convert(char(10),e.entFechaDoc,126) as entFechaDoc, e.claveProveedor, e.entSubtotal, e.entImpuesto, e.entTotal, e.descuentoProveedor, e.entSumaProductos, cast((select isnull((select sum(p.ventaUnitaria*d.entprodcant) from entinventariodet d 
        left join articulosalmacen aa on aa.idalmacen=d.entalmacenid and aa.idarticulo=d.entproductoid
        left join precios_venta p on p.idalmacen=aa.idalmacen and p.idproducto=aa.idarticulo and p.idpreciopublico=aa.idpreciopublico
        where d.entalmacenid=e.entalmacenid and d.entfolioid=e.entfolioid),0)) as decimal(15,2)) as compra,e.terminado FROM entinventarioenc e left join catAlmacenes c on c.idAlmacen=e.entAlmacenID where e.entAlmacenID=@tienda order by entFolioID desc"
                            UpdateCommand="UPDATE [entinventarioenc] SET [documento] = @documento, [entAlmacenID] = @entAlmacenID, [claveProveedor] = @claveProveedor, [fechaDocumento] = @fechaDocumento, [subtotal] = @subtotal, [impuesto] = @impuesto, [total] = @total, [descuentoProveedor] = @descuentoProveedor, [sumaProductos] = @sumaProductos WHERE [entFolioID] = @entFolioID">
                            <InsertParameters>
                                <asp:Parameter Name="documento" Type="String" />
                                <asp:Parameter Name="entAlmacenID" Type="Int16" />
                                <asp:Parameter Name="claveProveedor" Type="Int32" />
                                <asp:Parameter DbType="Date" Name="fechaDocumento" />
                                <asp:Parameter Name="subtotal" Type="Decimal" />
                                <asp:Parameter Name="impuesto" Type="Decimal" />
                                <asp:Parameter Name="total" Type="Decimal" />
                                <asp:Parameter Name="descuentoProveedor" Type="Decimal" />
                                <asp:Parameter Name="sumaProductos" Type="Decimal" />
                            </InsertParameters>
                            <UpdateParameters>
                                <asp:Parameter Name="documento" Type="String" />
                                <asp:Parameter Name="entAlmacenID" Type="Int16" />
                                <asp:Parameter Name="claveProveedor" Type="Int32" />
                                <asp:Parameter DbType="Date" Name="fechaDocumento" />
                                <asp:Parameter Name="subtotal" Type="Decimal" />
                                <asp:Parameter Name="impuesto" Type="Decimal" />
                                <asp:Parameter Name="total" Type="Decimal" />
                                <asp:Parameter Name="descuentoProveedor" Type="Decimal" />
                                <asp:Parameter Name="sumaProductos" Type="Decimal" />
                                <asp:Parameter Name="entFolioID" Type="Int32" />
                            </UpdateParameters>
                            <SelectParameters>
                                <asp:ControlParameter Name="tienda" ControlID="RadGrid1" DefaultValue="0" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>                    
                </div>
                    </div>
                </div>
                <asp:Label runat="server" ID="lblFolioSelect" Visible="false" />                
            </div>
            <br />
            <div class="ancho95 centrado">
                <div class="row">                    
                    <div class="col-lg-12 col-sm-12 center">
                        <asp:LinkButton ID="lnkImprime" runat="server" OnClick="lnkImprime_Click" CssClass="btn btn-info"><i class="icon-print"></i>&nbsp;<span>Imprimir</span></asp:LinkButton>
                    </div>
                </div>                
            </div>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <asp:Panel ID="pnlMaskLoad" runat="server" CssClass="maskLoad"></asp:Panel>
                                <asp:Panel ID="pnlCargando" runat="server" CssClass="pnlPopUpLoad padding8px" >
                                    <asp:Image ID="imgLoad" runat="server" ImageUrl="~/img/loading.gif" Width="80%" />
                                </asp:Panel>
                            </ProgressTemplate>
                            
                        </asp:UpdateProgress>
             </ContentTemplate>
                </asp:UpdatePanel>
         <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>

            <asp:Panel ID="pnlMask" runat="server" Visible="false" CssClass="mask"></asp:Panel>
            <asp:Panel ID="pnlPopupNvoDoc" runat="server" Visible="false" CssClass="pnlPopUpEntrada padding8px" GroupingText="Crea Documento" ScrollBars="Vertical">
                <div class="alert-success text-center negritas">
                    <span>Encabezado de Documento</span>
                </div>
                <asp:Panel ID="Panel1" runat="server" CssClass="ancho95 centrado">

                    <%-- <asp:UpdatePanel runat="server" ID="updNvoDoc">
                    <ContentTemplate>--%>
                    <div class="row">
                        <asp:Label ID="lblEntrada" runat="server" Visible="false"></asp:Label>
                        <div class="col-lg-2 col-sm-2 text-left">
                            <asp:Label ID="Label10" runat="server" Text="Proveedor:"></asp:Label></div>
                        <div class="col-lg-4 col-sm-8 text-left">
                            <asp:DropDownList ID="ddlProve" runat="server" DataTextField="razonSocial" DataValueField="clave" AppendDataBoundItems="True" ValidationGroup="valEncDoc" DataSourceID="sqlDsrProveedor">
                                <asp:ListItem Value="-1">--Seleccionar Proveedor--</asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="sqlDsrProveedor" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="SELECT [clave], [razonSocial] FROM [clienteproveedor]"></asp:SqlDataSource>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-2 col-sm-2 text-left">
                            <asp:Label ID="Label13" runat="server" Text="Documento:"></asp:Label></div>
                        <div class="col-lg-4 col-sm-10 text-left">
                            <asp:TextBox ID="txtDocu" runat="server" MaxLength="50" CssClass="input-medium" CausesValidation="true" ValidationGroup="valEncDoc" placeholder="Documento" /></td>
                        </div>
                        <div class="col-lg-2 col-sm-2 text-left">
                            <asp:Label ID="Label14" runat="server" Text="Tipo Docto.:"></asp:Label></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:DropDownList ID="ddlTipoDoc" runat="server" CausesValidation="false" ValidationGroup="valEncDoc">
                                <asp:ListItem Text="Factura" Value="Fact"></asp:ListItem>
                                <asp:ListItem Text="Remisión" Value="Rem"></asp:ListItem>
                                <asp:ListItem Text="Otro" Value="Otro"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-2 col-sm-2 text-left">
                            <asp:Label ID="Label15" runat="server" Text="Fecha Docto.:"></asp:Label></div>
                        <div class="col-lg-4 col-sm-6 text-left">
                            <asp:TextBox runat="server" ID="txtFechaDoc" MaxLength="10" CssClass="input-small" ValidationGroup="valEncDoc" CausesValidation="true" OnTextChanged="txtFechaDoc_TextChanged" AutoPostBack="true" placeholder="aaaa-mm-dd" Enabled="false" />
                            <cc1:CalendarExtender ID="calFechaDoc" runat="server" Format="yyyy-MM-dd" PopupButtonID="lnkFFin" TargetControlID="txtFechaDoc" TodaysDateFormat="MMMM, yyyy" />
                            <asp:LinkButton ID="lnkFFin" runat="server" CssClass="btn btn-info"><i class="icon-calendar"></i></asp:LinkButton>
                        </div>
                    </div>
                    <div class="row alert-danger text-center">
                        <asp:ValidationSummary runat="server" ID="vlsEncDoc" ValidationGroup="valEncDoc" DisplayMode="List" EnableClientScript="true" ShowMessageBox="true" ShowSummary="true" />
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-4 col-sm-6">
                            <telerik:RadSkinManager ID="RadSkinManager1" runat="server"/>
                            <telerik:RadAutoCompleteBox Runat="server" RenderMode="Lightweight" ID="RadAutoCompleteBox" EmptyMessage="Producto" MaxResultCount="5" OnTextChanged="RadAutoCompleteBox1_TextChanged"
                DataSourceID="SqlDsProducto" DataTextField="descripcion" EnableAriaSupport="true" AccessKey="W" InputType="Text" Width="200px" DropDownWidth="200px" DropDownHeight="200px">
                            </telerik:RadAutoCompleteBox>
                        </div>                        
                        <div class="col-lg-2 col-sm-3">
                            <asp:TextBox ID="txtCant" placeholder="Cantidad" runat="server" CssClass="input-small" AutoPostBack="true" OnTextChanged="txtCant_TextChanged" Width="68px" />
                            <cc1:FilteredTextBoxExtender ID="txtCant_FilteredTextBoxExtender" runat="server" BehaviorID="txtCant_FilteredTextBoxExtender" TargetControlID="txtCant" FilterType="Numbers,Custom" ValidChars="." />
                            <asp:RegularExpressionValidator ControlToValidate="txtCant" CssClass="errores" Text="*" ID="RegularExpressionValidator2" runat="server" ValidationGroup="agregaProducto" ErrorMessage="La Cantidad es incorrecta" ValidationExpression="^(\$|)([1-9]\d{0,2}(\,\d{3})*|([1-9]\d*))(\.\d{3})?$" />
                        </div>
                        <div class="col-lg-2 col-sm-3">
                            <asp:TextBox ID="txtCosto" placeholder="C. Unitario" runat="server" CssClass="input-small" OnTextChanged="txtCosto_TextChanged" AutoPostBack="true" />
                            <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtCosto_FilteredTextBoxExtender" FilterType="Numbers,Custom" TargetControlID="txtCosto" ID="txtCosto_FilteredTextBoxExtender" ValidChars=".,"></cc1:FilteredTextBoxExtender>
                            <asp:RegularExpressionValidator ControlToValidate="txtCosto" CssClass="errores" Text="*" ID="RegularExpressionValidator1" runat="server" ValidationGroup="agregaProducto" ErrorMessage="El monto es incorrecto" ValidationExpression="^(\$|)([1-9]\d{0,2}(\,\d{3})*|([1-9]\d*))(\.\d{2})?$" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtCosto" Text="*" runat="server" ErrorMessage="Necesita colocar un monto" CssClass="errores" ValidationGroup="agregaProducto" />
                        </div>
                        <div class="col-lg-2 col-sm-3">
                            <asp:TextBox ID="txtPrVta" placeholder="P. Venta" runat="server" CssClass="input-small" />
                            <cc1:FilteredTextBoxExtender runat="server" BehaviorID="txtPrVta_FilteredTextBoxExtender" FilterType="Numbers,Custom" TargetControlID="txtCosto" ID="txtPrVta_FilteredTextBoxExtender" ValidChars=".,"></cc1:FilteredTextBoxExtender>
                            <asp:RegularExpressionValidator ControlToValidate="txtPrVta" CssClass="errores" Text="*" ID="rextxtPrVta" runat="server" ValidationGroup="agregaProducto" ErrorMessage="El precio de venta es incorrecto" ValidationExpression="^(\$|)([1-9]\d{0,2}(\,\d{3})*|([1-9]\d*))(\.\d{2})?$" />
                            <asp:RequiredFieldValidator ID="rfvtxtPrVta" ControlToValidate="txtPrVta" Text="*" runat="server" ErrorMessage="Necesita colocar un monto" CssClass="errores" ValidationGroup="agregaProducto" />
                        </div>
                        <div class="col-lg-2 col-sm-4">
                            <asp:TextBox ID="txtImporte" runat="server" CssClass="input-small" Enabled="false" placeholder="Importe"></asp:TextBox></div>
                        <div class="col-lg-1 col-sm-1">
                            <asp:Button ID="btnAgrProd" runat="server" CssClass="btn-info" Text="Agregar" OnClick="btnAgrProd_Click" /></div>
                    </div>
                    <div class="row alert-danger text-center">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errores" ValidationGroup="agregaProducto" />
                        <asp:Label ID="lblErrorDoc" runat="server" CssClass="errores" />
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-lg-12 col-sm-12 center">
                            <div class="table-responsive">
                                <asp:GridView runat="server" ID="grdDetProductos" AutoGenerateColumns="False" DataSourceID="" AllowPaging="true" PageSize="5"
                                    EmptyDataText="Ingrese los datos del producto y presione ''Agregar''" DataKeyNames="entID"
                                    ShowFooter="false" ShowHeaderWhenEmpty="True" OnRowCommand="grdDetProductos_RowCommand"
                                    OnRowEditing="grdDetProductos_RowEditing" OnRowUpdating="grdDetProductos_RowUpdating" OnPageIndexChanging="grdDetProductos_PageIndexChanging"
                                    OnRowDeleting="grdDetProductos_RowDeleting" OnRowCancelingEdit="grdDetProductos_RowCancelingEdit"
                                    SelectedRowStyle-BackColor="DarkSlateGray" CssClass="table table-bordered">
                                    <Columns>                                       
                                        <asp:TemplateField HeaderText="No.">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("entID") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("entID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cantidad" SortExpression="entCant">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox3" runat="server" placeholder="Cantidad" CssClass="input-small" Text='<%# Bind("entCant") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Text='<%# Bind("entCant") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Producto" SortExpression="entProducto">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label22" runat="server" Text='<%# Bind("entProducto") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <InsertItemTemplate>
                                                <asp:TextBox ID="txtProducto1" placeholder="Producto" Height="30px" runat="server"></asp:TextBox>
                                            </InsertItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("entProducto") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Descripción">
                                            <EditItemTemplate>                                                                                                
                                                <asp:Label runat="server" ID="lblProdEdit" Text='<%# Bind("entProdDesc") %>' />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label3" runat="server" Text='<%# Bind("entProdDesc") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Costo Unitario" SortExpression="entCosto">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox4" runat="server" CssClass="input-small"
                                                    Text='<%# Bind("entCosto") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Text='<%# Bind("entCosto","{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Precio Venta" SortExpression="entCosto">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtPrecVtaEdit" runat="server" CssClass="input-small"
                                                    Text='<%# Bind("entPrecVtaUnit") %>' Enabled="false"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblEntPreVta" runat="server" Text='<%# Bind("entPrecVtaUnit","{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Importe" SortExpression="entImporte">
                                            <EditItemTemplate>
                                                <asp:Label ID="Label63" runat="server" Text='<%# Bind("entImporte","{0:C2}") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Text='<%# Bind("entImporte","{0:C2}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <EditItemTemplate>
                                                <asp:Button ID="ImageButton41" runat="server" CausesValidation="True" CssClass="btn-success"
                                                    CommandName="Update" ToolTip="Guardar" Text="Guardar" />&nbsp;
                                                <asp:Button ID="ImageButton42" runat="server" CausesValidation="False" CssClass="btn-danger"
                                                    CommandName="Cancel" ToolTip="Cancelar" Text="Cancelar" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Button ID="ImageButton43" runat="server" CausesValidation="False" CssClass="btn-warning"
                                                    CommandName="Edit" ImageUrl="~/Img/edit.png" Text="Editar" />&nbsp;
                                                <asp:Button ID="ImageButton45" runat="server" CausesValidation="False" CssClass="btn-danger"
                                                    CommandName="Delete" ImageUrl="~/Img/eliminar.png" Text="Borrar" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle ForeColor="Red" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div style="margin-top: 6px;">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 70%; text-align: right; padding-right: 20px;">
                                    <asp:Label runat="server" ID="lblSumaProds" Visible="false"></asp:Label></td>
                                <td style="width: 18%;"></td>
                                <td style="text-align: right;">
                                    <asp:Label runat="server" ID="lblSubT" Visible="false"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;<asp:Label runat="server" ID="lblIva" Visible="false"></asp:Label></td>
                                <td style="text-align: right;">
                                    <asp:Label runat="server" ID="lblTotIva" Visible="false"></asp:Label></td>
                            </tr>
                            <tr class="negritas">
                                <td>&nbsp;</td>
                                <td>Total Compra:</td>
                                <td style="text-align: right;">
                                    <asp:Label runat="server" ID="lblTotal"></asp:Label></td>
                            </tr>
                            <tr class="negritas">
                                <td>&nbsp;</td>
                                <td>Total Venta:</td>
                                <td style="text-align: right;">
                                    <asp:Label runat="server" ID="lblTotalVenta"></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                    <div class="row">
                        <div class="col-lg-2 col-sm-2 center">
                            <asp:Button runat="server" ID="btnOrdenes" CssClass="btn-primary" Text="Seleccione Orden" OnClick="btnOrdenes_Click" />
                        </div>
                        <div class="col-lg-10 col-sm-10 center">
                            <asp:Button runat="server" ID="btnGuardaEnt" CssClass="btn-success" Text="Guardar Documento" OnClick="btnGuardaEnt_Click" Enabled="false" />&nbsp;&nbsp;
                            <asp:Button runat="server" ID="btnCancelar" CssClass="btn-danger" Text="Cancelar" OnClick="btnCancelar_Click" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-sm-12 center">
                            <asp:Label ID="lblErrorPop" runat="server" CssClass="errores alert-danger" />
                        </div>
                    </div>
                    <asp:SqlDataSource runat="server" ID="SqlDsProducto" ConnectionString="<%$ ConnectionStrings:PVW %>" ProviderName="System.Data.SqlClient" 
                        SelectCommand="select idProducto+' / '+descripcion as descripcion from catproductos">
                    </asp:SqlDataSource>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnlOrdenesCompra" runat="server" Visible="false" CssClass="pnlPopUpEntrada padding8px" ScrollBars="Vertical">
                <div class="row">
                    <div class="col-lg-12 col-sm-12 center">
                        <div class="table-responsive">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:GridView runat="server" ID="GridOrdenes" ShowHeaderWhenEmpty="True"
                                        AutoGenerateColumns="False" DataKeyNames="no_orden" AllowSorting="True"
                                        EmptyDataText="No existen Ordenes de Compra registradas" 
                                        EmptyDataRowStyle-ForeColor="Red" AllowPaging="True" PageSize="5"
                                        CssClass="table table-bordered" 
                                        DataSourceID="SqlDataSource10"
                                        onpageindexchanging="GridOrdenes_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="no_orden" HeaderText="No. Orden" SortExpression="no_orden" />
                                            <asp:BoundField DataField="obsevaciones" HeaderText="Nota" SortExpression="obsevaciones" />
                                            <asp:BoundField DataField="usuario" HeaderText="Usuario" SortExpression="usuario" />
                                            <asp:BoundField DataField="fecha" HeaderText="Fecha" ReadOnly="True" SortExpression="fecha" />
                                            <asp:BoundField DataField="hora" HeaderText="Hora" ReadOnly="True" SortExpression="hora" />
                                            <asp:BoundField DataField="estatus" HeaderText="estatus" SortExpression="estatus" Visible="false" />
                                            <asp:BoundField DataField="estatusOrden" HeaderText="Estatus" ReadOnly="True" SortExpression="estatusOrden" />
                                            <asp:CommandField ButtonType="Button" ShowSelectButton="True" ControlStyle-CssClass="btn-success" SelectText="Seleccionar" >
                                            <ControlStyle CssClass="btn-success" />
                                            </asp:CommandField> 
                                        </Columns>                                
                                        <EmptyDataRowStyle ForeColor="Red" />
                                        <SelectedRowStyle CssClass="alert-success-org" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource10" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                        SelectCommand="select no_orden,obsevaciones,usuario,convert(char(10),fecha,126) as fecha,convert(char(10),hora,108) as hora,estatus,case estatus when 'A' then 'Pendiente' when 'E' then 'Enviada' when 'V' then 'Procesando' else '' end as estatusOrden from orden_compra_encabezado_PV where idAlmacen=@idAlmacen and estatus='E' order by fecha desc, hora desc">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="RadGrid1" Name="idAlmacen" PropertyName="SelectedValue" />                                    
                                        </SelectParameters>
                                    </asp:SqlDataSource>                                                      
                                </ContentTemplate>
                            </asp:UpdatePanel>                        
                        </div>
                    </div>
                </div>        
                <br />
                <div class="row">
                    <div class="col-lg-12 col-sm-12 center">
                        <div class="table-responsive">
                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                    <asp:GridView runat="server" ID="GrdDetalleOrden" ShowHeaderWhenEmpty="True"
                                        AutoGenerateColumns="False" AllowSorting="True"
                                        EmptyDataText="Seleccione una orden de compra para ver su detalle" 
                                        EmptyDataRowStyle-ForeColor="Red" 
                                        CssClass="table table-bordered" 
                                        DataSourceID="SqlDataSource11">
                                        <Columns>
                                            <asp:BoundField DataField="no_orden" Visible="false" SortExpression="no_orden" />
                                            <asp:BoundField DataField="idAlmacen" Visible="false" SortExpression="idAlmacen" />
                                            <asp:BoundField DataField="no_detalle" Visible="false" SortExpression="no_detalle" />
                                            <asp:BoundField DataField="idArticulo" HeaderText="Codigo" SortExpression="idArticulo" />
                                            <asp:BoundField DataField="descripcion" HeaderText="Articulo" SortExpression="descripcion" />
                                            <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" />
                                            <asp:BoundField DataField="descripcion_categoria" HeaderText="Categoria" SortExpression="descripcion_categoria" />
                                        </Columns>
                                        <EmptyDataRowStyle ForeColor="Red" />
                                        <SelectedRowStyle CssClass="alert-success-org" />
                                    </asp:GridView>
                                    <asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                        SelectCommand="select no_orden,idAlmacen,no_detalle,idArticulo,descripcion,cantidad,descripcion_categoria from orden_compra_detalle_PV where no_orden=@no_orden and idAlmacen=@idAlmacen order by descripcion_categoria asc">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="GridOrdenes" Name="no_orden" PropertyName="SelectedValue" />
                                            <asp:ControlParameter ControlID="RadGrid1" Name="idAlmacen" PropertyName="SelectedValue" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>                                                            
                                </ContentTemplate>
                            </asp:UpdatePanel>                        
                        </div>
                    </div>
                </div>
                 <div class="row">
                    <div class="col-lg-6 col-sm-6 text-right">
                        <asp:Button runat="server" ID="btnAceptarOrden" CssClass="btn-success" Text="Aceptar" OnClick="btnAceptarOrden_Click" />
                    </div>
                    <div class="col-lg-6 col-sm-6 text-left">                        
                        <asp:Button runat="server" ID="btnCancelarOrden" CssClass="btn-danger" Text="Cancelar" OnClick="btnCancelarOrden_Click" />
                    </div>
                </div>
            </asp:Panel>
            <br /><br />

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="grdDetProductos" EventName="RowCommand" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
