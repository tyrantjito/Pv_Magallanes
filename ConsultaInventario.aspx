<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="ConsultaInventario.aspx.cs" Inherits="ConsultaInventario" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="ancho95 text-center centrado">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
               <i class="icon-search"></i> <asp:Label runat="server" ID="lblTitulo" Text="Consulta Inventario" CssClass="alert-success"></asp:Label>
            </div>
        </div>
        <div class="row marTop">
            <div class="col-lg-12 col-sm-12 center">            
                <div class="col-lg-5 col-sm-5 center">
                    <asp:Label ID="Label8" runat="server" Text="Tiendas:" />
                    <asp:DropDownList ID="ddlIslas" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource2"
                        DataTextField="nombre" DataValueField="idAlmacen" OnSelectedIndexChanged="ddlIslas_SelectedIndexChanged"
                        CssClass="dropdown">
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select u.id_punto as idAlmacen,p.nombre_pv as nombre from usuario_puntoventa U inner join punto_venta p on p.id_punto=u.id_punto where U.usuario=@usuario and U.estatus='A'">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="usuario" QueryStringField="u" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <div class="col-lg-5 col-sm-5 center">
                    <asp:Label ID="Label1" runat="server" Text="Producto:" />
                    <asp:TextBox ID="txtFiltroArticulo" runat="server" Height="30px" OnTextChanged="txtFiltroArticulo_TextChanged" AutoPostBack="true" placeholder="Buscar Producto" />
                    <ajaxToolkit:AutoCompleteExtender runat="server" ServiceMethod="obtieneProductos" 
                        BehaviorID="txtFiltroArticulo_AutoCompleteExtender" TargetControlID="txtFiltroArticulo" 
                        ID="txtFiltroArticulo_AutoCompleteExtender" Enabled="true" UseContextKey="true" 
                        MinimumPrefixLength="1" CompletionInterval="10" CompletionSetCount="10"/>
                </div>
                <div class="col-lg-2 col-sm-2 center">
                    <asp:LinkButton runat="server" CssClass="btn btn-info" OnClick="lnkImprime_Click" ID="lnkImprime"><i class="icon-pint"></i><span>Imprimir</span></asp:LinkButton>
                </div>
            </div>
        </div>
        <br />        
            <div class="row">
                <div class="col-lg-12 col-sm-12 center">
                    <div class="table-responsive">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:GridView runat="server" ID="GridInvetarioProductos" ShowHeaderWhenEmpty="True"
                                    AutoGenerateColumns="False" OnRowEditing="GridInvetarioProductos_RowEditing" AllowSorting="true"
                                    EmptyDataText="El inventario no cuenta con exitencias" EmptyDataRowStyle-ForeColor="Red" AllowPaging="true" PageSize="9"
                                    OnRowCancelingEdit="GridInvetarioProductos_RowCancelingEdit" OnPageIndexChanging="GridInvetarioProductos_PageIndexChanging"
                                    OnRowUpdating="GridInvetarioProductos_RowUpdating" EditRowStyle-ForeColor="#444444" ShowFooter="true"
                                    CssClass="table table-bordered" DataKeyNames="idAlmacen,idArticulo" OnRowDataBound="GridInvetarioProductos_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Clave" SortExpression="idArticulo" >
                                            <EditItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("idArticulo") %>' ForeColor="#444444" ID="Label2"></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("idArticulo") %>' ID="Label21"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Producto" SortExpression="descripcion" >
                                            <EditItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("descripcion") %>' ForeColor="#444444" ID="Label100"></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("descripcion") %>' ID="Label101"></asp:Label>
                                            </ItemTemplate>                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Existencia" SortExpression="cantidadExistencia">
                                            <EditItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("cantidadExistencia","{0:N3}") %>' ForeColor="#444444" ID="Label3"></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("cantidadExistencia","{0:N3}") %>' ID="Label31"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotArti"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Costo Unitario" SortExpression="costoUnitario" ItemStyle-CssClass=" text-right">
                                            <EditItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("costoUnitario","{0:C2}") %>' ForeColor="#444444" ID="Label32"></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("costoUnitario","{0:C2}") %>' ID="Label4"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stock Min" SortExpression="cantidadMinima" >
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" Height="30px" CssClass="ancho100px" Text='<%# Eval("cantidadMinima","{0:N3}") %>' ID="txtMin"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("cantidadMinima","{0:N3}") %>' ID="Label5"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stock Max" SortExpression="cantidadMaxima" >
                                            <EditItemTemplate>
                                                <asp:TextBox runat="server" Height="30px" CssClass="ancho100px" Text='<%# Eval("cantidadMaxima","{0:N3}") %>' ID="txtMax"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("cantidadMaxima","{0:N3}") %>' ID="Label6"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Precio Venta" SortExpression="ventaUnitaria" >
                                            <EditItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("ventaUnitaria","{0:C2}") %>' ForeColor="#444444" ID="Label64"></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("ventaUnitaria","{0:C2}") %>' ID="Label7"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Costo Unitario" SortExpression="totalCostoUnitario" >
                                            <EditItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("totalCostoUnitario","{0:C2}") %>' ForeColor="#444444" ID="Label61"></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("totalCostoUnitario","{0:C2}") %>' ID="Label71"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotCu"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Precio Venta" SortExpression="totalVenta" >
                                            <EditItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("totalVenta","{0:C2}") %>' ForeColor="#444444" ID="Label62"></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("totalVenta","{0:C2}") %>' ID="Label72"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotVp"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Utilidad" SortExpression="Utilidad" >
                                            <EditItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Utilidad","{0:C2}") %>' ForeColor="#444444" ID="Label63"></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Bind("Utilidad","{0:C2}") %>' ID="Label73"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotUtil"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False" ItemStyle-CssClass="text-center">
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
                                    <FooterStyle CssClass="alert-warning" />
                                </asp:GridView>
                                <br />
                                <div class="row">
                                    <div class="col-lg-12 col-sm-12 text-center alert-danger">
                                        <asp:Label ID="lblError" runat="server" ForeColor="Red" CssClass="centrado" />
                                    </div>
                                </div>                                
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        
                    </div>
                </div>
            </div>        
    </div>
</asp:Content>

