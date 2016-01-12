<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="CamposEditables.aspx.cs" Inherits="Medicuri.CamposEditables" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%-- Este es del header --%>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
   <%-- <style type="text/css">
        #Formulario
        {
            height: 271px;
        }
    </style>--%>
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
                    Height="257px" Width="845px" >
                    <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Almacenes">
                      <ContentTemplate>
                      <asp:Panel ID="Panel4" runat="server" GroupingText="Campos Alfabeticos" 
                                    Height="93px">
                                    <asp:Label ID="Label14" runat="server" Text="Campo 1:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbAlm1" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label15" runat="server" Text="Campo 2:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbAlm2" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label16" runat="server" Text="Campo 3:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbAlm3" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="Label17" runat="server" Text="Campo 4:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbAlm4" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label18" runat="server" Text="Campo 5:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbAlm5" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                              
                                <asp:Panel ID="Panel5" runat="server" GroupingText="Campos Numericos" 
                                    Height="75px">
                                    <asp:Label ID="Label28" runat="server" Text="Campo 6:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbAlm6" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label29" runat="server" Text="Campo 7:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbAlm7" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label21" runat="server" Text="Campo 8:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbAlm8" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                                <asp:Panel ID="Panel6" runat="server" GroupingText="Campos Decimales" 
                                    Height="63px">
                                    
                                    <asp:Label ID="Label22" runat="server" Text="Campo 9:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbAlm9" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label23" runat="server" Text="Campo 10:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbAlm10" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                      </ContentTemplate>
                    </asp:TabPanel>
                    
                    <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Clientes">
                        <ContentTemplate>
                      <asp:Panel ID="Panel1" runat="server" GroupingText="Campos Alfabeticos" 
                                    Height="93px" Width="845px">
                                    <asp:Label ID="Label61" runat="server" Text="Campo 1:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbCli1" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label2" runat="server" Text="Campo 2:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbCli2" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label3" runat="server" Text="Campo 3:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbCli3" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="Label4" runat="server" Text="Campo 4:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbCli4" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label5" runat="server" Text="Campo 5:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbCli5" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                              
                                <asp:Panel ID="Panel2" runat="server" GroupingText="Campos Numericos" 
                                    Height="75px">
                                    <asp:Label ID="Label6" runat="server" Text="Campo 6:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbCli6" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label7" runat="server" Text="Campo 7:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbCli7" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label8" runat="server" Text="Campo 8:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbCli8" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                                <asp:Panel ID="Panel3" runat="server" GroupingText="Campos Decimales" 
                                    Height="63px">
                                    
                                    <asp:Label ID="Label9" runat="server" Text="Campo 9:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbCli9" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label10" runat="server" Text="Campo 10:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbCli10" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                      </ContentTemplate>
                    </asp:TabPanel>
                      
                    <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Productos">
                     <ContentTemplate>
                      <asp:Panel ID="Panel7" runat="server" GroupingText="Campos Alfabeticos" 
                                    Height="93px">
                                    <asp:Label ID="Label11" runat="server" Text="Campo 1:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPro1" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label12" runat="server" Text="Campo 2:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPro2" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label13" runat="server" Text="Campo 3:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPro3" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="Label19" runat="server" Text="Campo 4:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPro4" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label20" runat="server" Text="Campo 5:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPro5" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                              
                                <asp:Panel ID="Panel8" runat="server" GroupingText="Campos Numericos" 
                                    Height="75px">
                                    <asp:Label ID="Label24" runat="server" Text="Campo 6:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPro6" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label25" runat="server" Text="Campo 7:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPro7" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                     <asp:Label ID="Label26" runat="server" Text="Campo 8:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPro8" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                                <asp:Panel ID="Panel9" runat="server" GroupingText="Campos Decimales" 
                                    Height="63px">
                                   
                                    <asp:Label ID="Label27" runat="server" Text="Campo 9:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPro9" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label30" runat="server" Text="Campo 10:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPro10" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                      </ContentTemplate>
                    </asp:TabPanel>
                     
                    <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="Proveedores">
                     <ContentTemplate>
                      <asp:Panel ID="Panel10" runat="server" GroupingText="Campos Alfabeticos" 
                                    Height="93px">
                                    <asp:Label ID="Label31" runat="server" Text="Campo 1:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPre1" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label32" runat="server" Text="Campo 2:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPre2" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label33" runat="server" Text="Campo 3:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPre3" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="Label34" runat="server" Text="Campo 4:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPre4" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label35" runat="server" Text="Campo 5:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPre5" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                              
                                <asp:Panel ID="Panel11" runat="server" GroupingText="Campos Numericos" 
                                    Height="75px">
                                    <asp:Label ID="Label36" runat="server" Text="Campo 6:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPre6" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label37" runat="server" Text="Campo 7:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPre7" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label38" runat="server" Text="Campo 8:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPre8" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                                <asp:Panel ID="Panel12" runat="server" GroupingText="Campos Decimales" 
                                    Height="63px">
                                    
                                    <asp:Label ID="Label39" runat="server" Text="Campo 9:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPre9" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label40" runat="server" Text="Campo 10:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbPre10" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                      </ContentTemplate>
                    </asp:TabPanel>
                     
                    <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="Usuarios">
                     <ContentTemplate>
                      <asp:Panel ID="Panel13" runat="server" GroupingText="Campos Alfabeticos" 
                                    Height="93px">
                                    <asp:Label ID="Label41" runat="server" Text="Campo 1:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbUsu1" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label42" runat="server" Text="Campo 2:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbUsu2" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label43" runat="server" Text="Campo 3:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbUsu3" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="Label44" runat="server" Text="Campo 4:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbUsu4" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label45" runat="server" Text="Campo 5:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbUsu5" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                              
                                <asp:Panel ID="Panel14" runat="server" GroupingText="Campos Numericos" 
                                    Height="75px">
                                    <asp:Label ID="Label46" runat="server" Text="Campo 6:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbUsu6" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label47" runat="server" Text="Campo 7:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbUsu7" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                     <asp:Label ID="Label48" runat="server" Text="Campo 8:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbUsu8" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                                <asp:Panel ID="Panel15" runat="server" GroupingText="Campos Decimales" 
                                    Height="63px">
                                   
                                    <asp:Label ID="Label49" runat="server" Text="Campo 9:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbUsu9" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label50" runat="server" Text="Campo 10:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbUsu10" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                      </ContentTemplate>
                    </asp:TabPanel>
                     
                    <asp:TabPanel ID="TabPanel6" runat="server" HeaderText="Vendedores">
                     <ContentTemplate>
                      <asp:Panel ID="Panel16" runat="server" GroupingText="Campos Alfabeticos" 
                                    Height="93px">
                                    <asp:Label ID="Label51" runat="server" Text="Campo 1:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbVen1" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label52" runat="server" Text="Campo 2:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbVen2" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label53" runat="server" Text="Campo 3:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbVen3" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="Label54" runat="server" Text="Campo 4:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbVen4" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label55" runat="server" Text="Campo 5:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbVen5" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                              
                                <asp:Panel ID="Panel17" runat="server" GroupingText="Campos Numericos" 
                                    Height="75px">
                                    <asp:Label ID="Label56" runat="server" Text="Campo 6:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbVen6" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label57" runat="server" Text="Campo 7:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbVen7" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label58" runat="server" Text="Campo 8:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                        <asp:TextBox ID="txbVen8" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                                  <asp:Panel ID="Panel18" runat="server" GroupingText="Campos Decimales" 
                                    Height="63px">
                                    
                                    <asp:Label ID="Label59" runat="server" Text="Campo 9:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbVen9" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label60" runat="server" Text="Campo 10:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbVen10" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                      </ContentTemplate>
                    </asp:TabPanel>
                
                    <asp:TabPanel ID="TabPanel7" runat="server" HeaderText="Líneas de Crédito">
                    <ContentTemplate>
                      <asp:Panel ID="Panel19" runat="server" GroupingText="Campos Alfabeticos" 
                                    Height="93px">
                                    <asp:Label ID="Label1" runat="server" Text="Campo 1:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbLin1" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label62" runat="server" Text="Campo 2:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbLin2" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label63" runat="server" Text="Campo 3:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbLin3" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="Label64" runat="server" Text="Campo 4:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbLin4" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label65" runat="server" Text="Campo 5:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbLin5" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                              
                                <asp:Panel ID="Panel20" runat="server" GroupingText="Campos Numericos" 
                                    Height="75px">
                                    <asp:Label ID="Label66" runat="server" Text="Campo 6:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbLin6" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label67" runat="server" Text="Campo 7:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbLin7" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label68" runat="server" Text="Campo 8:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                        <asp:TextBox ID="txbLin8" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                                  <asp:Panel ID="Panel21" runat="server" GroupingText="Campos Decimales" 
                                    Height="63px">
                                    
                                    <asp:Label ID="Label69" runat="server" Text="Campo 9:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbLin9" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                    <asp:Label ID="Label70" runat="server" Text="Campo 10:" CssClass="labelForms" 
                                        Width="100px"></asp:Label>
                                    <asp:TextBox ID="txbLin10" runat="server" MaxLength="25" Width="120px"></asp:TextBox>
                                </asp:Panel>
                      </ContentTemplate>
                    </asp:TabPanel>
                
                </asp:TabContainer>
            
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


    <div>
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
                <cr:crystalreportviewer ID="crvReporte" runat="server" AutoDataBind="true" 
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
    </div>    














</asp:Content>
