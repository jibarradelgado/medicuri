using System;
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
    public partial class VistaReporte : System.Web.UI.Page
    {
        
        protected DataSet LlenarDataSet(string sConsulta, string sNombreConnectionString, DataSet dsDataSet, string sTabla)
        {
            SqlConnection sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[sNombreConnectionString].ConnectionString);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = new SqlCommand(sConsulta, sqlConnection);
            sqlAdapter.Fill(dsDataSet, sTabla);
            return dsDataSet;
        }

        protected void ObtenerReporte()
        {
            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
            rptReporte.SetDataSource((DataSet)Session["dataset"]);
            crvReporte.Visible = true;
            crvReporte.ReportSource = rptReporte;
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

        private void GenerarReporte(int iReporte)
        {
            switch (iReporte)
            {
                case 0:
                    #region caso0
                    MedDAL.DataSets.dsRecetas odsRecetas = new MedDAL.DataSets.dsRecetas();
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas", "medicuriConnectionString", odsRecetas, "recetas");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsRecetas, "recetas_partida");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos", "medicuriConnectionString", odsRecetas, "productos");

                    Session["dataset"] = odsRecetas;
                    Session["reportdocument"] = "~\\rptReportes\\rptRecetas.rpt";
                    Session["titulo"] = "Recetas";
                    ReportDocument rptReporte = new ReportDocument();

                    rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
                    rptReporte.SetDataSource(odsRecetas);
                    crvReporte.Visible = true;
                    crvReporte.ReportSource = rptReporte;
                    #endregion
                    break;
                case 1:
                    #region caso1
                    odsRecetas = new MedDAL.DataSets.dsRecetas();
                    odsRecetas.EnforceConstraints = false;
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas", "medicuriConnectionString", odsRecetas, "recetas");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsRecetas, "recetas_partida");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos", "medicuriConnectionString", odsRecetas, "productos");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from poblaciones", "medicuriConnectionString", odsRecetas, "poblaciones");

                    Session["dataset"] = odsRecetas;
                    Session["reportdocument"] = "~\\rptReportes\\rptRecetasLocalidad.rpt";
                    Session["titulo"] = "Medicamentos prescritos por localidad";
                    rptReporte = new ReportDocument();

                    rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
                    rptReporte.SetDataSource(odsRecetas);
                    crvReporte.Visible = true;
                    crvReporte.ReportSource = rptReporte;
                    #endregion
                    break;
                case 2:
                    #region caso2
                    odsRecetas = new MedDAL.DataSets.dsRecetas();
                    odsRecetas.EnforceConstraints = false;
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas", "medicuriConnectionString", odsRecetas, "recetas");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsRecetas, "recetas_partida");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos", "medicuriConnectionString", odsRecetas, "productos");

                    Session["dataset"] = odsRecetas;
                    Session["reportdocument"] = "~\\rptReportes\\rptMedicamentosMasRecetados.rpt";
                    Session["titulo"] = "Medicamentos de mayor movimiento";
                    rptReporte = new ReportDocument();

                    rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
                    rptReporte.SetDataSource(odsRecetas);
                    crvReporte.Visible = true;
                    crvReporte.ReportSource = rptReporte;
                    #endregion
                    break;
                case 3:
                    #region caso3
                    odsRecetas = new MedDAL.DataSets.dsRecetas();
                    odsRecetas.EnforceConstraints = false;
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas", "medicuriConnectionString", odsRecetas, "recetas");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsRecetas, "recetas_partida");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos", "medicuriConnectionString", odsRecetas, "productos");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from vendedores", "medicuriConnectionString", odsRecetas, "vendedores");

                    Session["dataset"] = odsRecetas;
                    Session["reportdocument"] = "~\\rptReportes\\rptMedicamentosPrescritosMedico.rpt";
                    Session["titulo"] = "Medicamentos prescritos por médico";
                    rptReporte = new ReportDocument();

                    rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
                    rptReporte.SetDataSource(odsRecetas);
                    crvReporte.Visible = true;
                    crvReporte.ReportSource = rptReporte;
                    #endregion
                    break;
                case 4:
                    #region caso4
                    odsRecetas = new MedDAL.DataSets.dsRecetas();
                    odsRecetas.EnforceConstraints = false;
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas", "medicuriConnectionString", odsRecetas, "recetas");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsRecetas, "recetas_partida");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos", "medicuriConnectionString", odsRecetas, "productos");

                    Session["dataset"] = odsRecetas;
                    Session["reportdocument"] = "~\\rptReportes\\rptConsumosMedicamento.rpt";
                    Session["titulo"] = "Consumos por medicamento";
                    rptReporte = new ReportDocument();

                    rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
                    rptReporte.SetDataSource(odsRecetas);
                    crvReporte.Visible = true;
                    crvReporte.ReportSource = rptReporte;
                    #endregion
                    break;
                case 5:
                    #region caso5
                    odsRecetas = new MedDAL.DataSets.dsRecetas();
                    odsRecetas.EnforceConstraints = false;
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsRecetas, "almacenes");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas", "medicuriConnectionString", odsRecetas, "recetas");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsRecetas, "recetas_partida");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos", "medicuriConnectionString", odsRecetas, "productos");

                    Session["dataset"] = odsRecetas;
                    Session["reportdocument"] = "~\\rptReportes\\rptConsumosMedicamentoFarmacia.rpt";
                    Session["titulo"] = "Consumos de medicamento por farmacia";
                    rptReporte = new ReportDocument();

                    rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
                    rptReporte.SetDataSource(odsRecetas);
                    crvReporte.Visible = true;
                    crvReporte.ReportSource = rptReporte;
                    #endregion
                    break;
                case 6:
                    #region caso6
                    odsRecetas = new MedDAL.DataSets.dsRecetas();
                    odsRecetas.EnforceConstraints = false;
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsRecetas, "tipos");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas", "medicuriConnectionString", odsRecetas, "recetas");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsRecetas, "recetas_partida");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos", "medicuriConnectionString", odsRecetas, "productos");

                    Session["dataset"] = odsRecetas;
                    Session["reportdocument"] = "~\\rptReportes\\rptConsumosMedicamentoRequisicion.rpt";
                    Session["titulo"] = "Consumos de medicamento por requisición hospitalaria";
                    rptReporte = new ReportDocument();

                    rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
                    rptReporte.SetDataSource(odsRecetas);
                    crvReporte.Visible = true;
                    crvReporte.ReportSource = rptReporte;
                    #endregion
                    break;
                case 7:
                    #region caso7
                    odsRecetas = new MedDAL.DataSets.dsRecetas();
                    odsRecetas.EnforceConstraints = false;
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsRecetas, "almacenes");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsRecetas, "productos_almacen");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos_almacen_stocks", "medicuriConnectionString", odsRecetas, "productos_almacen_stocks");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos", "medicuriConnectionString", odsRecetas, "productos");

                    Session["dataset"] = odsRecetas;
                    Session["reportdocument"] = "~\\rptReportes\\rptInventarios.rpt";
                    Session["titulo"] = "Niveles de inventario";
                    rptReporte = new ReportDocument();

                    rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
                    rptReporte.SetDataSource(odsRecetas);
                    crvReporte.Visible = true;
                    crvReporte.ReportSource = rptReporte;
                    #endregion
                    break;
                case 8:
                    #region caso8
                    odsRecetas = new MedDAL.DataSets.dsRecetas();
                    odsRecetas.EnforceConstraints = false;
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsRecetas, "almacenes");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsRecetas, "productos_almacen");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos_almacen_stocks", "medicuriConnectionString", odsRecetas, "productos_almacen_stocks");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos", "medicuriConnectionString", odsRecetas, "productos");

                    Session["dataset"] = odsRecetas;
                    Session["reportdocument"] = "~\\rptReportes\\rptCaducos.rpt";
                    Session["titulo"] = "Medicamento caduco";
                    rptReporte = new ReportDocument();

                    rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
                    rptReporte.SetDataSource(odsRecetas);
                    crvReporte.Visible = true;
                    crvReporte.ReportSource = rptReporte;
                    #endregion
                    break;
                case 9:
                    #region caso9
                    odsRecetas = new MedDAL.DataSets.dsRecetas();
                    odsRecetas.EnforceConstraints = false;
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas", "medicuriConnectionString", odsRecetas, "recetas");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsRecetas, "recetas_partida");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos", "medicuriConnectionString", odsRecetas, "productos");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsRecetas, "clientes");

                    Session["dataset"] = odsRecetas;
                    Session["reportdocument"] = "~\\rptReportes\\rptRecetasPaciente.rpt";
                    Session["titulo"] = "Recetas por paciente";
                    rptReporte = new ReportDocument();

                    rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
                    rptReporte.SetDataSource(odsRecetas);
                    crvReporte.Visible = true;
                    crvReporte.ReportSource = rptReporte;
                    #endregion
                    break;
                case 10:
                    #region caso10
                    odsRecetas = new MedDAL.DataSets.dsRecetas();
                    odsRecetas.EnforceConstraints = false;
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas", "medicuriConnectionString", odsRecetas, "recetas");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsRecetas, "recetas_partida");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos", "medicuriConnectionString", odsRecetas, "productos");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsRecetas, "tipos");

                    Session["dataset"] = odsRecetas;
                    Session["reportdocument"] = "~\\rptReportes\\rptRecetasRequisicion.rpt";
                    Session["titulo"] = "Recetas por requisición hospitalaria";
                    rptReporte = new ReportDocument();

                    rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
                    rptReporte.SetDataSource(odsRecetas);
                    crvReporte.Visible = true;
                    crvReporte.ReportSource = rptReporte;
                    #endregion
                    break;
                case 11:
                    #region caso11
                    odsRecetas = new MedDAL.DataSets.dsRecetas();
                    odsRecetas.EnforceConstraints = false;
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas", "medicuriConnectionString", odsRecetas, "recetas");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsRecetas, "recetas_partida");
                    odsRecetas = (MedDAL.DataSets.dsRecetas)LlenarDataSet("select * from productos", "medicuriConnectionString", odsRecetas, "productos");

                    Session["dataset"] = odsRecetas;
                    Session["reportdocument"] = "~\\rptReportes\\rptRecetasConsumo.rpt";
                    Session["titulo"] = "Recetas por consumo";
                    rptReporte = new ReportDocument();

                    rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
                    rptReporte.SetDataSource(odsRecetas);
                    crvReporte.Visible = true;
                    crvReporte.ReportSource = rptReporte;
                    #endregion
                    break;
                case 12:
                    break;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["reportdocument"] = "";
                Session["titulo"] = "";
                Session["reporte"] = null;
                GenerarReporte(int.Parse(Session["numeroReporte"].ToString()));
            }
        }

        protected void crvReporte_Navigate(object source, CrystalDecisions.Web.NavigateEventArgs e)
        {
            ObtenerReporte();
        }

        protected void crvReporte_Load(object sender, EventArgs e)
        {

        }

        protected void crvReporte_ViewZoom(object source, CrystalDecisions.Web.ZoomEventArgs e)
        {
            ObtenerReporte();
        }

        protected void crvReporte_Search(object source, CrystalDecisions.Web.SearchEventArgs e)
        {
            ObtenerReporte();
        }

        protected void crvReporte_DataBinding(object sender, EventArgs e)
        {
            if (Session["reportdocument"].ToString() != "")
            {
                ObtenerReporte();
            }
        }

        protected void crvReporte_Drill(object source, CrystalDecisions.Web.DrillEventArgs e)
        {
            ObtenerReporte();
        }

        protected void crvReporte_DrillDownSubreport(object source, CrystalDecisions.Web.DrillSubreportEventArgs e)
        {
            ObtenerReporte();
        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            // Get the report document
            string sReporte = Session["reportdocument"].ToString();
            ReportDocument repDoc = getReportDocument(sReporte);
            //Esta linea soluciona el problema de la excepcion que no permite guardar los archivos
            repDoc.SetDataSource((DataSet)Session["dataset"]);
            // Stop buffering the response
            Response.Buffer = false;
            // Clear the response content and headers
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                // Export the Report to Response stream in PDF format
                repDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, Session["titulo"].ToString());
                // There are other format options available such as Word, Excel, CVS, and HTML in the ExportFormatType Enum given by crystal reports
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ex = null;
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            // Get the report document
            string sReporte = Session["reportdocument"].ToString();
            ReportDocument repDoc = getReportDocument(sReporte);
            //Esta linea soluciona el problema de la excepcion que no permite guardar los archivos
            repDoc.SetDataSource((DataSet)Session["dataset"]);
            // Stop buffering the response
            Response.Buffer = false;
            // Clear the response content and headers
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                // Export the Report to Response stream in Excel format
                repDoc.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, Response, true, Session["titulo"].ToString());
                // There are other format options available such as Word, Excel, CVS, and HTML in the ExportFormatType Enum given by crystal reports
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ex = null;
            }
        }

        protected void btnCrystal_Click(object sender, EventArgs e)
        {
            // Get the report document
            string sReporte = Session["reportdocument"].ToString();
            ReportDocument repDoc = getReportDocument(sReporte);
            //Esta linea soluciona el problema de la excepcion que no permite guardar los archivos
            repDoc.SetDataSource((DataSet)Session["dataset"]);
            // Stop buffering the response
            Response.Buffer = false;
            // Clear the response content and headers
            Response.ClearContent();
            Response.ClearHeaders();
            try
            {
                // Export the Report to Response stream in Excel format
                repDoc.ExportToHttpResponse(ExportFormatType.CrystalReport, Response, true, Session["titulo"].ToString());
                // There are other format options available such as Word, Excel, CVS, and HTML in the ExportFormatType Enum given by crystal reports
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ex = null;
            }
        }
    }
}