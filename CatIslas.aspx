<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CatIslas.aspx.cs" Inherits="CatIslas" MasterPageFile="~/Administracion.master" Culture="es-Mx" UICulture="es-Mx"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <div class="ancho95 centrado">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
               <i class="icon-map-marker"></i> <asp:Label runat="server" ID="lblTitulo" Text="Tiendas" CssClass="alert-success"></asp:Label>
            </div>
        </div>
        <br />
         
            <div class="col-lg-12 col-sm-12">
                <div class="col-lg-2 col-sm-2 text-left">
                    <asp:Label ID="Label8" runat="server" Text="Nombre:"></asp:Label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="input-small" MaxLength="30"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtNombre_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNombre_TextBoxWatermarkExtender" TargetControlID="txtNombre" WatermarkCssClass="water input-small" WatermarkText="Nombre" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar el nombre de la Tienda" ValidationGroup="agrega" ControlToValidate="txtNombre" Text="*" CssClass="errores"></asp:RequiredFieldValidator>
                </div>
                <div class="col-lg-4 col-sm-5 text-left">
                    <asp:Label ID="Label9" runat="server" Text="Dirección:"></asp:Label>
                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="input-xlarge" MaxLength="300"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtDireccion_TextBoxWatermarkExtender" runat="server" BehaviorID="txtDireccion_TextBoxWatermarkExtender" TargetControlID="txtDireccion" WatermarkCssClass="water input-xlarge" WatermarkText="Ubicación" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar la Ubicación" Text="*" CssClass="errores" ControlToValidate="txtDireccion" ValidationGroup="agrega"></asp:RequiredFieldValidator>
                </div>
                <div class="col-lg-3 col-sm-3 text-left">
                    <asp:Label ID="Label13" runat="server" Text="Encargado:"></asp:Label>
                    <asp:DropDownList ID="ddlEncargado" runat="server" CssClass="input-medium" DataSourceID="SqlDataSource6" DataTextField="NOMBRE" DataValueField="usuario"></asp:DropDownList>                    
                    <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select usuario,(RTRIM(LTRIM(NOMBRE))+' '+RTRIM(LTRIM(APELLIDO_PAT))+' '+RTRIM(LTRIM(isnull(APELLIDO_MAT,'')))) AS NOMBRE from usuarios_PV where perfil in(3,1) and estatus='A' "></asp:SqlDataSource>
                </div>
                <div class="col-lg-2 col-sm-2 text-left"> 
                    <asp:Label ID="Label14" runat="server" Text="Fondo Caja:"></asp:Label>                   
                    <asp:TextBox ID="txtFondo" runat="server" CssClass="input-small" MaxLength="15"></asp:TextBox>
                    <cc1:FilteredTextBoxExtender ID="txtFondo_FilteredTextBoxExtender" runat="server" BehaviorID="txtFondo_FilteredTextBoxExtender" TargetControlID="txtFondo" FilterType="Numbers, Custom" ValidChars="." />
                    <cc1:TextBoxWatermarkExtender ID="txtFondoWatermarkExtender1" runat="server" BehaviorID="txtFondo_TextBoxWatermarkExtender" TargetControlID="txtFondo" WatermarkCssClass="water input-small" WatermarkText="Fondo Caja" />                                        
                </div> 
                <div class="col-lg-1 col-sm-1 text-center">
                     <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn-info" ValidationGroup="agrega" onclick="btnAgregar_Click"  />
                </div>
             </div>              
                      
        <br />
        <div class="row">
            <div class="col-lg-1 col-sm-1 center"></div>
            <div class="col-lg-10 col-sm-10 center">
                <div class="table-responsive">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered center" EmptyDataText="No existen Tiendas Registradas" DataKeyNames="idAlmacen" DataSourceID="SqlDataSource1" AllowPaging="True" PageSize="7" AllowSorting="True" onrowcommand="GridView1_RowCommand" onrowdatabound="GridView1_RowDataBound" >
                                <Columns>
                                    <asp:BoundField DataField="idAlmacen" HeaderText="Clave" ReadOnly="True" SortExpression="idAlmacen" Visible="false"/>
                                    <asp:TemplateField HeaderText="Nombre" SortExpression="nombre">
                                        <EditItemTemplate>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el nombre" ValidationGroup="edita" ControlToValidate="txtNomMod" Text="*" CssClass="errores"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtNomMod" runat="server" Text='<%# Bind("nombre") %>' CssClass="input-small" MaxLength="30" ></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="txtNomMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNomMod_TextBoxWatermarkExtender" TargetControlID="txtNomMod" WatermarkText="Nombre" WatermarkCssClass="water input-small" />                                                                                        
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNombre" runat="server" Text='<%# Bind("nombre") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho150px" />
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField HeaderText="Dirección" SortExpression="direccion">
                                        <EditItemTemplate>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe indicar la Ubicación" Text="*" CssClass="errores" ControlToValidate="txtDireccionMod" ValidationGroup="edita"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtDireccionMod" runat="server" Text='<%# Bind("ubicacion") %>' MaxLength="300" CssClass="input-large"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="txtDireccionMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtDireccionMod_TextBoxWatermarkExtender" TargetControlID="txtDireccionMod" WatermarkCssClass="water input-large" WatermarkText="Ubicación" />                                            
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("ubicacion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField HeaderText="Encargado" SortExpression="nombreEncargado">
                                        <EditItemTemplate>                                            
                                            <asp:DropDownList ID="ddlEncargadoMod" runat="server" CssClass="input-medium" DataSourceID="SqlDataSource2" DataTextField="NOMBRE" DataValueField="usuario" SelectedValue='<%# Bind("userEncargado") %>' >                                            
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select '' as usuario, 'Seleccione Encargado' as NOMBRE union all select usuario,(RTRIM(LTRIM(NOMBRE))+' '+RTRIM(LTRIM(APELLIDO_PAT))+' '+RTRIM(LTRIM(isnull(APELLIDO_MAT,'')))) AS NOMBRE from usuarios_PV where perfil in(3,1) and estatus='A'"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("nombreEncargado") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho120px" />
                                    </asp:TemplateField>  
                                    <asp:TemplateField HeaderText="Fondo de Caja" SortExpression="fondo">
                                        <EditItemTemplate>                                            
                                            <asp:TextBox ID="txtFondoMod" runat="server" CssClass="input-small" MaxLength="15" Text='<%# Bind("fondo","{0:C2}") %>'></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="txtFondoMod_FilteredTextBoxExtender" runat="server" BehaviorID="txtFondoMod_FilteredTextBoxExtender" TargetControlID="txtFondoMod" FilterType="Numbers, Custom" ValidChars="." />
                                            <cc1:TextBoxWatermarkExtender ID="txtFondoModWatermarkExtender1" runat="server" BehaviorID="txtFondoMod_TextBoxWatermarkExtender" TargetControlID="txtFondoMod" WatermarkCssClass="water input-small" WatermarkText="Fondo Caja" />                                                                
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFondo" runat="server" Text='<%# Bind("fondo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho150px" />
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
                                                CommandName="Delete" Text="Inactiva" CommandArgument='<%# Eval("idAlmacen")+";"+Eval("estatus") %>'  />
                                        </ItemTemplate>                                        
                                        <ItemStyle CssClass="ancho50px" />                                        
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle ForeColor="Red" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select c.idAlmacen,c.nombre,c.estatus,c.ubicacion,isnull(c.idEncargado,0) as idEncargado,ltrim(rtrim(isnull(c.userEncargado,''))) as userEncargado,isnull(c.nombreEncargado,'') AS nombreEncargado,cast(isnull(p.saldo_inicial_pv,0) as decimal(12,2)) as fondo from catalmacenes c left join punto_venta p on p.id_almacen=c.idAlmacen" ></asp:SqlDataSource>                            
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
            <div class="col-lg-1 col-sm-1 center"></div>
        </div>
    </div>     
</asp:Content>