<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Facturas.aspx.cs" Inherits="Medicuri.Facturacion" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>
<%@ Register TagPrefix="FiltroReportes" TagName="Filtro" Src="~/FiltroReportes.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
                
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
      <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>
    <asp:EntityDataSource ID="entityFormaPago" runat="server" 
          DefaultContainerName="medicuriEntities" EnableFlattening="False" 
          EntitySetName="tipo_forma_pago" EntityTypeFilter="tipo_forma_pago" 
          ContextTypeName="MedDAL.DAL.medicuriEntities">
    </asp:EntityDataSource>
    <asp:EntityDataSource ID="entityMetodoPago" runat="server" 
          DefaultContainerName="medicuriEntities" EnableFlattening="False" 
          EntitySetName="metodo_pago" EntityTypeFilter="metodo_pago" 
          ContextTypeName="MedDAL.DAL.medicuriEntities">
    </asp:EntityDataSource>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label id="lblAviso" runat="server" Text="" ForeColor="Red"></asp:Label>
                <br />
                <asp:Label id="lblAviso2" runat="server" Text="" ForeColor="Red"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

     <div id="Listado">
        <asp:UpdatePanel ID="pnlCatalogo" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server" Height="350px" ScrollBars="Auto">
              
                <asp:GridView ID="dgvDatos" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="idPedido" onrowcreated="dgvDatos_RowCreated" 
                    onselectedindexchanged="dgvDatos_SelectedIndexChanged" BackColor="White" 
                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                    ForeColor="Black" GridLines="Vertical" AllowPaging="True" PageSize="100" 
                        AllowSorting="True" onpageindexchanging="dgvDatos_PageIndexChanging" 
                        onsorting="dgvDatos_Sorting">
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                     <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                        SelectText="-" ShowSelectButton="True" />

                        <asp:BoundField DataField="idFactura" HeaderText="idFactura" ReadOnly="True" 
                            SortExpression="idFactura" Visible="False" />

                       <%--<asp:BoundField DataField="idCliente" HeaderText="idCliente" 
                            SortExpression="idCliente" Visible="False" />--%>

                        <asp:BoundField DataField="Nombre" HeaderText="Nombre(s)" 
                            SortExpression="Nombre" >
                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" 
                            SortExpression="Apellidos" >
                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Pedido" HeaderText="Pedido" SortExpression="Pedido" >
                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                        </asp:BoundField>

                         <asp:BoundField DataField="Remision" HeaderText="Remision" SortExpression="Remision" >
                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Receta" HeaderText="Receta" SortExpression="Receta" >
                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio" >
                            <ItemStyle HorizontalAlign="Left" Width="75px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" 
                            DataFormatString="{0:dd/MM/yyyy}" >
                            <ItemStyle HorizontalAlign="Center" Width="75px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Estatus" HeaderText="Estatus" 
                            SortExpression="Estatus" >
                            <ItemStyle HorizontalAlign="Center" Width="75px" />
                        </asp:BoundField>

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

   

    <div id="Formulario">
        <asp:UpdatePanel ID="pnlFormulario" runat="server">
            <ContentTemplate>

            <asp:Panel ID="Panel2" runat="server" GroupingText="Datos" Height="146px" 
                    Width="845px">
                <asp:Label ID="Label25" runat="server" Text="Tipo:" CssClass="labelForms" 
                    Width="80px"></asp:Label>
                <asp:DropDownList ID="cmbModoFactura" runat="server" style="margin-bottom: 0px">
                <asp:ListItem Value="0">Electrónica</asp:ListItem>
                <asp:ListItem Value="1">Tradicional</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label26" runat="server" Text="Factura:" CssClass="labelForms" 
                    Width="80px"></asp:Label>
                <asp:DropDownList ID="cmbTipoFactura" runat="server" 
                    onselectedindexchanged="cmbTipoFactura_SelectedIndexChanged" 
                    AutoPostBack="True">
                <asp:ListItem Value="1">Directa</asp:ListItem>
                <asp:ListItem Value="2">Pedido</asp:ListItem>
                <asp:ListItem Value="3">Remisión</asp:ListItem>
                <asp:ListItem Value="4">Receta</asp:ListItem>
                
                </asp:DropDownList>
                <asp:Label ID="Label2" runat="server" Text="Folio:" CssClass="labelForms" 
                    Width="80px"></asp:Label>
                <asp:TextBox ID="txbFolio" runat="server"></asp:TextBox>
                <asp:Label ID="Label3" runat="server" Text="Fecha:" CssClass="labelForms" 
                    Width="80px"></asp:Label>
                <asp:TextBox ID="txbFecha" runat="server" Width="120px"></asp:TextBox>
                
                  
                <asp:CalendarExtender ID="txbFecha_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="txbFecha">
                </asp:CalendarExtender>
                
                  
                <asp:Label ID="Label24" runat="server" Text="Estatus:"></asp:Label>
                <asp:DropDownList ID="cmbEstatus" runat="server">
                <%--<asp:ListItem Value="1">Pedido</asp:ListItem>--%>
                <%--<asp:ListItem Value="2">Remitido</asp:ListItem>--%>
                 <asp:ListItem Value="3">Emitida</asp:ListItem>
                 <asp:ListItem Value="4">Cobrada</asp:ListItem>
                <asp:ListItem Value="5">Cancelada</asp:ListItem>
                </asp:DropDownList>
                
                  
                <br />
                <asp:Label ID="lblPedido" runat="server" CssClass="labelForms" Text="Pedido:" 
                    Width="80px"></asp:Label>
                <asp:TextBox ID="txbPedido" runat="server" AutoPostBack="True" 
                    ontextchanged="txbPedido_TextChanged" Enabled="False"></asp:TextBox>
                <asp:AutoCompleteExtender ID="txbPedido_AutoCompleteExtender" 
                    runat="server" DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx" 
                    TargetControlID="txbPedido" ServiceMethod="RecuperarFolioPedidos" 
                    MinimumPrefixLength="1">
                </asp:AutoCompleteExtender>
               
                <asp:Label ID="lblRemision" runat="server" Text="Remisión:" 
                    CssClass="labelForms" Width="80px"></asp:Label>
                <asp:TextBox ID="txbRemision" runat="server" 
                    AutoPostBack="True" ontextchanged="txbRemision_TextChanged" 
                    Enabled="False"></asp:TextBox>
                <asp:AutoCompleteExtender ID="txbRemision_AutoCompleteExtender" runat="server" 
                    DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx" 
                    TargetControlID="txbRemision" MinimumPrefixLength="1" ServiceMethod="RecuperarFolioRemisiones">
                </asp:AutoCompleteExtender>
   
                <asp:Label ID="lblReceta" runat="server" Text="Receta:" 
                    Width="80px" CssClass="labelForms"></asp:Label>
                <asp:TextBox ID="txbReceta" runat="server" AutoPostBack="True" 
                    ontextchanged="txbReceta_TextChanged" Enabled="False"></asp:TextBox>
                <asp:AutoCompleteExtender ID="txbReceta_AutoCompleteExtender" runat="server" 
                    DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx" 
                    TargetControlID="txbReceta" ServiceMethod="RecuperarFolioRecetas" MinimumPrefixLength="1">
                </asp:AutoCompleteExtender>
                <br />
                <asp:Label ID="lblFormaPago" runat="server" Text="Forma de pago:" CssClass="labelForms" Width="80px"></asp:Label>
                <asp:DropDownList ID="cmbFormaPago" runat="server" style="width: 145px;" 
                    DataSourceID="entityFormaPago" DataTextField="FormaPago" 
                    DataValueField="idTipoFormaPago">
                </asp:DropDownList>
                <asp:Label ID="lblMetodoPago" runat="server" Text="Método de pago:" CssClass="labelForms" Width="80px"></asp:Label>
                <asp:DropDownList ID="cmbMetodoPago" runat="server" style="width: 145px;" 
                    DataSourceID="entityMetodoPago" DataTextField="MetodoPago" 
                    DataValueField="idMetodoPago" AutoPostBack="True" 
                    onselectedindexchanged="cmbMetodoPago_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label ID="lblCuentaPago" runat="server" Text="No. Cuenta receptor:" CssClass="labelForms" Width="80px"></asp:Label>
                <asp:TextBox ID="txbCuentaPago" runat="server" Enabled="false"></asp:TextBox>                
                <br />
                <asp:Label ID="lblDatos" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                
                  
              </asp:Panel>
              
              

              <asp:Panel ID="pnlPartida" runat="server" GroupingText="Cliente" Height="100px" 
                    Width="845px">
                    <asp:Label ID="Label4" runat="server" Text="Nombre:" CssClass="labelForms" 
                        Width="130px"></asp:Label>
                    <asp:TextBox ID="txbCliente" runat="server" Width="250px" 
                        ontextchanged="txbCliente_TextChanged" AutoPostBack="True"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="txbCliente_AutoCompleteExtender" runat="server" 
                        DelimiterCharacters="" Enabled="True" TargetControlID="txbCliente"
                        ServiceMethod="RecuperarNombreCliente" 
                        ServicePath="BusquedasAsincronas.asmx" MinimumPrefixLength="1">
                    </asp:AutoCompleteExtender>

                    <br />
                    <asp:Label ID="Label5" runat="server" Text="Dirección:" CssClass="labelForms" 
                        Width="130px"></asp:Label>
                    <asp:TextBox ID="txbDireccion" runat="server" Width="250px" ReadOnly="True"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label6" runat="server" Text="Población:" CssClass="labelForms" 
                        Width="130px"></asp:Label>
                    <asp:TextBox ID="txbPoblacion" runat="server" Width="350px" ReadOnly="True"></asp:TextBox>
                                
                </asp:Panel>
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 

                <asp:Label ID="lblTotal" runat="server" Text="TOTAL:" Font-Bold="True" 
                    Font-Size="Medium" ForeColor="Red"></asp:Label>
                
                <asp:Panel ID="Panel3" runat="server" GroupingText="Detalle" Height="150px" 
                    ScrollBars="Auto" >
                    <asp:Table ID="Table1" runat="server" >
                        <%-- Fila de los encabezados --%>
                        <asp:TableRow>
                          
                          <asp:TableCell Width="125px" HorizontalAlign="Center">
                            <asp:Label ID="Label12" runat="server" Width="125px" BorderStyle="Ridge" BorderColor="Black" >Clave</asp:Label>
                          </asp:TableCell>
                          <asp:TableCell Width="200px" HorizontalAlign="Center">
                            <asp:Label ID="Label13" runat="server" Width="200px" BorderStyle="Ridge" BorderColor="Black">Producto</asp:Label>
                          </asp:TableCell>
                          <asp:TableCell  Width="50px" HorizontalAlign="Center">
                            <asp:Label ID="Label14" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">Cant.</asp:Label>
                          </asp:TableCell>
                          <%-- <asp:TableCell>
                          <asp:Label ID="Label15" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">Desc1</asp:Label>
                          </asp:TableCell>
                          <asp:TableCell>
                            <asp:Label ID="Label16" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">Desc2</asp:Label>
                          </asp:TableCell>--%>
                          <asp:TableCell  Width="50px" HorizontalAlign="Center">
                            <asp:Label ID="Label17" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">IEPS</asp:Label>
                          </asp:TableCell>
                          <%--<asp:TableCell>
                          <asp:Label ID="Label18" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">Imp1</asp:Label>
                          </asp:TableCell>
                          <asp:TableCell>
                            <asp:Label ID="Label19" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">Imp2</asp:Label>
                          </asp:TableCell>--%>
                          <asp:TableCell Width="50px" HorizontalAlign="Center">
                            <asp:Label ID="Label20" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">IVA</asp:Label>
                          </asp:TableCell>
                          <%-- <asp:TableCell>
                             <asp:Label ID="Label21" runat="server" Width="60px" BorderStyle="Ridge" BorderColor="Black">Comisión</asp:Label>
                          </asp:TableCell>--%>
                          <asp:TableCell  Width="50px" HorizontalAlign="Center">
                            <asp:Label ID="Label22" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">Precio</asp:Label>
                          </asp:TableCell>
                          <asp:TableCell  Width="200px" HorizontalAlign="Center">
                            <asp:Label ID="Label23" runat="server" Width="200px" BorderStyle="Ridge" BorderColor="Black">Obsr.</asp:Label>
                          </asp:TableCell>


                        </asp:TableRow>

                         <%-- Fila de los Campos de texto --%>
                        <asp:TableRow>
                          
                          <asp:TableCell  Width="125px" >
                           <asp:TextBox ID="txbClave" runat="server"  Width="125px" 
                    ontextchanged="txbClave_TextChanged" AutoPostBack="True"></asp:TextBox> 
                <asp:AutoCompleteExtender ID="txbClave_AutoCompleteExtender" runat="server" 
                    DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                    ServiceMethod="RecuperarClave1ProductoEnsambles" ServicePath="BusquedasAsincronas.asmx" 
                    TargetControlID="txbClave" MinimumPrefixLength="1">
                </asp:AutoCompleteExtender>
                          </asp:TableCell>

                          <asp:TableCell  Width="200px" >
                           <asp:TextBox ID="txbProducto" runat="server"  Width="200px" AutoPostBack="True" ontextchanged="txbProducto_TextChanged"></asp:TextBox> 

                <asp:AutoCompleteExtender ID="txbProducto_AutoCompleteExtender" runat="server" 
                    DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                    ServiceMethod="RecuperarNombreEnsambleProducto" ServicePath="BusquedasAsincronas.asmx" 
                    TargetControlID="txbProducto" MinimumPrefixLength="1">

                </asp:AutoCompleteExtender>
                          </asp:TableCell>
                          <asp:TableCell  Width="50px" >
                            <asp:TextBox ID="txbCant" runat="server"  Width="50px"></asp:TextBox>
                          </asp:TableCell>
                          <%--<asp:TableCell>
                            <asp:TextBox ID="txbDesc1" runat="server" Width="50px"></asp:TextBox>
                          </asp:TableCell>
                          <asp:TableCell>
                              <asp:TextBox ID="txbDesc2" runat="server" Width="50px"></asp:TextBox>
                          </asp:TableCell>--%>
                          <asp:TableCell  Width="50px" >
                             <asp:TextBox ID="txbIeps" runat="server" Width="50px"></asp:TextBox>
                          </asp:TableCell>
                          <%--<asp:TableCell>
                          <asp:TextBox ID="txbImp1" runat="server" Width="50px"></asp:TextBox>
                          </asp:TableCell>
                          <asp:TableCell>
                           <asp:TextBox ID="txbImp2" runat="server" Width="50px"></asp:TextBox>
                          </asp:TableCell>--%>
                          <asp:TableCell  Width="50px" >
                           <asp:TextBox ID="txbIva" runat="server" Width="50px" ></asp:TextBox>
                          </asp:TableCell>
                          <%--<asp:TableCell>
                           <asp:TextBox ID="txbComision" runat="server" Width="60px"></asp:TextBox>
                          </asp:TableCell>--%>
                          <asp:TableCell  Width="50px" >
                           <%--<asp:TextBox ID="txbPrecio" runat="server" Width="50px"></asp:TextBox>--%>
                           <asp:DropDownList ID="cmbPrecios" runat="server"></asp:DropDownList>
                          </asp:TableCell>
                          <asp:TableCell  Width="200px" >
                           <asp:TextBox ID="txbObservaciones" runat="server" Width="200px"></asp:TextBox>
                          </asp:TableCell>
                          <asp:TableCell  Width="15px" >
                           <asp:ImageButton ID="imbAgregarDetalle" runat="server" ImageUrl="~/Icons/plus_16.png" Height="16px" onclick="imbAgregarDetalle_Click"/>
                          </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <br />
                   <asp:GridView ID="dgvPartidaDetalle" runat="server" AutoGenerateColumns="False" 
                        onselectedindexchanged="dgvPartidaDetalle_SelectedIndexChanged" 
                        AllowPaging="True" PageSize="3" BackColor="White" BorderColor="#999999" 
                        BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" 
                        GridLines="Vertical" onrowcancelingedit="dgvPartidaDetalle_RowCancelingEdit" 
                        onrowediting="dgvPartidaDetalle_RowEditing" 
                        onrowupdating="dgvPartidaDetalle_RowUpdating" Width="830px" 
                        onpageindexchanging="dgvPartidaDetalle_PageIndexChanging">
                       <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" CancelImageUrl="~/Icons/delete_16.png" 
                            CausesValidation="False" EditImageUrl="~/Icons/right_16.png" 
                            ShowEditButton="True" UpdateImageUrl="~/Icons/tick_16.png" />
                      <asp:BoundField HeaderText="Clave" DataField="SClave" ReadOnly="True" >
                          <ItemStyle Width="125px" />
                        </asp:BoundField>
                      <asp:BoundField HeaderText="Producto" DataField="SProducto" ReadOnly="True" >
                          <ItemStyle Width="200px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Cant.">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("DCantidad") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("DCantidad") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="50px" />
                        </asp:TemplateField>
                      <asp:BoundField HeaderText="IEPS" DataField="DIeps" ReadOnly="True" >
                          <ItemStyle Width="50px" />
                        </asp:BoundField>
                      <%--<asp:BoundField HeaderText="Imp1" DataField="DImp1" />
                      <asp:BoundField HeaderText="Imp2" DataField="DImp2" />--%>
                      <asp:BoundField HeaderText="IVA" DataField="DIva" ReadOnly="True" >
                          <ItemStyle Width="50px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Precio">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("DPrecio") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("DPrecio") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="50px" />
                        </asp:TemplateField>
                      <asp:BoundField HeaderText="Obsr." DataField="SObservaciones" ReadOnly="True" >
                          <ItemStyle Width="200px" />
                        </asp:BoundField>
                      <asp:BoundField HeaderText="Total" DataField="DTotal" ReadOnly="True" >
                          <ItemStyle Width="100px" />
                        </asp:BoundField>
                      <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/delete_16.png" SelectText="-" ShowSelectButton="True" />  
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
    <div style="margin-left:60px">
        <asp:Panel ID="pnlFiltroReportes" runat="server">
            <FiltroReportes:Filtro runat="server" ID="frReportes" />
        </asp:Panel>
    </div>

</asp:Content>
