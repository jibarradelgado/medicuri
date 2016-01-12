<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Estados.aspx.cs" Inherits="Medicuri.Estados" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>    
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
    HeaderText="Verifique los siguientes campos" ValidationGroup="Estados" />     
    <div id="divCatalogo" class="divCatalogo">
        <div id="divCatalogoSub" class="divCatalogoSub">
            <asp:Panel ID="pnlCatalogo" runat="server" Height="350px" ScrollBars="Auto">
            <asp:UpdatePanel ID="upnCatalogo" runat="server">                    
                <ContentTemplate>
                    <asp:GridView ID="gdvDatos" runat="server" AutoGenerateColumns="False" 
                        onselectedindexchanged="gdvDatos_SelectedIndexChanged" PageSize="100" 
                        BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" 
                        CellPadding="3" ForeColor="Black" GridLines="Vertical" 
                        DataKeyNames="idEstado" AllowSorting="True" onsorting="gdvDatos_Sorting">                    
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                                SelectText="-" ShowSelectButton="True" />
                            <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" >
                                <ItemStyle Width="200px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" 
                                SortExpression="Nombre" >
                                <ItemStyle Width="600px" />
                            </asp:BoundField>
                            <asp:CheckBoxField AccessibleHeaderText="Activo" DataField="Activo" 
                                HeaderText="Activo" >
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
                </ContentTemplate>                    
            </asp:UpdatePanel>
            </asp:Panel>            
        </div>      
    </div>

    <div id="divFormulario">
        <asp:Panel ID="pnlFormulario" runat="server">        
            <div id="divFormularioSub">            
                 <asp:UpdatePanel ID="upnFormulario" runat="server" >
                    <ContentTemplate>
                        <asp:Panel ID="Panel1" runat="server" Height="150px" Width="805px">
                       
                        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                            Width="805px" Height="122px">

                            <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Datos De Estado">
                            
                            <ContentTemplate>

                            <asp:Label ID="lblClave" runat="server" CssClass="labelForms" Text="Clave:" 
                            Width="100px"></asp:Label>
                        <asp:TextBox ID="txbClave" runat="server" CssClass="textBoxForms" 
                            MaxLength="15" ValidationGroup="Estados"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvClave" runat="server" 
                            ControlToValidate="txbClave" ErrorMessage="Clave: Campo requerido" 
                            ValidationGroup="Estados">*</asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cmvClave" runat="server" 
                            ErrorMessage="Clave: ya existe" ControlToValidate="txbClave" Text="*" 
                            ValidationGroup="Estados" onservervalidate="cmvClave_ServerValidate"></asp:CustomValidator>                
                  <br />
                        <asp:Label ID="lblNombre" runat="server" Text="Nombre:" CssClass="labelForms" 
                            Width="100px"></asp:Label>
                        <asp:TextBox ID="txbNombre" runat="server" CssClass="textBoxForms" 
                            MaxLength="50" ValidationGroup="Estados"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvNombre" runat="server" 
                            ControlToValidate="txbNombre" ErrorMessage="Nombre: Campo requerido" 
                            ValidationGroup="Estados">*</asp:RequiredFieldValidator>
             <br />
                                               <asp:CheckBox ID="ckbActivo" runat="server" Text="Activo:"
                            Width="188px" />
                
                            </ContentTemplate>
                            </asp:TabPanel>
                        </asp:TabContainer>
                         </asp:Panel>
                    
                    </ContentTemplate>            
                </asp:UpdatePanel>
           </div>
        </asp:Panel>
    </div>        
    <br />
    <div>
        <asp:UpdatePanel ID="pnlAvisos" runat="server">
            <ContentTemplate>
                &nbsp;&nbsp;
&nbsp;                <asp:Label id="lblAviso" runat="server" Text=""></asp:Label>
                <br />
                &nbsp;&nbsp;
&nbsp; <asp:Label id="lblAviso2" runat="server" Text=""></asp:Label>                
            </ContentTemplate>            
        </asp:UpdatePanel>
    </div>    
</asp:Content>
