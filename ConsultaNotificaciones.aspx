<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConsultaNotificaciones.aspx.cs" Inherits="ConsultaNotificaciones" MasterPageFile="~/Administracion.master" Culture="es-Mx" UICulture="es-Mx" %>

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
               <i class="icon-bell"></i>&nbsp;&nbsp;<asp:Label runat="server" ID="lblTitulo" Text="Notificaciones" CssClass="alert-success"></asp:Label>&nbsp;&nbsp;<i class="icon-bell"></i>
            </div>
        </div>
        <br />
         <div class="row center centrado " >
             <asp:UpdatePanel ID="UpdatePanel1" runat="server">
             <ContentTemplate>                         
                <div class="col-lg-12 col-sm-12 center">
                    <div class="row">
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="Label1" runat="server" Text="Fecha:"></asp:Label>
                            <asp:TextBox ID="txtFechaIni" runat="server" CssClass="input-medium" MaxLength="10" Enabled="false" ></asp:TextBox>
                            <cc1:CalendarExtender ID="txtFechaIni_CalendarExtender" runat="server" PopupButtonID="lnkFini" BehaviorID="txtFechaIni_CalendarExtender" TargetControlID="txtFechaIni" Format="yyyy-MM-dd"></cc1:CalendarExtender>
                            <asp:LinkButton ID="lnkFini" runat="server" CssClass="btn btn-info"><i class="icon-calendar"></i></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnConsultar" runat="server" CssClass="btn btn-info" 
                                Text="Consultar" onclick="btnConsultar_Click" />
                        </div>                        
                    </div>                    
                    <div class="row">
                        <div class="col-lg-12 col-sm-12 center alert-danger">
                            <asp:Label ID="lblError" runat="server" CssClass="errores negritas"></asp:Label>
                        </div>
                    </div>               
                </div>                  
                
                <div class="ancho95 centrado center">
                    <div class="table-responsive">
                        <asp:GridView ID="GridView1" runat="server" 
                            EmptyDataRowStyle-CssClass="errores" EmptyDataText="No existen notificaciones registradas"
                            CssClass="table table-bordered center" AutoGenerateColumns="False"   
                            AllowPaging="True" AllowSorting="True" DataKeyNames="id_notificacion"
                            DataSourceID="SqlDataSource1" onrowdatabound="GridView1_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="id_notificacion" HeaderText="id_notificacion" SortExpression="id_notificacion" Visible="false" />
                                <asp:BoundField DataField="hora" HeaderText="hora" SortExpression="hora" ReadOnly="True" />
                                <asp:BoundField DataField="notificacion" HeaderText="notificacion" SortExpression="notificacion" />                                
                                <asp:BoundField DataField="estatus" HeaderText="estatus" SortExpression="estatus" Visible="false" />
                                <asp:BoundField DataField="clase" HeaderText="clase" SortExpression="clase" ReadOnly="True" Visible="false" />
                                <asp:BoundField DataField="usuario" HeaderText="usuario" SortExpression="usuario" Visible="false" />
                                <asp:BoundField DataField="nombreUsuario" HeaderText="nombreUsuario" SortExpression="nombreUsuario" ReadOnly="True" />                                                                                                
                                <asp:TemplateField HeaderText="Marcar como Leído">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkLeido" runat="server" oncheckedchanged="chkLeido_CheckedChanged" AutoPostBack="true" ToolTip='<%# Eval("id_notificacion") %>'  />                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                            </Columns>
                            <SelectedRowStyle CssClass="alert-success-org" />
                            <EmptyDataRowStyle CssClass="errores" />
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select n.id_notificacion,convert(char(10),n.hora,108) as hora,n.notificacion,n.clasificacion,n.estatus,
case n.clasificacion when 1 then 'icon-usd' when 2 then 'icon-file-new' when 3 then 'icon-graph' when 4 then 'icon-signin' else '' end clase,n.usuario,(u.nombre+' '+u.apellido_pat+' '+isnull(u.apellido_mat,'')) as nombreUsuario
from notificaciones_PV n 
left join usuarios_PV u on u.usuario=n.usuario
where n.fecha=@fecha order by hora desc ">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtFechaIni" Name="fecha" PropertyName="Text" />
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