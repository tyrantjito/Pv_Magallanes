<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CatProductos.aspx.cs" Inherits="CatProductos" MasterPageFile="~/Administracion.master" Culture="es-Mx" UICulture="es-Mx" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">  
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    <div class="ancho95 centrado">
        <div class="row">
            <div class="alert-success col-lg-12 col-sm-12 center negritas font-14 pad1em">
               <i class="icon-gift"></i> <asp:Label runat="server" ID="lblTitulo" Text="Productos" CssClass="alert-success"></asp:Label>
            </div>
        </div>
        <br />
        
        <div class="row">
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label1" runat="server" Text="Clave:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">                    
                <asp:TextBox ID="txtClave" runat="server" CssClass="input-medium" MaxLength="30"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="txtClave_TextBoxWatermarkExtender" runat="server" BehaviorID="txtClave_TextBoxWatermarkExtender" TargetControlID="txtClave" WatermarkCssClass="water input-medium" WatermarkText="Clave" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar la clave del producto" Text="*" ValidationGroup="agrega" ControlToValidate="txtClave" CssClass="errores"></asp:RequiredFieldValidator>                    
            </div>
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label2" runat="server" Text="Descripción:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">                    
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="input-large" MaxLength="250"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="txtDescripcion_TextBoxWatermarkExtender" runat="server" BehaviorID="txtDescripcion_TextBoxWatermarkExtender" TargetControlID="txtDescripcion" WatermarkCssClass="water input-large" WatermarkText="Descripción" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar el descripción del producto" ValidationGroup="agrega" ControlToValidate="txtDescripcion" Text="*" CssClass="errores"></asp:RequiredFieldValidator>                    
            </div>
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label13" runat="server" Text="Precio Venta:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">                    
                <asp:TextBox ID="txtPrecioVenta" runat="server" CssClass="input-small" MaxLength="15"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="txtPrecioVenta_FilteredTextBoxExtender" 
                    runat="server" BehaviorID="txtPrecioVenta_FilteredTextBoxExtender" 
                    TargetControlID="txtPrecioVenta" FilterType="Numbers, Custom" ValidChars="." />
                <cc1:TextBoxWatermarkExtender ID="txtPrecioVentaWatermarkExtender1" runat="server" BehaviorID="txtPrecioVenta_TextBoxWatermarkExtender" TargetControlID="txtPrecioVenta" WatermarkCssClass="water input-small" WatermarkText="P. Venta" />                    
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe indicar un precio al producto" Text="*" ValidationGroup="agrega" ControlToValidate="txtPrecioVenta" CssClass="errores"></asp:RequiredFieldValidator>                    
            </div> 
        </div>
        <div class="row marTop">
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label3" runat="server" Text="Unidad:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">
                <asp:DropDownList ID="ddlUnidad" runat="server" CssClass="input-medium" DataSourceID="SqlDataSource6" DataTextField="descripcion" DataValueField="idMedida"></asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select '' as idMedida, 'Seleccione Unidad' as descripcion union all select idMedida,descripcion from catunidmedida"></asp:SqlDataSource>
            </div>

            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label14" runat="server" Text="Categoría:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">
                <asp:DropDownList ID="ddlCategoria" runat="server" CssClass="input-medium" DataSourceID="SqlDataSourceCat" DataTextField="descripcion_categoria" DataValueField="id_categoria"></asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSourceCat" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select distinct tabla.id_categoria,tabla.descripcion_categoria from (                     
                    select case id_categoria when 0 then '0' else cast(id_categoria as char(10))end as id_categoria,descripcion_categoria from catcategorias)as tabla
                    order by 1"></asp:SqlDataSource>
            </div>

            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label4" runat="server" Text="Familia:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">                    
                <asp:DropDownList ID="ddlFamilia" runat="server" CssClass="input-medium" DataSourceID="SqlDataSource7" DataTextField="descripcionFamilia" DataValueField="idFamilia">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select '' as idFamilia,'Seleccione Familia' as descripcionFamilia union all select idFamilia,descripcionFamilia from catfamilias">
                </asp:SqlDataSource>
            </div>
        </div>
        <div class="row marTop">
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label5" runat="server" Text="Líneas:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">                    
                <asp:DropDownList ID="ddlLinea" runat="server" CssClass="input-medium" DataSourceID="SqlDataSource8" DataTextField="descripcionLinea" DataValueField="idLinea">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select '' as idLinea,'Seleccione Línea' as descripcionLinea union all select idLinea,descripcionLinea from catlineas">
                </asp:SqlDataSource>
            </div>
             <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label17" runat="server" Text="Producto con IVA:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">                    
                <asp:CheckBox runat="server" ID="chkIVA" Checked="false" />
            </div>
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label16" runat="server" Text="Venta a Granel:"></asp:Label></div>
            <div class="col-lg-3 col-sm-4 text-left">                    
                <asp:CheckBox runat="server" ID="chkGranel" Checked="false" />
            </div>
        </div>
        <div class="row marTop">
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label12" runat="server" Text="Detalles:"></asp:Label></div>
            <div class="col-lg-3 col-sm-9 text-left">                    
                <asp:TextBox ID="txtDetalles" runat="server" CssClass="input-xlarge" MaxLength="250"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="txtDetallesWatermarkExtender1" runat="server" BehaviorID="txtDetalles_TextBoxWatermarkExtender" TargetControlID="txtDetalles" WatermarkCssClass="water input-xlarge" WatermarkText="Detalles" />
            </div>
            <div class="col-lg-1 col-sm-2 text-left"><asp:Label ID="Label11" runat="server" Text="Observaciones:"></asp:Label></div>
            <div class="col-lg-3 col-sm-5 text-left">                    
                <asp:TextBox ID="txtObservaciones" runat="server" CssClass="input-xlarge" MaxLength="250"></asp:TextBox>
                <cc1:TextBoxWatermarkExtender ID="txtObservacionesWatermarkExtender1" runat="server" BehaviorID="txtObservaciones_TextBoxWatermarkExtender" TargetControlID="txtObservaciones" WatermarkCssClass="water input-xlarge" WatermarkText="Observaciones" />
            </div>
            <div class="col-lg-1 col-sm-2 text-center">
                    <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn-info" ValidationGroup="agrega" onclick="btnAgregar_Click"  />
            </div>
            <div class="col-lg-3 col-sm-4 text-left">
                <asp:TextBox ID="txtFiltro" runat="server" placeholder="Buscar" CssClass="input-medium" />
                <asp:Button ID="btnFiltro" runat="server" Text="Buscar" CssClass="btn-info" OnClick="btnFiltro_Click" />
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-lg-12 col-sm-12 center">
                <div class="table-responsive">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="idProducto"
                                CssClass="table table-bordered center" EmptyDataText="No existen Productos Registrados"
                                AllowPaging="True" PageSize="7" AllowSorting="True" OnPageIndexChanging="GridView1_PageIndexChanging"
                                onrowcommand="GridView1_RowCommand" onrowdatabound="GridView1_RowDataBound" OnRowDeleting="GridView1_RowDeleting" 
                                OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowCancelingEdit="GridView1_RowCancelingEdit">
                                <Columns>
                                    <asp:TemplateField HeaderText="Producto" SortExpression="idProducto">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblClaveMod" runat="server" Text='<%# Eval("idProducto") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkProducto" runat="server" CssClass="alert-info negritas" CommandArgument='<%# Eval("idProducto")+";"+Eval("descripcionProducto") %>' onclick="lnkProducto_Click"><asp:Label ID="lblClave" runat="server" Text='<%# Eval("idProducto") %>'></asp:Label></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descripción" SortExpression="descripcionProducto">
                                        <EditItemTemplate>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar el descripción del producto" ValidationGroup="edita" ControlToValidate="txtDescripcionMod" Text="*" CssClass="errores"></asp:RequiredFieldValidator>                    
                                            <asp:TextBox ID="txtDescripcionMod" runat="server" CssClass="input-medium" MaxLength="250" Text='<%# Eval("descripcionProducto") %>'></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="txtDescripcionMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtDescripcionMod_TextBoxWatermarkExtender" TargetControlID="txtDescripcionMod" WatermarkCssClass="water input-medium" WatermarkText="Descripción" />
                                        </EditItemTemplate>
                                        <ItemTemplate>                                            
                                            <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("descripcionProducto") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho250px" />
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField HeaderText="Unidad" SortExpression="descripcionMedida">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlUnidadMod" runat="server" CssClass="input-medium" DataSourceID="SqlDataSource2" DataTextField="descripcion" DataValueField="idMedida" SelectedValue='<%# Bind("idMedida") %>' ></asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select '' as idMedida, 'Seleccione Unidad' as descripcion union all select idMedida,descripcion from catunidmedida"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" runat="server" Text='<%# Eval("descripcionMedida") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho120px" />
                                    </asp:TemplateField>
                                        
                                    <asp:TemplateField HeaderText="Categoría" SortExpression="descripcion_categoria">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlCategoriaMod" runat="server" CssClass="input-medium" 
                                                DataSourceID="SqlDataSource50" DataTextField="descripcion_categoria" 
                                                DataValueField="id_categoria"></asp:DropDownList>
                                            
                                            <asp:SqlDataSource ID="SqlDataSource50" runat="server" 
                                                ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select distinct tabla.id_categoria,tabla.descripcion_categoria from (
select id_categoria,descripcion_categoria from catcategorias
)as tabla
order by 1"></asp:SqlDataSource>
                                            
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCat" runat="server" Text='<%# Eval("descripcion_categoria") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Familia" SortExpression="descripcionFamilia">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlFamiliaMod" runat="server" CssClass="input-medium" DataSourceID="SqlDataSource3" DataTextField="descripcionFamilia" DataValueField="idFamilia" SelectedValue='<%# Bind("idFamilia") %>' ></asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select '' as idFamilia,'Seleccione Familia' as descripcionFamilia union all select idFamilia,descripcionFamilia from catfamilias"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label7" runat="server" Text='<%# Eval("descripcionFamilia") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho120px" />
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="Línea" SortExpression="descripcionLinea">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlLineaMod" runat="server" CssClass="input-medium" DataSourceID="SqlDataSource4" DataTextField="descripcionLinea" DataValueField="idLinea" SelectedValue='<%# Bind("idLinea") %>' ></asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select '' as idLinea,'Seleccione Línea' as descripcionLinea union all select idLinea,descripcionLinea from catlineas"></asp:SqlDataSource>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label8" runat="server" Text='<%# Eval("descripcionLinea") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho120px" />
                                    </asp:TemplateField>   
                                    <asp:TemplateField HeaderText="Detalles" SortExpression="detalles">
                                        <EditItemTemplate>                                            
                                            <asp:TextBox ID="txtDetallesMod" runat="server" CssClass="textBoxTextArea" MaxLength="250" TextMode="MultiLine" Rows="5" Text='<%# Eval("detalles") %>'></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="txtDetallesMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtDetallesMod_TextBoxWatermarkExtender" TargetControlID="txtDetallesMod" WatermarkCssClass="water textBoxTextArea" WatermarkText="Detalles" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label9" runat="server" Text='<%# Eval("detalles") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho250px" />
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="Observaciones" SortExpression="observaciones">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtObservacionesMod" runat="server" CssClass="textBoxTextArea" TextMode="MultiLine" Rows="5" MaxLength="250" Text='<%# Eval("observaciones") %>'></asp:TextBox>
                                            <cc1:TextBoxWatermarkExtender ID="txtObservacionesMod_TextBoxWatermarkExtender" runat="server" BehaviorID="txtObservacionesMod_TextBoxWatermarkExtender" TargetControlID="txtObservacionesMod" WatermarkCssClass="water textBoxTextArea" WatermarkText="Observaciones" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label10" runat="server" Text='<%# Eval("observaciones") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho250px" />
                                    </asp:TemplateField>
                                      <asp:TemplateField HeaderText="Producto con IVA" SortExpression="IVA"  >
                                        <EditItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkIVAMod" Checked='<%# Eval("IVA") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkIVAModVista" Checked='<%# Eval("IVA") %>' Enabled="false" />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho50px" />
                                    </asp:TemplateField>          
                                    <asp:TemplateField HeaderText="Venta a Granel" SortExpression="granel">
                                        <EditItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkGranelMod" Checked='<%# Eval("granel") %>' />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkGranelModVista" Checked='<%# Eval("granel") %>' Enabled="false" />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="ancho50px" />
                                    </asp:TemplateField>                                
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>
                                            <asp:Button ID="btnActualizar" runat="server" CausesValidation="True" CommandName="Update" Text="Actualizar" ValidationGroup="edita" CssClass="btn-success" /><br /><br />
                                            <asp:Button ID="btnCancelar" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancelar" CssClass="btn-danger" />
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Button ID="btnEditar" runat="server" CausesValidation="False" CommandName="Edit" Text="Editar" CssClass="btn-warning" />
                                        </ItemTemplate>
                                        <ControlStyle CssClass="btn-warning ancho50px" />
                                        <ItemStyle CssClass="ancho50px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <EditItemTemplate>                                            
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Button ID="btnEliminar" runat="server" CausesValidation="False" CommandName="Delete" Text="Inactiva" CommandArgument='<%# Eval("idProducto")+";"+Eval("estatus") %>'  />
                                        </ItemTemplate>                                        
                                        <ItemStyle CssClass="ancho50px" />                                        
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle ForeColor="Red" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>"
                                SelectCommand="select p.idProducto,isnull(p.descripcion,'') as descripcionProducto,isnull(p.idMedida,'') as idMedida,
                                    isnull(u.descripcion,'') as descripcionMedida,isnull(p.idFamilia,'') as idFamilia,isnull(f.descripcionFamilia,'') as descripcionFamilia,
                                    isnull(p.idLinea,'') as idLinea,isnull(l.descripcionLinea,'') as descripcionLinea,p.detalles,p.observaciones,p.estatus,cat.descripcion_categoria,cat.id_categoria as id_categorias
                                    from catproductos p
                                    left join catunidmedida u on u.idMedida=p.idMedida
                                    left join catcategorias cat on cat.id_categoria=p.id_categoria
                                    left join catfamilias f on f.idFamilia=p.idFamilia
                                    left join catlineas l on l.idLinea=p.idLinea"><%-- 
                                    where p.idProducto like '%%' or p.descripcion like '%%' or p.idfamilia like '%%' '">--%>
                            </asp:SqlDataSource>
                            <div class="row">
                                <div class="col-lg-12 col-sm-12 alert-danger negritas center" >
                                    <asp:Label ID="lblErrores" runat="server" CssClass="errores"></asp:Label>
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="agrega" />
                                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="errores" DisplayMode="List" ValidationGroup="edita" />
                                </div>
                            </div>

                            <asp:Panel ID="PanelMask" runat="server" Visible="false" CssClass="mask"></asp:Panel>
                            
                       </ContentTemplate>
                    </asp:UpdatePanel>
                </div> 
                              
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlPrecios" runat="server" Visible="false" CssClass="popupventa padding8px" GroupingText="Precios de Venta" ScrollBars="Vertical">
                                <div class="alert-info text-center negritas">
                                    <div class="alert-info col-lg-11 col-sm-11 text-center">
                                        <asp:Label ID="lblDescripProd" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;<asp:Label ID="lblClaveProductoP" runat="server" ></asp:Label>
                                    </div>
                                    <div class=" alert-info col-lg-1 col-sm-1 text-right">
                                        <asp:LinkButton ID="btnCerrarPrecios" runat="server" ToolTip="Cerrar" CssClass="btn btn-danger" onclick="btnCerrarPrecios_Click"><i class="icon-remove"></i></asp:LinkButton>
                                    </div> 
                                </div>
                                <br /><br />
                                <asp:Panel ID="Panel1" runat="server" CssClass="ancho95 centrado center">
                                    <div class="row" align="center" >                                         
                                        <div class="col-lg-2 col-sm-2 text-left">
                                            <asp:Label ID="Label15" runat="server" Text="Precio Venta:"></asp:Label>
                                        </div>
                                        <div class="col-lg-2 col-sm-3 text-left">
                                            <asp:TextBox ID="txtVenta" runat="server" CssClass="input-small" MaxLength="15"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" BehaviorID="txtVenta_FilteredTextBoxExtender" TargetControlID="txtVenta" FilterType="Numbers, Custom" ValidChars="." />
                                            <cc1:TextBoxWatermarkExtender ID="txtVentaWatermarkExtender1" runat="server" BehaviorID="txtVenta_TextBoxWatermarkExtender" TargetControlID="txtVenta" WatermarkCssClass="water input-small" WatermarkText="P. Venta" />                    
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe indicar un precio al producto" Text="*" ValidationGroup="nuevo" ControlToValidate="txtVenta" CssClass="errores"></asp:RequiredFieldValidator>                    
                                        </div>
                                        <div class="col-lg-3 col-sm-3 text-left">
                                            <asp:CheckBox ID="chkTodos" runat="server" Text="  Todas las Tiendas" 
                                                AutoPostBack="true" oncheckedchanged="chkTodos_CheckedChanged" />                                            
                                        </div>
                                        <div class="col-lg-3 col-sm-3 text-left">
                                            <asp:DropDownList ID="ddlIsla" runat="server" DataSourceID="SqlDataSource12" DataTextField="nombre" DataValueField="idAlmacen" CssClass="input-medium"></asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource12" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="SELECT [idAlmacen], [nombre] FROM [catalmacenes] WHERE ([estatus] = 'A')"></asp:SqlDataSource>
                                        </div>
                                        <div class="col-lg-1 col-sm-1">
                                            <asp:Button ID="btnAgregaPrecio" runat="server" Text="Agregar" CssClass="btn-info" ValidationGroup="nuevo" onclick="btnAgregaPrecio_Click" />
                                        </div>
                                    </div>                                    
                                    <div class="row alert-danger negritas text-center">
                                        <asp:Label ID="lblErrorNuevo" runat="server" CssClass="errores"></asp:Label>
                                        <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="nuevo" CssClass="errores" DisplayMode="List"  />
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-6 col-sm-6 center centrado">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <div class="table-responsive">
                                                    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered center" EmptyDataText="No existen Tiendas Registradas" DataKeyNames="idAlmacen" DataSourceID="SqlDataSource11" AllowPaging="True" EmptyDataRowStyle-ForeColor="Red" AllowSorting="True" PageSize="7">
                                                        <Columns>
                                                            <asp:BoundField DataField="idAlmacen" HeaderText="idAlmacen" ReadOnly="True" Visible="false" SortExpression="idAlmacen" />
                                                            <asp:BoundField DataField="nombre" HeaderText="Tienda" SortExpression="nombre" />
                                                            <asp:BoundField DataField="idPrecioPublico" HeaderText="idPrecioPublico" Visible="false" ReadOnly="True" SortExpression="idPrecioPublico" />
                                                            <asp:BoundField DataField="ventaUnitaria" HeaderText="Precio Venta" ReadOnly="True" SortExpression="ventaUnitaria" />
                                                            <asp:CommandField ButtonType="Button" ShowSelectButton="True" ControlStyle-CssClass="btn-success" SelectText="Seleccionar" />
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="alert-success-org" />
                                                    </asp:GridView>
                                                    </div>
                                                    <asp:SqlDataSource ID="SqlDataSource11" runat="server" ConnectionString="<%$ ConnectionStrings:PVW %>" SelectCommand="select tabla.idAlmacen,tabla.nombre,tabla.idPrecioPublico,cast((select isnull((select ventaUnitaria from precios_venta where idalmacen=tabla.idAlmacen and idproducto=@idProducto and idpreciopublico=tabla.idPrecioPublico),0)) as decimal(12,2)) as ventaUnitaria from (select c.idAlmacen,c.nombre,cast((select isnull((select aa.idpreciopublico from articulosalmacen aa where aa.idalmacen=c.idAlmacen and aa.idarticulo=@idProducto),0)) as int) as idPrecioPublico from catalmacenes c) as tabla">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="lblClaveProductoP" Name="idProducto" PropertyName="Text" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>                                            
                                        </div> 
                                        <div class="col-lg-6 col-sm-6 center centrado">
                                            <div>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
                                                            GridLines="None" AllowPaging="True" AllowSorting="True" PageSize="8" 
                                                            DataSourceID="SqlDataSource10" DataKeyNames="idPrecioPublico" 
                                                            CssClass=" table table-bordered center centrado" 
                                                            EmptyDataText="No existen precios de venta para mostrar" 
                                                            EmptyDataRowStyle-ForeColor="Red" 
                                                            onselectedindexchanged="GridView2_SelectedIndexChanged" 
                                                            onrowdeleted="GridView2_RowDeleted">
                                                            <Columns>
                                                                <asp:BoundField DataField="idPrecioPublico" HeaderText="idPrecioPublico" Visible="false" SortExpression="idPrecioPublico" />
                                                                <asp:BoundField DataField="ventaUnitaria" HeaderText="Precio Venta" SortExpression="ventaUnitaria" />
                                                                <asp:BoundField DataField="fecha" HeaderText="Fecha Modificación" SortExpression="fecha" />
                                                                <asp:BoundField DataField="usuario" HeaderText="Usuario" SortExpression="usuario" />
                                                                <asp:CommandField ButtonType="Button" ShowDeleteButton="True" ControlStyle-CssClass="btn-danger" DeleteText="Eliminar"  />
                                                            </Columns>
                                                        </asp:GridView>                                        
                                                        <asp:SqlDataSource ID="SqlDataSource10" runat="server" 
                                                            ConnectionString="<%$ ConnectionStrings:PVW %>" 
                                                            SelectCommand="select idPrecioPublico,ventaUnitaria,convert(char(10),fecha,126) as fecha,upper(usuario) as usuario from precios_venta where idProducto=@idProducto and idAlmacen=@idAlmacen order by fecha DESC,ventaUnitaria desc"
                                                            DeleteCommand="delete from precios_venta where idProducto=@idProducto and idPrecioPublico=@idPrecioPublico and idAlmacen=@idAlmacen">
                                                            <DeleteParameters>
                                                                <asp:ControlParameter ControlID="lblClaveProductoP" Name="idProducto" PropertyName="Text" />
                                                                <asp:ControlParameter ControlID="GridView2" Name="idPrecioPublico" />
                                                                <asp:ControlParameter ControlID="GridView3" Name="idAlmacen" PropertyName="SelectedValue" />
                                                            </DeleteParameters>                                                            
                                                            <SelectParameters>
                                                                <asp:ControlParameter ControlID="lblClaveProductoP" Name="idProducto" 
                                                                    PropertyName="Text" />
                                                                <asp:ControlParameter ControlID="GridView3" Name="idAlmacen" 
                                                                    PropertyName="SelectedValue" />
                                                            </SelectParameters>
                                                        </asp:SqlDataSource>                                        
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                       </div> 
                                   </div>
                                </asp:Panel>                               
                                                              
                            </asp:Panel>  
                    </ContentTemplate>
                </asp:UpdatePanel> 
         
    </div>     
</asp:Content>