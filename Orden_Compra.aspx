<%@ Page Title="" Language="C#" MasterPageFile="~/PuntoVenta.master" AutoEventWireup="true" CodeFile="Orden_Compra.aspx.cs" Inherits="Orden_Compra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false"></telerik:RadSkinManager>
    
    <asp:Panel ID="Panel2" runat="server" Visible="false">
    <center>
        <asp:RadioButtonList Visible="false" ID="rbtnOrdenVista" runat="server" OnSelectedIndexChanged="rbtnOrdenVista_SelectedIndexChanged"
            AutoPostBack="true" RepeatDirection="Horizontal" CellPadding="10" >
            <asp:ListItem Value="N" Text="Nueva Orden de Compra" Selected="True" />
            <asp:ListItem Value="O" Text="Ordenes de Compra Generadas" />
        </asp:RadioButtonList>
        <br />
        <asp:TextBox ID="txtRequerimiento" runat="server" placeholder="Requerimiento" Rows="5" TextMode="MultiLine" CssClass="form-control textNota" />
        <asp:Button ID="btnAceptar" runat="server" Text="Agregar" CssClass="btn-success" OnClick="btnAceptar_Click" />
        <br />
        <br />
        <div class="row center ">
            <div class="col-lg-3 col-sm-3 center"></div>
                <div class="col-lg-6 col-sm-6 center">
                    <div class="table-responsive">
                        <asp:GridView ID="GridTempDetalle" runat="server" EmptyDataText="No existen solicitudes generadas"
                            CssClass="table table-bordered center" AllowSorting="true" AllowPaging="true" PageIndex="5" 
                            AutoGenerateColumns="false" OnRowDeleting="GridTempDetalle_RowDeleting" >
                            <Columns>
                                <asp:TemplateField HeaderText="Texto" SortExpression="texto">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTextoGrid" runat="server" Text='<%# Eval("texto") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRenglonGrid" runat="server" Text='<%# Eval("renglon") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Button runat="server" Text="Eliminar" CssClass="btn-danger" CommandName="Delete" CausesValidation="False" ID="Button1" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataRowStyle CssClass="errores" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        <br />
        <div class="row center ">
        <asp:Button ID="btnNuevo" Visible="false" runat="server" Text="Generar" CssClass="btn-success" OnClick="btnNuevo_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnCancelarNuevo" Visible="false" runat="server" Text="Cancelar" CssClass="btn-danger" OnClick="btnCancelarNuevo_Click" />
            </div>
        <br />
        <asp:Label ID="lblErrores" runat="server" CssClass="errores" />
    </center></asp:Panel>
    <asp:Panel ID="Panel3" runat="server" CssClass="venta">
    <div class="row">
        <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
            <i class="icon-folder-open"></i>&nbsp;<asp:Label runat="server" ID="lblTitulo" Text="Orden de Compra" CssClass="alert-success"></asp:Label>
        </div>
    </div>
    <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <div class="col-lg-12 col-sm-12 center text-center marTop">
        <div class="col-lg-4 col-sm-4 center text-center">
            <asp:Label ID="Label2" runat="server" Text="Categoría:"></asp:Label>&nbsp;
            <asp:DropDownList ID="ddlCategoria" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                DataSourceID="SqlDataSource101" DataTextField="descripcion_categoria" 
                DataValueField="id_categoria" 
                onselectedindexchanged="ddlCategoria_SelectedIndexChanged">
                <asp:ListItem Value="-1" Text="Todas"></asp:ListItem>
            </asp:DropDownList>
            <asp:SqlDataSource ID="SqlDataSource101" runat="server" 
                ConnectionString="<%$ ConnectionStrings:PVW %>" 
                SelectCommand="select distinct isnull(p.id_categoria,0) as id_categoria, isnull(c.descripcion_categoria,'Sin Categoría') as descripcion_categoria 
