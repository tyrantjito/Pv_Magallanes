<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="UsuariosFacturacion.aspx.cs" Inherits="UsuariosFacturacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <center>
        <div class="ancho95 centrado center">
            <div class="row">
                <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
                   <i class="icon-user"></i>&nbsp;&nbsp;<asp:Label runat="server" ID="lblTitulo" Text="Usuarios Facturación Tienda" CssClass="alert-success"></asp:Label>&nbsp;&nbsp;<i class="icon-shopping-cart"></i>
                </div>
            </div>
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="Panel1" runat="server">
                        <div class="inline ancho95 text-center">
                            <asp:Label ID="Label1" runat="server" Text="Usuarios" CssClass="inline" />&nbsp;
                            <asp:DropDownList ID="ddlUsuarios" runat="server" CssClass="inline" DataSourceID="SqlDataSource1" DataTextField="nombre" DataValueField="usuario" AutoPostBack="true" OnSelectedIndexChanged="ddlUsuarios_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                                SelectCommand="SELECT usuario,(nombre+' '+apellido_pat+' '+isnull(apellido_mat,'')) as nombre FROM usuarios_PV where Usuario<>'Supervisor' and estatus='A' and perfil=1">
                            </asp:SqlDataSource>
                        </div>
                        <br />
                        <table class="centrado">
                            <tr>
                                <td class="text-center padding8px">
                                    <asp:Label ID="Label2" runat="server" Text="Seleccione una Tienda para asignar." CssClass="errores" />
                                </td>
                                <td class="text-center padding8px">
                                    <asp:Label ID="Label3" runat="server" Text="Seleccione una Tienda para eliminar." CssClass="errores" />
                                </td>
                            </tr>
                            <tr>
                                <td class="text-center top0 verticalTop">
                                     <asp:GridView ID="GridPermisos" runat="server" AutoGenerateColumns="False" DataKeyNames="idAlmacen" DataSourceID="SqlDataSource2"
                                        OnRowCommand="GridPermisos_RowCommand" EmptyDataText="No hay permisos que asignar" AllowPaging="true" PageSize="10"
                                        CssClass="table table-bordered center top0 verticalTop">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Tienda" SortExpression="nombre">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDaPermiso" runat="server" Text='<%# Bind("nombre") %>' CommandName="permiso" CssClass="sinDecoracion" CommandArgument='<%# Bind("idAlmacen") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataRowStyle CssClass="errores" />
                                    </asp:GridView>
                                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                                        SelectCommand="select * from catalmacenes c where c.idAlmacen not in (select uf.idAlmacen from Usuarios_Facturacion uf where uf.usuario=@usuario)">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddlUsuarios" PropertyName="SelectedValue" Name="usuario" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                                <td class="text-center top0 verticalTop">
                                    <asp:GridView ID="GridUsuariosPermisos" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource3" DataKeyNames="idAlmacen"
                                        OnRowCommand="GridUsuariosPermisos_RowCommand" EmptyDataText="No hay permisos asignados" AllowPaging="true" PageSize="10"
                                        CssClass="table table-bordered center top0 verticalTop">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Permiso" SortExpression="nombre">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" Text='<%# Bind("nombre") %>' CommandName="quitaPermiso" ID="btnQuitaPermiso" CssClass="sinDecoracion" CommandArgument='<%# Bind("idAlmacen") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataRowStyle CssClass="errores" />
                                    </asp:GridView>
                                    <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                                        SelectCommand="select uf.idAlmacen,uf.usuario,c.nombre from Usuarios_Facturacion uf inner join catalmacenes c on c.idAlmacen=uf.idAlmacen where usuario=@usuario">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddlUsuarios" PropertyName="SelectedValue" Name="usuario" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="center padding8px">
                                    <asp:Label ID="lblError" runat="server" CssClass="errores centrado" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </center>
</asp:Content>

