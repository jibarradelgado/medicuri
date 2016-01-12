using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using MedNeg.Poblaciones;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Medicuri
{
    public partial class Poblaciones : System.Web.UI.Page
    {
        ImageButton imbNuevo, imbEditar, imbEliminar, imbImprimir, imbMostrar, imbAceptar, imbCancelar, imbReportes;
        RadioButton rdbNombre, rdbClave, rdbTodos;
        Button btnBuscar;
        TextBox txbBuscar;
        Label lblNombreModulo;
        IQueryable<MedDAL.DAL.estados> iqrEstados;
        IQueryable<MedDAL.DAL.municipios> iqrMunicipios;
        MedNeg.Poblaciones.BlPoblaciones oblPoblaciones;
        MedNeg.Municipios.BlMunicipios oblMunicipios;
        MedNeg.Estados.BlEstados oblEstados;
        MedNeg.Bitacora.BlBitacora oblBitacora;        
        MedDAL.DAL.poblaciones oPoblacion;
        MedDAL.DAL.bitacora oBitacora;

        /// <summary>
        /// Actualiza la variable de sesion "lstEstados", la cual es una lista de los estados activos
        /// </summary>
        protected void ActualizarSesionEstados()
        {
            iqrEstados = oblEstados.BuscarEnum();
            Session["lstEstados"] = iqrEstados;
        }

        /// <summary>
        /// Actualiza la variable de sesion de Municipios dependiendo de los Estados existentes.
        /// </summary>
        /// <param name="iTipo">1=De los estados en catalogo, 2=De los estados en formulario"</param>
        protected void ActualizarSesionMunicipios(int iTipo)
        {
            switch (iTipo)
            {
                case 1:
                    iqrMunicipios = oblMunicipios.BuscarEstados(int.Parse(cmbEstadoCatalogo.SelectedValue));
                    Session["lstMunicipios"] = iqrMunicipios;
                    break;
                case 2:
                    iqrMunicipios = oblMunicipios.BuscarEstados(int.Parse(cmbEstadoFormulario.SelectedValue));
                    Session["lstMunicipios"] = iqrMunicipios;
                    break;
            }            
        }

        /// <summary>
        /// Cargar el formulario de datos
        /// </summary>
        /// <param name="bDatos">True si será cargado con los datos de un row del gridview seleccionado, False si no</param>
        protected void CargarFormulario(bool bDatos)
        {
            pnlSeleccionable.Visible = true;
            upnFormulario.Visible = true;
            pnlCatalogo.Visible = false;
            pnlCatalogoSeleccionable.Visible = false;           

            ActualizarSesionEstados();

            cmbEstadoFormulario.Items.Clear();
            cmbEstadoFormulario.DataSource = (IQueryable<MedDAL.DAL.estados>)Session["lstEstados"];
            cmbEstadoFormulario.DataBind();

            if (cmbEstadoFormulario.Items.Count != 0) 
            {
                if (bDatos)
                {
                    cmbEstadoFormulario.SelectedValue = cmbEstadoCatalogo.SelectedValue;

                    ActualizarSesionMunicipios(2);

                    cmbMunicipioFormulario.Items.Clear();
                    cmbMunicipioFormulario.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstMunicipios"];
                    cmbMunicipioFormulario.DataBind();
                    if (cmbMunicipioFormulario.Items.Count != 0) 
                    {
                        cmbMunicipioFormulario.SelectedValue = cmbMunicipioCatalogo.SelectedValue;

                        txbClave.Enabled = false;
                        rfvClave.Enabled = false;
                        txbNombre.Enabled = true;
                        rfvNombre.Enabled = true;
                        ckbActivo.Visible = true;

                        txbClave.Text = gdvDatos.SelectedRow.Cells[1].Text;
                        txbNombre.Text = gdvDatos.SelectedRow.Cells[2].Text;
                        ckbActivo.Checked = ((CheckBox)gdvDatos.SelectedRow.Cells[3].FindControl("ctl01")).Checked;
                    }
                }
                else
                {
                    cmbEstadoFormulario.SelectedIndex = 0;

                    ActualizarSesionMunicipios(2);

                    cmbMunicipioFormulario.Items.Clear();
                    cmbMunicipioFormulario.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstMunicipios"];
                    cmbMunicipioFormulario.DataBind();
                    #region Interfaz
                    if (cmbMunicipioFormulario.Items.Count != 0)
                    {
                        cmbMunicipioFormulario.SelectedIndex = 0;

                        txbClave.Enabled = true;
                        rfvClave.Enabled = true;
                        txbNombre.Enabled = true;
                        rfvNombre.Enabled = true;
                        ckbActivo.Visible = false;

                        txbClave.Text = "";
                        txbNombre.Text = "";
                    }
                    else 
                    {
                        txbClave.Enabled = false;
                        rfvClave.Enabled = false;
                        txbNombre.Enabled = false;
                        rfvNombre.Enabled = false;
                        ckbActivo.Visible = false;

                        txbClave.Text = "";
                        txbNombre.Text = "";
                    }
                    #endregion
                }
            }            
        }        
        
        /// <summary>
        /// Cargar el catálogo de datos a su estado inicial
        /// </summary>
        protected void CargarCatalogo()
        {
            //movimientos de interfaz
            pnlSeleccionable.Visible = false;
            upnFormulario.Visible = false;
            pnlCatalogo.Visible = true;
            pnlCatalogoSeleccionable.Visible = true;            

            ActualizarSesionEstados();
                        
            cmbEstadoCatalogo.Items.Clear();            
            cmbEstadoCatalogo.DataSource = (IQueryable<MedDAL.DAL.estados>)Session["lstEstados"];
            cmbEstadoCatalogo.DataBind();
            
            if (cmbEstadoCatalogo.Items.Count != 0) 
            { 
                cmbEstadoCatalogo.SelectedIndex = 0;

                ActualizarSesionMunicipios(1);

                cmbMunicipioCatalogo.Items.Clear();
                cmbMunicipioCatalogo.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstMunicipios"];
                cmbMunicipioCatalogo.DataBind();
                if (cmbMunicipioCatalogo.Items.Count != 0) cmbMunicipioCatalogo.SelectedIndex = 0;
            }
        }

        protected void Buscar(string sCadena, int iIdMunicipio)
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
            else if (rdbNombre.Checked)
            {
                iTipo = 3;
            }

            var oQuery = oblPoblaciones.Buscar(sCadena, iIdMunicipio, iTipo);
            Session["resultadoquery"] = oQuery;

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Poblaciones.PoblacionesView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Clave ASC";

            try
            {
                gdvDatos.DataSource = dv;
                gdvDatos.DataKeyNames = new string[] { "idPoblacion" };
                gdvDatos.DataBind();
                gdvDatos.Visible = true;
                if (txbBuscar.Text == "")
                {
                    gdvDatos.EmptyDataText = "No existen poblaciones registradas aun";
                }
                else
                {
                    gdvDatos.EmptyDataText = "No existen poblaciones que coincidan con la búsqueda";
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
            //Crear objeto y poblarlo con los datos de la GUI
            oPoblacion = new MedDAL.DAL.poblaciones();
            oPoblacion.Clave = txbClave.Text;
            oPoblacion.Nombre = txbNombre.Text;
            oPoblacion.Activo = true;

            //se obtiene la lista de estados y se asigna la EntityKey a la población a dar de alta.
            List<MedDAL.DAL.estados> lstEstados = new List<MedDAL.DAL.estados>();
            lstEstados.AddRange((IQueryable<MedDAL.DAL.estados>)Session["lstEstados"]);
            oPoblacion.idEstado = lstEstados[cmbEstadoFormulario.SelectedIndex].idEstado;

            //se obtiene la lista de municipio y se asigna la EntityKey a la población a dar de alta.
            List<MedDAL.DAL.municipios> lstMunicipios = new List<MedDAL.DAL.municipios>();
            lstMunicipios.AddRange((IQueryable<MedDAL.DAL.municipios>)Session["lstMunicipios"]);
            oPoblacion.idMunicipio = lstMunicipios[cmbMunicipioFormulario.SelectedIndex].idMunicipio;

            //Si el registro del municipio es exitoso, registrar en la bitácora.            
            if (oblPoblaciones.NuevoRegistro(oPoblacion))
            {
                lblAviso.Text = "La población se ha registrado con éxito";
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Poblaciones";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Nueva Población";
                oBitacora.Descripcion = "Clave: " + txbClave.Text + ", Nombre: " + txbNombre.Text;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "La población no pudo ser registrada";
            }
        }

        protected void Editar()
        {
            oPoblacion = new MedDAL.DAL.poblaciones();
            oPoblacion.idPoblacion = int.Parse(gdvDatos.SelectedDataKey.Value.ToString());
            oPoblacion.Clave = txbClave.Text;
            oPoblacion.Nombre = txbNombre.Text;
            oPoblacion.Activo = ckbActivo.Checked;

            //se obtiene la lista de estados y se asigna la EntityKey a la población a modificar.
            List<MedDAL.DAL.estados> lstEstados = new List<MedDAL.DAL.estados>();
            lstEstados.AddRange((IQueryable<MedDAL.DAL.estados>)Session["lstEstados"]);
            oPoblacion.idEstado = lstEstados[cmbEstadoFormulario.SelectedIndex].idEstado;

            //se obtiene la lista de municipios y se asigna la EntityKey a la población a modificar.
            List<MedDAL.DAL.municipios> lstMunicipios = new List<MedDAL.DAL.municipios>();
            lstMunicipios.AddRange((IQueryable<MedDAL.DAL.municipios>)Session["lstMunicipios"]);
            oPoblacion.idMunicipio = lstMunicipios[cmbMunicipioFormulario.SelectedIndex].idMunicipio;

            if (oblPoblaciones.EditarRegistro(oPoblacion))
            {
                lblAviso.Text = "La población ha sido actualizada con éxito";
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Poblaciones";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Población Actualizada";
                oBitacora.Descripcion = "Clave: " + txbClave.Text + ", Nombre: " + txbNombre.Text;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "La población no pudo ser actualizada";
            }
        }

        protected void Eliminar()
        {
            oPoblacion = new MedDAL.DAL.poblaciones();
            string sClave = gdvDatos.SelectedRow.Cells[2].Text;
            string sNombre = gdvDatos.SelectedRow.Cells[3].Text;
            oPoblacion.idPoblacion = int.Parse(gdvDatos.SelectedDataKey.Value.ToString());
            if (oblPoblaciones.EliminarRegistro(oPoblacion))
            {
                lblAviso.Text = "La población fue eliminada";
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Poblaciones";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Población Eliminada";
                oBitacora.Descripcion = "Clave: " + sClave + ", Nombre: " + sNombre;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "La población no pudo ser eliminada, es posible que tenga datos relacionados";
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
                cPermiso = (char)htbPermisos["poblaciones"];
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
                imbAceptar.ValidationGroup = "Poblaciones";
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
                lblNombreModulo.Text = "Poblaciones";

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
                oblPoblaciones = new BlPoblaciones();
                oblMunicipios = new MedNeg.Municipios.BlMunicipios();
                oblEstados = new MedNeg.Estados.BlEstados();
                oblBitacora = new MedNeg.Bitacora.BlBitacora();

                if (!IsPostBack)
                {
                    Session["poblacionesaccion"] = 0;
                    pnlCatalogo.Visible = false;
                    upnFormulario.Visible = false;
                    Session["resultadoquery"] = "";
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;


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
                pnlCatalogo.Visible = false;
                upnFormulario.Visible = false;
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }               

        protected void imbNuevo_Click(object sender, EventArgs e)
        {
            CargarFormulario(false);
            gdvDatos.SelectedIndex = -1;
            Session["poblacionesaccion"] = 1;
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
                Session["poblacionesaccion"] = 2;
                //0175 GT
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);
            }
            else
            {
                CargarCatalogo();
                //0175 GT
                ConfigurarMenuBotones(true, true, true, false, false, false, true, true);
                Buscar("", int.Parse(cmbMunicipioCatalogo.SelectedValue));
            }
            lblAviso.Text = "";
            lblAviso2.Text = "";
        }
        protected void imbEliminar_Click(object sender, EventArgs e)
        {
            if (pnlCatalogo.Visible && gdvDatos.SelectedIndex != -1)
            {
                Eliminar();
                Buscar(txbBuscar.Text, int.Parse(cmbMunicipioCatalogo.SelectedValue));
                //CargarCatalogo();
            }
            else
            {
                CargarCatalogo();
                Buscar(txbBuscar.Text, int.Parse(cmbMunicipioCatalogo.SelectedValue));
            }
        }
        protected void imbMostrar_Click(object sender, EventArgs e)
        {
            CargarCatalogo();

            //Si no se tienen datos en cmbMunicipioCatalogo, se utiliza el valor 0 como default
            //Lo cual provoca que aparezca el mensaje "No existen poblaciones aun"
            if (cmbMunicipioCatalogo.Items.Count != 0)
            {
                Buscar("", int.Parse(cmbMunicipioCatalogo.SelectedValue));
                //0175 GT
                ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
            }
            else Buscar("", 0);

            gdvDatos.SelectedIndex = -1;
            Session["poblacionesaccion"] = 0;
            lblAviso.Text = "";
            lblAviso2.Text = "";
            
        }
        protected void imbAceptar_Click(object sender, EventArgs e)
        {
            int iAccion;
            if (Session["poblacionesaccion"] != null)
            {
                iAccion = (int)Session["poblacionesaccion"];
            }
            else iAccion = 0;
            switch (iAccion)
            {
                case 0:
                    break;
                case 1:
                    if (txbClave.Enabled && cmvClave.IsValid)
                    {
                        Nuevo();
                        CargarFormulario(false);
                        //GT 0175
                        ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    }
                    Session["poblacionesaccion"] = 1;
                    break;
                case 2:
                    if (txbNombre.Enabled) Editar();
                    //CargarFormulario(true);                    
                    //Buscar("", int.Parse(cmbMunicipioCatalogo.SelectedValue));
                    Session["poblacionesaccion"] = 2;
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    break;
            }
        }
        
        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            Session["poblacionesaccion"] = 0;
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
            pnlCatalogo.Visible = false;
            upnFormulario.Visible = false;
        }

        protected void gdvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (pnlCatalogo.Visible)
            {
                Buscar(txbBuscar.Text, int.Parse(cmbMunicipioCatalogo.SelectedValue));
                ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
            }
        }

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
        }

        protected void cmbEstadoCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarSesionMunicipios(1);
            cmbMunicipioCatalogo.Items.Clear();
            cmbMunicipioCatalogo.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstMunicipios"];
            cmbMunicipioCatalogo.DataBind();
            if (cmbMunicipioCatalogo.Items.Count != 0)
            {
                cmbMunicipioCatalogo.SelectedIndex = 0;
                Buscar("", int.Parse(cmbMunicipioCatalogo.SelectedValue));
            }
            else
            {
                gdvDatos.DataSource = null;
                gdvDatos.DataBind();
                cmbMunicipioCatalogo.ClearSelection();
                cmbMunicipioCatalogo.Items.Clear();
                ((TextBox)cmbMunicipioCatalogo.FindControl("TextBox")).Text = "";
            }
        }

        protected void cmbMunicipioCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Buscar("", int.Parse(cmbMunicipioCatalogo.SelectedValue));
        }

        protected void cmbEstadoFormulario_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarSesionMunicipios(2);

            cmbMunicipioFormulario.Items.Clear();
            cmbMunicipioFormulario.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstMunicipios"];
            cmbMunicipioFormulario.DataBind();
            if (cmbMunicipioFormulario.Items.Count != 0)
            { 
                cmbMunicipioFormulario.SelectedIndex = 0;

                if ((int)Session["poblacionesaccion"] == 1) 
                {
                    txbClave.Enabled = true;
                    rfvClave.Enabled = true;
                    txbNombre.Enabled = true;
                    rfvNombre.Enabled = true;
                    ckbActivo.Visible = false;
                }
                else if ((int)Session["poblacionesaccion"] == 2)
                {
                    txbClave.Enabled = false;
                    rfvClave.Enabled = false;
                    txbNombre.Enabled = true;
                    rfvNombre.Enabled = true;
                    ckbActivo.Visible = true;
                }
                
            }
            else
            {
                cmbMunicipioFormulario.ClearSelection();
                cmbMunicipioFormulario.Items.Clear();
                ((TextBox)cmbMunicipioFormulario.FindControl("TextBox")).Text = "";

                txbClave.Enabled = false;
                rfvClave.Enabled = false;
                txbNombre.Enabled = false;
                rfvNombre.Enabled = false;
                ckbActivo.Visible = false;
            }
        }

        protected void cmvClave_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (int.Parse(Session["poblacionesaccion"].ToString()) != 2)
            {
                string sClave = args.Value.ToString();
                MedDAL.DAL.poblaciones oPoblacion = oblPoblaciones.Buscar(sClave);
                args.IsValid = oPoblacion == null ? true : false;
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
            pnlSeleccionable.Visible = false;
            upnFormulario.Visible = false;
            pnlCatalogo.Visible = false;
            pnlCatalogoSeleccionable.Visible = false;            

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from estados", "medicuriConnectionString", odsDataSet, "estados");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from municipios", "medicuriConnectionString", odsDataSet, "municipios");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from poblaciones", "medicuriConnectionString", odsDataSet, "poblaciones");

            Session["campoaordenar"] = "";
            Session["dataset"] = odsDataSet;
            Session["titulo"] = "Impresión de Poblaciones";
            //Session["configuracionsistema"] = objConfiguracion;
            Session["sortfield"] = 0;
            Session["reportdocument"] = "~\\rptReportes\\rptPoblaciones.rpt";

            if (gdvDatos.SelectedIndex != -1)
            {
                Session["recordselection"] = "{poblaciones.idPoblacion}=" + gdvDatos.SelectedDataKey.Values[0].ToString();
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

        #region SortingPaging

        protected void gdvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Poblaciones.PoblacionesView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            gdvDatos.DataSource = oMaster.Sorting(e, ref oDireccion, dv);
            ViewState["direccionsorting"] = oDireccion;
            ViewState["sortexpression"] = e.SortExpression;
            gdvDatos.DataBind(); 
        }

        protected void gdvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Poblaciones.PoblacionesView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            gdvDatos.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Clave" : ViewState["sortexpression"].ToString(), dv, ref gdvDatos, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            gdvDatos.DataBind();
        }

        #endregion

    }
}