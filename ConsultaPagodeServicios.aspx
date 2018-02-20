<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConsultaPagodeServicios.aspx.cs" Inherits="ConsultaPagodeServicios" MasterPageFile="~/Administracion.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
    <div class="ancho95 text-center centrado">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
               <i class="icon-folder-open"></i> <asp:Label runat="server" ID="lblTitulo" Text="Consulta Pagos de Servicios" CssClass="alert-success"></asp:Label>
            </div>
        </div>
        <br />
        <div class="row center centrado " >
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                 <ContentTemplate>
                 <div class="col-lg-1 col-sm-1 center"></div> 
                     <div class="col-lg-3 col-sm-3 text-left">
                             <asp:Label ID="Label7" runat="server" Text="Tienda:"></asp:Label>
                            <asp:DropDownList ID="ddlIslas" runat="server" DataSourceID="SqlDataSource2" CssClass="input-medium" 
                                 DataTextField="nombre" DataValueField="idAlmacen"  
                                  ></asp:DropDownList>
                             <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="SELECT 0 AS idAlmacen,'Seleccione una Tienda' AS nombre union all select idAlmacen,nombre from catalmacenes where estatus='A' order by 1"></asp:SqlDataSource>
                        </div>



                 </ContentTemplate>
                 </asp:UpdatePanel>
            </div>
        
        </div>
        </center>
        </asp:Content>