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
using System.Xml;

namespace Medicuri
{
    public partial class Facturacion : System.Web.UI.Page
    {
        ImageButton imbNuevo, imbEditar, imbEliminar, imbImprimir, imbMostrar, imbAceptar, imbCancelar, imbReportes, imbReimprimir, imbTimbrar;
        RadioButton rdbNombre, rdbClave, rdbTodos;
        Label lblReportes, lblNombreModulo, lblReimprimir, lblTimbrar;
        Button btnBuscar;
        TextBox txbBuscar;
        MedNeg.Facturas.BlDetallePartida oblDetallePartida;        
               
        MedDAL.DAL.facturas oFactura;
        MedNeg.Facturas.BlFacturas oblFacturas;
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
                cPermiso = (char)htbPermisos["facturas"];

                Master.FindControl("btnAlertaStock").Visible = true;

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
                imbReimprimir = (ImageButton)Master.FindControl("imgBtnPrecios");
                imbReimprimir.Click += new ImageClickEventHandler(this.imbReimprimir_Click);
                imbReimprimir.ImageUrl = "~/Icons/reports32.png";
                lblReimprimir = (Label)Master.FindControl("lblPrecios");
                lblReimprimir.Text = "Reenviar";
                imbTimbrar = (ImageButton)Master.FindControl("imgBtnAlertas");                
                imbTimbrar.Click += new ImageClickEventHandler(this.imbTimbrar_Click);
                imbTimbrar.ImageUrl = "~/Icons/up_32.png";
                lblTimbrar = (Label)Master.FindControl("lblAlertas");
                lblTimbrar.Text = "Timbrar";

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
                Master.FindControl("btnPrecios").Visible = true;

                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "Facturas";

                //GT 0175
                imbReportes = (ImageButton)Master.FindControl("imgBtnReportes");
                imbReportes.Click += new ImageClickEventHandler(this.imbReportes_Click);

                //2013/03/14 JID Se ocultan un par de campos
                cmbModoFactura.Visible = false;
                Label25.Visible = false;

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
                    Session["resultadoquery"] = "";
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;

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
                    oblFacturas = new MedNeg.Facturas.BlFacturas();
                    Session["iFolioAutomatico"] = oblFacturas.RecuperaFolioAutomatico(Server.MapPath("~/Archivos/Configuracion.xml"));
                    txbFolio.Text = Session["iFolioAutomatico"].ToString();
                    Session["sEsDePedido"] = false;
                    Session["sEsDeRemision"] = false;
                    Session["sEsDeReceta"] = false;
                    Session["lstDetallePartida"] = new List<MedNeg.Facturas.BlDetallePartida>();
                    pnlCatalogo.Visible = false;
                    pnlFormulario.Visible = false;
                    //pnlReportes.Visible = false;   
                    Session["sTotalFactura"] = 0;

