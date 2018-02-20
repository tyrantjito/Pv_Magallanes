<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="AjusteInventario.aspx.cs" Inherits="AjusteInventario" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="ancho95 text-center centrado">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
                <i class="icon-edit"></i>
                <asp:Label runat="server" ID="lblTitulo" Text="Ajuste Inventario" CssClass="alert-success"></asp:Label>
            </div>
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
        <div class="row marTop">
            <div class="col-lg-12 col-sm-12 center">
                <div class="col-lg-4 col-sm-4 center">
                    <asp:Label ID="Label8" runat="server" Text="Tiendas:" />
                    <asp:DropDownList ID="ddlIslas" runat="server" DataSourceID="SqlDataSource2"
                        DataTextField="nombre" DataValueField="idAlmacen" CssClass="dropdown">
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select u.id_punto as idAlmacen,p.nombre_pv as nombre from usuario_puntoventa U inner join punto_venta p on p.id_punto=u.id_punto where U.usuario=@usuario and U.estatus='A'">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="usuario" QueryStringField="u" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <div class="col-lg-4 col-sm-4 center">
                    <asp:Label ID="Label1" runat="server" Text="Producto:" />
                    <asp:TextBox ID="txtFiltroArticulo" runat="server" Height="30px" placeholder="Buscar Producto" />
                    <ajaxToolkit:AutoCompleteExtender runat="server" ServiceMethod="obtieneProductos"
                        BehaviorID="txtFiltroArticulo_AutoCompleteExtender" TargetControlID="txtFiltroArticulo"
                        ID="txtFiltroArticulo_AutoCompleteExtender" Enabled="true" UseContextKey="true"
                        MinimumPrefixLength="1" CompletionInterval="10" CompletionSetCount="10" />
                </div>
                <div class="col-lg-2 col-sm-2 center">
                    <asp:LinkButton runat="server" CssClass="btn btn-info" OnClick="lnkBuscar_Click" ID="lnkBuscar"><i class="icon-search"></i><span>&nbsp;Buscar</span></asp:LinkButton>
                </div>
                <div class="col-lg-2 col-sm-2 center">
                    <asp:LinkButton runat="server" CssClass="btn btn-info" OnClick="lnkConcluirTodo_Click" ID="lnkConcluirTodo" OnClientClick="return confirm('¿Esta seguro de concluir todo? Tome en cuenta que una vez concluido sus conteos se sumaraizaran para generar la ultima existencia')"><i class="icon-check"></i><span>&nbsp;Concluir Todo</span></asp:LinkButton>
                </div>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-lg-12 col-sm-12 center">
                <div class="table-responsive">
                    
                            <div class="row">
                                <div class="col-lg-12 col-sm-12 text-center alert-danger">
                                    <asp:Label ID="lblError" runat="server" ForeColor="Red" CssClass="centrado negritas" />
                                </div>
                            </div>
                            <br />
                    <asp:GridView runat="server" ID="GridInvetarioProductos" ShowHeaderWhenEmpty="True"
                                AutoGenerateColumns="False" OnRowEditing="GridInvetarioProductos_RowEditing" AllowSorting="true"
                                EmptyDataText="El inventario no cuenta con exitencias" EmptyDataRowStyle-ForeColor="Red"
                                OnRowCancelingEdit="GridInvetarioProductos_RowCancelingEdit"
                                OnRowUpdating="GridInvetarioProductos_RowUpdating" EditRowStyle-ForeColor="#444444"
                                CssClass="table table-bordered" DataKeyNames="idAlmacen,idArticulo" OnRowDataBound="GridInvetarioProductos_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Clave" SortExpression="idArticulo">
                                        <EditItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("idArticulo") %>' ForeColor="#444444" ID="Label2"></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkIdArticulo" runat="server" OnClick="lnkIdArticulo_Click" CommandArgument='<%# Eval("idArticulo")+";"+Eval("descripcion") %>' Text='<%# Bind("idArticulo") %>'>
                                                
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Producto" SortExpression="descripcion">
                                        <EditItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("descripcion") %>' ForeColor="#444444" ID="Label100"></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("descripcion") %>' ID="lblDescripcion"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Existencia" ControlStyle-CssClass="text-right" SortExpression="cantidadExistencia">
                                        <EditItemTemplate>
                                            <asp:Label runat="server" CssClass="negritas" Text='<%# Bind("cantidadExistencia","{0:N3}") %>' ForeColor="#444444" ID="lblExistenciaN"></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" CssClass="negritas" Text='<%# Bind("cantidadExistencia","{0:N3}") %>' ID="lblExistenciaN"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Precio Venta" ControlStyle-CssClass="text-right" SortExpression="ventaUnitaria">
                                        <EditItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("ventaUnitaria","{0:C2}") %>' ForeColor="#444444" ID="lblPrecioVenta"></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("ventaUnitaria","{0:C2}") %>' ID="lblPrecioVenta"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Subtotal">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTotal"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label runat="server" ID="lblTotal"></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Inventario" SortExpression="cantidadExistencia">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtAjuste" runat="server" CssClass="input-small" Text="0"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="txtAjusteNR" Skin="MetroTouch" CssClass="input-mini" Value="0" ShowSpinButtons="true" NumberFormat-DecimalDigits="3"></telerik:RadNumericTextBox>
                                            <asp:TextBox ID="txtAjusteN" Visible="false" OnTextChanged="txtAjusteN_TextChanged" AutoPostBack="true" runat="server" CssClass="input-small" Text="0"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="Conteos" >                                       
                                        <ItemTemplate>
                                             <asp:Label ID="lblConteos" runat="server" CssClass="input-small" ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkOK" runat="server" CssClass="btn btn-info" OnClick="lnkOK_Click"><span>OK</span></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkConcluir" runat="server" CssClass="btn btn-info" OnClick="lnkConcluir_Click" CommandArgument='<%# Eval("idArticulo")+";"+Eval("descripcion")%>'><span>Concluir</span></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Costo Unitario" Visible="false" ControlStyle-CssClass="text-right" SortExpression="costoUnitario" ItemStyle-CssClass=" text-right">
                                        <EditItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("costoUnitario","{0:C2}") %>' ForeColor="#444444" ID="Label32"></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("costoUnitario","{0:C2}") %>' ID="Label4"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Existencia Final" ControlStyle-CssClass="text-right" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExistenciaFinalN" runat="server" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:Label ID="lblExistenciaFinal" runat="server" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False" Visible="false" ItemStyle-CssClass="text-center">
                                        <EditItemTemplate>
                                            <asp:Button runat="server" Text="Actualizar" CssClass="btn-success" CommandArgument='<%# Eval("idsAlmaArt") %>' CommandName="Update" CausesValidation="True" ID="Button1"></asp:Button>&nbsp;
                                                <asp:Button runat="server" Text="Cancelar" CssClass="btn-danger" CommandName="Cancel" CausesValidation="False" ID="Button2"></asp:Button>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Button runat="server" Text="Editar" CssClass="btn-warning" CommandName="Edit" CausesValidation="False" ID="Button1"></asp:Button>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle ForeColor="Red" />
                            </asp:GridView>
                            <br />
                            <br />
                            <br />

                            <asp:Panel ID="PanelMask" runat="server" Visible="false" CssClass="mask"></asp:Panel>
                            <asp:Panel ID="PanelPopDetalle" runat="server" Visible="false" CssClass="pnlPopUpEntrada" ScrollBars="Vertical">
                                <div class="col-lg-12 col-sm-12 text-center ">
                                    <div class="col-lg-10 col-sm-10 text-center padding8px negritas alert-info">
                                        <h3><asp:Label ID="lbltituPop" runat="server" CssClass="negritas"></asp:Label></h3>
                                    </div>
                                    <div class="col-lg-2 col-sm-2 text-right padding8px alert-info">
                                        <asp:LinkButton ID="lnkClose" runat="server" CssClass="btn btn-danger" OnClick="lnkClose_Click"><i class="icon-remove"></i></asp:LinkButton>
                                    </div>
                                    <div class="col-lg-8 col-sm-8 text-left">
                                        <h4><asp:Label ID="lbl1" runat="server" Text="Ajuste" /></h4>
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-right ">
                                        <h4><asp:Label ID="lblAjusteDet" runat="server" /></h4>
                                    </div>
                                    <div class="col-lg-8 col-sm-8 text-left">
                                        <h4><asp:Label ID="lbl2" runat="server" Text="Entrada" /></h4>
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-right ">
                                        <h4><asp:Label ID="lblEntradaDet" runat="server" /></h4>
                                    </div>
                                    <div class="col-lg-8 col-sm-8 text-left">
                                        <h4><asp:Label ID="lbl3" runat="server" Text="Venta" /></h4>
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-right ">
                                        <h4><asp:Label ID="lblVentaDet" runat="server" /></h4>
                                    </div>
                                    
                                    <div class="col-lg-8 col-sm-8 text-left">
                                        <h2><asp:Label ID="lbl4" runat="server" Text="Existencia Actual" /></h2>
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-right ">
                                        <h2><asp:Label ID="lblExistenciaDet" runat="server" /></h2>
                                    </div>
                                </div>
                            </asp:Panel>

                             
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
    </div>
</asp:Content>

