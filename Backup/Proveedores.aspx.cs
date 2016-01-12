using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Medicuri
{
    public partial class Proveedores : System.Web.UI.Page
    {
        const string cnsSinTipos = "Sin tipos...", cnsSinEstados = "Sin estados...", cnsSinMunicipios = "Sin municipios...", cnsSinPoblaciones = "Sin poblaciones...", cnsSinColonias = "Sin colonias...";
        ImageButton imbNuevo, imbEditar, imbEliminar, imbImprimir, imbMostrar, imbAceptar, imbCancelar, imbReportes;
        RadioButton rdbNombre, rdbClave, rdbTipos;
        Button btnBuscar;
        TextBox txbBuscar;
        Label lblNombreModulo;
        MedNeg.Proveedores.BlProveedores oblProveedores;
        MedNeg.ProveedoresContactos.BlProveedoresContactos oblProveedoresContactos;
        MedNeg.Colonias.BlColonias oblColonias;
        MedNeg.Poblaciones.BlPoblaciones oblPoblaciones;
        MedNeg.Municipios.BlMunicipios oblMunicipios;
        MedNeg.Estados.BlEstados oblEstados;
        MedNeg.Tipos.BlTipos oblTipos;
        MedNeg.CamposEditables.BlCamposEditables oblCamposEditables;
        MedNeg.Bitacora.BlBitacora oblBitacora;        
        MedDAL.DAL.proveedores oProveedor;
        MedDAL.DAL.bitacora oBitacora;       
      
        /// <summary>
        /// Actualiza la variable de sesión "lsttiposproveedores", la cual es una lista de todos los tipos de tipo Proveedor
        /// </summary>
        protected void ActualizarSesionTipos(bool bDatos) 
        {
            List<MedDAL.DAL.tipos> lstTipos = new List<MedDAL.DAL.tipos>();
            if (!bDatos)
            {
                lstTipos = oblTipos.RecuperarTiposProveedores();
            }
            else
            {
                lstTipos = oblTipos.RecuperarTipos();
            }
            Session["lsttiposproveedores"] = lstTipos;
        }

        /// <summary>
        /// Actualiza la variable de sesion "lstestadosproveedores", la cual es una lista de los estados activos
        /// </summary>
        protected void ActualizarSesionEstados()
        {
            IQueryable<MedDAL.DAL.estados> iqrEstados = oblEstados.BuscarEnum();
            Session["lstestadosproveedores"] = iqrEstados;
        }

        /// <summary>
        /// Actualiza la variable de sesion de Municipios dependiendo de los Estados existentes.
        /// </summary>        
        protected void ActualizarSesionMunicipios()
        {
            IQueryable<MedDAL.DAL.municipios> iqrMunicipios = oblMunicipios.BuscarEstados(int.Parse(cmbEstado.SelectedValue));
            Session["lstmunicipiosproveedores"] = iqrMunicipios;
        }

        /// <summary>
        /// Actualiza la variable de sesión de Poblaciones dependiendo de los Municipios existentes.
        /// </summary>        
        protected void ActualizarSesionPoblaciones()
        {
            IQueryable<MedDAL.DAL.poblaciones> iqrPoblaciones = oblPoblaciones.BuscarMunicipios(int.Parse(cmbMunicipio.SelectedValue));
            Session["lstpoblacionesproveedores"] = iqrPoblaciones;                    
        }

        /// <summary>
        /// Actualiza la variable de sesión de Colonias dependiendo de las Poblaciones existentes.
        /// </summary>
        protected void ActualizarSesionColonias()
        {
            IQueryable<MedDAL.DAL.colonias> iqrColonias = oblColonias.BuscarPoblaciones(int.Parse(cmbPoblacion.SelectedValue));
            Session["lstcoloniasproveedores"] = iqrColonias;
        }

        /// <summary>
        /// Carga cmbEstado, cmbMunicipio, cmbPoblacion y cmbColonia
        /// </summary>
        /// <param name="bDatos"></param>
        protected void CargarEstados(bool bDatos) 
        {
            ActualizarSesionEstados();

            cmbEstado.Items.Clear();
            cmbEstado.DataSource = (IQueryable<MedDAL.DAL.estados>)Session["lstestadosproveedores"];
            cmbEstado.DataBind();

            if (cmbEstado.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbEstado.SelectedIndex = 0;
                }
                else
                {
                    cmbEstado.SelectedValue = gdvLista.SelectedDataKey.Values[1].ToString();
                }
                CargarMunicipios(bDatos);
            }
            else 
            {
                cmbEstado.Items.Add(cnsSinEstados);
                cmbMunicipio.ClearSelection();
                cmbMunicipio.Items.Clear();                
                cmbMunicipio.Items.Add(cnsSinMunicipios);
                cmbPoblacion.ClearSelection();
                cmbPoblacion.Items.Clear();                
                cmbPoblacion.Items.Add(cnsSinPoblaciones);
                cmbColonia.ClearSelection();
                cmbColonia.Items.Clear();                
                cmbColonia.Items.Add(cnsSinColonias);
            }
        }
        /// <summary>
        /// Carga cmbMunicipio, cmbPoblacion y cmbColonia
        /// </summary>
        /// <param name="bDatos"></param>
        protected void CargarMunicipios(bool bDatos)
        {
            ActualizarSesionMunicipios();

            cmbMunicipio.Items.Clear();
            cmbMunicipio.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstmunicipiosproveedores"];
            cmbMunicipio.DataBind();

            if (cmbMunicipio.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbMunicipio.SelectedIndex = 0;
                }
                else
                {
                    cmbMunicipio.SelectedValue = gdvLista.SelectedDataKey.Values[2].ToString();
                }
                CargarPoblaciones(bDatos);
            }
            else 
            {
                cmbMunicipio.Items.Add(cnsSinMunicipios);
                cmbPoblacion.ClearSelection();
                cmbPoblacion.Items.Clear();
                cmbPoblacion.Items.Add(cnsSinPoblaciones);
                cmbColonia.ClearSelection();
                cmbColonia.Items.Clear();
                cmbColonia.Items.Add(cnsSinColonias);                
            }
        }
        /// <summary>
        /// Carga cmbPoblacion y cmbColonia
        /// </summary>
        /// <param name="bDatos"></param>
        protected void CargarPoblaciones(bool bDatos)
        {
            ActualizarSesionPoblaciones();

            cmbPoblacion.Items.Clear();
            cmbPoblacion.DataSource = (IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblacionesproveedores"];
            cmbPoblacion.DataBind();

            if (cmbPoblacion.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbPoblacion.SelectedIndex = 0;
                }
                else
                {
                    cmbPoblacion.SelectedValue = gdvLista.SelectedDataKey.Values[3].ToString();
                }
                CargarColonias(bDatos);
            }
            else
            {
                cmbPoblacion.Items.Add(cnsSinPoblaciones);
                cmbColonia.ClearSelection();
                cmbColonia.Items.Clear();
                cmbColonia.Items.Add(cnsSinColonias);                
            }
        }
        /// <summary>
        /// Carga cmbColonia
        /// </summary>
        /// <param name="bDatos"></param>
        protected void CargarColonias(bool bDatos)
        {
            ActualizarSesionColonias();

            cmbColonia.Items.Clear();
            cmbColonia.DataSource = (IQueryable<MedDAL.DAL.colonias>)Session["lstcoloniasproveedores"];
            cmbColonia.DataBind();

            if (cmbColonia.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbColonia.SelectedIndex = 0;
                }
                else
                {
                    cmbColonia.SelectedValue = gdvLista.SelectedDataKey.Values[4].ToString();
                }
            }
            else
            {                
                cmbColonia.Items.Add(cnsSinColonias);
            }
        }
        protected void CargarTipos(bool bDatos)
        {
            ActualizarSesionTipos(false);

            cmbTipo.Items.Clear();
            cmbTipo.DataSource = (List<MedDAL.DAL.tipos>)Session["lsttiposproveedores"];
            cmbTipo.DataBind();

            if (cmbTipo.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbTipo.SelectedIndex = 0;
                }
                else
                {
                    ActualizarSesionTipos(false);

                    cmbTipo.Items.Clear();
                    cmbTipo.DataSource = (List<MedDAL.DAL.tipos>)Session["lsttiposproveedores"];
                    cmbTipo.DataBind();

                    cmbTipo.SelectedValue = gdvLista.SelectedDataKey.Values[5].ToString();
                }
            }
            else
            {
                cmbTipo.Items.Add(cnsSinTipos);
            }
        }
        /// <summary>
        /// Carga el texto que debe de aparecer en los labels de campos editables
        /// </summary>
        protected void CargarCamposEditables()
        {
            List<MedDAL.DAL.campos_editables> lstCamposEditables = oblCamposEditables.Buscar("Proveedores");
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

        public void LimpiarValores(Control c)
        {
            if (c is TextBox )
            {
                if(c.ID != "txtPais") ((TextBox)c).Text = "";                
            }
            else if (c is CheckBox) 
            {
                ((CheckBox)c).Checked = false;
            }

            foreach (Control ctrl in c.Controls)
            {
                LimpiarValores(ctrl);
            }
        }

        public void LimpiarContactos() 
        {
            txtCntNombre.Text = "";
            txtCntApellidos.Text = "";
            txtCntCel.Text = "";
            txtCntCorreoE.Text = "";
            txtCntDepto.Text = "";
            txtCntFax.Text = "";
            txtCntTel.Text = "";           
        }

        protected void CargarCampos(bool bDatos)
        {
            if (!bDatos)
            {
                LimpiarValores(tbcForm);
                txtClave.Enabled = true;
                ckbActivo.Visible = false;

            }
            else
            {
                int idProveedor = (int)gdvLista.SelectedValue;
                MedDAL.DAL.proveedores oProveedor = oblProveedores.Buscar(idProveedor);

                txtClave.Enabled = false;
                ckbActivo.Visible = true;
                
                txtClave.Text = oProveedor.Clave;
                txtNombre.Text = oProveedor.Nombre;
                txtApellidos.Text = oProveedor.Apellidos;
                txtRfc.Text = oProveedor.Rfc;
                txtCurp.Text = oProveedor.Curp;
                ckbActivo.Checked = oProveedor.Activo;
                txtCalle.Text = oProveedor.Calle;
                txtNumExt.Text = oProveedor.NumeroExt;
                txtNumInt.Text = oProveedor.NumeroInt;
                txtPais.Text = "México";
                txtCodigoPostal.Text = oProveedor.CodigoPostal;
                txtTelefono.Text = oProveedor.Telefono;
                txtCelular.Text = oProveedor.Celular;
                txtFax.Text = oProveedor.Fax;
                txtCorreoElectronico.Text = oProveedor.CorreoElectronico;
                cmbTipoPersona.SelectedValue = oProveedor.TipoPersona;

                List<MedDAL.DAL.proveedores_contactos> lstProveedores = new List<MedDAL.DAL.proveedores_contactos>();
                lstProveedores.AddRange(oProveedor.proveedores_contactos);

                Session["lstproveedorescontactos"] = lstProveedores;
                gdvContactos.DataSource = lstProveedores;
                gdvContactos.DataBind();

                txtAlfanumerico1.Text = oProveedor.Campo1;
                txtAlfanumerico2.Text = oProveedor.Campo2;
                txtAlfanumerico3.Text = oProveedor.Campo3;
                txtAlfanumerico4.Text = oProveedor.Campo4;
                txtAlfanumerico5.Text = oProveedor.Campo5;
                txtEntero1.Text = oProveedor.Campo6.ToString();
                txtEntero2.Text = oProveedor.Campo7.ToString();
                txtEntero3.Text = oProveedor.Campo8.ToString();
                txtDecimal1.Text = oProveedor.Campo9.ToString();
                txtDecimal2.Text = oProveedor.Campo10.ToString();
            }
        }

        protected void Buscar(string sCadena)
        {
            int iTipo = 1;
            if (rdbTipos.Checked)
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

            
            var oQuery = oblProveedores.Buscar(sCadena, iTipo);
            Session["resultadoquery"] = oQuery;
            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Proveedores.ProveedoresView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Clave ASC";    

            try
            {
                gdvLista.DataSource = dv;
                //gdvLista.DataKeyNames = new string[] { "" };
                gdvLista.DataBind();
                gdvLista.Visible = true;
                if (txbBuscar.Text == "")
                {
                    gdvLista.EmptyDataText = "No existen proveedores registrados aun";
                }
                else
                {
                    gdvLista.EmptyDataText = "No existen proveedores que coincidan con la búsqueda";
                }
                gdvLista.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void OcultarTodo()
        {
            upnForm.Visible = false;
            pnlLista.Visible = false;
            pnlFiltroReportes.Visible = false;
        }

        protected void CargarFormulario(bool bDatos)
        {
            upnForm.Visible = true;
            pnlLista.Visible = false;            
            pnlFiltroReportes.Visible = false;

            CargarEstados(bDatos);
            CargarCampos(bDatos);
            CargarCamposEditables();
            CargarTipos(bDatos);
        }

        protected void CargarCatalogo() 
        {
            upnForm.Visible = false;
            pnlLista.Visible = true;            
            pnlFiltroReportes.Visible = false;
        }

        protected void Nuevo() 
        {
            int iErrores = 0;

            oProveedor = new MedDAL.DAL.proveedores();
            oProveedor.Clave = txtClave.Text;
            oProveedor.Nombre = txtNombre.Text;
            oProveedor.Apellidos = txtApellidos.Text;
            oProveedor.Rfc = txtRfc.Text;
            oProveedor.Curp = txtCurp.Text;
            oProveedor.Telefono = txtTelefono.Text;
            oProveedor.Celular = txtCelular.Text;
            oProveedor.Fax = txtFax.Text;
            oProveedor.CorreoElectronico = txtCorreoElectronico.Text;
            oProveedor.Fecha = DateTime.Now;
            oProveedor.Calle = txtCalle.Text;
            oProveedor.NumeroExt = txtNumExt.Text;
            oProveedor.NumeroInt = txtNumInt.Text;
            oProveedor.CodigoPostal = txtCodigoPostal.Text;
            oProveedor.TipoPersona = cmbTipoPersona.SelectedValue;
            oProveedor.Activo = true;

            oProveedor.IdTipoProveedor = int.Parse(cmbTipo.SelectedValue);
            oProveedor.IdEstado = int.Parse(cmbEstado.SelectedValue);
            oProveedor.IdMunicipio = int.Parse(cmbMunicipio.SelectedValue);
            oProveedor.IdPoblacion = int.Parse(cmbPoblacion.SelectedValue);
            oProveedor.IdColonia = int.Parse(cmbColonia.SelectedValue);

            oProveedor.Campo1 = txtAlfanumerico1.Text;
            oProveedor.Campo2 = txtAlfanumerico2.Text;
            oProveedor.Campo3 = txtAlfanumerico3.Text;
            oProveedor.Campo4 = txtAlfanumerico4.Text;
            oProveedor.Campo5 = txtAlfanumerico5.Text;
            oProveedor.Campo6 = txtEntero1.Text == ""? 0 : int.Parse(txtEntero1.Text);
            oProveedor.Campo7 = txtEntero2.Text == ""? 0 : int.Parse(txtEntero2.Text);
            oProveedor.Campo8 = txtEntero3.Text == ""? 0 : int.Parse(txtEntero3.Text);
            oProveedor.Campo9 = txtDecimal1.Text == ""? 0 : decimal.Parse(txtDecimal1.Text);
            oProveedor.Campo10 = txtDecimal2.Text == ""? 0 : decimal.Parse(txtDecimal2.Text);

            if (oblProveedores.NuevoRegistro(oProveedor))
            {
                lblAviso.Text = "El proveedor ha sido agregado con éxito";
                
                oProveedor = oblProveedores.Buscar(txtClave.Text);

                foreach (MedDAL.DAL.proveedores_contactos oContacto in (List<MedDAL.DAL.proveedores_contactos>)Session["lstproveedorescontactos"])
                {
                    oContacto.idProveedor = oProveedor.IdProveedor;
                    if (!oblProveedoresContactos.NuevoRegistro(oContacto))
                    {
                        iErrores++;
                    }
                }

                lblAviso2.Text = iErrores != 0 ? "No se agregaron los contactos del proveedor" : "";
                
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Proveedores";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Nuevo Proveedor";
                oBitacora.Descripcion = "Clave: " + txtClave.Text + ", Razón social: " + txtNombre.Text;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso3.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "No se agrego el nuevo proveedor";
            }
        }

        protected void Editar()
        {
            int iErrores = 0;

            oProveedor = new MedDAL.DAL.proveedores();
            oProveedor.IdProveedor = int.Parse(gdvLista.SelectedDataKey.Values[0].ToString());
            oProveedor.Nombre = txtNombre.Text;
            oProveedor.Apellidos = txtApellidos.Text;
            oProveedor.Rfc = txtRfc.Text;
            oProveedor.Curp = txtCurp.Text;
            oProveedor.Telefono = txtTelefono.Text;
            oProveedor.Celular = txtCelular.Text;
            oProveedor.Fax = txtFax.Text;
            oProveedor.CorreoElectronico = txtCorreoElectronico.Text;
            oProveedor.Calle = txtCalle.Text;
            oProveedor.NumeroExt = txtNumExt.Text;
            oProveedor.NumeroInt = txtNumInt.Text;
            oProveedor.CodigoPostal = txtCodigoPostal.Text;
            oProveedor.TipoPersona = cmbTipoPersona.SelectedValue;
            oProveedor.Activo = ckbActivo.Checked;

            oProveedor.IdTipoProveedor = int.Parse(cmbTipo.SelectedValue);
            oProveedor.IdEstado = int.Parse(cmbEstado.SelectedValue);
            oProveedor.IdMunicipio = int.Parse(cmbMunicipio.SelectedValue);
            oProveedor.IdPoblacion = int.Parse(cmbPoblacion.SelectedValue);
            oProveedor.IdColonia = int.Parse(cmbColonia.SelectedValue);

            oProveedor.Campo1 = txtAlfanumerico1.Text;
            oProveedor.Campo2 = txtAlfanumerico2.Text;
            oProveedor.Campo3 = txtAlfanumerico3.Text;
            oProveedor.Campo4 = txtAlfanumerico4.Text;
            oProveedor.Campo5 = txtAlfanumerico5.Text;
            oProveedor.Campo6 = txtEntero1.Text == "" ? 0 : int.Parse(txtEntero1.Text);
            oProveedor.Campo7 = txtEntero2.Text == "" ? 0 : int.Parse(txtEntero2.Text);
            oProveedor.Campo8 = txtEntero3.Text == "" ? 0 : int.Parse(txtEntero3.Text);
            oProveedor.Campo9 = txtDecimal1.Text == "" ? 0 : decimal.Parse(txtDecimal1.Text);
            oProveedor.Campo10 = txtDecimal2.Text == "" ? 0 : decimal.Parse(txtDecimal2.Text);

            if (oblProveedores.EditarRegistro(oProveedor))
            {
                lblAviso.Text = "El proveedor ha sido editado con éxito";

                oblProveedoresContactos.EliminarRegistro(oProveedor);

                foreach (MedDAL.DAL.proveedores_contactos oContacto in (List<MedDAL.DAL.proveedores_contactos>)Session["lstproveedorescontactos"])
                {
                    oContacto.idProveedor = oProveedor.IdProveedor;
                    if (!oblProveedoresContactos.NuevoRegistro(oContacto))
                    {
                        iErrores++;
                    }
                }

                lblAviso2.Text = iErrores != 0 ? "No se agregaron los contactos del proveedor" : "";

                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Proveedores";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Nuevo Proveedor";
                oBitacora.Descripcion = "Clave: " + txtClave.Text + ", Razón social: " + txtNombre.Text;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso3.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "No se editó el proveedor";
            }
        }

        protected void Eliminar()
        {
            int idProveedor = (int)gdvLista.SelectedValue;
            MedDAL.DAL.proveedores oProveedor = oblProveedores.Buscar(idProveedor);
            string sClave = oProveedor.Clave;
            
            //Si el proveedor tiene productos, no se debe de eliminar
            //if (oProveedor.productos.Count == 0)
            if (oProveedor.proveedores_productos.Count() == 0)
            {
                if (oblProveedores.EliminarRegistro(oProveedor))
                { 
                    lblAviso.Text = "El proveedor fue eliminado";
                    oBitacora = new MedDAL.DAL.bitacora();
                    oBitacora.FechaEntradaSrv = DateTime.Now;
                    oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                    oBitacora.Modulo = "Proveedor";
                    oBitacora.Usuario = Session["usuario"].ToString();
                    oBitacora.Nombre = Session["nombre"].ToString();
                    oBitacora.Accion = "Proveedor Eliminado";
                    oBitacora.Descripcion = "Clave: " + sClave;
                    if (!oblBitacora.NuevoRegistro(oBitacora))
                    {
                        lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                    }                
                }
                else
                {
                    lblAviso.Text = "El proveedor no pudo ser eliminado, es posible que tenga datos relacionados";
                }
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

            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';
            try
            {
                #region Interfaz
                cPermiso = (char)htbPermisos["proveedores"];
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
                imbAceptar.ValidationGroup = "Proveedores";
                imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
                imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
                rdbTipos = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbTipos.Text = "Tipo";
                rdbClave = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbClave.Text = "Clave";
                rdbNombre = (RadioButton)Master.FindControl("rdbFiltro3");
                rdbNombre.Text = "Nombre";
                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
                txbBuscar = (TextBox)Master.FindControl("txtBuscar");
                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "Proveedores";

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

                oblProveedores = new MedNeg.Proveedores.BlProveedores();
                oblProveedoresContactos = new MedNeg.ProveedoresContactos.BlProveedoresContactos();
                oblBitacora = new MedNeg.Bitacora.BlBitacora();
                oblCamposEditables = new MedNeg.CamposEditables.BlCamposEditables();
                oblColonias = new MedNeg.Colonias.BlColonias();
                oblEstados = new MedNeg.Estados.BlEstados();
                oblMunicipios = new MedNeg.Municipios.BlMunicipios();
                oblPoblaciones = new MedNeg.Poblaciones.BlPoblaciones();
                oblTipos = new MedNeg.Tipos.BlTipos();

                if (!IsPostBack)
                {
                    Session["lstproveedorescontactos"] = new List<MedDAL.DAL.proveedores_contactos>();
                    Session["provedoresaccion"] = 1;
                    CargarFormulario(false);
                    upnForm.Visible = false;
                    pnlFiltroReportes.Visible = false;
                    Session["resultadoquery"] = "";
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;

                    Session["reporteactivo"] = 0;
                    Session["reportdocument"] = "";
                    Session["titulo"] = "";
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                }

                gdvContactos.Visible = true;
                gdvContactos.ShowHeader = true;
                gdvContactos.DataSource = ((List<MedDAL.DAL.proveedores_contactos>)Session["lstproveedorescontactos"]);
                gdvContactos.DataBind();
                gdvContactos.DataKeyNames = new String[] { "idContactoProveedor" };
            }
            catch (NullReferenceException)
            {
                if (!ClientScript.IsStartupScriptRegistered("alertsession"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alertsession", "alertarSesion();", true);
                }                
                upnForm.Visible = false;
                pnlFiltroReportes.Visible = false;
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }

        protected void imbNuevo_Click(object sender, EventArgs e)
        {
            CargarFormulario(false);
            gdvLista.SelectedIndex = -1;
            Session["provedoresaccion"] = 1;
            ((List<MedDAL.DAL.proveedores_contactos>)Session["lstproveedorescontactos"]).Clear();
            lblAviso.Text = "";
            lblAviso2.Text = "";
            lblAviso3.Text = "";
            //0175 GT
            ConfigurarMenuBotones(true, false, false, false, true, true, false, false);
        }

        protected void imbEditar_Click(object sender, EventArgs e)
        {
            if (gdvLista.SelectedIndex != -1)
            {
                CargarFormulario(true);
                Session["provedoresaccion"] = 2;
                //0175 GT
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);
            }
            else
            {
                CargarCatalogo();
                Buscar("");
                //0175 GT
                ConfigurarMenuBotones(true, true, true, false, false, false, true, true);
            }
            lblAviso.Text = "";
            lblAviso2.Text = "";
            lblAviso3.Text = "";
        }

        protected void imbEliminar_Click(object sender, EventArgs e)
        {
            if (pnlLista.Visible && gdvLista.SelectedIndex != -1)
            {
                Eliminar();
                Buscar(txbBuscar.Text);
                //CargarCatalogo();
            }
            else
            {
                CargarCatalogo();
                Buscar(txbBuscar.Text);
            }
        }

        protected void imbMostrar_Click(object sender, EventArgs e)
        {
            CargarCatalogo();

            Buscar("");

            if (gdvLista.Rows.Count != 0)
            {
                gdvLista.SelectedIndex = -1;
                Session["provedoresaccion"] = 0;
                lblAviso.Text = "";
                lblAviso2.Text = "";
                lblAviso3.Text = "";
            }
            //0175 GT
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }

        protected void imbAceptar_Click(object sender, EventArgs e)
        {
            int iAccion;
            if (Session["provedoresaccion"] != null)
            {
                iAccion = (int)Session["provedoresaccion"];
            }
            else iAccion = 0;
            switch (iAccion)
            {
                case 0:
                    break;
                case 1:
                    if (txtClave.Enabled && CustomValidator1.IsValid && cmvTipo.IsValid && cmvEstado.IsValid && cmvMunicipio.IsValid && cmvPoblacion.IsValid && cmvColonia.IsValid)
                    {
                        Nuevo();
                        CargarFormulario(false);                        
                        ((List<MedDAL.DAL.proveedores_contactos>)Session["lstproveedorescontactos"]).Clear();
                    }
                    Session["provedoresaccion"] = 1;
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    break;
                case 2:
                    if (cmvTipo.IsValid && cmvEstado.IsValid && cmvMunicipio.IsValid && cmvPoblacion.IsValid && cmvColonia.IsValid) Editar();
                    //CargarFormulario(true);                    
                    //Buscar("", int.Parse(cmbMunicipioCatalogo.SelectedValue));
                    Session["provedoresaccion"] = 2;
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    break;
            }
        }

        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
            Session["provedoresaccion"] = 1;
            OcultarTodo();
            //CargarFormulario(false);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarCatalogo();

            Buscar(txbBuscar.Text);            

            if (gdvLista.Rows.Count != 0)
            {
                gdvLista.SelectedIndex = -1;
                Session["provedoresaccion"] = 0;
                lblAviso.Text = "";
                lblAviso2.Text = "";
                lblAviso3.Text = "";
            }
            //0175 GT
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }        

        protected void imbAgregarContacto_Click(object sender, ImageClickEventArgs e)
        {
            MedDAL.DAL.proveedores_contactos oContacto = new MedDAL.DAL.proveedores_contactos();
            oContacto.Nombre = txtCntNombre.Text;
            oContacto.Apellidos = txtCntApellidos.Text;
            oContacto.Telefono = txtCntTel.Text;
            oContacto.Fax = txtCntFax.Text;
            oContacto.Celular = txtCntCel.Text;
            oContacto.CorreoElectronico = txtCntCorreoE.Text;
            oContacto.Departamento = txtCntDepto.Text;
            oContacto.Activo = true;

            ((List<MedDAL.DAL.proveedores_contactos>)Session["lstproveedorescontactos"]).Add(oContacto);
            gdvContactos.DataBind();

            LimpiarContactos();
        }

        protected void gdvLista_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void cmbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarMunicipios(false);
        }

        protected void cmbMunicipio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPoblaciones(false);
        }

        protected void cmbPoblacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarColonias(false);
        }

        protected void cmbColonia_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (int.Parse(Session["provedoresaccion"].ToString()) != 2)
            {
                string sCosa = args.Value.ToString();
                MedDAL.DAL.proveedores oProveedor = oblProveedores.Buscar(sCosa);
                args.IsValid = oProveedor == null ? true : false;
            }
        }

        protected void gdvContactos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((List<MedDAL.DAL.proveedores_contactos>)Session["lstproveedorescontactos"]).RemoveAt(gdvContactos.SelectedIndex);
            gdvContactos.DataBind();
        }

        protected void cmvTipo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = cmbTipo.Items[0].Text != cnsSinTipos ? true : false;
        }
        protected void cmvEstado_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = cmbEstado.Items[0].Text != cnsSinEstados ? true : false;
        }
        protected void cmvMunicipio_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = cmbMunicipio.Items[0].Text != cnsSinMunicipios ? true : false;
        }
        protected void cmvPoblacion_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = cmbPoblacion.Items[0].Text != cnsSinPoblaciones ? true : false;
        }
        protected void cmvColonia_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = cmbColonia.Items[0].Text != cnsSinColonias ? true : false;
        }

        protected void cmbTipoPersona_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTipoPersona.SelectedValue == "FISICA")
            {
                lblNombre.Text = "Nombre(s):";
                txtApellidos.Enabled = true;                
            }
            else
            {
                lblNombre.Text = "Razon Social:";
                txtApellidos.Enabled = false;                
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

        public void CargarListaReportes()
        {
            ListBox lsbReportes = (ListBox)frReportes.FindControl("lsbSeleccionf");
            frReportes.LimpiarPaneles();
            lsbReportes.SelectedIndex = -1;
            lsbReportes.Items.Clear();
            if (Server.MapPath("~\\rptReportes\\Proveedores\\rptProveedores.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de proveedores");
            }
        }

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
            upnForm.Visible = false;
            pnlLista.Visible = false;            
            pnlFiltroReportes.Visible = true;

            CargarListaReportes();
        }

        protected void CargarReporte()
        {
            upnForm.Visible = false;
            pnlLista.Visible = false;            
            pnlFiltroReportes.Visible = false;

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from proveedores", "medicuriConnectionString", odsDataSet, "proveedores");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from proveedores_contactos", "medicuriConnectionString", odsDataSet, "proveedores_contactos");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from estados", "medicuriConnectionString", odsDataSet, "estados");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from municipios", "medicuriConnectionString", odsDataSet, "municipios");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from poblaciones", "medicuriConnectionString", odsDataSet, "poblaciones");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from colonias", "medicuriConnectionString", odsDataSet, "colonias");

            Session["campoaordenar"] = "";
            Session["dataset"] = odsDataSet;
            Session["titulo"] = "Impresión de Proveedores";
            //Session["configuracionsistema"] = objConfiguracion;
            Session["sortfield"] = 0;
            Session["reportdocument"] = "~\\rptReportes\\rptProveedores.rpt";

            if (gdvLista.SelectedIndex != -1)
            {
                Session["recordselection"] = "{proveedores.IdProveedor}=" + gdvLista.SelectedDataKey.Values[0].ToString();
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
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
            CargarReporte();
        }        
        #endregion

        #region SortingPaging

        protected void gdvLista_Sorting(object sender, GridViewSortEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Proveedores.ProveedoresView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            gdvLista.DataSource = oMaster.Sorting(e, ref oDireccion, dv);
            ViewState["direccionsorting"] = oDireccion;
            ViewState["sortexpression"] = e.SortExpression;
            gdvLista.DataBind(); 
        }

        protected void gdvLista_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Proveedores.ProveedoresView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            gdvLista.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Clave" : ViewState["sortexpression"].ToString(), dv, ref gdvLista, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            gdvLista.DataBind();
        }

        #endregion
    }
}