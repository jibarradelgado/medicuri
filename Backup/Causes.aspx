<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true"
    CodeBehind="Causes.aspx.cs" Inherits="Medicuri.Causes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <div>
        <asp:Label ID="lblResults" runat="server" Font-Bold="True"></asp:Label></div>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ToolkitScriptManager>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="causes" ForeColor="Red" />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="cie" ForeColor="Red" />
    <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="medicamentos" ForeColor="Red"/>
    <%-- Items List --%>
    <div id="divCatalogo">
        <asp:Panel ID="pnlList" runat="server" Height="350px" ScrollBars="Auto">
            <asp:UpdatePanel ID="upnList" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gdvDatos" runat="server" BackColor="White" BorderColor="#999999"
                    BorderStyle="Solid" BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical"
                    AutoGenerateColumns="False" Width="845px" DataKeyNames="idCause" 
                    AllowPaging="True" AllowSorting="True" 
                    onpageindexchanging="gdvDatos_PageIndexChanging" onsorting="gdvDatos_Sorting" 
                    PageSize="100">
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" ShowSelectButton="True">
                            <HeaderStyle Width="16px" />
                        </asp:CommandField>
                        <asp:BoundField DataField="idCause" HeaderText="idCause" SortExpression="idCause"
                            Visible="False" />
                        <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave">
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre">
                            <HeaderStyle Width="120px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" 
                            SortExpression="Descripcion" />
                        <asp:BoundField DataField="Conglomerado" HeaderText="Conglomerado" SortExpression="Conglomerado">
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
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
                <asp:TabContainer ID="tbcForm" runat="server" ActiveTabIndex="0" 
                    Width="845px">
                    <%-- tabDatosCause --%>
                    <asp:TabPanel runat="server" HeaderText="Datos de Cause" ID="tabDatosCause">
                        <ContentTemplate>
                            <asp:Panel ID="pnlDatosGenerales" runat="server" GroupingText="Datos Generales">
                                <asp:Label ID="lblClave" runat="server" Text="Clave:" CssClass="labelForms" 
                                    Width="80px"></asp:Label>
                                <asp:TextBox ID="txbClave" runat="server" CssClass="textBoxForms" 
                                    ValidationGroup="causes"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvClave" runat="server" ErrorMessage="Campo requerido: Clave"
                                    ForeColor="Red" ValidationGroup="causes" ControlToValidate="txbClave" 
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="cmvClave" runat="server" ControlToValidate="txbClave" 
                                    Display="Dynamic" ErrorMessage="Clave: ya existe" ForeColor="Red" 
                                    onservervalidate="cmvClave_ServerValidate" ValidationGroup="causes">*</asp:CustomValidator>
                                <asp:Label ID="lblNombre" runat="server" Text="Nombre:" CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbNombre" runat="server" CssClass="textBoxForms" 
                                    ValidationGroup="causes"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNomre" runat="server" ErrorMessage="Campo requerido: Nombre"
                                    ForeColor="Red" ValidationGroup="causes" ControlToValidate="txbNombre" 
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:Label ID="lblConglomerado" runat="server" Text="Conglomerado: " CssClass="labelForms"
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbConglomerado" runat="server" CssClass="textBoxForms"></asp:TextBox><br />
                                <asp:Label ID="lblDescripcion" runat="server" Text="Descripción:" CssClass="labelFormsNextTextArea"
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txaDescripcion" runat="server" TextMode="MultiLine" Height="110px"
                                    Width="420px"></asp:TextBox>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel runat="server" HeaderText="Datos de CIE" ID="tabDatosCIE">
                        <HeaderTemplate>
                            Datos de CIE
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="pnlCIE" runat="server" GroupingText="CIE">
                                <asp:Label ID="lblClaveCIE" runat="server" Text="Clave CIE:" CssClass="labelForms"
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txbClaveCIE" runat="server" CssClass="textBoxForms" 
                                    ValidationGroup="cie"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfv" runat="server" ErrorMessage="Campo requerido: Clave CIE"
                                    ForeColor="Red" ValidationGroup="cie" 
                                    ControlToValidate="txbClaveCIE" Display="Dynamic">*</asp:RequiredFieldValidator>
                                <asp:Label ID="lblTipoCIE" runat="server" Text="Tipo:" CssClass="labelForms" Width="90px"></asp:Label>
                                <asp:TextBox ID="txbTipoCIE" runat="server" CssClass="textBoxForms" 
                                    ValidationGroup="cie"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTipoCIE" runat="server" ErrorMessage="Campo requerido: Tipo CIE"
                                    ForeColor="Red" ValidationGroup="cie" ControlToValidate="txbTipoCIE" 
                                    Display="Dynamic">*</asp:RequiredFieldValidator>
                                &nbsp&nbsp
                                <asp:ImageButton ID="imbAgregarCie" runat="server" 
                                    ImageUrl="~/Icons/plus_16.png" ValidationGroup="cie" 
                                    onclick="imbAgregarCie_Click" />
                                <br />
                                <asp:Label ID="lblDescripcionCIE" runat="server" Text="Descripción:" CssClass="labelFormsNextTextArea"
                                    Width="90px"></asp:Label>
                                <asp:TextBox ID="txaDescripcionCIE" runat="server" TextMode="MultiLine" Height="110px"
                                    Width="420px"></asp:TextBox>
                            </asp:Panel>
                            <br />
                            <br />
                            <asp:Panel ID="pnlCatalogoCIE" runat="server">
                                <asp:UpdatePanel ID="upnCatalogoCIE" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gdvCatalogoCIE" runat="server" Width="840px" 
                                        AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" 
                                        BorderStyle="Solid" BorderWidth="1px" CellPadding="3" DataKeyNames="idCauseCie" 
                                        ForeColor="Black" GridLines="Vertical" 
                                        onselectedindexchanged="gdvCatalogoCIE_SelectedIndexChanged1">
                                        <AlternatingRowStyle BackColor="#CCCCCC" />
                                        <Columns>
                                            <asp:BoundField DataField="Clave" HeaderText="Clave">
                                            <HeaderStyle Width="120px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Tipo" HeaderText="Tipo">
                                            <HeaderStyle Width="120px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/delete_16.png" 
                                                ShowSelectButton="True">
                                            <HeaderStyle Width="15px" />
                                            </asp:CommandField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCCCC" />
                                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                        <sortedascendingcellstyle backcolor="#F1F1F1" />
                                        <sortedascendingheaderstyle backcolor="#808080" />
                                        <sorteddescendingcellstyle backcolor="#CAC9C9" />
                                        <sorteddescendingheaderstyle backcolor="#383838" />
                                    </asp:GridView>
                                </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="tabMedicamentos" runat="server" HeaderText="Medicamentos">
                        <ContentTemplate>
                            <asp:Panel ID="pnlMedicamentos" runat="server" GroupingText="Medicamentos">
                                <asp:Table ID="Table1" runat="server" Width="835px">
                                    <asp:TableRow runat="server">
                                        <asp:TableCell runat="server" HorizontalAlign="Center" Width="75px"><asp:Label ID="Label12" runat="server" BorderColor="Black" BorderStyle="Ridge" Width="125px">Clave</asp:Label>
