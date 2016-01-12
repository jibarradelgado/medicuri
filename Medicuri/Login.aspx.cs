using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.IO;

namespace Medicuri
{
    public partial class Login : System.Web.UI.Page
    {
        protected Hashtable permisos;
        MedNeg.LogIn.BlLogIn lLogin;
        string sRutaArchivoConfig;
        MedDAL.Configuracion.DALConfiguracion cConfiguracion;
        MedNeg.Configuracion.BlConfiguracion oblConfiguracion;

        protected void Page_Load(object sender, EventArgs e){
            lLogin = new MedNeg.LogIn.BlLogIn();
            txbUsername.Focus();
            CargarCSS();            
        }
                
        /// <summary>
        /// Agrega el hmtl link al header del catalogo, del css correspondiente a cargar
        /// </summary>
        protected void CargarCSS()
        {
            string cColor = null;
            HtmlLink link = new HtmlLink();

            sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            if (File.Exists(sRutaArchivoConfig))
            {
                oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
                cConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);
                try
                {
                    if (!cConfiguracion.sColorInterfaz.Equals(""))
                        cColor = cConfiguracion.sColorInterfaz;
                    else
                        cColor = "Gris";
                }
                catch
                {
                    cColor = "Gris";
                }
            }
            else
                cColor = "Gris";

            link.Href = "Css/" + cColor + ".css";
            link.Attributes.Add("rel", "stylesheet");
            link.Attributes.Add("type", "text/css");
            Page.Header.Controls.Add(link);
        }

        /// <summary>
        /// añade los datos del usuario a las variables de sesion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnIniciar_Click(object sender, EventArgs e)
        {
            #region nuevo loginDB
            if(lLogin.ValidarUsuario(txbUsername.Text, txbPassword.Text))
            {
                FormsAuthentication.RedirectFromLoginPage(txbUsername.Text, false);
                Session.Add("usuarioid",lLogin.IdDelUsuario().ToString());
                Session.Add("usuario", txbUsername.Text);
                Session.Add("nombre", lLogin.NombreDelUsuario());
                Session.Add("permisos", lLogin.CargarPermisos());
                Session["alertabitacora"] = false;   
            }
            else
                lblError.Text = "Usuario o contraseña NO validos.";
            #endregion
        }

        /// <summary>
        /// limpia los campos de usuario y contraseña
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdlimpiar_Click(object sender, EventArgs e)
        {
            txbUsername.Text = "";
            txbPassword.Text = "";
            lblError.Text = "";
        }

        
    }
}