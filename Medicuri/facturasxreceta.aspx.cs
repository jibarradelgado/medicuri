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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


namespace Medicuri
{
    public partial class facturasxporgrama : System.Web.UI.Page
    {
       
        ImageButton imbNuevo, imbEditar, imbEliminar, imbImprimir, imbMostrar, imbAceptar, imbCancelar;
        RadioButton rdbNombre, rdbClave, rdbTodos;
        Label lblReportes, lblNombreModulo;
        Button btnBuscar;
        TextBox txbBuscar;

        //Declaración del objeto de la capa de negocio de almacenes
        MedNeg.Almacenes.BlAlmacenes oblAlmacen;
        IQueryable<MedDAL.DAL.almacenes> iqrAlmacenes;

        //Declaracion del objeto de la capa de negocio de lineas de créditos
        MedNeg.LineasCredito.BlLineasCredito oblLineasCredito;
        IQueryable<MedDAL.DAL.lineas_creditos> iqrLineasCreditos;

        List<MedNeg.Facturas.BlFacturacionDeLineas> lstTotalFacturadoPorLinea = new List<MedNeg.Facturas.BlFacturacionDeLineas>();
        List<MedNeg.Facturas.BlDetalleFacturaReceta> lstDetalleReceta = new List<MedNeg.Facturas.BlDetalleFacturaReceta>();
           const decimal consdIva = 0.16M;
           int iContadorRenglones = 0;
           int iContadorFacturas = 1;

        /// <summary>
        /// Actualiza la variable de sesion "lstAlmacenesFacturas", la cual es una lista de los almacenes activos
        /// </summary>
        protected void ActualizarSesionAlmacenes()
        {
            oblAlmacen = new MedNeg.Almacenes.BlAlmacenes();
            iqrAlmacenes = oblAlmacen.BuscarAlmacenesActivos();
            Session["lstAlmacenesFacturas"] = iqrAlmacenes;
        }

                    
        protected void Page_Load(object sender, EventArgs e)
        {

            
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';

            try
            {
                #region Interfaz
                cPermiso = (char)htbPermisos["facturas"];
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
                //Master.FindControl("btnNuevo").Visible = false;
                Master.FindControl("btnEliminar").Visible = false;
                Master.FindControl("btnCancelar").Visible = false;
                Master.FindControl("btnEditar").Visible = false;
                Master.FindControl("btnAceptar").Visible = false;
                Master.FindControl("rdbFiltro1").Visible = false;
                Master.FindControl("rdbFiltro2").Visible = false;
                Master.FindControl("rdbFiltro3").Visible = false;
                Master.FindControl("btnBuscar").Visible = false;
                Master.FindControl("txtBuscar").Visible = false;
                Master.FindControl("lblBuscar").Visible = false;

                rdbTodos = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbTodos.Text = "Folio";
                rdbClave = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbClave.Text = "Cliente";
                rdbNombre = (RadioButton)Master.FindControl("rdbFiltro3");
                rdbNombre.Text = "Fecha";

                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
                txbBuscar = (TextBox)Master.FindControl("txtBuscar");
                Master.FindControl("btnReportes").Visible = true;

                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "Facturación por recetas";

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
                    txbFechaDesde.Enabled = false;
                    txbFechaHasta.Enabled = false;
                    txbFolioDesde.Enabled = false;
                    txbFolioHasta.Enabled = false;
                    //MostrarLista();
                    //Recuperar los almacenes y llenar el combo
                    ActualizarSesionAlmacenes();
                    cmbAlmacenes.Items.Clear();
                    cmbAlmacenes.DataSource = (IQueryable<MedDAL.DAL.almacenes>)Session["lstAlmacenesFacturas"];
                    cmbAlmacenes.DataBind();
                    pnlCatalogoTotales.Visible = false;
                    pnlFormulario.Visible = false;
                    ////pnlReportes.Visible = false;
                    Session["resultadoquery"] = "";
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;

                    Session["reporteactivo"] = 0;
                    Session["reportdocument"] = "";
                    Session["titulo"] = "";
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
                pnlCatalogoTotales.Visible = false;                
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }

        }

        //private void CargaCamposEditablesProductos()
        //{
        //    MedNeg.CamposEditables.BlCamposEditables oblCamposEditables = new MedNeg.CamposEditables.BlCamposEditables();

        //    var oQuery = oblCamposEditables.Buscar("Productos");
        //    foreach (MedDAL.DAL.campos_editables oCampo in oQuery)
        //    {
        //        //si tiene un valor asignado se carga
        //        if (oCampo.Valor != "")
        //        {
        //            cmbDatosOpcionales.Items.Add(oCampo.Valor);
        //        }
        //    }
        //}

