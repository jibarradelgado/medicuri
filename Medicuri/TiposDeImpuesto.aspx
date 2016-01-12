<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="TiposDeImpuesto.aspx.cs" Inherits="Medicuri.TiposIva" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="TiposIva" HeaderText="Verifique los siguientes campos" />
    <div id="divCatalogo" class="divCatalogo">
        <div id="divCatalogoSub" class="divCatalogoSub">   
            <asp:Panel ID="pnlCatalogo" runat="server" Height="350px" ScrollBars="Auto">
                <asp:UpdatePanel ID="upnCatalogo" runat="server">                    
                <ContentTemplate>
                    <asp:GridView ID="gdvDatos" runat="server" AutoGenerateColumns="False" 
                        PageSize="100" onselectedindexchanged="gdvDatos_SelectedIndexChanged" 
                        DataKeyNames="idTipoIva" 
                        EmptyDataText="No existen tipos de iva registrados aun" BackColor="White" 
                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                        ForeColor="Black" GridLines="Vertical" Width="845px" AllowPaging="True" 
                        AllowSorting="True" onpageindexchanging="gdvDatos_PageIndexChanging" 
                        onsorting="gdvDatos_Sorting">                    
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                                SelectText="-" ShowSelectButton="True" />
                            <asp:BoundField DataField="Zona" HeaderText="Nombre" SortExpression="Zona" >
                                <ItemStyle Width="700px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Iva" HeaderText="Impuesto" 
                                SortExpression="Iva" >
                                <ItemStyle Width="100px" />
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
                </ContentTemplate>                    
            </asp:UpdatePanel>
            </asp:Panel>         
            
        </div>      
    </div>
    <div id="divFormulario" class="divFormulario">
        <asp:Panel ID="pnlFormulario" runat="server" GroupingText="Datos Generales"  Width="805px">        
            <div id="divFormularioSub" >
                <asp:UpdatePanel ID="pnlFormularioSub" runat="server">
                    <ContentTemplate>
                        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                            Width="805px">
                            <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Datos Impuesto">
                            <ContentTemplate>
                             <asp:Label ID="lblZona" runat="server" CssClass="labelForms" Text="Nombre:" 
                            Width="100px"></asp:Label>
                        <asp:TextBox ID="txbZona" runat="server" CssClass="textBoxForms" 
                            MaxLength="15" ValidationGroup="TiposIva"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvZona" runat="server" 
                            ControlToValidate="txbZona" ErrorMessage="Nombre: Campo requerido" 
                            ValidationGroup="TiposIva">*</asp:RequiredFieldValidator>                       
                        <asp:Label ID="lblIva" runat="server" Text="Impuesto:" CssClass="labelForms" 
                            Width="100px"></asp:Label>
                        <asp:TextBox ID="txbIva" runat="server" CssClass="textBoxForms" 
                            ValidationGroup="TiposIva"></asp:TextBox>                        
                        <asp:RequiredFieldValidator ID="rfvIva" runat="server" 
                            ControlToValidate="txbIva" ErrorMessage="Impuesto: Campo requerido" 
                            ValidationGroup="TiposIva">*</asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator runat="server" 
                            ErrorMessage="Impuesto: debe ser un número" ID="revIva" ControlToValidate="txbIva" 
                            ValidationExpression="^\d*[0-9](|.\d*[0-9]|,\d*[0-9])?$" 
                            ValidationGroup="TiposIva">*</asp:RegularExpressionValidator>
                        &nbsp&nbsp&nbsp&nbsp&nbsp
                        <asp:CheckBox ID="ckbActivo" runat="server" Text="Activo" />

                            </ContentTemplate>
                            </asp:TabPanel>
                        </asp:TabContainer>
                       
                   
                    
                    </ContentTemplate>            
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
    </div>
    <%--<div>
        <asp:Panel ID="pnlReportes" runat="server" Width="845px" Height="440px" ScrollBars="Auto">        
            <asp:Panel ID="pnlBotones" runat="server">
                <asp:Button ID="btnPdf" runat="server" Text="Exportar a PDF" 
                    onclick="btnPdf_Click" />
                &nbsp&nbsp&nbsp&nbsp
                <asp:Button ID="btnExcel" runat="server" Text="Exportar a Excel" 
                    onclick="btnExcel_Click" />
                &nbsp&nbsp&nbsp&nbsp
                <asp:Button ID="btnCrystal" runat="server" Text="Exportar a Crystal Reports" 
                    onclick="btnCrystal_Click" />
            </asp:Panel>
            <asp:Panel ID="pnlReporte" runat="server">
                <asp:UpdatePanel ID="upnReportes" runat="server">
                <ContentTemplate>  
                <CR:CrystalReportViewer ID="crvReporte" runat="server" AutoDataBind="true" 
                    HasCrystalLogo="False" HasExportButton="False" HasToggleGroupTreeButton="True" 
                    onnavigate="crvReporte_Navigate" 
                    onsearch="crvReporte_Search" onviewzoom="crvReporte_ViewZoom" 
                    ondrill="crvReporte_Drill" HasPrintButton="False" 
                    ondatabinding="crvReporte_DataBinding" 
                    ondrilldownsubreport="crvReporte_DrillDownSubreport"/>
                </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>        
        </asp:Panel>
    </div>--%>
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
