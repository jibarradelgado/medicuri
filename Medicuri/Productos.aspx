<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="Medicuri.Productos" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="FiltroReportes" TagName="Filtro" Src="~/FiltroReportes.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
    <style type="text/css">
        .ajax__combobox_buttoncontainer BUTTON
        {
            width: 20px !important;
            height: 21px !important;
        }
        .ajax__combobox_itemlist 
         {
          display: list-item !important;
          width: 110px !important;
          height: auto !important;                 
         }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <div><asp:Label ID="lblResults" runat="server" Font-Bold="True"></asp:Label></div>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>

     <%-- Items List --%>
    <div id="divCatalogo">
        <asp:Panel ID="pnlList" runat="server" Height="350px" ScrollBars="Auto">
            <asp:UpdatePanel ID="upnList" runat="server">
            <ContentTemplate>
                <asp:GridView ID="dgvProducto" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                    CellPadding="3" ForeColor="Black" GridLines="Vertical" Width="845px" 
                    onrowcreated="dgvProducto_RowCreated" DataKeyNames="idProducto" 
                    AllowPaging="True" AllowSorting="True" 
                    onpageindexchanging="dgvProducto_PageIndexChanging" 
                    onsorting="dgvProducto_Sorting" PageSize="100">
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                            SelectText="" ShowSelectButton="True" />
                        <asp:BoundField DataField="idProducto" HeaderText="idProducto" 
                            SortExpression="idProducto" Visible="False" />
                        <asp:BoundField DataField="idTipoIva" HeaderText="idTipoIva" 
                            SortExpression="idTipoIva" Visible="False" />
                        <asp:BoundField DataField="Clave1" HeaderText="Clave1" 
                            SortExpression="Clave1" />
                        <asp:BoundField DataField="Clave2" HeaderText="Clave2" 
                            SortExpression="Clave2" />
                        <asp:BoundField DataField="Clave3" HeaderText="Clave3" 
                            SortExpression="Clave3" Visible="False" />
                        <asp:BoundField DataField="Clave4" HeaderText="Clave4" 
                            SortExpression="Clave4" Visible="False" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" 
                            SortExpression="Nombre" />
                        <asp:BoundField DataField="Presentacion" HeaderText="Presentacion" 
                            SortExpression="Presentacion" />
                        <asp:BoundField DataField="StockMinimo" HeaderText="Stock Mínimo" 
                            SortExpression="StockMinimo" />
                        <asp:BoundField DataField="StockMaximo" HeaderText="Stock Máximo" 
                            SortExpression="StockMaximo" />
                        <asp:BoundField DataField="PrecioMinimo" HeaderText="Precio Mínimo" 
                            SortExpression="PrecioMinimo" />
                        <asp:BoundField DataField="PrecioPublico" HeaderText="Precio Público" 
                            SortExpression="PrecioPublico" />
                        <asp:BoundField DataField="UnidadMedida" HeaderText="Unidad de Medida" 
                            SortExpression="UnidadMedida" Visible="False" />
                        <asp:BoundField DataField="TipoProducto" HeaderText="Tipo de Producto" 
                            SortExpression="TipoProducto" />
                        <asp:BoundField DataField="DescripcionAdicional" 
                            HeaderText="DescripcionAdicional" SortExpression="DescripcionAdicional" 
                            Visible="False" />
                        <asp:BoundField DataField="Ubicacion" HeaderText="Ubicacion" 
                            SortExpression="Ubicacion" Visible="False" />
                        <asp:BoundField DataField="Categoria" HeaderText="Categoria" 
                            SortExpression="Categoria" Visible="False" />
                        <asp:BoundField DataField="Precio1" HeaderText="Precio1" 
                            SortExpression="Precio1" Visible="False" />
                        <asp:BoundField DataField="Precio2" HeaderText="Precio2" 
                            SortExpression="Precio2" Visible="False" />
                        <asp:BoundField DataField="Precio3" HeaderText="Precio3" 
                            SortExpression="Precio3" Visible="False" />
                        <asp:BoundField DataField="TipoMoneda" HeaderText="TipoMoneda" 
                            SortExpression="TipoMoneda" Visible="False" />
                        <asp:BoundField DataField="TasaIeps" HeaderText="TasaIeps" 
                            SortExpression="TasaIeps" Visible="False" />
                        <asp:BoundField DataField="TasaImpuesto1" HeaderText="TasaImpuesto1" 
                            SortExpression="TasaImpuesto1" Visible="False" />
                        <asp:BoundField DataField="TasaImpuesto2" HeaderText="TasaImpuesto2" 
                            SortExpression="TasaImpuesto2" Visible="False" />
                        <asp:CheckBoxField DataField="ManejaLote" HeaderText="ManejaLote" 
                            SortExpression="ManejaLote" Visible="False" />
                        <asp:CheckBoxField DataField="ManejaSeries" HeaderText="ManejaSeries" 
                            SortExpression="ManejaSeries" Visible="False" />
                        <asp:BoundField DataField="DiasResurtido" HeaderText="DiasResurtido" 
                            SortExpression="DiasResurtido" Visible="False" />
                        <asp:BoundField DataField="Campo1" HeaderText="Campo1" SortExpression="Campo1" 
                            Visible="False" />
                        <asp:BoundField DataField="Campo2" HeaderText="Campo2" SortExpression="Campo2" 
                            Visible="False" />
                        <asp:BoundField DataField="Campo3" HeaderText="Campo3" SortExpression="Campo3" 
                            Visible="False" />
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" 
                            SortExpression="Activo" />
                        <asp:BoundField DataField="Campo4" HeaderText="Campo4" SortExpression="Campo4" 
                            Visible="False" />
                        <asp:BoundField DataField="Campo5" HeaderText="Campo5" SortExpression="Campo5" 
                            Visible="False" />
                        <asp:BoundField DataField="Campo6" HeaderText="Campo6" SortExpression="Campo6" 
                            Visible="False" />
                        <asp:BoundField DataField="Campo7" HeaderText="Campo7" SortExpression="Campo7" 
                            Visible="False" />
                        <asp:BoundField DataField="Campo8" HeaderText="Campo8" SortExpression="Campo8" 
                            Visible="False" />
                        <asp:BoundField DataField="Campo9" HeaderText="Campo9" SortExpression="Campo9" 
                            Visible="False" />
                        <asp:BoundField DataField="Campo10" HeaderText="Campo10" 
                            SortExpression="Campo10" Visible="False" />
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" />
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#808080" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#383838" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        </asp:Panel>
        
    </div>
    <br />

    <%-- Item Form --%>
    <div id="divFormulario">
    <asp:UpdatePanel ID="upnForm" runat="server">
            <ContentTemplate>
                <asp:TabContainer ID="tabContainerPrincipal" runat="server" Width="845px" 
                    ActiveTabIndex="0">
                   
                   <%-- Datos generales de producto --%>
                    <asp:TabPanel ID="tabGeneral" runat="server" HeaderText="General">
                        <ContentTemplate>
                            <asp:TabContainer ID="TabContainerProducto" runat="server" Width="840px" 
                                ActiveTabIndex="0">
                             
                                <asp:TabPanel runat="server" HeaderText="Datos de Identificación" ID="tabDatosIdentificacion">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlDatosIdentificacion" runat="server" GroupingText="Claves">
                                            <asp:Label ID="lblClave1" runat="server" Text="Clave 1:" CssClass="labelForms" 
                                                    Width="90px"></asp:Label>  
                                            <asp:TextBox ID="txbClave1" runat="server" CssClass="textBoxForms" 
                                                Width="130px" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                ControlToValidate="txbClave1" ErrorMessage="Campo requerido: Clave1" 
                                                ForeColor="Red" ValidationGroup="vgProductos">*</asp:RequiredFieldValidator>
                                            <asp:Label ID="lblClave2" runat="server" Text="Clave 2:" CssClass="labelForms" 
                                                    Width="77px"></asp:Label>  
                                            <asp:TextBox ID="txbClave2" runat="server" CssClass="textBoxForms" 
                                                Width="130px" AutoPostBack="True" ontextchanged="txbClave2_TextChanged"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server"
                                                DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                                                ServiceMethod="RecuperarClave2Producto" ServicePath="BusquedasAsincronas.asmx" 
                                                TargetControlID="txbClave2" MinimumPrefixLength="1">
                                            </asp:AutoCompleteExtender>
                                            <asp:Label ID="lblClave3" runat="server" Text="Clave 3:" CssClass="labelForms" 
                                                    Width="90px"></asp:Label>  
                                            <asp:TextBox ID="txbClave3" runat="server" CssClass="textBoxForms" 
                                                Width="130px"></asp:TextBox>
                                            <br />
                                            
                                            <asp:Label ID="lblClave4" runat="server" Text="Clave 4:" CssClass="labelForms" 
                                                    Width="90px"></asp:Label>  
                                            <asp:TextBox ID="txbClave4" runat="server" CssClass="textBoxForms" 
                                                Width="130px"></asp:TextBox>
                                            <asp:Label ID="lblNombre" runat="server" Text="Nombre:" 
                                                    CssClass="labelForms" Width="90px"></asp:Label>
                                            <asp:TextBox ID="txbNombre" runat="server" Width="350px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                                ControlToValidate="txbNombre" ErrorMessage="Campo requerido: Nombre" 
                                                ForeColor="Red" ValidationGroup="vgProductos">*</asp:RequiredFieldValidator>
                                            <br />                                            
                                            <asp:Label ID="lblDescripcion" runat="server" Text="Descripción:" 
                                                    CssClass="labelForms" Width="90px"></asp:Label>
                                            <asp:TextBox ID="txaDescripcion" runat="server" Width="570px" 
                                                CssClass="textBoxForms" MaxLength="255"></asp:TextBox>
                                            <br />                                                                                      
                                            <asp:Label ID="lblTipoProducto" runat="server" Text="Tipo:" CssClass="labelForms" 
                                                    Width="90px"></asp:Label>
                                            <asp:ComboBox ID="cmbTipo" runat="server" CssClass="textBoxForms" 
                                                DataTextField="Nombre" DataValueField="idTipo" 
                                                Width="110px" MaxLength="0" AppendDataBoundItems="True" 
                                                DropDownStyle="DropDownList">
                                            </asp:ComboBox>
                                            &nbsp&nbsp&nbsp
                                            <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" CssClass="labelForms" Width="100px"/>  
                                        </asp:Panel><br />
                                        <asp:Panel ID="pnlPresentacion" runat="server" GroupingText="Presentación">
                                            <asp:Label ID="lblPresentacion" runat="server" Text="Presentación:" CssClass="labelForms" 
                                                Width="90px"></asp:Label> 
                                            <asp:TextBox ID="txbPresentacion" runat="server" CssClass="textBoxForms" 
                                                Width="575px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                                ControlToValidate="txbPresentacion" 
                                                ErrorMessage="Campo requerido: Presentación" ForeColor="Red" 
                                                ValidationGroup="vgProductos">*</asp:RequiredFieldValidator>
                                            <br />
                                            <asp:Label ID="lblUnidadMedida" runat="server" Text="Unidad de Medida:" CssClass="labelForms" 
                                                Width="90px"></asp:Label> 
                                            <asp:TextBox ID="txbUnidadMedida" runat="server" CssClass="textBoxForms" 
                                                Width="130px" ></asp:TextBox>
                                            <br />
                                            <asp:Label ID="lblDescripcionAdicional" runat="server" 
                                                Text="Descripción Adicional:" CssClass="labelForms" 
                                                Width="90px"></asp:Label> 
                                            <asp:TextBox ID="txaDescripcionAdicional" runat="server"  Width="570px" 
                                                CssClass="textBoxForms" MaxLength="255"></asp:TextBox>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:TabPanel>

                                <asp:TabPanel runat="server" HeaderText="Datos del Proveedor" ID="tabDatosProveedor">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlDatosProveedor" runat="server" GroupingText="General">
                                            <asp:Label ID="lblCodigoProveedor" runat="server" Text="Codigo del Proveedor:" CssClass="labelForms" 
                                                Width="90px"></asp:Label>
                                            <asp:TextBox ID="txbCodigoProveedor" runat="server" CssClass="textBoxForms" 
                                                OnTextChanged="txbCodigoProveedor_TextChanged" AutoPostBack="True" 
                                                Width="130px"></asp:TextBox>
                                            <asp:AutoCompleteExtender ID="txbCodigoProveedor_AutoCompleteExtender" runat="server" 
                                                DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                                                ServiceMethod="RecuperarNombreProveedor" ServicePath="BusquedasAsincronas.asmx" 
                                                TargetControlID="txbCodigoProveedor" MinimumPrefixLength="1">
                                            </asp:AutoCompleteExtender>
                                            <asp:Label ID="lblNombreProveedor" runat="server" Text="Nombre:" 
                                                CssClass="labelForms" Width="90px"></asp:Label>
                                            <asp:TextBox ID="txbNombreProveedor" runat="server" ReadOnly="True" 
                                                Width="130px"></asp:TextBox>                                            
                                            <asp:Label ID="lblTelefonoProveedor" runat="server" Text="Teléfono:" CssClass="labelForms" 
                                                Width="90px"></asp:Label> 
                                            <asp:TextBox ID="txbTelefonoProveedor" runat="server" CssClass="textBoxForms" 
                                                Width="130px" ReadOnly="True"></asp:TextBox>
                                            &nbsp&nbsp&nbsp
                                            <asp:ImageButton ID="imbAgregarProveedor" runat="server" 
                                                ImageUrl="~/Icons/plus_16.png" ValidationGroup="Proveedores" 
                                                onclick="imbAgregarProveedor_Click" />
                                            <br />
                                            <asp:GridView ID="gdvCatalogoProveedor" runat="server" 
                                                    AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" 
                                                    BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" 
                                                    GridLines="Vertical" Width="840px" 
                                                DataKeyNames="idProveedorProducto,idProducto,idProveedor" 
                                                onrowcreated="gdvCatalogoProveedor_RowCreated" 
                                                onselectedindexchanged="gdvCatalogoProveedor_SelectedIndexChanged">
                                                
                                                    <AlternatingRowStyle BackColor="#CCCCCC" />
                                                    <Columns>
                                                        <asp:BoundField DataField="idProveedor" HeaderText="Clave" />
                                                        <asp:BoundField DataField="idProveedor" HeaderText="Nombre" />
                                                        <asp:BoundField DataField="idProveedor" HeaderText="Teléfono" />
                                                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/delete_16.png" 
                                                            ShowSelectButton="True">
                                                        <HeaderStyle Width="30px" />
                                                        </asp:CommandField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCCCC" />
                                                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                                    <sortedascendingcellstyle backcolor="#F1F1F1" />
                                                    <sortedascendingheaderstyle backcolor="Gray" />
                                                    <sorteddescendingcellstyle backcolor="#CAC9C9" />
                                                    <sorteddescendingheaderstyle backcolor="#383838" />                                                
                                                </asp:GridView>
                                        </asp:Panel>                                        
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="tabDatosOpcionales" runat="server" HeaderText="Datos Opcionales">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlaAlfanumericos" runat="server" GroupingText="Campos Alfanumericos">
                                            <asp:Label ID="lblAlfanumerico1" runat="server" Text="Campo 1:" 
                                                CssClass="labelForms" Width="90px"></asp:Label>
                                            <asp:TextBox ID="txbAlfanumerico1" runat="server" CssClass="textBoxForms" 
                                                Width="130px"></asp:TextBox>
                                            <asp:Label ID="lblAlfanumerico2" runat="server" Text="Campo 2:" 
                                                CssClass="labelForms" Width="90px"></asp:Label>
                                            <asp:TextBox ID="txbAlfanumerico2" runat="server" CssClass="textBoxForms" 
                                                Width="130px"></asp:TextBox>
                                            <asp:Label ID="lblAlfanumerico3" runat="server" Text="Campo 3:" 
                                                CssClass="labelForms" Width="90px"></asp:Label>
                                            <asp:TextBox ID="txbAlfanumerico3" runat="server" CssClass="textBoxForms" 
                                                Width="130px"></asp:TextBox>
                                            <br />
                                            <asp:Label ID="lblAlfanumerico4" runat="server" Text="Campo 4:" 
                                                CssClass="labelForms" Width="90px"></asp:Label>
                                            <asp:TextBox ID="txbAlfanumerico4" runat="server" CssClass="textBoxForms" 
                                                Width="130px"></asp:TextBox>
                                            <asp:Label ID="lblAlfanumerico5" runat="server" Text="Campo 5:" 
                                                CssClass="labelForms" Width="90px"></asp:Label>
                                            <asp:TextBox ID="txbAlfanumerico5" runat="server" CssClass="textBoxForms" 
                                                Width="130px"></asp:TextBox>
                                            </asp:Panel>

                                            <asp:Panel ID="pnlEnteros" runat="server" GroupingText="Campos Enteros">
                                            <asp:Label ID="lblEntero1" runat="server" Text="Campo 6:" CssClass="labelForms" 
                                                Width="90px"></asp:Label>
                                            <asp:TextBox ID="txbEntero1" runat="server" CssClass="textBoxForms" Width="130px"></asp:TextBox>
                                            <asp:Label ID="lblEntero2" runat="server" Text="Campo 7:" CssClass="labelForms" 
                                                Width="90px"></asp:Label>
                                            <asp:TextBox ID="txbEntero2" runat="server" CssClass="textBoxForms" Width="130px"></asp:TextBox>
                                            <asp:Label ID="lblEntero3" runat="server" Text="Campo 8:" CssClass="labelForms" 
                                                Width="90px"></asp:Label>
                                            <asp:TextBox ID="txbEntero3" runat="server" CssClass="textBoxForms" Width="130px"></asp:TextBox>
                                                <br />
                                            </asp:Panel>

                                            <asp:Panel ID="pnlDecimales" runat="server" GroupingText="Campos Decimales">
                                            <asp:Label ID="lblDecimal1" runat="server" Text="Campo 9:" 
                                                CssClass="labelForms" Width="90px"></asp:Label>
                                            <asp:TextBox ID="txbDecimal1" runat="server" CssClass="textBoxForms" Width="130px"></asp:TextBox>
                                            <asp:Label ID="lblDecimal2" runat="server" Text="Campo 10:" 
                                                CssClass="labelForms" Width="90px"></asp:Label>
                                            <asp:TextBox ID="txbDecimal2" runat="server" CssClass="textBoxForms" Width="130px"></asp:TextBox>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:TabPanel>
                            </asp:TabContainer>
                        </ContentTemplate>
                    </asp:TabPanel>

                    <asp:TabPanel ID="tabPrecios" runat="server" HeaderText="Precios">
                        <ContentTemplate>
                            <asp:Panel ID="pnlInfoGralPrecios" runat="server" GroupingText="Información General">
                                <asp:Label ID="lblPublico" runat="server" Text="Público:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbPublico" runat="server" CssClass="textBoxForms" 
                                    Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                    ControlToValidate="txbPublico" ErrorMessage="Campo requerido: Público" 
                                    ForeColor="Red" ValidationGroup="vgProductos">*</asp:RequiredFieldValidator>
                                <asp:Label ID="lblMinimo" runat="server" Text="Mínimo:" CssClass="labelForms" 
                                    Width="77px"></asp:Label>
                                <asp:TextBox ID="txbMinimo" runat="server" CssClass="textBoxForms" 
                                    Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                    ControlToValidate="txbMinimo" ErrorMessage="Campo requerido: Mínimo" 
                                    ForeColor="Red" ValidationGroup="vgProductos">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="lblPrecio1" runat="server" Text="Precio 1:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbPrecio1" runat="server" CssClass="textBoxForms" 
                                    Width="130px"></asp:TextBox>
                                <asp:CompareValidator ID="cvPrecio1Menor" runat="server" 
                                    ErrorMessage="El precio1 debe ser menor al precio público" 
                                    ForeColor="Red" ValidationGroup="vgProductos" 
                                    ControlToCompare="txbPublico" ControlToValidate="txbPrecio1" 
                                    Operator="LessThanEqual" Type="Currency">*</asp:CompareValidator>
                                <asp:CompareValidator ID="cvPrecio1Mayor" runat="server" 
                                    ErrorMessage="El precio1 debe ser mayor al precio mínimo" 
                                    ForeColor="Red" ValidationGroup="vgProductos" 
                                    ControlToCompare="txbMinimo" ControlToValidate="txbPrecio1" 
                                    Operator="GreaterThanEqual" Type="Currency">*</asp:CompareValidator>
                                <asp:Label ID="lblPrecio2" runat="server" Text="Precio 2:" CssClass="labelForms" 
                                    Width="64px"></asp:Label>
                                <asp:TextBox ID="txbPrecio2" runat="server" CssClass="textBoxForms" 
                                    Width="130px"></asp:TextBox>
                                <asp:CompareValidator ID="cvPrecio2Mayor" runat="server" 
                                    ErrorMessage="El precio2 debe ser mayor al precio mínimo" 
                                    ForeColor="Red" ValidationGroup="vgProductos" 
                                    ControlToCompare="txbMinimo" 
                                    Operator="GreaterThanEqual" Type="Currency"
                                    ControlToValidate="txbPrecio2">*</asp:CompareValidator>
                                <asp:CompareValidator ID="cvPrecio2Menor" runat="server" 
                                    ErrorMessage="El precio2 debe ser menor al precio público" 
                                    ForeColor="Red" ValidationGroup="vgProductos" 
                                    ControlToCompare="txbPublico" 
                                    Operator="LessThanEqual" Type="Currency"
                                    ControlToValidate="txbPrecio2">*</asp:CompareValidator>
                                <asp:Label ID="lblPrecio3" runat="server" Text="Precio 3:" CssClass="labelForms" 
                                    Width="64px"></asp:Label>
                                <asp:TextBox ID="txbPrecio3" runat="server" CssClass="textBoxForms" 
                                    Width="130px"></asp:TextBox>
                                <asp:CompareValidator ID="cvPrecio3Mayor" runat="server" 
                                    ErrorMessage="El precio3 debe ser mayor al precio mínimo" 
                                    ForeColor="Red" ValidationGroup="vgProductos" 
                                    ControlToCompare="txbMinimo" 
                                    Operator="GreaterThanEqual" Type="Currency"
                                    ControlToValidate="txbPrecio3">*</asp:CompareValidator>
                                <asp:CompareValidator ID="cvPrecio3Menor" runat="server" 
                                    ErrorMessage="El precio3 debe ser menor al precio público" 
                                    ForeColor="Red" ValidationGroup="vgProductos" 
                                    ControlToCompare="txbPublico" 
                                    Operator="LessThanEqual" Type="Currency"
                                    ControlToValidate="txbPrecio3">*</asp:CompareValidator>
                                <br />
                                <asp:Label ID="lblIVA" runat="server" Text="IVA:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:ComboBox ID="cmbIVA" runat="server" AppendDataBoundItems="True" 
                                    CssClass="textBoxForms" DataTextField="Zona" DataValueField="idTipoIva" 
                                    Width="110px" MaxLength="0" DropDownStyle="DropDownList">
                                </asp:ComboBox>                                 
                                <asp:Label ID="lblIEPS" runat="server" Text="Tasa IEPS:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbIEPS" runat="server" CssClass="textBoxForms" Width="130px"></asp:TextBox>                                
                                <asp:Label ID="lblCosteo" runat="server" Text="Costeo:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:ComboBox ID="cmbCosteo" runat="server" Width="110px" MaxLength="0" 
                                    AppendDataBoundItems="True" DropDownStyle="DropDownList">
                                    <asp:ListItem>Promedio</asp:ListItem>
                                    <asp:ListItem>PEPS</asp:ListItem>
                                    <asp:ListItem>UEPS</asp:ListItem>
                                </asp:ComboBox>       
                                <br />                        
                                <asp:Label ID="lblMoneda" runat="server" Text="Moneda:" CssClass="labelForms" 
                                    Width="90px" Visible="False"></asp:Label>
                                <asp:TextBox ID="txbMondea" runat="server" CssClass="textBoxForms" 
                                    Width="130px" Visible="False"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revMoneda" runat="server" 
                                    ForeColor="Red" ValidationGroup="vgProductos" 
                                    ControlToValidate="txbMondea" ValidationExpression="\w{0,3}"
                                    ErrorMessage="Campo Moneda: No puede ser mayor a 3 carácteres" 
                                    Enabled="False" Visible="False">*</asp:RegularExpressionValidator>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>

                    <asp:TabPanel ID="tabInventarios" runat="server" HeaderText="Inventarios">
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server">
                                <asp:CheckBox ID="chkLote" runat="server" Text="Maneja número de lote" 
                                    CssClass="textBoxForms" Width="200px" />
                                <asp:CheckBox ID="chkSerie" runat="server" Text="Maneja número de serie" 
                                    CssClass="textBoxForms" Width="200px" />
                                <br />

                                <asp:Label ID="lblStockMaximo" runat="server" Text="Stock Maximo:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbStockMaximo" runat="server" CssClass="textBoxForms" 
                                    Width="130px">0</asp:TextBox>
                                <asp:Label ID="lblStockMinimo" runat="server" Text="Stock Minimo:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbStockMinimo" runat="server" CssClass="textBoxForms" 
                                    Width="130px">0</asp:TextBox>                                
                                <asp:Label ID="lblDiasSurtido" runat="server" Text="Días para resurtir:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbDiasSurtido" runat="server" CssClass="textBoxForms" 
                                    Width="130px">0</asp:TextBox>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                </asp:TabContainer>
                <asp:ValidationSummary ID="vsProducto" runat="server" 
                    ValidationGroup="vgProductos" ForeColor="Red" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="divCambioPrecios" runat="server">
      <asp:UpdatePanel ID="upnCambioPrecios" runat="server">
        <ContentTemplate>       
            <asp:Panel ID="pnlRangoArticulos" runat="server" GroupingText="Rango de Artículos">
                <asp:CheckBox ID="chbCmPrTodos" runat="server" Text="Aplicar a todos" 
                    AutoPostBack="True" oncheckedchanged="chbCmPrTodos_CheckedChanged" /><br/>
                <asp:Label ID="Label11" runat="server" Text="Desde:"></asp:Label>
                <asp:ComboBox ID="ddlCmPrDesde" runat="server" Width="110px" 
                    DataTextField="Clave1" DataValueField="idProducto"></asp:ComboBox>                
                <asp:Label ID="Label12" runat="server" Text="Hasta:"></asp:Label>
                <asp:ComboBox ID="ddlCmPrHasta" runat="server" Width="110px" 
                    DataTextField="Clave1" DataValueField="idProducto"></asp:ComboBox>                
            </asp:Panel><br />
             <asp:Panel ID="pnlOperativo" runat="server" GroupingText="Operativo">
                <asp:Label ID="lbCmPrAvisos" runat="server" Text="" CssClass="labelForms" ForeColor="Red"></asp:Label><br />
                <asp:Label ID="Label13" runat="server" Text="Lista de precios:" 
                     CssClass="labelForms" Width="90px"></asp:Label>
                <asp:ComboBox ID="ddlCmPrListasPrecios" runat="server" Width="110px" 
                     DropDownStyle="DropDownList">
                    <asp:ListItem>Precio público</asp:ListItem>
                    <asp:ListItem>Precio 1</asp:ListItem>
                    <asp:ListItem>Precio 2</asp:ListItem>
                    <asp:ListItem>Precio 3</asp:ListItem>
                    <asp:ListItem>Precio mínimo</asp:ListItem> 
                </asp:ComboBox>                
                <asp:Label ID="Label14" runat="server" Text="Operación:" CssClass="labelForms" 
                     Width="90px"></asp:Label>
                <asp:ComboBox ID="ddlCmPrOperacion" runat="server" Width="110px" 
                     DropDownStyle="DropDownList">
                    <asp:ListItem>Aumento</asp:ListItem>
                    <asp:ListItem>Disminución</asp:ListItem>
                </asp:ComboBox>                
                <asp:Label ID="Label15" runat="server" Text="Tipo:" CssClass="labelForms" 
                     Width="90px"></asp:Label>
                <asp:ComboBox ID="ddlCmPrTipo" runat="server" Width="110px" 
                     DropDownStyle="DropDownList">
                    <asp:ListItem>Monto</asp:ListItem>
                    <asp:ListItem>Porcentaje</asp:ListItem>
                </asp:ComboBox>
                <br />
                <asp:Label ID="Label16" runat="server" Text="Cantidad:" CssClass="labelForms" 
                    Width="90px"></asp:Label>
                <asp:TextBox ID="txtCmPrCantidad" runat="server" CssClass="textBoxForms" 
                     Width="130px"></asp:TextBox>
                <div id="Div1" runat="server" align="left" >
                    <asp:GridView ID="grvCmPrErrores" runat="server"
                      AutoGenerateColumns="False" 
                      ShowFooter="True" Visible="False" BackColor="White" BorderColor="#999999" 
                     BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" 
                     GridLines="Vertical" Width="830px">
                      <AlternatingRowStyle BackColor="#CCCCCC" />
                      <Columns>
                          <asp:BoundField HeaderText="Código" DataField="CodigoProducto" />
                          <asp:BoundField HeaderText="Mensaje" DataField="MensajeError"/>
                      </Columns>
                      <FooterStyle BackColor="#CCCCCC" />
                      <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                      <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                      <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                      <SortedAscendingCellStyle BackColor="#F1F1F1" />
                      <SortedAscendingHeaderStyle BackColor="#808080" />
                      <SortedDescendingCellStyle BackColor="#CAC9C9" />
                      <SortedDescendingHeaderStyle BackColor="#383838" />
                  </asp:GridView>
                 </div>
                 <asp:Button ID="btnGuardarCambioPrecio" runat="server" onclick="btnGuardarCambioPrecio_Click" 
                     Text="Guardar"/>
             </asp:Panel>         
      </ContentTemplate>
    </asp:UpdatePanel>
    </div>        
    <div style="margin-left:60px">
        <asp:Panel ID="pnlFiltroReportes" runat="server">
            <FiltroReportes:Filtro runat="server" ID="frReportes" />
        </asp:Panel>
    </div>
</asp:Content>
