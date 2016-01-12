<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="LineasDeCredito.aspx.cs" Inherits="Medicuri.LineasCredito" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>
<%@ Register TagPrefix="FiltroReportes" TagName="Filtro" Src="~/FiltroReportes.ascx" %>
<%-- Este es del header --%>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>


<%-- Este es del body o work area --%>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="True">

    </asp:ToolkitScriptManager>

    <div id="divCatalogo">
        <asp:Panel ID="pnlCatalogo" runat="server" Height="350px" ScrollBars="Auto">
            <asp:UpdatePanel ID="upnCatalogo" runat="server">
                <ContentTemplate>           
                    <asp:GridView ID="dgvDatos" runat="server" AutoGenerateColumns="False" 
                        DataKeyNames="idLineaCredito" 
                        onselectedindexchanged="dgvDatos_SelectedIndexChanged" BackColor="White" 
                        BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                        ForeColor="Black" GridLines="Vertical" Width="845px" AllowPaging="True" 
                        AllowSorting="True" onpageindexchanging="dgvDatos_PageIndexChanging" 
                        onsorting="dgvDatos_Sorting" PageSize="100">
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                                SelectText="-" ShowSelectButton="True" />
                            <asp:BoundField DataField="Clave" HeaderText="Clave" SortExpression="Clave" />
                            <asp:BoundField DataField="InstitucionEmisora" HeaderText="Institucion Emisora" 
                                SortExpression="InstitucionEmisora" >
                                <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FuenteCuenta" HeaderText="Cuenta Fuente" 
                                SortExpression="FuenteCuenta" >
                                <ItemStyle Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NumeroCuenta" HeaderText="Numero De Cuenta" 
                                SortExpression="NumeroCuenta" >
                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Monto" HeaderText="Monto" SortExpression="Monto" 
                                DataFormatString="{0:C}" >
                            <ItemStyle HorizontalAlign="Right" Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaMinistracion" HeaderText="Fecha Ministracion" 
                                SortExpression="FechaMinistracion" DataFormatString="{0:dd/MM/yyyy}" >
                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaVencimiento" HeaderText="Fecha Vencimiento" 
                                SortExpression="FechaVencimiento" DataFormatString="{0:dd/MM/yyyy}" >
                            <ItemStyle HorizontalAlign="Center" Width="150px" />
                            </asp:BoundField>
                            <asp:CheckBoxField DataField="Activo" HeaderText="Activo" 
                                SortExpression="Activo" />
                            <asp:BoundField DataField="idLineaCredito" HeaderText="idLineaCredito" 
                                ReadOnly="True" SortExpression="idLineaCredito" Visible="False" />
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

    <div id="Formulario">
        <asp:UpdatePanel ID="pnlFormulario" runat="server">
            <ContentTemplate>


                <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                    Height="285px" Width="840px">
                    <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Datos Generales">
                        <HeaderTemplate>
                            &nbsp;Datos Generales
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server" GroupingText="Datos Generales">
                            
                            <asp:Label ID="Label8" runat="server" Text="Clave:" CssClass="labelForms" 
                                Width="90px"></asp:Label>
                            <asp:TextBox
                                ID="txbClave" runat="server" Width="130px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvClave" runat="server" 
                                ControlToValidate="txbClave" ErrorMessage="Clave: Campo Requerido" 
                                ForeColor="Red" ValidationGroup="LineaCredito">*</asp:RequiredFieldValidator>
                            <asp:Label ID="Label7" runat="server" CssClass="labelForms" 
                                Text="Institución Emisora:" Width="90px"></asp:Label>
                            <asp:TextBox ID="txbInstitucion" runat="server" CssClass="textBoxForms" 
                                    Width="130px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvInstitucion" runat="server" 
                                ControlToValidate="txbInstitucion" ErrorMessage="Institución: Campo Requerido" 
                                ValidationGroup="LineaCredito" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:Label ID="Label2" runat="server" CssClass="labelForms" 
                                Text="Fuente:" Width="90px"></asp:Label>
                            <asp:TextBox ID="txbFuente" runat="server" CssClass="textBoxForms" Width="130px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvFuente" runat="server" 
                                ControlToValidate="txbFuente" ErrorMessage="Fuente: Campo Requerido" 
                                ValidationGroup="LineaCredito" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="Label3" runat="server" CssClass="labelForms" Text="Cuenta N°:" 
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txbCuenta" runat="server" CssClass="textBoxForms" Width="130px"></asp:TextBox>
                            <asp:Label ID="Label4" runat="server" CssClass="labelForms" Text="Monto:" 
                                Width="95px"></asp:Label>
                            <asp:TextBox ID="txbMonto" runat="server" CssClass="textBoxForms" Width="135px" 
                                ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvMonto" runat="server" 
                                ControlToValidate="txbMonto" ErrorMessage="Monto: Campo Requerido" 
                                ValidationGroup="LineaCredito" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <asp:Label ID="Label5" runat="server" CssClass="labelForms" Text="Fecha Ministración:" 
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txbFecha" runat="server" CssClass="textBoxForms" Width="130px"></asp:TextBox>
                            <asp:CalendarExtender ID="txbFecha_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="txbFecha">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="rfvMinistracion" runat="server" 
                                ControlToValidate="txbFecha" ErrorMessage="Ministración: Campo Requerido" 
                                ValidationGroup="LineaCredito" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:Label ID="Label6" runat="server" CssClass="labelForms" Text="Vence:" 
                                Width="90px"></asp:Label>
                            <asp:TextBox ID="txbVence" runat="server" CssClass="textBoxForms" Width="130px"></asp:TextBox>
                            <asp:CalendarExtender ID="txbVence_CalendarExtender" runat="server" 
                                Enabled="True" TargetControlID="txbVence">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="rfvVence" runat="server" 
                                ControlToValidate="txbVence" ErrorMessage="Vence: Campo Requerido" 
                                ValidationGroup="LineaCredito" ForeColor="Red">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" />
                            <br />
                            
                            <br />
                            &nbsp;
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Datos Opcionales">
                        <ContentTemplate>
                                <asp:Panel ID="Panel4" runat="server" GroupingText="Campos Alfabeticos">
                                    <asp:Label ID="lblAlfanumerico1" runat="server" Text="Campo 1:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo1" runat="server" MaxLength="25"></asp:TextBox>

                                    <asp:Label ID="lblAlfanumerico2" runat="server" Text="Campo 2:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo2" runat="server" MaxLength="25"></asp:TextBox>
                                    
                                    <asp:Label ID="lblAlfanumerico3" runat="server" Text="Campo 3:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo3" runat="server" MaxLength="25"></asp:TextBox>                                    
                                    <br />               
                                    <br />                     
                                    <asp:Label ID="lblAlfanumerico4" runat="server" Text="Campo 4:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo4" runat="server" MaxLength="25"></asp:TextBox>
                                    
                                    <asp:Label ID="lblAlfanumerico5" runat="server" Text="Campo 5:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo5" runat="server" MaxLength="25"></asp:TextBox>
                                </asp:Panel>
                                
                                <asp:Panel ID="Panel5" runat="server" GroupingText="Campos Numericos">
                                    
                                    <asp:Label ID="lblEntero1" runat="server" Text="Campo 6:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo6" runat="server"></asp:TextBox>
                                    
                                    <asp:Label ID="lblEntero2" runat="server" Text="Campo 7:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo7" runat="server"></asp:TextBox>
                                    <asp:Label ID="lblEntero3" runat="server" Text="Campo 8:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo8" runat="server"></asp:TextBox>
                                    <br />
                                </asp:Panel>
                                
                                <asp:Panel ID="Panel6" runat="server" GroupingText="Campos Decimales">
                                                                 
                                    <asp:Label ID="lblDecimal1" runat="server" Text="Campo 9:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo9" runat="server"></asp:TextBox>
                                    
                                    <asp:Label ID="lblDecimal2" runat="server" Text="Campo 10:" CssClass="labelForms" 
                                        Width="90px"></asp:Label>
                                    <asp:TextBox ID="txbCampo10" runat="server"></asp:TextBox>
                               </asp:Panel>
                          </ContentTemplate>
                    </asp:TabPanel>
                </asp:TabContainer>

                <asp:Label ID="lblAviso" runat="server"></asp:Label>
                            <br />
                            <asp:Label ID="lblAviso2" runat="server"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <br />
                            <asp:Label ID="lblAviso3" runat="server" ForeColor="Red"></asp:Label>
                            <br />
                            <asp:ValidationSummary ID="vlsGrupoValidaciones" runat="server" Height="126px" 
                                ValidationGroup="LineaCredito" ForeColor="Red" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
   <%-- <div>
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
    </div>    --%>
    <div style="margin-left:60px">
        <asp:Panel ID="pnlFiltroReportes" runat="server">
            <FiltroReportes:Filtro runat="server" ID="frReportes" />
        </asp:Panel>
    </div>

</asp:Content>
