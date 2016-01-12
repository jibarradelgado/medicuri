<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Almacenes.aspx.cs" Inherits="Medicuri.Almacenes" EnableSessionState="True" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="FiltroReportes" TagName="Filtro" Src="~/FiltroReportes.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Almacenes" HeaderText="Verifique los siguientes campos" />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Contactos" HeaderText="Verifique los siguientes campos" /> 
    <div id="lista">
        <asp:Panel ID="pnlLista" runat="server"  Height="350px" ScrollBars="Auto">
        <asp:UpdatePanel ID="upnLista" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gdvLista" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                    CellPadding="3" ForeColor="Black" GridLines="Vertical" 
                    
                    DataKeyNames="idAlmacen,idEstados,idMunicipios,idPoblaciones,idColonias,idTipos" Width="840px" 
                    AllowPaging="True" PageSize="100" AllowSorting="True" 
                    onsorting="gdvLista_Sorting" 
                    onpageindexchanging="gdvLista_PageIndexChanging">
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                            ShowSelectButton="True" />
                        <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" 
                            SortExpression="Nombre" />
                        <asp:BoundField DataField="Telefono" HeaderText="Telefono" 
                            SortExpression="Telefono" />
                        <asp:BoundField DataField= "Estado" HeaderText="Estado" 
                            SortExpression="Estado" />
                        <asp:BoundField DataField= "Poblacion" HeaderText="Poblacion" 
                            SortExpression="Poblacion" />
                        <asp:BoundField HeaderText="Tipo" DataField="Tipo" SortExpression="Tipo" />
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" ReadOnly="True" />
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
                <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Datos Almacén">
                    <HeaderTemplate>
                       Datos Almacén
                    </HeaderTemplate>
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server" GroupingText="General">
                            <asp:Label ID="Label2" runat="server" Text="Clave:" CssClass="labelForms"  
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txtClave" runat="server" CssClass="textBoxForms" 
                                ValidationGroup="Almacenes" MaxLength="15"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvClave" runat="server" ErrorMessage="Clave: campo requerido" ControlToValidate="txtClave" Text="*" ValidationGroup="Almacenes"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cmvClave" runat="server" 
                                ErrorMessage="Clave: ya existe" ControlToValidate="txtClave" 
                                ValidationGroup="Almacenes" Text="*" onservervalidate="cmvClave_ServerValidate"></asp:CustomValidator>
                            <asp:Label ID="Label3" runat="server" Text="Nombre:" CssClass="labelForms" 
                                Width="64px"></asp:Label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="textBoxForms" 
                                MaxLength="75"></asp:TextBox>                            
                            <asp:Label ID="Label14" runat="server" Text="Tipo:" CssClass="labelForms"  
                                Width="90px"></asp:Label>
                            <asp:ComboBox ID="cmbTipos" runat="server" DataTextField="Nombre" 
                                DataValueField="idTipo" MaxLength="0" ValidationGroup="Almacenes" 
                                DropDownStyle="DropDownList"></asp:ComboBox>
                            <asp:CustomValidator ID="cmvTipo" runat="server" 
                                ErrorMessage="Tipo: Dato no válido" ControlToValidate="cmbTipos" 
                                ValidationGroup="Almacenes" Text="*" onservervalidate="cmvTipo_ServerValidate"></asp:CustomValidator>
                            <br />
                            <asp:CheckBox ID="ckbActivo" runat="server" Text="Activo" 
                                CssClass="labelForms" Width="141px" />
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server" GroupingText="Contacto">
                            <asp:Label ID="Label1" runat="server" Text="Calle:" CssClass="labelForms" 
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txtCalle" runat="server" CssClass="textBoxForms" 
                                MaxLength="75"></asp:TextBox>                            
                            <asp:Label ID="Label4" runat="server" Text="Num. Ext.:" CssClass="labelForms" 
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txtNumExt" runat="server" CssClass="textBoxForms" 
                                MaxLength="5"></asp:TextBox>
                            <asp:Label ID="Label5" runat="server" Text="Num. Int.:" CssClass="labelForms" 
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txtNumInt" runat="server" CssClass="textBoxForms" 
                                MaxLength="5"></asp:TextBox>
                            <br />
                            <asp:Label ID="Label9" runat="server" Text="Estado:" CssClass="labelForms" 
                                Width="90px"></asp:Label>
                            <asp:ComboBox ID="cmbEstados" runat="server" DataTextField="Nombre" 
                                DataValueField="idEstado" MaxLength="0" AutoPostBack="True" 
                                onselectedindexchanged="cmbEstados_SelectedIndexChanged" Width="108px" 
                                ValidationGroup="Almacenes" DropDownStyle="DropDownList"></asp:ComboBox>
                            <asp:CustomValidator ID="cmvEstados" runat="server" 
                                ErrorMessage="Estados: Dato no válido" ControlToValidate="cmbEstados" 
                                ValidationGroup="Almacenes" Text="*" 
                                onservervalidate="cmvEstados_ServerValidate"></asp:CustomValidator>
                            <asp:Label ID="Label8" runat="server" Text="Del./Mcpio.:" CssClass="labelForms" 
                                Width="95px"></asp:Label>
                            <asp:ComboBox ID="cmbMunicipios" runat="server" DataTextField="Nombre" 
                                DataValueField="idMunicipio" MaxLength="0" AutoPostBack="True" 
                                onselectedindexchanged="cmbMunicipios_SelectedIndexChanged" Width="108px" 
                                ValidationGroup="Almacenes" DropDownStyle="DropDownList"></asp:ComboBox>
                            <asp:CustomValidator ID="cmvMunicipios" runat="server" 
                                ErrorMessage="Municipio: Dato no válido" ControlToValidate="cmbMunicipios" 
                                ValidationGroup="Almacenes" Text="*" 
                                onservervalidate="cmvMunicipios_ServerValidate"></asp:CustomValidator>
                            <asp:Label ID="Label7" runat="server" Text="Población:" CssClass="labelForms" 
                                Width="90px"></asp:Label>
                            <asp:ComboBox ID="cmbPoblaciones" runat="server" DataTextField="Nombre" 
                                DataValueField="idPoblacion" MaxLength="0" AutoPostBack="True" 
                                onselectedindexchanged="cmbPoblaciones_SelectedIndexChanged" Width="108px" 
                                ValidationGroup="Almacenes" DropDownStyle="DropDownList"></asp:ComboBox>
                            <asp:CustomValidator ID="cmvPoblaciones" runat="server" 
                                ErrorMessage="Poblacion: Dato no válido" ControlToValidate="cmbPoblaciones" 
                                ValidationGroup="Almacenes" Text="*" 
                                onservervalidate="cmvPoblaciones_ServerValidate"></asp:CustomValidator>
                            <br />
                            <br />
                            <asp:Label ID="Label6" runat="server" Text="Colonia:" CssClass="labelForms" 
                                Width="90px"></asp:Label>
                            <asp:ComboBox ID="cmbColonias" runat="server" DataTextField="Nombre" 
                                DataValueField="idColonia" MaxLength="0" Width="108px" 
                                ValidationGroup="Almacenes" DropDownStyle="DropDownList"></asp:ComboBox>
                            <asp:CustomValidator ID="cmvColonias" runat="server" 
                                ErrorMessage="Colonia: Dato no válido" ControlToValidate="cmbColonias" 
                                ValidationGroup="Almacenes" Text="*" 
                                onservervalidate="cmvColonias_ServerValidate"></asp:CustomValidator>
                            <asp:Label ID="Label10" runat="server" Text="País:" CssClass="labelForms" 
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txtPais" runat="server" CssClass="textBoxForms" 
                                ReadOnly="True" MaxLength="50">México</asp:TextBox> 
                            <asp:Label ID="Label11" runat="server" Text="Código Postal:" 
                                CssClass="labelForms" Width="90px"></asp:Label>
                            <asp:TextBox ID="txtCodigoPostal" runat="server" CssClass="textBoxForms" 
                                MaxLength="5"></asp:TextBox>
                            <br />
                            <br />
                            <asp:Label ID="Label12" runat="server" Text="Teléfono:" CssClass="labelForms" 
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="textBoxForms" 
                                MaxLength="15"></asp:TextBox>
                            <asp:Label ID="Label13" runat="server" Text="Fax:" CssClass="labelForms"  
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txtFax" runat="server" CssClass="textBoxForms" MaxLength="15"></asp:TextBox>
                            
                        </asp:Panel>

                       
                    </ContentTemplate>
                </asp:TabPanel>
                 <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Contacto">
                     <HeaderTemplate>
                         Datos de Contactos
                     </HeaderTemplate>
                    <ContentTemplate> 
                     <asp:Label ID="Label99" runat="server" Text="Nombre(s):" CssClass="labelForms" 
                            Width="90px"></asp:Label>
                     <asp:TextBox ID="txtCntNombre" runat="server" CssClass="textBoxForms" 
                            ValidationGroup="Contactos" MaxLength="75"></asp:TextBox>
                     <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ErrorMessage="Nombre: Campo requerido" ControlToValidate="txtCntNombre" ValidationGroup="Contactos">*</asp:RequiredFieldValidator>
                     <asp:Label ID="Label98" runat="server" Text="Apellido(s):" CssClass="labelForms" 
                            Width="77px" Height="16px"></asp:Label>
                     <asp:TextBox ID="txtCntApellidos" runat="server" CssClass="textBoxForms" 
                            ValidationGroup="Contactos" MaxLength="75"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvApellidos" runat="server" 
                            ErrorMessage="Apellidos: Campo requerido" Text="*" ValidationGroup="Contactos" 
                            ControlToValidate="txtCntApellidos"></asp:RequiredFieldValidator>                     
                     <asp:Label ID="Label97" runat="server" Text="Teléfono:" CssClass="labelForms" 
                            Width="77px"></asp:Label>
                     <asp:TextBox ID="txtCntTel" runat="server" CssClass="textBoxForms" MaxLength="15"></asp:TextBox>      
                     <br />                  
                     <asp:Label ID="Label96" runat="server" Text="Celular:" CssClass="labelForms" 
                            Width="90px"></asp:Label>
                     <asp:TextBox ID="txtCntCel" runat="server" CssClass="textBoxForms" MaxLength="15"></asp:TextBox>
                     <asp:Label ID="Label95" runat="server" Text="Correo Electrónico:" 
                            CssClass="labelForms" Width="90px"></asp:Label>
                     <asp:TextBox ID="txtCntCorreoE" runat="server" CssClass="textBoxForms" 
                            MaxLength="50"></asp:TextBox>
                     <asp:ImageButton ID="imbAgregarContacto" runat="server" 
                            onclick="imbAgregarContacto_Click" ImageUrl="~/Icons/plus_16.png" 
                            ValidationGroup="Contactos" />
                        <asp:GridView ID="gdvContactos" runat="server" AutoGenerateColumns="False" 
                            onselectedindexchanged="grvContactos_SelectedIndexChanged" 
                            BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                            CellPadding="3" ForeColor="Black" GridLines="Vertical" Width="840px">
                            <AlternatingRowStyle BackColor="#CCCCCC" />
                            <Columns>
                                <asp:BoundField HeaderText="Nombre(s)" DataField="Nombre" />
                                <asp:BoundField HeaderText="Apellido(s)" DataField="Apellidos" />
                                <asp:BoundField HeaderText="Teléfono" DataField="Telefono" />
                                <asp:BoundField HeaderText="Celular" DataField="celular" />
                                <asp:BoundField HeaderText="Correo electrónico" DataField="CorreoElectronico" />
                                 <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/delete_16.png" 
                                    SelectText="-" ShowSelectButton="True" />
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
                <asp:TabPanel ID="tabDatosOpcionales" runat="server" HeaderText="Datos Opcionales">
                        <ContentTemplate>
                            <asp:Panel ID="pnlaAlfanumericos" runat="server" GroupingText="Otros Alfanumericos">
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

                            <asp:Panel ID="pnlEnteros" runat="server" GroupingText="Otros Enteros">
                                <asp:Label ID="lblEntero1" runat="server" Text="Campo 6:" CssClass="labelForms" 
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txtEntero1" runat="server" CssClass="textBoxForms" 
                                    ValidationGroup="Almacenes"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revEntero1" runat="server" ErrorMessage="Entero1: Numeros enteros" ValidationExpression="^(\+|-)?\d+$" ValidationGroup="Almacenes" ControlToValidate="txtEntero1" Text="*"></asp:RegularExpressionValidator>
                                <asp:Label ID="lblEntero2" runat="server" Text="Campo 7:" CssClass="labelForms" 
                                    Width="77px"></asp:Label>
                                <asp:TextBox ID="txtEntero2" runat="server" CssClass="textBoxForms" 
                                    ValidationGroup="Almacenes"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revEntero2" runat="server" ErrorMessage="Entero2: Numeros enteros" ValidationExpression="^(\+|-)?\d+$" ValidationGroup="Almacenes" ControlToValidate="txtEntero2" Text="*"></asp:RegularExpressionValidator>
                                <asp:Label ID="lblEntero3" runat="server" Text="Campo 8:" CssClass="labelForms" 
                                    Width="77px"></asp:Label>
                                <asp:TextBox ID="txtEntero3" runat="server" CssClass="textBoxForms" 
                                    ValidationGroup="Almacenes"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revEntero3" runat="server" ErrorMessage="Entero3: Numeros enteros" ValidationExpression="^(\+|-)?\d+$" ValidationGroup="Almacenes" ControlToValidate="txtEntero3" Text="*"></asp:RegularExpressionValidator>
                                <br />
                            </asp:Panel>

                            <asp:Panel ID="pnlDecimales" runat="server" GroupingText="Otros Decimales">
                                <asp:Label ID="lblDecimal1" runat="server" Text="Campo 9:" 
                                    CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txtDecimal1" runat="server" CssClass="textBoxForms" 
                                    ValidationGroup="Almacenes"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revDecimal1" runat="server" ErrorMessage="Decimal1: Numeros decimales" ValidationExpression="^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$" ValidationGroup="Almacenes" ControlToValidate="txtDecimal1" Text="*"></asp:RegularExpressionValidator>
                                <asp:Label ID="lblDecimal2" runat="server" Text="Campo 10:" 
                                    CssClass="labelForms" Width="77px"></asp:Label>
                                <asp:TextBox ID="txtDecimal2" runat="server" CssClass="textBoxForms" ValidationGroup="Almacenes"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revDecimal2" runat="server" ErrorMessage="Decimal2: Numeros decimales" ValidationExpression="^[+-]?([0-9]*\.?[0-9]+|[0-9]+\.?[0-9]*)([eE][+-]?[0-9]+)?$" ValidationGroup="Almacenes" ControlToValidate="txtDecimal2" Text="*"></asp:RegularExpressionValidator>                                    
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
