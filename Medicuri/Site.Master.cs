using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.IO;

namespace Medicuri
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected Hashtable permisos;
        string sRutaArchivoConfig;
        MedDAL.Configuracion.DALConfiguracion cConfiguracion;
        MedNeg.Configuracion.BlConfiguracion oblConfiguracion;

        protected void Page_Init(object sender, EventArgs e)
        {
            sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
            CargarCSS();
        }

        /// <summary>
        /// Agrega el hmtl link al header del catalogo, del css correspondiente a cargar
        /// antes de que se cargue la pagina
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
        /// carga los permisos a la variable de session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            permisos = (Hashtable)Session["permisos"];
            Create_MenuPanel(permisos.Keys);
        }

        /// <summary>
        /// Separa en listas los menus que se crean en el panel izquierdo (menu desplegable)
        /// </summary>
        /// <param name="keyColl"></param>
        protected void Create_MenuPanel(ICollection keyColl)
        {
            List<string> lstUsuario = new List<string>();
            string[] asCatalogo = new string[9];
            string[] asFactura = new string[6];
            string[] asLocalidad = new string[4];
            List<string> lstCatalogo = new List<string>();
            List<string> lstAlmacen = new List<string>();
            List<string> lstFactura = new List<string>();
            List<string> lstCause = new List<string>();
            List<string> lstBitacora = new List<string>();
            List<string> lstConfiguracion = new List<string>();
            List<string> lstCampoEditable = new List<string>();
            List<string> lstTipo = new List<string>();
            List<string> lstEnsamble = new List<string>();
            List<string> lstMovimiento = new List<string>();
            List<string> lstDefault = new List<string>();

            foreach (string s in keyColl)
            {
                switch (s)
                {
                    case "usuarios":
                    case "perfiles":
                    case "cambiar contraseña":
                        lstUsuario.Add(s);
                        break;
                    //Catalogo
                    case "clientes":
                        asCatalogo[0] = s;
                        break;
                    case "proveedores":
                        asCatalogo[1] = s;
                        break;
                    case "vendedores":
                        asCatalogo[2] = s;
                        break;
                    case "lineas de credito":
                        asCatalogo[3] = s;
                        break;
                    case "tipos de iva":
                        asCatalogo[4] = "tipos de impuesto";
                        break;
                    case "estados":
                        asLocalidad[0] = s;
                        break;
                    case "municipios":
                        asLocalidad[1] = s;
                        break;
                    case "poblaciones":
                        asLocalidad[2] = s;
                        break;
                    case "colonias":
                        asLocalidad[3] = s;
                        break;                    

                    case "almacenes":
                    case "productos":
                    case "inventarios":
                    case "ensambles":
                    case "movimientos":
                        lstAlmacen.Add(s);
                        break;

                        //Facturacion
                    case "pedidos":
                        asFactura[0] = s;
                        break;
                    case "recetas":
                         asFactura[2] = s;
                        break;
                    case "remisiones":
                         asFactura[1] = s;
                        break;
                    case "cuentas x cobrar":
                         asFactura[5] = s;
                        break;
                    case "facturas":
                         asFactura[3] = s;
                        break;
                    case "facturas x receta":
                         asFactura[4] = s;
                        break;
                    case "causes":
                        lstCause.Add(s);
                        break;
                    case "bitacora":
                        lstBitacora.Add(s);
                        break;
                    case "configuracion":
                        lstConfiguracion.Add(s);
                        break;
                    case "campos editables":
                        lstCampoEditable.Add(s);
                        break;
                    case "tipos":
                        lstTipo.Add(s);
                        break;
                    /*case "ensambles":
                        lstEnsamble.Add(s);
                        break;
                    case "movimientos":
                        lstMovimiento.Add(s);
                        break;*/
                    case "reportes":
                        lstDefault.Add(s);
                        break;
                }
            }
            Constructor(lstUsuario, "Usuarios");
            Constructor(asCatalogo, "Catalogos");
            Constructor(lstAlmacen, "Inventarios");
            //Constructor(lstEnsamble, "Ensambles");
            //Constructor(lstMovimiento, "Movimientos");
            Constructor(asLocalidad, "Localidad");
            Constructor(asFactura, "Facturación");
            Constructor(lstCause, "Biblioteca");
            Constructor(lstBitacora, "Bitacora");
            Constructor(lstConfiguracion, "Configuración");
            Constructor(lstCampoEditable, "Campos");
            Constructor(lstTipo, "Tipos");
            Constructor(lstDefault, "Reportes");
        }

        protected void Constructor(string[] array, string menuName)
        {
            bool bCandado = false;
            if (array.Count<string>() > 0)
            {
                foreach (string s in array)
                {
                    if (s != null)
                        bCandado = true;
                }
                if (bCandado)
                {
                    AccordionPane newPane = Create_Menu(menuName, menuName);
                    foreach (string s in array)
                    {
                        if (s != null)
                            Create_SubMenu(newPane, menuName.ToLower(), s, true);
                    }
                    AccordionPrincipal.Panes.Add(newPane);
                }        
            }            
        }

        /// <summary>
        /// Crea un nuevo menu con sus respectivos submenus dada una lista y nombre
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="menuName"></param>
        protected void Constructor(List<string> lst, string menuName) {
            if (lst.Count > 0) {
                AccordionPane newPane =  Create_Menu(menuName, menuName);
                foreach (string s in lst) {
                    Create_SubMenu(newPane, menuName.ToLower(), s, true);
                }
                AccordionPrincipal.Panes.Add(newPane);
            }

        }


        /// <summary>
        /// Crea el sumbenu para los menus desplegables
        /// </summary>
        /// <param name="paneId"></param>
        /// <param name="paneText"></param>
        /// <returns></returns>
        protected AccordionPane Create_Menu(string paneId, string paneText) {
            AccordionPane pane = new AccordionPane();
            pane.ID = paneId;
            Label headerLabel = new Label();
            headerLabel.Text = paneText;
            pane.HeaderContainer.Controls.Add(headerLabel);
            return pane;
        }


        /* Submenu */
        protected void Create_SubMenu(AccordionPane pane, string subMenuId, string subMenuText, bool enterRequired) {
            Label submenu = new Label();
            submenu.Text = subMenuText.Substring(0, 1).ToUpper() + subMenuText.Substring(1);;
            string temp = subMenuId.Substring(0, 3); 
            submenu.ID = temp+""+subMenuText;
            submenu.CssClass = "labelMenu";
            submenu.Attributes.Add("onclick", "loadHTMLonDiv('"+subMenuText.Replace(" ", string.Empty)+".aspx', 'content');");
            pane.ContentContainer.Controls.Add(submenu);
            if(enterRequired)
                pane.ContentContainer.Controls.Add(new LiteralControl("<br />"));
            
        }
    }
}