using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Medicuri
{
    public partial class CambiarContraseña : System.Web.UI.Page
    {
        ImageButton imbEditar, imbAceptar, imbCancelar;
        //RadioButton rdbNombre, rdbClave, rdbTodos;
        Label lblNombreModulo;
        //Button btnBuscar;
        //TextBox txbBuscar;

        MedNeg.Usuarios.BlUsuarios blUsuario;
        MedDAL.Usuarios.DALUsuarios odalUsuario;

        protected void Page_Load(object sender, EventArgs e)
        {

            #region Interfaz
            //Hashtable htbPermisos = (Hashtable)Session["permisos"];
            //char cPermiso = (char)htbPermisos["estados"];
            Master.FindControl("btnNuevo").Visible = false;
            Master.FindControl("btnEliminar").Visible = false;
            Master.FindControl("btnMostrar").Visible = false;
            Master.FindControl("btnReportes").Visible = false;
            Master.FindControl("rdbFiltro1").Visible = false;
            Master.FindControl("rdbFiltro2").Visible = false;
            Master.FindControl("rdbFiltro3").Visible = false;
            Master.FindControl("btnBuscar").Visible = false;
            Master.FindControl("txtBuscar").Visible = false;
            Master.FindControl("lblBuscar").Visible = false;

            //imbNuevo = (ImageButton)Master.FindControl("imgBtnNuevo");
            //imbNuevo.Click += new ImageClickEventHandler(this.imbNuevo_Click);
            imbEditar = (ImageButton)Master.FindControl("imgBtnEditar");
            imbEditar.Click += new ImageClickEventHandler(this.imbEditar_Click);

            //imbEliminar = (ImageButton)Master.FindControl("imgBtnEliminar");
            //imbEliminar.Click += new ImageClickEventHandler(this.imbEliminar_Click);
            //imbMostrar = (ImageButton)Master.FindControl("imgBtnMostrar");
            //imbMostrar.Click += new ImageClickEventHandler(this.imbMostrar_Click);
            //imbReportes = (ImageButton)Master.FindControl("imgBtnReportes");
            //imbReportes.Visible = false;
            imbAceptar = (ImageButton)Master.FindControl("imgBtnAceptar");
            imbAceptar.Click += new ImageClickEventHandler(this.imbAceptar_Click);
            imbAceptar.ValidationGroup = "Configuracion";
            imbCancelar = (ImageButton)Master.FindControl("imgBtnCancelar");
            imbCancelar.Click += new ImageClickEventHandler(this.imbCancelar_Click);
            //rdbTodos = (RadioButton)Master.FindControl("rdbFiltro1");
            //rdbTodos.Text = "Nombre y Clave";
            //rdbTodos.Checked = true;
            //rdbClave = (RadioButton)Master.FindControl("rdbFiltro2");
            //rdbClave.Text = "Clave";
            //rdbNombre = (RadioButton)Master.FindControl("rdbFiltro3");
            //rdbNombre.Text = "Nombre";
            //lblReportes = (Label)Master.FindControl("lblReportes");
            //lblReportes.Visible = false;
            //btnBuscar = (Button)Master.FindControl("btnBuscar");
            //btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
            //txbBuscar = (TextBox)Master.FindControl("txtBuscar");
            lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
            lblNombreModulo.Text = "Cambiar Contraseña";
            #endregion

            if (!IsPostBack)
            {
                txbUsuario.Text = Session["usuario"].ToString();
                txbUsuario.Enabled = false;
                Habilita();

            }
        }


        protected bool ValidarContraseña()
        {
            MedDAL.DAL.usuarios oUsuario = new MedDAL.DAL.usuarios();
            MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();

            oUsuario = (MedDAL.DAL.usuarios)oblUsuario.Buscar(Session["usuario"].ToString());

            if (oUsuario.Contrasena.Equals(oblUsuario.EncriptarMD5(txbAnterior.Text)))
                return true;
            else
                return false;
        }


        protected bool ValidarControles()
        {
            rfvNueva.Validate();
            rfvAnterior.Validate();
            rfvConfirmar.Validate();
            cpComparacion.Validate();
            return Page.IsValid;
        }

        private void Habilita()
        {
            txbAnterior.Enabled = true;
            txbNueva.Enabled = true;
            txbConfirmacion.Enabled = true;
        }

        protected void Limpia()
        {
            txbAnterior.Text = "";
            txbNueva.Text = "";
            txbConfirmacion.Text = "";
        }

        private void Deshabilita()
        {
            txbAnterior.Enabled = false;
            txbNueva.Enabled = false;
            txbConfirmacion.Enabled = false;
        }


        protected void imbAceptar_Click(object sender, EventArgs e)
        {
            if (ValidarControles())
            {

                if (ValidarContraseña())
                {
                    MedNeg.Usuarios.BlUsuarios oblUsuario = new MedNeg.Usuarios.BlUsuarios();
                    string sContraseña = oblUsuario.EncriptarMD5(txbNueva.Text);

                    //Cambiar la contraseña
                    if (oblUsuario.CambiarContraseña(Session["usuario"].ToString(), sContraseña))
                    {
                        lblAviso.Text = "La contraseña se ha cambiado con exito";
                    }
                    else
                    {
                        lblAviso.Text = "No se pudo cambiar la contraseña, por favor intente de nuevo";
                    }
                }
            }
        }


        protected void imbEditar_Click(object sender, EventArgs e)
        {
            Habilita();
        }

        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            Limpia();
            Deshabilita();
        }

    }
}