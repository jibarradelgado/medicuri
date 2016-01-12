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
    public partial class Remisiones : System.Web.UI.Page
    {
        ImageButton imbNuevo, imbEditar, imbEliminar, imbImprimir, imbMostrar, imbAceptar, imbCancelar, imbReportes;
        RadioButton rdbNombre, rdbClave, rdbTodos;
        Label lblReportes, lblNombreModulo;
        Button btnBuscar;
        TextBox txbBuscar;
        MedNeg.Facturas.BlDetallePartida oblDetallePartida;
               
        MedDAL.DAL.remisiones oRemision;
        MedNeg.Remisiones.BlRemisiones oblRemision;
        //Declaración del objeto de la capa de negocio de la bitacora
        MedNeg.Bitacora.BlBitacora oblBitacora;

        //Declaración del objeto de la capa de Datos de bitacora
        MedDAL.DAL.bitacora oBitacora;
        string sDatosBitacora = string.Empty;

        /// <summary>
        /// PAGE LOAD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';
            try
            {
                #region Interfaz
                cPermiso = (char)htbPermisos["remisiones"];
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
                imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
                imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
                imbImprimir = (ImageButton)Master.FindControl("imgBtnImprimir");
                imbImprimir.Click += new ImageClickEventHandler(this.imbImprimir_Click);

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
                lblNombreModulo.Text = "Remisiones";

                //GT 0175
                imbReportes = (ImageButton)Master.FindControl("imgBtnReportes");
                imbReportes.Click += new ImageClickEventHandler(this.imbReportes_Click);

                lblDatos.Text = "";                
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

                if (!IsPostBack)
                {
                    Session["editarpartidas"] = 0;

                    Session["reporteactivo"] = 0;
                    Session["reportdocument"] = "";
                    Session["titulo"] = "";

                    //GT: Lista donde se guardan los productos que se agregan en una edicion de remisiones a los ya existentes
                    Session["lstremisionespartidaedicion"] = new List<MedNeg.Facturas.BlDetallePartida>();
                    Session["accion"] = 0;


                }

                if (int.Parse(Session["editarpartidas"].ToString()) == 1)
                {
                    Session["editarpartidas"] = 0;
                }
                else
                {
                    dgvPartidaDetalle.Visible = true;
                    dgvPartidaDetalle.ShowHeader = true;
                    dgvPartidaDetalle.EmptyDataText = "Sin Detalle";
                    dgvPartidaDetalle.DataSource = ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]);
                    dgvPartidaDetalle.DataBind();
                    txbClave.TextChanged += new EventHandler(txbClave_TextChanged);
                }

                if (!IsPostBack)
                {

                    // Saber si estan activados los folios automaticos y poner su valor por default
                    // Se almacena en una variable de sesión para comparar que se esta respetando el formato automatico
                    // y validar que no haya cambiado el folio de pedidos debido a otro registro mientras se hacia el actual 
                    oblRemision = new MedNeg.Remisiones.BlRemisiones();
                    Session["iFolioAutomatico"] = oblRemision.RecuperaFolioAutomatico(Server.MapPath("~/Archivos/Configuracion.xml"));
                    txbFolio.Text = Session["iFolioAutomatico"].ToString();

                    //Session["lstDetallePartida"] = null;
                    Session["lstDetallePartida"] = new List<MedNeg.Facturas.BlDetallePartida>();
                    Session["sEsDePedido"] = false;
                    pnlFormulario.Visible = false;
                    pnlCatalogo.Visible = false;
                    //pnlReportes.Visible = false;
                    Session["sTotalFactura"] = 0;
                    Session["resultadoquery"] = "";
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;

                    //Variable de sesion para saber si es un ensamble al momento de registrar el detalle de la partida
                    // 0 = False, 1 = True
                    Session["sBolEsEnsamble"] = 0;

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

        protected void CargarCatalogo()
        {
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = true;
           
        }

        /// <summary>
        /// Mostrar la lista de todos los pedidos
        /// </summary>
        protected void MostrarLista()
        {
            oblRemision = new MedNeg.Remisiones.BlRemisiones();
            var oQuery = oblRemision.MostrarLista();
            Session["resultadoquery"] = oQuery;                
            
            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Remisiones.RemisionesView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Nombre ASC";

            try
            {
                dgvDatos.DataSource = dv;
                dgvDatos.DataKeyNames = new string[] { "idRemision" };
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
            oblRemision = new MedNeg.Remisiones.BlRemisiones();

            if (!oblRemision.ValidarFolioRepetido(txbFolio.Text))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Registrar nuevo pedido
        /// </summary>
        private void Nuevo()
        {

            string sRutaArchivoConfig=Server.MapPath("~/Archivos/Configuracion.xml");

            oRemision = new MedDAL.DAL.remisiones();
            oblRemision = new MedNeg.Remisiones.BlRemisiones();

            if (Session["sIdCliente"] != null)
            {
                oRemision.idCliente = (int)Session["sIdCliente"];
                oRemision.Fecha = DateTime.Now;
                oRemision.Estatus = cmbEstatus.SelectedValue;

                //Validar Folio Repetido
                if (ValidaFolioRepetido())
                {

                    //Validar si se esta respetando el folio automatico y verificar si aun es el mismo o cambio su valor
                    if (Session["iFolioAutomatico"].Equals(txbFolio.Text))
                    {
                        oRemision.Folio = oblRemision.RecuperaFolioAutomatico(sRutaArchivoConfig).ToString();
                    }
                    else
                    {
                        oRemision.Folio = txbFolio.Text;
                    }

                    if ((bool)Session["sEsDePedido"] == true)
                    {
                        oRemision.idPedido = (int)Session["sIdPedido"];
                    }

                    if (oblRemision.NuevoRegistro(oRemision))
                    {
                        //Datos de la bitacora
                        string sDatosBitacora = string.Empty;
                        sDatosBitacora += "Tipo:" + cmbTipoRemision.SelectedValue.ToString() + " ";
                        sDatosBitacora += "Folio:" + txbFolio.Text + " ";
                        sDatosBitacora += "Fecha:" + txbFecha.Text + " ";
                        sDatosBitacora += "Estatus:" + cmbEstatus.SelectedItem.ToString() + " ";
                        sDatosBitacora += "Cliente:" + txbCliente.Text + " ";


                        // Registrar la partida de la remision
                        oRemision = new MedDAL.DAL.remisiones();
                        oRemision = oblRemision.BuscarRemisionFolio(txbFolio.Text);
                        int iIdRemision = oRemision.idRemision;
                        bool bRegistroFallido = false;

                        //Recorrer el objeto de sesion lstDetallePartida que contiene los datos de la partida
                        foreach (MedNeg.Facturas.BlDetallePartida pedidoDetalle in (List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])
                        {

                            //0087 Saber si es un ensamble el que se esta registrando
                            if (pedidoDetalle.bEsEnsamble == true)
                            {
                                AgregarDetalleEnsamble(pedidoDetalle, iIdRemision);
                            }
                            else
                            {
                                oblRemision = new MedNeg.Remisiones.BlRemisiones();
                                MedDAL.DAL.remisiones_partida oRemisionPartida = new MedDAL.DAL.remisiones_partida();

                                oRemisionPartida.idRemision = iIdRemision;
                                oRemisionPartida.idProducto = pedidoDetalle.iIdProducto;
                                oRemisionPartida.Cantidad = pedidoDetalle.dCantidad;
                                oRemisionPartida.IEPS = pedidoDetalle.dIeps;
                                oRemisionPartida.Iva = pedidoDetalle.dIva;
                                oRemisionPartida.Precio = pedidoDetalle.dPrecio;
                                oRemisionPartida.Observaciones = pedidoDetalle.sObservaciones;
                                oRemisionPartida.Descripcion = pedidoDetalle.sDescripcion;

                                //Registrar el detalle del pedido
                                if (!oblRemision.NuevoDetallePartida(oRemisionPartida))
                                {
                                    bRegistroFallido = true;
                                }
                                else
                                {
                                    sDatosBitacora += "Producto:" + pedidoDetalle.iIdProducto.ToString() + " ";
                                    sDatosBitacora += "Cant:" + pedidoDetalle.dCantidad.ToString() + " ";
                                    sDatosBitacora += "IEPS:" + pedidoDetalle.dIeps.ToString() + " ";
                                    sDatosBitacora += "Iva:" + pedidoDetalle.dIva.ToString() + " ";
                                    sDatosBitacora += "Precio:" + pedidoDetalle.dPrecio.ToString() + " ";
                                    sDatosBitacora += "Total:" + Convert.ToDecimal((pedidoDetalle.dCantidad * pedidoDetalle.dPrecio) + pedidoDetalle.dIeps + pedidoDetalle.dIva) + ", ";
                                }


                            }
                        }


                        if (!bRegistroFallido)
                        {
                            //Registrar datos de la remision en la bitacora
                            //lblAviso.Text = "El usuario se ha registrado con éxito";
                            oBitacora = new MedDAL.DAL.bitacora();
                            oblBitacora = new MedNeg.Bitacora.BlBitacora();
                            oBitacora.FechaEntradaSrv = DateTime.Now;
                            oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                            oBitacora.Modulo = "Remisiones";
                            oBitacora.Usuario = Session["usuario"].ToString();
                            oBitacora.Nombre = Session["nombre"].ToString();
                            oBitacora.Accion = "Nueva Remisión";
                            oBitacora.Descripcion = sDatosBitacora;
                            if (!oblBitacora.NuevoRegistro(oBitacora))
                            {
                                lblDatos.Text = "El evento no pudo ser registrado en la bitácora";
                            }

                            //Actualizar el consecutivo en configuracion (la validacion de si esta activa o no la opcion se hace dentro de la misma funcion)
                            oblRemision.ActualizarFolioRemision(sRutaArchivoConfig);
                            Session["sIdCliente"] = null;

                            if ((bool)Session["sEsDePedido"] == true)
                            {
                                //Actualizar el estatus del pedido en caso de que se haya hecho la remision a partir de un pedido
                                MedDAL.DAL.pedidos oPedido = new MedDAL.DAL.pedidos();
                                MedNeg.Pedidos.BlPedidos oblPedido = new MedNeg.Pedidos.BlPedidos();

                                //Actualizar el estatus del pedido
                                oPedido = oblPedido.BuscarPedido((int)Session["sIdPedido"]);
                                oPedido.Estatus = "2";

                                if (!oblPedido.EditarRegistro(oPedido))
                                {
                                    lblDatos.Text = "No se pudo cambiar el estatus del pedido, contacte al administrador";
                                }
                            }

                            /******* Realizar la resta de las existencias ***********/

                            MedNeg.Productos.BlProductos oblProductos = new MedNeg.Productos.BlProductos();
                            MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();


                            oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());

                            MedNeg.Remisiones.BlRemisiones oblRemisiones;
                            bool bModificarExistenciasError = false;
                            bool bStockMin = false;

                            //Recorrer el objeto de sesion lstDetallePartida que contiene los datos de la partida
                            //Checar la existencia del producto despues de extraer la cantidad marcada en el detalle y alertar en caso necesario sobre stock bajo
                            foreach (MedNeg.Facturas.BlDetallePartida oPedidoDetalle in (List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])
                            {
                                oblRemisiones = new MedNeg.Remisiones.BlRemisiones();
                                if (!oblRemisiones.ModificarExistenciaProducto(oUsuario.idAlmacen, oPedidoDetalle.iIdProducto, oPedidoDetalle.dCantidad, 1))
                                    bModificarExistenciasError = true;
                                else
                                {
                                    //Si el proceso de extraer se da exitosamente, se revisa ahora las existencias de todos los lotes de ese almacen
                                    //Posteriormente se revisa si la existencia esta por igual o debajo del Stock Minimo requerido
                                    decimal dCantidad = 0;
                                    foreach (MedDAL.DAL.productos_almacen oProductoAlmacen in oblProductos.ObtenerExistenciaProducto(oPedidoDetalle.iIdProducto, oUsuario.idAlmacen))
                                    {
                                        dCantidad += oProductoAlmacen.Cantidad;
                                    }

                                    MedDAL.DAL.productos_almacen_stocks oProductoAlmacenStocks = oblProductos.ObtenerProductoAlmacenStock(oUsuario.idAlmacen, oPedidoDetalle.iIdProducto);
                                    if (dCantidad <= oProductoAlmacenStocks.StockMin)
                                    {
                                        bStockMin = true;
                                    }
                                }
                            }

                            if (bModificarExistenciasError == true)
                            {
                                lblDatos.Text = "No se pudo modificar la existencia de los productos, por favor contacte al administrador";
                            }
                            if (bStockMin)
                            {
                                ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarStock(1);", true);
                            }
                            /****** Termina resta de las existencias ***************************/
                            Session["sTotalFactura"] = 0;
                        }
                        else
                        {
                            //Eliminar la remisión, su partida e indicar al usuario que lo intente de nuevo, limpiar la cadena de bitacora
                            Eliminar(iIdRemision);
                            sDatosBitacora = "";
                            lblDatos.Text = "No se pudo registrar la remisión, por favor verifique los datos y vuelva a intentarlo";
                        }

                    }
                    else
                    {
                        //Fallo esl registro de la remisión
                        lblDatos.Text = "No se pudo registrar la remisión, por favor verifique los datos y vuelva a intentarlo";
                    }

                }
                else  //si es folio repetido
                {
                    lblDatos.Text = "Folio Repetido, no se puede generar la remisión.";

                }
            }
            else
            {
                lblDatos.Text = "Por favor, elija a un Cliente";
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

            //0087 Agregar el try catch para buscar en ensambles
            try
            {
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
                //0087 Agregar el try catch para buscar en ensambles
                Session["sBolEsEnsamble"] = 0; 
            }
            catch
            {
                //Ver si es un esamble
                MedDAL.DAL.ensamble oEnsamble = new MedDAL.DAL.ensamble();
                MedNeg.Ensambles.BlEnsambles oblEnsamble = new MedNeg.Ensambles.BlEnsambles();

                oEnsamble = oblEnsamble.BuscarNombre(sNombre);

                try
                {
                    txbClave.Text = oEnsamble.ClaveBom.ToString();
                    txbProducto.Text = oEnsamble.Descripcion.ToString();
                    txbIeps.Text = "0";
                    //txbImp1.Text = oProducto.TasaImpuesto1.ToString();
                    //txbImp2.Text = oProducto.TasaImpuesto2.ToString();
                    txbIva.Text = "0";
                    cmbPrecios.Items.Clear();
                    cmbPrecios.Items.Add(oEnsamble.PrecioPublico.ToString());
                    cmbPrecios.Items.Add(oEnsamble.PrecioMinimo.ToString());
                    txbCant.Text = "1";
                    Session["sBolEsEnsamble"] = 1;
                }
                catch
                {
                    txbProducto.Focus();
                }
            }
            txbProducto.Focus();
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
                //0087 Agregar el try catch para buscar en ensambles
                Session["sBolEsEnsamble"] = 0; 
            }
            catch
            {
                //Ver si es un esamble
                MedDAL.DAL.ensamble oEnsamble = new MedDAL.DAL.ensamble();
                MedNeg.Ensambles.BlEnsambles oblEnsamble = new MedNeg.Ensambles.BlEnsambles();

                oEnsamble = oblEnsamble.BuscarEnsamble1(sClaveProducto);

                try
                {
                    txbProducto.Text = oEnsamble.Descripcion.ToString();
                    txbIeps.Text = "0";
                    //txbImp1.Text = oProducto.TasaImpuesto1.ToString();
                    //txbImp2.Text = oProducto.TasaImpuesto2.ToString();
                    txbIva.Text = "0";
                    cmbPrecios.Items.Clear();
                    cmbPrecios.Items.Add(oEnsamble.PrecioPublico.ToString());
                    cmbPrecios.Items.Add(oEnsamble.PrecioMinimo.ToString());
                    txbCant.Text = "1";
                    Session["sBolEsEnsamble"] = 1;
                }
                catch
                {
                    txbProducto.Focus();
                }
            }
            txbProducto.Focus();

        }

        /// <summary>
        ///Cargar los datos del cliente
        /// </summary>
        /// <param name="sNombre"></param>
        private void CargaDatosCliente(string sNombre)
        {
            MedDAL.DAL.clientes oCliente = new MedDAL.DAL.clientes();
            MedNeg.BlClientes.BlClientes oblCliente = new MedNeg.BlClientes.BlClientes();

            oCliente = oblCliente.BuscarPorClave(sNombre.Substring(0, sNombre.IndexOf(" ")));
            
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
                txbClave.Focus();
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
                oblRemision = new MedNeg.Remisiones.BlRemisiones();
                oRemision = new MedDAL.DAL.remisiones();
                oRemision = (MedDAL.DAL.remisiones)oblRemision.BuscarRemision(int.Parse(dgvDatos.SelectedDataKey[0].ToString()));
                
                //Llenar los campos del pedido
                txbFolio.Text = oRemision.Folio;
                txbFecha.Text = oRemision.Fecha.ToShortDateString();
                int iContador = 0;
                cmbEstatus.SelectedIndex = -1;
                foreach (ListItem elemento in cmbEstatus.Items)
                {
                    if (elemento.Value.Equals(oRemision.Estatus.ToString()))
                    {
                        elemento.Selected = true;
                    }
                    iContador++;
                }
                
                //Llenar los campos del cliente
                txbCliente.Text = oRemision.clientes.Nombre + " " + oRemision.clientes.Apellidos;
                txbDireccion.Text = oRemision.clientes.Calle + " " + oRemision.clientes.NumeroExt;
                if (oRemision.clientes.NumeroInt != null)
                {
                    txbDireccion.Text += "Int: " + oRemision.clientes.NumeroInt;
                }

               txbPoblacion.Text = oRemision.clientes.poblaciones.Nombre.ToString() + ", " + oRemision.clientes.municipios.Nombre.ToString() +", " + oRemision.clientes.estados.Nombre.ToString();

               //Lenar los datos de la partida del detalle
               oblRemision = new MedNeg.Remisiones.BlRemisiones();

               //Recuperar la partida del pedido
               var oQuery = oblRemision.RecuperarPartidaRemision(oRemision.idRemision);

               //Limpíar la lista en caso de que tenga algo
               if (((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Count() > 0)
               {
                   ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Clear();
               }

               Session["sTotalFactura"] = 0;

               //0087 Variables para gestionar la carga de datos del producto o del ensamble
               string sClave;
               string sNombre;
               bool bEsEnsamble;


               //Recorrer el resultado y meterlo al datagridview
               foreach (MedDAL.DAL.remisiones_partida oDetalle in oQuery)
               {
                   //0087 Identificar si es un producto o un ensamble
                   if (oDetalle.idEnsamble.Equals(null))
                   {
                       //Datos del producto
                       sClave = oDetalle.productos.Clave1;
                       sNombre = oDetalle.productos.Nombre;
                       bEsEnsamble = false;
                   }
                   else
                   {
                       //Datos del ensamble
                       sClave = oDetalle.ensamble.ClaveBom;
                       sNombre = oDetalle.ensamble.Descripcion;
                       bEsEnsamble = true;
                   }
                   oblDetallePartida = new MedNeg.Facturas.BlDetallePartida(
                       Convert.ToInt32(oDetalle.idProducto),
                       //0087 Comentar para enviar las variables correspondientes que tienen la info del producto o ensamble
                       //oDetalle.productos.Clave1,
                       //oDetalle.productos.Nombre,
                       sClave,
                       sNombre,
                       oDetalle.Cantidad,
                       Convert.ToDecimal(oDetalle.IEPS),
                       Convert.ToDecimal(oDetalle.Iva),
                       Convert.ToDecimal(oDetalle.Precio),
                      oDetalle.Observaciones,
                       Convert.ToDecimal((oDetalle.Precio * oDetalle.Cantidad)+oDetalle.IEPS+oDetalle.Iva),
                       oDetalle.Descripcion,
                       bEsEnsamble //0087
                       );

                     ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Add(oblDetallePartida);
                     Session["sTotalFactura"] = Convert.ToDecimal(Session["sTotalFactura"]) + Convert.ToDecimal((oDetalle.Precio * oDetalle.Cantidad) + oDetalle.IEPS + oDetalle.Iva);
               }
               
                //Hacer el binding de la data al dgvDatos
               lblTotal.Text = "TOTAL:$" + Session["sTotalFactura"].ToString();
                dgvPartidaDetalle.DataBind();

                //si el estatus es 1 (Pedido) aun se pueden agregar articulos de lo contario ya no
               if (oRemision.Estatus == "2")
               {
                   HabilitaRemision();
                   Deshabilita();
               }
               else
               {
                   DeshabilitaRemision();
                   Deshabilita();
               }


            }
            else
            {
                Session["lstDetallePartidaPedidos"] = new List<MedNeg.Facturas.BlDetallePartida>();
                dgvPartidaDetalle.DataSource = ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartidaPedidos"]);
                dgvPartidaDetalle.DataBind();
                //Limpia();
                //Deshabilita();

            }
        }
              

        private void Deshabilita()
        {
            txbFolio.Enabled = false;
            txbFecha.Enabled = false;
            cmbTipoRemision.Enabled = false;
            
        }

        /// <summary>
        /// Editar
        /// </summary>
        private void Editar()
        {
            oRemision = new MedDAL.DAL.remisiones();
            oblRemision = new MedNeg.Remisiones.BlRemisiones();
            //oRemision.idRemision = int.Parse(dgvDatos.SelectedDataKey.Value.ToString());
            oRemision = oblRemision.BuscarRemisionFolio(txbFolio.Text);
            oRemision.Estatus = cmbEstatus.SelectedValue.ToString();

            oblRemision = new MedNeg.Remisiones.BlRemisiones();
            if(oblRemision.EditarRegistro(oRemision))
            {
                //Datos de la bitacora
                sDatosBitacora += "Tipo:" + cmbTipoRemision.SelectedValue.ToString() + " ";
                sDatosBitacora += "Folio:" + txbFolio.Text + " ";
                sDatosBitacora += "Fecha:" + txbFecha.Text + " ";
                sDatosBitacora += "Estatus:" + cmbEstatus.SelectedItem.ToString() + " ";
                sDatosBitacora += "Cliente:" + txbCliente.Text + " ";

                oblRemision = new MedNeg.Remisiones.BlRemisiones();
                if (oblRemision.EliminarRemisionPartida(oRemision.idRemision))
                {
                    bool bRegistroFallido = false;

                    //Recorrer el objeto de sesion lstDetallePartida que contiene los datos de la partida
                    foreach (MedNeg.Facturas.BlDetallePartida pedidoDetalle in (List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])
                    {
                        //Saber si es un ensamble el que se esta registrando
                        if (pedidoDetalle.bEsEnsamble == true)
                        {
                            AgregarDetalleEnsamble(pedidoDetalle, oRemision.idRemision);
                        }
                        else
                        {

                            oblRemision = new MedNeg.Remisiones.BlRemisiones();
                            MedDAL.DAL.remisiones_partida oRemisionPartida = new MedDAL.DAL.remisiones_partida();

                            oRemisionPartida.idRemision = oRemision.idRemision;
                            oRemisionPartida.idProducto = pedidoDetalle.iIdProducto;
                            oRemisionPartida.Cantidad = pedidoDetalle.dCantidad;
                            oRemisionPartida.IEPS = pedidoDetalle.dIeps;
                            oRemisionPartida.Iva = pedidoDetalle.dIva;
                            oRemisionPartida.Precio = pedidoDetalle.dPrecio;

                            //Registrar el detalle del pedido
                            if (!oblRemision.NuevoDetallePartida(oRemisionPartida))
                            {
                                bRegistroFallido = true;
                            }
                            else
                            {
                                sDatosBitacora += "Producto:" + pedidoDetalle.iIdProducto.ToString() + " ";
                                sDatosBitacora += "Cant:" + pedidoDetalle.dCantidad.ToString() + " ";
                                sDatosBitacora += "IEPS:" + pedidoDetalle.dIeps.ToString() + " ";
                                sDatosBitacora += "Iva:" + pedidoDetalle.dIva.ToString() + " ";
                                sDatosBitacora += "Precio:" + pedidoDetalle.dPrecio.ToString() + " ";
                                sDatosBitacora += "Total:" + Convert.ToDecimal((pedidoDetalle.dCantidad * pedidoDetalle.dPrecio) + pedidoDetalle.dIeps + pedidoDetalle.dIva) + ", ";

                           }
                        }
                    }

                    /****** GT: Modificar las existencias de los productos nuevos ***************/

                    MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    MedNeg.Productos.BlProductos oblProductos = new MedNeg.Productos.BlProductos();
                    MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();

                    bool bModificarExistenciasError = false;
                    bool bStockMin = false;

                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());

                    oblRemision = new MedNeg.Remisiones.BlRemisiones();
                    foreach (MedNeg.Facturas.BlDetallePartida remisionDetalleNuevos in (List<MedNeg.Facturas.BlDetallePartida>)Session["lstremisionespartidaedicion"])
                    {
                        if (oblRemision.ModificarExistenciaProducto(oUsuario.idAlmacen, remisionDetalleNuevos.iIdProducto, remisionDetalleNuevos.dCantidad, 1))
                        {
                            //Si el proceso de extraer se da exitosamente, se revisa ahora las existencias de todos los lotes de ese almacen
                            //Posteriormente se revisa si la existencia esta por igual o debajo del Stock Minimo requerido
                            decimal dCantidad = 0;
                            foreach (MedDAL.DAL.productos_almacen oProductoAlmacen in oblProductos.ObtenerExistenciaProducto(remisionDetalleNuevos.iIdProducto, oUsuario.idAlmacen))
                            {
                                dCantidad += oProductoAlmacen.Cantidad;
                            }

                            MedDAL.DAL.productos_almacen_stocks oProductoAlmacenStocks = oblProductos.ObtenerProductoAlmacenStock(oUsuario.idAlmacen, remisionDetalleNuevos.iIdProducto);
                            if (dCantidad <= oProductoAlmacenStocks.StockMin)
                            {
                                bStockMin = true;
                            }
                        }
                        else 
                        {
                            bModificarExistenciasError = true;
                        }
                    }
                    if (bModificarExistenciasError == true)
                    {
                        lblDatos.Text = "No se pudo modificar la existencia de los productos, por favor contacte al administrador";
                    }

                    if (bStockMin)
                    {
                        ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarStock(1);", true);
                    }

                    /****** GT: Modificar las existencias de los productos nuevos ***************/

                    //Anotar en la bitacora la modificación a la remision
                    oBitacora = new MedDAL.DAL.bitacora();
                    oblBitacora = new MedNeg.Bitacora.BlBitacora();
                    oBitacora.FechaEntradaSrv = DateTime.Now;
                    oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                    oBitacora.Modulo = "Remisiones";
                    oBitacora.Usuario = Session["usuario"].ToString();
                    oBitacora.Nombre = Session["nombre"].ToString();
                    oBitacora.Accion = "Edición de Remision";
                    oBitacora.Descripcion = sDatosBitacora;
                    if (!oblBitacora.NuevoRegistro(oBitacora))
                    {
                        //lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                    }
                    Session["sTotalFactura"] = 0;
                }

                
            }

        }

        /// <summary>
        /// Eliminar
        /// </summary>
        /// <param name="iIdRemision"></param>
        private void Eliminar(int iIdRemision)
        {

            //Eliminar primero la partida para la integridad referencial
            oblRemision = new MedNeg.Remisiones.BlRemisiones();
            string sDatosBitacora= string.Empty;

            //Guardar los datos del pedido para la bitacora
            oRemision = new MedDAL.DAL.remisiones();
            oRemision = oblRemision.BuscarRemision(iIdRemision);

            sDatosBitacora += "Folio:" + oRemision.Folio.ToString()+" ";
            sDatosBitacora += "Fecha:" + oRemision.Fecha.ToShortDateString()+" ";
            switch (oRemision.Estatus)
            {
                case "1":
                    sDatosBitacora += "Estatus:Pedido ";
                    break;
                case "2":
                     sDatosBitacora +="Estatus:Remitido ";
                    break;
                case "3":
                    sDatosBitacora +="Estatus:Facturado ";
                    break;
                case "4":
                    sDatosBitacora += "Estatus:Cancelado ";
                    break;
            }
            
            //Recuperar la partida del pedido
            oblRemision = new MedNeg.Remisiones.BlRemisiones();
            var oQuery = oblRemision.RecuperarPartidaRemision(iIdRemision);
            //Recorrer el resultado y meterlo al datagridview
            foreach (MedDAL.DAL.remisiones_partida oDetalle in oQuery)
            {
                sDatosBitacora += "Producto:" + oDetalle.productos.Nombre.ToString()+" ";
                sDatosBitacora += "Cantidad:" + oDetalle.Cantidad.ToString()+ " ";
                sDatosBitacora += "IEPS:" + oDetalle.IEPS.ToString() + " ";
                sDatosBitacora += "Iva:" + oDetalle.Iva.ToString() + " ";
                sDatosBitacora += "Precio:" + oDetalle.Precio.ToString()+ " ";
                sDatosBitacora += "Total:" + Convert.ToDecimal((oDetalle.Cantidad * oDetalle.Precio) + oDetalle.IEPS + oDetalle.Iva)+ ", ";

            }

            if (oblRemision.EliminarRemisionPartida(iIdRemision))
            {
                oblRemision= new MedNeg.Remisiones.BlRemisiones();
                if (oblRemision.EliminarRegistro(iIdRemision))
                {
                    
                    MedDAL.DAL.bitacora oBitacora = new MedDAL.DAL.bitacora();
                    MedNeg.Bitacora.BlBitacora oblBitacora = new MedNeg.Bitacora.BlBitacora();
                    oBitacora.FechaEntradaSrv = DateTime.Now;
                    oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                    oBitacora.Modulo = "Remisiones";
                    oBitacora.Usuario = Session["usuario"].ToString();
                    oBitacora.Nombre = Session["nombre"].ToString();
                    oBitacora.Accion = "Eliminación de Remisión";
                    oBitacora.Descripcion = sDatosBitacora;

                    if (!oblBitacora.NuevoRegistro(oBitacora))
                    {
                        lblDatos.Text = "El evento no pudo ser registrado en la bitácora";
                    }
                }
                else
                {
                    lblDatos.Text = "No se pudo eliminar la remisión, por favor vuelva a intentarlo"; 
                }

            }
            else
            {
                lblDatos.Text = "No se pudo eliminar la remisión, por favor vuelva a intentarlo"; 
            }
           
        }

        /// <summary>
        /// 0087 Metodo que que desgloza los ensambles en sus productos para registrarlo en la partida
        /// </summary>
        protected void AgregarDetalleEnsamble(MedNeg.Facturas.BlDetallePartida renglonEnsamble, int iIdRemision)
        {
            //Recuperar los datos del ensamble
            MedDAL.DAL.ensamble oEnsamble = new MedDAL.DAL.ensamble();
            MedDAL.DAL.ensamble_productos oEnsambleProductos = new MedDAL.DAL.ensamble_productos();
            MedNeg.Ensambles.BlEnsambles oblEnsamble = new MedNeg.Ensambles.BlEnsambles();

            oEnsamble = oblEnsamble.BuscarNombre(renglonEnsamble.sProducto);
            oblEnsamble = new MedNeg.Ensambles.BlEnsambles();
            oEnsambleProductos = oblEnsamble.RecuperarProducto(oEnsamble.ClaveBom);


            //Para registrar el detalle de la remision
            oblRemision = new MedNeg.Remisiones.BlRemisiones();
            MedDAL.DAL.remisiones_partida oRemisionPartida = new MedDAL.DAL.remisiones_partida();

            oRemisionPartida.idRemision = iIdRemision;
            oRemisionPartida.idEnsamble = oEnsamble.idEnsamble;
            oRemisionPartida.idProducto = oEnsambleProductos.idProducto;
            oRemisionPartida.Cantidad = renglonEnsamble.dCantidad;
            oRemisionPartida.IEPS = 0;
            oRemisionPartida.Iva = 0;
            oRemisionPartida.Precio = renglonEnsamble.dPrecio;
            oRemisionPartida.Observaciones = renglonEnsamble.sObservaciones;
            oRemisionPartida.Descripcion = oEnsamble.Descripcion;



            //Registrar el detalle del pedido
            if (!oblRemision.NuevoDetallePartida(oRemisionPartida))
            {
                //bRegistroFallido = true;
            }
            else
            {
                sDatosBitacora += "Producto:" + renglonEnsamble.sProducto.ToString() + " ";
                sDatosBitacora += "Cant:" + renglonEnsamble.dCantidad.ToString() + " ";
                sDatosBitacora += "IEPS:" + renglonEnsamble.dIeps.ToString() + " ";
                sDatosBitacora += "Iva:" + renglonEnsamble.dIva.ToString() + " ";
                sDatosBitacora += "Precio:" + renglonEnsamble.dPrecio.ToString() + " ";
                sDatosBitacora += "Total:" + Convert.ToDecimal((renglonEnsamble.dCantidad * renglonEnsamble.dPrecio) + renglonEnsamble.dIeps + renglonEnsamble.dIva) + ", ";
                sDatosBitacora += "Obs:" + renglonEnsamble.sObservaciones;
            }


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

            //0087 Variables para gestionar la carga de datos del producto o del ensamble
            string sClave;
            string sNombre;
            bool bEsEnsamble;

            //Recorrer el resultado y meterlo al datagridview
            foreach (MedDAL.DAL.pedidos_partida oDetalle in oQuery)
            {
                //0087 Identificar si es un producto o un ensamble
                if (oDetalle.idEnsamble.Equals(null))
                {
                    //Datos del producto
                    sClave = oDetalle.productos.Clave1;
                    sNombre = oDetalle.productos.Nombre;
                    bEsEnsamble = false;
                }
                else
                {
                    //Datos del ensamble
                    sClave = oDetalle.ensamble.ClaveBom;
                    sNombre = oDetalle.ensamble.Descripcion;
                    bEsEnsamble = true;
                }

                oblDetallePartida = new MedNeg.Facturas.BlDetallePartida(
                    Convert.ToInt32(oDetalle.idProducto),
                    //0087 Comentar para enviar las variables correspondientes que tienen la info del producto o ensamble
                    //oDetalle.productos.Clave1,
                    //oDetalle.productos.Nombre,
                    sClave,
                    sNombre,
                    oDetalle.Cantidad,
                    Convert.ToDecimal(oDetalle.IEPS),
                    Convert.ToDecimal(oDetalle.Iva),
                    Convert.ToDecimal(oDetalle.Precio),
                   oDetalle.Observaciones,
                    Convert.ToDecimal((oDetalle.Precio * oDetalle.Cantidad) + oDetalle.IEPS + oDetalle.Iva),
                    oDetalle.Descripcion,
                    bEsEnsamble //0087
                    );

                ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Add(oblDetallePartida);
                Session["sTotalFactura"] = Convert.ToDecimal(Session["sTotalFactura"]) + Convert.ToDecimal((oDetalle.Precio * oDetalle.Cantidad) + oDetalle.IEPS + oDetalle.Iva);
            }

            //Hacer el binding de la data al dgvDatos
            lblTotal.Text = "TOTAL:$" + Session["sTotalFactura"].ToString();
            dgvPartidaDetalle.DataBind();


        }
        protected void imbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedIndex != -1)
            {
                CargarFormulario(true);
                Session["accion"] = 2;
                lblAviso.Text = "";
                //0175 GT
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);
            }
            else
            {
                CargarCatalogo();
                MostrarLista();
                lblAviso.Text = "";
                //0175 GT
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);
            }
        }

        /// <summary>
        /// Buscar
        /// </summary>
        /// <param name="sCadena"></param>
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

            //oblPedido = new MedNeg.Pedidos.BlPedidos();
            //var oQuery = oblPedido.Buscar(sCadena, iTipo);
            oblRemision = new MedNeg.Remisiones.BlRemisiones();
            var oQuery = oblRemision.Buscar(sCadena, iTipo);
            Session["resultadoquery"] = oQuery;

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Remisiones.RemisionesView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Nombre ASC";

            try
            {
                dgvDatos.DataSource = dv;
                dgvDatos.DataKeyNames = new string[] { "idRemision" };
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

        protected void imbEliminar_Click(object sender, EventArgs e)
        {
            if (pnlCatalogo.Visible && dgvDatos.SelectedIndex != -1)
            {
                Eliminar((int)dgvDatos.SelectedValue);
                MostrarLista();
                lblAviso.Text = "";
                CargarCatalogo();
                dgvDatos.SelectedIndex = -1;
            }
            else
            {
                CargarCatalogo();
                lblAviso.Text = "";
                MostrarLista();
            }
        }

        protected void imbMostrar_Click(object sender, EventArgs e)
        {
            dgvDatos.SelectedIndex = -1;
            MostrarLista();
            lblAviso.Text = "";
            //pnlReportes.Visible = false;
            //0175 GT
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }

        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
            lblAviso.Text = "";
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;
            //pnlReportes.Visible = false;
        }

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            //GT0175
            lblAviso.Text = "";
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
        }
      
        protected void imbNuevo_Click(object sender, EventArgs e)
        {
            CargarFormulario(false);
            Session["accion"] = 1;
            txbFecha.Text = DateTime.Now.ToShortDateString();
            dgvDatos.SelectedIndex = -1;
            //lblAviso.Text = "";
            //lblAviso2.Text = "";
            //Habilita();
            lblAviso.Text = "";
            //Limpia();

            //0175 GT
            ConfigurarMenuBotones(true, false, false, false, true, true, false, false);
        }

        /// <summary>
        /// Boton Aceptar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    pnlFormulario.Visible = true;
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    break;
                case 2:
                    Editar();
                    Deshabilita();
                    Session["accion"] = 0;
                    Limpia();
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    break;
            }

        }

        private void Limpia()
        {
            txbFolioPedido.Text = "";
            txbFolio.Text = "";
            txbCliente.Text = "";
            txbDireccion.Text = "";
            txbPoblacion.Text = "";
            lblAviso.Text = "";
            LimpiarDatosDetalle();
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;
            ////pnlReportes.Visible = false;
            Session["sTotalFactura"] = 0;
            //Limpíar la lista en caso de que tenga algo
            if (((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Count() > 0)
            {
                ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Clear();
            }
        }

        /// <summary>
        /// Agregar detalle a la partida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbAgregarDetalle_Click(object sender, ImageClickEventArgs e)
        {
            lblAviso.Text = "";
            //Evitar mandar valores nullos para las conversiones
            //int iCantidad;
            decimal dCantidad,dIeps, dImp1=0, dImp2=0, dIva, dPrecio, dTotal;

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

            MedNeg.Usuarios.BlUsuarios oblUsuarios = new MedNeg.Usuarios.BlUsuarios();
            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
            oUsuario = (MedDAL.DAL.usuarios)oblUsuarios.Buscar(int.Parse(Session["usuarioid"].ToString()));

            List<MedDAL.DAL.productos_almacen> lstProductosAlmacen = new List<MedDAL.DAL.productos_almacen>();
            lstProductosAlmacen.AddRange(oblProducto.ObtenerExistenciaProducto(iIdProducto, oUsuario.idAlmacen));

            string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            MedDAL.Configuracion.DALConfiguracion objConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
            objConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

            if (lstProductosAlmacen.Count != 0)
            {
                if (lstProductosAlmacen[0].Cantidad >= dCantidad || (objConfiguracion.iVentasNegativas == 1 && lstProductosAlmacen[0].Cantidad < dCantidad) || lstProductosAlmacen[0].FechaCaducidad.Value > DateTime.Today)
                {
                    // 0087 Datos del ensamble
                    bool bEsEnsamble;
                    if ((int)Session["sBolEsEnsamble"] == 1)
                        bEsEnsamble = true;
                    else
                        bEsEnsamble = false;

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
                        dTotal,
                        txbProducto.Text, // 0087 Datos del ensamble
                        bEsEnsamble); // 0087 Datos del ensamble

                    //Agregar el objeto detalle partida al objeto lstDetallePartida
                    ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Add(oblDetallePartida);
                    dgvPartidaDetalle.DataBind();

                    //GT: Agregar los nuevos detalles a una lista alterna para poder modificar sus existencias sin duplicar las de los productos ya existentes
                    if ((int)Session["accion"] == 2)
                        ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstremisionespartidaedicion"]).Add(oblDetallePartida);



                    Session["sTotalFactura"] = Convert.ToDecimal(Session["sTotalFactura"]) + dTotal;
                    lblTotal.Text = "TOTAL:$" + Session["sTotalFactura"].ToString();
                    LimpiarDatosDetalle();
                    txbClave.Focus();

                    if (lstProductosAlmacen[0].FechaCaducidad.Value <= DateTime.Today.AddDays(objConfiguracion.iCaducidad))
                    {
                        ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarCaducidad(2);", true);
                    }
                }
                else if (objConfiguracion.iVentasNegativas == 0 && lstProductosAlmacen[0].Cantidad < dCantidad)
                {
                    ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarStock(2);", true);  
                }
                else if (lstProductosAlmacen[0].FechaCaducidad.Value <= DateTime.Today)
                {
                    ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarCaducidad(3);", true);
                }
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
                    dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[7].Text = "Facturada";
                if (dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[7].Text == "4")
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
            //dgvDatos.SelectedRowStyle.BackColor = System.Drawing.Color.Yellow;
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
        /// Select index del combo de tipo remision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbTipoRemision_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTipoRemision.SelectedValue == "2")
            {
                txbFolioPedido.Enabled = true;
                Session["sEsDePedido"] = true;
                txbFolioPedido.Focus();
            }
            else
            {
                txbFolioPedido.Enabled = false;
                Session["sEsDePedido"] = false;
                txbFolio.Focus();
            }
        }

        /// <summary>
        /// Change del folio pedido
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txbFolioPedido_TextChanged(object sender, EventArgs e)
        {
            //Saber si es una remision de un pedido
            if (cmbTipoRemision.SelectedValue == "2" && txbFolioPedido.Text.Count() > 0)
            {
                CargaPedido(txbFolioPedido.Text);
            }
        }

        /// <summary>
        /// Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            lblAviso.Text = "";
            Buscar(txbBuscar.Text);
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }

        #region Editar Precios y Cantidad
        protected void dgvPartidaDetalle_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dgvPartidaDetalle.EditIndex = e.NewEditIndex;
            DataBind();
            dgvPartidaDetalle.Width = new Unit("830px");
            dgvPartidaDetalle.Rows[dgvPartidaDetalle.EditIndex].Cells[0].Width = new Unit("40px");
            dgvPartidaDetalle.Rows[dgvPartidaDetalle.EditIndex].Cells[1].Width = new Unit("80px");
            dgvPartidaDetalle.Rows[dgvPartidaDetalle.EditIndex].Cells[2].Width = new Unit("80px");
            ((TextBox)dgvPartidaDetalle.Rows[dgvPartidaDetalle.EditIndex].Cells[3].FindControl("TextBox1")).Width = new Unit("30px");
            dgvPartidaDetalle.Rows[dgvPartidaDetalle.EditIndex].Cells[4].Width = new Unit("80px");
            dgvPartidaDetalle.Rows[dgvPartidaDetalle.EditIndex].Cells[5].Width = new Unit("80px");
            ((TextBox)dgvPartidaDetalle.Rows[dgvPartidaDetalle.EditIndex].Cells[6].FindControl("TextBox2")).Width = new Unit("40px");
            dgvPartidaDetalle.Rows[dgvPartidaDetalle.EditIndex].Cells[7].Width = new Unit("200px");
            dgvPartidaDetalle.Rows[dgvPartidaDetalle.EditIndex].Cells[8].Width = new Unit("80px");
            dgvPartidaDetalle.Rows[dgvPartidaDetalle.EditIndex].Cells[9].Width = new Unit("30px");
            Session["editarpartidas"] = 1;
        }

        protected void dgvPartidaDetalle_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dgvPartidaDetalle.EditIndex = -1;
            dgvPartidaDetalle.DataSource = ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]);
            dgvPartidaDetalle.DataBind();
        }

        protected void dgvPartidaDetalle_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])[dgvPartidaDetalle.EditIndex].DCantidad = decimal.Parse(((TextBox)dgvPartidaDetalle.Rows[dgvPartidaDetalle.EditIndex].Cells[3].FindControl("TextBox1")).Text);
            ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])[dgvPartidaDetalle.EditIndex].DPrecio = decimal.Parse(((TextBox)dgvPartidaDetalle.Rows[dgvPartidaDetalle.EditIndex].Cells[6].FindControl("TextBox2")).Text);
            ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])[dgvPartidaDetalle.EditIndex].DTotal = ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])[dgvPartidaDetalle.EditIndex].DCantidad * ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])[dgvPartidaDetalle.EditIndex].DPrecio + ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])[dgvPartidaDetalle.EditIndex].DIva + ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])[dgvPartidaDetalle.EditIndex].DIeps;
            dgvPartidaDetalle.Rows[dgvPartidaDetalle.EditIndex].Cells[8].Text = ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])[dgvPartidaDetalle.EditIndex].DTotal.ToString();
            dgvPartidaDetalle.EditIndex = -1;
            dgvPartidaDetalle.DataSource = ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]);
            dgvPartidaDetalle.DataBind();
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
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;
            //pnlReportes.Visible = true;

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsPedidos odsPedidos = new MedDAL.DataSets.dsPedidos();
            odsPedidos = (MedDAL.DataSets.dsPedidos)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsPedidos, "clientes");
            odsPedidos = (MedDAL.DataSets.dsPedidos)LlenarDataSet("select * from remisiones", "medicuriConnectionString", odsPedidos, "remisiones");
            odsPedidos = (MedDAL.DataSets.dsPedidos)LlenarDataSet("select * from remisiones_partida", "medicuriConnectionString", odsPedidos, "remisiones_partida");

            //GT 0179
            Session["campoaordenar"] = "";
            Session["sortfield"] = 0;  

            Session["dataset"] = odsPedidos;
            Session["reportdocument"] = "~\\rptReportes\\rptRemisiones.rpt";
            Session["titulo"] = "Remisiones";
            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));

            //string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            //MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            //MedDAL.Configuracion.DALConfiguracion objConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
            //objConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

            rptReporte.SetDataSource(odsPedidos);

            //foreach (FormulaFieldDefinition oFormula in rptReporte.DataDefinition.FormulaFields)
            //{
            //    if (oFormula.FormulaName == "{@fRazonSocial}")
            //    {
            //        oFormula.Text = "'" + objConfiguracion.sRazonSocial.ToString() + "'";
            //    }
            //    if (oFormula.FormulaName == "{@fRFC}")
            //    {
            //        oFormula.Text = "'" + objConfiguracion.sRfc.ToString() + "'";
            //    }
            //    if (oFormula.FormulaName == "{@fDomicilio}")
            //    {
            //        oFormula.Text = "'" + objConfiguracion.sDomicilio.ToString() + "'";
            //    }
            //}

            //Saber si es general o se tiene un usuario seleccionado
            if (dgvDatos.SelectedIndex != -1)
            {
                Session["recordselection"] = "{remisiones.idRemision}=" + dgvDatos.SelectedDataKey.Values[0].ToString();
            }
            else
            {
                Session["recordselection"] = "";
            }

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

            //string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            //MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            //MedDAL.Configuracion.DALConfiguracion objConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
            //objConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

            //foreach (FormulaFieldDefinition oFormula in rptReporte.DataDefinition.FormulaFields)
            //{
            //    if (oFormula.FormulaName == "{@fRazonSocial}")
            //    {
            //        oFormula.Text = "'" + objConfiguracion.sRazonSocial.ToString() + "'";
            //    }
            //    if (oFormula.FormulaName == "{@fRFC}")
            //    {
            //        oFormula.Text = "'" + objConfiguracion.sRfc.ToString() + "'";
            //    }
            //    if (oFormula.FormulaName == "{@fDomicilio}")
            //    {
            //        oFormula.Text = "'" + objConfiguracion.sDomicilio.ToString() + "'";
            //    }
            //}
            //crvReporte.Visible = true;
            //crvReporte.ReportSource = rptReporte;
            //crvReporte.PageZoomFactor = 100;
        }

        
        /// <summary>
        /// Obtiene el ReportDocument a partir de un reporte existente en el proyecto.
        /// </summary>
        /// <param name="sNombreReporte"></param>
        /// <returns></returns>
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
                
        //        string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
        //        MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
        //        MedDAL.Configuracion.DALConfiguracion objConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
        //        objConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);
                
        //        foreach (FormulaFieldDefinition oFormula in repDoc.DataDefinition.FormulaFields)
        //        {
        //            if (oFormula.FormulaName == "{@fRazonSocial}")
        //            {
        //                oFormula.Text = "'" + objConfiguracion.sRazonSocial.ToString() + "'";
        //            }
        //            if (oFormula.FormulaName == "{@fRFC}")
        //            {
        //                oFormula.Text = "'" + objConfiguracion.sRfc.ToString() + "'";
        //            }
        //            if (oFormula.FormulaName == "{@fDomicilio}")
        //            {
        //                oFormula.Text = "'" + objConfiguracion.sDomicilio.ToString() + "'";
        //            }
        //        }
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

        //        string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
        //        MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
        //        MedDAL.Configuracion.DALConfiguracion objConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
        //        objConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

        //        foreach (FormulaFieldDefinition oFormula in repDoc.DataDefinition.FormulaFields)
        //        {
        //            if (oFormula.FormulaName == "{@fRazonSocial}")
        //            {
        //                oFormula.Text = "'" + objConfiguracion.sRazonSocial.ToString() + "'";
        //            }
        //            if (oFormula.FormulaName == "{@fRFC}")
        //            {
        //                oFormula.Text = "'" + objConfiguracion.sRfc.ToString() + "'";
        //            }
        //            if (oFormula.FormulaName == "{@fDomicilio}")
        //            {
        //                oFormula.Text = "'" + objConfiguracion.sDomicilio.ToString() + "'";
        //            }
        //        }

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

        //        string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
        //        MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
        //        MedDAL.Configuracion.DALConfiguracion objConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
        //        objConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

        //        foreach (FormulaFieldDefinition oFormula in repDoc.DataDefinition.FormulaFields)
        //        {
        //            if (oFormula.FormulaName == "{@fRazonSocial}")
        //            {
        //                oFormula.Text = "'" + objConfiguracion.sRazonSocial.ToString() + "'";
        //            }
        //            if (oFormula.FormulaName == "{@fRFC}")
        //            {
        //                oFormula.Text = "'" + objConfiguracion.sRfc.ToString() + "'";
        //            }
        //            if (oFormula.FormulaName == "{@fDomicilio}")
        //            {
        //                oFormula.Text = "'" + objConfiguracion.sDomicilio.ToString() + "'";
        //            }
        //        }
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

        protected void dgvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Remisiones.RemisionesView>)Session["resultadoquery"];
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
            var result = (IQueryable<MedDAL.Remisiones.RemisionesView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvDatos.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Nombre" : ViewState["sortexpression"].ToString(), dv, ref dgvDatos, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            dgvDatos.DataBind();
        }

        #endregion

    }    
}
