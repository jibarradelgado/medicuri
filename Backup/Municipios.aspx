<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Municipios.aspx.cs" Inherits="Medicuri.Municipios" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
    
    <style type="text/css">
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
    ValidationGroup="Municipios" />
    <div id="divCatalogo" class="divCatalogo">
        <div id="divCatalogoSub">
        <asp:Panel ID="pnlCatalogo" runat="server" Height="350px" ScrollBars="Auto">
            <asp:UpdatePanel ID="upnCatalogo" runat="server">                    
                <ContentTemplate>
                <div id="divCatalogoSeleccionable">    
                    <asp:Panel ID="pnlCatalogoSeleccionable" runat="server" 
                        style="text-align: left">
                        <asp:UpdatePanel ID="upnCatalogoSeleccionable" runat="server" >
                        <ContentTemplate>
                            <asp:Label ID="lblEstado2" runat="server" Text="Estado:" Width="100px" CssClass="labelForms"></asp:Label>
                            <asp:ComboBox ID="cmbEstadoCatalogo" runat="server" AutoPostBack="True" 
                                MaxLength="0" style="display: inline; text-align: left;" AppendDataBoundItems="True" 
                                onselectedindexchanged="cmbEstado2_SelectedIndexChanged" 
                                DataTextField="Nombre" DataValueField="idEstado" 
                                DropDownStyle="DropDownList">
                            </asp:ComboBox>                            
                        </ContentTemplate>                        
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
                <br />
                <br />
                <div id="divCatalogoTabla"  >
                    <asp:GridView ID="gdvDatos" runat="server" AutoGenerateColumns="False" 
                        EmptyDataText="No existen municipios registrados aun" 
                        DataKeyNames="idMunicipio" 
                        onselectedindexchanged="gdvDatos_SelectedIndexChanged" PageSize="100" 
                        BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                        CellPadding="3" ForeColor="Black" GridLines="Vertical" AllowPaging="True" 
                        AllowSorting="True" onpageindexchanging="gdvDatos_PageIndexChanging" 
                        onsorting="gdvDatos_Sorting"  >                    
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                                SelectText="-" ShowSelectButton="True" />
                            <asp:BoundField DataField="Clave" HeaderText="Clave" ReadOnly="True" 
                                SortExpression="Clave" >
                                <ItemStyle Width="200px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" ReadOnly="True" 
                                SortExpression="Nombre" >
                                <ItemStyle Width="600px" />
                            </asp:BoundField>
                            <asp:CheckBoxField DataField="Activo" HeaderText="Activo" >
                                <ItemStyle Width="50px" />
                            </asp:CheckBoxField>
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
      <asp:UpdatePanel ID="pnlFormulario" runat="server" >
     <ContentTemplate>
         <asp:Panel ID="Panel1" runat="server">
         
         <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                 Width="843px">
             <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Datos Municipio">
                <ContentTemplate>
                <br />
                 <asp:Label ID="lblEstado" runat="server" CssClass="labelForms" Text="Estado:" 
                            Width="100px"></asp:Label>
                        <asp:ComboBox ID="cmbEstadoFormulario" runat="server" AutoPostBack="True" 
                            MaxLength="0" style="display: inline;" AppendDataBoundItems="True" 
                            DataTextField="Nombre" DataValueField="idEstado"></asp:ComboBox>
                            <br /><br />
             <asp:Label ID="lblClave" runat="server" CssClass="labelForms" Text="Clave:" 
                            Width="100px"></asp:Label>
                        <asp:TextBox ID="txbClave" runat="server" CssClass="textBoxForms" 
                            ValidationGroup="Municipios" MaxLength="15"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvClave" runat="server" 
                        ErrorMessage="Clave: campo requerido" ControlToValidate="txbClave" 
                        ValidationGroup="Municipios">*</asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cmvClave" runat="server" 
                            ErrorMessage="Clave: ya existe" ControlToValidate="txbClave" Text="*" 
                            ValidationGroup="Municipios" onservervalidate="cmvClave_ServerValidate"></asp:CustomValidator>
                            <br /><br />
                        <asp:Label ID="lblNombre" runat="server" CssClass="labelForms" Text="Nombre:" 
                            Width="100px"></asp:Label>
                        <asp:TextBox ID="txbNombre" runat="server" CssClass="textBoxForms" 
                            ValidationGroup="Municipios" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                        ErrorMessage="Nombre: campo requerido" ControlToValidate="txbNombre" 
                        ValidationGroup="Municipios">*</asp:RequiredFieldValidator>
                        <br /><br />
                        <asp:CheckBox ID="ckbActivo" runat="server" Text="Activo:" TextAlign="Left" CssClass="checkBoxForms" Width="188px"/>
                </ContentTemplate>
             </asp:TabPanel>
         </asp:TabContainer>
    </asp:Panel>
     </ContentTemplate>
     </asp:UpdatePanel>
</div>        
    <div>
        <asp:UpdatePanel ID="pnlAvisos" runat="server">
            <ContentTemplate>
                <asp:Label id="lblAviso" runat="server" Text=""></asp:Label>
                <br />
                <asp:Label id="lblAviso2" runat="server" Text=""></asp:Label>                
            </ContentTemplate>            
        </asp:UpdatePanel>
    </div>
</asp:Content>
