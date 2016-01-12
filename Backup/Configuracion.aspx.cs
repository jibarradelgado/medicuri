using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Web.Security;
using MedNeg;
using System.IO;

namespace Medicuri
{
	public partial class Configuracion : System.Web.UI.Page
	{
        ImageButton imbEditar, imbAceptar, imbCancelar;
        //RadioButton rdbNombre, rdbClave, rdbTodos;
        Label lblNombreModulo;
        //Button btnBuscar;
        //TextBox txbBuscar;
        MedNeg.Configuracion.BlConfiguracion oblConfiguracion;
        MedDAL.Configuracion.DALConfiguracion odalConfiguracion;
        string sRutaArchivoConfig;

      
        
		protected void Page_Load(object sender, EventArgs e)
		{
            sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
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
            Master.FindControl("btnImprimir").Visible = false;

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
            //Master.FindControl("btnReportes").Visible = false;
            //Master.FindControl("btnReportes").Visible = false;
           

            lblNombreModulo = (Label)Master.FindControl("lblNombreModulo");
            lblNombreModulo.Text = "Configuración";
            #endregion

            if (!IsPostBack)
            {
              
                //if (File.Exists("C:\\Bioxor\\Medicuri\\Configuracion.xml"))
                if (File.Exists(sRutaArchivoConfig))
                    CargarDatos();
                else
                {
                    //StreamWriter strWriter = new StreamWriter("C:\\Bioxor\\Medicuri\\Configuracion.xml");
                    StreamWriter strWriter = new StreamWriter(sRutaArchivoConfig);
                    strWriter.Close();
                    odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
                    
                    odalConfiguracion.sRazonSocial = "";
                    odalConfiguracion.sRfc = "";
                    odalConfiguracion.sDomicilio = "";
                    odalConfiguracion.sCodigoPostal = "";
                    odalConfiguracion.sMunicipio = "";
                    odalConfiguracion.sEstado = "";
                    odalConfiguracion.sPais = "";
                    odalConfiguracion.sRutaCertificado = "";
                    odalConfiguracion.sRutaLlave="";
                    odalConfiguracion.sRutaFirma = "";
                    odalConfiguracion.sContraseña = "";
                    odalConfiguracion.sServidorBD = "";
                    odalConfiguracion.sUsuarioBD = "";
                    odalConfiguracion.sContraseñaBD = "";
                    odalConfiguracion.sColorInterfaz = "";
                    odalConfiguracion.sRutaBaner="";
                    odalConfiguracion.sServidorSmtp="";
                    odalConfiguracion.sPuertoSmtp = "";
                    odalConfiguracion.sCorreoEmisor = "";
                    odalConfiguracion.sContraseñaSmtp = "";

                    oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
                    oblConfiguracion.GuardarDatos(odalConfiguracion, sRutaArchivoConfig);
                    rbVerde.Checked = true;

                }
                
                Deshabilita();
                //GT 0175
                ConfigurarMenuBotones(false, false, false, true, false, false, false, false);

            }
           
            
		}

