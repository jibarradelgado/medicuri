using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.IO;
using System.Web.UI.HtmlControls;

namespace Medicuri
{
    public partial class Recuperar_Contraseña : System.Web.UI.Page
    {
        string from;// <- source direction
        string fromPwd;//<- password of source direction
        string to = ""; //if more than 1 direction, separate it by ','
        string subject = "Recuperación de contraseña";
        string smtpClient;
        string sRutaArchivoConfig;
        int port;
        bool confExists;
        MedDAL.Configuracion.DALConfiguracion cConfiguracion;
        MedNeg.Configuracion.BlConfiguracion oblConfiguracion;
        MedNeg.RecuperarContraseña.BlRecuperarContraseña oblRecuperarPass;


        protected void Page_Load(object sender, EventArgs e)
        {
            CargarCSS();
            oblRecuperarPass = new MedNeg.RecuperarContraseña.BlRecuperarContraseña();
            sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            if (File.Exists(sRutaArchivoConfig))
            {
                oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
                cConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

                from = cConfiguracion.sCorreoEmisor;
                fromPwd = cConfiguracion.sContraseñaSmtp;
                to = this.txbNombreUsuario.Text;
                smtpClient = cConfiguracion.sServidorSmtp;
                if(!cConfiguracion.sPuertoSmtp.Equals(""))
                    port = Convert.ToInt32(cConfiguracion.sPuertoSmtp);
                else
                    port = 25;
                confExists = true;
            }
            else
                confExists = false;
        }

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

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            if (confExists)
            {
                if (!from.Equals("") && !fromPwd.Equals("") && !smtpClient.Equals(""))
                    this.lblInfo.Text = oblRecuperarPass.SendMail(from, fromPwd, to, subject, smtpClient, port);
                else
                    this.lblInfo.Text = "El archivo de configuración NO contiene datos validos para el servidor de correo. Contacte a su administrador";
            }
            else
                this.lblInfo.Text = "Hubo un problema con el archivo de configuración. Contacte a su administrador";
        }

        
    }
}