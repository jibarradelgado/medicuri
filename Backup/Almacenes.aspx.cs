using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Medicuri
{
	public partial class Almacenes : System.Web.UI.Page
	{
        const string cnsSinTipos = "Sin tipos...", cnsSinEstados = "Sin estados...", cnsSinMunicipios = "Sin municipios...", cnsSinPoblaciones = "Sin poblaciones...", cnsSinColonias = "Sin colonias...";
        ImageButton imbNuevo, imbEditar, imbEliminar, imbReportes, imbMostrar, imbAceptar, imbCancelar, imbImprimir;
        RadioButton rdbNombre, rdbClave, rdbTipo; 
        Button btnBuscar;
        TextBox txbBuscar;
        Label lblNombreModulo;
        MedNeg.Almacenes.BlAlmacenes oblAlmacenes;
        MedNeg.AlmacenesContactos.BlAlmacenesContactos oblAlmacenesContactos;
        MedNeg.Colonias.BlColonias oblColonias;
        MedNeg.Poblaciones.BlPoblaciones oblPoblaciones;
        MedNeg.Municipios.BlMunicipios oblMunicipios;
        MedNeg.Estados.BlEstados oblEstados;
        MedNeg.Tipos.BlTipos oblTipos;
        MedNeg.Bitacora.BlBitacora oblBitacora;
        MedNeg.CamposEditables.BlCamposEditables oblCamposEditables;
        MedDAL.DAL.almacenes oAlmacen;
        MedDAL.DAL.bitacora oBitacora;

        #region ActualizarSesiones
        /// <summary>
        /// Actualiza la variable de sesión "lsttiposalmacenes", la cual es una lista de todos los tipos de tipo Proveedor
        /// </summary>
        protected void ActualizarSesionTipos(bool bDatos)
        {
            List<MedDAL.DAL.tipos> lstTipos = new List<MedDAL.DAL.tipos>();
            if (!bDatos)
            {
                lstTipos = oblTipos.RecuperarTiposAlmacen();
            }
            else
            {
                lstTipos = oblTipos.RecuperarTipos();
            }

            Session["lsttiposalmacenes"] = lstTipos;
        }



        /// <summary>
        /// Actualiza la variable de sesion "lstestadosalmacenes", la cual es una lista de los estados activos
        /// </summary>
        protected void ActualizarSesionEstados()
        {
            IQueryable<MedDAL.DAL.estados> iqrEstados = oblEstados.BuscarEnum();
            Session["lstestadosalmacenes"] = iqrEstados;
        }

        /// <summary>
        /// Actualiza la variable de sesion de Municipios dependiendo de los Estados existentes.
        /// </summary>        
        protected void ActualizarSesionMunicipios()
        {
            IQueryable<MedDAL.DAL.municipios> iqrMunicipios = oblMunicipios.BuscarEstados(int.Parse(cmbEstados.SelectedValue));
            Session["lstmunicipiosalmacenes"] = iqrMunicipios;
        }

        /// <summary>
        /// Actualiza la variable de sesión de Poblaciones dependiendo de los Municipios existentes.
        /// </summary>        
        protected void ActualizarSesionPoblaciones()
        {
            IQueryable<MedDAL.DAL.poblaciones> iqrPoblaciones = oblPoblaciones.BuscarMunicipios(int.Parse(cmbMunicipios.SelectedValue));
            Session["lstpoblacionesalmacenes"] = iqrPoblaciones;
        }

        /// <summary>
        /// Actualiza la variable de sesión de Colonias dependiendo de las Poblaciones existentes.
        /// </summary>
        protected void ActualizarSesionColonias()
        {
            IQueryable<MedDAL.DAL.colonias> iqrColonias = oblColonias.BuscarPoblaciones(int.Parse(cmbPoblaciones.SelectedValue));            
            Session["lstcoloniasalmacenes"] = iqrColonias;
        }
        #endregion

        #region CargarLimpiarControles
        /// <summary>
        /// Carga cmbEstado, cmbMunicipio, cmbPoblacion y cmbColonia
        /// </summary>
        /// <param name="bDatos"></param>
        protected void CargarEstados(bool bDatos)
        {
            ActualizarSesionEstados();

            cmbEstados.Items.Clear();
            cmbEstados.DataSource = (IQueryable<MedDAL.DAL.estados>)Session["lstestadosalmacenes"];
            cmbEstados.DataBind();

            if (cmbEstados.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbEstados.SelectedIndex = 0;
                }
                else
                {
                    cmbEstados.SelectedValue = gdvLista.SelectedDataKey.Values[1].ToString();
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
        /// <summary>
        /// Carga cmbMunicipio, cmbPoblacion y cmbColonia
        /// </summary>
        /// <param name="bDatos"></param>
        protected void CargarMunicipios(bool bDatos)
        {
            ActualizarSesionMunicipios();

            cmbMunicipios.Items.Clear();
            cmbMunicipios.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstmunicipiosalmacenes"];
            cmbMunicipios.DataBind();

            if (cmbMunicipios.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbMunicipios.SelectedIndex = 0;
                }
                else
                {
                    cmbMunicipios.SelectedValue = gdvLista.SelectedDataKey.Values[2].ToString();
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
        /// <summary>
        /// Carga cmbPoblacion y cmbColonia
        /// </summary>
        /// <param name="bDatos"></param>
        protected void CargarPoblaciones(bool bDatos)
        {
            ActualizarSesionPoblaciones();

            cmbPoblaciones.Items.Clear();
            cmbPoblaciones.DataSource = (IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblacionesalmacenes"];
            cmbPoblaciones.DataBind();

            if (cmbPoblaciones.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbPoblaciones.SelectedIndex = 0;
                }
                else
                {
                    cmbPoblaciones.SelectedValue = gdvLista.SelectedDataKey.Values[3].ToString();
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
        /// <summary>
        /// Carga cmbColonia
        /// </summary>
        /// <param name="bDatos"></param>
        protected void CargarColonias(bool bDatos)
        {
            ActualizarSesionColonias();

            cmbColonias.Items.Clear();
            cmbColonias.DataSource = (IQueryable<MedDAL.DAL.colonias>)Session["lstcoloniasalmacenes"];
            cmbColonias.DataBind();

            if (cmbColonias.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbColonias.SelectedIndex = 0;
                }
                else
                {
                    cmbColonias.SelectedValue = gdvLista.SelectedDataKey.Values[4].ToString();
                }
            }
            else
            {
                cmbColonias.Items.Add(cnsSinColonias);
            }
        }
        /// <summary>
        /// Carga cmbTipos
        /// </summary>
        /// <param name="bDatos"></param>
        protected void CargarTipos(bool bDatos)
        {
            ActualizarSesionTipos(false);

            cmbTipos.Items.Clear();
            cmbTipos.DataSource = (List<MedDAL.DAL.tipos>)Session["lsttiposalmacenes"];
            cmbTipos.DataBind();

            if (cmbTipos.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbTipos.SelectedIndex = 0;
                }
                else
                {
                    ActualizarSesionTipos(false);

                    cmbTipos.Items.Clear();
                    cmbTipos.DataSource = (List<MedDAL.DAL.tipos>)Session["lsttiposalmacenes"];
                    cmbTipos.DataBind();

                    cmbTipos.SelectedValue = gdvLista.SelectedDataKey.Values[5].ToString();
                }
            }
            else
            {
                cmbTipos.Items.Add("Sin tipos...");
            }
        }
        /// <summary>
        /// Carga el texto que debe de aparecer en los labels de campos editables
        /// </summary>
        protected void CargarCamposEditables()
        {
            List<MedDAL.DAL.campos_editables> lstCamposEditables = oblCamposEditables.Buscar("Almacenes");
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
            if (c is TextBox)
            {
                if (c.ID != "txtPais") ((TextBox)c).Text = "";
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
                int idAlmacen = (int)gdvLista.SelectedValue;
                MedDAL.DAL.almacenes oAlmacen = oblAlmacenes.Buscar(idAlmacen);                

                txtClave.Enabled = false;
                ckbActivo.Visible = true;

                txtClave.Text = oAlmacen.Clave;
                txtNombre.Text = oAlmacen.Nombre;                
                ckbActivo.Checked = oAlmacen.Activo;
                txtCalle.Text = oAlmacen.Calle;
                txtNumExt.Text = oAlmacen.NumeroExt;
                txtNumInt.Text = oAlmacen.NumeroInt;
                txtPais.Text = "México";
                txtCodigoPostal.Text = oAlmacen.CodigoPostal;
                txtTelefono.Text = oAlmacen.Telefono;
                txtFax.Text = oAlmacen.Fax;

                List<MedDAL.DAL.almacenes_contactos> lstContactos = new List<MedDAL.DAL.almacenes_contactos>();
                lstContactos.AddRange(oAlmacen.almacenes_contactos);

                Session["lstalmacenescontactos"] = lstContactos;
                gdvContactos.DataSource = lstContactos;
                gdvContactos.DataBind();

                txtAlfanumerico1.Text = oAlmacen.Campo1;
                txtAlfanumerico2.Text = oAlmacen.Campo2;
                txtAlfanumerico3.Text = oAlmacen.Campo3;
                txtAlfanumerico4.Text = oAlmacen.Campo4;
                txtAlfanumerico5.Text = oAlmacen.Campo5;
                txtEntero1.Text = oAlmacen.Campo6.ToString();
                txtEntero2.Text = oAlmacen.Campo7.ToString();
                txtEntero3.Text = oAlmacen.Campo8.ToString();
                txtDecimal1.Text = oAlmacen.Campo9.ToString();
                txtDecimal2.Text = oAlmacen.Campo10.ToString();
            }
        }
        
        private void CargarFormulario(bool bDatos)
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

        protected void OcultarTodo()
        {
            upnForm.Visible = false;
            pnlLista.Visible = false;
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

        public void deshabilitaControl(Control c, bool borrar)
        {

            if (c is TextBox)
            {
                ((TextBox)c).Enabled = false;
                if (borrar)
                    ((TextBox)c).Text = string.Empty;
            }
            else if (c is DropDownList)
            {
                ((DropDownList)c).Enabled = false;
                if (borrar)
                    ((DropDownList)c).ClearSelection();
            }
            else if (c is CheckBox)
            {
                ((CheckBox)c).Enabled = false;
                if (borrar)
                    ((CheckBox)c).Checked = false;
            }

            foreach (Control ctrl in c.Controls)
                deshabilitaControl(ctrl, borrar);
        }
        #endregion

        #region InteraccionBL
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

            MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
            
            oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
            
            if (oUsuario.FiltradoActivado == true)
            {
                var oQuery = oblAlmacenes.BuscarFiltradaAlmacenes(sCadena, iTipo,oUsuario.idAlmacen);
                Session["resultadoquery"] = oQuery;
            }
            else
            {
                var oQuery = oblAlmacenes.Buscar(sCadena, iTipo);
                Session["resultadoquery"] = oQuery;                
            }

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Almacenes.AlmacenesView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Clave ASC";
            gdvLista.DataSource = dv;

            try
            {
                //gdvLista.DataSource = lstAlmacenes;                
                gdvLista.DataBind();
                gdvLista.Visible = true;
                if (txbBuscar.Text == "")
                {
                    gdvLista.EmptyDataText = "No existen almacenes registrados aun";
                }
                else
                {
                    gdvLista.EmptyDataText = "No existen almacenes que coincidan con la búsqueda";
                }
                gdvLista.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void Nuevo()
        {
            int iErrores = 0;

            oAlmacen = new MedDAL.DAL.almacenes();
            oAlmacen.Clave = txtClave.Text;
            oAlmacen.Nombre = txtNombre.Text;            
            oAlmacen.Telefono = txtTelefono.Text;
            oAlmacen.Fax = txtFax.Text;            
            oAlmacen.Calle = txtCalle.Text;
            oAlmacen.NumeroExt = txtNumExt.Text;
            oAlmacen.NumeroInt = txtNumInt.Text;
            oAlmacen.CodigoPostal = txtCodigoPostal.Text;
            oAlmacen.Activo = true;

            oAlmacen.idTipoAlmacen = int.Parse(cmbTipos.SelectedValue);
            oAlmacen.idEstado = int.Parse(cmbEstados.SelectedValue);
            oAlmacen.idMunicipio = int.Parse(cmbMunicipios.SelectedValue);
            oAlmacen.idPoblacion = int.Parse(cmbPoblaciones.SelectedValue);
            oAlmacen.idColonia = int.Parse(cmbColonias.SelectedValue);
            oAlmacen.Pais = txtPais.Text;

            oAlmacen.Campo1 = txtAlfanumerico1.Text;
            oAlmacen.Campo2 = txtAlfanumerico2.Text;
            oAlmacen.Campo3 = txtAlfanumerico3.Text;
            oAlmacen.Campo4 = txtAlfanumerico4.Text;
            oAlmacen.Campo5 = txtAlfanumerico5.Text;
            oAlmacen.Campo6 = txtEntero1.Text == "" ? 0 : int.Parse(txtEntero1.Text);
            oAlmacen.Campo7 = txtEntero2.Text == "" ? 0 : int.Parse(txtEntero2.Text);
            oAlmacen.Campo8 = txtEntero3.Text == "" ? 0 : int.Parse(txtEntero3.Text);
            oAlmacen.Campo9 = txtDecimal1.Text == "" ? 0 : decimal.Parse(txtDecimal1.Text);
            oAlmacen.Campo10 = txtDecimal2.Text == "" ? 0 : decimal.Parse(txtDecimal2.Text);

            if (oblAlmacenes.NuevoRegistro(oAlmacen))
            {
                lblAviso.Text = "El almacén ha sido agregado con éxito";

                oAlmacen = oblAlmacenes.Buscar(txtClave.Text);

                foreach (MedDAL.DAL.almacenes_contactos oContacto in (List<MedDAL.DAL.almacenes_contactos>)Session["lstalmacenescontactos"])
                {                    
                    oContacto.idAlmacen = oAlmacen.idAlmacen;
                    if (!oblAlmacenesContactos.NuevoRegistro(oContacto))
                    {
                        iErrores++;
                    }
                }

                lblAviso2.Text = iErrores != 0 ? "No se agregaron los contactos del almacén" : "";

                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Almacenes";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Nuevo Almacén";
                oBitacora.Descripcion = "Clave: " + txtClave.Text + ", Nombre: " + txtNombre.Text;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso3.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "No se agrego el nuevo almacén";
            }
        }

        protected void Editar()
        {
            int iErrores = 0;

            oAlmacen = new MedDAL.DAL.almacenes();
            oAlmacen.idAlmacen = int.Parse(gdvLista.SelectedDataKey.Values[0].ToString());
            oAlmacen.Nombre = txtNombre.Text;
            oAlmacen.Telefono = txtTelefono.Text;
            oAlmacen.Fax = txtFax.Text;
            oAlmacen.Calle = txtCalle.Text;
            oAlmacen.NumeroExt = txtNumExt.Text;
            oAlmacen.NumeroInt = txtNumInt.Text;
            oAlmacen.CodigoPostal = txtCodigoPostal.Text;
            oAlmacen.Activo = ckbActivo.Checked;

            oAlmacen.idTipoAlmacen = int.Parse(cmbTipos.SelectedValue);
            oAlmacen.idEstado = int.Parse(cmbEstados.SelectedValue);
            oAlmacen.idMunicipio = int.Parse(cmbMunicipios.SelectedValue);
            oAlmacen.idPoblacion = int.Parse(cmbPoblaciones.SelectedValue);
            oAlmacen.idColonia = int.Parse(cmbColonias.SelectedValue);

            oAlmacen.Campo1 = txtAlfanumerico1.Text;
            oAlmacen.Campo2 = txtAlfanumerico2.Text;
            oAlmacen.Campo3 = txtAlfanumerico3.Text;
            oAlmacen.Campo4 = txtAlfanumerico4.Text;
            oAlmacen.Campo5 = txtAlfanumerico5.Text;
            oAlmacen.Campo6 = txtEntero1.Text == "" ? 0 : int.Parse(txtEntero1.Text);
            oAlmacen.Campo7 = txtEntero2.Text == "" ? 0 : int.Parse(txtEntero2.Text);
            oAlmacen.Campo8 = txtEntero3.Text == "" ? 0 : int.Parse(txtEntero3.Text);
            oAlmacen.Campo9 = txtDecimal1.Text == "" ? 0 : decimal.Parse(txtDecimal1.Text);
            oAlmacen.Campo10 = txtDecimal2.Text == "" ? 0 : decimal.Parse(txtDecimal2.Text);

            if (oblAlmacenes.EditarRegistro(oAlmacen))
            {
                lblAviso.Text = "El almacén ha sido editado con éxito";

                oblAlmacenesContactos.EliminarRegistro(oAlmacen);

                
                foreach (MedDAL.DAL.almacenes_contactos oContacto in (List<MedDAL.DAL.almacenes_contactos>)Session["lstalmacenescontactos"])
                {                    
                    MedDAL.DAL.almacenes_contactos oContactoNuevo = new MedDAL.DAL.almacenes_contactos();
                    oContactoNuevo.idAlmacen = oAlmacen.idAlmacen;
                    oContactoNuevo.Nombre = oContacto.Nombre;
                    oContactoNuevo.Apellidos = oContacto.Apellidos;
                    oContactoNuevo.Telefono = oContacto.Telefono;
                    oContactoNuevo.Celular = oContacto.Celular;
                    oContactoNuevo.CorreoElectronico = oContacto.CorreoElectronico;

                    if (!oblAlmacenesContactos.NuevoRegistro(oContactoNuevo))
                    {
                        iErrores++;
                    }
                }

                lblAviso2.Text = iErrores != 0 ? "No se agregaron los contactos del almacén" : "";

                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Almacenes";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Nuevo Almacén";
                oBitacora.Descripcion = "Clave: " + txtClave.Text + ", Nombre: " + txtNombre.Text;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso3.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "No se editó el almacén";
            }
        }

        protected void Eliminar()
        {
            //string sClave = (((List<MedDAL.DAL.almacenes>)Session["lstAlmacenes"])[gdvLista.SelectedIndex]).Clave;
            int idAlmacen = (int)gdvLista.SelectedValue;
            MedDAL.DAL.almacenes oAlmacen = oblAlmacenes.Buscar(idAlmacen);
            string sClave = oAlmacen.Clave;

            if (oAlmacen.productos_almacen.Count == 0 && oAlmacen.faltantes.Count == 0 && oAlmacen.usuarios.Count == 0)
            {
                if (oblAlmacenes.EliminarRegistro(oAlmacen))
                {
                    lblAviso.Text = "El almacén fue eliminado";
                    oBitacora = new MedDAL.DAL.bitacora();
                    oBitacora.FechaEntradaSrv = DateTime.Now;
                    oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                    oBitacora.Modulo = "Almacén";
                    oBitacora.Usuario = Session["usuario"].ToString();
                    oBitacora.Nombre = Session["nombre"].ToString();
                    oBitacora.Accion = "Almacén Eliminado";
                    oBitacora.Descripcion = "Clave: " + sClave;
                    if (!oblBitacora.NuevoRegistro(oBitacora))
                    {
                        lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                    }
                }
                else
                {
                    lblAviso.Text = "El almacén no pudo ser eliminado, es posible que tenga datos relacionados";
                }
            }            
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
		{            
            
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';

            try
            {
                #region Interfaz
                cPermiso = (char)htbPermisos["almacenes"];

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
                imbAceptar.ValidationGroup = "Almacenes";
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
                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "Almacenes";

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

                oblAlmacenes = new MedNeg.Almacenes.BlAlmacenes();
                oblAlmacenesContactos = new MedNeg.AlmacenesContactos.BlAlmacenesContactos();
                oblColonias = new MedNeg.Colonias.BlColonias();
                oblPoblaciones = new MedNeg.Poblaciones.BlPoblaciones();
                oblMunicipios = new MedNeg.Municipios.BlMunicipios();
                oblEstados = new MedNeg.Estados.BlEstados();
                oblBitacora = new MedNeg.Bitacora.BlBitacora();
                oblCamposEditables = new MedNeg.CamposEditables.BlCamposEditables();
                oblTipos = new MedNeg.Tipos.BlTipos();                

                if (!IsPostBack)
                {                
                    Session["lstalmacenescontactos"] = new List<MedDAL.DAL.almacenes_contactos>();
                    Session["almacenesaccion"] = 0;    
                    //CargarFormulario(false);
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
                gdvContactos.DataSource = ((List<MedDAL.DAL.almacenes_contactos>)Session["lstalmacenescontactos"]);
                gdvContactos.DataBind();
                gdvContactos.DataKeyNames = new String[] { "idContacto" };
            }
            catch (NullReferenceException)
            {
                if (!ClientScript.IsStartupScriptRegistered("alertsession"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alertsession", "alertarSesion();", true);
                }
                upnForm.Visible = false;
                pnlLista.Visible = false;            
                pnlFiltroReportes.Visible = false;
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }

        #region DelegadosEventos
        protected void  imbNuevo_Click(object sender, EventArgs e)
        {
            CargarFormulario(false);
            gdvLista.SelectedIndex = -1;
            Session["almacenesaccion"] = 1;
            ((List<MedDAL.DAL.almacenes_contactos>)Session["lstalmacenescontactos"]).Clear();
            DataBind();
            lblAviso.Text = "";
            lblAviso2.Text = "";
            lblAviso3.Text = "";
            //0175 GT
            ConfigurarMenuBotones(true, false, false, false, true, true, false, false);
        }
        
        protected void  imbEditar_Click(object sender, EventArgs e)
        {
            if (gdvLista.SelectedIndex != -1)
            {
                CargarFormulario(true);
                Session["almacenesaccion"] = 2;
                //0175 GT
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);
            }
            else
            {
                CargarCatalogo();
                Buscar("");
            }
            lblAviso.Text = "";
            lblAviso2.Text = "";
            lblAviso3.Text = "";
        }
        
        protected void  imbEliminar_Click(object sender, EventArgs e)
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

        protected void  imbMostrar_Click(object sender, EventArgs e)
        {
            CargarCatalogo();

            Buscar("");

            if (gdvLista.Rows.Count != 0)
            {
                gdvLista.SelectedIndex = -1;
                Session["almacenesaccion"] = 0;
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
            if (Session["almacenesaccion"] != null)
            {
                iAccion = (int)Session["almacenesaccion"];
            }
            else iAccion = 0;
            switch (iAccion)
            {
                case 0:
                    break;
                case 1:
                    if (txtClave.Enabled && cmvClave.IsValid && cmvTipo.IsValid && cmvEstados.IsValid && cmvMunicipios.IsValid && cmvPoblaciones.IsValid && cmvColonias.IsValid)
                    {
                        Nuevo();
                        CargarFormulario(false);
                        ((List<MedDAL.DAL.almacenes_contactos>)Session["lstalmacenescontactos"]).Clear();
                        DataBind();
                        //GT 0175
                        ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    }
                    Session["almacenesaccion"] = 1;
                    break;
                case 2:
                    if (cmvTipo.IsValid && cmvEstados.IsValid && cmvMunicipios.IsValid && cmvPoblaciones.IsValid && cmvColonias.IsValid) Editar();
                    //CargarFormulario(true);                    
                    //Buscar("", int.Parse(cmbMunicipioCatalogo.SelectedValue));
                    Session["almacenesaccion"] = 2;
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    break;
            }
        }

        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            OcultarTodo();
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
            Session["almacenesaccion"] = 0;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar(txbBuscar.Text);
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);            
        }

        protected void imbAgregarContacto_Click(object sender, ImageClickEventArgs e)
        {
            MedDAL.DAL.almacenes_contactos contacto = new MedDAL.DAL.almacenes_contactos();
            contacto.Activo = true;
            contacto.Nombre = txtCntNombre.Text;
            contacto.Apellidos = txtCntApellidos.Text;
            contacto.Telefono = txtCntTel.Text;
            contacto.Celular = txtCntCel.Text;
            contacto.CorreoElectronico = txtCntCorreoE.Text;
            ((List<MedDAL.DAL.almacenes_contactos>)Session["lstalmacenescontactos"]).Add(contacto);
            gdvContactos.DataBind();

            LimpiarContactos();
        }

        protected void grvContactos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((List<MedDAL.DAL.almacenes_contactos>)Session["lstalmacenescontactos"]).RemoveAt(gdvContactos.SelectedIndex);
            gdvContactos.DataBind();
        }

        protected void cmbEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarMunicipios(false);
        }

        protected void cmbMunicipios_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPoblaciones(false);
        }

        protected void cmbPoblaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarColonias(false);
        }

        protected void cmvClave_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (int.Parse(Session["almacenesaccion"].ToString()) != 2)
            {
                string sClave = args.Value.ToString();
                MedDAL.DAL.almacenes oAlmacen = oblAlmacenes.Buscar(sClave);
                args.IsValid = oAlmacen == null ? true : false;
            }
        }

        protected void cmvTipo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = cmbTipos.Items[0].Text != cnsSinTipos ? true : false;
        }

        protected void cmvEstados_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = cmbEstados.Items[0].Text != cnsSinEstados ? true : false;
        }

        protected void cmvMunicipios_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = cmbMunicipios.Items[0].Text != cnsSinMunicipios ? true : false;
        }

        protected void cmvPoblaciones_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = cmbPoblaciones.Items[0].Text != cnsSinPoblaciones ? true : false;
        }

        protected void cmvColonias_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = cmbColonias.Items[0].Text != cnsSinColonias ? true : false;
        }

        protected void gdvLista_SelectedIndexChanged(object sender, EventArgs e)
        {

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
            if (Server.MapPath("~\\rptReportes\\Almacenes\\rptAlmacenes.rpt") != "") 
            {
                lsbReportes.Items.Add("Reporte de almacenes"); 
            }
            if (Server.MapPath("~\\rptReportes\\Almacenes\\rptDistribucionExistencias.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de distribución de existencias");
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
            MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes")
                :
                (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes_contactos", "medicuriConnectionString", odsDataSet, "almacenes_contactos");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from estados", "medicuriConnectionString", odsDataSet, "estados");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from municipios", "medicuriConnectionString", odsDataSet, "municipios");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from poblaciones", "medicuriConnectionString", odsDataSet, "poblaciones");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from colonias", "medicuriConnectionString", odsDataSet, "colonias");

            Session["campoaordenar"] = "";
            Session["dataset"] = odsDataSet;
            Session["titulo"] = "Impresión de Almacenes";
            //Session["configuracionsistema"] = objConfiguracion;
            Session["sortfield"] = 0;            
            Session["reportdocument"] = "~\\rptReportes\\rptAlmacenes.rpt";

            if (gdvLista.SelectedIndex != -1)
            {
                Session["recordselection"] = "{almacenes.idAlmacenes}=" + gdvLista.SelectedDataKey.Values[0].ToString();
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

        protected void gdvLista_Sorting(object sender, GridViewSortEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Almacenes.AlmacenesView>)Session["resultadoquery"];
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
            var result = (IQueryable<MedDAL.Almacenes.AlmacenesView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            gdvLista.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Clave" : ViewState["sortexpression"].ToString(), dv, ref gdvLista, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            gdvLista.DataBind();
        }
        #endregion
    }
}