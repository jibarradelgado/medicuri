using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Medicuri
{
    public partial class FiltroReportes : System.Web.UI.UserControl
    {
        const string cnsSinEstados = "Sin estados...", cnsSinMunicipios = "Sin municipios...", cnsSinPoblaciones = "Sin poblaciones..."; 
        MedNeg.Poblaciones.BlPoblaciones oblPoblaciones;
        MedNeg.Municipios.BlMunicipios oblMunicipios;
        MedNeg.Estados.BlEstados oblEstados;
        
        #region ActualizarSesiones
        
        /// <summary>
        /// Actualiza la variable de sesion "lstestadosalmacenes", la cual es una lista de los estados activos
        /// </summary>
        protected void ActualizarSesionEstados()
        {
            IQueryable<MedDAL.DAL.estados> iqrEstados = oblEstados.BuscarEnum();
            Session["lstestadosalmacenes"] = iqrEstados;
        }

        /// <summary>
        /// Actualiza la variable de sesion de Municipios dependiendo de los Estados existentes.
        /// </summary>        
        protected void ActualizarSesionMunicipios()
        {
            IQueryable<MedDAL.DAL.municipios> iqrMunicipios = oblMunicipios.BuscarEstados(int.Parse(cmbEstadof.SelectedValue));
            Session["lstmunicipiosalmacenes"] = iqrMunicipios;
        }

        /// <summary>
        /// Actualiza la variable de sesión de Poblaciones dependiendo de los Municipios existentes.
        /// </summary>        
        protected void ActualizarSesionPoblaciones()
        {
            IQueryable<MedDAL.DAL.poblaciones> iqrPoblaciones = oblPoblaciones.BuscarMunicipios(int.Parse(cmbMunicipiof.SelectedValue));
            Session["lstpoblacionesalmacenes"] = iqrPoblaciones;
        }
        
        #endregion

        #region CargarLimpiarControles
        /// <summary>
        /// Carga cmbEstado, cmbMunicipio, cmbPoblacion y cmbColonia
        /// </summary>
        /// <param name="bDatos"></param>
        protected void CargarEstados()
        {
            ActualizarSesionEstados();

            cmbEstadof.Items.Clear();
            cmbEstadof.DataSource = (IQueryable<MedDAL.DAL.estados>)Session["lstestadosalmacenes"];
            cmbEstadof.DataBind();

            if (cmbEstadof.Items.Count != 0)
            {                
                cmbEstadof.SelectedIndex = 0;    
                CargarMunicipios();
            }
            else
            {
                cmbEstadof.Items.Add(cnsSinEstados);
                cmbMunicipiof.ClearSelection();
                cmbMunicipiof.Items.Clear();
                cmbMunicipiof.Items.Add(cnsSinMunicipios);
                cmbPoblacionf.ClearSelection();
                cmbPoblacionf.Items.Clear();
                cmbPoblacionf.Items.Add(cnsSinPoblaciones);                
            }
        }
        /// <summary>
        /// Carga cmbMunicipio, cmbPoblacion y cmbColonia
        /// </summary>
        /// <param name="bDatos"></param>
        protected void CargarMunicipios()
        {
            ActualizarSesionMunicipios();

            cmbMunicipiof.Items.Clear();
            cmbMunicipiof.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstmunicipiosalmacenes"];
            cmbMunicipiof.DataBind();

            if (cmbMunicipiof.Items.Count != 0)
            {
                cmbMunicipiof.SelectedIndex = 0;
                CargarPoblaciones();
            }
            else
            {
                cmbMunicipiof.Items.Add(cnsSinMunicipios);
                cmbPoblacionf.ClearSelection();
                cmbPoblacionf.Items.Clear();
                cmbPoblacionf.Items.Add(cnsSinPoblaciones);
            }
        }
        /// <summary>
        /// Carga cmbPoblacion
        /// </summary>
        /// <param name="bDatos"></param>
        protected void CargarPoblaciones()
        {
            ActualizarSesionPoblaciones();

            cmbPoblacionf.Items.Clear();
            cmbPoblacionf.DataSource = (IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblacionesalmacenes"];
            cmbPoblacionf.DataBind();

            if (cmbPoblacionf.Items.Count != 0)
            {    
                cmbPoblacionf.SelectedIndex = 0;                        
            }
            else
            {
                cmbPoblacionf.Items.Add(cnsSinPoblaciones);
            }
        }
       

        /// <summary>
        /// Funcion que oculta o muestra los paneles necesarios para el filtro
        /// Agregar la funcionalidad de lo text box dependiendo del modulo
        /// </summary>
        /// <param name="bFecha"></param>
        /// <param name="bClave"></param>
        /// <param name="bLocalidad"></param>
        /// <param name="bEstatus"></param>
        /// <param name="bTipo"></param>
        /// <param name="bEspecialidad"></param>
        /// <param name="bVinculacion"></param>
        /// <param name="bVendedor"></param>
        /// <param name="bCliente"></param>
        /// <param name="bAlmacen"></param>
        /// <param name="bExistencias"></param>
        /// <param name="bProveedor"></param>
        /// <param name="bTipoMovimiento"></param>
        /// <param name="bFechaCaducidad"></param>
        /// <param name="bOrdenNombre"></param>
        /// <param name="bOrdenClave"></param>
        /// <param name="bOrdenTipo"></param>
        /// <param name="bOrdenEspecialidad"></param>
        /// <param name="bOrdenVinculacion"></param>
        /// <param name="bOrdenDescripción"></param>
        /// <param name="bOrdenExistencias"></param>
        /// <param name="bOrdenProveedor"></param>
        /// <param name="bOrdenFechaUltimaEntrada"></param>
        /// <param name="bOrdenUltimaEntrada"></param>
        /// <param name="bOrdenUltimaSalida"></param>
        /// <param name="bEstatusFacturacion"></param>
        public void CargarPaneles(string sModulo, bool bFecha, bool bClave, bool bLocalidad, bool bEstatus, bool bTipo, bool bEspecialidad, bool bVinculacion, bool bVendedor, bool bCliente, bool bAlmacen, bool bExistencias, bool bProveedor, bool bTipoMovimiento, bool bFechaCaducidad, bool bOrdenNombre, bool bOrdenClave, bool bOrdenTipo, bool bOrdenEspecialidad, bool bOrdenVinculacion, bool bOrdenDescripción, bool bOrdenExistencias, bool bOrdenProveedor, bool bOrdenFechaUltimaEntrada, bool bOrdenUltimaEntrada, bool bOrdenUltimaSalida,bool bEstatusFacturacion, bool bPedimento)
        {
            #region Asignaciones
            pnlFechaf.Visible = bFecha;
            pnlClavef.Visible = bClave;
            pnlLocalidadf.Visible = bLocalidad;
            pnlEstatusf.Visible = bEstatus;
            pnlTipof.Visible = bTipo;
            pnlEspecialidadf.Visible = bEspecialidad;
            pnlVinculacionf.Visible = bVinculacion;
            pnlVendedoresf.Visible = bVendedor;
            pnlClientef.Visible = bCliente;
            pnlAlmacenf.Visible = bAlmacen;
            pnlSoloExistenciasf.Visible = bExistencias;
            pnlProveedorf.Visible = bProveedor;
            pnlTipoMovimientosf.Visible = bTipoMovimiento;
            pnlFechaCaducidadf.Visible = bFechaCaducidad;
            pnlNombreOrdenf.Visible = bOrdenNombre;
            pnlClaveOrdenf.Visible = bOrdenClave;
            pnlTipoOrdenf.Visible = bOrdenTipo;
            pnlEspecialidadOrdenf.Visible = bOrdenEspecialidad;
            pnlVinculacionOrdenf.Visible = bOrdenVinculacion;
            pnlDescripcionOrdenf.Visible = bOrdenDescripción;
            pnlExistenciasOrdenf.Visible = bOrdenExistencias;
            pnlProveedorOrdenf.Visible = bOrdenProveedor;
            pnlFechaUltimaEntradaOrdenf.Visible = bOrdenFechaUltimaEntrada;
            pnlUltimaEntradaOrdenf.Visible = bOrdenUltimaEntrada;
            pnlUltimaSalidaOrdenf.Visible = bOrdenUltimaSalida;
            //GT 0217  05-Jul-2011 Crear un panel nuevo para los estatus de facturacion
            pnlEstatusFacturacion.Visible = bEstatusFacturacion;
            pnlPedimentof.Visible = bPedimento;
            #endregion

            if (pnlLocalidadf.Visible)
            {
                CargarEstados();
            }

            switch (sModulo)
            { 
                case "Almacenes":
                    #region Almacenes
                    if (pnlClavef.Visible)
                    {
                        txbClave1f_AutoCompleteExtender.ServiceMethod = txbClave2f_AutoCompleteExtender.ServiceMethod = "RecuperarClaveAlmacenes";                         
                    }
                    if (pnlTipof.Visible)
                    {
                        txbTipo_AutoCompleteExtender.ServiceMethod = "RecuperarTiposAlmacenes";
                    }
                    #endregion
                    break;
                case "Proveedores":
                    #region Proveedores
                    if (pnlClavef.Visible)
                    {
                        txbClave1f_AutoCompleteExtender.ServiceMethod = txbClave2f_AutoCompleteExtender.ServiceMethod = "RecuperarClaveProveedores";
                    }
                    if (pnlTipof.Visible)
                    {
                        txbTipo_AutoCompleteExtender.ServiceMethod = "RecuperarTiposProveedores";
                    }
                    #endregion
                    break;
                case "Vendedores":
                    #region Vendedores
                    if (pnlClavef.Visible)
                    {
                        txbClave1f_AutoCompleteExtender.ServiceMethod = txbClave2f_AutoCompleteExtender.ServiceMethod = "RecuperarClaveVendedores";
                    }
                    if (pnlTipof.Visible)
                    {
                        txbTipo_AutoCompleteExtender.ServiceMethod = "RecuperarTiposVendedores";
                    }
                    if (pnlEspecialidadf.Visible)
                    {
                        txbEspecialidadf_AutoCompleteExtender.ServiceMethod = "RecuperarVendedoresEspecialidad";
                    }
                    if (pnlVinculacionf.Visible)
                    {
                        txbVinculacionf_AutoCompleteExtender.ServiceMethod = "RecuperarVendedoresVinculacion";
                    }
                    #endregion
                    break;
                case "Clientes":
                    #region Clientes
                    if (pnlClavef.Visible)
                    {
                        txbClave1f_AutoCompleteExtender.ServiceMethod = txbClave2f_AutoCompleteExtender.ServiceMethod = "RecuperarClave1Cliente";
                    }
                    if (pnlTipof.Visible)
                    {
                        txbTipo_AutoCompleteExtender.ServiceMethod = "RecuperarTiposClientes";
                    }                    
                    #endregion
                    break;
                case "Productos":
                case "Inventarios":
                    #region Productos
                    if (pnlClavef.Visible)
                    {
                        txbClave1f_AutoCompleteExtender.ServiceMethod = txbClave2f_AutoCompleteExtender.ServiceMethod = "RecuperarClave2Producto";
                    }
                    if (pnlTipof.Visible)
                    {
                        txbTipo_AutoCompleteExtender.ServiceMethod = "RecuperarTiposProductos";
                    }    
                    #endregion
                    break;
                case "Facturacion":
                    #region Pendientes por surtir
                    //Productos Clave 1
                    if (pnlClavef.Visible)
                    {
                        txbClave1f_AutoCompleteExtender.ServiceMethod = txbClave2f_AutoCompleteExtender.ServiceMethod = "RecuperarClave1Producto";
                    }
                    //Almacenes Clave
                    if (pnlAlmacenf.Visible)
                    {
                        txbAlmacenf_AutoCompleteExtender.ServiceMethod = "RecuperarClaveAlmacenes";
                    }
                    #endregion
                    break;
                case "Facturacion1":
                    #region Resumen detallado de facturas
                    //Folio Facturas
                    if (pnlClavef.Visible)
                    {
                        txbClave1f_AutoCompleteExtender.ServiceMethod = txbClave2f_AutoCompleteExtender.ServiceMethod = "RecuperarFolioFacturas";
                    }
                    // Clientes
                    if (pnlClientef.Visible)
                    {
                        txbClientef_AutoCompleteExtender.ServiceMethod = "RecuperarClaveVendedores";
                    }
                    //Vededor de la tabla facturas(en este caso es los usuarios que emiten la factura)
                    if (pnlVendedoresf.Visible)
                    {
                        txbVendedoresf_AutoCompleteExtender.ServiceMethod = "RecuperarFacturasVendedores";
                    }
                    #endregion
                    break;
                case "Facturacion2":
                    #region Resumen general de facturas
                    //Folio Facturas
                    if (pnlClavef.Visible)
                    {
                        txbClave1f_AutoCompleteExtender.ServiceMethod = txbClave2f_AutoCompleteExtender.ServiceMethod = "RecuperarFolioFacturas";
                    }
                    // Clientes
                    if (pnlClientef.Visible)
                    {
                        txbClientef_AutoCompleteExtender.ServiceMethod = "RecuperarClaveVendedores";
                    }
                    //Vededor de la tabla facturas(en este caso es los usuarios que emiten la factura)
                    if (pnlVendedoresf.Visible)
                    {
                        txbVendedoresf_AutoCompleteExtender.ServiceMethod = "RecuperarFacturasVendedores";
                    }
                    #endregion
                    break;        
                case "LineasCredito":
                    #region LineasCredito
                    if (pnlClavef.Visible)
                    {
                        txbClave1f_AutoCompleteExtender.ServiceMethod = txbClave2f_AutoCompleteExtender.ServiceMethod = "RecuperarClaveLineasCredito";
                    }
                    #endregion
                    break;
                case "Movimientos":
                    #region Movimientos
                    
                    #endregion
                    break;
                case "CuentasPorCobrar":
                    #region Cuentas por cobrar
                    if (pnlClavef.Visible)
                    {
                        txbClave1f_AutoCompleteExtender.ServiceMethod = txbClave2f_AutoCompleteExtender.ServiceMethod = "RecuperarClave1Cliente";
                    }
                    #endregion
                    break;
            }
            //Falta cargar tipos de movimientos
        }
        /// <summary>
        /// Pone todos los paneles en false
        /// </summary>
        public void LimpiarPaneles()
        {
            CargarPaneles("", false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false,false, false);
            btnGenerar.Visible = false;
            pnlOrden.Visible = false;
        }
        #endregion

        #region LLenar DataSet
        protected DataSet LlenarDataSet(string sConsulta, string sNombreConnectionString, DataSet dsDataSet, string sTabla)
        {
            SqlConnection sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[sNombreConnectionString].ConnectionString);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = new SqlCommand(sConsulta, sqlConnection);
            sqlAdapter.Fill(dsDataSet, sTabla);
            return dsDataSet;
        }
        #endregion
        
        #region Agregar Filtros
        /// <summary>
        /// Filtrar por clave
        /// </summary>
        /// <param name="sRecordSelection"></param>
        /// <param name="sTablaCampo"></param>
        protected void AgregarFiltroClave(ref string sRecordSelection, string sTablaCampo)
        {
            if (sRecordSelection == "")
            {
                if (txbClave1f.Text != "")
                {
                    sRecordSelection = "{" + sTablaCampo +"}>='" + txbClave1f.Text + "'";
                    if (txbClave2f.Text != "")
                    {
                        sRecordSelection += " and {" + sTablaCampo + "}<='" + txbClave2f.Text + "'";
                    }
                }
                else if (txbClave1f.Text == "" && txbClave2f.Text != "")
                {
                    sRecordSelection = "{"+ sTablaCampo + "}<='" + txbClave2f.Text + "'";
                }
            }
            else
            {
                if (txbClave1f.Text != "")
                {
                    sRecordSelection += " and {" + sTablaCampo + "}>='" + txbClave1f.Text + "'";
                    if (txbClave2f.Text != "")
                    {
                        sRecordSelection += " and {" + sTablaCampo + "}<='" + txbClave2f.Text + "'";
                    }
                }
                else if (txbClave1f.Text == "" && txbClave2f.Text != "")
                {
                    sRecordSelection += " and {" + sTablaCampo + "}<='" + txbClave2f.Text + "'";
                }
            }
        }
        protected void AgregarFiltroTipo(ref string sRecordSelection)
        {            
            if (txbTipof.Text != "")
            {
                if (sRecordSelection == "")
                {
                    sRecordSelection = "{tipos.Nombre} like '" + txbTipof.Text + "'";
                }
                else
                {
                    sRecordSelection += " and {tipos.Nombre} like '" + txbTipof.Text + "'";
                }
            }                        
        }
        protected void AgregarFiltroLocalidad(ref string sRecordSelection)
        {
            if (rblFiltroLocalidad.SelectedValue != "4")
            {
                if (rblFiltroLocalidad.SelectedValue == "1")
                {
                    if (sRecordSelection == "")
                    {
                        sRecordSelection = "{estados.idEstado} = " + cmbEstadof.SelectedValue + "";
                    }
                    else
                    {
                        sRecordSelection += " and {estados.idEstado} = " + cmbEstadof.SelectedValue + "";
                    }
                }
                if (rblFiltroLocalidad.SelectedValue == "2")
                {
                    if (sRecordSelection == "")
                    {
                        sRecordSelection = "{municipios.idMunicipio} = " + cmbMunicipiof.SelectedValue + "";
                    }
                    else
                    {
                        sRecordSelection += " and {municipios.idMunicipio} = " + cmbMunicipiof.SelectedValue + "";
                    }
                }
                if (rblFiltroLocalidad.SelectedValue == "3")
                {
                    if (sRecordSelection == "")
                    {
                        sRecordSelection = "{poblaciones.idPoblacion} = " + cmbPoblacionf.SelectedValue + "";
                    }
                    else
                    {
                        sRecordSelection += " and {poblaciones.idPoblacion} = " + cmbPoblacionf.SelectedValue + "";
                    }
                }
            }
        }
        protected void AgregarFiltroEstatus(ref string sRecordSelection, string sTablaCampo)
        {
            if (rblEstatusf.SelectedValue == "1")
            {
                if (sRecordSelection == "")
                {
                    sRecordSelection = "{" + sTablaCampo + "} = " + true;
                }
                else
                {
                    sRecordSelection += " and {" + sTablaCampo + "} = " + true;
                }
            }
            else if (rblEstatusf.SelectedValue == "0")
            {
                if (sRecordSelection == "")
                {
                    sRecordSelection = "{" + sTablaCampo + "} = " + false;
                }
                else
                {
                    sRecordSelection += " and {" + sTablaCampo + "} = " + false;
                }
            }
        }
        protected void AgregarFiltroEspecialidad(ref string sRecordSelection)
        {
            if (txbEspecialidadf.Text != "")
            {
                if (sRecordSelection == "")
                {
                    sRecordSelection = "{vendedores_especialidad.Especialidad} like '" + txbEspecialidadf.Text + "'";
                }
                else 
                {
                    sRecordSelection += " and {vendedores_especialidad.Especialidad} like '" + txbEspecialidadf.Text + "'";
                }
            }
        }
        protected void AgregarFiltroVinculacion(ref string sRecordSelection)
        {
            if (txbVinculacionf.Text != "")
            {
                if (sRecordSelection == "")
                {
                    sRecordSelection = "{vendedores_vinculacion.Vinculacion} like '" + txbVinculacionf.Text + "'";
                }
                else
                {
                    sRecordSelection += " and {vendedores_vinculacion.Vinculacion} like '" + txbVinculacionf.Text + "'";
                }
            }
        }
        /// <summary>
        /// Filtrar por fecha
        /// </summary>
        /// <param name="sRecordSelection"></param>
        protected void AgregarFiltroFecha(ref string sRecordSelection,string sTablaCampo)
        {

            if (sRecordSelection == "")
            {
                if (txbFecha1f.Text != "")
                {
                    sRecordSelection = "Date({" + sTablaCampo + "})>= Date('" + txbFecha1f.Text + "')";
                    if (txbFecha2f.Text != "")
                    {
                        sRecordSelection += " and Date({" + sTablaCampo + "})<= Date('" + txbFecha2f.Text + "')";
                    }
                }
                else if (txbFecha1f.Text == "" && txbFecha2f.Text != "")
                {
                    sRecordSelection = "Date({" + sTablaCampo + "})<=Date('" + txbFecha2f.Text + "')";
                }
            }
            else
            {
                if (txbFecha1f.Text != "")
                {
                    sRecordSelection += " and Date({" + sTablaCampo + "})>=Date('" + txbFecha1f.Text + "')";
                    if (txbFecha2f.Text != "")
                    {
                        sRecordSelection += " and Date({" + sTablaCampo + "})<=Date('" + txbFecha2f.Text + "')";
                    }
                }
                else if (txbFecha1f.Text == "" && txbFecha2f.Text != "")
                {
                    sRecordSelection += " and Date({" + sTablaCampo + "})<=Date('" + txbFecha2f.Text + "')";
                }
            }
        } 
        protected void AgregarFiltroAlmacen(ref string sRecordSelection)
        {
            if (txbAlmacenf.Text != "")
            {
                if (sRecordSelection == "")
                {
                    sRecordSelection = "{almacenes.Clave}='" + txbAlmacenf.Text + "'";
                }
                else
                {
                    sRecordSelection += " and {almacenes.Clave}='" + txbAlmacenf.Text + "'";
                }

                if (Session["reportdocument"].ToString() == "~\\rptReportes\\Productos\\rptStockMinimo.rpt")
                {
                    Session["reportdocument"] = "~\\rptReportes\\Productos\\rptStockMinimoAlmacen.rpt";
                }
            }
            else
            {
                if (Session["reportdocument"].ToString() == "~\\rptReportes\\Productos\\rptStockMinimoAlmacen.rpt")
                {
                    Session["reportdocument"] = "~\\rptReportes\\Productos\\rptStockMinimo.rpt";
                }
            }
        }
        /// <summary>
        /// Agregar filtro cliente Clave1
        /// </summary>
        /// <param name="sRecordSelection"></param>
        protected void AgregarFiltroCliente(ref string sRecordSelection)
        {
            if ( txbClientef.Text!="")
            {
                if (sRecordSelection == "")
                {
                    sRecordSelection = "{clientes.Clave1}='" + txbClientef.Text + "'";
                }
                else
                {
                    sRecordSelection += " and {clientes.Clave1}='" + txbClientef.Text + "'";
                }
            }
        }
        protected void AgregarFiltroProveedor(ref string sRecordSelection)
        {
            if (txbProveedorf.Text != "")
            {
                if (sRecordSelection == "")
                {
                    sRecordSelection = "{proveedores.Clave}='" + txbProveedorf.Text + "'";
                }
                else
                {
                    sRecordSelection += " and {proveedores.Clave}='" + txbProveedorf.Text + "'";
                }
            }
        }
        /// <summary>
        /// Agregar filtro vendedor pero de facturas
        /// </summary>
        /// <param name="sRecordSelection"></param>
        protected void AgregarFiltroVendedorFacturas(ref string sRecordSelection)
        {
            string[] strArrParametros = null;
            string str = txbVendedoresf.Text;
            char[] chrSeparador = { ' ' };
            strArrParametros = str.Split(chrSeparador);

            if (txbVendedoresf.Text != "")
            {
                if (sRecordSelection == "")
                {
                    sRecordSelection = "{facturas.vendedor}='" + txbVendedoresf.Text + "'";
                }
                else
                {
                    sRecordSelection += " and {facturas.vendedor}='" + txbVendedoresf.Text + "'";
                }
            }
        }
        protected void AgregarFiltroSoloExistencias(ref string sRecordSelection)
        {
            if (ckbSoloExistenciasf.Checked)
            {
                if (sRecordSelection == "")
                {
                    sRecordSelection = "{@fTotalExistencia} > 0";
                }
                else
                {
                    sRecordSelection = " and {@fTotalExistencia} > 0";
                }
            }
        }

       /// <summary>
       /// Para seleccionar facturas mediante un estatus especifico
       /// </summary>
       /// <param name="sRecordSelection">Cadena del record seletion formula</param>
       /// <param name="sEstatusFactura">Estatus por el cual se deben de recuperar las facturas</param>
        protected void AgregarFiltroFacturaEstatus(ref string sRecordSelection,string sEstatusFactura)
        {
            if (sRecordSelection == "")
            {
                sRecordSelection = "{facturas.estatus}='" + sEstatusFactura+"'";
            }
            else
            {
                sRecordSelection += " and {facturas.estatus}='" + sEstatusFactura+"'";
            }

        }

        protected void AgregarFiltroPedimentos(ref string sRecordSelection, string sTabla)
        {
            if (txbPedimentof.Text != "")
            {
                if (sRecordSelection == "")
                {
                    sRecordSelection = "{" + sTabla + "}='" + txbPedimentof.Text + "'";
                }
                else
                {
                    sRecordSelection += " and {" + sTabla + "}='" + txbPedimentof.Text + "'";
                }
            }
        }

        #endregion

        #region Seleccion
        protected string ObtenerRecordSelection(string sReporte)
        {
            string sRecordSelection = ""; 
            switch (sReporte) 
            { 
                case "~\\rptReportes\\Almacenes\\rptAlmacenes.rpt":
                    AgregarFiltroClave(ref sRecordSelection, "almacenes.Clave");
                    AgregarFiltroTipo(ref sRecordSelection);
                    AgregarFiltroLocalidad(ref sRecordSelection);
                    AgregarFiltroEstatus(ref sRecordSelection, "almacenes.Activo");
                    break;
                case "~\\rptReportes\\Almacenes\\rptDistribucionExistencias.rpt":
                    AgregarFiltroClave(ref sRecordSelection, "almacenes.Clave");
                    AgregarFiltroTipo(ref sRecordSelection);
                    AgregarFiltroAlmacen(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\Proveedores\\rptProveedores.rpt":
                    AgregarFiltroFecha(ref sRecordSelection, "proveedores.Fecha");
                    AgregarFiltroClave(ref sRecordSelection, "proveedores.Clave");
                    AgregarFiltroTipo(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\Vendedores\\rptVendedores.rpt":
                    AgregarFiltroFecha(ref sRecordSelection, "vendedores.FechaAlta");
                    AgregarFiltroClave(ref sRecordSelection, "vendedores.Clave");
                    AgregarFiltroTipo(ref sRecordSelection);
                    AgregarFiltroLocalidad(ref sRecordSelection);
                    AgregarFiltroEstatus(ref sRecordSelection, "vendedores.Activo");
                    AgregarFiltroEspecialidad(ref sRecordSelection);
                    AgregarFiltroVinculacion(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\Facturacion\\rptPendientesPorSurtir.rpt":
                    AgregarFiltroClave(ref sRecordSelection, "productos.clave2");
                    AgregarFiltroFecha(ref sRecordSelection, "recetas_partida_faltantes.fecha");
                    AgregarFiltroAlmacen(ref sRecordSelection);   
                    break;
                case "~\\rptReportes\\Clientes\\rptClientes.rpt":
                    AgregarFiltroFecha(ref sRecordSelection, "clientes.FechaAlta");
                    AgregarFiltroClave(ref sRecordSelection, "clientes.Clave1");
                    AgregarFiltroTipo(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\Clientes\\rptEstadoCuentaGeneral.rpt":
                    AgregarFiltroClave(ref sRecordSelection, "clientes.Clave1");
                    AgregarFiltroTipo(ref sRecordSelection);
                    AgregarFiltroLocalidad(ref sRecordSelection);
                    AgregarFiltroFecha(ref sRecordSelection, "facturas.FechaAplicacion");
                    break;
                case "~\\rptReportes\\Clientes\\rptPorConcepto.rpt":
                    AgregarFiltroClave(ref sRecordSelection, "clientes.Clave1");
                    AgregarFiltroTipo(ref sRecordSelection);
                    AgregarFiltroLocalidad(ref sRecordSelection);
                    AgregarFiltroFecha(ref sRecordSelection, "facturas.FechaAplicacion");
                    break;
                case "~\\rptReportes\\Productos\\rptProductos.rpt":
                    AgregarFiltroAlmacen(ref sRecordSelection);
                    AgregarFiltroClave(ref sRecordSelection, "productos.Clave2");
                    AgregarFiltroTipo(ref sRecordSelection);
                    AgregarFiltroEstatus(ref sRecordSelection, "productos.Activo");                    
                    break;
                case "~\\rptReportes\\Productos\\rptListaPrecios.rpt":
                    AgregarFiltroClave(ref sRecordSelection, "productos.Clave2");
                    AgregarFiltroTipo(ref sRecordSelection);
                    AgregarFiltroEstatus(ref sRecordSelection, "productos.Activo");
                    //AgregarFiltroSoloExistencias(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\Productos\\rptStockMinimo.rpt":
                case "~\\rptReportes\\Productos\\rptStockMaximo.rpt":
                    AgregarFiltroClave(ref sRecordSelection, "productos.Clave2");
                    AgregarFiltroTipo(ref sRecordSelection);
                    AgregarFiltroProveedor(ref sRecordSelection);
                    AgregarFiltroAlmacen(ref sRecordSelection);
                    break;
                    //GT 0249
                case "~\\rptReportes\\Productos\\rptProductosLista.rpt":
                    AgregarFiltroClave(ref sRecordSelection, "productos.Clave2");
                    AgregarFiltroTipo(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\Facturacion\\rptDetalladoDeFacturas.rpt":
                    AgregarFiltroFecha(ref sRecordSelection, "facturas.fecha");
                    AgregarFiltroClave(ref sRecordSelection, "facturas.folio");
                    AgregarFiltroCliente(ref sRecordSelection);
                    AgregarFiltroVendedorFacturas(ref sRecordSelection); 
                    AgregarFiltroLocalidad(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\Facturacion\\rptResumenGeneralDeFacturas.rpt":
                    AgregarFiltroFecha(ref sRecordSelection, "facturas.fecha");
                    AgregarFiltroClave(ref sRecordSelection, "facturas.folio");
                    AgregarFiltroCliente(ref sRecordSelection);
                    AgregarFiltroVendedorFacturas(ref sRecordSelection); 
                    AgregarFiltroLocalidad(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\Inventarios\\rptProductosCaducos.rpt":
                    AgregarFiltroFecha(ref sRecordSelection, "productos_almacen.FechaCaducidad");
                    AgregarFiltroClave(ref sRecordSelection, "productos.Clave2");
                    AgregarFiltroTipo(ref sRecordSelection);
                    AgregarFiltroAlmacen(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\Inventarios\\rptComprasProveedor.rpt":
                    AgregarFiltroFecha(ref sRecordSelection, "inventario.Fecha");
                    AgregarFiltroClave(ref sRecordSelection, "productos.Clave2");
                    AgregarFiltroAlmacen(ref sRecordSelection);
                    AgregarFiltroProveedor(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\Inventarios\\rptVentasPeriodo.rpt":
                    AgregarFiltroFecha(ref sRecordSelection, "facturas.Fecha");
                    AgregarFiltroClave(ref sRecordSelection, "productos.Clave2");
                    AgregarFiltroAlmacen(ref sRecordSelection);
                    AgregarFiltroTipo(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\Inventarios\\rptVentasCliente.rpt":
                    AgregarFiltroFecha(ref sRecordSelection, "facturas.Fecha");
                    AgregarFiltroClave(ref sRecordSelection, "productos.Clave2");
                    AgregarFiltroAlmacen(ref sRecordSelection);
                    AgregarFiltroCliente(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\Inventarios\\rptNumerosSerie.rpt":
                case "~\\rptReportes\\Inventarios\\rptPedimentosAduanalesLotes.rpt":
                    AgregarFiltroClave(ref sRecordSelection, "productos.Clave2");
                    AgregarFiltroTipo(ref sRecordSelection);
                    AgregarFiltroAlmacen(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\Inventarios\\rptHistorialExistencias.rpt":
                    AgregarFiltroFecha(ref sRecordSelection, "inventario.Fecha");
                    AgregarFiltroClave(ref sRecordSelection, "productos.Clave2");
                    AgregarFiltroAlmacen(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\LineasCredito\\rptAcumuladoLineaCredito.rpt":
                    AgregarFiltroClave(ref sRecordSelection, "lineas_creditos.Clave");
                    AgregarFiltroFecha(ref sRecordSelection, "FacturacionDeRecetas.Fecha");
                    break;
                case "~\\rptReportes\\Facturacion\\rptAntiguedadDeSaldos.rpt":
                    AgregarFiltroFacturaEstatus(ref sRecordSelection, "3");
                    AgregarFiltroFecha(ref sRecordSelection, "facturas.Fecha");
                    AgregarFiltroLocalidad(ref sRecordSelection);
                    AgregarFiltroAlmacen(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\Movimientos\\rptMovimientos.rpt":
                    AgregarFiltroFecha(ref sRecordSelection, "vistamovimientos.Fecha");
                    AgregarFiltroPedimentos(ref sRecordSelection, "vistamovimientos.Pedimento");
                    break;
                case "~\\rptReportes\\Facturacion\\rptCuentasPorCobrar.rpt":
                    AgregarFiltroCliente(ref sRecordSelection);
                    AgregarFiltroFecha(ref sRecordSelection, "facturas.fecha");
                    if (pnlEstatusFacturacion.Visible == true)
                    {
                        if (rblEstatusFacturacion.SelectedValue == "3")
                            AgregarFiltroFacturaEstatus(ref sRecordSelection, "3");
                        if(rblEstatusFacturacion.SelectedValue == "4")
                            AgregarFiltroFacturaEstatus(ref sRecordSelection, "4");
                        if (rblEstatusFacturacion.SelectedValue == "5")
                            AgregarFiltroFacturaEstatus(ref sRecordSelection, "5");
                    }

                    break;
                case "~\\rptReportes\\rptRecetas.rpt":
                case "~\\rptReportes\\rptRecetasLocalidad.rpt":
                case "~\\rptReportes\\rptRecetasLineaCredito.rpt":
                case "~\\rptReportes\\rptRecetasDiagnostico.rpt":
                case "~\\rptReportes\\rptMedicamentosMasRecetados.rpt":
                case "~\\rptReportes\\rptMedicamentosPrescritosMedico.rpt":
                case "~\\rptReportes\\rptConsumosMedicamento.rpt":
                case "~\\rptReportes\\rptConsumosMedicamentoFarmacia.rpt":
                case "~\\rptReportes\\rptConsumosMedicamentoRequisicion.rpt":
                case "~\\rptReportes\\rptRecetasPaciente.rpt":
                case "~\\rptReportes\\rptRecetasRequisicion.rpt":
                case "~\\rptReportes\\rptRecetasConsumo.rpt":
                case "~\\rptReportes\\rptCaducos.rpt":
                    AgregarFiltroFecha(ref sRecordSelection, "recetas.Fecha");
                    AgregarFiltroAlmacen(ref sRecordSelection);
                    break;
                case "~\\rptReportes\\rptInventarios.rpt":  
                case "~\\rptReportes\\rptInventariosLotes.rpt":
                    AgregarFiltroAlmacen(ref sRecordSelection);
                    break;
            }
            return sRecordSelection;
        }
        #endregion

        #region Campos Orden
        protected string ObtenerCampoOrden(string sReporte)
        {
            string sOrden = "";
            switch (sReporte)
            {
                case "~\\rptReportes\\Almacenes\\rptAlmacenes.rpt":
                    #region rptAlmacenes
                    if (rdbClaveOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "almacenes";
                        sOrden = "Clave";
                    }
                    else if (rdbNombreOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "almacenes";
                        sOrden = "Nombre";
                    }
                    else if (rdbTipoOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "tipos";
                        sOrden = "Nombre";
                    }
                    #endregion
                    break;
                case "~\\rptReportes\\Almacenes\\rptDistribucionExistencias.rpt":
                    #region rptDistribucionExistencias
                    if (rdbClaveOrdenf.Checked)
                    {
                        Session["reportdocument"] = "~\\rptReportes\\Almacenes\\rptDistribucionExistenciasClave.rpt";
                        Session["tablaordenar"] = "productos";
                        sOrden = "Clave2";
                    }
                    else if (rdbNombreOrdenf.Checked)
                    {
                        Session["reportdocument"] = "~\\rptReportes\\Almacenes\\rptDistribucionExistenciasNombre.rpt";
                        Session["tablaordenar"] = "productos";
                        sOrden = "Nombre";
                    }
                    else if (rdbNoOrdenar.Checked)
                    {
                        Session["reportdocument"] = "~\\rptReportes\\Almacenes\\rptDistribucionExistencias.rpt";
                    }
                    #endregion
                    break;
                case "~\\rptReportes\\Proveedores\\rptProveedores.rpt":
                    #region rptProveedores
                    if (rdbClaveOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "proveedores";
                        sOrden = "Clave";
                    }
                    else if (rdbNombreOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "proveedores";
                        sOrden = "Nombre";
                    }
                    else if (rdbTipoOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "tipos";
                        sOrden = "Nombre";
                    }
                    #endregion
                    break;
                case "~\\rptReportes\\Vendedores\\rptVendedores.rpt":
                    #region rptVendedores
                    if (rdbClaveOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "vendedores";
                        sOrden = "Clave";
                    }
                    else if (rdbNombreOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "vendedores";
                        sOrden = "Nombre";
                    }
                    else if (rdbTipoOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "tipos";
                        sOrden = "Nombre";
                    }
                    else if (rdbEspecialidadOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "vendedores_especialidad";
                        sOrden = "Especialidad";
                    }
                    else if (rdbVinculacionOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "vendedores_vinculacion";
                        sOrden = "Vinculacion";
                    }
                    #endregion
                    break;
                case "~\\rptReportes\\Clientes\\rptClientes.rpt":
                    #region rptClientes
                    if (rdbClaveOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "clientes";
                        sOrden = "Clave1";
                    }
                    else if (rdbNombreOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "clientes";
                        sOrden = "Nombre";
                    }
                    else if (rdbTipoOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "tipos";
                        sOrden = "Nombre";
                    }
                    #endregion
                    break;                
                case "~\\rptReportes\\Clientes\\rptEstadoCuentaGeneral.rpt":
                    #region rptEstadoCuentaGeneral
                    if (rdbClaveOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "clientes";
                        sOrden = "Clave1";
                    }
                    else if (rdbNombreOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "clientes";
                        sOrden = "Nombre";
                    }
                    else if (rdbTipoOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "tipos";
                        sOrden = "Nombre";
                    }
                    #endregion
                    break;
                case "~\\rptReportes\\Clientes\\rptPorConcepto.rpt":
                    #region rptPorConcepto
                    if (rdbClaveOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "clientes";
                        sOrden = "Clave1";
                    }
                    else if (rdbNombreOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "clientes";
                        sOrden = "Nombre";
                    }
                    else if (rdbTipoOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "tipos";
                        sOrden = "Nombre";
                    }
                    #endregion
                    break;
                case "~\\rptReportes\\Productos\\rptProductos.rpt":
                case "~\\rptReportes\\Inventarios\\rptProductosCaducos.rpt":
                case "~\\rptReportes\\Inventarios\\rptVentasPeriodo.rpt":
                case "~\\rptReportes\\Inventarios\\rptNumerosSerie.rpt":
                case "~\\rptReportes\\Inventarios\\rptPedimentosAduanalesLotes.rpt":
                    #region rptProductos
                    if (rdbClaveOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "productos";
                        sOrden = "Clave2";
                    }
                    else if (rdbNombreOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "productos";
                        sOrden = "Nombre";
                    }
                    else if (rdbTipoOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "tipos";
                        sOrden = "Nombre";
                    }
                    #endregion
                    break;
                case "~\\rptReportes\\Productos\\rptListaPrecios.rpt":
                    #region rptListaPrecios
                    if (rdbClaveOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "productos";
                        sOrden = "Clave2";
                    }
                    else if (rdbNombreOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "productos";
                        sOrden = "Nombre";
                    }
                    else if (rdbTipoOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "tipos";
                        sOrden = "Nombre";
                    }
                    #endregion
                    break;
                case "~\\rptReportes\\Productos\\rptStockMinimo.rpt":                
                    #region rptStockMinimo
                    if (rdbClaveOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "productos";
                        sOrden = "Clave2";
                    }
                    else if (rdbNombreOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "productos";
                        sOrden = "Nombre";
                    }
                    else if (rdbProveedorOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "proveedores";
                        sOrden = "Clave";
                    }
                    else if (rdbExistenciasOrdenf.Checked && (Session["reportdocument"].ToString() == "~\\rptReportes\\Productos\\rptStockMinimoAlmacen.rpt"))
                    {
                        Session["tablaordenar"] = "productos_almacen";
                        sOrden = "Cantidad";
                    }
                    #endregion
                    break;
                case "~\\rptReportes\\Productos\\rptStockMaximo.rpt":
                    #region rptStockMaximo
                    if (rdbClaveOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "productos";
                        sOrden = "Clave2";
                    }
                    else if (rdbNombreOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "productos";
                        sOrden = "Nombre";
                    }
                    else if (rdbProveedorOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "proveedores";
                        sOrden = "Clave";
                    }
                    else if (rdbExistenciasOrdenf.Checked)
                    {
                        Session["tablaordenar"] = "productos_almacen";
                        sOrden = "Cantidad";
                    }
                    #endregion
                    break;    
            }
            return sOrden;
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            oblEstados = new MedNeg.Estados.BlEstados();
            oblPoblaciones = new MedNeg.Poblaciones.BlPoblaciones();
            oblMunicipios = new MedNeg.Municipios.BlMunicipios();

            MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
            try
            {
                MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
                oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());

                if ((bool)oUsuario.FiltradoActivado)
                {
                    txbAlmacenf_AutoCompleteExtender.Enabled = false;
                }
                else
                {
                    txbAlmacenf_AutoCompleteExtender.Enabled = true;
                }

                if (lsbSeleccionf.SelectedIndex == -1)
                {
                    btnGenerar.Visible = false;
                }

                if (!IsPostBack)
                {
                    Session["reportdocument"] = "";

                    LimpiarPaneles();
                    if (pnlLocalidadf.Visible)
                    {
                        CargarEstados();
                    }
                }
            }
            catch (NullReferenceException)
            {
                //if (!Page.ClientScript.IsStartupScriptRegistered("alertsession"))
                //{
                //    Page.ClientScript.RegisterStartupScript(this.GetType(),
                //        "alertsession", "alertarSesion();", true);
                //}
                //pnlGeneral.Visible = false;
            }
        }

        protected void btnGenerar_Click(object sender, EventArgs e)
        {           
            string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            MedDAL.Configuracion.DALConfiguracion objConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
            objConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

            switch (Session["reportdocument"].ToString())
            { 
                case "~\\rptReportes\\Almacenes\\rptAlmacenes.rpt":
                    #region rptAlmacenes.rpt
                    MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
                    MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes_contactos", "medicuriConnectionString", odsDataSet, "almacenes_contactos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from estados", "medicuriConnectionString", odsDataSet, "estados");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from municipios", "medicuriConnectionString", odsDataSet, "municipios");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from poblaciones", "medicuriConnectionString", odsDataSet, "poblaciones");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from colonias", "medicuriConnectionString", odsDataSet, "colonias");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");

                    //Variables de sesión a utilizar
                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Almacenes\\rptAlmacenes.rpt");
                    Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Almacenes\\rptAlmacenes.rpt");
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;
                    
                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    Almacenes oAlmacenes = (Almacenes)Parent.Page;
                    oAlmacenes.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\Almacenes\\rptDistribucionExistencias.rpt": 
                case "~\\rptReportes\\Almacenes\\rptDistribucionExistenciasClave.rpt":
                case "~\\rptReportes\\Almacenes\\rptDistribucionExistenciasNombre.rpt":
                    #region rptDistribucionExistencias.rpt
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen_stocks", "medicuriConnectionString", odsDataSet, "productos_almacen_stocks");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Almacenes\\rptDistribucionExistencias.rpt");
                    Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Almacenes\\rptDistribucionExistencias.rpt");
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");                                
                    oAlmacenes = (Almacenes)Parent.Page;
                    oAlmacenes.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\Proveedores\\rptProveedores.rpt":
                    #region rptProveedores.rpt
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from proveedores", "medicuriConnectionString", odsDataSet, "proveedores");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from proveedores_contactos", "medicuriConnectionString", odsDataSet, "proveedores_contactos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from estados", "medicuriConnectionString", odsDataSet, "estados");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from municipios", "medicuriConnectionString", odsDataSet, "municipios");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from poblaciones", "medicuriConnectionString", odsDataSet, "poblaciones");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from colonias", "medicuriConnectionString", odsDataSet, "colonias");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Proveedores\\rptProveedores.rpt");
                    Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Proveedores\\rptProveedores.rpt");
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    Proveedores oProveedores = (Proveedores)Parent.Page;
                    oProveedores.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\Vendedores\\rptVendedores.rpt":
                    #region rptVendedores
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from vendedores", "medicuriConnectionString", odsDataSet, "vendedores");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from vendedores_especialidad", "medicuriConnectionString", odsDataSet, "vendedores_especialidad");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from vendedores_vinculacion", "medicuriConnectionString", odsDataSet, "vendedores_vinculacion");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from estados", "medicuriConnectionString", odsDataSet, "estados");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from municipios", "medicuriConnectionString", odsDataSet, "municipios");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from poblaciones", "medicuriConnectionString", odsDataSet, "poblaciones");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from colonias", "medicuriConnectionString", odsDataSet, "colonias");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Vendedores\\rptVendedores.rpt");
                    Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Vendedores\\rptVendedores.rpt");
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    Vendedores oVendedores = (Vendedores)Parent.Page;
                    oVendedores.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\Clientes\\rptClientes.rpt":
                    #region rptClientes
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsDataSet, "clientes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes_contacto", "medicuriConnectionString", odsDataSet, "clientes_contacto");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from estados", "medicuriConnectionString", odsDataSet, "estados");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from municipios", "medicuriConnectionString", odsDataSet, "municipios");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from poblaciones", "medicuriConnectionString", odsDataSet, "poblaciones");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from colonias", "medicuriConnectionString", odsDataSet, "colonias");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Clientes\\rptClientes.rpt");
                    Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Clientes\\rptClientes.rpt");
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");


                    Clientes oCliente = (Clientes)Parent.Page;
                    oCliente.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\Clientes\\rptEstadoCuentaGeneral.rpt":
                    #region rptEstadoCuentaGeneral
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsDataSet, "clientes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas", "medicuriConnectionString", odsDataSet, "facturas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas_partida", "medicuriConnectionString", odsDataSet, "facturas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from estados", "medicuriConnectionString", odsDataSet, "estados");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from municipios", "medicuriConnectionString", odsDataSet, "municipios");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from poblaciones", "medicuriConnectionString", odsDataSet, "poblaciones");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from colonias", "medicuriConnectionString", odsDataSet, "colonias");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Clientes\\rptEstadoCuentaGeneral.rpt");
                    Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Clientes\\rptEstadoCuentaGeneral.rpt");
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oCliente = (Clientes)Parent.Page;
                    oCliente.CargarListaReportes();

                    #endregion
                    break; 
                case "~\\rptReportes\\Clientes\\rptPorConcepto.rpt":
                    #region rptPorConcepto
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsDataSet, "clientes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas", "medicuriConnectionString", odsDataSet, "facturas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas_partida", "medicuriConnectionString", odsDataSet, "facturas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from estados", "medicuriConnectionString", odsDataSet, "estados");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from municipios", "medicuriConnectionString", odsDataSet, "municipios");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from poblaciones", "medicuriConnectionString", odsDataSet, "poblaciones");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from colonias", "medicuriConnectionString", odsDataSet, "colonias");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Clientes\\rptPorConcepto.rpt");
                    Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Clientes\\rptPorConcepto.rpt");
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oCliente = (Clientes)Parent.Page;
                    oCliente.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\Facturacion\\rptPendientesPorSurtir.rpt":
                    #region rptPendientesPorSurtir
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida_faltantes", "medicuriConnectionString", odsDataSet, "recetas_partida_faltantes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas", "medicuriConnectionString", odsDataSet, "recetas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Facturacion\\rptPendientesPorSurtir.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;

                    //Variables de session para las formulas de almacen y productos desde hasta
                    Session["sProductoDesdePendientes"] = txbClave1f.Text;
                    Session["sProductoHastaPendientes"] = txbClave2f.Text;
                    Session["sAlmacenNombre"] = txbAlmacenf.Text;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    Facturacion oFacturacion = (Facturacion)Parent.Page;
                    oFacturacion.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\Productos\\rptProductos.rpt":
                case "~\\rptReportes\\Productos\\rptProductosUltimaFactura.rpt":
                case "~\\rptReportes\\Productos\\rptProductosUltimaReceta.rpt":
                case "~\\rptReportes\\Productos\\rptProductosUltimaRemision.rpt":
                    #region rptProductos
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from proveedores", "medicuriConnectionString", odsDataSet, "proveedores");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from proveedores_productos", "medicuriConnectionString", odsDataSet, "proveedores_productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario", "medicuriConnectionString", odsDataSet, "inventario");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario_partida", "medicuriConnectionString", odsDataSet, "inventario_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas", "medicuriConnectionString", odsDataSet, "facturas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas_partida", "medicuriConnectionString", odsDataSet, "facturas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas", "medicuriConnectionString", odsDataSet, "recetas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from remisiones", "medicuriConnectionString", odsDataSet, "remisiones");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from remisiones_partida", "medicuriConnectionString", odsDataSet, "remisiones_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
                    
                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Productos\\rptProductos.rpt");
                    Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Productos\\rptProductos.rpt");
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 1;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    Productos oProductos = (Productos)Parent.Page;
                    oProductos.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\Productos\\rptListaPrecios.rpt":
                    #region rptListaPrecios
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario", "medicuriConnectionString", odsDataSet, "inventario");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario_partida", "medicuriConnectionString", odsDataSet, "inventario_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Productos\\rptListaPrecios.rpt");
                    Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Productos\\rptListaPrecios.rpt");
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oProductos = (Productos)Parent.Page;
                    oProductos.CargarListaReportes();

                    #endregion
                    break;
                    //GT 0249
                case "~\\rptReportes\\Productos\\rptProductosLista.rpt":
                    #region Reporte lista productos (rptProductosLista.rpt)
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from proveedores", "medicuriConnectionString", odsDataSet, "proveedores");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from proveedores_productos", "medicuriConnectionString", odsDataSet, "proveedores_productos");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Productos\\rptProductosLista.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oProductos = (Productos)Parent.Page;
                    oProductos.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\Productos\\rptStockMinimo.rpt":
                case "~\\rptReportes\\Productos\\rptStockMinimoAlmacen.rpt":
                    #region rptStockMinimo
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "inventario") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario", "medicuriConnectionString", odsDataSet, "inventario");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario_partida", "medicuriConnectionString", odsDataSet, "inventario_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from proveedores", "medicuriConnectionString", odsDataSet, "proveedores");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from proveedores_productos", "medicuriConnectionString", odsDataSet, "proveedores_productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida_faltantes", "medicuriConnectionString", odsDataSet, "recetas_partida_faltantes");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Productos\\rptStockMinimo.rpt");
                    Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Productos\\rptStockMinimo.rpt");
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    if (Session["recordselection"].ToString() == "~\\rptReportes\\Productos\\rptStockMinimo.rpt")
                    {
                        Session["sortfield"] = 0;
                    }
                    else
                    {
                        Session["sortfield"] = 1;
                    }

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oProductos = (Productos)Parent.Page;
                    oProductos.CargarListaReportes();
                    
                    #endregion
                    break;
                case "~\\rptReportes\\Productos\\rptStockMaximo.rpt":
                    #region rptStockMaximo
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "inventario") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario", "medicuriConnectionString", odsDataSet, "inventario");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario_partida", "medicuriConnectionString", odsDataSet, "inventario_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from proveedores", "medicuriConnectionString", odsDataSet, "proveedores");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from proveedores_productos", "medicuriConnectionString", odsDataSet, "proveedores_productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida_faltantes", "medicuriConnectionString", odsDataSet, "recetas_partida_faltantes");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Productos\\rptStockMaximo.rpt");
                    Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Productos\\rptStockMaximo.rpt");
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;                    
                    Session["sortfield"] = 1;                   

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");
                    
                    oProductos = (Productos)Parent.Page;
                    oProductos.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\Facturacion\\rptDetalladoDeFacturas.rpt":
                    #region rptDetalladoDeFacturas
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from usuarios", "medicuriConnectionString", odsDataSet, "usuarios");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas", "medicuriConnectionString", odsDataSet, "facturas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas_partida", "medicuriConnectionString", odsDataSet, "facturas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsDataSet, "clientes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from estados", "medicuriConnectionString", odsDataSet, "estados");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from municipios", "medicuriConnectionString", odsDataSet, "municipios");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from poblaciones", "medicuriConnectionString", odsDataSet, "poblaciones");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from colonias", "medicuriConnectionString", odsDataSet, "colonias");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Facturacion\\rptDetalladoDeFacturas.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");
                    
                    oFacturacion = (Facturacion)Parent.Page;
                    oFacturacion.CargarListaReportes();
                    
                    #endregion
                    break;
                case "~\\rptReportes\\Facturacion\\rptResumenGeneralDeFacturas.rpt":
                    #region rptResumenGeneralDeFacturas
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from usuarios", "medicuriConnectionString", odsDataSet, "usuarios");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas", "medicuriConnectionString", odsDataSet, "facturas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas_partida", "medicuriConnectionString", odsDataSet, "facturas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsDataSet, "clientes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from estados", "medicuriConnectionString", odsDataSet, "estados");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from municipios", "medicuriConnectionString", odsDataSet, "municipios");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from poblaciones", "medicuriConnectionString", odsDataSet, "poblaciones");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from colonias", "medicuriConnectionString", odsDataSet, "colonias");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Facturacion\\rptResumenGeneralDeFacturas.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");
                    
                    oFacturacion = (Facturacion)Parent.Page;
                    oFacturacion.CargarListaReportes();
                    
                    #endregion
                    break;
                case "~\\rptReportes\\Inventarios\\rptProductosCaducos.rpt":
                    #region rptProductosCaducos
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");                    
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");
                    
                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Inventarios\\rptProductosCaducos.rpt");
                    Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Inventarios\\rptProductosCaducos.rpt");
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;                    

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    Inventarios oInventarios = (Inventarios)Parent.Page;
                    oInventarios.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\Inventarios\\rptComprasProveedor.rpt":
                    #region rptComprasProveedor
                        odsDataSet = new MedDAL.DataSets.dsDataSet();
                        oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                        oUsuario = new MedDAL.DAL.usuarios();
                        oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                        odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
                        odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "inventario") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario", "medicuriConnectionString", odsDataSet, "inventario");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario_partida", "medicuriConnectionString", odsDataSet, "inventario_partida");                    
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from proveedores", "medicuriConnectionString", odsDataSet, "proveedores");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from proveedores_productos", "medicuriConnectionString", odsDataSet, "proveedores_productos");

                        Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Inventarios\\rptComprasProveedor.rpt");
                        Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Inventarios\\rptComprasProveedor.rpt");
                        Session["dataset"] = odsDataSet;
                        Session["titulo"] = txbTitulo.Text;
                        Session["configuracionsistema"] = objConfiguracion;
                        Session["sortfield"] = 0;                    

                        Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");
                    
                        oInventarios = (Inventarios)Parent.Page;
                    oInventarios.CargarListaReportes();
    
                    #endregion
                    break;
                case "~\\rptReportes\\Inventarios\\rptVentasPeriodo.rpt":
                    #region rptVentasPeriodo
                        odsDataSet = new MedDAL.DataSets.dsDataSet();
                        oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                        oUsuario = new MedDAL.DAL.usuarios();
                        oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                        odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
                        odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "inventario") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario", "medicuriConnectionString", odsDataSet, "inventario");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario_partida", "medicuriConnectionString", odsDataSet, "inventario_partida");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas", "medicuriConnectionString", odsDataSet, "facturas");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas_partida", "medicuriConnectionString", odsDataSet, "facturas_partida");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");

                        Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Inventarios\\rptVentasPeriodo.rpt");
                        Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Inventarios\\rptVentasPeriodo.rpt");
                        Session["dataset"] = odsDataSet;
                        Session["titulo"] = txbTitulo.Text;
                        Session["configuracionsistema"] = objConfiguracion;
                        Session["sortfield"] = 1;                    

                        Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");
                    
                    oInventarios = (Inventarios)Parent.Page;
                    oInventarios.CargarListaReportes();
                    
                    #endregion
                    break;
                case "~\\rptReportes\\Inventarios\\rptVentasCliente.rpt":
                    #region rptVentasCliente
                        odsDataSet = new MedDAL.DataSets.dsDataSet();
                        oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                        oUsuario = new MedDAL.DAL.usuarios();
                        oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                        odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsDataSet, "clientes");                        
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas", "medicuriConnectionString", odsDataSet, "facturas");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas_partida", "medicuriConnectionString", odsDataSet, "facturas_partida");

                        Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Inventarios\\rptVentasCliente.rpt");
                        Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Inventarios\\rptVentasCliente.rpt");
                        Session["dataset"] = odsDataSet;
                        Session["titulo"] = txbTitulo.Text;
                        Session["configuracionsistema"] = objConfiguracion;
                        Session["sortfield"] = 0;                    

                        Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");
                    
                        oInventarios = (Inventarios)Parent.Page;
                    oInventarios.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\Inventarios\\rptNumerosSerie.rpt":
                    #region rptNumerosSerie
                        odsDataSet = new MedDAL.DataSets.dsDataSet();
                        oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
                        odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "inventario") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario", "medicuriConnectionString", odsDataSet, "inventario");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario_partida", "medicuriConnectionString", odsDataSet, "inventario_partida");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas", "medicuriConnectionString", odsDataSet, "facturas");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas_partida", "medicuriConnectionString", odsDataSet, "facturas_partida");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas", "medicuriConnectionString", odsDataSet, "recetas");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");

                        Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Inventarios\\rptNumerosSerie.rpt");
                        Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Inventarios\\rptNumerosSerie.rpt");
                        Session["dataset"] = odsDataSet;
                        Session["titulo"] = txbTitulo.Text;
                        Session["configuracionsistema"] = objConfiguracion;
                        Session["sortfield"] = 1;                    

                        Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");
                    
                        oInventarios = (Inventarios)Parent.Page;
                    oInventarios.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\Inventarios\\rptPedimentosAduanalesLotes.rpt":
                    #region rptPedimentosAduanalesLotes
                        odsDataSet = new MedDAL.DataSets.dsDataSet();
                        oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
                        odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "inventario") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario", "medicuriConnectionString", odsDataSet, "inventario");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario_partida", "medicuriConnectionString", odsDataSet, "inventario_partida");                        
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");

                        Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Inventarios\\rptPedimentosAduanalesLotes.rpt");
                        Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Inventarios\\rptPedimentosAduanalesLotes.rpt");
                        Session["dataset"] = odsDataSet;
                        Session["titulo"] = txbTitulo.Text;
                        Session["configuracionsistema"] = objConfiguracion;
                        Session["sortfield"] = 1;                    

                        Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");
                    
                    oInventarios = (Inventarios)Parent.Page;
                    oInventarios.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\Inventarios\\rptHistorialExistencias.rpt":
                    #region rptHistorialExistencias
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "inventario") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario", "medicuriConnectionString", odsDataSet, "inventario");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario_partida", "medicuriConnectionString", odsDataSet, "inventario_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Inventarios\\rptHistorialExistencias.rpt");
                    Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\Inventarios\\rptHistorialExistencias.rpt");
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");
                   
                    oInventarios = (Inventarios)Parent.Page;
                    oInventarios.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\LineasCredito\\rptAcumuladoLineaCredito.rpt":
                    #region rptAcumuladoLineaCredito
                        odsDataSet = new MedDAL.DataSets.dsDataSet();
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from FacturacionDeRecetas", "medicuriConnectionString", odsDataSet, "FacturacionDeRecetas");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from lineas_creditos", "medicuriConnectionString", odsDataSet, "lineas_creditos");                        

                        Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\LineasCredito\\rptAcumuladoLineaCredito.rpt");
                        Session["campoaordenar"] = ObtenerCampoOrden("~\\rptReportes\\LineasCredito\\rptAcumuladoLineaCredito.rpt");
                        Session["dataset"] = odsDataSet;
                        Session["titulo"] = txbTitulo.Text;
                        Session["configuracionsistema"] = objConfiguracion;
                        Session["sortfield"] = 0;

                        Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                        LineasCredito oLineasCredito = (LineasCredito)Parent.Page;
                        oLineasCredito.CargarListaReportes();
                    
                    #endregion
                    break;
                case "~\\rptReportes\\Facturacion\\rptAntiguedadDeSaldos.rpt":
                    #region rptAntiguedadDeSaldos.rpt
                        odsDataSet = new MedDAL.DataSets.dsDataSet();
                        oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas", "medicuriConnectionString", odsDataSet, "facturas");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas_partida", "medicuriConnectionString", odsDataSet, "facturas_partida");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from usuarios", "medicuriConnectionString", odsDataSet, "usuarios");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsDataSet, "clientes");

                        Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Facturacion\\rptAntiguedadDeSaldos.rpt");
                        Session["campoaordenar"] = "";
                        Session["dataset"] = odsDataSet;
                        Session["titulo"] = txbTitulo.Text;
                        Session["configuracionsistema"] = objConfiguracion;
                        Session["sortfield"] = 0;
                        Session["sAlmacenNombre"] = txbAlmacenf.Text;

                        Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");
                   
                        oFacturacion = (Facturacion)Parent.Page;
                        oFacturacion.CargarListaReportes();

                    #endregion
                    break;
                case "~\\rptReportes\\Facturacion\\rptCuentasPorCobrar.rpt":
                    #region Cuentas por cobrar
                     odsDataSet = new MedDAL.DataSets.dsDataSet();
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from facturas", "medicuriConnectionString", odsDataSet, "facturas");
                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsDataSet, "clientes");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Facturacion\\rptCuentasPorCobrar.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    cuentasxcobrar oCuentasPorCobrar = (cuentasxcobrar)Parent.Page;
                    oCuentasPorCobrar.CargarListaReportes();
                                      
                    #endregion
                    break;
                case "~\\rptReportes\\Movimientos\\rptMovimientos.rpt":
                    #region rptMovimientos
                        odsDataSet = new MedDAL.DataSets.dsDataSet();
                        oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                        oUsuario = new MedDAL.DAL.usuarios();
                        oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());

                        odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from VistaMovimientos", "medicuriConnectionString", odsDataSet, "VistaMovimientos");
                        //odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                        //odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                        //odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "inventario") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario", "medicuriConnectionString", odsDataSet, "inventario");
                        //odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario_partida", "medicuriConnectionString", odsDataSet, "inventario_partida");
                        //odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
                        //odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from MovimientoSalida", "medicuriConnectionString", odsDataSet, "MovimientoSalida");

                        Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\Movimientos\\rptMovimientos.rpt");
                        Session["tablaordenar"] = "VistaMovimientos";
                        Session["campoaordenar"] = "NombreAlmacen";
                        Session["dataset"] = odsDataSet;
                        Session["titulo"] = txbTitulo.Text;
                        Session["configuracionsistema"] = objConfiguracion;
                        Session["sortfield"] = 0;
                        //Session["sAlmacenNombre"] = txbAlmacenf.Text;

                        Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");
                   
                        Movimientos oMovimientos = (Movimientos)Parent.Page;
                        oMovimientos.CargarListaReportes();
                    #endregion
                    break;
                case "~\\rptReportes\\rptRecetas.rpt":
                    #region Recetas
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4 and idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "recetas") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4", "medicuriConnectionString", odsDataSet, "recetas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsDataSet, "clientes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from vendedores", "medicuriConnectionString", odsDataSet, "vendedores");


                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\rptRecetas.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    //Session["reportdocument"] = "~\\rptReportes\\rptRecetas.rpt";
                    Session["configuracionsistema"] = objConfiguracion;                    
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    Reportes oReportes = (Reportes)Parent.Page;
                    oReportes.CargarListaReportes();
                    #endregion
                    break;
                case "~\\rptReportes\\rptRecetasDiagnostico.rpt":
                    #region RecetasDiagnostico
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4 and idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "recetas") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4", "medicuriConnectionString", odsDataSet, "recetas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsDataSet, "clientes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from vendedores", "medicuriConnectionString", odsDataSet, "vendedores");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from causes", "medicuriConnectionString", odsDataSet, "causes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from causes_cie", "medicuriConnectionString", odsDataSet, "causes_cie");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from causes_medicamentos", "medicuriConnectionString", odsDataSet, "causes_medicamentos");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\rptRecetasDiagnostico.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    //Session["reportdocument"] = "~\\rptReportes\\rptRecetas.rpt";
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oReportes = (Reportes)Parent.Page;
                    oReportes.CargarListaReportes();
                    #endregion
                    break;
                case "~\\rptReportes\\rptRecetasLineaCredito.rpt":
                    #region RecetasLineaCredito
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4 and idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "recetas")
                        :
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4", "medicuriConnectionString", odsDataSet, "recetas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsDataSet, "clientes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from vendedores", "medicuriConnectionString", odsDataSet, "vendedores");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from lineas_creditos", "medicuriConnectionString", odsDataSet, "lineas_creditos");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\rptRecetasLineaCredito.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;
                    //Session["reportdocument"] = "~\\rptReportes\\rptRecetas.rpt";
                    Session["configuracionsistema"] = objConfiguracion;
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oReportes = (Reportes)Parent.Page;
                    oReportes.CargarListaReportes();
                    #endregion
                    break;
                case "~\\rptReportes\\rptRecetasLocalidad.rpt":
                    #region RecetasLocalidad
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4 and idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "recetas")
                        :
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4", "medicuriConnectionString", odsDataSet, "recetas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from poblaciones", "medicuriConnectionString", odsDataSet, "poblaciones");


                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\rptRecetasLocalidad.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;                    
                    Session["configuracionsistema"] = objConfiguracion;                    
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oReportes = (Reportes)Parent.Page;
                    oReportes.CargarListaReportes();
                    #endregion
                    break;
                case "~\\rptReportes\\rptMedicamentosMasRecetados.rpt":
                    #region MedicamentosMasRecetados
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet.EnforceConstraints = false;
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4 and idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "recetas")
                        :
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4", "medicuriConnectionString", odsDataSet, "recetas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");


                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\rptMedicamentosMasRecetados.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;                    
                    Session["configuracionsistema"] = objConfiguracion;                    
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oReportes = (Reportes)Parent.Page;
                    oReportes.CargarListaReportes();
                    #endregion
                    break;
                case "~\\rptReportes\\rptMedicamentosPrescritosMedico.rpt":
                    #region MedicamentosPrescritosMedico
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet.EnforceConstraints = false;
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4 and idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "recetas")
                        :
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4", "medicuriConnectionString", odsDataSet, "recetas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from vendedores", "medicuriConnectionString", odsDataSet, "vendedores");


                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\rptMedicamentosPrescritosMedico.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;                    
                    Session["configuracionsistema"] = objConfiguracion;                    
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oReportes = (Reportes)Parent.Page;
                    oReportes.CargarListaReportes();
                    #endregion
                    break;
                case "~\\rptReportes\\rptConsumosMedicamento.rpt":
                    #region ConsumosMedicamento
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet.EnforceConstraints = false;
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4 and idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "recetas")
                        :
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4", "medicuriConnectionString", odsDataSet, "recetas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");


                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\rptConsumosMedicamento.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;                    
                    Session["configuracionsistema"] = objConfiguracion;                    
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oReportes = (Reportes)Parent.Page;
                    oReportes.CargarListaReportes();
                    #endregion
                    break;
                case "~\\rptReportes\\rptConsumosMedicamentoFarmacia.rpt":
                    #region ConsumosMedicamentoFarmacia
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet.EnforceConstraints = false;
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas", "medicuriConnectionString", odsDataSet, "recetas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\rptConsumosMedicamentoFarmacia.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;                    
                    Session["configuracionsistema"] = objConfiguracion;                    
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oReportes = (Reportes)Parent.Page;
                    oReportes.CargarListaReportes();
                    #endregion
                    break;
                case "~\\rptReportes\\rptConsumosMedicamentoRequisicion.rpt":
                    #region ConsumosMedicamentoRequisicion
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet.EnforceConstraints = false;
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4 and idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "recetas")
                        :
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4", "medicuriConnectionString", odsDataSet, "recetas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");


                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\rptConsumosMedicamentoRequisicion.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;                    
                    Session["configuracionsistema"] = objConfiguracion;                    
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oReportes = (Reportes)Parent.Page;
                    oReportes.CargarListaReportes();
                    #endregion
                    break;
                case "~\\rptReportes\\rptInventarios.rpt":
                    #region Inventarios
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet.EnforceConstraints = false;
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen_stocks", "medicuriConnectionString", odsDataSet, "productos_almacen_stocks");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");                   

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\rptInventarios.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;                    
                    Session["configuracionsistema"] = objConfiguracion;                    
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oReportes = (Reportes)Parent.Page;
                    oReportes.CargarListaReportes();
                    #endregion
                    break;
                case "~\\rptReportes\\rptInventariosLotes.rpt":
                    #region InventariosLotes
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet.EnforceConstraints = false;
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen_stocks", "medicuriConnectionString", odsDataSet, "productos_almacen_stocks");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");                   

                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\rptInventariosLotes.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;                    
                    Session["configuracionsistema"] = objConfiguracion;                    
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oReportes = (Reportes)Parent.Page;
                    oReportes.CargarListaReportes();
                    #endregion
                    break; 
                case "~\\rptReportes\\rptCaducos.rpt":
                    #region Caducos
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet.EnforceConstraints = false;
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen_stocks", "medicuriConnectionString", odsDataSet, "productos_almacen_stocks");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    //odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");


                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\rptCaducos.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;                    
                    Session["configuracionsistema"] = objConfiguracion;                    
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oReportes = (Reportes)Parent.Page;
                    oReportes.CargarListaReportes();
                    #endregion
                    break;
                case "~\\rptReportes\\rptRecetasPaciente.rpt":
                    #region RecetasPaciente
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet.EnforceConstraints = false;
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4 and idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "recetas")
                        :
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4", "medicuriConnectionString", odsDataSet, "recetas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsDataSet, "clientes");


                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\rptRecetasPaciente.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;                    
                    Session["configuracionsistema"] = objConfiguracion;                    
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oReportes = (Reportes)Parent.Page;
                    oReportes.CargarListaReportes();
                    #endregion
                    break;
                case "~\\rptReportes\\rptRecetasRequisicion.rpt":
                    #region RecetasRequisicion
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet.EnforceConstraints = false;
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4 and idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "recetas")
                        :
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4", "medicuriConnectionString", odsDataSet, "recetas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");


                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\rptRecetasRequisicion.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;                    
                    Session["configuracionsistema"] = objConfiguracion;                    
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oReportes = (Reportes)Parent.Page;
                    oReportes.CargarListaReportes();
                    #endregion
                    break;
                case "~\\rptReportes\\rptRecetasConsumo.rpt":
                    #region RecetasConsumo
                    odsDataSet = new MedDAL.DataSets.dsDataSet();
                    odsDataSet.EnforceConstraints = false;
                    oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") 
                        : 
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
                    odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4 and idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "recetas")
                        :
                        (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4", "medicuriConnectionString", odsDataSet, "recetas");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");
                    odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");


                    Session["recordselection"] = ObtenerRecordSelection("~\\rptReportes\\rptRecetasConsumo.rpt");
                    Session["campoaordenar"] = "";
                    Session["dataset"] = odsDataSet;
                    Session["titulo"] = txbTitulo.Text;                    
                    Session["configuracionsistema"] = objConfiguracion;                    
                    Session["sortfield"] = 0;

                    Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

                    oReportes = (Reportes)Parent.Page;
                    oReportes.CargarListaReportes();
                    #endregion
                    break;
            }
        }

        protected void cmbEstadof_SelectedIndexChanged(object sender, EventArgs e)
        {            
            CargarMunicipios();            
        }

        protected void cmbMunicipiof_SelectedIndexChanged(object sender, EventArgs e)
        {            
            CargarPoblaciones();            
        }
        
        protected void lsbSeleccionf_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Le puse ToLower por si se cambia la notacio de estandar a camel o a puras mayusculas, etc...
            LimpiarPaneles();

            switch (lsbSeleccionf.SelectedValue.ToLower())
            {
                #region reportesespecializados
                case "reporte de almacenes":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Almacenes", false, true, true, true, true, false, false, false, false, false, false, false, false, false
                        , true, true, true, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Almacenes\\rptAlmacenes.rpt";
                    txbTitulo.Text=lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "reporte de distribución de existencias":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Almacenes", false, true, false, false, true, false, false, false, false, true, false, false, false, false, true
                        , true, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Almacenes\\rptDistribucionExistencias.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "reporte de proveedores":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Proveedores", true, true, false, true, true, false, false, false, false, false, false, false, false, false
                        , true, true, true, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Proveedores\\rptProveedores.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "reporte de vendedores":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Vendedores", true, true, true, true, true, true, true, false, false, false, false, false, false, false
                        , true, true, true, true, true, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Vendedores\\rptVendedores.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;                
                case "reporte de clientes":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Clientes", true, true, true, true, true, false, false, false, false, false, false, false, false, false
                        , true, true, true, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Clientes\\rptClientes.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "reporte de estado de cuenta general":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Clientes", true, true, true, false, true, false, false, false, false, false, false, false, false, false
                        , true, true, true, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Clientes\\rptEstadoCuentaGeneral.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "reporte por concepto":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Clientes", true, true, true, false, true, false, false, false, false, false, false, false, false, false
                        , true, true, true, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Clientes\\rptPorConcepto.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "reporte de productos por almacen":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Productos", false, true, false, true, true, false, false, false, false, true, false, false, false, false
                        , true, true, true, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Productos\\rptProductos.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "reporte de productos con dato de ultima salida por factura":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Productos", false, true, false, true, true, false, false, false, false, true, false, false, false, false
                        , true, true, true, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Productos\\rptProductosUltimaFactura.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "reporte de productos con dato de ultima salida por receta":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Productos", false, true, false, true, true, false, false, false, false, true, false, false, false, false
                        , true, true, true, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Productos\\rptProductosUltimaReceta.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "reporte de productos con dato de ultima salida por remisión":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Productos", false, true, false, true, true, false, false, false, false, true, false, false, false, false
                        , true, true, true, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Productos\\rptProductosUltimaRemision.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "reporte de lista de precios":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Productos", false, true, false, true, true, false, false, false, false, false, false, false, false, false
                        , true, true, true, false, false, false, true, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Productos\\rptListaPrecios.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "reporte de stock mínimo":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Productos", false, true, false, false, true, false, false, false, false, true, false, true, false, false,
                        true, true, false, false, false, false, true, true, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Productos\\rptStockMinimo.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "reporte de stock máximo":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Productos", false, true, false, false, true, false, false, false, false, true, false, true, false, false,
                        true, true, false, false, false, false, true, true, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Productos\\rptStockMaximo.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "pendientes por surtir":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Facturacion", true, true, false, false, false, false, false, false, false, true, false, false, false, false
                        , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Facturacion\\rptPendientesPorSurtir.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "detallado de facturas":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Facturacion1", true, true, true, false, false, false, false, true, true, false, false, false, false, false
                       , true, true, true, false, false, false, true, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Facturacion\\rptDetalladoDeFacturas.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "resumen general de facturas":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Facturacion2", true, true, true, false, false, false, false, true, true, false, false, false, false, false
                       , true, true, true, false, false, false, true, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Facturacion\\rptResumenGeneralDeFacturas.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "antiguedad de saldos":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("AntiguedadDeSaldos", true, false, true, false, false, false, false, false, false, true, false, false, false, false
                       , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Facturacion\\rptAntiguedadDeSaldos.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break; 
                case "reporte de productos caducos":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Inventarios", true, true, false, false, true, false, false, false, false, true, false, false, false, false
                       , true, true, true, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Inventarios\\rptProductosCaducos.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();                    
                    break;
                case "reporte de compras por proveedor":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Inventarios", true, true, false, false, false, false, false, false, false, true, false, true, false, false
                       , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Inventarios\\rptComprasProveedor.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();                    
                    break;
                case "reporte de ventas del periodo":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Inventarios", true, true, false, false, true, false, false, false, false, true, false, false, false, false
                       , true, true, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Inventarios\\rptVentasPeriodo.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();                    
                    break;                  
                case "reporte de ventas por cliente":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Inventarios", true, true, false, false, false, false, false, false, true, true, false, false, false, false
                       , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Inventarios\\rptVentasCliente.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();                    
                    break;  
                case "reporte de números de serie":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Inventarios", false, true, false, false, true, false, false, false, false, true, false, false, false, false
                       , true, true, true, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Inventarios\\rptNumerosSerie.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();                    
                    break;
                case "reporte de pedimentos aduanales y lotes":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = true;
                    CargarPaneles("Inventarios", false, true, false, false, true, false, false, false, false, true, false, false, false, false
                       , true, true, true, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Inventarios\\rptPedimentosAduanalesLotes.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "reporte de historial de existencias":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Inventarios", true, true, false, false, false, false, false, false, false, true, false, false, false, false
                       , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Inventarios\\rptHistorialExistencias.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "reporte de acumulado de lineas de crédito":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("LineasCredito", true, true, false, false, false, false, false, false, false, false, false, false, false, false
                       , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\LineasCredito\\rptAcumuladoLineaCredito.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();                    
                    break;
                case "reporte de movimientos":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Movimientos", true, false, false, false, false, false, false, false, false, false, false, false, false, false
                       , false, false, false, false, false, false, false, false, false, false, false,false, true);
                    Session["reportdocument"] = "~\\rptReportes\\Movimientos\\rptMovimientos.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();             
                    break;
                case "cuentas por cobrar":
                     btnGenerar.Visible = true;
                     pnlOrden.Visible = false;
                     CargarPaneles("CuentasPorCobrar", true,false, false,false, false, false, false, false,true, false, false, false, false, false
                       , false, false, false, false, false, false, false, false, false, false, false, true, false);
                    Session["reportdocument"] = "~\\rptReportes\\Facturacion\\rptCuentasPorCobrar.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    rblEstatusFacturacion.SelectedIndex = -1;

                    break;
                    //GT 0249
                case "reporte de productos":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Productos", false, true, false, false, true, false, false, false,false, false, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\Productos\\rptProductosLista.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                #endregion
                #region ReportesLicitacion
                case "recetas":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Reportes", true, false, false, false, false, false, false, false, false, true, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\rptRecetas.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "recetas por diagnóstico":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Reportes", true, false, false, false, false, false, false, false, false, true, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\rptRecetasDiagnostico.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "recetas por linea de crédito":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Reportes", true, false, false, false, false, false, false, false, false, true, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\rptRecetasLineaCredito.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "medicamentos prescritos por localidad":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Reportes", true, false, false, false, false, false, false, false, false, true, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\rptRecetasLocalidad.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "medicamentos de mayor movimiento":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Reportes", true, false, false, false, false, false, false, false, false, true, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\rptMedicamentosMasRecetados.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "medicamentos prescritos por médico":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Reportes", true, false, false, false, false, false, false, false, false, true, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\rptMedicamentosPrescritosMedico.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "consumos por medicamento":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Reportes", true, false, false, false, false, false, false, false, false, true, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\rptConsumosMedicamento.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "consumos de medicamento por farmacia":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Reportes", true, false, false, false, false, false, false, false, false, true, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\rptConsumosMedicamentoFarmacia.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "consumos de medicamento por requisición hospitalaria":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Reportes", true, false, false, false, false, false, false, false, false, true, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\rptConsumosMedicamentoRequisicion.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "niveles de inventario":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Reportes", false, false, false, false, false, false, false, false, false, true, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\rptInventarios.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "inventario por lotes":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Reportes", false, false, false, false, false, false, false, false, false, true, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\rptInventariosLotes.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "medicamento caduco":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Reportes", true, false, false, false, false, false, false, false, false, true, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\rptCaducos.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "recetas por paciente":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Reportes", true, false, false, false, false, false, false, false, false, true, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\rptRecetasPaciente.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "recetas por requisición hospitalaria":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Reportes", true, false, false, false, false, false, false, false, false, true, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\rptRecetasRequisicion.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break;
                case "recetas por consumo":
                    btnGenerar.Visible = true;
                    pnlOrden.Visible = false;
                    CargarPaneles("Reportes", true, false, false, false, false, false, false, false, false, true, false, false, false, false
                      , false, false, false, false, false, false, false, false, false, false, false, false, false);
                    Session["reportdocument"] = "~\\rptReportes\\rptRecetasConsumo.rpt";
                    txbTitulo.Text = lsbSeleccionf.SelectedValue.ToString();
                    break; 
                #endregion




            }
            
        }

    }
}
