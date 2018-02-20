<%@ Page Title="" Language="C#" MasterPageFile="~/PuntoVenta.master" AutoEventWireup="true" Culture="es-Mx" UICulture="es-Mx"
    CodeFile="PVenta.aspx.cs" Inherits="PVenta" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">
        function KeyPress() {
            if (window.event.keyCode == 13)
            //window.event.keyCode = 9;
                window.document.getElementById("btnValidar").click();
        }

        function desactivaMontoPagado(chkCredito) {
            var txtMontoPagado = $get('<%= txtMontoPagado.ClientID %>');$
            if(chkCredito.checked)
            {
                /*Desactiva montopagado*/
                txtMontoPagado.value = "0.00";
                txtMontoPagado.disabled = true;
            }
            else
            {
                txtMontoPagado.disabled = false;
            }
        }
        
        

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"/>    
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

    <div class="venta">
        <div class="row centrado">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
                <i class="icon-shopping-cart"></i>
                <asp:Label runat="server" ID="lblTitulo" Text="Venta" CssClass="alert-success"></asp:Label>
                <asp:Label runat="server" ID="lblNumTicket" Visible="false"></asp:Label>
            </div>
        </div>
        <%--<div class="row center">
            <div class="col-lg-12 col-sm-12 text-right">
                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                    <ContentTemplate>
                        <h2 class="text-success">
                            <asp:Label ID="Label13" runat="server" Text="Total:" Visible="false"></asp:Label>
                             asp:Label ID="lblTotal" runat="server" CssClass="text-right" Visible="false"></asp:Label>
                        </h2>
                        <div class="row text-right">
                            <asp:Button ID="btnAceptarVenta" runat="server" Text="Aceptar" Visible="false" CssClass="btn btn-icon btn-success alineados" OnClick="btnAceptarVenta_Click" />&nbsp;&nbsp;&nbsp;
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>--%>
            <div class="col-lg-12 col-sm-12 text-center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblError" runat="server" CssClass="validaciones"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="validaciones" DisplayMode="List" ValidationGroup="agregar" />
                        <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="validaciones" DisplayMode="List" ValidationGroup="editar" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            
            <div class="col-lg-6 col-sm-6">
                <div class="row">
                    <div class="col-lg-12 col-sm-12 center ">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="Label1" runat="server" Text="Producto:" CssClass="alineados middle"></asp:Label>
                                <telerik:RadNumericTextBox RenderMode="Classic" Width="100px" CssClass="input-small" runat="server" ID="txtCantidadNew" Value="1" EmptyMessage="El valor minimo es de 1" MinValue="1" ShowSpinButtons="true" NumberFormat-DecimalDigits="0" Skin="MetroTouch" />&nbsp;X&nbsp;
                                <asp:TextBox ID="txtProd" runat="server" MaxLength="15" CssClass="input-large alineados middle"
                                    AutoPostBack="True" OnTextChanged="txtProd_TextChanged"></asp:TextBox>
                                <asp:Button ID="btnValidar" runat="server" OnClick="btnValidar_Click" Style="display: none;" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-12 col-sm-12 center pad1em">
                        <asp:Panel runat="server" ID="pnlProd" CssClass="maxAlto500px" ScrollBars="auto">
                            <div class="table-responsive">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="grvVenta" runat="server"  CssClass="table table-bordered center textcentrado"
                                            AutoGenerateColumns="False" EmptyDataText="No existen Productos " AllowPaging="false"
                                            DataKeyNames="renglon">
                                            <Columns>
                                                <asp:TemplateField HeaderText="No." Visible="false">
                                                    <EditItemTemplate>
                                                        <asp:Label ID="lblIdArticulo" runat="server" Text='<%# Eval("renglon") %>'></asp:Label>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label10" runat="server" Text='<%# Bind("renglon") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Clave">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblClaveProd" runat="server" Text='<%# Eval("clave") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="producto" HeaderText="Producto" ReadOnly="true" />
                                                <asp:TemplateField HeaderText="Cantidad">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCant" runat="server" Text='<%# Bind("cantidad") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Precio">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPrecio" runat="server" Text='<%# Bind("precio") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Descuento">
                                                    <ItemTemplate>
                                                        <asp:TextBox runat="server" ID="txtDescto" MaxLength="5" Text='<%# Bind("porc_descuento") %>' Width="48" AutoPostBack="true" OnTextChanged="txtDescto_TextChanged" CssClass="input-large alineados middle" />
                                                        <span>%</span>
                                                        <cc1:FilteredTextBoxExtender runat="server" ID="filtTxtDescto" FilterType="Numbers,Custom" TargetControlID="txtDescto" FilterMode="ValidChars" ValidChars="." />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Importe">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblImp" runat="server" Text='<%# Bind("total") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnEliminar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("renglon") %>'
                                                            CommandName="Delete" ToolTip="Eliminar" CssClass="btn btn-danger alineados" OnClick="btnEliminar_Click"><i class="icon-trash"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
            <div class="col-lg-6 col-sm-6">
                 <telerik:RadTabStrip RenderMode="Lightweight" ID="radTabOpciones" runat="server"  SelectedIndex="4" MultiPageID="multiOpciones" Skin="MetroTouch" Align="Left" Orientation="HorizontalTop" CssClass="col-lg-12 col-sm-12">
                    <Tabs>                        
                        <telerik:RadTab Text="Pago"></telerik:RadTab>                        
                        <telerik:RadTab Text="Cliente"></telerik:RadTab>                       
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage runat="server" ID="multiOpciones" SelectedIndex="0" CssClass="col-lg-12 col-sm-12" SkinID="MetroTouch">
                <telerik:RadPageView runat="server" ID="detallePago" CssClass="text-left">
                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate>
                            <div class="row marTop text-center">
                                <div class="col-lg-8 col-sm-8 text-left">
                                    <asp:Label ID="Label5" runat="server" Text="Producto:&nbsp;" CssClass="alineados middle"></asp:Label>                             
                                    <telerik:RadAutoCompleteBox runat="server" RenderMode="Lightweight" ID="RadAutoCompleteBox"
                                        EmptyMessage="Producto" MaxResultCount="5" OnTextChanged="RadAutoCompleteBox1_TextChanged"
                                        DataSourceID="SqlDsProduct" DataTextField="descripcion" EnableAriaSupport="true" Skin="MetroTouch"
                                        AccessKey="W" InputType="Text" Width="300px" DropDownWidth="300px" DropDownHeight="200px">
                                    </telerik:RadAutoCompleteBox>
                                    <asp:SqlDataSource runat="server" ID="SqlDsProduct" ConnectionString="<%$ ConnectionStrings:PVW %>"
                                        ProviderName="System.Data.SqlClient" SelectCommand="select aa.idarticulo+' / '+c.descripcion as descripcion from articulosalmacen aa inner join catproductos c on c.idproducto=aa.idarticulo where aa.idalmacen=@almacen">
                                        <SelectParameters>
                                            <asp:QueryStringParameter Name="almacen" QueryStringField="p" DbType="Int32" DefaultValue="0" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                    <asp:Label ID="lblProducto" runat="server" CssClass="alineados middle margenLeft" Visible="false"></asp:Label>
                                    <asp:Label ID="lblPrecioVentaOriginal" runat="server" CssClass="alineados middle margenLeft" Visible="false"></asp:Label>
                                    <asp:Image ID="imgProducto" runat="server" GenerateEmptyAlternateText="True" Width="60" Height="60" Visible="false" />
                                     <div class="col-lg-8 col-sm-8 text-left">
                                          <%--<asp:Label ID="Label13" runat="server" CssClass="alineados middle margenLeft" Text=""></asp:Label>
                                     <asp:TextBox ID="txtProcentaje" runat="server" MaxLength="6" CssClass="input-small alineados middle text-right" placeholder="Porcentaje" AutoPostBack="true" OnTextChanged="txtProcentaje_TextChanged"></asp:TextBox>--%>
                                         </div>
                              <asp:Label ID="Label9" runat="server" CssClass="alineados middle margenLeft" Text="Precio Venta:"></asp:Label>
                                    <asp:TextBox ID="lblCostoVenta" runat="server" MaxLength="10" CssClass="input-small alineados middle text-right" placeholder="Precio" AutoPostBack="true" OnTextChanged="lblCostoVenta_TextChanged" Enabled="false" ></asp:TextBox>
                                    
                            </div>
                            <div class="row marTop text-center">
                                <div class="col-lg-6 col-sm-6 text-left">
                                    <asp:Label ID="Label6" runat="server" Text="Cantidad:&nbsp;" CssClass="alineados middle"></asp:Label>
                                    <asp:TextBox ID="txtCantidad" runat="server" MaxLength="12" CssClass="input-small alineados middle"
                                        placeholder="Cantidad" AutoPostBack="true" OnTextChanged="txtCantidad_TextChanged"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar una cantidad"
                                        ControlToValidate="txtCantidad" CssClass="validaciones alineados" ValidationGroup="agregar"
                                        Text="*"></asp:RequiredFieldValidator>
                                    <cc1:FilteredTextBoxExtender ID="txtCantidad_FilteredTextBoxExtender" runat="server"
                                        BehaviorID="txtCantidad_FilteredTextBoxExtender2" TargetControlID="txtCantidad"
                                        FilterType="Numbers,Custom" ValidChars="." />
                                    <asp:RegularExpressionValidator ControlToValidate="txtCantidad" CssClass="errores"
                                        Text="*" ID="RegularExpressionValidator3" runat="server" ValidationGroup="agregar"
                                        ErrorMessage="La Cantidad es incorrecta" ValidationExpression="^(\$|)([0-9]\d{0,2}(\,\d{3})*|([0-9]\d*))(\.\d{3})?$" />
                                    <asp:TextBox ID="txtVentaAgranel" runat="server" MaxLength="5" CssClass="input-small alineados middle"
                                        placeholder="Monto" AutoPostBack="true" OnTextChanged="txtVentaAgranel_TextChanged"
                                        Visible="true"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" BehaviorID="txtVentaAgranel_FilteredTextBoxExtender"
                                        TargetControlID="txtVentaAgranel" FilterType="Numbers,Custom" ValidChars="." />
                                </div>
                                <div class="col-lg-3 col-sm-3 text-left">
                                    <h5 class="text-primary"><asp:Label ID="Label11" runat="server" CssClass="alineados middle margenLeft" Text=""></asp:Label><asp:Label ID="lblTotalProducto" runat="server" CssClass="alineados middle margenLeft"></asp:Label></h5>
                                </div>
                                <div class="col-lg-3 col-sm-3 text-left">
                                    <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-info alineados margenLeft"
                                        ToolTip="Agregar Producto" OnClick="btnAgregar_Click" CausesValidation="true"
                                        ValidationGroup="agregar" />
                                </div>
                                 <div class="col-lg-12 col-sm-12 text-left">
                                  
                                      <asp:Label ID="Label13" runat="server" CssClass="alineados middle margenLeft" Text="Requisición :"></asp:Label>
                                   
                                   
                                    <asp:TextBox ID="txtrequi" runat="server" MaxLength="10" CssClass="input-medium alineados large text-center" placeholder="" AutoPostBack="true"   ></asp:TextBox>
                                     
                                 
                                      <asp:Label ID="Label21" runat="server" CssClass="alineados middle margenLeft" Text="Entrega :"></asp:Label>
                                       
                                
                                    <asp:TextBox ID="txtentrega" runat="server" MaxLength="50" CssClass="input-large alineados large text-center" placeholder="" AutoPostBack="true"  ></asp:TextBox>
                                        
                                 </div>

                           </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <asp:UpdatePanel ID="updMontos" runat="server">
                        <ContentTemplate>
                        <div class="row text-center">
                        <table style="width:99%; margin:0 auto">
                            <tr class="h4">
                                <td class="col-lg-2 col-sm-2 text-right" style="width:24%; padding:5px;"><asp:Label ID="Label12" runat="server" Text="Imp. Neto:" /></td>
                                <td class="col-lg-1 col-sm-1 text-left" style="width:35%"><asp:Label ID="lblSubtotal" runat="server" /></td>
                                <td class="col-lg-2 col-sm-2 text-right" style="width:34%"><asp:Label ID="Label48" runat="server" Text="Monto a Pagar:" /></td>
                                <td class="col-lg-1 col-sm-1 text-left"><asp:Label ID="lblMonto" runat="server" CssClass="text-info" /></td>
                            </tr>
                            <tr class="h4">
                                <td class="col-lg-2 col-sm-2 text-right" style="padding:5px"><asp:Label ID="lblDsctoGral" runat="server" Text="Descuento:" /></td>
                                <td class="col-lg-1 col-sm-1 text-left"><asp:TextBox runat="server" ID="txtDsctoGral" Text="0.00" Width="58" AutoPostBack="true" OnTextChanged="txtDsctoGral_TextChanged" CssClass="input-small" /><span>%&nbsp;</span>
                                    <asp:Label ID="lblMtoDcto" runat="server" Text="0.00" ></asp:Label>
                                </td>
                                <td class="col-lg-2 col-sm-2 text-right"><asp:Label ID="Label49" runat="server" Text="Monto Pagado:" /></td>
                                <td class="col-lg-1 col-sm-1 text-left"><asp:TextBox ID="txtMontoPagado" runat="server" CssClass="input-medium" placeholder="0.00" Width="98px" ontextchanged="txtMontoPagado_TextChanged" AutoPostBack="true"></asp:TextBox></td>
                            </tr>
                            <tr class="h4">
                                <td class="col-lg-2 col-sm-2 text-right" style="padding:5px"><asp:Label ID="Label8" runat="server" Text="Subtotal:" /></td>
                                <td class="col-lg-1 col-sm-1 text-left"><asp:Label ID="lblTotal" runat="server" /></td>
                                <td class="col-lg-2 col-sm-2 text-right"><asp:Label ID="Label52" runat="server" Text="Cambio:" /></td>
                                <td class="col-lg-1 col-sm-1 text-left"><asp:Label ID="lblCambio" runat="server" CssClass="text-info" /></td>
                            </tr>                                                                                                        
                            <tr class="h4">
                                <td class="col-lg-2 col-sm-2 text-right" style="padding:5px">
                                    <asp:Label ID="Label14" runat="server" Text="I.V.A. (16%):" /></td>
                                <td class="col-lg-1 col-sm-1 text-left"><asp:Label ID="lblIva" runat="server" /></td>
                                <td class="col-lg-2 col-sm-2 text-right"><asp:Label ID="Label53" runat="server" Text="Restan:" /></td>
                                <td class="col-lg-1 col-sm-1 text-left"><asp:Label ID="lblRestan" runat="server" CssClass="text-info" /></td>
                            </tr>
                        </table>
                            <br />
                        </div>    
                        
                        <div class="row marTop">
                            <div class="col-sm-6 col-lg-6">
                                <asp:Label ID="Label2" runat="server" Text="Forma de Pago:" CssClass="alineados middle" />
                                <asp:DropDownList ID="ddlFormaPago" runat="server" CssClass="alineados middle" OnSelectedIndexChanged="ddlFormaPago_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem Selected="True" Value="E">Efectivo</asp:ListItem>
                                    <asp:ListItem Value="D">Tarjeta Débito</asp:ListItem>
                                    <asp:ListItem Value="A">Tarjeta de Crédito</asp:ListItem>
                                    <asp:ListItem Value="T">Transferencia</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-sm-6 col-lg-6">
                                <asp:Label ID="Label3" runat="server" Text="Tarjeta:" CssClass="alineados middle"></asp:Label>
                                <asp:DropDownList ID="ddlTarjeta" runat="server" CssClass="alineados middle">
                                    <asp:ListItem Selected="True" Value="">Seleccione Tipo de Tarjeta</asp:ListItem>
                                    <asp:ListItem Value="VI">VISA</asp:ListItem>
                                    <asp:ListItem Value="MC">MASTERCARD</asp:ListItem>
                                    <asp:ListItem Value="AE">AMERICAN EXPRESS</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar el tipo de tarjeta"
                                    ControlToValidate="ddlTarjeta" CssClass="validaciones alineados" ValidationGroup="pago"
                                    Text="*"></asp:RequiredFieldValidator>
                                <%--<asp:TextBox ID="txtReferencia" runat="server" MaxLength="4" CssClass="input-mini alineados middle"
                                    placeholder="Dígitos"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="txtReferencia_FilteredTextBoxExtender" runat="server"
                                    BehaviorID="txtReferencia_FilteredTextBoxExtender" TargetControlID="txtReferencia"
                                    FilterType="Numbers" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar los últimos 4 dígitos de la tarjeta"
                                    ControlToValidate="txtReferencia" CssClass="validaciones alineados" ValidationGroup="pago"
                                    Text="*"></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                        <div class="row marTop">
                            <div class="col-sm-6 col-lg-6">
                                <asp:Label ID="Label4" runat="server" Text="Banco:" CssClass="alineados middle"></asp:Label>
                                <asp:DropDownList ID="ddlBanco" runat="server" DataSourceID="SqlDataSource1" DataTextField="nombrebanco"
                                    DataValueField="clvbanco" AppendDataBoundItems="true" CssClass="alineados middle">
                                    <asp:ListItem Selected="True" Text="Seleccione Banco" Value=""></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar el banco"
                                    ControlToValidate="ddlBanco" CssClass="validaciones alineados" ValidationGroup="pago"
                                    Text="*"></asp:RequiredFieldValidator>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                                    SelectCommand="select clvbanco, nombrebanco from catbancos"></asp:SqlDataSource>
                            </div>
                            <div class="col-sm-6 col-lg-6">
                                <asp:Label ID="Label54" runat="server" Text="Aplica Meses sin intereses:" CssClass="alineados middle"></asp:Label>
                                <asp:CheckBox ID="chkIntereses" runat="server" oncheckedchanged="chkIntereses_CheckedChanged" AutoPostBack="true"/>
                            </div>
                        </div>
                        <div class="row marTop">
                                <div class="col-sm-6 col-lg-6 text-left">
                                    <asp:Label ID="Label51" runat="server" Text="Parcialización:" ></asp:Label>&nbsp;
                                    <asp:DropDownList ID="ddlParcial" runat="server" CssClass="input-medium">
                                        <asp:ListItem Value="00" Text="Ninguno" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="03" Text="3 Meses"></asp:ListItem>
                                        <asp:ListItem Value="06" Text="6 Meses"></asp:ListItem>
                                        <asp:ListItem Value="09" Text="9 Meses"></asp:ListItem>
                                        <asp:ListItem Value="12" Text="12 Meses"></asp:ListItem>
                                        <asp:ListItem Value="15" Text="15 Meses"></asp:ListItem>
                                        <asp:ListItem Value="18" Text="18 Meses"></asp:ListItem>
                                        <asp:ListItem Value="24" Text="24 Meses"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-6 col-lg-6 text-left">
                                    <asp:Label ID="Label50" runat="server" Text="Diferimiento:"></asp:Label>&nbsp;
                                    <telerik:RadNumericTextBox RenderMode="Lightweight" runat="server" ID="radDiferimiento" Skin="MetroTouch" CssClass="input-mini" Value="0" MinValue="0" MaxValue="99" ShowSpinButtons="true" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                                </div>
                            </div>
                        <div class="row marTop">
                                 <div class="col-lg-12 col-sm-12 text-center">                                    
                                    <asp:ValidationSummary ID="ValidationSummary5" runat="server" CssClass="validaciones" DisplayMode="List" ValidationGroup="pago" />                                        
                                </div>
                                <div class="col-lg-6 col-sm-6 text-center ">
                                    <asp:Button ID="btnPagar" runat="server" Text="Pagar" CssClass="btn  btn-icon btn-success font-20"
                                       OnClick="btnPagar_Click" ValidationGroup="pago" />
                                </div>
                                <div class="col-lg-6 col-sm-6 text-center ">
                                        <asp:Button ID="btnCancelarVenta" runat="server" Text="Cancelar" CssClass="btn btn-icon btn-danger alineados font-20" OnClick="btnCancelarVenta_Click" />
                                     </div>
                                     </div>
                        <div class="row marTop text-right">
                                    <%--<asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <div class="col-lg-9 col-sm-6 text-right"></div>
                                            <div class="col-lg-3 col-sm-6 text-right">
                                                <table>
                                                    <tr class="h4">
                                                        <td class="col-lg-2 col-sm-2 text-left">
                                                            <asp:Label ID="Label12" runat="server" Text="Subtotal:" Visible="true"></asp:Label>
                                                        </td>
                                                        <td class="col-lg-1 col-sm-1 text-right">
                                                            <asp:Label ID="lblSubtotal" runat="server" Visible="true"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr class="h4">
                                                        <td class="col-lg-2 col-sm-2 text-left">
                                                            <asp:Label ID="Label14" runat="server" Text="I.V.A. (16%):" Visible="true"></asp:Label>
                                                        </td>
                                                        <td class="col-lg-1 col-sm-1 text-right">
                                                            <asp:Label ID="lblIva" runat="server" Visible="true"></asp:Label>
                                                        </td>
                                                    </tr>                                                                                                        
                                                </table>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </telerik:RadPageView>
                  <telerik:RadPageView runat="server" ID="VtaTaller"> 
                    <asp:UpdatePanel ID="updVtaTaller" runat="server">
                        <ContentTemplate>
                            <div class="row marTop">
                                 
                               <asp:Label ID="lblagr" runat="server" Text=":" CssClass="alineados middle"></asp:Label>

                                <div class="col-lg-8 col-sm-8 text-left">
                                    <asp:Label ID="lblSelecTaller" runat="server" Visible="true" Text="R.F.C" />
                                     <asp:TextBox ID="TextBox1" Visible="true" runat="server" placeholder="R.F.C." CssClass="input-medium" MaxLength="13"   AutoPostBack="true"/>&nbsp;&nbsp;&nbsp;
                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ValidationGroup="alta" ControlToValidate="txtRFC" Text="*" CssClass="errores" runat="server" ErrorMessage="El formato del R.F.C. Es incorrecto" ValidationExpression="^[A-Za-z]{3,4}[0-9]{6}[0-9A-Za-z]{3}$" />
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="alta" ControlToValidate="txtRFC" Text="*" CssClass="errores"  ErrorMessage="Necesita colocar el R.F.C." />
                                     <asp:Button ID="btnActualizar" runat="server" Visible="true" Text="Validar" OnClick="btnActualizar_Click" CssClass="btn-success"  />&nbsp;&nbsp;&nbsp;
                                     <asp:Button ID="Button1" runat="server" Text="Cancelar" Visible="true" ToolTip="Cancelar"  CssClass="btn-danger"  />
                                     <div class="alert-danger col-lg-12 col-sm-12 text-center">
            <asp:Label ID="lblErroresFolio" runat="server" CssClass="errores" />
        </div>
            
                                    <asp:DropDownList ID="ddlTaller" runat="server" DataSourceID="sqlDsTaller" CssClass="input-medium" Visible="false" DataTextField="nombre_taller" DataValueField="id_taller" AppendDataBoundItems="true" AutoPostBack="true" >
                                        <asp:ListItem Text="Seleccione" Value="-1">--Seleccione Tienda--</asp:ListItem>
                                    </asp:DropDownList>

                                    <asp:SqlDataSource runat="server" ID="sqlDsTaller" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                                        SelectCommand="SELECT [id_taller], [nombre_taller] FROM [Talleres]">
                                    </asp:SqlDataSource>
                                </div>

                                 <div class="col-lg-12 col-sm-12 text-left">
                            <div class="col-lg-2 col-sm-2 text-left"><asp:Label ID="Label17" Visible="true" runat="server" Text="Persona:" CssClass="verticalAlineado" /></div>
                       
                    </div>



                                <div class="col-lg-4 col-sm-4 text-left">
                                    <asp:Label ID="lblOrden" runat="server" Visible="false" Text="Orden: &nbsp;"></asp:Label>
                                    <asp:TextBox ID="txtOrden" runat="server" Visible="false" MaxLength="10" placeholder="Orden" AutoPostBack="true" CssClass="alingMiddle input-small" OnTextChanged="txtOrden_TextChanged"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="filtTxtOrden" runat="server" FilterType="Numbers" TargetControlID="txtOrden" />
                                </div>
                                <div class="col-lg-4 col-sm-4 text-left">
                                    <asp:Label ID="Label7" runat="server" Visible="false" Text="Solicitud: &nbsp;"></asp:Label>
                                    <asp:DropDownList ID="ddlRegistro" Visible="false" runat="server" DataSourceID="SqlDataSource2" CssClass="input-medium" DataTextField="solicitud" DataValueField="id_solicitud" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Seleccione" Value="-1">--Seleccione Registro--</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                                        SelectCommand="select id_solicitud, ltrim(rtrim(folio_solicitud))+' - '+ltrim(rtrim(detalle)) as solicitud from registro_pinturas where id_taller=@taller and no_orden=@orden and estatus<>'T'">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="ddlTaller" PropertyName="SelectedValue" Name="taller" />
                                            <asp:ControlParameter ControlID="txtOrden" PropertyName="Text" Name="orden" DefaultValue="0" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                            </div>

                         
                            
                            <asp:Panel ID="clientespanel" Visible="true" runat="server" CssClass="col-lg-12 col-sm-12 masxHeight marTop" ScrollBars="Auto" >
                                
                                 <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label28" runat="server" Text="R.F.C.: " /></div>
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="Debe indicar la Razón Social" Text="*" ValidationGroup="crea" ControlToValidate="txtRazonNew" CssClass="alineado errores"></asp:RequiredFieldValidator>                               
                        </div>
                    </div>                
                                  <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label97" runat="server" Text="Calle: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:TextBox ID="TextBox2" runat="server" MaxLength="200" CssClass="input-xxlarge" placeholder="Calle"></asp:TextBox>
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
                            <asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select cve_edo,nom_edo from Estados_f where cve_pais=@pais">
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="Debe indicar el Correo CC." Text="*" CssClass="alineado errores" ControlToValidate="txtCorreoCC" ValidationGroup="crea"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-4 col-sm-4 text-right"><asp:Label ID="Label96" runat="server" Text="Correo CCO: " /></div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:TextBox ID="txtCorreoCCO" runat="server"  MaxLength="250"  CssClass="input-xxlarge" placeholder="Correo CCO"></asp:TextBox>
                        </div>
                        <div class="col-lg-4 col-sm-4 text-left">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="Debe indicar el Correo CCO." Text="*" CssClass="alineado errores" ControlToValidate="txtCorreoCCO" ValidationGroup="crea"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    </div>
                                 <asp:Button ID="btnAgregaCli" runat="server" Text="Agregar Cliente" Visible="true" OnClick="btnAgregaCli_Click"  CssClass="btn-grey"  />
                                <asp:Button ID="btnActualiza" runat="server" Text="Actualizar Cliente" Visible="false"  OnClick="btnActualiza_Click"  CssClass="btn-grey"  />

                    </div>
                            </asp:Panel>
                            
                            
                            
                  
                               <asp:Panel ID="PanelDetalleVenta" Visible="false" runat="server" CssClass="col-lg-12 col-sm-12 masxHeight marTop" ScrollBars="Auto" >
                                 <div class="row marTop">
                                <div class="col-sm-12 col-lg-12 text-left">
                                    <asp:Label ID="lblCliente" runat="server" Visible="false" Text="Cliente: &nbsp;" />
                                    <asp:TextBox ID="txtCliente" runat="server" Visible="false" CssClass="alingMiddle input-xxlarge" placeholder="Cliente"></asp:TextBox>                                    
                                </div>                                
                            </div>
                            <div class="row marTop">
                                <div class="col-lg-6 col-sm-6 text-left">
                                    <asp:Label ID="lblProv" runat="server" Visible="false" Text="Proveedor:&nbsp;"></asp:Label>
                                    <asp:DropDownList ID="ddlProv" Visible="false"  runat="server" DataSourceID="SqlDsProv" DataTextField="razon_social" DataValueField="id_cliprov" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Seleccione" Value="-1">--Seleccione Proveedor--</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:SqlDataSource runat="server" ID="SqlDsProv" ConnectionString='<%$ ConnectionStrings:PVW %>' 
                                        SelectCommand="SELECT [id_cliprov], [razon_social] FROM [Cliprov] WHERE ([tipo] = @tipo) ORDER BY [id_cliprov]">
                                        <SelectParameters>
                                            <asp:Parameter DefaultValue="P" Name="tipo" Type="String"></asp:Parameter>
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </div>
                                <div class="col-lg-6 col-sm-6 text-left">
                                    <asp:Label ID="lblSelectArea"  Visible="false" runat="server" Text="Area de Aplicación:&nbsp;"></asp:Label>
                                    <asp:DropDownList ID="ddlAreaApp" runat="server" Visible="false" CssClass="input-medium">
                                        <asp:ListItem Text="Pinturas" Value="Pn" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Caja Chica" Value="Cc"></asp:ListItem>
                                        <asp:ListItem Text="Llantas" Value="Ll"></asp:ListItem>
                                        <asp:ListItem Text="Almacen" Value="Al"></asp:ListItem>
                                    </asp:DropDownList>                           
                                </div>
                            </div> 
                                 <asp:Panel ID="pnlDatosIni" Visible="false" runat="server" CssClass="col-lg-12 col-sm-12">                                
                                <div class="row" style="margin-top:10px;">
                                    <div class="col-lg-4 col-sm-4 text-left">
                                        <asp:CheckBox ID="chkVtaTaller" runat="server" Visible="false" AutoPostBack="true" OnCheckedChanged="chkVtaTaller_CheckedChanged" Text="&nbsp;Realizar Venta a Tienda" CssClass="" CausesValidation="true" ValidationGroup="agrega" />
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-left">
                                        <asp:CheckBox ID="chkVtaCredito" runat="server" Visible="false" AutoPostBack="true" Text="&nbsp;Venta a Crédito" CssClass="" CausesValidation="true" ValidationGroup="agrega" OnClick="javascript:desactivaMontoPagado(this);" />
                                    </div>
                                    <div class="col-lg-4 col-sm-4 text-left">
                                        <asp:CheckBox ID="chkEspecial" runat="server" Visible="false" AutoPostBack="true" Text="&nbsp;Cliente Especial" CausesValidation="true" OnCheckedChanged="chkEspecial_CheckedChanged"/>
                                    </div>
                                </div>
                            </asp:Panel>
                            </asp:Panel>
                                                      
                           
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </telerik:RadPageView> 
                </telerik:RadMultiPage>
            </div>
        </div>        
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:Panel ID="PanelMask" runat="server" Visible="false" CssClass="mask">
                </asp:Panel>
                <asp:Panel ID="pnlTicket" runat="server" Visible="false" CssClass="popupw pading8px">
                    <asp:Panel ID="Panel1" runat="server" CssClass="text-center">
                        <div class="col-lg-12 col-sm-12 text-center ticket">
                            <asp:Label ID="Label15" runat="server" Text="Ticket:"></asp:Label>
                            <asp:Label ID="lblTicket" runat="server" CssClass="text-info"></asp:Label>
                        </div>
                        
                                            
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:CheckBox ID="chkFacturacionPend" Checked="false" runat="server" Text="Facturar"
                                AutoPostBack="true" OnCheckedChanged="chkFacturacionPend_CheckedChanged" />
                        </div>
                        <div class="col-lg-12 col-sm-12 text-center">
                            <asp:Label ID="Label34" runat="server" Text="R.F.C: " Visible="false" />
                            <asp:TextBox ID="txtRFCVerifica" runat="server" Visible="false" placeholder="R.F.C."
                                MaxLength="13" CssClass="input-medium" />
                            <asp:Label ID="lblRazon" runat="server" CssClass="text-info negritas"></asp:Label>
                            <asp:Button ID="btnRevisaRFC" runat="server" Visible="false" Text="Buscar" OnClick="btnRevisaRFC_Click" />
                        </div>
                       
                        <div class="col-lg-12 col-sm-12 text-center alert-danger">
                            <asp:Label ID="lblErrorFacCliente" runat="server" CssClass="errores" /><br />
                            <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List"
                                CssClass="errores" ValidationGroup="alta" />
                            
                        </div>
                        <br />
                        <br />
                        <asp:Panel ID="pnlCliente" runat="server" CssClass="col-lg-12 col-sm-12 centrado text-center"
                            Visible="false">
                            <div class="col-lg-6 col-sm-6">
                                <div class="col-lg-3 col-sm-2 text-left">
                                    <asp:Label ID="Label16" runat="server" Text="Persona:" /></div>
                                <div class="col-lg-5 col-sm-6 text-left">
                                    <asp:RadioButtonList ID="rbtnPersona" runat="server" RepeatDirection="Horizontal"
                                        CssClass="alineados verticalAlineado" AutoPostBack="true" OnSelectedIndexChanged="rbtnPersona_SelectedIndexChanged"
                                        ToolTip="Tipo Persona" CellSpacing="20">
                                        <asp:ListItem Text="Moral" Value="M" Selected="True" />
                                        <asp:ListItem Text="Fisica" Value="F" />
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-6">
                                <div class="col-lg-3 col-sm-2 text-left">
                                    <asp:Label ID="Label18" runat="server" Text="R.F.C.:" /></div>
                                <div class="col-lg-6 col-sm-6 text-left">
                                    <asp:TextBox ID="txtRFC" runat="server" placeholder="R.F.C." CssClass="input-medium"
                                        MaxLength="13" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="alta"
                                        ControlToValidate="txtRFC" Text="*" CssClass="errores" runat="server" ErrorMessage="El formato del R.F.C. Es incorrecto"
                                        ValidationExpression="^[A-Za-z]{3,4}[0-9]{6}[0-9A-Za-z]{3}$" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ValidationGroup="alta"
                                        ControlToValidate="txtRFC" Text="*" CssClass="errores" ErrorMessage="Necesita colocar el R.F.C." />
                                </div>
                            </div>
                            <div class="col-lg-12 col-sm-12 text-center">
                                <div class="col-lg-12 col-sm-12 text-left">
                                    <asp:Label ID="Label19" runat="server" Text="Razón Social:" />
                                    <asp:TextBox ID="txtArchivarNombre" runat="server" placeholder="Razón Social" CssClass="input-large"
                                        MaxLength="128" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtArchivarNombre" Text="*" CssClass="errores"
                                        ValidationGroup="alta" ID="RequiredFieldValidator7" runat="server" ErrorMessage="RequiredFieldValidator" />
                                    <asp:Label ID="Label20" runat="server" Text="Nombre:" Visible="false" CssClass="alineados verticalAlineado" />
                                    <asp:TextBox ID="txtNombre" runat="server" placeholder="Nombre" Visible="false" CssClass="input-medium alineados verticalAlineado"
                                        MaxLength="40" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtNombre" ID="RequiredFieldValidator8"
                                        runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores"
                                        ValidationGroup="alta" />
                                    <asp:TextBox ID="txtPaterno" runat="server" placeholder="Apellido Pat" Visible="false"
                                        CssClass="input-small alineados verticalAlineado" MaxLength="40" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtPaterno" ID="RequiredFieldValidator9"
                                        runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores"
                                        ValidationGroup="alta" />
                                    <asp:TextBox ID="txtMaterno" runat="server" placeholder="Apellido Mat" Visible="false"
                                        CssClass="input-small alineados verticalAlineado" MaxLength="40" />
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-6 marTop">
                                <div class="col-lg-3 col-sm-2 text-left">
                                    <asp:Label ID="Label23" runat="server" Text="Calle:" CssClass="alineados verticalAlineado" /></div>
                                <div class="col-lg-3 col-sm-6 text-left">
                                    <asp:TextBox ID="txtCalle" runat="server" placeholder="Calle" CssClass="input-large alineados verticalAlineado"
                                        MaxLength="40" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtCalle" ID="RequiredFieldValidator11"
                                        runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores"
                                        ValidationGroup="alta" />
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-6 marTop">
                                <div class="col-lg-3 col-sm-2 text-left">
                                    <asp:Label ID="Label25" runat="server" Text="No.:" CssClass="alineados verticalAlineado" /></div>
                                <div class="col-lg-3 col-sm-6 text-left">
                                    <asp:TextBox ID="txtNumero" runat="server" placeholder="No." CssClass="input-mini alineados verticalAlineado"
                                        MaxLength="20" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtNumero" ID="RequiredFieldValidator12"
                                        runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores"
                                        ValidationGroup="alta" />
                                </div>
                                <div class="col-lg-3 col-sm-2 text-left">
                                    &nbsp;&nbsp;<asp:Label ID="Label32" runat="server" Text="Int.:" CssClass="alineados verticalAlineado"
                                        MaxLength="25" /></div>
                                <div class="col-lg-3 col-sm-6 text-left">
                                    <asp:TextBox ID="txtNumeroInt" runat="server" placeholder="Int." CssClass="input-mini alineados verticalAlineado" /></div>
                            </div>
                            <div class="col-lg-6 col-sm-6">
                                <div class="col-lg-3 col-sm-2 text-left">
                                    <asp:Label ID="Label24" runat="server" Text="Colonia:" CssClass="alineados verticalAlineado" /></div>
                                <div class="col-lg-3 col-sm-6 text-left">
                                    <asp:TextBox ID="txtColonia" runat="server" CssClass="input-large verticalAlineado"
                                        MaxLength="40" placeholder="Colonia" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtColonia" ID="RequiredFieldValidator13"
                                        runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores"
                                        ValidationGroup="alta" />
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-6">
                                <div class="col-lg-3 col-sm-2 text-left">
                                    <asp:Label ID="Label29" runat="server" Text="Mun./Deleg.:" /></div>
                                <div class="col-lg-3 col-sm-6 text-left">
                                    <asp:TextBox ID="txtDelegacion" runat="server" placeholder="Mun./Deleg." CssClass="input-large"
                                        MaxLength="50" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtDelegacion" ID="RequiredFieldValidator15"
                                        runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores"
                                        ValidationGroup="alta" />
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-6">
                                <div class="col-lg-3 col-sm-2 text-left">
                                    <asp:Label ID="Label30" runat="server" Text="Estado:" /></div>
                                <div class="col-lg-3 col-sm-6 text-left">
                                    <asp:TextBox ID="txtEstado" runat="server" placeholder="Estado" CssClass="input-large"
                                        MaxLength="20" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtEstado" ID="RequiredFieldValidator16"
                                        runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores"
                                        ValidationGroup="alta" />
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-6">
                                <div class="col-lg-3 col-sm-2 text-left">
                                    <asp:Label ID="Label31" runat="server" Text="Ciudad:" /></div>
                                <div class="col-lg-3 col-sm-6 text-left">
                                    <asp:TextBox ID="txtCiudad" runat="server" placeholder="Ciudad" CssClass="input-large"
                                        MaxLength="40" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtCiudad" ID="RequiredFieldValidator17"
                                        runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores"
                                        ValidationGroup="alta" />
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-6">
                                <div class="col-lg-3 col-sm-2 text-left">
                                    <asp:Label ID="Label26" runat="server" Text="C.P.:" /></div>
                                <div class="col-lg-3 col-sm-6 text-left">
                                    <asp:TextBox ID="txtCP" runat="server" placeholder="C.P." CssClass="input-small"
                                        MaxLength="5" />
                                    <cc1:FilteredTextBoxExtender ID="txtCP_FilteredTextBoxExtender" runat="server" BehaviorID="txtCP_FilteredTextBoxExtender"
                                        TargetControlID="txtCP" FilterType="Numbers" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtCP" ID="RequiredFieldValidator14"
                                        runat="server" ErrorMessage="RequiredFieldValidator" Text="*" CssClass="errores"
                                        ValidationGroup="alta" />
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-6">
                                <div class="col-lg-3 col-sm-2 text-left">
                                    <asp:Label ID="Label27" runat="server" Text="Email:" /></div>
                                <div class="col-lg-3 col-sm-6 text-left">
                                    <asp:TextBox ID="txtEMail" runat="server" placeholder="Email" CssClass="input-large"
                                        MaxLength="40" />
                                    <asp:RequiredFieldValidator ControlToValidate="txtEMail" ID="RequiredFieldValidator18"
                                        runat="server" ErrorMessage="Debe indicar el Email" Text="*" CssClass="errores"
                                        ValidationGroup="alta" />
                                    <asp:RegularExpressionValidator ControlToValidate="txtEMail" ValidationExpression="^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$"
                                        ID="RegularExpressionValidator2" runat="server" ErrorMessage="El Email no tiene el formato correcto"
                                        Text="*" CssClass="errores" ValidationGroup="alta" />
                                </div>
                            </div>
                            <div class="col-lg-12 col-sm-12">
                                <div class="col-lg-2 col-sm-2 text-left">
                                    <asp:Label ID="Label33" runat="server" Text="Referencia:" /></div>
                                <div class="col-lg-10 col-sm-10 text-left">
                                    <asp:TextBox ID="txtReferenciaFacCliente" runat="server" placeholder="Referencia"
                                        CssClass="input-large" MaxLength="50" />
                                </div>
                            </div>
                            <div class="col-lg-12 col-sm-12">
                                <div class="col-lg-6 col-sm-6 text-center marTop">
                                    <asp:Button ID="btnNuevoCliente" runat="server" Text="Guardar Cambios" ToolTip="Guardar Cambios"
                                        CssClass="btn btn-success alineados" OnClick="btnNuevoCliente_Click" /></div>
                                <div class="col-lg-6 col-sm-6 text-center marTop">
                                    <asp:Button ID="btnCancelarCliente" runat="server" Text="Solicitar Factura" ToolTip="Solicitar Factura"
                                        CssClass="btn btn-success alineados" OnClick="btnCancelarCliente_Click" /></div>
                            </div>
                        </asp:Panel>
                        <div class="col-lg-12 col-sm-12 text-center marTop marBot">
                            <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" Target="_blank" CssClass="btn btn-success"
                                OnClick="btnImprimir_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnCancelarImpresion" runat="server" Text="Cerrar" CssClass="btn btn-danger"
                                OnClick="btnCancelarImpresion_Click" />
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
     
</asp:Content>
