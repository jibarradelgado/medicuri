using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.IO;

namespace Medicuri
{
    public partial class Site1 : System.Web.UI.MasterPage
    {

        string sRutaArchivoConfig;
        MedDAL.Configuracion.DALConfiguracion cConfiguracion;
        MedNeg.Configuracion.BlConfiguracion oblConfiguracion;

        protected void Page_Load(object sender, EventArgs e)
        {
            sRutaArchivoConfig = Server.MapPath("~/Archivos/Configuracion.xml");
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

        public void DeshabilitarControles(Control c)
        {
            if (c is TextBox)
            {
                ((TextBox)c).Enabled = false;
            }
            else if (c is Button)
            {
                ((Button)c).Enabled = false;
            }
            else if (c is DropDownList)
            {
                ((DropDownList)c).Enabled = false;
            }
            else if (c is CheckBox)
            {
                ((CheckBox)c).Enabled = false;
            }
            else if (c is ListBox)
            {
                ((ListBox)c).Enabled = false;
            }
            else if (c is ImageButton)
            {
                ((ImageButton)c).Enabled = false;
            }
            foreach (Control ctrl in c.Controls)
            {
                DeshabilitarControles(ctrl);
            }
        }

        public void DeshabilitarControles() 
        {
            this.imgBtnAceptar.Enabled = false;
            this.imgBtnCancelar.Enabled = false;
            this.imgBtnEditar.Enabled = false;
            this.imgBtnEliminar.Enabled = false;
            this.imgBtnImprimir.Enabled = false;
            this.imgBtnMostrar.Enabled = false;
            this.imgBtnNuevo.Enabled = false;
            this.imgBtnPrecios.Enabled = false;
            this.imgBtnReportes.Enabled = false;
        }

        private const string ASCENDING = " ASC";
        private const string DESCENDING = " DESC";

        public DataView Sorting(GridViewSortEventArgs e, ref System.Web.UI.WebControls.SortDirection oDireccion, DataView dv)
        {
            string sSortExpression = e.SortExpression;
            ViewState["sortexpression"] = e.SortExpression;
            
            if (oDireccion == System.Web.UI.WebControls.SortDirection.Ascending)
            {
                //e.SortDirection = System.Web.UI.WebControls.SortDirection.Descending;
                dv.Sort = sSortExpression + DESCENDING;
                oDireccion = System.Web.UI.WebControls.SortDirection.Descending;
            }
            else
            {
                //e.SortDirection = System.Web.UI.WebControls.SortDirection.Ascending;
                dv.Sort = sSortExpression + ASCENDING;
                oDireccion = System.Web.UI.WebControls.SortDirection.Ascending;
            }

            return dv;
        }

        public DataView Paging(GridViewPageEventArgs e, string sExpresionSort, DataView dv, ref GridView gdvDatos, ref System.Web.UI.WebControls.SortDirection oDireccion)
        {
            int iPagina = e.NewPageIndex;
            gdvDatos.PageIndex = iPagina;
            
            if (oDireccion == System.Web.UI.WebControls.SortDirection.Ascending)
            {
                dv.Sort = sExpresionSort + ASCENDING;
            }
            else
            {
                dv.Sort = sExpresionSort + DESCENDING;
            }
            return dv;
        }

    }
}