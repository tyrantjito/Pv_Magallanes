<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CatEmpresa.aspx.cs" Inherits="CatEmpresa" MasterPageFile="~/Administracion.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <div class="ancho95 centrado">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
               <i class="icon-road"></i> <asp:Label runat="server" ID="lblTitulo" Text="Empresa" CssClass="alert-success"></asp:Label>
            </div>
        </div>
        <br />        
        <div class="row">
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label1" runat="server" Text="R.F.C:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">                    
                <asp:TextBox ID="txtRfc" runat="server" CssClass="input-medium" MaxLength="13"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="txtRfc_TextBoxWatermarkExtender" runat="server" BehaviorID="txtRfc_TextBoxWatermarkExtender" TargetControlID="txtRfc" WatermarkCssClass="water input-medium" WatermarkText="R.F.C." />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el R.F.C." Text="*" ValidationGroup="agrega" ControlToValidate="txtRfc" CssClass="errores"></asp:RequiredFieldValidator>                    
            </div>
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label2" runat="server" Text="Razón Social:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">                    
                <asp:TextBox ID="txtRazon" runat="server" CssClass="input-large" MaxLength="200"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="txtRazon_TextBoxWatermarkExtender" runat="server" BehaviorID="txtRazon_TextBoxWatermarkExtender" TargetControlID="txtRazon" WatermarkCssClass="water input-large" WatermarkText="Razón Social" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar la razón social" ValidationGroup="agrega" ControlToValidate="txtRazon" Text="*" CssClass="errores"></asp:RequiredFieldValidator>                    
            </div>
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label13" runat="server" Text="Servicio de Correo:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">                    
                <asp:DropDownList ID="ddlTipo" runat="server">
                    <asp:ListItem Value="" Selected="True">Seleccione Servicio</asp:ListItem>
                    <asp:ListItem>SMTP</asp:ListItem>
                    <asp:ListItem>POP3</asp:ListItem>
                    <asp:ListItem>IMAP</asp:ListItem>
                </asp:DropDownList>
            </div> 
        </div>
        <div class="row marTop">
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label3" runat="server" Text="Servidor Entrante:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">
                <asp:TextBox ID="txtEntrante" runat="server" CssClass="input-large" MaxLength="200"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="txtEntranteWatermarkExtender1" runat="server" BehaviorID="txtEntrante_TextBoxWatermarkExtender" TargetControlID="txtEntrante" WatermarkCssClass="water input-large" WatermarkText="Servidor Entrante" />
            </div>
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label4" runat="server" Text="Servidor Saliente:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">                    
                <asp:TextBox ID="txtSaliente" runat="server" CssClass="input-large" MaxLength="200"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="txtSalienteWatermarkExtender1" runat="server" BehaviorID="txtSaliente_TextBoxWatermarkExtender" TargetControlID="txtSaliente" WatermarkCssClass="water input-large" WatermarkText="Servidor Saliente" />
            </div>
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label5" runat="server" Text="Puerto:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">                    
                <asp:TextBox ID="txtPuerto" runat="server" CssClass="input-mini" MaxLength="4"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="txtPuerto_FilteredTextBoxExtender" runat="server" BehaviorID="txtPuerto_FilteredTextBoxExtender" TargetControlID="txtPuerto" FilterType="Numbers" />
                <cc1:TextBoxWatermarkExtender ID="txtPuertoWatermarkExtender1" runat="server" BehaviorID="txtPuerto_TextBoxWatermarkExtender" TargetControlID="txtPuerto" WatermarkCssClass="water input-mini" WatermarkText="Puerto" />
                &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="ckhSSL" runat="server" Text="   Seguridad SSL" />
            </div>
        </div>
        <div class="row marTop">
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label14" runat="server" Text="Usuario:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">
                <asp:TextBox ID="txtUsuario" runat="server" CssClass="input-large" MaxLength="200"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="txtUsuarioWatermarkExtender1" runat="server" BehaviorID="txtUsuario_TextBoxWatermarkExtender" TargetControlID="txtUsuario" WatermarkCssClass="water input-large" WatermarkText="Usuario" />
            </div>
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label16" runat="server" Text="Contraseña:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">                    
                <asp:TextBox ID="txtContraseña" runat="server" CssClass="input-large" MaxLength="200" placeholder="Contraseña" TextMode="Password"></asp:TextBox>                
            </div>
            <div class="col-lg-1 col-sm-2 text-center">
                    <asp:Button ID="btnActualizar" runat="server" Text="Guardar Cambios" 
                        CssClass="btn btn-success" ValidationGroup="agrega" 
                        onclick="btnActualizar_Click" />
            </div>
        </div>
         <div class="row">
            <div class="col-lg-12 col-sm-12 alert-danger negritas center" >
                <asp:Label ID="lblError" runat="server" CssClass="errores"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="agrega" />                
            </div>
        </div>         
    </div>     
</asp:Content>