<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Medicuri.Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>.:Inicio de sesión:.</title>
    <link href="Css/Master.css" rel="stylesheet" type="text/css" />
    <link href="Css/StyleSheetCatalogos.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ((document.documentElement.clientHeight - 140) > 170) {
                var altura = (document.documentElement.clientHeight - 140) + 'px';
                $('.bg_Login').css('min-height', altura);
            }
            $(window).bind("resize", resizeWindow);
            function resizeWindow(e) {
                if ((document.documentElement.clientHeight - 140) > 170) {
                    var altura = (document.documentElement.clientHeight - 140) + 'px';
                    $('.bg_Login').css('min-height', altura);
                }
            }
        });
        
    
    </script>
    </head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div align="center">
        <div class="Contenedor">
            <div align="center">
			    <div class="Cuerpo">
                    <%--Banner/Header--%>
                    <div class="bg_Logo" align="center">
                        <div class="Logo">
                            <div id="header">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/logo.png" />
		                    </div>
                        </div>
                    </div>
       
                    <div align="center" style="width: 974px" class="bg_Login">
                        <br />
                        <asp:Panel ID="Panel1" runat="server">
                        </asp:Panel>
                        <table style="width: 27%; height: 150px;">
                            <tr>
                                <td align="center" bgcolor="#CCCCCC" colspan="2" class="style1">
                                    <asp:Label Font-Names="Calibri" ID="Label3" runat="server" 
                                        Text="Iniciar Sesión" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label Font-Names="Calibri" ID="LabelUsu" runat="server" Text="Nombre usuario:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txbUsername" runat="server" ToolTip="Usuario" Width="129px" 
                                        BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label Font-Names="Calibri" ID="LabelContra0" runat="server" Text="Contraseña:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txbPassword" runat="server" ToolTip="Contrasena" 
                                        TextMode="Password" Width="129px" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:Button ID="btnIniciar" runat="server" Height="26px" Text="Iniciar" 
                                        Width="50px" Font-Underline="False" onclick="btnIniciar_Click" />
                                    <asp:Button ID="cmdlimpiar" runat="server" Height="26px" 
                                        Text=" Limpiar datos " UseSubmitBehavior="False" 
                                        onclick="cmdlimpiar_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" bgcolor="#CCCCCC" colspan="2">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:Label Font-Names="Calibri" ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" bgcolor="#CCCCCC" colspan="2">
                                <asp:HyperLink ID="hlkRecuperarPass" runat="server" NavigateUrl="~/RecuperarContraseña.aspx">Recuperar contraseña</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                        <br />
                    </div>

                    <div align="center">
                    <div class="Cuerpo">
                        <div class="Pie" align="center">
                            2011
                        </div>
                    </div>
                </div> 
                </div>
            </div>      
        </div>
    </div>
    </form>
</body>
</html>