        protected void CargarDatos()
        {
            sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();
            MedDAL.Configuracion.DALConfiguracion objConfiguracion = new MedDAL.Configuracion.DALConfiguracion();
            objConfiguracion = (MedDAL.Configuracion.DALConfiguracion)oblConfiguracion.CargaDatos(sRutaArchivoConfig);

            //Empresa
            txbRazonSocial.Text = objConfiguracion.sRazonSocial.ToString();
            txbRfc.Text = objConfiguracion.sRfc.ToString();
            txbRegimenFiscal.Text = objConfiguracion.sRegimenFiscal == null ? "" : objConfiguracion.sRegimenFiscal.ToString();
            txbDomicilio.Text = objConfiguracion.sDomicilio.ToString();
            txbCodigoPostal.Text = objConfiguracion.sCodigoPostal.ToString();
            txbMunicipio.Text = objConfiguracion.sMunicipio.ToString();
            txbEstado.Text = objConfiguracion.sEstado.ToString();
            txbPais.Text = objConfiguracion.sPais.ToString();
            txbCaducidad.Text = objConfiguracion.iCaducidad == null ? "0" : objConfiguracion.iCaducidad.ToString();

            try
            {
                lbCertificado.Text = objConfiguracion.sRutaCertificado.ToString();
            }
            catch (Exception)
            {
                lbCertificado.Text = "";
            }
            try
            {
                lbLlave.Text = objConfiguracion.sRutaLlave.ToString();
            }
            catch
            {
                lbLlave.Text = "";
            }
            try
            {
                lblFirma.Text = objConfiguracion.sRutaFirma.ToString();
            }
            catch
            {
                lblFirma.Text = "";
            }
            try
            {
                txbContraseñaFac.Attributes.Add("value", objConfiguracion.sContraseña.ToString());
            }
            catch
            {
                txbContraseñaFac.Attributes.Add("value", "");
            }
            //Base de datos
            txbServidor.Text = objConfiguracion.sServidorBD.ToString();
            txbUsuario.Text = objConfiguracion.sUsuarioBD.ToString();
            txbContraseñaBd.Attributes.Add("value", objConfiguracion.sContraseñaBD.ToString());

            //Diseño
            //txbColor.Text = objConfiguracion.sColorInterfaz.ToString();
            switch (objConfiguracion.sColorInterfaz.ToString())
            {
                case "Verde":
                    rbVerde.Checked = true;
                    break;
                case "Verde1":
                    rbVerde1.Checked = true;
                    break;
                case "Azul":
                    rbAzul.Checked = true;
                    break;
                case "Azul1":
                    rbAzul1.Checked = true;
                    break;
                case "Cafe":
                    rbCafe.Checked = true;
                    break;
                case "Rojo":
                    rbRojo.Checked = true;
                    break;
                case "Amarillo":
                    rbAmarillo.Checked = true;
                    break;
                case "Naranja":
                    rbNaranja.Checked = true;
                    break;
                case "Morado":
                    rbMorado.Checked = true;
                    break;
                case "Gris":
                    rbGris.Checked = true;
                    break;
            }
            
            lbLogotipo.Text = objConfiguracion.sRutaBaner.ToString();


            //Folios
            txbFolioPedidos.Text = objConfiguracion.iFolioPedidos.ToString();
            if (objConfiguracion.iPedidosAutomatico == 1)
                ckbPedidos.Checked = true;
            else
                ckbPedidos.Checked = false;

            txbFolioRecetas.Text = objConfiguracion.iFolioRecetas.ToString();
            if (objConfiguracion.iRecetasAutomatico == 1)
                ckbRecetas.Checked = true;
            else
                ckbRecetas.Checked = false;

            txbFolioRemisiones.Text = objConfiguracion.iFolioRemisiones.ToString();
            if (objConfiguracion.iRemisionesAutomatico == 1)
                ckbRemisiones.Checked = true;
            else
                ckbRemisiones.Checked = false;

            txbFolioFacturas.Text = objConfiguracion.iFolioFacturas.ToString();
            if (objConfiguracion.iFacturasAutomatico == 1)
                ckbFacturas.Checked = true;
            else
                ckbFacturas.Checked = false;

            if (objConfiguracion.iVentasNegativas == 1)
                ckbVentasNegativas.Checked = true;
            else
                ckbVentasNegativas.Checked = false;

            //Datos del SMTP
            txbSevidorSmtp.Text = objConfiguracion.sServidorSmtp.ToString();
            txbUsuarioSmtp.Text = objConfiguracion.sCorreoEmisor.ToString();
            txbPuertoSmtp.Text=objConfiguracion.sPuertoSmtp.ToString();
            txbContraseñaSmtp.Attributes.Add("value",objConfiguracion.sContraseñaSmtp.ToString());

            //Renglones de detalle en las facturas
            txbNumRenglonesFactura.Text=objConfiguracion.iNoMaxRenglonesFactura.ToString();
                
            
            //Variables auxiliares para cuando guardan y no modifican los paths
            Session["sAuxCertificado"] = objConfiguracion.sRutaCertificado.ToString();
            Session["sAuxLlave"] = objConfiguracion.sRutaLlave.ToString();
            try
            {
                Session["sAuxFirma"] = objConfiguracion.sRutaFirma.ToString();
            }
            catch
            {
                Session["sAuxFirma"] = "";
            }
            Session["sAuxLogotipo"] = objConfiguracion.sRutaBaner.ToString();

        }

