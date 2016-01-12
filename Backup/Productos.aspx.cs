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
    public partial class Productos : System.Web.UI.Page
    {
        #region Variables/Objetos
        Label lblNombreModulo;
        ImageButton imbNuevo, imbEditar, imbEliminar, imbMostrar, imbImprimir, imbAceptar, imbCancelar, imbPrecios, imbReportes;
        RadioButton rdbNombre, rdbClave, rdbTipo, rdbClave2;
        Button btnBuscar;
        TextBox txbBuscar;

        //Colecciones de objetos de los Entities
        IQueryable<MedDAL.DAL.tipos> iqrTipos;
        IQueryable<MedDAL.DAL.tipo_iva> iqrTiposIva;

        //Objetos de la capa de negocios
        MedNeg.Tipos.BlTipos oblTipos;
        MedNeg.TiposIva.BlTiposIva oblTiposIva;
        MedNeg.Productos.BlProductos oblProducto;
        MedNeg.Bitacora.BlBitacora oblBitacora;
        MedNeg.Proveedores.BlProveedores oblProveedores;
        MedNeg.Inventarios.BlInventarios oblInventarios;

        //Objetos de las entities
        MedDAL.DAL.bitacora oBitacora;
        MedDAL.DAL.productos oProductos;
        #endregion

        #region Configuraciones de inicio
        protected void Page_Load(object sender, EventArgs e)
        {//Asignar titulo de modulo
            lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
            lblNombreModulo.Text = "Productos";
            txbClave1.Attributes.Add("onkeydown", "return (event.keyCode!=13);");
            //Cargar permisos
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';

            try
            {
                //Obtener los controles de master.
                cPermiso = (char)htbPermisos["productos"];
                imbNuevo = (ImageButton)Master.FindControl("imgBtnNuevo");
                imbNuevo.Click += new ImageClickEventHandler(this.imbNuevo_Click);
                imbEditar = (ImageButton)Master.FindControl("imgBtnEditar");
                imbEditar.Click += new ImageClickEventHandler(this.imbEditar_Click);
                imbEliminar = (ImageButton)Master.FindControl("imgBtnEliminar");
                imbEliminar.Click += new ImageClickEventHandler(this.imbEliminar_Click);
                imbPrecios = (ImageButton)Master.FindControl("imgBtnPrecios");
                imbPrecios.Click += new ImageClickEventHandler(this.imbPrecios_Click);
                imbImprimir = (ImageButton)Master.FindControl("imgBtnImprimir");
                imbImprimir.Click += new ImageClickEventHandler(this.imbImprimir_Click);
                imbReportes = (ImageButton)Master.FindControl("imgBtnReportes");
                imbReportes.Click += new ImageClickEventHandler(this.imbReportes_Click);
                imbMostrar = (ImageButton)Master.FindControl("imgBtnMostrar");
                imbMostrar.Click += new ImageClickEventHandler(this.imbMostrar_Click);
                imbAceptar = (ImageButton)Master.FindControl("imgBtnAceptar");
                imbAceptar.Click += new ImageClickEventHandler(this.imbAceptar_Click);
                imbAceptar.ValidationGroup = "vgProductos";
                imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
                imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
                rdbTipo = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbTipo.Text = "Tipo";
                rdbClave = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbClave.Text = "Clave 1";
                rdbNombre = (RadioButton)Master.FindControl("rdbFiltro3");
                rdbNombre.Text = "Nombre";
                rdbClave2 = (RadioButton)Master.FindControl("rdbFiltro4");
                rdbClave2.Text = "Clave 2";
                rdbClave2.Visible = true;
                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
                txbBuscar = (TextBox)Master.FindControl("txtBuscar");

                //GT 0175
                imbReportes = (ImageButton)Master.FindControl("imgBtnReportes");
                imbReportes.Click += new ImageClickEventHandler(this.imbReportes_Click);

                Master.FindControl("btnPrecios").Visible = true;
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
                oblTipos = new MedNeg.Tipos.BlTipos();
                oblTiposIva = new MedNeg.TiposIva.BlTiposIva();
                oblBitacora = new MedNeg.Bitacora.BlBitacora();
                oblProducto = new MedNeg.Productos.BlProductos();
                oblProveedores = new MedNeg.Proveedores.BlProveedores();
                oblInventarios = new MedNeg.Inventarios.BlInventarios();

                CargarCamposEditables();
                if (!IsPostBack)
                {
                    Session["lstproveedores"] = new List<MedDAL.DAL.proveedores_productos>();
                    MostrarAreaTrabajo(false, false);
                    divCambioPrecios.Visible = false;
                    Session["resultadoquery"] = "";
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;

                    Session["reporteactivo"] = 0;
                    Session["reportdocument"] = "";
                    Session["titulo"] = "";
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                }

                gdvCatalogoProveedor.DataSource = ((List<MedDAL.DAL.proveedores_productos>)Session["lstproveedores"]);
                gdvCatalogoProveedor.DataBind();
            }
            catch (NullReferenceException)
            {
                if (!ClientScript.IsStartupScriptRegistered("alertsession"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alertsession", "alertarSesion();", true);
                }
                MostrarAreaTrabajo(false, false);
                divCambioPrecios.Visible = false;
                pnlFiltroReportes.Visible = false;
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }
        #endregion

        #region EventHandlers

        protected void txbClave2_TextChanged(object sender, EventArgs e)
        {
            oblProducto = new MedNeg.Productos.BlProductos();
            MedDAL.DAL.productos oProducto = oblProducto.buscarProductoClave2(txbClave2.Text);
            if (oProducto != null)
            {
                txbNombre.Text = oProducto.Nombre;
                txaDescripcion.Text = oProducto.Descripcion;
                cmbTipo.SelectedValue = oProducto.idTipoProducto.ToString();
                txbPresentacion.Text = oProducto.Presentacion;
            }
        }

        public void txbCodigoProveedor_TextChanged(object sender, EventArgs e)
        {
            try
            {
                MedDAL.Proveedores.DALProveedores dalProveedor = new MedDAL.Proveedores.DALProveedores();
                MedDAL.DAL.proveedores oProveedor = new MedDAL.DAL.proveedores();
                MedDAL.Estados.DALEstados oEstados = new MedDAL.Estados.DALEstados();
                MedDAL.Tipos.DALTipos dalTipos = new MedDAL.Tipos.DALTipos();
                oProveedor = dalProveedor.Buscar(txbCodigoProveedor.Text);
                txbTelefonoProveedor.Text = oProveedor.Telefono;
                txbNombreProveedor.Text = oProveedor.Nombre;
                Session["oproveedor"] = oProveedor;
            }
            catch {
                txbCodigoProveedor.Text = "No disponible";
            }
        }

        public void imbNuevo_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            Session["modoGuardar"] = true;
            ((List<MedDAL.DAL.proveedores_productos>)Session["lstproveedores"]).Clear();
            gdvCatalogoProveedor.DataBind();
            MostrarAreaTrabajo(false, true);
            ModificarControl(this.tabContainerPrincipal, true, true);
            txbTelefonoProveedor.Enabled = false;
            txbNombreProveedor.Enabled = false;
            chkActivo.Checked = true;
            CargarDdlTipos(false);
            CargarDdlIva();
            dgvProducto.SelectedIndex = -1;
            divCambioPrecios.Visible = false;            
            //0175 GT
            ConfigurarMenuBotones(true, false, false, false, true, true, false, false);
        }

        public void imbEditar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            if (dgvProducto.SelectedIndex != -1)
            {
                Session["modoGuardar"] = false;
                MostrarAreaTrabajo(false, true);
                divCambioPrecios.Visible = false;                
                Editar();
                //0175 GT
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);
            }
            else
            {                
                Buscar("");
                divCambioPrecios.Visible = false;                
            }
        }

        public void imbEliminar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            if (pnlList.Visible && dgvProducto.SelectedIndex != -1)
                Eliminar((int)dgvProducto.SelectedValue);

            Buscar(txbBuscar.Text);
        }

        public void imbMostrar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            divCambioPrecios.Visible = false;
            upnForm.Visible = false;
            
            Buscar("");
            //0175 GT
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }
        protected void imbPrecios_Click(object sender, EventArgs e)
        {
            pnlList.Visible = false;
            upnForm.Visible = false;
            divCambioPrecios.Visible = true;
            
            cargaDdlCambioPrecios();
        }
        public void imbAceptar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            if ((bool)Session["modoGuardar"])
            {
                NuevoRegistro();                
            }
            else
            {
                EditarRegistro();
            }
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
        }

        public void imbCancelar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            ModificarControl(this.tabContainerPrincipal, false, true);
            MostrarAreaTrabajo(false, false);
            divCambioPrecios.Visible = false;
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
        }

        public void btnBuscar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            Buscar(txbBuscar.Text);            
            divCambioPrecios.Visible = false;
            upnForm.Visible = false;
           
            //0175 GT
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }

         protected void imbAgregarProveedor_Click(object sender, ImageClickEventArgs e)
        {
            if(Session["oproveedor"] != null)
            {
            MedDAL.DAL.proveedores_productos oProveedorProducto = new MedDAL.DAL.proveedores_productos();
            oProveedorProducto.idProveedor = ((MedDAL.DAL.proveedores)Session["oproveedor"]).IdProveedor;
            oProveedorProducto.proveedores = new MedDAL.DAL.proveedores();
            oProveedorProducto.proveedores.Nombre = txbNombreProveedor.Text;
            oProveedorProducto.proveedores.Clave = txbCodigoProveedor.Text;
            oProveedorProducto.proveedores.Telefono = txbTelefonoProveedor.Text;
            ((List<MedDAL.DAL.proveedores_productos>)Session["lstproveedores"]).Add(oProveedorProducto);
            gdvCatalogoProveedor.DataBind();
            
            LimpiarFormularioProveedor();
            Session["oproveedor"] = null;
            }
        }

         protected void gdvCatalogoProveedor_SelectedIndexChanged(object sender, EventArgs e)
         {
             ((List<MedDAL.DAL.proveedores_productos>)Session["lstproveedores"]).RemoveAt(gdvCatalogoProveedor.SelectedIndex);
             gdvCatalogoProveedor.DataBind();
         }

        protected void dgvProducto_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //if (dgvProducto.Rows.Count > 0)
            //{
            //    dgvProducto.Rows[dgvProducto.Rows.Count - 1].Cells[14].Text = ((List<MedDAL.DAL.productos>)Session["lstProductos"])[dgvProducto.Rows.Count - 1].tipos.Nombre;
            //}
        }

        protected void gdvCatalogoProveedor_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (gdvCatalogoProveedor.Rows.Count > 0)
            {
                gdvCatalogoProveedor.Rows[gdvCatalogoProveedor.Rows.Count - 1].Cells[0].Text = ((List<MedDAL.DAL.proveedores_productos>)Session["lstProveedores"])[gdvCatalogoProveedor.Rows.Count - 1].proveedores.Clave;
                gdvCatalogoProveedor.Rows[gdvCatalogoProveedor.Rows.Count - 1].Cells[1].Text = ((List<MedDAL.DAL.proveedores_productos>)Session["lstProveedores"])[gdvCatalogoProveedor.Rows.Count - 1].proveedores.Nombre;
                gdvCatalogoProveedor.Rows[gdvCatalogoProveedor.Rows.Count - 1].Cells[2].Text = ((List<MedDAL.DAL.proveedores_productos>)Session["lstProveedores"])[gdvCatalogoProveedor.Rows.Count - 1].proveedores.Telefono;
            }
        }

        protected void chbCmPrTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (chbCmPrTodos.Checked)
            {
                ddlCmPrDesde.Visible = ddlCmPrHasta.Visible = Label11.Visible = Label12.Visible = false;
            }
            else
            {
                ddlCmPrDesde.Visible = ddlCmPrHasta.Visible = Label11.Visible = Label12.Visible = true;
            }
        }

        protected void btnGuardarCambioPrecio_Click(object sender, EventArgs e)
        {
            FinalizaCambioPrecios();
        }
        #endregion

        #region Cargar datos por omisión

        private void CargarDdlTipos(bool bDatos)
        {
            if (!bDatos)
            {
                iqrTipos = (IQueryable<MedDAL.DAL.tipos>)oblTipos.Buscar("Productos", 2);
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

        private void CargarDdlIva()
        {
            iqrTiposIva = oblTiposIva.BuscarGral();
            cmbIVA.Items.Clear();
            cmbIVA.DataSource = iqrTiposIva;
            cmbIVA.DataBind();
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

        protected void LimpiarFormularioProveedor()
        {
            txbCodigoProveedor.Text = "";
            txbNombreProveedor.Text = "";
            txbTelefonoProveedor.Text = "";
        }

        /// <summary>
        /// Carga el texto que debe de aparecer en los labels de campos editables
        /// </summary>
        protected void CargarCamposEditables()
        {
            MedNeg.CamposEditables.BlCamposEditables oblCamposEditables = new MedNeg.CamposEditables.BlCamposEditables();
            List<MedDAL.DAL.campos_editables> lstCamposEditables = oblCamposEditables.Buscar("Productos");
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
            else if (rdbClave2.Checked)
            {
                iTipo = 4;
            }

            MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();

            oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());

            if (oUsuario.FiltradoActivado == true)
            {
                var oQuery = oblProducto.BuscarFiltradaAlmacen(sCadena, iTipo,oUsuario.idAlmacen);
                Session["resultadoquery"] = oQuery;
                //List<MedDAL.DAL.productos> lstProductos = new List<MedDAL.DAL.productos>();
                //lstProductos.AddRange((IQueryable<MedDAL.DAL.productos>)oQuery);
                //Session["lstProductos"] = lstProductos;                
            }
            else
            {
                var oQuery = oblProducto.Buscar(sCadena, iTipo);
                Session["resultadoquery"] = oQuery;                
                //List<MedDAL.DAL.productos> lstProductos = new List<MedDAL.DAL.productos>();
                //lstProductos.AddRange((IQueryable<MedDAL.DAL.productos>)oQuery);
                //Session["lstProductos"] = lstProductos;                
            }
            
            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Productos.ProductoView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Clave1 ASC";
            dgvProducto.DataSource = dv;

            try
            {
                
                dgvProducto.DataKeyNames = new string[] { "idProducto" };
                dgvProducto.DataBind();
                MostrarAreaTrabajo(true, false);
                if (dgvProducto.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    dgvProducto.EmptyDataText = "No existen productos registrados aun";
                }
                else
                {
                    dgvProducto.EmptyDataText = "No existen productos que coincidan con la búsqueda";
                }
                dgvProducto.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }
        #endregion

        #region Insertar Nuevo Registro
        protected void NuevoRegistro()
        {
            oProductos = new MedDAL.DAL.productos();
            PoblarDatosGeneralesClaves();
            PoblarDatosGeneralesPresentacion();
            PoblarDatosOpcionales();
            PoblarDatosPrecios();
            PoblarDatosInventarios();
            if (ValidarProducto(1))
            {
                if (oblProducto.NuevoRegistro(oProductos))
                {
                    int iErrores = 0;

                    oProductos = oblProducto.buscarProducto(oProductos.Clave1);

                    foreach (MedDAL.DAL.proveedores_productos oProveedor in (List<MedDAL.DAL.proveedores_productos>)Session["lstproveedores"])
                    {
                        MedDAL.DAL.proveedores_productos oProveedorNuevo = new MedDAL.DAL.proveedores_productos();
                        oProveedorNuevo.idProducto = oProductos.idProducto;
                        oProveedorNuevo.idProveedor = oProveedor.idProveedor;

                        if (!oblProducto.NuevoProveedorProducto(oProveedorNuevo))
                        {
                            iErrores++;
                        }
                    }

                    if (iErrores == 0)
                    {
                        NotificarAccion(true, "Se ha agregado correctamente el producto");
                        ModificarControl(this.tabContainerPrincipal, true, true);
                        ((List<MedDAL.DAL.proveedores_productos>)Session["lstproveedores"]).Clear();
                        gdvCatalogoProveedor.DataBind();
                        CargarDdlTipos(false);
                        dgvProducto.SelectedIndex = -1;
                        RegistrarEvento("Productos", "Agregar producto", "Se ha agregado el producto " + oProductos.idProducto + ": " + oProductos.Nombre +
                            ", Clave1: " + oProductos.Clave1 + ", Descripción:" + oProductos.Descripcion + ", Precio Publico: "+ oProductos.PrecioPublico + 
                            ", Precio minimo: " + oProductos.PrecioMinimo);
                    }else
                        NotificarAccion(false, "Se ha agregado el producto pero no se pudo agregar el proveedor");
                }
                else
                    NotificarAccion(false, "No se ha podido agregar el producto");
            }
            else
                NotificarAccion(false, "Ya existe un producto con esa clave1");
        }

        protected void PoblarDatosGeneralesClaves()
        {
            /*Datos generales - claves*/
            oProductos.Clave1 = txbClave1.Text;
            oProductos.Clave2 = txbClave2.Text;
            oProductos.Clave3 = txbClave3.Text;
            oProductos.Clave4 = txbClave4.Text;
            oProductos.Nombre = txbNombre.Text;
            oProductos.idTipoProducto = int.Parse(cmbTipo.SelectedValue);
            oProductos.Descripcion = txaDescripcion.Text;
            oProductos.Activo = chkActivo.Checked;
        }

        protected void PoblarDatosGeneralesPresentacion()
        {
            /*Datos generales - presentacion*/
            oProductos.Presentacion = txbPresentacion.Text;
            //oProductos.Costo = txbCostoEstandar.Text;
            oProductos.UnidadMedida = txbUnidadMedida.Text;
            oProductos.DescripcionAdicional = txaDescripcionAdicional.Text;

        }

        protected void PoblarDatosProveedor() { 
            //oProductos.idTipoProducto
        }
        
        protected void PoblarDatosOpcionales()
        {
            /*Datos opcionales*/
            oProductos.Campo1 = txbAlfanumerico1.Text;
            oProductos.Campo2 = txbAlfanumerico2.Text;
            oProductos.Campo3 = txbAlfanumerico3.Text;
            oProductos.Campo4 = txbAlfanumerico4.Text;
            oProductos.Campo5 = txbAlfanumerico5.Text;

            if (txbEntero1.Text.Equals(""))
                oProductos.Campo6 = 0;
            else
                oProductos.Campo6 = Convert.ToInt32(txbEntero1.Text);

            if (txbEntero2.Text.Equals(""))
                oProductos.Campo7 = 0;
            else
                oProductos.Campo7 = Convert.ToInt32(txbEntero2.Text);

            if (txbEntero3.Text.Equals(""))
                oProductos.Campo8 = 0;
            else
                oProductos.Campo8 = Convert.ToInt32(txbEntero3.Text);

            if (txbDecimal1.Text.Equals(""))
                oProductos.Campo9 = 0;
            else
                oProductos.Campo9 = Convert.ToDecimal(txbDecimal1.Text);


            if (txbDecimal2.Text.Equals(""))
                oProductos.Campo10 = 0;
            else
                oProductos.Campo10 = Convert.ToDecimal(txbDecimal2.Text);
        }

        protected void PoblarDatosPrecios() {
            if (txbPublico.Text.Equals(""))
                oProductos.PrecioPublico = 0;
            else
                oProductos.PrecioPublico = Convert.ToDecimal(txbPublico.Text);
            
            if (txbMinimo.Text.Equals(""))
                oProductos.PrecioMinimo = 0;
            else
                oProductos.PrecioMinimo = Convert.ToDecimal(txbMinimo.Text);

            if (txbPrecio1.Text.Equals(""))
                oProductos.Precio1 = 0;
            else
                oProductos.Precio1 = Convert.ToDecimal(txbPrecio1.Text);

            if (txbPrecio2.Text.Equals(""))
                oProductos.Precio2 = 0;
            else
                oProductos.Precio2 = Convert.ToDecimal(txbPrecio2.Text);

            if (txbPrecio3.Text.Equals(""))
                oProductos.Precio3 = 0;
            else
                oProductos.Precio3 = Convert.ToDecimal(txbPrecio3.Text);
            oProductos.TipoMoneda = txbMondea.Text;
            oProductos.idTipoIva = int.Parse(cmbIVA.SelectedValue);

            if (txbIEPS.Text.Equals(""))
                oProductos.TasaIeps = 0;
            else
                oProductos.TasaIeps = Convert.ToDecimal(txbIEPS.Text);

            oProductos.Costeo = cmbCosteo.SelectedValue;
        }

        protected void PoblarDatosInventarios() {
            string manejaLote = "0";
            string manejaSerie = "0";
            if (chkLote.Checked)
                manejaLote = "1";
            oProductos.ManejaLote = manejaLote;
            if (chkSerie.Checked)
                manejaSerie = "1";
            oProductos.ManejaSeries = manejaSerie;
            if (txbStockMaximo.Text == "")
            {
                oProductos.StockMaximo = 0;
            }
            else
            {
                oProductos.StockMaximo = Convert.ToInt32(txbStockMaximo.Text);
            }

            if (txbStockMinimo.Text == "")
            {
                oProductos.StockMinimo = 0;
            }
            else
            {
                oProductos.StockMinimo = Convert.ToInt32(txbStockMinimo.Text);
            }

            if (txbDiasSurtido.Text == "")
            {
                oProductos.DiasResurtido = 0;
            }
            else
            {
                oProductos.DiasResurtido = Convert.ToInt32(txbDiasSurtido.Text);
            }
           
            
        }

        protected bool ValidarProducto(int iTipo)
        {
            if (iTipo == 1)
            {
                if (oblProducto.ValidarProductoRepetido(txbClave1.Text) >= 1)
                    return false;
                else
                    return true;
            }
            else
            {
                if (oblProducto.ValidarProductoRepetido(txbClave1.Text, (int)dgvProducto.SelectedValue) >= 1)
                {
                    return false;
                }
                else
                    return true;
            }
        }

        #endregion

        #region Eliminar Registro
        protected void Eliminar(int idProducto)
        {
            oProductos = new MedDAL.DAL.productos();
            oProductos = oblProducto.Buscar(idProducto);
            if (oblProducto.EliminarProveedorProducto(idProducto))
            {
                if (oblProducto.EliminarRegistro(oProductos.idProducto))
                {
                    NotificarAccion(true, "Se ha eliminado correctamente el producto");
                    RegistrarEvento("Producto", "Eliminar producto", "Se ha elminado el producto " + oProductos.idProducto + " Nombre: " + oProductos.Nombre +
                        ", Clave1: " + oProductos.Clave1);
                }
                else
                    NotificarAccion(false, "No se ha podido eliminar el producto");
            }
            else
                NotificarAccion(false, "El producto todavia tiene proveedores asociados, no se ha podido eliminar");
        }
        #endregion

        #region Editar registro

        protected void EditarRegistro()
        {
            int idProducto = (int)dgvProducto.SelectedValue;
            oProductos = new MedDAL.DAL.productos();
            oProductos = (MedDAL.DAL.productos)oblProducto.Buscar(idProducto);
            PoblarDatosGeneralesClaves();
            PoblarDatosGeneralesPresentacion();
            PoblarDatosProveedor();
            PoblarDatosOpcionales();
            PoblarDatosPrecios();
            PoblarDatosInventarios();
            if (ValidarProducto(2))
            {
                if (oblProducto.EditarRegistro(oProductos))
                {
                    int iErrores = 0;

                    oblProducto.EliminarProveedorProducto(oProductos.idProducto);

                    foreach (MedDAL.DAL.proveedores_productos oProveedor in (List<MedDAL.DAL.proveedores_productos>)Session["lstproveedores"])
                    {
                        MedDAL.DAL.proveedores_productos oProveedorNuevo = new MedDAL.DAL.proveedores_productos();
                        oProveedorNuevo.idProducto = oProductos.idProducto;
                        oProveedorNuevo.idProveedor = oProveedor.idProveedor;

                        if (!oblProducto.NuevoProveedorProducto(oProveedorNuevo))
                        {
                            iErrores++;
                        }
                    }

                    if (iErrores != 0)
                    {
                        NotificarAccion(false, "Se ha editado el producto, pero no se guardaron sus proveedores");
                    }

                    RegistrarEvento("Producto", "Editar producto", "Se ha editado el producto " + oProductos.idProducto + ": " + oProductos.Nombre +
                                ", Clave1: " + oProductos.Clave1 + ", Descripción:" + oProductos.Descripcion + ", Precio Publico: " + oProductos.PrecioPublico +
                                ", Precio minimo: " + oProductos.PrecioMinimo);
                    ModificarControl(this.tabContainerPrincipal, false, false);
                }
                else
                    NotificarAccion(false, "No se ha podido editar el producto");
            }
            else
                NotificarAccion(false, "La Clave1 que quiere ingresar esta repetida");
        }

        protected void Editar()
        {
            int idProducto = (int)dgvProducto.SelectedValue;
            ModificarControl(this.tabContainerPrincipal, true, true);
            oProductos = new MedDAL.DAL.productos();
            oProductos = (MedDAL.DAL.productos)oblProducto.Buscar(idProducto);
            CargarDdlIva();
            LlenarDatosGeneralesClaves();
            LlenarDatosGeneralesPresentacion();
            LlenarDatosProveedor();
            LlenarDatosOpcionales();
            LlenarDatosPrecios();
            LlenarDatosInventarios();
        }

        protected void LlenarDatosGeneralesClaves()
        {
            txbClave1.Text = oProductos.Clave1;
            txbClave2.Text = oProductos.Clave2;
            txbClave3.Text = oProductos.Clave3;
            txbClave4.Text = oProductos.Clave4;
            txbNombre.Text = oProductos.Nombre;
            chkActivo.Checked = oProductos.Activo;
            CargarDdlTipos(false);
            cmbTipo.SelectedValue = oProductos.idTipoProducto.ToString();
            txaDescripcion.Text = oProductos.Descripcion;
        }

        protected void LlenarDatosGeneralesPresentacion()
        {
            txbPresentacion.Text = oProductos.Presentacion;
            txbUnidadMedida.Text = oProductos.UnidadMedida;
            txaDescripcionAdicional.Text = oProductos.DescripcionAdicional;
        }

        protected void LlenarDatosProveedor()
        {
            List<MedDAL.DAL.proveedores_productos> lstProveedores = new List<MedDAL.DAL.proveedores_productos>();
            lstProveedores.AddRange(oProductos.proveedores_productos);

            Session["lstproveedores"] = lstProveedores;
            gdvCatalogoProveedor.DataSource = lstProveedores;
            gdvCatalogoProveedor.DataBind();
        }

        protected void LlenarDatosOpcionales()
        {
            /*Datos opcionales*/
            txbAlfanumerico1.Text = oProductos.Campo1;
            txbAlfanumerico2.Text = oProductos.Campo2;
            txbAlfanumerico3.Text = oProductos.Campo3;
            txbAlfanumerico4.Text = oProductos.Campo4;
            txbAlfanumerico5.Text = oProductos.Campo5;

            txbEntero1.Text = oProductos.Campo6.ToString();
            txbEntero2.Text = oProductos.Campo7.ToString();
            txbEntero3.Text = oProductos.Campo8.ToString();

            txbDecimal1.Text = oProductos.Campo9.ToString();
            txbDecimal2.Text = oProductos.Campo10.ToString();
        }

        protected void LlenarDatosPrecios() {
            txbPublico.Text = oProductos.PrecioPublico.ToString();
            txbMinimo.Text = oProductos.PrecioMinimo.ToString();
            txbPrecio1.Text = oProductos.Precio1.ToString();
            txbPrecio2.Text = oProductos.Precio2.ToString();
            txbPrecio3.Text = oProductos.Precio3.ToString();
            txbMondea.Text = oProductos.TipoMoneda;
            txbIEPS.Text = oProductos.TasaIeps.ToString();
            cmbCosteo.SelectedValue = oProductos.Costeo;
            cmbIVA.SelectedValue = oProductos.idTipoIva.ToString();
        }

        protected void LlenarDatosInventarios() { 
            bool serie = false;
            bool lote = false;
            if (oProductos.ManejaSeries!=null)
                if( oProductos.ManejaSeries.Equals("1"))
                    serie = true;
            if (oProductos.ManejaLote!=null) 
                if(oProductos.ManejaLote.Equals("1"))
                    lote = true;
            chkLote.Checked = lote;
            chkSerie.Checked = serie;
            txbStockMaximo.Text = oProductos.StockMaximo.ToString();
            txbStockMinimo.Text = oProductos.StockMinimo.ToString();
            txbDiasSurtido.Text = oProductos.DiasResurtido.ToString();
            
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

        #region CambioPrecios
        private void FinalizaCambioPrecios()
        {
            try
            {
                List<MedDAL.Inventarios.ErrorCambioPrecio> errores;
                MedDAL.DAL.bitacora oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Inventarios";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Cambio de precios";


                if (chbCmPrTodos.Checked)
                {
                    errores = oblInventarios.FinalizaCambioPrecios(ddlCmPrListasPrecios.SelectedValue, ddlCmPrOperacion.SelectedValue,
                        ddlCmPrTipo.SelectedValue, decimal.Parse(txtCmPrCantidad.Text));
                    oBitacora.Descripcion = "Todos | " + ddlCmPrOperacion.SelectedValue + " | " + ddlCmPrTipo.SelectedValue + " | " + "Cantidad: " + txtCmPrCantidad.Text;
                }
                else
                {
                    errores = oblInventarios.FinalizaCambioPrecios(ddlCmPrDesde.SelectedItem.Text, ddlCmPrHasta.SelectedItem.Text, ddlCmPrListasPrecios.SelectedValue, ddlCmPrOperacion.SelectedValue,
                           ddlCmPrTipo.SelectedValue, decimal.Parse(txtCmPrCantidad.Text));
                    oBitacora.Descripcion = "Desde: " + ddlCmPrDesde.Text + " Hasta: " + ddlCmPrHasta.Text + " | " +
                                            ddlCmPrOperacion.SelectedValue + " | " + ddlCmPrTipo.SelectedValue + " | " + "Cantidad: " + txtCmPrCantidad.Text;
                }

                oblInventarios.NuevoRegistroBitacora(oBitacora);

                if (errores != null)
                {
                    lbCmPrAvisos.Text = "Proceso finalizado con reporte de violaciones a la política de precios";
                    grvCmPrErrores.Visible = true;
                    grvCmPrErrores.DataSource = errores;
                    grvCmPrErrores.DataBind();
                }
                else
                    lbCmPrAvisos.Text = "Proceso finalizado";
            }
            catch (Exception ex)
            {
                lbCmPrAvisos.Text = "Error: " + ex.Message;
            }
        }

        private void cargaDdlCambioPrecios()
        {
            ddlCmPrDesde.DataSource = ddlCmPrHasta.DataSource = oblInventarios.buscarTodosProductos();
            ddlCmPrDesde.DataBind();
            ddlCmPrHasta.DataBind();
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

            //GT 0249 14/jul/2011 
            if (Server.MapPath("~\\rptReportes\\Productos\\rptProductosLista.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de productos");
            }

            if (Server.MapPath("~\\rptReportes\\Productos\\rptProductos.rpt") != "")
            {
                //GT 0248 14/jul/2011 
                //lsbReportes.Items.Add("Reporte de productos");
                lsbReportes.Items.Add("Reporte de productos por almacen");
            }
            if (Server.MapPath("~\\rptReportes\\Productos\\rptProductosUltimaFactura.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de productos con dato de ultima salida por factura");
            }
            if (Server.MapPath("~\\rptReportes\\Productos\\rptProductosUltimaReceta.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de productos con dato de ultima salida por receta");
            }
            if (Server.MapPath("~\\rptReportes\\Productos\\rptProductosUltimaRemision.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de productos con dato de ultima salida por remisión");
            }
            if (Server.MapPath("~\\rptReportes\\Productos\\rptListaPrecios.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de lista de precios");
            }
            if (Server.MapPath("~\\rptReportes\\Productos\\rptStockMinimo.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de stock mínimo");
            }
            if (Server.MapPath("~\\rptReportes\\Productos\\rptStockMaximo.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de stock máximo");
            }
        }

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
            MostrarAreaTrabajo(false, false);
            divCambioPrecios.Visible = false;
            pnlFiltroReportes.Visible = true;

            CargarListaReportes();
        }

        protected void CargarReporte()
        {
            MostrarAreaTrabajo(false, false);
            divCambioPrecios.Visible = false;            

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");

            Session["campoaordenar"] = "";
            Session["dataset"] = odsDataSet;
            Session["titulo"] = "Impresión de Productos";
            //Session["configuracionsistema"] = objConfiguracion;
            Session["sortfield"] = 0;
            Session["reportdocument"] = "~\\rptReportes\\rptProductos.rpt";

            if (dgvProducto.SelectedIndex != -1)
            {
                Session["recordselection"] = "{productos.idProducto}=" + dgvProducto.SelectedDataKey.Values[0].ToString();
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
                
        protected void dgvProducto_Sorting(object sender, GridViewSortEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Productos.ProductoView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvProducto.DataSource = oMaster.Sorting(e, ref oDireccion, dv);
            ViewState["direccionsorting"] = oDireccion;
            ViewState["sortexpression"] = e.SortExpression;
            dgvProducto.DataBind();            
            //string sSortExpression = e.SortExpression;
            //ViewState["sortexpression"] = e.SortExpression;

            //if ((System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"] == System.Web.UI.WebControls.SortDirection.Ascending)
            //{
            //    e.SortDirection = System.Web.UI.WebControls.SortDirection.Descending;
            //    SortGridView(sSortExpression, DESCENDING);
            //    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Descending;
            //}
            //else
            //{
            //    e.SortDirection = System.Web.UI.WebControls.SortDirection.Ascending;
            //    SortGridView(sSortExpression, ASCENDING);
            //    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            //}
        }

        protected void dgvProducto_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Productos.ProductoView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvProducto.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Clave1" : ViewState["sortexpression"].ToString(), dv, ref dgvProducto, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            dgvProducto.DataBind();
            //int iPagina = e.NewPageIndex;
            //dgvProducto.PageIndex = iPagina;
            //var result = (IQueryable<MedDAL.Productos.ProductoView>)Session["resultadoquery"];                        
            //DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            //DataView dv = new DataView(dt);
            //if ((System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"] == System.Web.UI.WebControls.SortDirection.Ascending)
            //{
            //    dv.Sort = ViewState["sortexpression"] == null ? "Clave1 ASC" : ViewState["sortexpression"].ToString() + ASCENDING;
            //}
            //else
            //{
            //    dv.Sort = ViewState["sortexpression"] == null ? "Clave1 DESC" : ViewState["sortexpression"].ToString() + DESCENDING;
            //}            
            
            //dgvProducto.DataSource = dv;
            //dgvProducto.DataBind();
        }

        private void SortGridView(string sortExpression, string direction)
        {
            //var result = oblEstados.Buscar("", 1);
            //var result = (IQueryable<MedDAL.Productos.ProductoView>)Session["resultadoquery"];            
            //DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            //DataView dv = new DataView(dt);
            //dv.Sort = sortExpression + direction;
            
            //dgvProducto.DataSource = dv;
            //dgvProducto.DataBind();
        }
                
        #endregion

        



    }
}