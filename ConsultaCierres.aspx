<%@ Page Title="" Language="C#" MasterPageFile="~/Administracion.master" AutoEventWireup="true" CodeFile="ConsultaCierres.aspx.cs" Inherits="ConsultaCierres" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <center>
        <div class="ancho95 centrado center">
            <div class="row">
                <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
                   <i class="icon-briefcase"></i> <asp:Label runat="server" ID="lblTitulo" Text="Cierres Diarios" CssClass="alert-success"></asp:Label>
                </div>
            </div>
            <div class="col-lg-12 col-sm-12">
                <div class="col-lg-3 col-sm-3">
                    <asp:Label ID="Label1" runat="server" Text="Inicio:"></asp:Label>
                    <asp:TextBox runat="server" ID="txtFechIni" CssClass="input-small" Enabled="false"/>
                    <cc1:CalendarExtender ID="calExtFechIni" runat="server" TargetControlID="txtFechIni" PopupButtonID="lnkFini" Format="yyyy-MM-dd" />
                    <asp:LinkButton ID="lnkFini" runat="server" CssClass="btn btn-info"><i class="icon-calendar"></i></asp:LinkButton>
                </div>
                <div class="col-lg-2 col-sm-3">
                    <asp:Label ID="Label2" runat="server" Text="Fin:"></asp:Label>
                    <asp:TextBox ID="txtFechaFin" runat="server" CssClass="input-small" MaxLength="10" Enabled="false"></asp:TextBox>                        
                    <cc1:CalendarExtender ID="txtFechaFin_CalendarExtender" runat="server" PopupButtonID="lnkFFin"
                        BehaviorID="txtFechaFin_CalendarExtender" TargetControlID="txtFechaFin" Format="yyyy-MM-dd">
                    </cc1:CalendarExtender>
                    <asp:LinkButton ID="lnkFFin" runat="server" CssClass="btn btn-info"><i class="icon-calendar"></i></asp:LinkButton>
                </div>
                <div class="col-lg-3 col-sm-3">
                    <asp:Label ID="Label7" runat="server" Text="Tienda:"></asp:Label>
                    <asp:DropDownList ID="ddlIslas" runat="server" CssClass="input-medium" AppendDataBoundItems="true"
                        DataSourceID="SqlDataSource2" DataTextField="nombre" DataValueField="idAlmacen" AutoPostBack="true">
                        <asp:ListItem Selected="True" Text="Todos" Value="T" />
                    </asp:DropDownList>
                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' SelectCommand="select u.id_punto as idAlmacen,p.nombre_pv as nombre from usuario_puntoventa U inner join punto_venta p on p.id_punto=u.id_punto where U.usuario=@usuario and U.estatus='A'">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="usuario" QueryStringField="u" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <div class="col-lg-3 col-sm-3">
                    <asp:Label ID="Label3" runat="server" Text="Usuarios:"></asp:Label>
                    <asp:CheckBoxList ID="chkListUsuarios" runat="server" RepeatColumns="3"
                        DataSourceID="SqlDataSource3" DataTextField="nombre" DataValueField="usuario">
                    </asp:CheckBoxList>
                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select upper(u.usuario) as usuario,upper(u.usuario+' ('+rtrim(ltrim(u.nombre))+' '+rtrim(ltrim(u.apellido_pat))+' '+rtrim(ltrim(u.apellido_mat))+')') as nombre from usuarios_PV u inner join usuario_puntoventa p on p.usuario=u.usuario and CAST(p.id_punto AS CHAR(10))=CAST(@tienda AS CHAR(10)) and u.perfil in(2) where u.usuario<>'Supervisor'">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="ddlIslas" Name="tienda" PropertyName="SelectedValue" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
                <div class="col-lg-1 col-sm-1"><asp:Button ID="btnBuscar" runat="server" Text="Consultar" CssClass="btn-info" ValidationGroup="busca" OnClick="btnBuscar_Click" /></div>
            </div>                    
            <div class="col-lg-12 col-sm-12 center alert-danger">
                <asp:Label ID="lblError" runat="server" CssClass="errores negritas"></asp:Label>
            </div>
            <div class="col-lg-12 col-sm-12 center marTop">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnBuscar" />
                        <asp:PostBackTrigger ControlID="GridCierre" />
                    </Triggers>
                    <ContentTemplate>
                        <div class="col-lg-12 col-sm-12 center marTop">
                        <asp:GridView ID="GridCierre" EmptyDataText="No hay cierres de caja para mostrar." runat="server" 
                            CssClass="table table-bordered center" AutoGenerateColumns="false" PageSize="5" AllowSorting="true"
                            AllowPaging="true" OnRowDataBound="GridCierre_RowDataBound" OnPageIndexChanging="GridCierre_PageIndexChanging" >
                            <Columns>
                                <asp:TemplateField HeaderText="No. Cierre" SortExpression="id_cierre" >
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lknCierre" OnClick="lknCierre_Click" CommandArgument='<%# Eval("datosComand") %>' runat="server" Text='<%# Eval("id_cierre") %>' CssClass="alert-info negritas" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fehca Cierre" SortExpression="cierre" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label42" runat="server" Text='<%# Eval("cierre") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tienda" SortExpression="nombre" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label43" runat="server" Text='<%# Eval("nombre") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Usuario" SortExpression="nombreU" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label44" runat="server" Text='<%# Eval("nombreU") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fondo" SortExpression="fondo" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label45" runat="server" Text='<%# Eval("fondo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Efectivo" SortExpression="efectivo" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label46" runat="server" Text='<%# Eval("efectivo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Debito" SortExpression="debito" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label47" runat="server" Text='<%# Eval("debito") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Credito" SortExpression="credito" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label48" runat="server" Text='<%# Eval("credito") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Recargas" SortExpression="recargas" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label488" runat="server" Text='<%# Eval("recargas") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pago Servicios" SortExpression="pagoServicios" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label499" runat="server" Text='<%# Eval("pagoServicios") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Gastos" SortExpression="gastos" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label49" runat="server" Text='<%# Eval("gastos") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cancelaciones" SortExpression="cancelaciones" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label491" runat="server" Text='<%# Eval("cancelaciones") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total" SortExpression="total" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label50" runat="server" Text='<%# Eval("total") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Venta Taller" SortExpression="ventaTaller" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label501" runat="server" Text='<%# Eval("ventaTaller") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Venta Crédito" SortExpression="ventaCredito" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label502" runat="server" Text='<%# Eval("ventaCredito") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataRowStyle CssClass="errores" />
                        </asp:GridView>
                    </div>
                <div class="col-lg-12 col-sm-12 center marTop alert-info">
                    <asp:Label ID="lblCierreSelect" runat="server" CssClass="negritas"/>
                </div>
                    <div class="col-lg-12 col-sm-12 center marTop"> 
                        <asp:GridView ID="GridDesglose" EmptyDataText="No hay desglose para mostrar." runat="server" 
                        CssClass="table table-bordered center" AutoGenerateColumns="false" AllowSorting="true"
                        PageSize="3" AllowPaging="true" OnPageIndexChanging="GridDesglose_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="Cajas Generadas" SortExpression="id_cajaS" >
                                <ItemTemplate>
                                    <asp:Label ID="lblArgumentos" runat="server" Text='<%# Eval("id_cajaS") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Usuario" SortExpression="nombre" >
                                <ItemTemplate>
                                    <asp:Label ID="labeld1" runat="server" Text='<%# Eval("nombre") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Efectivo" SortExpression="efectivo" >
                                <ItemTemplate>
                                    <asp:Label ID="labeld2" runat="server" Text='<%# Eval("efectivoS") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Credito" SortExpression="t_credito" >
                                <ItemTemplate>
                                    <asp:Label ID="labeld3" runat="server" Text='<%# Eval("t_creditoS") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Recargas" SortExpression="recargas" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label488" runat="server" Text='<%# Eval("recargas") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pago Servicios" SortExpression="pagoServicios" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label499" runat="server" Text='<%# Eval("pagoServicios") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            <asp:TemplateField HeaderText="Debito" SortExpression="t_debito" >
                                <ItemTemplate>
                                    <asp:Label ID="labeld4" runat="server" Text='<%# Eval("t_debitoS") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Gastos" SortExpression="t_gastos" >
                                <ItemTemplate>
                                    <asp:Label ID="labeld5" runat="server" Text='<%# Eval("t_gastosS") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cancelaciones" SortExpression="t_cancelacion" >
                                <ItemTemplate>
                                    <asp:Label ID="labeld5" runat="server" Text='<%# Eval("t_cancelacion") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Venta Taller" SortExpression="ventaTaller" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label5011" runat="server" Text='<%# Eval("ventaTaller") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Venta Crédito" SortExpression="ventaCredito" >
                                    <ItemTemplate>
                                        <asp:Label ID="Label5021" runat="server" Text='<%# Eval("ventaCredito") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total" SortExpression="total" >
                                <ItemTemplate>
                                    <asp:Label ID="labeld6" runat="server" Text='<%# Eval("total") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>                            
                        </Columns>
                        <EmptyDataRowStyle CssClass="errores" />
                    </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Button runat="server" ID="btnGenPDF" OnClick="btnGenPDF_Click" Text="Imprimir"/>
        </div>
        </div>
    </center>
</asp:Content>

