<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecuperarContraseña.aspx.cs" Inherits="Medicuri.Recuperar_Contraseña" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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

    <%--Banner/Header--%>
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
                            <asp:Panel ID="pnlRecuperarPass" runat="server" 
                                GroupingText="Recuperar contraseña"
                                HorizontalAlign="Center" >
                                <asp:Label ID="lblNombre" runat="server" Text="Nombre de usuario:"></asp:Label>
                                <asp:TextBox ID="txbNombreUsuario" runat="server"></asp:TextBox>
                                 <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ErrorMessage="Nombre de usuario requerido" 
                                ValidationGroup="NombreVG" ControlToValidate="txbNombreUsuario" 
                                    ForeColor="Red">*</asp:RequiredFieldValidator>
                                <asp:Button ID="btnEnviar" runat="server" Text="Enviar" 
                                    ValidationGroup="NombreVG" onclick="btnEnviar_Click" />
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:ValidationSummary runat="server" ValidationGroup="NombreVG" ID="vlsGrupoValidaciones" ForeColor="Red">
                                        </asp:ValidationSummary>
                                        <br />
                                        <asp:Label ID="lblInfo" runat="server" Text="Su nueva contraseña se enviará al correo que tenga registrado"></asp:Label>
                                        <br />
                                        <asp:HyperLink ID="hlkRegresar" runat="server" NavigateUrl="~/Login.aspx">regresar</asp:HyperLink>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                            <asp:RoundedCornersExtender ID="RoundedCornersExtender1" runat="server" TargetControlID="pnlRecuperarPass" Corners="All">
                            </asp:RoundedCornersExtender>
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
