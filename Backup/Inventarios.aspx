<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Inventarios.aspx.cs" Inherits="Medicuri.Inventarios" EnableSessionState="True" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="FiltroReportes" TagName="Filtro" Src="~/FiltroReportes.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Inventarios" />
    <div id="divCatalogo">
        <asp:Panel ID="pnlCatalogo" runat="server" Height="350px" ScrollBars="Auto">
            <asp:UpdatePanel ID="upnCatalogo" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gdvDatos" runat="server" AutoGenerateColumns="False"  
                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                    CellPadding="3" DataKeyNames="idProAlmStocks" ForeColor="Black" 
                    GridLines="Vertical" Width="845px" AllowPaging="True" AllowSorting="True" 
                    onpageindexchanging="gdvDatos_PageIndexChanging" onsorting="gdvDatos_Sorting" 
                    PageSize="100">
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                            ShowSelectButton="True" />
                        <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                        <asp:BoundField DataField="Producto" HeaderText="Producto" 
                            SortExpression="Producto" />
                        <asp:BoundField DataField="StockMin" HeaderText="Stock mínimo" 
                            SortExpression="StockMin" />
                        <asp:BoundField DataField="StockMax" HeaderText="Stock máximo" 
                            SortExpression="StockMax" />
                        <asp:BoundField DataField="Almacen" HeaderText="Almacen" 
                            SortExpression="Almacen" />
                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" 
                            SortExpression="Cantidad" />
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
    <div id="divFormulario">
        <asp:Panel ID="pnlFormulario" runat="server">
            <asp:TabContainer ID="tbcFormulario" runat="server" ActiveTabIndex="0" Width="840px">
                <asp:TabPanel ID="tbpDatosInventario" runat="server" HeaderText="Datos del Inventario">
                    <HeaderTemplate>
                        Datos del Inventario
                    </HeaderTemplate>
                    <ContentTemplate>
                        <asp:Panel ID="pnlDatosGenerales" runat="server" GroupingText="Datos Generales">
                            <asp:UpdatePanel ID="upnFormulario" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblClave" runat="server" Text="Clave:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbClave" runat="server" Width="130px" ReadOnly="True"></asp:TextBox>
                                    <asp:Label ID="lblProducto" runat="server" Text="Producto:" 
                                        CssClass="labelForms" Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbProducto" runat="server" Width="130px" ReadOnly="True" 
                                        ViewStateMode="Disabled"></asp:TextBox>
                                    <asp:Label ID="lblAlmacen" runat="server" Text="Almacén:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbAlmacen" runat="server" Width="130px" ReadOnly="True"></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblMinimo" runat="server" Text="Stock mínimo:" 
                                        CssClass="labelForms" Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbMinimo" runat="server" ValidationGroup="Inventarios" 
                                        Width="130px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvMinimo" runat="server" ErrorMessage="Stock mínimo: campo requerido" ControlToValidate="txbMinimo" Text="*" ValidationGroup="Inventarios"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revMinimo" runat="server" ErrorMessage="Stock mínimo: campo requerido" ControlToValidate="txbMinimo" Text="*" ValidationGroup="Inventarios" ValidationExpression="^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$"></asp:RegularExpressionValidator>                                    
                                    <asp:Label ID="lblMaximo" runat="server" Text="Stock máximo:" 
                                        CssClass="labelForms" Width="64px"></asp:Label>
                                    <asp:TextBox ID="txbMaximo" runat="server" ValidationGroup="Inventarios" 
                                        Width="130px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvMaximo" runat="server" ErrorMessage="Stock máximo: campo requerido" ControlToValidate="txbMaximo" Text="*" ValidationGroup="Inventarios"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revMaximo" runat="server" ErrorMessage="Stock máximo: campo requerido" ControlToValidate="txbMaximo" Text="*" ValidationGroup="Inventarios" ValidationExpression="^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$"></asp:RegularExpressionValidator>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
        </asp:Panel>
    </div>
    <div id="divInventarioFísico" runat="server">
       <asp:UpdatePanel ID="upnInventarioFisico" runat="server">
        <ContentTemplate>
              <asp:Panel ID="pnlGeneral" runat="server" GroupingText="General">
                <asp:Label ID="lblInvFsAvisoPermanente" runat="server" Text="Label" ForeColor="Red"></asp:Label>
                <asp:Label ID="lblInvFsAvisos" runat="server" ForeColor="Red"></asp:Label><br/>
                <asp:CheckBox ID="ckbInvFsTodos" runat="server" Text="Aplicar a todos" 
                    AutoPostBack="True" oncheckedchanged="ckbInvFsTodos_CheckedChanged"  />
                  <br/>
                <asp:Label ID="Label24" runat="server" Text="Desde:" CssClass="labelForms"
                      Width="50px"></asp:Label>
                <asp:ComboBox ID="ddlInvFsClave1Desde" runat="server" CssClass="WindowsStyle" 
                      DataTextField="Clave1" DataValueField="idProducto" 
                      DropDownStyle="DropDownList"></asp:ComboBox>
                <asp:Label ID="Label25" runat="server" Text="Hasta:" CssClass="labelForms" 
                      Width="50px"></asp:Label>
                <asp:ComboBox ID="ddlInvFsClave1Hasta" runat="server" CssClass="WindowsStyle" 
                      DataTextField="Clave1" DataValueField="idProducto" 
                      DropDownStyle="DropDownList"></asp:ComboBox>
                <asp:Label ID="Label22" runat="server" Text="Almacén:" CssClass="labelForms" 
                      Width="90px"></asp:Label>
                <asp:ComboBox ID="ddlInvFsAlmacen" runat="server" CssClass="WindowsStyle" 
                      DataTextField="Nombre" DataValueField="idAlmacen" 
                      DropDownStyle="DropDownList"></asp:ComboBox>      
                &nbsp&nbsp          
                <asp:Button ID="btnBuscarFisico" runat="server" Text="Buscar" 
                      onclick="btnBuscarFisico_Click" />
              </asp:Panel>

              <asp:Panel ID="pnlArticulos" runat="server" GroupingText="Artículo">
                <asp:Table ID="TableInvFsArticulos" runat="server" Width="835px">
                    <asp:TableRow>
                       <asp:TableCell>
                         <asp:Label ID="Label17" runat="server" Text="Clave" CssClass="labelForms" Width="30px" BorderStyle="Ridge" BorderColor="Black"></asp:Label>
                       </asp:TableCell>
                        <asp:TableCell>
                          <asp:Label ID="Label19" runat="server" Text="Descripción" Width="68px"  CssClass="labelForms" BorderStyle="Ridge" BorderColor="Black"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="Label26" runat="server" Text="Lote" Width="30px" CssClass="labelForms" BorderStyle="Ridge" BorderColor="Black"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="Label27" runat="server" Text="Serie" Width="30px" CssClass="labelForms" BorderStyle="Ridge" BorderColor="Black"></asp:Label>
                        </asp:TableCell>
                         <asp:TableCell>
                             <asp:Label ID="Label20" runat="server" Text="Cant. teórica" Width="80px" CssClass="labelForms" BorderStyle="Ridge" BorderColor="Black"></asp:Label>
                        </asp:TableCell>
                           <asp:TableCell>
                               <asp:Label ID="Label21" runat="server" Text="Cant. real" Width="60px" CssClass="labelForms" BorderStyle="Ridge" BorderColor="Black"></asp:Label>
                        </asp:TableCell>
                     </asp:TableRow>
                       <asp:TableRow>
                           <asp:TableCell>
                                <asp:TextBox ID="txtInvFsClave" runat="server" CssClass="textBoxForms"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>
                              <asp:TextBox ID="txtInvFsDescripcionArticulo" runat="server" CssClass="textBoxForms"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="txtInvFsLoteArticulo" runat="server" CssClass="textBoxForms"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>
                              <asp:TextBox ID="txtInvFsSerieArticulo" runat="server" CssClass="textBoxForms"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="txtInvFsCantidadTeoricaArticulo" runat="server" CssClass="textBoxForms" Width="60px"></asp:TextBox>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="txtInvFsCantidadRealArticulo" runat="server" 
                                CssClass="textBoxForms" AutoPostBack="True" 
                                ontextchanged="txtInvFsCantidadRealArticulo_TextChanged" Width="60px"></asp:TextBox>
                            </asp:TableCell>
                       </asp:TableRow>
                   </asp:Table>
                 
                  <asp:GridView ID="grvInvFsArticulos" runat="server" AutoGenerateColumns="False" 
                      AllowPaging="True" 
                      ShowHeaderWhenEmpty="True" BackColor="White" BorderColor="#999999" 
                      BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" 
                      GridLines="Vertical" Width="840px" 
                      onselectedindexchanged="grvInvFsArticulos_SelectedIndexChanged" 
                      onrowcreated="grvInvFsArticulos_RowCreated" 
                      onpageindexchanging="grvInvFsArticulos_PageIndexChanging" >
                      <AlternatingRowStyle BackColor="#CCCCCC" />
                      <Columns>
                      	  <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                              ShowSelectButton="True">
                              <HeaderStyle Width="20px" />
                          </asp:CommandField>
                      	  <asp:BoundField HeaderText="Clave" DataField="codigo"/>
                          <asp:BoundField HeaderText="Descripción" DataField="descripcion" />
                          <asp:BoundField HeaderText="Lote" DataField="lote"/>
                          <asp:BoundField HeaderText="Serie" DataField="serie"/>
                          <asp:BoundField HeaderText="Cantidad teórica" DataField="existenciaTeorica"/>
                          <asp:BoundField HeaderText="Cantidad Real" DataField="existenciaReal"/>
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
                  <asp:Button ID="btnGuardarFisico" runat="server" 
                      Text="Guardar" onclick="btnGuardarFisico_Click" />
              </asp:Panel>
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
    <div>
        <asp:UpdatePanel ID="pnlAvisos" runat="server">
            <ContentTemplate>
                <asp:Label id="lblAviso" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label id="lblAviso2" runat="server" Text=""></asp:Label>                
            </ContentTemplate>            
        </asp:UpdatePanel>
    </div>
    <div style="margin-left:60px">
        <asp:Panel ID="pnlFiltroReportes" runat="server">
            <FiltroReportes:Filtro runat="server" ID="frReportes" />
        </asp:Panel>
    </div>
</asp:Content>
