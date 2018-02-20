<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="ConsultarAjuste.aspx.cs" Inherits="ConsultarAjuste" %>


<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="ancho95 text-center centrado">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
                <i class="icon-search"></i>
                <asp:Label runat="server" ID="lblTitulo" Text="Consulta de Ajuste Inventario" CssClass="alert-success"></asp:Label>
            </div>
        </div>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
        <div class="row marTop">
            <div class="col-lg-12 col-sm-12 center">
                <div class="col-lg-3 col-sm-3 center">
                    <asp:Label ID="Label8" runat="server" Text="Tiendas:" />
                    <asp:DropDownList ID="ddlIslas" runat="server" DataSourceID="SqlDataSource2" AutoPostBack="true" OnSelectedIndexChanged="ddlIslas_SelectedIndexChanged"
                        DataTextField="nombre" DataValueField="idAlmacen" CssClass="dropdown">
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select u.id_punto as idAlmacen,p.nombre_pv as nombre from usuario_puntoventa U inner join punto_venta p on p.id_punto=u.id_punto where U.usuario=@usuario and U.estatus='A'">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="usuario" QueryStringField="u" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <div class="col-lg-3 col-sm-3 center">
                    <asp:Label ID="Label5" runat="server" Text="Fecha de Inventario:" />
                    <asp:DropDownList ID="ddlFechas" runat="server" DataSourceID="SqlDataSource1" 
                        DataTextField="fecha" DataValueField="fecha" CssClass="dropdown">
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select distinct convert(char(10),fecha,120) as fecha from inventario_inicial where idAlmacen=@isla order by fecha desc">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlIslas" PropertyName="SelectedValue" Name="isla" />
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
                    <asp:GridView runat="server" ID="GridInvetarioProductos" ShowHeaderWhenEmpty="True" AutoGenerateColumns="False" AllowSorting="true"
                                EmptyDataText="El inventario no cuenta con exitencias" EmptyDataRowStyle-ForeColor="Red" OnRowDataBound="GridInvetarioProductos_RowDataBound1"
                                EditRowStyle-ForeColor="#444444" CssClass="table table-bordered" DataKeyNames="idArticulo" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Clave" SortExpression="idarticulo">                                        
                                        <ItemTemplate>
                                           <asp:Label runat="server" Text='<%# Eval("idarticulo") %>' ForeColor="#444444" ID="Label2"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Producto" SortExpression="descripcion">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("descripcion") %>' ID="lblDescripcion"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Precio Venta" SortExpression="precio">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("precio", "{0:C2}") %>' ID="lblPrecio"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Existencia Inicial" ControlStyle-CssClass="text-right" SortExpression="existenciaIni">
                                        <ItemTemplate>
                                            <asp:Label runat="server" CssClass="negritas" Text='<%# Bind("existenciaIni","{0:F3}") %>' ID="lblExistenciaN"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Existencia Final" ControlStyle-CssClass="text-right" SortExpression="existenciaFin">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("existenciaFin","{0:F3}") %>' ID="lblExistenciaFin"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Diferencia" ControlStyle-CssClass="text-right" SortExpression="valor_modificado">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("valor_modificado","{0:F3}") %>' ID="lblModificado"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Inicial" ControlStyle-CssClass="text-right" SortExpression="inicial">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("inicial","{0:C2}") %>' ID="lblInicial"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total Final" ControlStyle-CssClass="text-right" SortExpression="final">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("final","{0:C2}") %>' ID="lblFinal"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Utilidad" ControlStyle-CssClass="text-right" SortExpression="utilidad">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("utilidad","{0:C2}") %>' ID="lblUtilidad"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Usuario" ControlStyle-CssClass="text-right" SortExpression="usuario">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Bind("usuario") %>' ID="lblUsuario"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle ForeColor="Red" />
                            </asp:GridView>
                    <br />
                    <asp:Panel ID="pnltotales" runat="server" CssClass="row negritas">
                        <div class="col-lg-4 col-sm-4">
                            <asp:Label ID="Label3" runat="server" Text="Inicio:"></asp:Label>
                            <asp:Label ID="lblInicio" runat="server"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4">
                            <asp:Label ID="Label4" runat="server" Text="Final:"></asp:Label>
                            <asp:Label ID="lblFinal" runat="server" ></asp:Label>
                        </div>
                        <div class="col-lg-4 col-sm-4">
                            <asp:Label ID="Label6" runat="server" Text="Utilidad:"></asp:Label>
                            <asp:Label ID="lblUtilidad" runat="server" ></asp:Label>
                        </div>
                    </asp:Panel>                    
                    <br />
                    <br />
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