                    //Variable de sesion para saber si es un ensamble al momento de registrar el detalle de la partida
                    // 0 = False, 1 = True
                    Session["sBolEsEnsamble"] = 0;

                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);

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
            pnlFiltroReportes.Visible = false;
            
           
        }

        /// <summary>
        /// Mostrar la lista de todos los pedidos
        /// </summary>
        protected void MostrarLista()
        {
            oblFacturas = new MedNeg.Facturas.BlFacturas();
            var oQuery = oblFacturas.MostrarLista();
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
        /// Deprecated: 0087 Metodo que que desgloza los ensambles en sus productos para registrarlo en la partida
        /// </summary>
        protected void AgregarDetalleEnsamble(MedNeg.Facturas.BlDetallePartida renglonEnsamble, int iIdFactura)
        {
            //Recuperar los datos del ensamble
            MedDAL.DAL.ensamble oEnsamble = new MedDAL.DAL.ensamble();
            MedDAL.DAL.ensamble_productos oEnsambleProductos = new MedDAL.DAL.ensamble_productos();
            MedNeg.Ensambles.BlEnsambles oblEnsamble = new MedNeg.Ensambles.BlEnsambles();

            oEnsamble = oblEnsamble.BuscarNombre(renglonEnsamble.sProducto);
            oblEnsamble = new MedNeg.Ensambles.BlEnsambles();
            oEnsambleProductos = oblEnsamble.RecuperarProducto(oEnsamble.ClaveBom);

            //Para registrar el detalle de la remision
            oblFacturas = new MedNeg.Facturas.BlFacturas();
            MedDAL.DAL.facturas_partida oFacturaPartida = new MedDAL.DAL.facturas_partida();

            oFacturaPartida.idFactura = iIdFactura;
            oFacturaPartida.idEnsamble = oEnsamble.idEnsamble;
            oFacturaPartida.idProducto = oEnsambleProductos.idProducto;
            oFacturaPartida.Cantidad = renglonEnsamble.dCantidad;
            oFacturaPartida.IEPS = 0;
            oFacturaPartida.Iva = 0;
            oFacturaPartida.Precio = renglonEnsamble.dPrecio;
            oFacturaPartida.Observaciones = renglonEnsamble.sObservaciones;
            oFacturaPartida.Descripcion = oEnsamble.Descripcion;

            //Registrar el detalle del pedido
            if (!oblFacturas.NuevoDetallePartida(oFacturaPartida))
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

        #region Agregar Nuevo
        /// <summary>
        /// 2013/03/10 JID Desgloza los ensambles en sus productos para registrarlo en la factura
        /// </summary>
        /// <param name="renglonEnsamble"></param>
        /// <param name="oFactura"></param>
        private bool SetPartidaEnsamble(MedNeg.Facturas.BlDetallePartida renglonEnsamble, ref MedDAL.DAL.facturas oFactura)
        {
            MedDAL.DAL.ensamble oEnsamble = new MedDAL.DAL.ensamble();
            MedDAL.DAL.ensamble_productos oEnsambleProductos = new MedDAL.DAL.ensamble_productos();
            MedNeg.Ensambles.BlEnsambles oblEnsamble = new MedNeg.Ensambles.BlEnsambles();

            //2013/03/10 Se busca el ensamble por su nombre
            oEnsamble = oblEnsamble.BuscarNombre(renglonEnsamble.sProducto);
            if (oEnsamble == null)
            {
                return false;
            }
            //oblEnsamble = new MedNeg.Ensambles.BlEnsambles();
            oEnsambleProductos = oblEnsamble.RecuperarProducto(oEnsamble.ClaveBom);
            if (oEnsambleProductos == null)
            {
                return false;
            }

            MedDAL.DAL.facturas_partida oFacturaPartida = new MedDAL.DAL.facturas_partida();

            oFacturaPartida.idFactura = 0;
            oFacturaPartida.idEnsamble = oEnsamble.idEnsamble;
            oFacturaPartida.idProducto = oEnsambleProductos.idProducto;
            oFacturaPartida.Cantidad = renglonEnsamble.dCantidad;
            oFacturaPartida.IEPS = 0;
            oFacturaPartida.Iva = 0;
            oFacturaPartida.Precio = renglonEnsamble.dPrecio;
            oFacturaPartida.Observaciones = renglonEnsamble.sObservaciones;
            oFacturaPartida.Descripcion = oEnsamble.Descripcion;
            //2013/03/10 Se agrega la partida a la factura
            oFactura.facturas_partida.Add(oFacturaPartida);

            sDatosBitacora += "Producto:" + renglonEnsamble.sProducto.ToString() + " ";
            sDatosBitacora += "Cant:" + renglonEnsamble.dCantidad.ToString() + " ";
            sDatosBitacora += "IEPS:" + renglonEnsamble.dIeps.ToString() + " ";
            sDatosBitacora += "Iva:" + renglonEnsamble.dIva.ToString() + " ";
            sDatosBitacora += "Precio:" + renglonEnsamble.dPrecio.ToString() + " ";
            sDatosBitacora += "Total:" + Convert.ToDecimal((renglonEnsamble.dCantidad * renglonEnsamble.dPrecio) + renglonEnsamble.dIeps + renglonEnsamble.dIva) + ", ";
            sDatosBitacora += "Obs:" + renglonEnsamble.sObservaciones;

            return true;
        }

        /// <summary>
        /// 2013/03/10 JID Agrega el detalle simple a la factura
        /// </summary>
        /// <param name="facturaDetalle"></param>
        /// <param name="oFactura"></param>
        private void SetPartida(MedNeg.Facturas.BlDetallePartida facturaDetalle, ref MedDAL.DAL.facturas oFactura)
        {            
            //Tomar en cuenta que el tipo de la partida ya es diferente ya que este se guardara en la base de datos
            MedDAL.DAL.facturas_partida oFacturaPartida = new MedDAL.DAL.facturas_partida();

            oFacturaPartida.idFactura = 0;
            oFacturaPartida.idProducto = facturaDetalle.iIdProducto;
            oFacturaPartida.Cantidad = facturaDetalle.dCantidad;
            oFacturaPartida.IEPS = facturaDetalle.dIeps;
            oFacturaPartida.Iva = facturaDetalle.dIva;
            oFacturaPartida.Precio = facturaDetalle.dPrecio;
            oFacturaPartida.Observaciones = facturaDetalle.sObservaciones;
            oFacturaPartida.Descripcion = facturaDetalle.SProducto;

            oFactura.facturas_partida.Add(oFacturaPartida);
            
            sDatosBitacora += "Producto:" + facturaDetalle.iIdProducto.ToString() + " ";
            sDatosBitacora += "Cant:" + facturaDetalle.dCantidad.ToString() + " ";
            sDatosBitacora += "IEPS:" + facturaDetalle.dIeps.ToString() + " ";
            sDatosBitacora += "Iva:" + facturaDetalle.dIva.ToString() + " ";
            sDatosBitacora += "Precio:" + facturaDetalle.dPrecio.ToString() + " ";
            sDatosBitacora += "Total:" + Convert.ToDecimal((facturaDetalle.dCantidad * facturaDetalle.dPrecio) + facturaDetalle.dIeps + facturaDetalle.dIva) + ", ";            
        }

        /// <summary>
        /// 2013/03/10 Jorge Ibarra
        /// Carga los datos que debe contener una partida de factura
        /// </summary>
        /// <param name="oFactura"></param>
        private bool SetObjetoFacturaPartida (ref MedDAL.DAL.facturas oFactura, MedDAL.DAL.medicuriEntities oEntities)
        {
            MedDAL.Usuarios.DALUsuarios oDALUsuario = new MedDAL.Usuarios.DALUsuarios();
            MedDAL.Productos.DALProductos oDALProductos = new MedDAL.Productos.DALProductos();
            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();

            oUsuario = (MedDAL.DAL.usuarios)oDALUsuario.Buscar(Session["usuario"].ToString());

            //Recorrer el objeto de sesion lstDetallePartida que contiene los datos de la partida
            foreach (MedNeg.Facturas.BlDetallePartida facturaDetalle in (List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])
            {
                //0087 Saber si es un ensamble el que se esta registrando y agregarlo a la factura
                if (facturaDetalle.bEsEnsamble == true)
                {
                    if (!SetPartidaEnsamble(facturaDetalle, ref oFactura))
                    {
                        return false;
                    }
                }
                else
                {
                    SetPartida(facturaDetalle, ref oFactura);
                }

                //Modificar la existencia en el almacén que el usuario tenga asignado en configuración
                if (!oDALProductos.ModificarExistenciaProducto(oUsuario.idAlmacen, facturaDetalle.iIdProducto, facturaDetalle.dCantidad, 1, oEntities))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 2013/03/10 Jorge Ibarra
        /// Carga los datos que debe de contener un objeto factura
        /// </summary>
        /// <returns></returns>
        private MedDAL.DAL.facturas SetObjetoFactura()
        {
            string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");

            if (Session["ObjetoFactura"] == null)
            {
                oFactura = new MedDAL.DAL.facturas();
            }
            else
            {
                oFactura = (MedDAL.DAL.facturas)Session["ObjetoFactura"];
            }

            oFactura.idCliente = (int)Session["sIdCliente"];

            if ((bool)Session["sEsDePedido"] == true)
                oFactura.idPedido = (int)Session["sIdPedido"];

            if ((bool)Session["sEsDeRemision"] == true)
                oFactura.idRemision = (int)Session["sIdRemision"];

            if ((bool)Session["sEsDeReceta"] == true)
                oFactura.idReceta = (int)Session["sIdReceta"];

            oFactura.TipoFactura = cmbTipoFactura.SelectedValue.ToString();
            oFactura.Fecha = DateTime.Now;
            oFactura.FechaAplicacion = DateTime.Now;
            oFactura.Estatus = cmbEstatus.SelectedValue;
            oFactura.idUsuario = Convert.ToInt32(Session["usuarioid"]);
            oFactura.Vendedor = Session["nombre"].ToString();
            //2013 02 19 Nuevos campos para factura 3.2
            oFactura.idMetodoPago = Convert.ToInt32(cmbMetodoPago.SelectedValue);
            oFactura.idTipoFormaPago = Convert.ToInt32(cmbFormaPago.SelectedValue);
            oFactura.NumeroCuentaPago = txbCuentaPago.Text;

            //Validar si se esta respetando el folio automatico y verificar si aun es el mismo o cambio su valor
            if (Session["iFolioAutomatico"].Equals(txbFolio.Text))
            {
                oFactura.Folio = oblFacturas.RecuperaFolioAutomatico(sRutaArchivoConfig).ToString();
            }
            else
            {
                oFactura.Folio = txbFolio.Text;
            }

            //2013/03/10 JID Se colocan los datos que van a la bitácora
            sDatosBitacora += "Tipo:" + cmbTipoFactura.SelectedValue.ToString() + " ";
            sDatosBitacora += "Folio:" + txbFolio.Text + " ";
            sDatosBitacora += "Fecha:" + txbFecha.Text + " ";
            sDatosBitacora += "Estatus:" + cmbEstatus.SelectedItem.ToString() + " ";
            sDatosBitacora += "Cliente:" + txbCliente.Text + " ";

            return oFactura;
        }

        /// <summary>
        /// 2013/03/10 Jorge Ibarra
        /// Crea una instancia de factura, la guarda, y en caso de ser electrónica la procesa para ser timbrada
        /// </summary>
        private bool AddFactura()
        {
            string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");            
            MedDAL.Facturas.DALFacturas oDALFacturas = new MedDAL.Facturas.DALFacturas();
            oblFacturas = new MedNeg.Facturas.BlFacturas();

            oFactura = SetObjetoFactura();
            //Coloca la partida en la factura y además resta las existencias en el almacén correspondiente
            if (!SetObjetoFacturaPartida(ref oFactura, oDALFacturas.MedicuriEntities))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert(Ha ocurrido un error: \n" + oDALFacturas.GetError() + ")", true);
                return false;
            }

            if (!oDALFacturas.Add(oFactura, oDALFacturas.MedicuriEntities))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert(Ha ocurrido un error: \n" + oDALFacturas.GetError() + ")", true);
                return false;
            }

            //Saber si se va a actualizar el estatus del pedido
            if ((bool)Session["sEsDePedido"] == true)
            {
                oFactura.pedidos.Estatus = "3";                
            }

            //Saber si se va a actualizar el estatus de la remision
            if ((bool)Session["sEsDeRemision"] == true)
            {
                oFactura.remisiones.Estatus = "3";                
            }

            //Saber si se va a actualizar el estatus de la remision
            if ((bool)Session["sEsDeReceta"] == true)
            {
                oFactura.recetas.Estatus = "2";                
            }

            if (oDALFacturas.SaveChanges(oDALFacturas.MedicuriEntities))
            {
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

                oblFacturas.ActualizarFolioFactura(sRutaArchivoConfig);

                return true;
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert(Ha ocurrido un error: \n" + oDALFacturas.GetError() + ")", true);
                return false;
            }
        }
        #endregion

        #region Facturar/Timbrar
        public bool Timbrar(int iIdFactura)
        {
            //string sRutaCertificados = Server.MapPath("~/Archivos/");

            //oblFacturas = new MedNeg.Facturas.BlFacturas();
            //MedDAL.DAL.facturas oDALFacturas = new MedDAL.DAL.facturas();
            //string sDatosBitacora = string.Empty;

            //oFactura = new MedDAL.DAL.facturas();
            //oFactura = oblFacturas.BuscarFactura(iIdFactura);

            //int iResultado = oblFacturas.GenerarFacturaElectronica(iIdFactura, sRutaCertificados, Session["usuario"].ToString(), oFactura.idCliente, oFactura.Folio);

            //if (iResultado == 0)
            //{
            //    oblFacturas.CrearZip(new string[] { Server.MapPath("~/Archivos/FacturasElectronicasTimbradas"), Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFactura.Folio + ".zip") }, Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFactura.Folio + ".xml"), Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFactura.Folio + ".pdf"));

            //    ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarFacturas(0);", true);

            //    System.IO.FileInfo fFactura = new System.IO.FileInfo(Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFactura.Folio + ".zip"));

            //    Response.Clear();
            //    Response.AddHeader("Content-Disposition", "attachment; filename=" + fFactura.Name);
            //    Response.AddHeader("Content-Length", fFactura.Length.ToString());
            //    Response.ContentType = "application/....";
            //    Response.WriteFile(fFactura.FullName);
            //    Response.End();

            //}
            //else if (iResultado == 1)
            //{
            //    ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarFacturas(1);", true);
            //}
            //else if (iResultado == 2)
            //{
            //    ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarFacturas(2);", true);
            //}
            //else if (iResultado == 3)
            //{
            //    ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarFacturas(3);", true);
            //}
            //else if (iResultado == 4)
            //{
            //    ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarFacturas(4);", true);
            //}            
            ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarFacturas(6);", true);
            return true;
        }
        #endregion

        /// <summary>
        /// 2013/03/15 -Eliminado- 
        /// Registrar nueva factura
        /// </summary>
        //private void Nuevo()
        //{
        //    #region Codigo Antiguo
        //    string sRutaArchivoConfig=Server.MapPath("~/Archivos/Configuracion.xml");
        //    string  sRutaCertificados= Server.MapPath("~/Archivos/");

        //    oFactura = new MedDAL.DAL.facturas();
        //    oblFacturas = new MedNeg.Facturas.BlFacturas();
        //    oFactura.idCliente = (int)Session["sIdCliente"];

        //    if ((bool)Session["sEsDePedido"] == true)
        //        oFactura.idPedido = (int)Session["sIdPedido"];

        //    if ((bool)Session["sEsDeRemision"] == true)
        //        oFactura.idRemision = (int)Session["sIdRemision"];

        //    if ((bool)Session["sEsDeReceta"] == true)
        //        oFactura.idReceta = (int)Session["sIdReceta"];

        //    oFactura.TipoFactura = cmbTipoFactura.SelectedValue.ToString();
        //    oFactura.Fecha = DateTime.Now;
        //    oFactura.FechaAplicacion = DateTime.Now;
        //    oFactura.Estatus = cmbEstatus.SelectedValue;
        //    oFactura.idUsuario = Convert.ToInt32(Session["usuarioid"]);
        //    oFactura.Vendedor = Session["nombre"].ToString();
        //    //2013 02 19 Nuevos campos para factura 3.2
        //    oFactura.idMetodoPago = Convert.ToInt32(cmbMetodoPago.SelectedValue);
        //    oFactura.idTipoFormaPago = Convert.ToInt32(cmbFormaPago.SelectedValue);
        //    oFactura.NumeroCuentaPago = txbCuentaPago.Text;

        //    //Validar Folio Repetido
        //    if (ValidaFolioRepetido())
        //    {

        //        //Validar si se esta respetando el folio automatico y verificar si aun es el mismo o cambio su valor
        //        if (Session["iFolioAutomatico"].Equals(txbFolio.Text))
        //        {
        //            oFactura.Folio = oblFacturas.RecuperaFolioAutomatico(sRutaArchivoConfig).ToString();
        //        }
        //        else
        //        {
        //            oFactura.Folio = txbFolio.Text;
        //        }

        //        if (oblFacturas.NuevoRegistro(oFactura))
        //        {
        //            //Datos de la bitacora
        //            //string sDatosBitacora = string.Empty;
        //            sDatosBitacora += "Tipo:" + cmbTipoFactura.SelectedValue.ToString() + " ";
        //            sDatosBitacora += "Folio:" + txbFolio.Text + " ";
        //            sDatosBitacora += "Fecha:" + txbFecha.Text + " ";
        //            sDatosBitacora += "Estatus:" + cmbEstatus.SelectedItem.ToString() + " ";
        //            sDatosBitacora += "Cliente:" + txbCliente.Text + " ";


        //            // Registrar la partida de la remision
        //            oFactura = new MedDAL.DAL.facturas();
        //            oFactura = oblFacturas.BuscarFacturasFolio(txbFolio.Text);
        //            int iIdFactura = oFactura.idFactura;
        //            bool bRegistroFallido = false;

        //            //Recorrer el objeto de sesion lstDetallePartida que contiene los datos de la partida
        //            foreach (MedNeg.Facturas.BlDetallePartida facturaDetalle in (List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])
        //            {
        //                //0087 Saber si es un ensamble el que se esta registrando
        //                if (facturaDetalle.bEsEnsamble == true)
        //                {
        //                    AgregarDetalleEnsamble(facturaDetalle, iIdFactura);
        //                }
        //                else
        //                {
        //                    oblFacturas = new MedNeg.Facturas.BlFacturas();
        //                    MedDAL.DAL.facturas_partida oFacturaPartida = new MedDAL.DAL.facturas_partida();

        //                    oFacturaPartida.idFactura = iIdFactura;
        //                    oFacturaPartida.idProducto = facturaDetalle.iIdProducto;
        //                    oFacturaPartida.Cantidad = facturaDetalle.dCantidad;
        //                    oFacturaPartida.IEPS = facturaDetalle.dIeps;
        //                    oFacturaPartida.Iva = facturaDetalle.dIva;
        //                    oFacturaPartida.Precio = facturaDetalle.dPrecio;
        //                    oFacturaPartida.Observaciones = facturaDetalle.sObservaciones;
        //                    oFacturaPartida.Descripcion = facturaDetalle.SProducto;

        //                    //Registrar el detalle del pedido
        //                    if (!oblFacturas.NuevoDetallePartida(oFacturaPartida))
        //                    {
        //                        bRegistroFallido = true;
        //                    }
        //                    else
        //                    {
        //                        sDatosBitacora += "Producto:" + facturaDetalle.iIdProducto.ToString() + " ";
        //                        sDatosBitacora += "Cant:" + facturaDetalle.dCantidad.ToString() + " ";
        //                        sDatosBitacora += "IEPS:" + facturaDetalle.dIeps.ToString() + " ";
        //                        sDatosBitacora += "Iva:" + facturaDetalle.dIva.ToString() + " ";
        //                        sDatosBitacora += "Precio:" + facturaDetalle.dPrecio.ToString() + " ";
        //                        sDatosBitacora += "Total:" + Convert.ToDecimal((facturaDetalle.dCantidad * facturaDetalle.dPrecio) + facturaDetalle.dIeps + facturaDetalle.dIva) + ", ";
        //                    }
        //                }
        //            }            

        //            if (!bRegistroFallido)
        //            {
        //                //Registrar datos de la remision en la bitacora
        //                //lblAviso.Text = "El usuario se ha registrado con éxito";
        //                oBitacora = new MedDAL.DAL.bitacora();
        //                oblBitacora = new MedNeg.Bitacora.BlBitacora();
        //                oBitacora.FechaEntradaSrv = DateTime.Now;
        //                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
        //                oBitacora.Modulo = "Factura";
        //                oBitacora.Usuario = Session["usuario"].ToString();
        //                oBitacora.Nombre = Session["nombre"].ToString();
        //                oBitacora.Accion = "Nueva Factura";
        //                oBitacora.Descripcion = sDatosBitacora;
        //                if (!oblBitacora.NuevoRegistro(oBitacora))
        //                {
        //                    lblDatos.Text = "El evento no pudo ser registrado en la bitácora";
        //                }

        //                //Actualizar el consecutivo en la bitacora
        //                oblFacturas.ActualizarFolioFactura(sRutaArchivoConfig);                        

        //                //Saber si se va a actualizar el estatus del pedido
        //                if ((bool)Session["sEsDePedido"] == true)
        //                {
        //                    //Actualizar el estatus del pedido en caso de que se haya hecho la remision a partir de un pedido
        //                    MedDAL.DAL.pedidos oPedido = new MedDAL.DAL.pedidos();
        //                    MedNeg.Pedidos.BlPedidos oblPedido = new MedNeg.Pedidos.BlPedidos();

        //                    //Actualizar el estatus del pedido
        //                    oPedido = oblPedido.BuscarPedido((int)Session["sIdPedido"]);
        //                    oPedido.Estatus = "3";

        //                    if (!oblPedido.EditarRegistro(oPedido))
        //                    {
        //                        lblDatos.Text = "No se pudo cambiar el estatus del pedido, contacte al administrador";
        //                    }
        //                }

        //                //Saber si se va a actualizar el estatus de la remision
        //                if ((bool)Session["sEsDeRemision"] == true)
        //                {
        //                    //Actualizar el estatus del pedido
        //                    MedDAL.DAL.remisiones oRemision = new MedDAL.DAL.remisiones();
        //                    MedNeg.Remisiones.BlRemisiones oblRemision = new MedNeg.Remisiones.BlRemisiones();

        //                    oRemision = oblRemision.BuscarRemision((int)Session["sIdRemision"]);
        //                    oRemision.Estatus = "3";

        //                    if (!oblRemision.EditarRegistro(oRemision))
        //                    {
        //                        lblDatos.Text = "No se pudo cambiar el estatus de la remisión, contacte al administrador";
        //                    }
        //                }

        //                //Saber si se va a actualizar el estatus de la remision
        //                if ((bool)Session["sEsDeReceta"] == true)
        //                {
        //                    //Actualizar el estatus del pedido
        //                    MedDAL.DAL.remisiones oRemision = new MedDAL.DAL.remisiones();
        //                    MedNeg.Remisiones.BlRemisiones oblRemision = new MedNeg.Remisiones.BlRemisiones();

        //                    MedDAL.DAL.recetas oReceta = new MedDAL.DAL.recetas();
        //                    MedNeg.Recetas.BlRecetas oblRecetas = new MedNeg.Recetas.BlRecetas();

        //                    oReceta = oblRecetas.BuscarReceta((int)Session["sIdReceta"]);
        //                    oReceta.Estatus = "2";

        //                    if (!oblRecetas.EditarRegistro(oReceta))
        //                    {
        //                        lblDatos.Text = "No se pudo cambiar el estatus de la receta, contacte al administrador";
        //                    }
        //                }

        //                /******* Realizar la resta de las existencias ***********/

        //                MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
        //                MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();

        //                oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());

        //                MedNeg.Remisiones.BlRemisiones oblRemisiones;
        //                bool bModificarExistenciasError = false;

        //                //Recorrer el objeto de sesion lstDetallePartida que contiene los datos de la partida
        //                foreach (MedNeg.Facturas.BlDetallePartida facturaDetalle in (List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"])
        //                {
        //                    oblRemisiones = new MedNeg.Remisiones.BlRemisiones();
        //                    if (!oblRemisiones.ModificarExistenciaProducto(oUsuario.idAlmacen, facturaDetalle.iIdProducto, facturaDetalle.dCantidad, 1))
        //                        bModificarExistenciasError = true;

        //                }

        //                if (bModificarExistenciasError == true)
        //                {
        //                    lblDatos.Text = "No se pudo modificar la existencia de los productos, por favor contacte al administrador";
        //                }
        //                /****** Termina resta de las existencias ***************************/
        //                #region FACTURACION ELECTRONICA

        //                //TODO GT : aqui se mandan los datos de la factura electronica
        //                //Generar la factura electronica
        //                if (cmbModoFactura.SelectedValue == "0")
        //                {
        //                    oblFacturas = new MedNeg.Facturas.BlFacturas();
        //                    int iResultado = oblFacturas.GenerarFacturaElectronica(iIdFactura, sRutaCertificados, Session["usuario"].ToString(), (int)Session["sIdCliente"], oFactura.Folio);

        //                    if (iResultado == 0)
        //                    {
        //                        oblFacturas.CrearZip(new string[] { Server.MapPath("~/Archivos/FacturasElectronicasTimbradas"), Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFactura.Folio + ".zip") }, Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFactura.Folio + ".xml"), Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFactura.Folio + ".pdf"));

        //                        ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarFacturas(0);", true);

        //                        System.IO.FileInfo fFactura = new System.IO.FileInfo(Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFactura.Folio + ".zip"));

        //                        Response.Clear();
        //                        Response.AddHeader("Content-Disposition", "attachment; filename=" + fFactura.Name);
        //                        Response.AddHeader("Content-Length", fFactura.Length.ToString());
        //                        Response.ContentType = "application/....";
        //                        Response.WriteFile(fFactura.FullName);
        //                        Response.End();

        //                    }
        //                    else if (iResultado == 1)
        //                    {
        //                        ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarFacturas(1);", true);
        //                    }
        //                    else if (iResultado == 2)
        //                    {
        //                        ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarFacturas(2);", true);
        //                    }
        //                    else if (iResultado == 3)
        //                    {
        //                        ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarFacturas(3);", true);
        //                    }
        //                    else if (iResultado == 4)
        //                    {
        //                        ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarFacturas(4);", true);
        //                    }
        //                }

        //                #endregion
        //            }
        //            else
        //            {
        //                //Eliminar la factura, su partida e indicar al usuario que lo intente de nuevo, limpiar la cadena de bitacora
        //                Eliminar(iIdFactura);
        //                sDatosBitacora = "";
        //                lblDatos.Text = "No se pudo registrar la factura, por favor verifique los datos y vuelva a intentarlo";
        //            }
                    
        //        }
        //        else
        //        {
        //            //Fallo esl registro de la factura
        //            lblDatos.Text = "No se pudo registrar la factura, por favor verifique los datos y vuelva a intentarlo";
        //        }

        //    }
        //    else  //si es folio repetido
        //    {
        //        lblDatos.Text = "Folio Repetido, no se puede generar el pedido";

        //    }
        //    #endregion
        //}

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
            pnlFiltroReportes.Visible = false;

            if (bDatos)
            {

                //Objeto que contiene el id del pedido 
                oblFacturas = new MedNeg.Facturas.BlFacturas();
                oFactura = new MedDAL.DAL.facturas();
                oFactura = (MedDAL.DAL.facturas)oblFacturas.BuscarFactura(int.Parse(dgvDatos.SelectedDataKey[0].ToString()));
                
                //Llenar los campos de la factura
                txbFolio.Text = oFactura.Folio;

                if (oFactura.idPedido.Equals(null))
                    txbPedido.Text = "";
                else
                    txbPedido.Text = oFactura.pedidos.Folio.ToString();

                if (oFactura.idRemision.Equals(null))
                    txbRemision.Text = "";
                else
                    txbRemision.Text = oFactura.remisiones.Folio.ToString();

                if (oFactura.idReceta.Equals(null))
                    txbReceta.Text = "";
                else
                    txbReceta.Text = oFactura.recetas.Folio.ToString();

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

               txbPoblacion.Text = oFactura.clientes.poblaciones.Nombre.ToString() + ", " + oFactura.clientes.municipios.Nombre.ToString() +", " + oFactura.clientes.estados.Nombre.ToString();

               //Lenar los datos de la partida del detalle
               oblFacturas = new MedNeg.Facturas.BlFacturas();

               //Recuperar la partida del pedido
               List<MedDAL.DAL.facturas_partida> oQuery = new List<MedDAL.DAL.facturas_partida>();
               oQuery.AddRange(oblFacturas.RecuperarPartidaFactura(oFactura.idFactura));

               //Session["lstDetallePartida"] = new List<MedNeg.Facturas.BlDetallePartida>();
               if (((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Count() > 0)
               {
                   ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Clear();
               }
                
               //Recorrer el resultado y meterlo al datagridview
               Session["sTotalFactura"] = 0;

               //0087 Variables para gestionar la carga de datos del producto o del ensamble
               string sClave;
               string sNombre;
               bool bEsEnsamble;

               foreach (MedDAL.DAL.facturas_partida oDetalle in oQuery)
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
               if (oFactura.Estatus == "3")
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
            //oFactura.idFactura = int.Parse(dgvDatos.SelectedDataKey.Value.ToString());
            oFactura = oblFacturas.BuscarFacturasFolio(txbFolio.Text);
            oFactura.Estatus = cmbEstatus.SelectedValue.ToString();

            oblFacturas = new MedNeg.Facturas.BlFacturas();
            if(oblFacturas.EditarRegistro(oFactura))
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
                        //Saber si es un ensamble el que se esta registrando
                        if (facturaDetalle.bEsEnsamble == true)
                        {
                            AgregarDetalleEnsamble(facturaDetalle, oFactura.idFactura);
                        }
                        else
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
                    }
                    
                    /****** GT: Modificar las existencias de los productos nuevos ***************/

                    MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();

                    oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());

                    //oblRemision = new MedNeg.Remisiones.BlRemisiones();
                    oblFacturas = new MedNeg.Facturas.BlFacturas();
                    foreach (MedNeg.Facturas.BlDetallePartida remisionDetalleNuevos in (List<MedNeg.Facturas.BlDetallePartida>)Session["lstremisionespartidaedicion"])
                    {
                        oblFacturas.ModificarExistenciaProducto(oUsuario.idAlmacen, remisionDetalleNuevos.iIdProducto, remisionDetalleNuevos.dCantidad, 1);

                    }

                    /****** GT: Modificar las existencias de los productos nuevos ***************/


                    //Anotar en la bitacora la modificación a la factura
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

                    if (oFactura.Estatus == "5")
                    {
                        string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
                        string sRutaCertificados = Server.MapPath("~/Archivos/");
                        string[] sUUID = new string[1];
                        string sMensaje = "";
                        bool bEncontrado = false;

                        XmlTextReader oXMLReader = new XmlTextReader(sRutaCertificados + "/FacturasElectronicasTimbradas/FacturaE-" + oFactura.Folio + ".xml");
                        while (oXMLReader.Read())
                        {
                            switch (oXMLReader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    if (oXMLReader.Name == "tfd:TimbreFiscalDigital")
                                    {
                                        sUUID[0] = oXMLReader.GetAttribute("UUID");
                                        bEncontrado = true;
                                        break;
                                    }
                                    break;
                            }
                        }

                        if (bEncontrado)
                        {
                            int iResultado = oblFacturas.CancelarFacturaElectronica(sUUID, sRutaCertificados, out sMensaje);

                            if (iResultado == 0)
                            {
                                ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarCancelacionFacturas(0);", true);

                                System.IO.FileInfo fFactura = new System.IO.FileInfo(Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + sUUID[0] + ".xml"));

                                Response.Clear();
                                Response.AddHeader("Content-Disposition", "attachment; filename=" + fFactura.Name);
                                Response.AddHeader("Content-Length", fFactura.Length.ToString());
                                Response.ContentType = "application/....";
                                Response.WriteFile(fFactura.FullName);
                                Response.End();
                            }
                            else if (iResultado == 1)
                            {
                                ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarCancelacionFacturas(1);", true);
                            }
                        }
                    }
                }
            }

        }

        private void Reimprimir(int iIdFactura)
        {
            oblFacturas = new MedNeg.Facturas.BlFacturas();
            oFactura = new MedDAL.DAL.facturas();
            oFactura = oblFacturas.BuscarFactura(iIdFactura);

            if (System.IO.File.Exists(Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFactura.Folio + ".zip")))
            {
                System.IO.FileInfo fFactura = new System.IO.FileInfo(Server.MapPath("~/Archivos/FacturasElectronicasTimbradas/FacturaE-" + oFactura.Folio + ".zip"));

                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fFactura.Name);
                Response.AddHeader("Content-Length", fFactura.Length.ToString());
                Response.ContentType = "application/....";
                Response.WriteFile(fFactura.FullName);
                Response.End();
            }
            else
            {
                ScriptManager.RegisterStartupScript(pnlFormulario, pnlFormulario.GetType(), "alertstock", "alertarFacturas(5);", true);
            }
        }

        private void Eliminar(int iIdFactura)
        {

            //Eliminar primero la partida para la integridad referencial
            oblFacturas = new MedNeg.Facturas.BlFacturas();
            string sDatosBitacora= string.Empty;

            //Guardar los datos del pedido para la bitacora
            oFactura = new MedDAL.DAL.facturas();
            oFactura = oblFacturas.BuscarFactura(iIdFactura);

            sDatosBitacora += "Folio:" + oFactura.Folio.ToString()+" ";
            sDatosBitacora += "Fecha:" + oFactura.Fecha.ToShortDateString()+" ";
            switch (oFactura.Estatus)
            {
                case "1":
                    sDatosBitacora += "Estatus:Pedido ";
                    break;
                case "2":
                     sDatosBitacora +="Estatus:Remitido ";
                    break;
                case "3":
                    sDatosBitacora +="Estatus:Emitida ";
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
            List<MedDAL.DAL.facturas_partida> lstFacturasPartida = new List<MedDAL.DAL.facturas_partida>();
            lstFacturasPartida.AddRange(oQuery);
            //Recorrer el resultado y meterlo al datagridview
            foreach (MedDAL.DAL.facturas_partida oDetalle in lstFacturasPartida)
            {
                sDatosBitacora += "Producto:" + oDetalle.productos.Nombre.ToString()+" ";
                sDatosBitacora += "Cantidad:" + oDetalle.Cantidad.ToString()+ " ";
                sDatosBitacora += "IEPS:" + oDetalle.IEPS.ToString() + " ";
                sDatosBitacora += "Iva:" + oDetalle.Iva.ToString() + " ";
                sDatosBitacora += "Precio:" + oDetalle.Precio.ToString()+ " ";
                sDatosBitacora += "Total:" + Convert.ToDecimal((oDetalle.Cantidad * oDetalle.Precio) + oDetalle.IEPS + oDetalle.Iva)+ ", ";

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
                    lblDatos.Text = "No se pudo eliminar la factura, por favor vuelva a intentarlo";
                }

            }
            else
            {
                lblDatos.Text = "No se pudo eliminar la factura, por favor vuelva a intentarlo";
            }
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

        protected void imbEliminar_Click(object sender, EventArgs e)
        {
            if (pnlCatalogo.Visible && dgvDatos.SelectedIndex != -1)
            {
                Eliminar((int)dgvDatos.SelectedValue);
                MostrarLista();
                CargarCatalogo();
                dgvDatos.SelectedIndex = -1;
                lblAviso.Text = "";
            }
            else
            {
                CargarCatalogo();
                MostrarLista();
                lblAviso.Text = "";
            }
        }

        protected void imbReimprimir_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedIndex >= 0)
            {
                Reimprimir((int)dgvDatos.SelectedValue);
                MostrarLista();
                CargarCatalogo();
                dgvDatos.SelectedIndex = -1;
                lblAviso.Text = "";
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

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
            lblAviso.Text = "";
            pnlFiltroReportes.Visible = true;
            CargarListaReportes();

        }

        protected void imbMostrar_Click(object sender, EventArgs e)
        {
            dgvDatos.SelectedIndex = -1;
            lblAviso.Text = "";
            //0175 GT
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
            MostrarLista();
            //pnlReportes.Visible = false;
        }

        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
            pnlCatalogo.Visible = false;
            pnlFormulario.Visible = false;
            //pnlReportes.Visible = false;   
            Limpia();
        }

        //protected void btnBuscar_Click() { }

        protected void imbNuevo_Click(object sender, EventArgs e)
        {
            CargarFormulario(false);
            Session["accion"] = 1;
            txbFecha.Text = DateTime.Now.ToShortDateString();
            lblAviso.Text = "";
            dgvDatos.SelectedIndex = -1;
            //pnlReportes.Visible = false;
            //lblAviso.Text = "";
            //lblAviso2.Text = "";
            //Habilita();
            //Limpia();
            //0175 GT
            ConfigurarMenuBotones(true, false, false, false, true, true, false, false);
        }

        protected void imbTimbrar_Click(object sender, EventArgs e)
        {
            if (pnlCatalogo.Visible && dgvDatos.SelectedIndex != -1)
            {
                Timbrar((int)dgvDatos.SelectedValue);
                MostrarLista();
                CargarCatalogo();
                dgvDatos.SelectedIndex = -1;
                lblAviso.Text = "";
            }
            else
            {
                CargarCatalogo();
                MostrarLista();
                lblAviso.Text = "";
            }
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
                    if (ValidaFolioRepetido())
                    {
                        if (AddFactura())
                        {
                            //Modifica la manera en que los botones del master estan habilitado o no.
                            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                        }
                        Limpia();
                    }
                    else
                    {
                        lblDatos.Text = "Folio Repetido, no se puede generar el pedido";
                    }                    
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
            txbFolio.Text = "";
            txbPedido.Text = "";
            txbRemision.Text = "";
            txbReceta.Text = "";
            txbCliente.Text = "";
            txbDireccion.Text = "";
            txbPoblacion.Text = "";
            lblAviso.Text = "";
            LimpiarDatosDetalle();
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;

            Session["sTotalFactura"] = 0;
            //Session["lstDetallePartida"] = new List<MedNeg.Facturas.BlDetallePartida>();
            if (((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Count() > 0)
            {
                ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Clear();
            }
            ////pnlReportes.Visible = false;
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
            List<MedDAL.DAL.pedidos_partida> oQuery = new List<MedDAL.DAL.pedidos_partida>();
            oQuery.AddRange(oblPedido.RecuperarPartidaPedido(oPedido.idPedido));
            
            //Session["lstDetallePartida"] = new List<MedNeg.Facturas.BlDetallePartida>();
            if (((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Count()>0)
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
                    bEsEnsamble);

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
            List<MedDAL.DAL.remisiones_partida> oQuery = new List<MedDAL.DAL.remisiones_partida>();
            oQuery.AddRange(oblRemision.RecuperarPartidaRemision(oRemision.idRemision));
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
                    //0087 Identificar si es un producto o un ensamble
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
                    bEsEnsamble);

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
            try
            {
                Session["sIdReceta"] = oReceta.idReceta;
            }
            catch
            {
                Session["sIdReceta"] = 0;
            }

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
            List<MedDAL.DAL.recetas_partida> oQuery = new List<MedDAL.DAL.recetas_partida>();
            oQuery.AddRange(oblReceta.RecuperarPartidaRecetas(oReceta.idReceta));
            if (((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Count() > 0)
            {
                ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Clear();
            }

            Session["sTotalFactura"] = 0;
            //Recorrer el resultado y meterlo al datagridview
            foreach (MedDAL.DAL.recetas_partida oDetalle in oQuery)
            {
                //Incidencia 1548 21/01/2013
                decimal dCantidad, dIeps, dIva, dPrecio, dTotal;
                dCantidad = Convert.ToDecimal(oDetalle.CantidaSurtida);
                dPrecio = Convert.ToDecimal(oDetalle.Precio);
                dTotal = dCantidad * dPrecio;
                dIeps = dTotal * (Convert.ToDecimal(oDetalle.productos.TasaIeps) / 100);
                dIva = dTotal * (Convert.ToDecimal(oDetalle.productos.tipo_iva.Iva) / 100);

                oblDetallePartida = new MedNeg.Facturas.BlDetallePartida(
                    Convert.ToInt32(oDetalle.idProducto),
                    oDetalle.productos.Clave1,
                    oDetalle.productos.Nombre,
                    dCantidad,
                    dIeps,
                    //dTotal * (decimal.Parse(txbIva.Text) / 100)
                    dIva,
                    dPrecio,
                   "",
                  dTotal + dIeps + dIva);

                ((List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"]).Add(oblDetallePartida);
                Session["sTotalFactura"] = Convert.ToDecimal(Session["sTotalFactura"]) + dTotal + dIeps + dIva;
            }

            //Hacer el binding de la data al dgvDatos
            lblTotal.Text = "TOTAL:$" + Session["sTotalFactura"].ToString();
            dgvPartidaDetalle.DataBind();

        }

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
                if (lstProductosAlmacen[0].Cantidad >= dCantidad || (objConfiguracion.iVentasNegativas == 1 && lstProductosAlmacen[0].Cantidad < dCantidad))
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
                        bEsEnsamble);     // 0087 Datos del ensamble

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
                }
                else if (objConfiguracion.iVentasNegativas == 0 && lstProductosAlmacen[0].Cantidad < dCantidad)
                {
                    lblAviso.Text = "El producto que desea agregar no cuenta con suficientes existencias";
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
                if (dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[9].Text == "1")
                    dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[9].Text = "Pedido";
                if (dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[9].Text == "2")
                   dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[9].Text = "Emitida";
                if (dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[9].Text == "3")
                    dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[9].Text = "Emitida";
                if (dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[9].Text == "4")
                    dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[9].Text = "Cobrada";
                if (dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[9].Text == "5")
                    dgvDatos.Rows[dgvDatos.Rows.Count - 1].Cells[9].Text = "Cancelada";

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
        /// Buscar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            dgvDatos.SelectedIndex = -1;
            Buscar(txbBuscar.Text);            
            lblAviso.Text = "";            
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
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

        protected void cmbMetodoPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            string  MetodoPago = cmbMetodoPago.SelectedItem.Text;
            if (MetodoPago == "Depósito en cuenta" || MetodoPago == "Traspaso")
            {
                txbCuentaPago.Enabled = true;
            }
            else
            {
                txbCuentaPago.Enabled = false;
            }
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

        public void CargarListaReportes()
        {
            ListBox lsbReportes = (ListBox)frReportes.FindControl("lsbSeleccionf");
            frReportes.LimpiarPaneles();
            lsbReportes.SelectedIndex = -1;
            lsbReportes.Items.Clear();
            if (Server.MapPath("~\\rptReportes\\Facturacion\\rptPendientesPorSurtir.rpt") != "")
            {
                lsbReportes.Items.Add("Pendientes por surtir");
            }
            if (Server.MapPath("~\\rptReportes\\Facturacion\\rptDetalladoDeFacturas.rpt") != "")
            {
                lsbReportes.Items.Add("Detallado de facturas");
            }
            if (Server.MapPath("rptResumenGeneralDeFacturas.rpt") != "")
            {
                lsbReportes.Items.Add("Resumen general de facturas");
            }
            if (Server.MapPath("rptAntiguedadDeSaldos.rpt") != "")
            {
                lsbReportes.Items.Add("Antiguedad de saldos");
            }
          
        }


        protected void CargarReporte()
        {
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;
            //pnlReportes.Visible = true;
            pnlFiltroReportes.Visible = false;

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsPedidos odsPedidos = new MedDAL.DataSets.dsPedidos();
            odsPedidos = (MedDAL.DataSets.dsPedidos)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsPedidos, "clientes");
            odsPedidos = (MedDAL.DataSets.dsPedidos)LlenarDataSet("select * from facturas", "medicuriConnectionString", odsPedidos, "facturas");
            odsPedidos = (MedDAL.DataSets.dsPedidos)LlenarDataSet("select * from facturas_partida", "medicuriConnectionString", odsPedidos, "facturas_partida");

            //GT 0179
            Session["campoaordenar"] = "";
            Session["sortfield"] = 0;  

            Session["dataset"] = odsPedidos;
            Session["reportdocument"] = "~\\rptReportes\\rptFacturas.rpt";
            Session["titulo"] = "Facturas";
            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));

         

            //Saber si es general o se tiene un usuario seleccionado
            if (dgvDatos.SelectedIndex != -1)
            {
                Session["recordselection"] = "{facturas.idFactura}=" + dgvDatos.SelectedDataKey.Values[0].ToString();
            }
            else
            {
                Session["recordselection"] = "";
            }

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

            
        }

        protected void imbImprimir_Click(object sender, EventArgs e)
        {
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
            CargarReporte();
        }


        #endregion    

        #region SortingPaging

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

        protected void dgvPartidaDetalle_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (List<MedNeg.Facturas.BlDetallePartida>)Session["lstDetallePartida"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvPartidaDetalle.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "SClave" : ViewState["sortexpression"].ToString(), dv, ref dgvPartidaDetalle, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            dgvPartidaDetalle.DataBind();
        }        

        #endregion

        

        

        
    }

}

       

        


        

