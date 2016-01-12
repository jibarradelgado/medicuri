using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using MedNeg.CamposEditables;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Medicuri
{
    public partial class CamposEditables : System.Web.UI.Page
    {
        ImageButton imbEditar, imbAceptar, imbCancelar, imbImprimir;
        Label lblNombreModulo;
        BlCamposEditables oblCamposEditables;
        MedNeg.Bitacora.BlBitacora oblBitacora;        
        MedDAL.DAL.bitacora oBitacora;
        List<MedDAL.DAL.campos_editables> lstCamposEditables;

        protected void Editar() 
        {
            int iContadorErrores = 0;
           
            

            lstCamposEditables = (List<MedDAL.DAL.campos_editables>)Session["lstcamposeditables"];
            
            for (int i = 0; i < 10; i++)
            {
                lstCamposEditables[i].Valor = ObtenerValor(TabContainer1, "txbAlm" + (i + 1).ToString());
                lstCamposEditables[i + 10].Valor = ObtenerValor(TabContainer1, "txbCli" + (i + 1).ToString());
                lstCamposEditables[i + 20].Valor = ObtenerValor(TabContainer1, "txbPro" + (i + 1).ToString());
                lstCamposEditables[i + 30].Valor = ObtenerValor(TabContainer1, "txbPre" + (i + 1).ToString());
                lstCamposEditables[i + 40].Valor = ObtenerValor(TabContainer1, "txbUsu" + (i + 1).ToString());
                lstCamposEditables[i + 50].Valor = ObtenerValor(TabContainer1, "txbVen" + (i + 1).ToString());
                lstCamposEditables[i + 60].Valor = ObtenerValor(TabContainer1, "txbLin" + (i + 1).ToString());

            }

            foreach (MedDAL.DAL.campos_editables oCampoEditable in lstCamposEditables)
            {
                if (!oblCamposEditables.EditarRegistro(oCampoEditable)) 
                {
                    iContadorErrores++;
                }
            }

            if (iContadorErrores == 0)
            {
                lblAviso.Text = "Los campos han sido editados exitosamente";
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Campos Editables";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Actualización de Campos Editables";
                oBitacora.Descripcion = "Todos los Campos Editables fueron editados";
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "Los campos no pudieron ser editados";
            }
        } 

        protected void CargarTextBoxes() 
        {
            lstCamposEditables = oblCamposEditables.Buscar();

            Session["lstcamposeditables"] = lstCamposEditables;

            for (int i = 0; i < 10; i++)
            {
                AsignarValores(this.TabContainer1, "txbAlm" + (i + 1).ToString(), lstCamposEditables[i].Valor);
                AsignarValores(this.TabContainer1, "txbCli" + (i + 1).ToString(), lstCamposEditables[i + 10].Valor);
                AsignarValores(this.TabContainer1, "txbPro" + (i + 1).ToString(), lstCamposEditables[i + 20].Valor);
                AsignarValores(this.TabContainer1, "txbPre" + (i + 1).ToString(), lstCamposEditables[i + 30].Valor);
                AsignarValores(this.TabContainer1, "txbUsu" + (i + 1).ToString(), lstCamposEditables[i + 40].Valor);
                AsignarValores(this.TabContainer1, "txbVen" + (i + 1).ToString(), lstCamposEditables[i + 50].Valor);
                AsignarValores(this.TabContainer1, "txbLin" + (i + 1).ToString(), lstCamposEditables[i + 60].Valor);
                
            }

        }

        public string ObtenerValor(Control c, string sNombreControl)
        {
            if (c is TextBox)
            {
                if (((TextBox)c).ID == sNombreControl)
                {
                    return ((TextBox)c).Text;
                }
            }
            
            foreach (Control ctrl in c.Controls)
            {
                string sValor = ObtenerValor(ctrl, sNombreControl);
                if (sValor != "") return sValor;
            }

            return "";
        }

        public void AsignarValores(Control c, string sNombreControl, string sValor)
        {

            if (c is TextBox)
            {
                //((TextBox)c).Enabled = true;
                if (((TextBox)c).ID == sNombreControl) 
                {
                    ((TextBox)c).Text = sValor;    
                    return;
                }            
            }            

            foreach (Control ctrl in c.Controls)
            {
                AsignarValores(ctrl, sNombreControl, sValor);
            }
        }

        protected void DesactivarEdicionEliminacion()
        {
            Master.FindControl("btnEditar").Visible = false;            
        }

        protected void Habilita()
        {

            foreach (Control c in Page.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    foreach (Control c2 in c.Controls)
                    {
                        foreach (Control c3 in c2.Controls)
                        {
                            foreach (Control c4 in c3.Controls)
                            {
                                foreach (Control c5 in c4.Controls)
                                {
                                    foreach (Control c6 in c5.Controls)
                                    {
                                        foreach (Control c7 in c6.Controls)
                                        {
                                            foreach (Control c8 in c7.Controls)
                                            {
                                                foreach (Control c9 in c8.Controls)
                                                {
                                                    foreach (Control c10 in c9.Controls)
                                                    {
                                                        if (c10.GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                                                        {
                                                            ((TextBox)c10).Enabled = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void Deshabilita()
        {
            foreach (Control c in Page.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    foreach (Control c2 in c.Controls)
                    {
                        foreach (Control c3 in c2.Controls)
                        {
                            foreach (Control c4 in c3.Controls)
                            {
                                foreach (Control c5 in c4.Controls)
                                {
                                    foreach (Control c6 in c5.Controls)
                                    {
                                        foreach (Control c7 in c6.Controls)
                                        {
                                            foreach (Control c8 in c7.Controls)
                                            {
                                                foreach (Control c9 in c8.Controls)
                                                {
                                                    foreach (Control c10 in c9.Controls)
                                                    {
                                                        if (c10.GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                                                        {
                                                            ((TextBox)c10).Enabled = false;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {                      
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';

            try
            {
                #region Interfaz
                cPermiso = (char)htbPermisos["campos editables"];

                Master.FindControl("btnNuevo").Visible = false;
                Master.FindControl("btnEliminar").Visible = false;
                Master.FindControl("btnMostrar").Visible = false;
                Master.FindControl("btnReportes").Visible = false;
                Master.FindControl("rdbFiltro1").Visible = false;
                Master.FindControl("rdbFiltro2").Visible = false;
                Master.FindControl("rdbFiltro3").Visible = false;
                Master.FindControl("btnBuscar").Visible = false;
                Master.FindControl("txtBuscar").Visible = false;
                Master.FindControl("lblBuscar").Visible = false;
                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "Campos Editables";
                Master.FindControl("btnImprimir").Visible = false;

                imbEditar = (ImageButton)Master.FindControl("imgBtnEditar");
                imbEditar.Click += new ImageClickEventHandler(this.imbEditar_Click);
                imbAceptar = (ImageButton)Master.FindControl("imgBtnAceptar");
                imbAceptar.Click += new ImageClickEventHandler(this.imbAceptar_Click);
                imbAceptar.ValidationGroup = "Configuracion";
                imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
                imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
                imbImprimir = (ImageButton)Master.FindControl("imgBtnImprimir");
                imbImprimir.Click += new ImageClickEventHandler(this.imbImprimir_Click);
                #endregion

                oblCamposEditables = new BlCamposEditables();
                oblBitacora = new MedNeg.Bitacora.BlBitacora();

                switch (cPermiso)
                {
                    case 'T':
                        break;
                    case 'E':
                        DesactivarEdicionEliminacion();
                        break;
                    case 'L':
                        DesactivarEdicionEliminacion();
                        break;
                }

                if (!IsPostBack)
                {
                    Session["camposeditablesaccion"] = 0;
                    CargarTextBoxes();
                    pnlReportes.Visible = false;

                    Session["reporteactivoCampos"] = 0;
                    Session["reportdocumentCampos"] = "";
                    Session["tituloCampos"] = "";
                    //GT 0175
                    ConfigurarMenuBotones(false, false, false, true, false, false, false, false);
                }

                Deshabilita();
            }
            catch (NullReferenceException)
            {
                if (!ClientScript.IsStartupScriptRegistered("alertsession"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alertsession", "alertarSesion();", true);
                }
                Deshabilita();
                pnlReportes.Visible = false;
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }

        protected void imbEditar_Click(object sender, EventArgs e)
        {
            UpdatePanel1.Visible = true;
            pnlReportes.Visible = false;
            lblAviso.Text = "";
            Session["camposeditablesaccion"] = 1;
            Habilita();
            //0175 GT
            ConfigurarMenuBotones(false, false, true, false, true, true, false, false);
            
        }

        protected void imbAceptar_Click(object sender, EventArgs e)
        {
            if ((int)Session["camposeditablesaccion"] == 1) 
            {
                Editar();
                Session["camposeditablesaccion"] = 0;
                //GT 0175
                ConfigurarMenuBotones(false, false, false, true, false, false, false, false);
            }
        }

        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            UpdatePanel1.Visible = true;
            pnlReportes.Visible = false;
            lblAviso.Text = "";
            Session["camposeditablesaccion"] = 0;
            Deshabilita();
            //GT 0175
            ConfigurarMenuBotones(false, false, false, true, false, false, false, false);
        }


        #region Estado de los botones

        private void ConfigurarMenuBotones(bool bNuevo, bool bMostrar, bool bEliminar, bool bEditar, bool bAceptar, bool bCancelar, bool bReportes, bool bImprmir)
        {
            ((ImageButton)Master.FindControl("imgBtnNuevo")).Enabled = bNuevo;
            ((ImageButton)Master.FindControl("imgBtnMostrar")).Enabled = bMostrar;
            ((ImageButton)Master.FindControl("imgBtnEliminar")).Enabled = bEliminar;
            ((ImageButton)Master.FindControl("imgBtnEditar")).Enabled = bEditar;
            ((ImageButton)Master.FindControl("imgBtnAceptar")).Enabled = bAceptar;
            ((ImageButton)Master.FindControl("imgBtnCancelar")).Enabled = bCancelar;
            ((ImageButton)Master.FindControl("imgBtnReportes")).Enabled = bReportes;
            ((ImageButton)Master.FindControl("imgBtnImprimir")).Enabled = bImprmir;
        }

        #endregion
        

        #region Reportes

        protected void CargarReporte()
        {
           
            UpdatePanel1.Visible = false;
            pnlReportes.Visible = true;

            Session["reporteactivoCampos"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from campos_editables", "medicuriConnectionString", odsDataSet, "campos_editables");
          
            Session["datasetCamposEditables"] = odsDataSet;
            Session["reportdocumentCampos"] = "~\\rptReportes\\rptCamposEditables.rpt";
            Session["tituloCampos"] = "Campos Editables";
            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocumentCampos"].ToString()));

            //if (dgvDatos.SelectedIndex != -1)
            //{
            //    rptReporte.RecordSelectionFormula = "{perfiles.idPerfil}=" + dgvDatos.SelectedDataKey.Values[0].ToString();
            //}

            rptReporte.SetDataSource(odsDataSet);
            crvReporte.Visible = true;
            crvReporte.ReportSource = rptReporte;
        }
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
            rptReporte.Load(Server.MapPath(Session["reportdocumentCampos"].ToString()));
            rptReporte.SetDataSource((DataSet)Session["datasetCamposEditables"]);
            crvReporte.Visible = true;
            crvReporte.ReportSource = rptReporte;
            crvReporte.PageZoomFactor = 100;
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
            return repDoc;
        }
        protected void imbImprimir_Click(object sender, EventArgs e)
        {
            CargarReporte();
        }
        protected void crvReporte_Drill(object source, CrystalDecisions.Web.DrillEventArgs e)
        {
            ObtenerReporte();
        }
        protected void crvReporte_DrillDownSubreport(object source, CrystalDecisions.Web.DrillSubreportEventArgs e)
        {
            ObtenerReporte();
        }
        protected void crvReporte_Navigate(object source, CrystalDecisions.Web.NavigateEventArgs e)
        {
            ObtenerReporte();
            crvReporte.PageZoomFactor = 50;
        }
        protected void crvReporte_Search(object source, CrystalDecisions.Web.SearchEventArgs e)
        {
            ObtenerReporte();
            crvReporte.PageZoomFactor = 50;
        }
        protected void crvReporte_ViewZoom(object source, CrystalDecisions.Web.ZoomEventArgs e)
        {
            ObtenerReporte();
        }
        protected void crvReporte_DataBinding(object sender, EventArgs e)
        {
            if (Session["reportdocumentCampos"].ToString() != "")
            {
                ObtenerReporte();
            }
        }
        protected void btnPdf_Click(object sender, EventArgs e)
        {
            if (int.Parse(Session["reporteactivoCampos"].ToString()) == 1)
            {
                // Get the report document
                string sReporte = Session["reportdocumentCampos"].ToString();
                ReportDocument repDoc = getReportDocument(sReporte);
                //Esta linea soluciona el problema de la excepcion que no permite guardar los archivos
                repDoc.SetDataSource((DataSet)Session["datasetCamposEditables"]);
                // Stop buffering the response
                Response.Buffer = false;
                // Clear the response content and headers
                Response.ClearContent();
                Response.ClearHeaders();
                try
                {
                    // Export the Report to Response stream in PDF format
                    repDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, Session["tituloCampos"].ToString());
                    // There are other format options available such as Word, Excel, CVS, and HTML in the ExportFormatType Enum given by crystal reports
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ex = null;
                }
            }
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (int.Parse(Session["reporteactivoCampos"].ToString()) == 1)
            {
                // Get the report document
                string sReporte = Session["reportdocumentCampos"].ToString();
                ReportDocument repDoc = getReportDocument(sReporte);
                //Esta linea soluciona el problema de la excepcion que no permite guardar los archivos
                repDoc.SetDataSource((DataSet)Session["datasetCamposEditables"]);
                // Stop buffering the response
                Response.Buffer = false;
                // Clear the response content and headers
                Response.ClearContent();
                Response.ClearHeaders();
                try
                {
                    // Export the Report to Response stream in Excel format
                    repDoc.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, Response, true, Session["tituloCampos"].ToString());
                    // There are other format options available such as Word, Excel, CVS, and HTML in the ExportFormatType Enum given by crystal reports
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ex = null;
                }
            }
        }
        protected void btnCrystal_Click(object sender, EventArgs e)
        {
            if (int.Parse(Session["reporteactivoCampos"].ToString()) == 1)
            {
                // Get the report document
                string sReporte = Session["reportdocumentCampos"].ToString();
                ReportDocument repDoc = getReportDocument(sReporte);
                //Esta linea soluciona el problema de la excepcion que no permite guardar los archivos
                repDoc.SetDataSource((DataSet)Session["datasetCamposEditables"]);
                // Stop buffering the response
                Response.Buffer = false;
                // Clear the response content and headers
                Response.ClearContent();
                Response.ClearHeaders();
                try
                {
                    // Export the Report to Response stream in Crystal Report format
                    repDoc.ExportToHttpResponse(ExportFormatType.CrystalReport, Response, true, Session["tituloCampos"].ToString());
                    // There are other format options available such as Word, Excel, CVS, and HTML in the ExportFormatType Enum given by crystal reports
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ex = null;
                }
            }
        }
        #endregion
    }

}