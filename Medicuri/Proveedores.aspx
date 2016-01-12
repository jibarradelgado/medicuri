<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Proveedores.aspx.cs" Inherits="Medicuri.Proveedores" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="FiltroReportes" TagName="Filtro" Src="~/FiltroReportes.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Proveedores" HeaderText="Verifique los siguientes campos" />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" HeaderText="Verifique los siguientes campos" ValidationGroup="Contactos" />
    <div id="lista">
        <asp:Panel ID="pnlLista" runat="server" Height="350px" ScrollBars="Auto">
            <asp:UpdatePanel ID="upnLista" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gdvLista" runat="server" AutoGenerateColumns="False" 
                    PageSize="100" 
                    
                    DataKeyNames="idProveedor,idEstado,idMunicipio,idPoblacion,idColonia,idTipoProveedor" 
                    onselectedindexchanged="gdvLista_SelectedIndexChanged" BackColor="White" 
                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                    ForeColor="Black" GridLines="Vertical" Width="840px" AllowPaging="True" 
                    AllowSorting="True" onpageindexchanging="gdvLista_PageIndexChanging" 
                    onsorting="gdvLista_Sorting">
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" ShowSelectButton="True" 
                            SelectImageUrl="~/Icons/right_16.png" />
                        <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" 
                            SortExpression="Nombre" />
                        <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" 
                            SortExpression="Apellidos" />
                        <asp:BoundField DataField="TipoPersona" HeaderText="Tipo de Persona" 
                            SortExpression="TipoPersona" />
                        <asp:BoundField DataField="Rfc" HeaderText="RFC" SortExpression="RFC" />
                        <asp:BoundField DataField="Telefono" HeaderText="Teléfono" 
                            SortExpression="Telefono" />
                        <asp:BoundField DataField="Celular" HeaderText="Celular" 
                            SortExpression="Celular" />
                        <asp:BoundField DataField="CorreoElectronico" HeaderText="Correo Electrónico" 
                            SortExpression="CorreoElectronico" />
                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha de Registro" 
                            SortExpression="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" ReadOnly="True" 
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
            </ContentTemplate>
        </asp:UpdatePanel>
        </asp:Panel>        
    </div>
    <div id="form">
     <asp:UpdatePanel ID="upnForm" runat="server">
        <ContentTemplate>
            <asp:TabContainer ID="tbcForm" runat="server" ActiveTabIndex="0" Width="840px">
                <asp:TabPanel ID="tbpDatosAlmacen" runat="server" HeaderText="Datos del Proveedor">
                    <HeaderTemplate>
                       Datos del Proveedor
                    </HeaderTemplate>
                    <ContentTemplate>
                        <asp:Panel ID="pnlGeneral" runat="server" GroupingText="General">
                            <asp:Label ID="lblClave" runat="server" Text="Clave:" CssClass="labelForms"  
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txtClave" runat="server" CssClass="textBoxForms" 
                                MaxLength="25" Width="130px"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" 
                                ControlToValidate="txtClave" ErrorMessage="Clave: ya existe" 
                                onservervalidate="CustomValidator1_ServerValidate" 
                                ValidationGroup="Proveedores" ForeColor="Red">*</asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="rfvClave" runat="server" 
                                ErrorMessage="Clave: campo requerido" ControlToValidate="txtClave" 
                            ValidationGroup="Proveedores" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:Label ID="lblNombre" runat="server" Text="Nombre(s):" 
                                CssClass="labelForms" Width="64px"></asp:Label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="textBoxForms" 
                                MaxLength="75" Width="130px"></asp:TextBox>
                            <asp:Label ID="lblApellidos" runat="server" Text="Apellido(s):" 
                                CssClass="labelForms" Width="90px"></asp:Label>
                            <asp:TextBox ID="txtApellidos" runat="server" CssClass="textBoxForms" 
                                MaxLength="75" Width="130px"></asp:TextBox>
                            <br />                            
                            <asp:Label ID="lblRfc" runat="server" Text="R.F.C.:" CssClass="labelForms" 
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txtRfc" runat="server" CssClass="textBoxForms" MaxLength="13" 
                                Width="130px"></asp:TextBox>                            
                            <asp:RegularExpressionValidator ID="revRFC" runat="server" 
                                    ControlToValidate="txtRfc" ErrorMessage="RFC Incorrecto" 
                                    ValidationGroup="Proveedores" ValidationExpression="\w{10,13}"
                                    ForeColor="Red" Text="*"></asp:RegularExpressionValidator>                           
                            <asp:Label ID="lblCurp" runat="server" Text="C.U.R.P.:" CssClass="labelForms" 
                                Width="77px"></asp:Label>
                            <asp:TextBox ID="txtCurp" runat="server" CssClass="textBoxForms" MaxLength="18" 
                                Width="130px"></asp:TextBox>                                
                            <asp:Label ID="lblTipo" runat="server" Text="Tipo:" CssClass="labelForms"  
                                Width="90px"></asp:Label>
                            <asp:ComboBox ID="cmbTipo" runat="server" MaxLength="0" Width="110px" 
                                DataTextField="Nombre" DataValueField="idTipo" 
                                ValidationGroup="Proveedores" DropDownStyle="DropDownList">
                            </asp:ComboBox>
                            <asp:CustomValidator ID="cmvTipo" runat="server" ControlToValidate="cmbTipo" 
                                ErrorMessage="Tipo: Dato no válido" Text="*" ValidationGroup="Proveedores" 
                                onservervalidate="cmvTipo_ServerValidate" ForeColor="Red"></asp:CustomValidator>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                            <br />
                            <asp:CheckBox ID="ckbActivo" runat="server" Text="Activo" Width="130px" 
                                CssClass="labelForms" />
                        </asp:Panel>
                        <asp:Panel ID="pnlContacto" runat="server" GroupingText="Domicilio">
                            <asp:Label ID="lblCalle" runat="server" Text="Calle:" CssClass="labelForms" 
                                Width="90px" Height="16px"></asp:Label>
                            <asp:TextBox ID="txtCalle" runat="server" CssClass="textBoxForms" 
                                MaxLength="75" Width="130px"></asp:TextBox>                            
                            <asp:Label ID="lblNumExt" runat="server" Text="Num. Ext.:" CssClass="labelForms" 
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txtNumExt" runat="server" CssClass="textBoxForms" 
                                MaxLength="10" Width="130px"></asp:TextBox>
                            <asp:Label ID="lblNumInt" runat="server" Text="Num. Int.:" CssClass="labelForms" 
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txtNumInt" runat="server" CssClass="textBoxForms" 
                                MaxLength="10" Width="130px"></asp:TextBox>
                            <br />
                            <asp:Label ID="lblEstado" runat="server" Text="Estado:" CssClass="labelForms" 
                                Width="90px"></asp:Label>
                            <asp:ComboBox ID="cmbEstado" runat="server" MaxLength="0" 
                                CssClass="comboBoxForms" Width="110px" DataTextField="Nombre" 
                                DataValueField="idEstado" AutoPostBack="True" 
                                onselectedindexchanged="cmbEstado_SelectedIndexChanged" 
                                ValidationGroup="Proveedores" DropDownStyle="DropDownList"></asp:ComboBox>
                            <asp:CustomValidator ID="cmvEstado" runat="server" 
                                ErrorMessage="Estado: Dato no válido" Text="*" ValidationGroup="Proveedores" 
                                onservervalidate="cmvEstado_ServerValidate" ForeColor="Red"></asp:CustomValidator>
                            <asp:Label ID="lblDelMcpio" runat="server" Text="Del./Mcpio.:" 
                                CssClass="labelForms" Width="77px"></asp:Label>
                            <asp:ComboBox ID="cmbMunicipio" runat="server" MaxLength="0" Width="110px" 
                                CssClass="comboBoxForms" DataTextField="Nombre" 
                                DataValueField="idMunicipio" AutoPostBack="True" 
                                onselectedindexchanged="cmbMunicipio_SelectedIndexChanged" 
                                ValidationGroup="Proveedores" DropDownStyle="DropDownList"></asp:ComboBox>
                            <asp:CustomValidator ID="cmvMunicipio" runat="server" 
                                ErrorMessage="Municipio: Dato no válido" ControlToValidate="cmbMunicipio" 
                                Text="*" ValidationGroup="Proveedores" 
                                onservervalidate="cmvMunicipio_ServerValidate" ForeColor="Red"></asp:CustomValidator>
                            <asp:Label ID="lblCiudad" runat="server" Text="Ciudad:" CssClass="labelForms" 
                                Width="77px"></asp:Label>
                            <asp:ComboBox ID="cmbPoblacion" runat="server" MaxLength="0" Width="110px" 
                                CssClass="comboBoxForms" DataTextField="Nombre" 
                                DataValueField="idPoblacion" AutoPostBack="True" 
                                onselectedindexchanged="cmbPoblacion_SelectedIndexChanged" 
                                ValidationGroup="Proveedores" DropDownStyle="DropDownList"></asp:ComboBox>
                            <asp:CustomValidator ID="cmvPoblacion" runat="server" 
                                ErrorMessage="Población: Dato no válido" Text="*" ValidationGroup="Proveedores" 
                                onservervalidate="cmvPoblacion_ServerValidate" ForeColor="Red"></asp:CustomValidator>
                            <br />           
                            <br />                 
                            <asp:Label ID="lblColonia" runat="server" Text="Colonia:" CssClass="labelForms" 
                                Width="90px"></asp:Label>
                            <asp:ComboBox ID="cmbColonia" runat="server" MaxLength="0" Width="110px" 
                                CssClass="comboBoxForms" DataTextField="Nombre" DataValueField="idColonia" 
                                onselectedindexchanged="cmbColonia_SelectedIndexChanged" 
                                ValidationGroup="Proveedores" DropDownStyle="DropDownList"></asp:ComboBox>
                            <asp:CustomValidator ID="cmvColonia" runat="server" 
                                ErrorMessage="Colonia: Dato no válido" Text="*" ValidationGroup="Proveedores" 
                                onservervalidate="cmvColonia_ServerValidate" ForeColor="Red"></asp:CustomValidator>                                      
                            <asp:Label ID="lblPais" runat="server" Text="País:" CssClass="labelForms" 
                                Width="77px"></asp:Label>                            
                            <asp:TextBox ID="txtPais" runat="server" CssClass="textBoxForms" 
                                ReadOnly="True" Width="130px">México</asp:TextBox>
                            <asp:Label ID="lblCodigoPostal" runat="server" Text="Código Postal:" 
                                CssClass="labelForms" Width="90px"></asp:Label>
                            <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="textBoxForms" 
                                MaxLength="5" Width="130px"></asp:TextBox>
                            <br />                            
                            <asp:Label ID="lblTelefono" runat="server" Text="Teléfono:" 
                                CssClass="labelForms" Width="90px"></asp:Label>
                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="textBoxForms" 
                                MaxLength="15" Width="130px"></asp:TextBox>
                            <asp:Label ID="lblCelular" runat="server" Text="Celular:" 
                                CssClass="labelForms" Width="90px"></asp:Label>
                            <asp:TextBox ID="txtCelular" runat="server" CssClass="textBoxForms" 
                                MaxLength="15" Width="130px"></asp:TextBox>
                            <asp:Label ID="lblFax" runat="server" Text="Fax:" CssClass="labelForms"  
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txtFax" runat="server" CssClass="textBoxForms" MaxLength="15" 
                                Width="130px"></asp:TextBox>
                            <br />
                            <asp:Label ID="lblCorreoElectronico" runat="server" Text="Correo Electrónico:" CssClass="labelForms"  
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txtCorreoElectronico" runat="server" CssClass="textBoxForms" 
                                MaxLength="50" Width="130px"></asp:TextBox>
                            <asp:Label ID="lblTipoPersona" runat="server" Text="Tipo de Persona:" CssClass="labelForms" Width="90px"></asp:Label>
                            <asp:ComboBox ID="cmbTipoPersona" runat="server" MaxLength="0" 
                                AutoPostBack="True" 
                                Width="110px" onselectedindexchanged="cmbTipoPersona_SelectedIndexChanged" 
                                DropDownStyle="DropDownList">
                                <asp:ListItem Value="FISICA">Fisica</asp:ListItem>
                                <asp:ListItem Value="MORAL">Moral</asp:ListItem>
                            </asp:ComboBox>

                        </asp:Panel>                       
                    </ContentTemplate>
                </asp:TabPanel>
                 <asp:TabPanel ID="tbpContacto" runat="server" HeaderText="Datos de Contactos">
                     <HeaderTemplate>
                         Datos de Contactos
                     </HeaderTemplate>
                     <ContentTemplate>
                     <asp:Label ID="lblCntNombre" runat="server" Text="Nombre(s):" CssClass="labelForms" 
                             Width="90px"></asp:Label>
                     <asp:TextBox ID="txtCntNombre" runat="server" CssClass="textBoxForms" 
                             ValidationGroup="Contactos" MaxLength="75"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rfvNombreContacto" runat="server" 
                             ControlToValidate="txtCntNombre" ErrorMessage="Nombre: campo requerido" 
                             ValidationGroup="Contactos" ForeColor="Red">*</asp:RequiredFieldValidator>
                     <asp:Label ID="lblCntApellidos" runat="server" Text="Apellido(s):" 
                             CssClass="labelForms" Width="77px"></asp:Label>
                     <asp:TextBox ID="txtCntApellidos" runat="server" CssClass="textBoxForms" 
                             ValidationGroup="Contactos" MaxLength="75"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rfvApellidos" runat="server" 
                             ControlToValidate="txtCntApellidos" ErrorMessage="Apellidos: Campo requerido" 
                             ValidationGroup="Contactos" ForeColor="Red">*</asp:RequiredFieldValidator>
                     
                     <asp:Label ID="lblCntTel" runat="server" Text="Teléfono:" CssClass="labelForms" 
                             Width="77px"></asp:Label>
                     <asp:TextBox ID="txtCntTel" runat="server" CssClass="textBoxForms" 
                             ValidationGroup="Contactos" MaxLength="15"></asp:TextBox>   
                     <br />                      
                     <asp:Label ID="lblCntCel" runat="server" Text="Celular:" CssClass="labelForms" 
                             Width="90px"></asp:Label>
                     <asp:TextBox ID="txtCntCel" runat="server" CssClass="textBoxForms" 
                             ValidationGroup="Contactos" MaxLength="15"></asp:TextBox>
                     <asp:Label ID="lblCntCorreoE" runat="server" Text="Correo Electrónico:" 
                             CssClass="labelForms" Width="90px"></asp:Label>
                     <asp:TextBox ID="txtCntCorreoE" runat="server" CssClass="textBoxForms" 
                             ValidationGroup="Contactos" MaxLength="50"></asp:TextBox>
                     <asp:Label ID="lblCntFax" runat="server" Text="Fax:" CssClass="labelForms" 
                             Width="90px"></asp:Label>
                     <asp:TextBox ID="txtCntFax" runat="server" CssClass="textBoxForms" 
                             ValidationGroup="Contactos" MaxLength="15"></asp:TextBox>
                         <br />
                     <asp:Label ID="lblCntDepto" runat="server" Text="Departamento:" 
                             CssClass="labelForms" Width="90px"></asp:Label>
                     <asp:TextBox ID="txtCntDepto" runat="server" CssClass="textBoxForms" 
                             ValidationGroup="Contactos" MaxLength="75"></asp:TextBox>
                     <asp:ImageButton ID="imbAgregarContacto" runat="server" 
                            onclick="imbAgregarContacto_Click" ImageUrl="~/Icons/plus_16.png" 
                             ValidationGroup="Contactos" />
                        <asp:GridView ID="gdvContactos" runat="server" AutoGenerateColumns="False" 
                             onselectedindexchanged="gdvContactos_SelectedIndexChanged" 
                             BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                             CellPadding="3" ForeColor="Black" GridLines="Vertical" Width="840px">
                            <AlternatingRowStyle BackColor="#CCCCCC" />
                            <Columns>
                                 <asp:BoundField HeaderText="Nombre(s)" DataField="Nombre" />
                                 <asp:BoundField HeaderText="Apellido(s)" DataField="Apellidos" />
                                 <asp:BoundField HeaderText="Teléfono" DataField="Telefono" />
                                 <asp:BoundField HeaderText="Fax" DataField="Fax" />
                                 <asp:BoundField HeaderText="Celular" DataField="Celular" />
                                 <asp:BoundField HeaderText="Correo electrónico" DataField="CorreoElectronico" />
                                 <asp:BoundField HeaderText="Departamento" DataField="Departamento" />
                                 <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/delete_16.png" 
                                    ShowSelectButton="True" />
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
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tbpDatosOpcionales" runat="server" HeaderText="Datos Opcionales">
                        <ContentTemplate>
                            <asp:Panel ID="pnlaAlfanumericos" runat="server" GroupingText="Campos Alfanumericos">
                                <asp:Label ID="lblAlfanumerico1" runat="server" Text="Campo 1:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txtAlfanumerico1" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25"></asp:TextBox>
                                <asp:Label ID="lblAlfanumerico2" runat="server" Text="Campo 2:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txtAlfanumerico2" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25"></asp:TextBox>
                                <asp:Label ID="lblAlfanumerico3" runat="server" Text="Campo 3:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txtAlfanumerico3" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25"></asp:TextBox>
                                <br />                            
                                <asp:Label ID="lblAlfanumerico4" runat="server" Text="Campo 4:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txtAlfanumerico4" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25"></asp:TextBox>                                
                                <asp:Label ID="lblAlfanumerico5" runat="server" Text="Campo 5:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txtAlfanumerico5" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25"></asp:TextBox>
                            </asp:Panel>
                            <asp:Panel ID="pnlEnteros" runat="server" GroupingText="Campos Enteros">
                                <asp:Label ID="lblEntero1" runat="server" Text="Campo 6:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txtEntero1" runat="server" CssClass="textBoxForms"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revEntero1" runat="server" 
                                    ErrorMessage="Entero1: Numeros enteros" ValidationExpression="^(\+|-)?\d+$" 
                                    ValidationGroup="Proveedores" ControlToValidate="txtEntero1" ForeColor="Red">*</asp:RegularExpressionValidator>
                                <asp:Label ID="lblEntero2" runat="server" Text="Campo 7:" CssClass="labelForms" 
                                    Width="77px"></asp:Label>
                                <asp:TextBox ID="txtEntero2" runat="server" CssClass="textBoxForms"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revEntero2" runat="server" 
                                    ErrorMessage="Entero2: Numeros enteros" ValidationExpression="^(\+|-)?\d+$" 
                                    ValidationGroup="Proveedores" ControlToValidate="txtEntero2" ForeColor="Red">*</asp:RegularExpressionValidator>
                                <asp:Label ID="lblEntero3" runat="server" Text="Campo 8:" CssClass="labelForms" 
                                    Width="77px"></asp:Label>
                                <asp:TextBox ID="txtEntero3" runat="server" CssClass="textBoxForms"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revEntero3" runat="server" 
                                    ErrorMessage="Entero3: Numeros enteros" ValidationExpression="^(\+|-)?\d+$" 
                                    ValidationGroup="Proveedores" ControlToValidate="txtEntero3" ForeColor="Red">*</asp:RegularExpressionValidator>
                                <br />
                            </asp:Panel>                            
                            <asp:Panel ID="pnlDecimales" runat="server" GroupingText="Campos Decimales">
                                <asp:Label ID="lblDecimal1" runat="server" Text="Campo 9:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txtDecimal1" runat="server" CssClass="textBoxForms"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revDecimal1" runat="server" 
                                    ErrorMessage="Decimal1: Numeros decimales" 
                                    ValidationExpression="^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$" 
                                    ValidationGroup="Proveedores" ControlToValidate="txtDecimal1" Text="*" 
                                    ForeColor="Red"></asp:RegularExpressionValidator>
                                <asp:Label ID="lblDecimal2" runat="server" Text="Campo 10:" 
                                    CssClass="labelForms" Width="77px"></asp:Label>
                                <asp:TextBox ID="txtDecimal2" runat="server" CssClass="textBoxForms"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revDecimal2" runat="server" 
                                    ErrorMessage="Decimal2: Numeros decimales" 
                                    ValidationExpression="^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$" 
                                    ValidationGroup="Proveedores" ControlToValidate="txtDecimal2" Text="*" 
                                    ForeColor="Red"></asp:RegularExpressionValidator>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
            </asp:TabContainer>
        </ContentTemplate>
     </asp:UpdatePanel>
    </div>        
    <div>
        <asp:UpdatePanel ID="upnAvisos" runat="server">
            <ContentTemplate>
                <asp:Label id="lblAviso" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label id="lblAviso2" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label ID="lblAviso3" runat="server" Text=""></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div style="margin-left:60px">
        <asp:Panel ID="pnlFiltroReportes" runat="server">
            <FiltroReportes:Filtro runat="server" ID="frReportes" />
        </asp:Panel>
    </div>
</asp:Content>

