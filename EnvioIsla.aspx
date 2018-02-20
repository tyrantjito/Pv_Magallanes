<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EnvioIsla.aspx.cs" Inherits="EnvioIsla" MasterPageFile="~/Administracion.master" Culture="es-Mx" UICulture="es-Mx" %>
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
               <i class="icon-file"></i>&nbsp;&nbsp;<asp:Label runat="server" ID="lblTitulo" Text="Envio a Tienda" CssClass="alert-success"></asp:Label>&nbsp;&nbsp;<i class="icon-map-marker"></i>
            </div>
        </div>
        <br />
        <div class="row center centrado " >
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
             <ContentTemplate>
                <div class="col-lg-1 col-sm-1 center"></div>          
                <div class="col-lg-9 col-sm-9 center">
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:Label ID="Label1" runat="server" Text="Fecha Incial:"></asp:Label>
                            <asp:TextBox ID="txtFechaIni" runat="server" CssClass="input-medium" MaxLength="10" Enabled="false"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" PopupButtonID="lnkFini"
                                BehaviorID="txtFechaIni_CalendarExtender" TargetControlID="txtFechaIni" Format="yyyy-MM-dd">
                            </cc1:CalendarExtender>
                            <asp:LinkButton ID="lnkFini" runat="server" CssClass="btn btn-info"><i class="icon-calendar"></i></asp:LinkButton>
                        </div>                        
                        <div class="col-lg-3 col-sm-3 text-left">
                             <asp:Label ID="Label7" runat="server" Text="Tienda:"></asp:Label>
                            <asp:DropDownList ID="ddlIslas" runat="server" DataSourceID="SqlDataSource2" CssClass="input-medium" DataTextField="nombre" DataValueField="idAlmacen"></asp:DropDownList>
                             <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="SELECT 0 AS idAlmacen,'Seleccione una Tienda' AS nombre union all select idAlmacen,nombre from catalmacenes where estatus='A' order by 1"></asp:SqlDataSource>
                        </div>
                        <div class="col-lg-2 col-sm-1 text-center">
                            <asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="btn btn-info" ValidationGroup="busca" onclick="btnBuscar_Click"  />
                        </div>
                        <div class="col-lg-1 col-sm-1 text-center">
                            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" CssClass="btn btn-info" OnClick="btnImprimir_Click"  />
                        </div>
                    </div>                    
                    <div class="row">
                        <div class="col-lg-10 col-sm-10 center alert-danger">
                            <asp:Label ID="lblError" runat="server" CssClass="errores negritas"></asp:Label>
                        </div>
                    </div>               
                </div>  
                <div class="col-lg-2 col-sm-2 center"></div>  
                <br /><br />
                <div class="ancho95 centrado center">
                    <div class="table-responsive">
                        <asp:GridView ID="GridView1" runat="server" 
                            EmptyDataRowStyle-CssClass="errores" EmptyDataText="No existen productos a enviar"
                            CssClass="table table-bordered center" AutoGenerateColumns="False"  
                            AllowPaging="True" AllowSorting="True" PageSize="7" 
                            DataSourceID="SqlDataSource1">
                            <Columns>
                                <asp:BoundField DataField="entProductoID" HeaderText="Clave" SortExpression="entProductoID" />
                                <asp:BoundField DataField="descripcion" HeaderText="Producto" SortExpression="descripcion" />
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" SortExpression="cantidad" ReadOnly="True" />
                            </Columns>
                            <SelectedRowStyle CssClass="alert-success-org" />
                            <EmptyDataRowStyle CssClass="errores" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select d.entProductoID,c.descripcion,isnull(sum(d.entProdCant),0) as cantidad
from entinventariodet d
left join catproductos c on c.idProducto=d.entProductoID
where d.entAlmacenID=@entAlmacenID and 
d.entFolioID in (select e.entFolioID from entinventarioenc e where e.entAlmacenID=d.entAlmacenID and CONVERT(CHAR(10),e.entFechaDoc,126)=@entFechaDoc)
group by d.entProductoID,c.descripcion">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlIslas" Name="entAlmacenID" PropertyName="SelectedValue" />
                                <asp:ControlParameter ControlID="txtFechaIni" Name="entFechaDoc" PropertyName="Text" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>                                             
             </ContentTemplate>
             </asp:UpdatePanel>   
        </div>          
    </div>   
    </center>  
</asp:Content>
