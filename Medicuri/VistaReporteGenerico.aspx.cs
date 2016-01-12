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
    public partial class VistaReporteGenerico : System.Web.UI.Page
    {
        protected void ObtenerReporte()
        {
            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));

            if (Session["campoaordenar"].ToString() != "")
            {
                FieldDefinition FieldDef;
                FieldDef = rptReporte.Database.Tables[Session["tablaordenar"].ToString()].Fields[Session["campoaordenar"].ToString()];
                rptReporte.DataDefinition.SortFields[int.Parse(Session["sortfield"].ToString())].Field = FieldDef;
                rptReporte.DataDefinition.SortFields[int.Parse(Session["sortfield"].ToString())].SortDirection = CrystalDecisions.Shared.SortDirection.DescendingOrder;
            }
            
            rptReporte.SetDataSource((DataSet)Session["dataset"]);
            rptReporte.RecordSelectionFormula = Session["recordselection"].ToString();            
            crvReporte.Visible = true;
            crvReporte.ReportSource = rptReporte;

            MeterFormulas(rptReporte);            
        }

        private void MeterFormulas(ReportDocument rptReporte)
        {
            //Para datos de facturacion
            string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            MedDAL.Configuracion.DALConfiguracion objConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
            objConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

            foreach (FormulaFieldDefinition oFormula in rptReporte.DataDefinition.FormulaFields)
            {
                if (oFormula.FormulaName == "{@fRazonSocial}")
                {
                    //oFormula.Text = "'" + ((MedDAL.Configuracion.DALConfiguracion)Session["configuracionsistema"]).sRazonSocial.ToString() + "'";
                    oFormula.Text = "'" + objConfiguracion.sRazonSocial.ToString() + "'";
                }
                if (oFormula.FormulaName == "{@fRFC}")
                {
                    //oFormula.Text = "'" + ((MedDAL.Configuracion.DALConfiguracion)Session["configuracionsistema"]).sRfc.ToString() + "'";
                    oFormula.Text = "'" + objConfiguracion.sRfc.ToString() + "'";
                }
                if (oFormula.FormulaName == "{@fDomicilio}")
                {
                    //oFormula.Text = "'" + ((MedDAL.Configuracion.DALConfiguracion)Session["configuracionsistema"]).sDomicilio.ToString() + "'";
                    oFormula.Text = "'" + objConfiguracion.sDomicilio.ToString() + "'";
                }
                if (oFormula.FormulaName == "{@fTituloReporte}")
                {
                    oFormula.Text = "'" + Session["titulo"].ToString() + "'";
                }
                //Producto desde
                if (oFormula.FormulaName == "{@fProductoDesde}")
                {
                    if (Session["sProductoDesdePendientes"].ToString() != "")
                        oFormula.Text = "'" + Session["sProductoDesdePendientes"].ToString() + "'";
                }
                //Producto hasta
                if (oFormula.FormulaName == "{@fProductoHasta}")
                {
                    if (Session["sProductoHastaPendientes"].ToString() != "")
                        oFormula.Text = "'" + Session["sProductoHastaPendientes"].ToString() + "'";
                }
                //Almacen Nombre
                if (oFormula.FormulaName == "{@fAlmacenNombre}")
                {
                    if (Session["sAlmacenNombre"].ToString() != "")
                        oFormula.Text = "'" + Session["sAlmacenNombre"].ToString() + "'";
                    else
                        oFormula.Text = "'Todos'";
                }
            }
        }

        /// <summary>
        /// Obtiene el ReportDocument a partir de un reporte existente en el proyecto.
        /// </summary>
        /// <param name="sNombreReporte"></param>
        /// <returns></returns>
        private ReportDocument getReportDocument(string sNombreReporte)
        {
            string repFilePath = Server.MapPath(sNombreReporte);
            ReportDocument repDoc = new ReportDocument();
            repDoc.Load(repFilePath);
            MeterFormulas(repDoc);
            return repDoc;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //ObtenerReporte();

            if (!IsPostBack)
            {
                //Session["yapasoporaquielpuntero"] = false;
            }
        }

        protected void crvReporte_Navigate(object source, CrystalDecisions.Web.NavigateEventArgs e)
        {
            //if(!bool.Parse(Session["yapasoporaquielpuntero"].ToString())) ObtenerReporte();
            //else Session["yapasoporaquielpuntero"] = false;
            ObtenerReporte();
        }

        /*protected void crvReporte_Load(object sender, EventArgs e)
        {

        }*/

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
                Session["yapasoporaquielpuntero"] = true;
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
            repDoc.RecordSelectionFormula = Session["recordselection"].ToString();
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
            repDoc.RecordSelectionFormula = Session["recordselection"].ToString();
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
            repDoc.RecordSelectionFormula = Session["recordselection"].ToString();
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