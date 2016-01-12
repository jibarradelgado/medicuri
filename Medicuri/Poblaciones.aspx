<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Poblaciones.aspx.cs" Inherits="Medicuri.Poblaciones" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
    <style type="text/css">
        #divCatalogoSeleccionable
        {
            text-align: left;
        }
        .style1
        {
            text-align: left;
        }
        .labelForms
        {
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="Verifique los siguientes campos" 
    ValidationGroup="Poblaciones" />
    <div id="divCatalogo" class="divCatalogo">
        <div id="divCatalogoSub">
            <asp:Panel ID="pnlCatalogo" runat="server" Height="350px" ScrollBars="Auto">
            <asp:UpdatePanel ID="upnCatalogo" runat="server">                    
                <ContentTemplate>
                <div id="divCatalogoSeleccionable">
                    <asp:UpdatePanel ID="pnlCatalogoSeleccionable" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblEstado2" runat="server" Text="Estado:" Width="100px" CssClass="labelForms"></asp:Label>
                            <asp:ComboBox ID="cmbEstadoCatalogo" runat="server" MaxLength="0" 
                                AppendDataBoundItems="True" AutoPostBack="true"
                                onselectedindexchanged="cmbEstadoCatalogo_SelectedIndexChanged" 
                                DataTextField="Nombre" DataValueField="idEstado">
                            </asp:ComboBox>
                            <asp:Label ID="lblMunicipio2" runat="server" Text="Municipio:" Width="100px" CssClass="labelForms"></asp:Label>
                            <asp:ComboBox ID="cmbMunicipioCatalogo" runat="server" MaxLength="0" 
                                AppendDataBoundItems="True" AutoPostBack="true" 
                                onselectedindexchanged="cmbMunicipioCatalogo_SelectedIndexChanged" 
                                DataTextField="Nombre" DataValueField="idMunicipio">
                            </asp:ComboBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <br />
                <br />
                <div id="divCatalogoTabla"  >
                    <asp:GridView ID="gdvDatos" runat="server" AutoGenerateColumns="False" 
                        EmptyDataText="No existen poblaciones registradas aun" 
                        DataKeyNames="idPoblacion" 
                        onselectedindexchanged="gdvDatos_SelectedIndexChanged" PageSize="100" 
                        BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                        CellPadding="3" ForeColor="Black" GridLines="Vertical" Width="830px" 
                        AllowPaging="True" AllowSorting="True" 
                        onpageindexchanging="gdvDatos_PageIndexChanging" onsorting="gdvDatos_Sorting">                    
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                                SelectText="-" ShowSelectButton="True" >
                                <HeaderStyle Width="30px" />
                            </asp:CommandField>
                            <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" >
                                <HeaderStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" >
                                <HeaderStyle Width="600px" />
                            </asp:BoundField>
                            <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
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
            
        </div>      
    </div>
    <div id="divFormulario" class="divFormulario">
        <asp:UpdatePanel ID="upnFormulario" runat="server">
            <ContentTemplate>
                <asp:TabContainer ID="tbcFormulario" runat="server" ActiveTabIndex="0" Width="840px">
            <asp:TabPanel ID="tbpDatosPoblacion" runat="server" HeaderText="Datos de la Población">
                <HeaderTemplate>
                       Datos de la Población
                </HeaderTemplate>
                <ContentTemplate>
        <asp:Panel ID="pnlFormulario" runat="server" GroupingText="Datos Generales">           
            <div id="divSeleccionable" >            
                <asp:UpdatePanel ID="pnlSeleccionable" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblEstado" runat="server" CssClass="labelForms" Text="Estado:" 
                            Width="90px"></asp:Label>
                        <asp:ComboBox ID="cmbEstadoFormulario" runat="server" MaxLength="0" AutoPostBack="true"
                            style="display: inline;" DataTextField="Nombre" DataValueField="idEstado" 
                            onselectedindexchanged="cmbEstadoFormulario_SelectedIndexChanged" 
                            Width="110px" DropDownStyle="DropDownList">
                        </asp:ComboBox>                                        
                        <asp:Label ID="lblMunicipio" runat="server" CssClass="labelForms" 
                            Text="Municipio:" Width="90px" ></asp:Label>
                        <asp:ComboBox ID="cmbMunicipioFormulario" runat="server" MaxLength="0" 
                            style="display: inline;" DataTextField="Nombre" 
                            DataValueField="idMunicipio" Width="110px" DropDownStyle="DropDownList" >
                        </asp:ComboBox>
                    </ContentTemplate>                
                </asp:UpdatePanel>
            </div>
            <br />
            <div id="divFormularioSub">
                <asp:UpdatePanel ID="pnlFormularioSub" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="lblClave" runat="server" CssClass="labelForms" Text="Clave:" 
                            Width="90px"></asp:Label>
                        <asp:TextBox ID="txbClave" runat="server" CssClass="textBoxForms" 
                            ValidationGroup="Poblaciones" MaxLength="15" Width="130px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvClave" runat="server" ErrorMessage="Clave: campo requerido" 
                        ControlToValidate="txbClave" ValidationGroup="Poblaciones">*</asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cmvClave" runat="server" 
                            ErrorMessage="Clave: ya existe" ControlToValidate="txbClave" Text="*" 
                            ValidationGroup="Poblaciones" onservervalidate="cmvClave_ServerValidate"></asp:CustomValidator>                        
                        <asp:Label ID="lblNombre" runat="server" CssClass="labelForms" Text="Nombre:" 
                            Width="64px"></asp:Label>
                        <asp:TextBox ID="txbNombre" runat="server" CssClass="textBoxForms" 
                            ValidationGroup="Poblaciones" MaxLength="75" Width="130px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ErrorMessage="Nombre: campo requerido" 
                        ControlToValidate="txbNombre" ValidationGroup="Poblaciones">*</asp:RequiredFieldValidator>
                        &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                        <asp:CheckBox ID="ckbActivo" runat="server" Text="Activo:" TextAlign="Left" 
                            CssClass="checkBoxForms" Width="90px"/>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
                </ContentTemplate>
            </asp:TabPanel>
        </asp:TabContainer>
            </ContentTemplate>
        </asp:UpdatePanel>                
    </div> 
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label id="lblAviso" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label id="lblAviso2" runat="server" Text=""></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
