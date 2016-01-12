<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VistaReporte.aspx.cs" Inherits="Medicuri.VistaReporte" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
    <div>
        <div id="divBotones">
            <asp:Panel ID="pnlBotones" runat="server" GroupingText="Opciones de exportación">
                <asp:Button ID="btnPdf" runat="server" Text="Exportar a PDF" 
                    onclick="btnPdf_Click" />
                &nbsp&nbsp&nbsp&nbsp
                <asp:Button ID="btnExcel" runat="server" Text="Exportar a Excel" 
                    onclick="btnExcel_Click" />
                &nbsp&nbsp&nbsp&nbsp
                <asp:Button ID="btnCrystal" runat="server" Text="Exportar a Crystal Reports" 
                    onclick="btnCrystal_Click" />
            </asp:Panel>
        </div>
        <div id="divReporte">
            <asp:Panel ID="pnlReporte" runat="server" GroupingText="Vista del reporte"  ScrollBars="auto">
                <asp:UpdatePanel ID="upnReporte" runat="server">
                <ContentTemplate>
                    <CR:CrystalReportViewer ID="crvReporte" runat="server" AutoDataBind="true" 
                        HasCrystalLogo="False" HasExportButton="False" 
                        HasRefreshButton="False" HasToggleGroupTreeButton="True" 
                        onload="crvReporte_Load" onnavigate="crvReporte_Navigate" 
                        onsearch="crvReporte_Search" onviewzoom="crvReporte_ViewZoom" 
                        ondrill="crvReporte_Drill" PrintMode="ActiveX" 
                        ondatabinding="crvReporte_DataBinding" 
                        ondrilldownsubreport="crvReporte_DrillDownSubreport" Width="350px" />
                </ContentTemplate>
                </asp:UpdatePanel>                    
            </asp:Panel>
        </div>
    </div>
    </form>
</body>
</html>
