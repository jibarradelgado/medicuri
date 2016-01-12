using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Medicuri
{
    public partial class Causes : System.Web.UI.Page
    {
        ImageButton imbNuevo, imbEditar, imbEliminar, imbMostrar, imbAceptar, imbCancelar, imbReportes;
        RadioButton rdbFiltro3, rdbFiltro2, rdbFiltro1;
        Button btnBuscar;
        TextBox txbBuscar;
        Label lblNombreModulo;
        MedNeg.Causes.BlCauses oblCauses;
        MedNeg.Bitacora.BlBitacora oblBitacora;
        MedNeg.Productos.BlProductos oblProductos;
        MedDAL.DAL.causes oCauses;
        MedDAL.DAL.bitacora oBitacora;

        public void LimpiarValores(Control c)
        {
            if (c is TextBox)
            {
                ((TextBox)c).Text = "";
            }
            else if (c is CheckBox)
            {
                ((CheckBox)c).Checked = false;
            }

            foreach (Control ctrl in c.Controls)
            {
                LimpiarValores(ctrl);
            }
        }

        private void CargarCampos(bool bDatos)
        {
            if (!bDatos)
            {
                LimpiarValores(tbcForm);
            }
            else 
            {
                //oCauses = ((List<MedDAL.DAL.causes>)Session["lstcauses"])[gdvDatos.SelectedIndex];
                int idCause = (int)gdvDatos.SelectedValue;
                MedDAL.DAL.causes oCauses = oblCauses.Buscar(idCause);

                txbClave.Text = oCauses.Clave;
                txbNombre.Text = oCauses.Nombre;
                txbConglomerado.Text = oCauses.Conglomerado;
                txaDescripcion.Text = oCauses.Descripcion;

                List<MedDAL.DAL.causes_cie> lstCausesCIE = new List<MedDAL.DAL.causes_cie>();
                lstCausesCIE.AddRange(oCauses.causes_cie);

                Session["lstcie"] = lstCausesCIE;
                gdvCatalogoCIE.DataSource = lstCausesCIE;
                gdvCatalogoCIE.DataBind();

                List<MedNeg.Causes.BLCausesMedicamentos> lstCausesMedicamentos = new List<MedNeg.Causes.BLCausesMedicamentos>();
                                
                foreach (MedDAL.DAL.causes_medicamentos oCauseMedicamento in oCauses.causes_medicamentos)
                { 
                    MedDAL.DAL.productos oProducto = oblProductos.Buscar(int.Parse(oCauseMedicamento.idProducto.ToString())); 
                    
                    lstCausesMedicamentos.Add(new MedNeg.Causes.BLCausesMedicamentos(oProducto.Clave1, oProducto.Nombre, oProducto.Presentacion, oProducto.idProducto, oCauseMedicamento.Descripcion, oCauseMedicamento.CuadroBasico));
                }

                Session["lstmedicamentos"] = lstCausesMedicamentos;
                gdvCausesMedicamentos.DataSource = lstCausesMedicamentos;
                gdvCausesMedicamentos.DataBind();                
            }
        }

        private void CargarFormulario(bool bDatos)
        {
            upnForm.Visible = true;
            pnlList.Visible = false;
            
            CargarCampos(bDatos);            
        }

        private void CargarCatalogo() 
        {
            upnForm.Visible = false;
            pnlList.Visible = true;
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

        protected void LimpiarCie()
        {
            txbClaveCIE.Text = "";
            txbTipoCIE.Text = "";
            txaDescripcionCIE.Text = "";
        }

        protected void LimpiarMedicamentos()
        {
            txbClaveMedicamento.Text = "";
            txbNombreMedicamento.Text = "";
            txbPresentacionMedicamento.Text = "";
            txbDescripcion.Text = "";
            txbCuadroBasico.Text = "";
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
                txbNombreMedicamento.Text = oProducto.Nombre;
                txbPresentacionMedicamento.Text = oProducto.Presentacion;
                txbidProducto.Text = oProducto.idProducto.ToString();

                txbDescripcion.Focus();
            }
            catch
            {
                txbClaveMedicamento.Focus();
            }
        }

        #region InteraccionBL
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
            else if (rdbFiltro3.Checked)
            {
                iTipo = 3;
            }

            //List<MedDAL.DAL.causes> lstCauses = oblCauses.Buscar(sCadena, iTipo);
            //Session["lstcauses"] = lstCauses;

            var oQuery = oblCauses.Buscar(sCadena, iTipo);
            Session["resultadoquery"] = oQuery;

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.DAL.causes>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Clave ASC";
            gdvDatos.DataSource = dv;

            try
            {
                //gdvDatos.DataSource = lstCauses;
                //gdvLista.DataKeyNames = new string[] { "" };
                gdvDatos.DataBind();
                gdvDatos.Visible = true;
                if (txbBuscar.Text == "")
                {
                    gdvDatos.EmptyDataText = "No existen causes registrados aun";
                }
                else
                {
                    gdvDatos.EmptyDataText = "No existen causes que coincidan con la búsqueda";
                }
                gdvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void Nuevo()
        {
            int iErrores = 0;

            oCauses = new MedDAL.DAL.causes();
            oCauses.Clave = txbClave.Text;
            oCauses.Nombre = txbNombre.Text;
            oCauses.Descripcion = txaDescripcion.Text;
            oCauses.Conglomerado = txbConglomerado.Text;

            if (oblCauses.NuevoRegistro(oCauses))
            {
                lblAviso1.Text = "El CAUSES ha sido agregado con éxito";

                oCauses = oblCauses.Buscar(txbClave.Text);

                foreach (MedDAL.DAL.causes_cie oCausesCie in (List<MedDAL.DAL.causes_cie>)Session["lstcie"])
                {
                    oCausesCie.idCause = oCauses.idCause;
                    if (!oblCauses.NuevoRegistro(oCausesCie))
                    {
                        iErrores++;
                    }
                }

                lblAviso2.Text = iErrores != 0 ? "No se agregaron los CIE del CAUSES" : "";

                iErrores = 0;

                foreach (MedNeg.Causes.BLCausesMedicamentos oBlCausesMedicamento in (List<MedNeg.Causes.BLCausesMedicamentos>)Session["lstmedicamentos"])
                {
                    MedDAL.DAL.causes_medicamentos oCausesMedicamento = new MedDAL.DAL.causes_medicamentos();
                    oCausesMedicamento.idCause = oCauses.idCause;
                    oCausesMedicamento.idProducto = oBlCausesMedicamento.idMedicamento;
                    oCausesMedicamento.Descripcion = oBlCausesMedicamento.Descripcion;
                    oCausesMedicamento.CuadroBasico = oBlCausesMedicamento.CuadroBasico;

                    if (!oblCauses.NuevoRegistro(oCausesMedicamento))
                    {
                        iErrores++;
                    }
                }

                lblAviso3.Text = iErrores != 0 ? "No se agregaron los medicamentos del CAUSES" : "";

                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "CAUSES";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Nuevo CAUSES";
                oBitacora.Descripcion = "Clave: " + txbClave.Text + ", Nombre: " + txbNombre.Text;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso4.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso1.Text = "No se agrego el nuevo CAUSES";
            }
        }

        protected void Editar() 
        {
            int iErrores = 0;

            oCauses = new MedDAL.DAL.causes();
            oCauses.idCause = int.Parse(gdvDatos.SelectedDataKey.Values[0].ToString());
            oCauses.Clave = txbClave.Text;
            oCauses.Nombre = txbNombre.Text;
            oCauses.Descripcion = txaDescripcion.Text;
            oCauses.Conglomerado = txbConglomerado.Text;

            if (oblCauses.EditarRegistro(oCauses))
            {
                lblAviso1.Text = "El CAUSES ha sido editado con éxito";

                oblCauses.EliminarRegistroCie(oCauses);
                
                foreach (MedDAL.DAL.causes_cie oCausesCie in (List<MedDAL.DAL.causes_cie>)Session["lstcie"])
                {
                    MedDAL.DAL.causes_cie oCauseCieNuevo = new MedDAL.DAL.causes_cie();
                    oCauseCieNuevo.idCause = oCausesCie.idCause;
                    oCauseCieNuevo.Clave = oCausesCie.Clave;
                    oCauseCieNuevo.Tipo = oCausesCie.Tipo;
                    oCauseCieNuevo.Descripcion = oCausesCie.Descripcion;

                    if (!oblCauses.NuevoRegistro(oCauseCieNuevo))
                    {
                        iErrores++;
                    }
                }

                lblAviso2.Text = iErrores != 0 ? "No se agregaron los CIE del CAUSES" : "";

                iErrores = 0;

                oblCauses.EliminarRegistroMedicamento(oCauses);

                foreach (MedNeg.Causes.BLCausesMedicamentos oBlCausesMedicamento in (List<MedNeg.Causes.BLCausesMedicamentos>)Session["lstmedicamentos"])
                {
                    MedDAL.DAL.causes_medicamentos oCausesMedicamentoNuevo = new MedDAL.DAL.causes_medicamentos();
                    oCausesMedicamentoNuevo.idCause = oCauses.idCause;
                    oCausesMedicamentoNuevo.idProducto = oBlCausesMedicamento.idMedicamento;
                    oCausesMedicamentoNuevo.Descripcion = oBlCausesMedicamento.Descripcion;
                    oCausesMedicamentoNuevo.CuadroBasico = oBlCausesMedicamento.CuadroBasico;

                    if (!oblCauses.NuevoRegistro(oCausesMedicamentoNuevo))
                    {
                        iErrores++;
                    }
                }

                lblAviso3.Text = iErrores != 0 ? "No se agregaron los medicamentos del CAUSES" : "";

                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "CAUSES";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Editar CAUSES";
                oBitacora.Descripcion = "Clave: " + txbClave.Text + ", Nombre: " + txbNombre.Text;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso4.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso1.Text = "No se editó el CAUSES seleccionado";
            }
        }

        protected void Eliminar()
        {
            //MedDAL.DAL.causes oCauses = ((List<MedDAL.DAL.causes>)Session["lstcauses"])[gdvDatos.SelectedIndex];
            //string sClave = oCauses.Clave;

            int idCause = (int)gdvDatos.SelectedValue;
            MedDAL.DAL.causes oCauses = oblCauses.Buscar(idCause);
            string sClave = oCauses.Clave;

            if (oCauses.causes_cie.Count == 0 && oCauses.causes_medicamentos.Count == 0)
            {
                if (oblCauses.EliminarRegistro(oCauses.idCause))
                {
                    lblAviso1.Text = "El CAUSES fue eliminado";
                    oBitacora = new MedDAL.DAL.bitacora();
                    oBitacora.FechaEntradaSrv = DateTime.Now;
                    oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                    oBitacora.Modulo = "CAUSES";
                    oBitacora.Usuario = Session["usuario"].ToString();
                    oBitacora.Nombre = Session["nombre"].ToString();
                    oBitacora.Accion = "CAUSES Eliminado";
                    oBitacora.Descripcion = "Clave: " + sClave;
                    if (!oblBitacora.NuevoRegistro(oBitacora))
                    {
                        lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                    }
                }
                else
                {
                    lblAviso1.Text = "El CAUSES no pudo ser eliminado, es posible que tenga datos relacionados";
                }
            }
            else 
            {
                lblAviso1.Text = "El CAUSES no pudo ser eliminado, es posible que tenga datos relacionados";
            }
        }
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';
            try
            {
                #region Interfaz
                cPermiso = (char)htbPermisos["causes"];
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
                imbAceptar.ValidationGroup = "causes";
                imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
                imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
                rdbFiltro1 = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbFiltro1.Text = "Nombre y Clave";
                rdbFiltro2 = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbFiltro2.Text = "Clave";
                rdbFiltro3 = (RadioButton)Master.FindControl("rdbFiltro3");
                rdbFiltro3.Text = "Nombre";
                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
                txbBuscar = (TextBox)Master.FindControl("txtBuscar");
                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "CAUSES";
                Master.FindControl("btnReportes").Visible = false;
                Master.FindControl("btnImprimir").Visible = false;

                imbAgregarMedicamento.Click += new ImageClickEventHandler(imbAgregarMedicamento_Click);
                txbClaveMedicamento.TextChanged += new EventHandler(txbClaveMedicamento_TextChanged);
                txbNombreMedicamento.TextChanged += new EventHandler(txbNombreMedicamento_TextChanged);

                //GT 0175
                imbReportes = (ImageButton)Master.FindControl("imgBtnReportes");
                imbReportes.Click += new ImageClickEventHandler(this.imbReportes_Click);

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

                oblBitacora = new MedNeg.Bitacora.BlBitacora();
                oblCauses = new MedNeg.Causes.BlCauses();
                oblProductos = new MedNeg.Productos.BlProductos();

                if (!IsPostBack)
                {
                    Session["lstcie"] = new List<MedDAL.DAL.causes_cie>();
                    Session["lstmedicamentos"] = new List<MedNeg.Causes.BLCausesMedicamentos>();
                    Session["accion"] = 1;
                    CargarFormulario(false);
                    Session["resultadoquery"] = "";
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    upnForm.Visible = false;
                }

                gdvCatalogoCIE.Visible = true;
                gdvCatalogoCIE.DataSource = ((List<MedDAL.DAL.causes_cie>)Session["lstcie"]);
                gdvCatalogoCIE.DataBind();

                gdvCausesMedicamentos.Visible = true;
                gdvCausesMedicamentos.DataSource = ((List<MedNeg.Causes.BLCausesMedicamentos>)Session["lstmedicamentos"]);
                gdvCausesMedicamentos.DataBind();
            }
            catch (NullReferenceException)
            {
                if (!ClientScript.IsStartupScriptRegistered("alertsession"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alertsession", "alertarSesion();", true);
                }
                CargarFormulario(false);
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }

        protected void imbNuevo_Click(object sender, EventArgs e)
        {
            CargarFormulario(false);
            gdvDatos.SelectedIndex = -1;
            Session["accion"] = 1;
            ((List<MedDAL.DAL.causes_cie>)Session["lstcie"]).Clear();
            ((List<MedNeg.Causes.BLCausesMedicamentos>)Session["lstmedicamentos"]).Clear();
            DataBind();
            lblAviso1.Text = "";
            lblAviso2.Text = "";
            lblAviso3.Text = "";
            lblAviso4.Text = "";
            //0175 GT
            ConfigurarMenuBotones(true, false, false, false, true, true, false, false);
        }

        protected void imbImprimir_Click(object sender, EventArgs e)
        {
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
            //CargarReporte();
        }
        protected void imbEditar_Click(object sender, EventArgs e)
        {
            if (gdvDatos.SelectedIndex != -1)
            {
                CargarFormulario(true);
                Session["accion"] = 2;
                //0175 GT
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);
            }
            else
            {
                CargarCatalogo();
                Buscar("");
                //0175 GT
                ConfigurarMenuBotones(false, false, false, true, true, true, false, false);
            }
            lblAviso1.Text = "";
            lblAviso2.Text = "";
            lblAviso3.Text = "";
            lblAviso4.Text = "";
        }

        protected void imbEliminar_Click(object sender, EventArgs e)
        {
            if (pnlList.Visible && gdvDatos.SelectedIndex != -1)
            {
                Eliminar();
                Buscar(txbBuscar.Text);
                //CargarCatalogo();
            }
            else
            {
                CargarCatalogo();
                Buscar(txbBuscar.Text);
            }
        }

        protected void imbMostrar_Click(object sender, EventArgs e)
        {
            CargarCatalogo();

            Buscar("");

            if (gdvDatos.Rows.Count != 0)
            {
                gdvDatos.SelectedIndex = -1;
                Session["accion"] = 0;
                lblAviso1.Text = "";
                lblAviso2.Text = "";
                lblAviso3.Text = "";
                lblAviso4.Text = "";
                //0175 GT
                ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
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
                    if (txbClave.Enabled && cmvClave.IsValid)
                    {
                        Nuevo();
                        CargarFormulario(false);
                        ((List<MedDAL.DAL.causes_cie>)Session["lstcie"]).Clear();
                        ((List<MedNeg.Causes.BLCausesMedicamentos>)Session["lstmedicamentos"]).Clear();
                        DataBind();
                        //GT 0175
                        ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    }
                    Session["accion"] = 1;
                    break;
                case 2:
                    Editar();
                    //CargarFormulario(true);                    
                    //Buscar("", int.Parse(cmbMunicipioCatalogo.SelectedValue));
                    Session["accion"] = 2;
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    break;
            }
        }

        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
            CargarFormulario(false);
            Session["accion"] = 1;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarCatalogo();
            Buscar(txbBuscar.Text);
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }

        protected void cmvClave_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (int.Parse(Session["accion"].ToString()) != 2)
            {
                string sClave = args.Value.ToString();
                MedDAL.DAL.causes oCauses = oblCauses.Buscar(sClave);
                args.IsValid = oCauses == null ? true : false;
            }
        }

        protected void gdvCatalogoCIE_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((List<MedDAL.DAL.causes_cie>)Session["lstcie"]).RemoveAt(gdvCatalogoCIE.SelectedIndex);
            gdvCatalogoCIE.DataBind();
        }

        protected void gdvCausesMedicamentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((List<MedNeg.Causes.BLCausesMedicamentos>)Session["lstmedicamentos"]).RemoveAt(gdvCausesMedicamentos.SelectedIndex);
            gdvCausesMedicamentos.DataBind();
            gdvCausesMedicamentos.SelectedIndex = -1;
        }

        protected void gdvCatalogoCIE_SelectedIndexChanged1(object sender, EventArgs e)
        {
            ((List<MedDAL.DAL.causes_cie>)Session["lstcie"]).RemoveAt(gdvCatalogoCIE.SelectedIndex);
            gdvCatalogoCIE.DataBind();
            gdvCatalogoCIE.SelectedIndex = -1;
        }

        protected void imbAgregarCie_Click(object sender, ImageClickEventArgs e)
        {
            MedDAL.DAL.causes_cie oCausesCie = new MedDAL.DAL.causes_cie();
            oCausesCie.Clave = txbClaveCIE.Text;
            oCausesCie.Tipo = txbTipoCIE.Text;
            oCausesCie.Descripcion = txaDescripcionCIE.Text;
            ((List<MedDAL.DAL.causes_cie>)Session["lstcie"]).Add(oCausesCie);
            gdvCatalogoCIE.DataBind();

            LimpiarCie();
        }

        protected void imbAgregarMedicamento_Click(object sender, ImageClickEventArgs e)
        {
            MedNeg.Causes.BLCausesMedicamentos oCausesMedicamento = new MedNeg.Causes.BLCausesMedicamentos();
            oCausesMedicamento.idMedicamento = int.Parse(txbidProducto.Text);
            oCausesMedicamento.Clave = txbClaveMedicamento.Text;
            oCausesMedicamento.Nombre = txbNombreMedicamento.Text;
            oCausesMedicamento.Presentacion = txbPresentacionMedicamento.Text;
            oCausesMedicamento.Descripcion = txbDescripcion.Text;
            oCausesMedicamento.CuadroBasico = txbCuadroBasico.Text;
            ((List<MedNeg.Causes.BLCausesMedicamentos>)Session["lstmedicamentos"]).Add(oCausesMedicamento);

            gdvCausesMedicamentos.DataBind();

            LimpiarMedicamentos();
        }

        protected void txbClaveMedicamento_TextChanged(object sender, EventArgs e)
        {
            CargaDatosProducto(txbClaveMedicamento.Text);            
        }

        protected void txbNombreMedicamento_TextChanged(object sender, EventArgs e)
        { 
        
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

        #region Reportes
        protected void imbReportes_Click(object sender, EventArgs e)
            {
                //GT0175
                ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
            }

        #endregion

        #region PagingSorting

        protected void gdvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.DAL.causes>)Session["resultadoquery"];
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
            var result = (IQueryable<MedDAL.DAL.causes>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            gdvDatos.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Clave" : ViewState["sortexpression"].ToString(), dv, ref gdvDatos, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            gdvDatos.DataBind();
        }

        #endregion
    }
}