<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="Usuarios_Permisos.aspx.cs" Inherits="Usuarios_Permisos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <center>
        <div class="ancho95 centrado center">
            <div class="row">
                <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
                   <i class="icon-user"></i>&nbsp;&nbsp;<asp:Label runat="server" ID="lblTitulo" Text="Usuarios Permisos" CssClass="alert-success"></asp:Label>&nbsp;&nbsp;<i class="icon-lock"></i>
                </div>
            </div>
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="Panel1" runat="server">
                        <div class="inline ancho95 text-center">
                            <asp:Label ID="Label1" runat="server" Text="Usuarios" CssClass="inline" />&nbsp;
                            <asp:DropDownList ID="ddlUsuarios" runat="server" CssClass="inline" DataSourceID="SqlDataSource1" DataTextField="nombreCompleto" DataValueField="usuario" AutoPostBack="true" OnSelectedIndexChanged="ddlUsuarios_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                                SelectCommand="SELECT usuario,(nombre+' '+apellido_pat+' '+isnull(apellido_mat,'')) as nombreCompleto FROM usuarios_PV where Usuario<>'Supervisor' and estatus='A'">
                            </asp:SqlDataSource>
                        </div>
                        <br />
                        <table class="centrado">
                            <tr>
                                <td class="text-center padding8px">
                                    <asp:Label ID="Label2" runat="server" Text="Seleccione permiso para asignar." CssClass="errores" />
                                </td>
                                <td class="text-center padding8px">
                                    <asp:Label ID="Label3" runat="server" Text="Seleccione permiso para eliminar." CssClass="errores" />
                                </td>
                            </tr>
                            <tr>
                                <td class="text-center top0 verticalTop">
                                     <asp:GridView ID="GridPermisos" runat="server" AutoGenerateColumns="False" DataKeyNames="id_permiso" DataSourceID="SqlDataSource2"
                                        OnRowCommand="GridPermisos_RowCommand" EmptyDataText="No hay permisos que asignar" AllowPaging="true" PageSize="10"
                                        CssClass="table table-bordered center top0 verticalTop">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Permiso" SortExpression="descripcion">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDaPermiso" runat="server" Text='<%# Bind("descripcion") %>' CommandName="permiso" CssClass="sinDecoracion" CommandArgument='<%# Bind("id_permiso") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataRowStyle CssClass="errores" />
                                    </asp:GridView>
                                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select * from permisos_PV p where p.id_permiso not in (select u.id_permiso from usuarios_permisos_PV u where u.usuario=@usuario)">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddlUsuarios" PropertyName="SelectedValue" Name="usuario" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                                <td class="text-center top0 verticalTop">
                                    <asp:GridView ID="GridUsuariosPermisos" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource3" DataKeyNames="id_permiso"
                                        OnRowCommand="GridUsuariosPermisos_RowCommand" EmptyDataText="No hay permisos asignados" AllowPaging="true" PageSize="10"
                                        CssClass="table table-bordered center top0 verticalTop">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Permiso" SortExpression="usuario">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" Text='<%# Bind("descripcion") %>' CommandName="quitaPermiso" ID="btnQuitaPermiso" CssClass="sinDecoracion" CommandArgument='<%# Bind("id_permiso") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataRowStyle CssClass="errores" />
                                    </asp:GridView>
                                    <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                                        SelectCommand="select up.id_permiso,up.usuario,p.descripcion from usuarios_permisos_PV up inner join permisos_PV p on p.id_permiso=up.id_permiso where usuario=@usuario">
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

