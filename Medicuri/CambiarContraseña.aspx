<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="CambiarContraseña.aspx.cs" Inherits="Medicuri.CambiarContraseña" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">

<div id="Formulario">
        
        <asp:UpdatePanel ID="pnlFormulario" runat="server">
          <ContentTemplate>
                                   
              <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
              </asp:ToolkitScriptManager>
                                   
              <asp:Panel ID="Panel1" runat="server" GroupingText="Cambiar Contraseña" 
                  Height="138px">
                  <asp:Label ID="Label5" runat="server" CssClass="labelForms" Text="Usuario:" 
                      Width="230px"></asp:Label>
                  <asp:TextBox ID="txbUsuario" runat="server"></asp:TextBox>
                  <br />
                  <asp:Label ID="Label4" runat="server" CssClass="labelForms" 
                      Text="Contraseña Anterior:" Width="230px"></asp:Label>
                  <asp:TextBox ID="txbAnterior" runat="server" TextMode="Password"></asp:TextBox>
                  <asp:RequiredFieldValidator ID="rfvAnterior" runat="server" 
                      ControlToValidate="txbAnterior" 
                      ErrorMessage="Contraseña Anterior: Campo Requerido" ForeColor="Red" 
                      ValidationGroup="CambioContra">*</asp:RequiredFieldValidator>
                  <br />
                  <asp:Label ID="Label2" runat="server" CssClass="labelForms" 
                      Text="Contraseña Nueva:" Width="230px"></asp:Label>
                  <asp:TextBox ID="txbNueva" runat="server" TextMode="Password"></asp:TextBox>
                  <asp:RequiredFieldValidator ID="rfvNueva" runat="server" 
                      ControlToValidate="txbNueva" ErrorMessage="Contraseña Nueva: Campo Requerido" 
                      ForeColor="Red" ValidationGroup="CambioContra">*</asp:RequiredFieldValidator>
                  <br />
                  <asp:Label ID="Label3" runat="server" CssClass="labelForms" 
                      Text="Confirmar Contraseña:" Width="230px"></asp:Label>
                  <asp:TextBox ID="txbConfirmacion" runat="server" TextMode="Password"></asp:TextBox>
                  <asp:RequiredFieldValidator ID="rfvConfirmar" runat="server" 
                      ErrorMessage="Confirmar Contraseña: Campo Requerido" 
                      ControlToValidate="txbConfirmacion" ForeColor="Red" 
                      ValidationGroup="CambioContra">*</asp:RequiredFieldValidator>
                  <asp:CompareValidator ID="cpComparacion" runat="server" 
                      ControlToCompare="txbNueva" ControlToValidate="txbConfirmacion" 
                      ErrorMessage="La confirmación de contraseña no es correcta" ForeColor="Red" 
                      ValidationGroup="CambioContra" ValueToCompare="txbNueva.text">*</asp:CompareValidator>
              </asp:Panel>
               <asp:Label ID="lblAviso" runat="server"></asp:Label>
               <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                  ValidationGroup="CambioContra" />
            </ContentTemplate>
          </asp:UpdatePanel>
</div>



</asp:Content>
