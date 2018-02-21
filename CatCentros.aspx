<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="CatCentros.aspx.cs" Inherits="CatCentros" %>

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
               <i class="icon-th-large"></i> <asp:Label runat="server" ID="lblTitulo" Text="Centros" CssClass="alert-success"></asp:Label>
            </div>
        </div>
        <br />
         <div class="row center " >
            <div class="col-lg-2 col-sm-2 center"></div>
            <div class="col-lg-9 col-sm-9 center">
                <div class="row">
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:Label ID="Label1" runat="server" Text="Nombre:"></asp:Label>
                        <asp:TextBox ID="txtNombreCentro" runat="server" CssClass="input-small" MaxLength="30"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtNombreCentro_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNombreCentro_TextBoxWatermarkExtender" TargetControlID="txtNombreCentro" WatermarkCssClass="water input-small" WatermarkText="Nombre" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Indicar el Nombre" Text="*" ValidationGroup="agrega" ControlToValidate="txtNombreCentro" CssClass="errores"></asp:RequiredFieldValidator>                        
                    </div>
                    <div class="col-lg-3 col-sm-3 text-left">
                         <asp:Label ID="Label8" runat="server" Text="Nomenclatura:"></asp:Label>
                        <asp:TextBox ID="txtNomen" runat="server" CssClass="input-small" MaxLength="10"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtNomen_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNomen_TextBoxWatermarkExtender" TargetControlID="txtNomen" WatermarkCssClass="water input-small" WatermarkText="Nomenclatura" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Indicar Nomenclatura" ValidationGroup="agrega" ControlToValidate="txtNomen" Text="*" CssClass="errores"></asp:RequiredFieldValidator>                        
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
                                CssClass="table table-bordered center " EmptyDataText="No existen Centros Registradas"
                                DataKeyNames="IDCentro" DataSourceID="SqlDataSource1" AllowPaging="True" PageSize="7"
                                AllowSorting="True" onrowcommand="GridView1_RowCommand" 
                                onrowdatabound="GridView1_RowDataBound" >
                                <Columns>
                                    <asp:BoundField DataField="IDCentro" HeaderText="ID" ReadOnly="True" SortExpression="IDCentro"/>

                                    <asp:TemplateField HeaderText="Nombre" SortExpression="Nom_Centro">
                                        <EditItemTemplate>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Indicar Nombre" ValidationGroup="edita" ControlToValidate="txtNomT" Text="*" CssClass="errores"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtNomT" runat="server" Text='<%# Bind("Nom_Centro") %>' CssClass="input-large" MaxLength="30" ></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="txtNomT_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNomT_TextBoxWatermarkExtender" TargetControlID="txtNomT" WatermarkText="Nombre" WatermarkCssClass="water input-medium" />                                                                                        
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNombre" runat="server" Text='<%# Bind("Nom_Centro") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho200px" />
                                    </asp:TemplateField>       
                                    
                                   
                                    <asp:TemplateField HeaderText="Nomenclatura" SortExpression="Nomenclatura">
                                        <EditItemTemplate>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Indicar Nomenclatura" ValidationGroup="edita" ControlToValidate="txtClaT" Text="*" CssClass="errores"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtClaT" runat="server" Text='<%# Bind("NomenclaturaCentro") %>' CssClass="input-large" MaxLength="30" ></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="txtClaT_TextBoxWatermarkExtender" runat="server" BehaviorID="txtClaT_TextBoxWatermarkExtender" TargetControlID="txtClaT" WatermarkText="Nomenclatura" WatermarkCssClass="water input-medium" />                                                                                        
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNomenclatura" runat="server" Text='<%# Bind("NomenclaturaCentro") %>'></asp:Label>
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
                                                CommandName="Delete" Text="Elimina" CommandArgument='<%# Eval("IDCentro")%>'  />
                                        </ItemTemplate>                                                                            
                                        <ItemStyle CssClass="ancho50px" />                                        
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle ForeColor="Red" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"                                 
                                SelectCommand="select *from PV_Centros" >
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
