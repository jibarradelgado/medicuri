<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Configuracion.aspx.cs" Inherits="Medicuri.Configuracion" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%-- Este es del header --%>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
    <style type="text/css">
        #Formulario
        {
            height: 384px;
        }
    </style>
</asp:Content>

<%-- Este es del body o work area --%>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>

    <div id="Formulario">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                    Height="391px" Width="873px">
                            
                    <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Empresa">
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server" GroupingText="Datos Fiscales" 
                                Height="231px">
                                <br />
                                <asp:Label ID="lblRazonSocial" runat="server" Text="Razon Social" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbRazonSocial" runat="server" ValidationGroup="Configuracion"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvRazonSocial" runat="server" 
                                    ControlToValidate="txbRazonSocial" ErrorMessage="Razon Social: Campo requerido" 
                                    ValidationGroup="Configuracion" ForeColor="Red">*</asp:RequiredFieldValidator>                                
                                <asp:Label ID="lblRFC" runat="server" Text="RFC:" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbRfc" runat="server" ValidationGroup="Configuracion"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvRfc" runat="server" 
                                    ControlToValidate="txbRfc" ErrorMessage="RFC: Campo Requerido" 
                                    ValidationGroup="Configuracion" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="lblRegimenFiscal" runat="server" Text="Régimen Fiscal:" CssClass="labelForms" Width="130px"></asp:Label>
                                <asp:TextBox ID="txbRegimenFiscal" runat="server" Width="320px" ValidationGroup="Configuracion"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvRegimenFiscal" runat="server" 
                                    ControlToValidate="txbRegimenFiscal" ErrorMessage="Régimen fiscal: Campo Requerido" 
                                    ValidationGroup="Configuracion" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="lblDomicilio" runat="server" Text="Domicilio:" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbDomicilio" runat="server" Width="320px" 
                                    ValidationGroup="Configuracion"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvDomicilio" runat="server" 
                                    ControlToValidate="txbDomicilio" ErrorMessage="Domicilio: Campo Requerido" 
                                    ValidationGroup="Configuracion" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="lblCodigoPostal" runat="server" CssClass="labelForms" 
                                    Text="Código Postal:" Width="130px"></asp:Label>
                                <asp:TextBox ID="txbCodigoPostal" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCodigoPostal" runat="server" 
                                    ControlToValidate="txbCodigoPostal" 
                                    ErrorMessage="Código Postal: Campo Requerido" ForeColor="Red" 
                                    ValidationGroup="Configuracion">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="lblMunicipio" runat="server" CssClass="labelForms" Text="Municipio" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbMunicipio" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMunicipio" runat="server" 
                                    ControlToValidate="txbMunicipio" ErrorMessage="Municipio: Campo Requerido" 
                                    ForeColor="Red" ValidationGroup="Configuracion">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="Label30" runat="server" CssClass="labelForms" Text="Estado" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbEstado" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvEstado" runat="server" 
                                    ControlToValidate="txbEstado" ErrorMessage="Estado: Campo Requerido" 
                                    ForeColor="Red" ValidationGroup="Configuracion">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="Label31" runat="server" CssClass="labelForms" Text="País:" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbPais" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPais" runat="server" 
                                    ControlToValidate="txbPais" ErrorMessage="País: Campo Requerido" 
                                    ForeColor="Red" ValidationGroup="Configuracion">*</asp:RequiredFieldValidator>
                                <br />
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="Panel2" runat="server" Height="125px" 
                                GroupingText="Facturación electronica">
                                <br />
                                <asp:Label ID="Label12" runat="server" Text="Certificado:" 
                                    CssClass="labelForms" Width="130px"></asp:Label>
                                <asp:FileUpload ID="ofdCertificado" runat="server" />
                                &nbsp;<asp:Label ID="lbCertificado" runat="server" Width="130px"></asp:Label>
                                <br />
                                <asp:Label ID="Label13" runat="server" Text="Llave:" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:FileUpload ID="ofdLlave" runat="server" />
                                &nbsp;<asp:Label ID="lbLlave" runat="server" Width="130px"></asp:Label>
                                <br />
                                <asp:Label ID="Label1" runat="server" Text="Firma:" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:FileUpload ID="ofdFirma" runat="server" />
                                &nbsp;<asp:Label ID="lblFirma" runat="server" Width="130px"></asp:Label>
                                <br />
                                <asp:Label ID="Label22" runat="server" Text="Contraseña:" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbContraseñaFac" runat="server" TextMode="Password"></asp:TextBox>
                                &nbsp;</asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Base de datos">
                        <ContentTemplate>
                            <br />
                            <asp:Panel ID="Panel7" runat="server" GroupingText="Datos de conexión" 
                                Height="156px">
                                <br />
                                <asp:Label ID="Label14" runat="server" Text="Servidor:" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbServidor" runat="server" Width="168px" 
                                    ValidationGroup="Configuracion"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvServidor" runat="server" 
                                    ControlToValidate="txbServidor" ErrorMessage="Servidor: Campo requerido" 
                                    ValidationGroup="Configuracion" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="Label15" runat="server" Text="Usuario:" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbUsuario" runat="server" ValidationGroup="Configuracion"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" 
                                    ControlToValidate="txbUsuario" ErrorMessage="Usuario: Campo Requerido" 
                                    ValidationGroup="Configuracion" ForeColor="Red">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="Label16" runat="server" Text="Contraseña:" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbContraseñaBd" runat="server" TextMode="Password" 
                                    ValidationGroup="Configuracion"></asp:TextBox>
                                &nbsp;<asp:RequiredFieldValidator ID="rfvContraseñaBd" runat="server" 
                                    ControlToValidate="txbContraseñaBd" ErrorMessage="Contraseña: Campo requerido" 
                                    ValidationGroup="Configuracion" ForeColor="Red">*</asp:RequiredFieldValidator>
                            </asp:Panel>
                            <br />
                            &nbsp;
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Diseño">
                        <ContentTemplate>
                            <br />
                            <asp:Panel ID="Panel8" runat="server" GroupingText="Interfaz Grafica" 
                                Height="153px">
                                <asp:Label ID="Label17" runat="server" Text="Color:" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <br />
                                <asp:RadioButton ID="rbVerde" runat="server" BackColor="#009933" 
                                     Width="50px" GroupName="colors"/>
                                <asp:RadioButton ID="rbVerde1" runat="server" BackColor="#33CC33" 
                                     Width="50px" GroupName="colors"/>
                                <asp:RadioButton ID="rbAzul" runat="server" BackColor="#000099" 
                                     Width="50px" GroupName="colors"/>
                                <asp:RadioButton ID="rbAzul1" runat="server" BackColor="Blue" 
                                    Width="50px" GroupName="colors"/>
                                <asp:RadioButton ID="rbCafe" runat="server" BackColor="#996633" 
                                     Width="50px" GroupName="colors"/>
                                <br />
                                <asp:RadioButton ID="rbRojo" runat="server" BackColor="#CC0000" Width="50px" GroupName="colors"/>
                                <asp:RadioButton ID="rbAmarillo" runat="server" BackColor="Yellow" 
                                    Width="50px" GroupName="colors"/>
                                <asp:RadioButton ID="rbNaranja" runat="server" BackColor="#FF9900" 
                                    Width="50px" GroupName="colors"/>
                                <asp:RadioButton ID="rbMorado" runat="server" BackColor="#993399" 
                                    Width="50px" GroupName="colors"/>
                                <asp:RadioButton ID="rbGris" runat="server" BackColor="Silver" Width="50px" GroupName="colors"/>
                                <br />
                                <br />
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="Panel10" runat="server" GroupingText="Imagenes" 
                                Height="59px">

                                <asp:Label ID="Label23" runat="server" Text="Logotipo:" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:FileUpload ID="ofdLogotipo" runat="server" />
                                &nbsp;<asp:Label ID="lbLogotipo" runat="server" Width="130px"></asp:Label>
                                <br />
                                <asp:Label ID="Label32" runat="server" Font-Bold="True" ForeColor="Red" 
                                    Text="* Imagen en formato .png de 984*100 px"></asp:Label>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="Folios">
                        <ContentTemplate>
                            <asp:Panel ID="Panel3" runat="server" GroupingText="Pedidos" Height="79px">
                                <asp:Label ID="Label18" runat="server" Text="Folio N°" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbFolioPedidos" runat="server"></asp:TextBox>
                                <asp:CheckBox ID="ckbPedidos" runat="server" Text="Automatico" />
                            </asp:Panel>
                            <asp:Panel ID="Panel4" runat="server" GroupingText="Recetas" Height="79px">
                                <asp:Label ID="Label19" runat="server" Text="Folio N°" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbFolioRecetas" runat="server"></asp:TextBox>
                                <asp:CheckBox ID="ckbRecetas" runat="server" Text="Automatico" />
                            </asp:Panel>
                            <asp:Panel ID="Panel5" runat="server" GroupingText="Remisiones" Height="79px">
                                <asp:Label ID="Label20" runat="server" Text="Folio N°" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbFolioRemisiones" runat="server"></asp:TextBox>
                                <asp:CheckBox ID="ckbRemisiones" runat="server" Text="Automatico" />
                            </asp:Panel>
                            <asp:Panel ID="Panel6" runat="server" GroupingText="Facturas" Height="100px">
                                <asp:Label ID="Label21" runat="server" Text="Folio N°" CssClass="labelForms" 
                                    Width="130px"></asp:Label>
                                <asp:TextBox ID="txbFolioFacturas" runat="server"></asp:TextBox>
                                <asp:CheckBox ID="ckbFacturas" runat="server" Text="Automatico" />
                                <br />
                                <br />
                                <asp:Label ID="Label33" runat="server" CssClass="labelForms" 
                                    Text="No. Máx. De Reglones Por Factura:" Width="225px"></asp:Label>
                                <asp:TextBox ID="txbNumRenglonesFactura" runat="server" Width="70px" 
                                    MaxLength="3" >20</asp:TextBox>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                
                    <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="Adicionales">
                        <ContentTemplate>
                            <br />
                            <asp:Panel ID="Panel9" runat="server" GroupingText="Ventas:">
                                <asp:CheckBox ID="ckbVentasNegativas" runat="server" 
                                    Text="Permitir vender articulos en existencia negativa" />
                            </asp:Panel>
                            <asp:Panel ID="Panel12" runat="server" GroupingText="Alertas:">
                                <asp:Label ID="lblCaducidad" runat="server" Text="Días de anticipación para alertar caducidad próxima de productos: "></asp:Label>
                                <asp:RegularExpressionValidator ID="revCaducidad" runat="server" 
                                    ControlToValidate="txbCaducidad" 
                                    ErrorMessage="Caducidad: debe ser un numero entero" ForeColor="Red" 
                                    ValidationExpression="^(\+|-)?\d+$" ValidationGroup="Configuracion">*</asp:RegularExpressionValidator>
                                <asp:TextBox ID="txbCaducidad" runat="server"></asp:TextBox>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                    
                    <asp:TabPanel ID="TabPanel6" runat="server" HeaderText="SMTP">
                        <ContentTemplate>
                            <asp:Panel ID="Panel11" runat="server" 
                                GroupingText="Servidor de correo Electrónico">
                                <asp:Label ID="Label24" runat="server" CssClass="labelForms" Text="Servidor:" 
                                    Width="150px"></asp:Label>
                                <asp:TextBox ID="txbSevidorSmtp" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvSevidorSmtp" runat="server" 
                                    ControlToValidate="txbSevidorSmtp" 
                                    ErrorMessage="Servidor SMTP: Campo Requerido" ForeColor="Red" 
                                    ValidationGroup="Configuracion">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="Label25" runat="server" CssClass="labelForms" Text="Puerto:" 
                                    Width="150px"></asp:Label>
                                <asp:TextBox ID="txbPuertoSmtp" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPuertoSmtp" runat="server" ControlToValidate="txbPuertoSmtp" 
                                    ErrorMessage="Servidor SMTP: Campo Requerido" ForeColor="Red" 
                                    ValidationGroup="Configuracion">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="Label26" runat="server" CssClass="labelForms" Text="Correo Emisor:" 
                                    Width="150px
                                    "></asp:Label>
                                <asp:TextBox ID="txbUsuarioSmtp" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCorreo" runat="server" 
                                    ErrorMessage="Campo Requerido: Correo Emisor" 
                                    ControlToValidate="txbUsuarioSmtp" ForeColor="Red" 
                                    ValidationGroup="Configuracion">*</asp:RequiredFieldValidator>
                                <br />
                                <asp:Label ID="Label27" runat="server" CssClass="labelForms" Text="Contraseña:" 
                                    Width="150px"></asp:Label>
                                <asp:TextBox ID="txbContraseñaSmtp" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvContraseñaSMTP" runat="server" 
                                    ErrorMessage="Campo Requerido: Contraseña SMTP" 
                                    ControlToValidate="txbContraseñaSmtp" ForeColor="Red" 
                                    ValidationGroup="Configuracion">*</asp:RequiredFieldValidator>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                    
                </asp:TabContainer>
                <asp:ValidationSummary ID="vsConfiguracion" runat="server" 
                    ValidationGroup="Configuracion" ForeColor="Red" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


</asp:Content>