        /// <summary>
        /// Validar que el folio no exista en el sistema
        /// </summary>
        /// <returns>False si existe</returns>
        private bool ValidaFolioRepetido(string sFolioValidar)
        {
            MedNeg.Facturas.BlFacturas oblFacturas = new MedNeg.Facturas.BlFacturas();

            if (!oblFacturas.ValidarFolioRepetido(sFolioValidar))
                return false;
            else
                return true;
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


        private string CrearFacturaSubTotales(string sFolio,decimal dSubtotalFacturar)
        {
            //TODO: GT Paso 6: Paso opcional, aqui se genera la factura por sub totales si lo eligio el usuario

            //objeto que contiene la receta
            string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            string sRutaCertificados = Server.MapPath("~/Archivos/");

            bool bRegistroFallido = false;
            //MedDAL.DAL.recetas oReceta = new MedDAL.DAL.recetas();
            //MedNeg.Recetas.BlRecetas oblReceta = new MedNeg.Recetas.BlRecetas();

            MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            MedDAL.Configuracion.DALConfiguracion odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();

            MedDAL.DAL.facturas oFacturas = new MedDAL.DAL.facturas();
            MedNeg.Facturas.BlFacturas oblFacturas = new MedNeg.Facturas.BlFacturas();

            //decimal dSubtotal = 0;
            //int idReceta = 0;

            odalConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

            //ID Cliente
            oFacturas.idCliente = (int)Session["sIdCliente"];

            //IDRECETA
            //oFacturas.idReceta = oReceta.idReceta;

            //Asiganr el folio de la factura
            if (odalConfiguracion.iFacturasAutomatico == 1)
            {
                oFacturas.Folio = odalConfiguracion.iFolioFacturas++.ToString();
            }
            else
            {
                //oFacturas.Folio = oReceta.Folio;
                oFacturas.Folio = sFolio + "-GastoAdm";
            }

            string sIdFolioFactura = oFacturas.Folio;

            //Fecha
            oFacturas.Fecha = DateTime.Now;
            oFacturas.FechaAplicacion = DateTime.Now;

            //tipo (4 por que son de receta)
            oFacturas.TipoFactura = "4";

            //Estatus (3 de emitida)
            oFacturas.Estatus = "3";

            //Id del usuario que genero la factura
            oFacturas.idUsuario=Convert.ToInt32(Session["usuarioid"]);

            //Nombre del vendedor que en este caso es el usuario
            oFacturas.Vendedor = Session["nombre"].ToString();

            //Registrar la factura
            if (oblFacturas.NuevoRegistro(oFacturas))
            {

                oFacturas = new MedDAL.DAL.facturas();
                oblFacturas = new MedNeg.Facturas.BlFacturas();

                ////Recuperar el id de la factura crea| da
                oFacturas = oblFacturas.BuscarFacturasFolio(sIdFolioFactura);

                //Actualizar el consecutivo en la bitacora
                oblFacturas.ActualizarFolioFactura(sRutaArchivoConfig);


                //Insertar el detalle de factura
                    oblFacturas = new MedNeg.Facturas.BlFacturas();
                    MedDAL.DAL.facturas_partida oFacturaPartida = new MedDAL.DAL.facturas_partida();

                    oFacturaPartida.idFactura = oFacturas.idFactura;
                    oFacturaPartida.idProducto = (int)Session["sIdProductoFxR"];
                    oFacturaPartida.Cantidad = 1;
                    oFacturaPartida.IEPS = 0;
                   
                   

                    //Identificar de cuanto va a ser la factura de subtotales, si un porciento del subtotal o un monto especifico
                    if (txbMonto.Text == "" && txbPorcentaje.Text=="")
                    {
                        oFacturaPartida.Precio = dSubtotalFacturar;
                        oFacturaPartida.Iva = dSubtotalFacturar * consdIva;
                    }
                    else if (txbMonto.Text != "") //El sub total es por monto
                    {
                        oFacturaPartida.Precio = Convert.ToDecimal(txbMonto.Text);
                        oFacturaPartida.Iva = Convert.ToDecimal(txbMonto.Text) * consdIva;

                    }
                    else if(txbPorcentaje.Text!="") //El sub total es por porcentaje del monto de sub total
                    {
                        oFacturaPartida.Precio=dSubtotalFacturar*(Convert.ToDecimal(txbPorcentaje.Text)/100);
                        oFacturaPartida.Iva=consdIva*(dSubtotalFacturar*(Convert.ToDecimal(txbPorcentaje.Text)/100));
                    }
                                  

                    oFacturaPartida.Descripcion = txbProductos.Text;
                    oFacturaPartida.Observaciones = "";

                    //Registrar el detalle del pedido
                    if (!oblFacturas.NuevoDetallePartida(oFacturaPartida))
                    {
                        bRegistroFallido = true;
                    }
                    else
                    {
                      
                    }
                //TODO: GT Paso 6.1 Aqui si bRegistroFallido es false entonces aqui se generaria la factura electronica con los datos de sub totales
                    if (cmbModoFactura.SelectedValue == "2" && !bRegistroFallido)
                    {
                        oblFacturas = new MedNeg.Facturas.BlFacturas();
                        oblFacturas.GenerarFacturaElectronica(oFacturas.idFactura, sRutaCertificados, Session["usuario"].ToString(), (int)Session["sIdCliente"], oFacturas.Folio);

                        return Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFacturas.Folio + ".xml");
                        //System.IO.FileInfo fFactura = new System.IO.FileInfo(Server.MapPath("~/Archivos/FacturasElectronicas/FacturaE-" + oFacturas.Folio + ".xml"));

                        //Response.Clear();
                        //Response.AddHeader("Content-Disposition", "attachment; filename=" + fFactura.Name);
                        //Response.AddHeader("Content-Length", fFactura.Length.ToString());
                        //Response.ContentType = "application/....";
                        //Response.WriteFile(fFactura.FullName);
                        //Response.End();
                    }
                    return "";
            }
            return "";
        }



