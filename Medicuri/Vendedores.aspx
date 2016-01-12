<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Vendedores.aspx.cs" Inherits="Medicuri.Vendedores" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register TagPrefix="FiltroReportes" TagName="Filtro" Src="~/FiltroReportes.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <div><asp:Label ID="lblResults" runat="server" Font-Bold="True"></asp:Label></div>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>

    <br />
    <div id="divCatalogo">
        <asp:Panel ID="pnlList" runat="server" Height="350px" ScrollBars="Auto">
            <asp:UpdatePanel ID="upnList" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gdvDatos" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                    CellPadding="3" ForeColor="Black" GridLines="Vertical" Width="845px" 
                    DataKeyNames="idVendedor,idEstado,idMunicipio,idPoblacion,idColonia" 
                    AllowPaging="True" AllowSorting="True" 
                    onpageindexchanging="gdvDatos_PageIndexChanging" onsorting="gdvDatos_Sorting" 
                    PageSize="100">
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                            SelectText="" ShowSelectButton="True" />
                        <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" 
                            SortExpression="Nombre" />
                        <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" 
                            SortExpression="Apellidos" />
                        <asp:BoundField DataField="TipoPersona" HeaderText="Tipo de Persona" />
                        <asp:BoundField DataField="Rfc" HeaderText="RFC" SortExpression="Rfc" />
                        <asp:BoundField DataField="Telefono" HeaderText="Telefono" 
                            SortExpression="Telefono" />
                        <asp:BoundField DataField="CorreoElectronico" HeaderText="CorreoElectronico" 
                            SortExpression="CorreoElectronico" Visible="False" />
                        <asp:BoundField DataField="TipoVendedor" HeaderText="Tipo de Vendedor" 
                            SortExpression="TipoVendedor" />
                        <asp:BoundField DataField="FechaAlta" HeaderText="Fecha de Alta" 
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
                <%-- tabDatosVendedor --%>
                    <asp:TabPanel runat="server" HeaderText="Datos de Vendedor" ID="tabDatosVendedor">
                        <ContentTemplate>
                            <asp:Panel ID="pnlDatosGenerales" runat="server" GroupingText="Datos Generales">
                            <asp:Label ID="lblClave" runat="server" Text="Clave:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>  
                            <asp:TextBox ID="txbClave" runat="server" CssClass="textBoxForms" Enabled="False" 
                                    MaxLength="15" Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClave" runat="server" 
                                    ControlToValidate="txbClave" ErrorMessage="Clave: Campo Requerido" 
                                    ValidationGroup="vendedorVG" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cmvClave" runat="server" ErrorMessage="Clave repetida"
                                    ControlToValidate="txbClave" ValidationGroup="vendedorVG" ForeColor="Red" OnServerValidate="cmvClave_ServerValidate">*</asp:CustomValidator>
                            <asp:Label ID="lblNombres" runat="server" Text="Nombre(s):" CssClass="labelForms" 
                                    Width="77px"></asp:Label>  
                            <asp:TextBox ID="txbNombres" runat="server" CssClass="textBoxForms" Enabled="False" 
                                    MaxLength="75" Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNombres" runat="server" 
                                    ControlToValidate="txbNombres" ErrorMessage="Nombres: Campo Requerido" 
                                    ValidationGroup="vendedorVG" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:Label ID="lblApellidos" runat="server" Text="Apellido(s):" CssClass="labelForms" 
                                    Width="77px"></asp:Label> 
                            <asp:TextBox ID="txbApellidos" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="75" Width="130px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvApellidos" runat="server" 
                                    ControlToValidate="txbApellidos" ErrorMessage="Apellidos: Campo Requerido" 
                                    ValidationGroup="vendedorVG" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <br />
                            <asp:Label ID="lblEstatus" runat="server" Text="Tipo:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                            <asp:ComboBox ID="cmbTipo" runat="server" CssClass="textBoxForms" 
                                    DataTextField="Nombre" DataValueField="idTipo" 
                                    Width="110px" MaxLength="0" DropDownStyle="DropDownList">                            
                            </asp:ComboBox>                                
                            <asp:Label ID="lblFechaAlta" runat="server" Text="Fecha Alta:" 
                                    CssClass="labelForms" Width="102px"></asp:Label>
                            <asp:TextBox ID="txbFechaAlta" runat="server" Enabled="False" 
                                    CssClass="textBoxForms" Width="130px" ReadOnly="True"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvFechaAlta" runat="server" 
                                    ControlToValidate="txbFechaAlta" ErrorMessage="Fecha alta: : Campo Requerido" 
                                    ValidationGroup="vendedorVG" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:regularexpressionvalidator id="revFechaAlta" runat="server" ControlToValidate="txbFechaAlta" 
                                ErrorMessage="Formato de fecha no valido" ForeColor="Red" ValidationGroup="vendedorVG"
                                ValidationExpression="\w{2}\/\w{2}\/\w{4}">**</asp:regularexpressionvalidator>
                            <asp:CalendarExtender ID="Calendar" runat="server" TargetControlID="txbFechaAlta" 
                                    Enabled="False"></asp:CalendarExtender>
                                <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" Enabled="False" />
                            <br />
                            <asp:Label ID="lblTipoPersona" runat="server" Text="Tipo de Persona:" CssClass="labelForms" Width="90px"></asp:Label>
                            <asp:ComboBox ID="cmbTipoPersona" runat="server" MaxLength="0" 
                                AutoPostBack="True" 
                                onselectedindexchanged="cmbTipoPersona_SelectedIndexChanged" Width="110px" 
                                    DropDownStyle="DropDownList">
                                <asp:ListItem Value="FISICA">Fisica</asp:ListItem>
                                <asp:ListItem Value="MORAL">Moral</asp:ListItem>
                            </asp:ComboBox>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>

                    <asp:TabPanel runat="server" HeaderText="Datos de Contacto" ID="tabDatosContacto">
                        <ContentTemplate>
                            <asp:Panel ID="pnlDireccion" runat="server" GroupingText="Dirección">
                                <asp:Label ID="lblCalle" runat="server" Text="Calle:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbCalle" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="75" Width="130px"></asp:TextBox>
                                <asp:Label ID="lblNumeroExterior" runat="server" Text="No. Exterior:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>  
                                <asp:TextBox ID="txbNumeroExterior" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="5" Width="130px"></asp:TextBox>
                                <asp:Label ID="lblNumeroInterior" runat="server" Text="No. Interior:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>  
                                <asp:TextBox ID="txbNumeroInterior" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="5" Width="130px"></asp:TextBox>
                                <br />

                                <asp:Label ID="lblPais" runat="server" Text="País:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbPais" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" ReadOnly="True" Width="130px">México</asp:TextBox>
                                <asp:Label ID="lblEstado" runat="server" Text="Estado:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:ComboBox ID="cmbEstados" runat="server" AutoPostBack="True" 
                                    DataTextField="Nombre" DataValueField="idEstado"
                                    onselectedindexchanged="cmbEstadoFormulario_SelectedIndexChanged" 
                                    Width="110px" CssClass="textBoxForms" MaxLength="0" 
                                    DropDownStyle="DropDownList">
                                </asp:ComboBox>                                 
                                <asp:Label ID="lblMunicipio" runat="server" Text="Municipio:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:ComboBox ID="cmbMunicipios" runat="server" AutoPostBack="True" DataTextField="Nombre" 
                                    DataValueField="idMunicipio" 
                                    onselectedindexchanged="cmbMunicipioFormulario_SelectedIndexChanged" 
                                    Width="110px" CssClass="textBoxForms" MaxLength="0" 
                                    DropDownStyle="DropDownList">
                                </asp:ComboBox>                                 
                                <br />

                                <asp:Label ID="lblPoblacion" runat="server" Text="Población:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:ComboBox ID="cmbPoblaciones" runat="server" AutoPostBack="True" DataTextField="Nombre" 
                                    DataValueField="idPoblacion" 
                                    onselectedindexchanged="cmbPoblacionFormulario_SelectedIndexChanged" 
                                    Width="110px" CssClass="textBoxForms" MaxLength="0" 
                                    DropDownStyle="DropDownList">
                                </asp:ComboBox>                                
                                <asp:Label ID="lblColonia" runat="server" Text="Colonia:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:ComboBox ID="cmbColonias" runat="server" DataTextField="Nombre" DataValueField="idColonia" 
                                    Width="110px" CssClass="textBoxForms" MaxLength="0" 
                                    DropDownStyle="DropDownList">
                                </asp:ComboBox>                                 
                                <asp:Label ID="lblCodigoPostal" runat="server" Text="Codigo Postal:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbCP" runat="server" CssClass="textBoxForms" Enabled="False" 
                                    MaxLength="5" Width="130px"></asp:TextBox>
                            </asp:Panel><br />
                            <asp:Panel ID="pnlContacto" runat="server" GroupingText="Contacto">
                                <asp:Label ID="lblTelefono" runat="server" Text="Teléfono:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbTelefono" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" Width="130px"></asp:TextBox>
                                <asp:Label ID="lblCelular" runat="server" Text="Celular:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbCelular" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" Width="130px"></asp:TextBox>
                                <asp:Label ID="lblFax" runat="server" Text="Fax:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbFax" runat="server" CssClass="textBoxForms" Enabled="False" 
                                    Width="130px"></asp:TextBox>
                                <br />
                                <asp:Label ID="lblCorreoE" runat="server" Text="Email:" CssClass="labelForms" 
                                    Width="90px"></asp:Label> 
                                <asp:TextBox ID="txbCorreoE" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" Width="130px"></asp:TextBox>
                                <asp:regularexpressionvalidator id="revEmail" runat="server" ControlToValidate="txbCorreoE" 
                                ErrorMessage="Formato de correo no valido" ForeColor="Red" ValidationGroup="vendedorVG"
                                ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">**</asp:regularexpressionvalidator>

                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>

                    <asp:TabPanel ID="tabDatosProfesionales" runat="server" HeaderText="Datos Profesionales">
                        <ContentTemplate>
                            <asp:Panel ID="pnlDatosProfesionales" runat="server" GroupingText="Información Profesional">
                                <asp:Label ID="lblRFC" runat="server" Text="RFC:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbRFC" runat="server" CssClass="textBoxForms" Enabled="False" 
                                    MaxLength="13"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revRFC" runat="server" 
                                    ControlToValidate="txbRFC" ErrorMessage="RFC Incorrecto" 
                                    ValidationGroup="vendedorVG" ValidationExpression="\w{10,13}"
                                    ForeColor="Red">*</asp:RegularExpressionValidator>
                                <asp:Label ID="lblCurp" runat="server" Text="CURP:" CssClass="labelForms" 
                                    Width="77px"></asp:Label>
                                <asp:TextBox ID="txbCurp" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="18"></asp:TextBox>                                
                                <asp:Label ID="lblCedulaProfesional" runat="server" Text="Cédula Profesional:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbCedulaProfesional" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="25"></asp:TextBox>
                                <br />
                                <asp:Label ID="lblTitulo" runat="server" Text="Título:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbTitulo" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="50"></asp:TextBox>                                
                                <asp:Label ID="lblEspecialidad" runat="server" Text="Especialidad:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbEspecialidad" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" AutoPostBack="True"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="txbEspecialidad_AutoCompleteExtender" runat="server" 
                                    DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                                    ServiceMethod="RecuperarVendedoresEspecialidad" ServicePath="BusquedasAsincronas.asmx" 
                                    TargetControlID="txbEspecialidad" MinimumPrefixLength="1">
                                </asp:AutoCompleteExtender>
                                <asp:Label ID="lblVinculacion" runat="server" Text="Vinculación:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbVinculacion" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" AutoPostBack="True"></asp:TextBox>
                                <asp:AutoCompleteExtender ID="txbVinculacion_AutoCompleteExtender" runat="server" 
                                    DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                                    ServiceMethod="RecuperarVendedoresVinculacion" ServicePath="BusquedasAsincronas.asmx" 
                                    TargetControlID="txbVinculacion" MinimumPrefixLength="1">
                                </asp:AutoCompleteExtender>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>

                    <asp:TabPanel ID="tabDatosOpcionales" runat="server" HeaderText="Datos Opcionales">
                        <ContentTemplate>
                            <asp:Panel ID="pnlaAlfanumericos" runat="server" GroupingText="Campos Alfanumericos">
                                <asp:Label ID="lblAlfanumerico1" runat="server" Text="Campo 1:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbAlfanumerico1" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="25"></asp:TextBox>
                                <asp:Label ID="lblAlfanumerico2" runat="server" Text="Campo 2:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbAlfanumerico2" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="25"></asp:TextBox>
                                <asp:Label ID="lblAlfanumerico3" runat="server" Text="Campo 3:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbAlfanumerico3" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="25"></asp:TextBox>
                                <br />
                                <asp:Label ID="lblAlfanumerico4" runat="server" Text="Campo 4:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbAlfanumerico4" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="25"></asp:TextBox>
                                <asp:Label ID="lblAlfanumerico5" runat="server" Text="Campo 5:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbAlfanumerico5" runat="server" CssClass="textBoxForms" 
                                    Enabled="False" MaxLength="25"></asp:TextBox>
                            </asp:Panel>

                            <asp:Panel ID="pnlEnteros" runat="server" GroupingText="Campos Enteros">
                                <asp:Label ID="lblEntero1" runat="server" Text="Campo 6:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbEntero1" runat="server" CssClass="textBoxForms" 
                                    Enabled="False"></asp:TextBox>
                                <asp:Label ID="lblEntero2" runat="server" Text="Campo 7:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbEntero2" runat="server" CssClass="textBoxForms" 
                                    Enabled="False"></asp:TextBox>
                                <asp:Label ID="lblEntero3" runat="server" Text="Campo 8:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbEntero3" runat="server" CssClass="textBoxForms" 
                                    Enabled="False"></asp:TextBox>
                                <br />
                            </asp:Panel>

                            <asp:Panel ID="pnlDecimales" runat="server" GroupingText="Campos Decimales">
                                <asp:Label ID="lblDecimal1" runat="server" Text="Campo 9:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbDecimal1" runat="server" CssClass="textBoxForms" 
                                    Enabled="False"></asp:TextBox>
                                <asp:Label ID="lblDecimal2" runat="server" Text="Campo 10:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbDecimal2" runat="server" CssClass="textBoxForms" 
                                    Enabled="False"></asp:TextBox>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                </asp:TabContainer>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>    
    <div style="margin-left:60px">
        <asp:Panel ID="pnlFiltroReportes" runat="server">
            <FiltroReportes:Filtro runat="server" ID="frReportes" />
        </asp:Panel>
    </div>
    <asp:ValidationSummary ID="vlsGrupoValidaciones" runat="server" 
        ValidationGroup="vendedorVG" ForeColor="Red" />
</asp:Content>
