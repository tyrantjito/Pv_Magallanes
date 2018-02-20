<%@ Page Title="" Language="C#" MasterPageFile="~/PuntoVenta.master" AutoEventWireup="true" CodeFile="ConsultaInventarioPV.aspx.cs" Inherits="ConsultaInventarioPV" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" CssClass="venta">
        <div class="row marTop">
            <div class="col-lg-12 col-sm-12 center">
                <div class="col-lg-5 col-sm-5 center">
                    <asp:Label ID="Label1" runat="server" Text="Producto:" />
                    <asp:TextBox ID="txtFiltroArticulo" runat="server" Height="30px" OnTextChanged="txtFiltroArticulo_TextChanged" AutoPostBack="true" placeholder="Buscar Producto" />
                    <ajaxToolkit:AutoCompleteExtender runat="server" ServiceMethod="obtieneProductos"
                        BehaviorID="txtFiltroArticulo_AutoCompleteExtender" TargetControlID="txtFiltroArticulo"
                        ID="txtFiltroArticulo_AutoCompleteExtender" Enabled="true" UseContextKey="true"
                        MinimumPrefixLength="1" CompletionInterval="10" CompletionSetCount="10" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12 col-sm-12 center padding-top-10">
                <div class="table-responsive">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="GridInvetarioProductos" ShowHeaderWhenEmpty="True"
                                AutoGenerateColumns="False" AllowSorting="true" OnRowDataBound="GridInvetarioProductos_RowDataBound"
                                EmptyDataText="El inventario no cuenta con exitencias" EmptyDataRowStyle-ForeColor="Red" AllowPaging="true" PageSize="15"
                                OnPageIndexChanging="GridInvetarioProductos_PageIndexChanging"
                                EditRowStyle-ForeColor="#444444" ShowFooter="true"
                                CssClass="table table-bordered" DataKeyNames="idAlmacen,idArticulo">
                                <Columns>
                                    <asp:TemplateField HeaderText="Clave" SortExpression="idArticulo">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("idArticulo") %>' ID="Label21"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Producto" SortExpression="descripcion">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("descripcion") %>' ID="Label101"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Existencia" SortExpression="cantidadExistencia">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("cantidadExistencia","{0:N3}") %>' ID="lblCantidadExistencia"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Costo Unitario" Visible="false" SortExpression="costoUnitario" ItemStyle-CssClass=" text-right">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("costoUnitario","{0:C2}") %>' ID="Label4"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stock Min" SortExpression="cantidadMinima">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("cantidadMinima","{0:N3}") %>' ID="Label5"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stock Max" SortExpression="cantidadMaxima">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("cantidadMaxima","{0:N3}") %>' ID="Label6"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Precio Venta" SortExpression="ventaUnitaria">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("ventaUnitaria","{0:C2}") %>' ID="Label7"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
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
    </asp:Panel>
</asp:Content>

