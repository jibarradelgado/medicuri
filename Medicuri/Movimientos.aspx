<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Movimientos.aspx.cs" Inherits="Medicuri.Movimientos" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register TagPrefix="FiltroReportes" TagName="Filtro" Src="~/FiltroReportes.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>  
    <div id="EntradaSalida" runat="server">
       <asp:UpdatePanel ID="upnEntradaSalida" runat="server">
        <ContentTemplate>
         <asp:Panel ID="Panel1" runat="server" GroupingText="General">
                  <asp:Label ID="Label1" runat="server" Text="Concepto" CssClass="labelForms" 
                      Width="100px"></asp:Label>
                <asp:DropDownList ID="ddlEntSalConceptoMov" runat="server" 
                      CssClass="comboBoxForms" Width="145px" 
                      onselectedindexchanged="ddlEntSalConceptoMov_SelectedIndexChanged" AutoPostBack="True"> </asp:DropDownList>
                  <asp:Label ID="Label2" runat="server" Text="Tipo" CssClass="labelForms" 
                      Width="100px"></asp:Label>
                   <asp:DropDownList ID="ddlEntSalTiposMovimiento" runat="server" 
                      CssClass="comboBoxForms" Width="145px"> </asp:DropDownList>
                  <asp:Label ID="Label3" runat="server" Text="Almacén afectado" CssClass="labelForms" Width="100px"></asp:Label>
                  <asp:DropDownList ID="ddlEntSalAlmacenes" runat="server" 
                      CssClass="comboBoxForms" Width="145px"> </asp:DropDownList>
                  <br />
                  <!-- ocultar en caso de salida -->
                  <asp:Label ID="lblAlmacenSalida" runat="server" CssClass="labelForms" Text="Almacen de Salida" 
                      Width="100px" Visible="False"></asp:Label>
                  <asp:DropDownList ID="ddlEntSalAlmacenSalida" runat="server" 
                      CssClass="comboBoxForms" Width="145px" Visible="False"> </asp:DropDownList>
                  <asp:Label ID="lblLineaCredito" runat="server" CssClass="labelForms" 
                      Text="Línea de crédito" Width="100px"></asp:Label>
                  <asp:DropDownList ID="ddlEntSalLineasDeCredito" runat="server" 
                      CssClass="comboBoxForms" Width="145px"> </asp:DropDownList>
                  <asp:Label ID="Label30" runat="server" CssClass="labelForms" Text="Pedimento:" 
                      Width="100px"></asp:Label>
                  <asp:TextBox ID="txbPedimento" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                  <br/>
                  <!-- ocultar en caso de salida -->
                  <asp:Label ID="Label6" runat="server" CssClass="labelFormsNextTextArea" 
                      Text="Comentarios" Width="100px"></asp:Label>
                  <asp:TextBox ID="txtEntSalComentarios" runat="server" CssClass="textBoxForms" 
                      TextMode="MultiLine" Width="500px" Height="75px" MaxLength="200"></asp:TextBox>
              </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" GroupingText="Artículo">
               <asp:Table ID="Table1" runat="server" Width="835px">
                    <asp:TableRow>
                       <asp:TableCell>   
                            <asp:Label ID="Label7" runat="server" Text="Artículo" CssClass="labelForms"  BorderStyle="Ridge" BorderColor="Black" Width="90px"></asp:Label>
                       </asp:TableCell>
                       <asp:TableCell> 
                            <asp:Label ID="Label9" runat="server" Text="Nombre" Width="90px"   BorderStyle="Ridge" BorderColor="Black" CssClass="labelForms"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:Label ID="Label8" runat="server" CssClass="labelForms" Text="Lotes disponibles"  BorderStyle="Ridge" BorderColor="Black" Width="90px"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:Label ID="Label18" runat="server" CssClass="labelForms" Text="Series disponibles"  BorderStyle="Ridge" BorderColor="Black" Width="90px"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                          <asp:Label ID="Label28" runat="server" CssClass="labelForms" Text="Fec. Caducidad"  BorderStyle="Ridge" BorderColor="Black" Width="90px"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:Label ID="Label23" runat="server" Text="Costo" Width="60px"  BorderStyle="Ridge" BorderColor="Black" CssClass="labelForms"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:Label ID="Label10" runat="server" Text="Cantidad" Width="60px"  BorderStyle="Ridge" BorderColor="Black" CssClass="labelForms"></asp:Label>
                        </asp:TableCell>
                         <asp:TableCell>
                             
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell> 
                            <asp:TextBox ID="txtEntSalArticulo" runat="server" CssClass="labelForms" AutoPostBack="True" Width="90px" TabIndex="1"></asp:TextBox>
                             <asp:AutoCompleteExtender ID="txbClave_AutoCompleteExtender" runat="server" 
                    DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                    ServiceMethod="RecuperarClave1Producto" ServicePath="BusquedasAsincronas.asmx" 
                    TargetControlID="txtEntSalArticulo" MinimumPrefixLength="1"></asp:AutoCompleteExtender>
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:TextBox ID="txtEntSalDescripcionArticulo" runat="server" CssClass="labelForms" AutoPostBack="True" Width="90px" TabIndex="1"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="txbProducto_AutoCompleteExtender" runat="server" 
                                    DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                                    ServiceMethod="RecuperarNombreProducto" ServicePath="BusquedasAsincronas.asmx" 
                                    TargetControlID="txtEntSalDescripcionArticulo" MinimumPrefixLength="1">
                                </asp:AutoCompleteExtender>
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:DropDownList ID="ddlEntSalLotes" CssClass="labelForms" runat="server" onselectedindexchanged="ddlEntSalLotes_SelectedIndexChanged"  AutoPostBack="True" TabIndex="2"></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:DropDownList ID="ddlEntSalSeries" CssClass="labelForms" runat="server" onselectedindexchanged="ddlEntSalSeries_SelectedIndexChanged"  AutoPostBack="True" TabIndex="3"></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:TextBox ID="txtEntSalFechaCadArt" runat="server" CssClass="labelForms" Width="90px" TabIndex="6"></asp:TextBox>
                                <asp:CalendarExtender ID="txbFecha_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="txtEntSalFechaCadArt" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                        </asp:TableCell> 
                        <asp:TableCell> 
                            <asp:TextBox ID="txtEntSalCostoArticulo" runat="server" CssClass="labelForms" Width="70px" TabIndex="7"></asp:TextBox>
                            </asp:TableCell>
                        <asp:TableCell> 
                            <asp:TextBox ID="txtEntSalCantidadArticulo" runat="server" CssClass="labelForms" Width="70px" TabIndex="8"></asp:TextBox>
                         </asp:TableCell>
                           <asp:TableCell>
                            <asp:ImageButton ID="imbEntSalAgregarDetalle" runat="server"  ImageUrl="~/Icons/plus_16.png" OnClick="imbEntSalAgregarDetalle_Click" TabIndex="9" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell> 
                        </asp:TableCell>
                        <asp:TableCell> 
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:TextBox ID="txtEntSalLotes" runat="server" CssClass="textBoxForms"  Visible="False" Width="60px" TabIndex="4"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:TextBox ID="txtEntSalSeries" runat="server" CssClass="textBoxForms" Visible="False" Width="80px" TabIndex="5"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell> 
                        </asp:TableCell>
                        <asp:TableCell> 
                        </asp:TableCell>
                         <asp:TableCell> 
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                  <asp:Label ID="lbEntSalAvisos" runat="server"  Width="100px" CssClass="labelForms" ForeColor="Red"></asp:Label>
        <asp:Panel ID="Panel7" runat="server" ScrollBars="Auto" Height="200px">
                    <asp:GridView ID="grvEntSalArticulos" runat="server" AutoGenerateColumns="False"                         
                         BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                         CellPadding="3" ForeColor="Black" GridLines="Vertical" 
                   Width="840px" onselectedindexchanged="grvEntSalArticulos_SelectedIndexChanged" 
                        TabIndex="10" >
                       <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                     <asp:BoundField HeaderText="Clave" DataField="codigo" />
                          <asp:BoundField HeaderText="Descripción" DataField="descripcion"/>
                          <asp:BoundField DataField="Lote" HeaderText="Lote" />
                          <asp:BoundField DataField="Serie" HeaderText="Serie" />
                          <asp:BoundField DataField="strFechaCaducidadShort" HeaderText="Fecha caducidad" />
                          <asp:BoundField HeaderText="Cantidad" DataField="cantidad"/>
                          <asp:BoundField HeaderText="Costo" DataField="costo"/>
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
                   
              <asp:Button ID="Button2" runat="server" Text="Guardar" onclick="Button2_Click" 
                   TabIndex="11" />
              </asp:Panel>
      </ContentTemplate>
    </asp:UpdatePanel>
    </div>   
    <div id="divCatalogo" class="divCatalogo">
        <div id="divCatalogoSub" class="divCatalogoSub">
            <asp:Panel ID="pnlCatalogo" runat="server" Height="350px" ScrollBars="Auto">
            <asp:UpdatePanel ID="upnCatalogo" runat="server">                    
                <ContentTemplate>
                    <asp:GridView ID="gdvDatos" runat="server" AutoGenerateColumns="False" 
                        PageSize="100" 
                        BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                        CellPadding="3" ForeColor="Black" GridLines="Vertical" 
                        DataKeyNames="idInventarioMov" Width="840px" AllowPaging="True" 
                        AllowSorting="True" onpageindexchanging="gdvDatos_PageIndexChanging" 
                        onsorting="gdvDatos_Sorting">
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png"
                                ShowSelectButton="True" />
                            <asp:BoundField DataField="Concepto" HeaderText="Concepto" 
                                SortExpression="Concepto" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" 
                                SortExpression="Observaciones" />
                            <asp:BoundField DataField="Pedimento" HeaderText="Pedimento" 
                                SortExpression="Pedimento" />
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
    </div>
    <div style="margin-left:60px">
        <asp:Panel ID="pnlFiltroReportes" runat="server">
            <FiltroReportes:Filtro runat="server" ID="frReportes" />
        </asp:Panel>
    </div>
</asp:Content>
