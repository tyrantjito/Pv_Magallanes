<%@ Page Title="" Language="C#" MasterPageFile="~/PuntoVenta.master" AutoEventWireup="true" CodeFile="Facturacion_Posterior.aspx.cs" Inherits="Facturacion_Posterior" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <center>
    <div class="ancho95 centrado center">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 negritas font-14 pad1em">
               <i class="icon-th-list"></i> <asp:Label runat="server" ID="lblTitulo" Text="Facturación" CssClass="alert-success"></asp:Label>
            </div>
        </div>
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

            
         <div class="row center" >
             <div class="col-lg-6 col-sm-6">
            <asp:TextBox ID="txtFolio" runat="server" CssClass="input-small" placeholder="Folio Ticket" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnBuscarFolio" runat="server" Text="Buscar" CssClass="btn-info" OnClick="btnBuscarFolio_Click" />
                 <asp:Button ID="btnMultiFacturacion" runat="server" Text="Facturar Varios Tickets" CssClass="btn-info" OnClick="btnMultiFacturacion_Click" />
                 </div>
             <div class="col-lg-6 col-sm-6">
                 <asp:TextBox ID="txtRFC" runat="server" placeholder="R.F.C." CssClass="input-medium" MaxLength="13" Visible="false" />&nbsp;&nbsp;&nbsp;
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="alta" ControlToValidate="txtRFC" Text="*" CssClass="errores" runat="server" ErrorMessage="El formato del R.F.C. Es incorrecto" ValidationExpression="^[A-Za-z]{3,4}[0-9]{6}[0-9A-Za-z]{3}$" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="alta" ControlToValidate="txtRFC" Text="*" CssClass="errores"  ErrorMessage="Necesita colocar el R.F.C." />
                <asp:Button ID="btnActualizar" runat="server" Text="Validar" CssClass="btn-success" Visible="false" OnClick="btnActualizar_Click" OnClientClick="return confirm('¿Está seguro que el RFC ingresado es correcto?')" />&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancelarCliente" runat="server" Text="Cancelar" ToolTip="Cancelar" Visible="false" CssClass="btn-danger" OnClick="btnCancelarCliente_Click" />
            </div>
        </div>
        <div class="alert-danger col-lg-12 col-sm-12 text-center">
            <asp:Label ID="lblErroresFolio" runat="server" CssClass="errores" />
        </div>
        <asp:Panel ID="PanelDetalleVenta" Visible="false" runat="server" CssClass="col-lg-12 col-sm-12 masxHeight marTop" ScrollBars="Auto" >
            <div class="row">
                <div class="col-sm-6 col-lg-6">
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label87" runat="server" Text="Persona: " /></div>
                        <div class="col-lg-8 col-sm-8 text-right">
                            <asp:RadioButtonList ID="rbtnPersona" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CellPadding="5" CellSpacing="10"  OnSelectedIndexChanged="rbtnPersona_SelectedIndexChanged" >
                                <asp:ListItem Value="F" Selected="True" Text="Fisica" />
                                <asp:ListItem Value="M" Text="Moral" />
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label1" runat="server" Text="R.F.C.: " /></div>
                        <div class="col-lg-8 col-sm-8 text-left">
                            <asp:TextBox ID="txtRfcCap" runat="server" CssClass="input-medium" MaxLength="13" placeholder="R.F.C." />                                
                        </div>                        
                    </div>                
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label92" runat="server" Text="Razón Social: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:TextBox ID="txtRazonNew" runat="server" CssClass="input-xxlarge" MaxLength="200" placeholder="Razón Social" />                                
                        </div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar la Razón Social" Text="*" ValidationGroup="crea" ControlToValidate="txtRazonNew" CssClass="alineado errores"></asp:RequiredFieldValidator>                               
                        </div>
                    </div>                        
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label97" runat="server" Text="Calle: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:TextBox ID="txtCalle" runat="server" MaxLength="200" CssClass="input-xxlarge" placeholder="Calle"></asp:TextBox>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator71" runat="server" ErrorMessage="Debe indicar la Calle." Text="*" CssClass="alineado errores" ControlToValidate="txtCalle" ValidationGroup="crea"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label98" runat="server" Text="No. Ext.: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:TextBox ID="txtNoExt" runat="server" MaxLength="20" CssClass="input-small" placeholder="No. Ext."></asp:TextBox>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator72" runat="server" ErrorMessage="Debe indicar el No. Exterior." Text="*" CssClass="alineado errores" ControlToValidate="txtNoExt" ValidationGroup="crea"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label99" runat="server" Text="No. Int.: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left"><asp:TextBox ID="txtNoIntMod" runat="server" MaxLength="20" CssClass="input-small" placeholder="No. Int."></asp:TextBox></div>
                        <div class="col-lg-4 col-sm-4 text-left">&nbsp;</div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label100" runat="server" Text="País: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlPais" runat="server" Width="200" Height="200px" DataValueField="cve_pais" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="false" EnableLoadOnDemand="false" AutoCompleteSeparator="None" Filter="Contains" 
                                EmptyMessage="Seleccione País..." DataSourceID="SqlDataSource10" DataTextField="desc_pais" Skin="Silk" OnDataBinding="PreventErrorOnbinding" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource10" runat="server" SelectCommand="select cve_pais,desc_pais from Paises_f"></asp:SqlDataSource>                                
                        </div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator73" runat="server" ErrorMessage="Debe indicar el País." Text="*" CssClass="alineado errores" ControlToValidate="ddlPais" ValidationGroup="crea"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label101" runat="server" Text="Estado: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left">                                
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlEstado" runat="server" Width="200" Height="200px" DataValueField="cve_edo" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="false" EnableLoadOnDemand="false" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione Estado..." DataSourceID="SqlDataSource11" DataTextField="nom_edo" Skin="Silk" OnDataBinding="PreventErrorOnbinding" OnSelectedIndexChanged="ddlEstado_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource11" runat="server" SelectCommand="select cve_edo,nom_edo from Estados where cve_pais=@pais">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPais" Name="pais" PropertyName="SelectedValue" DefaultValue="0" />
                                </SelectParameters>
                            </asp:SqlDataSource>                                
                        </div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator74" runat="server" ErrorMessage="Debe indicar el Estado." Text="*" CssClass="alineado errores" ControlToValidate="ddlEstado" ValidationGroup="crea"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label102" runat="server" Text="Municip. o Deleg.: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlMunicipio" runat="server" Width="200" Height="200px" OnDataBinding="PreventErrorOnbinding" DataValueField="ID_Del_Mun" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="false" EnableLoadOnDemand="false" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione Deleg./Municip. ..." DataSourceID="SqlDataSource12" DataTextField="Desc_Del_Mun" Skin="Silk" OnSelectedIndexChanged="ddlMunicipio_SelectedIndexChanged">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource12" runat="server" SelectCommand="select ID_Del_Mun,Desc_Del_Mun from DelegacionMunicipio where cve_pais=@pais and ID_Estado=@estado">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPais" Name="pais" PropertyName="SelectedValue" DefaultValue="0"/>
                                    <asp:ControlParameter ControlID="ddlEstado" Name="estado" PropertyName="SelectedValue" DefaultValue="0"/>
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator75" runat="server" ErrorMessage="Debe indicar el Municipio." Text="*" CssClass="alineado errores" ControlToValidate="ddlMunicipio" ValidationGroup="crea"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label103" runat="server" Text="Colonia: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlColonia" runat="server" Width="200" Height="200px" OnDataBinding="PreventErrorOnbinding" DataValueField="ID_Colonia" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="false" EnableLoadOnDemand="false" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione Colonia ..." DataSourceID="SqlDataSource13" DataTextField="Desc_Colonia" Skin="Silk" OnSelectedIndexChanged="ddlColonia_SelectedIndexChanged">                                    
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource13" runat="server" SelectCommand="select ID_Colonia,Desc_Colonia from Colonias where cve_pais=@pais and ID_Estado=@estado and ID_Del_Mun=@municipio">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPais" Name="pais" PropertyName="SelectedValue" DefaultValue="0"/>
                                    <asp:ControlParameter ControlID="ddlEstado" Name="estado" PropertyName="SelectedValue" DefaultValue="0"/>
                                    <asp:ControlParameter ControlID="ddlMunicipio" Name="municipio" PropertyName="SelectedValue" DefaultValue="0"/>
                                </SelectParameters>
                            </asp:SqlDataSource>                                
                        </div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator76" runat="server" ErrorMessage="Debe indicar la Colonia." Text="*" CssClass="alineado errores" ControlToValidate="ddlColonia" ValidationGroup="crea"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label104" runat="server" Text="C.P.: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <telerik:RadComboBox RenderMode="Lightweight" ID="ddlCodigo" runat="server" Width="100" Height="100px" OnDataBinding="PreventErrorOnbinding" DataValueField="ID_Cod_Pos" AutoPostBack="true" MarkFirstMatch="true" AllowCustomText="false" EnableLoadOnDemand="false" AutoCompleteSeparator="None" Filter="Contains"
                                EmptyMessage="Seleccione Código Postal ..." DataSourceID="SqlDataSource14" DataTextField="ID_Cod_Pos" Skin="Silk">
                            </telerik:RadComboBox>
                            <asp:SqlDataSource ID="SqlDataSource14" runat="server" SelectCommand="select case len(id_cod_pos) when 4 then '0'+id_cod_pos else id_cod_pos end as ID_Cod_Pos from Colonias where cve_pais=@pais and ID_Estado=@estado and ID_Del_Mun=@municipio and ID_Colonia=@colonia">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlPais" Name="pais" PropertyName="SelectedValue" DefaultValue="0"/>
                                    <asp:ControlParameter ControlID="ddlEstado" Name="estado" PropertyName="SelectedValue" DefaultValue="0"/>
                                    <asp:ControlParameter ControlID="ddlMunicipio" Name="municipio" PropertyName="SelectedValue" DefaultValue="0"/>
                                    <asp:ControlParameter ControlID="ddlColonia" Name="colonia" PropertyName="SelectedValue" DefaultValue="0"/>
                                </SelectParameters>
                            </asp:SqlDataSource>                                
                        </div>
                        <div class="col-lg-4 col-sm-4 text-left"><asp:RequiredFieldValidator ID="RequiredFieldValidator77" runat="server" ErrorMessage="Debe indicar el Código Postal." Text="*" CssClass="alineado errores" ControlToValidate="ddlCodigo" ValidationGroup="crea"></asp:RequiredFieldValidator></div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label105" runat="server" Text="Localidad: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left"><asp:TextBox ID="txtLocalidad" runat="server" MaxLength="50" CssClass="input-large" placeholder="Localidad"></asp:TextBox></div>
                        <div class="col-lg-4 col-sm-4 text-left">&nbsp;</div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label106" runat="server" Text="Referencia: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left"><asp:TextBox ID="txtReferenciaMod" runat="server" MaxLength="50" CssClass="input-large" placeholder="Referencia"></asp:TextBox></div>
                        <div class="col-lg-4 col-sm-4 text-left"></div>
                    </div>                        
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label115" runat="server" Text="Correo: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:TextBox ID="txtCorreo" runat="server"  MaxLength="250" CssClass="input-xxlarge" placeholder="Correo"></asp:TextBox>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator78" runat="server" ErrorMessage="Debe indicar el Correo." Text="*" CssClass="alineado errores" ControlToValidate="txtCorreo" ValidationGroup="crea"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label94" runat="server" Text="Correo CC: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:TextBox ID="txtCorreoCC" runat="server"  MaxLength="250" CssClass="input-xxlarge" placeholder="Correo CC"></asp:TextBox>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el Correo CC." Text="*" CssClass="alineado errores" ControlToValidate="txtCorreoCC" ValidationGroup="crea"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label96" runat="server" Text="Correo CCO: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:TextBox ID="txtCorreoCCO" runat="server"  MaxLength="250"  CssClass="input-xxlarge" placeholder="Correo CCO"></asp:TextBox>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar el Correo CCO." Text="*" CssClass="alineado errores" ControlToValidate="txtCorreoCCO" ValidationGroup="crea"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="lblErrorActuraliza" runat="server" CssClass="errores"></asp:Label><br />
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="crea" CssClass="errores" DisplayMode="List" />
                        </div>
                    </div>
                </div>


                <div class="col-lg-6 col-sm-6 text-left">
                    <%--<div class="col-lg-12 col-sm-12 text-left">
                            <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label16" runat="server" Text="Persona:" CssClass="verticalAlineado" /></div>
                            <div class="col-lg-10 col-sm-10 text-left">
                                <asp:RadioButtonList ID="rbtnPersona" runat="server" RepeatDirection="Horizontal"  
                                    AutoPostBack="true" OnSelectedIndexChanged="rbtnPersona_SelectedIndexChanged" 
                                    ToolTip="Tipo Persona" CellSpacing="20" CssClass="input-xlarge">
                                    <asp:ListItem Text="Moral" Value="M" Selected="True"/>
                                    <asp:ListItem Text="Fisica" Value="F" />
                                </asp:RadioButtonList>
                            </div>
                    </div>
                        <div class="col-lg-12 col-sm-12 text-left marTop">
                            <asp:Label ID="Label19" runat="server" Text="Razón Social:" />&nbsp;
                            <asp:TextBox ID="txtArchivarNombre" runat="server" placeholder="Razón Social" CssClass="input-xlarge" />
                            <asp:RequiredFieldValidator ControlToValidate="txtArchivarNombre" Text="*" CssClass="errores" ValidationGroup="alta" ID="RequiredFieldValidator7" runat="server" ErrorMessage="RequiredFieldValidator" />
                            <asp:Label ID="Label20" runat="server" Text="Nombre:" Visible="false" />&nbsp;
                            <asp:TextBox ID="txtNombre" runat="server" placeholder="Nombre" Visible="false" CssClass="input-medium" />&nbsp;
                            <asp:RequiredFieldValidator ControlToValidate="txtNombre" ID="RequiredFieldValidator8" runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores" ValidationGroup="alta" />
                            <asp:TextBox ID="txtPaterno" runat="server" placeholder="A Paterno" Visible="false" CssClass="input-small" />&nbsp;
                            <asp:RequiredFieldValidator ControlToValidate="txtPaterno" ID="RequiredFieldValidator9" runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores" ValidationGroup="alta" />
                            <asp:TextBox ID="txtMaterno" runat="server" placeholder="A Materno" Visible="false" CssClass="input-small" />
                        </div>
                    <div class="col-lg-12 col-sm-12 text-left">
                            <div class="col-lg-2 col-sm-2 text-left marTop"><asp:Label ID="Label23" runat="server" Text="Calle:" /></div>
                            <div class="col-lg-5 col-sm-5 text-left marTop">
                                <asp:TextBox ID="txtCalle" runat="server" placeholder="Calle" CssClass="input-large" />
                                <asp:RequiredFieldValidator ControlToValidate="txtCalle" ID="RequiredFieldValidator11" runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores" ValidationGroup="alta" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left marTop"><asp:Label ID="Label25" runat="server" Text="No:" /></div>
                            <div class="col-lg-3 col-sm-3 text-left marTop">
                                <asp:TextBox ID="txtNumero" runat="server" placeholder="No." CssClass="input-mini" />
                                <asp:RequiredFieldValidator ControlToValidate="txtNumero" ID="RequiredFieldValidator12" runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores" ValidationGroup="alta" />
                            </div>
                    </div>
                    <div class="col-lg-12 col-sm-12 text-left">
                            <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label24" runat="server" Text="Colonia:" /></div>
                            <div class="col-lg-5 col-sm-5 text-left">
                                <asp:TextBox ID="txtColonia" runat="server" CssClass="input-large" placeholder="Colonia" />
                                <asp:RequiredFieldValidator ControlToValidate="txtColonia" ID="RequiredFieldValidator13" runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores" ValidationGroup="alta" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label32" runat="server" Text="Interior:" /></div>
                            <div class="col-lg-3 col-sm-3 text-left"><asp:TextBox ID="txtNumeroInt" runat="server" placeholder="No. Int." CssClass="input-mini" /></div>
                    </div>
                    <div class="col-lg-12 col-sm-12 text-left">
                            <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label29" runat="server" Text="Mun./Deleg.:" /></div>
                            <div class="col-lg-5 col-sm-5 text-left">
                                <asp:TextBox ID="txtDelegacion" runat="server" placeholder="Mun./Deleg." CssClass="input-large" />
                                <asp:RequiredFieldValidator ControlToValidate="txtDelegacion" ID="RequiredFieldValidator15" runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores" ValidationGroup="alta" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label26" runat="server" Text="C.P.:" /></div>
                            <div class="col-lg-3 col-sm-3 text-left">
                                <asp:TextBox ID="txtCP" runat="server" placeholder="C.P." CssClass="input-mini" />
                                <asp:RequiredFieldValidator ControlToValidate="txtCP" ID="RequiredFieldValidator14" runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores" ValidationGroup="alta" />
                            </div>
                    </div>
                    <div class="col-lg-12 col-sm-12 text-left">
                            <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label30" runat="server" Text="Estado:" /></div>
                            <div class="col-lg-5 col-sm-5 text-left">
                                <asp:TextBox ID="txtEstado" runat="server" placeholder="Estado" CssClass="input-large" />
                                <asp:RequiredFieldValidator ControlToValidate="txtEstado" ID="RequiredFieldValidator16" runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores" ValidationGroup="alta" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label31" runat="server" Text="Ciudad:" /></div>
                            <div class="col-lg-3 col-sm-3 text-left">
                                <asp:TextBox ID="txtCiudad" runat="server" placeholder="Ciudad:" CssClass="input-medium" />
                                <asp:RequiredFieldValidator ControlToValidate="txtCiudad" ID="RequiredFieldValidator17" runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores" ValidationGroup="alta" />
                            </div>
                    </div>
                    <div class="col-lg-12 col-sm-12 text-left">
                            <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label27" runat="server" Text="Correo:" /></div>                            
                            <div class="col-lg-5 col-sm-5 text-left">
                                <asp:TextBox ID="txtEMail" runat="server" placeholder="Correo Electronico" CssClass="input-large" />
                                <asp:RequiredFieldValidator ControlToValidate="txtEMail" ID="RequiredFieldValidator18" runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores" ValidationGroup="alta" />
                                <asp:RegularExpressionValidator ControlToValidate="txtEMail" ValidationExpression ="^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$" ID="RegularExpressionValidator2" runat="server" ErrorMessage="RegularExpressionValidator"  Text="*" CssClass="errores" ValidationGroup="alta" />
                            </div>
                            <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label33" runat="server" Text="Referencia:" /></div>
                            <div class="col-lg-3 col-sm-3 text-left">
                                <asp:TextBox ID="txtReferenciaFacCliente" runat="server" placeholder="Referencia" CssClass="input-medium" />
                            </div>
                    </div>--%>
                    </div>
                <div class="col-lg-6 col-sm-6 text-left">
                    <div class="col-lg-12 col-sm-12 text-right  negritas">
                        <asp:Label ID="lblUsuario" runat="server" />&nbsp;&nbsp;
                        <asp:Label ID="lblFecha" runat="server" />&nbsp;&nbsp;
                        <asp:Label ID="lblHora" runat="server" />
                    </div>
                    <div class="col-lg-12 col-sm-12 marTop">
                        <asp:GridView ID="GridDetalleVenta" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
                            CssClass="table table-bordered center">
                            <Columns>
                                <asp:BoundField DataField="id_refaccion" HeaderText="Clv Articulo" SortExpression="id_refaccion"></asp:BoundField>
                                <asp:BoundField DataField="descripcion" HeaderText="Descripción" SortExpression="descripcion"></asp:BoundField>
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad"></asp:BoundField>
                                <asp:BoundField DataField="importe" HeaderText="Importe" SortExpression="importe"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select id_refaccion,descripcion,cantidad,importe from venta_det where ticket=@ticket and id_punto=@id_punto">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtFolio" PropertyName="Text" Name="ticket"></asp:ControlParameter>
                                <asp:QueryStringParameter Name="id_punto" QueryStringField="p" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                    <div class="col-lg-12 col-sm-12 text-right negritas">
                        <asp:Label ID="lblNeto" runat="server" /><br />
                        <asp:Label ID="lblDescuentoPorc" runat="server" />
                        <asp:Label ID="lblDesceunto" runat="server" /><br />
                        <asp:Label ID="lblSubTotal" runat="server" /><br />
                        <asp:Label ID="lblIVA" runat="server" /><br />
                        <asp:Label ID="lblTotal" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-lg-12 col-sm-12">
                <div class="col-lg-12 col-sm-12">
                    <asp:Button ID="btnActualizaCliente" runat="server" Text="Actualizar" ToolTip="Actualizar" CssClass="btn-success" OnClick="btnActualizaCliente_Click" Visible="false" ValidationGroup="crea" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnNuevoCliente" runat="server" Text="Aceptar" ToolTip="Aceptar" CssClass="btn-success" OnClick="btnNuevoCliente_Click" Visible="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnFacturar" runat="server" Text="Facturar" ToolTip="Facturar" CssClass="btn-success" OnClick="btnFacturar_Click"  />
                </div>
            </div>
            <div class="col-lg-12 col-sm-12">
                <div class="col-lg-12 col-sm-12">
                    <asp:Label ID="lblErrorFacCliente" runat="server" CssClass="errores" />
                </div>
            </div>
            <div class="col-lg-12 col-sm-12">
                <div class="col-lg-12 col-sm-12">
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" CssClass="errores" />
                </div>
            </div>        
        </asp:Panel>
                </ContentTemplate>
        </asp:UpdatePanel>
        <br /><br /><br />
    </div>
    </center>

</asp:Content>