        protected bool ValidarControles()
        {
            rfvRazonSocial.Validate();
            rfvRfc.Validate();
            rfvRegimenFiscal.Validate();
            rfvDomicilio.Validate();
            rfvCodigoPostal.Validate();
            rfvMunicipio.Validate();
            rfvEstado.Validate();
            rfvPais.Validate();
            rfvServidor.Validate();
            rfvUsuario.Validate();
            rfvContraseñaBd.Validate();
            revCaducidad.Validate();
            return Page.IsValid;
        }

        protected void Deshabilita()
        {
            txbRazonSocial.Enabled = false;
            txbRfc.Enabled = false;
            txbRegimenFiscal.Enabled = false;
            txbDomicilio.Enabled = false;
            txbCodigoPostal.Enabled = false;
            txbMunicipio.Enabled = false;
            txbEstado.Enabled = false;
            txbPais.Enabled = false;
            ofdCertificado.Enabled = false;
            ofdLlave.Enabled = false;
            ofdFirma.Enabled = false;
            txbContraseñaFac.Enabled = false;
            txbServidor.Enabled = false;
            txbUsuario.Enabled = false;
            txbContraseñaBd.Enabled = false;
            //txbColor.Enabled = false;
            ofdLogotipo.Enabled = false;
            txbFolioPedidos.Enabled = false;
            txbFolioRecetas.Enabled = false;
            txbFolioRemisiones.Enabled = false;
            txbFolioFacturas.Enabled = false;
            ckbPedidos.Enabled = false;
            ckbRecetas.Enabled = false;
            ckbRemisiones.Enabled = false;
            ckbFacturas.Enabled = false;
            ckbVentasNegativas.Enabled = false;
            txbSevidorSmtp.Enabled = false;
            txbPuertoSmtp.Enabled = false;
            txbUsuarioSmtp.Enabled = false;
            txbContraseñaSmtp.Enabled = false;
            txbNumRenglonesFactura.Enabled = false;
            txbCaducidad.Enabled = false;

            rbVerde.Enabled=false;
            rbVerde1.Enabled=false;
            rbAzul.Enabled=false;
            rbAzul.Enabled=false;
            rbCafe.Enabled=false;
            rbRojo.Enabled=false;
            rbAmarillo.Enabled=false;
            rbNaranja.Enabled=false;
            rbMorado.Enabled=false;
            rbGris.Enabled = false;
        }

        protected void Habilita()
        {

            txbRazonSocial.Enabled=true;
            txbRfc.Enabled=true;
            txbRegimenFiscal.Enabled = true;
            txbDomicilio.Enabled=true;
            txbCodigoPostal.Enabled = true;
            txbMunicipio.Enabled = true;
            txbEstado.Enabled = true;
            txbPais.Enabled = true;
            ofdCertificado.Enabled=true;
            ofdLlave.Enabled=true;
            ofdFirma.Enabled = true;
            txbContraseñaFac.Enabled=true;
            txbServidor.Enabled=true;
            txbUsuario.Enabled=true;
            txbContraseñaBd.Enabled=true;
            //txbColor.Enabled=true;
            ofdLogotipo.Enabled=true;
            txbFolioPedidos.Enabled=true;
            txbFolioRecetas.Enabled=true;
            txbFolioRemisiones.Enabled=true;
            txbFolioFacturas.Enabled=true;
            ckbPedidos.Enabled=true;
            ckbRecetas.Enabled=true;
            ckbRemisiones.Enabled=true;
            ckbFacturas.Enabled=true;
            ckbVentasNegativas.Enabled = true;
            txbSevidorSmtp.Enabled = true;
            txbPuertoSmtp.Enabled = true;
            txbUsuarioSmtp.Enabled = true;
            txbContraseñaSmtp.Enabled = true;
            txbNumRenglonesFactura.Enabled = true;
            txbCaducidad.Enabled = true;

            rbVerde.Enabled = true;
            rbVerde1.Enabled = true;
            rbAzul.Enabled = true;
            rbAzul.Enabled = true;
            rbCafe.Enabled = true;
            rbRojo.Enabled = true;
            rbAmarillo.Enabled = true;
            rbNaranja.Enabled = true;
            rbMorado.Enabled = true;
            rbGris.Enabled = true;

        }

       

        protected void imbEditar_Click(object sender, EventArgs e)
        {
            Habilita();
            //0175 GT
            ConfigurarMenuBotones(false, false, false, false, true, true, false, false);
            
        }
        

