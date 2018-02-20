<%@ Page Title="" Language="C#" MasterPageFile="~/PuntoVenta.master" AutoEventWireup="true" CodeFile="MultiFacturacion.aspx.cs" Inherits="MultiFacturacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">        
    </asp:ScriptManager>
     <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnBuscarFolio">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2"></telerik:AjaxUpdatedControl>                    
                </UpdatedControls>
            </telerik:AjaxSetting>            
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="MetroTouch"></telerik:RadAjaxLoadingPanel>
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
             <div class="col-lg-12 col-sm-12">
                 <asp:TextBox ID="txtRFC" runat="server" placeholder="R.F.C." CssClass="input-medium" MaxLength="13" />&nbsp;&nbsp;&nbsp;
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="alta" ControlToValidate="txtRFC" Text="*" CssClass="errores" runat="server" ErrorMessage="El formato del R.F.C. Es incorrecto" ValidationExpression="^[A-Za-z]{3,4}[0-9]{6}[0-9A-Za-z]{3}$" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="alta" ControlToValidate="txtRFC" Text="*" CssClass="errores"  ErrorMessage="Necesita colocar el R.F.C." />&nbsp;&nbsp;&nbsp;
                <asp:CheckBox ID="chkDesglosado" runat="server" Text="Desglosado" />
            </div>
             <div class="col-lg-12 col-sm-12">
<asp:Button ID="btnCargaDatos" runat="server" Text="Validar" CssClass="btn-success" OnClick="btnCargaDatos_Click" OnClientClick="return confirm('¿Está seguro que el RFC ingresado es correcto?')" />&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancelarCliente" runat="server" Text="Cancelar" ToolTip="Cancelar" CssClass="btn-danger" OnClick="btnCancelarCliente_Click" />
             </div>
        </div>
        <div class="alert-danger col-lg-12 col-sm-12 text-center">
            <asp:Label ID="lblError" runat="server" CssClass="errores" />
        </div>
                 </ContentTemplate>
            </asp:UpdatePanel>
        
            <div class="row">
                <div class="col-sm-6 col-lg-6">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
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
                            <asp:SqlDataSource ID="SqlDataSource10" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select cve_pais,desc_pais from Paises_f"></asp:SqlDataSource>                                
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
                            <asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString="<%$ ConnectionStrings:PVW%>" SelectCommand="select cve_edo,nom_edo from Estados_f where cve_pais=@pais">
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
                            <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Del_Mun,Desc_Del_Mun from DelegacionMunicipio_f where cve_pais=@pais and ID_Estado=@estado">
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
                            <asp:SqlDataSource ID="SqlDataSource13" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select ID_Colonia,Desc_Colonia from Colonias_f where cve_pais=@pais and ID_Estado=@estado and ID_Del_Mun=@municipio">
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
                            <asp:SqlDataSource ID="SqlDataSource14" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select case len(id_cod_pos) when 4 then '0'+id_cod_pos else id_cod_pos end as ID_Cod_Pos from Colonias_f where cve_pais=@pais and ID_Estado=@estado and ID_Del_Mun=@municipio and ID_Colonia=@colonia">
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
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="crea" CssClass="errores" DisplayMode="List" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Button ID="btnActualizaCliente" runat="server" Text="Actualizar Información Cliente" ToolTip="Actualizar" CssClass="btn-success" OnClick="btnActualizaCliente_Click" Visible="false" ValidationGroup="crea" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
                </div>

                <div class="col-lg-6 col-sm-6">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                    <div class="row">
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:TextBox ID="txtFolio" runat="server" CssClass="input-small" placeholder="Ticket" />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnBuscarFolio" runat="server" Text="Agregar" CssClass="btn-info" OnClick="btnBuscarFolio_Click" /> 
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
                    <div class="row"> 
                        <div class="col-lg-12 col-sm-12 marTop">
                             <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowPaging="true" PageSize="20" DataSourceID="SqlDataSource5" Skin="Metro">
                                <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                                    <Selecting AllowRowSelect="true"></Selecting>
                                </ClientSettings>
                                <MasterTableView DataKeyNames="ticket" Width="100%" AutoGenerateColumns="False">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="ticket" HeaderText="Ticket" ReadOnly="True" SortExpression="ticket" UniqueName="ticket" />
                                        <telerik:GridBoundColumn DataField="fecha_venta" HeaderText="Fecha Venta" ReadOnly="True" SortExpression="fecha_venta" UniqueName="fecha_venta" DataFormatString="{0:yyyy-MM-dd}" />
                                        <telerik:GridBoundColumn DataField="subtotal" HeaderText="Subtotal" ReadOnly="True" SortExpression="subtotal" UniqueName="subtotal" />
                                        <telerik:GridBoundColumn DataField="porc_descuento" HeaderText="% Descuento" ReadOnly="True" SortExpression="porc_descuento" UniqueName="porc_descuento" />
                                        <telerik:GridBoundColumn DataField="descuento" HeaderText="Descuento" ReadOnly="True" SortExpression="descuento" UniqueName="descuento" />
                                        <telerik:GridBoundColumn DataField="iva" HeaderText="Iva" ReadOnly="True" SortExpression="iva" UniqueName="iva" />
                                        <telerik:GridBoundColumn DataField="importe" HeaderText="Importe" ReadOnly="True" SortExpression="importe" UniqueName="importe" />                                        
                                    </Columns>
                                </MasterTableView>
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                            </telerik:RadGrid>
                            <asp:SqlDataSource ID="SqlDataSource5" ConnectionString="<%$ ConnectionStrings:PVW %>" ProviderName="System.Data.SqlClient" SelectCommand="select v.ticket,v.fecha_venta,v.subtotal,v.porc_descuento,v.descuento,cast((v.total*(v.porc_iva/100)) as decimal(15,2)) as iva,cast(v.total+(v.total*(v.porc_iva/100)) as decimal(15,2)) as importe
