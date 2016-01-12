<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true"
    CodeBehind="Ensambles.aspx.cs" Inherits="Medicuri.Ensambles" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <div>
        <asp:Label ID="lblResults" runat="server" Font-Bold="True"></asp:Label></div>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>
    <br />
    <%-- Catalogo --%>
    <div id="divCatalogo">
        <asp:Panel ID="pnlList" runat="server" Height="350px" ScrollBars="Auto">
            <asp:UpdatePanel ID="upnList" runat="server">
            <ContentTemplate>
                <asp:GridView ID="dgvEnsambles" runat="server" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                    CellPadding="3" ForeColor="Black"
                    GridLines="Vertical" Width="845px" DataKeyNames="idEnsamble" 
                    AllowPaging="True" AllowSorting="True" 
                    onpageindexchanging="dgvEnsambles_PageIndexChanging" 
                    onsorting="dgvEnsambles_Sorting" PageSize="100">
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" SelectText=""
                            ShowSelectButton="True">
                            <HeaderStyle Width="25px" />
                        </asp:CommandField>
                        <asp:BoundField DataField="idEnsamble" HeaderText="idEnsamble" SortExpression="idEnsamble"
                            Visible="False" />
                        <asp:BoundField DataField="ClaveBom" HeaderText="Clave Bom" SortExpression="ClaveBom">
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Descripcion" HeaderText="Nombre" 
                            SortExpression="Descripcion">
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PrecioPublico" HeaderText="Precio Público" 
                            SortExpression="PrecioPublico" />
                        <asp:BoundField DataField="PrecioMinimo" HeaderText="Precio Mínimo" 
                            SortExpression="PrecioMinimo" />
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" />
                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="Gray" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#383838" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        </asp:Panel>        
    </div>
    <br />
    <%-- Formulario --%>
    <div id="divFormulario">
        <asp:UpdatePanel ID="upnForm" runat="server">
            <ContentTemplate>
                <%-- Ensamble --%>
                <asp:Panel ID="pnlEnsamble" runat="server" GroupingText="Ensamble">
                    <asp:Label ID="lblClaveBom" runat="server" Text="Clave BOM:" CssClass="labelForms"
                        Width="90px"></asp:Label>
                    <asp:TextBox ID="txbClaveBom" runat="server" 
                        Width="130px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txbClaveBom"
                        ErrorMessage="Campor requerido: Clave BOM" ForeColor="Red" ValidationGroup="vgEnsamble">*</asp:RequiredFieldValidator>
                    <asp:Label ID="lblUnidadMedida" runat="server" Text="Unidad de medida:" CssClass="labelForms"
                        Width="77px"></asp:Label>
                    <asp:TextBox ID="txbUnidadMedida" runat="server" 
                        Width="130px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txbUnidadMedida"
                        ErrorMessage="Campo requerido: Unidad de Medida" ForeColor="Red" ValidationGroup="vgEnsamble">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revUnidadMedida" runat="server" ErrorMessage="La unidad de medida dede contener máximo 5 carácteres"
                        ForeColor="Red" ValidationGroup="vgEnsamble" ValidationExpression="\w{1,5}" ControlToValidate="txbUnidadMedida">*</asp:RegularExpressionValidator>
                    <asp:Label ID="lblDescripcion" runat="server" Text="Nombre:" CssClass="labelForms"
                        Width="64px"></asp:Label>
                    <asp:TextBox ID="txbNombre" runat="server"
                        Width="130px" MaxLength="255"></asp:TextBox>
                    <br />
                    <asp:Label ID="lblPrecioPublico" runat="server" Text="Precio Público:" 
                        CssClass="labelForms" Width="90px"></asp:Label>
                    <asp:TextBox ID="txbPrecioPublico" runat="server" Width="130px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revPrecioPublico" runat="server" 
                        ErrorMessage="Precio Público: Debe de ser un número" ForeColor="Red" 
                        ControlToValidate="txbPrecioPublico" 
                        ValidationExpression="^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$" 
                        ValidationGroup="vgEnsamble">*</asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cpvPrecioPublico1" runat="server" 
                        ErrorMessage="Precio Público: debe ser mayor o igual al precio mínimo" 
                        ControlToCompare="txbPrecioMinimo" ControlToValidate="txbPrecioPublico" 
                        Operator="GreaterThanEqual" Text="*" Type="Currency" 
                        ValidationGroup="vgEnsamble" ForeColor="Red"></asp:CompareValidator>                    
                    <asp:Label ID="lblPrecioMinimo" runat="server" Text="Precio Mínimo:" 
                        CssClass="labelForms" Width="64px"></asp:Label>
                    <asp:TextBox ID="txbPrecioMinimo" runat="server" Width="130px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revPrecioMinimo" runat="server" 
                        ErrorMessage="Precio Mínimo: Debe de ser un número" ForeColor="Red" 
                        ControlToValidate="txbPrecioMinimo" 
                        ValidationExpression="^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$" 
                        ValidationGroup="vgEnsamble">*</asp:RegularExpressionValidator>                                  
                </asp:Panel>
                <%-- Ensamble Productos --%>
                <asp:Panel ID="pnlEnsambleProductos" runat="server" GroupingText="Productos ensamble">
                    <asp:Table ID="Table1" runat="server">
                        <%-- Fila de los encabezados --%>
                        <asp:TableRow>
                            <asp:TableCell Width="125px" HorizontalAlign="Center">
                                <asp:Label ID="Label12" runat="server" Width="125px" BorderStyle="Ridge" BorderColor="Black">Clave</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="200px" HorizontalAlign="Center">
                                <asp:Label ID="Label13" runat="server" Width="200px" BorderStyle="Ridge" BorderColor="Black">Producto</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="50px" HorizontalAlign="Center">
                                <asp:Label ID="Label14" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">Cant.</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="50px" HorizontalAlign="Center">
                                <asp:Label ID="Label17" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">IEPS</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="50px" HorizontalAlign="Center">
                                <asp:Label ID="Label20" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">IVA</asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="50px" HorizontalAlign="Center">
                                <asp:Label ID="Label22" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">Precio</asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                        <%-- Fila de los Campos de texto --%>
                        <asp:TableRow>
                            <asp:TableCell Width="125px">
                                <asp:TextBox ID="txbClave" runat="server" Width="125px" OnTextChanged="txbClave_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="txbClave_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                    Enabled="True" FirstRowSelected="True" ServiceMethod="RecuperarClave1Producto"
                                    ServicePath="BusquedasAsincronas.asmx" TargetControlID="txbClave" MinimumPrefixLength="1">
                                </asp:AutoCompleteExtender>
                            </asp:TableCell>
                            <asp:TableCell Width="200px">
                                <asp:TextBox ID="txbProducto" runat="server" Width="200px" AutoPostBack="True" OnTextChanged="txbProducto_TextChanged"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="txbProducto_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                    Enabled="True" FirstRowSelected="True" ServiceMethod="RecuperarNombreProducto"
                                    ServicePath="BusquedasAsincronas.asmx" TargetControlID="txbProducto" MinimumPrefixLength="1">
                                </asp:AutoCompleteExtender>
                            </asp:TableCell>
                            <asp:TableCell Width="50px">
                                <asp:TextBox ID="txbCant" runat="server" Width="50px"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell Width="50px">
                                <asp:TextBox ID="txbIeps" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell Width="50px">
                                <asp:TextBox ID="txbIva" runat="server" Width="50px" Enabled="false"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell Width="50px">
                                <asp:DropDownList ID="cmbPrecios" runat="server">
                                </asp:DropDownList>
                            </asp:TableCell>
                            <asp:TableCell Width="25px">
                                <asp:ImageButton ID="imbAgregarDetalle" runat="server" ImageUrl="~/Icons/plus_16.png"
                                    Height="16px" OnClick="imbAgregarDetalle_Click" ValidationGroup="vgEnsambleProducto" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <%-- Validadores --%>
                    <asp:RequiredFieldValidator ID="rfvClaveProducto" runat="server" ControlToValidate="txbClave"
                        ErrorMessage="Campo requerido: Clave producto" ForeColor="Red" ValidationGroup="vgEnsambleProducto">*</asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="rfvCantidadProducto" runat="server" ControlToValidate="txbCant"
                        ErrorMessage="Campo requerido: Cantidad producto" ForeColor="Red" ValidationGroup="vgEnsambleProducto">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revCantidadProducto" runat="server" ForeColor="Red"
                        ErrorMessage="El campo cantidad solo puede contener números" ValidationExpression="\d*"
                        ValidationGroup="vgEnsambleProducto" ControlToValidate="txbCant">*</asp:RegularExpressionValidator>
                    <br />
                    <asp:GridView ID="dgvEnsambleProductos" runat="server" AutoGenerateColumns="False"
                        BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                        CellPadding="3" ForeColor="Black" GridLines="Vertical" 
                        OnSelectedIndexChanged="grvProductos_SelectedIndexChanged" Width="840px">
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:BoundField DataField="idProducto" HeaderText="idProducto" SortExpression="idProducto"
                                Visible="False" />
                            <asp:BoundField DataField="idEnsamble" HeaderText="idEnsamble" SortExpression="idEnsamble"
                                Visible="False" />
                            <asp:BoundField DataField="Clave1" HeaderText="Clave" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                            <asp:BoundField DataField="Presentacion" HeaderText="Presentación" SortExpression="Presentacion" />
                            <asp:BoundField DataField="PrecioPublico" HeaderText="Precio público" SortExpression="PrecioPublico" />
                            <asp:BoundField HeaderText="Cantidad" DataField="Cantidad" SortExpression="Cantidad" />
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/delete_16.png" ShowSelectButton="True" />
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
                </asp:Panel>
                <asp:ValidationSummary ID="vsEnsamble" runat="server" ForeColor="Red" ValidationGroup="vgEnsamble" />
                <asp:ValidationSummary ID="vsEnsambleProducto" runat="server" ForeColor="Red" ValidationGroup="vgEnsambleProducto" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
   <%-- <div>
        <asp:Panel ID="pnlReportes" runat="server" Width="845px" Height="440px" ScrollBars="Auto">        
            <asp:Panel ID="pnlBotones" runat="server">
                <asp:Button ID="btnPdf" runat="server" Text="Exportar a PDF" 
                    onclick="btnPdf_Click" />
                &nbsp&nbsp&nbsp&nbsp
                <asp:Button ID="btnExcel" runat="server" Text="Exportar a Excel" 
                    onclick="btnExcel_Click" />
                &nbsp&nbsp&nbsp&nbsp
                <asp:Button ID="btnCrystal" runat="server" Text="Exportar a Crystal Reports" 
                    onclick="btnCrystal_Click" />
            </asp:Panel>
            <asp:Panel ID="pnlReporte" runat="server">
                <asp:UpdatePanel ID="upnReportes" runat="server">
                <ContentTemplate>   
                <CR:CrystalReportViewer ID="crvReporte" runat="server" AutoDataBind="true" 
                    HasCrystalLogo="False" HasExportButton="False" HasToggleGroupTreeButton="True" 
                    onnavigate="crvReporte_Navigate" 
                    onsearch="crvReporte_Search" onviewzoom="crvReporte_ViewZoom" 
                    ondrill="crvReporte_Drill" HasPrintButton="False" 
                    ondatabinding="crvReporte_DataBinding" 
                    ondrilldownsubreport="crvReporte_DrillDownSubreport"/>
                </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>        
        </asp:Panel>
    </div>    --%>
</asp:Content>
