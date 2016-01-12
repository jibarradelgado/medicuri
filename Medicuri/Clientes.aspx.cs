using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Drawing;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Medicuri
{
    public partial class Clientes : System.Web.UI.Page
    {
        #region Variables/Objetos
        const string cnsSinEstados = "Sin estados...", cnsSinMunicipios = "Sin municipios...", cnsSinPoblaciones = "Sin poblaciones...", cnsSinColonias = "Sin colonias..."; 
        Label lblNombreModulo;
        ImageButton imbNuevo, imbEditar, imbEliminar, imbImprimir, imbMostrar, imbAceptar, imbCancelar, imbReportes;
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
        MedNeg.BlClientes.BlClientes oblCliente;
        MedNeg.Bitacora.BlBitacora oblBitacora;
        MedNeg.ClientesContactos.BlClientesContactos oblClientesContactos;

        //Objetos de las entities
        MedDAL.DAL.bitacora oBitacora;
        MedDAL.DAL.clientes oCliente;

        //Lista de contactos del cliente
        //List<MedDAL.DAL.clientes_contacto> lstContactosNuevos, lstContactosEliminar, lstContactosGriedView, lstContactosBD;
        #endregion

        #region Configuración de inicio
        protected void Page_Load(object sender, EventArgs e)
        {
            //Asignar titulo de modulo
            lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
            lblNombreModulo.Text = "Clientes";

            //Cargar permisos
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';

            //cPermiso = (char)htbPermisos["vendedores"];

            try
            {    
                cPermiso = (char)htbPermisos["clientes"];

                //Obtener los controles de master.
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
            imbAceptar.ValidationGroup = "vgCliente";
            imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
            imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
            rdbTipo = (RadioButton)Master.FindControl("rdbFiltro1");
            rdbTipo.Text = "Tipo";            
            rdbClave = (RadioButton)Master.FindControl("rdbFiltro2");
            rdbClave.Text = "Clave1";
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
            oblCliente = new MedNeg.BlClientes.BlClientes();
            //lstContactosGriedView = new List<MedDAL.DAL.clientes_contacto>();

            gdvContactosCliente.Visible = true;
            gdvContactosCliente.ShowHeader = true;            
            gdvContactosCliente.DataSource = ((List<MedDAL.DAL.clientes_contacto>)Session["lstContactosDB"]);
            gdvContactosCliente.DataBind();
            gdvContactosCliente.DataKeyNames = new String[] { "idContacto" };

            CargarCamposEditables();
            
            if (!IsPostBack)
            {
                MostrarAreaTrabajo(false, false);
                Session["lstContactosDB"] = new List<MedDAL.DAL.clientes_contacto>();
                Session["gridviewdatasource"] = null;
                Session["ajustecontrolesreporte"] = false;
                Session["resultadoquery"] = "";
                ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;

                Session["reporteactivo"] = 0;
                Session["reportdocument"] = "";
                Session["titulo"] = "";

                //GT 0175
                ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
            }

            if ((bool)Session["ajustecontrolesreporte"] &&  !(bool)Session["ajustecontrolesreportecandado"])
            {
                CargarListaReportes();
                Session["ajustecontrolesreporte"] = false;
            }

            }
            catch (NullReferenceException)
            {
                //this.Page.LoadControl("~/Login.aspx");
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
            gdvDatos.SelectedIndex = -1;
            Session["modoGuardarClientes"] = true;
            MostrarAreaTrabajo(false, true);
            ModificarControl(this.tabContainer, true, true);
            txbPais.Text = "México"; 
            txbPais.Enabled = false;
            txbFechaAlta.Text = DateTime.Now.ToShortDateString();
            txbFechaAlta.Enabled = false;
            chkActivo.Checked = true;
            CargarEstados(false);
            CargarCmbTipos(false);
            Session["lstContactosDB"] = new List<MedDAL.DAL.clientes_contacto>();
            gdvContactosCliente.DataSource = ((List<MedDAL.DAL.clientes_contacto>)Session["lstContactosDB"]);
            //20130424 Jid Cambio solicitado por el cliente.
            cmbTipoPersona.SelectedIndex = 0;
            gdvContactosCliente.DataBind();
            //0175 GT
            ConfigurarMenuBotones(true, false, false, false, true, true, false, false);
        }

        public void imbEditar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            if (gdvDatos.SelectedIndex >= 0)
            {
                Session["modoGuardarClientes"] = false;
                MostrarAreaTrabajo(false, true);
                Editar();
                //0175 GT
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);
            }
            else 
            {
                MostrarLista();
                //0175 GT
                ConfigurarMenuBotones(true, true, true, false, false, false, true, true);
            }
        }

        public void imbEliminar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            if (pnlList.Visible && gdvDatos.SelectedIndex != -1)
            {
                Eliminar((int)gdvDatos.SelectedValue);                
            }
            Buscar(txbBuscar.Text);
        }

        public void imbMostrar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            gdvDatos.SelectedIndex = -1;
            //Buscar("");
            MostrarLista();
            //0175 GT
            ConfigurarMenuBotones(true, true,true, true, false, true, true, true);
        }

        public void imbAceptar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            if (cmvClave.IsValid)
            {
                if ((bool)Session["modoGuardarClientes"])
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
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
            ModificarControl(this.tabContainer, false, true);
            MostrarAreaTrabajo(false, false);
            gdvDatos.SelectedIndex = -1;
        }

        public void btnBuscar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            Buscar(txbBuscar.Text);
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

        protected void imbAgregarContacto_Click(object sender, ImageClickEventArgs e)
        {
            MedDAL.DAL.clientes_contacto contacto = new MedDAL.DAL.clientes_contacto();
            contacto.Activo = true;
            contacto.Nombre = txbNombreContacto.Text;
            contacto.Apellidos = txbApellidosContacto.Text;
            contacto.Telefono = txbTelefonoContacto.Text;
            contacto.Celular = txbCelularConatcto.Text;
            contacto.CorreoElectronico = txbCorreoEContacto.Text;
            LLenarListas(contacto);
            gdvContactosCliente.DataBind();
            ModificarControl(this.tabContactosPersonales, true, true);
        }

        protected void grvContactos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((List<MedDAL.DAL.clientes_contacto>)Session["lstContactosDB"]).RemoveAt(gdvContactosCliente.SelectedIndex);
            gdvContactosCliente.DataBind();
        }

        protected void cmbTipoPersona_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTipoPersona.SelectedValue == "FISICA")
            {
                lblNombres.Text = "Nombre(s):";
                txbApellidos.Enabled = true;
                rfvApellidos.Enabled = true;
                txbEdad.Enabled = true;
                cmbSexo.Enabled = true;
            }
            else
            {
                lblNombres.Text = "Razon Social:";
                txbEdad.Enabled = false;
                rfvApellidos.Enabled = false;
                cmbSexo.Enabled = false;
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
                iqrTipos = (IQueryable<MedDAL.DAL.tipos>)oblTipos.Buscar("Clientes", 2);
                ddlTipo.Items.Clear();
                ddlTipo.DataSource = iqrTipos;
                ddlTipo.DataBind();
                foreach (ListItem item in ddlTipo.Items)
                {
                    if (item.Text == "Paciente")
                    {
                        ddlTipo.SelectedValue = item.Value;
                        break;
                    }
                }                
            }
            else
            {
                ddlTipo.Items.Clear();
                ddlTipo.DataSource = oblTipos.RecuperarTipos();
                ddlTipo.DataBind();
            }
        }

        /// <summary>
        /// Carga el texto que debe de aparecer en los labels de campos editables
        /// </summary>
        protected void CargarCamposEditables()
        {
            MedNeg.CamposEditables.BlCamposEditables oblCamposEditables = new MedNeg.CamposEditables.BlCamposEditables();
            List<MedDAL.DAL.campos_editables> lstCamposEditables = oblCamposEditables.Buscar("Clientes");
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

        protected void NotificarAccion(bool status, string resultadoAccion)
        {
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

            var oQuery = oblCliente.Buscar(sCadena, iTipo);
            Session["resultadoquery"] = oQuery;
            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Clientes.ClientesView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Clave1 ASC";
            gdvDatos.DataSource = dv;

            try
            {
                //gdvDatos.DataSource = oQuery;
                
                //gdvDatos.DataKeyNames = new string[] { "idCliente" };
                gdvDatos.DataBind();
                MostrarAreaTrabajo(true, false);
                if (gdvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    gdvDatos.EmptyDataText = "No existen clientes registrados aun";
                }
                else
                {
                    gdvDatos.EmptyDataText = "No existen clientes que coincidan con la búsqueda";
                }
                gdvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void MostrarLista()
        {
            MedNeg.BlClientes.BlClientes oblCliente = new MedNeg.BlClientes.BlClientes();
            var oQuery = oblCliente.MostrarLista();
            Session["resultadoquery"] = oQuery;
            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Clientes.ClientesView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Clave1 ASC";
            gdvDatos.DataSource = dv;

            try
            {
                //DataSet ds = oQuery;
                //gdvDatos.DataSource = oQuery;                
                //gdvDatos.DataKeyNames = new string[] { "idCliente" };
                gdvDatos.DataBind();
                MostrarAreaTrabajo(true, false);
                if (gdvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    gdvDatos.EmptyDataText = "No existen clientes registrados aun";
                }
                else
                {
                    gdvDatos.EmptyDataText = "No existen clientes que coincidan con la búsqueda";
                }
                gdvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }


        }
        #endregion

        #region Nuevo Registro
        protected void NuevoRegistro()
        {
            oCliente = new MedDAL.DAL.clientes();
            PoblarDatosGenerales();
            PoblarDatosContacto();
            PoblarDatosProfesionales();
            PoblarDatosOpcionales();
            //if (ValidarCliente())
            //{
                if (oblCliente.NuevoRegistro(oCliente))
                {
                    NotificarAccion(true, "Se ha agregado correctamente el cliente");
                    ModificarControl(this.tabContainer, true, true);
                    //Session["lstContactosDB"] = new List<MedDAL.DAL.clientes_contacto>();
                    //gdvContactosCliente.DataBind();
                    CargarEstados(false);
                    CargarCmbTipos(false);
                    gdvDatos.SelectedIndex = -1;
                    RegistrarEvento("Cliente", "Agregar cliente", "Se ha agregado el Cliente " + oCliente.idCliente + ": " + oCliente.Nombre + " " + oCliente.Apellidos +
                        ", Clave: " + oCliente.Clave1 + ", Correo electronico:" + oCliente.CorreoElectronico + ", RFC: " + oCliente.Rfc + "");

                    oblClientesContactos = new MedNeg.ClientesContactos.BlClientesContactos();
                    if (!oblClientesContactos.NuevoRegistro((List<MedDAL.DAL.clientes_contacto>)Session["lstContactosDB"], oCliente.idCliente))
                        NotificarAccion(false, "Se ha agregado correctamente el cliente, pero no se pudieron agregar 1 o mas contactos");
                }
                else
                    NotificarAccion(false, "No se ha podido agregar el cliente");
            //}
            //else
            //    NotificarAccion(false, "Ya existe un cliente con esa clave");
        }

        protected void LLenarListas(MedDAL.DAL.clientes_contacto contacto)
        {
            if (!((List<MedDAL.DAL.clientes_contacto>)Session["lstContactosDB"]).Exists(delegate(MedDAL.DAL.clientes_contacto c) { return (c.Nombre == contacto.Nombre) & (c.Apellidos == contacto.Apellidos); }))
            {
                ((List<MedDAL.DAL.clientes_contacto>)Session["lstContactosDB"]).Add(contacto);
            }
            else
                NotificarAccion(false, "Ya existe un contacto del cliente con esos datos");
        }

        protected void PoblarDatosGenerales()
        {
            /*Datos generales*/
            oCliente.Clave1 = txbClave1.Text;
            oCliente.Clave2 = txbClave2.Text;
            oCliente.Clave3 = txbClave3.Text;
            oCliente.Nombre = txbNombres.Text;
            oCliente.Apellidos = cmbTipoPersona.SelectedValue == "FISICA" ? txbApellidos.Text : "";
            oCliente.idTipoCliente = int.Parse(ddlTipo.SelectedValue);
            oCliente.FechaAlta = Convert.ToDateTime(txbFechaAlta.Text);
            oCliente.Activo = chkActivo.Checked;
            oCliente.Sexo = cmbSexo.Enabled? cmbSexo.SelectedValue : cmbSexo.Items[0].Value;
            oCliente.TipoPersona = cmbTipoPersona.SelectedValue;
            if (txbEdad.Text != string.Empty && txbEdad.Enabled)
                oCliente.Edad = int.Parse(txbEdad.Text);
            else
                oCliente.Edad = 0;
        }

        protected void PoblarDatosContacto()
        {
            /*Datos de contacto*/
            //Direccion
            oCliente.Calle = txbCalle.Text;
            oCliente.NumeroExt = txbNumeroExterior.Text;
            oCliente.NumeroInt = txbNumeroInterior.Text;
            oCliente.idEstado = int.Parse(cmbEstados.SelectedValue);
            oCliente.idMunicipio = int.Parse(cmbMunicipios.SelectedValue);
            oCliente.idPoblacion = int.Parse(cmbPoblaciones.SelectedValue);
            oCliente.idColonia = int.Parse(cmbColonias.SelectedValue);
            oCliente.CodigoPostal = txbCP.Text;

            //Contacto
            oCliente.Telefono = txbTelefono.Text;
            oCliente.Celular = txbCelular.Text;
            //oCliente.Fax = txbFax.Text;
            oCliente.CorreoElectronico = txbCorreoE.Text;
        }

        protected void PoblarDatosProfesionales()
        {
            /*Datos profesionales*/
            oCliente.Rfc = txbRFC.Text;
            oCliente.Curp = txbCurp.Text;
        }

        protected void PoblarDatosOpcionales()
        {
            /*Datos opcionales*/
            oCliente.Campo1 = txbAlfanumerico1.Text;
            oCliente.Campo2 = txbAlfanumerico2.Text;
            oCliente.Campo3 = txbAlfanumerico3.Text;
            oCliente.Campo4 = txbAlfanumerico4.Text;
            oCliente.Campo5 = txbAlfanumerico5.Text;

            if (txbEntero1.Text.Equals(""))
                oCliente.Campo6 = 0;
            else
                oCliente.Campo6 = Convert.ToInt32(txbEntero1.Text);

            if (txbEntero2.Text.Equals(""))
                oCliente.Campo7 = 0;
            else
                oCliente.Campo7 = Convert.ToInt32(txbEntero2.Text);

            if (txbEntero3.Text.Equals(""))
                oCliente.Campo8 = 0;
            else
                oCliente.Campo8 = Convert.ToInt32(txbEntero3.Text);

            if (txbDecimal1.Text.Equals(""))
                oCliente.Campo9 = 0;
            else
                oCliente.Campo9 = Convert.ToDecimal(txbDecimal1.Text);


            if (txbDecimal2.Text.Equals(""))
                oCliente.Campo10 = 0;
            else
                oCliente.Campo10 = Convert.ToDecimal(txbDecimal2.Text);
        }

        protected bool ValidarCliente()
        {

            if (oblCliente.ValidarClienteRepetido(txbClave1.Text) >= 1)
                return false;
            else
                return true;
        }

        #endregion

        #region Eliminar Registro
        protected void Eliminar(int idCliente)
        {
            oCliente = new MedDAL.DAL.clientes();
            oCliente = (MedDAL.DAL.clientes)oblCliente.BuscarCliente(idCliente);
            oblClientesContactos = new MedNeg.ClientesContactos.BlClientesContactos();
            if (oblClientesContactos.EliminarSimultaneos(idCliente))
            {
                if (oblCliente.EliminarRegistro(oCliente.idCliente))
                {
                    gdvDatos.SelectedIndex = -1;
                    NotificarAccion(true, "Se ha eliminado correctamente el vendedor");
                    RegistrarEvento("Clientes", "Eliminar cliente", "Se ha elminado el cliente " + oCliente.idCliente + ": " + oCliente.Nombre + " " + oCliente.Apellidos +
                        ", Clave: " + oCliente.Clave1 + ", Correo electronico:" + oCliente.CorreoElectronico + ", RFC: " + oCliente.Rfc);
                }
                else
                    NotificarAccion(false, "No se ha podido eliminar al cliente");
            }
            else
                NotificarAccion(false, "No se ha podido eliminar al cliente porque aun tiene contactos asociados");
        }

        protected void EliminarContactos(int idCliente)
        {

        }
        #endregion

        #region Editar registro

        protected void EditarRegistro()
        {
            int idCliente = (int)gdvDatos.SelectedValue;
            oCliente = new MedDAL.DAL.clientes();
            oCliente = (MedDAL.DAL.clientes)oblCliente.BuscarCliente(idCliente);
            PoblarDatosGenerales();
            PoblarDatosContacto();
            PoblarDatosProfesionales();
            PoblarDatosOpcionales();
            if (oblCliente.EditarRegistro(oCliente))
            {
                oblClientesContactos = new MedNeg.ClientesContactos.BlClientesContactos();
                if (oblClientesContactos.EliminarSimultaneos(idCliente)&oblClientesContactos.NuevoRegistro((List<MedDAL.DAL.clientes_contacto>)Session["lstContactosDB"],idCliente))
                    NotificarAccion(true, "Se ha editado correctamente el cliente");    
                else
                    NotificarAccion(true, "Se ha editado correctamente el cliente, pero no se actualizaron los contactos");
                RegistrarEvento("Clientes", "Editar cliente", "Se ha editado el cliente " + oCliente.idCliente + ": " + oCliente.Nombre + " " + oCliente.Apellidos +
                        ", Clave: " + oCliente.Clave1 + ", Correo electronico:" + oCliente.CorreoElectronico + ", RFC: " + oCliente.Rfc);
                ModificarControl(this.tabContainer, false, false);
            }
            else
                NotificarAccion(false, "No se ha podido editar el cliente");
        }

        protected void Editar()
        {
            int idCliente = (int)gdvDatos.SelectedValue;
            ModificarControl(this.tabContainer, true, true);
            oCliente = new MedDAL.DAL.clientes();
            oCliente = (MedDAL.DAL.clientes)oblCliente.BuscarCliente(idCliente);
            oblClientesContactos = new MedNeg.ClientesContactos.BlClientesContactos();
            Session["lstContactosDB"] = oblClientesContactos.BuscarContactos(idCliente); ;
            gdvContactosCliente.DataSource = ((List<MedDAL.DAL.clientes_contacto>)Session["lstContactosDB"]);
            gdvContactosCliente.DataBind();
            LlenarDatosGenerales();
            LlenarDatosContacto();
            LlenarDatosProfesionales();
            LlenarDatosOpcionales();
        }

        protected void LlenarDatosGenerales()
        {
            txbClave1.Text = oCliente.Clave1;
            txbClave2.Text = oCliente.Clave2;
            txbClave3.Text = oCliente.Clave3;
            txbClave1.Enabled = false;
            txbNombres.Text = oCliente.Nombre;
            txbApellidos.Text = oCliente.Apellidos;
            txbEdad.Text = oCliente.Edad.ToString();
            cmbSexo.SelectedValue = oCliente.Sexo;
            CargarCmbTipos(false);
            if (!(oCliente.idTipoCliente == 0))
            {
                ddlTipo.SelectedValue = oCliente.idTipoCliente.ToString();
            }
            txbFechaAlta.Text = Convert.ToDateTime(oCliente.FechaAlta).ToShortDateString();
            txbFechaAlta.Enabled = false;
            chkActivo.Checked = oCliente.Activo;
            cmbTipoPersona.SelectedIndex = oCliente.TipoPersona == "FISICA" ? 0 : 1;
            rfvApellidos.Enabled = txbEdad.Enabled = cmbSexo.Enabled = oCliente.TipoPersona == "FISICA" ? true : false;            
        }

        protected void LlenarDatosContacto()
        {
            /*Datos de contacto*/
            //Direccion
            txbCalle.Text = oCliente.Calle;
            txbNumeroExterior.Text = oCliente.NumeroExt;
            txbNumeroInterior.Text = oCliente.NumeroInt;
            txbPais.Text = "México"; //Temporal
            CargarEstados(true);
            /*cmbEstados.ClearSelection();
            cmbEstados.SelectedValue = oCliente.idEstado.ToString();
            CargarMunicipios(oCliente.idEstado);
            cmbMunicipios.ClearSelection();
            cmbMunicipios.SelectedValue = oCliente.idMunicipio.ToString();
            CargarPoblaciones(oCliente.idMunicipio);
            cmbPoblaciones.ClearSelection();
            cmbPoblaciones.SelectedValue = oCliente.idPoblacion.ToString();
            CargarColonias(oCliente.idPoblacion);
            cmbColonias.ClearSelection();
            cmbColonias.SelectedValue = oCliente.idColonia.ToString();*/
            txbCP.Text = oCliente.CodigoPostal;

            //Contacto
            txbTelefono.Text = oCliente.Telefono;
            txbCelular.Text = oCliente.Celular;
            //txbFax.Text = oCliente.Fax;
            txbCorreoE.Text = oCliente.CorreoElectronico;
        }

        protected void LlenarDatosProfesionales()
        {
            txbCurp.Text = oCliente.Curp;
            txbRFC.Text = oCliente.Rfc;
        }

        protected void LlenarDatosOpcionales()
        {
            /*Datos opcionales*/
            txbAlfanumerico1.Text = oCliente.Campo1;
            txbAlfanumerico2.Text = oCliente.Campo2;
            txbAlfanumerico3.Text = oCliente.Campo3;
            txbAlfanumerico4.Text = oCliente.Campo4;
            txbAlfanumerico5.Text = oCliente.Campo5;

            txbEntero1.Text = oCliente.Campo6.ToString();
            txbEntero2.Text = oCliente.Campo7.ToString();
            txbEntero3.Text = oCliente.Campo8.ToString();

            txbDecimal1.Text = oCliente.Campo9.ToString();
            txbDecimal2.Text = oCliente.Campo10.ToString();
        }

        #endregion

        #region Registro en bitacora
        protected void RegistrarEvento(string sModulo, string sAccion, string sDescripcion)
        {
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
            if (Server.MapPath("~\\rptReportes\\Clientes\\rptClientes.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de clientes");
            }
            if (Server.MapPath("~\\rptReportes\\Clientes\\rptEstadoCuentaGeneral.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de estado de cuenta general");
            }
            if (Server.MapPath("~\\rptReportes\\Clientes\\rptPorConcepto.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte por concepto");
            }
        }

        public void imbReportes_Click(object sender, EventArgs e)
        {
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
            MostrarAreaTrabajo(false, false);
            pnlFiltroReportes.Visible = true;

            CargarListaReportes();
        }

        protected void CargarReporte()
        {
            MostrarAreaTrabajo(false, false);            

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsDataSet, "clientes");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes_contacto", "medicuriConnectionString", odsDataSet, "clientes_contacto");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from estados", "medicuriConnectionString", odsDataSet, "estados");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from municipios", "medicuriConnectionString", odsDataSet, "municipios");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from poblaciones", "medicuriConnectionString", odsDataSet, "poblaciones");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from colonias", "medicuriConnectionString", odsDataSet, "colonias");

            Session["campoaordenar"] = "";
            Session["dataset"] = odsDataSet;
            Session["titulo"] = "Impresión de Almacenes";
            //Session["configuracionsistema"] = objConfiguracion;
            Session["sortfield"] = 0;
            Session["reportdocument"] = "~\\rptReportes\\rptClientes.rpt";

            if (gdvDatos.SelectedIndex != -1)
            {
                Session["recordselection"] = "{clientes.idCliente}=" + gdvDatos.SelectedDataKey.Values[0].ToString();
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
            var result = (IQueryable<MedDAL.Clientes.ClientesView>)Session["resultadoquery"];
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
            var result = (IQueryable<MedDAL.Clientes.ClientesView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            gdvDatos.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Clave1" : ViewState["sortexpression"].ToString(), dv, ref gdvDatos, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            gdvDatos.DataBind();
        }

        #endregion

        protected void cmvClave_ServerValidate(object source, ServerValidateEventArgs args)
        {
            MedDAL.Clientes.DALClientes oDALClientes = new MedDAL.Clientes.DALClientes();
            if (oDALClientes.BuscarPorClave(txbClave1.Text) != null && (bool)Session["modoGuardarClientes"])
            {
                args.IsValid = false;
            }
            else args.IsValid = true;
        }
    }
}