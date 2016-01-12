<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="Medicuri.Usuarios" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%-- Este es del header --%>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>

<%-- Este es del body o work area --%>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>
    <asp:Panel ID="pnlCatalogo" runat="server" Height="350px" ScrollBars="Auto">
        <asp:UpdatePanel ID="upnCatalogo" runat="server" >
            <ContentTemplate>
             <div id="Listado" class="divCatalogoPanel">
                <asp:GridView ID="dgvDatos" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="idUsuario" 
                    onselectedindexchanged="dgvDatos_SelectedIndexChanged" BackColor="White" 
                     BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                     ForeColor="Black" GridLines="Vertical" Width="845px" AllowPaging="True" 
                     AllowSorting="True" onpageindexchanging="dgvDatos_PageIndexChanging" 
                     onsorting="dgvDatos_Sorting" PageSize="100">
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                        SelectText="-" ShowSelectButton="True" />
                        <asp:BoundField DataField="idUsuario" HeaderText="idUsuario" ReadOnly="True" 
                            SortExpression="idUsuario" Visible="False" />
                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" 
                            SortExpression="Usuario" >
                            <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Contrasena" HeaderText="Contrasena" 
                            SortExpression="Contrasena" Visible="False" >
                            <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" 
                            SortExpression="Nombre" >
                            <ItemStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" 
                            SortExpression="Apellidos" >
                            <ItemStyle Width="200px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CorreoElectronico" HeaderText="CorreoElectronico" 
                            SortExpression="CorreoElectronico" >
                            <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Perfil" HeaderText="Perfil" >
                            <ItemStyle HorizontalAlign="Left" />
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
                </div>
            </ContentTemplate>

            
        </asp:UpdatePanel>
    </asp:Panel>
        
    
    
    
    
        <asp:UpdatePanel ID="pnlFormulario" runat="server">
            <ContentTemplate>
             <div id="Formulario" class="divCatalogoPanel" runat="server">
                <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                    Height="420px" Width="845px">
                    <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1"><HeaderTemplate>Datos de Usuario</HeaderTemplate>
                        <ContentTemplate><br />
                            <asp:Panel ID="Panel1" runat="server" GroupingText="Datos del usuario" 
                             Height="175px"><asp:Label ID="Label10" runat="server" Text="Perfil:" CssClass="labelForms" 
                                 Width="130px"></asp:Label>
                                <asp:DropDownList ID="cmbPerfiles" runat="server" CssClass="textBoxForms" 
                                 DataTextField="Nombre" DataValueField="idPerfil" 
                                    onselectedindexchanged="cmbPerfiles_SelectedIndexChanged" 
                                    AppendDataBoundItems="True" AutoPostBack="True" Width="130px"></asp:DropDownList>
                                   
                                    <br /><asp:Label ID="Label27" runat="server" Text="Almacen:" CssClass="labelForms" 
                                 Width="130px"></asp:Label>
                                <asp:DropDownList ID="cmbAlmacenes" runat="server" Width="130px" 
                                 DataTextField="Nombre" DataValueField="idAlmacen" TabIndex="3"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvAlmacen" runat="server" 
                                 ControlToValidate="cmbAlmacenes" ErrorMessage="Almacen: Campo Requerido" 
                                 ValidationGroup="Usuario" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="Label1" runat="server" Text="Usuario:" CssClass="labelForms" 
                                 Width="130px"></asp:Label>
                                <asp:TextBox ID="txbUsuario" runat="server" 
                                    CssClass="textBoxForms" TabIndex="1" Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvUsurio" runat="server" 
                                 ControlToValidate="txbUsuario" ErrorMessage="Usuario: Campo Requerido" 
                                 ValidationGroup="Usuario" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <br /><asp:Label ID="Label9" runat="server" Text="Contraseña:" CssClass="labelForms" 
                                 Width="130px"></asp:Label>&#160;<asp:TextBox ID="txbContraseña" 
                            runat="server" TextMode="Password" TabIndex="2" Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvContrasena" runat="server" 
                                 ControlToValidate="txbContraseña" ErrorMessage="Contraseña: Campo Requerido" 
                                 ValidationGroup="Usuario" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="chkFiltrado" runat="server" Text="Filtrado Por Almacen" />
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" Width="130px" /></asp:Panel> </ContentTemplate></asp:TabPanel>
                    <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2"><HeaderTemplate>Datos Personales</HeaderTemplate>
                        <ContentTemplate><br />
                            <asp:Panel ID="Panel2" runat="server" GroupingText="Datos Personales" 
                                Height="138px"><asp:Label ID="Label12" runat="server" Text="Nombre(s):" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbNombre" runat="server" CssClass="textBoxForms"></asp:TextBox>
                                <br /><asp:Label ID="Label13" runat="server" Text="Apellidos:" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbApellidos" runat="server" CssClass="textBoxForms"></asp:TextBox><br />
                                <asp:Label ID="Label11" runat="server" Text="Correo Electrónico:" 
                                    CssClass="labelForms" Width="130px"></asp:Label>
                                <asp:TextBox ID="txbCorreo" runat="server" CssClass="textBoxForms"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCorreoElectronico" runat="server" 
                                    ControlToValidate="txbCorreo" 
                                    ErrorMessage="Correo Electrónico: Campo Requerido" ValidationGroup="Usuario">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revCorreo" runat="server" 
                            ErrorMessage="Correo: Formato de correo no valido" 
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                            ValidationGroup="Usuario" ControlToValidate="txbCorreo">*</asp:RegularExpressionValidator>
                        </asp:Panel><br /><br /></ContentTemplate></asp:TabPanel>
                    <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="TabPanel3"><HeaderTemplate>Datos Opcionales</HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="Panel3" runat="server" GroupingText="Campos Editables" 
                                Height="267px" Width="865px"><asp:Panel ID="Panel4" runat="server" GroupingText="Campos Alfabeticos" 
                                    Height="66px"><asp:Label ID="lblAlfanumerico1" runat="server" Text="Campo 1:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo1" runat="server"></asp:TextBox>

                                    <asp:Label ID="lblAlfanumerico2" runat="server" Text="Campo 2:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo2" runat="server" Height="25px"></asp:TextBox>
                                    
                                    <asp:Label ID="lblAlfanumerico3" runat="server" Text="Campo 3:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo3" runat="server"></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:Label ID="lblAlfanumerico4" runat="server" Text="Campo 4:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo4" runat="server"></asp:TextBox>
                                    
                                    <asp:Label ID="lblAlfanumerico5" runat="server" Text="Campo 5:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo5" runat="server"></asp:TextBox></asp:Panel>
                                <br />
                                <asp:Panel ID="Panel5" runat="server" GroupingText="Campos Numericos" 
                                    Height="75px" style="margin-top: 9px">
                                    
                                    <asp:Label ID="lblEntero1" runat="server" Text="Campo 6:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo6" runat="server"></asp:TextBox>
                                    
                                    <asp:Label ID="lblEntero2" runat="server" Text="Campo 7:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo7" runat="server"></asp:TextBox>
                                    
                                    <asp:Label ID="lblEntero3" runat="server" Text="Campo 8:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo8" runat="server"></asp:TextBox>
                                    </asp:Panel>                                    
                                <asp:Panel ID="Panel6" runat="server" GroupingText="Campos Decimales" 
                                    Height="82px">
                                    <asp:Label ID="lblDecimal1" runat="server" Text="Campo 9:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo9" runat="server"></asp:TextBox>
                                    
                                    <asp:Label ID="lblDecimal2" runat="server" Text="Campo 10:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo10" runat="server"></asp:TextBox></asp:Panel></asp:Panel></ContentTemplate></asp:TabPanel>
                    <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="TabPanel4"><HeaderTemplate>Permisos</HeaderTemplate>
                        <ContentTemplate>
                            <asp:Label ID="Label30" runat="server" Font-Italic="True" 
                                Text="N: Ninguno L: Lectura E: Escritura T: Total "></asp:Label>
                            <br />
                        <asp:Table ID="Table1" runat="server" Font-Size="X-Small">
                            <asp:TableRow runat="server"><asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                                <asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
                                  <!-- Tipos Impuesto -->
                                  <asp:Panel ID="pnlIva" runat="server" GroupingText="Impuesto" 
                                    Width="200px">
                                    <asp:RadioButtonList ID="rblIva" runat="server" 
                                        RepeatDirection="Horizontal"><asp:ListItem Selected="True" Value="N">N</asp:ListItem>
