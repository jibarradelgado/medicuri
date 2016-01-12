<%@ Page Title="" Language="C#" MasterPageFile="~/InterfazCatalogo.Master" AutoEventWireup="true" CodeBehind="Tipos.aspx.cs" Inherits="Medicuri.Tipos" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%-- Este es del header --%>
<asp:Content ID="Content1" ContentPlaceHolderID="contentPlaceHolderHeader" runat="server">
</asp:Content>

<%-- Este es del body o work area --%>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolderBody" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" 
        EnableScriptGlobalization="True">
    </asp:ToolkitScriptManager>

     <div id="Listado">
         <asp:Panel ID="pnlCatalogo" runat="server" Height="350px" ScrollBars="Auto">
            <asp:UpdatePanel ID="upnCatalogo" runat="server">
            <ContentTemplate>

                <asp:GridView ID="dgvDatos" runat="server" AutoGenerateColumns="False" 
                    DataKeyNames="idTipo" 
                    onselectedindexchanged="dgvDatos_SelectedIndexChanged" BackColor="White" 
                    BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                    ForeColor="Black" GridLines="Vertical" Width="845px" AllowPaging="True" 
                    AllowSorting="True" onpageindexchanging="dgvDatos_PageIndexChanging" 
                    onsorting="dgvDatos_Sorting" PageSize="100" >
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/Icons/right_16.png" 
                        SelectText="-" ShowSelectButton="True" >
                            <HeaderStyle Width="30px" />
                        </asp:CommandField>
                        <asp:BoundField DataField="idTipo" HeaderText="idTipo" ReadOnly="True" 
                            SortExpression="idTipo" Visible="False" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" 
                            SortExpression="Nombre" >
                            <ItemStyle Width="150px" />
                        </asp:BoundField>
                        <asp:CheckBoxField DataField="Almacenes" HeaderText="Almacenes" 
                            SortExpression="Almacenes" >
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:CheckBoxField>
                        <asp:CheckBoxField DataField="Clientes" HeaderText="Clientes" 
                            SortExpression="Clientes" >
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:CheckBoxField>
                        <asp:CheckBoxField DataField="Productos" HeaderText="Productos" 
                            SortExpression="Productos" >
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:CheckBoxField>
                        <asp:CheckBoxField DataField="Proveedores" HeaderText="Proveedores" 
                            SortExpression="Proveedores" >
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:CheckBoxField>
                        <asp:CheckBoxField DataField="Vendedores" HeaderText="Vendedores" 
                            SortExpression="Vendedores" >
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:CheckBoxField>
                        <asp:CheckBoxField DataField="Recetas" HeaderText="Recetas" 
                            SortExpression="Recetas" >
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:CheckBoxField>
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" 
                            SortExpression="Activo" >
                            <ItemStyle Width="50px" HorizontalAlign="Center" VerticalAlign="Middle" />
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

    <div id="Formulario">
        <asp:UpdatePanel ID="pnlFormulario" runat="server">
            <ContentTemplate>
                <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" 
                    Height="275px" Width="840px" >
                    <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Datos de Tipo">
                        <ContentTemplate>
                            <asp:Panel ID="Panel2" runat="server">
                            </asp:Panel>
                            <asp:Panel ID="Panel1" runat="server" GroupingText="Datos Generales" 
                                Height="53px">
                                <asp:Label ID="Label1" runat="server" Text="Tipo:" CssClass="labelForms" 
                                    Width="50px"></asp:Label>
                                <asp:TextBox ID="txbTipo" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvTipo" runat="server" 
                                    ControlToValidate="txbTipo" ErrorMessage="Tipo: Campo Requerido" 
                                    ForeColor="Red" ValidationGroup="Tipos">*</asp:RequiredFieldValidator>
                                <br />
                              
                            </asp:Panel>
                            <br />

                             <asp:Panel ID="Panel3" runat="server" GroupingText="Modulos" >
                                 <asp:CheckBox ID="chkAlmacenes" runat="server" Text="Almacenes" />
                                 <br />
                                 <asp:CheckBox ID="chkClientes" runat="server" Text="Clientes" />
                                 <br />
                                 <asp:CheckBox ID="chkProductos" runat="server" Text="Productos" />
                                 <br />
                                 <asp:CheckBox ID="chkProveedores" runat="server" Text="Proveedores" />
                                 <br />
                                 <asp:CheckBox ID="chkVendedores" runat="server" Text="Vendedores" />
                                 <br />
                                 <asp:CheckBox ID="chkRecetas" runat="server" Text="Recetas" />
                                 <br />
                                 <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" />
                                 <br />
                                
                                 
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                    
                 
                </asp:TabContainer>
               
                 <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" 
                                     ValidationGroup="Tipos" />
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
