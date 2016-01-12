using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using MedNeg.LineasCredito; //Capa de negocio de lineas de credito
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Medicuri
{
    public partial class LineasCredito : System.Web.UI.Page
    {
        ImageButton imbNuevo, imbEditar, imbEliminar, imbImprimir, imbMostrar, imbAceptar, imbCancelar, imbReportes;
        RadioButton rdbNombre, rdbClave, rdbTodos;
        Label lblReportes, lblNombreModulo;
        Button btnBuscar;
        TextBox txbBuscar;

        MedNeg.CamposEditables.BlCamposEditables oblCamposEditables;
       
        //Declaración del objeto de la capa de negocio de la bitacora
        MedNeg.Bitacora.BlBitacora oblBitacora;
       
        //Declaración del objeto de la capa de Datos de bitacora
        MedDAL.DAL.bitacora oBitacora;

        //Declaración del objeto de la capa de negocio de líneas de crédito
        MedNeg.LineasCredito.BlLineasCredito oblLineaCredito;

        //Declaración del objeto de la capa de datos de líneas de crédito
        MedDAL.DAL.lineas_creditos oLineaCredito;


        protected string QuitarFormatoMoneda(string sParametro)
        {
            //string sPattern="[\\.\\,]";
            string sReplacement=string.Empty;
            Regex regex = new Regex("[\\$\\.\\,]");

            return regex.Replace(sParametro, sReplacement);

        }


        protected void CargarFormulario(bool bDatos)
        {
            pnlFormulario.Visible = true;
            pnlCatalogo.Visible = false;
            pnlFiltroReportes.Visible = false;
            //pnlReportes.Visible = false;
            CargarCamposEditables();

            if (bDatos)
            {
                //Crear el objeto que contendra los datos a mostrar
                MedDAL.DAL.lineas_creditos oLineaCredito = new MedDAL.DAL.lineas_creditos();
                MedNeg.LineasCredito.BlLineasCredito oblLineaCredito = new MedNeg.LineasCredito.BlLineasCredito();
                int iIdLineaCredito = Convert.ToInt32(dgvDatos.SelectedDataKey[0].ToString());

                oLineaCredito = (MedDAL.DAL.lineas_creditos)oblLineaCredito.Buscar(iIdLineaCredito);

                txbClave.Text = dgvDatos.SelectedRow.Cells[1].Text;
                txbInstitucion.Text = dgvDatos.SelectedRow.Cells[2].Text;
                txbFuente.Text = dgvDatos.SelectedRow.Cells[3].Text;
                txbCuenta.Text = dgvDatos.SelectedRow.Cells[4].Text;
                txbMonto.Text = String.Format("{0:C}", dgvDatos.SelectedRow.Cells[5].Text);
                txbFecha.Text = dgvDatos.SelectedRow.Cells[6].Text;
                txbVence.Text = dgvDatos.SelectedRow.Cells[7].Text;
                chkActivo.Checked = ((CheckBox)dgvDatos.SelectedRow.Cells[8].FindControl("ctl01")).Checked;

                txbCampo1.Text = oLineaCredito.Campo1 == null? "" : oLineaCredito.Campo1.ToString();
                txbCampo2.Text = oLineaCredito.Campo2 == null ? "" : oLineaCredito.Campo2.ToString();
                txbCampo3.Text = oLineaCredito.Campo3 == null ? "" : oLineaCredito.Campo3.ToString();
                txbCampo4.Text = oLineaCredito.Campo4 == null ? "" : oLineaCredito.Campo4.ToString();
                txbCampo5.Text = oLineaCredito.Campo5 == null ? "" : oLineaCredito.Campo5.ToString();
                txbCampo6.Text = oLineaCredito.Campo6 == null ? "" : oLineaCredito.Campo6.ToString();
                txbCampo7.Text = oLineaCredito.Campo7 == null ? "" : oLineaCredito.Campo7.ToString();
                txbCampo8.Text = oLineaCredito.Campo8 == null ? "" : oLineaCredito.Campo8.ToString();
                txbCampo9.Text = oLineaCredito.Campo9 == null ? "" : oLineaCredito.Campo9.ToString();
                txbCampo10.Text = oLineaCredito.Campo10 == null ? "" : oLineaCredito.Campo10.ToString();
                
                Habilita();
                txbClave.Enabled = false;
                
            }
            else
            {
               
                Limpia();
                Deshabilita();
                
            }
        }

        protected void Habilita()
        {
            txbClave.Enabled = true;
           txbInstitucion.Enabled=true;
           txbFuente.Enabled = true;
           txbCuenta.Enabled = true;
           txbMonto.Enabled = true;
           txbFecha.Enabled = true;
           txbVence.Enabled = true;
           chkActivo.Enabled = true;
           txbCampo1.Enabled = true;
           txbCampo2.Enabled = true;
           txbCampo3.Enabled = true;
           txbCampo4.Enabled = true;
           txbCampo5.Enabled = true;
           txbCampo6.Enabled = true;
           txbCampo7.Enabled = true;
           txbCampo8.Enabled = true;
           txbCampo9.Enabled = true;
           txbCampo10.Enabled = true;
           
        }

        protected void Deshabilita()
        {
            txbClave.Enabled = false;
            txbInstitucion.Enabled = false;
            txbFuente.Enabled = false;
            txbCuenta.Enabled = false;
            txbMonto.Enabled = false;
            txbFecha.Enabled = false;
            txbVence.Enabled = false;
            chkActivo.Enabled = false;
            txbCampo1.Enabled = false;
            txbCampo2.Enabled = false;
            txbCampo3.Enabled = false;
            txbCampo4.Enabled = false;
            txbCampo5.Enabled = false;
            txbCampo6.Enabled = false;
            txbCampo7.Enabled = false;
            txbCampo8.Enabled = false;
            txbCampo9.Enabled = false;
            txbCampo10.Enabled = false;
        }

        protected void Limpia()
        {
            txbClave.Text = "";
            txbInstitucion.Text = "";
            txbFuente.Text = "";
            txbCuenta.Text = "";
            txbMonto.Text = "";
            txbFecha.Text = "";
            txbVence.Text = "";
            chkActivo.Checked = false;
            lblAviso.Text = "";
            lblAviso2.Text = "";
            lblAviso3.Text = "";
        }


        protected void CargarCatalogo()
        {
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = true;
            pnlFiltroReportes.Visible = false;
            //pnlReportes.Visible = false;
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

            //var oQuery = oblEstados.Buscar(sCadena, iTipo);
            var oQuery = oblLineaCredito.Buscar(sCadena, iTipo);
            
            try
            {              
                dgvDatos.DataSource = oQuery;
                dgvDatos.DataKeyNames = new string[] { "idLineaCredito" };
                dgvDatos.DataBind();
               
                CargarCatalogo();
                if (dgvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    dgvDatos.EmptyDataText = "No existen líneas de crédito registradas aun";
                }
                else
                {
                    dgvDatos.EmptyDataText = "No existen líneas de crédito que coincidan con la búsqueda";
                }
                dgvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }


        protected void MostrarLista()
        {
            //var oQuery = oblEstados.Buscar(sCadena, iTipo);
            var oQuery = oblLineaCredito.MostrarLista();
            Session["resultadoquery"] = oQuery;

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.DAL.lineas_creditos>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Clave ASC";            

            try
            {
                dgvDatos.DataSource = dv;
                dgvDatos.DataKeyNames = new string[] { "idLineaCredito" };
                dgvDatos.DataBind();
                CargarCatalogo();
                if (dgvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    dgvDatos.EmptyDataText = "No existen líneas de crédito registradas aun";
                }
                else
                {
                    dgvDatos.EmptyDataText = "No existen líneas de crédito que coincidan con la búsqueda";
                }
                dgvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected bool ValidarLineaCreditoRepetida()
        {
            MedNeg.LineasCredito.BlLineasCredito oblLineaCredito = new MedNeg.LineasCredito.BlLineasCredito();

            //if (oblUsuario.ValidarUsuarioRepetido(txbUsuario.Text) >= 1)
            if (oblLineaCredito.ValidarClaveRepetida(txbClave.Text) >= 1)
                return false;
            else
                return true;
        }

        protected void Nuevo()
        {

            if (ValidarControles())
            {
                if(ValidarFechas())
                {
                    if (ValidarLineaCreditoRepetida())
                    {
                        //Declarar el objeto nuevo linea de credito a registrar
                        oLineaCredito = new MedDAL.DAL.lineas_creditos();

                        oLineaCredito.Clave = txbClave.Text;
                        oLineaCredito.InstitucionEmisora = txbInstitucion.Text;
                        oLineaCredito.FuenteCuenta = txbFuente.Text;
                        oLineaCredito.NumeroCuenta = txbCuenta.Text;
                        oLineaCredito.Monto = Convert.ToDecimal(txbMonto.Text);
                        oLineaCredito.FechaMinistracion = Convert.ToDateTime(txbFecha.Text);
                        oLineaCredito.FechaVencimiento = Convert.ToDateTime(txbVence.Text);
                        if (txbCampo6.Text.Equals(""))
                            oLineaCredito.Campo6 = 0;
                        else
                            oLineaCredito.Campo6 = Convert.ToInt32(txbCampo6.Text);

                        if (txbCampo7.Text.Equals(""))
                            oLineaCredito.Campo7 = 0;
                        else
                            oLineaCredito.Campo7 = Convert.ToInt32(txbCampo7.Text);

                        if (txbCampo8.Text.Equals(""))
                            oLineaCredito.Campo8 = 0;
                        else
                            oLineaCredito.Campo8 = Convert.ToInt32(txbCampo8.Text);

                        if (txbCampo9.Text.Equals(""))
                            oLineaCredito.Campo9 = 0;
                        else
                            oLineaCredito.Campo9 = Convert.ToDecimal(txbCampo9.Text);

                        if (txbCampo10.Text.Equals(""))
                            oLineaCredito.Campo10 = 0;
                        else
                            oLineaCredito.Campo10 = Convert.ToDecimal(txbCampo10.Text);


                        if (chkActivo.Checked == true)
                            oLineaCredito.Activo = true;
                        else
                            oLineaCredito.Activo = false;



                        //Intentar insertar el registro en la base de datos
                        if (oblLineaCredito.NuevoRegistro(oLineaCredito))
                        {
                            lblAviso.Text = "La línea de crédito se ha registrado con éxito";
                            oBitacora = new MedDAL.DAL.bitacora();
                            oBitacora.FechaEntradaSrv = DateTime.Now;
                            oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                            oBitacora.Modulo = "Líneas de Crédito";
                            oBitacora.Usuario = Session["usuario"].ToString();
                            oBitacora.Nombre = Session["nombre"].ToString();
                            oBitacora.Accion = "Nueva Línea Crédito";
                            oBitacora.Descripcion = "Institución: " + txbInstitucion.Text + ", Fuente: " + txbFuente.Text + ", Cuenta:" + txbCuenta.Text + ", Monto: " + txbMonto.Text;

                            if (!oblBitacora.NuevoRegistro(oBitacora))
                            {
                                lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                            }

                            Deshabilita();
                            Session["accion"] = 0;
                        }
                        else
                        {
                            lblAviso.Text = "La línea de crédito no pudo ser registrado";
                        }
                    }
                    else
                    {
                        lblAviso.Text = "Ya existe una línea de crédito con clave: "+txbClave.Text;
                    }
                }
            }
            
        }
        /// <summary>
        /// Carga el texto que debe de aparecer en los labels de campos editables
        /// </summary>
        protected void CargarCamposEditables()
        {
            List<MedDAL.DAL.campos_editables> lstCamposEditables = oblCamposEditables.Buscar("LineasCredito");
            lblAlfanumerico1.Text = lstCamposEditables[0].Valor != "" ? lstCamposEditables[0].Valor : lstCamposEditables[0].Campo;
            lblAlfanumerico2.Text = lstCamposEditables[1].Valor != "" ? lstCamposEditables[1].Valor : lstCamposEditables[1].Campo;
            lblAlfanumerico3.Text = lstCamposEditables[2].Valor != "" ? lstCamposEditables[2].Valor : lstCamposEditables[2].Campo;
            lblAlfanumerico4.Text = lstCamposEditables[3].Valor != "" ? lstCamposEditables[3].Valor : lstCamposEditables[3].Campo;
            lblAlfanumerico5.Text = lstCamposEditables[4].Valor != "" ? lstCamposEditables[4].Valor : lstCamposEditables[4].Campo;

            lblEntero1.Text = lstCamposEditables[5].Valor != "" ? lstCamposEditables[5].Valor : lstCamposEditables[5].Campo;
            lblEntero2.Text = lstCamposEditables[6].Valor != "" ? lstCamposEditables[6].Valor : lstCamposEditables[6].Campo;
            lblEntero3.Text = lstCamposEditables[7].Valor != "" ? lstCamposEditables[7].Valor : lstCamposEditables[7].Campo;

            lblDecimal1.Text = lstCamposEditables[8].Valor != "" ? lstCamposEditables[8].Valor : lstCamposEditables[8].Campo;
            lblDecimal2.Text = lstCamposEditables[9].Valor != "" ? lstCamposEditables[9].Valor : lstCamposEditables[9].Campo;
        }

        protected bool ValidarControles()
        {
            rfvClave.Validate();
            rfvInstitucion.Validate();
            rfvFuente.Validate();
            rfvMonto.Validate();
            rfvMinistracion.Validate();
            rfvVence.Validate();
            
            return Page.IsValid;
        }

        protected bool ValidarFechas()
        {
            if (DateTime.Compare(Convert.ToDateTime(txbFecha.Text), Convert.ToDateTime(txbVence.Text)) > 0)
            {
                lblAviso3.Text = "La fecha de vigencia no puede ser menor a la fecha de ministración";
                return false;
            }
            else
            {
                lblAviso3.Text = "";
                return true;
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

            oblLineaCredito = new MedNeg.LineasCredito.BlLineasCredito();
            oblBitacora = new MedNeg.Bitacora.BlBitacora();
            oblCamposEditables = new MedNeg.CamposEditables.BlCamposEditables();
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';

            try
            {
                #region Interfaz
                cPermiso = (char)htbPermisos["lineas de credito"];

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
                imbAceptar.ValidationGroup = "Estados";
                imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
                imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
                imbImprimir = (ImageButton)Master.FindControl("imgBtnImprimir");
                imbImprimir.Click += new ImageClickEventHandler(this.imbImprimir_Click);


                rdbTodos = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbTodos.Text = "Institución";
                rdbClave = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbClave.Text = "Fuente";
                rdbNombre = (RadioButton)Master.FindControl("rdbFiltro3");
                rdbNombre.Text = "Cuenta";
                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
                txbBuscar = (TextBox)Master.FindControl("txtBuscar");
                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "Líneas de Crédito";

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

                if (!IsPostBack)
                {
                    Session["accion"] = 1;
                    CargarFormulario(false);
                    Limpia();
                    Deshabilita();
                    pnlFormulario.Visible = false;
                    pnlCatalogo.Visible = false;
                    //pnlReportes.Visible = false;
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
                Limpia();
                Deshabilita();
                pnlFormulario.Visible = false;
                pnlCatalogo.Visible = false;
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }

        protected void imbNuevo_Click(object sender, EventArgs e)
        {
            CargarFormulario(false);
            Session["accion"] = 1;
            lblAviso.Text = "";
            lblAviso2.Text="";
            Habilita();
            Limpia();
            //0175 GT
            ConfigurarMenuBotones(true, false, false, false, true, true, false, false);
        }

        protected void imbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedValue != null)
            {
                CargarFormulario(true);
                //0175 GT
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);
            }
            else
            {
                CargarCatalogo();
                Buscar("");
                //0175 GT
                ConfigurarMenuBotones(true, true, true, false, false, false, true, true);
            }
            Session["accion"] = 2;
            lblAviso.Text = "";
            lblAviso2.Text = "";
            lblAviso3.Text = "";
        }

        protected void imbEliminar_Click(object sender, EventArgs e)
        {
            if (pnlCatalogo.Visible && dgvDatos.SelectedIndex != -1)
            {
                Eliminar();
                MostrarLista();
                CargarCatalogo();
            }
            else
            {
                CargarCatalogo();
                MostrarLista();
            }
        }


        private void Eliminar()
        {
            
            //Crear el objeto con los datos de la linea de credito a eliminar
            oLineaCredito = new MedDAL.DAL.lineas_creditos();
            oLineaCredito.idLineaCredito=int.Parse(dgvDatos.SelectedDataKey.Value.ToString());

            //Recuperar los valores la linea de credito antes de eliminar para enviar a la bitacora
            string sDatosBitcora;
                      
            sDatosBitcora = "Institución: " + dgvDatos.SelectedRow.Cells[2].Text.ToString() + " ";
            sDatosBitcora += "Fuente: " + dgvDatos.SelectedRow.Cells[3].Text.ToString() + " ";
            sDatosBitcora += "Cuenta: " + dgvDatos.SelectedRow.Cells[4].Text.ToString() + " ";
            sDatosBitcora += "Monto: " + dgvDatos.SelectedRow.Cells[5].Text + " ";
            sDatosBitcora += "Ministración: " + dgvDatos.SelectedRow.Cells[6].Text.ToString() + " ";
            sDatosBitcora += "Vence: " + dgvDatos.SelectedRow.Cells[7].Text.ToString() + " ";

            if(oblLineaCredito.EliminarRegistro(oLineaCredito))
            {
                lblAviso.Text = "La línea de crédito se ha eliminado con éxito";
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Líneas de Crédito";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Eliminación Línea Crédito";
                oBitacora.Descripcion = sDatosBitcora;

                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "La línea de crédito no pudo ser eliminado, es posible que tenga datos relacionados";
            }
        }


        protected void imbMostrar_Click(object sender, EventArgs e)
        {
            //Buscar("");
            MostrarLista();
            dgvDatos.SelectedIndex = -1;
            Session["accion"] = 0;
            lblAviso.Text = "";
            //0175 GT
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);

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
                    Nuevo();
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    break;
                case 2:
                    Editar();
                    Deshabilita();
                    Session["accion"] = 0;
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    break;
            }

        }

        private void Editar()
        {
           
            oLineaCredito = new MedDAL.DAL.lineas_creditos();
            oLineaCredito.idLineaCredito = int.Parse(dgvDatos.SelectedDataKey.Value.ToString());
            oLineaCredito.InstitucionEmisora = txbInstitucion.Text;
            oLineaCredito.FuenteCuenta = txbFuente.Text;
            oLineaCredito.NumeroCuenta = txbCuenta.Text;
            oLineaCredito.Monto = Convert.ToDecimal(QuitarFormatoMoneda(txbMonto.Text));
            oLineaCredito.FechaMinistracion = Convert.ToDateTime(txbFecha.Text);
            oLineaCredito.FechaVencimiento = Convert.ToDateTime(txbVence.Text);
            if (chkActivo.Checked == true)
                oLineaCredito.Activo = true;
            else
                oLineaCredito.Activo = false;

            if (oblLineaCredito.EditarRegistro(oLineaCredito))
            {
                lblAviso.Text = "La línea de crédito ha sido actualizada con éxito";
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Líneas Créditos";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Actualización de Línea Crédito";
                oBitacora.Descripcion = "Institución: " + txbInstitucion.Text + ", Fuente: " + txbFuente.Text+", Cuenta: "+txbCuenta.Text+", Monto: "+txbMonto.Text+", Fecha Mins:"+ txbFecha.Text+ ",Fecha Venc: "+txbVence.Text+", Activo: "+chkActivo.Checked;
                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Text = "La línea de crédito no pudo ser actualizado";
            }
        }

        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
            Limpia();
            Deshabilita();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar(txbBuscar.Text);
            dgvDatos.SelectedIndex = -1;
            Session["accion"] = 0;
            lblAviso.Text = "";
            //0175 GT
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
        }

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            //dgvDatos.SelectedRow
            //dgvDatos.SelectedRowStyle.BackColor = System.Drawing.Color.Yellow;
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

        public void CargarListaReportes()
        {
            ListBox lsbReportes = (ListBox)frReportes.FindControl("lsbSeleccionf");
            frReportes.LimpiarPaneles();
            lsbReportes.SelectedIndex = -1;
            lsbReportes.Items.Clear();
            if (Server.MapPath("~\\rptReportes\\LineasCredito\\rptAcumuladoLineaCredito.rpt") != "")
            {
                lsbReportes.Items.Add("Reporte de acumulado de lineas de crédito");
            }
        }

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;
            pnlFiltroReportes.Visible = true;

            CargarListaReportes();
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
        }

        protected void CargarReporte()
        {
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;
            pnlFiltroReportes.Visible = false;
            //pnlReportes.Visible = true;

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from lineas_creditos", "medicuriConnectionString", odsDataSet, "lineas_creditos");

            //GT 0179
            Session["campoaordenar"] = "";
            Session["sortfield"] = 0;  

            Session["dataset"] = odsDataSet;
            Session["reportdocument"] = "~\\rptReportes\\rptLineasCredito.rpt";
            Session["titulo"] = "Lineas de Crédito";
            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));

            if (dgvDatos.SelectedIndex != -1)
            {
                Session["recordselection"] = "{lineas_creditos.idLineaCredito}=" + dgvDatos.SelectedDataKey.Values[0].ToString();
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

        //protected void ObtenerReporte()
        //{
        //    ReportDocument rptReporte = new ReportDocument();
        //    rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
        //    rptReporte.SetDataSource((DataSet)Session["dataset"]);
        //    //crvReporte.Visible = true;
        //    //crvReporte.ReportSource = rptReporte;
        //    //crvReporte.PageZoomFactor = 100;
        //}
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

        #region PagingSorting

        protected void dgvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.DAL.lineas_creditos>)Session["resultadoquery"];
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
            var result = (IQueryable<MedDAL.DAL.lineas_creditos>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvDatos.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Clave" : ViewState["sortexpression"].ToString(), dv, ref dgvDatos, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            dgvDatos.DataBind();
        }

        #endregion
    }
}