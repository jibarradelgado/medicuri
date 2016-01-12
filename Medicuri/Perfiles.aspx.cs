using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using MedNeg.Perfiles;//Referencia a la capa de negocio de perfiles
using System.Collections;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Medicuri
{
    public partial class Perfiles : System.Web.UI.Page
    {


        ImageButton imbNuevo, imbEditar, imbEliminar, imbImprimir, imbMostrar, imbAceptar, imbCancelar, imbReportes;
        RadioButton rdbFiltro3, rdbDescripcion, rdbNombre;
        Label lblReportes, lblNombreModulo;
        Button btnBuscar;
        TextBox txbBuscar;
        

        //Declaración del objeto de la capa de negocio de la bitacora
        MedNeg.Bitacora.BlBitacora oblBitacora;

        //Declaración del objeto de la capa de Datos de bitacora
        MedDAL.DAL.bitacora oBitacora;

        //Declaración del objeto de la capa de negocio de pefiles
        MedNeg.Perfiles.BlPerfiles oblPerfil;

        //Declaración del objeto de la capa de datos de perfiles
        MedDAL.DAL.perfiles oPerfil;

        protected void CargarCatalogo()
        {
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = true;
            ////pnlReportes.Visible = false;
        }


        protected void MostrarLista()
        {
            //var oQuery = oblEstados.Buscar(sCadena, iTipo);

            var oQuery = oblPerfil.MostrarLista();
            Session["resultadoquery"] = oQuery;

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.DAL.perfiles>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Nombre ASC";

            try
            {
                dgvDatos.DataSource = dv;
                dgvDatos.DataKeyNames = new string[] { "idPerfil" };
                dgvDatos.DataBind();
                CargarCatalogo();
                if (dgvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    dgvDatos.EmptyDataText = "No existen perfiles registrados aun";
                }
                else
                {
                    dgvDatos.EmptyDataText = "No existen perfiles que coincidan con la búsqueda";
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
            rfvPerfil.Validate();
            return Page.IsValid;
        }

        protected bool RegistraPermiso(MedDAL.DAL.permisos_perfiles oPermisoRegistrar)
        {

            MedNeg.PermisosPerfiles.BlPermisosPerfiles oblPermisosPerfil = new MedNeg.PermisosPerfiles.BlPermisosPerfiles();
            //insertar el permiso
            return oblPermisosPerfil.AgregarPermisos(oPermisoRegistrar);
        }

        private bool GuardaPermisos()
        {
            //Objeto que contiene el id del perfil registrado
            //oUsuario = new MedDAL.DAL.usuarios();
            //oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(txbUsuario.Text);
            oPerfil = new MedDAL.DAL.perfiles();
            oPerfil = (MedDAL.DAL.perfiles)oblPerfil.Buscar(txbPerfil.Text);

            //Objeto que contiene el id del permiso a registrar
            MedDAL.DAL.permisos oPermiso = new MedDAL.DAL.permisos();
            MedNeg.Permisos.BlPermisos oblPermisos = new MedNeg.Permisos.BlPermisos();

            //Objeto que contiene permisoperfil a registrar
            //MedDAL.DAL.permisos_usuarios oPermisoUsuario = new MedDAL.DAL.permisos_usuarios();
            MedDAL.DAL.permisos_perfiles oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();

            bool bRegistroFallido = false;

            #region usuarios

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Usuarios");
            
            //Crear el permiso a insertar
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblUsuarios.SelectedValue.ToString();
           
            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;
            #endregion


            #region perfiles

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Perfiles");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblPerfiles.SelectedValue.ToString();



            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region clientes

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Clientes");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblClientes.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Estados

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Estados");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblEstados.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Municipios

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Municipios");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblMunicipios.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Lineas de credito

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("LineasCreditos");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblLineasCreditos.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Iva

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Iva");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblIva.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Vendedores

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Vendedores");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblVendedores.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Poblaciones

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Poblaciones");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblPoblaciones.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Proveedores

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Proveedores");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblProveedores.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Colonias

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Colonias");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblColonias.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Almacenes

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Almacenes");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblAlmacenes.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Productos

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Productos");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblProductos.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Inventarios

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Inventarios");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblInventarios.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Facturas

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Facturas");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblFacturas.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Recetas

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Recetas");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblRecetas.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Remisiones

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Remisiones");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblRemisiones.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region CuentasxCobrar

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("CuentasxCobrar");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblCuentasxCobrar.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Pedidos

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Pedidos");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblPedidos.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Causes

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Biblioteca");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblCauses.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Bitacora

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Bitacora");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblBitacora.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Configuracion

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Configuracion");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblConfiguracion.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Campos Editables

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("CamposEditables");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblCamposEditables.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Tipos

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Tipos");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblTipos.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
                bRegistroFallido = true;

            #endregion

            #region Ensambles

            //Recuperar permiso a insertar
            oPermiso = (MedDAL.DAL.permisos)oblPermisos.Buscar("Ensambles");

            //Crear el permiso a insertar
            oPermisoPerfil = new MedDAL.DAL.permisos_perfiles();
            oPermisoPerfil.idPerfil = oPerfil.idPerfil;
            oPermisoPerfil.idPermiso = oPermiso.idPermiso;
            oPermisoPerfil.TipoAcceso = rblEnsambles.SelectedValue.ToString();

            if (!RegistraPermiso(oPermisoPerfil))
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


        protected void CargarFormulario(bool bDatos)
        {
            pnlFormulario.Visible = true;
            pnlCatalogo.Visible = false;
            
            if (bDatos)
            {

                //Objeto que contiene el id del perfil registrado y se usa para recuperar sus permisos
                oPerfil = new MedDAL.DAL.perfiles();
                oPerfil = (MedDAL.DAL.perfiles)oblPerfil.Buscar(dgvDatos.SelectedRow.Cells[2].Text);

                txbPerfil.Text = dgvDatos.SelectedRow.Cells[2].Text;
                txbDescripcion.Text = dgvDatos.SelectedRow.Cells[3].Text;
                chkActivo.Checked = ((CheckBox)dgvDatos.SelectedRow.Cells[4].FindControl("ctl01")).Checked;

                LlenaPermisosPerfil(oPerfil.idPerfil);
                Habilita();
                txbPerfil.Enabled = false;
               
            }
            else
            {

                Limpia();
                Deshabilita();

            }
        }


        protected bool Nuevo()
        {

            if (!ValidarControles())
                return false;


            //oUsuario = new MedDAL.DAL.usuarios();
            oPerfil = new MedDAL.DAL.perfiles();

            oPerfil.Nombre = txbPerfil.Text;
            oPerfil.Descrpcion = txbDescripcion.Text;
            if (chkActivo.Enabled == true)
                oPerfil.Activo = true;
            else
                oPerfil.Activo = false;
           

            //Si el registro del perfil es exitoso, registrar en la bitácora.            
            if (oblPerfil.NuevoRegistro(oPerfil))
            {

                if (GuardaPermisos())
                {
                    lblAviso.Text = "El Perfil se ha registrado con éxito";
                    oBitacora = new MedDAL.DAL.bitacora();
                    oBitacora.FechaEntradaSrv = DateTime.Now;
                    oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                    oBitacora.Modulo = "Perfiles";
                    oBitacora.Usuario = Session["usuario"].ToString();
                    oBitacora.Nombre = Session["nombre"].ToString();
                    oBitacora.Accion = "Nuevo Perfil";
                    oBitacora.Descripcion = "Perfil: " + txbPerfil.Text + ", Descripción: " + txbDescripcion.Text;
                    if (!oblBitacora.NuevoRegistro(oBitacora))
                    {
                        lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                    }
                    return true;
                }
                else
                {
                    lblAviso.Text = "El perfil no pudo ser registrado";
                   
                    return false;
                }

            }
            else
            {
                lblAviso.Text = "El perfil no pudo ser registrado";
                return false;

            }
        }

        private bool EditarPermisos(int iIdPerfil)
        {


            MedNeg.PermisosPerfiles.BlPermisosPerfiles oblPermisosPerfil = new MedNeg.PermisosPerfiles.BlPermisosPerfiles();

            bool bRegistroFallido = false;

            #region usuarios
            if (!oblPermisosPerfil.EditarPermiso(1, iIdPerfil, rblUsuarios.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region perfiles
            if (!oblPermisosPerfil.EditarPermiso(2, iIdPerfil, rblPerfiles.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region clientes
            if (!oblPermisosPerfil.EditarPermiso(3, iIdPerfil, rblClientes.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Vendedores
            if (!oblPermisosPerfil.EditarPermiso(4, iIdPerfil, rblVendedores.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Proveedores
            if (!oblPermisosPerfil.EditarPermiso(5, iIdPerfil, rblProveedores.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Estados
            if (!oblPermisosPerfil.EditarPermiso(6, iIdPerfil, rblEstados.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Municipios
            if (!oblPermisosPerfil.EditarPermiso(7, iIdPerfil, rblMunicipios.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Poblaciones
            if (!oblPermisosPerfil.EditarPermiso(8, iIdPerfil, rblPoblaciones.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Colonias
            if (!oblPermisosPerfil.EditarPermiso(9, iIdPerfil, rblColonias.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Almacenes
            if (!oblPermisosPerfil.EditarPermiso(10, iIdPerfil, rblAlmacenes.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Productos
            if (!oblPermisosPerfil.EditarPermiso(11, iIdPerfil, rblProductos.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Inventarios
            if (!oblPermisosPerfil.EditarPermiso(12, iIdPerfil, rblInventarios.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Pedidos
            if (!oblPermisosPerfil.EditarPermiso(13, iIdPerfil, rblPedidos.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Recetas
            if (!oblPermisosPerfil.EditarPermiso(14, iIdPerfil, rblRecetas.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Remisiones
            if (!oblPermisosPerfil.EditarPermiso(15, iIdPerfil, rblRemisiones.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Facturas
            if (!oblPermisosPerfil.EditarPermiso(16, iIdPerfil, rblFacturas.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Causes
            if (!oblPermisosPerfil.EditarPermiso(17, iIdPerfil, rblCauses.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Bitacora
            if (!oblPermisosPerfil.EditarPermiso(18, iIdPerfil, rblBitacora.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Configuracion
            if (!oblPermisosPerfil.EditarPermiso(19, iIdPerfil, rblConfiguracion.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Campos Editables
            if (!oblPermisosPerfil.EditarPermiso(20, iIdPerfil, rblConfiguracion.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Tipos
            if (!oblPermisosPerfil.EditarPermiso(21, iIdPerfil, rblTipos.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region CuentasxCobrar
            if (!oblPermisosPerfil.EditarPermiso(22, iIdPerfil, rblCuentasxCobrar.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Iva
            if (!oblPermisosPerfil.EditarPermiso(23, iIdPerfil, rblIva.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Ensambles
            if (!oblPermisosPerfil.EditarPermiso(24, iIdPerfil, rblEnsambles.SelectedValue))
                bRegistroFallido = true;
            #endregion

            #region Lineas de credito
            if (!oblPermisosPerfil.EditarPermiso(25, iIdPerfil, rblLineasCreditos.SelectedValue))
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

        private void Editar()
        {
            oPerfil = new MedDAL.DAL.perfiles();
            oPerfil.idPerfil = int.Parse(dgvDatos.SelectedDataKey.Value.ToString());
            oPerfil.Nombre = txbPerfil.Text;
            oPerfil.Descrpcion = txbDescripcion.Text;
                    
            if (chkActivo.Checked == true)
                oPerfil.Activo = true;
            else
                oPerfil.Activo = false;

            if (oblPerfil.EditarRegistro(oPerfil))
            {
                if (EditarPermisos(oPerfil.idPerfil))
                {
                    lblAviso.Text = "El perfil se ha modificado con éxito";
                    oBitacora = new MedDAL.DAL.bitacora();
                    oBitacora.FechaEntradaSrv = DateTime.Now;
                    oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                    oBitacora.Modulo = "Usuarios";
                    oBitacora.Usuario = Session["usuario"].ToString();
                    oBitacora.Nombre = Session["nombre"].ToString();
                    oBitacora.Accion = "Edición Perfil";
                    oBitacora.Descripcion = "Perfil: "+txbPerfil.Text + "Descripción: " + txbDescripcion.Text;
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

        private void Eliminar(int iIdPerfil)
        {

            //Eliminar primero los permisos
            //MedNeg.PermisosUsuarios.BlPermisosUsuarios oblPermisos = new MedNeg.PermisosUsuarios.BlPermisosUsuarios();
            MedNeg.PermisosPerfiles.BlPermisosPerfiles oblPermisos = new MedNeg.PermisosPerfiles.BlPermisosPerfiles();

            if (oblPermisos.EliminarPermisos(iIdPerfil))
            {
                string sDatosBitacora;
                
                oPerfil = new MedDAL.DAL.perfiles();
                oPerfil = (MedDAL.DAL.perfiles)oblPerfil.Buscar(dgvDatos.SelectedRow.Cells[2].Text);

                sDatosBitacora = "Perfil: " + oPerfil.Nombre.ToString();
                sDatosBitacora += "Descripción: " + oPerfil.Descrpcion.ToString();


                if (oblPerfil.EliminarRegistro((int)oPerfil.idPerfil))
                {
                    //lblAviso.Text = "El usuario se ha eliminado con éxito";
                    oBitacora = new MedDAL.DAL.bitacora();
                    oBitacora.FechaEntradaSrv = DateTime.Now;
                    oBitacora.FechaEntradaCte = DateTime.Now;//Linea Temporal
                    oBitacora.Modulo = "Perfiles";
                    oBitacora.Usuario = Session["usuario"].ToString();
                    oBitacora.Nombre = Session["nombre"].ToString();
                    oBitacora.Accion = "Eliminación de Perfil";
                    oBitacora.Descripcion = sDatosBitacora;

                    if (!oblBitacora.NuevoRegistro(oBitacora))
                    {
                        lblAviso2.Text = "El evento no pudo ser registrado en la bitácora";
                    }
                }
                else
                {
                    lblAviso.Text = "El perfil no pudo ser eliminado, es posible que tenga datos relacionados";
                }

            }
            else
            {
                lblAviso.Text = "El perfil no pudo ser eliminado, es posible que tenga datos relacionados";
            }
        }


        private void Buscar(string sCadena)
        {
            int iTipo = 1;
            if (rdbNombre.Checked)
            {
                iTipo = 1;
            }
            else if (rdbDescripcion.Checked)
            {
                iTipo = 2;
            }
            /*else if (rdbFiltro3.Checked)
            {
                iTipo = 3;
            }*/

            var oQuery = oblPerfil.Buscar(sCadena, iTipo);
            Session["resultadoquery"] = oQuery;

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.DAL.perfiles>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "Nombre ASC";

            try
            {
                dgvDatos.DataSource = dv;
                dgvDatos.DataKeyNames = new string[] { "idPerfil" };
                dgvDatos.DataBind();

                CargarCatalogo();
                if (dgvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    dgvDatos.EmptyDataText = "No existen perfiles registrados aun";
                }
                else
                {
                    dgvDatos.EmptyDataText = "No existen perfiles que coincidan con la búsqueda";
                }
                dgvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        private void Deshabilita()
        {
            txbDescripcion.Enabled = false;
            txbDescripcion.Enabled = false;
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

        private void Limpia()
        {
            txbDescripcion.Text = "";
            txbPerfil.Text = "";
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

        private void Habilita()
        {

            txbDescripcion.Enabled = true;
            txbDescripcion.Enabled = true;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            //Obtener los controles de master.
            
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';

            try
            {
                #region Interfaz
                cPermiso = (char)htbPermisos["perfiles"];
                imbNuevo = (ImageButton)Master.FindControl("imgBtnNuevo");
                imbNuevo.Click += new ImageClickEventHandler(this.imbNuevo_Click);
                imbEditar = (ImageButton)Master.FindControl("imgBtnEditar");
                imbEditar.Click += new ImageClickEventHandler(this.imbEditar_Click);
                imbEliminar = (ImageButton)Master.FindControl("imgBtnEliminar");
                imbEliminar.Click += new ImageClickEventHandler(this.imbEliminar_Click);
                imbMostrar = (ImageButton)Master.FindControl("imgBtnMostrar");
                imbMostrar.Click += new ImageClickEventHandler(this.imbMostrar_Click);
                imbImprimir = (ImageButton)Master.FindControl("imgBtnImprimir");
                imbImprimir.Click += new ImageClickEventHandler(this.imbImprimir_Click);
                imbAceptar = (ImageButton)Master.FindControl("imgBtnAceptar");
                imbAceptar.Click += new ImageClickEventHandler(this.imbAceptar_Click);
                imbAceptar.ValidationGroup = "Municipios";
                imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
                imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
                rdbNombre = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbNombre.Text = "Nombre";
                rdbDescripcion = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbDescripcion.Text = "Descripción";
                rdbFiltro3 = (RadioButton)Master.FindControl("rdbFiltro3");
                rdbFiltro3.Visible = false;
                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
                txbBuscar = (TextBox)Master.FindControl("txtBuscar");
                //Master.FindControl("btnReportes").Visible = false;
                lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
                lblNombreModulo.Text = "Perfiles";

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
                    Deshabilita();
                    pnlFormulario.Visible = false;
                    pnlCatalogo.Visible = false;
                    //pnlReportes.Visible = false;

                    Session["reporteactivoPerfiles"] = 0;
                    Session["reportdocument"] = "";
                    Session["titulo"] = "";
                    Session["resultadoquery"] = "";
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;

                    //GT 0175
                    ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                }

                oblBitacora = new MedNeg.Bitacora.BlBitacora();
                oblPerfil = new MedNeg.Perfiles.BlPerfiles();
            }
            catch (NullReferenceException)
            {
                if (!ClientScript.IsStartupScriptRegistered("alertsession"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alertsession", "alertarSesion();", true);
                }
                Deshabilita();
                pnlCatalogo.Visible = false;
                pnlFormulario.Visible = false;
                Site1 oPrincipal = (Site1)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }    
        }

        private void ActivarPermisos()
        {
            rblUsuarios.Items[0].Selected = true;
            rblPerfiles.Items[0].Selected = true;
            rblClientes.Items[0].Selected = true;
            rblVendedores.Items[0].Selected = true;
            rblProveedores.Items[0].Selected = true;
            rblEstados.Items[0].Selected = true;
            rblMunicipios.Items[0].Selected = true;
            rblPoblaciones.Items[0].Selected = true;
            rblColonias.Items[0].Selected = true;
            rblAlmacenes.Items[0].Selected = true;
            rblProductos.Items[0].Selected = true;
            rblInventarios.Items[0].Selected = true;
            rblPedidos.Items[0].Selected = true;
            rblRecetas.Items[0].Selected = true;
            rblRemisiones.Items[0].Selected = true;
            rblFacturas.Items[0].Selected = true;
            rblCauses.Items[0].Selected = true;
            rblBitacora.Items[0].Selected = true;
            rblConfiguracion.Items[0].Selected = true;
            rblCamposEditables.Items[0].Selected = true;
            rblTipos.Items[0].Selected = true;
            rblCuentasxCobrar.Items[0].Selected = true;
            rblIva.Items[0].Selected = true;
            rblEnsambles.Items[0].Selected = true;
            rblLineasCreditos.Items[0].Selected = true;
        }

        protected void imbNuevo_Click(object sender, EventArgs e)
        {
            CargarFormulario(false);
            Habilita();
            ActivarPermisos();
            txbPerfil.Enabled = true;
            dgvDatos.SelectedIndex = -1;
            Session["accion"] = 1;
            lblAviso.Text = "";
            //pnlReportes.Visible = false;
            //lblAviso2.Text = "";

            //0175 GT
            ConfigurarMenuBotones(true, false, false, false, true, true, false, false);

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
                    if (Nuevo())
                    {
                        CargarFormulario(false);

                        //Limpiar los campos despues de registrado
                        Limpia();
                        Deshabilita();
                        ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
                    }
                    else
                    {
                        Habilita();
                    }
                    Session["accion"] = 1;                   
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

        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            Limpia();
            Deshabilita();
            pnlFormulario.Visible = false;
            pnlCatalogo.Visible = false;
            //GT 0175
            ConfigurarMenuBotones(true, true, false, false, false, false, true, true);
        }

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            //GT0175
            ConfigurarMenuBotones(true, true, false, false, false, true, true, true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            Buscar(txbBuscar.Text);
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
            //pnlReportes.Visible = true;

            Session["reporteactivoPerfiles"] = 1;
            MedDAL.DataSets.dsDataSet odsDataSet = new MedDAL.DataSets.dsDataSet();
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from perfiles", "medicuriConnectionString", odsDataSet, "perfiles");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from permisos_perfiles", "medicuriConnectionString", odsDataSet, "permisos_perfiles");
            odsDataSet = (MedDAL.DataSets.dsDataSet)LlenarDataSet("select * from permisos", "medicuriConnectionString", odsDataSet, "permisos");

            //GT 0179
            Session["campoaordenar"] = "";
            Session["sortfield"] = 0;  

            Session["dataset"] = odsDataSet;
            Session["reportdocument"] = "~\\rptReportes\\rptPerfil.rpt";
            Session["titulo"] = "Perfiles";
            ReportDocument rptReporte = new ReportDocument();
            rptReporte.Load(Server.MapPath(Session["reportdocument"].ToString()));

            if (dgvDatos.SelectedIndex != -1)
            {
                Session["recordselection"] = "{perfiles.idPerfil}=" + dgvDatos.SelectedDataKey.Values[0].ToString();
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
        //    if (int.Parse(Session["reporteactivoPerfiles"].ToString()) == 1)
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
        //    if (int.Parse(Session["reporteactivoPerfiles"].ToString()) == 1)
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
        //    if (int.Parse(Session["reporteactivoPerfiles"].ToString()) == 1)
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
            var result = (IQueryable<MedDAL.DAL.perfiles>)Session["resultadoquery"];
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
            var result = (IQueryable<MedDAL.DAL.perfiles>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dgvDatos.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "Nombre" : ViewState["sortexpression"].ToString(), dv, ref dgvDatos, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            dgvDatos.DataBind();
        }

        #endregion
    }
}