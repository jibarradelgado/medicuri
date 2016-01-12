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
    public partial class Inventarios : System.Web.UI.Page
    {
        ImageButton imbFisico, imbEditar, imbReportes, imbEliminar, imbMostrar, imbImprimir, imbAceptar, imbCancelar, imbAlertas;
        RadioButton rdbNombre, rdbClave, rdbTodos;
        Button btnBuscar;
        TextBox txbBuscar;
        Label lblNombreModulo;
        MedNeg.Inventarios.BlInventarios oblInventarios;
        MedNeg.Bitacora.BlBitacora oblBitacora;
        MedDAL.DAL.productos_almacen_stocks oProductosAlmacenStocks;
        MedDAL.DAL.bitacora oBitacora;
        MedNeg.Usuarios.BlUsuarios oblUsuario;

        protected void CargarFormulario(bool bDatos)
        {
            pnlFormulario.Visible = true;
            pnlCatalogo.Visible = false;
            divInventarioFísico.Visible = false;
            pnlFiltroReportes.Visible = false;
            //pnlReportes.Visible = false;
            txbClave.Text = gdvDatos.SelectedRow.Cells[1].Text;
            //Page.Server.HtmlDecode(dg.Rows(dg.SelectedIndex).C ells(2).Text)
            txbProducto.Text = Page.Server.HtmlDecode(gdvDatos.SelectedRow.Cells[2].Text);
            txbAlmacen.Text = Page.Server.HtmlDecode(gdvDatos.SelectedRow.Cells[5].Text);
            txbMaximo.Text = gdvDatos.SelectedRow.Cells[4].Text;
            txbMinimo.Text = gdvDatos.SelectedRow.Cells[3].Text;            
        }

        protected void CargarCatalogo()
        {
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = true;
            divInventarioFísico.Visible = false;
            pnlFiltroReportes.Visible = false;
            //pnlReportes.Visible = false;
        }

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

        protected void limpiaTextBox(Control c)
        {
            if (c is TextBox)
                ((TextBox)c).Text = string.Empty;

            foreach (Control ctrl in c.Controls)
            {
                limpiaTextBox(ctrl);
            }
        }

        protected void cargaDdlInvFisico()
        {
            ddlInvFsClave1Desde.DataSource = ddlInvFsClave1Hasta.DataSource = oblInventarios.buscarTodosProductos();
            ddlInvFsClave1Desde.DataBind();
            ddlInvFsClave1Hasta.DataBind();
            MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
            oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
            if (oUsuario.FiltradoActivado == true)
            {
                ddlInvFsAlmacen.DataSource = oblInventarios.BuscarAlmacenesFiltrado(oUsuario.idAlmacen);
            }
            else
            {
                ddlInvFsAlmacen.DataSource = oblInventarios.ObtenerAlmacenes();
            }                    
            ddlInvFsAlmacen.DataBind();
        }

        protected void CargaGdvInvFisico()
        {
            List<MedNeg.Inventarios.Producto> lstProductos;
            if (ckbInvFsTodos.Checked)
                lstProductos = oblInventarios.ObtenerExistenciaTeorica(int.Parse(ddlInvFsAlmacen.SelectedValue));
            else
                lstProductos = oblInventarios.ObtenerExistenciaTeorica(int.Parse(ddlInvFsAlmacen.SelectedValue), ddlInvFsClave1Desde.SelectedItem.Text, ddlInvFsClave1Hasta.SelectedItem.Text);

            var ProductosSort = from q in lstProductos orderby q.codigo ascending select q;
            lstProductos = ProductosSort.ToList<MedNeg.Inventarios.Producto>();

            for (int i = 0; i < lstProductos.Count; i++)
            {
                lstProductos[i].existenciaReal = lstProductos[i].existenciaTeorica;
                lstProductos[i].strExistenciaReal = lstProductos[i].existenciaTeorica.ToString();
            }

            Session["inventariofisicoproductos"] = lstProductos;
            Session["resultadoquery2"] = lstProductos.AsQueryable<MedNeg.Inventarios.Producto>();

            grvInvFsArticulos.DataSource = lstProductos;

            var result = (IQueryable<MedNeg.Inventarios.Producto>)Session["resultadoquery2"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "codigo ASC";
            
            grvInvFsArticulos.DataSource = dv;
            grvInvFsArticulos.DataBind();
            grvInvFsArticulos.SelectedIndex = -1;

            //foreach (GridViewRow Row in grvInvFsArticulos.Rows)
            //{
            //    Row.Cells[6].Text = Row.Cells[5].Text;
            //    ((List<MedNeg.Inventarios.Producto>)Session["inventariofisicoproductos"])[Row.RowIndex].existenciaReal = decimal.Parse(Row.Cells[5].Text);
            //    ((List<MedNeg.Inventarios.Producto>)Session["inventariofisicoproductos"])[Row.RowIndex].strExistenciaReal = Row.Cells[5].Text;
            //}

            //JID 03/05/2012 Fix para que la lista de productos y el gridview se encuentren ordenados
            ((List<MedNeg.Inventarios.Producto>)Session["inventariofisicoproductos"]).Clear();
            ((List<MedNeg.Inventarios.Producto>)Session["inventariofisicoproductos"]).AddRange(ProductosSort);

            foreach (MedNeg.Inventarios.Producto p in lstProductos)
            {
                oblInventarios.BloquearProducto(int.Parse(ddlInvFsAlmacen.SelectedValue), p.dbProducto.idProducto, true);
            }
            limpiaTextBox(divInventarioFísico);
        }
        protected void InvFscargaProductoEnTxtBxs(MedNeg.Inventarios.Producto oProducto)
        {
            txtInvFsClave.Text = oProducto.codigo;
            txtInvFsDescripcionArticulo.Text = oProducto.descripcion;
            txtInvFsLoteArticulo.Text = oProducto.lote;
            txtInvFsSerieArticulo.Text = oProducto.serie;
            txtInvFsCantidadTeoricaArticulo.Text = oProducto.existenciaTeorica.ToString();
        }
        protected void FinalizaInventarioFisico()
        {
            try
            {
                List<MedNeg.Inventarios.Producto> lProducto = (List<MedNeg.Inventarios.Producto>)Session["inventariofisicoproductos"];

                oblInventarios.EstablecerExistenciaProducto(lProducto, int.Parse(ddlInvFsAlmacen.SelectedValue));

                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Inventario";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Actualización del inventario físico";
                oBitacora.Descripcion = "Almacen: " + ddlInvFsAlmacen.SelectedItem.Text;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }

                foreach (MedNeg.Inventarios.Producto p in lProducto)
                {
                    oblInventarios.BloquearProducto(int.Parse(ddlInvFsAlmacen.SelectedValue), p.dbProducto.idProducto, false);
                }
                lblInvFsAvisos.Text = "El inventario fue actualizado exitosamente";
            }
            catch (Exception ex)
            {
                lblInvFsAvisos.Text = "Error: " + ex.Message;
            }
        }

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
            }

            

            MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();

            oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());

            if (oUsuario.FiltradoActivado == true)
            {
                var oQuery = oblInventarios.BuscarProductosAlmacenFiltradaAlmacen(sCadena, iTipo,oUsuario.idAlmacen);
                gdvDatos.DataSource = oQuery;
                Session["resultadoquery"] = oQuery;
            }
            else
            {
                var oQuery = oblInventarios.BuscarProductosAlmacen(sCadena, iTipo);
                gdvDatos.DataSource = oQuery;
                Session["resultadoquery"] = oQuery;
            }

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Inventarios.InventariosView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Clave ASC";
            gdvDatos.DataSource = dv;

            try
            {
                
                gdvDatos.DataKeyNames = new string[] { "idProAlmStocks" };
                gdvDatos.DataBind();
                CargarCatalogo();
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

        protected void Editar()
        {
            oProductosAlmacenStocks = new MedDAL.DAL.productos_almacen_stocks();
            oProductosAlmacenStocks.idProAlmStocks = int.Parse(gdvDatos.SelectedDataKey.Value.ToString());
            oProductosAlmacenStocks.StockMin = int.Parse(txbMinimo.Text);
            oProductosAlmacenStocks.StockMax = int.Parse(txbMaximo.Text);

            if (oblInventarios.EditarProductosAlmacenStock(oProductosAlmacenStocks))
            {
                lblAviso.Text = "El máximo y mínimo de stock se actualizó con éxito";
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Inventario";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Actualización de Stock";
                oBitacora.Descripcion = "Mínimo: " + txbMinimo.Text + ", Máximo: " + txbMaximo.Text;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "El máximo y mínimo de stock no pudo ser actualizado";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            //Obtener los controles de master
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';

            try
            {
                #region Interfaz
                cPermiso = (char)htbPermisos["inventarios"];

                Master.FindControl("btnEliminar").Visible = false;
                Master.FindControl("btnAlertaStock").Visible = true;
                
                imbFisico = (ImageButton)Master.FindControl("imgBtnNuevo");
                imbFisico.ImageUrl = "~/Icons/packing32.png";
                ((Label)(Master.FindControl("lblNuevo"))).Text = "Físico";
                imbFisico.Click += new ImageClickEventHandler(this.imbFisico_Click);
                imbEditar = (ImageButton)Master.FindControl("imgBtnEditar");
                imbEditar.Click += new ImageClickEventHandler(this.imbEditar_Click);
                imbReportes = (ImageButton)Master.FindControl("imgBtnReportes");
                imbReportes.Click += new ImageClickEventHandler(this.imbReportes_Click);
                imbMostrar = (ImageButton)Master.FindControl("imgBtnMostrar");
                imbMostrar.Click += new ImageClickEventHandler(this.imbMostrar_Click);
                imbImprimir = (ImageButton)Master.FindControl("imgBtnImprimir");
                imbImprimir.Click += new ImageClickEventHandler(this.imbImprimir_Click);
                imbAlertas = (ImageButton)Master.FindControl("imgBtnAlertas");
                imbAlertas.Click += new ImageClickEventHandler(this.imbAlertas_Click);                
                imbAceptar = (ImageButton)Master.FindControl("imgBtnAceptar");
                imbAceptar.Click += new ImageClickEventHandler(this.imbAceptar_Click);
                imbAceptar.ValidationGroup = "Inventarios";
                imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
                imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
                rdbTodos = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbTodos.Text = "Productos y almacenes";
                rdbClave = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbClave.Text = "Producto";
                rdbNombre = (RadioButton)Master.FindControl("rdbFiltro3");
                rdbNombre.Text = "Almacén";
                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
                txbBuscar = (TextBox)Master.FindControl("txtBuscar");
                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "Inventarios";
                lblInvFsAvisoPermanente.Visible = false;

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
                oblInventarios = new MedNeg.Inventarios.BlInventarios();
                oblBitacora = new MedNeg.Bitacora.BlBitacora();
                oblUsuario = new MedNeg.Usuarios.BlUsuarios();

                lblInvFsAvisos.Text = "";
                if (!IsPostBack)
                {
                    Session["accion"] = 0;
                    pnlFormulario.Visible = false;
                    pnlCatalogo.Visible = false;
                    divInventarioFísico.Visible = false;
                    pnlFiltroReportes.Visible = false;
                    //pnlReportes.Visible = false;
                    Session["resultadoquery"] = "";
                    Session["resultadoquery2"] = ""; //09/02/2012 JID sirve para el gridview de inventario fisico
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;

                    Session["reporteactivo"] = 0;
                    Session["reportdocument"] = "";
                    Session["titulo"] = "";

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
                divInventarioFísico.Visible = false;
                pnlFiltroReportes.Visible = false;
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }

        protected void imbEditar_Click(object sender, EventArgs e)
        {
            if (gdvDatos.SelectedIndex != -1)
            {
                CargarFormulario(true);
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);
            }
            else
            {
                Buscar("");
                CargarCatalogo();
            }
            Session["accion"] = 1;
            lblAviso.Text = "";
            lblAviso2.Text = "";
        }
        protected void imbFisico_Click(object sender, EventArgs e)
        {
            lblInvFsAvisoPermanente.Text = "Evite que los productos sean bloqueados guardando el inventario fisico";
            lblInvFsAvisoPermanente.Visible = true;
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;
            divInventarioFísico.Visible = true;
            //pnlReportes.Visible = false;
            Session["inventariofisicoproductos"] = null;
            grvInvFsArticulos.DataSource = null;
            grvInvFsArticulos.DataBind();
            cargaDdlInvFisico();
        }
        protected void imbMostrar_Click(object sender, EventArgs e)
        {
            Buscar("");
            gdvDatos.SelectedIndex = -1;
            Session["accion"] = 0;
            lblAviso.Text = "";
            lblAviso2.Text = "";
            ConfigurarMenuBotones(true, true, false, true, false, true, true, true);
        }

        protected void imbAlertas_Click(object sender, EventArgs e)
        {
            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
            oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());
            if (oUsuario.FiltradoActivado == true)
            {
                if (oblInventarios.BuscarProductosBajoStock(oUsuario.idAlmacen))
                {
                    if (!ClientScript.IsStartupScriptRegistered("alertstock"))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(),
                            "alertstock", "alertarStock(1);", true);
                    }
                }
                else
                {
                    if (!ClientScript.IsStartupScriptRegistered("alertstock"))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(),
                            "alertstock", "alertarStock(0);", true);
                    }
                }

                if (oblInventarios.BuscarProductosCaducos(oUsuario.idAlmacen))
                {
                    if (!ClientScript.IsStartupScriptRegistered("alertcaducidad"))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(),
                            "alertcaducidad", "alertarCaducidad(1);", true);
                    }
                }
                else
                {
                    if (!ClientScript.IsStartupScriptRegistered("alertcaducidad"))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(),
                            "alertcaducidad", "alertarCaducidad(0);", true);
                    }
                }
            }
            else
            {
                if (oblInventarios.BuscarProductosBajoStock())
                {
                    if (!ClientScript.IsStartupScriptRegistered("alertstock"))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(),
                            "alertstock", "alertarStock(1);", true);
                    }
                }
                else 
                {
                    if (!ClientScript.IsStartupScriptRegistered("alertstock"))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(),
                            "alertstock", "alertarStock(0);", true);
                    }
                }

                if (oblInventarios.BuscarProductosCaducos())
                {
                    if (!ClientScript.IsStartupScriptRegistered("alertcaducidad"))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(),
                            "alertcaducidad", "alertarCaducidad(1);", true);
                    }
                }
                else
                {
                    if (!ClientScript.IsStartupScriptRegistered("alertcaducidad"))
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(),
                            "alertcaducidad", "alertarCaducidad(0);", true);
                    }
                }
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
                    Editar();
                    Buscar("");
                    CargarFormulario(true);
                    Session["accion"] = 1;
                    break;
            }

            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
        }
        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            Session["accion"] = 0;
            pnlCatalogo.Visible = false;
            pnlFormulario.Visible = false;
            divInventarioFísico.Visible = false;
            //pnlReportes.Visible = false;
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar(txbBuscar.Text);
            gdvDatos.SelectedIndex = -1;
            Session["accion"] = 0;
            lblAviso.Text = "";
            lblAviso2.Text = "";
            ConfigurarMenuBotones(true, true, false, true, false, true, true, true);
        }

        protected void btnBuscarFisico_Click(object sender, EventArgs e)
        {
            CargaGdvInvFisico();
        }
        protected void grvInvFsArticulos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (grvInvFsArticulos.SelectedIndex != -1)
            {
                //03/10/2012 Parche agregado unas semanas antes para arreglar un bug de inventario
                int iSelectedIndex = grvInvFsArticulos.SelectedIndex;
                iSelectedIndex = iSelectedIndex.ToString().Length > 1 ? int.Parse(iSelectedIndex.ToString().Substring(1)) : iSelectedIndex;
                int indice = int.Parse(grvInvFsArticulos.PageIndex.ToString() + iSelectedIndex.ToString());
                if (indice < ((List<MedNeg.Inventarios.Producto>)Session["inventariofisicoproductos"]).Count)
                {
                    InvFscargaProductoEnTxtBxs(((List<MedNeg.Inventarios.Producto>)Session["inventariofisicoproductos"])//[grvInvFsArticulos.SelectedIndex]);
                        [indice]);
                    txtInvFsCantidadRealArticulo.Focus();
                }
                //
            }
        }
        protected void grvInvFsArticulos_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (grvInvFsArticulos.Rows.Count > 0)
            {
            
            }
        }
        protected void ckbInvFsTodos_CheckedChanged(object sender, EventArgs e)
        {
            ddlInvFsClave1Hasta.Visible = ddlInvFsClave1Desde.Visible = Label24.Visible = Label25.Visible = !ckbInvFsTodos.Checked;
        }
        protected void txtInvFsCantidadRealArticulo_TextChanged(object sender, EventArgs e)
        {            
            //03/10/2012 Parche agregado para el bug del inventario fisico 
            int iSelectedIndex = grvInvFsArticulos.SelectedIndex;
            iSelectedIndex = iSelectedIndex.ToString().Length > 1 ? int.Parse(iSelectedIndex.ToString().Substring(1)) : iSelectedIndex;
            int indice = int.Parse(grvInvFsArticulos.PageIndex.ToString() + iSelectedIndex.ToString());
            //
            ((List<MedNeg.Inventarios.Producto>)Session["inventariofisicoproductos"])[indice].existenciaReal = decimal.Parse(txtInvFsCantidadRealArticulo.Text);
            ((List<MedNeg.Inventarios.Producto>)Session["inventariofisicoproductos"])[indice].strExistenciaReal = txtInvFsCantidadRealArticulo.Text;

            grvInvFsArticulos.DataSource = ((List<MedNeg.Inventarios.Producto>)Session["inventariofisicoproductos"]);
            grvInvFsArticulos.DataBind();
                                    
            limpiaTextBox(this.divInventarioFísico);
            
            lblInvFsAvisos.Text = "Para actualizar, presione el botón Guardar";
            if (indice < ((List<MedNeg.Inventarios.Producto>)Session["inventariofisicoproductos"]).Count - 1)
            {
                indice++;
                if (indice > 9)
                {
                    int paginaGridView = Convert.ToInt32(indice.ToString().Substring(0, indice.ToString().Length - 1));
                    grvInvFsArticulos.PageIndex = paginaGridView;
                    grvInvFsArticulos.DataBind();
                }
                int indiceRow = Convert.ToInt32(indice.ToString().Substring(indice.ToString().Length - 1));
                grvInvFsArticulos.SelectRow(indiceRow);
                InvFscargaProductoEnTxtBxs(((List<MedNeg.Inventarios.Producto>)Session["inventariofisicoproductos"])[indice]);
                
            }

            txtInvFsCantidadRealArticulo.Text = string.Empty;
            txtInvFsCantidadRealArticulo.Focus();
            
        }
        protected void btnGuardarFisico_Click(object sender, EventArgs e)
        {
            FinalizaInventarioFisico();
        }

        #region Reportes

        public void CargarListaReportes()
        {
            ListBox lsbReportes = (ListBox)frReportes.FindControl("lsbSeleccionf");
            frReportes.LimpiarPaneles();
            lsbReportes.SelectedIndex = -1;
            lsbReportes.Items.Clear();
            if (Server.MapPath("~\\rptReportes\\Inventarios\\rptProductosCaducos.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de productos caducos");
            }
            if (Server.MapPath("~\\rptReportes\\Inventarios\\rptComprasVendedor.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de compras por proveedor");
            }
            if (Server.MapPath("~\\rptReportes\\Inventarios\\rptVentasPeriodo.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de ventas del periodo");
            }
            if (Server.MapPath("~\\rptReportes\\Inventarios\\rptVentasCliente.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de ventas por cliente");
            }
            if (Server.MapPath("~\\rptReportes\\Inventarios\\rptNumerosSerie.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de números de serie");
            }
            if (Server.MapPath("~\\rptReportes\\Inventarios\\rptPedimentosAduanalesLotes.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de pedimentos aduanales y lotes");
            }
            if (Server.MapPath("~\\rptReportes\\Inventarios\\rptHistorialExistencias.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de historial de existencias");
            }            
        }

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            //GT0175
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;
            divInventarioFísico.Visible = false;
            pnlFiltroReportes.Visible = true;

            CargarListaReportes();
        }

        protected void CargarReporte()
        {
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;
            divInventarioFísico.Visible = false;
            pnlFiltroReportes.Visible = false;
            //pnlReportes.Visible = true;
            MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            odsDataSet = (bool)oUsuario.FiltradoActivado ? (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes where idAlmacen = " + oUsuario.idAlmacen, "medicuriConnectionString", odsDataSet, "almacenes")
                :
                (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos", "medicuriConnectionString", odsDataSet, "productos");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen", "medicuriConnectionString", odsDataSet, "productos_almacen");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from productos_almacen_stocks", "medicuriConnectionString", odsDataSet, "productos_almacen_stocks");

            //GT 0179
            Session["campoaordenar"] = "";
            Session["sortfield"] = 0;
            Session["recordselection"] = "";

            Session["dataset"] = odsDataSet;
            Session["reportdocument"] = "~\\rptReportes\\rptInventarios.rpt";
            Session["titulo"] = "Inventario";
            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
            
            rptReporte.SetDataSource(odsDataSet);
            
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
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
            CargarReporte();
        }
        #endregion

        #region SortingPaging

        protected void gdvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Inventarios.InventariosView>)Session["resultadoquery"];
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
            var result = (IQueryable<MedDAL.Inventarios.InventariosView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            gdvDatos.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Clave" : ViewState["sortexpression"].ToString(), dv, ref gdvDatos, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            gdvDatos.DataBind();
        }

        protected void grvInvFsArticulos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedNeg.Inventarios.Producto>)Session["resultadoquery2"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            grvInvFsArticulos.DataSource = oMaster.Paging(e, "codigo", dv, ref grvInvFsArticulos, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            grvInvFsArticulos.DataBind();
        }

        #endregion
        
        

        

    }
}