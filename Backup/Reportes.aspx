<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="Medicuri.Reportes" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="FiltroReportes" TagName="Filtro" Src="~/FiltroReportes.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>
    <%--<div id="divSeleccion">
            <asp:Panel ID="pnlSeleccion" runat="server" GroupingText="Seleccion de reportes">
            <asp:ListBox ID="lsbSeleccion" runat="server" Height="60px" Width="384px">
                <asp:ListItem Value="0">Medicamentos prescritos</asp:ListItem>
                <asp:ListItem Value="1">Medicamentos prescritos por localidad</asp:ListItem>
                <asp:ListItem Value="2">Medicamentos con mayor movimiento</asp:ListItem>
                <asp:ListItem Value="3">Medicamentos prescritos por médico</asp:ListItem>
                <asp:ListItem Value="4">Consumos por medicamento</asp:ListItem>
                <asp:ListItem Value="5">Consumos de medicamento por farmacia</asp:ListItem>
                <asp:ListItem Value="6">Consumos de medicamento por requisición hospitalaria</asp:ListItem>
                <asp:ListItem Value="7">Niveles de inventario</asp:ListItem>
                <asp:ListItem Value="8">Medicamento caduco</asp:ListItem>
                <asp:ListItem Value="9">Recetas por paciente</asp:ListItem>
                <asp:ListItem Value="10">Recetas por requisición hospitalaria</asp:ListItem>
                <asp:ListItem Value="11">Recetas por consumo</asp:ListItem>
            </asp:ListBox>        
            <br />
                <asp:Label ID="lblTitulo" runat="server" Text="Mostrar el reporte con el siguiente título:"></asp:Label>
                <asp:TextBox ID="txbTitulo" runat="server" MaxLength="255" Width="130px"></asp:TextBox>
            <br />
            <asp:Button ID="btnGenerar" runat="server" Text="Generar" 
                onclick="btnGenerar_Click" />    
        </asp:Panel>    
    </div>   --%>
    <div style="margin-left:60px">
        <asp:Panel ID="pnlFiltroReportes" runat="server">
            <FiltroReportes:Filtro runat="server" ID="frReportes" />
        </asp:Panel>
    </div>
</asp:Content>
