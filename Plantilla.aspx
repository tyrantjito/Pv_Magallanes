<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Plantilla.aspx.cs" Inherits="Plantilla" Culture="es-Mx" UICulture="es-Mx" MasterPageFile="~/Administracion.master" %>

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
               <i class="icon-edit"></i><asp:Label runat="server" ID="lblTitulo" Text="Plantilla Ticket" CssClass="alert-success"></asp:Label><i class="icon-shopping-cart"></i>
            </div>
        </div>
        <br />
         <div class="row center centrado " >
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
             <ContentTemplate>                         
                <div class="col-lg-12 col-sm-12 center">
                    <div class="row">
                        <div class="col-lg-5 col-sm-5 text-center">
                            <asp:Label ID="Label11" runat="server" Text="Encabezado:"></asp:Label>
                            <asp:TextBox ID="txtEncabezado" runat="server" MaxLength="3000" Rows="5" placeholder="Encabezado" TextMode="MultiLine" CssClass="form-control textNota" ></asp:TextBox>
                        </div>                                            
                        <div class="col-lg-5 col-sm-5 text-center">
                            <asp:Label ID="Label1" runat="server" Text="Notas:"></asp:Label>
                            <asp:TextBox ID="txtNotas" runat="server" MaxLength="3000" Rows="5" placeholder="Notas" TextMode="MultiLine" CssClass="form-control textNota" ></asp:TextBox>
                        </div>
                        <div class="col-lg-2 col-sm-2 text-center">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Cambios" 
                                CssClass="btn btn-success" ToolTip="Guardar Cambios" 
                                onclick="btnGuardar_Click" />
                        </div>                        
                    </div>                  
                    <div class="row">
                        <div class="col-lg-12 col-sm-12 center alert-danger">
                            <asp:Label ID="lblError" runat="server" CssClass="errores negritas"></asp:Label>
                        </div>
                    </div>               
                </div>                  
             </ContentTemplate>
             </asp:UpdatePanel>   
                
        </div>
        
    </div>   
    </center>  
</asp:Content>
