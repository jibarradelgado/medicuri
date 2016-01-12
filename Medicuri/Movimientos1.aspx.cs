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
        BlInventarios blInventarios;
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
                imgBtnImprimir.Click += new ImageClickEventHandler(this.imbImprimir_Click);
                imgBtnAceptar.Click += new ImageClickEventHandler(imbAceptar_Click);
                imgBtnAceptar.ValidationGroup = ""; //ojo aqui
                imgBtnCancelar.Click += new ImageClickEventHandler(imbCancelar_Click);
                imgBtnReportes.Click += new ImageClickEventHandler(this.imgBtnReportes_Click);

                txtEntSalArticulo.TextChanged += new EventHandler(txtEntSalArticulo_TextChanged);
                txtEntSalDescripcionArticulo.TextChanged += new EventHandler(txtEntSalDescripcionArticulo_TextChanged);
                txtEntSalCantidadArticulo.TextChanged += new EventHandler(txtEntSalCantidadArticulo_TextChanged);

                if (!IsPostBack)
                {
                    EntradaSalida.Visible = false;
                    CambioPrecios.Visible = false;
                    InventarioFísico.Visible = false;
                    pnlFiltroReportes.Visible = false;
                    pnlCatalogo.Visible = false;
                    //pnlReportes.Visible = false;

                    Session["reporteactivo"] = 0;
                    Session["reportdocument"] = "";
                    Session["titulo"] = "";

                    #region entradaSalida
                    ddlEntSalConceptoMov.Items.Add("Entrada");
                    ddlEntSalConceptoMov.Items.Add("Salida");

                    ddlEntSalTiposMovimiento.DataTextField = "TipoMovimiento";
                    ddlEntSalTiposMovimiento.DataValueField = "idTipoMovimiento";
                    ddlEntSalTiposMovimiento.DataSource = blInventarios.ObtenerTiposMovimientos();
                    ddlEntSalTiposMovimiento.DataBind();

                    ddlEntSalAlmacenes.DataTextField = "Nombre";
                    ddlEntSalAlmacenes.DataValueField = "idAlmacen";
                    ddlEntSalAlmacenes.DataSource = blInventarios.ObtenerAlmacenes();
                    ddlEntSalAlmacenes.DataBind();

                    //ddlEntSalProveedor.DataTextField = "RazonSocial";
                    ddlEntSalProveedor.DataTextField = "Nombre";
                    ddlEntSalProveedor.DataValueField = "IdProveedor";
                    ddlEntSalProveedor.DataSource = blInventarios.ObtenerProveedores();
                    ddlEntSalProveedor.DataBind();

                    ddlEntSalLineasDeCredito.DataTextField = "InstitucionEmisora";//ojo aqui
                    ddlEntSalLineasDeCredito.DataValueField = "idLineaCredito";
                    ddlEntSalLineasDeCredito.DataSource = blInventarios.ObtenerLineasDeCredito();
                    ddlEntSalLineasDeCredito.DataBind();

                    inicializaGuiEntSal(false);

                    Session["invEntSalLsProductos"] = new List<MedNeg.Inventarios.Producto>();
                    grvEntSalArticulos.DataSource = (List<MedNeg.Inventarios.Producto>)Session["invEntSalLsProductos"];
                    grvEntSalArticulos.DataBind();
                    #endregion

                    #region cambioPrecios
                    cargaDdlCambioPrecios();

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
                CambioPrecios.Visible = false;
                InventarioFísico.Visible = false;
                pnlFiltroReportes.Visible = false;
                pnlCatalogo.Visible = false;
                DeshabilitarControles(this);
                DeshabilitarControles();
            }

        }

        public void DeshabilitarControles(Control c)
        {
            if (c is TextBox)
            {
                ((TextBox)c).Enabled = false;
            }
            else if (c is Button)
            {
                ((Button)c).Enabled = false;
            }
            else if (c is DropDownList)
            {
                ((DropDownList)c).Enabled = false;
            }
            else if (c is CheckBox)
            {
                ((CheckBox)c).Enabled = false;
            }
            else if (c is ListBox)
            {
                ((ListBox)c).Enabled = false;
            }
            else if (c is ImageButton)
            {
                ((ImageButton)c).Enabled = false;
            }
            foreach (Control ctrl in c.Controls)
            {
                DeshabilitarControles(ctrl);
            }
        }

        public void DeshabilitarControles()
        {            
            this.imgBtnAceptar.Enabled = false;
            this.imgBtnCancelar.Enabled = false;            
            this.imgBtnImprimir.Enabled = false;
            this.imgBtnMostrar.Enabled = false;            
            this.imgBtnPrecios.Enabled = false;
            this.imgBtnReportes.Enabled = false;
            this.imgBtnEntradaSalida.Enabled = false;
            this.imgBtnFisico.Enabled = false;
        }

        void imbCancelar_Click(object sender, ImageClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        void imbAceptar_Click(object sender, ImageClickEventArgs e)
        {

            if (EntradaSalida.Visible)
            {
                FinalizaEntradaSalida();
            }
            else if (CambioPrecios.Visible)
            {
                FinalizaCambioPrecios();
            }
            else if (InventarioFísico.Visible)
            {
                FinalizaInventarioFisico();
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
                Label4.Visible = Label5.Visible = ddlEntSalProveedor.Visible = ddlEntSalLineasDeCredito.Visible = false;
            }
            else
            {
                Label4.Visible = Label5.Visible = ddlEntSalProveedor.Visible = ddlEntSalLineasDeCredito.Visible = true;
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
                                                                                                     DateTime.Parse( txtEntSalFechaCadArt.Text)
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
           txtEntSalCostoArticulo.Text= txtEntSalArticulo.Text = txtEntSalDescripcionArticulo.Text = txtEntSalCantidadArticulo.Text =
              txtEntSalLotes.Text = txtEntSalSeries.Text = string.Empty;
            ddlEntSalSeries.Enabled = txtEntSalSeries.Visible =
              ddlEntSalLotes.Enabled = txtEntSalLotes.Visible = false;
            if (setFocustxtEntSalArticulo)
                txtEntSalArticulo.Focus();
        }

        public void FinalizaEntradaSalida()
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
                        blInventarios.GestionarExistenciaProducto(idAlmacen, p.dbProducto.idProducto, p.lote, p.serie,p.fechaCaducidad, p.cantidad, ddlEntSalConceptoMov.SelectedValue,Convert.ToInt32(ddlEntSalLineasDeCredito.SelectedValue.ToString()));
                    }

                    MedDAL.DAL.inventario inventario = new MedDAL.DAL.inventario();
                    inventario.idLineaCredito = int.Parse(ddlEntSalLineasDeCredito.SelectedValue);
                    inventario.idTipoMovimiento = int.Parse(ddlEntSalTiposMovimiento.SelectedValue);
                    inventario.idAlmacen = idAlmacen;
                    inventario.Observaciones = txtEntSalComentarios.Text;
                    inventario.Fecha = DateTime.Now;
                    inventario.Concepto = ddlEntSalConceptoMov.SelectedValue;
                    inventario.Pedimento = txbPedimento.Text;

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

                    //Codigo Renard, para que funcione el modulo de inventarios. 
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
                }
                catch (Exception ex)
                {
                    lbEntSalAvisos.Text = ex.Message;
                }
            }
            else
                lbEntSalAvisos.Text = "Fondos insuficientes en Línea de crédito: " + ddlEntSalLineasDeCredito.SelectedItem.Text;
        }

        protected void ddlEntSalLotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEntSalLotes.Visible = ddlEntSalLotes.SelectedValue.Equals("No existe...");
        }

        protected void ddlEntSalSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEntSalSeries.Visible = ddlEntSalSeries.SelectedValue.Equals("No existe...");
        }

        #endregion

        #region CambioDePrecios

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
                    errores = blInventarios.FinalizaCambioPrecios(ddlCmPrListasPrecios.SelectedValue, ddlCmPrOperacion.SelectedValue,
                        ddlCmPrTipo.SelectedValue, decimal.Parse(txtCmPrCantidad.Text));
                    oBitacora.Descripcion = "Todos | " + ddlCmPrOperacion.SelectedValue + " | " + ddlCmPrTipo.SelectedValue + " | " + "Cantidad: " + txtCmPrCantidad.Text;
                }
                else
                {
                    errores = blInventarios.FinalizaCambioPrecios(ddlCmPrDesde.SelectedItem.Text, ddlCmPrHasta.SelectedItem.Text, ddlCmPrListasPrecios.SelectedValue, ddlCmPrOperacion.SelectedValue,
                           ddlCmPrTipo.SelectedValue, decimal.Parse(txtCmPrCantidad.Text));
                    oBitacora.Descripcion = "Desde: " + ddlCmPrDesde.Text + " Hasta: " + ddlCmPrHasta.Text + " | " +
                                            ddlCmPrOperacion.SelectedValue + " | " + ddlCmPrTipo.SelectedValue + " | " + "Cantidad: " + txtCmPrCantidad.Text;
                }

                blInventarios.NuevoRegistroBitacora(oBitacora);

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
                lbCmPrAvisos.Text = "Error: "+ex.Message;
            }
        }

        private void cargaDdlCambioPrecios()
        {
            ddlCmPrDesde.DataSource = ddlCmPrHasta.DataSource = blInventarios.buscarTodosProductos();
            ddlCmPrDesde.DataTextField = ddlCmPrHasta.DataTextField = "Clave1";
            ddlCmPrDesde.DataValueField = ddlCmPrHasta.DataValueField = "idProducto";
            ddlCmPrDesde.DataBind();
            ddlCmPrHasta.DataBind();
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
        #endregion

        #region inventarioFisico

        private void cargaDdlInvFisico()
        {
            ddlInvFsClave1Desde.DataSource = ddlInvFsClave1Hasta.DataSource = blInventarios.buscarTodosProductos();
            ddlInvFsClave1Desde.DataTextField = ddlInvFsClave1Hasta.DataTextField = "Clave1";
            ddlInvFsClave1Desde.DataValueField = ddlInvFsClave1Hasta.DataValueField = "idProducto";
            ddlInvFsClave1Desde.DataBind();
            ddlInvFsClave1Hasta.DataBind();
            ddlInvFsAlmacen.DataTextField = "Nombre";
            ddlInvFsAlmacen.DataValueField = "idAlmacen";
            ddlInvFsAlmacen.DataSource = blInventarios.ObtenerAlmacenes();
            ddlInvFsAlmacen.DataBind();
        }

        protected void chbInvFsTodos_CheckedChanged(object sender, EventArgs e)
        {
            ddlInvFsClave1Hasta.Visible = ddlInvFsClave1Desde.Visible = Label24.Visible = Label25.Visible = !chbInvFsTodos.Checked;
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            cargaGrvInvFs();
        }

        void cargaGrvInvFs()
        {
            grvInvFsArticulos.SelectedIndexChanged += new EventHandler(grvInvFsArticulos_SelectedIndexChanged);
            List<Producto> lProductos;
            if (chbInvFsTodos.Checked)
                lProductos = blInventarios.ObtenerExistenciaTeorica(int.Parse(ddlInvFsAlmacen.SelectedValue));
            else
                lProductos = blInventarios.ObtenerExistenciaTeorica(int.Parse(ddlInvFsAlmacen.SelectedValue), ddlInvFsClave1Desde.SelectedItem.Text, ddlInvFsClave1Hasta.SelectedItem.Text);

            Session["InventarioInvFsLProductos"] = lProductos;

            grvInvFsArticulos.DataSource = lProductos;
            grvInvFsArticulos.DataBind();
            grvInvFsArticulos.SelectedIndex = -1;

            foreach (Producto p in lProductos)
            {
                blInventarios.BloquearProducto(int.Parse(ddlInvFsAlmacen.SelectedValue), p.dbProducto.idProducto, true);
            }
        }

        void grvInvFsArticulos_SelectedIndexChanged(object sender, EventArgs e)
        {
            InvFscargaProductoEnTxtBxs(((List<Producto>)Session["InventarioInvFsLProductos"])[grvInvFsArticulos.SelectedIndex]);
            txtInvFsCantidadRealArticulo.Focus();
        }

        protected void txtInvFsCantidadRealArticulo_TextChanged(object sender, EventArgs e)
        {
            ((List<Producto>)Session["InventarioInvFsLProductos"])[grvInvFsArticulos.SelectedIndex].existenciaReal = decimal.Parse(txtInvFsCantidadRealArticulo.Text);
            ((List<Producto>)Session["InventarioInvFsLProductos"])[grvInvFsArticulos.SelectedIndex].strExistenciaReal = txtInvFsCantidadRealArticulo.Text;
            grvInvFsArticulos.DataSource = ((List<Producto>)Session["InventarioInvFsLProductos"]);
            grvInvFsArticulos.DataBind();
            limpiaTextBox(this.InventarioFísico);
            grvInvFsArticulos.SelectRow(grvInvFsArticulos.SelectedIndex + 1);
            if (grvInvFsArticulos.SelectedIndex <= ((List<Producto>)Session["InventarioInvFsLProductos"]).Count)
                InvFscargaProductoEnTxtBxs(((List<Producto>)Session["InventarioInvFsLProductos"])[grvInvFsArticulos.SelectedIndex - 1]);

            txtInvFsCantidadRealArticulo.Text = string.Empty;
            txtInvFsCantidadRealArticulo.Focus();
        }

        protected void limpiaTextBox(Control c)
        {
            if (c is TextBox)
                ((TextBox)c).Text = string.Empty;
        }

        void InvFscargaProductoEnTxtBxs(Producto p)
        {
            txtInvFsClave.Text = p.codigo;
            txtInvFsDescripcionArticulo.Text = p.descripcion;
            txtInvFsLoteArticulo.Text = p.lote;
            txtInvFsSerieArticulo.Text = p.serie;
            txtInvFsCantidadTeoricaArticulo.Text = p.existenciaTeorica.ToString();
        }


        private void FinalizaInventarioFisico()
        {

            try
            {
                List<Producto> lProducto = (List<Producto>)Session["InventarioInvFsLProductos"];

                blInventarios.EstablecerExistenciaProducto(lProducto, int.Parse(ddlInvFsAlmacen.SelectedValue));

                foreach (Producto p in lProducto)
                {
                    blInventarios.BloquearProducto(int.Parse(ddlInvFsAlmacen.SelectedValue), p.dbProducto.idProducto, false);
                }
                lbInvFsAvisos.Text = "Proceso finalizado con éxito";
            }
            catch (Exception ex)
            {
                lbInvFsAvisos.Text = "Error: "+ex.Message;
            }
            

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
            /*else if (rdbFiltro2.Checked)
            {
                iTipo = 2;
            }
            else if (rdbFiltro3.Checked)
            {
                iTipo = 3;
            }*/

            var oQuery = blInventarios.Buscar(sCadena, iTipo);

            try
            {
                gdvDatos.DataSource = oQuery;
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            FinalizaCambioPrecios();
        }

        protected void imgBtnEntradaSalida_Click(object sender, ImageClickEventArgs e)
        {
            CambioPrecios.Visible = false;
            InventarioFísico.Visible = false;
            EntradaSalida.Visible = true;
            pnlCatalogo.Visible = false;
            pnlFiltroReportes.Visible = false;
        }

        protected void imgBtnMostrar_Click(object sender, ImageClickEventArgs e)
        {
            CambioPrecios.Visible = false;
            InventarioFísico.Visible = false;
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
            CambioPrecios.Visible = false;
            InventarioFísico.Visible = false;
            EntradaSalida.Visible = false;
            pnlCatalogo.Visible = true;
            pnlFiltroReportes.Visible = false;
            Buscar(txbBuscar.Text);
            gdvDatos.SelectedIndex = -1;
        }

        protected void imgBtnPrecios_Click(object sender, ImageClickEventArgs e)
        {
            EntradaSalida.Visible = false;
            InventarioFísico.Visible = false;
            CambioPrecios.Visible = true;
            pnlCatalogo.Visible = false;
            pnlFiltroReportes.Visible = false;
        }

        protected void imgBtnFisico_Click(object sender, ImageClickEventArgs e)
        {
            EntradaSalida.Visible = false;
            CambioPrecios.Visible = false;
            InventarioFísico.Visible = true;
            pnlCatalogo.Visible = false;
            pnlFiltroReportes.Visible = false;
            cargaDdlInvFisico();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            FinalizaEntradaSalida();            
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            FinalizaInventarioFisico();            
        }               

        protected void CargarReporte()
        {
            EntradaSalida.Visible = false;
            CambioPrecios.Visible = false;
            InventarioFísico.Visible = false;
            pnlCatalogo.Visible = false;
            pnlFiltroReportes.Visible = false;
            //pnlReportes.Visible = true;

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario", "medicuriConnectionString", odsDataSet, "inventario");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from inventario_partida", "medicuriConnectionString", odsDataSet, "inventario_partida");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_lotes", "medicuriConnectionString", odsDataSet, "productos_lotes");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_series", "medicuriConnectionString", odsDataSet, "productos_series");

            //GT 0179
            Session["campoaordenar"] = "";
            Session["sortfield"] = 0;
            Session["recordselection"] = "";

            Session["dataset"] = odsDataSet;
            Session["reportdocument"] = "~\\rptReportes\\rptMovimientos.rpt";
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
            CambioPrecios.Visible = false;
            InventarioFísico.Visible = false;
            pnlCatalogo.Visible = false;
            pnlFiltroReportes.Visible = true;
            CargarListaReportes();
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

    }
}