<asp:ListItem Value="L">L</asp:ListItem>
<asp:ListItem Value="E">E</asp:ListItem>
<asp:ListItem Value="T">T</asp:ListItem>
</asp:RadioButtonList>



                                


                                </asp:Panel>
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                                <asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                                <asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                                <asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                                <asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell><asp:TableCell runat="server">
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
</asp:TableCell></asp:TableRow><asp:TableRow runat="server">
                                <asp:TableCell runat="server">
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
</asp:TableCell></asp:TableRow></asp:Table></ContentTemplate></asp:TabPanel>
                </asp:TabContainer>
                <br />
                <asp:Label ID="lblAviso" runat="server" ForeColor="Red"></asp:Label>
                 <asp:Label ID="lblAviso2" runat="server" ForeColor="Red"></asp:Label>
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                             ValidationGroup="Usuario" ForeColor="Red" />
                </div>
            </ContentTemplate>
      </asp:UpdatePanel>

       <%--<div id="divReportes">
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
            
            <asp:UpdatePanel ID="upnReportes" runat="server">
            <ContentTemplate>
                <CR:CrystalReportViewer ID="crvReporte" runat="server" AutoDataBind="true" 
                    BorderStyle="None" HasExportButton="False" 
                    HasPrintButton="False" Height="50px" ReuseParameterValuesOnRefresh="True" 
                    ToolPanelWidth="150px"
                    onnavigate="crvReporte_Navigate" 
                    onsearch="crvReporte_Search" onviewzoom="crvReporte_ViewZoom" 
                    ondrill="crvReporte_Drill" ondatabinding="crvReporte_DataBinding"  />
            </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>--%>
    
    




</asp:Content>

