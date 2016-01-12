using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MedNeg.Inventarios;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Medicuri
{
    public partial class Movimientos : System.Web.UI.Page
    {
        ImageButton imbNuevo, imbEditar, imbEliminar, imbReportes, imbMostrar, imbAceptar, imbCancelar, imbImprimir;
        Label lblNuevo, lblEditar, lblEliminar;
        RadioButton rdbFiltro1, rdbFiltro2; 
        Button btnBuscar;
        TextBox txbBuscar;
        Label lblNombreModulo;
        BlInventarios blInventarios;
        MedNeg.Usuarios.BlUsuarios oblUsuario;
        MedNeg.RecetasPartidaFaltantes.BlRecetasPartidaFaltantes oblRecetasPartidaFaltantes;
        MedDAL.DAL.productos producto;

        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';

            blInventarios = new BlInventarios();
            oblRecetasPartidaFaltantes = new MedNeg.RecetasPartidaFaltantes.BlRecetasPartidaFaltantes();

            try
            {
                cPermiso = (char)htbPermisos["movimientos"];

                //Master.FindControl("rdbFiltro2").Visible = false;
                Master.FindControl("rdbFiltro3").Visible = false;
                Master.FindControl("btnEditar").Visible = false;
                //Master.FindControl("btnEliminar").Visible = false;

                imbNuevo = (ImageButton)Master.FindControl("imgBtnNuevo");
                imbNuevo.ImageUrl = "~/Icons/document_32.png";
                imbNuevo.Click += new ImageClickEventHandler(this.imgBtnEntradaSalida_Click);
                lblNuevo = (Label)Master.FindControl("lblNuevo");
                lblNuevo.Text = "Ent./Sal.";
                //imbEditar = (ImageButton)Master.FindControl("imgBtnEditar");
                //imbEditar.ImageUrl = "~/Icons/statistics_32.png";                
                //imbEditar.Click += new ImageClickEventHandler(this.imgBtnFisico_Click);
                imbEliminar = (ImageButton)Master.FindControl("imgBtnEliminar");
                imbEliminar.OnClientClick = "return confirm('¿Está usted seguro de que desea cancelar el movimiento?');";
                imbEliminar.Click += new ImageClickEventHandler(imbCancelarMovimiento_Click);
                lblEliminar = (Label)Master.FindControl("lblEliminar");
                lblEliminar.Text = "Canc.Mov.";                
                imbMostrar = (ImageButton)Master.FindControl("imgBtnMostrar");
                imbMostrar.Click += new ImageClickEventHandler(this.imgBtnMostrar_Click);
                imbImprimir = (ImageButton)Master.FindControl("imgBtnImprimir");
                imbImprimir.Click += new ImageClickEventHandler(this.imbImprimir_Click);
                imbReportes = (ImageButton)Master.FindControl("imgBtnReportes");
                imbReportes.Click += new ImageClickEventHandler(this.imgBtnReportes_Click);
                imbAceptar = (ImageButton)Master.FindControl("imgBtnAceptar");
                imbAceptar.Click += new ImageClickEventHandler(this.imbAceptar_Click);
                imbAceptar.ValidationGroup = "Almacenes";
                imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
                imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
                rdbFiltro1 = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbFiltro1.Text = "Concepto";
                rdbFiltro2 = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbFiltro2.Text = "Pedimento";
                //rdbClave = (RadioButton)Master.FindControl("rdbFiltro2");
                //rdbClave.Text = "Clave";
                //rdbNombre = (RadioButton)Master.FindControl("rdbFiltro3");
                //rdbNombre.Text = "Nombre";
                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
                txbBuscar = (TextBox)Master.FindControl("txtBuscar");
                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "Movimientos";
                
                

                //imgBtnImprimir.Click += new ImageClickEventHandler(this.imbImprimir_Click);
                //imgBtnAceptar.Click += new ImageClickEventHandler(imbAceptar_Click);
                //imgBtnAceptar.ValidationGroup = ""; //ojo aqui
                //imgBtnCancelar.Click += new ImageClickEventHandler(imbCancelar_Click);
                //imgBtnReportes.Click += new ImageClickEventHandler(this.imgBtnReportes_Click);

                txtEntSalArticulo.TextChanged += new EventHandler(txtEntSalArticulo_TextChanged);
                txtEntSalDescripcionArticulo.TextChanged += new EventHandler(txtEntSalDescripcionArticulo_TextChanged);
                txtEntSalCantidadArticulo.TextChanged += new EventHandler(txtEntSalCantidadArticulo_TextChanged);

                if (!IsPostBack)
                {
                    EntradaSalida.Visible = false;                    
                    pnlFiltroReportes.Visible = false;
                    pnlCatalogo.Visible = false;
                    //pnlReportes.Visible = false;

                    Session["reporteactivo"] = 0;
                    Session["reportdocument"] = "";
                    Session["titulo"] = "";
                    Session["resultadoquery"] = "";
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;

                    #region entradaSalida
                    ddlEntSalConceptoMov.Items.Add("Entrada");
                    ddlEntSalConceptoMov.Items.Add("Salida");

                    ddlEntSalTiposMovimiento.DataTextField = "TipoMovimiento";
                    ddlEntSalTiposMovimiento.DataValueField = "idTipoMovimiento";
                    ddlEntSalTiposMovimiento.DataSource = blInventarios.ObtenerTiposMovimientos();
                    ddlEntSalTiposMovimiento.DataBind();

                    ddlEntSalAlmacenes.DataTextField = "Nombre";
                    ddlEntSalAlmacenes.DataValueField = "idAlmacen";
                    //JID 21/09/2011 Se obtiene o el almacen del usuario o todos los almacenes.
                    MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
                    if (oUsuario.FiltradoActivado == true)
                    {
                        ddlEntSalAlmacenes.DataSource = blInventarios.BuscarAlmacenesFiltrado(oUsuario.idAlmacen);                        
                    }
                    else
                    {
                        ddlEntSalAlmacenes.DataSource = blInventarios.ObtenerAlmacenes();                        
                    }                    
                    ddlEntSalAlmacenes.DataBind();

                    //ddlEntSalAlmacenSalida.DataTextField = "RazonSocial";
                    ddlEntSalAlmacenSalida.DataTextField = "Nombre";
                    ddlEntSalAlmacenSalida.DataValueField = "idAlmacen";
                    ddlEntSalAlmacenSalida.DataSource = blInventarios.ObtenerAlmacenes();
                    ddlEntSalAlmacenSalida.DataBind();

                    ddlEntSalLineasDeCredito.DataTextField = "InstitucionEmisora";//ojo aqui
                    ddlEntSalLineasDeCredito.DataValueField = "idLineaCredito";
                    ddlEntSalLineasDeCredito.DataSource = blInventarios.ObtenerLineasDeCredito();
                    ddlEntSalLineasDeCredito.DataBind();

                    inicializaGuiEntSal(false);

                    Session["invEntSalLsProductos"] = new List<MedNeg.Inventarios.Producto>();
                    grvEntSalArticulos.DataSource = (List<MedNeg.Inventarios.Producto>)Session["invEntSalLsProductos"];
                    grvEntSalArticulos.DataBind();
                    #endregion                                        
                }
            }
            catch (NullReferenceException)
            {
                if (!ClientScript.IsStartupScriptRegistered("alertsession"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alertsession", "alertarSesion();", true);
                }
                EntradaSalida.Visible = false;                
                pnlFiltroReportes.Visible = false;
                pnlCatalogo.Visible = false;
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }

        void imbCancelar_Click(object sender, ImageClickEventArgs e)
        {
            EntradaSalida.Visible = false;            
            pnlFiltroReportes.Visible = false;
            pnlCatalogo.Visible = false;
        }

        void imbAceptar_Click(object sender, ImageClickEventArgs e)
        {

            if (EntradaSalida.Visible)
            {
                FinalizaEntradaSalida();
            }            
        }


        void imbCancelarMovimiento_Click(object sender, ImageClickEventArgs e)
        {
            if (pnlCatalogo.Visible)
            {
                int idMovimiento = (int)gdvDatos.SelectedValue;
                MedDAL.DAL.inventario oInventario = blInventarios.BuscarPorId(idMovimiento);
                if (oInventario.Concepto.ToUpper() == "ENTRADA")
                {
                    oInventario.Concepto = "Entrada Cancelada";
                }
                else if (oInventario.Concepto.ToUpper() == "SALIDA")
                {
                    oInventario.Concepto = "Salida Cancelada";
                }
                blInventarios = new BlInventarios();
                if (blInventarios.EditarRegistro(oInventario))
                {
                    //GestionarExistenciasPorPartida
                    decimal dPrecioTotal = 0; 
                    foreach (MedDAL.DAL.inventario_partida oInventarioPartida in oInventario.inventario_partida)
                    {
                        blInventarios.GestionarCancelacion(oInventario.idAlmacen, oInventarioPartida.idProducto, oInventarioPartida.Lote, oInventarioPartida.NoSerie, Convert.ToDecimal(oInventarioPartida.Cantidad), oInventario.Concepto);
                        dPrecioTotal += Convert.ToDecimal(oInventarioPartida.Cantidad) * oInventarioPartida.Precio;
                    }
                    //Si el concepto era de entrada, hay que ponerle el credito a la linea
                    if (oInventario.Concepto == "Entrada Cancelada")
                    {
                        blInventarios.AumentarCreditoALinea(oInventario.idLineaCredito, dPrecioTotal);
                    }
                    //Checar si los lotes quedan en 0 y eliminarlos                    
                    Buscar(txbBuscar.Text);
                 }
            }
            else
            {
                EntradaSalida.Visible = false;
                pnlFiltroReportes.Visible = false;
                pnlCatalogo.Visible = true;
            }
        }

        #region entradaSalida

        protected void grvEntSalArticulos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((List<MedNeg.Inventarios.Producto>)Session["invEntSalLsProductos"]).RemoveAt(grvEntSalArticulos.SelectedIndex);
            grvEntSalArticulos.DataSource = (List<MedNeg.Inventarios.Producto>)Session["invEntSalLsProductos"];
            grvEntSalArticulos.DataBind();
        }

        protected void ddlEntSalConceptoMov_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlEntSalConceptoMov.SelectedValue.Equals("Salida"))
            {
                lblLineaCredito.Visible = ddlEntSalLineasDeCredito.Visible = false;
                lblAlmacenSalida.Visible = ddlEntSalAlmacenSalida.Visible = true;
            }
            else
            {
                lblLineaCredito.Visible = ddlEntSalLineasDeCredito.Visible = true;
                lblAlmacenSalida.Visible = ddlEntSalAlmacenSalida.Visible = false;
            }
        }


        protected void imbEntSalAgregarDetalle_Click(object sender, ImageClickEventArgs e)
        {
            agregaProductoaDgProductos();
        }

        void txtEntSalCantidadArticulo_TextChanged(object sender, EventArgs e)
        {
            //agregaProductoaDgProductos();
        }

        void txtEntSalArticulo_TextChanged(object sender, EventArgs e)
        {
            buscarProducto(1);
        }

        void txtEntSalDescripcionArticulo_TextChanged(object sender, EventArgs e)
        {
            buscarProducto(2);
        }

        void buscarProducto(int source)
        {
            if (source == 1)
            {
                Session["invEntSalProducto"] = producto = blInventarios.buscarProducto(txtEntSalArticulo.Text);
            }
            else if (source == 2)
            {
                Session["invEntSalProducto"] = producto = blInventarios.buscarProductoPorNombre(txtEntSalDescripcionArticulo.Text);
            }


            if (producto != null)
            {
                lbEntSalAvisos.Text = "";
                txtEntSalDescripcionArticulo.Text = producto.Nombre;
                txtEntSalArticulo.Text = producto.Clave1;
                //txtEntSalCostoArticulo.Text = blInventarios.buscarCostoProducto(int.Parse(ddlEntSalAlmacenes.SelectedValue), producto.idProducto);

                if (producto.ManejaLote.Equals("1"))
                {
                    ddlEntSalLotes.Enabled = true;
                    ddlEntSalLotes.DataSource = blInventarios.buscarLotesProducto(producto.idProducto, int.Parse(ddlEntSalAlmacenes.SelectedValue));
                    ddlEntSalLotes.DataBind();
                    ddlEntSalLotes.Items.Add("No existe...");
                    if (ddlEntSalLotes.Items.Count == 1)
                        txtEntSalLotes.Visible = true;
                    else if (ddlEntSalLotes.Items.Count > 1)
                    {
                        txtEntSalFechaCadArt.Text = blInventarios.BuscarFechaCaducidad(producto.idProducto, int.Parse(ddlEntSalAlmacenes.SelectedValue), ddlEntSalLotes.SelectedValue).ToShortDateString();
                    }
                }
                else
                {
                    ddlEntSalLotes.Enabled = txtEntSalLotes.Visible = false;
                }

                if (producto.ManejaSeries.Equals("1"))
                {
                    ddlEntSalSeries.Enabled = true;
                    ddlEntSalSeries.DataSource = blInventarios.buscarSeriesProducto(producto.idProducto, int.Parse(ddlEntSalAlmacenes.SelectedValue));
                    ddlEntSalSeries.DataBind();
                    ddlEntSalSeries.Items.Add("No existe...");
                    if (ddlEntSalSeries.Items.Count == 1)
                        txtEntSalSeries.Visible = true;
                }
                else
                {
                    ddlEntSalSeries.Enabled = txtEntSalSeries.Visible = false;
                }

                if (ddlEntSalLotes.Enabled)
                    ddlEntSalLotes.Focus();
                else
                    if (ddlEntSalSeries.Enabled)
                        ddlEntSalSeries.Focus();
                    else
                        txtEntSalCostoArticulo.Focus();
            }
            else
            {
                lbEntSalAvisos.Text = "Artículo no encontrado";
                txtEntSalArticulo.Text = String.Empty;
                Session["invEntSalProducto"] = null;
            }
        }

        private void agregaProductoaDgProductos()
        {
            if (Session["invEntSalProducto"] != null)
            {
                string lote = string.Empty;
                string serie = string.Empty;

                if (((MedDAL.DAL.productos)Session["invEntSalProducto"]).ManejaLote.Equals("1"))
                {
                    if (txtEntSalLotes.Visible)
                        lote = txtEntSalLotes.Text;
                    else
                        lote = ddlEntSalLotes.SelectedValue;
                }

                if (((MedDAL.DAL.productos)Session["invEntSalProducto"]).ManejaSeries.Equals("1"))
                {
                    if (txtEntSalSeries.Visible)
                        serie = txtEntSalSeries.Text;
                    else
                        serie = ddlEntSalSeries.SelectedValue;
                }



                ((List<MedNeg.Inventarios.Producto>)Session["invEntSalLsProductos"]).Add(
                                                                                        new Producto(
                                                                                                    (MedDAL.DAL.productos)Session["invEntSalProducto"],
                                                                                                     txtEntSalArticulo.Text,
                                                                                                     txtEntSalDescripcionArticulo.Text,
                                                                                                     decimal.Parse(txtEntSalCantidadArticulo.Text),
                                                                                                     lote,
                                                                                                     serie,
                                                                                                     decimal.Parse(txtEntSalCostoArticulo.Text),
                                                                                                     DateTime.Parse(txtEntSalFechaCadArt.Text)
                                                                                                     ));
                grvEntSalArticulos.DataSource = (List<MedNeg.Inventarios.Producto>)Session["invEntSalLsProductos"];
                grvEntSalArticulos.DataBind();
            }
            else
            {
                lbEntSalAvisos.Text = "Artículo no encontrado";
            }

            inicializaGuiEntSal(true);
        }

        public void inicializaGuiEntSal(bool setFocustxtEntSalArticulo)
        {
            txtEntSalCostoArticulo.Text = txtEntSalArticulo.Text = txtEntSalDescripcionArticulo.Text = txtEntSalCantidadArticulo.Text =
               txtEntSalLotes.Text = txtEntSalSeries.Text = string.Empty;
            ddlEntSalSeries.Enabled = txtEntSalSeries.Visible =
              ddlEntSalLotes.Enabled = txtEntSalLotes.Visible = false;
            if (setFocustxtEntSalArticulo)
                txtEntSalArticulo.Focus();
        }

        public void LimpiarPantallaEntradaSalida() 
        {
            grvEntSalArticulos.DataSource = null;
            grvEntSalArticulos.DataBind();

            ddlEntSalConceptoMov.SelectedIndex = 0;
            ddlEntSalTiposMovimiento.SelectedIndex = 0;
            ddlEntSalAlmacenes.SelectedIndex = 0;
            ddlEntSalAlmacenSalida.SelectedIndex = 0;
            ddlEntSalLineasDeCredito.SelectedIndex = 0;
            ddlEntSalLotes.Items.Clear();
            ddlEntSalSeries.Items.Clear();
            txbPedimento.Text = "";
            txtEntSalComentarios.Text = "";
            txtEntSalArticulo.Text = "";
            txtEntSalDescripcionArticulo.Text = "";
            txtEntSalLotes.Text = "";
            txtEntSalSeries.Text = "";
            txtEntSalFechaCadArt.Text = "";
            txtEntSalCostoArticulo.Text = "";
            txtEntSalCantidadArticulo.Text = "";
        }

        public void FinalizaEntradaSalida()
        {
            if (grvEntSalArticulos.Rows.Count > 0)
            {
                decimal costoTotal = 0M;
                foreach (Producto p in ((List<MedNeg.Inventarios.Producto>)Session["invEntSalLsProductos"]))
                {
                    costoTotal += p.costo;
                }
                if (costoTotal <= blInventarios.BuscarCantCreditoLineaCredito(int.Parse(ddlEntSalLineasDeCredito.SelectedValue)))
                {
                    try
                    {
                        int idAlmacen = int.Parse(ddlEntSalAlmacenes.SelectedValue);
                        foreach (Producto p in ((List<MedNeg.Inventarios.Producto>)Session["invEntSalLsProductos"]))
                        {
                            blInventarios.GestionarExistenciaProducto(idAlmacen, p.dbProducto.idProducto, p.lote, p.serie, p.fechaCaducidad, p.cantidad, ddlEntSalConceptoMov.SelectedValue, Convert.ToInt32(ddlEntSalLineasDeCredito.SelectedValue.ToString()));
                        }

                        MedDAL.DAL.inventario inventario = new MedDAL.DAL.inventario();
                        inventario.idLineaCredito = int.Parse(ddlEntSalLineasDeCredito.SelectedValue);
                        inventario.idTipoMovimiento = int.Parse(ddlEntSalTiposMovimiento.SelectedValue);
                        inventario.idAlmacen = idAlmacen;
                        inventario.Observaciones = txtEntSalComentarios.Text;
                        inventario.Fecha = DateTime.Now;
                        inventario.Concepto = ddlEntSalConceptoMov.SelectedValue;
                        inventario.Pedimento = txbPedimento.Text;
                        if (inventario.Concepto == "Salida")
                        {
                            inventario.idAlmacenSalida = int.Parse(ddlEntSalAlmacenSalida.SelectedValue);
                        }

                        List<MedDAL.DAL.inventario_partida> lpartida = new List<MedDAL.DAL.inventario_partida>(); ;
                        MedDAL.DAL.inventario_partida partida;
                        costoTotal = 0M;
                        foreach (Producto p in ((List<MedNeg.Inventarios.Producto>)Session["invEntSalLsProductos"]))
                        {
                            partida = new MedDAL.DAL.inventario_partida();
                            partida.idProducto = p.dbProducto.idProducto;
                            partida.Cantidad = p.cantidad.ToString();
                            partida.Precio = p.costo;
                            partida.Lote = p.lote;
                            partida.NoSerie = p.serie;

                            costoTotal += p.costo;
                            lpartida.Add(partida);
                        }

                        blInventarios.agregarRegistroInventario(inventario, lpartida);
                        
                        foreach (MedDAL.DAL.inventario_partida oInventarioPartida in lpartida)
                        {
                            if (!blInventarios.BuscarAlmacenProductoStocks(idAlmacen, oInventarioPartida.idProducto))
                            {
                                MedDAL.DAL.productos_almacen_stocks oProductoAlmacenStock = new MedDAL.DAL.productos_almacen_stocks();
                                oProductoAlmacenStock.idAlmacen = idAlmacen;
                                oProductoAlmacenStock.idProducto = oInventarioPartida.idProducto;
                                oProductoAlmacenStock.StockMax = 0;
                                oProductoAlmacenStock.StockMin = 0;
                                blInventarios.NuevoProductosAlmacenStock(oProductoAlmacenStock);
                            }

                            List<MedDAL.DAL.recetas_partida_faltantes> lstRetecasPartidaFaltantes = oblRecetasPartidaFaltantes.BuscarPorProductoAlmacen(oInventarioPartida.idProducto, idAlmacen);

                            if (lstRetecasPartidaFaltantes.Count > 0)
                            {
                                foreach (MedDAL.DAL.recetas_partida_faltantes oRecetaPartidaFaltante in lstRetecasPartidaFaltantes)
                                {
                                    oblRecetasPartidaFaltantes.EliminarRegistro(oRecetaPartidaFaltante);
                                }
                            }
                        }

                        if (ddlEntSalConceptoMov.SelectedValue.Equals("Entrada"))
                            blInventarios.DescontarCreditoALinea(int.Parse(ddlEntSalLineasDeCredito.SelectedValue), costoTotal);

                        lbEntSalAvisos.Text = "Proceso finalizado con éxito";

                        Session["invEntSalLsProductos"] = new List<MedNeg.Inventarios.Producto>();

                        LimpiarPantallaEntradaSalida();                        
                    }
                    catch (Exception ex)
                    {
                        lbEntSalAvisos.Text = ex.Message;
                    }
                }
                else
                    lbEntSalAvisos.Text = "Fondos insuficientes en Línea de crédito: " + ddlEntSalLineasDeCredito.SelectedItem.Text;
            }
            else
            {
                if (ddlEntSalConceptoMov.SelectedIndex == 0)
                {
                    lbEntSalAvisos.Text = "La entrada no tiene productos registrados";
                }
                else if (ddlEntSalConceptoMov.SelectedIndex == 1)
                {
                    lbEntSalAvisos.Text = "La salida no tiene productos registrados";
                }
            }
        }

        protected void ddlEntSalLotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtEntSalLotes.Visible = ddlEntSalLotes.SelectedValue.Equals("No existe...");
            if (ddlEntSalLotes.Items.Count == 1 || ddlEntSalLotes.SelectedValue.Equals("No existe..."))
                txtEntSalLotes.Visible = true;
            else if (ddlEntSalLotes.Items.Count > 1 && !ddlEntSalLotes.SelectedValue.Equals("No existe..."))
            {
                txtEntSalFechaCadArt.Text = blInventarios.BuscarFechaCaducidad(((MedDAL.DAL.productos)Session["invEntSalProducto"]).idProducto, int.Parse(ddlEntSalAlmacenes.SelectedValue), ddlEntSalLotes.SelectedValue).ToShortDateString();
                txtEntSalLotes.Visible = false;
            }            
        }

        protected void ddlEntSalSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEntSalSeries.Visible = ddlEntSalSeries.SelectedValue.Equals("No existe...");
        }

        #endregion

        #region Mostrar

        protected void Buscar(string sCadena)
        {
            int iTipo = 1;
            if (rdbFiltro1.Checked)
            {
                iTipo = 1;
            }
            else if (rdbFiltro2.Checked)
            {
                iTipo = 2;
            }
            /*else if (rdbFiltro3.Checked)
            {
                iTipo = 3;
            }*/

            oblUsuario = new MedNeg.Usuarios.BlUsuarios();

             MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
            oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
            if (oUsuario.FiltradoActivado == true)
            {
                var oQuery = blInventarios.Buscar(sCadena, iTipo, oUsuario.idAlmacen);
                Session["resultadoquery"] = oQuery;
            }
            else
            {
                var oQuery = blInventarios.Buscar(sCadena, iTipo);
                Session["resultadoquery"] = oQuery;
            }
            

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Inventarios.MovimientosView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Concepto ASC";            

            try
            {
                gdvDatos.DataSource = dv;
                //gdvDatos.DataKeyNames = new string[] { "idEstado" };
                gdvDatos.DataBind();
                if (gdvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    gdvDatos.EmptyDataText = "No existen registros aun";
                }
                else
                {
                    gdvDatos.EmptyDataText = "No existen registros que coincidan con la búsqueda";
                }
                gdvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        #endregion
                
        protected void imgBtnEntradaSalida_Click(object sender, ImageClickEventArgs e)
        {            
            EntradaSalida.Visible = true;
            pnlCatalogo.Visible = false;
            pnlFiltroReportes.Visible = false;

            MedDAL.DAL.inventario oInventario = blInventarios.ObtenerUltimoMovimiento();
            string sPedimento = oInventario != null ? oInventario.Pedimento : "";
            txbPedimento.Text = sPedimento != "" ? (Convert.ToInt32(oInventario.Pedimento) + 1).ToString() : "1"; 
        }

        protected void imgBtnMostrar_Click(object sender, ImageClickEventArgs e)
        {            
            EntradaSalida.Visible = false;
            pnlCatalogo.Visible = true;
            pnlFiltroReportes.Visible = false;
            Buscar("");
            gdvDatos.SelectedIndex = -1;

            ////0175 GT
            //ConfigurarMenuBotones(true, true, false, true, false, true, true, true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {            
            EntradaSalida.Visible = false;
            pnlCatalogo.Visible = true;
            pnlFiltroReportes.Visible = false;
            Buscar(txbBuscar.Text);
            gdvDatos.SelectedIndex = -1;
        }

        protected void imgBtnFisico_Click(object sender, ImageClickEventArgs e)
        {
            //EntradaSalida.Visible = false;
            //CambioPrecios.Visible = false;
            //InventarioFísico.Visible = true;
            //pnlCatalogo.Visible = false;
            //pnlFiltroReportes.Visible = false;
            //cargaDdlInvFisico();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            FinalizaEntradaSalida();
        }
              
        protected void CargarReporte()
        {
            EntradaSalida.Visible = false;            
            pnlCatalogo.Visible = false;
            pnlFiltroReportes.Visible = false;
            //pnlReportes.Visible = true;

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
            oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());

            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from VistaMovimientos", "medicuriConnectionString", odsDataSet, "VistaMovimientos");
            //odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
            //odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
            //odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "inventario") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario", "medicuriConnectionString", odsDataSet, "inventario");
            //odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario_partida", "medicuriConnectionString", odsDataSet, "inventario_partida");
            //odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
            //odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from MovimientoSalida", "medicuriConnectionString", odsDataSet, "MovimientoSalida");

            //GT 0179
            Session["tablaordenar"] = "VistaMovimientos";
            Session["campoaordenar"] = "NombreAlmacen";
            Session["sortfield"] = 0;
            Session["recordselection"] = "";

            Session["dataset"] = odsDataSet;
            Session["reportdocument"] = "~\\rptReportes\\Movimientos\\rptMovimientos.rpt";
            Session["titulo"] = "Movimientos";
            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));

            /*if (gdvDatos.SelectedIndex != -1)
            {
                rptReporte.RecordSelectionFormula = "{inventario.idInventario}=" + gdvDatos.SelectedDataKey.Values[0].ToString();
            }*/

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
        protected void ObtenerReporte()
        {
            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
            rptReporte.SetDataSource((DataSet)Session["dataset"]);
            //crvReporte.Visible = true;
            //crvReporte.ReportSource = rptReporte;
            //crvReporte.PageZoomFactor = 100;
        }
        protected void imbImprimir_Click(object sender, EventArgs e)
        {
            CargarReporte();
        }

        #region Reportes

        public void CargarListaReportes()
        {
            ListBox lsbReportes = (ListBox)frReportes.FindControl("lsbSeleccionf");
            frReportes.LimpiarPaneles();
            lsbReportes.SelectedIndex = -1;
            lsbReportes.Items.Clear();
            if (Server.MapPath("~\\rptReportes\\Movimientos\\rptMovimientos.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de movimientos");
            }
        }

        protected void imgBtnReportes_Click(object sender, EventArgs e)
        {
            //GT0175
            //ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
            EntradaSalida.Visible = false;            
            pnlCatalogo.Visible = false;
            pnlFiltroReportes.Visible = true;
            CargarListaReportes();
        }
       
        #endregion

        #region SortingPaging

        protected void gdvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Inventarios.MovimientosView>)Session["resultadoquery"];
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
            var result = (IQueryable<MedDAL.Inventarios.MovimientosView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            gdvDatos.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Concepto" : ViewState["sortexpression"].ToString(), dv, ref gdvDatos, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            gdvDatos.DataBind();
        }

        #endregion
    }
}