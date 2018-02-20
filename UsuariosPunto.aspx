<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UsuariosPunto.aspx.cs" Inherits="UsuariosPunto" MasterPageFile="~/Administracion.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>
                <div class="ancho95 centrado center">
                    <div class="row">
                        <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
                           <i class="icon-user"></i>&nbsp;&nbsp;<asp:Label runat="server" ID="lblTitulo" Text="Usuarios - " CssClass="alert-success"></asp:Label>
                           <i class="icon-map-marker"></i>&nbsp;&nbsp;<asp:Label runat="server" ID="Label2" Text="Tiendas" CssClass="alert-success"></asp:Label>
                        </div>
                    </div>
                    <br />
                     <div class="row center " >                        
                        <div class="col-lg-12 col-sm-12 text-center">
                            <div class="row">
                                <div class="col-lg-12 col-sm-12 text-center">
                                    <asp:Label ID="Label1" runat="server" Text="Usuario:"></asp:Label>
                                    <asp:DropDownList ID="ddlUsuario" runat="server" DataSourceID="SqlDataSource2" 
                                        DataTextField="nombre" DataValueField="usuario" AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe seleccionar un usuario" Text="*" ValidationGroup="agrega" ControlToValidate="ddlUsuario" CssClass="errores"></asp:RequiredFieldValidator>
                                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                        SelectCommand="select usr.usuario,(usr.nombre+' '+usr.apellido_pat+' '+isnull(usr.apellido_mat,'')) as nombre  from usuarios_PV usr  where usr.usuario&lt;&gt;'Supervisor' and usr.estatus='A'">
                                    </asp:SqlDataSource>
                                </div>                                                            
                            </div>                
                            <div class="row">
                                <div class="col-lg-12 col-sm-12 alert-danger negritas center" >
                                    <asp:Label ID="lblErrores" runat="server" CssClass="errores"></asp:Label>
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="agrega" />
                                </div>
                            </div>
                        </div>  
                    </div>
                    <br />
                    <div class="row center ">
                        <div class="col-lg-6 col-sm-6 center">
                            <div class="text-center pad1em alert-info negritas">
                                <asp:Label ID="Label3" runat="server" Text="Seleccione las Tiendas a asignar"></asp:Label>&nbsp;&nbsp;
                                <asp:CheckBox ID="chkAgregaTodas" runat="server" Text="Agregar Todo" oncheckedchanged="chkAgregaTodas_CheckedChanged" AutoPostBack="true" />
                            </div>
                            <div class="table-responsive">
                                <asp:GridView ID="grdIslas" runat="server" AutoGenerateColumns="False" AllowPaging="True" AllowSorting="true"
                                    DataKeyNames="idAlmacen" DataSourceID="SqlDataSource4" PageSize="9"
                                    CssClass="table table-bordered center " 
                                    EmptyDataText="No existen Tiendas Registradas">
                                    <Columns>
                                        <asp:BoundField DataField="idAlmacen" HeaderText="idAlmacen" ReadOnly="True" SortExpression="idAlmacen" Visible="false" />
                                        <asp:TemplateField HeaderText="Tienda" SortExpression="nombre">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("nombre") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkIsla" runat="server" CommandArgument='<%# Eval("idAlmacen") %>' Text='<%# Bind("nombre") %>' CssClass="link" OnClick="lnkIsla_Click"></asp:LinkButton>                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select idAlmacen,nombre from catalmacenes where estatus='A' AND idAlmacen not in(select id_punto from usuario_puntoventa where usuario=@usuario)">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlUsuario" Name="usuario" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="col-lg-6 col-sm-6 center">
                            <div class="text-center pad1em alert-info negritas">
                                <asp:Label ID="Label4" runat="server" Text="Seleccione las Tiendas a desasignar"></asp:Label>&nbsp;&nbsp;
                                <asp:CheckBox ID="CheckBox1" runat="server" Text="Desasignar Todo" oncheckedchanged="CheckBox1_CheckedChanged" AutoPostBack="true"/>
                            </div>
                            <div class="table-responsive">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                                    CssClass="table table-bordered center " EmptyDataText="No existen Tiendas Asignadas"  DataSourceID="SqlDataSource1" 
                                    AllowPaging="True" PageSize="9" AllowSorting="True" >
                                    <Columns>
                                        <asp:BoundField DataField="id_punto" HeaderText="id_punto" SortExpression="id_punto" Visible="false"/>
                                        <asp:TemplateField HeaderText="Tienda" SortExpression="nombre">                                            
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkIslaDel" runat="server" 
                                                    CommandArgument='<%# Eval("id_punto") %>' Text='<%# Bind("nombre") %>' 
                                                    CssClass="link" onclick="lnkIslaDel_Click"></asp:LinkButton>                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle ForeColor="Red" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select u.id_punto,c.nombre from usuario_puntoventa u inner join catalmacenes c on c.idAlmacen=u.id_punto where u.usuario=@usuario" >
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="ddlUsuario" Name="usuario" PropertyName="SelectedValue" />
                                    </SelectParameters>
                                </asp:SqlDataSource>                                
                            </div>                
                        </div>
                        <div class="col-lg-3 col-sm-3 center"></div>
                    </div>
                </div>   
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
