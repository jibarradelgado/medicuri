<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FiltroReportes.ascx.cs" Inherits="Medicuri.FiltroReportes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Panel ID="pnlGeneral" runat="server">
    <asp:Panel ID="pnlOpcionesf" runat="server">
        <asp:ListBox ID="lsbSeleccionf" runat="server" Height="50px" Width="501px" 
            AutoPostBack="True" 
            onselectedindexchanged="lsbSeleccionf_SelectedIndexChanged"></asp:ListBox>
        <br />
            <asp:Label ID="lblTitulo" runat="server" Text="Mostrar el reporte con el siguiente título:"></asp:Label>
            <asp:TextBox ID="txbTitulo" runat="server" MaxLength="255" Width="257px" 
            Height="20px"></asp:TextBox>
        
    </asp:Panel>
    <asp:Panel ID="pnlFiltrof" runat="server">
        <asp:Panel ID="pnlFechaf" runat="server">
            <asp:Label ID="lblFechaf" runat="server" Text="Fecha" Width="90px"></asp:Label>
            <br />
            <asp:Label ID="lblFechaDesdef" runat="server" Text="Desde:" 
                CssClass="labelForms" Width="90px"></asp:Label>
            <asp:TextBox ID="txbFecha1f" runat="server" CssClass="textBoxForms" 
                Width="130px"></asp:TextBox>
            <asp:CalendarExtender ID="CalendarExtender1f" runat="server" TargetControlID="txbFecha1f" Format="dd/MM/yyyy">
            </asp:CalendarExtender>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblFechaHastaf" runat="server" Text="Hasta:" 
                CssClass="labelForms" Width="90px"></asp:Label>
            <asp:TextBox ID="txbFecha2f" runat="server" CssClass="textBoxForms" 
                Width="130px"></asp:TextBox>
            <asp:CalendarExtender ID="CalendarExtender2f" runat="server" TargetControlID="txbFecha2f" Format="dd/MM/yyyy">
            </asp:CalendarExtender>
        </asp:Panel>
        <asp:Panel ID="pnlClavef" runat="server">
            <asp:Label ID="lblClavef" runat="server" Text="Clave"></asp:Label>
            <br />
            <asp:Label ID="lblClaveDesdef" runat="server" Text="Desde:" 
                CssClass="labelForms" Width="90px"></asp:Label>
            <asp:TextBox ID="txbClave1f" runat="server" CssClass="textBoxForms" 
                Width="130px"></asp:TextBox>     
            <asp:AutoCompleteExtender ID="txbClave1f_AutoCompleteExtender" runat="server" 
                DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx"
                TargetControlID="txbClave1f" ServiceMethod="Nada" MinimumPrefixLength="1">
            </asp:AutoCompleteExtender>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblClaveHastaf" runat="server" Text="Hasta:" 
                CssClass="labelForms" Width="90px"></asp:Label>
            <asp:TextBox ID="txbClave2f" runat="server" CssClass="textBoxForms" 
                Width="130px"></asp:TextBox>            
            <asp:AutoCompleteExtender ID="txbClave2f_AutoCompleteExtender" runat="server" 
                DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx" 
                TargetControlID="txbClave2f" ServiceMethod="Nada" MinimumPrefixLength="1">
            </asp:AutoCompleteExtender>
        </asp:Panel>
        <asp:Panel ID="pnlLocalidadf" runat="server">
            <asp:Label ID="lblLocalidadf" runat="server" Text="Localidad" Width="90px"></asp:Label>
            <br />
            <asp:Label ID="lblEstadof" runat="server" Text="Estado:" CssClass="labelForms" 
                Width="90px"></asp:Label>
            <asp:ComboBox ID="cmbEstadof" runat="server" CssClass="comboBoxForms" 
                Width="110px" AutoPostBack="True" DataTextField="Nombre" 
                DataValueField="idEstado" 
                onselectedindexchanged="cmbEstadof_SelectedIndexChanged">
            </asp:ComboBox>
            &nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblMunicipiof" runat="server" Text="Municipio:" 
                CssClass="labelForms" Width="92px"></asp:Label>
            <asp:ComboBox ID="cmbMunicipiof" runat="server" CssClass="comboBoxForms" 
                Width="110px" AutoPostBack="True" DataTextField="Nombre" 
                DataValueField="idMunicipio" 
                onselectedindexchanged="cmbMunicipiof_SelectedIndexChanged">
            </asp:ComboBox>
            &nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblPoblacionf" runat="server" Text="Población:" 
                CssClass="labelForms" Width="90px"></asp:Label>
            <asp:ComboBox ID="cmbPoblacionf" runat="server" CssClass="comboBoxForms" 
                Width="110px" DataTextField="Nombre" DataValueField="idPoblacion">
            </asp:ComboBox>
            <br />
            <asp:RadioButtonList ID="rblFiltroLocalidad" runat="server" 
                RepeatDirection="Horizontal">
                <asp:ListItem Value="1">Estado</asp:ListItem>
                <asp:ListItem Value="2">Municipio</asp:ListItem>
                <asp:ListItem Value="3">Población</asp:ListItem>
                <asp:ListItem Value="4">No filtrar</asp:ListItem>
            </asp:RadioButtonList>
        </asp:Panel>
        <asp:Panel ID="pnlEstatusf" runat="server">
            <asp:Label ID="lblEstatusf" runat="server" Text="Estatus"></asp:Label>
            &nbsp;&nbsp;&nbsp;
            <asp:RadioButtonList ID="rblEstatusf" runat="server" 
                RepeatDirection="Horizontal">
                <asp:ListItem Value="1">Activo</asp:ListItem>
                <asp:ListItem Value="0">Suspendido</asp:ListItem>
                <asp:ListItem Value="2">Todos</asp:ListItem>
            </asp:RadioButtonList>
        </asp:Panel>
        <asp:Panel ID="pnlEstatusFacturacion" runat="server">
            <asp:Label ID="lblEstatusFacturacion" runat="server" Text="Estatus Facturación"></asp:Label>
            &nbsp;&nbsp;&nbsp;
            <asp:RadioButtonList ID="rblEstatusFacturacion" runat="server" 
                RepeatDirection="Horizontal">
                <asp:ListItem Value="3">Emitida</asp:ListItem>
                <asp:ListItem Value="4">Cobrada</asp:ListItem>
                <asp:ListItem Value="5">Cancelada</asp:ListItem>
                <asp:ListItem Value="0">Todos</asp:ListItem>
            </asp:RadioButtonList>
        </asp:Panel>
        <asp:Panel ID="pnlTipof" runat="server">
            <asp:Label ID="lblTipof" runat="server" Text="Tipo:" CssClass="labelForms" 
                Width="90px"></asp:Label>
            <asp:TextBox ID="txbTipof" runat="server" CssClass="textBoxForms" Width="130px"></asp:TextBox>            
            <asp:AutoCompleteExtender ID="txbTipo_AutoCompleteExtender" runat="server" 
                DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx" TargetControlID="txbTipof"
                ServiceMethod="Nada" MinimumPrefixLength="1">
            </asp:AutoCompleteExtender>
        </asp:Panel>
        <asp:Panel ID="pnlEspecialidadf" runat="server">
            <asp:Label ID="lblEspecialidadf" runat="server" Text="Especialidad:" 
                CssClass="labelForms" Width="90px"></asp:Label>
            <asp:TextBox ID="txbEspecialidadf" runat="server" CssClass="textBoxForms" 
                Width="130px"></asp:TextBox>
            <asp:AutoCompleteExtender ID="txbEspecialidadf_AutoCompleteExtender" 
                runat="server" DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx" 
                TargetControlID="txbEspecialidadf" ServiceMethod="RecuperarVendedoresEspecialidad" MinimumPrefixLength="1">
            </asp:AutoCompleteExtender>
        </asp:Panel>
        <asp:Panel ID="pnlVinculacionf" runat="server">
            <asp:Label ID="lblVinculacionf" runat="server" Text="Vinculación:" 
                CssClass="labelForms" Width="90px"></asp:Label>
            <asp:TextBox ID="txbVinculacionf" runat="server" CssClass="textBoxForms" 
                Width="130px"></asp:TextBox>
            <asp:AutoCompleteExtender ID="txbVinculacionf_AutoCompleteExtender" 
                runat="server" DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx" 
                TargetControlID="txbVinculacionf" ServiceMethod="RecuperarVendedoresVinculacion" MinimumPrefixLength="1">
            </asp:AutoCompleteExtender>
        </asp:Panel>
        <asp:Panel ID="pnlVendedoresf" runat="server">
            <asp:Label ID="lblVendedoresf" runat="server" Text="Vendedor:" 
                CssClass="labelForms" Width="90px"></asp:Label>
            <asp:TextBox ID="txbVendedoresf" runat="server" CssClass="textBoxForms" 
                Width="130px"></asp:TextBox>
            <asp:AutoCompleteExtender ID="txbVendedoresf_AutoCompleteExtender" 
                runat="server" DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx" 
                TargetControlID="txbVendedoresf" ServiceMethod="RecuperarClaveVendedores" MinimumPrefixLength="1">
            </asp:AutoCompleteExtender>
        </asp:Panel>
        <asp:Panel ID="pnlClientef" runat="server">
            <asp:Label ID="lblClientef" runat="server" Text="Cliente:" 
                CssClass="labelForms" Width="90px"></asp:Label>
            <asp:TextBox ID="txbClientef" runat="server" CssClass="textBoxForms" 
                Width="130px"></asp:TextBox>
            <asp:AutoCompleteExtender ID="txbClientef_AutoCompleteExtender" runat="server" 
                DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx" 
                TargetControlID="txbClientef" ServiceMethod="RecuperarClave1Cliente" MinimumPrefixLength="1">
            </asp:AutoCompleteExtender>
        </asp:Panel>
        <asp:Panel ID="pnlAlmacenf" runat="server">
            <asp:Label ID="lblAlmacenf" runat="server" Text="Almacen:" 
                CssClass="labelForms" Width="90px"></asp:Label>
            <asp:TextBox ID="txbAlmacenf" runat="server" CssClass="textBoxForms" 
                Width="130px"></asp:TextBox>
            <asp:AutoCompleteExtender ID="txbAlmacenf_AutoCompleteExtender" runat="server" 
                DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx" 
                TargetControlID="txbAlmacenf" ServiceMethod="RecuperarClaveAlmacenes" MinimumPrefixLength="1">
            </asp:AutoCompleteExtender>
        </asp:Panel>
        <asp:Panel ID="pnlSoloExistenciasf" runat="server">
            <asp:CheckBox ID="ckbSoloExistenciasf" runat="server" Text="Solo con existencias" />
        </asp:Panel>
        <asp:Panel ID="pnlProveedorf" runat="server">
            <asp:Label ID="lblProveedorf" runat="server" Text="Proveedor:" 
                CssClass="labelForms" Width="90px"></asp:Label>
            <asp:TextBox ID="txbProveedorf" runat="server" CssClass="textBoxForms" 
                Width="130px"></asp:TextBox>
            <asp:AutoCompleteExtender ID="txbProveedorf_AutoCompleteExtender" 
                runat="server" DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx" 
                TargetControlID="txbProveedorf" ServiceMethod="RecuperarClaveProveedores" MinimumPrefixLength="1">
            </asp:AutoCompleteExtender>
        </asp:Panel>
        <asp:Panel ID="pnlPedimentof" runat="server">
            <asp:Label ID="lblPedimentof" runat="server" Text="Pedimento:" 
                CssClass="labelForms" Width="90px"></asp:Label>
            <asp:TextBox ID="txbPedimentof" runat="server" CssClass="textBoxForms" 
                Width="130px"></asp:TextBox>
            <asp:AutoCompleteExtender ID="txbPedimentof_AutoCompleteExtender" 
                runat="server" DelimiterCharacters="" Enabled="True" ServicePath="BusquedasAsincronas.asmx" 
                TargetControlID="txbPedimentof" ServiceMethod="RecuperarPedimentos" MinimumPrefixLength="1">
            </asp:AutoCompleteExtender>
        </asp:Panel>
        <asp:Panel ID="pnlTipoMovimientosf" runat="server">
            <asp:Label ID="lblTipoMovimientosf" runat="server" Text="Tipo de movimiento:" 
                CssClass="labelForms" Width="90px"></asp:Label>
            <asp:DropDownList ID="ddlTipoMovimientosf" runat="server" 
                CssClass="comboBoxForms">
            </asp:DropDownList>
        </asp:Panel>
        <asp:Panel ID="pnlFechaCaducidadf" runat="server">
            <asp:Label ID="lblFechaCaducidadf" runat="server" Text="Fecha de Caducidad:" 
                CssClass="labelForms" Width="90px"></asp:Label>
            <asp:TextBox ID="txbFechaCaducidadf" runat="server" CssClass="textBoxForms" 
                Width="130px"></asp:TextBox>
            <asp:CalendarExtender ID="txbFechaCaducidadf_CalendarExtender" runat="server" 
                Enabled="True" TargetControlID="txbFechaCaducidadf">
            </asp:CalendarExtender>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="pnlOrden" runat="server">
        <asp:Label ID="lblOrdenarf" runat="server" Text="Ordenar por:"></asp:Label>
        <asp:Panel ID="pnlNoOrdenar" runat="server">
            <asp:RadioButton ID="rdbNoOrdenar" runat="server" GroupName="Orden" 
                Text="No ordenar" Width="200px"/>
        </asp:Panel>
        <asp:Panel ID="pnlClaveOrdenf" runat="server">
            <asp:RadioButton ID="rdbClaveOrdenf" runat="server" GroupName="Orden" 
                Text="Clave" Width="200px"/>
        </asp:Panel>
        <asp:Panel ID="pnlNombreOrdenf" runat="server">
            <asp:RadioButton ID="rdbNombreOrdenf" runat="server" GroupName="Orden" 
                Text="Nombre" Width="200px"/>    
        </asp:Panel>
        <asp:Panel ID="pnlTipoOrdenf" runat="server">
            <asp:RadioButton ID="rdbTipoOrdenf" runat="server" GroupName="Orden" 
                Text="Tipo" Width="200px"/>
        </asp:Panel>
        <asp:Panel ID="pnlEspecialidadOrdenf" runat="server">
            <asp:RadioButton ID="rdbEspecialidadOrdenf" runat="server" GroupName="Orden" 
                Text="Especialidad" Width="200px"/>
        </asp:Panel>
        <asp:Panel ID="pnlVinculacionOrdenf" runat="server">
            <asp:RadioButton ID="rdbVinculacionOrdenf" runat="server" GroupName="Orden" 
                Text="Vinculación" Width="200px"/>
        </asp:Panel>
        <asp:Panel ID="pnlDescripcionOrdenf" runat="server">
            <asp:RadioButton ID="rdbDescripcionOrdenf" runat="server" GroupName="Orden" 
                Text="Descripción" Width="200px"/>
        </asp:Panel>
        <asp:Panel ID="pnlExistenciasOrdenf" runat="server">
            <asp:RadioButton ID="rdbExistenciasOrdenf" runat="server" GroupName="Orden" 
                Text="Existencias" Width="200px"/>
        </asp:Panel>
        <asp:Panel ID="pnlProveedorOrdenf" runat="server">
            <asp:RadioButton ID="rdbProveedorOrdenf" runat="server" GroupName="Orden" 
                Text="Proveedor" Width="200px"/>
        </asp:Panel>
        <asp:Panel ID="pnlFechaUltimaEntradaOrdenf" runat="server">
            <asp:RadioButton ID="rdbFechaUltimaEntradaOrdenf" runat="server" 
                GroupName="Orden" Text="Fecha de última entrada" Width="200px"/>
        </asp:Panel>
        <asp:Panel ID="pnlUltimaEntradaOrdenf" runat="server">
            <asp:RadioButton ID="rdbUltimaEntradaOrdenf" runat="server" GroupName="Orden" 
                Text="Ultima entrada" Width="200px"/>
        </asp:Panel>
        <asp:Panel ID="pnlUltimaSalidaOrdenf" runat="server">
            <asp:RadioButton ID="rdbUltimaSalidaOrdenf" runat="server" GroupName="Orden" 
                Text="Ultima salida" Width="200px"/>
        </asp:Panel>
    </asp:Panel>
    <br />
        <asp:Button ID="btnGenerar" runat="server" Text="Generar" 
            onclick="btnGenerar_Click" />
</asp:Panel>
