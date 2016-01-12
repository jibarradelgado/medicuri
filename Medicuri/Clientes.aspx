<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="Medicuri.Clientes" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>
<%@ Register TagPrefix="FiltroReportes" TagName="Filtro" Src="~/FiltroReportes.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <div><asp:Label ID="lblResults" runat="server" Font-Bold="True"></asp:Label></div>
    <asp:ToolkitScriptManager runat="server" EnableScriptGlobalization="true">
    </asp:ToolkitScriptManager>

    <%-- Items List --%>
    <div id="divCatalogo">
        <asp:Panel ID="pnlList" runat="server" Height="350px" ScrollBars="Auto">
            <asp:UpdatePanel ID="upnList" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gdvDatos" runat="server" BackColor="White" 
                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                    ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" 
                    Width="845px" 
                    DataKeyNames="idCliente,idEstado,idMunicipio,idPoblacion,idColonia" 
                    AllowSorting="True" onsorting="gdvDatos_Sorting" AllowPaging="True" 
                    onpageindexchanging="gdvDatos_PageIndexChanging" PageSize="100">
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                            ShowSelectButton="True" />
                        <asp:BoundField DataField="idCliente" HeaderText="idCliente" 
                            SortExpression="idCliente" Visible="False" />
                        <asp:BoundField DataField="Clave1" HeaderText="Clave" 
                            SortExpression="Clave1" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" 
                            SortExpression="Nombre" />
                        <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" 
                            SortExpression="Apellidos" />
                        <asp:BoundField DataField="Rfc" HeaderText="RFC" SortExpression="Rfc" />
                        <asp:BoundField DataField="TipoPersona" HeaderText="Persona" />
                        <asp:BoundField DataField="Telefono" HeaderText="Teléfono" 
                            SortExpression="Telefono" />
                        <asp:BoundField DataField="Celular" HeaderText="Celular" 
                            SortExpression="Celular" />
                        <asp:BoundField DataField="CorreoElectronico" HeaderText="Correo Electrónico" 
                            SortExpression="CorreoElectronico" />
                        <asp:BoundField DataField="TipoCliente" HeaderText="Tipo" />
                        <asp:BoundField DataField="FechaAlta" HeaderText="Fecha Alta" 
                            SortExpression="FechaAlta" DataFormatString="{0:d}" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
        </asp:Panel>
        
    </div>
    <br />

    <%-- Item Form --%>
    <div id="divFormulario">
    <asp:UpdatePanel ID="upnForm" runat="server">
            <ContentTemplate>
                <asp:TabContainer ID="tabContainer" runat="server" ActiveTabIndex="0" 
                    Width="845px">
                <%-- tabDatosClientes --%>
                    <asp:TabPanel runat="server" HeaderText="Datos de Cliente" ID="tabDatosCliente">
                        <ContentTemplate>
                            <asp:Panel ID="pnlDatosGenerales" runat="server" GroupingText="Datos Generales">
                                <asp:Label ID="lblClave1" runat="server" Text="Clave 1:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>  
                                <asp:TextBox ID="txbClave1" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25" Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClave" runat="server" 
                                    ErrorMessage="Campo requerido: Clave1" ForeColor="Red" 
                                    ValidationGroup="vgCliente" ControlToValidate="txbClave1">*</asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cmvClave" runat="server" ErrorMessage="La clave ya existe"
                                    ForeColor="Red" ValidationGroup="vgCliente" ControlToValidate="txbClave1" OnServerValidate="cmvClave_ServerValidate">*</asp:CustomValidator>
                                <asp:Label ID="lblClave2" runat="server" Text="Clave 2:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>  
                                <asp:TextBox ID="txbClave2" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25" Width="130px"></asp:TextBox>
                                <asp:Label ID="lblClave3" runat="server" Text="Clave 3:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>  
                                <asp:TextBox ID="txbClave3" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25" Height="22px" Width="130px"></asp:TextBox>
                                <br />
                                <asp:Label ID="lblEstatus" runat="server" Text="Tipo:" CssClass="labelForms" 
                                        Width="90px"></asp:Label> 
                                <asp:ComboBox ID="ddlTipo" runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Nombre" DataValueField="idTipo" 
                                    Width="110px" MaxLength="0" DropDownStyle="DropDownList">
                                </asp:ComboBox>
                                <asp:Label ID="lblFechaAlta" runat="server" Text="Fecha Alta:" 
                                        CssClass="labelForms" Width="102px"></asp:Label>
                                <asp:TextBox ID="txbFechaAlta" runat="server" Width="130px" 
                                    Enabled="False"></asp:TextBox>
                                <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" />
                            </asp:Panel>
                            <asp:Panel ID="pnlComplementos" runat="server" GroupingText="Complementarios">
                                <asp:Label ID="lblNombres" runat="server" Text="Nombre(s):" CssClass="labelForms" 
                                    Width="90px"></asp:Label>  
                                <asp:TextBox ID="txbNombres" runat="server" CssClass="textBoxForms" 
                                    MaxLength="75" Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                                    ErrorMessage="Campo requerido: Nombres" ForeColor="Red" 
                                    ValidationGroup="vgCliente" ControlToValidate="txbNombres">*</asp:RequiredFieldValidator>
                                <asp:Label ID="lblApellidos" runat="server" Text="Apellido(s):" CssClass="labelForms" 
                                        Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbApellidos" runat="server" CssClass="textBoxForms" 
                                    MaxLength="75" Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvApellidos" runat="server" 
                                    ErrorMessage="Campo requerido: Apellidos" ForeColor="Red" 
                                    ValidationGroup="vgCliente" ControlToValidate="txbApellidos">*</asp:RequiredFieldValidator>   
                                <asp:Label ID="lblTipoPersona" runat="server" Text="Tipo de Persona:" 
                                    CssClass="labelForms" Width="77px"></asp:Label>
                                <asp:ComboBox ID="cmbTipoPersona" runat="server" MaxLength="0" 
                                    AutoPostBack="True" 
                                    onselectedindexchanged="cmbTipoPersona_SelectedIndexChanged" Width="110px" 
                                    DropDownStyle="DropDownList">
                                    <asp:ListItem Value="FISICA">Fisica</asp:ListItem>
                                    <asp:ListItem Value="MORAL">Moral</asp:ListItem>
                                </asp:ComboBox>
                                <br />                                
                                <asp:Label ID="lblRFC" runat="server" Text="RFC:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbRFC" runat="server" CssClass="textBoxForms" MaxLength="13" 
                                    Width="130px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revRFC" runat="server" 
                                    ControlToValidate="txbRFC" ErrorMessage="RFC Incorrecto" 
                                    ValidationGroup="vgCliente" ValidationExpression="\w{10,13}"
                                    ForeColor="Red">*</asp:RegularExpressionValidator>
                                <asp:Label ID="lblCurp" runat="server" Text="CURP:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbCurp" runat="server" CssClass="textBoxForms" MaxLength="18" 
                                    Width="130px"></asp:TextBox>
                                <asp:Label ID="lblSexo" runat="server" Text="Sexo:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:ComboBox ID="cmbSexo" runat="server" Width="110px" MaxLength="0" 
                                    DropDownStyle="DropDownList">
                                    <asp:ListItem Value="M">Masculino</asp:ListItem>
                                    <asp:ListItem Value="F">Femenino</asp:ListItem>
                                </asp:ComboBox>                        
                                <br />      
                                <asp:Label ID="lblEdad" runat="server" Text="Edad:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbEdad" runat="server" CssClass="textBoxForms" Width="130px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revEdad" runat="server" 
                                    ErrorMessage="No es una edad valida" ForeColor="Red" 
                                    ValidationGroup="vgCliente" ControlToValidate="txbEdad" 
                                    ValidationExpression="\d{0,2}">*</asp:RegularExpressionValidator>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>

                    <asp:TabPanel runat="server" HeaderText="Datos de Contacto" ID="tabDatosContacto">
                        <ContentTemplate>
                            <asp:Panel ID="pnlDireccion" runat="server" GroupingText="Dirección">
                                <asp:Label ID="lblCalle" runat="server" Text="Calle:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbCalle" runat="server" CssClass="textBoxForms" 
                                    MaxLength="75" Width="130px"></asp:TextBox>
                                <asp:Label ID="lblNumeroExterior" runat="server" Text="No. Exterior:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>  
                                <asp:TextBox ID="txbNumeroExterior" runat="server" CssClass="textBoxForms" 
                                    MaxLength="5" Width="130px"></asp:TextBox>
                                <asp:Label ID="lblNumeroInterior" runat="server" Text="No. Interior:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>  
                                <asp:TextBox ID="txbNumeroInterior" runat="server" CssClass="textBoxForms" 
                                    MaxLength="5" Width="130px"></asp:TextBox>
                                <br />

                                <asp:Label ID="lblPais" runat="server" Text="País:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbPais" runat="server" CssClass="textBoxForms" 
                                    ReadOnly="True" Width="130px">México</asp:TextBox>
                                <asp:Label ID="lblEstado" runat="server" Text="Estado:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:ComboBox ID="cmbEstados" runat="server" AutoPostBack="True" 
                                    DataTextField="Nombre" DataValueField="idEstado"
                                    onselectedindexchanged="cmbEstadoFormulario_SelectedIndexChanged" 
                                    Width="110px" MaxLength="0" DropDownStyle="DropDownList">
                                </asp:ComboBox> 
                                <asp:Label ID="lblMunicipio" runat="server" Text="Municipio:" CssClass="labelForms" 
                                    Width="90px" ></asp:Label>
                                <asp:ComboBox ID="cmbMunicipios" runat="server"
                                    Width="110px" AutoPostBack="True" DataTextField="Nombre" 
                                    DataValueField="idMunicipio" 
                                    onselectedindexchanged="cmbMunicipioFormulario_SelectedIndexChanged" 
                                    MaxLength="0" 
                                    AutoCompleteMode="SuggestAppend">    
                                </asp:ComboBox>
                                <br />
                                <asp:Label ID="lblCiudad" runat="server" Text="Población:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:ComboBox ID="cmbPoblaciones" runat="server" 
                                    Width="110px" AutoPostBack="True" DataTextField="Nombre" 
                                    DataValueField="idPoblacion" 
                                    onselectedindexchanged="cmbPoblacionFormulario_SelectedIndexChanged" 
                                    MaxLength="0" DropDownStyle="DropDownList">
                                </asp:ComboBox>                                 
                                <asp:Label ID="lblColonia" runat="server" Text="Colonia:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:ComboBox ID="cmbColonias" runat="server"
                                    DataTextField="Nombre" DataValueField="idColonia" 
                                    Width="110px" MaxLength="0" DropDownStyle="DropDownList">
                                </asp:ComboBox>                                
                                <asp:Label ID="lblCodigoPostal" runat="server" Text="Codigo Postal:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbCP" runat="server" CssClass="textBoxForms" MaxLength="5" 
                                    Width="130px"></asp:TextBox>
                            </asp:Panel><br />
                            <asp:Panel ID="pnlContacto" runat="server" GroupingText="Contacto">
                                <asp:Label ID="lblTelefono" runat="server" Text="Teléfono:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbTelefono" runat="server" CssClass="textBoxForms" 
                                    Width="130px">15</asp:TextBox>
                                <asp:Label ID="lblCelular" runat="server" Text="Celular:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbCelular" runat="server" CssClass="textBoxForms" 
                                    Width="130px">15</asp:TextBox>                                
                                <asp:Label ID="lblCorreoE" runat="server" Text="Email:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbCorreoE" runat="server" CssClass="textBoxForms" 
                                    MaxLength="50" Width="130px"></asp:TextBox>
                                <asp:regularexpressionvalidator id="revEmail" runat="server" ControlToValidate="txbCorreoE" 
                                ErrorMessage="Formato de correo no valido" ForeColor="Red" ValidationGroup="vgCliente"
                                ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">**</asp:regularexpressionvalidator>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>

                    <asp:TabPanel ID="tabContactosPersonales" runat="server" HeaderText="Contactos Personales">
                        <ContentTemplate>
                            <asp:Panel ID="pnlNuevoContacto" runat="server" GroupingText="Nuevo Contacto">
                                
                                <asp:Label ID="lblNombreContacto" runat="server" Text="Nombre:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbNombreContacto" runat="server" CssClass="textBoxForms" 
                                    MaxLength="75" Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNombreContacto" runat="server" 
                                    ControlToValidate="txbNombreContacto" 
                                    ErrorMessage="El contacto requiere un Nombre" ForeColor="Red" 
                                    ValidationGroup="vgContactos">*</asp:RequiredFieldValidator>
                                <asp:Label ID="lblApellidosContacto" runat="server" Text="Apellidos:" CssClass="labelForms" 
                                    Width="77px"></asp:Label>
                                <asp:TextBox ID="txbApellidosContacto" runat="server" CssClass="textBoxForms" 
                                    MaxLength="75" Width="130px"></asp:TextBox>                                
                                <asp:Label ID="lblTelefonoContacto" runat="server" Text="Teléfono:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbTelefonoContacto" runat="server" CssClass="textBoxForms" 
                                    MaxLength="15" Width="130px"></asp:TextBox>                                
                                <asp:RequiredFieldValidator ID="rfvTelefonoContacto" runat="server" 
                                    ErrorMessage="El contacto requiere un telefono" ForeColor="Red" 
                                    ValidationGroup="vgContactos" ControlToValidate="txbTelefonoContacto">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revTelefonoContacto" runat="server" 
                                    ErrorMessage="El telefono solo puede incluir números" ForeColor="Red" 
                                    ValidationGroup="vgContactos" ControlToValidate="txbTelefonoContacto" 
                                    ValidationExpression="\d+">*</asp:RegularExpressionValidator>
                                <br />
                                <asp:Label ID="lblCelularContacto" runat="server" Text="Celular:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbCelularConatcto" runat="server" CssClass="textBoxForms" 
                                    MaxLength="15" Width="130px"></asp:TextBox>                                
                                <asp:Label ID="lblCorreoEContacto" runat="server" Text="E-mail:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbCorreoEContacto" runat="server" CssClass="textBoxForms" 
                                    MaxLength="50" Width="130px"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:ImageButton ID="ibtnAgregar" runat="server" 
                                    ImageUrl="~/Icons/plus_16.png" ValidationGroup="vgContactos" onclick="imbAgregarContacto_Click" />
                                <br />
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" 
                                    ValidationGroup="vgContactos" />
                            </asp:Panel>
                            <asp:Panel ID="pnlContactos" runat="server" GroupingText="Contactos Actuales">
                                <asp:GridView ID="gdvContactosCliente" runat="server" AutoGenerateColumns="False" 
                                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                                    CellPadding="3" ForeColor="Black" GridLines="Vertical" 
                                    onselectedindexchanged="grvContactos_SelectedIndexChanged" Width="840px">
                                    <AlternatingRowStyle BackColor="#CCCCCC" />
                                    <Columns>
                                        <asp:BoundField DataField="idContacto" HeaderText="idContacto" 
                                            SortExpression="idContacto" Visible="False" />
                                        <asp:BoundField DataField="idCliente" HeaderText="idCliente" 
                                            SortExpression="idCliente" Visible="False" />
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" 
                                            SortExpression="Nombre" />
                                        <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" 
                                            SortExpression="Apellidos" />
                                        <asp:BoundField DataField="Telefono" HeaderText="Teléfono" 
                                            SortExpression="Telefono" />
                                        <asp:BoundField DataField="Celular" HeaderText="Celular" 
                                            SortExpression="Celular" />
                                        <asp:BoundField DataField="CorreoElectronico" HeaderText="Correo Electrónico" 
                                            SortExpression="CorreoElectronico" />
                                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" 
                                            SortExpression="Activo" />
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
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>

                    <asp:TabPanel ID="tabDatosOpcionales" runat="server" HeaderText="Datos Opcionales">
                        <ContentTemplate>
                            <asp:Panel ID="pnlaAlfanumericos" runat="server" GroupingText="Campos Alfanumericos">
                                <asp:Label ID="lblAlfanumerico1" runat="server" Text="Campo 1:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbAlfanumerico1" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25" Width="130px"></asp:TextBox>
                                <asp:Label ID="lblAlfanumerico2" runat="server" Text="Campo 2:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbAlfanumerico2" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25" Width="130px"></asp:TextBox>
                                <asp:Label ID="lblAlfanumerico3" runat="server" Text="Campo 3:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbAlfanumerico3" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25" Width="130px"></asp:TextBox>
                                <br />
                                <asp:Label ID="lblAlfanumerico4" runat="server" Text="Campo 4:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbAlfanumerico4" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25" Width="130px"></asp:TextBox>
                                <asp:Label ID="lblAlfanumerico5" runat="server" Text="Campo 5:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbAlfanumerico5" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25" Width="130px"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel ID="pnlEnteros" runat="server" GroupingText="Campos Enteros">
                                <asp:Label ID="lblEntero1" runat="server" Text="Campo 6:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbEntero1" runat="server" CssClass="textBoxForms" 
                                    Width="130px"></asp:TextBox>
                                <asp:Label ID="lblEntero2" runat="server" Text="Campo 7:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbEntero2" runat="server" CssClass="textBoxForms" 
                                    Width="130px"></asp:TextBox>
                                <asp:Label ID="lblEntero3" runat="server" Text="Campo 8:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbEntero3" runat="server" CssClass="textBoxForms" 
                                    Width="130px"></asp:TextBox>
                                <br />
                            </asp:Panel>

                            <asp:Panel ID="pnlDecimales" runat="server" GroupingText="Campos Decimales">
                                <asp:Label ID="lblDecimal1" runat="server" Text="Campo 9:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbDecimal1" runat="server" CssClass="textBoxForms" 
                                    Width="130px"></asp:TextBox>
                                <asp:Label ID="lblDecimal2" runat="server" Text="Campo 10:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbDecimal2" runat="server" CssClass="textBoxForms" 
                                    Width="130px"></asp:TextBox>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                </asp:TabContainer>
                <asp:ValidationSummary ID="vsCliente" runat="server" 
                    ValidationGroup="vgCliente" ForeColor="Red" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div style="margin-left:60px">
        <asp:Panel ID="pnlFiltroReportes" runat="server">
            <FiltroReportes:Filtro runat="server" ID="frReportes" />
        </asp:Panel>
    </div>
</asp:Content>