        /// <summary>
        /// Factura que crea una receta con gastos de administracion
        /// </summary>
        /// <param name="sFolio"></param>
        /// <param name="lstDetalleFactura"></param>
        private string CrearFacturaGtoAdministrativo(string sFolio,List<MedNeg.Facturas.BlDetalleFacturaReceta> lstDetalleFactura)
        {
            //TODO: GT Paso 5: Paso opcional, aqui se genera la factura por gastos administrativos si lo eligio el usuario

            //objeto que contiene la receta
            string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            string sRutaCertificados = Server.MapPath("~/Archivos/");

            bool bRegistroFallido = false;
            //MedDAL.DAL.recetas oReceta = new MedDAL.DAL.recetas();
            //MedNeg.Recetas.BlRecetas oblReceta = new MedNeg.Recetas.BlRecetas();

            MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            MedDAL.Configuracion.DALConfiguracion odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();

            MedDAL.DAL.facturas oFacturas = new MedDAL.DAL.facturas();
            MedNeg.Facturas.BlFacturas oblFacturas = new MedNeg.Facturas.BlFacturas();

            //decimal dSubtotal = 0;
            //int idReceta = 0;

            odalConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

            //ID Cliente
            oFacturas.idCliente = (int)Session["sIdCliente"];

            //IDRECETA
            //oFacturas.idReceta = oReceta.idReceta;

            //Asiganr el folio de la factura
            if (odalConfiguracion.iFacturasAutomatico == 1)
            {
                oFacturas.Folio = odalConfiguracion.iFolioFacturas++.ToString();
            }
            else
            {
                //oFacturas.Folio = oReceta.Folio;
                oFacturas.Folio = sFolio + "-GastoAdm";
            }

            string sIdFolioFactura = oFacturas.Folio;

            //Fecha
            oFacturas.Fecha = DateTime.Now;
            oFacturas.FechaAplicacion = DateTime.Now;

            //tipo (4 por que son de receta)
            oFacturas.TipoFactura = "4";

            //Estatus (3 de emitida)
            oFacturas.Estatus = "3";

            //Id del usuario que genero la factura
            oFacturas.idUsuario = Convert.ToInt32(Session["usuarioid"]);

            //Nombre del vendedor que en este caso es el usuario
            oFacturas.Vendedor = Session["nombre"].ToString();

            //Registrar la factura
            if (oblFacturas.NuevoRegistro(oFacturas))
            {

                oFacturas = new MedDAL.DAL.facturas();
                oblFacturas = new MedNeg.Facturas.BlFacturas();

                ////Recuperar el id de la factura crea| da
                oFacturas = oblFacturas.BuscarFacturasFolio(sIdFolioFactura);

                //Actualizar el consecutivo en la bitacora
                oblFacturas.ActualizarFolioFactura(sRutaArchivoConfig);


                //Insertar el detalle de factura
                //Recorrer el objeto de sesion lstDetallePartida que contiene los datos de la partida
                foreach (MedNeg.Facturas.BlDetalleFacturaReceta renglon in lstDetalleFactura)
                {
                    oblFacturas = new MedNeg.Facturas.BlFacturas();
                    MedDAL.DAL.facturas_partida oFacturaPartida = new MedDAL.DAL.facturas_partida();

                    oFacturaPartida.idFactura = oFacturas.idFactura;
                    oFacturaPartida.idProducto = renglon.iIdProducto;
                    oFacturaPartida.Cantidad = renglon.dCantidad;
                    oFacturaPartida.IEPS = renglon.dIeps;
                    oFacturaPartida.Iva = renglon.DImp1 * consdIva;
                    oFacturaPartida.Precio = renglon.DImp1;

                    //Registrar el detalle del pedido
                    if (!oblFacturas.NuevoDetallePartida(oFacturaPartida))
                    {
                        bRegistroFallido = true;
                    }
                    else
                    {
                       
                        
                    }
                }

                //TODO: GT Paso 5.1 Aqui si bRegistroFallido es false entonces aqui se generaria la factura electronica de gastos administrativos
               
                if (cmbModoFactura.SelectedValue == "2" && !bRegistroFallido)
                {
                    oblFacturas = new MedNeg.Facturas.BlFacturas();
                    oblFacturas.GenerarFacturaElectronica(oFacturas.idFactura, sRutaCertificados, Session["usuario"].ToString(), (int)Session["sIdCliente"], oFacturas.Folio);

                    return Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFacturas.Folio + ".xml");
                    //System.IO.FileInfo fFactura = new System.IO.FileInfo(Server.MapPath("~/Archivos/FacturasElectronicas/FacturaE-" + oFacturas.Folio + ".xml"));

                    //Response.Clear();
                    //Response.AddHeader("Content-Disposition", "attachment; filename=" + fFactura.Name);
                    //Response.AddHeader("Content-Length", fFactura.Length.ToString());
                    //Response.ContentType = "application/....";
                    //Response.WriteFile(fFactura.FullName);
                    //Response.End();
                }
                return "";
            }
            return "";
        }



         /// <summary>
        /// funcion que crea una factura nueva sin gastos de administracion
        /// </summary>
        /// <param name="sFolio"></param>
        /// <param name="iTipo">1-Tradicional, 2-Electronica</param>
        /// <returns></returns>
        private void CrearFactura(List<MedNeg.Facturas.BlDetalleFacturaReceta> lstDetalleFactura, int iTipo)
        {
            //TODO: GT Paso 3: Generar la factura con todo lo contabilizado de las recetas que van a conformar esta factura.

             //objeto que contiene la receta
            string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            string sRutaCertificados = Server.MapPath("~/Archivos/");
            string sFacturaAdicional = "";
            string sFacturaAdicional2 = "";

            MedDAL.DAL.recetas oReceta = new MedDAL.DAL.recetas();
            MedNeg.Recetas.BlRecetas oblReceta = new MedNeg.Recetas.BlRecetas();
            
            MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            MedDAL.Configuracion.DALConfiguracion odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();

            MedDAL.DAL.facturas oFacturas = new MedDAL.DAL.facturas();
            MedNeg.Facturas.BlFacturas oblFacturas = new MedNeg.Facturas.BlFacturas();
            MedDAL.DAL.FacturacionDeRecetas oFacturacionRecetas = new MedDAL.DAL.FacturacionDeRecetas();
           

            decimal dSubtotal = 0;
            int idReceta = 0;

            odalConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);
          
