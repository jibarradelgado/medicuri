<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Movimientos1.aspx.cs" Inherits="Medicuri.Movimientos" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register TagPrefix="FiltroReportes" TagName="Filtro" Src="~/FiltroReportes.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
    <div id="ToolBar" class="toolbar">
        
        <div id="btnNuevo" class="botonToolbar">
        <asp:ImageButton ID="imgBtnEntradaSalida" runat="server"
                ImageAlign="TextTop" ImageUrl="~/Icons/document_32.png" 
                onclick="imgBtnEntradaSalida_Click" />
            <asp:Label ID="lblNuevo" runat="server" Text="Ent./Sal." Font-Size="Small"></asp:Label>
        </div>

        <div id="btnMostrar" class="botonToolbar">
            <asp:ImageButton ID="imgBtnMostrar" runat="server"
                ImageAlign="TextTop" ImageUrl="~/Icons/clipboard_32.png" 
                onclick="imgBtnMostrar_Click" />
            <asp:Label ID="lblMostrar" runat="server" Text="Mostrar" Font-Size="Small"></asp:Label>
        </div>

        <div id="btnImprimir" class="botonToolbar">
        <asp:ImageButton ID="imgBtnImprimir" runat="server"
                ImageAlign="TextTop" ImageUrl="~/Icons/reports32.png" />
            <asp:Label ID="lblImprimir" runat="server" Text="Imprimir" Font-Size="Small"></asp:Label>
        </div>

         <div id="btnReportes" class="botonToolbar">
        <asp:ImageButton ID="imgBtnReportes" runat="server"
                ImageAlign="TextTop" ImageUrl="~/Icons/statistics_32.png" />
            <asp:Label ID="lblReportes" runat="server" Text="Reportes" Font-Size="Small"></asp:Label>
        </div>

        <div id="btnEliminar" class="botonToolbar" style="display:none">
        <asp:ImageButton ID="imgBtnPrecios" runat="server"
                ImageAlign="TextTop" ImageUrl="~/Icons/delete_32.png" 
                onclick="imgBtnPrecios_Click" />
            <asp:Label ID="lblEliminar" runat="server" Text="Precios" Font-Size="Small"></asp:Label>
        </div>

        <div id="btnFisico" class="botonToolbar" style="display:none">
        <asp:ImageButton ID="imgBtnFisico" runat="server"
                ImageAlign="TextTop" ImageUrl="~/Icons/statistics_32.png" 
                onclick="imgBtnFisico_Click" />
            <asp:Label ID="lblFisico" runat="server" Text="Físico" Font-Size="Small"></asp:Label>
        </div>

        <div>
        <div id="btnCancelar" class="botonToolbarRight">
        <asp:ImageButton ID="imgBtnCancelar" runat="server" 
                ImageAlign="TextTop" ImageUrl="~/Icons/block_32.png" Visible="False" />
            <asp:Label ID="lblCancelar" runat="server" Text="Cancelar" Font-Size="Small" 
                Visible="False"></asp:Label>
        </div>

        <div id="btnAceptar" class="botonToolbarRight">
        <asp:ImageButton ID="imgBtnAceptar" runat="server" 
                ImageAlign="TextTop" ImageUrl="~/Icons/tick_32.png" Visible="False" />
            <asp:Label ID="lblGuardar" runat="server" Text="Guardar" Font-Size="Small" 
                Visible="False"></asp:Label>
        </div>
        </div>
        
    </div>
    <div id="Busqueda" class="divBusqueda">
        <table>
            <tr>
                <td class="Busqueda_filtro" width="550px" align="right">
                    <asp:RadioButton id="rdbFiltro1" runat="server"  GroupName="Filtro" Text="Concepto"></asp:RadioButton>
                    <%-- %><asp:RadioButton id="rdbFiltro2" runat="server"  GroupName="Filtro" Text="Filtro2"></asp:RadioButton>
                    <asp:RadioButton id="rdbFiltro3" runat="server"  GroupName="Filtro" Text="Filtro3"></asp:RadioButton>--%>
                </td>
                <td class="Busqueda_texto" width="150px" align="right">
                    <asp:Label ID="lblBuscar" runat="server" Text="Buscar:"></asp:Label>
                    <asp:TextBox ID="txbBuscar" runat="server"></asp:TextBox>
                </td>
                <td class="Busqueda_boton" width="145px" align="left">    
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" 
                        onclick="btnBuscar_Click" />
                </td>
            </tr>
        </table>
    </div>
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
                  <asp:Label ID="Label4" runat="server" CssClass="labelForms" Text="Proveedor" Width="100px"></asp:Label>
                  <asp:DropDownList ID="ddlEntSalProveedor" runat="server" 
                      CssClass="comboBoxForms" Width="145px"> </asp:DropDownList>
                  <asp:Label ID="Label5" runat="server" CssClass="labelForms" Text="Línea de crédito" Width="100px"></asp:Label>
                  <asp:DropDownList ID="ddlEntSalLineasDeCredito" runat="server" 
                      CssClass="comboBoxForms" Width="145px"> </asp:DropDownList>
                  <asp:Label ID="Label30" runat="server" CssClass="labelForms" Text="Pedimento:" 
                      Width="100px"></asp:Label>
                  <asp:TextBox ID="txbPedimento" runat="server"></asp:TextBox>
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
                            <asp:TextBox ID="txtEntSalArticulo" runat="server" CssClass="textBoxForms" AutoPostBack="True" Width="90px"></asp:TextBox>
                             <asp:AutoCompleteExtender ID="txbClave_AutoCompleteExtender" runat="server" 
                    DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                    ServiceMethod="RecuperarClave1Producto" ServicePath="BusquedasAsincronas.asmx" 
                    TargetControlID="txtEntSalArticulo" MinimumPrefixLength="1"></asp:AutoCompleteExtender>
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:TextBox ID="txtEntSalDescripcionArticulo" runat="server" CssClass="textBoxForms" AutoPostBack="True" Width="90px"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="txbProducto_AutoCompleteExtender" runat="server" 
                                    DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                                    ServiceMethod="RecuperarNombreProducto" ServicePath="BusquedasAsincronas.asmx" 
                                    TargetControlID="txtEntSalDescripcionArticulo" MinimumPrefixLength="1">
                                </asp:AutoCompleteExtender>
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:DropDownList ID="ddlEntSalLotes" runat="server" onselectedindexchanged="ddlEntSalLotes_SelectedIndexChanged"  AutoPostBack="True"></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:DropDownList ID="ddlEntSalSeries" runat="server" onselectedindexchanged="ddlEntSalSeries_SelectedIndexChanged"  AutoPostBack="True"></asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:TextBox ID="txtEntSalFechaCadArt" runat="server" CssClass="textBoxForms" Width="90px"></asp:TextBox>
                                <asp:CalendarExtender ID="txbFecha_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="txtEntSalFechaCadArt" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                        </asp:TableCell> 
                        <asp:TableCell> 
                            <asp:TextBox ID="txtEntSalCostoArticulo" runat="server" CssClass="textBoxForms" Width="70px"></asp:TextBox>
                            </asp:TableCell>
                        <asp:TableCell> 
                            <asp:TextBox ID="txtEntSalCantidadArticulo" runat="server" CssClass="textBoxForms" Width="70px"></asp:TextBox>
                         </asp:TableCell>
                           <asp:TableCell>
                            <asp:ImageButton ID="imbEntSalAgregarDetalle" runat="server"  ImageUrl="~/Icons/plus_16.png" OnClick="imbEntSalAgregarDetalle_Click" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell> 
                        </asp:TableCell>
                        <asp:TableCell> 
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:TextBox ID="txtEntSalLotes" runat="server" CssClass="textBoxForms"  Visible="False" Width="60px"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell> 
                            <asp:TextBox ID="txtEntSalSeries" runat="server" CssClass="textBoxForms" Visible="False" Width="80px"></asp:TextBox>
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
                
                   <asp:GridView ID="grvEntSalArticulos" runat="server" AutoGenerateColumns="False"                         
                         BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                         CellPadding="3" ForeColor="Black" GridLines="Vertical" 
                   Width="840px" >
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
              <asp:Button ID="Button2" runat="server" Text="Guardar" onclick="Button2_Click" />
              </asp:Panel>
      </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    <div id="CambioPrecios" runat="server">
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
       
            <asp:Panel ID="Panel3" runat="server" GroupingText="Rango de Artículos">
                <asp:CheckBox ID="chbCmPrTodos" runat="server" Text="Aplicar a todos" 
                    AutoPostBack="True" oncheckedchanged="chbCmPrTodos_CheckedChanged" /><br/>
                <asp:Label ID="Label11" runat="server" Text="Desde:"></asp:Label>
                <asp:DropDownList ID="ddlCmPrDesde" runat="server"> </asp:DropDownList>
                <asp:Label ID="Label12" runat="server" Text="Hasta:"></asp:Label>
                <asp:DropDownList ID="ddlCmPrHasta" runat="server"> </asp:DropDownList>
            </asp:Panel><br />
             <asp:Panel ID="Panel4" runat="server" GroupingText="Operativo">
                <asp:Label ID="lbCmPrAvisos" runat="server" Text="" CssClass="labelForms" ForeColor="Red"></asp:Label><br />
               <asp:Label ID="Label13" runat="server" Text="Lista de precios:" 
                     CssClass="labelForms" Width="100px"></asp:Label>
                <asp:DropDownList ID="ddlCmPrListasPrecios" runat="server"> 
                   <asp:ListItem>Precio público</asp:ListItem>
                   <asp:ListItem>Precio 1</asp:ListItem>
                   <asp:ListItem>Precio 2</asp:ListItem>
                   <asp:ListItem>Precio 3</asp:ListItem>
                   <asp:ListItem>Precio mínimo</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label14" runat="server" Text="Operación:" CssClass="labelForms" 
                     Width="100px"></asp:Label>
                <asp:DropDownList ID="ddlCmPrOperacion" runat="server"> 
                    <asp:ListItem>Aumento</asp:ListItem>
                    <asp:ListItem>Disminución</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label15" runat="server" Text="Tipo:" CssClass="labelForms" 
                     Width="100px"></asp:Label>
                <asp:DropDownList ID="ddlCmPrTipo" runat="server"> 
                    <asp:ListItem>Monto</asp:ListItem>
                    <asp:ListItem>Porcentaje</asp:ListItem>
                 </asp:DropDownList><br />
                 <asp:Label ID="Label16" runat="server" Text="Cantidad:" CssClass="labelForms" 
                     Width="100px"></asp:Label>
                 <asp:TextBox ID="txtCmPrCantidad" runat="server" CssClass="textBoxForms"></asp:TextBox>
                 <div id="Div1" runat=server align="left" >
                  <asp:GridView ID="grvCmPrErrores" runat="server"
                      AutoGenerateColumns="False" 
                      ShowFooter="True" Visible="False" BackColor="White" BorderColor="#999999" 
                     BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" 
                     GridLines="Vertical" Width="840px">
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
                 <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                     Text="Guardar" Enabled="False" />
                  
                  

             </asp:Panel>
         
      </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    <div id="InventarioFísico" runat="server">
       <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
              <asp:Panel ID="Panel6" runat="server" GroupingText="General">
                <asp:Label ID="lbInvFsAvisos" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label><br/>
                <asp:CheckBox ID="chbInvFsTodos" runat="server" Text="Aplicar a todos" 
                    AutoPostBack="True" oncheckedchanged="chbInvFsTodos_CheckedChanged"  />
                  <br/>
                <asp:Label ID="Label24" runat="server" Text="Desde"></asp:Label>
                <asp:DropDownList ID="ddlInvFsClave1Desde" runat="server"> </asp:DropDownList>
                <asp:Label ID="Label25" runat="server" Text="Hasta"></asp:Label>
                <asp:DropDownList ID="ddlInvFsClave1Hasta" runat="server"> </asp:DropDownList>
                <asp:Label ID="Label22" runat="server" Text="Almacén" CssClass="labelForms" Width="100px"></asp:Label>
                  <asp:DropDownList ID="ddlInvFsAlmacen" runat="server"> </asp:DropDownList>
                  <asp:Button ID="Button3" runat="server" onclick="Button3_Click" Text="Buscar" />
              </asp:Panel>

              <asp:Panel ID="Panel5" runat="server" GroupingText="Artículo">
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
                      GridLines="Vertical" Width="835px" >
                      <AlternatingRowStyle BackColor="#CCCCCC" />
                      <Columns>
                      	  <asp:BoundField HeaderText="Clave" DataField="codigo"/>
                          <asp:BoundField HeaderText="Descripción" DataField="descripcion" />
                          <asp:BoundField HeaderText="Lote" DataField="lote"/>
                          <asp:BoundField HeaderText="Serie" DataField="serie"/>
                          <asp:BoundField HeaderText="Cantidad teórica" DataField="existenciaTeorica"/>
                          <asp:BoundField HeaderText="Cantidad Real" DataField="strExistenciaReal"/>
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
                  <asp:Button ID="Button4" runat="server" onclick="Button4_Click" 
                      Text="Guardar" />
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
                        DataKeyNames="idInventarioMov" Width="840px">
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:BoundField DataField="Concepto" HeaderText="Concepto" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" >
                            </asp:BoundField>
                            <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                            <asp:BoundField DataField="Pedimento" HeaderText="Pedimento" />
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
    <%--<div>
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
