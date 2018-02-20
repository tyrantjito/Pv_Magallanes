<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VentaPublico.aspx.cs" Inherits="VentaPublico" MasterPageFile="~/Administracion.master" Culture="es-Mx" UICulture="es-Mx" %>
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
               <i class="icon-shopping-cart"></i>&nbsp;&nbsp;<asp:Label runat="server" ID="lblTitulo" Text="Venta Público" CssClass="alert-success"></asp:Label>&nbsp;&nbsp;<i class="icon-map-marker"></i>
            </div>
        </div>
        <br />
        <div class="row center centrado " >
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
             <ContentTemplate>                          
                <div class="col-lg-12 col-sm-12 center">
                    <div class="row">                                              
                        <div class="col-lg-12 col-sm-12 text-center">
                             <asp:Label ID="Label7" runat="server" Text="Tienda:"></asp:Label>
                            <asp:DropDownList ID="ddlIslas" runat="server" DataSourceID="SqlDataSource2" CssClass="input-medium" DataTextField="nombre" DataValueField="id_punto"></asp:DropDownList>
                             <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                                 ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                 SelectCommand="select 0 as id_punto, 'Seleccione Tienda' as nombre union all select p.id_punto,isnull(c.nombre,'') as nombre from usuario_puntoventa p left join catalmacenes c on c.idAlmacen=p.id_punto where p.usuario=@usuario order by 1">
                                 <SelectParameters>
                                     <asp:QueryStringParameter QueryStringField="u" Name="usuario" />
                                 </SelectParameters>
                             </asp:SqlDataSource>
                             <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="btn btn-info" ValidationGroup="busca" onclick="btnBuscar_Click"  />
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

