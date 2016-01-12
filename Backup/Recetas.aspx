<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Recetas.aspx.cs" Inherits="Medicuri.Recetas" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="DetallePartida" />
    <div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Label id="lblAviso" runat="server" Text="" ForeColor="Red"></asp:Label>
                <br />
                <asp:Label id="lblAviso2" runat="server" Text="" ForeColor="Red"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
     <div id="divListado" runat="server" visible="false">
         <asp:Panel ID="pnlCatalogo" runat="server" Height="350px" ScrollBars="Auto">
            <asp:UpdatePanel ID="upnCatalogo" runat="server">
            <ContentTemplate>
                <asp:GridView ID="dgvListado" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#999999" 
                    BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" 
                    GridLines="Vertical" Width="840px" onrowcreated="dgvListado_RowCreated" 
                    
                    
                    DataKeyNames="idReceta,idEstadoExp,idMunicipioExp,idPoblacionExp,idColoniaExp,idEstadoSur,idMunicipioSur,idPoblacionSur,idColoniaSur" 
                    AllowPaging="True" AllowSorting="True" 
                    onpageindexchanging="dgvListado_PageIndexChanging" 
                    onsorting="dgvListado_Sorting" PageSize="100"  >
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                     <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" SelectText="-" ShowSelectButton="True" />
                        <asp:BoundField DataField="Folio" HeaderText="Folio" SortExpression="Folio"  />
                        <asp:BoundField DataField="Paciente" HeaderText="Paciente" 
                            SortExpression="Paciente"  />
                         <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                         <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha"  DataFormatString="{0:dd/MM/yyyy}" > </asp:BoundField>
                        <asp:BoundField DataField="EstatusMedico" HeaderText="Estatus Médico" > </asp:BoundField>
                        <asp:BoundField DataField="Estatus" HeaderText="Estatus Facturación" 
                            SortExpression="Estatus" />
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

    <div id="divFormulario" runat="server" visible="false">
        <asp:UpdatePanel ID="pnlFormulario" runat="server">
            <ContentTemplate>

            <asp:Panel ID="PanelDatos" runat="server" GroupingText="Datos" Height="130px" 
                    Width="845px">
                <asp:Label ID="Label2" runat="server" Text="Folio:" CssClass="labelForms" 
                    Width="90px"></asp:Label>
                <asp:TextBox ID="txbFolio" runat="server" Width="130px" ></asp:TextBox>
                <asp:Label ID="Label3" runat="server" Text="Fecha:" CssClass="labelForms" 
                    Width="90px"></asp:Label>
                <asp:TextBox ID="txbFecha" runat="server" Width="130px"></asp:TextBox>
               
                  
                <asp:CalendarExtender ID="txbFecha_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="txbFecha">
                </asp:CalendarExtender>
                
                  
                <asp:Label ID="Label24" runat="server" Text="Estatus:"></asp:Label>
                <asp:DropDownList ID="cmbEstatus" runat="server">
                <asp:ListItem Value="1">Surtida</asp:ListItem>
                <asp:ListItem Value="2">Parcial</asp:ListItem>
                 <asp:ListItem Value="3">Facturada</asp:ListItem>
                <asp:ListItem Value="4">Cancelada</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label5" runat="server" Text="Tipo"></asp:Label>
                <asp:DropDownList ID="cmbTipoReceta" runat="server">
                </asp:DropDownList>
                <br />
                <asp:Label ID="Label8" runat="server" Text="Numero de Afiliación:" 
                    CssClass="labelForms" Width="90px"></asp:Label>
                <asp:TextBox ID="txbNumeroSeguroSocial" runat="server" Width="130px" 
                    MaxLength="25" ontextchanged="txbNumeroSeguroSocial_TextChanged" 
                    AutoPostBack="True"></asp:TextBox>                
                <asp:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" 
                    DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                    ServiceMethod="RecuperarClave1Cliente" ServicePath="BusquedasAsincronas.asmx" 
                    TargetControlID="txbNumeroSeguroSocial" MinimumPrefixLength="1">
                </asp:AutoCompleteExtender>                
                <asp:Label ID="Label4" runat="server" Text="Nombre del Paciente:" CssClass="labelForms" 
                        Width="90px"></asp:Label>
                <asp:TextBox ID="txbCliente" runat="server" Width="250px" AutoPostBack="True" 
                    ontextchanged="txbCliente_TextChanged"></asp:TextBox>
                <asp:AutoCompleteExtender ID="AutoCompleteExtender2" runat="server"
                    DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                    ServiceMethod="RecuperarNombreCliente" ServicePath="BusquedasAsincronas.asmx" 
                    TargetControlID="txbCliente" MinimumPrefixLength="1">
                </asp:AutoCompleteExtender>
                <asp:ImageButton ID="imbAgregarCliente" runat="server" 
                    AlternateText="Nuevo Cliente" ImageUrl="~/Icons/plus_16.png" 
                    onclick="imbAgregarCliente_Click" />
                <br />
                     <asp:Label ID="Label6" runat="server" Text="Teléfono del Paciente:" CssClass="labelForms" 
                        Width="90px"></asp:Label>
                    <asp:TextBox ID="txbClienteTelefono" runat="server" Width="130px"></asp:TextBox>
                <br />
                <asp:Label ID="lblDatos" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
              </asp:Panel>
              
              
              <%-- Panel de los clientes nuevos  --%>
                <asp:Panel ID="pnlClientes" runat="server" Visible="False" 
                    GroupingText="Clientes" Height="350px" ScrollBars="Auto">
                    <div id="div1">
    <asp:UpdatePanel ID="upnForm" runat="server">
            <ContentTemplate>
                <asp:TabContainer ID="tbcClientesNuevos" runat="server" ActiveTabIndex="0" 
                    Width="827px">
                <%-- tabDatosClientes --%>
                    <asp:TabPanel runat="server" HeaderText="Datos de Cliente" ID="tabDatosCliente"><ContentTemplate>
                            <asp:Panel ID="pnlDatosGenerales" runat="server" GroupingText="Datos Generales">
                                <asp:Label ID="lblClave1" runat="server" Text="No. de Afiliación:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                <asp:TextBox ID="txbClave1" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25" Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClave" runat="server" 
                                    ErrorMessage="Campo requerido: Clave1" ForeColor="Red" 
                                    ValidationGroup="vgCliente" ControlToValidate="txbClave1">*</asp:RequiredFieldValidator>
                                <asp:Label ID="lblClave2" runat="server" Text="Clave 2:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                <asp:TextBox ID="txbClave2" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25" Width="130px"></asp:TextBox>
                                <asp:Label ID="lblClave3" runat="server" Text="Clave 3:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                <asp:TextBox ID="txbClave3" runat="server" CssClass="textBoxForms" 
                                    MaxLength="25" Height="22px" Width="130px"></asp:TextBox>
                                <br /><asp:Label ID="lblEstatus" runat="server" Text="Tipo:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                <asp:ComboBox ID="ddlTipo" runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Nombre" DataValueField="idTipo" 
                                    Width="110px" MaxLength="0" DropDownStyle="DropDownList"></asp:ComboBox>
                                <asp:Label ID="lblFechaAlta" runat="server" Text="Fecha Alta:" 
                                        CssClass="labelForms" Width="102px"></asp:Label>
                                <asp:TextBox ID="txbFechaAlta" runat="server" Width="130px" ReadOnly="True"></asp:TextBox><asp:CalendarExtender ID="Calendar" runat="server" TargetControlID="txbFechaAlta" 
                                        Enabled="False"></asp:CalendarExtender>
                                <asp:RequiredFieldValidator ID="rfvFechaAlta" runat="server" 
                                    ErrorMessage="Campo requerido: Fecha alta" ForeColor="Red" 
                                    ValidationGroup="vgCliente" ControlToValidate="txbFechaAlta">*</asp:RequiredFieldValidator>
                                <asp:regularexpressionvalidator id="revFechaAlta" runat="server" ControlToValidate="txbFechaAlta" 
                                ErrorMessage="Formato de fecha no valido" ForeColor="Red" ValidationGroup="vgCliente"
                                ValidationExpression="\w{2}\/\w{2}\/\w{4}">**</asp:regularexpressionvalidator>&#160;&#160;&#160;<asp:CheckBox ID="chkActivo" runat="server" Text="Activo" /></asp:Panel>
                            <asp:Panel ID="pnlComplementos" runat="server" GroupingText="Complementarios"><asp:Label ID="lblNombres" runat="server" Text="Nombre(s):" CssClass="labelForms" 
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
                                <asp:Label ID="lblEdad" runat="server" Text="Edad:" CssClass="labelForms" 
                                    Width="77px"></asp:Label><asp:TextBox ID="txbEdad" runat="server" CssClass="textBoxForms" Width="130px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revEdad" runat="server" 
                                    ErrorMessage="No es una edad valida" ForeColor="Red" 
                                    ValidationGroup="vgCliente" ControlToValidate="txbEdad" 
                                    ValidationExpression="\d{0,2}">*</asp:RegularExpressionValidator>
                                <br />
                                <asp:Label ID="lblRFC" runat="server" Text="RFC:" CssClass="labelForms" 
                                    Width="90px"></asp:Label><asp:TextBox ID="txbRFC" runat="server" CssClass="textBoxForms" MaxLength="13" 
                                    Width="130px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revRFC" runat="server" 
                                    ControlToValidate="txbRFC" ErrorMessage="El RFC debe contener 13 carácteres" 
                                    ValidationGroup="vgCliente" ValidationExpression="\w{13}"
                                    ForeColor="Red">*</asp:RegularExpressionValidator>
                                <asp:Label ID="lblCurp" runat="server" Text="CURP:" CssClass="labelForms" 
                                    Width="90px"></asp:Label><asp:TextBox ID="txbCurp" runat="server" CssClass="textBoxForms" MaxLength="18" 
                                    Width="130px"></asp:TextBox><asp:Label ID="lblSexo" runat="server" Text="Sexo:" CssClass="labelForms" 
                                    Width="90px"></asp:Label><asp:ComboBox ID="cmbSexo" runat="server" 
                                    Width="110px" MaxLength="0" DropDownStyle="DropDownList">
                                    <asp:ListItem Value="M">Masculino</asp:ListItem>
                                    <asp:ListItem Value="F">Femenino</asp:ListItem>
                                </asp:ComboBox><br /><asp:Label ID="lblTipoPersona" runat="server" Text="Tipo de Persona:" CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:ComboBox 
                                    ID="cmbTipoPersona" runat="server" MaxLength="0" 
                                    AutoPostBack="True" 
                                    onselectedindexchanged="cmbTipoPersona_SelectedIndexChanged" Width="110px" 
                                    DropDownStyle="DropDownList">
                                    <asp:ListItem Value="FISICA">Fisica</asp:ListItem>
                                    <asp:ListItem Value="MORAL">Moral</asp:ListItem></asp:ComboBox></asp:Panel></ContentTemplate></asp:TabPanel>

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
                                    MaxLength="0" DropDownStyle="DropDownList">
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
                                <asp:ValidationSummary ID="ValidationSummary2" runat="server" ForeColor="Red" 
                                    ValidationGroup="vgContactos" />
                            </asp:Panel>
                            <asp:Panel ID="pnlContactos" runat="server" GroupingText="Contactos Actuales">
                                <asp:GridView ID="gdvContactosCliente" runat="server" AutoGenerateColumns="False" 
                                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                                    CellPadding="3" ForeColor="Black" GridLines="Vertical" 
                                    onselectedindexchanged="grvContactos_SelectedIndexChanged" Width="800px">
                                    <AlternatingRowStyle BackColor="#CCCCCC" />
                                    <Columns>
                                        <asp:BoundField DataField="idContacto" HeaderText="idContacto" 
                                            SortExpression="idContacto" Visible="False" />
                                        <asp:BoundField DataField="idCliente" HeaderText="idCliente" 
                                            SortExpression="idCliente" Visible="False" />
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" 
                                            SortExpression="Nombre" >
                                        <ItemStyle Width="50px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" 
                                            SortExpression="Apellidos" >
                                        <ItemStyle Width="50px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Telefono" HeaderText="Teléfono" 
                                            SortExpression="Telefono" >
                                        <ItemStyle Width="50px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Celular" HeaderText="Celular" 
                                            SortExpression="Celular" >
                                        <ItemStyle Width="50px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CorreoElectronico" HeaderText="Correo Electrónico" 
                                            SortExpression="CorreoElectronico" >
                                        <ItemStyle Width="50px" />
                                        </asp:BoundField>
                                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" 
                                            SortExpression="Activo" >
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="25px" />
                                        </asp:CheckBoxField>
                                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/delete_16.png" 
                                            ShowSelectButton="True" >
                                        <ItemStyle Width="25px" />
                                        </asp:CommandField>
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
                
                <asp:Button ID="btnGuardarCliente" runat="server" Text="Guardar" 
                    onclick="btnGuardarCliente_Click" />
                <asp:Button ID="btnCancelarCliente" runat="server" Text="Cancelar" 
                    onclick="btnCancelarCliente_Click" />
                    <br />
                
                <asp:ValidationSummary ID="vsCliente" runat="server" 
                    ValidationGroup="vgCliente" ForeColor="Red" />
            </ContentTemplate>
        </asp:UpdatePanel>
       
    </div>
                </asp:Panel>
                 <asp:Label ID="lblResults" runat="server" ForeColor="Red"></asp:Label>
               <%-- Termina clientes nuevos --%>                 
                <asp:Panel ID="pnlDetalle" runat="server" GroupingText="Detalle">
                     <asp:Label ID="lAvisosProducto" runat="server" ForeColor="Black"></asp:Label>
                    <asp:Table ID="Table1" runat="server" Width="835px">
                        <%-- Fila de los encabezados --%>
                        <asp:TableRow>
                          
                          <asp:TableCell>
                            <asp:Label ID="Label12" runat="server" Width="110px" BorderStyle="Ridge" BorderColor="Black" >Clave</asp:Label>
                          </asp:TableCell>
                          <asp:TableCell>
                            <asp:Label ID="Label13" runat="server" Width="110px" BorderStyle="Ridge" BorderColor="Black">Producto</asp:Label>
                          </asp:TableCell>
                          <asp:TableCell>
                            <asp:Label ID="Label14" runat="server" Width="55px" BorderStyle="Ridge" BorderColor="Black">Recetada</asp:Label>
                          </asp:TableCell>
                            <asp:TableCell>
                            <asp:Label ID="Label17" runat="server" Width="45px" BorderStyle="Ridge" BorderColor="Black">Surtida</asp:Label>
                          </asp:TableCell>
                              <asp:TableCell>
                            <asp:Label ID="Label20" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">Intención</asp:Label>
                          </asp:TableCell>
                            <asp:TableCell>
                            <asp:Label ID="Label22" runat="server" Width="100px" BorderStyle="Ridge" BorderColor="Black">Causes</asp:Label>
                          </asp:TableCell>
                         <%-- <asp:TableCell>
                             <asp:Label ID="Label23" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">Precio</asp:Label>
                          </asp:TableCell>--%>
                          <asp:TableCell>
                            <asp:Label ID="Label99" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">Lote</asp:Label>
                          </asp:TableCell>
                          <asp:TableCell>
                            <asp:Label ID="Label98" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">Núm. Serie</asp:Label>
                          </asp:TableCell>
                          <asp:TableCell>
                              <asp:Label ID="Label7" runat="server" Width="50px" BorderStyle="Ridge" BorderColor="Black">Línea de Crédito</asp:Label>
                          </asp:TableCell>

                        </asp:TableRow>

                         <%-- Fila de los Campos de texto --%>
                        <asp:TableRow>
                          
                          <asp:TableCell>
                           <asp:TextBox ID="txbClave" runat="server"  Width="110px" 
                    ontextchanged="txbClave_TextChanged" AutoPostBack="True"></asp:TextBox> 
                 <asp:AutoCompleteExtender ID="txbClave_AutoCompleteExtender" runat="server" 
                    DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                    ServiceMethod="RecuperarClave1Producto" ServicePath="BusquedasAsincronas.asmx" 
                    TargetControlID="txbClave" MinimumPrefixLength="1">
                </asp:AutoCompleteExtender>
                          </asp:TableCell>
                          <asp:TableCell>
                           <asp:TextBox ID="txbProducto" runat="server"  Width="110px" AutoPostBack="True" ontextchanged="txbProducto_TextChanged"></asp:TextBox> 
                <asp:AutoCompleteExtender ID="txbProducto_AutoCompleteExtender" runat="server" 
                    DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                    ServiceMethod="RecuperarNombreProducto" ServicePath="BusquedasAsincronas.asmx" 
                    TargetControlID="txbProducto" MinimumPrefixLength="1">
                </asp:AutoCompleteExtender>
                          </asp:TableCell>
                          <asp:TableCell>
                            <asp:TextBox ID="txbCantRecetada" runat="server"  Width="55px" ValidationGroup="DetallePartida" Text="0" AutoPostBack="false"></asp:TextBox>
                           <%-- <asp:RegularExpressionValidator ID="revCantRecetada" runat="server" ErrorMessage="Cantidad Recetada: Solo números" ControlToValidate="txbCantRecetada" ValidationGroup="DetallePartida" Text="*" ValidationExpression="^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfvCantRecetada" runat="server" ErrorMessage="Cantidad Recetada: Escriba un número" ControlToValidate="txbCantRecetada" ValidationGroup="DetallePartida" Text="*"></asp:RequiredFieldValidator>--%>
                          </asp:TableCell>
                          <asp:TableCell>
                          <%--GT 14/10/2011 0578 Se quita el post back para que no limpie los campos de texto, quizas ya no funcione el validator--%> 
                            <asp:TextBox ID="txbCantSurtida" runat="server"  Width="45px" Text="0" AutoPostBack="false"></asp:TextBox>
                           <%-- <asp:RegularExpressionValidator ID="revCantSurtida" runat="server" ErrorMessage="Cantidad Surtida: Solo números" ControlToValidate="txbCantSurtida" ValidationGroup="DetallePartida" Text="*" ValidationExpression="^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfvCantSurtida" runat="server" ErrorMessage="Cantidad Surtida: Escriba un número" ControlToValidate="txbCantSurtida" ValidationGroup="DetallePartida" Text="*"></asp:RequiredFieldValidator>--%>
                          </asp:TableCell>
                         <asp:TableCell>
                               <asp:RadioButton id="rdbIntencionPrimera" runat="server"  GroupName="Intencion" Text="1"></asp:RadioButton><br />
                               <asp:RadioButton id="rdbIntencionSegunda" runat="server"  GroupName="Intencion" Text="2"></asp:RadioButton>
                          </asp:TableCell>
                         <asp:TableCell>
                              <asp:TextBox ID="txbClaveCie" runat="server" Width="100px" 
                                AutoPostBack="True" ontextchanged="txbClaveCie_TextChanged"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="AutoCompleteExtender3" runat="server" 
                                MinimumPrefixLength="1" ServiceMethod="RecuperarClaveCausesCIE" 
                                ServicePath="BusquedasAsincronas.asmx" TargetControlID="txbClaveCie">
                </asp:AutoCompleteExtender>
                              <%--<asp:RadioButton id="rdbCauseSi" runat="server"  GroupName="Causes" Text="Sí"></asp:RadioButton><br />
                               <asp:RadioButton id="rdbCauseNo" runat="server"  GroupName="Causes" Text="No"></asp:RadioButton>--%>
                          </asp:TableCell>
                          <%-- <asp:TableCell>
                           <asp:DropDownList ID="ddlPrecios" runat="server"></asp:DropDownList>
                          </asp:TableCell>--%> 
                          <asp:TableCell>
                            <asp:DropDownList ID="ddlProductoLotes" runat="server"></asp:DropDownList>
                          </asp:TableCell>
                          <asp:TableCell>
                           <asp:DropDownList ID="ddlProductoSeries" runat="server"></asp:DropDownList>
                          </asp:TableCell>
                          <asp:TableCell>
                              <asp:ComboBox ID="cmbLineasCredito" runat="server" Width="80px" DataValueField="idLineaCredito" DataTextField="Clave"></asp:ComboBox>
                          </asp:TableCell>
                          <asp:TableCell>
                           <asp:ImageButton ID="imbAgregarDetalle" runat="server" ImageUrl="~/Icons/plus_16.png" Height="16px" onclick="imbAgregarDetalle_Click" ValidationGroup="DetallePartida" />
                          </asp:TableCell>
                          
                        </asp:TableRow>
                         
                    </asp:Table>                    
                    <br />
                    <asp:Panel ID="Panel5" runat="server" ScrollBars="Auto" Height="75px">
                        <asp:GridView ID="dgvPartidaDetalle" runat="server" AutoGenerateColumns="False" 
                         OnSelectedIndexChanged="dgvPartidaDetalle_SelectedIndexChanged" 
                         BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                         CellPadding="3" ForeColor="Black" GridLines="Vertical" Width="835px" 
                         DataKeyNames="idRecetaPartida,idProducto,idLineaCredito" 
                         onrowcreated="dgvPartidaDetalle_RowCreated" >
                       <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                      <asp:BoundField HeaderText="Clave" DataField="idProducto" />
                      <asp:BoundField HeaderText="Producto"  DataField="idProducto"/>
                      <asp:BoundField HeaderText="Recetada"  DataField="CantidadRecetada"/>
                      <asp:BoundField HeaderText="Surtida"  DataField="CantidaSurtida"/>
                      <asp:BoundField HeaderText="Intención"  DataField="PrimeraIntencion"/>
                      <asp:BoundField HeaderText="Cause CIE"  DataField="Cause"/>
                      <%-- <asp:BoundField HeaderText="Precio"  DataField="precio"/>--%>
                      <asp:BoundField HeaderText="Lote"  DataField="Lote"/>
                      <asp:BoundField HeaderText="Num. Serie"  DataField="NoSerie"/>
                      <%-- <asp:BoundField HeaderText="Total"  DataField="totalPrecio"/>--%>
                        <asp:BoundField DataField="idLineaCredito" HeaderText="Linea de Credito" />
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
                </asp:Panel>
             
                 <asp:Panel id="pPie" runat="server" >
                    <asp:Label ID="lAvisosPie" runat="server" ForeColor="Black"></asp:Label>
                      <asp:Panel id="pMedico" runat="server" GroupingText="Médico">
                        <asp:Label ID="Label97" runat="server" Text="Cédula Prof." CssClass="labelForms" Width="100px"></asp:Label>
                        <asp:TextBox ID="txbPieCedulaProf" runat="server" CssClass="textBoxForms" 
                              AutoPostBack="True" ontextchanged="txbPieCedulaProf_TextChanged"></asp:TextBox>
                          <asp:AutoCompleteExtender ID="txbPieCedulaProf_AutoCompleteExtender" 
                              runat="server" DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx" ServiceMethod="RecuperarCedulaVendedores" 
                    MinimumPrefixLength="1" CompletionSetCount="15"
                              TargetControlID="txbPieCedulaProf">
                          </asp:AutoCompleteExtender>
                        <asp:Label ID="Label96" runat="server" Text="Nombre" CssClass="labelForms" Width="100px"></asp:Label>
                        <asp:TextBox ID="txbPieMedico" runat="server" CssClass="textBoxForms" 
                              AutoPostBack="True" ontextchanged="txbPieMedico_TextChanged" ></asp:TextBox>
                          <asp:AutoCompleteExtender ID="txbPieMedico_AutoCompleteExtender" runat="server" 
                              DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx" ServiceMethod="RecuperarNombreVendedores"
                              TargetControlID="txbPieMedico" MinimumPrefixLength="1" CompletionSetCount="15">
                          </asp:AutoCompleteExtender>
                        
                        <asp:Label ID="Label95" runat="server" Text="Título expedido por" CssClass="labelForms" Width="100px"></asp:Label>
                        <asp:TextBox ID="txbPieTituloExp" runat="server" CssClass="textBoxForms" 
                              ReadOnly="True" ></asp:TextBox>
                         
                        <asp:Label ID="Label94" runat="server" Text="Reg. Especialidad" CssClass="labelForms" Width="100px"></asp:Label>
                        <asp:TextBox ID="txbPieRegEspecialidad" runat="server" CssClass="textBoxForms" 
                              ReadOnly="True" ></asp:TextBox>
                         <asp:ImageButton ID="imbNuevoMedico" runat="server" 
                              ImageUrl="~/Icons/plus_16.png" onclick="imbNuevoMedico_Click" />
                      </asp:Panel>

                      <%-- Panel de vendedores nuevos --%>
                     <asp:Panel ID="pnlVendedoresNuevos" runat="server" GroupingText="Medicos" 
                         ScrollBars="Auto" Visible="False">
                       <div id="div2">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:TabContainer ID="tbcVendedores" runat="server" ActiveTabIndex="0" 
                    Width="845px">
                <%-- tabDatosVendedor --%>
                    <asp:TabPanel runat="server" HeaderText="Datos de Vendedor" ID="tabDatosVendedor">
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server" GroupingText="Datos Generales">
                            <asp:Label ID="lblClave" runat="server" Text="Clave:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>  
                            <asp:TextBox ID="txbClaveVendedor" runat="server" CssClass="textBoxForms" Enabled="False" 
                                    MaxLength="15" Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClaveVendedor" runat="server" 
                                    ControlToValidate="txbClaveVendedor" ErrorMessage="Clave: Campo Requerido" 
                                    ValidationGroup="vendedorVG" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:Label ID="Label9" runat="server" Text="Nombre(s):" CssClass="labelForms" 
                                    Width="77px"></asp:Label>  
                            <asp:TextBox ID="txbNombreVendedor" runat="server" CssClass="textBoxForms" Enabled="False" 
                                    MaxLength="75" Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNombresVendedor" runat="server" 
                                    ControlToValidate="txbNombreVendedor" ErrorMessage="Nombres: Campo Requerido" 
                                    ValidationGroup="vendedorVG" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:Label ID="Label10" runat="server" Text="Apellido(s):" CssClass="labelForms" 
                                    Width="77px"></asp:Label> 
                            <asp:TextBox ID="txbApellidoVendedor" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="75" Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvApellidoVendedor" runat="server" 
                                    ControlToValidate="txbApellidoVendedor" ErrorMessage="Apellidos: Campo Requerido" 
                                    ValidationGroup="vendedorVG" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <br />
                            <asp:Label ID="Label11" runat="server" Text="Tipo:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                            <asp:ComboBox ID="cmbTipoVendedor" runat="server" CssClass="textBoxForms" 
                                    DataTextField="Nombre" DataValueField="idTipo" 
                                    Width="110px" MaxLength="0" DropDownStyle="DropDownList">                            
                            </asp:ComboBox>                                
                            <asp:Label ID="Label15" runat="server" Text="Fecha Alta:" 
                                    CssClass="labelForms" Width="102px"></asp:Label>
                            <asp:TextBox ID="txbFechaVendedor" runat="server" Enabled="False" 
                                    CssClass="textBoxForms" Width="130px" ReadOnly="True"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFechaVendedor" runat="server" 
                                    ControlToValidate="txbFechaVendedor" ErrorMessage="Fecha alta: : Campo Requerido" 
                                    ValidationGroup="vendedorVG" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:regularexpressionvalidator id="rxvFechaVendedor" runat="server" ControlToValidate="txbFechaVendedor" 
                                ErrorMessage="Formato de fecha no valido" ForeColor="Red" ValidationGroup="vendedorVG"
                                ValidationExpression="\w{2}\/\w{2}\/\w{4}">**</asp:regularexpressionvalidator>
                            
                                <asp:CheckBox ID="chkActivoVendedor" runat="server" Text="Activo" 
                                    Enabled="False" />
                            <br />
                            <asp:Label ID="Label16" runat="server" Text="Tipo de Persona:" CssClass="labelForms" Width="90px"></asp:Label>
                            <asp:ComboBox ID="cmbTipoPersonaVendedor" runat="server" MaxLength="0" 
                                AutoPostBack="True" 
                                onselectedindexchanged="cmbTipoPersona_SelectedIndexChanged" Width="110px" 
                                    DropDownStyle="DropDownList">
                                <asp:ListItem Value="FISICA">Fisica</asp:ListItem>
                                <asp:ListItem Value="MORAL">Moral</asp:ListItem>
                            </asp:ComboBox>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>

                    <asp:TabPanel runat="server" HeaderText="Datos de Contacto" ID="TabPanel1">
                        <ContentTemplate>
                            <asp:Panel ID="Panel2" runat="server" GroupingText="Dirección">
                                <asp:Label ID="Label18" runat="server" Text="Calle:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbCalleVendedor" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="75" Width="130px"></asp:TextBox>
                                <asp:Label ID="Label19" runat="server" Text="No. Exterior:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>  
                                <asp:TextBox ID="txbNoExtVendedor" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="5" Width="130px"></asp:TextBox>
                                <asp:Label ID="Label21" runat="server" Text="No. Interior:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>  
                                <asp:TextBox ID="txbNoIntVendedor" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="5" Width="130px"></asp:TextBox>
                                <br />

                                <asp:Label ID="Label23" runat="server" Text="País:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbPaisVendedor" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" ReadOnly="True" Width="130px">México</asp:TextBox>
                                <asp:Label ID="Label25" runat="server" Text="Estado:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:ComboBox ID="cmbEstadoVendedor" runat="server" AutoPostBack="True" 
                                    DataTextField="Nombre" DataValueField="idEstado"
                                    onselectedindexchanged="cmbEstadoVendedor_SelectedIndexChanged" 
                                    Width="110px" CssClass="textBoxForms" MaxLength="0">
                                </asp:ComboBox>                                 
                                <asp:Label ID="Label26" runat="server" Text="Municipio:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:ComboBox ID="cmbMunicipioVendedores" runat="server" AutoPostBack="True" DataTextField="Nombre" 
                                    DataValueField="idMunicipio" 
                                    onselectedindexchanged="cmbMunicipioVendedores_SelectedIndexChanged" 
                                    Width="110px" CssClass="textBoxForms" MaxLength="0">
                                </asp:ComboBox>                                 
                                <br />

                                <asp:Label ID="lblPoblacion" runat="server" Text="Población:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:ComboBox ID="cmbPoblacionVendedores" runat="server" AutoPostBack="True" DataTextField="Nombre" 
                                    DataValueField="idPoblacion" 
                                    onselectedindexchanged="cmbPoblacionVendedores_SelectedIndexChanged" 
                                    Width="110px" CssClass="textBoxForms" MaxLength="0" 
                                    DropDownStyle="DropDownList">
                                </asp:ComboBox>                                
                                <asp:Label ID="Label27" runat="server" Text="Colonia:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:ComboBox ID="cmbColoniaVendedores" runat="server" DataTextField="Nombre" DataValueField="idColonia" 
                                    Width="110px" CssClass="textBoxForms" MaxLength="0">
                                </asp:ComboBox>                                 
                                <asp:Label ID="Label28" runat="server" Text="Codigo Postal:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbCodigoPostalVendedores" runat="server" 
                                    CssClass="textBoxForms" Enabled="False" 
                                    MaxLength="5" Width="130px"></asp:TextBox>
                            </asp:Panel><br />
                            <asp:Panel ID="Panel4" runat="server" GroupingText="Contacto">
                                <asp:Label ID="Label29" runat="server" Text="Teléfono:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbTelefonoVendedores" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" Width="130px"></asp:TextBox>
                                <asp:Label ID="Label30" runat="server" Text="Celular:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbCelularVendedores" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" Width="130px"></asp:TextBox>
                                <asp:Label ID="lblFax" runat="server" Text="Fax:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbFaxVendedores" runat="server" CssClass="textBoxForms" Enabled="False" 
                                    Width="130px"></asp:TextBox>
                                <br />
                                <asp:Label ID="Label31" runat="server" Text="Email:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbEmailVendedores" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" Width="130px"></asp:TextBox>
                                <asp:regularexpressionvalidator id="rxvCorreoVendedor" runat="server" ControlToValidate="txbEmailVendedores" 
                                ErrorMessage="Formato de correo no valido" ForeColor="Red" ValidationGroup="vendedorVG"
                                ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">**</asp:regularexpressionvalidator>

                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>

                    <asp:TabPanel ID="tabDatosProfesionales" runat="server" HeaderText="Datos Profesionales">
                        <ContentTemplate>
                            <asp:Panel ID="pnlDatosProfesionales" runat="server" GroupingText="Información Profesional">
                                <asp:Label ID="Label32" runat="server" Text="RFC:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbRfcVendedores" runat="server" CssClass="textBoxForms" Enabled="False" 
                                    MaxLength="13"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                    ControlToValidate="txbRfcVendedores" ErrorMessage="El RFC debe contener 13 carácteres" 
                                    ValidationGroup="vendedorVG" ValidationExpression="\w{13}"
                                    ForeColor="Red">*</asp:RegularExpressionValidator>
                                <asp:Label ID="Label33" runat="server" Text="CURP:" CssClass="labelForms" 
                                    Width="77px"></asp:Label>
                                <asp:TextBox ID="txbCurpVendedores" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="18"></asp:TextBox>                                
                                <asp:Label ID="lblCedulaProfesional" runat="server" Text="Cédula Profesional:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbCedulaProfesionalVendedores" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="25"></asp:TextBox>
                                <br />
                                <asp:Label ID="lblTitulo" runat="server" Text="Título:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbTituloVendedores" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="50"></asp:TextBox>                                
                                <asp:Label ID="lblEspecialidad" runat="server" Text="Especialidad:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbEspecialidadVendedores" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" AutoPostBack="True"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="txbEspecialidadVendedores_AutoCompleteExtender" runat="server" 
                                    DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                                    ServiceMethod="RecuperarVendedoresEspecialidad" ServicePath="BusquedasAsincronas.asmx" 
                                    TargetControlID="txbEspecialidadVendedores" MinimumPrefixLength="1">
                                </asp:AutoCompleteExtender>
                                <asp:Label ID="lblVinculacion" runat="server" Text="Vinculación:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbVinculacionVendedores" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" AutoPostBack="True"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="txbVinculacionVendedores_AutoCompleteExtender" runat="server" 
                                    DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                                    ServiceMethod="RecuperarVendedoresVinculacion" ServicePath="BusquedasAsincronas.asmx" 
                                    TargetControlID="txbVinculacionVendedores" MinimumPrefixLength="1">
                                </asp:AutoCompleteExtender>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>

                    <asp:TabPanel ID="tabDatosOpcionalesVendedor" runat="server" HeaderText="Datos Opcionales">
                        <ContentTemplate><asp:Panel ID="pnlaAlfanumericosV" runat="server" GroupingText="Campos Alfanumericos">
                            <asp:Label ID="lblAlfanumerico1V" runat="server" Text="Campo 1:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                            <asp:TextBox ID="txbAlfanumerico1V" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="25"></asp:TextBox>
                            <asp:Label ID="lblAlfanumerico2V" runat="server" Text="Campo 2:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                            <asp:TextBox ID="txbAlfanumerico2V" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="25"></asp:TextBox>
                            <asp:Label ID="lblAlfanumerico3V" runat="server" Text="Campo 3:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                            <asp:TextBox ID="txbAlfanumerico3V" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="25"></asp:TextBox>
                            <br />
                            <asp:Label ID="lblAlfanumerico4V" runat="server" Text="Campo 4:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                            <asp:TextBox ID="txbAlfanumerico4V" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="25"></asp:TextBox>
                            <asp:Label ID="lblAlfanumerico5V" runat="server" Text="Campo 5:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                            <asp:TextBox ID="txbAlfanumerico5V" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="25"></asp:TextBox></asp:Panel>
                            <asp:Panel ID="pnlEnterosV" runat="server" GroupingText="Campos Enteros"><asp:Label ID="lblEntero1V" runat="server" Text="Campo 6:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbEntero1V" runat="server" CssClass="textBoxForms" 
                                    Enabled="False"></asp:TextBox>
                                <asp:Label ID="lblEntero2V" runat="server" Text="Campo 7:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbEntero2V" runat="server" CssClass="textBoxForms" 
                                    Enabled="False"></asp:TextBox>
                                <asp:Label ID="lblEntero3V" runat="server" Text="Campo 8:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbEntero3V" runat="server" CssClass="textBoxForms" 
                                    Enabled="False"></asp:TextBox>
                                <br /></asp:Panel><asp:Panel ID="pnlDecimalesV" runat="server" GroupingText="Campos Decimales">
                                <asp:Label ID="lblDecimal1V" runat="server" Text="Campo 9:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbDecimal1V" runat="server" CssClass="textBoxForms" 
                                    Enabled="False"></asp:TextBox>
                                <asp:Label ID="lblDecimal2V" runat="server" Text="Campo 10:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbDecimal2V" runat="server" CssClass="textBoxForms" 
                                    Enabled="False"></asp:TextBox></asp:Panel></ContentTemplate></asp:TabPanel>
                </asp:TabContainer>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:ValidationSummary ID="vlsGrupoValidaciones" runat="server" 
        ValidationGroup="vendedorVG" ForeColor="Red" />
                         <asp:Button ID="btnGuardarMedico" runat="server" Text="Guardar" 
                             onclick="btnGuardarMedico_Click" />
                         <asp:Button ID="btnCancelarMedico" runat="server" Text="Cancelar" 
                             onclick="btnCancelarMedico_Click" />
                     </asp:Panel>
                     <asp:Label ID="lblAvisosVendedores" runat="server" Font-Bold="True" 
                         ForeColor="Red"></asp:Label>
                      <%-- Termina Panel de vendedores nuevos --%>

                      <asp:Panel id="pSurtidoEn" runat="server" GroupingText="Surtido en">
                      <asp:Label ID="Label93" runat="server" Text="Estado" CssClass="labelForms" 
                              Width="70px"></asp:Label>
                            <asp:ComboBox ID="cmbSurtidoEnEstados" runat="server" AutoPostBack="True" 
                              DataTextField="Nombre" DataValueField="idEstado" 
                              onselectedindexchanged="cmbSurtidoEnEstados_SelectedIndexChanged" 
                              Width="90px" DropDownStyle="DropDownList">
                            </asp:ComboBox>                            
                            <asp:Label ID="Label92" runat="server" Text="Del./Mcpio." 
                              CssClass="labelForms" Width="70px"></asp:Label>
                            <asp:ComboBox ID="cmbSurtidoEnMunicipios" runat="server" AutoPostBack="True" 
                              DataTextField="Nombre" DataValueField="idMunicipio" 
                              onselectedindexchanged="cmbSurtidoEnMunicipios_SelectedIndexChanged" 
                              Width="90px" DropDownStyle="DropDownList">
                            </asp:ComboBox>
                                <asp:Label ID="Label91" runat="server" Text="Población" 
                              CssClass="labelForms" Width="70px"></asp:Label>
                            <asp:ComboBox ID="cmbSurtidoEnPoblaciones" runat="server" AutoPostBack="True" 
                              DataTextField="Nombre" DataValueField="idPoblacion" 
                              onselectedindexchanged="cmbSurtidoEnPoblaciones_SelectedIndexChanged" 
                              Width="90px" DropDownStyle="DropDownList">
                            </asp:ComboBox>
                              <asp:Label ID="Label90" runat="server" Text="Colonia" CssClass="labelForms" 
                              Width="70px"></asp:Label>
                            <asp:ComboBox ID="cmbSurtidoEnColonias" runat="server" AutoPostBack="True" 
                              DataTextField="Nombre" DataValueField="idColonia" Width="90px" 
                              DropDownStyle="DropDownList">
                            </asp:ComboBox>
                            <br />
                            <br />
                           
                      </asp:Panel>
                       <asp:Panel id="pExpedidoEn" runat="server" GroupingText="Expedido en">
                      <asp:Label ID="Label89" runat="server" Text="Estado" CssClass="labelForms" 
                               Width="70px"></asp:Label>
                            <asp:ComboBox ID="cmbExpedidoEnEstados" runat="server" AutoPostBack="True" 
                               DataTextField="Nombre" DataValueField="idEstado" 
                               onselectedindexchanged="cmbExpedidoEnEstados_SelectedIndexChanged" 
                               Width="90px" DropDownStyle="DropDownList">
                           </asp:ComboBox>
                            <asp:Label ID="Label88" runat="server" Text="Del./Mcpio." 
                               CssClass="labelForms" Width="70px"></asp:Label>
                                <asp:ComboBox ID="cmbExpedidoEnMunicipios" runat="server" 
                               AutoPostBack="True" DataTextField="Nombre" DataValueField="idMunicipio" 
                               onselectedindexchanged="cmbExpedidoEnMunicipios_SelectedIndexChanged" 
                               Width="90px" DropDownStyle="DropDownList">
                           </asp:ComboBox>
                                <asp:Label ID="Label87" runat="server" Text="Población" 
                               CssClass="labelForms" Width="70px"></asp:Label>
                              <asp:ComboBox ID="cmbExpedidoEnPoblaciones" runat="server" 
                               AutoPostBack="True" DataTextField="Nombre" DataValueField="idPoblacion" 
                               onselectedindexchanged="cmbExpedidoEnPoblaciones_SelectedIndexChanged" 
                               Width="90px" DropDownStyle="DropDownList">
                           </asp:ComboBox>
                              <asp:Label ID="Label86" runat="server" Text="Colonia" CssClass="labelForms" 
                               Width="70px"></asp:Label>
                            <asp:ComboBox ID="cmbExpedidoEnColonias" runat="server" AutoPostBack="True" 
                               DataTextField="Nombre" DataValueField="idColonia" Width="90px" 
                               DropDownStyle="DropDownList">
                           </asp:ComboBox>
                            <br />
                            <br />
                           
                      </asp:Panel>
                       <asp:Panel id="pFarmacia" runat="server" GroupingText="Farmacia">
                           <asp:RadioButton id="rdbFarmaciaPropia" runat="server"  GroupName="Farmacia" 
                               Text="Propia" Checked="True"></asp:RadioButton>
                           <asp:RadioButton id="rdbFarmaciaPrivada" runat="server"  GroupName="Farmacia" Text="Privada"></asp:RadioButton> 
                       </asp:Panel>
                 </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>        
</asp:Content>

