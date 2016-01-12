using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using MedNeg.Estados;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Medicuri
{
    public partial class Estados : System.Web.UI.Page
    {
        ImageButton imbNuevo, imbEditar, imbEliminar, imbImprimir, imbMostrar, imbAceptar, imbCancelar, imbReportes;
        RadioButton rdbNombre, rdbClave, rdbTodos;        
        Button btnBuscar;
        TextBox txbBuscar;
        Label lblNombreModulo;
        MedNeg.Estados.BlEstados oblEstados;
        MedNeg.Bitacora.BlBitacora oblBitacora;
        MedDAL.DAL.estados oEstados;
        MedDAL.DAL.bitacora oBitacora;

        protected void CargarFormulario(bool bDatos)
        {
            pnlFormulario.Visible = true;
            pnlCatalogo.Visible = false;            

            if (bDatos)
            {
                txbClave.Text = gdvDatos.SelectedRow.Cells[1].Text;
                txbClave.Enabled = false;
                rfvClave.Enabled = false;
                txbNombre.Text = gdvDatos.SelectedRow.Cells[2].Text;
                //ckbActivo.Visible = true;                
                ckbActivo.Checked = ((CheckBox)gdvDatos.SelectedRow.Cells[3].FindControl("ctl01")).Checked;
            }
            else 
            {
                txbClave.Enabled = true;
                rfvClave.Enabled = true;
                txbClave.Text = "";                
                txbNombre.Text = "";
                //ckbActivo.Visible = false;
            }
        }
        
        protected void CargarCatalogo()
        {
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = true;            
        }

        protected void Buscar(string sCadena)
        {
            int iTipo = 1;
            if (rdbTodos.Checked)
            {
                iTipo = 1;
            }
            else if (rdbClave.Checked)
            {
                iTipo = 2;
            }
            else if(rdbNombre.Checked)
            {
                iTipo = 3;
            }

            var oQuery = oblEstados.Buscar(sCadena, iTipo);            
            
            try
            {                
                gdvDatos.DataSource = oQuery;
                Session["resultadoquery"] = oQuery;
                ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
                //gdvDatos.DataKeyNames = new string[] { "idEstado" };
                gdvDatos.DataBind();
                CargarCatalogo();
                if (gdvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    gdvDatos.EmptyDataText = "No existen estados registrados aun";                    
                }
                else
                {
                    gdvDatos.EmptyDataText = "No existen estados que coincidan con la búsqueda";                    
                }
                gdvDatos.ShowHeader = true;                
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void Nuevo()
        {
            oEstados = new MedDAL.DAL.estados();
            oEstados.Clave = txbClave.Text;
            oEstados.Nombre = txbNombre.Text;
            oEstados.Activo = true;

            if (oblEstados.NuevoRegistro(oEstados))
            {
                lblAviso.Text = "El estado se ha registrado con éxito";
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Estados";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Nuevo Estado";
                oBitacora.Descripcion = "Clave: " + txbClave.Text + ", Nombre: " + txbNombre.Text;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "El estado no pudo ser registrado";
            }
        }

        protected void Editar() 
        {
            oEstados = new MedDAL.DAL.estados();            
            oEstados.idEstado = int.Parse(gdvDatos.SelectedDataKey.Value.ToString());
            oEstados.Clave = txbClave.Text;
            oEstados.Nombre = txbNombre.Text;
            oEstados.Activo = ckbActivo.Enabled;

            if (oblEstados.EditarRegistro(oEstados))
            {
                lblAviso.Text = "El estado ha sido actualizado con éxito";
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Estados";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Actualización de Estado";
                oBitacora.Descripcion = "Clave: " + txbClave.Text + ", Nombre: " + txbNombre.Text;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "El estado no pudo ser actualizado";
            }
        }

        protected void Eliminar() 
        {
            oEstados = new MedDAL.DAL.estados();
            string sClave = gdvDatos.SelectedRow.Cells[2].Text;
            string sNombre = gdvDatos.SelectedRow.Cells[3].Text;
            oEstados.idEstado = int.Parse(gdvDatos.SelectedDataKey.Value.ToString());
            if (oblEstados.EliminarRegistro(oEstados)) 
            {
                lblAviso.Text = "El estado fue eliminado";
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Estados";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Eliminación de Estado";
                oBitacora.Descripcion = "Clave: " + sClave + ", Nombre: " + sNombre;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "El estado no pudo ser eliminado, es posible que tenga datos relacionados";
            }
        }

        protected void DesactivarEdicionEliminacion()
        {
            Master.FindControl("btnEditar").Visible = false;
            Master.FindControl("btnEliminar").Visible = false;
        }
        protected void DesactivarNuevo() 
        {
            Master.FindControl("btnNuevo").Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Obtener los controles de master
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';

            try
            {
                #region Interfaz

                cPermiso = (char)htbPermisos["estados"];
                imbNuevo = (ImageButton)Master.FindControl("imgBtnNuevo");
                imbNuevo.Click += new ImageClickEventHandler(this.imbNuevo_Click);
                imbEditar = (ImageButton)Master.FindControl("imgBtnEditar");
                imbEditar.Click += new ImageClickEventHandler(this.imbEditar_Click);
                imbEliminar = (ImageButton)Master.FindControl("imgBtnEliminar");
                imbEliminar.Click += new ImageClickEventHandler(this.imbEliminar_Click);
                imbMostrar = (ImageButton)Master.FindControl("imgBtnMostrar");
                imbMostrar.Click += new ImageClickEventHandler(this.imbMostrar_Click);
                imbImprimir = (ImageButton)Master.FindControl("imgBtnImprimir");
                imbImprimir.Click += new ImageClickEventHandler(this.imbImprimir_Click);
                imbAceptar = (ImageButton)Master.FindControl("imgBtnAceptar");
                imbAceptar.Click += new ImageClickEventHandler(this.imbAceptar_Click);
                imbAceptar.ValidationGroup = "Estados";
                imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
                imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
                rdbTodos = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbTodos.Text = "Nombre y Clave";
                rdbClave = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbClave.Text = "Clave";
                rdbNombre = (RadioButton)Master.FindControl("rdbFiltro3");
                rdbNombre.Text = "Nombre";
                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
                txbBuscar = (TextBox)Master.FindControl("txtBuscar");
                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "Estados";

                //GT 0175
                imbReportes = (ImageButton)Master.FindControl("imgBtnReportes");
                imbReportes.Click += new ImageClickEventHandler(this.imbReportes_Click);

                switch (cPermiso)
                {
                    case 'T':
                        break;
                    case 'E':
                        DesactivarEdicionEliminacion();
                        break;
                    case 'L':
                        DesactivarEdicionEliminacion();
                        DesactivarNuevo();
                        break;
                }
                #endregion
                oblEstados = new MedNeg.Estados.BlEstados();
                oblBitacora = new MedNeg.Bitacora.BlBitacora();
                if (!IsPostBack)
                {
                    Session["estadosaccion"] = 0;
                    pnlFormulario.Visible = false;
                    pnlCatalogo.Visible = false;
                    Session["resultadoquery"] = "";
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
                    //CargarFormulario(false); 

                    Session["reporteactivo"] = 0;
                    Session["reportdocument"] = "";
                    Session["titulo"] = "";
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                }
            }
            catch (NullReferenceException)
            {
                if (!ClientScript.IsStartupScriptRegistered("alertsession"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alertsession", "alertarSesion();", true);
                }
                pnlFormulario.Visible = false;
                pnlCatalogo.Visible = false;
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }  

        protected void imbNuevo_Click(object sender, EventArgs e)
        {
            CargarFormulario(false);
            gdvDatos.SelectedIndex = -1;
            Session["estadosaccion"] = 1;
            lblAviso.Text = "";
            lblAviso2.Text = "";
            //0175 GT
            ConfigurarMenuBotones(true, false, false, false, true, true, false, false);
        }
        protected void imbEditar_Click(object sender, EventArgs e)
        {
            if (gdvDatos.SelectedIndex != -1)
            {
                CargarFormulario(true);
                //0175 GT
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);
            }
            else
            {
                Buscar("");
                CargarCatalogo();
                //0175 GT
                ConfigurarMenuBotones(true, true, true, false, false, false, true, true);
            }
            Session["estadosaccion"] = 2;
            lblAviso.Text = "";
            lblAviso2.Text = "";
        }
        protected void imbEliminar_Click(object sender, EventArgs e)
        {   
            if (pnlCatalogo.Visible && gdvDatos.SelectedIndex != -1)
            {
                Eliminar();
                Buscar(txbBuscar.Text);
                CargarCatalogo();
            }
            else 
            {                
                CargarCatalogo();
                Buscar(txbBuscar.Text);
            }
        }        
        protected void imbMostrar_Click(object sender, EventArgs e)
        {
            Buscar("");
            gdvDatos.SelectedIndex = -1;
            Session["estadosaccion"] = 0;            
            lblAviso.Text = "";
            lblAviso2.Text = "";
            //0175 GT
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }
        protected void imbAceptar_Click(object sender, EventArgs e)
        {
            int iAccion;
            if (Session["estadosaccion"] != null) 
            {
                iAccion = (int)Session["estadosaccion"];
            }
            else iAccion = 0;
            switch (iAccion)
            {
                case 0:
                    break;
                case 1:
                    if (cmvClave.IsValid)
                    {
                        Nuevo();
                        Buscar("");
                        CargarFormulario(false);
                        //GT 0175
                        ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    }
                    Session["estadosaccion"] = 1;                       
                    break;
                case 2:
                    Editar();
                    Buscar("");
                    CargarFormulario(true);
                    Session["estadosaccion"] = 2;
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    break;                    
            }
        }
        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            Session["estadosaccion"] = 0;
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar(txbBuscar.Text);
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
        }

        protected void gdvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void cmvClave_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (int.Parse(Session["estadosaccion"].ToString()) != 2)
            {
                string sClave = args.Value.ToString();
                MedDAL.DAL.estados oEstado = oblEstados.Buscar(sClave);
                args.IsValid = oEstado == null ? true : false;
            }
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
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;            

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from estados", "medicuriConnectionString", odsDataSet, "estados");

            Session["campoaordenar"] = "";
            Session["dataset"] = odsDataSet;
            Session["titulo"] = "Impresión de Estados";
            //Session["configuracionsistema"] = objConfiguracion;
            Session["sortfield"] = 0;
            Session["reportdocument"] = "~\\rptReportes\\rptEstados.rpt";

            if (gdvDatos.SelectedIndex != -1)
            {
                Session["recordselection"] = "{estados.idEstado}=" + gdvDatos.SelectedDataKey.Values[0].ToString();
            }
            else
            {
                Session["recordselection"] = "";
            }

            Response.Write("<script type='text/javascript'>detailedresults=window.open('VistaReporteGenerico.aspx');</script>");

        }
        protected DataSet LlenarDataSet(string sConsulta, string sNombreConnectionString, DataSet dsDataSet, string sTabla)
        {
            SqlConnection sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[sNombreConnectionString].ConnectionString);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = new SqlCommand(sConsulta, sqlConnection);
            sqlAdapter.Fill(dsDataSet, sTabla);
            return dsDataSet;
        }
        
        protected void imbImprimir_Click(object sender, EventArgs e)
        {
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
            CargarReporte();
        }        
        #endregion

        private const string ASCENDING = " ASC";
        private const string DESCENDING = " DESC";
        
        protected void gdvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sSortExpression = e.SortExpression;

            if ((System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"] == System.Web.UI.WebControls.SortDirection.Ascending)
            {
                e.SortDirection = System.Web.UI.WebControls.SortDirection.Descending;
                SortGridView(sSortExpression, DESCENDING);
                ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Descending;
            }
            else
            {
                e.SortDirection = System.Web.UI.WebControls.SortDirection.Ascending;
                SortGridView(sSortExpression, ASCENDING);
                ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            }
        }

        private void SortGridView(string sortExpression, string direction)
        {
            //var result = oblEstados.Buscar("", 1);
            var result = (IQueryable<MedDAL.DAL.estados>)Session["resultadoquery"];

            MedDAL.DAL.medicuriEntities oMedicuriEntities = new MedDAL.DAL.medicuriEntities();

            //var result = from s in oMedicuriEntities.estados
            //             select s;
            
            DataTable dt =  MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);

            DataView dv = new DataView(dt);
            dv.Sort = sortExpression + direction;

            gdvDatos.DataSource = dv;
            gdvDatos.DataBind();
        }
    }
}