            //ID Cliente
            oFacturas.idCliente = (int)Session["sIdCliente"];

            //IDRECETA
            //oFacturas.idReceta = oReceta.idReceta;
            
            //Asiganr el folio de la factura
            if (odalConfiguracion.iFacturasAutomatico == 1)
            {
                oFacturas.Folio = odalConfiguracion.iFolioFacturas++.ToString();
            }
            else
            {
                string sFolioAux;
                sFolioAux = "FxR" + DateTime.Now.ToShortDateString() + iContadorFacturas.ToString();
                
                //Validar que no exista el folio
                while (!ValidaFolioRepetido(sFolioAux))
                    sFolioAux = "FxR" + DateTime.Now.ToShortDateString() + iContadorFacturas++.ToString();

                oFacturas.Folio = sFolioAux;
            }

            string sIdFolioFactura=oFacturas.Folio;

            //Fecha
            oFacturas.Fecha = DateTime.Now;
            oFacturas.FechaAplicacion = DateTime.Now;

            //tipo (4 por que son de receta)
            oFacturas.TipoFactura = "4";

            //Estatus (3 de emitida)
            oFacturas.Estatus = "3";

            //Id del usuario que genero la factura
            oFacturas.idUsuario = Convert.ToInt32(Session["usuarioid"]);

            //Nombre del vendedor que en este caso es el usuario
            oFacturas.Vendedor = Session["nombre"].ToString();

            //Registrar la factura
            if (oblFacturas.NuevoRegistro(oFacturas))
            {
                
                oFacturas = new MedDAL.DAL.facturas();
                oblFacturas = new MedNeg.Facturas.BlFacturas();

                ////Recuperar el id de la factura crea| da
                oFacturas = oblFacturas.BuscarFacturasFolio(sIdFolioFactura);
                
                //Actualizar el consecutivo en la configuracion
                oblFacturas.ActualizarFolioFactura(sRutaArchivoConfig);
                iContadorFacturas++;

                //Insertar el detalle de factura
                //Recorrer el objeto de sesion lstDetallePartida que contiene los datos de la partida
                foreach (MedNeg.Facturas.BlDetalleFacturaReceta renglon in lstDetalleFactura)
                {
                    oblFacturas = new MedNeg.Facturas.BlFacturas();
                    MedDAL.DAL.facturas_partida oFacturaPartida = new MedDAL.DAL.facturas_partida();

                    oFacturaPartida.idFactura = oFacturas.idFactura;
                    oFacturaPartida.idProducto = renglon.iIdProducto;
                    oFacturaPartida.Cantidad = renglon.dCantidad;
                    oFacturaPartida.IEPS = renglon.dIeps;
                    oFacturaPartida.Iva = renglon.dIva;
                    oFacturaPartida.Precio = renglon.dPrecio;

                    //Registrar el detalle del pedido
                    if (!oblFacturas.NuevoDetallePartida(oFacturaPartida))
                    {
                        //bRegistroFallido = true;
                    }
                    else
                    {
                        //Guardar el monto de las lineas
                        MedNeg.Facturas.BlFacturacionDeLineas oblFacturacionLinea = new MedNeg.Facturas.BlFacturacionDeLineas();

                        oblFacturacionLinea.DMonto = oFacturaPartida.Cantidad * oFacturaPartida.Precio;
                        oblFacturacionLinea.DtFecha = DateTime.Now;
                        oblFacturacionLinea.IIdLineaCredito = renglon.IIdLineaCredito;

                        lstTotalFacturadoPorLinea.Add(oblFacturacionLinea);

                        //Guardar el subtotal para cuando sea una facturacion de Gtos. Admon por subtotales
                        dSubtotal += oFacturaPartida.Cantidad * oFacturaPartida.Precio;

                        //Modificar el estatus de la receta
                        if (idReceta != renglon.IIdReceta)
                        {
                            MedDAL.DAL.recetas oRecetaEditar = new MedDAL.DAL.recetas();
                            MedNeg.Recetas.BlRecetas oblRecetas = new MedNeg.Recetas.BlRecetas();

                            oRecetaEditar.idReceta = renglon.IIdReceta;
                            oRecetaEditar.Estatus = "2";

                            oblRecetas = new MedNeg.Recetas.BlRecetas();
                            oblRecetas.EditarRegistro(oRecetaEditar);

                            idReceta = renglon.IIdReceta;

                        }

                        //TODO: Paso 3.1 Aqui generar la factura electronica de las recetas facturadas
                    }
                }


                if (iTipo == 2)
                {
                    oblFacturas = new MedNeg.Facturas.BlFacturas();
                    oblFacturas.GenerarFacturaElectronica(oFacturas.idFactura, sRutaCertificados, Session["usuario"].ToString(), (int)Session["sIdCliente"], oFacturas.Folio);
                }

                ///TODO
                /// Mandar a imprimir la de gastos de administración por renglones o por subtotales
                /// 1 = Renglones, 2 = Sub totales
                /// 

                //TODO: GT Paso 4: una vez generada una factura y segun como eligio el usuario se debe de hacer una factura de tipo Gasto administrativo o por sub totales A LA PAR DE LA FACTURA EMITIDA, es regla de negocio por eso por cada factura siempre existiran 2
                if (chkPanelGtosAdmon.Checked == true)
                {
                    if (rblGenerarFacturaTipo.SelectedValue == "1")
                    {
                        sFacturaAdicional = CrearFacturaGtoAdministrativo(sIdFolioFactura, lstDetalleFactura);
                        sFacturaAdicional2 = sFacturaAdicional.Replace(".xml", ".pdf");
                    }

                    if (rblGenerarFacturaTipo.SelectedValue == "2")
                    {
                        //Generar la factura agregandole el monto a la factura
                        sFacturaAdicional = CrearFacturaSubTotales(sIdFolioFactura, dSubtotal);
                        sFacturaAdicional2 = sFacturaAdicional.Replace(".xml", ".pdf");
                        dSubtotal = 0;
                    }
                }

                //Gurdar en la base de datos lo facturado por linea de credito
                foreach (MedNeg.Facturas.BlFacturacionDeLineas registro in lstTotalFacturadoPorLinea)
                {

                    oFacturacionRecetas.idLineaCredito = registro.IIdLineaCredito;
                    oFacturacionRecetas.Fecha = registro.DtFecha;
                    oFacturacionRecetas.Monto = registro.dMonto;
                    oblFacturas = new MedNeg.Facturas.BlFacturas();
                    //MedDAL.DAL.facturas_partida oFacturaPartida = new MedDAL.DAL.facturas_partida();

                    //Registrar el renglon de lo facturado
                    if(!oblFacturas.NuevoRegistroFacturacionReceta(oFacturacionRecetas))
                    {
                       //bRegistroFallido = true;
                    }
               }

                if (iTipo == 2)
                {
                    try
                    {
                        oblFacturas.CrearZip(new string[] { Server.MapPath("~/Archivos/FacturasElectronicasTimbradas"), Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFacturas.Folio + ".zip") }, Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFacturas.Folio + ".xml"), Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFacturas.Folio + ".pdf"), sFacturaAdicional, sFacturaAdicional2);

                        ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarFacturas(0);", true);

