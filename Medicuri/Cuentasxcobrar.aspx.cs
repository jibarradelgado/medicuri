using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Data.Sql;

namespace Medicuri
{
    public partial class cuentasxcobrar : System.Web.UI.Page
    {
        ImageButton imbNuevo, imbEditar, imbEliminar, imbReportes, imbMostrar, imbAceptar, imbCancelar;
        RadioButton rdbNombre, rdbClave, rdbTodos;
        Label lblReportes, lblNombreModulo, lblEditar;
        Button btnBuscar;
        TextBox txbBuscar;
        MedNeg.Facturas.BlDetallePartida oblDetallePartida;

        MedDAL.DAL.facturas oFactura;
        MedNeg.Facturas.BlFacturas oblFacturas;
        //Declaración del objeto de la capa de negocio de la bitacora
        MedNeg.Bitacora.BlBitacora oblBitacora;

        //Declaración del objeto de la capa de Datos de bitacora
        MedDAL.DAL.bitacora oBitacora;

        /// <summary>
        /// PAGE LOAD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            #region Interfaz
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';

            try
            {
                cPermiso = (char)htbPermisos["facturas"];

                Master.FindControl("btnNuevo").Visible = false;
                Master.FindControl("btnEliminar").Visible = false;
                Master.FindControl("btnAceptar").Visible = false;
                Master.FindControl("btnCancelar").Visible = false;
                Master.FindControl("btnImprimir").Visible = false;
                //Master.FindControl("imgBtnReportes").Visible = false;
                //Master.FindControl("btnMostrar").Visible = false;
                //Master.FindControl("btnReportes").Visible = false;
                //Master.FindControl("rdbFiltro1").Visible = false;
                //Master.FindControl("rdbFiltro2").Visible = false;
                //Master.FindControl("rdbFiltro3").Visible = false;
                //Master.FindControl("btnBuscar").Visible = false;
                //Master.FindControl("txtBuscar").Visible = false;
                //Master.FindControl("lblBuscar").Visible = false;


                //imbNuevo = (ImageButton)Master.FindControl("imgBtnNuevo");
                //imbNuevo.Click += new ImageClickEventHandler(this.imbNuevo_Click);
                imbEditar = (ImageButton)Master.FindControl("imgBtnEditar");
                imbEditar.Click += new ImageClickEventHandler(this.imbEditar_Click);
                //imbEliminar = (ImageButton)Master.FindControl("imgBtnEliminar");
                //imbEliminar.Click += new ImageClickEventHandler(this.imbEliminar_Click);
                imbMostrar = (ImageButton)Master.FindControl("imgBtnMostrar");
                imbMostrar.Click += new ImageClickEventHandler(this.imbMostrar_Click);
                imbAceptar = (ImageButton)Master.FindControl("imgBtnAceptar");
                imbAceptar.Click += new ImageClickEventHandler(this.imbAceptar_Click);
                //imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
                //imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
                imbReportes = (ImageButton)Master.FindControl("imgBtnReportes");
                imbReportes.Click += new ImageClickEventHandler(this.imbReportes_Click);

                rdbTodos = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbTodos.Text = "Folio";
                rdbClave = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbClave.Text = "Cliente";
                rdbNombre = (RadioButton)Master.FindControl("rdbFiltro3");
                rdbNombre.Text = "Fecha";

                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
                txbBuscar = (TextBox)Master.FindControl("txtBuscar");


                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "Cuentas Por Cobrar";
                lblEditar = (Label)Master.FindControl("lblEditar");
                lblEditar.Text = "Aplicación";




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

                dgvPartidaDetalle.Visible = true;
                dgvPartidaDetalle.ShowHeader = true;
                dgvPartidaDetalle.EmptyDataText = "Sin Detalle";
                dgvPartidaDetalle.DataSource = ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]);
                dgvPartidaDetalle.DataBind();
                txbClave.TextChanged += new EventHandler(txbClave_TextChanged);

                if (!IsPostBack)
                {

                    // Saber si estan activados los folios automaticos y poner su valor por default
                    // Se almacena en una variable de sesión para comparar que se esta respetando el formato automatico
                    // y validar que no haya cambiado el folio de pedidos debido a otro registro mientras se hacia el actual 
                    oblFacturas = new MedNeg.Facturas.BlFacturas();
                    Session["iFolioAutomatico"] = oblFacturas.RecuperaFolioAutomatico(Server.MapPath("~/Archivos/Configuracion.xml"));
                    txbFolio.Text = Session["iFolioAutomatico"].ToString();
                    Session["sEsDePedido"] = false;
                    Session["sEsDeRemision"] = false;
                    Session["sEsDeReceta"] = false;
                    Session["lstDetallePartida"] = new List<MedNeg.Facturas.BlDetallePartida>();
                    pnlCatalogo.Visible = false;
                    pnlFormulario.Visible = false;
                    //pnlReporte.Visible = false;
                    Session["sTotalFactura"] = 0;
                    Session["resultadoquery"] = "";
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;

                    pnlFiltroReportes.Visible = false;
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
                pnlFormulario.Visible = false;
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }

        protected void CargarCatalogo()
        {
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = true;

        }


