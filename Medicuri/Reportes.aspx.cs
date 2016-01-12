using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Medicuri
{
    public partial class Reportes : System.Web.UI.Page
    {        
        RadioButton rdbNombre, rdbClave, rdbTodos;
        Label lblNombreModulo;
        Button btnBuscar;
        TextBox txbBuscar;
        MedNeg.Facturas.BlFacturas oblFacturas;
        MedNeg.Recetas.BlRecetas oblRecetas;
        MedNeg.Poblaciones.BlPoblaciones oblPoblaciones;

        /// <summary>
        /// Obtiene todas las recetas segun la localidad
        /// </summary>
        /// <returns></returns>
        private object ObtenerRecetasLocalidad() 
        { 
            //Funcion en estado de test
            var recetas = oblRecetas.BuscarReceta();
            return recetas;
        }

        protected void ObtenerReporte()
        {
            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
            rptReporte.SetDataSource((DataSet)Session["dataset"]);
            //crvReporte.Visible = true;
            //crvReporte.ReportSource = rptReporte;
        }

        /// <summary>
        /// Obtiene el ReportDocument a partir de un reporte existente en el proyecto.
        /// </summary>
        /// <param name="sNombreReporte"></param>
        /// <returns></returns>
        private ReportDocument getReportDocument(string sNombreReporte)
        {
            // path del Crystal Report
            
            string repFilePath = Server.MapPath(sNombreReporte);
            // Declara un nuevo objeto ReportDocument y lo carga con el path del Archivo
            // Crystal Report
            ReportDocument repDoc = new ReportDocument();
            repDoc.Load(repFilePath);

            // Coloca el datasource obteniendo la coleccion de datos desde la capa de negocios
            repDoc.SetDataSource((IQueryable)Session["reporte"]);

            return repDoc;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';

            try
            {
                #region Interfaz
                cPermiso = (char)htbPermisos["reportes"];
                Master.FindControl("btnNuevo").Visible = false;
                Master.FindControl("btnEditar").Visible = false;
                Master.FindControl("btnEliminar").Visible = false;
                Master.FindControl("btnReportes").Visible = false;
                Master.FindControl("btnMostrar").Visible = false;
                Master.FindControl("btnCancelar").Visible = false;
                Master.FindControl("btnAceptar").Visible = false;
                Master.FindControl("lblBuscar").Visible = false;

                rdbTodos = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbTodos.Visible = false;
                rdbClave = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbClave.Visible = false;
                rdbNombre = (RadioButton)Master.FindControl("rdbFiltro3");
                rdbNombre.Visible = false;

                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Visible = false;
                txbBuscar = (TextBox)Master.FindControl("txtBuscar");
                txbBuscar.Visible = false;


                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "Reportes";

                /*switch (cPermiso)
                {
                    case 'T':
                        break;
                    case 'E':                    
                        break;
                    case 'L':                    
                        break;
                }*/
                #endregion

                oblFacturas = new MedNeg.Facturas.BlFacturas();
                oblRecetas = new MedNeg.Recetas.BlRecetas();
                oblPoblaciones = new MedNeg.Poblaciones.BlPoblaciones();

                if (!IsPostBack)
                {
                    Session["reporteactivo"] = 0;
                    Session["reportdocument"] = "";
                    Session["titulo"] = "";
                    Session["reporte"] = null;
                    pnlFiltroReportes.Visible = true;
                    CargarListaReportes();
                }
            }
            catch (NullReferenceException)
            {
                if (!ClientScript.IsStartupScriptRegistered("alertsession"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alertsession", "alertarSesion();", true);
                }
                
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }

        //protected DataSet LlenarDataSet(string sConsulta, string sNombreConnectionString, DataSet dsDataSet, string sTabla)
        //{ 
        //    SqlConnection sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[sNombreConnectionString].ConnectionString);
        //    SqlDataAdapter sqlAdapter = new SqlDataAdapter();
        //    sqlAdapter.SelectCommand = new SqlCommand(sConsulta, sqlConnection);
        //    sqlAdapter.Fill(dsDataSet, sTabla);
        //    return dsDataSet;
        //}

        #region Reportes

        public void CargarListaReportes()
        {
            ListBox lsbReportes = (ListBox)frReportes.FindControl("lsbSeleccionf");
            frReportes.LimpiarPaneles();
            lsbReportes.SelectedIndex = -1;
            lsbReportes.Items.Clear();
            if (Server.MapPath("~\\rptReportes\\rptRecetas.rpt") != "")
            {
                lsbReportes.Items.Add("Recetas");
            }
            if (Server.MapPath("~\\rptReportes\\rptRecetasDiagnostico.rpt") != "")
            {
                lsbReportes.Items.Add("Recetas por diagnóstico");
            }
            if (Server.MapPath("~\\rptReportes\\rptRecetasLineaCredito.rpt") != "")
            {
                lsbReportes.Items.Add("Recetas por linea de crédito");
            }
            if (Server.MapPath("~\\rptReportes\\rptRecetasLocalidad.rpt") != "")
            {
                lsbReportes.Items.Add("Medicamentos prescritos por localidad");
            }
            if (Server.MapPath("~\\rptReportes\\rptMedicamentosMasRecetados.rpt") != "")
            {
                lsbReportes.Items.Add("Medicamentos de mayor movimiento");
            }
            if (Server.MapPath("~\\rptReportes\\rptMedicamentosPrescritosMedico.rpt") != "")
            {
                lsbReportes.Items.Add("Medicamentos prescritos por médico");
            }
            if (Server.MapPath("~\\rptReportes\\rptConsumosMedicamento.rpt") != "")
            {
                lsbReportes.Items.Add("Consumos por medicamento");
            }
            if (Server.MapPath("~\\rptReportes\\rptConsumosMedicamentoFarmacia.rpt") != "")
            {
                lsbReportes.Items.Add("Consumos de medicamento por farmacia");
            }
            if (Server.MapPath("~\\rptReportes\\rptConsumosMedicamentoRequisicion.rpt") != "")
            {
                lsbReportes.Items.Add("Consumos de medicamento por requisición hospitalaria");
            }
            if (Server.MapPath("~\\rptReportes\\rptInventarios.rpt") != "")
            {
                lsbReportes.Items.Add("Niveles de inventario");
            }
            if (Server.MapPath("~\\rptReportes\\rptInventariosLotes.rpt") != "")
            {
                lsbReportes.Items.Add("Inventario por lotes");
            }
            if (Server.MapPath("~\\rptReportes\\rptCaducos.rpt") != "")
            {
                lsbReportes.Items.Add("Medicamento caduco");
            }
            if (Server.MapPath("~\\rptReportes\\rptRecetasPaciente.rpt") != "")
            {
                lsbReportes.Items.Add("Recetas por paciente");
            }
            if (Server.MapPath("~\\rptReportes\\rptRecetasRequisicion.rpt") != "")
            {
                lsbReportes.Items.Add("Recetas por requisición hospitalaria");
            }
            if (Server.MapPath("~\\rptReportes\\rptRecetasConsumo.rpt") != "")
            {
                lsbReportes.Items.Add("Recetas por consumo");
            }
        }

        //protected DataSet LlenarDataSet(string sConsulta, string sNombreConnectionString, DataSet dsDataSet, string sTabla)
        //{
        //    SqlConnection sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[sNombreConnectionString].ConnectionString);
        //    SqlDataAdapter sqlAdapter = new SqlDataAdapter();
        //    sqlAdapter.SelectCommand = new SqlCommand(sConsulta, sqlConnection);
        //    sqlAdapter.Fill(dsDataSet, sTabla);
        //    return dsDataSet;
        //}

        #endregion

    }
}