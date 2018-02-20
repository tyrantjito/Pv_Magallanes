<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Acceso</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no"/>
	<meta name="apple-mobile-web-app-capable" content="yes"/> 
    <link rel="shortcut icon" type="image/x-icon" href="img/favicon.ico"/>

	<link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
	<link href="css/bootstrap-responsive.min.css" rel="stylesheet" type="text/css" />

	<link href="css/font-awesome.css" rel="stylesheet"/>
	<link href="http://fonts.googleapis.com/css?family=Open+Sans:400italic,600italic,400,600" rel="stylesheet"/>

	<link href="css/style.css" rel="stylesheet" type="text/css"/>
    <link href="css/clases.css" rel="stylesheet" type="text/css"/>
	<link href="css/pages/signin.css" rel="stylesheet" type="text/css"/>

</head>
<body>    
    <div class="navbar navbar-fixed-top">
        <div class="navbar-inner">
			<div class="container">
				<img src="img/icono.png" alt="E-PuntoVenta" class="icono" />
            </div> <!-- /container -->
		</div> <!-- /navbar-inner -->
	</div> <!-- /navbar -->
    <div class="account-container">
		<div class="content clearfix">
            <form id="form1" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <center>
					        <h1>ACCESO</h1>	
                            <a href="#"><img src="img/logo.png" alt="ACCOM" class="img-responsive imgLogoLogin"/></a>
					        <br/><br/>	
				        </center>
				        <div class="login-fields">
					        <p>Ingrese sus datos</p>
					        <div class="field">						
                                <asp:TextBox ID="username" runat="server" CssClass="login username-field alineados" MaxLength="15" placeholder="Usuario" AutoCompleteType="None" ></asp:TextBox>                        
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe Indicar el Usuario" CssClass="validaciones" ValidationGroup="ingreso" ControlToValidate="username"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe Indicar el Usuario" CssClass="validaciones" ValidationGroup="ingreso2" ControlToValidate="username"></asp:RequiredFieldValidator>
					        </div> <!-- /field -->
					
					        <div class="field">						
                                <asp:TextBox ID="password" runat="server" CssClass="login password-field" MaxLength="15" TextMode="Password" placeholder="Contraseña" ></asp:TextBox>						                        
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe Indicar la Contraseña" CssClass="validaciones" ValidationGroup="ingreso" ControlToValidate="password"></asp:RequiredFieldValidator>
					        </div> <!-- /password -->
					
				        </div> <!-- /login-fields -->
                        <center>                            
                            <asp:Label ID="lblError" runat="server" CssClass="validaciones"></asp:Label>
                            <asp:Button ID="btnCierreSesion" runat="server" Text="Cerrar Sesión" CssClass="btn  btn-lg" Visible="false" onclick="btnCierreSesion_Click" ValidationGroup="ingreso" />
                            <asp:Button ID="btnCierreCaja" runat="server" Text="Cerrar Caja" CssClass="btn  btn-lg" Visible="false" onclick="btnCierreCaja_Click" ValidationGroup="ingreso"/>
                        </center>
                        <asp:LinkButton ID="lnkOlvido" runat="server" class="pull-left" 
                            ValidationGroup="ingreso2" onclick="lnkOlvido_Click">Olvidaste tu contrase&ntilde;a</asp:LinkButton>                        
                        <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" CssClass="btn  btn-lg pull-right" ValidationGroup="ingreso" onclick="btnIngresar_Click" />
                        
                        
                
                        
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <asp:Panel ID="pnlMaskLoad" runat="server" CssClass="maskLoad"></asp:Panel>
                                <asp:Panel ID="pnlCargando" runat="server" CssClass="pnlPopUpLoad padding8px" >
                                    <asp:Image ID="imgLoad" runat="server" ImageUrl="~/img/loading.gif" Width="80%" />
                                </asp:Panel>
                            </ProgressTemplate>
                            
                        </asp:UpdateProgress>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlMask" runat="server" CssClass="mask" Visible="false"></asp:Panel>
                        <asp:Panel ID="pnlIslas" runat="server" CssClass="pnlPopUpIslas text-center padding8px" GroupingText="Seleccione Tienda" Visible="false">
                            <div class="pading8px">                                           
                                <div class="col-lg-3 col-sm-3 text-center">
                                    <asp:Label ID="Label9" runat="server" Text="Tienda:"></asp:Label>
                                    <asp:DropDownList ID="ddlIsla" runat="server" DataTextField="nombre" DataValueField="id_punto" ValidationGroup="valEncDoc" DataSourceID="SqlDataSourceIslas"></asp:DropDownList>
                                    <asp:SqlDataSource runat="server" ID="SqlDataSourceIslas" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select 0 as id_punto,'Seleccione Tienda' as nombre union all select u.id_punto,isnull(c.nombre,'Desconocida') as nombre  from usuario_puntoventa u left join catalmacenes c on c.idAlmacen=u.id_punto where u.usuario=@usuario">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="username" Name="usuario" PropertyName="Text" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div class="col-lg-4 col-sm-4 text-center alert-danger">
                                    <asp:Label ID="lblErrorIsla" runat="server" ></asp:Label>
                                </div>                   
                            </div>
                            <div class="pading8px text-center">                                
                                <div class="col-lg-4 col-sm-4 text-center">
                                    <asp:Button ID="btnSeleccionar" runat="server" Text="Seleccionar" CssClass="btn-success" OnClick="btnSeleccionar_Click" />
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn-danger" OnClick="btnCancelar_Click" />
                                </div>                                                             
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </form>
        </div>
    </div>
</body>
</html>
