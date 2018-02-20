<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CatLineas.aspx.cs" Inherits="CatLineas" MasterPageFile="~/Administracion.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <center>
    <div class="ancho95 centrado center">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
               <i class="icon-align-justify"></i> <asp:Label runat="server" ID="lblTitulo" Text="Líneas" CssClass="alert-success"></asp:Label>
            </div>
        </div>
        <br />
         <div class="row center " >
            <div class="col-lg-2 col-sm-2 center"></div>
            <div class="col-lg-9 col-sm-9 center">
                <div class="row">
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:Label ID="Label1" runat="server" Text="Clave:"></asp:Label>
                        <asp:TextBox ID="txtClave" runat="server" CssClass="input-medium" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtClave_TextBoxWatermarkExtender" runat="server" BehaviorID="txtClave_TextBoxWatermarkExtender" TargetControlID="txtClave" WatermarkCssClass="water input-medium" WatermarkText="Clave" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar la clave" Text="*" ValidationGroup="agrega" ControlToValidate="txtClave" CssClass="errores"></asp:RequiredFieldValidator>                        
                    </div>
                    <div class="col-lg-5 col-sm-5 text-left">
                         <asp:Label ID="Label8" runat="server" Text="Línea:"></asp:Label>
                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="input-large" MaxLength="60"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtDescripcion_TextBoxWatermarkExtender" runat="server" BehaviorID="txtDescripcion_TextBoxWatermarkExtender" TargetControlID="txtDescripcion" WatermarkCssClass="water input-large" WatermarkText="Línea" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar la línea" ValidationGroup="agrega" ControlToValidate="txtDescripcion" Text="*" CssClass="errores"></asp:RequiredFieldValidator>                        
                    </div>
                    <div class="col-lg-1 col-sm-1">
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn-info" ValidationGroup="agrega" onclick="btnAgregar_Click"  />
                    </div>
                </div>                
            </div>  
            <div class="col-lg-1 col-sm-1 center"></div>        
        </div>
        <br />
        <div class="row center ">
            <div class="col-lg-3 col-sm-3 center"></div>
            <div class="col-lg-6 col-sm-6 center">
                <div class="table-responsive">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-bordered center " EmptyDataText="No existen Líneas Registradas"
                                DataKeyNames="idLinea" DataSourceID="SqlDataSource1" AllowPaging="True" PageSize="7"
                                AllowSorting="True" onrowcommand="GridView1_RowCommand" 
                                onrowdatabound="GridView1_RowDataBound" >
                                <Columns>
                                    <asp:BoundField DataField="idLinea" HeaderText="Clave" ReadOnly="True" SortExpression="idLinea"/>
                                    <asp:TemplateField HeaderText="Línea" SortExpression="descripcionFamilia">
                                        <EditItemTemplate>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar la Línea" ValidationGroup="edita" ControlToValidate="txtDescMod" Text="*" CssClass="errores"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtDescMod" runat="server" Text='<%# Bind("descripcionLinea") %>' CssClass="input-large" MaxLength="60" ></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="txtDescMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtDescMod_TextBoxWatermarkExtender" TargetControlID="txtDescMod" WatermarkText="Línea" WatermarkCssClass="water input-large" />                                                                                        
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# Bind("descripcionLinea") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho200px" />
                                    </asp:TemplateField>                                                                        
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:Button ID="btnActualizar" runat="server" CausesValidation="True" 
                                                CommandName="Update" Text="Actualizar" ValidationGroup="edita" CssClass="btn-success" /><br /><br />
                                            <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" 
                                                CommandName="Cancel" Text="Cancelar" CssClass="btn-danger" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Button ID="btnEditar" runat="server" CausesValidation="False" 
                                                CommandName="Edit" Text="Editar" CssClass="btn-warning" />
                                        </ItemTemplate>
                                        <ControlStyle CssClass="btn-warning ancho50px" />
                                        <ItemStyle CssClass="ancho50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>                                            
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Button ID="btnEliminar" runat="server" CausesValidation="False" 
                                                CommandName="Delete" Text="Elimina" CommandArgument='<%# Eval("idLinea")%>'  />
                                        </ItemTemplate>                                                                            
                                        <ItemStyle CssClass="ancho50px" />                                        
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle ForeColor="Red" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"                                 
                                SelectCommand="select idLinea,descripcionLinea from catlineas" >
                            </asp:SqlDataSource>
                            
                            <div class="row">
                                <div class="col-lg-12 col-sm-12 alert-danger negritas center" >
                                    <asp:Label ID="lblErrores" runat="server" CssClass="errores"></asp:Label>
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="agrega" />
                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="edita" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>                
            </div>
            <div class="col-lg-3 col-sm-3 center"></div>
        </div>        
    </div>   
    </center>  
</asp:Content>