using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MedNeg.Colonias;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Medicuri
{
    public partial class Colonias : System.Web.UI.Page
    {
        ImageButton imbNuevo, imbEditar, imbEliminar, imbImprimir, imbMostrar, imbAceptar, imbCancelar, imbReportes;
        RadioButton rdbNombre, rdbClave, rdbTodos;
        Button btnBuscar;
        TextBox txbBuscar;
        Label lblNombreModulo;
        IQueryable<MedDAL.DAL.estados> iqrEstados;
        IQueryable<MedDAL.DAL.municipios> iqrMunicipios;
        IQueryable<MedDAL.DAL.poblaciones> iqrPoblaciones;
        MedNeg.Colonias.BlColonias oblColonias;
        MedNeg.Poblaciones.BlPoblaciones oblPoblaciones;
        MedNeg.Municipios.BlMunicipios oblMunicipios;
        MedNeg.Estados.BlEstados oblEstados;
        MedNeg.Bitacora.BlBitacora oblBitacora;
        MedDAL.DAL.colonias oColonia;
        MedDAL.DAL.bitacora oBitacora;
        
        //MedDAL.DAL.medicuriEntities oMedicuriEntities;

        /// <summary>
        /// Activa textboxes y checkbox si es necesario
        /// </summary>
        protected void ActivarControlesInput()
        {
            if ((int)Session["coloniasaccion"] == 1)
            {
                txbClave.Enabled = true;
                rfvClave.Enabled = true;
                txbNombre.Enabled = true;
                rfvNombre.Enabled = true;
                ckbActivo.Visible = false;
            }
            else if ((int)Session["coloniasaccion"] == 2)
            {
                txbClave.Enabled = false;
                rfvClave.Enabled = false;
                txbNombre.Enabled = true;
                rfvNombre.Enabled = true;
                ckbActivo.Visible = true;
            }
        }

        /// <summary>
        /// Desactiva textboxes y checbox
        /// </summary>
        protected void DesactivarControlesInput()
        {
            txbClave.Enabled = false;
            rfvClave.Enabled = false;
            txbNombre.Enabled = false;
            rfvNombre.Enabled = false;
            ckbActivo.Visible = false;
        }

        /// <summary>
        /// Actualiza la variable de sesion "lstestadoscolonias", la cual es una lista de los estados activos
        /// </summary>
        protected void ActualizarSesionEstados()
        {
            iqrEstados = oblEstados.BuscarEnum();
            Session["lstestadoscolonias"] = iqrEstados;
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
                    Session["lstmunicipioscolonias"] = iqrMunicipios;
                    break;
                case 2:
                    iqrMunicipios = oblMunicipios.BuscarEstados(int.Parse(cmbEstadoFormulario.SelectedValue));
                    Session["lstmunicipioscolonias"] = iqrMunicipios;
                    break;
            }
        }

        /// <summary>
        /// Actualiza la variable de sesión de Poblaciones dependiendo de los Municipios existentes.
        /// </summary>
        /// <param name="iTipo">1=De los municipios en catalogo, 2=De los municipios en formulario"</param>
        protected void ActualizarSesionPoblaciones(int iTipo)
        {
            switch (iTipo)
            {
                case 1:
                    iqrPoblaciones = oblPoblaciones.BuscarMunicipios(int.Parse(cmbMunicipioCatalogo.SelectedValue));
                    Session["lstpoblacionescolonias"] = iqrPoblaciones;
                    break;
                case 2:
                    iqrPoblaciones = oblPoblaciones.BuscarMunicipios(int.Parse(cmbMunicipioFormulario.SelectedValue));
                    Session["lstpoblacionescolonias"] = iqrPoblaciones;
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
            cmbEstadoFormulario.DataSource = (IQueryable<MedDAL.DAL.estados>)Session["lstestadoscolonias"];
            cmbEstadoFormulario.DataBind();
            //Determinar si se tienen estados registrados
            if (cmbEstadoFormulario.Items.Count != 0)
            {
                if (bDatos)
                {
                    cmbEstadoFormulario.SelectedValue = cmbEstadoCatalogo.SelectedValue;
                }
                else 
                {
                    cmbEstadoFormulario.SelectedIndex = 0;
                }
                
                ActualizarSesionMunicipios(2);

                cmbMunicipioFormulario.Items.Clear();
                cmbMunicipioFormulario.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstmunicipioscolonias"];
                cmbMunicipioFormulario.DataBind();
                //Determinar si se tienen Municipios registrados
                if (cmbMunicipioFormulario.Items.Count != 0)
                {
                    if (bDatos)
                    {
                        cmbMunicipioFormulario.SelectedValue = cmbMunicipioCatalogo.SelectedValue;

                        ActualizarSesionPoblaciones(2);

                        cmbPoblacionFormulario.Items.Clear();
                        cmbPoblacionFormulario.DataSource = (IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblacionescolonias"];
                        cmbPoblacionFormulario.DataBind();
                        //Determinar si se tienen Poblaciones registradas
                        if (cmbPoblacionFormulario.Items.Count != 0)
                        {
                            cmbPoblacionFormulario.SelectedValue = cmbPoblacionCatalogo.SelectedValue;

                            txbClave.Enabled = false;
                            rfvClave.Enabled = false;
                            txbNombre.Enabled = true;
                            rfvNombre.Enabled = true;
                            ckbActivo.Visible = true;

                            txbClave.Text = gdvDatos.SelectedRow.Cells[1].Text;
                            txbNombre.Text = gdvDatos.SelectedRow.Cells[2].Text;
                            ckbActivo.Checked = ((CheckBox)gdvDatos.SelectedRow.Cells[3].FindControl("ctl01")).Checked;
                        }
                        else 
                        {
                            cmbPoblacionFormulario.ClearSelection();
                            cmbPoblacionFormulario.Items.Clear();
                            ((TextBox)cmbPoblacionFormulario.FindControl("TextBox")).Text = "";
                        }
                    }
                    else
                    {
                        cmbMunicipioFormulario.SelectedIndex = 0;

                        ActualizarSesionPoblaciones(2);

                        cmbPoblacionFormulario.Items.Clear();
                        cmbPoblacionFormulario.DataSource = (IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblacionescolonias"];
                        cmbPoblacionFormulario.DataBind();
                        #region Interfaz
                        if (cmbPoblacionFormulario.Items.Count != 0)
                        {
                            cmbPoblacionFormulario.SelectedIndex = 0;

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
            cmbEstadoCatalogo.DataSource = (IQueryable<MedDAL.DAL.estados>)Session["lstestadoscolonias"];
            cmbEstadoCatalogo.DataBind();

            if (cmbEstadoCatalogo.Items.Count != 0)
            {
                cmbEstadoCatalogo.SelectedIndex = 0;

                ActualizarSesionMunicipios(1);

                cmbMunicipioCatalogo.Items.Clear();
                cmbMunicipioCatalogo.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstmunicipioscolonias"];
                cmbMunicipioCatalogo.DataBind();
                if (cmbMunicipioCatalogo.Items.Count != 0)
                {
                    cmbMunicipioCatalogo.SelectedIndex = 0;

                    ActualizarSesionPoblaciones(1);

                    cmbPoblacionCatalogo.Items.Clear();
                    cmbPoblacionCatalogo.DataSource = (IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblacionescolonias"];
                    cmbPoblacionCatalogo.DataBind();

                    if (cmbPoblacionCatalogo.Items.Count != 0)
                    {
                        cmbPoblacionCatalogo.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbPoblacionCatalogo.ClearSelection();
                        cmbPoblacionCatalogo.Items.Clear();
                        ((TextBox)cmbPoblacionCatalogo.FindControl("TextBox")).Text = "";
                    }
                }
                else 
                {
                    cmbMunicipioCatalogo.ClearSelection();
                    cmbMunicipioCatalogo.Items.Clear();
                    ((TextBox)cmbMunicipioCatalogo.FindControl("TextBox")).Text = "";

                    cmbPoblacionCatalogo.ClearSelection();
                    cmbPoblacionCatalogo.Items.Clear();
                    ((TextBox)cmbPoblacionCatalogo.FindControl("TextBox")).Text = "";
                }
            }
        }

        protected void Buscar(string sCadena, int iIdPoblacion)
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

            var oQuery = oblColonias.Buscar(sCadena, iIdPoblacion, iTipo);
            Session["resultadoquery"] = oQuery;
            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Colonias.ColoniasView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Clave ASC";
            gdvDatos.DataSource = dv;

            try
            {
                //gdvDatos.DataSource = oQuery;
                gdvDatos.DataKeyNames = new string[] { "idColonia" };
                gdvDatos.DataBind();
                gdvDatos.Visible = true;
                if (txbBuscar.Text == "")
                {
                    gdvDatos.EmptyDataText = "No existen colonias registradas aun";
                }
                else
                {
                    gdvDatos.EmptyDataText = "No existen colonias que coincidan con la búsqueda";
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
            oColonia = new MedDAL.DAL.colonias();
            oColonia.Clave = txbClave.Text;
            oColonia.Nombre = txbNombre.Text;
            oColonia.Activo = true;

            //se obtiene la lista de estados y se asigna la EntityKey a la población a modificar.
            List<MedDAL.DAL.estados> lstEstados = new List<MedDAL.DAL.estados>();
            lstEstados.AddRange((IQueryable<MedDAL.DAL.estados>)Session["lstestadoscolonias"]);
            oColonia.IdEstado = lstEstados[cmbEstadoFormulario.SelectedIndex].idEstado;

            //se obtiene la lista de municipios y se asigna la EntityKey a la población a modificar.
            List<MedDAL.DAL.municipios> lstMunicipios = new List<MedDAL.DAL.municipios>();
            lstMunicipios.AddRange((IQueryable<MedDAL.DAL.municipios>)Session["lstmunicipioscolonias"]);
            oColonia.IdMunicipio = lstMunicipios[cmbMunicipioFormulario.SelectedIndex].idMunicipio;

            //se obtiene la lista de poblaciones y se asigna la EntityKey a la colonia a dar de alta.
            List<MedDAL.DAL.poblaciones> lstPoblaciones = new List<MedDAL.DAL.poblaciones>();
            lstPoblaciones.AddRange((IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblacionescolonias"]);
            oColonia.IdPoblacion = lstPoblaciones[cmbPoblacionFormulario.SelectedIndex].idPoblacion;

            //Si el registro de la colonia es exitoso, registrar en la bitácora.
            if (oblColonias.NuevoRegistro(oColonia))
            {
                lblAviso.Text = "La Colonia se ha registrado con éxito";
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Colonias";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Nueva Colonia";
                oBitacora.Descripcion = "Clave: " + txbClave.Text + ", Nombre: " + txbNombre.Text;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "La colonia no pudo ser registrada";
            }
        }

        protected void Editar()
        {
            oColonia = new MedDAL.DAL.colonias();
            oColonia.idColonia = int.Parse(gdvDatos.SelectedDataKey.Value.ToString());
            oColonia.Clave = txbClave.Text;
            oColonia.Nombre = txbNombre.Text;
            oColonia.Activo = ckbActivo.Checked;

            //se obtiene la lista de estados y se asigna la EntityKey a la población a modificar.
            List<MedDAL.DAL.estados> lstEstados = new List<MedDAL.DAL.estados>();
            lstEstados.AddRange((IQueryable<MedDAL.DAL.estados>)Session["lstestadoscolonias"]);
            oColonia.IdEstado = lstEstados[cmbEstadoFormulario.SelectedIndex].idEstado;

            //se obtiene la lista de municipios y se asigna la EntityKey a la población a modificar.
            List<MedDAL.DAL.municipios> lstMunicipios = new List<MedDAL.DAL.municipios>();
            lstMunicipios.AddRange((IQueryable<MedDAL.DAL.municipios>)Session["lstmunicipioscolonias"]);
            oColonia.IdMunicipio = lstMunicipios[cmbMunicipioFormulario.SelectedIndex].idMunicipio;

            //se obtiene la lista de poblaciones y se asigna la EntityKey a la colonia a dar de alta.
            List<MedDAL.DAL.poblaciones> lstPoblaciones = new List<MedDAL.DAL.poblaciones>();
            lstPoblaciones.AddRange((IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblacionescolonias"]);
            oColonia.IdPoblacion = lstPoblaciones[cmbPoblacionFormulario.SelectedIndex].idPoblacion;

            //Si la actualización de la colonia es exitoso, registrar en la bitácora.
            if (oblColonias.EditarRegistro(oColonia))
            {
                lblAviso.Text = "La colonia ha sido actualizada con éxito";
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Colonias";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Colonia Actualizada";
                oBitacora.Descripcion = "Clave: " + txbClave.Text + ", Nombre: " + txbNombre.Text;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "La colonia no pudo ser actualizada";
            }
        }

        protected void Eliminar()
        {
            oColonia = new MedDAL.DAL.colonias();
            string sClave = gdvDatos.SelectedRow.Cells[2].Text;
            string sNombre = gdvDatos.SelectedRow.Cells[3].Text;
            oColonia.idColonia = int.Parse(gdvDatos.SelectedDataKey.Value.ToString());
            if (oblColonias.EliminarRegistro(oColonia))
            {
                lblAviso.Text = "La colonia fue eliminada";
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Colonia";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Colonia Eliminada";
                oBitacora.Descripcion = "Clave: " + sClave + ", Nombre: " + sNombre;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "La colonia no pudo ser eliminada, es posible que tenga datos relacionados";
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
                cPermiso = (char)htbPermisos["colonias"];

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
                imbAceptar.ValidationGroup = "Colonias";
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
                lblNombreModulo.Text = "Colonias";

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
                oblColonias = new BlColonias();
                oblPoblaciones = new MedNeg.Poblaciones.BlPoblaciones();
                oblMunicipios = new MedNeg.Municipios.BlMunicipios();
                oblEstados = new MedNeg.Estados.BlEstados();
                oblBitacora = new MedNeg.Bitacora.BlBitacora();

                if (!IsPostBack)
                {
                    Session["coloniasaccion"] = 0;
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
            Session["coloniasaccion"] = 1;
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
                Session["coloniasaccion"] = 2;
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
                Buscar(txbBuscar.Text, int.Parse(cmbPoblacionCatalogo.SelectedValue));
                //CargarCatalogo();
            }
            else
            {
                CargarCatalogo();
                Buscar(txbBuscar.Text, int.Parse(cmbPoblacionCatalogo.SelectedValue));
            }
        }
        protected void imbAceptar_Click(object sender, EventArgs e)
        {
            int iAccion;
            if (Session["coloniasaccion"] != null)
            {
                iAccion = (int)Session["coloniasaccion"];
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
                    Session["coloniasaccion"] = 1;
                    break;
                case 2:
                    if (txbNombre.Enabled) Editar();
                    //CargarFormulario(true);                    
                    //Buscar("", int.Parse(cmbMunicipioCatalogo.SelectedValue));
                    Session["coloniasaccion"] = 2;
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    break;
            }
        }
        protected void imbMostrar_Click(object sender, EventArgs e)
        {
            CargarCatalogo();

            //Si no se tienen datos en cmbMunicipioCatalogo, se utiliza el valor 0 como default
            //Lo cual provoca que aparezca el mensaje "No existen poblaciones aun"
            if (cmbPoblacionCatalogo.Items.Count != 0)
            {
                Buscar("", int.Parse(cmbPoblacionCatalogo.SelectedValue));
                //0175 GT
                ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
            }
            else Buscar("", 0);

            gdvDatos.SelectedIndex = -1;
            Session["coloniasaccion"] = 0;
            lblAviso.Text = "";
            lblAviso2.Text = "";
        }
        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            Session["coloniasaccion"] = 0;
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
            pnlCatalogo.Visible = false;
            upnFormulario.Visible = false;
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (pnlCatalogo.Visible)
            {
                Buscar(txbBuscar.Text, int.Parse(cmbPoblacionCatalogo.SelectedValue));
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
            lblAviso.Text = "";
            lblAviso2.Text = "";

            ActualizarSesionMunicipios(1);

            cmbMunicipioCatalogo.Items.Clear();
            cmbMunicipioCatalogo.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstmunicipioscolonias"];
            cmbMunicipioCatalogo.DataBind();

            if (cmbMunicipioCatalogo.Items.Count != 0) 
            { 
                cmbMunicipioCatalogo.SelectedIndex = 0;

                ActualizarSesionPoblaciones(1);

                cmbPoblacionCatalogo.Items.Clear();
                cmbPoblacionCatalogo.DataSource = (IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblacionescolonias"];
                cmbPoblacionCatalogo.DataBind();

                if (cmbPoblacionCatalogo.Items.Count != 0)
                {
                    cmbPoblacionCatalogo.SelectedIndex = 0;
                    Buscar("", int.Parse(cmbPoblacionCatalogo.SelectedValue));
                }
                else 
                {
                    gdvDatos.DataSource = null;
                    gdvDatos.DataBind();

                    cmbPoblacionCatalogo.ClearSelection();
                    cmbPoblacionCatalogo.Items.Clear();
                    ((TextBox)cmbPoblacionCatalogo.FindControl("TextBox")).Text = "";
                }
            }
            else
            {
                gdvDatos.DataSource = null;
                gdvDatos.DataBind();

                cmbMunicipioCatalogo.ClearSelection();
                cmbMunicipioCatalogo.Items.Clear();
                ((TextBox)cmbMunicipioCatalogo.FindControl("TextBox")).Text = "";

                cmbPoblacionCatalogo.ClearSelection();
                cmbPoblacionCatalogo.Items.Clear();
                ((TextBox)cmbPoblacionCatalogo.FindControl("TextBox")).Text = "";
            }
        }
        protected void cmbMunicipioCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblAviso.Text = "";
            lblAviso2.Text = "";

            ActualizarSesionPoblaciones(1);

            cmbPoblacionCatalogo.Items.Clear();
            cmbPoblacionCatalogo.DataSource = (IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblacionescolonias"];
            cmbPoblacionCatalogo.DataBind();

            if (cmbPoblacionCatalogo.Items.Count != 0)
            {
                cmbPoblacionCatalogo.SelectedIndex = 0;

                Buscar("", int.Parse(cmbPoblacionCatalogo.SelectedValue));
            }
            else
            {
                gdvDatos.DataSource = null;
                gdvDatos.DataBind();

                cmbPoblacionCatalogo.ClearSelection();
                cmbPoblacionCatalogo.Items.Clear();
                ((TextBox)cmbPoblacionCatalogo.FindControl("TextBox")).Text = "";
            }
        }
        protected void cmbPoblacionCatalogo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblAviso.Text = "";
            lblAviso2.Text = "";

            Buscar("", int.Parse(cmbPoblacionCatalogo.SelectedValue));
        }
        protected void cmbEstadoFormulario_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblAviso.Text = "";
            lblAviso2.Text = "";

            ActualizarSesionMunicipios(2);

            cmbMunicipioFormulario.Items.Clear();
            cmbMunicipioFormulario.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstmunicipioscolonias"];
            cmbMunicipioFormulario.DataBind();

            if (cmbMunicipioFormulario.Items.Count != 0)
            {
                cmbMunicipioFormulario.SelectedIndex = 0;

                ActualizarSesionPoblaciones(2);

                cmbPoblacionFormulario.Items.Clear();
                cmbPoblacionFormulario.DataSource = (IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblacionescolonias"];
                cmbPoblacionFormulario.DataBind();

                if (cmbPoblacionFormulario.Items.Count != 0)
                {
                    cmbPoblacionFormulario.SelectedIndex = 0;

                    ActivarControlesInput();
                }
                else 
                {
                    cmbPoblacionFormulario.ClearSelection();
                    cmbPoblacionFormulario.Items.Clear();
                    ((TextBox)cmbPoblacionFormulario.FindControl("TextBox")).Text = "";

                    DesactivarControlesInput();
                }
            }
            else
            {
                cmbMunicipioFormulario.ClearSelection();
                cmbMunicipioFormulario.Items.Clear();
                ((TextBox)cmbMunicipioFormulario.FindControl("TextBox")).Text = "";

                cmbPoblacionFormulario.ClearSelection();
                cmbPoblacionFormulario.Items.Clear();
                ((TextBox)cmbPoblacionFormulario.FindControl("TextBox")).Text = "";

                DesactivarControlesInput();
            }
        }
        protected void cmbMunicipioFormulario_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblAviso.Text = "";
            lblAviso2.Text = "";

            ActualizarSesionPoblaciones(2);

            cmbPoblacionFormulario.Items.Clear();
            cmbPoblacionFormulario.DataSource = (IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblacionescolonias"];
            cmbPoblacionFormulario.DataBind();

            if (cmbPoblacionFormulario.Items.Count != 0)
            {
                cmbPoblacionFormulario.SelectedIndex = 0;

                ActivarControlesInput();
            }
            else
            {
                cmbPoblacionFormulario.ClearSelection();
                cmbPoblacionFormulario.Items.Clear();
                ((TextBox)cmbPoblacionFormulario.FindControl("TextBox")).Text = "";

                DesactivarControlesInput();
            }
        }

        protected void gdvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void cmvClave_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (int.Parse(Session["coloniasaccion"].ToString()) != 2)
            {
                string sClave = args.Value.ToString();
                MedDAL.DAL.colonias oColonia = oblColonias.Buscar(sClave);
                args.IsValid = oColonia == null ? true : false;
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
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from colonias", "medicuriConnectionString", odsDataSet, "colonias");

            Session["campoaordenar"] = "";
            Session["dataset"] = odsDataSet;
            Session["titulo"] = "Impresión de Colonias";
            //Session["configuracionsistema"] = objConfiguracion;
            Session["sortfield"] = 0;
            Session["reportdocument"] = "~\\rptReportes\\rptColonias.rpt";

            if (gdvDatos.SelectedIndex != -1)
            {
                Session["recordselection"] = "{colonias.idColonia}=" + gdvDatos.SelectedDataKey.Values[0].ToString();
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
            var result = (IQueryable<MedDAL.Colonias.ColoniasView>)Session["resultadoquery"];
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
            var result = (IQueryable<MedDAL.Colonias.ColoniasView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            gdvDatos.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Clave" : ViewState["sortexpression"].ToString(), dv, ref gdvDatos, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            gdvDatos.DataBind();
        }

        #endregion
    }
}