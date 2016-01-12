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
    public partial class Ensambles : System.Web.UI.Page
    {
        #region Variables/Objetos
        Label lblNombreModulo;
        ImageButton imbNuevo, imbEditar, imbEliminar, imbImprimir, imbMostrar, imbAceptar, imbCancelar, imbReportes;
        RadioButton rdbNombre, rdbClave, rdbTodos;
        Button btnBuscar;
        TextBox txbBuscar;

        
        //Objetos de la capa de negocios
        MedNeg.Bitacora.BlBitacora oblBitacora;
        MedNeg.Ensambles.BlEnsambles oblEnsambles;
        MedNeg.Productos.BlProductos oblProductos;

        //Objetos de las entities
        MedDAL.DAL.bitacora oBitacora;
        MedDAL.DAL.ensamble oEnsamble;

        #endregion

        #region Configuracion de inicio
        protected void Page_Load(object sender, EventArgs e)
        {
            //Asignar titulo de modulo
            lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
            lblNombreModulo.Text = "Ensambles";

            //Cargar permisos
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';

            try
            {
                cPermiso = (char)htbPermisos["ensambles"];
                //Obtener los controles de master.
                imbNuevo = (ImageButton)Master.FindControl("imgBtnNuevo");
                imbNuevo.Click += new ImageClickEventHandler(this.imbNuevo_Click);
                imbEditar = (ImageButton)Master.FindControl("imgBtnEditar");
                imbEditar.Click += new ImageClickEventHandler(this.imbEditar_Click);
                imbEliminar = (ImageButton)Master.FindControl("imgBtnEliminar");
                imbEliminar.Click += new ImageClickEventHandler(this.imbEliminar_Click);
                imbMostrar = (ImageButton)Master.FindControl("imgBtnMostrar");
                imbMostrar.Click += new ImageClickEventHandler(this.imbMostrar_Click);
                imbAceptar = (ImageButton)Master.FindControl("imgBtnAceptar");
                imbAceptar.Click += new ImageClickEventHandler(this.imbAceptar_Click);
                imbImprimir = (ImageButton)Master.FindControl("imgBtnImprimir");
                imbImprimir.Click += new ImageClickEventHandler(this.imbImprimir_Click);
                imbAceptar.ValidationGroup = "vgEnsamble";
                imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
                imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
                rdbTodos = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbTodos.Text = "Clave BOM";
                rdbTodos.Checked = true;
                rdbClave = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbClave.Enabled = false;
                rdbClave.Visible = false;
                rdbNombre = (RadioButton)Master.FindControl("rdbFiltro3");
                rdbNombre.Enabled = false;
                rdbNombre.Visible = false;
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
                oblBitacora = new MedNeg.Bitacora.BlBitacora();
                oblEnsambles = new MedNeg.Ensambles.BlEnsambles();
                oblProductos = new MedNeg.Productos.BlProductos();

                if (!IsPostBack)
                {
                    MostrarAreaTrabajo(false, false);
                    Session["lstProductosDB"] = new List<MedDAL.DAL.ensamble_productos>();
                    Session["lstProductos"] = new List<MedNeg.Ensambles.EnsambleProductos>();
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
                MostrarAreaTrabajo(false, false);
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }
        #endregion

        #region EventHandlers

        /// <summary>
        /// Text changed de Clave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txbClave_TextChanged(object sender, EventArgs e)
        {
            CargaDatosProducto(txbClave.Text);
            txbCant.Focus();
        }

        /// <summary>
        /// Text changed de productos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txbProducto_TextChanged(object sender, EventArgs e)
        {
            BuscarProductoNombre(txbProducto.Text);
            txbCant.Focus();
        }


        public void imbNuevo_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            Session["modoGuardarEnsambles"] = true;
            MostrarAreaTrabajo(false, true);
            ModificarControl(this.pnlEnsamble, true, true);
            ModificarControl(this.pnlEnsambleProductos, true, true);
            Session["lstProductosDB"] = new List<MedDAL.DAL.ensamble_productos>();
            Session["lstProductos"] = new List<MedNeg.Ensambles.EnsambleProductos>();
            dgvEnsambleProductos.DataBind();
            dgvEnsambles.SelectedIndex = -1;
            //0175 GT
            ConfigurarMenuBotones(true, false, false, false, true, true, false, false);
        }

        public void imbEditar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            if (dgvEnsambles.SelectedIndex != -1)
            {
                Session["modoGuardarEnsambles"] = false;
                MostrarAreaTrabajo(false, true);
                Editar();
                //0175 GT
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);
            }
            else
            {
                MostrarAreaTrabajo(true, false);
                dgvEnsambles.SelectedIndex = -1;
            }
        }

        public void imbEliminar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            if (pnlList.Visible && dgvEnsambles.SelectedIndex != -1)
                Eliminar((int)dgvEnsambles.SelectedValue);

            Buscar(txbBuscar.Text);
        }

        public void imbMostrar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            Buscar("");
            //0175 GT
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }

        public void imbAceptar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            if ((bool)Session["modoGuardarEnsambles"])
                NuevoRegistro();
            else
                EditarRegistro();
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
        }

        public void imbCancelar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
            ModificarControl(this.pnlEnsamble, false, true);
            ModificarControl(this.pnlEnsambleProductos, false, true);
            MostrarAreaTrabajo(false, false);
            dgvEnsambles.SelectedIndex = -1;
        }

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
        }

        public void btnBuscar_Click(object sender, EventArgs e)
        {
            lblResults.Text = "";
            Buscar(txbBuscar.Text);
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }

        protected void imbAgregarDetalle_Click(object sender, ImageClickEventArgs e)
        {
            oblProductos = new MedNeg.Productos.BlProductos();
            MedDAL.DAL.productos nvoProducto = oblProductos.buscarProducto(txbClave.Text);
            MedNeg.Ensambles.EnsambleProductos nvoProductoEnsamble = new MedNeg.Ensambles.EnsambleProductos();

            nvoProductoEnsamble.clave1 = nvoProducto.Clave1;
            nvoProductoEnsamble.idProducto = nvoProducto.idProducto;
            nvoProductoEnsamble.nombre = nvoProducto.Nombre;
            nvoProductoEnsamble.presentacion = nvoProducto.Presentacion;
            nvoProductoEnsamble.precioPublico = nvoProducto.PrecioPublico.ToString();
            nvoProductoEnsamble.cantidad = int.Parse(txbCant.Text);
            ((List<MedNeg.Ensambles.EnsambleProductos>)Session["lstProductos"]).Add(nvoProductoEnsamble);
            dgvEnsambleProductos.DataSource = ((List<MedNeg.Ensambles.EnsambleProductos>)Session["lstProductos"]);
            dgvEnsambleProductos.DataBind();

            MedDAL.DAL.ensamble_productos nvoProdEnsamble = new MedDAL.DAL.ensamble_productos();
            nvoProdEnsamble.Cantidad = nvoProductoEnsamble.cantidad;
            nvoProdEnsamble.idProducto = nvoProductoEnsamble.idProducto;
            ((List<MedDAL.DAL.ensamble_productos>)Session["lstProductosDB"]).Add(nvoProdEnsamble);
            LimpiarDatosDetalle();
            txbClave.Focus();
        }

        protected void grvProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((List<MedDAL.DAL.ensamble_productos>)Session["lstProductosDB"]).RemoveAt(dgvEnsambleProductos.SelectedIndex);
            ((List<MedNeg.Ensambles.EnsambleProductos>)Session["lstProductos"]).RemoveAt(dgvEnsambleProductos.SelectedIndex);
            dgvEnsambleProductos.DataSource = ((List<MedNeg.Ensambles.EnsambleProductos>)Session["lstProductos"]);
            dgvEnsambleProductos.DataBind();
            dgvEnsambleProductos.SelectedIndex = -1;
        }
        #endregion

        #region Otros
        /// <summary>
        /// Cargar los datos de la partida con los datos del producto
        /// </summary>
        /// <param name="sClaveProducto"></param>
        private void CargaDatosProducto(string sClaveProducto)
        {
            MedDAL.DAL.productos oProducto = new MedDAL.DAL.productos();
            MedNeg.Productos.BlProductos oblProducto = new MedNeg.Productos.BlProductos();

            oProducto = oblProducto.buscarProducto(sClaveProducto);

            try
            {
                txbProducto.Text = oProducto.Nombre;
                txbIeps.Text = oProducto.TasaIeps.ToString();
                txbIva.Text = oProducto.tipo_iva.Iva.ToString();
                cmbPrecios.Items.Clear();
                cmbPrecios.Items.Add(oProducto.PrecioPublico.ToString());
                cmbPrecios.Items.Add(oProducto.Precio1.ToString());
                cmbPrecios.Items.Add(oProducto.Precio2.ToString());
                cmbPrecios.Items.Add(oProducto.Precio3.ToString());
                cmbPrecios.Items.Add(oProducto.PrecioMinimo.ToString());
                txbCant.Text = "1";
            }
            catch
            {
                txbProducto.Focus();
            }

        }

        /// <summary>
        /// Buscar producto por su nombre para llenar la partida
        /// </summary>
        /// <param name="sNombre"></param>
        private void BuscarProductoNombre(string sNombre)
        {
            MedDAL.DAL.productos oProducto = new MedDAL.DAL.productos();
            MedNeg.Productos.BlProductos oblProducto = new MedNeg.Productos.BlProductos();

            oProducto = oblProducto.BuscarProductoNombre(sNombre);
            try
            {
                txbClave.Text = oProducto.Clave1.ToString();
                txbProducto.Text = oProducto.Nombre;
                txbIeps.Text = oProducto.TasaIeps.ToString();
                txbIva.Text = oProducto.tipo_iva.Iva.ToString();
                cmbPrecios.Items.Clear();
                cmbPrecios.Items.Add(oProducto.PrecioPublico.ToString());
                cmbPrecios.Items.Add(oProducto.Precio1.ToString());
                cmbPrecios.Items.Add(oProducto.Precio2.ToString());
                cmbPrecios.Items.Add(oProducto.Precio3.ToString());
                cmbPrecios.Items.Add(oProducto.PrecioMinimo.ToString());
                txbCant.Text = "1";
            }
            catch
            {
                txbProducto.Focus();
            }
        }

        /// <summary>
        /// Limpiar los datos del detalle de la partida para ingresar nuevos valores
        /// </summary>
        private void LimpiarDatosDetalle()
        {
            txbClave.Text = "";
            txbProducto.Text = "";
            txbCant.Text = "";
            txbIeps.Text = "";
            txbIva.Text = "";
            //cmbPrecios.SelectedIndex = -1;
            cmbPrecios.Items.Clear();
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
            //pnlReportes.Visible = false;
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

            var oQuery = oblEnsambles.BuscarEnsamble(sCadena);
            Session["resultadoquery"] = oQuery;
            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;

            try
            {
                dgvEnsambles.DataSource = oQuery;
                dgvEnsambles.DataKeyNames = new string[] { "idEnsamble" };
                dgvEnsambles.DataBind();
                dgvEnsambles.SelectedIndex = -1;
                MostrarAreaTrabajo(true, false);
                if (dgvEnsambles.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    dgvEnsambles.EmptyDataText = "No existen Ensambles registrados aun";
                }
                else
                {
                    dgvEnsambles.EmptyDataText = "No existen Ensambles que coincidan con la búsqueda";
                }
                dgvEnsambles.ShowHeader = true;
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
            oEnsamble = new MedDAL.DAL.ensamble();
            PoblarDatosGenerales();
            if (ValidarEnsamble())
            {
                if (oblEnsambles.NuevoRegistroEnsamble(oEnsamble))
                {
                    NotificarAccion(true, "Se ha agregado correctamente el ensamble");
                    ModificarControl(this.pnlEnsamble, true, true);
                    ModificarControl(this.pnlEnsambleProductos, true, true);
                    dgvEnsambles.SelectedIndex = -1;
                    RegistrarEvento("Ensambles", "Agregar ensamble", "Se ha agregado el Ensamble " + oEnsamble.idEnsamble + ". Clave BOM: " + oEnsamble.ClaveBom +
                        ", Descripción: " + oEnsamble.Descripcion + ", Unidad de medida:" + oEnsamble.UnidadMedida );

                    oblEnsambles = new MedNeg.Ensambles.BlEnsambles();
                    if(!oblEnsambles.NuevoRegistroEnsambleProductos((List<MedDAL.DAL.ensamble_productos>)Session["lstProductosDB"], oEnsamble.idEnsamble))
                        NotificarAccion(false, "Se ha agregado correctamente el ensamble, pero no se pudieron agregar 1 o mas productos");
                }
                else
                    NotificarAccion(false, "No se ha podido agregar el ensamble");
            }
            else
                NotificarAccion(false, "Ya existe un ensamble con esa clave");
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
            oEnsamble.ClaveBom = txbClaveBom.Text;
            oEnsamble.Descripcion = txbNombre.Text;
            oEnsamble.UnidadMedida = txbUnidadMedida.Text;
            oEnsamble.PrecioMinimo = txbPrecioMinimo.Text == "" ? 0 : decimal.Parse(txbPrecioMinimo.Text);
            oEnsamble.PrecioPublico = txbPrecioPublico.Text == "" ? 0 : decimal.Parse(txbPrecioPublico.Text);
        }

        protected bool ValidarEnsamble()
        {

            if (oblEnsambles.ValidarEnsamble(txbClaveBom.Text) >= 1)
                return false;
            else
                return true;
        }

        #endregion

        #region Eliminar Registro
        protected void Eliminar(int idEnsamble)
        {
            oEnsamble = new MedDAL.DAL.ensamble();
            oblEnsambles = new MedNeg.Ensambles.BlEnsambles();
            oEnsamble = oblEnsambles.BuscarEnsamble(idEnsamble);
            if (oblEnsambles.EliminarEnsambleProductos(idEnsamble))
            {
                if (oblEnsambles.EliminarEnsamble(oEnsamble.idEnsamble))
                {
                    dgvEnsambles.SelectedIndex = -1;
                    NotificarAccion(true, "Se ha eliminado correctamente el ensamble");
                    RegistrarEvento("Ensambles", "Eliminar ensamble", "Se ha eliminado el Ensamble " + oEnsamble.idEnsamble + ". Clave BOM: " + oEnsamble.ClaveBom +
                        ", Descripción: " + oEnsamble.Descripcion + ", Unidad de medida:" + oEnsamble.UnidadMedida);
                }
                else
                    NotificarAccion(false, "No se ha podido eliminar al ensamble");
            }
            else
                NotificarAccion(false, "No se ha podido eliminar el ensamble porque aun tiene productos asociados");
        }

        #endregion

        #region Editar registro

        protected void EditarRegistro()
        {
            int idEnsamble = (int)dgvEnsambles.SelectedValue;
            oEnsamble = new MedDAL.DAL.ensamble();
            oEnsamble = oblEnsambles.BuscarEnsamble(idEnsamble);
            PoblarDatosGenerales();
            if (oblEnsambles.EditarRegistroEnsamble(oEnsamble))
            {
                oblEnsambles = new MedNeg.Ensambles.BlEnsambles();
                if (oblEnsambles.EliminarEnsambleProductos(idEnsamble) & oblEnsambles.NuevoRegistroEnsambleProductos((List<MedDAL.DAL.ensamble_productos>)Session["lstProductosDB"], idEnsamble))
                    NotificarAccion(true, "Se ha editado correctamente el cliente");
                else
                    NotificarAccion(true, "Se ha editado correctamente el Ensamble");
                RegistrarEvento("Ensambles", "Editar ensamble", "Se ha editado el Ensamble " + oEnsamble.idEnsamble + ". Clave BOM: " + oEnsamble.ClaveBom +
                        ", Descripción: " + oEnsamble.Descripcion + ", Unidad de medida:" + oEnsamble.UnidadMedida);
                ModificarControl(this.pnlEnsamble, false, false);
                ModificarControl(this.pnlEnsambleProductos, false, false);
            }
            else
                NotificarAccion(false, "No se ha podido editar el ensamble");
        }

        protected void Editar()
        {
            int idEnsamble = (int)dgvEnsambles.SelectedValue;
            ModificarControl(this.pnlEnsamble, true, true);
            ModificarControl(this.pnlEnsambleProductos, true, true);
            oEnsamble = new MedDAL.DAL.ensamble();
            oEnsamble = oblEnsambles.BuscarEnsamble(idEnsamble);
            LlenarDatosGenerales();
            Session["lstProductosDB"] = oblEnsambles.BuscarProductosEnsamble(idEnsamble);
            LlenarLista();
        }

        protected void LlenarDatosGenerales()
        {
            txbClaveBom.Text = oEnsamble.ClaveBom;
            txbUnidadMedida.Text = oEnsamble.UnidadMedida;
            txbNombre.Text = oEnsamble.Descripcion;
            txbPrecioPublico.Text = oEnsamble.PrecioPublico.ToString();
            txbPrecioMinimo.Text = oEnsamble.PrecioMinimo.ToString();
        }

        public void LlenarLista() {
            Session["lstProductos"] = new List<MedNeg.Ensambles.EnsambleProductos>();
            foreach (MedDAL.DAL.ensamble_productos oProducto in (List<MedDAL.DAL.ensamble_productos>)Session["lstProductosDB"])
            {
                oblProductos = new MedNeg.Productos.BlProductos();
                MedDAL.DAL.productos nvoProducto = oblProductos.Buscar(oProducto.idProducto);
                MedNeg.Ensambles.EnsambleProductos nvoProductoEnsamble = new MedNeg.Ensambles.EnsambleProductos();

                nvoProductoEnsamble.clave1 = nvoProducto.Clave1;
                nvoProductoEnsamble.idProducto = nvoProducto.idProducto;
                nvoProductoEnsamble.nombre = nvoProducto.Nombre;
                nvoProductoEnsamble.presentacion = nvoProducto.Presentacion;
                nvoProductoEnsamble.precioPublico = nvoProducto.PrecioPublico.ToString();
                nvoProductoEnsamble.cantidad = Convert.ToInt32(oProducto.Cantidad);
                ((List<MedNeg.Ensambles.EnsambleProductos>)Session["lstProductos"]).Add(nvoProductoEnsamble);

            }
            dgvEnsambleProductos.DataSource = ((List<MedNeg.Ensambles.EnsambleProductos>)Session["lstProductos"]);
            dgvEnsambleProductos.DataBind();
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
        protected void CargarReporte()
        {
            MostrarAreaTrabajo(false, false);
            //pnlReportes.Visible = true;

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from ensamble", "medicuriConnectionString", odsDataSet, "ensamble");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from ensamble_productos", "medicuriConnectionString", odsDataSet, "ensamble_productos");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");

              //GT 0179
            Session["campoaordenar"] = "";
            Session["sortfield"] = 0;  

            Session["dataset"] = odsDataSet;
            Session["reportdocument"] = "~\\rptReportes\\rptEnsambles.rpt";
            Session["titulo"] = "Ensambles";
            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));

            if (dgvEnsambles.SelectedIndex != -1)
            {
                Session["recordselection"] = "{ensamble.idEnsamble}=" + dgvEnsambles.SelectedDataKey.Values[0].ToString();
            }
            else
            {
                Session["recordselection"] = "";
            }


            rptReporte.SetDataSource(odsDataSet);
            //crvReporte.Visible = true;
            //crvReporte.ReportSource = rptReporte;

            //GT 0179 
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

        protected void ObtenerReporte()
        {
            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
            rptReporte.SetDataSource((DataSet)Session["dataset"]);
            //crvReporte.Visible = true;
            //crvReporte.ReportSource = rptReporte;
            //crvReporte.PageZoomFactor = 100;
        }

        
        ///// <summary>
        ///// Obtiene el ReportDocument a partir de un reporte existente en el proyecto.
        ///// </summary>
        ///// <param name="sNombreReporte"></param>
        ///// <returns></returns>
        //private ReportDocument getReportDocument(string sNombreReporte)
        //{
        //    string repFilePath = Server.MapPath(sNombreReporte);
        //    ReportDocument repDoc = new ReportDocument();
        //    repDoc.Load(repFilePath);
        //    return repDoc;
        //}
       
        //protected void //crvReporte_Drill(object source, CrystalDecisions.Web.DrillEventArgs e)
        //{
        //    ObtenerReporte();
        //}
        //protected void //crvReporte_DrillDownSubreport(object source, CrystalDecisions.Web.DrillSubreportEventArgs e)
        //{
        //    ObtenerReporte();
        //}
        //protected void //crvReporte_Navigate(object source, CrystalDecisions.Web.NavigateEventArgs e)
        //{
        //    ObtenerReporte();
        //    //crvReporte.PageZoomFactor = 50;
        //}
        //protected void //crvReporte_Search(object source, CrystalDecisions.Web.SearchEventArgs e)
        //{
        //    ObtenerReporte();
        //    //crvReporte.PageZoomFactor = 50;
        //}
        //protected void //crvReporte_ViewZoom(object source, CrystalDecisions.Web.ZoomEventArgs e)
        //{
        //    ObtenerReporte();
        //}
        //protected void //crvReporte_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["reportdocument"].ToString() != "")
        //    {
        //        ObtenerReporte();
        //    }
        //}
        //protected void btnPdf_Click(object sender, EventArgs e)
        //{
        //    if (int.Parse(Session["reporteactivo"].ToString()) == 1)
        //    {
        //        // Get the report document
        //        string sReporte = Session["reportdocument"].ToString();
        //        ReportDocument repDoc = getReportDocument(sReporte);
        //        //Esta linea soluciona el problema de la excepcion que no permite guardar los archivos
        //        repDoc.SetDataSource((DataSet)Session["dataset"]);
        //        // Stop buffering the response
        //        Response.Buffer = false;
        //        // Clear the response content and headers
        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        try
        //        {
        //            // Export the Report to Response stream in PDF format
        //            repDoc.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, Session["titulo"].ToString());
        //            // There are other format options available such as Word, Excel, CVS, and HTML in the ExportFormatType Enum given by crystal reports
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            ex = null;
        //        }
        //    }
        //}
        //protected void btnExcel_Click(object sender, EventArgs e)
        //{
        //    if (int.Parse(Session["reporteactivo"].ToString()) == 1)
        //    {
        //        // Get the report document
        //        string sReporte = Session["reportdocument"].ToString();
        //        ReportDocument repDoc = getReportDocument(sReporte);
        //        //Esta linea soluciona el problema de la excepcion que no permite guardar los archivos
        //        repDoc.SetDataSource((DataSet)Session["dataset"]);
        //        // Stop buffering the response
        //        Response.Buffer = false;
        //        // Clear the response content and headers
        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        try
        //        {
        //            // Export the Report to Response stream in Excel format
        //            repDoc.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, Response, true, Session["titulo"].ToString());
        //            // There are other format options available such as Word, Excel, CVS, and HTML in the ExportFormatType Enum given by crystal reports
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            ex = null;
        //        }
        //    }
        //}
        //protected void btnCrystal_Click(object sender, EventArgs e)
        //{
        //    if (int.Parse(Session["reporteactivo"].ToString()) == 1)
        //    {
        //        // Get the report document
        //        string sReporte = Session["reportdocument"].ToString();
        //        ReportDocument repDoc = getReportDocument(sReporte);
        //        //Esta linea soluciona el problema de la excepcion que no permite guardar los archivos
        //        repDoc.SetDataSource((DataSet)Session["dataset"]);
        //        // Stop buffering the response
        //        Response.Buffer = false;
        //        // Clear the response content and headers
        //        Response.ClearContent();
        //        Response.ClearHeaders();
        //        try
        //        {
        //            // Export the Report to Response stream in Crystal Report format
        //            repDoc.ExportToHttpResponse(ExportFormatType.CrystalReport, Response, true, Session["titulo"].ToString());
        //            // There are other format options available such as Word, Excel, CVS, and HTML in the ExportFormatType Enum given by crystal reports
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            ex = null;
        //        }
        //    }
        //}
        #endregion

        #region SortingPaging

        protected void dgvEnsambles_Sorting(object sender, GridViewSortEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.DAL.ensamble>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvEnsambles.DataSource = oMaster.Sorting(e, ref oDireccion, dv);
            ViewState["direccionsorting"] = oDireccion;
            ViewState["sortexpression"] = e.SortExpression;
            dgvEnsambles.DataBind();
        }

        protected void dgvEnsambles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.DAL.ensamble>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvEnsambles.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Clave" : ViewState["sortexpression"].ToString(), dv, ref dgvEnsambles, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            dgvEnsambles.DataBind();
        }

        #endregion
    }
}