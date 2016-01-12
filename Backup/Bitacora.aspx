<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazBitacora.Master" AutoEventWireup="true" CodeBehind="Bitacora.aspx.cs" Inherits="Medicuri.Bitacora" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" namespace="System.Web.UI.WebControls" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
    <script runat="server">
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ToolkitScriptManager>    

    



    <div id="divCatalogo" class="divCatalogo">
        <asp:Panel ID="pnlCatalogo" runat="server" ScrollBars="Auto" Height="350px" 
            Width="845px">
        <div id="divCatalogoUpload" class="divSeleccionable">
            <asp:UpdatePanel ID="pnlCatalogoUpload" runat="server">
            <ContentTemplate>
                <asp:FileUpload ID="fupRespaldo" runat="server" Width="250px" />
            </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <br />
        <br />            
        <div id="divCatalogoSub" class="divCatalogoSub">        
            <asp:UpdatePanel ID="pnlCatalogoSub" runat="server">                    
                <ContentTemplate>
                    <asp:GridView ID="gdvDatos" runat="server" 
                        AutoGenerateColumns="False" 
                        EmptyDataText="No existen registros que cumplan con lo indicado" 
                        PageSize="100" BackColor="White" BorderColor="#999999" BorderStyle="Solid" 
                        BorderWidth="1px" CellPadding="3" ForeColor="Black" GridLines="Vertical" 
                        onselectedindexchanged="gdvDatos_SelectedIndexChanged" Width="845px" 
                        AllowPaging="True" AllowSorting="True" 
                        onpageindexchanging="gdvDatos_PageIndexChanging" onsorting="gdvDatos_Sorting" >                    
                        <AlternatingRowStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:BoundField DataField="FechaEntradaSrv" 
                                HeaderText="Fecha de Entrada del Servidor" 
                                SortExpression="FechaEntradaSrv" >
                            <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FechaEntradaCte" 
                                HeaderText="Fecha de Entrada del Cliente" Visible="False" 
                                SortExpression="FechaEntradaCte" />
                            <asp:BoundField DataField="Modulo" HeaderText="Modulo" SortExpression="Modulo" >
                            <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Usuario" HeaderText="Usuario" 
                                SortExpression="Usuario" >
                            <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" >
                            <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Accion" HeaderText="Accion" SortExpression="Accion" >
                            <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" 
                                SortExpression="Descripcion" >
                            <ItemStyle Width="400px" />
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
        </div>
        </asp:Panel>        
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