                        System.IO.FileInfo fFactura = new System.IO.FileInfo(Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFacturas.Folio + ".zip"));

                        Response.Clear();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + fFactura.Name);
                        Response.AddHeader("Content-Length", fFactura.Length.ToString());
                        Response.ContentType = "application/....";
                        Response.WriteFile(fFactura.FullName);
                        Response.End();
                    }
                    catch (Exception)
                    {
                        ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarExcepcion(0);", true);
                    }
                }
            }

            

        }

        /// <summary>
        /// Carga los detalles de la receta
        /// </summary>
        private void CargaReceta(string sFolio)
        {
            ////TODO: GT Paso 2: Cuando se recibe una receta para facturar

            //objeto que contiene la receta
            string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            MedDAL.DAL.recetas oReceta = new MedDAL.DAL.recetas();
            MedNeg.Recetas.BlRecetas oblReceta = new MedNeg.Recetas.BlRecetas();
            
            MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            MedDAL.Configuracion.DALConfiguracion odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();

            odalConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

                   
            /**************** Registrar la partida de la factura *****************/

            //Llenar los datos de la partida de la remisión
            oblReceta = new MedNeg.Recetas.BlRecetas();

            //Recuperar datos de la receta mediante el folio enviado
            oReceta = oblReceta.BuscarRecetaFolio(sFolio);

            //Recuperar la partida del pedido
            var oQuery = oblReceta.RecuperarPartidaRecetas(oReceta.idReceta);
            int iIndiceElemento;
            decimal dGastoAdministracion=0;

            //Recorrer el resultado y meterlo al datagridview
            foreach (MedDAL.DAL.recetas_partida oDetalle in oQuery)
            {
                //Buscar si ya existe el producto en la lista, si es así entonces solo se modifica la cantidad surtida,
                //de lo contrario entonces se crea un renglon nuevo y se incrementa el contador

                iIndiceElemento = lstDetalleReceta.FindIndex(det => det.iIdProducto == oDetalle.idProducto);
                if (iIndiceElemento > -1)
                {
                    lstDetalleReceta[iIndiceElemento].dCantidad += Convert.ToDecimal(oDetalle.CantidaSurtida);
                }
                else
                {
                    //Identificar que precio o monto se debe de enviar acorde a los filtros
                    //si es por gasto de admon relacionado al producto se toma el campo 9
                    if (chkGtoAdmon.Checked == true && rblGenerarFacturaTipo.SelectedValue=="1")
                        dGastoAdministracion = Convert.ToDecimal(oDetalle.productos.Campo9);
                    if (chkMonto.Checked == true && rblGenerarFacturaTipo.SelectedValue == "1")
                        dGastoAdministracion = Convert.ToDecimal(txbMonto.Text);
                    if (chkPorcentaje.Checked == true && rblGenerarFacturaTipo.SelectedValue == "1")
                        dGastoAdministracion = Convert.ToDecimal(oDetalle.Precio * (Convert.ToDecimal(txbPorcentaje.Text) / 100));
                    

                    MedNeg.Facturas.BlDetalleFacturaReceta oblDetallePartida = new MedNeg.Facturas.BlDetalleFacturaReceta(
                        Convert.ToInt32(oDetalle.idProducto),
                        oDetalle.productos.Clave1,
                        oDetalle.productos.Nombre,
                        Convert.ToDecimal(oDetalle.CantidaSurtida),
                        Convert.ToDecimal(oDetalle.productos.TasaIeps),
                        Convert.ToDecimal(oDetalle.productos.tipo_iva.Iva),
                        dGastoAdministracion,
                        Convert.ToDecimal(oDetalle.Precio),
                        "",
                        Convert.ToDecimal(0),
                        oDetalle.idLineaCredito,
                        oReceta.idReceta);
                    
                    lstDetalleReceta.Add(oblDetallePartida);
                    iContadorRenglones++;
                }


                //Saber si ya se va a mandar a facturar
                if (iContadorRenglones == odalConfiguracion.iNoMaxRenglonesFactura && cmbModoFactura.SelectedValue == "1")
                {
                    CrearFactura(lstDetalleReceta, 1);
                    
                    //Re iniciar contadores y listas
                    iContadorRenglones = 0;
                    lstDetalleReceta.Clear();
                }

            }

            //Si al terminar el foreach el contador de renglones es menor a odalConfiguracion.iNoMaxRenglonesFactura
            //quiere decir que quedan reglones por facturar es por eso que se requiere nuevamente esta validación
            if (iContadorRenglones < odalConfiguracion.iNoMaxRenglonesFactura && cmbModoFactura.SelectedValue == "1")
            {
                CrearFactura(lstDetalleReceta, 1);

                //Re iniciar contadores y listas
                iContadorRenglones = 0;
                lstDetalleReceta.Clear();
            }

            //JID Si el modo de factura es electronico, hace el proceso con la lista completa
            if (cmbModoFactura.SelectedValue == "2")
            {
                CrearFactura(lstDetalleReceta, 2);

                //Re iniciar contadores y listas
                iContadorRenglones = 0;
                lstDetalleReceta.Clear();
            }

      }

       

        protected void MostrarLista(object oQuery)
        {
            //MedNeg.Recetas.BlRecetas oblRecetas = new MedNeg.Recetas.BlRecetas();
            //var oQuery = oblRecetas.RecetasFacturarPorPrograma();

            try
            {
                dgvDatos.DataSource = oQuery;
                dgvDatos.DataKeyNames = new string[] { "idReceta" };
                dgvDatos.DataBind();
                //CargarCatalogo();
                if (dgvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    dgvDatos.EmptyDataText = "No existen recetas registradas aun";
                }
                else
                {
                    dgvDatos.EmptyDataText = "No existen recetas que coincidan con la búsqueda";
                }
                dgvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

        }

        protected void txbHasta_TextChanged(object sender, EventArgs e)
        {

        }

        protected void chkAlmacen_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAlmacen.Checked == true)
            {
                cmbAlmacenes.Enabled = true;
            }
            else
            {
                cmbAlmacenes.Enabled = false;
            }

                
        }

        protected void chkFecha_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFecha.Checked == true)
            {
                txbFechaDesde.Enabled = true;
                txbFechaHasta.Enabled = true;
                txbFolioDesde.Enabled = false;
                txbFolioHasta.Enabled = false;
                chkFolio.Checked = false;
                txbFolioDesde.Text="";
                txbFolioHasta.Text = "";
                txbFolioDesde.Focus();
            }
            else
            {
                txbFechaDesde.Enabled = false;
                txbFechaHasta.Enabled = false;
                txbFolioDesde.Enabled = false;
                txbFolioHasta.Enabled = false;
            }
        }

        protected void chkFolio_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFolio.Checked == true)
            {
                txbFechaDesde.Enabled = false;
                txbFechaHasta.Enabled = false;
                txbFolioDesde.Enabled = true;
                txbFolioHasta.Enabled = true;
                chkFecha.Checked = false;
                txbFechaDesde.Text="";
                txbFechaHasta.Text = "";
                txbFolioDesde.Focus();
            }
            else
            {
                txbFechaDesde.Enabled = false;
                txbFechaHasta.Enabled = false;
                txbFolioDesde.Enabled = false;
                txbFolioHasta.Enabled = false;
            }
        }
        /// <summary>
        /// Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //Buscar(txbBuscar.Text);
        }



        protected void dgvDatos_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (dgvDatos.Rows.Count > 0)
            {
                if (dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[2].Text == "1")
                    dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[2].Text = "Surtida";
                if (dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[2].Text == "2")
                {
                    dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[2].Text = "Parcial";
                    dgvDatos.Rows[dgvDatos.Rows.Count - 1].BackColor = System.Drawing.Color.Yellow;
                }
                if (dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[2].Text == "3")
                    dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[2].Text = "Cancelada";
                if (dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[2].Text == "4")
                    dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[2].Text = "Facturada";
           }
        }

        protected void imbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedIndex != -1)
            {
                //CargarFormulario(true);
                Session["accion"] = 2;
            }
            else
            {
                //CargarCatalogo();

            }
        }

        protected void imbEliminar_Click(object sender, EventArgs e)
        {
            if (pnlCatalogo.Visible && dgvDatos.SelectedIndex != -1)
            {
                //Eliminar((int)dgvDatos.SelectedValue);
                //MostrarLista();
                //CargarCatalogo();
            }
            else
            {
                //CargarCatalogo();
                //MostrarLista();
            }
        }

        protected void imbMostrar_Click(object sender, EventArgs e)
        {
            pnlCatalogoTotales.Visible=true;
            pnlFormulario.Visible = false;
            MostrarLista();
        }

        private void MostrarLista()
        {
            MedNeg.Facturas.BlFacturas oblFacturas = new MedNeg.Facturas.BlFacturas();
            var oQuery = oblFacturas.TotalFacturadoPorLineaCredito();
            Session["resultadoquery"] = oQuery;
            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Facturas.FacturasxRecetaView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "FuenteCuenta ASC";            

            try
            {
                
                dgvFacturacionTotales.DataSource = dv;
                dgvFacturacionTotales.DataKeyNames = new string[] { "idLineaCredito" };
                dgvFacturacionTotales.DataBind();
                //CargarCatalogo();
                if (dgvFacturacionTotales.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    dgvFacturacionTotales.EmptyDataText = "No existen facturas registradas aun";
                }
                else
                {
                    dgvFacturacionTotales.EmptyDataText = "No existen facturas que coincidan con la búsqueda";
                }
                dgvFacturacionTotales.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            } 
        }

        protected void imbCancelar_Click(object sender, EventArgs e)
        {

        }

        //protected void btnBuscar_Click() { }

        protected void imbNuevo_Click(object sender, EventArgs e)
        {
            pnlCatalogoTotales.Visible = false;
            pnlFormulario.Visible = true;
        }

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
                    //Nuevo();
                    break;
                case 2:
                    //Editar();
                    //Deshabilita();
                    Session["accion"] = 0;
                    break;
            }

        }

        /// <summary>
        /// Identificar las recetas que se desean facturar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFacturar_Click(object sender, EventArgs e)
        {
            //TODO: GT Paso 1: Facturacion electronica automatica (Empezar la explicacion por aqui para chorcho)
            foreach (GridViewRow fila in dgvDatos.Rows)
            {
                if (((CheckBox)fila.Cells[4].FindControl("CheckBox1")).Checked == true)
                {

                    CargaReceta(fila.Cells[0].Text);

                   
                    lblAviso.Text = "Proceso Terminado.";
                }
            }
        }

        protected void cmbTipoFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////Llenar las lineas de credito
            //if (cmbTipoFactura.SelectedValue == "2")
            //{
            //    lblLineaCredito.Visible = true;
            //    oblLineasCredito = new MedNeg.LineasCredito.BlLineasCredito();
            //    iqrLineasCreditos = oblLineasCredito.MostrarListaActivos();

            //    cmbLineasCredito.Enabled = true;
            //    cmbLineasCredito.Items.Clear();
            //    //cmbLineasCredito.DataSource = (IQueryable<MedDAL.DAL.lineas_creditos>)Session["lstLineasCreditosFacturas"];

            //    cmbLineasCredito.DataSource = iqrLineasCreditos;
            //    cmbLineasCredito.DataBind();
            //}
            //else
            //{
            //    cmbLineasCredito.Enabled = false;
            //    cmbLineasCredito.SelectedIndex = -1;
            //    lblLineaCredito.Visible = false;
            //}
        }

        /// <summary>
        /// Buscar las recetas por los diferentes filtros
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscarRec_Click(object sender, EventArgs e)
        {
            MedNeg.Recetas.BlRecetas oblRecetas = new MedNeg.Recetas.BlRecetas();
            //var oQuery = oblRecetas.RecetasFacturarPorPrograma();
           
            //Por fechas
            if (chkFecha.Checked==true)
            {
               var oQuery = oblRecetas.RecetasFacturarPorProgramaFecha(txbFechaDesde.Text, txbFechaHasta.Text);
               MostrarLista(oQuery);
            }

            //Por fechas con almacen
            if (chkFecha.Checked == true && chkAlmacen.Checked==true)
            {
                var oQuery = oblRecetas.RecetasFacturarPorProgramaFecha(txbFechaDesde.Text, txbFechaHasta.Text);
                MostrarLista(oQuery);
            }
            
            //Por Folio
            if (chkFolio.Checked == true)
            {
                var oQuery = oblRecetas.RecetasFacturarPorProgramaFolio(txbFolioDesde.Text, txbFolioHasta.Text);
                MostrarLista(oQuery);
            }

            //Por Folio y almacen
            if (chkFolio.Checked == true && chkAlmacen.Checked==true)
            {
                var oQuery = oblRecetas.RecetasFacturarPorProgramaFolio(txbFolioDesde.Text, txbFolioHasta.Text);
                MostrarLista(oQuery);
            }

            //Filtrado solo por almacen
            if (chkAlmacen.Checked == true && chkFolio.Checked == false && chkFecha.Checked == false)
            {
                var oQuery = oblRecetas.RecetasFacturarPorPrograma(Convert.ToInt32(cmbAlmacenes.SelectedValue));
                MostrarLista(oQuery);
            }
            
            //Sin filtros
            if (chkAlmacen.Checked == false && chkFolio.Checked == false && chkFecha.Checked == false)
            {
                var oQuery = oblRecetas.RecetasFacturarPorPrograma();
                MostrarLista(oQuery);
            }

            chkTodas.Visible = true;
            chkTodas.Checked = false;
        }

        /// <summary>
        /// Habilitar o deshabilitar los campos de los gastos administrativos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkPanelGtosAdmon_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPanelGtosAdmon.Checked == true)
            {
                //cmbTipoGasto.Enabled = true;
                chkGtoAdmon.Enabled = true;
                //cmbDatosOpcionales.Enabled = true;
                chkMonto.Enabled = true;
                //txbMonto.Enabled = true;
                chkPorcentaje.Enabled = true;
                //txbPorcentaje.Enabled = true;
                //txbDescripcion.Enabled = true;

            }
            else
            {
                //cmbTipoGasto.Enabled = false;
                chkGtoAdmon.Enabled = false;
                //cmbDatosOpcionales.Enabled = false;
                chkMonto.Enabled = false;
                txbMonto.Enabled = false;
                chkPorcentaje.Enabled = false;
                txbPorcentaje.Enabled = false;
                //txbDescripcion.Enabled = false;
            }
        }

        protected void chkGtoAdmon_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGtoAdmon.Checked == true)
            {
                //cmbDatosOpcionales.Enabled = true;

                chkMonto.Checked = false;
                txbMonto.Enabled = false;

                chkPorcentaje.Checked = false;
                txbPorcentaje.Enabled = false;
            }
        }

        protected void chkMonto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMonto.Checked == true)
            {
                chkGtoAdmon.Checked = false;
                //cmbDatosOpcionales.Enabled = false;

                txbMonto.Enabled = true;
                txbMonto.Focus();

                chkPorcentaje.Checked = false;
                txbPorcentaje.Enabled = false;
            }
        }

        protected void chkPorcentaje_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPorcentaje.Checked == true)
            {
                chkGtoAdmon.Checked = false;
                //cmbDatosOpcionales.Enabled = false;

                chkMonto.Checked = false;
                txbMonto.Enabled = false;

                txbPorcentaje.Enabled = true;
                txbPorcentaje.Focus();
            }
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
                
            }
            catch
            {
                txbCliente.Focus();
            }
        }


        /// <summary>
        /// Checkbox que controla si se seleccionan todas o no
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkTodas_CheckedChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow fila in dgvDatos.Rows)
            {
                if(chkTodas.Checked==true)
                    ((CheckBox)fila.Cells[4].FindControl("CheckBox1")).Checked=true;
                else
                    ((CheckBox)fila.Cells[4].FindControl("CheckBox1")).Checked = false;
            }
        }

        /// <summary>
        /// Radio buton list que gestiona si se muestra el text box para capturar el producto o servicio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rblGenerarFacturaTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblGenerarFacturaTipo.SelectedIndex == 0)
            {
                txbProductos.Enabled = false;
                txbProductos.Text = "";
            }
            if(rblGenerarFacturaTipo.SelectedIndex == 1)
            {
                txbProductos.Enabled = true;
                txbProductos_AutoCompleteExtender.Enabled = true;
                txbProductos.Focus();
            }
        }

        /// <summary>
        /// Evento del text change para recuper el id del producto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txbProductos_TextChanged(object sender, EventArgs e)
        {
            MedDAL.DAL.productos oProducto = new MedDAL.DAL.productos();
            MedNeg.Productos.BlProductos oblProducto = new MedNeg.Productos.BlProductos();

            oProducto = oblProducto.BuscarProductoNombre(txbProductos.Text);
            Session["sIdProductoFxR"] = oProducto.idProducto;
            txbCliente.Focus();
        }


        #region Reportes

        protected void CargarReporte()
        {
            pnlCatalogoTotales.Visible = false;
            pnlFormulario.Visible = false;
            //pnlReportes.Visible = true;

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsDataSet odsFacturasPorRecetas = new MedDAL.DataSets.dsDataSet();
            odsFacturasPorRecetas = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from lineas_creditos", "medicuriConnectionString", odsFacturasPorRecetas, "lineas_creditos");

            odsFacturasPorRecetas = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from FacturacionDeRecetas", "medicuriConnectionString", odsFacturasPorRecetas, "FacturacionDeRecetas");

            //GT 0179
            Session["campoaordenar"] = "";
            Session["sortfield"] = 0;  

            Session["dataset"] = odsFacturasPorRecetas;
            Session["reportdocument"] = "~\\rptReportes\\rptFacturasPorRecetas.rpt";
            Session["titulo"] = "Facturas Por Recetas";
            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));

            string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            MedDAL.Configuracion.DALConfiguracion objConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
            objConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

            rptReporte.SetDataSource(odsFacturasPorRecetas);

            //Saber si es general o se tiene un usuario seleccionado
            if (dgvDatos.SelectedIndex != -1)
            {
                Session["recordselection"] = "{lineas_creditos.idLineaCredito}=" + dgvDatos.SelectedDataKey.Values[0].ToString();
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
        protected void ObtenerReporte()
        {
            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
            rptReporte.SetDataSource((DataSet)Session["dataset"]);

            //string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            //MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            //MedDAL.Configuracion.DALConfiguracion objConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
            //objConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

            //crvReporte.Visible = true;
            //crvReporte.ReportSource = rptReporte;
            //crvReporte.PageZoomFactor = 100;
        }

        protected void imbImprimir_Click(object sender, EventArgs e)
        {
            CargarReporte();
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
        
        //protected void crvReporte_Drill(object source, CrystalDecisions.Web.DrillEventArgs e)
        //{
        //    ObtenerReporte();
        //}
        //protected void crvReporte_DrillDownSubreport(object source, CrystalDecisions.Web.DrillSubreportEventArgs e)
        //{
        //    ObtenerReporte();
        //}
        //protected void crvReporte_Navigate(object source, CrystalDecisions.Web.NavigateEventArgs e)
        //{
        //    ObtenerReporte();
        //    //crvReporte.PageZoomFactor = 50;
        //}
        //protected void crvReporte_Search(object source, CrystalDecisions.Web.SearchEventArgs e)
        //{
        //    ObtenerReporte();
        //    //crvReporte.PageZoomFactor = 50;
        //}
        //protected void crvReporte_ViewZoom(object source, CrystalDecisions.Web.ZoomEventArgs e)
        //{
        //    ObtenerReporte();
        //}
        //protected void crvReporte_DataBinding(object sender, EventArgs e)
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

        protected void dgvFacturacionTotales_Sorting(object sender, GridViewSortEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Facturas.FacturasxRecetaView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvFacturacionTotales.DataSource = oMaster.Sorting(e, ref oDireccion, dv);
            ViewState["direccionsorting"] = oDireccion;
            ViewState["sortexpression"] = e.SortExpression;
            dgvFacturacionTotales.DataBind();
        }

        protected void dgvFacturacionTotales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Facturas.FacturasxRecetaView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvFacturacionTotales.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "FuenteCuenta" : ViewState["sortexpression"].ToString(), dv, ref dgvFacturacionTotales, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            dgvFacturacionTotales.DataBind();
        }

        #endregion
    }
}