</asp:TableCell>
                                        <asp:TableCell runat="server" HorizontalAlign="Center" Width="150px"><asp:Label ID="Label13" runat="server" BorderColor="Black" BorderStyle="Ridge" Width="150px">Medicamento</asp:Label>
</asp:TableCell>
                                        <asp:TableCell runat="server" HorizontalAlign="Center" Width="100px"><asp:Label ID="Label14" runat="server" BorderColor="Black" BorderStyle="Ridge" Width="100px">Presentación</asp:Label>
</asp:TableCell>
                                        <asp:TableCell runat="server" HorizontalAlign="Center" Width="145px"><asp:Label ID="Label17" runat="server" BorderColor="Black" BorderStyle="Ridge" Width="120px">Descripción</asp:Label>
</asp:TableCell>
                                        <asp:TableCell runat="server" HorizontalAlign="Center" Width="200px"><asp:Label ID="Label20" runat="server" BorderColor="Black" BorderStyle="Ridge" Width="200px">Cuadro Básico</asp:Label>
</asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow runat="server">
                                        <asp:TableCell Width="75px" runat="server"><asp:TextBox ID="txbClaveMedicamento" runat="server" Width="125px" AutoPostBack="True" ValidationGroup="Medicamentos" OnTextChanged="txbClaveMedicamento_TextChanged"></asp:TextBox>