        protected void imbAceptar_Click(object sender, EventArgs e)
        {
            int iResult;
            sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            if (ValidarControles())
            {

                odalConfiguracion = new MedDAL.Configuracion.DALConfiguracion();

                odalConfiguracion.sRazonSocial = txbRazonSocial.Text.ToString();
                odalConfiguracion.sRfc = txbRfc.Text.ToString();
                odalConfiguracion.sRegimenFiscal = txbRegimenFiscal.Text.ToString();
                odalConfiguracion.sDomicilio = txbDomicilio.Text.ToString();
                odalConfiguracion.sCodigoPostal=txbCodigoPostal.Text;
                odalConfiguracion.sMunicipio=txbMunicipio.Text;
                odalConfiguracion.sEstado=txbEstado.Text;
                odalConfiguracion.sPais = txbPais.Text;

                if (txbCaducidad.Text == "" || !int.TryParse(txbCaducidad.Text, out iResult))
                {
                    odalConfiguracion.iCaducidad = 0;
                }
                else  
                {
                    odalConfiguracion.iCaducidad = int.Parse(txbCaducidad.Text);
                }
               if(ofdCertificado.FileName.Equals(""))
                    odalConfiguracion.sRutaCertificado = Session["sAuxCertificado"].ToString();
                else
                    odalConfiguracion.sRutaCertificado = ofdCertificado.FileName.ToString();

               if(ofdLlave.FileName.Equals(""))
                    odalConfiguracion.sRutaLlave = Session["sAuxLlave"].ToString();
                else
                    odalConfiguracion.sRutaLlave = ofdLlave.FileName.ToString();

               if (ofdFirma.FileName.Equals(""))
                   odalConfiguracion.sRutaFirma = Session["sAuxFirma"].ToString();
               else
                   odalConfiguracion.sRutaFirma = ofdFirma.FileName.ToString();
                
                odalConfiguracion.sContraseña = txbContraseñaFac.Text.ToString();
                odalConfiguracion.sServidorBD = txbServidor.Text.ToString();
                odalConfiguracion.sUsuarioBD = txbUsuario.Text.ToString();
                odalConfiguracion.sContraseñaBD = txbContraseñaFac.Text.ToString();
                //odalConfiguracion.sColorInterfaz = txbColor.Text.ToString();

                if (rbVerde.Checked == true)
                    odalConfiguracion.sColorInterfaz = "Verde";
                if (rbVerde1.Checked == true)
                    odalConfiguracion.sColorInterfaz = "Verde1";
                if (rbAzul.Checked == true)
                    odalConfiguracion.sColorInterfaz = "Azul";
                if (rbAzul1.Checked == true)
                    odalConfiguracion.sColorInterfaz = "Azul1";
                if (rbCafe.Checked == true)
                    odalConfiguracion.sColorInterfaz = "Cafe";
                if (rbRojo.Checked == true)
                    odalConfiguracion.sColorInterfaz = "Rojo";
                if (rbAmarillo.Checked == true)
                    odalConfiguracion.sColorInterfaz = "Amarillo";
                if (rbNaranja.Checked == true)
                    odalConfiguracion.sColorInterfaz = "Naranja";
                if (rbMorado.Checked == true)
                    odalConfiguracion.sColorInterfaz = "Morado";
                if (rbGris.Checked == true)
                    odalConfiguracion.sColorInterfaz = "Gris";


                if(ofdLogotipo.FileName.Equals(""))
                    odalConfiguracion.sRutaBaner = Session["sAuxLogotipo"].ToString();
                else
                    odalConfiguracion.sRutaBaner = ofdLogotipo.FileName.ToString();

                if(txbFolioPedidos.Text.Equals(""))
                    odalConfiguracion.iFolioPedidos = 0;
                else
                    odalConfiguracion.iFolioPedidos = Convert.ToInt32(txbFolioPedidos.Text);
                
                if (txbFolioRemisiones.Text.Equals(""))
                    odalConfiguracion.iFolioRemisiones = 0;
                else
                    odalConfiguracion.iFolioRemisiones = Convert.ToInt32(txbFolioRemisiones.Text);
                
                if (txbFolioRecetas.Text.Equals(""))
                    odalConfiguracion.iFolioRecetas = 0;
                else
                    odalConfiguracion.iFolioRecetas = Convert.ToInt32(txbFolioRecetas.Text);
                
                if (txbFolioFacturas.Text.Equals(""))
                    odalConfiguracion.iFolioFacturas = 0;
                else
                    odalConfiguracion.iFolioFacturas = Convert.ToInt32(txbFolioFacturas.Text);
                
                if (ckbPedidos.Checked == true)
                    odalConfiguracion.iPedidosAutomatico = 1;
                else
                    odalConfiguracion.iPedidosAutomatico = 0;

                if (ckbRemisiones.Checked == true)
                    odalConfiguracion.iRemisionesAutomatico = 1;
                else
                    odalConfiguracion.iRemisionesAutomatico = 0;
                
                if (ckbRecetas.Checked == true)
                    odalConfiguracion.iRecetasAutomatico = 1;
                else
                    odalConfiguracion.iRecetasAutomatico = 0;
                
                if (ckbFacturas.Checked == true)
                    odalConfiguracion.iFacturasAutomatico = 1;
                else
                    odalConfiguracion.iFacturasAutomatico = 0;

                if (ckbVentasNegativas.Checked == true)
                    odalConfiguracion.iVentasNegativas = 1;
                else
                    odalConfiguracion.iVentasNegativas = 0;

                //SMTP
                odalConfiguracion.sServidorSmtp = txbSevidorSmtp.Text.ToString();
                odalConfiguracion.sPuertoSmtp = txbPuertoSmtp.Text.ToString();
                odalConfiguracion.sCorreoEmisor = txbUsuarioSmtp.Text.ToString();
                odalConfiguracion.sContraseñaSmtp = txbContraseñaSmtp.Text.ToString();

                //Num maximo de renglones
                if (txbNumRenglonesFactura.Text == "")
                {
                    odalConfiguracion.iNoMaxRenglonesFactura = 20;
                }
                else
                {
                    odalConfiguracion.iNoMaxRenglonesFactura = Convert.ToInt32(txbNumRenglonesFactura.Text);
                }

                oblConfiguracion = new MedNeg.Configuracion.BlConfiguracion();

                if (oblConfiguracion.GuardarDatos(odalConfiguracion, sRutaArchivoConfig))
                {
                    //Subir los archivos
                    SubirArchivosHacienda();
                    SubirLogo();

                    CargarDatos();
                    Deshabilita();

                    //GT 0175
                    ConfigurarMenuBotones(false, false, false, true, false, false, false, false);
                }
               
            }
            else
            {
                Habilita();
            }
        }

