<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="CatCategoria.aspx.cs" Inherits="CatCategoria" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <div class="ancho95 centrado center">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
                <i class="icon-th"></i>
                <asp:Label runat="server" ID="lblTitulo" Text="Categorias" CssClass="alert-success"></asp:Label>
            </div>
        </div>
        <br />
        <div class="row center ">
            <div class="col-lg-12 col-sm-12 center">
                <asp:Label ID="Label8" runat="server" Text="Categoria:"></asp:Label>
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="input-large" placeholder="Categoria" MaxLength="30"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar la descripción" ValidationGroup="agrega" ControlToValidate="txtDescripcion" Text="*" CssClass="errores"></asp:RequiredFieldValidator>
                <asp:LinkButton ID="lnkAceptarNew" runat="server" CssClass="btn btn-success" Text="Aceptar" ValidationGroup="agrega" OnClick="lnkAceptarNew_Click" />
            </div>
        </div>
        <br />
        <div class="row center">
            <div class="col-lg-12 col-sm-12 text-center">
                <div class="col-lg-3 col-sm-3 text-center"></div>
                <div class="col-lg-6 col-sm-6 text-center" style="margin: 0 auto;">
                    <asp:GridView ID="GridCategoria" runat="server" CssClass="table table-bordered" AllowPaging="True" PageSize="7" AllowSorting="True"
                        EmptyDataText="No se han ingresado Categorias" EmptyDataRowStyle-CssClass="errores" AutoGenerateColumns="False" DataKeyNames="id_categoria" DataSourceID="SqlDataSource1">
                        <Columns>
                            <asp:BoundField DataField="id_categoria" Visible="false" HeaderText="id_categoria" ReadOnly="True" SortExpression="id_categoria"></asp:BoundField>
                            <asp:TemplateField HeaderText="Categoria" SortExpression="descripcion_categoria">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" CssClass="input-large" placeholder="Categoria" Text='<%# Bind("descripcion_categoria") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("descripcion_categoria") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="col-lg-8 col-sm-8 text-left" />
                            </asp:TemplateField>
                            <asp:TemplateField ShowHeader="False">
                                <EditItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" CssClass="btn btn-success"><i class="icon-save"></i></asp:LinkButton>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="False" CommandName="Cancel" CssClass="btn btn-danger"><i class="icon-ban-circle"></i></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CausesValidation="False" CommandName="Edit" CssClass="btn btn-warning"><i class="icon-edit"></i></asp:LinkButton>
                                    <asp:LinkButton Visible="false" ID="LinkButton4" runat="server" CausesValidation="False" CommandName="Delete" CssClass="btn btn-danger"><i class="icon-trash"></i></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle CssClass="col-lg-4 col-sm-4 text-center" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataRowStyle CssClass="errores"></EmptyDataRowStyle>
                    </asp:GridView>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>'
                        DeleteCommand="delete FROM CatCategorias where id_categoria=@id_categoria" InsertCommand="insert into catcategorias values((
                            select isnull((select top 1 c.id_categoria from catcategorias c order by c.id_categoria desc),0)+1),@descripcion_categoria)"
                        SelectCommand="SELECT id_categoria, descripcion_categoria FROM CatCategorias" UpdateCommand="update CatCategorias set descripcion_categoria=@descripcion_categoria where id_categoria=@id_categoria">
                        <DeleteParameters>
                            <asp:Parameter Name="id_categoria"></asp:Parameter>
                        </DeleteParameters>
                        <InsertParameters>
                            <asp:ControlParameter ControlID="txtDescripcion" Name="descripcion_categoria" />
                        </InsertParameters>
                        <UpdateParameters>
                            <asp:Parameter Name="descripcion_categoria"></asp:Parameter>
                            <asp:Parameter Name="id_categoria"></asp:Parameter>
                        </UpdateParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
            <div class="col-lg-12 col-sm-12 negritas center">
                <asp:Label ID="lblErrores" runat="server" CssClass="errores alert-danger"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errores alert-danger" DisplayMode="List" ValidationGroup="agrega" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="errores alert-danger" DisplayMode="List" ValidationGroup="edita" />
            </div>
        </div>
    </div>
</asp:Content>