from catproductos p left join CatCategorias c on c.id_categoria=p.id_categoria 
order by 1">
            </asp:SqlDataSource>
        </div>
        <div class="col-lg-4 col-sm-4 center text-center marTop">
            <asp:Label ID="Label1" runat="server"><i class="icon-search"></i></asp:Label>&nbsp;
            <telerik:RadAutoCompleteBox Runat="server" ID="RadAutoCompleteBox" EmptyMessage="Producto" MaxResultCount="5" OnTextChanged="RadAutoCompleteBox_TextChanged"
                DataSourceID="SqlDataSource100" RenderMode="Auto" CssClass="input-medium" Skin="MetroTouch" DataTextField="descripcion" EnableAriaSupport="true" AccessKey="W" InputType="Text" DropDownWidth="300px" DropDownHeight="200px">
            </telerik:RadAutoCompleteBox>
            <asp:SqlDataSource runat="server" ID="SqlDataSource100" ConnectionString="<%$ ConnectionStrings:PVW %>" ProviderName="System.Data.SqlClient" SelectCommand="select aa.idarticulo+' / '+c.descripcion as descripcion from articulosalmacen aa inner join catproductos c on c.idproducto=aa.idarticulo left join CatCategorias a on a.id_categoria=c.id_categoria where aa.idalmacen=@almacen and a.id_categoria=@categoria">
                <SelectParameters>
                    <asp:QueryStringParameter Name="almacen" QueryStringField="p" DbType="Int32" DefaultValue="0" />
                    <asp:ControlParameter ControlID="ddlCategoria" Name="categoria" DbType="Int32" DefaultValue="0" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:Label ID="lblProducto" runat="server" Visible="false" />
        </div>
        <div class="col-lg-2 col-sm-2 center text-center marTop">
            <telerik:RadNumericTextBox CssClass="input-small" runat="server" ID="txtCantidad" EmptyMessage="Cantidad" Skin="MetroTouch" MinValue="1" ShowSpinButtons="true" NumberFormat-DecimalDigits="3"></telerik:RadNumericTextBox>
        </div>
        <div class="col-lg-2 col-sm-2 center text-center marTop">
            <asp:LinkButton ID="lnkAgregarProd" OnClick="lnkAgregarProd_Click" runat="server" CssClass="btn btn-info"><i class="icon-plus"></i><span>&nbsp;Agregar</span></asp:LinkButton>
        </div>
        <div class="col-lg-2 col-sm-2 center text-center">
            <asp:Label ID="lblIdArticulo" runat="server" Visible="false"/>
            <asp:Label ID="lblCategoria" runat="server" Visible="false"/>
        </div>
    </div>    
    <div class="col-lg-12 col-sm-12 center text-center marTop">
        <asp:Label ID="lblError" runat="server" CssClass="alert-danger text-danger negritas" />
    </div>
    
    <div class="col-lg-12 col-sm-12 center text-center marTop">

        <asp:Panel ID="PanelGrid" runat="server" ScrollBars="Auto" Height="350px">

            <asp:GridView ID="GridOrdenCompra" runat="server" CssClass="table table-bordered center" AutoGenerateColumns="false"
                OnRowCancelingEdit="GridOrdenCompra_RowCancelingEdit" DataKeyNames="IdArticulo"
                OnRowDeleting="GridOrdenCompra_RowDeleting" 
                OnRowEditing="GridOrdenCompra_RowEditing"
                OnRowUpdating="GridOrdenCompra_RowUpdating">
                <Columns>
                    <asp:TemplateField HeaderText="Categoría" SortExpression="descripcion_categoria">
                        <ItemTemplate>
                            <asp:Label ID="lblCategoItem" runat="server" Text='<%# Eval("descripcion_categoria") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblCategoEdit" runat="server" Text='<%# Eval("descripcion_categoria") %>' />                            
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Producto" SortExpression="producto">
                        <ItemTemplate>
                            <asp:Label ID="lblProdGrid" runat="server" Text='<%# Eval("producto") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblProdGridE" runat="server" Text='<%# Eval("producto") %>' />
                            <asp:Label ID="lblIdArticuloEdit" runat="server" Text='<%# Eval("IdArticulo") %>' Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cantidad" SortExpression="cantidad">
                        <ItemTemplate>
                            <asp:Label ID="lblCantidadGrid" runat="server" Text='<%# string.Format("{0:n3}", Eval("cantidad")) %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadNumericTextBox CssClass="input-small" runat="server" Skin="MetroTouch" ID="txtCantidadEdit" EmptyMessage="Cantidad" Text='<%# string.Format("{0:n3}", Eval("cantidad")) %>' MinValue="0.001" IncrementSettings-Step="1" ShowSpinButtons="true" NumberFormat-DecimalDigits="3"></telerik:RadNumericTextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" ToolTip="Actualizar" CommandName="Update" CausesValidation="True" ID="lnkActualiza" CssClass="btn btn-success"><i class="icon-save"></i></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ToolTip="Editar" CommandName="Edit" CausesValidation="False" ID="lnkEdita" CssClass="btn btn-warning"><i class="icon-edit"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ToolTip="Eliminar" CommandName="Delete" CausesValidation="False" ID="lnkElimina" CssClass="btn btn-danger" OnClientClick="return confirm('¿Está seguro de eliminar el producto señalado?')"><i class="icon-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" ToolTip="Cancelar" CommandName="Cancel" CausesValidation="False" ID="lnkCancela" CssClass="btn btn-danger"><i class="icon-ban-circle"></i></asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        
            <telerik:RadGrid RenderMode="Lightweight" Visible="false" ID="GridOrdenCompra2" runat="server" PageSize="20"
                AllowSorting="True" AllowMultiRowSelection="True" AllowPaging="True" ShowGroupPanel="True"
                AutoGenerateColumns="False" GridLines="none" Skin="MetroTouch" >
                <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                <MasterTableView Width="100%">
                    <GroupByExpressions>
                        <telerik:GridGroupByExpression>
                            <SelectFields>
                                <telerik:GridGroupByField FieldAlias="Categoria" FieldName="descripcion_categoria"></telerik:GridGroupByField>
                            </SelectFields>
                            <GroupByFields>
                                <telerik:GridGroupByField FieldName="descripcion_categoria" SortOrder="Descending"></telerik:GridGroupByField>
                            </GroupByFields>
                        </telerik:GridGroupByExpression>
                    </GroupByExpressions>
                    <Columns>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn">
                        </telerik:GridEditCommandColumn>
                        <telerik:GridBoundColumn SortExpression="producto" HeaderText="Producto" HeaderButtonType="TextButton"
                            DataField="producto"></telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Producto">
                            <ItemTemplate>
                                <asp:Label ID="lblProductoGrid" runat="server" Text='<%# Eval("producto") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtProductoEdit" runat="server" Text='<%# Eval("producto") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn SortExpression="cantidad" HeaderText="Cantidad" HeaderButtonType="TextButton"
                            DataField="cantidad" DataFormatString="{0:0.0000}" >
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings ReorderColumnsOnClient="True" AllowDragToGroup="True" AllowColumnsReorder="True">
                    <Selecting AllowRowSelect="True"></Selecting>
                    <Resizing AllowRowResize="True" AllowColumnResize="True" EnableRealTimeResize="True"
                        ResizeGridOnColumnResize="False"></Resizing>
                </ClientSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            </telerik:RadGrid>

        </asp:Panel>
        <div class="col-lg-12 col-sm-12 center text-center martop">
            <asp:LinkButton ID="lnkGenerarOrden" OnClick="lnkGenerarOrden_Click" runat="server" CssClass="btn btn-info" OnClientClick="return confirm('¿Está seguro de enviar la solicitud de orden de compra?. Una vez generada no se pueden efectuar cambios posteriores; tendrá que generar una nueva orden.')"><i class="icon-save"></i><span>&nbsp;Mandar Orden de Compra</span></asp:LinkButton>
        </div>
        
    </div>
    
    </ContentTemplate>
    </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="Panel1" runat="server" Visible="false">
        <asp:Label ID="lblUsuario" runat="server" />
        <asp:Label ID="lblIsla" runat="server" />
    </asp:Panel>
</asp:Content>

