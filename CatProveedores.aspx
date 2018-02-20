<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CatProveedores.aspx.cs" Inherits="CatProveedores" MasterPageFile="~/Administracion.master" Culture="es-Mx" UICulture="es-Mx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .btnPad
        {
            margin-top:6px;
        }
        .btnAlign
        { vertical-align:central;}
    </style>
    <script language="javascript" type="text/javascript">
        function msjEstatus(RazSoc, est, tieneEnt)
        {
            if (est == "A")
            {
                if(tieneEnt > 0)
                    return confirm('¿Desea Inactivar el Proveedor: ' + RazSoc + '?');
                else
                    return confirm('Desea eliminar permanentemente, el Proveedor: ' + RazSoc + '?');
            }
            else
                return confirm('¿Está seguro de Reactivar el Proveedor: ' + RazSoc + '?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <div class="ancho95 centrado">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
                <i class="icon-gift"></i>
                <asp:Label runat="server" ID="lblTitulo" Text="Proveedores" CssClass="alert-success"></asp:Label>
            </div>
        </div>

        <asp:UpdatePanel ID="updProvedores" runat="server">
            <ContentTemplate>
                <div class="row marTop">
                <div class="col-lg-1 col-sm-1 text-left">
                    <asp:Label ID="lblRFC" runat="server" Text="R.F.C:"></asp:Label></div>
                <div class="col-lg-3 col-sm-3 text-left">
                    <asp:TextBox ID="txtRFC" runat="server" CssClass="input-large" MaxLength="13" Size="13" />
                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" BehaviorID="txtRFC_TextBoxWatermarkExtender" TargetControlID="txtRFC" WatermarkCssClass="water input-large" WatermarkText="R.F.C" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar el RFC del proveedor" ValidationGroup="agrega" ControlToValidate="txtRFC" Text="*" CssClass="errores"></asp:RequiredFieldValidator>
                </div>
                <div>
                    <div class="col-lg-1 col-sm-1 text-left">
                        <asp:Label ID="Lblrdbtn" runat="server" Text="Persona:"></asp:Label></div>
                    <asp:RadioButtonList ID="rbtPersona" runat="server" OnSelectedIndexChanged="rbtPersona_SelectedIndexChanged"
                        AutoPostBack="true" ToolTip="Tipo Persona" CellSpacing="10" RepeatDirection="Vertical">
                        <asp:ListItem Text="Moral" Value="M" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Fisica" Value="F"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
                <div class="row marTop">
                    <div class="col-lg-1 col-sm-1 text-left">
                        <asp:Label ID="lblRazon" runat="server" Text="Razón Social:"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="input-large" MaxLength="250" ViewStateMode="Disabled" EnableViewState="false"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtRazonSocial_TextBoxWatermarkExtender" runat="server" BehaviorID="txtRazonSocial_TextBoxWatermarkExtender" TargetControlID="txtRazonSocial" WatermarkCssClass="water input-large" WatermarkText="Razón Social" />
                        <asp:RequiredFieldValidator ID="rfvRazSoc" runat="server" ErrorMessage="Debe indicar Razon Social del proveedor" ValidationGroup="agrega" ControlToValidate="txtRazonSocial" Text="*" CssClass="errores"></asp:RequiredFieldValidator>
                    </div>
                </div>

                <div class="row marTop">
                    <div class="col-lg-1 col-sm-1 text-left">
                        <asp:Label ID="lblnombre" runat="server" Text="Nombre:" Visible="false"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="input-large" MaxLength="80" Visible="false"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender13" runat="server" BehaviorID="txtNombre_TextBoxWatermarkExtender" TargetControlID="txtNombre" WatermarkCssClass="water input-large" WatermarkText="Nombre" />
                        <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ErrorMessage="Debe indicar Nombre del proveedor" Text="*" ValidationGroup="agrega" ControlToValidate="txtNombre" CssClass="errores" Enabled="false"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left">
                        <asp:Label ID="lblAP" runat="server" Text="A. Paterno:" Visible="false"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtAP" runat="server" CssClass="input-large" MaxLength="40" Visible="false"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender14" runat="server" BehaviorID="txtAP_TextBoxWatermarkExtender" TargetControlID="txtAP" WatermarkCssClass="water input-large" WatermarkText="Apellido P." />
                        <asp:RequiredFieldValidator ID="rfvApPat" runat="server" ErrorMessage="Debe indicar Apellido Paterno" ValidationGroup="agrega" ControlToValidate="txtAP" Text="*" CssClass="errores" Enabled="false"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left">
                        <asp:Label ID="lblAM" runat="server" Text="A. Materno:" Visible="false"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtAM" runat="server" CssClass="input-large" MaxLength="40" Visible="false"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender15" runat="server" BehaviorID="txtAM_TextBoxWatermarkExtender" TargetControlID="txtAM" WatermarkCssClass="water input-large" WatermarkText="Apellido M." Enabled="false" />
                    </div>
                </div>

                <div class="row marTop">
                    <div class="col-lg-1 col-sm-1 text-left">
                        <asp:Label ID="LblCalle" runat="server" Text="Calle:"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtCalle" runat="server" CssClass="input-large" MaxLength="150"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" BehaviorID="txtCalle_TextBoxWatermarkExtender" TargetControlID="txtCalle" WatermarkCssClass="water input-large" WatermarkText="Calle" />
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left">
                        <asp:Label ID="Label4" runat="server" Text="N° Exterior:"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="TxtNE" runat="server" CssClass="input-medium" MaxLength="10"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" BehaviorID="txtNE_TextBoxWatermarkExtender" TargetControlID="txtNE" WatermarkCssClass="water input-medium" WatermarkText="Numero Exterior" />
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left">
                        <asp:Label ID="Label5" runat="server" Text="N° Interior:"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtNI" runat="server" CssClass="input-medium" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" BehaviorID="txtNI_TextBoxWatermarkExtender" TargetControlID="txtNI" WatermarkCssClass="water input-medium" WatermarkText="Interior" />
                    </div>
                </div>

                <div class="row marTop">
                    <div class="col-lg-1 col-sm-1 text-left">
                        <asp:Label ID="Label6" runat="server" Text="Colonia:"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtCol" runat="server" CssClass="input-large" MaxLength="40"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txwColo" runat="server" TargetControlID="txtCol" WatermarkCssClass="water input-medium" WatermarkText="Colonia" />
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left">
                        <asp:Label ID="Label7" runat="server" Text="Codigo Postal:"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtCP" runat="server" CssClass="input-medium" MaxLength="5" Size="5"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender6" runat="server" BehaviorID="txtCP_TextBoxWatermarkExtender" TargetControlID="txtCP" WatermarkCssClass="water input-medium" WatermarkText="C.P." />
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left">
                        <asp:Label ID="Label8" runat="server" Text="Ciudad:"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtCiudad" runat="server" CssClass="input-large" MaxLength="40"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender7" runat="server" BehaviorID="txtCiudad_TextBoxWatermarkExtender" TargetControlID="txtCiudad" WatermarkCssClass="water input-large" WatermarkText="Ciudad" />
                    </div>
                </div>

                <div class="row marTop">
                    <div class="col-lg-1 col-sm-1 text-left">
                        <asp:Label ID="Label9" runat="server" Text="Estado:"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtEstado" runat="server" CssClass="input-large" MaxLength="20"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender8" runat="server" BehaviorID="txtEstado_TextBoxWatermarkExtender" TargetControlID="txtEstado" WatermarkCssClass="water input-large" WatermarkText="Estado" />
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left">
                        <asp:Label ID="Label10" runat="server" Text="Pais:"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtPais" runat="server" CssClass="input-large" MaxLength="40" Text="MÉXICO"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender9" runat="server" BehaviorID="txtPais_TextBoxWatermarkExtender" TargetControlID="txtPais" WatermarkCssClass="water input-large" WatermarkText="Pais" />
                    </div>
                    <div class="col-lg-1 col-sm-1 text-left">
                        <asp:Label ID="Label11" runat="server" Text="Correo:"></asp:Label></div>
                    <div class="col-lg-3 col-sm-3 text-left">
                        <asp:TextBox ID="txtCorreo" runat="server" CssClass="input-large" MaxLength="150"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender10" runat="server" BehaviorID="txtCorreo_TextBoxWatermarkExtender" TargetControlID="txtCorreo" WatermarkCssClass="water input-large" WatermarkText="Correo" />
                    </div>
                </div>
                <div class="row marTop">

            <div class="col-lg-1 col-sm-1 text-left">
                <asp:Label ID="Label12" runat="server" Text="Telefono:"></asp:Label></div>
            <div class="col-lg-3 col-sm-3 text-left">
                <asp:TextBox ID="txtTel" runat="server" CssClass="input-large" MaxLength="30"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender11" runat="server" BehaviorID="txtTel_TextBoxWatermarkExtender" TargetControlID="txtTel" WatermarkCssClass="water input-large" WatermarkText="Telefono" />
            </div>
            <div class="col-lg-1 col-sm-1 text-left">
                <asp:Label ID="Label13" runat="server" Text="Celular:"></asp:Label></div>
            <div class="col-lg-3 col-sm-3 text-left">
                <asp:TextBox ID="txtCel" runat="server" CssClass="input-large" MaxLength="30"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender12" runat="server" BehaviorID="txtCel_TextBoxWatermarkExtender" TargetControlID="txtCel" WatermarkCssClass="water input-large" WatermarkText="Celular" />
            </div>
            <div class="col-lg-1 col-sm-1 text-left">
                <asp:Label ID="lblcontact" runat="server" Text="Contacto:"></asp:Label></div>
            <div class="col-lg-3 col-sm-3 text-left">
                <asp:TextBox ID="txtcontact" runat="server" CssClass="input-large" MaxLength="30"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender16" runat="server" BehaviorID="txtcontact_TextBoxWatermarkExtender" TargetControlID="txtcontact" WatermarkCssClass="water input-large" WatermarkText="Contacto" />
            </div>
        </div>
        <div class="row marTop">
                    <div class="col-lg-2 col-sm-3 text-center">
                        <asp:Button ID="btnAgrAct" runat="server" Text="Agregar" CssClass="btn-info" ValidationGroup="agrega" OnClick="btnAgrAct_Click" CommandName="Inserta" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn-info" CausesValidation="false" OnClick="btnCancelar_Click" Visible="false" />
                    </div>
                    <div class="row">
                        <div class="col-lg-12 col-sm-12 center">
                            <div class="table-responsive">
                              <div class="row">
                                    <div class="col-lg-12 col-sm-12 alert-danger negritas center">
                                        <asp:Label ID="lblErrores" runat="server" CssClass="errores"></asp:Label>
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="agrega" />
                                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="edita" />
                                    </div>
                                </div>
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" EnablePersistedSelection="true"
                                    CssClass="table table-bordered center" EmptyDataText="No existen Proveedores Registrados"
                                    DataKeyNames="clave" DataSourceID="SqlDSProvs" AllowPaging="True" PageSize="7"
                                    AllowSorting="True" OnRowCommand="GridView1_RowCommand" OnRowUpdating="GridView1_RowUpdating"
                                    OnRowDataBound="GridView1_RowDataBound" >
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" SortExpression="clave">
                                            <EditItemTemplate>
                                                <asp:Label ID="lblIDMod" runat="server" Text='<%# Eval("clave") %>'></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Text='<%# Bind("clave") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="ancho5px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="R.F.C." SortExpression="RFC">
                                            <EditItemTemplate>
                                                <asp:RequiredFieldValidator ID="RFVRFC" runat="server" ErrorMessage="Debe indicar el R.F.C." ValidationGroup="edita" ControlToValidate="txtRFCMod" Text="*" CssClass="errores"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtRFCMod" runat="server" CssClass="input-medium" MaxLength="13" Text='<%# Bind("RFC") %>' Size="13" ToolTip="R.F.C."></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtRFCMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtRFCMod_TextBoxWatermarkExtender" TargetControlID="txtRFCMod" WatermarkCssClass="water input-medium" WatermarkText="R.F.C." />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRFC" runat="server" Text='<%# Bind("RFC") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="ancho50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Razon Social" SortExpression="razonSocial">
                                            <EditItemTemplate>
                                                <asp:RadioButtonList ID="rbtTipoPer" runat="server" OnSelectedIndexChanged="rbtTipoPer_SelectedIndexChanged" ValidationGroup="edita"
                                                    AutoPostBack="true" ToolTip="Tipo Persona" CellSpacing="20" RepeatDirection="Vertical">
                                                    <asp:ListItem Text="Fisica" Value="F"></asp:ListItem>
                                                    <asp:ListItem Text="Moral" Value="M"></asp:ListItem>
                                                </asp:RadioButtonList>
                                                <asp:TextBox ID="txtModRazon" runat="server" Text='<%# Bind("razonSocial") %>' CssClass="input-small" MaxLength="250" ToolTip="Razón Social" ></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtModRazonWatermarkExtender17" runat="server" BehaviorID="txtModRazon_TextBoxWatermarkExtender" TargetControlID="txtModRazon" WatermarkText="Razon Social" WatermarkCssClass="water input-small" />
                                                <asp:RequiredFieldValidator ID="RRVRazon" runat="server" ErrorMessage="Debe indicar Razon Social" ValidationGroup="edita" ControlToValidate="txtModRazon" Text="*" CssClass="errores"></asp:RequiredFieldValidator>

                                                <asp:TextBox ID="txtNomMod" runat="server" Text='<%# Bind("nombres") %>' CssClass="input-small btnPad" MaxLength="80" ToolTip="Nombre(s)"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtNomMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtNomMod_TextBoxWatermarkExtender" TargetControlID="txtNomMod" WatermarkText="Nombre" WatermarkCssClass="water input-small" />

                                                <asp:TextBox ID="txtApPatMod" runat="server" Text='<%# Bind("apellidoPaterno") %>' CssClass="input-small btnPad" MaxLength="40" ToolTip="Apellido Paterno"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtApPatMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtApPatMod_TextBoxWatermarkExtender" TargetControlID="txtApPatMod" WatermarkText="Ap. Paterno" WatermarkCssClass="water input-small btnPad" />
                                                
                                                <asp:TextBox ID="txtEdtApMat" runat="server" Text='<%# Bind("apellidoMaterno") %>' CssClass="input-small btnPad" MaxLength="40" ToolTip="Apellido Materno"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server" TargetControlID="txtEdtApMat" WatermarkText="Ap. Materno" WatermarkCssClass="water input-small btnPad" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRazSoc" runat="server" Text='<%# (Eval("personaFiscal").ToString() == "M") ? DataBinder.Eval(Container.DataItem, "razonSocial") : DataBinder.Eval(Container.DataItem, "nombreCompleto") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="ancho150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Domicilio" SortExpression="Domicilio">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCalleMod" runat="server" Text='<%# Bind("calle") %>' CssClass="input-small btnPad" MaxLength="100" ToolTip="Calle"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtcalleModWatermarkExtender17" runat="server" BehaviorID="txtcalleMod_TextBoxWatermarkExtender" TargetControlID="txtCalleMod" WatermarkText="Calle" WatermarkCssClass="water input-small" />
                                                <asp:TextBox ID="txtnEMod" runat="server" Text='<%# Bind("numExt") %>' CssClass="input-small btnPad" MaxLength="10" ToolTip="Número Exterior"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtnEMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtnEMod_TextBoxWatermarkExtender" TargetControlID="txtnEMod" WatermarkText="Exterior" WatermarkCssClass="water input-small" />
                                                <asp:TextBox ID="txtNumInt" runat="server" Text='<%# Bind("numInt") %>' CssClass="input-small btnPad" MaxLength="20" ToolTip="Número Interior"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtnIMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtnIMod_TextBoxWatermarkExtender" TargetControlID="txtNumInt" WatermarkText="Interior" WatermarkCssClass="water input-small" />
                                                <asp:TextBox ID="txtEdtCol" runat="server" Text='<%# Bind("colonia") %>' CssClass="input-small btnPad" MaxLength="40" ToolTip="Colonia"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txwColonia" runat="server" BehaviorID="txwColonia" TargetControlID="txtEdtCol" WatermarkText="Colonia" WatermarkCssClass="water input-small" />
                                                <asp:TextBox ID="txtEdtCd" runat="server" Text='<%# Bind("ciudad") %>' CssClass="input-small btnPad" MaxLength="40" ToolTip="Ciudad"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txwCd" runat="server" BehaviorID="txwCd" TargetControlID="txtEdtCd" WatermarkText="Ciudad" WatermarkCssClass="water input-small" />
                                                <asp:TextBox ID="txtEdtEdo" runat="server" Text='<%# Bind("estado") %>' CssClass="input-small btnPad" MaxLength="40" ToolTip="Estado"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txwEstado" runat="server" BehaviorID="txwEstado" TargetControlID="txtEdtEdo" WatermarkText="Estado" WatermarkCssClass="water input-small" />
                                                <asp:TextBox ID="txtEdtPais" runat="server" Text='<%# Bind("pais") %>' CssClass="input-small btnPad" MaxLength="40" ToolTip="País"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txwPais" runat="server" BehaviorID="txwPais" TargetControlID="txtEdtPais" WatermarkText="País" WatermarkCssClass="water input-small" />
                                                <br />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDirecc" runat="server" Text='<%# Bind("Domicilio") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="ancho150px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Correo" SortExpression="email">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtmailMod" runat="server" CssClass="input-medium" MaxLength="250" Text='<%# Bind("email") %>' ToolTip="Correo"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtmailMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtmailMod_TextBoxWatermarkExtender" TargetControlID="txtmailMod" WatermarkCssClass="water input-medium" WatermarkText="Correo" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("email") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="ancho50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Telefono" SortExpression="TelefonoPart">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txttelMod" runat="server" CssClass="input-medium" MaxLength="250" Text='<%# Bind("telefonoPart") %>' ToolTip="Teléfono Particular"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txttelMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txttelMod_TextBoxWatermarkExtender" TargetControlID="txttelMod" WatermarkCssClass="water input-medium" WatermarkText="Telefono" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbltel" runat="server" Text='<%# Bind("telefonoPart") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="ancho50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Celular" SortExpression="telefonoCel">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtcelMod" runat="server" CssClass="input-medium" MaxLength="250" Text='<%# Bind("telefonoCel") %>' ToolTip="Teléfono Celular"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtcelMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtcelMod_TextBoxWatermarkExtender" TargetControlID="txtcelMod" WatermarkCssClass="water input-medium" WatermarkText="Celular" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblcel" runat="server" Text='<%# Bind("telefonoCel") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="ancho50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Contacto" SortExpression="contacto">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtcontMod" runat="server" CssClass="input-medium" MaxLength="250" Text='<%# Bind("contacto") %>' ToolTip="Contacto"></asp:TextBox>
                                                <cc1:TextBoxWatermarkExtender ID="txtcontMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtcontMod_TextBoxWatermarkExtender" TargetControlID="txtcontMod" WatermarkCssClass="water input-medium" WatermarkText="Contacto" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblcont" runat="server" Text='<%# Bind("contacto") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="ancho50px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false">
                                            <EditItemTemplate></EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Button runat="server" ID="btnEditar" CommandName="EditarProv" CommandArgument='<%# Eval("clave") %>' CssClass="btn-warning btnAlign" CausesValidation="false"
                                                    Text="Editar" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <EditItemTemplate></EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Button ID="btnEliminar" runat="server" CausesValidation="False" CommandName="Elimina/Inactiva" CssClass="btn-danger btnAlign"
                                                    OnClientClick='<%# String.Format("return msjEstatus(\"{0}\",\"{1}\",\"{2}\");", Eval("razonSocial"), Eval("estatus"), Eval("tieneEntradas")) %>'
                                                    Text="Inactiva/Reactiva" CommandArgument='<%# Eval("clave")+";"+Eval("estatus") %>'  />
                                            </ItemTemplate>
                                            <ItemStyle CssClass="ancho50px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle ForeColor="Red" />
                                </asp:GridView>
                              </div>
                            </div>
                        </div>
                    </div>
        </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowCommand" />
                <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="DataBound" />
                <asp:AsyncPostBackTrigger ControlID="GridView1" EventName="RowDeleted" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
        <asp:SqlDataSource ID="SqlDSProvs" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
        SelectCommand="SELECT [clave], [RFC], [razonSocial], nombres + ' ' + ISNULL(apellidoPaterno, '') + ' ' + ISNULL(apellidoMaterno, '') AS nombreCompleto, 
        [calle], [numExt], [numInt], (calle + ' ' + numExt + '-' + ISNULL(numInt, '') + ', ' + colonia + ', ' + ISNULL(ciudad, '') + ', ' + ISNULL(estado, '') + ', ' + ISNULL(pais, '')) AS Domicilio, 
        pais, [estado], [ciudad], [colonia], [telefonoPart], [telefonoCel], [codigoPostal], [apellidoPaterno], [apellidoMaterno], [nombres], [email], [contacto], [personaFiscal], nombreComercial, [estatus], (SELECT COUNT(*) FROM entinventarioenc WHERE claveProveedor=clienteproveedor.clave) AS tieneEntradas FROM [clienteproveedor]" 
        InsertCommand="INSERT INTO [clienteproveedor] ([RFC], [razonSocial], pais, [estado], [ciudad], [calle], [numExt], [numInt], [colonia], [telefonoPart], [telefonoCel], [codigoPostal], [apellidoPaterno], [apellidoMaterno], [nombres], [email], [contacto], personaFiscal, Tipo, nombreComercial) VALUES (@RFC, @razonSocial, @pais, @estado, @ciudad, @calle, @numExt, @numInt, @colonia, @telefonoPart, @telefonoCel, @codigoPostal, @apellidoPaterno, @apellidoMaterno, @nombres, @email, @contacto, @personaFiscal, @Tipo, @nombreComercial)" 
        UpdateCommand="UPDATE [clienteproveedor] SET [RFC] = @RFC, [razonSocial] = @razonSocial, pais = @pais, [calle] = @calle, [numExt] = @numExt, [numInt] = @numInt, [colonia] = @colonia, [telefonoPart] = @telefonoPart, [telefonoCel] = @telefonoCel, [apellidoPaterno] = @apellidoPaterno, [apellidoMaterno] = @apellidoMaterno, [nombres] = @nombres, [email] = @email, [contacto] = @contacto, personaFiscal = @personaFiscal, nombreComercial = @nombreComercial WHERE [clave] = @clave">
        <InsertParameters>
            <asp:Parameter Name="RFC" Type="String" />
            <asp:Parameter Name="razonSocial" Type="String" />
            <asp:Parameter Name="pais" Type="String" DefaultValue="MÉXICO" />
            <asp:Parameter Name="estado" Type="String" />
            <asp:Parameter Name="ciudad" Type="String" />
            <asp:Parameter Name="calle" Type="String" />
            <asp:Parameter Name="numExt" Type="String" />
            <asp:Parameter Name="numInt" Type="String" />
            <asp:Parameter Name="colonia" Type="String" />
            <asp:Parameter Name="telefonoPart" Type="String" />
            <asp:Parameter Name="telefonoCel" Type="String" />
            <asp:Parameter Name="codigoPostal" Type="String" />
            <asp:Parameter Name="apellidoPaterno" Type="String" />
            <asp:Parameter Name="apellidoMaterno" Type="String" />
            <asp:Parameter Name="nombres" Type="String" />
            <asp:Parameter Name="email" Type="String" />
            <asp:Parameter Name="contacto" Type="String" />
            <asp:Parameter Name="personaFiscal" Type="String" />
            <asp:Parameter Name="Tipo" Type="String" DefaultValue="P" />
            <asp:Parameter Name="nombreComercial" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="RFC" Type="String" />
            <asp:Parameter Name="razonSocial" Type="String" />
            <asp:Parameter Name="pais" Type="String" />
            <asp:Parameter Name="estado" Type="String" />
            <asp:Parameter Name="ciudad" Type="String" />
            <asp:Parameter Name="calle" Type="String" />
            <asp:Parameter Name="numExt" Type="String" />
            <asp:Parameter Name="numInt" Type="String" />
            <asp:Parameter Name="colonia" Type="String" />
            <asp:Parameter Name="telefonoPart" Type="String" />
            <asp:Parameter Name="telefonoCel" Type="String" />
            <asp:Parameter Name="codigoPostal" Type="String" />
            <asp:Parameter Name="apellidoPaterno" Type="String" />
            <asp:Parameter Name="apellidoMaterno" Type="String" />
            <asp:Parameter Name="nombres" Type="String" />
            <asp:Parameter Name="email" Type="String" />
            <asp:Parameter Name="contacto" Type="String" />
            <asp:Parameter Name="clave" Type="Int32" />
            <asp:Parameter Name="personaFiscal" Type="String" />
            <asp:Parameter Name="nombreComercial" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>