from venta_enc v 
inner join ticketsfacturar t on t.isla=v.id_punto and t.ticket = v.ticket" runat="server"></asp:SqlDataSource>
                        </div>
                        <div class="col-lg-12 col-sm-12 marTop">
                            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid2" ShowStatusBar="true" runat="server" AllowPaging="True" PageSize="20" DataSourceID="SqlDataSource6" Skin="Metro">
                                <MasterTableView Width="100%" AutoGenerateColumns="False" DataKeyNames="renglon" DataSourceID="SqlDataSource6">
                                    <Columns>                                        
                                        <telerik:GridBoundColumn DataField="id_refaccion" HeaderText="Clave" ReadOnly="True" SortExpression="id_refaccion" UniqueName="id_refaccion" />
                                        <telerik:GridBoundColumn DataField="descripcion" HeaderText="Producto" ReadOnly="True" SortExpression="descripcion" UniqueName="descripcion" />
                                        <telerik:GridBoundColumn DataField="venta_unitaria" HeaderText="V.U." ReadOnly="True" SortExpression="venta_unitaria" UniqueName="venta_unitaria" />
                                        <telerik:GridBoundColumn DataField="porc_descuento" HeaderText="% Dcto." ReadOnly="True" SortExpression="porc_descuento" UniqueName="porc_descuento" />
                                        <telerik:GridBoundColumn DataField="valor_descuento" HeaderText="Descuento" ReadOnly="True" SortExpression="valor_descuento" UniqueName="valor_descuento" />
                                        <telerik:GridBoundColumn DataField="importe" HeaderText="Importe" ReadOnly="True" SortExpression="importe" UniqueName="importe" />
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                                    <Selecting AllowRowSelect="true"></Selecting>
                                </ClientSettings>
                                <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                            </telerik:RadGrid>
                            <asp:SqlDataSource ID="SqlDataSource6" ConnectionString="<%$ ConnectionStrings:PVW %>" ProviderName="System.Data.SqlClient" SelectCommand="select v.renglon,v.id_refaccion,v.descripcion,v.venta_unitaria,v.porc_descuento,v.valor_descuento,v.importe
from venta_det v 
where v.ticket=@ticket aND v.id_punto=@isla" runat="server">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="RadGrid1" Name="ticket" />
                                    <asp:QueryStringParameter Name="isla" QueryStringField="p" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div> 
                    </div>
                </div>
            </div>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="col-lg-12 col-sm-12">
                        <asp:Button ID="btnGuardarFacturar" runat="server" Text="Guardar y Facturar" CssClass="btn-success" OnClick="btnGuardarFacturar_Click" /> 
                    </div>
                </div>
            </ContentTemplate></asp:UpdatePanel>
        
               
        </div>
    </center>
</asp:Content>

