using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using MedNeg.Usuarios;//Referencia a la capa de negocio de Usuarios 
using System.Collections;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Medicuri
{
    public partial class Usuarios : System.Web.UI.Page
    {

        ImageButton imbNuevo, imbEditar, imbEliminar, imbImprimir, imbMostrar, imbAceptar, imbCancelar,imbReportes;
        RadioButton rdbNombre, rdbClave, rdbTodos;
        Label lblReportes,lblNombreModulo;
        Button btnBuscar;
        TextBox txbBuscar;
        IQueryable<MedDAL.DAL.perfiles> iqrPerfiles;
        IQueryable<MedDAL.DAL.almacenes> iqrAlmacenes;

        //Declaración del objeto de la capa de negocio de la bitacora
        MedNeg.Bitacora.BlBitacora oblBitacora;

        //Declaración del objeto de la capa de Datos de bitacora
        MedDAL.DAL.bitacora oBitacora;

        //Declaración del objeto de la capa de negocio de usuarios
        MedNeg.Usuarios.BlUsuarios oblUsuario;

        //Declaración del objeto de la capa de datos de usuarios
        MedDAL.DAL.usuarios oUsuario;

        //Declaración del objeto de la capa de negocio de pefiles
        MedNeg.Perfiles.BlPerfiles oblPerfil;

        //Declaración del objeto de la capa de negocio de almacenes
        MedNeg.Almacenes.BlAlmacenes oblAlmacen;



        /// <summary>
        /// Carga el texto que debe de aparecer en los labels de campos editables
        /// </summary>
        protected void CargarCamposEditables()
        {
            MedNeg.CamposEditables.BlCamposEditables oblCamposEditables = new MedNeg.CamposEditables.BlCamposEditables();
            List<MedDAL.DAL.campos_editables> lstCamposEditables = oblCamposEditables.Buscar("Usuarios");
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

        /// <summary>
        /// Actualiza la variable de sesion "lstPerfiles", la cual es una lista de los perfiles activos
        /// </summary>
        protected void ActualizarSesionPerfiles()
        {
            iqrPerfiles = oblPerfil.BuscarEnum();
            Session["lstPerfiles"] = iqrPerfiles;
        }

        /// <summary>
        /// Actualiza la variable de sesion "lstAlmacenes", la cual es una lista de los almacenes activos
        /// </summary>
        protected void ActualizarSesionAlmacenes()
        {
            iqrAlmacenes = oblAlmacen.BuscarAlmacenesActivos();
            Session["lstAlmacenes"] = iqrAlmacenes;
        }

        protected void CargarCatalogo()
        {
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = true;
        }

        protected void MostrarLista()
        {
            //var oQuery = oblEstados.Buscar(sCadena, iTipo);
            oblUsuario = new MedNeg.Usuarios.BlUsuarios();
            oUsuario = new MedDAL.DAL.usuarios();

            oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());


            if (oUsuario.FiltradoActivado == true)
            {
                var oQuery = oblUsuario.MostrarListaAlmacenFiltrada(oUsuario.idAlmacen);
                Session["resultadoquery"] = oQuery;
            }
            else
            {
                var oQuery = oblUsuario.MostrarLista();                
                Session["resultadoquery"] = oQuery;                
            }

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Usuarios.UsuarioView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Usuario ASC";            

            try
            {
                dgvDatos.DataSource = dv;
                dgvDatos.DataKeyNames = new string[] { "idUsuario" };
                dgvDatos.DataBind();
                CargarCatalogo();
                if (dgvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    dgvDatos.EmptyDataText = "No existen usuarios registrados aun";
                }
                else
                {
                    dgvDatos.EmptyDataText = "No existen usuarios que coincidan con la búsqueda";
                }
                dgvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }


        protected bool ValidarCorreo()
        {
            MedNeg.RegEx.BlRegEx oblRegexValidador = new MedNeg.RegEx.BlRegEx();

            return oblRegexValidador.ValidarCorreo(txbCorreo.Text);

        }


        protected bool ValidarUsuario()
        {

            if (oblUsuario.ValidarUsuarioRepetido(txbUsuario.Text) >= 1)
                return false;
            else
                return true;
        }

        protected bool ValidarControles()
        {
            rfvUsurio.Validate();
            rfvContrasena.Validate();
            rfvAlmacen.Validate();
            rfvCorreoElectronico.Validate();
            revCorreo.Validate();

            return Page.IsValid;
        }

        private void LlenaPermisosPerfil(int iIdPerfil)
        {
          
            MedDAL.DAL.permisos_perfiles oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            MedNeg.PermisosPerfiles.BlPermisosPerfiles oblPermisosPerfil = new MedNeg.PermisosPerfiles.BlPermisosPerfiles();
           

            //Usuarios
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 1);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblUsuarios);

            //Perfiles
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 2);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblPerfiles);

            //Clientes
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 3);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblClientes);

            //Vendedores
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 4);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblVendedores);

            //Proveedores
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 5);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblProveedores);

            //Estados
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 6);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblEstados);

            //Municipios
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 7);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblMunicipios);

            //Poblaciones
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 8);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblPoblaciones);

            //Colonias
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 9);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblColonias);

            //Almacenes
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 10);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblAlmacenes);

            //Productos
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 11);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblProductos);

            //Inventarios
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 12);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblInventarios);

            //Pedidos
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 13);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblPedidos);

            //Recetas
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 14);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblRecetas);

            //Remisiones
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 15);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblRemisiones);

            //Facturas
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 16);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblFacturas);

            //Biblioteca
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 17);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblCauses);

            //Bitacora
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 18);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblBitacora);

            //Configuracion
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 19);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblConfiguracion);

            //Campos Editables
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 20);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblCamposEditables);

            //Tipos
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 21);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblTipos);

            //Cuentas x cobrar
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 22);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblCuentasxCobrar);

            //Iva
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 23);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblIva);

            //Ensambles
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 24);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblEnsambles);

            //Lineas de crédito
            oPermisoPerfil = (MedDAL.DAL.permisos_perfiles)oblPermisosPerfil.RecuperarPermiso(iIdPerfil, 25);
            SeleccionarPermiso(oPermisoPerfil.TipoAcceso, rblLineasCreditos);
        }

        protected void CargaPermisos(int iIdUsuario)
        {
          
            MedDAL.DAL.permisos_usuarios oPermisosUsuario = new MedDAL.DAL.permisos_usuarios();
            MedNeg.PermisosUsuarios.BlPermisosUsuarios oblPermisosUsuario = new MedNeg.PermisosUsuarios.BlPermisosUsuarios();

            //Usuarios
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 1);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblUsuarios);

            //Perfiles
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 2);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblPerfiles);

            //Clientes
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 3);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblClientes);

            //Vendedores
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 4);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblVendedores);

            //Proveedores
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 5);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblProveedores);

            //Estados
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 6);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblEstados);

            //Municipios
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 7);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblMunicipios);
            
            //Poblaciones
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 8);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblPoblaciones);
           
            //Colonias
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 9);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblColonias);

            //Almacenes
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 10);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblAlmacenes);
            
            //Productos
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 11);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblProductos);

            //Inventarios
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 12);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblInventarios);

            //Pedidos
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 13);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblPedidos);

            //Recetas
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 14);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblRecetas);

            //Remisiones
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 15);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblRemisiones);

            //Facturas
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 16);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblFacturas);

            //Biblioteca
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 17);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblCauses);

            //Bitacora
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 18);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblBitacora);

            //Configuracion
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 19);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblConfiguracion);

            //Campos Editables
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 20);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso,rblCamposEditables);

            //Tipos
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 21);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso,rblTipos);

            //Cuentas x cobrar
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 22);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblCuentasxCobrar);

            //Iva
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 23);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblIva);

            //Ensambles
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 24);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso, rblEnsambles);

            //Lineas de crédito
            oPermisosUsuario = (MedDAL.DAL.permisos_usuarios)oblPermisosUsuario.RecuperarPermisos(iIdUsuario, 25);
            SeleccionarPermiso(oPermisosUsuario.TipoAcceso,rblLineasCreditos);

        }


        /// <summary>
        /// Funcion que selecciona la opción del permiso de acceso en el permiso correspondiente
        /// </summary>
        /// <param name="sTipoAcceso">Tipo de acceso del permiso</param>
        /// <param name="rblPermiso">radioButtomList a setear</param>
        private void SeleccionarPermiso(string sTipoAcceso, RadioButtonList rblPermiso)
        {
            switch (sTipoAcceso)
            {
                case "N":
                    rblPermiso.Items[0].Selected = true;
                    rblPermiso.Items[1].Selected = false;
                    rblPermiso.Items[2].Selected = false;
                    rblPermiso.Items[3].Selected = false;
                    break;

                case "L":
                    rblPermiso.Items[1].Selected = true;
                    rblPermiso.Items[0].Selected = false;
                    rblPermiso.Items[2].Selected = false;
                    rblPermiso.Items[3].Selected = false;
                    break;

                case "E":
                    rblPermiso.Items[2].Selected = true;
                    rblPermiso.Items[1].Selected = false;
                    rblPermiso.Items[0].Selected = false;
                    rblPermiso.Items[3].Selected = false;
                    break;


                case "T":
                    rblPermiso.Items[3].Selected = true;
                    rblPermiso.Items[1].Selected = false;
                    rblPermiso.Items[2].Selected = false;
                    rblPermiso.Items[0].Selected = false;
                    break;
            }
        }


        protected void CargarFormulario(bool bDatos)
        {
            pnlFormulario.Visible = true;
            pnlCatalogo.Visible = false;


            //Recuperar los perfiles y llenar el combo
            ActualizarSesionPerfiles();
            cmbPerfiles.Items.Clear();
            cmbPerfiles.DataSource = Session["lstPerfiles"];
            cmbPerfiles.DataBind();

            //Recuperar los almacenes y llenar el combo
            ActualizarSesionAlmacenes();
            cmbAlmacenes.Items.Clear();
            cmbAlmacenes.DataSource = Session["lstAlmacenes"];
            cmbAlmacenes.DataBind();

            if (bDatos)
            {

                //Objeto que contiene el id del usuario registrado
                oUsuario = new MedDAL.DAL.usuarios();
                oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(dgvDatos.SelectedRow.Cells[2].Text);

                txbUsuario.Text = dgvDatos.SelectedRow.Cells[2].Text;
                txbNombre.Text = dgvDatos.SelectedRow.Cells[4].Text;
                txbApellidos.Text = dgvDatos.SelectedRow.Cells[5].Text;
                txbCorreo.Text = dgvDatos.SelectedRow.Cells[6].Text;
                Session["sContraseñaUsuario"] = oUsuario.Contrasena.ToString();
                txbCampo1.Text = oUsuario.Campo1.ToString();
                txbCampo2.Text = oUsuario.Campo2.ToString();
                txbCampo3.Text = oUsuario.Campo3.ToString();
                txbCampo4.Text = oUsuario.Campo4.ToString();
                txbCampo5.Text = oUsuario.Campo5.ToString();
                txbCampo6.Text = oUsuario.Campo6.ToString();
                txbCampo7.Text = oUsuario.Campo7.ToString();
                txbCampo8.Text = oUsuario.Campo8.ToString();
                txbCampo9.Text = oUsuario.Campo9.ToString();
                txbCampo10.Text = oUsuario.Campo10.ToString();
                chkActivo.Checked = ((CheckBox)dgvDatos.SelectedRow.Cells[7].FindControl("ctl01")).Checked;
                if (oUsuario.FiltradoActivado == true)
                    chkFiltrado.Checked = true;
                else
                    chkFiltrado.Checked = false;

                int iContador = 0;
                foreach (ListItem elemento in cmbAlmacenes.Items)
                {

                    if (elemento.Value.Equals(oUsuario.idAlmacen.ToString()))
                    {
                       elemento.Selected=true;
                    }

                    iContador++;

                }

                iContador = 0;
                foreach (ListItem elemento in cmbPerfiles.Items)
                {

                   
                    if (elemento.Value.Equals(oUsuario.idPerfil.ToString()))
                    {
                        elemento.Selected = true;
                    }

                    iContador++;

                }

                CargaPermisos(oUsuario.idUsuario);
                Habilita();
                txbUsuario.Enabled = false;
                
            }
            else
            {

              //Cargar los permisos del perfil que aparece seleccionado por default
                LlenaPermisosPerfil((Convert.ToInt32(cmbPerfiles.SelectedValue)));

            }
        }

        protected bool RegistraPermiso(MedDAL.DAL.permisos_usuarios oPermisoRegistrar)
        {

            MedNeg.PermisosUsuarios.BlPermisosUsuarios oblPermisosUsuario = new MedNeg.PermisosUsuarios.BlPermisosUsuarios();
            //insertar el permiso
            return oblPermisosUsuario.AgregarPermisos(oPermisoRegistrar);
        }


        protected bool GuardaPermisos()
        {
                     
            //oblUsuario = new MedNeg.Usuarios.BlUsuarios();
         
            //Objeto que contiene el id del usuario registrado
            oUsuario = new MedDAL.DAL.usuarios();
            oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(txbUsuario.Text);
           
            //Objeto que contiene el id del permiso a registrar
            MedDAL.DAL.permisos oPermiso = new MedDAL.DAL.permisos();
            MedNeg.Permisos.BlPermisos oblPermisos= new MedNeg.Permisos.BlPermisos();

            //Objeto que contiene permisousuario a registrar
            MedDAL.DAL.permisos_usuarios oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
            

            bool bRegistroFallido = false;

            #region usuarios
            
                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Usuarios");

                //Crear el permiso a insertar
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblUsuarios.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;
            
            #endregion

            #region perfiles
                
                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Perfiles");
                
                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblPerfiles.SelectedValue.ToString();
           


                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

            #endregion

            #region clientes

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Clientes");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblClientes.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

            #endregion

            #region Estados

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Estados");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblEstados.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

            #endregion

             #region Municipios

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Municipios");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblMunicipios.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

             #endregion

                #region Lineas de credito

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("LineasCreditos");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblLineasCreditos.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Iva

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Iva");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblIva.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Vendedores

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Vendedores");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblVendedores.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Poblaciones

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Poblaciones");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblPoblaciones.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Proveedores

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Proveedores");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblProveedores.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Colonias

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Colonias");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblColonias.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Almacenes

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Almacenes");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblAlmacenes.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Productos

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Productos");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblProductos.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Inventarios

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Inventarios");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblInventarios.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Facturas

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Facturas");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblFacturas.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Recetas

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Recetas");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblRecetas.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Remisiones

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Remisiones");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblRemisiones.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region CuentasxCobrar

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("CuentasxCobrar");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblCuentasxCobrar.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Pedidos

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Pedidos");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblPedidos.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Causes

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Biblioteca");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblCauses.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Bitacora

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Bitacora");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblBitacora.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Configuracion

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Configuracion");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblConfiguracion.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Campos Editables

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("CamposEditables");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblCamposEditables.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Tipos

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Tipos");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblTipos.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

                #region Ensambles

                //Recuperar permiso a insertar
                oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Ensambles");

                //Crear el permiso a insertar
                oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
                oPermisoUsuario.idUsuario = oUsuario.idUsuario;
                oPermisoUsuario.idPermiso = oPermiso.idPermiso;
                oPermisoUsuario.TipoAcceso = rblEnsambles.SelectedValue.ToString();

                if (!RegistraPermiso(oPermisoUsuario))
                    bRegistroFallido = true;

                #endregion

            if (bRegistroFallido)
            {
                return false;
            }
            else
            {
                return true;
            }
           
            
            
        }


        private void Buscar(string sCadena)
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

            var oQuery = oblUsuario.Buscar(sCadena, iTipo);
            Session["resultadoquery"] = oQuery; 

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Usuarios.UsuarioView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Usuario ASC";

            try
            {
                dgvDatos.DataSource = dv;
                dgvDatos.DataKeyNames = new string[] { "idUsuario" };
                dgvDatos.DataBind();

                CargarCatalogo();
                if (dgvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    dgvDatos.EmptyDataText = "No existen usuarios registrados aun";
                }
                else
                {
                    dgvDatos.EmptyDataText = "No existen usuarios que coincidan con la búsqueda";
                }
                dgvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void CargaPerfiles()
        {
            ActualizarSesionPerfiles();
            cmbPerfiles.Items.Clear();
            cmbPerfiles.DataSource = (IQueryable<MedDAL.DAL.perfiles>)Session["lstPerfiles"];
            cmbPerfiles.DataBind();
            
            if (cmbPerfiles.Items.Count != 0)
                cmbPerfiles.SelectedIndex = 0;
            
        }

        private void CargaAlmacenes()
        {
            ActualizarSesionAlmacenes();
            cmbAlmacenes.Items.Clear();
            cmbAlmacenes.DataSource = (IQueryable<MedDAL.DAL.almacenes>)Session["lstAlmacenes"];
            cmbAlmacenes.DataBind();

            if (cmbAlmacenes.Items.Count != 0)
                cmbAlmacenes.SelectedIndex = 0;

        }

        protected void Buscar(string sCadena, int iIdEstado)
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

            var oQuery = oblUsuario.Buscar(sCadena, iTipo);
            Session["resultadoquery"] = oQuery; 

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.Usuarios.UsuarioView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Usuario ASC";

            try
            {
                dgvDatos.DataSource = dv;
                dgvDatos.DataKeyNames = new string[] { "idUsuario" };
                dgvDatos.DataBind();
                dgvDatos.Visible = true;


                if (dgvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    dgvDatos.EmptyDataText = "No existen usuarios registrados aun";
                }
                else
                {
                    dgvDatos.EmptyDataText = "No existen usuarios que coincidan con la búsqueda";
                }
                dgvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private bool EditarPermisos(int iIdUsuario)
        {

            MedNeg.PermisosUsuarios.BlPermisosUsuarios oblPermisosUsuario = new MedNeg.PermisosUsuarios.BlPermisosUsuarios();
          
            bool bRegistroFallido = false;

            #region usuarios
            if (!oblPermisosUsuario.EditarPermiso(1, iIdUsuario, rblUsuarios.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region perfiles
            if (!oblPermisosUsuario.EditarPermiso(2, iIdUsuario, rblPerfiles.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region clientes
            if (!oblPermisosUsuario.EditarPermiso(3, iIdUsuario, rblClientes.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Vendedores
            if (!oblPermisosUsuario.EditarPermiso(4, iIdUsuario, rblVendedores.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Proveedores
            if (!oblPermisosUsuario.EditarPermiso(5, iIdUsuario, rblProveedores.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Estados
            if (!oblPermisosUsuario.EditarPermiso(6, iIdUsuario, rblEstados.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Municipios
            if (!oblPermisosUsuario.EditarPermiso(7, iIdUsuario, rblMunicipios.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Poblaciones
            if (!oblPermisosUsuario.EditarPermiso(8, iIdUsuario, rblPoblaciones.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Colonias
            if (!oblPermisosUsuario.EditarPermiso(9, iIdUsuario, rblColonias.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Almacenes
            if (!oblPermisosUsuario.EditarPermiso(10, iIdUsuario, rblAlmacenes.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Productos
            if (!oblPermisosUsuario.EditarPermiso(11, iIdUsuario, rblProductos.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Inventarios
            if (!oblPermisosUsuario.EditarPermiso(12, iIdUsuario, rblInventarios.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Pedidos
            if (!oblPermisosUsuario.EditarPermiso(13, iIdUsuario, rblPedidos.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Recetas
            if (!oblPermisosUsuario.EditarPermiso(14, iIdUsuario, rblRecetas.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Remisiones
            if (!oblPermisosUsuario.EditarPermiso(15, iIdUsuario, rblRemisiones.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Facturas
            if (!oblPermisosUsuario.EditarPermiso(16, iIdUsuario, rblFacturas.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Causes
            if (!oblPermisosUsuario.EditarPermiso(17, iIdUsuario, rblCauses.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Bitacora
            if (!oblPermisosUsuario.EditarPermiso(18, iIdUsuario, rblBitacora.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Configuracion
            if (!oblPermisosUsuario.EditarPermiso(19, iIdUsuario, rblConfiguracion.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Campos Editables
            if (!oblPermisosUsuario.EditarPermiso(20, iIdUsuario, rblConfiguracion.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Tipos
            if (!oblPermisosUsuario.EditarPermiso(21, iIdUsuario, rblTipos.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region CuentasxCobrar
            if (!oblPermisosUsuario.EditarPermiso(22, iIdUsuario, rblCuentasxCobrar.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Iva
            if (!oblPermisosUsuario.EditarPermiso(23, iIdUsuario, rblIva.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Ensambles
            if (!oblPermisosUsuario.EditarPermiso(24, iIdUsuario, rblEnsambles.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Lineas de credito
            if (!oblPermisosUsuario.EditarPermiso(25, iIdUsuario, rblLineasCreditos.SelectedValue))
                bRegistroFallido = true;
            #endregion

            if (bRegistroFallido)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        protected bool Nuevo()
        {

           if (!ValidarControles())
                return false;

           if (!ValidarUsuario())
           {
               lblAviso.Text = "El usuario ya existe favor de ingresar uno diferente";
               return false;
           }
            
            oUsuario = new MedDAL.DAL.usuarios();
            oUsuario.Usuario = txbUsuario.Text;
            oUsuario.Contrasena = oblUsuario.EncriptarMD5(txbContraseña.Text);
            oUsuario.Nombre = txbNombre.Text;
            oUsuario.Apellidos = txbApellidos.Text;
            oUsuario.CorreoElectronico = txbCorreo.Text;
            oUsuario.Campo1 = txbCampo1.Text;
            oUsuario.Campo2 = txbCampo2.Text;
            oUsuario.Campo3 = txbCampo3.Text;
            oUsuario.Campo4 = txbCampo4.Text;
            oUsuario.Campo5 = txbCampo5.Text;

            if (txbCampo6.Text.Equals(""))
                oUsuario.Campo6 = 0;
            else
                oUsuario.Campo6 = Convert.ToInt32(txbCampo6.Text);

            if (txbCampo7.Text.Equals(""))
                oUsuario.Campo7 = 0;
            else
                oUsuario.Campo7 = Convert.ToInt32(txbCampo7.Text);

            if (txbCampo8.Text.Equals(""))
                oUsuario.Campo8 = 0;
            else
                oUsuario.Campo8 = Convert.ToInt32(txbCampo8.Text);

            if (txbCampo9.Text.Equals(""))
                oUsuario.Campo9 = 0;
            else
                oUsuario.Campo9 = Convert.ToDecimal(txbCampo9.Text);


            if (txbCampo10.Text.Equals(""))
                oUsuario.Campo10 = 0;
            else
                oUsuario.Campo10 = Convert.ToDecimal(txbCampo10.Text);

            if (chkActivo.Checked == true)
                oUsuario.Activo = true;
            else
                oUsuario.Activo = false;

            if (chkFiltrado.Checked == true)
                oUsuario.FiltradoActivado = true;
            else
                oUsuario.FiltradoActivado = false;
           
            oUsuario.idPerfil = Convert.ToInt32(cmbPerfiles.SelectedValue);
            oUsuario.idAlmacen = Convert.ToInt32(cmbAlmacenes.SelectedValue);

            //Si el registro del municipio es exitoso, registrar en la bitácora.            
            if(oblUsuario.NuevoRegistro(oUsuario))
            {

                if (GuardaPermisos())
                {
                    lblAviso.Text = "El usuario se ha registrado con éxito";
                    oBitacora = new MedDAL.DAL.bitacora();
                    oBitacora.FechaEntradaSrv = DateTime.Now;
                    oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                    oBitacora.Modulo = "Usuarios";
                    oBitacora.Usuario = Session["usuario"].ToString();
                    oBitacora.Nombre = Session["nombre"].ToString();
                    oBitacora.Accion = "Nuevo Usuario";
                    oBitacora.Descripcion = "Usuario: " + txbUsuario.Text + ", Nombre: " + txbNombre.Text + ", Correo: " + txbCorreo.Text + ", Almacen: " + cmbAlmacenes.SelectedValue;
                    if (!oblBitacora.NuevoRegistro(oBitacora))
                    {
                        lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                    }
                    return true;
                }
                else
                {
                    lblAviso.Text = "El usuario no pudo ser registrado";
                   
                    return false;
                }

            }
            else
            {
                lblAviso.Text = "El usuario no pudo ser registrado";
                return false;
                     
            }
        }



        private void Eliminar(int iIdUsuario)
        {

            //Eliminar primero los permisos
            MedNeg.PermisosUsuarios.BlPermisosUsuarios oblPermisos = new MedNeg.PermisosUsuarios.BlPermisosUsuarios();

            if (oblPermisos.EliminarPermisos(iIdUsuario))
            {
                string sDatosBitacora;
                oUsuario = new MedDAL.DAL.usuarios();
                oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(dgvDatos.SelectedRow.Cells[2].Text);

                sDatosBitacora = "Usuario: " + oUsuario.Usuario.ToString() + " ";
                sDatosBitacora += "Nombre: " + oUsuario.Nombre.ToString() + " " + oUsuario.Apellidos.ToString() + " ";
                sDatosBitacora += "Correo: " + oUsuario.CorreoElectronico.ToString() + " ";
                sDatosBitacora += "Almacen: " + oUsuario.almacenes.Nombre.ToString();

                if (oblUsuario.EliminarRegistro((int)oUsuario.idUsuario))
                {
                    //lblAviso.Text = "El usuario se ha eliminado con éxito";
                    oBitacora = new MedDAL.DAL.bitacora();
                    oBitacora.FechaEntradaSrv = DateTime.Now;
                    oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                    oBitacora.Modulo = "Usuarios";
                    oBitacora.Usuario = Session["usuario"].ToString();
                    oBitacora.Nombre = Session["nombre"].ToString();
                    oBitacora.Accion = "Eliminación de Usuario";
                    oBitacora.Descripcion = sDatosBitacora;

                    if (!oblBitacora.NuevoRegistro(oBitacora))
                    {
                        lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                    }
                }
                else
                {
                    lblAviso.Text = "El usuario no pudo ser eliminado, es posible que tenga datos relacionados";
                }

            }
            else
            {
                lblAviso.Text = "El usuario no pudo ser eliminado, es posible que tenga datos relacionados";
            }
        }


        private void Deshabilita()
        {
            txbUsuario.Enabled = false;
            txbContraseña.Enabled = false;
            cmbPerfiles.Enabled = false;
            cmbAlmacenes.Enabled = false;
            txbNombre.Enabled = false;
            txbApellidos.Enabled = false;
            txbCorreo.Enabled = false;
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
            chkActivo.Enabled = false;

            rblUsuarios.Enabled = false;
            rblPerfiles.Enabled = false;
            rblClientes.Enabled = false;
            rblEstados.Enabled = false;
            rblMunicipios.Enabled = false;
            rblLineasCreditos.Enabled = false;
            rblIva.Enabled = false;
            rblVendedores.Enabled = false;
            rblPoblaciones.Enabled = false;
            rblProveedores.Enabled = false;
            rblColonias.Enabled = false;
            rblAlmacenes.Enabled = false;
            rblProductos.Enabled = false;
            rblInventarios.Enabled = false;
            rblFacturas.Enabled = true;
            rblRecetas.Enabled = false;
            rblRemisiones.Enabled = false;
            rblCuentasxCobrar.Enabled = false;
            rblPedidos.Enabled = false;
            rblCauses.Enabled = false;
            rblBitacora.Enabled = false;
            rblConfiguracion.Enabled = false;
            rblCamposEditables.Enabled = false;
            rblTipos.Enabled = false;
            rblEnsambles.Enabled = false;



        }

        private void Habilita()
        {
            txbUsuario.Enabled = true;
            txbContraseña.Enabled = true;
            cmbPerfiles.Enabled = true;
            cmbAlmacenes.Enabled = true;
            txbNombre.Enabled = true;
            txbApellidos.Enabled = true;
            txbCorreo.Enabled = true;
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
            chkActivo.Enabled = true;

            rblUsuarios.Enabled = true;
            rblPerfiles.Enabled = true;
            rblClientes.Enabled = true;
            rblEstados.Enabled = true;
            rblMunicipios.Enabled = true;
            rblLineasCreditos.Enabled = true;
            rblIva.Enabled = true;
            rblVendedores.Enabled = true;
            rblPoblaciones.Enabled = true;
            rblProveedores.Enabled = true;
            rblColonias.Enabled = true;
            rblAlmacenes.Enabled = true;
            rblProductos.Enabled = true;
            rblInventarios.Enabled = true;
            rblFacturas.Enabled = true;
            rblRecetas.Enabled = true;
            rblRemisiones.Enabled = true;
            rblCuentasxCobrar.Enabled = true;
            rblPedidos.Enabled = true;
            rblCauses.Enabled = true;
            rblBitacora.Enabled = true;
            rblConfiguracion.Enabled = true;
            rblCamposEditables.Enabled = true;
            rblTipos.Enabled = true;
            rblEnsambles.Enabled = true;
            txbUsuario.Focus();

        }

        private void Limpia()
        {
            txbUsuario.Text = "";
            txbContraseña.Text = "";
            
            if(cmbPerfiles.SelectedIndex>0)
                cmbPerfiles.SelectedIndex = 0;
            
            if(cmbAlmacenes.SelectedIndex>0)
                cmbAlmacenes.SelectedIndex = 0;

            txbNombre.Text = "";
            txbApellidos.Text = "";
            txbCorreo.Text = "";
            txbCampo1.Text = "";
            txbCampo2.Text = "";
            txbCampo3.Text = "";
            txbCampo4.Text = "";
            txbCampo5.Text = "";
            txbCampo6.Text = "";
            txbCampo7.Text = "";
            txbCampo8.Text = "";
            txbCampo9.Text = "";
            txbCampo10.Text = "";
            chkActivo.Checked = false;

            rblUsuarios.ClearSelection();
            rblPerfiles.ClearSelection();
            rblClientes.ClearSelection();
            rblEstados.ClearSelection();
            rblMunicipios.ClearSelection(); 
            rblLineasCreditos.ClearSelection(); 
            rblIva.ClearSelection(); 
            rblVendedores.ClearSelection(); 
            rblPoblaciones.ClearSelection(); 
            rblProveedores.ClearSelection(); 
            rblColonias.ClearSelection();
            rblAlmacenes.ClearSelection();
            rblProductos.ClearSelection();
            rblInventarios.ClearSelection();
            rblFacturas.ClearSelection();
            rblRecetas.ClearSelection(); 
            rblRemisiones.ClearSelection();
            rblCuentasxCobrar.ClearSelection();
            rblPedidos.ClearSelection();
            rblCauses.ClearSelection();
            rblBitacora.ClearSelection(); 
            rblConfiguracion.ClearSelection(); 
            rblCamposEditables.ClearSelection(); 
            rblTipos.ClearSelection();
            rblEnsambles.ClearSelection(); 

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            //Obtener los controles de master.
            
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';
            try
            {
                #region Interfaz
                cPermiso = (char)htbPermisos["usuarios"];
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
                imbAceptar.ValidationGroup = "Municipios";
                imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
                imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
                rdbTodos = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbTodos.Text = "Usuario";
                rdbClave = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbClave.Text = "Nombre";
                rdbNombre = (RadioButton)Master.FindControl("rdbFiltro3");
                rdbNombre.Text = "Correo";
                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
                txbBuscar = (TextBox)Master.FindControl("txtBuscar");
                //Master.FindControl("btnReportes").Visible = false;
                imbImprimir = (ImageButton)Master.FindControl("imgBtnImprimir");
                imbImprimir.Click += new ImageClickEventHandler(this.imbImprimir_Click);

                //GT 0175
                imbReportes = (ImageButton)Master.FindControl("imgBtnReportes");
                imbReportes.Click += new ImageClickEventHandler(this.imbReportes_Click);

                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "Usuarios";

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
                    Deshabilita();
                    CargarCamposEditables();
                    pnlFormulario.Visible = false;
                    pnlCatalogo.Visible = false;
                    //GT 0179
                    //pnlReportes.Visible = false;
                    Session["resultadoquery"] = "";
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;

                    Session["reporteactivoUsuarios"] = 0;
                    Session["reportdocument"] = "";
                    Session["titulo"] = "";
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                }
                oblAlmacen = new MedNeg.Almacenes.BlAlmacenes();
                oblBitacora = new MedNeg.Bitacora.BlBitacora();
                oblPerfil = new MedNeg.Perfiles.BlPerfiles();
                oblUsuario = new MedNeg.Usuarios.BlUsuarios();
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

           
        private void DesactivarNuevo()
        {
            Master.FindControl("btnNuevo").Visible = false;
        }

        private void DesactivarEdicionEliminacion()
        {
            Master.FindControl("btnEditar").Visible = false;
            Master.FindControl("btnEliminar").Visible = false;
        }



        protected void imbNuevo_Click(object sender, EventArgs e)
        {
          
            CargarFormulario(false);
            Habilita();
            Limpia();
            LlenaPermisosPerfil((Convert.ToInt32(cmbPerfiles.SelectedValue)));
            dgvDatos.SelectedIndex = -1;
            Session["accion"] = 1;
            lblAviso.Text = "";
            lblAviso2.Text = "";
            //0175 GT
            ConfigurarMenuBotones(true, false, false, false, true, true,false,false);
        }

        protected void imbEditar_Click(object sender, EventArgs e)
        {
            if (dgvDatos.SelectedIndex != -1)
            {
                CargarFormulario(true);
                Session["accion"] = 2;
                //0175 GT
                ConfigurarMenuBotones(false, false, true, true, true, true, false, false);
            }
            else
            {                
                CargarCatalogo();
                //0175 GT
                ConfigurarMenuBotones(true, true, true, false, false, false, true, true);
                       
            }

            
        }

        protected void imbEliminar_Click(object sender, EventArgs e)
        {
            if (pnlCatalogo.Visible && dgvDatos.SelectedIndex != -1)
            {
                Eliminar((int)dgvDatos.SelectedValue);
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
            //0175 GT
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);
            MostrarLista();
            dgvDatos.SelectedIndex = -1;
            Session["accion"] = 0;
            lblAviso.Text = "";
           
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
                    if (Nuevo())
                    {
                        CargarFormulario(false);

                        //Limpiar los campos despues de registrado
                        Limpia();
                        Deshabilita();
                    }
                    else
                    {
                        Habilita();
                    }
                    Session["accion"] = 1;
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    break;
                case 2:
                    Editar();
                    CargarFormulario(true);
                    Session["accion"] = 2;
                    Limpia();
                    Deshabilita();
                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    break;
            }
        }

        protected void imbImprimir_Click(object sender, EventArgs e)
        {
           //GT0175
           ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
           CargarReporte();
        }

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
        }

        private void Editar()
        {
            oUsuario = new MedDAL.DAL.usuarios();
            oUsuario.idUsuario = int.Parse(dgvDatos.SelectedDataKey.Value.ToString());
            oUsuario.Usuario = txbUsuario.Text;
            
            //Validar la contraseña, si el txbcontraseña == "" entonces se guarda la de sesion
            if (txbContraseña.Text == "")
                oUsuario.Contrasena = Session["sContraseñaUsuario"].ToString();

            //Si txbContraeseña != "" entonces se guarda lo del txbContraseña
            if(txbContraseña.Text!="")
                oUsuario.Contrasena = oblUsuario.EncriptarMD5(txbContraseña.Text);

            
            oUsuario.Nombre = txbNombre.Text;
            oUsuario.Apellidos = txbApellidos.Text;
            oUsuario.CorreoElectronico = txbCorreo.Text;
            oUsuario.Campo1 = txbCampo1.Text;
            oUsuario.Campo2 = txbCampo2.Text;
            oUsuario.Campo3 = txbCampo3.Text;
            oUsuario.Campo4 = txbCampo4.Text;
            oUsuario.Campo5 = txbCampo5.Text;

            if (txbCampo6.Text.Equals(""))
                oUsuario.Campo6 = 0;
            else
                oUsuario.Campo6 = Convert.ToInt32(txbCampo6.Text);

            if (txbCampo7.Text.Equals(""))
                oUsuario.Campo7 = 0;
            else
                oUsuario.Campo7 = Convert.ToInt32(txbCampo7.Text);

            if (txbCampo8.Text.Equals(""))
                oUsuario.Campo8 = 0;
            else
                oUsuario.Campo8 = Convert.ToInt32(txbCampo8.Text);

            if (txbCampo9.Text.Equals(""))
                oUsuario.Campo9 = 0;
            else
                oUsuario.Campo9 = Convert.ToDecimal(txbCampo9.Text);


            if (txbCampo10.Text.Equals(""))
                oUsuario.Campo10 = 0;
            else
                oUsuario.Campo10 = Convert.ToDecimal(txbCampo10.Text);

            if (chkActivo.Checked == true)
                oUsuario.Activo = true;
            else
                oUsuario.Activo = false;

            if (chkFiltrado.Checked == true)
                oUsuario.FiltradoActivado = true;
            else
                oUsuario.FiltradoActivado = false;

            oUsuario.idPerfil = Convert.ToInt32(cmbPerfiles.SelectedValue);
            oUsuario.idAlmacen = Convert.ToInt32(cmbAlmacenes.SelectedValue);
           

            if (oblUsuario.EditarRegistro(oUsuario))
            {
                if (EditarPermisos(oUsuario.idUsuario))
                {
                    lblAviso.Text = "El usuario se ha modificado con éxito";
                    oBitacora = new MedDAL.DAL.bitacora();
                    oBitacora.FechaEntradaSrv = DateTime.Now;
                    oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                    oBitacora.Modulo = "Usuarios";
                    oBitacora.Usuario = Session["usuario"].ToString();
                    oBitacora.Nombre = Session["nombre"].ToString();
                    oBitacora.Accion = "Edición Usuario";
                    oBitacora.Descripcion = "Usuario: " + txbUsuario.Text + ", Nombre: " + txbNombre.Text + ", Correo: " + txbCorreo.Text + ", Almacen: " + cmbAlmacenes.SelectedValue;
                    if (!oblBitacora.NuevoRegistro(oBitacora))
                    {
                        lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                    }
                }
                else
                {
                    lblAviso.Text = "El usuario no pudo ser modificado, por favor intente de nuevo";
                   
                }
            }
            else
            {
                lblAviso.Text = "El usuario no pudo ser modificado, por favor intente de nuevo";
            }
            
        }

       

        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            Limpia();
            Deshabilita();
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar(txbBuscar.Text);
            ConfigurarMenuBotones(true, true, true, true, false, true, true, true);            
            dgvDatos.SelectedIndex = -1;
            Session["accion"] = 0;
            lblAviso.Text = "";
        }

       

        protected void dgvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {            
            //dgvDatos.SelectedRowStyle.BackColor = System.Drawing.Color.Yellow;
           
        }

        protected void cmbPerfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            LlenaPermisosPerfil((Convert.ToInt32(cmbPerfiles.SelectedValue)));
        }

        protected void cmbPerfiles_TextChanged(object sender, EventArgs e)
        {
            
        }

        #region Estado de los botones

        private void ConfigurarMenuBotones(bool bNuevo, bool bMostrar, bool bEliminar, bool bEditar, bool bAceptar, bool bCancelar,bool bReportes,bool bImprmir)
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

        protected DataSet LlenarDataSet(string sConsulta, string sNombreConnectionString, DataSet dsDataSet, string sTabla)
        {
            SqlConnection sqlConnection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[sNombreConnectionString].ConnectionString);
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            sqlAdapter.SelectCommand = new SqlCommand(sConsulta, sqlConnection);
            sqlAdapter.Fill(dsDataSet, sTabla);
        
            return dsDataSet;
        }

        protected void CargarReporte()
        {
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;
            //pnlReportes.Visible = true;

            Session["reporteactivoUsuarios"] = 1;

            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from usuarios", "medicuriConnectionString", odsDataSet, "usuarios");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from almacenes", "medicuriConnectionString", odsDataSet, "almacenes");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from perfiles", "medicuriConnectionString", odsDataSet, "perfiles");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from permisos", "medicuriConnectionString", odsDataSet, "permisos");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from permisos_usuarios", "medicuriConnectionString", odsDataSet, "permisos_usuarios");

            //GT 0179
            Session["campoaordenar"] = "";
            Session["sortfield"] = 0;  

            Session["dataset"] = odsDataSet;
            Session["reportdocument"] = "~\\rptReportes\\rptUsuarios.rpt";
            Session["titulo"] = "Usuarios";

            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));

            //GT 0179
            //Saber si es general o se tiene un usuario seleccionado
            if (dgvDatos.SelectedIndex != -1)
            {
                Session["recordselection"] = "{usuarios.idUsuario}=" + dgvDatos.SelectedDataKey.Values[0].ToString();
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

        

        //protected void ObtenerReporte()
        //{
        //    ReportDocument rptReporte = new ReportDocument();
        //    rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));
        //    rptReporte.SetDataSource((DataSet)Session["dataset"]);
        //    crvReporte.Visible = true;
        //    crvReporte.ReportSource = rptReporte;
        //    crvReporte.PageZoomFactor = 100;
        //}

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

        //protected void crvReporte_Drill(object source, CrystalDecisions.Web.DrillEventArgs e)
        //{
        //    ObtenerReporte();
        //}

        //protected void crvReporte_Navigate(object source, CrystalDecisions.Web.NavigateEventArgs e)
        //{
        //    ObtenerReporte();
        //    crvReporte.PageZoomFactor = 50;
        //}

        //protected void crvReporte_Search(object source, CrystalDecisions.Web.SearchEventArgs e)
        //{
        //    ObtenerReporte();
        //    crvReporte.PageZoomFactor = 50;
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
        //    if (int.Parse(Session["reporteactivoUsuarios"].ToString()) == 1)
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
        //    if (int.Parse(Session["reporteactivoUsuarios"].ToString()) == 1)
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
        //    if (int.Parse(Session["reporteactivoUsuarios"].ToString()) == 1)
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

        #region SortingPaging

        protected void dgvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Site1 oMaster = (Site1)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.Usuarios.UsuarioView>)Session["resultadoquery"];
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
            var result = (IQueryable<MedDAL.Usuarios.UsuarioView>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvDatos.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Usuario" : ViewState["sortexpression"].ToString(), dv, ref dgvDatos, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            dgvDatos.DataBind();
        }

        #endregion


    }


}
