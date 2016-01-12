<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Perfiles.aspx.cs" Inherits="Medicuri.Perfiles" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<%-- Este es del header --%>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>


     <div id="Listado">
        <asp:Panel ID="pnlCatalogo" runat="server" Height="350px" ScrollBars="Auto">
            <asp:UpdatePanel ID="upnCatalogo" runat="server">
            <ContentTemplate>               
                <asp:GridView ID="dgvDatos" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="idPerfil" 
                    onselectedindexchanged="dgvDatos_SelectedIndexChanged" BackColor="White" 
                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                    ForeColor="Black" GridLines="Vertical" Width="845px" AllowPaging="True" 
                    AllowSorting="True" onpageindexchanging="dgvDatos_PageIndexChanging" 
                    onsorting="dgvDatos_Sorting" PageSize="100">
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                        SelectText="-" ShowSelectButton="True" />
                        <asp:BoundField DataField="idPerfil" HeaderText="idPerfil" ReadOnly="True" 
                            SortExpression="idPerfil" Visible="False" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" 
                            SortExpression="Nombre" >
                            <ItemStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Descrpcion" HeaderText="Descrpcion" 
                            SortExpression="Descrpcion" >
                            <ItemStyle Width="300px" />
                        </asp:BoundField>
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" 
                            SortExpression="Activo" />
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
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        </asp:Panel>
        
    
    </div>

     <asp:UpdatePanel ID="pnlFormulario" runat="server">
         <ContentTemplate>
             <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                 Height="420px" Width="845px">
                 <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1">
                  <HeaderTemplate>Datos de Perfil</HeaderTemplate> 
                    <ContentTemplate>
                    <asp:Panel ID="Panel1" runat="server" GroupingText="Datos del usuario" 
                             Height="175px">
                                <asp:Label ID="Label1" runat="server" Text="Perfil:" CssClass="labelForms" 
                                 Width="130px"></asp:Label>
                                <asp:TextBox ID="txbPerfil" runat="server" 
                                    CssClass="textBoxForms" TabIndex="1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPerfil" runat="server" 
                                 ControlToValidate="txbPerfil" ErrorMessage="Perfil: Campo Requerido" 
                                 ValidationGroup="Perfil">*</asp:RequiredFieldValidator>
                                <br /><asp:Label ID="Label9" runat="server" Text="Descripción:" CssClass="labelForms" 
                                 Width="130px"></asp:Label>&#160;<asp:TextBox ID="txbDescripcion" 
                            runat="server" TabIndex="2" Width="316px"></asp:TextBox>
                                <br />
                                <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" Width="130px" /></asp:Panel>
                    
                    </ContentTemplate>
                    
                       

                  
                    
                 </asp:TabPanel>
                
                 <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                  <HeaderTemplate>Permisos</HeaderTemplate> 
                     <ContentTemplate>
                            <asp:Label ID="Label30" runat="server" Font-Italic="True" 
                                Text="N: Ninguno L: Lectura E: Escritura T: Total "></asp:Label>
                            <br />
                        <asp:Table ID="Table1" runat="server" Font-Size="X-Small">
                            <asp:TableRow ID="TableRow1" runat="server">
                                <asp:TableCell ID="TableCell1" runat="server">
                                     <!-- Usuarios -->
                                        <asp:Panel ID="pnlUsuarios" runat="server" GroupingText="Usuarios" 
                                            Width="200px">
                                            <asp:RadioButtonList ID="rblUsuarios" runat="server" 
                                                RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                        


                                        </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell2" runat="server">
                                    <!-- Perfiles -->
                                        <asp:Panel ID="pnlPerfiles" runat="server" GroupingText="Perfiles" 
                                            Width="200px">
                                            <asp:RadioButtonList ID="rblPerfiles" runat="server" 
                                                RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                         


                                         </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell3" runat="server">
                                     <!-- Clientes -->
                                    <asp:Panel ID="pnlClientes" runat="server" GroupingText="Clientes" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblClientes" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell4" runat="server">
                                    <!-- Estados -->
                                    <asp:Panel ID="pnlEstados" runat="server" GroupingText="Estados" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblEstados" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell></asp:TableRow><asp:TableRow ID="TableRow2" runat="server">
                                <asp:TableCell ID="TableCell5" runat="server">
                                     <!-- Municipios -->
                                     <asp:Panel ID="pnlMunicipios" runat="server" GroupingText="Municipios" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblMunicipios" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell6" runat="server">
                                 <!-- Líneas Créditos -->
                                 <asp:Panel ID="pnlLineasCreditos" runat="server" GroupingText="Líneas Créditos" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblLineasCreditos" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell7" runat="server">
                                  <!-- Tipos Impuesto -->
                                  <asp:Panel ID="pnlIva" runat="server" GroupingText="Impuestos" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblIva" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell8" runat="server">
                                 <!-- Vendedores -->
                                 <asp:Panel ID="pnlVendedores" runat="server" GroupingText="Vendedores" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblVendedores" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell></asp:TableRow><asp:TableRow ID="TableRow3" runat="server">
                                <asp:TableCell ID="TableCell9" runat="server">
                                   <!-- Poblaciones -->
                                   <asp:Panel ID="pnlPoblaciones" runat="server" GroupingText="Poblaciones" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblPoblaciones" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell10" runat="server">
                                  <!-- Proveedores -->
                                  <asp:Panel ID="pnlProveedores" runat="server" GroupingText="Proveedores" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblProveedores" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell11" runat="server">
                                    <!-- Colonias -->
                                   <asp:Panel ID="pnlColonias" runat="server" GroupingText="Colonias" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblColonias" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell12" runat="server">
                                    <!-- Almacenes -->
                                   <asp:Panel ID="Panel7" runat="server" GroupingText="Almacenes" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblAlmacenes" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell></asp:TableRow><asp:TableRow ID="TableRow4" runat="server">
                                <asp:TableCell ID="TableCell13" runat="server">
                                    <!-- Productos -->
                                   <asp:Panel ID="Panel8" runat="server" GroupingText="Productos" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblProductos" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell14" runat="server">
                                    <!-- Inventarios -->
                                   <asp:Panel ID="Panel9" runat="server" GroupingText="Inventarios" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblInventarios" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell15" runat="server">
                                    <!-- Facturas -->
                                   <asp:Panel ID="Panel10" runat="server" GroupingText="Facturas" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblFacturas" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell16" runat="server">
                                    <!-- Recetas -->
                                   <asp:Panel ID="Panel11" runat="server" GroupingText="Recetas" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblRecetas" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell></asp:TableRow><asp:TableRow ID="TableRow5" runat="server">
                                <asp:TableCell ID="TableCell17" runat="server">
                                    <!-- Remisiones -->
                                   <asp:Panel ID="Panel12" runat="server" GroupingText="Remisiones" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblRemisiones" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell18" runat="server">
                                    <!-- Cuentas por cobrar -->
                                   <asp:Panel ID="Panel13" runat="server" GroupingText="Cuentas x Cobrar" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblCuentasxCobrar" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell19" runat="server">
                                   <!-- Cuentas por Pedidos -->
                                   <asp:Panel ID="Panel14" runat="server" GroupingText="Pedidos" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblPedidos" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell20" runat="server">
                                   <!--Biblioteca-->
                                   <asp:Panel ID="Panel15" runat="server" GroupingText="Causes" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblCauses" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell></asp:TableRow><asp:TableRow ID="TableRow6" runat="server">
                                <asp:TableCell ID="TableCell21" runat="server">
                                   <!--Bitacora-->
                                   <asp:Panel ID="Panel16" runat="server" GroupingText="Bitacora" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblBitacora" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell22" runat="server">
                                   <!--Configuracion-->
                                   <asp:Panel ID="Panel17" runat="server" GroupingText="Configuración" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblConfiguracion" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell23" runat="server">
                                   <!--Campos-->
                                   <asp:Panel ID="Panel18" runat="server" GroupingText="Campos Editables" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblCamposEditables" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell ID="TableCell24" runat="server">
                                   <!--Tipos-->
                                   <asp:Panel ID="Panel19" runat="server" GroupingText="Tipos" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblTipos" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell></asp:TableRow><asp:TableRow ID="TableRow7" runat="server">
                                <asp:TableCell ID="TableCell25" runat="server">
                                     <!--Tipos-->
                                    <asp:Panel ID="Panel20" runat="server" GroupingText="Ensambles" 
                                        Width="200px">
                                        <asp:RadioButtonList ID="rblEnsambles" runat="server" 
                                            RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                     


                                     </asp:Panel>
</asp:TableCell></asp:TableRow></asp:Table></ContentTemplate>
                 
                 </asp:TabPanel>
             
            </asp:TabContainer>
            <br />
                <asp:Label ID="lblAviso" runat="server"></asp:Label><asp:Label ID="lblAviso2" runat="server"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                             ValidationGroup="Perfil" />
         </ContentTemplate>
            
     </asp:UpdatePanel>

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
                               
                <cr:crystalreportviewer ID="crvReporte" runat="server" AutoDataBind="true" 
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
