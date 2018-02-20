<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CatUsuarios.aspx.cs" Inherits="CatUsuarios" MasterPageFile="~/Administracion.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <div class="ancho95 centrado">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
               <i class="icon-user"></i> <asp:Label runat="server" ID="lblTitulo" Text="Usuarios" CssClass="alert-success"></asp:Label>
            </div>
        </div>
        <br />
         <div class="row">
            <div class="col-lg-12 col-sm-12">
                <div class="col-lg-1 col-sm-1 text-left">
                    <asp:Label ID="Label1" runat="server" Text="Usuario:"></asp:Label>                    
                </div>
                <div class="col-lg-2 col-sm-2 text-left">
                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="input-medium" MaxLength="15"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtUsuario_TextBoxWatermarkExtender" runat="server" BehaviorID="txtUsuario_TextBoxWatermarkExtender" TargetControlID="txtUsuario" WatermarkCssClass="water input-medium" WatermarkText="Usuario" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar el usuario" Text="*" ValidationGroup="agrega" ControlToValidate="txtUsuario" CssClass="errores"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="El usuario debe contener como mínimo 3 y un máximo de 15 caracteres." ControlToValidate="txtUsuario" ValidationExpression="[a-zA-Z0-9]{3,15}" ValidationGroup="agrega" CssClass="errores" Text="*"></asp:RegularExpressionValidator>
                </div>
                <div class="col-lg-1 col-sm-1 text-left">
                    <asp:Label ID="Label8" runat="server" Text="Nombre:"></asp:Label>
                </div>
                <div class="col-lg-5 col-sm-5 text-left">                    
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="input-medium" MaxLength="40"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtNombre_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNombre_TextBoxWatermarkExtender" TargetControlID="txtNombre" WatermarkCssClass="water input-medium" WatermarkText="Nombre" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar el nombre" ValidationGroup="agrega" ControlToValidate="txtNombre" Text="*" CssClass="errores"></asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtApPat" runat="server" CssClass="input-small" MaxLength="40"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtApPatWatermarkExtender1" runat="server" BehaviorID="txtApPat_TextBoxWatermarkExtender" TargetControlID="txtApPat" WatermarkCssClass="water input-small" WatermarkText="Ap. Paterno" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Debe indicar el apellido paterno" ValidationGroup="agrega" ControlToValidate="txtApPat" Text="*" CssClass="errores"></asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtApMat" runat="server" CssClass="input-small" MaxLength="40"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtApMatWatermarkExtender2" runat="server" BehaviorID="txtApMat_TextBoxWatermarkExtender" TargetControlID="txtApMat" WatermarkCssClass="water input-small" WatermarkText="Ap. Materno" />
                </div>
                <div class="col-lg-1 col-sm-1 text-left">
                    <asp:Label ID="Label13" runat="server" Text="Contraseña:"></asp:Label>
                </div>
                <div class="col-lg-2 col-sm-2 text-left">                    
                    <asp:TextBox ID="txtContraseña" runat="server" CssClass="input-medium" MaxLength="15"></asp:TextBox>  
                    <cc1:TextBoxWatermarkExtender ID="txtContraseña_TextBoxWatermarkExtender" runat="server" BehaviorID="txtContraseña_TextBoxWatermarkExtender" TargetControlID="txtContraseña" WatermarkCssClass="water input-medium" WatermarkText="Contraseña" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Debe indicar la contraseña" Text="*" ValidationGroup="agrega" ControlToValidate="txtContraseña" CssClass="errores"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="La contraseña debe contener como mínimo 3 y un máximo de 15 caracteres." ControlToValidate="txtContraseña" ValidationExpression="[a-zA-Z0-9]{3,15}" ValidationGroup="agrega" CssClass="errores" Text="*"></asp:RegularExpressionValidator>                     
                </div>
            </div>
        </div>
        <div class="row marTop">
            <div class="col-lg-12 col-sm-12">
                <div class="col-lg-1 col-sm-1 text-left">
                    <asp:Label ID="Label10" runat="server" Text="Teléfono:"></asp:Label>
                </div>
                <div class="col-lg-2 col-sm-2 text-left">                    
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="input-medium" MaxLength="20"></asp:TextBox> 
                    <cc1:FilteredTextBoxExtender ID="txtTelefono_FilteredTextBoxExtender" runat="server" BehaviorID="txtTelefono_FilteredTextBoxExtender" TargetControlID="txtTelefono" FilterType="Numbers" />
                    <cc1:TextBoxWatermarkExtender ID="txtTelefono_TextBoxWatermarkExtender" runat="server" BehaviorID="txtTelefono_TextBoxWatermarkExtender" TargetControlID="txtTelefono" WatermarkText="Teléfono" WatermarkCssClass="water input-medium" />                       
                </div>
                <div class="col-lg-1 col-sm-1 text-left">
                    <asp:Label ID="Label9" runat="server" Text="Dirección:"></asp:Label>
                </div>
                <div class="col-lg-5 col-sm-5 text-left">                    
                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="input-xlarge" MaxLength="200"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtDireccion_TextBoxWatermarkExtender" runat="server" BehaviorID="txtDireccion_TextBoxWatermarkExtender" TargetControlID="txtDireccion" WatermarkCssClass="water input-xlarge" WatermarkText="Dirección" />
                </div>
                <div class="col-lg-1 col-sm-1 text-left">
                    <asp:Label ID="Label14" runat="server" Text="F. Nacimiento:"></asp:Label>
                </div>
                <div class="col-lg-2 col-sm-2 text-left">                    
                    <asp:TextBox ID="txtFecha" runat="server" CssClass="input-small" MaxLength="10"></asp:TextBox>  
                    <cc1:TextBoxWatermarkExtender ID="txtFechaWatermarkExtender1" runat="server" BehaviorID="txtFecha_TextBoxWatermarkExtender" TargetControlID="txtFecha" WatermarkCssClass="water input-small" WatermarkText="aaaa-mm-dd" />                        
                    <cc1:CalendarExtender ID="txtFecha_CalendarExtender" runat="server" BehaviorID="txtFecha_CalendarExtender" TargetControlID="txtFecha" Format="yyyy-MM-dd" CssClass="MyCalendar"  />
                </div>
            </div>
        </div>
        <div class="row marTop">
            <div class="col-lg-12 col-sm-12">
                <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label12" runat="server" Text="Perfil:"></asp:Label></div>
                <div class="col-lg-2 col-sm-2 text-left">
                    <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="input-medium" DataSourceID="SqlDataSource5" DataTextField="descripcion" DataValueField="perfil"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="SELECT [perfil], [descripcion] FROM [perfiles_PV]"></asp:SqlDataSource>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Debe indicar el Perfil" Text="*" ValidationGroup="agrega" ControlToValidate="ddlPerfil" CssClass="errores"></asp:RequiredFieldValidator>
                </div>
                <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label11" runat="server" Text="Puesto:"></asp:Label></div>
                <div class="col-lg-5 col-sm-5 text-left">                                            
                        <asp:DropDownList ID="ddlPuesto" runat="server" CssClass="input-medium"
                            DataSourceID="SqlDataSource4" DataTextField="descripcion" 
                            DataValueField="puesto" >
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource4" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:PVW %>" 
                            SelectCommand="SELECT [puesto], [descripcion] FROM [puestos_PV] WHERE ([puesto] &lt;&gt; @puesto)">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="1" Name="puesto" Type="Int16" />
                            </SelectParameters>
                        </asp:SqlDataSource> 
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Debe indicar el puesto" Text="*" CssClass="errores" ControlToValidate="ddlPuesto" ValidationGroup="agrega"></asp:RequiredFieldValidator>                  
                </div>
                <div class="col-lg-1 col-sm-1 text-left"><asp:Label ID="Label15" runat="server" Text="Correo:"></asp:Label></div>
                <div class="col-lg-2 col-sm-2 text-left">
                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="input-large" MaxLength="200"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtCorreoWatermarkExtender1" runat="server" BehaviorID="txtCorreo_TextBoxWatermarkExtender" TargetControlID="txtCorreo" WatermarkCssClass="water input-large" WatermarkText="Correo" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Debe indicar el correo" ValidationGroup="agrega" ControlToValidate="txtCorreo" Text="*" CssClass="errores"></asp:RequiredFieldValidator>
                </div>                
                <div class="col-lg-3 col-sm-3 text-center">
                     <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn-info" ValidationGroup="agrega" onclick="btnAgregar_Click"  />
                </div>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-lg-12 col-sm-12 center">
                <div class="table-responsive">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-bordered center" EmptyDataText="No existen Usuarios Registrados"
                                DataKeyNames="usuario" DataSourceID="SqlDataSource1" AllowPaging="True" PageSize="6"
                                AllowSorting="True" onrowcommand="GridView1_RowCommand" 
                                onrowdatabound="GridView1_RowDataBound" >
                                <Columns>
                                    <asp:BoundField DataField="usuario" HeaderText="Usuario" ReadOnly="True" SortExpression="usuario"/>
                                    <asp:TemplateField HeaderText="Nombre" SortExpression="nombreCompleto">
                                        <EditItemTemplate>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el nombre" ValidationGroup="edita" ControlToValidate="txtNomMod" Text="*" CssClass="errores"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtNomMod" runat="server" Text='<%# Bind("nombre") %>' CssClass="input-small" MaxLength="40" ></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="txtNomMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNomMod_TextBoxWatermarkExtender" TargetControlID="txtNomMod" WatermarkText="Nombre" WatermarkCssClass="water input-small" />                                            
                                            <br />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar el apellido paterno" ValidationGroup="edita" ControlToValidate="txtApPatMod" Text="*" CssClass="errores"></asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtApPatMod" runat="server" Text='<%# Bind("apellido_pat") %>' CssClass="input-small" MaxLength="40" ></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="txtApPatMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtApPatMod_TextBoxWatermarkExtender" TargetControlID="txtApPatMod" WatermarkText="Ap. Paterno" WatermarkCssClass="water input-small"/>                                            
                                            <br />
                                            <asp:TextBox ID="txtApMatMod" runat="server" Text='<%# Bind("apellido_mat") %>' CssClass="input-small" MaxLength="40"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="txtApMatMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtApMatMod_TextBoxWatermarkExtender" TargetControlID="txtApMatMod" WatermarkText="Ap. Materno" WatermarkCssClass="water input-small"/>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblNombre" runat="server" Text='<%# Bind("nombreCompleto") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="F. Nacimiento" SortExpression="f_nacimiento">
                                        <EditItemTemplate>                                            
                                            <asp:TextBox ID="txtFechaMod" runat="server" Text='<%# Bind("f_nacimiento") %>' CssClass="input-small" MaxLength="10"></asp:TextBox>                        
                                            <cc1:CalendarExtender ID="txtFechaMod_CalendarExtender" runat="server" BehaviorID="txtFechaMod_CalendarExtender" TargetControlID="txtFechaMod" Format="yyyy-MM-dd" CssClass="MyCalendar"  />
                                            <cc1:TextBoxWatermarkExtender ID="txtFechaMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtFechaMod_TextBoxWatermarkExtender" TargetControlID="txtFechaMod" WatermarkText="aaaa-mm-dd" WatermarkCssClass="water input-small" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("f_nacimiento") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dirección" SortExpression="direccion">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDireccionMod" runat="server" Text='<%# Bind("direccion") %>' TextMode="MultiLine" Rows="3" MaxLength="200" CssClass="textBoxTextArea"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="txtDireccionMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtDireccionMod_TextBoxWatermarkExtender" TargetControlID="txtDireccionMod" WatermarkCssClass="water textBoxTextArea" WatermarkText="Dirección" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("direccion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Teléfono" SortExpression="telefono">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtTelefonoMod" runat="server" Text='<%# Bind("telefono") %>' CssClass="input-small" MaxLength="20"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="txtTelefonoMod_FilteredTextBoxExtender" runat="server" BehaviorID="txtTelefonoMod_FilteredTextBoxExtender" TargetControlID="txtTelefonoMod" FilterType="Numbers" />
                                            <cc1:TextBoxWatermarkExtender ID="txtTelefonoMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtTelefonoMod_TextBoxWatermarkExtender" TargetControlID="txtTelefonoMod" WatermarkText="Teléfono" WatermarkCssClass="water input-small" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("telefono") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Contraseña" SortExpression="password">
                                        <EditItemTemplate>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar la Contraseña" Text="*" ValidationGroup="edita" ControlToValidate="txtPasswordMod" CssClass="errores"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="La contraseña debe contener como mínimo 5 y un máximo de 15 caracteres." ControlToValidate="txtPasswordMod" ValidationExpression="[a-zA-Z0-9]{5,15}" ValidationGroup="edita" CssClass="errores" Text="*"></asp:RegularExpressionValidator>
                                            <asp:TextBox ID="txtPasswordMod" runat="server" Text='<%# Bind("password") %>' CssClass="input-small" MaxLength="15"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="txtPasswordMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtPasswordMod_TextBoxWatermarkExtender" TargetControlID="txtPasswordMod" WatermarkCssClass="water input-small" WatermarkText="Contraseña" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("password") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Puesto" SortExpression="nomPuesto">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlPuestoMod" runat="server" CssClass="input-medium"
                                                DataSourceID="SqlDataSource2" DataTextField="descripcion" 
                                                DataValueField="puesto" SelectedValue='<%# Bind("puesto") %>'>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                                ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                                SelectCommand="SELECT [puesto], [descripcion] FROM [puestos_PV] WHERE ([puesto] &lt;&gt; @puesto)">
                                                <SelectParameters>
                                                    <asp:Parameter DefaultValue="1" Name="puesto" Type="Int16" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("nomPuesto") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho120px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Perfil" SortExpression="nopmPerfil">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlPerfilMod" runat="server" CssClass="input-small"
                                                DataSourceID="SqlDataSource3" DataTextField="descripcion"  
                                                DataValueField="perfil" SelectedValue='<%# Bind("perfil") %>'>
                                            </asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                                                ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                                SelectCommand="SELECT [perfil], [descripcion] FROM [perfiles_PV]">
                                            </asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("nopmPerfil") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho120px" />
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="Correo" SortExpression="correo">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtCorreoMod" runat="server" Text='<%# Bind("correo") %>' TextMode="MultiLine" Rows="3" MaxLength="200" CssClass="textBoxTextArea"></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="txtCorreoWatermarkExtender" runat="server" BehaviorID="txtCorreoMod_TextBoxWatermarkExtender" TargetControlID="txtCorreoMod" WatermarkCssClass="water textBoxTextArea" WatermarkText="Correo" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="Debe indicar el correo" ValidationGroup="edita" ControlToValidate="txtCorreoMod" Text="*" CssClass="errores"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label80" runat="server" Text='<%# Bind("correo") %>'></asp:Label>
                                        </ItemTemplate>
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
                                                CommandName="Delete" Text="Inactiva" CommandArgument='<%# Eval("usuario")+";"+Eval("estatus") %>'  />
                                        </ItemTemplate>                                        
                                        <ItemStyle CssClass="ancho50px" />                                        
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle ForeColor="Red" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"                                 
                                SelectCommand="select u.usuario,rtrim(ltrim(u.nombre)) as nombre,rtrim(ltrim(u.apellido_pat)) as apellido_pat,rtrim(ltrim(u.apellido_mat)) as apellido_mat,(rtrim(ltrim(u.nombre))+' '+rtrim(ltrim(u.apellido_pat))+' '+rtrim(ltrim(isnull(u.apellido_mat,'')))) as nombreCompleto,convert(char(10),u.f_nacimiento,126) as f_nacimiento,rtrim(ltrim(u.direccion)) as direccion,rtrim(ltrim(u.telefono)) as telefono,rtrim(ltrim(u.password)) as password, u.puesto,rtrim(ltrim(p.descripcion)) as nomPuesto,u.perfil,rtrim(ltrim(f.descripcion)) as nopmPerfil, u.estatus, u.correo from usuarios_PV u inner join puestos_PV p on p.puesto=u.puesto inner join perfiles_PV f on f.perfil=u.perfil where u.usuario<>'Supervisor' order by u.usuario" >
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
        </div>
        <div class="row">
            <div class="col-lg-12 col-sm-12 center">
                
            </div>
        </div>
    </div>     
</asp:Content>