        /// <summary>
        /// Mostrar la lista de todas las facturas
        /// </summary>
        protected void MostrarLista()
        {
            oblFacturas = new MedNeg.Facturas.BlFacturas();
            var oQuery = oblFacturas.MostrarListaCuentasCobrar();
            Session["resultadoquery"] = oQuery;

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Facturas.CuentasxCobrarView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Nombre ASC";
            
            try
            {
                dgvDatos.DataSource = dv;
                dgvDatos.DataKeyNames = new string[] { "idFactura" };
                dgvDatos.DataBind();
                CargarCatalogo();
                if (dgvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    dgvDatos.EmptyDataText = "No existen facturas registradas aun";
                }
                else
                {
                    dgvDatos.EmptyDataText = "No existen facturas que coincidan con la búsqueda";
                }
                dgvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
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

        /// <summary>
        /// Validar que el folio no exista en el sistema
        /// </summary>
        /// <returns>False si existe</returns>
        private bool ValidaFolioRepetido()
        {
            oblFacturas = new MedNeg.Facturas.BlFacturas();

            if (!oblFacturas.ValidarFolioRepetido(txbFolio.Text))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Buscar
        /// </summary>
        /// <param name="sCadena"></param>
        protected void Buscar(string sCadena)
        {
            lblAviso.Text = "";
            lblAviso2.Text = "";
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
                DateTime dCadena;
                try
                {
                    dCadena = Convert.ToDateTime(sCadena);
                }
                catch
                {
                    dCadena = DateTime.Now;
                    txbBuscar.Text = dCadena.ToShortDateString();
                }
                sCadena = dCadena.Year.ToString() + "-" + dCadena.Month.ToString() + "-" + dCadena.Day.ToString();
            }



            oblFacturas = new MedNeg.Facturas.BlFacturas();
            var oQuery = oblFacturas.Buscar(sCadena, iTipo);
            Session["resultadoquery"] = oQuery;

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Facturas.CuentasxCobrarView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Nombre ASC";

            try
            {
                dgvDatos.DataSource = dv;
                dgvDatos.DataKeyNames = new string[] { "idFactura" };
                dgvDatos.DataBind();

                CargarCatalogo();
                if (dgvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    dgvDatos.EmptyDataText = "No existen remisiones registradas aun";
                }
                else
                {
                    dgvDatos.EmptyDataText = "No existen remisiones que coincidan con la búsqueda";
                }
                dgvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        /// <summary>
        /// Registrar nuevo pedido
        /// </summary>
        private void Nuevo()
        {

            string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            string sRutaCertificados = Server.MapPath("~/Archivos/");

            oFactura = new MedDAL.DAL.facturas();
            oblFacturas = new MedNeg.Facturas.BlFacturas();
            oFactura.idCliente = (int)Session["sIdCliente"];

            if ((bool)Session["sEsDePedido"] == true)
                oFactura.idPedido = (int)Session["sIdPedido"];

            if ((bool)Session["sEsDeRemision"] == true)
                oFactura.idRemision = (int)Session["sIdRemision"];

            if ((bool)Session["sEsDeReceta"] == true)
                oFactura.idReceta = (int)Session["sIdReceta"];

            oFactura.TipoFactura = cmbTipoFactura.SelectedValue.ToString();
            oFactura.Fecha = DateTime.Now;
            oFactura.Estatus = cmbEstatus.SelectedValue;

            //Validar Folio Repetido
            if (ValidaFolioRepetido())
            {

                //Validar si se esta respetando el folio automatico y verificar si aun es el mismo o cambio su valor
                if (Session["iFolioAutomatico"].Equals(txbFolio.Text))
                {
                    oFactura.Folio = oblFacturas.RecuperaFolioAutomatico(sRutaArchivoConfig).ToString();
                }
                else
                {
                    oFactura.Folio = txbFolio.Text;
                }

                if (oblFacturas.NuevoRegistro(oFactura))
                {
                    //Datos de la bitacora
                    string sDatosBitacora = string.Empty;
                    sDatosBitacora += "Tipo:" + cmbTipoFactura.SelectedValue.ToString() + " ";
                    sDatosBitacora += "Folio:" + txbFolio.Text + " ";
                    sDatosBitacora += "Fecha:" + txbFecha.Text + " ";
                    sDatosBitacora += "Estatus:" + cmbEstatus.SelectedItem.ToString() + " ";
                    sDatosBitacora += "Cliente:" + txbCliente.Text + " ";


                    // Registrar la partida de la remision
                    oFactura = new MedDAL.DAL.facturas();
                    oFactura = oblFacturas.BuscarFacturasFolio(txbFolio.Text);
                    int iIdFactura = oFactura.idFactura;
                    bool bRegistroFallido = false;

                    //Recorrer el objeto de sesion lstDetallePartida que contiene los datos de la partida
                    foreach (MedNeg.Facturas.BlDetallePartida facturaDetalle in (List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])
                    {
                        oblFacturas = new MedNeg.Facturas.BlFacturas();
                        MedDAL.DAL.facturas_partida oFacturaPartida = new MedDAL.DAL.facturas_partida();

                        oFacturaPartida.idFactura = iIdFactura;
                        oFacturaPartida.idProducto = facturaDetalle.iIdProducto;
                        oFacturaPartida.Cantidad = facturaDetalle.dCantidad;
                        oFacturaPartida.IEPS = facturaDetalle.dIeps;
                        oFacturaPartida.Iva = facturaDetalle.dIva;
                        oFacturaPartida.Precio = facturaDetalle.dPrecio;
                        oFacturaPartida.Descripcion = facturaDetalle.SProducto;

                        //Registrar el detalle del pedido
                        if (!oblFacturas.NuevoDetallePartida(oFacturaPartida))
                        {
                            bRegistroFallido = true;
                        }
                        else
                        {
                            sDatosBitacora += "Producto:" + facturaDetalle.iIdProducto.ToString() + " ";
                            sDatosBitacora += "Cant:" + facturaDetalle.dCantidad.ToString() + " ";
                            sDatosBitacora += "IEPS:" + facturaDetalle.dIeps.ToString() + " ";
                            sDatosBitacora += "Iva:" + facturaDetalle.dIva.ToString() + " ";
                            sDatosBitacora += "Precio:" + facturaDetalle.dPrecio.ToString() + " ";
                            sDatosBitacora += "Total:" + Convert.ToDecimal((facturaDetalle.dCantidad * facturaDetalle.dPrecio) + facturaDetalle.dIeps + facturaDetalle.dIva) + ", ";
                        }
                    }

                    //Registrar datos de la remision en la bitacora
                    //lblAviso.Text = "El usuario se ha registrado con éxito";
                    oBitacora = new MedDAL.DAL.bitacora();
                    oblBitacora = new MedNeg.Bitacora.BlBitacora();
                    oBitacora.FechaEntradaSrv = DateTime.Now;
                    oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                    oBitacora.Modulo = "Factura";
                    oBitacora.Usuario = Session["usuario"].ToString();
                    oBitacora.Nombre = Session["nombre"].ToString();
                    oBitacora.Accion = "Nueva Factura";
                    oBitacora.Descripcion = sDatosBitacora;
                    if (!oblBitacora.NuevoRegistro(oBitacora))
                    {
                        lblDatos.Text = "El evento no pudo ser registrado en la bitácora";
                    }

                    //Actualizar el consecutivo en la bitacora
                    oblFacturas.ActualizarFolioFactura(sRutaArchivoConfig);

                    //Generar la factura electronica
                    if (cmbModoFactura.SelectedValue == "2")
                    {
                        oblFacturas = new MedNeg.Facturas.BlFacturas();
                        oblFacturas.GenerarFacturaElectronica(iIdFactura, sRutaCertificados, Session["usuario"].ToString(), (int)Session["sIdCliente"], txbFolio.Text);

                        System.IO.FileInfo fFactura = new System.IO.FileInfo(Server.MapPath("~/Archivos/FacturasElectronicas/FacturaE-" + txbFolio.Text + ".xml"));

                        Response.Clear();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + fFactura.Name);
                        Response.AddHeader("Content-Length", fFactura.Length.ToString());
                        Response.ContentType = "application/....";
                        Response.WriteFile(fFactura.FullName);
                        Response.End();
                    }

                    //Saber si se va a actualizar el estatus del pedido
                    if ((bool)Session["sEsDePedido"] == true)
                    {
                        //Actualizar el estatus del pedido en caso de que se haya hecho la remision a partir de un pedido
                        MedDAL.DAL.pedidos oPedido = new MedDAL.DAL.pedidos();
                        MedNeg.Pedidos.BlPedidos oblPedido = new MedNeg.Pedidos.BlPedidos();

                        //Actualizar el estatus del pedido
                        oPedido = oblPedido.BuscarPedido((int)Session["sIdPedido"]);
                        oPedido.Estatus = "3";

                        if (!oblPedido.EditarRegistro(oPedido))
                        {
                            lblDatos.Text = "No se pudo cambiar el estatus del pedido, contacte al administrador";
                        }
                    }

                    //Saber si se va a actualizar el estatus de la remision
                    if ((bool)Session["sEsDeRemision"] == true)
                    {
                        //Actualizar el estatus del pedido
                        MedDAL.DAL.remisiones oRemision = new MedDAL.DAL.remisiones();
                        MedNeg.Remisiones.BlRemisiones oblRemision = new MedNeg.Remisiones.BlRemisiones();

                        oRemision = oblRemision.BuscarRemision((int)Session["sIdRemision"]);
                        oRemision.Estatus = "3";

                        if (!oblRemision.EditarRegistro(oRemision))
                        {
                            lblDatos.Text = "No se pudo cambiar el estatus de la remisión, contacte al administrador";
                        }
                    }

                    //Saber si se va a actualizar el estatus de la remision
                    if ((bool)Session["sEsDeReceta"] == true)
                    {
                        //Actualizar el estatus del pedido
                        MedDAL.DAL.remisiones oRemision = new MedDAL.DAL.remisiones();
                        MedNeg.Remisiones.BlRemisiones oblRemision = new MedNeg.Remisiones.BlRemisiones();

                        MedDAL.DAL.recetas oReceta = new MedDAL.DAL.recetas();
                        MedNeg.Recetas.BlRecetas oblRecetas = new MedNeg.Recetas.BlRecetas();

                        oReceta = oblRecetas.BuscarReceta((int)Session["sIdReceta"]);
                        oReceta.Estatus = "2";

                        if (!oblRecetas.EditarRegistro(oReceta))
                        {
                            lblDatos.Text = "No se pudo cambiar el estatus de la receta, contacte al administrador";
                        }
                    }

                }
                else
                {

                }

            }
            else  //si es folio repetido
            {
                lblDatos.Text = "Folio Repetido, no se puede generar el pedido";

            }
        }

        /// <summary>
        /// Eliminar un elemento de la lista lsdDetallePartida (el detalle de la partida)
        /// </summary>
        /// <param name="iIndixe"></param>
        private void EliminarDetalle(int iIndixe)
        {
            ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).RemoveAt(iIndixe);

            Session["sTotalFactura"] = 0;
            foreach (MedNeg.Facturas.BlDetallePartida elemento in (List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])
            {

                Session["sTotalFactura"] = Convert.ToDecimal(Session["sTotalFactura"]) + elemento.dTotal;
            }

            if (((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Count() > 0)
            {
                lblTotal.Text = "TOTAL:$" + Session["sTotalFactura"].ToString();
            }
            else
            {
                Session["sTotalFactura"] = 0;
                lblTotal.Text = "TOTAL:$" + Session["sTotalFactura"].ToString();
            }


            dgvPartidaDetalle.DataBind();
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

            txbClave.Text = oProducto.Clave1.ToString();
            txbProducto.Text = oProducto.Nombre;
            txbIeps.Text = oProducto.TasaIeps.ToString();
            //txbImp1.Text = oProducto.TasaImpuesto1.ToString();
            //txbImp2.Text = oProducto.TasaImpuesto2.ToString();
            txbIva.Text = oProducto.tipo_iva.Iva.ToString();
            cmbPrecios.Items.Clear();
            cmbPrecios.Items.Add(oProducto.PrecioPublico.ToString());
            cmbPrecios.Items.Add(oProducto.Precio1.ToString());
            cmbPrecios.Items.Add(oProducto.Precio2.ToString());
            cmbPrecios.Items.Add(oProducto.Precio3.ToString());
            cmbPrecios.Items.Add(oProducto.PrecioMinimo.ToString());
            txbCant.Text = "1";
        }

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
                //txbImp1.Text = oProducto.TasaImpuesto1.ToString();
                //txbImp2.Text = oProducto.TasaImpuesto2.ToString();
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
        ///Cargar los datos del cliente
        /// </summary>
        /// <param name="sNombre"></param>
        private void CargaDatosCliente(string sNombre)
        {
            MedDAL.DAL.clientes oCliente = new MedDAL.DAL.clientes();
            MedNeg.BlClientes.BlClientes oblCliente = new MedNeg.BlClientes.BlClientes();

            oCliente = oblCliente.BuscarClienteNombre(sNombre);

            try
            {
                txbCliente.Text = oCliente.Nombre + " " + oCliente.Apellidos;
                txbDireccion.Text = oCliente.Calle.ToString() + " " + oCliente.NumeroExt.ToString();
                if (oCliente.NumeroInt != null)
                {
                    txbDireccion.Text += " Int: " + oCliente.NumeroInt.ToString();
                }

                txbPoblacion.Text = oCliente.poblaciones.Nombre.ToString() + ", " + oCliente.municipios.Nombre.ToString() +
                       ", " + oCliente.estados.Nombre.ToString();

                Session["sIdCliente"] = oCliente.idCliente;

            }
            catch
            {
                txbCliente.Focus();
            }
        }

        private void HabilitaRemision()
        {
            txbCliente.Enabled = false;
            txbClave.Enabled = true;
            txbProducto.Enabled = true;
            txbCant.Enabled = true;
            txbIeps.Enabled = true;
            txbIva.Enabled = true;
            cmbPrecios.Enabled = true;
            txbObservaciones.Enabled = true;
            imbAgregarDetalle.Enabled = true;
            cmbEstatus.Enabled = true;

        }

        private void DeshabilitaRemision()
        {
            txbCliente.Enabled = false;
            txbClave.Enabled = false;
            txbProducto.Enabled = false;
            txbCant.Enabled = false;
            txbIeps.Enabled = false;
            txbIva.Enabled = false;
            cmbPrecios.Enabled = false;
            txbObservaciones.Enabled = false;
            imbAgregarDetalle.Enabled = false;
            cmbEstatus.Enabled = false;
            dgvPartidaDetalle.Enabled = false;
        }

        /// <summary>
        /// Cargar los datos del pedido y su partida
        /// </summary>
        /// <param name="bDatos"></param>
        protected void CargarFormulario(bool bDatos)
        {
            pnlFormulario.Visible = true;
            pnlCatalogo.Visible = false;

            if (bDatos)
            {

                //Objeto que contiene el id del pedido 
                oblFacturas = new MedNeg.Facturas.BlFacturas();
                oFactura = new MedDAL.DAL.facturas();
                oFactura = (MedDAL.DAL.facturas)oblFacturas.BuscarFactura(int.Parse(dgvDatos.SelectedDataKey[0].ToString()));

                //Llenar los campos del pedido
                txbFolio.Text = oFactura.Folio;
                txbFecha.Text = oFactura.Fecha.ToShortDateString();
                int iContador = 0;
                cmbEstatus.SelectedIndex = -1;
                foreach (ListItem elemento in cmbEstatus.Items)
                {
                    if (elemento.Value.Equals(oFactura.Estatus.ToString()))
                    {
                        elemento.Selected = true;
                    }
                    iContador++;
                }

                cmbTipoFactura.SelectedIndex = -1;
                iContador = 0;
                foreach (ListItem elemento in cmbTipoFactura.Items)
                {
                    if (elemento.Value.Equals(oFactura.TipoFactura.ToString()))
                    {
                        elemento.Selected = true;
                    }
                    iContador++;
                }

                //Llenar los campos del cliente
                txbCliente.Text = oFactura.clientes.Nombre + " " + oFactura.clientes.Apellidos;
                txbDireccion.Text = oFactura.clientes.Calle + " " + oFactura.clientes.NumeroExt;
                if (oFactura.clientes.NumeroInt != null)
                {
                    txbDireccion.Text += "Int: " + oFactura.clientes.NumeroInt;
                }

                txbPoblacion.Text = oFactura.clientes.poblaciones.Nombre.ToString() + ", " + oFactura.clientes.municipios.Nombre.ToString() + ", " + oFactura.clientes.estados.Nombre.ToString();

                //Lenar los datos de la partida del detalle
                oblFacturas = new MedNeg.Facturas.BlFacturas();

                //Recuperar la partida del pedido
                var oQuery = oblFacturas.RecuperarPartidaFactura(oFactura.idFactura);

                //Session["lstDetallePartida"] = new List<MedNeg.Facturas.BlDetallePartida>();
                if (((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Count() > 0)
                {
                    ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Clear();
                }

                //Recorrer el resultado y meterlo al datagridview
                Session["sTotalFactura"] = 0;
                foreach (MedDAL.DAL.facturas_partida oDetalle in oQuery)
                {
                    oblDetallePartida = new MedNeg.Facturas.BlDetallePartida(
                        Convert.ToInt32(oDetalle.idProducto),
                        oDetalle.productos.Clave1,
                        oDetalle.productos.Nombre,
                        oDetalle.Cantidad,
                        Convert.ToDecimal(oDetalle.IEPS),
                        Convert.ToDecimal(oDetalle.Iva),
                        Convert.ToDecimal(oDetalle.Precio),
                        oDetalle.Observaciones,
                        Convert.ToDecimal((oDetalle.Precio * oDetalle.Cantidad) + oDetalle.IEPS + oDetalle.Iva));

                    ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Add(oblDetallePartida);
                    Session["sTotalFactura"] = Convert.ToDecimal(Session["sTotalFactura"]) + Convert.ToDecimal((oDetalle.Precio * oDetalle.Cantidad) + oDetalle.IEPS + oDetalle.Iva);
                }

                //Hacer el binding de la data al dgvDatos
                lblTotal.Text = "TOTAL:$" + Session["sTotalFactura"].ToString();
                dgvPartidaDetalle.DataBind();

                ////si el estatus es 1 (Pedido) aun se pueden agregar articulos de lo contario ya no
                //if (oFactura.Estatus == "3")
                //{
                //    HabilitaRemision();
                //    Deshabilita();
                //}
                //else
                //{
                    DeshabilitaRemision();
                    Deshabilita();
                    //cmbEstatus.Enabled = true;
                //}


            }
            else
            {

                //Limpia();
                //Deshabilita();

            }
        }


        private void Deshabilita()
        {
            txbFolio.Enabled = false;
            txbFecha.Enabled = false;
            cmbModoFactura.Enabled = false;
            cmbTipoFactura.Enabled = false;

        }

        /// <summary>
        /// Editar
        /// </summary>
        private void Editar()
        {
            oFactura = new MedDAL.DAL.facturas();
            oblFacturas = new MedNeg.Facturas.BlFacturas();
            oFactura.idFactura = int.Parse(dgvDatos.SelectedDataKey.Value.ToString());
            oFactura.Estatus = cmbEstatus.SelectedValue.ToString();


            if (oblFacturas.EditarRegistro(oFactura))
            {
                //Datos de la bitacora
                string sDatosBitacora = string.Empty;
                sDatosBitacora += "Tipo:" + cmbTipoFactura.SelectedValue.ToString() + " ";
                sDatosBitacora += "Folio:" + txbFolio.Text + " ";
                sDatosBitacora += "Fecha:" + txbFecha.Text + " ";
                sDatosBitacora += "Estatus:" + cmbEstatus.SelectedItem.ToString() + " ";
                sDatosBitacora += "Cliente:" + txbCliente.Text + " ";

                oblFacturas = new MedNeg.Facturas.BlFacturas();
                if (oblFacturas.EliminarFacturaPartida(oFactura.idFactura))
                {
                    bool bRegistroFallido = false;

                    //Recorrer el objeto de sesion lstDetallePartida que contiene los datos de la partida
                    foreach (MedNeg.Facturas.BlDetallePartida facturaDetalle in (List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])
                    {
                        oblFacturas = new MedNeg.Facturas.BlFacturas();
                        MedDAL.DAL.facturas_partida oFacturaPartida = new MedDAL.DAL.facturas_partida();

                        oFacturaPartida.idFactura = oFactura.idFactura;
                        oFacturaPartida.idProducto = facturaDetalle.iIdProducto;
                        oFacturaPartida.Cantidad = facturaDetalle.dCantidad;
                        oFacturaPartida.IEPS = facturaDetalle.dIeps;
                        oFacturaPartida.Iva = facturaDetalle.dIva;
                        oFacturaPartida.Precio = facturaDetalle.dPrecio;

                        //Registrar el detalle del pedido
                        if (!oblFacturas.NuevoDetallePartida(oFacturaPartida))
                        {
                            bRegistroFallido = true;
                        }
                        else
                        {
                            sDatosBitacora += "Producto:" + facturaDetalle.iIdProducto.ToString() + " ";
                            sDatosBitacora += "Cant:" + facturaDetalle.dCantidad.ToString() + " ";
                            sDatosBitacora += "IEPS:" + facturaDetalle.dIeps.ToString() + " ";
                            sDatosBitacora += "Iva:" + facturaDetalle.dIva.ToString() + " ";
                            sDatosBitacora += "Precio:" + facturaDetalle.dPrecio.ToString() + " ";
                            sDatosBitacora += "Total:" + Convert.ToDecimal((facturaDetalle.dCantidad * facturaDetalle.dPrecio) + facturaDetalle.dIeps + facturaDetalle.dIva) + ", ";
                        }
                    }


                    //Anotar en la bitacora la modificación al pedido
                    oBitacora = new MedDAL.DAL.bitacora();
                    oblBitacora = new MedNeg.Bitacora.BlBitacora();
                    oBitacora.FechaEntradaSrv = DateTime.Now;
                    oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                    oBitacora.Modulo = "Facturas";
                    oBitacora.Usuario = Session["usuario"].ToString();
                    oBitacora.Nombre = Session["nombre"].ToString();
                    oBitacora.Accion = "Edición de Factura";
                    oBitacora.Descripcion = sDatosBitacora;
                    if (!oblBitacora.NuevoRegistro(oBitacora))
                    {
                        lblDatos.Text = "El evento no pudo ser registrado en la bitácora";
                    }
                }


            }

        }


        private void Eliminar(int iIdFactura)
        {

            //Eliminar primero la partida para la integridad referencial
            oblFacturas = new MedNeg.Facturas.BlFacturas();
            string sDatosBitacora = string.Empty;

            //Guardar los datos del pedido para la bitacora
            oFactura = new MedDAL.DAL.facturas();
            oFactura = oblFacturas.BuscarFactura(iIdFactura);

            sDatosBitacora += "Folio:" + oFactura.Folio.ToString() + " ";
            sDatosBitacora += "Fecha:" + oFactura.Fecha.ToShortDateString() + " ";
            switch (oFactura.Estatus)
            {
                case "1":
                    sDatosBitacora += "Estatus:Pedido ";
                    break;
                case "2":
                    sDatosBitacora += "Estatus:Remitido ";
                    break;
                case "3":
                    sDatosBitacora += "Estatus:Emitida ";
                    break;
                case "4":
                    sDatosBitacora += "Estatus:Cobrada ";
                    break;
                case "5":
                    sDatosBitacora += "Estatus:Cancelada ";
                    break;
            }

            //Recuperar la partida del pedido
            oblFacturas = new MedNeg.Facturas.BlFacturas();
            var oQuery = oblFacturas.RecuperarPartidaFactura(iIdFactura);
            //Recorrer el resultado y meterlo al datagridview
            foreach (MedDAL.DAL.facturas_partida oDetalle in oQuery)
            {
                sDatosBitacora += "Producto:" + oDetalle.productos.Nombre.ToString() + " ";
                sDatosBitacora += "Cantidad:" + oDetalle.Cantidad.ToString() + " ";
                sDatosBitacora += "IEPS:" + oDetalle.IEPS.ToString() + " ";
                sDatosBitacora += "Iva:" + oDetalle.Iva.ToString() + " ";
                sDatosBitacora += "Precio:" + oDetalle.Precio.ToString() + " ";
                sDatosBitacora += "Total:" + Convert.ToDecimal((oDetalle.Cantidad * oDetalle.Precio) + oDetalle.IEPS + oDetalle.Iva) + ", ";

            }


            if (oblFacturas.EliminarFacturaPartida(iIdFactura))
            {
                oblFacturas = new MedNeg.Facturas.BlFacturas();
                if (oblFacturas.EliminarRegistro(iIdFactura))
                {
                    //lblAviso.Text = "El usuario se ha eliminado con éxito";
                    MedDAL.DAL.bitacora oBitacora = new MedDAL.DAL.bitacora();
                    MedNeg.Bitacora.BlBitacora oblBitacora = new MedNeg.Bitacora.BlBitacora();
                    oBitacora.FechaEntradaSrv = DateTime.Now;
                    oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                    oBitacora.Modulo = "Facturas";
                    oBitacora.Usuario = Session["usuario"].ToString();
                    oBitacora.Nombre = Session["nombre"].ToString();
                    oBitacora.Accion = "Eliminación de Factura";
                    oBitacora.Descripcion = sDatosBitacora;

                    if (!oblBitacora.NuevoRegistro(oBitacora))
                    {
                        lblDatos.Text = "El evento no pudo ser registrado en la bitácora";
                    }
                }
                else
                {
                    //lblAviso.Text = "El usuario no pudo ser eliminado, es posible que tenga datos relacionados";
                }

            }
            else
            {
                //lblAviso.Text = "El usuario no pudo ser eliminado, es posible que tenga datos relacionados";
            }

        }


        protected void imbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedIndex != -1)
            {
                CargarFormulario(true);
                Session["accion"] = 2;
            }
            else
            {
                CargarCatalogo();

            }
        }

        //protected void imbEliminar_Click(object sender, EventArgs e)
        //{
        //    if (pnlCatalogo.Visible && dgvDatos.SelectedIndex != -1)
        //    {
        //        Eliminar((int)dgvDatos.SelectedValue);
        //        MostrarLista();
        //        CargarCatalogo();
        //    }
        //    else
        //    {
        //        CargarCatalogo();
        //        MostrarLista();
        //    }
        //}

        protected void imbMostrar_Click(object sender, EventArgs e)
        {
            pnlFiltroReportes.Visible = false;
            MostrarLista();
        }

        protected void imbCancelar_Click()
        {

        }

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            //GT0175
            //ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
            pnlCatalogo.Visible = false;
            lblAviso.Text = "";
            pnlFiltroReportes.Visible = true;
            CargarListaReportes();

        }

        //protected void btnBuscar_Click() { }

        //protected void imbNuevo_Click(object sender, EventArgs e)
        //{
        //    CargarFormulario(false);
        //    Session["accion"] = 1;
        //    txbFecha.Text = DateTime.Now.ToShortDateString();
        //    //lblAviso.Text = "";
        //    //lblAviso2.Text = "";
        //    //Habilita();
        //    //Limpia();
        //}

        protected void imbAceptar_Click(object sender, EventArgs e)
        {
            int iAccion;
            if (Session["accion"] != null)
            {
                iAccion = (int)Session["accion"];
            }
            else iAccion = 0;
            switch (iAccion)
            {
                case 0:
                    break;
                case 1:
                    Nuevo();
                    Limpia();
                    break;
                case 2:
                    Editar();
                    Deshabilita();
                    Session["accion"] = 0;
                    Limpia();
                    break;
            }

        }

        private void Limpia()
        {
            txbFolio.Text = "";
            txbPedido.Text = "";
            txbRemision.Text = "";
            txbReceta.Text = "";
            txbCliente.Text = "";
            txbDireccion.Text = "";
            txbPoblacion.Text = "";
            LimpiarDatosDetalle();
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;

            Session["sTotalFactura"] = 0;
            //Session["lstDetallePartida"] = new List<MedNeg.Facturas.BlDetallePartida>();
            if (((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Count() > 0)
            {
                ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Clear();
            }
            //pnlReportes.Visible = false;
        }

        /// <summary>
        /// Cargar los datos del pedido del cual se va a generar la remisión
        /// </summary>
        /// <param name="sFolioPedido"></param>
        private void CargaPedido(string sFolioPedido)
        {
            //Objeto que contiene el id del pedido 
            MedNeg.Pedidos.BlPedidos oblPedido = new MedNeg.Pedidos.BlPedidos();
            MedDAL.DAL.pedidos oPedido = new MedDAL.DAL.pedidos();
            oPedido = oblPedido.BuscarPedidoFolio(sFolioPedido);

            //Llenar los campos del pedido
            Session["sIdPedido"] = oPedido.idPedido;
            Session["sIdCliente"] = oPedido.idCliente;
            //txbFolio.Text = oPedido.Folio;
            //txbFecha.Text = oPedido.Fecha.ToShortDateString();


            //Llenar los campos del cliente
            txbCliente.Text = oPedido.clientes.Nombre + " " + oPedido.clientes.Apellidos;
            txbDireccion.Text = oPedido.clientes.Calle + " " + oPedido.clientes.NumeroExt;
            if (oPedido.clientes.NumeroInt != null)
            {
                txbDireccion.Text += "Int: " + oPedido.clientes.NumeroInt;
            }

            txbPoblacion.Text = oPedido.clientes.poblaciones.Nombre.ToString() + ", " + oPedido.clientes.municipios.Nombre.ToString() + ", " + oPedido.clientes.estados.Nombre.ToString();

            //Lenar los datos de la partida del detalle
            oblPedido = new MedNeg.Pedidos.BlPedidos();

            //Recuperar la partida del pedido
            var oQuery = oblPedido.RecuperarPartidaPedido(oPedido.idPedido);

            //Session["lstDetallePartida"] = new List<MedNeg.Facturas.BlDetallePartida>();
            if (((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Count() > 0)
            {
                ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Clear();
            }

            Session["sTotalFactura"] = 0;
            //Recorrer el resultado y meterlo al datagridview
            foreach (MedDAL.DAL.pedidos_partida oDetalle in oQuery)
            {
                oblDetallePartida = new MedNeg.Facturas.BlDetallePartida(
                    Convert.ToInt32(oDetalle.idProducto),
                    oDetalle.productos.Clave1,
                    oDetalle.productos.Nombre,
                    oDetalle.Cantidad,
                    Convert.ToDecimal(oDetalle.IEPS),
                    Convert.ToDecimal(oDetalle.Iva),
                    Convert.ToDecimal(oDetalle.Precio),
                   oDetalle.Observaciones,
                    Convert.ToDecimal((oDetalle.Precio * oDetalle.Cantidad) + oDetalle.IEPS + oDetalle.Iva));

                ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Add(oblDetallePartida);
                Session["sTotalFactura"] = Convert.ToDecimal(Session["sTotalFactura"]) + Convert.ToDecimal((oDetalle.Precio * oDetalle.Cantidad) + oDetalle.IEPS + oDetalle.Iva);
            }

            //Hacer el binding de la data al dgvDatos
            lblTotal.Text = "TOTAL:$" + Session["sTotalFactura"].ToString();
            dgvPartidaDetalle.DataBind();


        }

        /// <summary>
        /// Cargar los detalles de la remisión
        /// </summary>
        private void CargaRemision(string sFolioRemision)
        {
            //Objeto que contiene la remisión
            MedDAL.DAL.remisiones oRemision = new MedDAL.DAL.remisiones();
            MedNeg.Remisiones.BlRemisiones oblRemision = new MedNeg.Remisiones.BlRemisiones();

            oRemision = oblRemision.BuscarRemisionFolio(sFolioRemision);

            //Llenar los campos del pedido
            Session["sIdPedido"] = oRemision.idPedido;
            Session["sIdCliente"] = oRemision.idCliente;
            Session["sIdRemision"] = oRemision.idRemision;

            //Llenar los campos del cliente
            txbCliente.Text = oRemision.clientes.Nombre + " " + oRemision.clientes.Apellidos;
            txbDireccion.Text = oRemision.clientes.Calle + " " + oRemision.clientes.NumeroExt;
            if (oRemision.clientes.NumeroInt != null)
            {
                txbDireccion.Text += "Int: " + oRemision.clientes.NumeroInt;
            }

            txbPoblacion.Text = oRemision.clientes.poblaciones.Nombre.ToString() + ", " + oRemision.clientes.municipios.Nombre.ToString() + ", " + oRemision.clientes.estados.Nombre.ToString();

            //Llenar los datos de la partida de la remisión
            oblRemision = new MedNeg.Remisiones.BlRemisiones();

            //Recuperar la partida del pedido
            var oQuery = oblRemision.RecuperarPartidaRemision(oRemision.idRemision);
            //Session["lstDetallePartida"] = new List<MedNeg.Facturas.BlDetallePartida>();
            if (((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Count() > 0)
            {
                ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Clear();
            }

            Session["sTotalFactura"] = 0;
            //Recorrer el resultado y meterlo al datagridview
            foreach (MedDAL.DAL.remisiones_partida oDetalle in oQuery)
            {
                oblDetallePartida = new MedNeg.Facturas.BlDetallePartida(
                    Convert.ToInt32(oDetalle.idProducto),
                    oDetalle.productos.Clave1,
                    oDetalle.productos.Nombre,
                    oDetalle.Cantidad,
                    Convert.ToDecimal(oDetalle.IEPS),
                    Convert.ToDecimal(oDetalle.Iva),
                    Convert.ToDecimal(oDetalle.Precio),
                   oDetalle.Observaciones,
                    Convert.ToDecimal((oDetalle.Precio * oDetalle.Cantidad) + oDetalle.IEPS + oDetalle.Iva));

                ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Add(oblDetallePartida);

                Session["sTotalFactura"] = Convert.ToDecimal(Session["sTotalFactura"]) + Convert.ToDecimal((oDetalle.Precio * oDetalle.Cantidad) + oDetalle.IEPS + oDetalle.Iva);

            }

            //Hacer el binding de la data al dgvDatos
            lblTotal.Text = "TOTAL:$" + Session["sTotalFactura"].ToString();
            dgvPartidaDetalle.DataBind();
        }

        /// <summary>
        /// Carga los detalles de la receta
        /// </summary>
        private void CargaReceta(string sFolio)
        {
            //objeto que contiene la receta
            MedDAL.DAL.recetas oReceta = new MedDAL.DAL.recetas();
            MedNeg.Recetas.BlRecetas oblReceta = new MedNeg.Recetas.BlRecetas();

            oReceta = oblReceta.BuscarRecetaFolio(sFolio);

            //Recuperar el id de la receta y del usuario
            Session["sIdReceta"] = oReceta.idReceta;


            //Llenar los campos del cliente
            try
            {

                txbCliente.Text = oReceta.clientes.Nombre + " " + oReceta.clientes.Apellidos;
                txbDireccion.Text = oReceta.clientes.Calle + " " + oReceta.clientes.NumeroExt;
                if (oReceta.clientes.NumeroInt != null)
                {
                    txbDireccion.Text += "Int: " + oReceta.clientes.NumeroInt;
                }

                txbPoblacion.Text = oReceta.clientes.poblaciones.Nombre.ToString() + ", " + oReceta.clientes.municipios.Nombre.ToString() + ", " + oReceta.clientes.estados.Nombre.ToString();
                Session["sIdCliente"] = oReceta.idCliente;
            }
            catch
            {
                //En caso de que no tenga un cliente registrado
                txbCliente.Text = "";
                txbDireccion.Text = "";
                txbPoblacion.Text = "";
            }

            //Llenar los datos de la partida de la remisión
            oblReceta = new MedNeg.Recetas.BlRecetas();

            //Recuperar la partida del pedido
            var oQuery = oblReceta.RecuperarPartidaRecetas(oReceta.idReceta);
            if (((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Count() > 0)
            {
                ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Clear();
            }

            Session["sTotalFactura"] = 0;
            //Recorrer el resultado y meterlo al datagridview
            foreach (MedDAL.DAL.recetas_partida oDetalle in oQuery)
            {
                oblDetallePartida = new MedNeg.Facturas.BlDetallePartida(
                    Convert.ToInt32(oDetalle.idProducto),
                    oDetalle.productos.Clave1,
                    oDetalle.productos.Nombre,
                    Convert.ToDecimal(oDetalle.CantidaSurtida),
                    Convert.ToDecimal(oDetalle.productos.TasaIeps),
                    Convert.ToDecimal(oDetalle.productos.tipo_iva.Iva),
                    Convert.ToDecimal(oDetalle.Precio),
                   "",
                  Convert.ToDecimal((oDetalle.Precio * oDetalle.CantidaSurtida) + oDetalle.productos.tipo_iva.Iva));

                ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Add(oblDetallePartida);
                Session["sTotalFactura"] = Convert.ToDecimal(Session["sTotalFactura"]) + Convert.ToDecimal((oDetalle.Precio * oDetalle.CantidaSurtida) + oDetalle.productos.tipo_iva.Iva);
            }

            //Hacer el binding de la data al dgvDatos
            lblTotal.Text = "TOTAL:$" + Session["sTotalFactura"].ToString();
            dgvPartidaDetalle.DataBind();

        }

        protected void imbAgregarDetalle_Click(object sender, ImageClickEventArgs e)
        {
            //Evitar mandar valores nullos para las conversiones
            //int iCantidad;
            decimal dCantidad, dIeps, dImp1 = 0, dImp2 = 0, dIva, dPrecio, dTotal;

            if (txbCant.Text.Equals(""))
                dCantidad = 0;
            else
                dCantidad = int.Parse(txbCant.Text);

            //Calcular el total
            dPrecio = decimal.Parse(cmbPrecios.SelectedValue);
            dTotal = dCantidad * dPrecio;

            //IEPS
            if (txbIeps.Text.Equals(""))
                dIeps = 0;
            else
                dIeps = dTotal * (decimal.Parse(txbIeps.Text) / 100);

            ////Imp1
            //if (txbImp1.Text.Equals(""))
            //    dImp1 = 0;
            //else
            //    dImp1 = dTotal * (decimal.Parse(txbImp1.Text) / 100);

            /////Imp2
            //if (txbImp2.Text.Equals(""))
            //    dImp2 = 0;
            //else
            //    dImp2 = dTotal * (decimal.Parse(txbImp2.Text) / 100);

            ///Iva
            if (txbIva.Text.Equals(""))
                dIva = 0;
            else
                dIva = dTotal * (decimal.Parse(txbIva.Text) / 100);


            //Agregar los impuestos al total
            dTotal += dIeps + dImp1 + dImp2 + dIva;

            //Recuperar el id del producto para ya tenerlo en la lista
            MedNeg.Productos.BlProductos oblProducto = new MedNeg.Productos.BlProductos();
            int iIdProducto = oblProducto.RecuperarIdProducto(txbClave.Text);


            //Crear el objeto ya con sus parametros
            oblDetallePartida = new MedNeg.Facturas.BlDetallePartida(
                iIdProducto,
                txbClave.Text,
                txbProducto.Text,
                dCantidad,
                dIeps,
                //dImp1,
                //dImp2,
                dIva,
                dPrecio,
                txbObservaciones.Text,
                dTotal);

            //Agregar el objeto detalle partida al objeto lstDetallePartida
            ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Add(oblDetallePartida);
            dgvPartidaDetalle.DataBind();
            Session["sTotalFactura"] = Convert.ToDecimal(Session["sTotalFactura"]) + dTotal;
            lblTotal.Text = "TOTAL:$" + Session["sTotalFactura"].ToString();
            LimpiarDatosDetalle();
            txbClave.Focus();
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
            txbObservaciones.Text = "";
        }

        /// <summary>
        /// Cliente TextChnged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txbCliente_TextChanged(object sender, EventArgs e)
        {
            CargaDatosCliente(txbCliente.Text);

        }

        /// <summary>
        /// Selected index para eliminar un detalle de la partida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgvPartidaDetalle_SelectedIndexChanged(object sender, EventArgs e)
        {
            EliminarDetalle(dgvPartidaDetalle.SelectedIndex);
        }


        /// <summary>
        /// Cambiar el IdEstatus por el valor correspondiente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgvDatos_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (dgvDatos.Rows.Count > 0)
            {
                if (dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[7].Text == "1")
                    dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[7].Text = "Pedido";
                if (dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[7].Text == "2")
                    dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[7].Text = "Emitida";
                if (dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[7].Text == "3")
                    dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[7].Text = "Emitida";
                if (dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[7].Text == "4")
                    dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[7].Text = "Cobrada";
                if (dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[7].Text == "5")
                    dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[7].Text = "Cancelada";
            }
        }


        /// <summary>
        /// Selected index change del gridview de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
           ((DropDownList)dgvDatos.SelectedRow.Cells[8].FindControl("cmbEstatusCobranza")).Enabled = true;
        }

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


        /// <summary>
        /// Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar(txbBuscar.Text);
        }

        /// <summary>
        /// Para el cuando seleccionen de donde quieren generar la factura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbTipoFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Pedido
            if (cmbTipoFactura.SelectedValue == "2")
            {
                //lblPedido.Enabled = true;
                txbPedido.Enabled = true;
                //lblRemision.Visible = false;
                txbRemision.Enabled = false;
                //lblReceta.Visible = false;
                txbReceta.Enabled = false;
                txbPedido.Text = "";
                txbRemision.Text = "";
                txbReceta.Text = "";
                txbPedido.Focus();
                Session["sEsDePedido"] = true;
                Session["sEsDeRemision"] = false;
                Session["sEsDeReceta"] = false;
            }

            //Remision
            if (cmbTipoFactura.SelectedValue == "3")
            {
                //lblPedido.Visible = false;
                txbPedido.Enabled = false;
                //lblRemision.Visible = true;
                txbRemision.Enabled = true;
                //lblReceta.Visible = false;
                txbReceta.Enabled = false;
                txbPedido.Text = "";
                txbRemision.Text = "";
                txbReceta.Text = "";
                txbRemision.Focus();
                Session["sEsDePedido"] = false;
                Session["sEsDeRemision"] = true;
                Session["sEsDeReceta"] = false;
            }

            //Recetas
            if (cmbTipoFactura.SelectedValue == "4")
            {
                //lblPedido.Visible = false;
                txbPedido.Enabled = false;
                //lblRemision.Visible = false;
                txbRemision.Enabled = false;
                //lblReceta.Visible = true;
                txbReceta.Enabled = true;
                txbPedido.Text = "";
                txbRemision.Text = "";
                txbReceta.Text = "";
                txbReceta.Focus();
                Session["sEsDePedido"] = false;
                Session["sEsDeRemision"] = false;
                Session["sEsDeReceta"] = true;
            }
        }

        /// <summary>
        /// CArga pedido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txbPedido_TextChanged(object sender, EventArgs e)
        {

            CargaPedido(txbPedido.Text);
        }

        /// <summary>
        /// Carga Remision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txbRemision_TextChanged(object sender, EventArgs e)
        {
            CargaRemision(txbRemision.Text);
        }

        /// <summary>
        /// Carga recetas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txbReceta_TextChanged(object sender, EventArgs e)
        {
            CargaReceta(txbReceta.Text);
        }

      

        protected void cmbEstatusCobranza_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Recuperar el id de la factura seleccionada y mandar el nuevo estatus
            MedDAL.DAL.facturas oFacturas = new MedDAL.DAL.facturas();
            MedNeg.Facturas.BlFacturas oblFacturas = new MedNeg.Facturas.BlFacturas();


            oFacturas = oblFacturas.BuscarFacturasFolio(dgvDatos.SelectedRow.Cells[4].Text);
            oFacturas.Estatus = ((DropDownList)dgvDatos.SelectedRow.Cells[8].FindControl("cmbEstatusCobranza")).SelectedValue;
            //Actualizar la fecha de aplicación
            oFacturas.FechaAplicacion = DateTime.Now;

            if (oblFacturas.EditarRegistro(oFacturas))
            {
                lblAviso.Text = "La aplicación de la factura:" + dgvDatos.SelectedRow.Cells[4].Text + " fue correcta.";
                
                //Datos de la bitacora
                string sDatosBitacora = string.Empty;
                sDatosBitacora += "Folio:" + oFacturas.Folio + " ";
                sDatosBitacora += "Estatus Cambiado A: " + ((DropDownList)dgvDatos.SelectedRow.Cells[8].FindControl("cmbEstatusCobranza")).SelectedItem.ToString() + " ";
                sDatosBitacora += "Cliente:" + oFacturas.clientes.Nombre + " "+ oFacturas.clientes.Apellidos+" ";
                sDatosBitacora += "RFC:" + oFacturas.clientes.Rfc;

                //Registrar en la bitacora
                RegistrarEnBitacora(sDatosBitacora);
                MostrarLista();
               
            }
            else
            {
                lblAviso.Text = "La aplicación de la factura:" + dgvDatos.SelectedRow.Cells[4].Text + " fue incorrecta, por favor intentelo de nuevo.";
                MostrarLista();
                
            }
        }

        protected void RegistrarEnBitacora(string sDatosBitacora)
        {
            oBitacora = new MedDAL.DAL.bitacora();
            oblBitacora = new MedNeg.Bitacora.BlBitacora();

            oBitacora.FechaEntradaSrv = DateTime.Now;
            oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
            oBitacora.Modulo = "Cuentas por Cobrar";
            oBitacora.Usuario = Session["usuario"].ToString();
            oBitacora.Nombre = Session["nombre"].ToString();
            oBitacora.Accion = "Aplicación de la factura";
            oBitacora.Descripcion = sDatosBitacora;
            
            if (!oblBitacora.NuevoRegistro(oBitacora))
            {
                lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
            }

        }

        #region Reportes

        public void CargarListaReportes()
        {
            ListBox lsbReportes = (ListBox)frReportes.FindControl("lsbSeleccionf");
            frReportes.LimpiarPaneles();
            lsbReportes.SelectedIndex = -1;
            lsbReportes.Items.Clear();
            if (Server.MapPath("~\\rptReportes\\Facturacion\\rptCuentasPorCobrar.rpt") != "")
            {
                lsbReportes.Items.Add("Cuentas por cobrar");
            }
            

        }

        #endregion

        protected void dgvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Facturas.CuentasxCobrarView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvDatos.DataSource = oMaster.Sorting(e, ref oDireccion, dv);
            ViewState["direccionsorting"] = oDireccion;
            ViewState["sortexpression"] = e.SortExpression;
            dgvDatos.DataBind();
        }

        protected void dgvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Facturas.CuentasxCobrarView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvDatos.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Nombre" : ViewState["sortexpression"].ToString(), dv, ref dgvDatos, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            dgvDatos.DataBind();
        }


    }
}