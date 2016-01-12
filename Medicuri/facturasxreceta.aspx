<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="facturasxreceta.aspx.cs" Inherits="Medicuri.facturasxporgrama" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>

    <div id="Listado">
        <asp:UpdatePanel ID="pnlCatalogoTotales" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel2" runat="server" Height="350px" ScrollBars="Auto">
              
                <asp:GridView ID="dgvFacturacionTotales" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="idPedido" BackColor="White" BorderColor="#999999" 
                        BorderStyle="Solid" BorderWidth="1px" CellPadding="3"      ForeColor="Black" 
                        GridLines="Vertical" AllowPaging="True" PageSize="100" AllowSorting="True" 
                        onpageindexchanging="dgvFacturacionTotales_PageIndexChanging" 
                        onsorting="dgvFacturacionTotales_Sorting">
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    
                    <Columns>
                     <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                        SelectText="-" ShowSelectButton="True" />

                        <asp:BoundField DataField="FuenteCuenta" HeaderText="Programa" 
                            SortExpression="FuenteCuenta" >

                            <ItemStyle Width="300px" />
                        </asp:BoundField>

                       <%--<asp:BoundField DataField="idCliente" HeaderText="idCliente" 
                            SortExpression="idCliente" Visible="False" />--%>

                        <asp:BoundField DataField="Monto" HeaderText="Monto" DataFormatString="{0:C}" 
                            SortExpression="Monto" >
                            <ItemStyle Width="200px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Facturado" HeaderText="Facturado" 
                            DataFormatString="{0:C}" SortExpression="Facturado" >
                            <ItemStyle Width="200px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:BoundField DataField="FechaVencimiento" HeaderText="Vence" 
                            DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaVencimiento">
                            <ItemStyle Width="150px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>

                        <asp:CheckBoxField DataField="Activo" HeaderText="Activa" />

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
            </ContentTemplate>

            
        </asp:UpdatePanel>
    
    </div>

     <asp:UpdatePanel ID="pnlFormulario" runat="server">
            <ContentTemplate>
     <asp:Panel ID="pnlDatos" runat="server" GroupingText="Datos">
         <asp:DropDownList ID="cmbModoFactura" runat="server" style="margin-bottom: 0px">
                <asp:ListItem Value="1">Tradicional</asp:ListItem>
                <asp:ListItem Value="2">Electrónica</asp:ListItem>
         </asp:DropDownList>                  
     </asp:Panel>   
     <asp:Panel ID="pnlLista" runat="server" GroupingText="Filtros">
         <asp:Label ID="Label6" runat="server" Text="Tipo:" 
             Width="75px" Visible="False"></asp:Label>
         <asp:DropDownList ID="cmbTipoFactura" runat="server" AutoPostBack="True" 
             onselectedindexchanged="cmbTipoFactura_SelectedIndexChanged" 
             Visible="False">
         <asp:ListItem Value="1">Recetas</asp:ListItem>
         <asp:ListItem Value="2">Línea Crédito</asp:ListItem>
         </asp:DropDownList>
         <br />
         <asp:CheckBox ID="chkFecha" runat="server" AutoPostBack="True" 
             oncheckedchanged="chkFecha_CheckedChanged" Text="Fecha:" Width="75px" />
         <asp:Label ID="Label3" runat="server" CssClass="labelForms" Text="Desde:" 
             Width="50px"></asp:Label>
         <asp:TextBox ID="txbFechaDesde" runat="server"></asp:TextBox>
         <asp:Label ID="Label4" runat="server" CssClass="labelForms" Text="Hasta:" 
             Width="50px"></asp:Label>
         <asp:TextBox ID="txbFechaHasta" runat="server" 
             ontextchanged="txbHasta_TextChanged"></asp:TextBox>
         <br />
         <asp:CheckBox ID="chkFolio" runat="server" AutoPostBack="True" 
             oncheckedchanged="chkFolio_CheckedChanged" Text="Folio:" Width="75px" />
         <asp:Label ID="Label7" runat="server" CssClass="labelForms" Text="Desde:" 
             Width="50px"></asp:Label>
         <asp:TextBox ID="txbFolioDesde" runat="server"></asp:TextBox>
         <asp:Label ID="Label8" runat="server" CssClass="labelForms" Text="Hasta:" 
             Width="50px"></asp:Label>
         <asp:TextBox ID="txbFolioHasta" runat="server"></asp:TextBox>
         <br />
         <asp:CheckBox ID="chkAlmacen" runat="server" AutoPostBack="True" 
             oncheckedchanged="chkAlmacen_CheckedChanged" Text="Almacen:" />
         <asp:DropDownList ID="cmbAlmacenes" runat="server" DataTextField="Clave" 
             DataValueField="idAlmacen" Enabled="False" AutoPostBack="True">
         </asp:DropDownList>
       </asp:Panel>
     <asp:Panel ID="pnlGastosAdmon" runat="server" 
             GroupingText="Gastos Administrativos" Height="168px">
             
              <asp:CheckBox ID="chkPanelGtosAdmon" runat="server" AutoPostBack="True" 
             Font-Bold="True" Text="Aplicar Gtos. Administrativos" 
                  oncheckedchanged="chkPanelGtosAdmon_CheckedChanged" />
                 <br /><br />

             <asp:CheckBox ID="chkGtoAdmon" runat="server" Text="Gto. de Admon:" 
                  Width="115px" Enabled="False" AutoPostBack="True" 
                  oncheckedchanged="chkGtoAdmon_CheckedChanged" CssClass="labelForms" />
            
             <asp:CheckBox ID="chkMonto" runat="server" Text="Monto:" Width="75px" 
                  Enabled="False" AutoPostBack="True" 
                  oncheckedchanged="chkMonto_CheckedChanged" CssClass="labelForms" />
             <asp:TextBox ID="txbMonto" runat="server" Enabled="False" Width="100px"></asp:TextBox>
         
             <asp:CheckBox ID="chkPorcentaje" runat="server" Text="Porcentaje:" 
                  Width="100px" Enabled="False" AutoPostBack="True" 
                  oncheckedchanged="chkPorcentaje_CheckedChanged" CssClass="labelForms" />
             <asp:TextBox ID="txbPorcentaje" runat="server" Enabled="False" Width="50px"></asp:TextBox>
             <br />
             
              <br />
              <asp:Label ID="Label13" runat="server" 
                  Text="Factura de gastos administrativos por:"></asp:Label>
              <asp:RadioButtonList ID="rblGenerarFacturaTipo" runat="server" 
                  RepeatDirection="Horizontal" AutoPostBack="True" 
                  onselectedindexchanged="rblGenerarFacturaTipo_SelectedIndexChanged">
                  <asp:ListItem Selected="True" Value="1">Renglones</asp:ListItem>
                  <asp:ListItem Value="2">Sub Totales</asp:ListItem>
              </asp:RadioButtonList>
             
              <asp:TextBox ID="txbProductos" runat="server" 
                  ontextchanged="txbProductos_TextChanged" Width="300px" AutoPostBack="True" 
                  Enabled="False"></asp:TextBox>
              <asp:AutoCompleteExtender ID="txbProductos_AutoCompleteExtender" runat="server" 
                  DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx" 
                  TargetControlID="txbProductos" ServiceMethod="RecuperarNombreProducto" FirstRowSelected="True" MinimumPrefixLength="1">
              </asp:AutoCompleteExtender>
              <asp:Label ID="lblAvisoSubTotales" runat="server" Font-Bold="True" 
                  ForeColor="Red" 
                  Text="Seleccione el producto o servicio para la factura de subtotales"></asp:Label>
       </asp:Panel>
       <asp:Panel ID="Panel1" runat="server" GroupingText="Cliente" Height="100px" 
                    Width="845px">
        <asp:Label ID="Label14" runat="server" Text="Nombre:" CssClass="labelForms" 
                        Width="130px"></asp:Label>
                    <asp:TextBox ID="txbCliente" runat="server" Width="400px" 
                        ontextchanged="txbCliente_TextChanged" AutoPostBack="True"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="txbCliente_AutoCompleteExtender" runat="server" 
                        DelimiterCharacters="" Enabled="True" TargetControlID="txbCliente"
                        ServiceMethod="RecuperarNombreCliente" 
                        ServicePath="BusquedasAsincronas.asmx" MinimumPrefixLength="1">
                    </asp:AutoCompleteExtender>

                    <br />
                    <asp:Label ID="Label5" runat="server" Text="Dirección:" CssClass="labelForms" 
                        Width="130px"></asp:Label>
                    <asp:TextBox ID="txbDireccion" runat="server" Width="400px" ReadOnly="True"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label2" runat="server" Text="Población:" CssClass="labelForms" 
                        Width="130px"></asp:Label>
                    <asp:TextBox ID="txbPoblacion" runat="server" Width="400px" ReadOnly="True"></asp:TextBox>
                                
                </asp:Panel>

          <asp:Button ID="btnBuscarRec" runat="server" onclick="btnBuscarRec_Click" 
             Text="Buscar" />
             
    <asp:Panel ID="pnlListado" runat="server" GroupingText="Recetas" 
        Height="200px" ScrollBars="Auto">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:CheckBox ID="chkTodas" runat="server" AutoPostBack="True" Font-Bold="True" 
        Text="Todas" oncheckedchanged="chkTodas_CheckedChanged" Visible="False" />
        <asp:GridView ID="dgvDatos" runat="server" AutoGenerateColumns="False" 
            onrowcreated="dgvDatos_RowCreated" BackColor="White" BorderColor="#999999" 
            BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" 
            GridLines="Vertical" AllowPaging="True" PageSize="100">
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <Columns>
                <asp:BoundField DataField="Folio" HeaderText="Folio">
                <ItemStyle Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Fecha" DataFormatString="{0:dd/MM/yyyy}" 
                    HeaderText="Fecha">
                <ItemStyle Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="EstatusMedico" HeaderText="Estatus Medico.">
                <ItemStyle Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="Total" DataFormatString="{0:C}" HeaderText="Total">
                <ItemStyle Width="150px" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Facturar">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="150px" />
                </asp:TemplateField>
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
   
        <asp:Button ID="btnFacturar" runat="server" onclick="btnFacturar_Click" 
            Text="Facturar" />
        <br />
        <asp:Label ID="lblAviso" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
</ContentTemplate>
        </asp:UpdatePanel>


     <div id="divCatalogo">
      <asp:UpdatePanel ID="pnlCatalogo" runat="server">
        <ContentTemplate>
           
          
        </ContentTemplate>
      </asp:UpdatePanel>
    </div>

     <%--<div>
        <asp:Panel ID="pnlReportes" runat="server" Width="845px" Height="440px" ScrollBars="Auto">        
            <asp:Panel ID="pnlBotones" runat="server">
                <asp:Button ID="Button1" runat="server" Text="Exportar a PDF" 
                    onclick="btnPdf_Click" />
                &nbsp&nbsp&nbsp&nbsp
                <asp:Button ID="Button2" runat="server" Text="Exportar a Excel" 
                    onclick="btnExcel_Click" />
                &nbsp&nbsp&nbsp&nbsp
                <asp:Button ID="btnCrystal" runat="server" Text="Exportar a Crystal Reports" 
                    onclick="btnCrystal_Click" />
            </asp:Panel>
            <asp:Panel ID="Panel4" runat="server">
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
    </div> --%>


</asp:Content>