<asp:AutoCompleteExtender ID="txbClave_AutoCompleteExtender" runat="server" 
                                                DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                                                ServiceMethod="RecuperarClave1Producto" ServicePath="BusquedasAsincronas.asmx" 
                                                TargetControlID="txbClaveMedicamento" MinimumPrefixLength="1"></asp:AutoCompleteExtender>
</asp:TableCell>
                                        <asp:TableCell Width="150px" runat="server"><asp:TextBox ID="txbNombreMedicamento" runat="server" AutoPostBack="True" Width="150px" OnTextChanged="txbNombreMedicamento_TextChanged"></asp:TextBox>
<asp:AutoCompleteExtender ID="txbProducto_AutoCompleteExtender" runat="server" 
                                                DelimiterCharacters="" Enabled="True" FirstRowSelected="True" 
                                                ServiceMethod="RecuperarNombreProducto" ServicePath="BusquedasAsincronas.asmx" 
                                                TargetControlID="txbNombreMedicamento" MinimumPrefixLength="1"></asp:AutoCompleteExtender>
</asp:TableCell>
                                        <asp:TableCell Width="100px" runat="server"><asp:TextBox ID="txbPresentacionMedicamento" runat="server" Width="100px" Enabled="False"></asp:TextBox>
</asp:TableCell>
                                        <asp:TableCell Width="145px" runat="server"><asp:TextBox ID="txbDescripcion" runat="server" Width="120px"></asp:TextBox>
</asp:TableCell>
                                        <asp:TableCell Width="200px" runat="server"><asp:TextBox ID="txbCuadroBasico" runat="server" Width="200px"></asp:TextBox>
</asp:TableCell>
                                        <asp:TableCell Width="25px" runat="server"><asp:ImageButton ID="imbAgregarMedicamento" runat="server" Height="16px" ImageUrl="~/Icons/plus_16.png" ValidationGroup="medicamentos" />
</asp:TableCell>
                                        <asp:TableCell width="0px" runat="server"><asp:TextBox ID="txbidProducto" 
                                            runat="server" Width="0px" Visible="False"></asp:TextBox>
</asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                                <asp:RequiredFieldValidator ID="rfvMedicamentos" runat="server" ErrorMessage="Clave Medicamento: Campo Requerido" ControlToValidate="txbClaveMedicamento" Display="Dynamic" Text="*" ValidationGroup="medicamentos" ForeColor="Red"></asp:RequiredFieldValidator>
                                <br />
                                <asp:GridView ID="gdvCausesMedicamentos" runat="server" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px"
                                    CellPadding="3" ForeColor="Black" GridLines="Vertical" Width="840px" 
                                    DataKeyNames="IdMedicamento" 
                                    onselectedindexchanged="gdvCausesMedicamentos_SelectedIndexChanged">
                                    <AlternatingRowStyle BackColor="#CCCCCC" />
                                    <Columns>
                                        <asp:BoundField DataField="idProducto" HeaderText="idProducto" SortExpression="idProducto"
                                            Visible="False" />
                                        <asp:BoundField DataField="idEnsamble" HeaderText="idEnsamble" SortExpression="idEnsamble"
                                            Visible="False" />
                                        <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" />
                                        <asp:BoundField DataField="Presentacion" HeaderText="Presentación" SortExpression="Presentacion" />
                                        <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                        <asp:BoundField DataField="CuadroBasico" HeaderText="Cuadro Básico" />
                                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/delete_16.png" ShowSelectButton="True" />
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" />
                                    <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="Gray" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#383838" />
                                </asp:GridView>
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
                <asp:Label id="lblAviso1" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label id="lblAviso2" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label ID="lblAviso3" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label ID="lblAviso4" runat="server" Text=""></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
