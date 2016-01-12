using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using AjaxControlToolkit;
using System.Drawing;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Medicuri
{
    public partial class Vendedores : System.Web.UI.Page
    {
        #region Variables/Objetos
        const string cnsSinEstados = "Sin estados...", cnsSinMunicipios = "Sin municipios...", cnsSinPoblaciones = "Sin poblaciones...", cnsSinColonias = "Sin colonias...";
        Label lblNombreModulo;
        ImageButton imbNuevo, imbEditar, imbEliminar, imbMostrar, imbReportes, imbImprimir, imbAceptar, imbCancelar;
        RadioButton rdbNombre, rdbClave, rdbTipo;
        Button btnBuscar;
        TextBox txbBuscar;

        //Colecciones de objetos de los Entities
        IQueryable<MedDAL.DAL.estados> iqrEstados;
        IQueryable<MedDAL.DAL.municipios> iqrMunicipios;
        IQueryable<MedDAL.DAL.poblaciones> iqrPoblaciones;
        IQueryable<MedDAL.DAL.colonias> iqrColonias;
        IQueryable<MedDAL.DAL.tipos> iqrTipos;

        //Objetos de la capa de negocios
        MedNeg.Colonias.BlColonias oblColonias;
        MedNeg.Poblaciones.BlPoblaciones oblPoblaciones;
        MedNeg.Municipios.BlMunicipios oblMunicipios;
        MedNeg.Estados.BlEstados oblEstados;
        MedNeg.Tipos.BlTipos oblTipos;
        MedNeg.VendedorEspecialidad.BlVendedorEspecialidad oblEspecialidad;
        MedNeg.VendedorVinculacion.BlVendedorVinculacion oblVinculacion;
        MedNeg.Vendedores.BlVendedores oblVendedores;
        MedNeg.Bitacora.BlBitacora oblBitacora;

        //Objetos de las entities
        MedDAL.DAL.bitacora oBitacora;
        MedDAL.DAL.vendedores oVendedor;
        MedDAL.DAL.vendedores_especialidad oEspecialidad;
        MedDAL.DAL.vendedores_vinculacion oVinculacion;
        #endregion

        #region Configuraciones de inicio
        protected void Page_Load(object sender, EventArgs e)
        {
            //Asignar titulo de modulo
            lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
            lblNombreModulo.Text = "Vendedores";

            //Cargar permisos
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';
            try
            {
                //Obtener los controles de master.
                cPermiso = (char)htbPermisos["vendedores"];
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
                imbReportes = (ImageButton)Master.FindControl("imgBtnReportes");
                imbReportes.Click += new ImageClickEventHandler(this.imbReportes_Click);
                imbAceptar = (ImageButton)Master.FindControl("imgBtnAceptar");
                imbAceptar.Click += new ImageClickEventHandler(this.imbAceptar_Click);
                imbAceptar.ValidationGroup = "vendedorVG";
                imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
                imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
                rdbTipo = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbTipo.Text = "Tipo";
                rdbClave = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbClave.Text = "Clave";
                rdbNombre = (RadioButton)Master.FindControl("rdbFiltro3");
                rdbNombre.Text = "Nombre";
                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
                txbBuscar = (TextBox)Master.FindControl("txtBuscar");

                //GT 0175
                imbReportes = (ImageButton)Master.FindControl("imgBtnReportes");
                imbReportes.Click += new ImageClickEventHandler(this.imbReportes_Click);

                //Deshabilitar botones del toolbar
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

                //Inicializacion de objetos
                oblColonias = new MedNeg.Colonias.BlColonias();
                oblPoblaciones = new MedNeg.Poblaciones.BlPoblaciones();
                oblMunicipios = new MedNeg.Municipios.BlMunicipios();
                oblEstados = new MedNeg.Estados.BlEstados();
                oblTipos = new MedNeg.Tipos.BlTipos();
                oblBitacora = new MedNeg.Bitacora.BlBitacora();
                oblVinculacion = new MedNeg.VendedorVinculacion.BlVendedorVinculacion();
                oblEspecialidad = new MedNeg.VendedorEspecialidad.BlVendedorEspecialidad();
                oblVendedores = new MedNeg.Vendedores.BlVendedores();
                if (!IsPostBack)
                {
                    MostrarAreaTrabajo(false, false);
                    CargarCamposEditables();
                    Session["reporteactivo"] = 0;
                    Session["reportdocument"] = "";
                    Session["titulo"] = "";
                    Session["resultadoquery"] = "";
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
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
                MostrarAreaTrabajo(false, false);
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }
        #endregion

        #region EventHandlers
        public void imbNuevo_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            Session["modoGuardar"] = true;
            MostrarAreaTrabajo(false, true);
            ModificarControl(this.tabContainer, true, true);
            txbPais.Text = "México";
            chkActivo.Checked = true;
            txbFechaAlta.Text = DateTime.Now.ToShortDateString();
            chkActivo.Checked = true;
            CargarEstados(false);
            CargarCmbTipos(false);
            gdvDatos.SelectedIndex = -1;
            //0175 GT
            ConfigurarMenuBotones(true, false, false, false, true, true, false, false);
        }

        public void imbEditar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            if (gdvDatos.SelectedIndex != -1)
            {
                Session["modoGuardar"] = false;
                MostrarAreaTrabajo(false, true);
                Editar();
                //gdvDatos.SelectedIndex = -1;
                //0175 GT
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);
            }
            else
            {
                Buscar("");
                //0175 GT
                ConfigurarMenuBotones(true, true, true, false, false, false, true, true);
            }
        }

        public void imbEliminar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            if (pnlList.Visible && gdvDatos.SelectedIndex != -1)
                Eliminar((int)gdvDatos.SelectedValue);

            Buscar(txbBuscar.Text);
        }

        public void imbMostrar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            Buscar("");
            gdvDatos.SelectedIndex = -1;
            //0175 GT
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }

        public void imbAceptar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            if (cmvClave.IsValid)
            {
                if ((bool)Session["modoGuardar"])
                {
                    NuevoRegistro();
                }
                else
                {
                    EditarRegistro();
                }
                ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
            }
        }

        public void imbCancelar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            ModificarControl(this.tabContainer, false, true);
            MostrarAreaTrabajo(false, false);
            gdvDatos.SelectedIndex = -1;
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
        }

        public void btnBuscar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            Buscar(txbBuscar.Text);
            gdvDatos.SelectedIndex = -1;
            //0175 GT
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }

        protected void cmbPoblacionFormulario_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarColonias(false);
        }

        protected void cmbMunicipioFormulario_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPoblaciones(false);
        }

        protected void cmbEstadoFormulario_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarMunicipios(false);
        }

        protected void cmbTipoPersona_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTipoPersona.SelectedValue == "FISICA")
            {
                lblNombres.Text = "Nombre(s):";
                txbApellidos.Enabled = true;
                rfvApellidos.Enabled = true;
            }
            else
            {
                lblNombres.Text = "Razon Social:";
                txbApellidos.Enabled = false;
                rfvApellidos.Enabled = false;
            }
        }
        #endregion

        #region Cargar datos por omisión

        /// <summary>
        /// Actualiza la variable de sesion "lstestadosalmacenes", la cual es una lista de los estados activos
        /// </summary>
        protected void ActualizarSesionEstados()
        {
            IQueryable<MedDAL.DAL.estados> iqrEstados = oblEstados.BuscarEnum();
            Session["lstestados"] = iqrEstados;
        }

        /// <summary>
        /// Actualiza la variable de sesion de Municipios dependiendo de los Estados existentes.
        /// </summary>        
        protected void ActualizarSesionMunicipios()
        {
            IQueryable<MedDAL.DAL.municipios> iqrMunicipios = oblMunicipios.BuscarEstados(int.Parse(cmbEstados.SelectedValue));
            Session["lstmunicipios"] = iqrMunicipios;
        }

        /// <summary>
        /// Actualiza la variable de sesión de Poblaciones dependiendo de los Municipios existentes.
        /// </summary>        
        protected void ActualizarSesionPoblaciones()
        {
            IQueryable<MedDAL.DAL.poblaciones> iqrPoblaciones = oblPoblaciones.BuscarMunicipios(int.Parse(cmbMunicipios.SelectedValue));
            Session["lstpoblaciones"] = iqrPoblaciones;
        }

        /// <summary>
        /// Actualiza la variable de sesión de Colonias dependiendo de las Poblaciones existentes.
        /// </summary>
        protected void ActualizarSesionColonias()
        {
            IQueryable<MedDAL.DAL.colonias> iqrColonias = oblColonias.BuscarPoblaciones(int.Parse(cmbPoblaciones.SelectedValue));
            Session["lstcolonias"] = iqrColonias;
        }

        private void CargarEstados(bool bDatos)
        {
            ActualizarSesionEstados();

            cmbEstados.Items.Clear();
            cmbEstados.DataSource = (IQueryable<MedDAL.DAL.estados>)Session["lstestados"];
            cmbEstados.DataBind();

            if (cmbEstados.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbEstados.SelectedIndex = 0;
                }
                else
                {
                    cmbEstados.SelectedValue = gdvDatos.SelectedDataKey.Values[1].ToString();
                }
                CargarMunicipios(bDatos);
            }
            else
            {
                cmbEstados.Items.Add(cnsSinEstados);
                cmbMunicipios.ClearSelection();
                cmbMunicipios.Items.Clear();
                cmbMunicipios.Items.Add(cnsSinMunicipios);
                cmbPoblaciones.ClearSelection();
                cmbPoblaciones.Items.Clear();
                cmbPoblaciones.Items.Add(cnsSinPoblaciones);
                cmbColonias.ClearSelection();
                cmbColonias.Items.Clear();
                cmbColonias.Items.Add(cnsSinColonias);
            }
        }

        private void CargarMunicipios(bool bDatos)
        {
            ActualizarSesionMunicipios();

            cmbMunicipios.Items.Clear();
            cmbMunicipios.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstmunicipios"];
            cmbMunicipios.DataBind();

            if (cmbMunicipios.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbMunicipios.SelectedIndex = 0;
                }
                else
                {
                    cmbMunicipios.SelectedValue = gdvDatos.SelectedDataKey.Values[2].ToString();
                }
                CargarPoblaciones(bDatos);
            }
            else
            {
                cmbMunicipios.Items.Add(cnsSinMunicipios);
                cmbPoblaciones.ClearSelection();
                cmbPoblaciones.Items.Clear();
                cmbPoblaciones.Items.Add(cnsSinPoblaciones);
                cmbColonias.ClearSelection();
                cmbColonias.Items.Clear();
                cmbColonias.Items.Add(cnsSinColonias);
            }

        }

        private void CargarPoblaciones(bool bDatos)
        {
            ActualizarSesionPoblaciones();

            cmbPoblaciones.Items.Clear();
            cmbPoblaciones.DataSource = (IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblaciones"];
            cmbPoblaciones.DataBind();

            if (cmbPoblaciones.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbPoblaciones.SelectedIndex = 0;
                }
                else
                {
                    cmbPoblaciones.SelectedValue = gdvDatos.SelectedDataKey.Values[3].ToString();
                }
                CargarColonias(bDatos);
            }
            else
            {
                cmbPoblaciones.Items.Add(cnsSinPoblaciones);
                cmbColonias.ClearSelection();
                cmbColonias.Items.Clear();
                cmbColonias.Items.Add(cnsSinColonias);
            }
        }

        private void CargarColonias(bool bDatos)
        {
            ActualizarSesionColonias();

            cmbColonias.Items.Clear();
            cmbColonias.DataSource = (IQueryable<MedDAL.DAL.colonias>)Session["lstcolonias"];
            cmbColonias.DataBind();

            if (cmbColonias.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbColonias.SelectedIndex = 0;
                }
                else
                {
                    cmbColonias.SelectedValue = gdvDatos.SelectedDataKey.Values[4].ToString();
                }
            }
            else
            {
                cmbColonias.Items.Add(cnsSinColonias);
            }
        }


        private void CargarCmbTipos(bool bDatos) 
        {
            if (!bDatos)
            {
                iqrTipos = (IQueryable<MedDAL.DAL.tipos>)oblTipos.Buscar("Vendedores", 2);
                cmbTipo.Items.Clear();
                cmbTipo.DataSource = iqrTipos;
                cmbTipo.DataBind();
            }
            else 
            {
                cmbTipo.Items.Clear();
                cmbTipo.DataSource = oblTipos.RecuperarTipos();
                cmbTipo.DataBind();
            }
       }

        /// <summary>
        /// Carga el texto que debe de aparecer en los labels de campos editables
        /// </summary>
        protected void CargarCamposEditables()
        {
            MedNeg.CamposEditables.BlCamposEditables oblCamposEditables = new MedNeg.CamposEditables.BlCamposEditables();
            List<MedDAL.DAL.campos_editables> lstCamposEditables = oblCamposEditables.Buscar("Vendedores");
            lblAlfanumerico1.Text = lstCamposEditables[0].Valor != "" ? lstCamposEditables[0].Valor : lstCamposEditables[0].Campo;
            lblAlfanumerico2.Text = lstCamposEditables[1].Valor != "" ? lstCamposEditables[1].Valor : lstCamposEditables[1].Campo;
            lblAlfanumerico3.Text = lstCamposEditables[2].Valor != "" ? lstCamposEditables[2].Valor : lstCamposEditables[2].Campo;
            lblAlfanumerico4.Text = lstCamposEditables[3].Valor != "" ? lstCamposEditables[3].Valor : lstCamposEditables[3].Campo;
            lblAlfanumerico5.Text = lstCamposEditables[4].Valor != "" ? lstCamposEditables[4].Valor : lstCamposEditables[4].Campo;

            lblEntero1.Text = lstCamposEditables[5].Valor != "" ? lstCamposEditables[5].Valor : lstCamposEditables[5].Campo;
            lblEntero2.Text = lstCamposEditables[6].Valor != "" ? lstCamposEditables[6].Valor : lstCamposEditables[6].Campo;
            lblEntero3.Text = lstCamposEditables[7].Valor != "" ? lstCamposEditables[7].Valor : lstCamposEditables[7].Campo;

            lblDecimal1.Text = lstCamposEditables[8].Valor != "" ? lstCamposEditables[8].Valor : lstCamposEditables[8].Campo;
            lblDecimal2.Text = lstCamposEditables[9].Valor != "" ? lstCamposEditables[9].Valor : lstCamposEditables[9].Campo;
        }

        #endregion

        #region Modificar controles GUI

        protected void NotificarAccion(bool status, string resultadoAccion) {
            if (status)
                lblResults.ForeColor = Color.Green;
            else
                lblResults.ForeColor = Color.Red;
            lblResults.Text = resultadoAccion;
        }

        protected void MostrarAreaTrabajo(bool vCatalogo, bool vFormulario)
        {
            upnForm.Visible = vFormulario;
            pnlList.Visible = vCatalogo;
            pnlFiltroReportes.Visible = false;
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

        public void ModificarControl(Control c, bool habilitarCampos, bool limpiarCampos)
        {

            if (c is TextBox)
            {
                ((TextBox)c).Enabled = habilitarCampos;
                if (limpiarCampos)
                    ((TextBox)c).Text = string.Empty;
            }
            else if (c is DropDownList)
            {
                ((DropDownList)c).Enabled = habilitarCampos;
                if (limpiarCampos)
                    ((DropDownList)c).ClearSelection();
            }
            else if (c is CheckBox)
            {
                ((CheckBox)c).Enabled = habilitarCampos;
                if (limpiarCampos)
                    ((CheckBox)c).Checked = false;
            }

            foreach (Control ctrl in c.Controls)
            {
                ModificarControl(ctrl, habilitarCampos, limpiarCampos);
            }
        }
        #endregion

        #region Busqueda
        protected void Buscar(string sCadena)
        {
            int iTipo = 1;
            if (rdbTipo.Checked)
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

            var oQuery = oblVendedores.Buscar(sCadena, iTipo);
            Session["resultadoquery"] = oQuery; 

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Vendedores.VendedoresView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Clave ASC";
            //List<MedDAL.DAL.vendedores> lstVendedores = new List<MedDAL.DAL.vendedores>();
            //lstVendedores.AddRange((IQueryable<MedDAL.DAL.vendedores>)oQuery);
            //Session["lstVendedores"] = lstVendedores;

            try
            {
                gdvDatos.DataSource = dv;
                gdvDatos.DataBind();
                MostrarAreaTrabajo(true, false);
                if (gdvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    gdvDatos.EmptyDataText = "No existen vendedores registrados aun";
                }
                else
                {
                    gdvDatos.EmptyDataText = "No existen vendedores que coincidan con la búsqueda";
                }
                gdvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        #endregion

        #region Insertar Nuevo Registro
        protected void NuevoRegistro() {
            oVendedor = new MedDAL.DAL.vendedores();
            PoblarDatosGenerales();
            PoblarDatosContacto();
            PoblarDatosProfesionales();
            PoblarDatosOpcionales();
            if (ValidarVendedor())
            {
                if (oblVendedores.NuevoRegistro(oVendedor))
                {
                    NotificarAccion(true, "Se ha agregado correctamente el vendedor");
                    ModificarControl(this.tabContainer, true, true);
                    CargarEstados(false);
                    CargarCmbTipos(false);
                    gdvDatos.SelectedIndex = -1;
                    RegistrarEvento("Vendedores", "Agregar vendedor", "Se ha agregado al Vendedor " + oVendedor.idVendedor + ": " + oVendedor.Nombre + " " + oVendedor.Apellidos +
                        ", Clave: " + oVendedor.Clave + ", Correo electronico:" + oVendedor.CorreoElectronico + ", RFC: " + oVendedor.Rfc + ", Cedula profesional: " + oVendedor.CedulaProfesional);
                }
                else
                    NotificarAccion(false, "No se ha podido agregar el vendedor");
            }
            else
                NotificarAccion(false, "Ya existe un vendedor con esa clave");
        }

        protected void PoblarDatosGenerales() { 
            /*Datos generales*/
            oVendedor.Clave = txbClave.Text;
            oVendedor.Nombre = txbNombres.Text;
            oVendedor.Apellidos = cmbTipoPersona.SelectedValue == "FISICA" ? txbApellidos.Text : "";
            oVendedor.IdTipoVendedor = int.Parse(cmbTipo.SelectedValue);
            oVendedor.FechaAlta = Convert.ToDateTime(txbFechaAlta.Text);
            oVendedor.TipoPersona = cmbTipoPersona.SelectedValue;
            oVendedor.Activo = chkActivo.Checked;
        }

        protected void PoblarDatosContacto() {
            /*Datos de contacto*/
            //Direccion
            oVendedor.Calle = txbCalle.Text;
            oVendedor.NumeroExt = txbNumeroExterior.Text;
            oVendedor.NumeroInt = txbNumeroInterior.Text;
            oVendedor.IdEstado = int.Parse(cmbEstados.SelectedValue);
            oVendedor.IdMunicipio = int.Parse(cmbMunicipios.SelectedValue);
            oVendedor.IdPoblacion = int.Parse(cmbPoblaciones.SelectedValue);
            oVendedor.IdColonia = int.Parse(cmbColonias.SelectedValue);
            oVendedor.CodigoPostal = txbCP.Text;

            //Contacto
            oVendedor.Telefono = txbTelefono.Text;
            oVendedor.Celular = txbCelular.Text;
            oVendedor.Fax = txbFax.Text;
            oVendedor.CorreoElectronico = txbCorreoE.Text;
        }

        protected void PoblarDatosProfesionales() {
            /*Datos profesionales*/
            oVendedor.Rfc = txbRFC.Text;
            oVendedor.Curp = txbCurp.Text;
            oVendedor.CedulaProfesional = txbCedulaProfesional.Text;
            oVendedor.TituloExpedido = txbTitulo.Text;
            oEspecialidad = (MedDAL.DAL.vendedores_especialidad)oblEspecialidad.Buscar(txbEspecialidad.Text);
            if (oEspecialidad != null)
                oVendedor.IdEspecialidad = oEspecialidad.idEspecialidad;
            else { 
                oEspecialidad = new MedDAL.DAL.vendedores_especialidad();
                oEspecialidad.Especialidad = txbEspecialidad.Text;
                oblEspecialidad.NuevoRegistro(oEspecialidad);
                oVendedor.IdEspecialidad = oEspecialidad.idEspecialidad;
            }
            oVinculacion = (MedDAL.DAL.vendedores_vinculacion)oblVinculacion.Buscar(txbVinculacion.Text);
            if (oVinculacion != null)
                oVendedor.IdVinculacion = oVinculacion.idVinculacion;
            else {
                oVinculacion = new MedDAL.DAL.vendedores_vinculacion();
                oVinculacion.Vinculacion = txbVinculacion.Text;
                oblVinculacion.NuevoRegistro(oVinculacion);
                oVendedor.IdVinculacion = oVinculacion.idVinculacion;
            }
        }

        protected void PoblarDatosOpcionales(){
            /*Datos opcionales*/
            oVendedor.Campo1 = txbAlfanumerico1.Text;
            oVendedor.Campo2 = txbAlfanumerico2.Text;
            oVendedor.Campo3 = txbAlfanumerico3.Text;
            oVendedor.Campo4 = txbAlfanumerico4.Text;
            oVendedor.Campo5 = txbAlfanumerico5.Text;

            if (txbEntero1.Text.Equals(""))
                oVendedor.Campo6 = 0;
            else
                oVendedor.Campo6 = Convert.ToInt32(txbEntero1.Text);

            if (txbEntero2.Text.Equals(""))
                oVendedor.Campo7 = 0;
            else
                oVendedor.Campo7 = Convert.ToInt32(txbEntero2.Text);

            if (txbEntero3.Text.Equals(""))
                oVendedor.Campo8 = 0;
            else
                oVendedor.Campo8 = Convert.ToInt32(txbEntero3.Text);

            if (txbDecimal1.Text.Equals(""))
                oVendedor.Campo9 = 0;
            else
                oVendedor.Campo9 = Convert.ToDecimal(txbDecimal1.Text);


            if (txbDecimal2.Text.Equals(""))
                oVendedor.Campo10 = 0;
            else
                oVendedor.Campo10 = Convert.ToDecimal(txbDecimal2.Text);
        }

        protected bool ValidarVendedor()
        {

            if (oblVendedores.ValidarVendedorRepetido(txbClave.Text) >= 1)
                return false;
            else
                return true;
        }

        #endregion

        #region Eliminar Registro 
        protected void Eliminar(int idVendedor) {
            oVendedor = new MedDAL.DAL.vendedores();
            oVendedor = (MedDAL.DAL.vendedores)oblVendedores.Buscar(idVendedor);
            if (oblVendedores.EliminarRegistro(oVendedor.idVendedor))
            {
                NotificarAccion(true, "Se ha eliminado correctamente el vendedor");
                RegistrarEvento("Vendedores", "Eliminar vendedor", "Se ha elminado el Vendedor " + oVendedor.idVendedor + ": " + oVendedor.Nombre + " " + oVendedor.Apellidos +
                    ", Clave: " + oVendedor.Clave + ", Correo electronico:" + oVendedor.CorreoElectronico + ", RFC: " + oVendedor.Rfc + ", Cedula profesional: " + oVendedor.CedulaProfesional);
            }
            else
                NotificarAccion(false, "No se ha podido eliminar al vendedor");
        }
        #endregion

        #region Editar registro

        protected void EditarRegistro() {
            int idVendedor = int.Parse(gdvDatos.SelectedDataKey[0].ToString());
            oVendedor = new MedDAL.DAL.vendedores();
            oVendedor = (MedDAL.DAL.vendedores)oblVendedores.Buscar(idVendedor);
            PoblarDatosGenerales();
            PoblarDatosContacto();
            PoblarDatosProfesionales();
            PoblarDatosOpcionales();
            if (oblVendedores.EditarRegistro(oVendedor))
            {
                NotificarAccion(true, "Se ha editado correctamente el vendedor");
                RegistrarEvento("Vendedores", "Eliminar vendedor", "Se ha elminado el Vendedor " + oVendedor.idVendedor + ": " + oVendedor.Nombre + " " + oVendedor.Apellidos +
                    ", Clave: " + oVendedor.Clave + ", Correo electronico:" + oVendedor.CorreoElectronico + ", RFC: " + oVendedor.Rfc + ", Cedula profesional: " + oVendedor.CedulaProfesional);
                ModificarControl(this.tabContainer, false, false);
            }
            else
                NotificarAccion(false, "No se ha podido editar el vendedor");
        }

        protected void Editar() {
            int idVendedor = (int)gdvDatos.SelectedValue;
            ModificarControl(this.tabContainer, true, true);
            oVendedor = new MedDAL.DAL.vendedores();
            oVendedor = (MedDAL.DAL.vendedores)oblVendedores.Buscar(idVendedor);
            LlenarDatosGenerales();
            LlenarDatosContacto();
            LlenarDatosProfesionales();
            LlenarDatosOpcionales();
        }

        protected void LlenarDatosGenerales() {
            txbClave.Text = oVendedor.Clave;
            txbClave.Enabled = false;
            txbNombres.Text = oVendedor.Nombre;
            txbApellidos.Text = oVendedor.Apellidos;
            CargarCmbTipos(false);
            cmbTipo.SelectedValue = oVendedor.IdTipoVendedor.ToString();
            txbFechaAlta.Text = Convert.ToDateTime(oVendedor.FechaAlta).ToShortDateString();
            chkActivo.Checked = oVendedor.Activo;
            cmbTipoPersona.SelectedValue = oVendedor.TipoPersona;
        }

        protected void LlenarDatosContacto() {
            /*Datos de contacto*/
            //Direccion
            txbCalle.Text = oVendedor.Calle;
            txbNumeroExterior.Text = oVendedor.NumeroExt;
            txbNumeroInterior.Text = oVendedor.NumeroInt;
            txbPais.Text = "México"; //Temporal
            CargarEstados(true);
            /*cmbEstados.ClearSelection();
            cmbEstados.SelectedValue = oVendedor.IdEstado.ToString();
            CargarCmbMcpio(oVendedor.IdEstado);
            cmbMunicipios.ClearSelection();
            cmbMunicipios.SelectedValue = oVendedor.IdMunicipio.ToString();
            CargarCmbPoblacion(oVendedor.IdMunicipio);
            cmbPoblaciones.ClearSelection();
            cmbPoblaciones.SelectedValue = oVendedor.IdPoblacion.ToString();
            CargarCmbColonia(oVendedor.IdPoblacion);
            cmbColonias.ClearSelection();
            cmbColonias.SelectedValue = oVendedor.IdColonia.ToString();*/
            txbCP.Text = oVendedor.CodigoPostal;

            //Contacto
            txbTelefono.Text = oVendedor.Telefono;
            txbCelular.Text = oVendedor.Celular;
            txbFax.Text = oVendedor.Fax;
            txbCorreoE.Text = oVendedor.CorreoElectronico;
        }

        protected void LlenarDatosProfesionales() {
            txbCurp.Text = oVendedor.Curp;
            txbRFC.Text = oVendedor.Rfc;
            txbTitulo.Text = oVendedor.TituloExpedido;
            txbCedulaProfesional.Text = oVendedor.CedulaProfesional;
            oblEspecialidad = new MedNeg.VendedorEspecialidad.BlVendedorEspecialidad();
            txbEspecialidad.Text = ((MedDAL.DAL.vendedores_especialidad)oblEspecialidad.Buscar(oVendedor.IdEspecialidad)).Especialidad;
            txbVinculacion.Text = ((MedDAL.DAL.vendedores_vinculacion)oblVinculacion.Buscar(oVendedor.IdVinculacion)).Vinculacion;
        }

        protected void LlenarDatosOpcionales() {
            /*Datos opcionales*/
            txbAlfanumerico1.Text = oVendedor.Campo1;
            txbAlfanumerico2.Text = oVendedor.Campo2;
            txbAlfanumerico3.Text = oVendedor.Campo3;
            txbAlfanumerico4.Text = oVendedor.Campo4;
            txbAlfanumerico5.Text = oVendedor.Campo5;

            txbEntero1.Text = oVendedor.Campo6.ToString();
            txbEntero2.Text = oVendedor.Campo7.ToString();
            txbEntero3.Text = oVendedor.Campo8.ToString();

            txbDecimal1.Text = oVendedor.Campo9.ToString();
            txbDecimal2.Text = oVendedor.Campo10.ToString();
        }

        #endregion

        #region Registro en bitacora
        protected void RegistrarEvento(string sModulo, string sAccion, string sDescripcion) {
            oBitacora = new MedDAL.DAL.bitacora();
            oBitacora.FechaEntradaSrv = DateTime.Now;
            oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
            oBitacora.Modulo = sModulo;
            oBitacora.Usuario = Session["usuario"].ToString();
            oBitacora.Nombre = Session["nombre"].ToString();
            oBitacora.Accion = sAccion;
            oBitacora.Descripcion = sDescripcion;
            if (!oblBitacora.NuevoRegistro(oBitacora))
            {
                NotificarAccion(true, "No se ha podido registrar el movimiento en la bitacora");
            }
        }
        #endregion

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
        public void CargarListaReportes()
        {
            ListBox lsbReportes = (ListBox)frReportes.FindControl("lsbSeleccionf");
            frReportes.LimpiarPaneles();
            lsbReportes.SelectedIndex = -1;
            lsbReportes.Items.Clear();
            if (Server.MapPath("~\\rptReportes\\Vendedores\\rptVendedores.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de vendedores");
            }
        }

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            MostrarAreaTrabajo(false, false);            
            pnlFiltroReportes.Visible = true;

            CargarListaReportes();
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
        }

        protected void CargarReporte()
        {
            MostrarAreaTrabajo(false, false);            

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from vendedores", "medicuriConnectionString", odsDataSet, "vendedores");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from estados", "medicuriConnectionString", odsDataSet, "estados");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from municipios", "medicuriConnectionString", odsDataSet, "municipios");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from poblaciones", "medicuriConnectionString", odsDataSet, "poblaciones");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from colonias", "medicuriConnectionString", odsDataSet, "colonias");

            Session["campoaordenar"] = "";
            Session["dataset"] = odsDataSet;
            Session["titulo"] = "Impresión de Vendedores";
            //Session["configuracionsistema"] = objConfiguracion;
            Session["sortfield"] = 0;
            Session["reportdocument"] = "~\\rptReportes\\rptVendedores.rpt";

            if (gdvDatos.SelectedIndex != -1)
            {
                Session["recordselection"] = "{vendedores.idVendedor}=" + gdvDatos.SelectedDataKey.Values[0].ToString();
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
            var result = (IQueryable<MedDAL.Vendedores.VendedoresView>)Session["resultadoquery"];
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
            var result = (IQueryable<MedDAL.Vendedores.VendedoresView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            gdvDatos.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Clave" : ViewState["sortexpression"].ToString(), dv, ref gdvDatos, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            gdvDatos.DataBind();
        }

        #endregion

        protected void cmvClave_ServerValidate(object source, ServerValidateEventArgs args)
        {
            MedDAL.Vendedores.DALVendedores oDALVendedores = new MedDAL.Vendedores.DALVendedores();
            if (oDALVendedores.BuscarVendedorClave(txbClave.Text) != null && (bool)Session["modoGuardar"])
            {
                args.IsValid = false;
            }
            else args.IsValid = true;
        }
    }
}