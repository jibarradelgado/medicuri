using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using MedNeg.Recetas;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Medicuri
{
    public partial class Recetas : System.Web.UI.Page
    {
        const string cnsSinLineasCredito = "Sin lineas de crédito...", cnsSinEstados = "Sin estados...", cnsSinMunicipios = "Sin municipios...", cnsSinPoblaciones = "Sin poblaciones...", cnsSinColonias = "Sin colonias...";
        BlRecetas blRecetas;
        MedNeg.Usuarios.BlUsuarios oblUsuario;
        MedNeg.Colonias.BlColonias oblColonias;
        MedNeg.Poblaciones.BlPoblaciones oblPoblaciones;
        MedNeg.Municipios.BlMunicipios oblMunicipios;
        MedNeg.Estados.BlEstados oblEstados;
        MedNeg.LineasCredito.BlLineasCredito oblLineasCredito;
        MedNeg.Productos.BlProductos oblProductos;
        MedNeg.BitacoraFaltantes.BlBitacoraFaltantes oblBitacoraFaltantes;
        MedNeg.RecetasPartidaFaltantes.BlRecetasPartidaFaltantes oblRecetasPartidaFaltantes;
        MedNeg.Recetas.BlRecetas oblRecetas;        
        ImageButton imbAceptar, imbMostrar, imbImprimir, imbNuevo, imbEditar, imbReportes,imbCancelar;
        private RadioButton rdbFolio;
        private RadioButton rdbTipo;
        private RadioButton rdbFecha;
        private Button btnBuscar;
        private TextBox txbBuscar;
        Label lblNombreModulo;
        AjaxControlToolkit.CalendarExtender oCalendarExtender;
        //Objetos de las entities
        MedDAL.DAL.bitacora oBitacora;
        MedDAL.DAL.clientes oClienteNuevo = new MedDAL.DAL.clientes();
        IQueryable<MedDAL.DAL.tipos> iqrTiposVendedor;


        public Recetas()
        {
            this.InitComplete += new EventHandler(Recetas_InitComplete);
        }

        void Recetas_InitComplete(object sender, EventArgs e)
        {
            oblColonias = new MedNeg.Colonias.BlColonias();
            oblPoblaciones = new MedNeg.Poblaciones.BlPoblaciones();
            oblMunicipios = new MedNeg.Municipios.BlMunicipios();
            oblEstados = new MedNeg.Estados.BlEstados();
            oblLineasCredito = new MedNeg.LineasCredito.BlLineasCredito();
            oblProductos = new MedNeg.Productos.BlProductos();


            blRecetas = new BlRecetas();
            cargaDdlTipos();
            txbFecha.Text = DateTime.Today.ToShortDateString();

        }

        private void cargaDdlTipos()
        {
            cmbTipoReceta.DataTextField = "Nombre";
            cmbTipoReceta.DataValueField = "idTipo";
            cmbTipoReceta.DataSource = blRecetas.buscarTiposRecetas();
            cmbTipoReceta.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            blRecetas = new BlRecetas();
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';

            try
            {
                Master.FindControl("btnReportes").Visible = false;

                cPermiso = (char)htbPermisos["recetas"];
                oblColonias = new MedNeg.Colonias.BlColonias();
                oblPoblaciones = new MedNeg.Poblaciones.BlPoblaciones();
                oblMunicipios = new MedNeg.Municipios.BlMunicipios();
                oblEstados = new MedNeg.Estados.BlEstados();
                imbAceptar = (ImageButton)Master.FindControl("imgBtnAceptar");
                imbAceptar.Click += new ImageClickEventHandler(imbAceptar_Click);
                imbMostrar = (ImageButton)Master.FindControl("imgBtnMostrar");
                imbMostrar.Click += new ImageClickEventHandler(imbMostrar_Click);
                imbNuevo = (ImageButton)Master.FindControl("imgBtnNuevo");
                imbNuevo.Click += new ImageClickEventHandler(imbNuevo_Click);
                imbEditar = (ImageButton)Master.FindControl("imgBtnEditar");
                imbEditar.Click += new ImageClickEventHandler(imbEditar_Click);
                imbImprimir = (ImageButton)Master.FindControl("imgBtnImprimir");
                imbImprimir.Click += new ImageClickEventHandler(imbImprimir_Click);
                imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
                imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
                rdbFolio = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbFolio.Text = "Folio";
                rdbTipo = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbTipo.Text = "Tipo";
                rdbFecha = (RadioButton)Master.FindControl("rdbFiltro3");
                rdbFecha.Text = "Fecha";
                rdbFecha.AutoPostBack = true;
                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Click += new EventHandler(btnBuscar_Click);
                txbBuscar = (TextBox)Master.FindControl("txtBuscar");
                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "Recetas";
                lblAvisosVendedores.Text = "";

                //GT 0175
                imbReportes = (ImageButton)Master.FindControl("imgBtnReportes");
                imbReportes.Click += new ImageClickEventHandler(this.imbReportes_Click);                

                oCalendarExtender = new AjaxControlToolkit.CalendarExtender();
                oCalendarExtender.TargetControlID = "txbBuscar";

                oblBitacoraFaltantes = new MedNeg.BitacoraFaltantes.BlBitacoraFaltantes();
                oblRecetasPartidaFaltantes = new MedNeg.RecetasPartidaFaltantes.BlRecetasPartidaFaltantes();
                oblRecetas = new BlRecetas();
                oblProductos = new MedNeg.Productos.BlProductos();

                txbCantRecetada.TextChanged += new EventHandler(this.txbCantRecetada_TextChanged);
                txbCantSurtida.TextChanged += new EventHandler(this.txbCantSurtida_TextChanged);
                

                if (!IsPostBack)
                {   
                    oCalendarExtender.Enabled = false;
                    user = blRecetas.buscarUsuario(Session["usuario"].ToString());
                    Session["lstrecetaspartida"] = new List<MedDAL.DAL.recetas_partida>();
                    Session["lstrecetaspartidaedicion"] = new List<MedDAL.DAL.recetas_partida>();
                    lProductos = new List<Producto>();
                    Session["resultadoquery"] = "";
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
                    Session["recetasIdCliente"] = 0;
                    Session["recetasIdCausesCie"] = null;
                    Session["reporteactivo"] = 0;
                    Session["reportdocument"] = "";
                    Session["titulo"] = "";
                    estadoActual = 0;
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                }

                dgvPartidaDetalle.DataSource = ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"]);
                dgvPartidaDetalle.DataBind();
                
                
                if (estadoActual == 2)
                {
                    int iContador = ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"]).Count - ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartidaedicion"]).Count;
                    int i = 0;

                    foreach (GridViewRow oRow in dgvPartidaDetalle.Rows)
                    {
                        oRow.Cells[9].Controls.Clear();
                        i++;
                        if (i == iContador) break;
                    }
                }

                gdvContactosCliente.DataSource = ((List<MedDAL.DAL.clientes_contacto>)Session["lstContactosDB"]);
                gdvContactosCliente.DataBind();
                gdvContactosCliente.DataKeyNames = new String[] { "idContacto" };
            }
            catch (NullReferenceException)
            {
                if (!ClientScript.IsStartupScriptRegistered("alertsession"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alertsession", "alertarSesion();", true);
                }
                divFormulario.Visible = false;
                divListado.Visible = false;
                
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }

        void txbCantRecetada_TextChanged(object sender, EventArgs e)
        {
            //txbCantSurtida.Focus();
        }

        void txbCantSurtida_TextChanged(object sender, EventArgs e)
        {
            //rdbIntencionPrimera.Focus();
        }

        void rdbFecha_CheckedChanged(object sender, EventArgs e)
        {
            oCalendarExtender.Enabled = rdbFecha.Checked;
        }

        void btnBuscar_Click(object sender, EventArgs e)
        {
            /*Folio, paciente,fecha*/
            int iFiltro = 1;
            if (rdbFolio.Checked)
                iFiltro = 1;
            if (rdbTipo.Checked)
                iFiltro = 2;
            if (rdbFecha.Checked)
                iFiltro = 3;

            estadoActual = 4;
            divListado.Visible = true;
            divFormulario.Visible = false;
            

            DateTime dFecha;

            if (iFiltro == 3 && !DateTime.TryParse(txbBuscar.Text, out dFecha))
            {
                iFiltro = -1;
            }

            if (iFiltro != -1)
            {
                oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
                oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());

                if ((bool)oUsuario.FiltradoActivado)
                {
                    cargadgvListado(blRecetas.BuscarReceta(txbBuscar.Text, iFiltro, idAlmacen));
                }
                else
                { 
                    cargadgvListado(blRecetas.BuscarReceta(txbBuscar.Text, iFiltro));
                }
            }
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }

        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            //GT 0175
            lblAviso.Text = "";
            lblDatos.Text = "";
            divListado.Visible = false;
            divFormulario.Visible = false;
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
           
        }

        void imbEditar_Click(object sender, ImageClickEventArgs e)
        {
            if (dgvListado.SelectedIndex != -1)
            {   
                estadoActual = 2;
                lblAviso.Text = "";
                lblDatos.Text = "";
                List<MedDAL.Recetas.RecetasView> lTemp = new List<MedDAL.Recetas.RecetasView>();
                lTemp.AddRange(lRecetas);
                int idReceta = (int)dgvListado.SelectedValue;
                recetaParaEditar = oblRecetas.BuscarReceta(idReceta);
                cargarReceta(recetaParaEditar);
                txbPieTituloExp.Enabled = false;
                txbPieRegEspecialidad.Enabled = false;
                divListado.Visible = false;
                divFormulario.Visible = true;
                
                //0175 GT
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);

                //GT 0579 14-10-2011 Debido a que en la funcion que se invoca lineas arriba en este metodo cargarReceta(recetaParaEditar) se manda a llamar a la funcion deshabilitaTxtDdlHijos que deshabilita un div el de formulario deshabilita el combo de Estado y nunca se puede cambiar el estado de una receta al editar, por lo tanto aqui se habilita el combo para no afectar la función de deshabilitaTxtDdlHijos
                //cmbEstatus.Enabled = true;
            }
            else
            {
                lblAviso.Text = "";
                lblDatos.Text = "";
                divListado.Visible = true;
                divFormulario.Visible = false;
                
                cargadgvListado(blRecetas.BuscarTodasRecetas());
                //0175 GT
                ConfigurarMenuBotones(false, false, false, true, true, true, false, false);
            }
        }

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);            
        }

        void imbNuevo_Click(object sender, ImageClickEventArgs e)
        {
            estadoActual = 1;
            lblAviso.Text = "";
            lblDatos.Text = "";
            dgvListado.SelectedIndex = -1;
            imbAgregarDetalle.Enabled = true;
            LimpiarReceta();
            txbPieTituloExp.Enabled = false;
            txbPieRegEspecialidad.Enabled = false;
            CargaDdlSurtidoEstados(false);
            CargaDdlExpedidoEstados(false);
            CargaDdlSurtidoExpedidoUsuarioAlmacen();
            CargarCmbLineasCredito();
            divFormulario.Visible = true;
            divListado.Visible = false;
            // Saber si estan activados los folios automaticos y poner su valor por default
            // Se almacena en una variable de sesión para comparar que se esta respetando el formato automatico
            // y validar que no haya cambiado el folio de pedidos debido a otro registro mientras se hacia el actual 
            oblRecetas = new BlRecetas();
            Session["iFolioAutomatico"] = oblRecetas.RecuperaFolioAutomatico(Server.MapPath("~/Archivos/Configuracion.xml"));
            txbFolio.Text = Session["iFolioAutomatico"].ToString();
            
            //0175 GT
            ConfigurarMenuBotones(true, false, false, false, true, true, false, false);
        }

        void imbMostrar_Click(object sender, ImageClickEventArgs e)
        {
            estadoActual = 4;
            lblAviso.Text = "";
            dgvListado.SelectedIndex = -1;
            divListado.Visible = true;
            divFormulario.Visible = false;

            oblUsuario = new MedNeg.Usuarios.BlUsuarios();
            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
            oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());

            if ((bool)oUsuario.FiltradoActivado)
            {
                cargadgvListado(blRecetas.BuscarTodasRecetas(idAlmacen));
            }
            else
            {
                cargadgvListado(blRecetas.BuscarTodasRecetas());
            }

            
            //0175 GT
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }

        void imbAceptar_Click(object sender, ImageClickEventArgs e)
        {
            switch (estadoActual)
            {
                case 1:
                case 2: guardarReceta();
                    lblAviso.Text = "";
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, true, false, true, true);
                    break;
            }
        }

        private MedDAL.DAL.productos MedDalProducto
        {
            get { return (MedDAL.DAL.productos)Session["RecProducto"]; }
            set { Session["RecProducto"] = value; }
        }

        private int idAlmacen
        {
            get { return user.idAlmacen; }
        }

        private MedDAL.DAL.usuarios user
        {
            get { return (MedDAL.DAL.usuarios)Session["RecetasUsuario"]; }
            set { Session["RecetasUsuario"] = value; }
        }

        private List<Producto> lProductos
        {
            get { return (List<Producto>)Session["RecetasListaProductos"]; }
            set { Session["RecetasListaProductos"] = value; }
        }

        /// <summary>
        /// 1 nuevo 
        /// 2 editar
        /// 3 eliminar
        /// 4 mostrar
        /// </summary>
        private int estadoActual
        {
            get { return (int)Session["RecetasEstadoActual"]; }
            set { Session["RecetasEstadoActual"] = value; }
        }

        private MedDAL.DAL.vendedores medico
        {
            get { return (MedDAL.DAL.vendedores)Session["RecetasMedicoVendedor"]; }
            set { Session["RecetasMedicoVendedor"] = value; }
        }

        /// <summary>
        /// todas las recetas mostradas
        /// </summary>
        private IQueryable<MedDAL.Recetas.RecetasView> lRecetas
        {
            get { return (IQueryable<MedDAL.Recetas.RecetasView>)Session["recetasRecetasMostradas"]; }
            set { Session["recetasRecetasMostradas"] = value; }
        }

        private MedDAL.DAL.recetas recetaParaEditar
        {
            get { return (MedDAL.DAL.recetas)Session["recetasRecetaParaEditar"]; }
            set { Session["recetasRecetaParaEditar"] = value; }
        }


        private void inicializaGuiPartida(bool borrartxbClave1, bool borrartxbProducto)
        {
            if (borrartxbClave1)
                txbClave.Text = string.Empty;
            if (borrartxbProducto)
                txbProducto.Text = string.Empty;


            //GT 14-10-2011 0578 Se comenta esta linea por que se invoca con el text_changed de producto o clave y se borra lo que se tenia capturado en el campo cantidad surtida y el usuario se confunde, este cambio tambien afecta el metodo agregar detalle y se pone el respectivo comentario con el tag de la incidencia 0578
            //txbCantRecetada.Text = txbCantSurtida.Text = "0";

            rdbIntencionPrimera.Checked = true;
            //rdbCauseNo.Checked = true;
            //ddlPrecios.Items.Clear();
            ddlProductoLotes.Items.Clear();
            ddlProductoSeries.Items.Clear();
            txbClave.Focus();
        }

        int? tryParse(String s)
        {
            if (s.Equals(string.Empty))
                return null;

            return int.Parse(s);
        }

        private bool ValidarDetalle()
        {            
            if (((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"]).Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void guardarReceta()
        {
            bool bValidar = true;
            bValidar = ValidarDetalle();
            if ((estadoActual == 1 && bValidar) || (estadoActual == 2 && recetaParaEditar.EstatusMedico.Equals("2")) || (estadoActual == 2 && recetaParaEditar.EstatusMedico.Equals("1")))
            //if (estadoActual == 1 || estadoActual == 2)
            {
                string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
                bool bAutomatico = false;
                bool bStockMin = false;
                try
                {
                    MedDAL.DAL.recetas receta = null;

                    if (estadoActual == 1)
                    {
                        receta = new MedDAL.DAL.recetas();
                    }
                    else if (estadoActual == 2)
                    {
                        receta = recetaParaEditar;
                        Session["recetasIdCliente"] = recetaParaEditar.idCliente;
                    }

                    receta.idTipoReceta = int.Parse(cmbTipoReceta.SelectedValue);
                    receta.idVendedor = user.idUsuario;
                    receta.idCliente = int.Parse(Session["recetasIdCliente"].ToString());
                    receta.idAlmacen = idAlmacen;
                    receta.idEstadoExp = Convert.ToInt32(cmbExpedidoEnEstados.SelectedValue);

                    receta.idMunicipioExp = Convert.ToInt32(cmbExpedidoEnMunicipios.SelectedValue);
                    receta.idPoblacionExp = Convert.ToInt32(cmbExpedidoEnPoblaciones.SelectedValue);
                    receta.idColoniaExp = Convert.ToInt32(cmbExpedidoEnColonias.SelectedValue);
                    receta.idEstadoSur = Convert.ToInt32(cmbSurtidoEnEstados.SelectedValue);
                    receta.idMunicipioSur = Convert.ToInt32(cmbSurtidoEnMunicipios.SelectedValue);
                    receta.idPoblacionSur = Convert.ToInt32(cmbSurtidoEnPoblaciones.SelectedValue);
                    receta.idColoniaSur = Convert.ToInt32(cmbSurtidoEnColonias.SelectedValue);
                    receta.idVendedor = medico.idVendedor;
                    if (estadoActual == 1 && Session["iFolioAutomatico"].ToString() == txbFolio.Text)
                    {
                        receta.Folio = blRecetas.RecuperaFolioAutomatico(sRutaArchivoConfig).ToString();
                        bAutomatico = true;
                    }
                    else
                    {
                        receta.Folio = txbFolio.Text;
                    }
                    receta.ClaveMed = txbPieCedulaProf.Text;
                    receta.Paciente = txbCliente.Text;
                    //receta.Domicilio =
                    receta.Telefono = txbClienteTelefono.Text;
                    //receta.Celular
                    //receta.CorreoElectronico=
                    receta.Fecha = DateTime.Parse(txbFecha.Text);
                    receta.EstatusMedico = cmbEstatus.SelectedValue;
                    receta.Estatus = "1";

                    //Validar si la receta ya existe con el folio dado
                    MedDAL.DAL.recetas oRecetaValidar = oblRecetas.BuscarRecetaFolioRepetido(receta.Folio);
                    if ((oRecetaValidar == null && estadoActual == 1) || estadoActual == 2)
                    {

                        //JID Aqui se guarda o se edita la receta
                        if (estadoActual == 1)
                        {
                            blRecetas.guardarReceta(receta);
                            if (bAutomatico)
                            {
                                blRecetas.ActualizarFolioReceta(sRutaArchivoConfig);
                            }
                        }
                        else if (estadoActual == 2)
                        {
                            blRecetas.EditarReceta(receta);
                        }

                        MedDAL.DAL.recetas oReceta = new MedDAL.DAL.recetas();
                        if (estadoActual == 1)
                        {
                            oReceta = blRecetas.BuscarRecetaFolioRepetido(receta.Folio);
                        }

                        //Aqui es un nuevo registro
                        if (estadoActual == 1)
                        {

                            //blRecetas.guardarRecetasPartida(lPartida);
                            foreach (MedDAL.DAL.recetas_partida oRecetasPartida in (List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"])
                            {
                                oRecetasPartida.idReceta = oReceta.idReceta;
                                MedDAL.DAL.recetas_partida oRecetasPartidaNuevo = new MedDAL.DAL.recetas_partida();
                                oRecetasPartidaNuevo.idReceta = oReceta.idReceta;
                                oRecetasPartidaNuevo.idProducto = oRecetasPartida.idProducto;
                                oRecetasPartidaNuevo.idLineaCredito = oRecetasPartida.idLineaCredito;
                                oRecetasPartidaNuevo.idCausesCie = oRecetasPartida.idCausesCie;
                                oRecetasPartidaNuevo.CantidadRecetada = oRecetasPartida.CantidadRecetada;
                                oRecetasPartidaNuevo.CantidaSurtida = oRecetasPartida.CantidaSurtida;
                                oRecetasPartidaNuevo.Precio = oRecetasPartida.Precio;
                                oRecetasPartidaNuevo.Lote = oRecetasPartida.Lote;
                                oRecetasPartidaNuevo.NoSerie = oRecetasPartida.NoSerie;
                                oRecetasPartidaNuevo.PrimeraIntencion = oRecetasPartida.PrimeraIntencion;
                                oRecetasPartidaNuevo.SegundaIntencion = oRecetasPartida.SegundaIntencion;
                                oRecetasPartidaNuevo.Cause = oRecetasPartida.Cause;
                                oRecetasPartidaNuevo.Factura = oRecetasPartida.Factura;
                                blRecetas.NuevoRegistroPartida(oRecetasPartidaNuevo);

                                if (oRecetasPartida.CantidadRecetada > oRecetasPartida.CantidaSurtida)
                                {
                                    MedDAL.DAL.bitacora_faltantes oBitacoraFaltantes = new MedDAL.DAL.bitacora_faltantes();
                                    oBitacoraFaltantes.idReceta = oReceta.idReceta;
                                    oBitacoraFaltantes.idProducto = Convert.ToInt32(oRecetasPartida.idProducto);
                                    oBitacoraFaltantes.idAlmacen = int.Parse(oReceta.idAlmacen.ToString());
                                    oBitacoraFaltantes.Cantidad = Convert.ToInt32(oRecetasPartida.CantidadRecetada - oRecetasPartida.CantidaSurtida);
                                    oblBitacoraFaltantes.NuevoRegistro(oBitacoraFaltantes);

                                    MedDAL.DAL.recetas_partida_faltantes oRecetasPartidaFaltantes = new MedDAL.DAL.recetas_partida_faltantes();
                                    oRecetasPartidaFaltantes.idReceta = oReceta.idReceta;
                                    oRecetasPartidaFaltantes.idProducto = Convert.ToInt32(oRecetasPartida.idProducto);
                                    oRecetasPartidaFaltantes.idAlmacen = int.Parse(oReceta.idAlmacen.ToString());
                                    oRecetasPartidaFaltantes.Fecha = DateTime.Now;
                                    oblRecetasPartidaFaltantes.NuevoRegistro(oRecetasPartidaFaltantes);
                                }

                                MedDAL.DAL.productos_almacen oProductoAlmacen = oblProductos.ObtenerProductoLote(idAlmacen, Convert.ToInt32(oRecetasPartida.idProducto), oRecetasPartida.Lote);
                                MedDAL.DAL.productos_almacen_stocks oProductoAlmacenStocks = oblProductos.ObtenerProductoAlmacenStock(idAlmacen, Convert.ToInt32(oRecetasPartida.idProducto));
                                if (oProductoAlmacen.Cantidad - oRecetasPartidaNuevo.CantidaSurtida <= oProductoAlmacenStocks.StockMin)
                                {
                                    bStockMin = true;
                                }
                            }
                            blRecetas.disminuirExistencias(idAlmacen, (List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"]);

                            if (bStockMin)
                            {
                                ScriptManager.RegisterStartupScript(upnForm, upnForm.GetType(), "alertstock", "alertarStock(1);", true);
                            }
                            //Se actualizan las variables de sesion para que queden en blanco
                            Session["recetasIdCliente"] = 0;
                            Session["recetasIdCausesCie"] = null;
                        }
                        //Aqui es edicion
                        else if (estadoActual == 2)
                        {
                            //JID se eliminan todas las recetas de la partida
                            //blRecetas.eliminaRecetasPartida(receta.idReceta);

                            //JID se coloca la partida ya existente en la receta
                            foreach (MedDAL.DAL.recetas_partida oRecetasPartida in (List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartidaedicion"])
                            {
                                MedDAL.DAL.recetas_partida oRecetasPartidaNuevo = new MedDAL.DAL.recetas_partida();
                                oRecetasPartidaNuevo.idReceta = receta.idReceta;
                                oRecetasPartidaNuevo.idProducto = oRecetasPartida.idProducto;
                                oRecetasPartidaNuevo.idLineaCredito = oRecetasPartida.idLineaCredito;
                                oRecetasPartidaNuevo.idCausesCie = oRecetasPartida.idCausesCie;
                                oRecetasPartidaNuevo.CantidadRecetada = oRecetasPartida.CantidadRecetada;
                                oRecetasPartidaNuevo.CantidaSurtida = oRecetasPartida.CantidaSurtida;
                                oRecetasPartidaNuevo.Precio = oRecetasPartida.Precio;
                                oRecetasPartidaNuevo.Lote = oRecetasPartida.Lote;
                                oRecetasPartidaNuevo.NoSerie = oRecetasPartida.NoSerie;
                                oRecetasPartidaNuevo.PrimeraIntencion = oRecetasPartida.PrimeraIntencion;
                                oRecetasPartidaNuevo.SegundaIntencion = oRecetasPartida.SegundaIntencion;
                                oRecetasPartidaNuevo.Cause = oRecetasPartida.Cause;
                                oRecetasPartidaNuevo.Factura = oRecetasPartida.Factura;

                                blRecetas.NuevoRegistroPartida(oRecetasPartidaNuevo);

                                if (oRecetasPartida.CantidadRecetada > oRecetasPartida.CantidaSurtida)
                                {
                                    MedDAL.DAL.bitacora_faltantes oBitacoraFaltantes = new MedDAL.DAL.bitacora_faltantes();
                                    oBitacoraFaltantes.idReceta = receta.idReceta;
                                    oBitacoraFaltantes.idProducto = Convert.ToInt32(oRecetasPartida.idProducto);
                                    oBitacoraFaltantes.idAlmacen = int.Parse(oReceta.idAlmacen.ToString());
                                    oblBitacoraFaltantes.NuevoRegistro(oBitacoraFaltantes);

                                    MedDAL.DAL.recetas_partida_faltantes oRecetasPartidaFaltantes = new MedDAL.DAL.recetas_partida_faltantes();
                                    oRecetasPartidaFaltantes.idReceta = receta.idReceta;
                                    oRecetasPartidaFaltantes.idProducto = Convert.ToInt32(oRecetasPartida.idProducto);
                                    oRecetasPartidaFaltantes.idAlmacen = int.Parse(oReceta.idAlmacen.ToString());
                                    oblRecetasPartidaFaltantes.NuevoRegistro(oRecetasPartidaFaltantes);
                                }

                                MedDAL.DAL.productos_almacen oProductoAlmacen = oblProductos.ObtenerProductoLote(idAlmacen, Convert.ToInt32(oRecetasPartidaNuevo.idProducto), oRecetasPartida.Lote);
                                MedDAL.DAL.productos_almacen_stocks oProductoAlmacenStocks = oblProductos.ObtenerProductoAlmacenStock(idAlmacen, Convert.ToInt32(oRecetasPartida.idProducto));
                                if (oProductoAlmacen.Cantidad - oRecetasPartidaNuevo.CantidaSurtida <= oProductoAlmacenStocks.StockMin)
                                {
                                    bStockMin = true;
                                }
                            }

                            if (receta.EstatusMedico == "2")
                            {
                                //JID Aquí solo se agregan las partidas hechas en edición y se hace la reducción de stock                       
                                blRecetas.disminuirExistencias(idAlmacen, ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartidaedicion"]));

                                if (bStockMin)
                                {
                                    ScriptManager.RegisterStartupScript(upnForm, upnForm.GetType(), "alertstock", "alertarStock(1);", true);
                                }
                            }
                            //JID si la receta es cancelada
                            if (receta.EstatusMedico == "4")
                            {
                                blRecetas.aumentarExistencias((int)recetaParaEditar.idAlmacen, ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"]));
                            }
                            //blRecetas.guardarRecetasPartida(lPartida);
                            //List<Producto> lProductosAgregados = new List<Producto>();

                            //Editar esto con cuidado!!
                            /*
                            foreach (Producto p in lstProductos)
                            {
                                if (p.agregadoPorEdicionDePartida)
                                    lProductosAgregados.Add(p);
                            }
                            blRecetas.disminuirExistencias(idAlmacen, lProductosAgregados);*/
                        }
                        lblDatos.Text = "Proceso finalizado con éxito";
                    }
                    else
                    {
                        lblDatos.Text = "El folio que desea ingresar ya existe";
                    }
                }
                catch (Exception ex)
                {
                    lblDatos.Text = "Error: " + ex.Message;
                }
            }
            else
            {
                if (!bValidar)
                {
                    lblDatos.Text = "Atencion: Debe de agregar mínimo un producto o ensamble";
                }
                else
                {
                    lblDatos.Text = "Error: No se puede editar una receta con status diferente de \"Parcial\" o \"Surtida\"";
                }
            }
        }

        protected void LimpiarReceta()
        {
            HabilitaTxtDdlHijos(divFormulario);

            ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"]).RemoveRange(0, ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"]).Count);

            Session["lstrecetaspartidaedicion"] = new List<MedDAL.DAL.recetas_partida>();
            dgvPartidaDetalle.DataBind();
            txbFecha.Text = DateTime.Now.ToShortDateString();
        }

        protected void cargarReceta(MedDAL.DAL.recetas receta)
        {
            #region cargarEncabezado
            cargaDdlTipos();
            cmbTipoReceta.SelectedValue = receta.idTipoReceta.ToString();
            CargaDdlSurtidoEstados(true);
            CargaDdlExpedidoEstados(true);
            CargarCmbLineasCredito();
            /*cmbExpedidoEnEstados.SelectedValue=receta.idEstadoExp.ToString();
            CargaDdlExpedidoMunicipios();
            cmbExpedidoEnMunicipios.SelectedValue =receta.idMunicipioExp.ToString();
            CargaDdlExpedidoPoblaciones();
            cmbExpedidoEnPoblaciones.SelectedValue = receta.idPoblacionExp.ToString();
            CargaDdlExpedidoColonia();
            cmbExpedidoEnColonias.SelectedValue = receta.idColoniaExp.ToString();
            cmbSurtidoEnEstados.SelectedValue = receta.idEstadoSur.ToString();
            CargaDdlExpedidoMunicipios();
            cmbSurtidoEnMunicipios.SelectedValue = receta.idMunicipioSur.ToString();
            CargaDdlExpedidoPoblaciones();
            cmbSurtidoEnPoblaciones.SelectedValue = receta.idPoblacionSur.ToString();
            CargaDdlExpedidoColonia();
            cmbSurtidoEnColonias.SelectedValue = receta.idColoniaSur.ToString();*/
            txbFolio.Text = receta.Folio;
            txbPieCedulaProf.Text = receta.ClaveMed;
            buscaMedico();
            cmbEstatus.SelectedValue = receta.EstatusMedico;
            txbCliente.Text = receta.Paciente;            
            txbNumeroSeguroSocial.Text = receta.clientes == null ? "" : receta.clientes.Clave1;            
            txbClienteTelefono.Text = receta.Telefono;
            txbFecha.Text = receta.Fecha.ToShortDateString();            
            #endregion

            #region cargarDetalle

            /*List<Producto> lProductosTemp= new List<Producto>();
            IQueryable<MedDAL.DAL.recetas_partida> iPartida = blRecetas.RecuperarPartidaRecetas(recetaParaEditar.idReceta);*/

            List<MedDAL.DAL.recetas_partida> lstRecetasPartida = new List<MedDAL.DAL.recetas_partida>();
            lstRecetasPartida.AddRange(receta.recetas_partida);

            Session["lstrecetaspartidaedicion"] = new List<MedDAL.DAL.recetas_partida>();
            Session["lstrecetaspartida"] = lstRecetasPartida;
            dgvPartidaDetalle.DataSource = lstRecetasPartida;
            dgvPartidaDetalle.DataBind();

            foreach (GridViewRow oRow in dgvPartidaDetalle.Rows)
            {
                oRow.Cells[9].Controls.Clear();
            }

            #endregion

            //if (!receta.EstatusMedico.Equals("2") && !receta.Estatus.Equals("1"))
            //{
            //    imbAgregarDetalle.Visible = false;
            //    deshabilitaTxtDdlHijos(divFormulario);
            //}
            //else
            //{
                imbAgregarDetalle.Visible = true;
                HabilitaTxtDdlHijos(Table1);
                deshabilitaTxtDdlHijos(PanelDatos);
                deshabilitaTxtDdlHijos(pPie);
            //}
                if (cmbEstatus.SelectedValue == "1")
                {
                    cmbEstatus.Items[1].Enabled = false;
                    cmbEstatus.Items[2].Enabled = false;
                    cmbEstatus.Enabled = true;

                    imbAgregarDetalle.Visible = false;
                }
                else if (cmbEstatus.SelectedValue == "2")
                {
                    cmbEstatus.Items[1].Enabled = true;
                    cmbEstatus.Items[2].Enabled = true;
                    cmbEstatus.Enabled = true;

                    imbAgregarDetalle.Visible = true;
                }
                else if (cmbEstatus.SelectedValue == "3" || cmbEstatus.SelectedValue == "4")
                {
                    cmbEstatus.Enabled = false;
                    imbAgregarDetalle.Visible = false;
                }

        }

        protected void txbClave_TextChanged(object sender, EventArgs e)
        {
            try
            {
                inicializaGuiPartida(false, true);
                MedDalProducto = blRecetas.buscaProducto(txbClave.Text);
                txbProducto.Text = MedDalProducto.Nombre;
                txbCantRecetada.Focus();
                /*cargaDDlPrecios();*/
                cargaDDLProductosLotes();
                cargaDDLProductosSeries();
            }
            catch
            {
                lAvisosProducto.Text = "No se encontró el producto";
            }
        }

        private void HabilitaTxtDdlHijos(Control c)
        {
            if (c is TextBox)
            {
                ((TextBox)c).Enabled = true;
                ((TextBox)c).Text = "";
            }

            if (c is AjaxControlToolkit.ComboBox)
                ((AjaxControlToolkit.ComboBox)c).Enabled = true;

            if (c is DropDownList)
                ((DropDownList)c).Enabled = true;

            foreach (Control cHijo in c.Controls)
                HabilitaTxtDdlHijos(cHijo);
        }

        private void deshabilitaTxtDdlHijos(Control c)
        {
            if (c is TextBox)
            {

                ((TextBox)c).Enabled = false;
            }

            if (c is AjaxControlToolkit.ComboBox)
                ((AjaxControlToolkit.ComboBox)c).Enabled = false;


            if (c is DropDownList)
                ((DropDownList)c).Enabled = false;

            foreach (Control cHijo in c.Controls)
                deshabilitaTxtDdlHijos(cHijo);

        }

        private void cargaDDlPrecios()
        {
            //ddlPrecios.Items.Clear();
            //ddlPrecios.Items.Add(MedDalProducto.PrecioMinimo.ToString());
            //ddlPrecios.Items.Add(MedDalProducto.Precio1.ToString());
            //ddlPrecios.Items.Add(MedDalProducto.Precio2.ToString());
            //ddlPrecios.Items.Add(MedDalProducto.Precio3.ToString());
            //ddlPrecios.Items.Add(MedDalProducto.PrecioPublico.ToString());
        }

        private void cargaDDLProductosLotes()
        {
            IQueryable<string> lotes = blRecetas.buscaLotesProducto(MedDalProducto.idProducto, idAlmacen);
            foreach (string lote in lotes)
                ddlProductoLotes.Items.Add(lote);

        }

        private void cargaDDLProductosSeries()
        {
            IQueryable<string> series = blRecetas.buscaSeriesProducto(MedDalProducto.idProducto, idAlmacen);
            foreach (string serie in series)
                ddlProductoSeries.Items.Add(serie);
        }

        private void cargadgvListado(IQueryable<MedDAL.Recetas.RecetasView> oQuery)
        {
            Session["resultadoquery"] = oQuery;
            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Recetas.RecetasView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Folio ASC";            

            lRecetas = oQuery;
            dgvListado.DataSource = dv;
            dgvListado.DataBind();

        }

        protected void txbProducto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                inicializaGuiPartida(true, false);
                MedDalProducto = blRecetas.buscaProductoNombre(txbProducto.Text);
                txbClave.Text = MedDalProducto.Clave1;
                txbCantRecetada.Focus();
                cargaDDlPrecios();
                cargaDDLProductosLotes();
                cargaDDLProductosSeries();
            }
            catch
            {
                lAvisosProducto.Text = "No se encontró el producto";
            }
        }

        protected void imbAgregarDetalle_Click(object sender, ImageClickEventArgs e)
        {
            lblAviso.Text = "";
            int iIdProducto = MedDalProducto.idProducto;

            MedNeg.Usuarios.BlUsuarios oblUsuarios = new MedNeg.Usuarios.BlUsuarios();
            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
            oUsuario = (MedDAL.DAL.usuarios)oblUsuarios.Buscar(int.Parse(Session["usuarioid"].ToString()));

            MedNeg.Productos.BlProductos oblProducto = new MedNeg.Productos.BlProductos();
            List<MedDAL.DAL.productos_almacen> lstProductosAlmacen = new List<MedDAL.DAL.productos_almacen>();
            lstProductosAlmacen.AddRange(oblProducto.ObtenerExistenciaProducto(iIdProducto, oUsuario.idAlmacen, ddlProductoLotes.SelectedValue, ddlProductoSeries.SelectedValue));

            string sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            MedNeg.Configuracion.BlConfiguracion oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            MedDAL.Configuracion.DALConfiguracion objConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
            objConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

            decimal dCantidad = decimal.Parse(txbCantSurtida.Text);

            if (lstProductosAlmacen.Count != 0)
            {
                if ((lstProductosAlmacen[0].Cantidad >= dCantidad || (objConfiguracion.iVentasNegativas == 1 && lstProductosAlmacen[0].Cantidad < dCantidad)) && lstProductosAlmacen[0].FechaCaducidad.Value > DateTime.Today)
                {            
                    MedDAL.DAL.recetas_partida oRecetaPartida = new MedDAL.DAL.recetas_partida();
                    oRecetaPartida.idProducto = MedDalProducto.idProducto;
                    oRecetaPartida.idLineaCredito = int.Parse(cmbLineasCredito.SelectedValue);
                    oRecetaPartida.CantidadRecetada = decimal.Parse(txbCantRecetada.Text);
                    oRecetaPartida.CantidaSurtida = decimal.Parse(txbCantSurtida.Text);
                    oRecetaPartida.Precio = MedDalProducto.PrecioPublico;
                    oRecetaPartida.Lote = ddlProductoLotes.SelectedValue;
                    oRecetaPartida.NoSerie = ddlProductoSeries.SelectedValue;
                    oRecetaPartida.PrimeraIntencion = rdbIntencionPrimera.Checked;
                    oRecetaPartida.SegundaIntencion = rdbIntencionSegunda.Checked;

                    MedNeg.Causes.BlCauses oBLCauses = new MedNeg.Causes.BlCauses();
                    MedDAL.DAL.causes_cie oCausesCie = oBLCauses.BuscarCie(txbClaveCie.Text);
                    if (oCausesCie != null)
                    {
                        oRecetaPartida.Cause = true;
                        oRecetaPartida.idCausesCie = oCausesCie.idCauseCie;
                        oRecetaPartida.causes_cie = new MedDAL.DAL.causes_cie();
                        oRecetaPartida.causes_cie.Clave = oCausesCie.Clave;
                    }
                    else
                    {
                        oRecetaPartida.Cause = false;
                        oRecetaPartida.idCausesCie = null;
                    }
                    
                    //oRecetaPartida.Cause = rdbCauseSi.Checked;
                    oRecetaPartida.Factura = false;
                    oRecetaPartida.lineas_creditos = new MedDAL.DAL.lineas_creditos();
                    oRecetaPartida.lineas_creditos.Clave = cmbLineasCredito.SelectedItem.ToString();
                    oRecetaPartida.productos = new MedDAL.DAL.productos();
                    oRecetaPartida.productos.Nombre = txbProducto.Text;
                    oRecetaPartida.productos.Clave1 = txbClave.Text;
                    

                    if (lstProductosAlmacen[0].FechaCaducidad.Value <= DateTime.Today)
                    {
                        ScriptManager.RegisterStartupScript(upnForm, upnForm.GetType(), "alertstock", "alertarCaducidad(3);", true);
                    }
                    else if (lstProductosAlmacen[0].FechaCaducidad.Value <= DateTime.Today.AddDays(objConfiguracion.iCaducidad)) {
                        ScriptManager.RegisterStartupScript(upnForm, upnForm.GetType(), "alertstock", "alertarCaducidad(2);", true);
                    }
                    //else if (lstProductosAlmacen[0].FechaCaducidad.Value <= DateTime.Today) 
                    //{
                        
                    //}

                    /*Producto pTemp = new Producto(MedDalProducto, MedDalProducto.Clave1, MedDalProducto.Nombre,
                                              decimal.Parse(txbCantRecetada.Text),
                                              decimal.Parse(txbCantSurtida.Text),
                                              ddlProductoLotes.SelectedValue,
                                              ddlProductoSeries.SelectedValue,
                                              (decimal)0,
                                              //decimal.Parse(ddlPrecios.SelectedItem.Text),
                                              rdbIntencionPrimera.Checked ? 1 : 2,
                                              rdbCauseSi.Checked,
                                              blRecetas.buscarLineaCredito(idAlmacen, MedDalProducto.idProducto, ddlProductoSeries.SelectedValue, ddlProductoLotes.SelectedValue)
                                              );*/
                    if (estadoActual == 2)
                    {
                        ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartidaedicion"]).Add(oRecetaPartida);
                        
                        int iContador = ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"]).Count + 1 - ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartidaedicion"]).Count;
                        int i = 0;

                        foreach (GridViewRow oRow in dgvPartidaDetalle.Rows)
                        {
                            oRow.Cells[9].Controls.Clear();
                            i++;
                            if (i == iContador) break;
                        }                                            
                    }
                    ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"]).Add(oRecetaPartida);
                    dgvPartidaDetalle.DataBind();
                    //GT 14-10-2011 0578 Aqui es el lugar correcto donde se debe de limpiar el txbCantidadSurtida y no en los eventos text_changed de clave o producto por que la función inicializaGuiPartida va a borrar lo que el usuario capturo en el textbox de cantidad surtida, por eso se comenta esa linea en la funcion y se pone aqui
                    inicializaGuiPartida(true, true);
                    txbCantSurtida.Text = "0";
                    txbCantRecetada.Text = "0";
                }
                else if (objConfiguracion.iVentasNegativas == 0 && lstProductosAlmacen[0].Cantidad < dCantidad)
                {
                    ScriptManager.RegisterStartupScript(upnForm, upnForm.GetType(), "alertstock", "alertarStock(2);", true);
                }
                else if (lstProductosAlmacen[0].FechaCaducidad.Value <= DateTime.Today) 
                {
                    ScriptManager.RegisterStartupScript(upnForm, upnForm.GetType(), "alertstock", "alertarCaducidad(3);", true);
                }
            }
        }



        //Codigo Modificado por Renard
        #region gestiona comboboxes

        #region ActualizarInformacionLineasCredito
        private void ActualizarLineasCredito()
        {
            IQueryable<MedDAL.DAL.lineas_creditos> iqrLineasCredito = oblLineasCredito.MostrarListaActivos();
            Session["lstlineascredito"] = iqrLineasCredito;
        }
        #endregion

        #region ActualizarInformacionUbicacionesExpedidos
        /// <summary>
        /// Actualiza la variable de sesion de los comboboxes, la cual es una lista de los estados activos
        /// </summary>
        protected void ActualizarSesionEstadosExpedidos()
        {
            IQueryable<MedDAL.DAL.estados> iqrEstados = oblEstados.BuscarEnum();
            Session["lstestadosexpedidos"] = iqrEstados;
        }

        /// <summary>
        /// Actualiza la variable de sesion de Municipios dependiendo de los Estados existentes.
        /// </summary>        
        protected void ActualizarSesionMunicipiosExpedidos()
        {
            IQueryable<MedDAL.DAL.municipios> iqrMunicipios = oblMunicipios.BuscarEstados(int.Parse(cmbExpedidoEnEstados.SelectedValue));
            Session["lstmunicipiosexpedidos"] = iqrMunicipios;
        }

        /// <summary>
        /// Actualiza la variable de sesión de Poblaciones dependiendo de los Municipios existentes.
        /// </summary>        
        protected void ActualizarSesionPoblacionesExpedidos()
        {
            IQueryable<MedDAL.DAL.poblaciones> iqrPoblaciones = oblPoblaciones.BuscarMunicipios(int.Parse(cmbExpedidoEnMunicipios.SelectedValue));
            Session["lstpoblacionesexpedidos"] = iqrPoblaciones;
        }

        /// <summary>
        /// Actualiza la variable de sesión de Colonias dependiendo de las Poblaciones existentes.
        /// </summary>
        protected void ActualizarSesionColoniasExpedidos()
        {
            IQueryable<MedDAL.DAL.colonias> iqrColonias = oblColonias.BuscarPoblaciones(int.Parse(cmbExpedidoEnPoblaciones.SelectedValue));
            Session["lstcoloniasexpedidos"] = iqrColonias;
        }
        #endregion

        #region ActualizarInformacionUbicacionesSurtidos
        /// <summary>
        /// Actualiza la variable de sesion de los comboboxes, la cual es una lista de los estados activos
        /// </summary>
        protected void ActualizarSesionEstadosSurtidos()
        {
            IQueryable<MedDAL.DAL.estados> iqrEstados = oblEstados.BuscarEnum();
            Session["lstestadossurtidos"] = iqrEstados;
        }

        /// <summary>
        /// Actualiza la variable de sesion de Municipios dependiendo de los Estados existentes.
        /// </summary>        
        protected void ActualizarSesionMunicipiosSurtidos()
        {
            IQueryable<MedDAL.DAL.municipios> iqrMunicipios = oblMunicipios.BuscarEstados(int.Parse(cmbSurtidoEnEstados.SelectedValue));
            Session["lstmunicipiossurtidos"] = iqrMunicipios;
        }

        /// <summary>
        /// Actualiza la variable de sesión de Poblaciones dependiendo de los Municipios existentes.
        /// </summary>        
        protected void ActualizarSesionPoblacionesSurtidos()
        {
            IQueryable<MedDAL.DAL.poblaciones> iqrPoblaciones = oblPoblaciones.BuscarMunicipios(int.Parse(cmbSurtidoEnMunicipios.SelectedValue));
            Session["lstpoblacionessurtidos"] = iqrPoblaciones;
        }

        /// <summary>
        /// Actualiza la variable de sesión de Colonias dependiendo de las Poblaciones existentes.
        /// </summary>
        protected void ActualizarSesionColoniasSurtidos()
        {
            IQueryable<MedDAL.DAL.colonias> iqrColonias = oblColonias.BuscarPoblaciones(int.Parse(cmbSurtidoEnPoblaciones.SelectedValue));
            Session["lstcoloniassurtidos"] = iqrColonias;
        }
        #endregion

        private void CargaDdlSurtidoExpedidoUsuarioAlmacen()
        {
            MedNeg.Usuarios.BlUsuarios oblUsuarios = new MedNeg.Usuarios.BlUsuarios();
            MedNeg.Almacenes.BlAlmacenes oblAlmacenes = new MedNeg.Almacenes.BlAlmacenes();

            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
            oUsuario = (MedDAL.DAL.usuarios)oblUsuarios.Buscar(int.Parse(Session["usuarioid"].ToString()));

            MedDAL.DAL.almacenes oAlmacen = new MedDAL.DAL.almacenes();
            oAlmacen = (MedDAL.DAL.almacenes)oblAlmacenes.Buscar(oUsuario.idAlmacen);

            CargaDdlExpedidoEstados(false);
            cmbExpedidoEnEstados.SelectedValue = oAlmacen.idEstado.ToString();
            CargaDdlExpedidoMunicipios(false);
            cmbExpedidoEnMunicipios.SelectedValue = oAlmacen.idMunicipio.ToString();
            CargaDdlExpedidoPoblaciones(false);
            cmbExpedidoEnPoblaciones.SelectedValue = oAlmacen.idPoblacion.ToString();
            CargaDdlExpedidoColonia(false);
            cmbExpedidoEnColonias.SelectedValue = oAlmacen.idColonia.ToString();

            CargaDdlSurtidoEstados(false);
            cmbSurtidoEnEstados.SelectedValue = oAlmacen.idEstado.ToString();
            CargaDdlSurtidoMunicipios(false);
            cmbSurtidoEnMunicipios.SelectedValue = oAlmacen.idMunicipio.ToString();
            CargaDdlSurtidoPoblaciones(false);
            cmbSurtidoEnPoblaciones.SelectedValue = oAlmacen.idPoblacion.ToString();
            CargaDdlSurtidoColonia(false);
            cmbSurtidoEnColonias.SelectedValue = oAlmacen.idColonia.ToString();
        }

        #region cmb surtido

        private void CargaDdlSurtidoEstados(bool bDatos)
        {
            ActualizarSesionEstadosSurtidos();

            cmbSurtidoEnEstados.Items.Clear();
            cmbSurtidoEnEstados.DataSource = (IQueryable<MedDAL.DAL.estados>)Session["lstestadossurtidos"];
            cmbSurtidoEnEstados.DataBind();

            if (cmbSurtidoEnEstados.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbSurtidoEnEstados.SelectedIndex = 0;
                }
                else
                {
                    cmbSurtidoEnEstados.SelectedValue = dgvListado.SelectedDataKey.Values[5].ToString();
                }
                CargaDdlSurtidoMunicipios(bDatos);
            }
            else
            {
                cmbSurtidoEnEstados.Items.Add(cnsSinEstados);
                cmbSurtidoEnMunicipios.ClearSelection();
                cmbSurtidoEnMunicipios.Items.Clear();
                cmbSurtidoEnMunicipios.Items.Add(cnsSinMunicipios);
                cmbSurtidoEnPoblaciones.ClearSelection();
                cmbSurtidoEnPoblaciones.Items.Clear();
                cmbSurtidoEnPoblaciones.Items.Add(cnsSinPoblaciones);
                cmbSurtidoEnColonias.ClearSelection();
                cmbSurtidoEnColonias.Items.Clear();
                cmbSurtidoEnColonias.Items.Add(cnsSinColonias);
            }
        }

        private void CargaDdlSurtidoMunicipios(bool bDatos)
        {
            ActualizarSesionMunicipiosSurtidos();

            cmbSurtidoEnMunicipios.Items.Clear();
            cmbSurtidoEnMunicipios.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstmunicipiossurtidos"];
            cmbSurtidoEnMunicipios.DataBind();

            if (cmbSurtidoEnMunicipios.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbSurtidoEnMunicipios.SelectedIndex = 0;
                }
                else
                {
                    cmbSurtidoEnMunicipios.SelectedValue = dgvListado.SelectedDataKey.Values[6].ToString();
                }
                CargaDdlSurtidoPoblaciones(bDatos);
            }
            else
            {
                cmbSurtidoEnMunicipios.Items.Add(cnsSinMunicipios);
                cmbSurtidoEnPoblaciones.ClearSelection();
                cmbSurtidoEnPoblaciones.Items.Clear();
                cmbSurtidoEnPoblaciones.Items.Add(cnsSinPoblaciones);
                cmbSurtidoEnColonias.ClearSelection();
                cmbSurtidoEnColonias.Items.Clear();
                cmbSurtidoEnColonias.Items.Add(cnsSinColonias);
            }
        }

        private void CargaDdlSurtidoPoblaciones(bool bDatos)
        {
            ActualizarSesionPoblacionesSurtidos();

            cmbSurtidoEnPoblaciones.Items.Clear();
            cmbSurtidoEnPoblaciones.DataSource = (IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblacionessurtidos"];
            cmbSurtidoEnPoblaciones.DataBind();

            if (cmbSurtidoEnPoblaciones.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbSurtidoEnPoblaciones.SelectedIndex = 0;
                }
                else
                {
                    cmbSurtidoEnPoblaciones.SelectedValue = dgvListado.SelectedDataKey.Values[7].ToString();
                }
                CargaDdlSurtidoColonia(bDatos);
            }
            else
            {
                cmbSurtidoEnPoblaciones.Items.Add(cnsSinPoblaciones);
                cmbSurtidoEnColonias.ClearSelection();
                cmbSurtidoEnColonias.Items.Clear();
                cmbSurtidoEnColonias.Items.Add(cnsSinColonias);
            }
        }

        private void CargaDdlSurtidoColonia(bool bDatos)
        {
            ActualizarSesionColoniasSurtidos();

            cmbSurtidoEnColonias.Items.Clear();
            cmbSurtidoEnColonias.DataSource = (IQueryable<MedDAL.DAL.colonias>)Session["lstcoloniassurtidos"];
            cmbSurtidoEnColonias.DataBind();

            if (cmbSurtidoEnColonias.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbSurtidoEnColonias.SelectedIndex = 0;
                }
                else
                {
                    cmbSurtidoEnColonias.SelectedValue = dgvListado.SelectedDataKey.Values[8].ToString();
                }
            }
            else
            {
                cmbSurtidoEnColonias.Items.Add(cnsSinColonias);
            }
        }

        protected void cmbSurtidoEnEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargaDdlSurtidoMunicipios(false);
        }

        protected void cmbSurtidoEnMunicipios_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargaDdlSurtidoPoblaciones(false);
        }

        protected void cmbSurtidoEnPoblaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargaDdlSurtidoColonia(false);
        }

        #endregion

        #region cmb Expedido

        private void CargaDdlExpedidoEstados(bool bDatos)
        {
            ActualizarSesionEstadosExpedidos();

            cmbExpedidoEnEstados.Items.Clear();
            cmbExpedidoEnEstados.DataSource = (IQueryable<MedDAL.DAL.estados>)Session["lstestadosexpedidos"];
            cmbExpedidoEnEstados.DataBind();

            if (cmbExpedidoEnEstados.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbExpedidoEnEstados.SelectedIndex = 0;
                }
                else
                {
                    cmbExpedidoEnEstados.SelectedValue = dgvListado.SelectedDataKey.Values[1].ToString();
                }
                CargaDdlExpedidoMunicipios(bDatos);
            }
            else
            {
                cmbExpedidoEnEstados.Items.Add(cnsSinEstados);
                cmbExpedidoEnMunicipios.ClearSelection();
                cmbExpedidoEnMunicipios.Items.Clear();
                cmbExpedidoEnMunicipios.Items.Add(cnsSinMunicipios);
                cmbExpedidoEnPoblaciones.ClearSelection();
                cmbExpedidoEnPoblaciones.Items.Clear();
                cmbExpedidoEnPoblaciones.Items.Add(cnsSinPoblaciones);
                cmbExpedidoEnColonias.ClearSelection();
                cmbExpedidoEnColonias.Items.Clear();
                cmbExpedidoEnColonias.Items.Add(cnsSinColonias);
            }
        }

        private void CargaDdlExpedidoMunicipios(bool bDatos)
        {
            ActualizarSesionMunicipiosExpedidos();

            cmbExpedidoEnMunicipios.Items.Clear();
            cmbExpedidoEnMunicipios.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstmunicipiosexpedidos"];
            cmbExpedidoEnMunicipios.DataBind();

            if (cmbExpedidoEnMunicipios.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbExpedidoEnMunicipios.SelectedIndex = 0;
                }
                else
                {
                    cmbExpedidoEnMunicipios.SelectedValue = dgvListado.SelectedDataKey.Values[2].ToString();
                }
                CargaDdlExpedidoPoblaciones(bDatos);
            }
            else
            {
                cmbExpedidoEnMunicipios.Items.Add(cnsSinMunicipios);
                cmbExpedidoEnPoblaciones.ClearSelection();
                cmbExpedidoEnPoblaciones.Items.Clear();
                cmbExpedidoEnPoblaciones.Items.Add(cnsSinPoblaciones);
                cmbExpedidoEnColonias.ClearSelection();
                cmbExpedidoEnColonias.Items.Clear();
                cmbExpedidoEnColonias.Items.Add(cnsSinColonias);
            }
        }

        private void CargaDdlExpedidoPoblaciones(bool bDatos)
        {
            ActualizarSesionPoblacionesExpedidos();

            cmbExpedidoEnPoblaciones.Items.Clear();
            cmbExpedidoEnPoblaciones.DataSource = (IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblacionesexpedidos"];
            cmbExpedidoEnPoblaciones.DataBind();

            if (cmbExpedidoEnPoblaciones.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbExpedidoEnPoblaciones.SelectedIndex = 0;
                }
                else
                {
                    cmbExpedidoEnPoblaciones.SelectedValue = dgvListado.SelectedDataKey.Values[3].ToString();
                }
                CargaDdlExpedidoColonia(bDatos);
            }
            else
            {
                cmbExpedidoEnPoblaciones.Items.Add(cnsSinPoblaciones);
                cmbExpedidoEnColonias.ClearSelection();
                cmbExpedidoEnColonias.Items.Clear();
                cmbExpedidoEnColonias.Items.Add(cnsSinColonias);
            }
        }

        private void CargaDdlExpedidoColonia(bool bDatos)
        {
            ActualizarSesionColoniasExpedidos();

            cmbExpedidoEnColonias.Items.Clear();
            cmbExpedidoEnColonias.DataSource = (IQueryable<MedDAL.DAL.colonias>)Session["lstcoloniasexpedidos"];
            cmbExpedidoEnColonias.DataBind();

            if (cmbExpedidoEnColonias.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbExpedidoEnColonias.SelectedIndex = 0;
                }
                else
                {
                    cmbExpedidoEnColonias.SelectedValue = dgvListado.SelectedDataKey.Values[4].ToString();
                }
            }
            else
            {
                cmbExpedidoEnColonias.Items.Add(cnsSinColonias);
            }
        }

        protected void cmbExpedidoEnEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargaDdlExpedidoMunicipios(false);
        }

        protected void cmbExpedidoEnMunicipios_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargaDdlExpedidoPoblaciones(false);
        }

        protected void cmbExpedidoEnPoblaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargaDdlExpedidoColonia(false);
        }

        #endregion

        #region cmb Lineas Credito

        private void CargarCmbLineasCredito()
        {
            ActualizarLineasCredito();

            cmbLineasCredito.Items.Clear();
            cmbLineasCredito.DataSource = (IQueryable<MedDAL.DAL.lineas_creditos>)Session["lstlineascredito"];
            cmbLineasCredito.DataBind();

            if (cmbLineasCredito.Items.Count != 0)
            {
                cmbLineasCredito.SelectedIndex = 0;
            }
            else
            {
                cmbLineasCredito.Items.Add(cnsSinLineasCredito);
            }
        }

        #endregion

        #endregion

        protected void txbPieCedulaProf_TextChanged(object sender, EventArgs e)
        {
            buscaMedico();
        }

        protected void txbNumeroSeguroSocial_TextChanged(object sender, EventArgs e)
        {
            MedNeg.BlClientes.BlClientes oblClientes = new MedNeg.BlClientes.BlClientes();
            if (txbNumeroSeguroSocial.Text.Length > 0 && txbNumeroSeguroSocial.Text.IndexOf(" ") > -1)
            {
                string sClave = txbNumeroSeguroSocial.Text.Substring(0, txbNumeroSeguroSocial.Text.IndexOf(" "));
                string sNombre = txbNumeroSeguroSocial.Text.Substring(txbNumeroSeguroSocial.Text.IndexOf(" ") + 1, txbNumeroSeguroSocial.Text.IndexOf(",") - 2 - txbNumeroSeguroSocial.Text.IndexOf(" ") + 1);
                string sApellido = txbNumeroSeguroSocial.Text.Substring(txbNumeroSeguroSocial.Text.IndexOf(",") + 1);

                MedDAL.DAL.clientes oCliente = oblClientes.BuscarPorClaveNombreApellido(sClave, sNombre, sApellido);
                if (oCliente != null)
                {
                    txbNumeroSeguroSocial.Text = oCliente.Clave1;
                    Session["recetasIdCliente"] = oCliente.idCliente;
                    txbCliente.Text = oCliente.Nombre + " " + oCliente.Apellidos;
                    txbClienteTelefono.Text = oCliente.Telefono;
                }
            }
        }

        protected void txbCliente_TextChanged(object sender, EventArgs e)
        {
            MedNeg.BlClientes.BlClientes oblClientes = new MedNeg.BlClientes.BlClientes();
            if (txbCliente.Text.Length > 0 && txbCliente.Text.IndexOf(" ") > -1)
            {
                string sClave = txbCliente.Text.Substring(0, txbCliente.Text.IndexOf(" "));
                string sNombre = txbCliente.Text.Substring(txbCliente.Text.IndexOf(" ") + 1, txbCliente.Text.IndexOf(",") - 2 - txbCliente.Text.IndexOf(" ") + 1);
                string sApellido = txbCliente.Text.Substring(txbCliente.Text.IndexOf(",") + 1);

                MedDAL.DAL.clientes oCliente = oblClientes.BuscarPorClaveNombreApellido(sClave, sNombre, sApellido);
                if (oCliente != null)
                {
                    txbCliente.Text = oCliente.Nombre + " " + oCliente.Apellidos;
                    Session["recetasIdCliente"] = oCliente.idCliente;
                    txbNumeroSeguroSocial.Text = oCliente.Clave1;
                    txbClienteTelefono.Text = oCliente.Telefono;
                    
                }
            }
        }

        protected void txbClaveCie_TextChanged(object sender, EventArgs e)
        {
            MedNeg.Causes.BlCauses oblCauses = new MedNeg.Causes.BlCauses();
            if (txbClaveCie.Text.Length > 0 && txbClaveCie.Text.IndexOf(" ") > -1)
            {
                string sClave = txbClaveCie.Text.Substring(0, txbClaveCie.Text.IndexOf(" "));
                MedDAL.DAL.causes_cie oCausesCie = oblCauses.BuscarCie(sClave);

                if (oCausesCie != null)
                { 
                    txbClaveCie.Text = oCausesCie.Clave;
                    Session["recetasIdCausesCie"] = oCausesCie.idCauseCie;
                }
            }
        }
                
        private void buscaMedico()
        {
            lAvisosPie.Text = string.Empty;
            MedDAL.DAL.vendedores_especialidad oVendedoresEspecialidad;
            medico = blRecetas.buscarVendedor(txbPieCedulaProf.Text);
            if (medico != null)
            {
                
                txbPieMedico.Text = medico.Nombre + " " + medico.Apellidos;
                txbPieTituloExp.Enabled = true;
                txbPieRegEspecialidad.Enabled = true;
                txbPieTituloExp.Text = medico.TituloExpedido != null? medico.TituloExpedido : "";
                MedNeg.VendedorEspecialidad.BlVendedorEspecialidad oblVendedorEspecialidad = new MedNeg.VendedorEspecialidad.BlVendedorEspecialidad();
                oVendedoresEspecialidad = ((MedDAL.DAL.vendedores_especialidad)oblVendedorEspecialidad.Buscar(medico.IdEspecialidad));
                txbPieRegEspecialidad.Text = oVendedoresEspecialidad.Especialidad == null? "" : oVendedoresEspecialidad.Especialidad;
                txbPieTituloExp.Enabled = false;
                txbPieRegEspecialidad.Enabled = false;
            }
            else
                lAvisosPie.Text = "Médico no encontrado";
        }

        protected void dgvPartidaDetalle_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"]).RemoveAt(dgvPartidaDetalle.SelectedIndex);
            if (estadoActual == 2 && ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartidaedicion"]).Count != 0 && dgvPartidaDetalle.SelectedIndex >= ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartidaedicion"]).Count)
                ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartidaedicion"]).RemoveAt(dgvPartidaDetalle.SelectedIndex - ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"]).Count);
            //dgvPartidaDetalle.DataSource = lstProductos;
            dgvPartidaDetalle.DataBind();
            dgvPartidaDetalle.SelectedIndex = -1;
        }

        protected void dgvListado_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int iIdTipoReceta = 0;
            if (dgvListado.Rows.Count > 0)
            {
                MedNeg.Tipos.BlTipos oblTipo = new MedNeg.Tipos.BlTipos();

                //EstatusMedico
                if (dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[5].Text == "1")
                    dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[5].Text = "Surtida";
                if (dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[5].Text == "2")
                    dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[5].Text = "Parcial";
                //GT 14-10-11 0578 Estaban al reves los estatus 3=Facturada, 4=Cancelada
                if (dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[5].Text == "3")
                    //dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[5].Text = "Cancelada";
                    dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[5].Text = "Facturada";
                if (dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[5].Text == "4")
                    //dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[5].Text = "Facturada";
                    dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[5].Text = "Cancelada";

                //EstatusFactura
                if (dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[6].Text == "1")
                    dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[6].Text = "Emitido";
                if (dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[6].Text == "2")
                    dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[6].Text = "Facturado";
                if (dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[6].Text == "3")
                    dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[6].Text = "Cancelado";
                //1-Emitido 2-Facturado 3-Cancelado

                //Tipo de receta

                try
                {
                    iIdTipoReceta = Convert.ToInt32(dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[3].Text.ToString());
                    dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[3].Text = oblTipo.Buscar(iIdTipoReceta);
                }
                catch
                {
                    dgvListado.Rows[dgvListado.Rows.Count - 1].Cells[3].Text = "";
                }
            }
        }

        protected void dgvPartidaDetalle_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (dgvPartidaDetalle.Rows.Count > 0)
            {
                dgvPartidaDetalle.Rows[dgvPartidaDetalle.Rows.Count - 1].Cells[0].Text = ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"])[dgvPartidaDetalle.Rows.Count - 1].productos.Clave1; //oProducto.Clave1;
                dgvPartidaDetalle.Rows[dgvPartidaDetalle.Rows.Count - 1].Cells[1].Text = ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"])[dgvPartidaDetalle.Rows.Count - 1].productos.Nombre;
                dgvPartidaDetalle.Rows[dgvPartidaDetalle.Rows.Count - 1].Cells[4].Text = ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"])[dgvPartidaDetalle.Rows.Count - 1].PrimeraIntencion == true ? "Primera" : "Segunda";
                if (((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"])[dgvPartidaDetalle.Rows.Count - 1].causes_cie != null)
                {
                    dgvPartidaDetalle.Rows[dgvPartidaDetalle.Rows.Count - 1].Cells[5].Text = ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"])[dgvPartidaDetalle.Rows.Count - 1].causes_cie.Clave;
                }
                else
                {
                    dgvPartidaDetalle.Rows[dgvPartidaDetalle.Rows.Count - 1].Cells[5].Text = "Sin Diagnóstico";
                }
                dgvPartidaDetalle.Rows[dgvPartidaDetalle.Rows.Count - 1].Cells[8].Text = ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"])[dgvPartidaDetalle.Rows.Count - 1].lineas_creditos.Clave;
                if (((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"])[dgvPartidaDetalle.Rows.Count - 1].idRecetaPartida != null && estadoActual == 2 && !recetaParaEditar.EstatusMedico.Equals("2") && !recetaParaEditar.Estatus.Equals("1"))
                {
                    dgvPartidaDetalle.Rows[dgvPartidaDetalle.Rows.Count - 1].Cells[9].Controls.Clear();
                } 
                if (estadoActual == 2)
                {
                    int iContador = ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartida"]).Count - ((List<MedDAL.DAL.recetas_partida>)Session["lstrecetaspartidaedicion"]).Count;
                    if (dgvPartidaDetalle.Rows.Count - 1 < iContador)
                    {
                        dgvPartidaDetalle.Rows[dgvPartidaDetalle.Rows.Count - 1].Cells[9].Controls.Clear();
                    } 
                }
            }
        }


        #region Catalogo de clientes

        #region Variables
        IQueryable<MedDAL.DAL.tipos> iqrTipos;

        #endregion


        #region Cargar datos por omisión

        /// <summary>
        /// Actualiza la variable de sesion "lstestadosalmacenes", la cual es una lista de los estados activos
        /// </summary>
        protected void ActualizarSesionEstados()
        {
            IQueryable<MedDAL.DAL.estados> iqrEstados = oblEstados.BuscarEnum();
            Session["lstestados"] = iqrEstados;
        }

        /// <summary>
        /// Actualiza la variable de sesion de Municipios dependiendo de los Estados existentes.
        /// </summary>        
        protected void ActualizarSesionMunicipios()
        {
            IQueryable<MedDAL.DAL.municipios> iqrMunicipios = oblMunicipios.BuscarEstados(int.Parse(cmbEstados.SelectedValue));
            Session["lstmunicipios"] = iqrMunicipios;
        }

        /// <summary>
        /// Actualiza la variable de sesión de Poblaciones dependiendo de los Municipios existentes.
        /// </summary>        
        protected void ActualizarSesionPoblaciones()
        {
            IQueryable<MedDAL.DAL.poblaciones> iqrPoblaciones = oblPoblaciones.BuscarMunicipios(int.Parse(cmbMunicipios.SelectedValue));
            Session["lstpoblaciones"] = iqrPoblaciones;
        }

        /// <summary>
        /// Actualiza la variable de sesión de Colonias dependiendo de las Poblaciones existentes.
        /// </summary>
        protected void ActualizarSesionColonias()
        {
            IQueryable<MedDAL.DAL.colonias> iqrColonias = oblColonias.BuscarPoblaciones(int.Parse(cmbPoblaciones.SelectedValue));
            Session["lstcolonias"] = iqrColonias;
        }

        private void CargarEstados(bool bDatos)
        {
            ActualizarSesionEstados();

            cmbEstados.Items.Clear();
            cmbEstados.DataSource = (IQueryable<MedDAL.DAL.estados>)Session["lstestados"];
            cmbEstados.DataBind();

            if (cmbEstados.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbEstados.SelectedIndex = 0;
                }
                else
                {
                    //cmbEstados.SelectedValue = gdvDatos.SelectedDataKey.Values[1].ToString();
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

        private void CargarMunicipios(bool bDatos)
        {
            ActualizarSesionMunicipios();

            cmbMunicipios.Items.Clear();
            cmbMunicipios.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstmunicipios"];
            cmbMunicipios.DataBind();

            if (cmbMunicipios.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbMunicipios.SelectedIndex = 0;
                }
                else
                {
                    //cmbMunicipios.SelectedValue = gdvDatos.SelectedDataKey.Values[2].ToString();
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

        private void CargarPoblaciones(bool bDatos)
        {
            ActualizarSesionPoblaciones();

            cmbPoblaciones.Items.Clear();
            cmbPoblaciones.DataSource = (IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblaciones"];
            cmbPoblaciones.DataBind();

            if (cmbPoblaciones.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbPoblaciones.SelectedIndex = 0;
                }
                else
                {
                    //cmbPoblaciones.SelectedValue = gdvDatos.SelectedDataKey.Values[3].ToString();
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

        private void CargarColonias(bool bDatos)
        {
            ActualizarSesionColonias();

            cmbColonias.Items.Clear();
            cmbColonias.DataSource = (IQueryable<MedDAL.DAL.colonias>)Session["lstcolonias"];
            cmbColonias.DataBind();

            if (cmbColonias.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbColonias.SelectedIndex = 0;
                }
                else
                {
                    //cmbColonias.SelectedValue = gdvDatos.SelectedDataKey.Values[4].ToString();
                }
            }
            else
            {
                cmbColonias.Items.Add(cnsSinColonias);
            }
        }

        private void CargarCmbTipos()
        {
            MedNeg.Tipos.BlTipos oblTipos = new MedNeg.Tipos.BlTipos();
            iqrTipos = (IQueryable<MedDAL.DAL.tipos>)oblTipos.Buscar("Clientes", 2);
            ddlTipo.Items.Clear();
            ddlTipo.DataSource = iqrTipos;
            ddlTipo.DataBind();
            foreach (ListItem item in ddlTipo.Items)
            {
                if (item.Text == "Paciente")
                {
                    ddlTipo.SelectedValue = item.Value;
                    break;
                }
            }
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

        protected void NotificarAccion(bool status, string resultadoAccion)
        {
            if (status)
                lblResults.ForeColor = Color.Green;
            else
                lblResults.ForeColor = Color.Red;
            lblResults.Text = resultadoAccion;
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

        #region EventHandlers

        protected void imbAgregarCliente_Click(object sender, ImageClickEventArgs e)
        {
            pnlClientes.Visible = true;
            ModificarControl(this.tbcClientesNuevos, true, true);
            lblResults.Text = "";
            Session["modoGuardarClientes"] = true;
            //MostrarAreaTrabajo(false, true);
            //ModificarControl(this.tabContainer, true, true);
            txbPais.Text = "México"; txbPais.Enabled = false;
            txbFechaAlta.Text = DateTime.Now.ToShortDateString();
            chkActivo.Checked = true;
            CargarEstados(false);
            CargarCmbTipos();
            Session["lstContactosDB"] = new List<MedDAL.DAL.clientes_contacto>();
            //gdvContactosCliente.Visible = true;
            //gdvContactosCliente.ShowHeader = true;
            //gdvContactosCliente.DataSource = ((List<MedDAL.DAL.clientes_contacto>)Session["lstContactosDB"]);
            //gdvContactosCliente.DataBind();
            //gdvContactosCliente.DataKeyNames = new String[] { "idContacto" };


        }

        protected void btnCancelarCliente_Click(object sender, EventArgs e)
        {
            ModificarControl(this.tbcClientesNuevos, true, true);
            pnlClientes.Visible = false;
        }

        protected void cmbPoblacionFormulario_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarColonias(false);
        }

        protected void cmbMunicipioFormulario_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPoblaciones(false);
        }

        protected void cmbEstadoFormulario_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarMunicipios(false);
        }

        protected void imbAgregarContacto_Click(object sender, ImageClickEventArgs e)
        {
            MedDAL.DAL.clientes_contacto contacto = new MedDAL.DAL.clientes_contacto();
            contacto.Activo = true;
            contacto.Nombre = txbNombreContacto.Text;
            contacto.Apellidos = txbApellidosContacto.Text;
            contacto.Telefono = txbTelefonoContacto.Text;
            contacto.Celular = txbCelularConatcto.Text;
            contacto.CorreoElectronico = txbCorreoEContacto.Text;
            LLenarListas(contacto);
            gdvContactosCliente.DataBind();
            ModificarControl(this.tabContactosPersonales, true, true);
        }

        protected void grvContactos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((List<MedDAL.DAL.clientes_contacto>)Session["lstContactosDB"]).RemoveAt(gdvContactosCliente.SelectedIndex);
            gdvContactosCliente.DataBind();
        }

        protected void cmbTipoPersona_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTipoPersona.SelectedValue == "FISICA")
            {
                lblNombres.Text = "Nombre(s):";
                txbApellidos.Enabled = true;
                rfvApellidos.Enabled = true;
            }
            else
            {
                lblNombres.Text = "Razón Social:";
                txbApellidos.Enabled = false;
                rfvApellidos.Enabled = false;
            }
        }

        protected void PoblarDatosGenerales()
        {
            /*Datos generales*/
            oClienteNuevo.Clave1 = txbClave1.Text;
            oClienteNuevo.Clave2 = txbClave2.Text;
            oClienteNuevo.Clave3 = txbClave3.Text;
            oClienteNuevo.Nombre = txbNombres.Text;
            oClienteNuevo.Apellidos = cmbTipoPersona.SelectedValue == "FISICA" ? txbApellidos.Text : "";
            oClienteNuevo.idTipoCliente = int.Parse(ddlTipo.SelectedValue);
            oClienteNuevo.FechaAlta = Convert.ToDateTime(txbFechaAlta.Text);
            oClienteNuevo.Activo = chkActivo.Checked;
            oClienteNuevo.Sexo = cmbSexo.SelectedValue;
            oClienteNuevo.TipoPersona = cmbTipoPersona.SelectedValue;
            if (txbEdad.Text != string.Empty)
                oClienteNuevo.Edad = int.Parse(txbEdad.Text);
            else
                oClienteNuevo.Edad = 0;
        }

        protected void PoblarDatosContacto()
        {
            /*Datos de contacto*/
            //Direccion
            oClienteNuevo.Calle = txbCalle.Text;
            oClienteNuevo.NumeroExt = txbNumeroExterior.Text;
            oClienteNuevo.NumeroInt = txbNumeroInterior.Text;
            oClienteNuevo.idEstado = int.Parse(cmbEstados.SelectedValue);
            oClienteNuevo.idMunicipio = int.Parse(cmbMunicipios.SelectedValue);
            oClienteNuevo.idPoblacion = int.Parse(cmbPoblaciones.SelectedValue);
            oClienteNuevo.idColonia = int.Parse(cmbColonias.SelectedValue);
            oClienteNuevo.CodigoPostal = txbCP.Text;

            //Contacto
            oClienteNuevo.Telefono = txbTelefono.Text;
            oClienteNuevo.Celular = txbCelular.Text;
            //oCliente.Fax = txbFax.Text;
            oClienteNuevo.CorreoElectronico = txbCorreoE.Text;
        }

        protected void PoblarDatosProfesionales()
        {
            /*Datos profesionales*/
            oClienteNuevo.Rfc = txbRFC.Text;
            oClienteNuevo.Curp = txbCurp.Text;
        }

        protected void PoblarDatosOpcionales()
        {
            /*Datos opcionales*/
            oClienteNuevo.Campo1 = txbAlfanumerico1.Text;
            oClienteNuevo.Campo2 = txbAlfanumerico2.Text;
            oClienteNuevo.Campo3 = txbAlfanumerico3.Text;
            oClienteNuevo.Campo4 = txbAlfanumerico4.Text;
            oClienteNuevo.Campo5 = txbAlfanumerico5.Text;

            if (txbEntero1.Text.Equals(""))
                oClienteNuevo.Campo6 = 0;
            else
                oClienteNuevo.Campo6 = Convert.ToInt32(txbEntero1.Text);

            if (txbEntero2.Text.Equals(""))
                oClienteNuevo.Campo7 = 0;
            else
                oClienteNuevo.Campo7 = Convert.ToInt32(txbEntero2.Text);

            if (txbEntero3.Text.Equals(""))
                oClienteNuevo.Campo8 = 0;
            else
                oClienteNuevo.Campo8 = Convert.ToInt32(txbEntero3.Text);

            if (txbDecimal1.Text.Equals(""))
                oClienteNuevo.Campo9 = 0;
            else
                oClienteNuevo.Campo9 = Convert.ToDecimal(txbDecimal1.Text);


            if (txbDecimal2.Text.Equals(""))
                oClienteNuevo.Campo10 = 0;
            else
                oClienteNuevo.Campo10 = Convert.ToDecimal(txbDecimal2.Text);
        }
        
        protected bool ValidarCliente()
        {
            //JID 28/11/2011 Se quitó la validación que indica si los clientes tienen clave repetida
            //MedNeg.BlClientes.BlClientes oblCliente = new MedNeg.BlClientes.BlClientes();
            //if (oblCliente.ValidarClienteRepetido(txbClave1.Text) >= 1)
            //    return false;
            //else
                return true;
        }

        protected void RegistrarEvento(string sModulo, string sAccion, string sDescripcion)
        {
            oBitacora = new MedDAL.DAL.bitacora();
            MedNeg.Bitacora.BlBitacora oblBitacora = new MedNeg.Bitacora.BlBitacora();
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

        protected void btnGuardarCliente_Click(object sender, EventArgs e)
        {

            MedNeg.BlClientes.BlClientes oblCliente = new MedNeg.BlClientes.BlClientes();
            PoblarDatosGenerales();
            PoblarDatosContacto();
            PoblarDatosProfesionales();
            PoblarDatosOpcionales();
            if (ValidarCliente())
            {
                if (oblCliente.NuevoRegistro(oClienteNuevo))
                {
                    NotificarAccion(true, "Se ha agregado correctamente el cliente");
                    //ModificarControl(this.tbcClientesNuevos, true, true);
                    CargarEstados(false);
                    CargarCmbTipos();
                    //gdvDatos.SelectedIndex = -1;
                    RegistrarEvento("Cliente", "Agregar cliente", "Se ha agregado el Cliente " + oClienteNuevo.idCliente + ": " + oClienteNuevo.Nombre + " " + oClienteNuevo.Apellidos +
                        ", Clave: " + oClienteNuevo.Clave1 + ", Correo electronico:" + oClienteNuevo.CorreoElectronico + ", RFC: " + oClienteNuevo.Rfc + "");

                    MedNeg.ClientesContactos.BlClientesContactos oblClientesContactos = new MedNeg.ClientesContactos.BlClientesContactos();
                    if (!oblClientesContactos.NuevoRegistro((List<MedDAL.DAL.clientes_contacto>)Session["lstContactosDB"], oClienteNuevo.idCliente))
                        NotificarAccion(false, "Se ha agregado correctamente el cliente, pero no se pudieron agregar 1 o mas contactos");
                    pnlClientes.Visible = false;
                }
                else
                    NotificarAccion(false, "No se ha podido agregar el cliente");
            }
            else
                NotificarAccion(false, "Ya existe un cliente con esa clave");
        }
        #endregion

        #endregion







        #region Catalogo de Vendedores (Medicos)

            #region Variables

        MedDAL.DAL.vendedores oVendedor;
        MedDAL.DAL.vendedores_especialidad oEspecialidad;
        MedNeg.VendedorEspecialidad.BlVendedorEspecialidad oblEspecialidad = new MedNeg.VendedorEspecialidad.BlVendedorEspecialidad();
        MedNeg.VendedorVinculacion.BlVendedorVinculacion oblVinculacion = new MedNeg.VendedorVinculacion.BlVendedorVinculacion();
        MedDAL.DAL.vendedores_vinculacion oVinculacion;
            #endregion

            #region Cargar Datos Por Omisión
        /// <summary>
        /// Actualiza la variable de sesion "lstestadosalmacenes", la cual es una lista de los estados activos
        /// </summary>
        protected void ActualizarSesionEstadosVendedor()
        {
            IQueryable<MedDAL.DAL.estados> iqrEstadosVendedor = oblEstados.BuscarEnum();
            Session["lstestadosvendedor"] = iqrEstadosVendedor;
        }

        /// <summary>
        /// Actualiza la variable de sesion de Municipios dependiendo de los Estados existentes.
        /// </summary>        
        protected void ActualizarSesionMunicipiosVendedor()
        {
            IQueryable<MedDAL.DAL.municipios> iqrMunicipiosVendedor = oblMunicipios.BuscarEstados(int.Parse(cmbEstadoVendedor.SelectedValue));
            Session["lstmunicipiosvendedor"] = iqrMunicipiosVendedor;
        }

        /// <summary>
        /// Actualiza la variable de sesión de Poblaciones dependiendo de los Municipios existentes.
        /// </summary>        
        protected void ActualizarSesionPoblacionesVendedor()
        {
            IQueryable<MedDAL.DAL.poblaciones> iqrPoblacionesVendedor = oblPoblaciones.BuscarMunicipios(int.Parse(cmbMunicipioVendedores.SelectedValue));
            Session["lstpoblacionesvendedor"] = iqrPoblacionesVendedor;
        }

        /// <summary>
        /// Actualiza la variable de sesión de Colonias dependiendo de las Poblaciones existentes.
        /// </summary>
        protected void ActualizarSesionColoniasVendedor()
        {
            IQueryable<MedDAL.DAL.colonias> iqrColoniasVendedor = oblColonias.BuscarPoblaciones(int.Parse(cmbPoblacionVendedores.SelectedValue));
            Session["lstcoloniasvendedor"] = iqrColoniasVendedor;
        }


        private void CargarEstadosVendedores(bool bDatos)
        {
            ActualizarSesionEstadosVendedor();

            cmbEstadoVendedor.Items.Clear();
            cmbEstadoVendedor.DataSource = (IQueryable<MedDAL.DAL.estados>)Session["lstestadosvendedor"];
            cmbEstadoVendedor.DataBind();

            if (cmbEstadoVendedor.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbEstadoVendedor.SelectedIndex = 0;
                }
                else
                {
                    //cmbEstadoVendedor.SelectedValue = gdvDatos.SelectedDataKey.Values[1].ToString();
                }
                CargarMunicipiosVendedor(bDatos);
                
            }
            else
            {
                cmbEstadoVendedor.Items.Add(cnsSinEstados);
                cmbMunicipioVendedores.ClearSelection();
                cmbMunicipioVendedores.Items.Clear();
                cmbMunicipioVendedores.Items.Add(cnsSinMunicipios);

                cmbPoblacionVendedores.ClearSelection();
                cmbPoblacionVendedores.Items.Clear();
                cmbPoblacionVendedores.Items.Add(cnsSinPoblaciones);

                cmbColoniaVendedores.ClearSelection();
                cmbColoniaVendedores.Items.Clear();
                cmbColoniaVendedores.Items.Add(cnsSinColonias);
            }
        }

        private void CargarMunicipiosVendedor(bool bDatos)
        {
            ActualizarSesionMunicipiosVendedor();

            cmbMunicipioVendedores.Items.Clear();
            cmbMunicipioVendedores.DataSource = (IQueryable<MedDAL.DAL.municipios>)Session["lstmunicipiosvendedor"];
            cmbMunicipioVendedores.DataBind();

            if (cmbMunicipioVendedores.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbMunicipioVendedores.SelectedIndex = 0;
                }
                else
                {
                    //cmbMunicipios.SelectedValue = gdvDatos.SelectedDataKey.Values[2].ToString();
                }
                CargarPoblacionesVendedor(bDatos);
                
            }
            else
            {
                cmbMunicipioVendedores.Items.Add(cnsSinMunicipios);

                cmbPoblacionVendedores.ClearSelection();
                cmbPoblacionVendedores.Items.Clear();
                cmbPoblacionVendedores.Items.Add(cnsSinPoblaciones);

                cmbColoniaVendedores.ClearSelection();
                cmbColoniaVendedores.Items.Clear();
                cmbColoniaVendedores.Items.Add(cnsSinColonias);
            }

        }

        private void CargarPoblacionesVendedor(bool bDatos)
        {
            ActualizarSesionPoblacionesVendedor();

            cmbPoblacionVendedores.Items.Clear();
            cmbPoblacionVendedores.DataSource = (IQueryable<MedDAL.DAL.poblaciones>)Session["lstpoblacionesvendedor"];
            cmbPoblacionVendedores.DataBind();

            if (cmbPoblacionVendedores.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbPoblacionVendedores.SelectedIndex = 0;
                }
                else
                {
                    //cmbPoblaciones.SelectedValue = gdvDatos.SelectedDataKey.Values[3].ToString();
                }
                CargarColoniasVendedor(bDatos);
            }
            else
            {
                cmbPoblacionVendedores.Items.Add(cnsSinPoblaciones);

                cmbColoniaVendedores.ClearSelection();
                cmbColoniaVendedores.Items.Clear();
                cmbColoniaVendedores.Items.Add(cnsSinColonias);

            }
        }

        private void CargarColoniasVendedor(bool bDatos)
        {
            ActualizarSesionColoniasVendedor();

            
            cmbColoniaVendedores.Items.Clear();
            cmbColoniaVendedores.DataSource = (IQueryable<MedDAL.DAL.colonias>)Session["lstcoloniasvendedor"];
            cmbColoniaVendedores.DataBind();

            if (cmbColoniaVendedores.Items.Count != 0)
            {
                if (!bDatos)
                {
                    cmbColoniaVendedores.SelectedIndex = 0;
                }
                else
                {
                    //cmbColonias.SelectedValue = gdvDatos.SelectedDataKey.Values[4].ToString();
                }
            }
            else
            {
                cmbColoniaVendedores.Items.Add(cnsSinColonias);
            }
        }

        private void CargarCmbTiposVendedor()
        {
            MedNeg.Tipos.BlTipos oblTipos = new MedNeg.Tipos.BlTipos();
            iqrTiposVendedor = (IQueryable<MedDAL.DAL.tipos>)oblTipos.Buscar("Vendedores", 2);
            cmbTipoVendedor.Items.Clear();
            cmbTipoVendedor.DataSource = iqrTiposVendedor;
            cmbTipoVendedor.DataBind();
            
        }

            #endregion

            #region EventHandlers
        
        protected void imbNuevoMedico_Click(object sender, ImageClickEventArgs e)
        {
            pnlDetalle.Visible = false;
            pnlVendedoresNuevos.Visible = true;
            //lblResults.Text = "";
            //Session["modoGuardar"] = true;
            //MostrarAreaTrabajo(false, true);
            ModificarControl(this.tbcVendedores, true, true);
            txbPaisVendedor.Text = "México";
            //chkActivo.Checked = true;
            //txbFechaAlta.Text = DateTime.Now.ToShortDateString();
            txbFechaVendedor.Text = DateTime.Now.ToShortDateString();
            //chkActivo.Checked = true;
            chkActivoVendedor.Checked = true;
            CargarEstadosVendedores(false);
            CargarCmbTiposVendedor();
            //gdvDatos.SelectedIndex = -1;
        }

        protected void cmbEstadoVendedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarMunicipiosVendedor(false);
        }

        protected void cmbMunicipioVendedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarPoblacionesVendedor(false);
        }

        protected void cmbPoblacionVendedores_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarColoniasVendedor(false);
        }


        protected bool ValidarVendedor()
        {
            MedNeg.Vendedores.BlVendedores oblVendedores = new MedNeg.Vendedores.BlVendedores();
            if (oblVendedores.ValidarVendedorRepetido(txbClaveVendedor.Text) >= 1 || txbClaveVendedor.Text == "")
                return false;
            else
                return true;
        }

       

        protected void btnGuardarMedico_Click(object sender, EventArgs e)
        {

            oVendedor = new MedDAL.DAL.vendedores();
            MedNeg.Vendedores.BlVendedores oblVendedores = new MedNeg.Vendedores.BlVendedores();
            PoblarDatosGeneralesVendedor();
            PoblarDatosContactoVendedor();
            PoblarDatosProfesionalesVendedor();
            PoblarDatosOpcionalesVendedor();
            if (ValidarVendedor())
            {
                if (oblVendedores.NuevoRegistro(oVendedor))
                {
                    //NotificarAccion(true, "Se ha agregado correctamente el vendedor");
                    lblAvisosVendedores.Text = "Se ha agregado correctamente el vendedor";
                    ModificarControl(this.tbcVendedores, true, true);
                    CargarEstados(false);
                    CargarCmbTiposVendedor();
                    //gdvDatos.SelectedIndex = -1;
                    RegistrarEvento("Vendedores", "Agregar vendedor", "Se ha agregado al Vendedor " + oVendedor.idVendedor + ": " + oVendedor.Nombre + " " + oVendedor.Apellidos +
                        ", Clave: " + oVendedor.Clave + ", Correo electronico:" + oVendedor.CorreoElectronico + ", RFC: " + oVendedor.Rfc + ", Cedula profesional: " + oVendedor.CedulaProfesional);

                    pnlVendedoresNuevos.Visible = false;
                    pnlDetalle.Visible = true;
                }
                else
                    //NotificarAccion(false, "No se ha podido agregar el vendedor");
                    lblAvisosVendedores.Text = "No se ha podido agregar el vendedor";
            }
            else
                //NotificarAccion(false, "Ya existe un vendedor con esa clave");
                lblAvisosVendedores.Text = "Ya existe un vendedor con esa clave";
        }

        private void PoblarDatosOpcionalesVendedor()
        {
            /*Datos opcionales*/
            oVendedor.Campo1 = txbAlfanumerico1V.Text;
            oVendedor.Campo2 = txbAlfanumerico2V.Text;
            oVendedor.Campo3 = txbAlfanumerico3V.Text;
            oVendedor.Campo4 = txbAlfanumerico4V.Text;
            oVendedor.Campo5 = txbAlfanumerico5V.Text;

            if (txbEntero1V.Text.Equals(""))
                oVendedor.Campo6 = 0;
            else
                oVendedor.Campo6 = Convert.ToInt32(txbEntero1V.Text);

            if (txbEntero2V.Text.Equals(""))
                oVendedor.Campo7 = 0;
            else
                oVendedor.Campo7 = Convert.ToInt32(txbEntero2V.Text);

            if (txbEntero3V.Text.Equals(""))
                oVendedor.Campo8 = 0;
            else
                oVendedor.Campo8 = Convert.ToInt32(txbEntero3V.Text);

            if (txbDecimal1V.Text.Equals(""))
                oVendedor.Campo9 = 0;
            else
                oVendedor.Campo9 = Convert.ToDecimal(txbDecimal1V.Text);


            if (txbDecimal2V.Text.Equals(""))
                oVendedor.Campo10 = 0;
            else
                oVendedor.Campo10 = Convert.ToDecimal(txbDecimal2V.Text);
        }

        private void PoblarDatosProfesionalesVendedor()
        {

            oVendedor.Rfc = txbRfcVendedores.Text;
            oVendedor.Curp = txbCurpVendedores.Text;
            oVendedor.TituloExpedido = txbTituloVendedores.Text;
            oVendedor.CedulaProfesional = txbCedulaProfesionalVendedores.Text;
            oEspecialidad = (MedDAL.DAL.vendedores_especialidad)oblEspecialidad.Buscar(txbEspecialidadVendedores.Text);
            if (oEspecialidad != null)
                oVendedor.IdEspecialidad = oEspecialidad.idEspecialidad;
            else
            {
                oEspecialidad = new MedDAL.DAL.vendedores_especialidad();
                oEspecialidad.Especialidad = txbEspecialidadVendedores.Text;
                oblEspecialidad.NuevoRegistro(oEspecialidad);
                oVendedor.IdEspecialidad = oEspecialidad.idEspecialidad;
            }
            oVinculacion = (MedDAL.DAL.vendedores_vinculacion)oblVinculacion.Buscar(txbVinculacionVendedores.Text);
            if (oVinculacion != null)
                oVendedor.IdVinculacion = oVinculacion.idVinculacion;
            else
            {
                oVinculacion = new MedDAL.DAL.vendedores_vinculacion();
                oVinculacion.Vinculacion = txbVinculacionVendedores.Text;
                oblVinculacion.NuevoRegistro(oVinculacion);
                oVendedor.IdVinculacion = oVinculacion.idVinculacion;
            }
        }

        private void PoblarDatosContactoVendedor()
        {
            //Direccion
            oVendedor.Calle = txbCalleVendedor.Text;
            oVendedor.NumeroExt = txbNoExtVendedor.Text;
            oVendedor.NumeroInt = txbNoIntVendedor.Text;
            oVendedor.IdEstado = int.Parse(cmbEstadoVendedor.SelectedValue);
            oVendedor.IdMunicipio = int.Parse(cmbMunicipioVendedores.SelectedValue);
            oVendedor.IdPoblacion = int.Parse(cmbPoblacionVendedores.SelectedValue);
            oVendedor.IdColonia = int.Parse(cmbColoniaVendedores.SelectedValue);
            oVendedor.CodigoPostal = txbCodigoPostalVendedores.Text;

            //Contacto
            oVendedor.Telefono = txbTelefonoVendedores.Text;
            oVendedor.Celular = txbCelularVendedores.Text;
            oVendedor.Fax = txbFaxVendedores.Text;
            oVendedor.CorreoElectronico = txbEmailVendedores.Text;
        }

        private void PoblarDatosGeneralesVendedor()
        {
            /*Datos generales*/
            oVendedor.Clave = txbClaveVendedor.Text;
            oVendedor.Nombre = txbNombreVendedor.Text;
            oVendedor.Apellidos = cmbTipoPersonaVendedor.SelectedValue == "FISICA" ? txbApellidoVendedor.Text : "";
            oVendedor.IdTipoVendedor = int.Parse(cmbTipoVendedor.SelectedValue);
            oVendedor.FechaAlta = Convert.ToDateTime(txbFechaVendedor.Text);
            oVendedor.TipoPersona = cmbTipoPersonaVendedor.SelectedValue;
            oVendedor.Activo = chkActivoVendedor.Checked;
        }

        protected void btnCancelarMedico_Click(object sender, EventArgs e)
        {

            ModificarControl(this.tbcVendedores, true, true);
            pnlVendedoresNuevos.Visible = false;
            pnlDetalle.Visible = true;
        }

            #endregion //De evet handlers de vendedores

        

      

       

        
       

        #endregion //De Vendedores

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
            divFormulario.Visible = false;
            divListado.Visible = false;            

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
            oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());

            odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes") : (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
            odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4 and idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "recetas") 
                : 
                (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas where EstatusMedico != 4", "medicuriConnectionString", odsDataSet, "recetas");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from recetas_partida", "medicuriConnectionString", odsDataSet, "recetas_partida");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from clientes", "medicuriConnectionString", odsDataSet, "clientes");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from vendedores", "medicuriConnectionString", odsDataSet, "vendedores");

            Session["campoaordenar"] = "";
            Session["dataset"] = odsDataSet;
            Session["titulo"] = "Impresión de Recetas";
            //Session["configuracionsistema"] = objConfiguracion;
            Session["sortfield"] = 0;
            Session["reportdocument"] = "~\\rptReportes\\rptRecetas.rpt";

            if (dgvListado.SelectedIndex != -1)
            {
                Session["recordselection"] = "{recetas.idReceta}=" + dgvListado.SelectedDataKey.Values[0].ToString();
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

        protected void dgvListado_Sorting(object sender, GridViewSortEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Recetas.RecetasView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvListado.DataSource = oMaster.Sorting(e, ref oDireccion, dv);
            ViewState["direccionsorting"] = oDireccion;
            ViewState["sortexpression"] = e.SortExpression;
            dgvListado.DataBind();
        }

        protected void dgvListado_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Recetas.RecetasView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvListado.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Folio" : ViewState["sortexpression"].ToString(), dv, ref dgvListado, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            dgvListado.DataBind();
        }

        #endregion

        /// <summary>
        /// 0415 GT 15 de Agosto de 2011
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txbPieMedico_TextChanged(object sender, EventArgs e)
        {            
            lAvisosPie.Text = string.Empty;
            if (txbPieMedico.Text.Length > 0 && txbPieMedico.Text.IndexOf(" ") > -1)
            {
                string sClave = txbPieMedico.Text.Substring(0, txbPieMedico.Text.IndexOf(" "));
                medico = blRecetas.buscarVendedorClave(sClave);
                if (medico != null)
                {
                    txbPieCedulaProf.Text = medico.CedulaProfesional;
                    txbPieMedico.Text = medico.Nombre + " " + medico.Apellidos;
                    txbPieTituloExp.Text = medico.TituloExpedido;
                    MedNeg.VendedorEspecialidad.BlVendedorEspecialidad oblVendedorEspecialidad = new MedNeg.VendedorEspecialidad.BlVendedorEspecialidad();
                    txbPieRegEspecialidad.Text = ((MedDAL.DAL.vendedores_especialidad)oblVendedorEspecialidad.Buscar(medico.IdEspecialidad)).Especialidad;
                }
                else
                    lAvisosPie.Text = "Médico no encontrado";
            }
            else
            { 
                txbPieMedico.Text = "";
                txbPieTituloExp.Text = "";
                txbPieRegEspecialidad.Text = "";
            }            
        }

        


    }//clase

}//name space