       /// <summary>
       /// Subir el logo
       /// </summary>
        private void SubirLogo()
        {
            //string sRutaArchivoLogo = Server.MapPath("~/Archivos/Logo/");
            string sRutaArchivoLogo = Server.MapPath("~/Images/");
            if (ofdLogotipo.HasFile && (Path.GetExtension(ofdLogotipo.FileName)==".PNG" || Path.GetExtension(ofdLogotipo.FileName)==".png")) 
            {
                try
                {
                    ofdLogotipo.SaveAs(sRutaArchivoLogo + "logo.png");
                }
                catch
                {

                }
            }
        }

       /// <summary>
       /// Subir el certificado y la llave
       /// </summary>
        private void SubirArchivosHacienda()
        {
            string sRutaArchivosFacturacion = Server.MapPath("~/Archivos/Facturacion/");

            //Certificado
            if (ofdCertificado.HasFile && Path.GetExtension(ofdCertificado.FileName) == ".cer")
            {
                try
                {
                    ofdCertificado.SaveAs(sRutaArchivosFacturacion + ofdCertificado.FileName.ToString());
                }
                catch
                {

                }
            }

            //Llave
            if (ofdLlave.HasFile && Path.GetExtension(ofdLlave.FileName) == ".key")
            {
                try
                {
                    ofdLlave.SaveAs(sRutaArchivosFacturacion+ofdLlave.FileName.ToString());
                }
                catch
                {

                }

            }

            //Firma
            if (ofdFirma.HasFile && Path.GetExtension(ofdFirma.FileName) == ".pfx")
            {
                try
                {
                    ofdFirma.SaveAs(sRutaArchivosFacturacion + ofdFirma.FileName.ToString());
                }
                catch
                {

                }

            }
        }


        protected void imbCancelar_Click(object sender, EventArgs e)
        {
            CargarDatos();
            Deshabilita();
            //GT 0175
            ConfigurarMenuBotones(false, false, false, true, false, false, false, false);
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
        

        

        

 
        

	}
}