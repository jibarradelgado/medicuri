using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using MedNeg.Tipos;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Medicuri
{
    public partial class Tipos : System.Web.UI.Page
    {
        ImageButton imbNuevo, imbEditar, imbEliminar, imbImprimir, imbMostrar, imbAceptar, imbCancelar, imbReportes;
        RadioButton rdbNombre, rdbClave, rdbTodos;
        Label lblReportes, lblNombreModulo;
        Button btnBuscar;
        TextBox txbBuscar;

        //Declaración del objeto de la capa de negocio de la bitacora
        MedNeg.Bitacora.BlBitacora oblBitacora;

        //Declaración del objeto de la capa de Datos de bitacora
        MedDAL.DAL.bitacora oBitacora;

        //Declaración del objeto de la capa de negocio de la tipos
        MedNeg.Tipos.BlTipos oblTipo;

        //Declaración del objeto de la capa de Datos de tipos
        MedDAL.DAL.tipos oTipos;

        protected void CargarCatalogo()
        {
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = true;            
        }

        protected void MostrarLista()
        {
            //var oQuery = oblEstados.Buscar(sCadena, iTipo);

            var oQuery = oblTipo.MostrarLista();
            Session["resultadoquery"] = oQuery;    

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.DAL.tipos>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Nombre ASC";

            try
            {
                dgvDatos.DataSource = dv;
                dgvDatos.DataKeyNames = new string[] { "idTipo" };
                dgvDatos.DataBind();
                CargarCatalogo();
                if (dgvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    dgvDatos.EmptyDataText = "No existen tipos registrados aun";
                }
                else
                {
                    dgvDatos.EmptyDataText = "No existen tipos que coincidan con la búsqueda";
                }
                dgvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected bool ValidarControles()
        {
            rfvTipo.Validate();
            return Page.IsValid;
        }



        protected void CargarFormulario(bool bDatos)
        {
            pnlFormulario.Visible = true;
            pnlCatalogo.Visible = false;            

            if (bDatos)
            {

                txbTipo.Text = dgvDatos.SelectedRow.Cells[2].Text;
                chkAlmacenes.Checked = ((CheckBox)dgvDatos.SelectedRow.Cells[3].FindControl("ctl01")).Checked;
                chkClientes.Checked = ((CheckBox)dgvDatos.SelectedRow.Cells[4].FindControl("ctl02")).Checked;
                chkProductos.Checked=((CheckBox)dgvDatos.SelectedRow.Cells[5].FindControl("ctl03")).Checked;
                chkProveedores.Checked = ((CheckBox)dgvDatos.SelectedRow.Cells[6].FindControl("ctl04")).Checked;
                chkVendedores.Checked = ((CheckBox)dgvDatos.SelectedRow.Cells[7].FindControl("ctl05")).Checked;
                chkRecetas.Checked = ((CheckBox)dgvDatos.SelectedRow.Cells[8].FindControl("ctl06")).Checked;
                chkActivo.Checked = ((CheckBox)dgvDatos.SelectedRow.Cells[9].FindControl("ctl07")).Checked;

                Habilita();
                txbTipo.Enabled = false;
                chkActivo.Enabled = true;

            }
            else
            {

                Limpia();
                Deshabilita();

            }
        }

        private void Deshabilita()
        {
            txbTipo.Enabled = false;
            chkAlmacenes.Enabled = false;
            chkClientes.Enabled = false;
            chkProductos.Enabled = false;
            chkProveedores.Enabled = false;
            chkVendedores.Enabled = false;
            chkRecetas.Enabled = false;
            chkActivo.Enabled = false;

        }

        private void Limpia()
        {
            txbTipo.Text = "";
            chkAlmacenes.Checked = false;
            chkClientes.Checked = false;
            chkProductos.Checked = false;
            chkProveedores.Checked = false;
            chkVendedores.Checked = false;
            chkRecetas.Checked = false;
            chkActivo.Checked = false;
        }

        private void Habilita()
        {
            txbTipo.Enabled = true;
            chkAlmacenes.Enabled = true;
            chkClientes.Enabled = true;
            chkProductos.Enabled = true;
            chkProveedores.Enabled = true;
            chkVendedores.Enabled = true;
            chkRecetas.Enabled = true;
            chkActivo.Checked = true;
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

          
            var oQuery = oblTipo.Buscar(sCadena, iTipo);
            Session["resultadoquery"] = oQuery;

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.DAL.tipos>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Nombre ASC";

            try
            {
                dgvDatos.DataSource = dv;
                dgvDatos.DataKeyNames = new string[] { "idTipo" };
                dgvDatos.DataBind();

                CargarCatalogo();
                if (dgvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    dgvDatos.EmptyDataText = "No existen tipos registrados aun";
                }
                else
                {
                    dgvDatos.EmptyDataText = "No existen tipos que coincidan con la búsqueda";
                }
                dgvDatos.ShowHeader = true;
            }
            //catch (Exception ex)
            catch
            {
                //Response.Write(ex.Message);
                lblAviso2.Text="No existen tipos que coincidan con la búsqueda";
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

        protected void Nuevo()
        {

            if (ValidarControles())
            {

                string sDatosBitcora;

                //Declarar el objeto nuevo linea de credito a registrar
                oTipos = new MedDAL.DAL.tipos();
                
                oTipos.Nombre = txbTipo.Text;
                sDatosBitcora = "Tipo: " + txbTipo.Text + " ";

                if (chkAlmacenes.Checked == true)
                {
                    oTipos.Almacenes = true;
                    sDatosBitcora += "Almacenes: Sí";
                }
                else
                {
                    oTipos.Almacenes = false;
                    sDatosBitcora += "Almacenes: No ";
                }

                if (chkClientes.Checked == true)
                {
                    oTipos.Clientes = true;
                    sDatosBitcora += "Clientes: Sí ";
                }
                else
                {
                    oTipos.Clientes = false;
                    sDatosBitcora += "Clientes: No ";
                }

                if (chkProductos.Checked == true)
                {
                    oTipos.Productos = true;
                    sDatosBitcora += "Productos: Sí ";
                }
                else
                {
                    oTipos.Productos = false;
                    sDatosBitcora += "Productos: No ";
                }

                if (chkProveedores.Checked == true)
                {
                    oTipos.Proveedores = true;
                    sDatosBitcora += "Proveedores: Sí ";
                }
                else
                {
                    oTipos.Proveedores = false;
                    sDatosBitcora += "Proveedores: No ";
                }

                if (chkVendedores.Checked == true)
                {
                    oTipos.Vendedores = true;
                    sDatosBitcora += "Vendedores: Sí ";
                }
                else
                {
                    oTipos.Vendedores = false;
                    sDatosBitcora += "Vendedores: No ";
                }

                if (chkRecetas.Checked == true)
                {
                    oTipos.Recetas = true;
                    sDatosBitcora += "Recetas: Sí ";
                }
                else
                {
                    oTipos.Recetas = false;
                    sDatosBitcora += "Recetas: No ";
                }

                if (chkActivo.Checked == true)
                {
                    oTipos.Activo = true;
                    sDatosBitcora += "Activo: Sí ";
                }
                else
                {
                    oTipos.Activo = false;
                    sDatosBitcora += "Activo: No ";
                }


                //Intentar insertar el registro en la base de datos
                if (oblTipo.NuevoRegistro(oTipos))
                {
                        lblAviso.Text = "El tipo se ha registrado con éxito";
                        oBitacora = new MedDAL.DAL.bitacora();
                        oBitacora.FechaEntradaSrv = DateTime.Now;
                        oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                        oBitacora.Modulo = "Tipos";
                        oBitacora.Usuario = Session["usuario"].ToString();
                        oBitacora.Nombre = Session["nombre"].ToString();
                        oBitacora.Accion = "Nuevo Tipo";
                        oBitacora.Descripcion = sDatosBitcora;

                        if (!oblBitacora.NuevoRegistro(oBitacora))
                        {
                            lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                        }

                        Limpia();
                        Deshabilita();
                        Session["accion"] = 0;
                    }
                    else
                    {
                        lblAviso.Text = "El tipo no pudo ser registrado";
                    }
                
            }

        }

        private void Eliminar()
        {

            //Crear el objeto con los datos del tipo a eliminar
            //oTipos = new MedDAL.DAL.tipos();
            //oTipos.idTipo = int.Parse(dgvDatos.SelectedDataKey.Value.ToString());


            //Recuperar los valores la linea de credito antes de eliminar para enviar a la bitacora
            string sDatosBitcora;

            sDatosBitcora = "Tipo: " + txbTipo.Text + " ";

            if (chkAlmacenes.Checked == true)
            {
                //oTipos.Almacenes = true;
                sDatosBitcora += "Almacenes: Sí";
            }
            else
            {
                //oTipos.Almacenes = false;
                sDatosBitcora += "Almacenes: No ";
            }

            if (chkClientes.Checked == true)
            {
                //oTipos.Clientes = true;
                sDatosBitcora += "Clientes: Sí ";
            }
            else
            {
                //oTipos.Clientes = false;
                sDatosBitcora += "Clientes: No ";
            }

            if (chkProductos.Checked == true)
            {
                //oTipos.Productos = true;
                sDatosBitcora += "Productos: Sí ";
            }
            else
            {
                //oTipos.Productos = false;
                sDatosBitcora += "Productos: No ";
            }

            if (chkProveedores.Checked == true)
            {
                //oTipos.Proveedores = true;
                sDatosBitcora += "Proveedores: Sí ";
            }
            else
            {
               // oTipos.Proveedores = false;
                sDatosBitcora += "Proveedores: No ";
            }

            if (chkVendedores.Checked == true)
            {
                //oTipos.Vendedores = true;
                sDatosBitcora += "Vendedores: Sí ";
            }
            else
            {
                //oTipos.Vendedores = false;
                sDatosBitcora += "Vendedores: No ";
            }

            if (chkRecetas.Checked == true)
            {
                //oTipos.Recetas = true;
                sDatosBitcora += "Recetas: Sí ";
            }
            else
            {
                //oTipos.Recetas = false;
                sDatosBitcora += "Recetas: No ";
            }

            if (chkActivo.Checked == true)
            {
                //oTipos.Recetas = true;
                sDatosBitcora += "Activo: Sí ";
            }
            else
            {
                //oTipos.Recetas = false;
                sDatosBitcora += "Activo: No ";
            }


            if (oblTipo.EliminarRegistro(int.Parse(dgvDatos.SelectedDataKey.Value.ToString())))
            {
                lblAviso.Text = "El tipo se ha eliminado con éxito";
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Tipos";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Eliminación Tipo";
                oBitacora.Descripcion = sDatosBitcora;

                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Visible = true;
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }
            }
            else
            {
                lblAviso.Visible = true;
                lblAviso.Text = "El tipo no pudo ser eliminado, es posible que tenga datos relacionados";
            }
        }

        private void Editar()
        {

            //Crear el objeto a editar
            oTipos = new MedDAL.DAL.tipos();
            oTipos.idTipo = int.Parse(dgvDatos.SelectedDataKey.Value.ToString());
            oTipos.Nombre = txbTipo.Text;

            string sDatosBitcora = "Tipo: " + txbTipo.Text + " ";

            if (chkAlmacenes.Checked == true)
            {
                oTipos.Almacenes = true;
                sDatosBitcora += "Almacenes: Sí";
            }
            else
            {
                oTipos.Almacenes = false;
                sDatosBitcora += "Almacenes: No ";
            }

            if (chkClientes.Checked == true)
            {
                oTipos.Clientes = true;
                sDatosBitcora += "Clientes: Sí ";
            }
            else
            {
                oTipos.Clientes = false;
                sDatosBitcora += "Clientes: No ";
            }

            if (chkProductos.Checked == true)
            {
                oTipos.Productos = true;
                sDatosBitcora += "Productos: Sí ";
            }
            else
            {
                oTipos.Productos = false;
                sDatosBitcora += "Productos: No ";
            }

            if (chkProveedores.Checked == true)
            {
                oTipos.Proveedores = true;
                sDatosBitcora += "Proveedores: Sí ";
            }
            else
            {
                oTipos.Proveedores = false;
                sDatosBitcora += "Proveedores: No ";
            }

            if (chkVendedores.Checked == true)
            {
                oTipos.Vendedores = true;
                sDatosBitcora += "Vendedores: Sí ";
            }
            else
            {
                oTipos.Vendedores = false;
                sDatosBitcora += "Vendedores: No ";
            }

            if (chkRecetas.Checked == true)
            {
                oTipos.Recetas = true;
                sDatosBitcora += "Recetas: Sí ";
            }
            else
            {
                oTipos.Recetas = false;
                sDatosBitcora += "Recetas: No ";
            }

            if (chkActivo.Checked == true)
            {
                oTipos.Activo = true;
                sDatosBitcora += "Activo: Sí ";
            }
            else
            {
                oTipos.Activo = false;
                sDatosBitcora += "Activo: No ";
            }


            if (oblTipo.EditarRegistro(oTipos))
            {
                lblAviso.Text = "El tipo ha sido actualizada con éxito";
                oBitacora = new MedDAL.DAL.bitacora();
                oBitacora.FechaEntradaSrv = DateTime.Now;
                oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                oBitacora.Modulo = "Tipos";
                oBitacora.Usuario = Session["usuario"].ToString();
                oBitacora.Nombre = Session["nombre"].ToString();
                oBitacora.Accion = "Actualización de Tipos";
                oBitacora.Descripcion = sDatosBitcora;

                if (!oblBitacora.NuevoRegistro(oBitacora))
                {
                    lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                }

                Limpia();
            }
            else
            {
                lblAviso.Text = "El tipo no pudo ser actualizado";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            oblBitacora = new MedNeg.Bitacora.BlBitacora();
            oblTipo = new MedNeg.Tipos.BlTipos();
            
            
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';
            try
            {
                #region Interfaz
                cPermiso = (char)htbPermisos["tipos"];
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
                rdbTodos.Text = "Nombre";

                rdbClave = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbClave.Text = "Módulo";
                //rdbNombre = (RadioButton)Master.FindControl("rdbFiltro3");
                //rdbNombre.Text = "Cuenta";
                Master.FindControl("rdbFiltro3").Visible = false;
                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
                txbBuscar = (TextBox)Master.FindControl("txtBuscar");
                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "Tipos";

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
                    rdbTodos.Checked = true;
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
            lblAviso2.Text = "";
            Habilita();
            Limpia();
            chkActivo.Enabled = true;
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
                //0175 GT
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);
            }
            Session["accion"] = 2;
            lblAviso.Text = "";
            lblAviso2.Text = "";
          
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

        protected void imbMostrar_Click(object sender, EventArgs e)
        {
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

        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
            Limpia();
            Deshabilita();

        }

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
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
        protected void CargarReporte()
        {
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;            

            Session["reporteactivo"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from tipos", "medicuriConnectionString", odsDataSet, "tipos");

            Session["campoaordenar"] = "";
            Session["dataset"] = odsDataSet;
            Session["titulo"] = "Impresión de Tipos";
            //Session["configuracionsistema"] = objConfiguracion;
            Session["sortfield"] = 0;
            Session["reportdocument"] = "~\\rptReportes\\rptTipos.rpt";

            if (dgvDatos.SelectedIndex != -1)
            {
                Session["recordselection"] = "{tipos.idTipo}=" + dgvDatos.SelectedDataKey.Values[0].ToString();
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

        #region PagingSorting

        protected void dgvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.DAL.tipos>)Session["resultadoquery"];
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
            var result = (IQueryable<MedDAL.DAL.tipos>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvDatos.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Nombre" : ViewState["sortexpression"].ToString(), dv, ref dgvDatos, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            dgvDatos.DataBind();
        }

        #endregion

    }
}