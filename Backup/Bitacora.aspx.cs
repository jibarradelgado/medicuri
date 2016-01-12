using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.IO;
using MedNeg.Bitacora;

namespace Medicuri
{
    public partial class Bitacora : System.Web.UI.Page
    {
        ImageButton imbRespaldar, imbReportes, imbMostrar;
        RadioButton rdbUsuario, rdbModulo, rdbTodos;
        CheckBox ckbFechas;
        Button btnBuscar;
        TextBox txbBuscar, txbFechaInicio, txbFechaFin;
        MedNeg.Bitacora.BlBitacora oblBitacora;
        List<System.Xml.Linq.XElement> lstxBitacora;
        List<MedDAL.DAL.bitacora> lstbBitacora;
        
        //DAL.medicuriEntities oMedicuriEntities;
        protected void Buscar()
        {
            int iTipo = 1;
            if (rdbTodos.Checked)
            {
                iTipo = 1;
            }
            else if (rdbUsuario.Checked)
            {
                iTipo = 2;
            }
            else if (rdbModulo.Checked)
            {
                iTipo = 3;
            }

            IQueryable<MedDAL.DAL.bitacora> iqrBitacora =
                !ckbFechas.Checked ? oblBitacora.Buscar(txbBuscar.Text, iTipo) : oblBitacora.Buscar(txbBuscar.Text, iTipo, txbFechaInicio.Text, txbFechaFin.Text);
            Session["resultadoquery"] = iqrBitacora;

            //List<MedDAL.DAL.bitacora> lstBitacora = new List<MedDAL.DAL.bitacora>();
            //lstBitacora.AddRange(iqrBitacora);

            //Session["lstBitacora"] = lstBitacora;

            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.DAL.bitacora>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "FechaEntradaSrv ASC";            

            try
            {
                gdvDatos.DataSource = dv;
                //gdvLista.DataKeyNames = new string[] { "" };
                gdvDatos.DataBind();
                gdvDatos.Visible = true;
                if (txbBuscar.Text == "")
                {
                    gdvDatos.EmptyDataText = "No existen bitacoras registradas aun";
                }
                else
                {
                    gdvDatos.EmptyDataText = "No existen bitacoras que coincidan con la búsqueda";
                }
                gdvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //oMedicuriEntities = new DAL.medicuriEntities();
            Hashtable htbPermisos = (Hashtable)Session["permisos"];
            char cPermiso = 'N';
                        
            try
            {
                #region Interfaz
                cPermiso = (char)htbPermisos["bitacora"];
                imbRespaldar = (ImageButton)Master.FindControl("ImgBtnRespaldar");
                imbRespaldar.Click += new ImageClickEventHandler(this.imbRespaldar_Click);
                imbReportes = (ImageButton)Master.FindControl("ImgBtnReportes");
                imbReportes.Click += new ImageClickEventHandler(this.imbReportes_Click);
                rdbTodos = (RadioButton)Master.FindControl("rdbFiltro1");
                rdbTodos.Text = "Todos";
                rdbUsuario = (RadioButton)Master.FindControl("rdbFiltro2");
                rdbUsuario.Text = "Usuario";
                rdbModulo = (RadioButton)Master.FindControl("rdbFiltro3");
                rdbModulo.Text = "Modulo";
                ckbFechas = (CheckBox)Master.FindControl("ckbFiltro1");
                ckbFechas.Text = "Fechas";
                ckbFechas.CheckedChanged += new EventHandler(this.ckbFechas_Click);
                btnBuscar = (Button)Master.FindControl("btnBuscar");
                btnBuscar.Click += new EventHandler(this.btnBuscar_Click);
                txbBuscar = (TextBox)Master.FindControl("txbBuscar");
                txbFechaInicio = (TextBox)Master.FindControl("txbFechaInicio");
                txbFechaFin = (TextBox)Master.FindControl("txbFechaFin");
                imbMostrar = (ImageButton)Master.FindControl("imgBtnMostrar");
                imbMostrar.Click += new ImageClickEventHandler(this.imbMostrar_Click);

            #endregion
                oblBitacora = new MedNeg.Bitacora.BlBitacora();
                if (!IsPostBack)
                {
                    Session["accion"] = 0;
                    lblAviso.Text = "";
                    lblAviso2.Text = "";
                    pnlCatalogoUpload.Visible = true;
                    Session["resultadoquery"] = "";
                    ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
                }
                if ((int)Session["accion"] == 1)
                {
                    Session["accion"] = 0;
                    lblAviso.Text = "Se ha realizado el respaldo con éxito";
                }
                else if ((int)Session["accion"] == 2)
                {
                    Session["accion"] = 0;
                    lblAviso.Text = "Respaldo realizado, sin embargo los registros no pudieron ser eliminados";
                }
            }
            catch (NullReferenceException)
            {
                if (!ClientScript.IsStartupScriptRegistered("alertsession"))
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(),
                        "alertsession", "alertarSesion();", true);
                }
                pnlCatalogoUpload.Visible = false;
                pnlCatalogoSub.Visible = false;
                InterfazBitacora oPrincipal = (InterfazBitacora)this.Master;
                oPrincipal.DeshabilitarControles(this);
                oPrincipal.DeshabilitarControles();
            }
        }


        protected void MostrarLista()
        {
            var oQuery = oblBitacora.MostrarLista();
            Session["resultadoquery"] = oQuery;
            ViewState["direccionsorting"] = System.Web.UI.WebControls.SortDirection.Ascending;
            var result = (IQueryable<MedDAL.DAL.bitacora>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            dv.Sort = "FechaEntradaSrv ASC";

            Session["accion"] = 3;
            try
            {
                gdvDatos.DataSource = dv;
                gdvDatos.DataKeyNames = new string[] { "IdEntradaBitacora" };
                gdvDatos.DataBind();

                if (gdvDatos.Rows.Count == 0 && txbBuscar.Text == "")
                {
                    gdvDatos.EmptyDataText = "No existen tipos registrados aun";
                }
                else
                {
                    gdvDatos.EmptyDataText = "No existen tipos que coincidan con la búsqueda";
                }
                gdvDatos.ShowHeader = true;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

        }

        protected void imbMostrar_Click(object sender, EventArgs e)
        {
            pnlCatalogoSub.Visible = true;
           
            MostrarLista();
            
            //dgvDatos.SelectedIndex = -1;
            //Session["accion"] = 0;
            //lblAviso.Text = "";

        }

        protected void imbRespaldar_Click(object sender, EventArgs e)
        {            
            lblAviso.Text = "";
            string sFecha = DateTime.Now.ToString();
            while (sFecha.Contains(':'))
            {
                sFecha = sFecha.Remove(sFecha.IndexOf(':'), 1);                
            }
            while (sFecha.Contains('.'))
            {
                sFecha = sFecha.Remove(sFecha.IndexOf('.'), 1);
            }
            while (sFecha.Contains('/'))
            {
                sFecha = sFecha.Remove(sFecha.IndexOf('/'), 1);
            }
            
            string sXML = oblBitacora.ObtenerXML(Server.MapPath("~/Archivos/Bitacora" + sFecha + ".xml"));
            string sSQL = oblBitacora.ObtenerSQL(Server.MapPath("~/Archivos/Bitacora" + sFecha + ".txt"));

            oblBitacora.CrearZip(new string[] { Server.MapPath("~/Archivos"), Server.MapPath("~/Archivos/Bitacora" + sFecha + ".zip") });

            try
            {
                string path = Server.MapPath("~/Archivos/Bitacora" + sFecha + ".zip");

                System.IO.FileInfo file = new System.IO.FileInfo(path);
                if (file.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                    Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.ContentType = "application/....";
                    Response.WriteFile(file.FullName);

                    if (File.Exists(Server.MapPath("~/Archivos/Bitacora" + sFecha + ".xml")))
                    {
                        if (oblBitacora.EliminarTodo())
                        {
                            Session["accion"] = 1;
                        }
                        else
                        {
                            Session["accion"] = 2;                            
                        }
                    }
                    
                    Response.End();
                }
                else
                {
                    Response.Write("This file does not exist.");
                }
            }
            catch (Exception)
            {
                
            }            
        }       

        protected void imbReportes_Click(object sender, EventArgs e)
        {
            pnlCatalogoSub.Visible = true;
            
            lblAviso.Text = "";
            if (fupRespaldo.HasFile && Path.GetExtension(fupRespaldo.FileName) == ".xml")
            {
                Stream strArchivo = fupRespaldo.FileContent;
                StreamReader srdArchivo = new StreamReader(strArchivo, System.Text.Encoding.UTF8);
                string sArchivo = srdArchivo.ReadToEnd();

                lstbBitacora = oblBitacora.ObtenerBitacora(sArchivo);

                gdvDatos.DataSource = lstbBitacora;
                gdvDatos.DataKeyNames = new string[] { "idEntradaBitacora" };
                gdvDatos.DataBind();

                Session["streambitacora"] = sArchivo;
            }
            else
            {
                lblAviso.Text = "Seleccione un archivo xml de respaldo";
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            lblAviso.Text = "";
            if (int.Parse(Session["accion"].ToString()) == 3)
            {
                Buscar();
            }
            else
            {
                if (Session["streambitacora"] != null)
                {
                    XDocument xmlArchivo = XDocument.Parse(Session["streambitacora"].ToString());

                    int iTipo = 1;
                    if (rdbTodos.Checked)
                    {
                        iTipo = 1;
                    }
                    else if (rdbUsuario.Checked)
                    {
                        iTipo = 2;
                    }
                    else if (rdbModulo.Checked)
                    {
                        iTipo = 3;
                    }

                    if (ckbFechas.Checked && txbFechaInicio.Text != "" && txbFechaFin.Text != "")
                    {
                        gdvDatos.DataSource = oblBitacora.Buscar(xmlArchivo, txbFechaInicio.Text, txbFechaFin.Text, iTipo, txbBuscar.Text);
                        gdvDatos.DataBind();
                    }
                    else
                    {
                        gdvDatos.DataSource = oblBitacora.Buscar(xmlArchivo, iTipo, txbBuscar.Text);
                        gdvDatos.DataBind();
                    }

                    string sFechaInicio = ckbFechas.Checked ? txbFechaInicio.Text : "";
                    string sFechaFin = ckbFechas.Checked ? txbFechaFin.Text : "";


                }
            }
        }

        protected void ckbFechas_Click(object sender, EventArgs e) 
        {
            
        }

        protected void gdvDatos_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gdvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            InterfazBitacora oMaster = (InterfazBitacora)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.DAL.bitacora>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            gdvDatos.DataSource = oMaster.Sorting(e, ref oDireccion, dv);
            ViewState["direccionsorting"] = oDireccion;
            ViewState["sortexpression"] = e.SortExpression;
            gdvDatos.DataBind();
        }

        protected void gdvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            InterfazBitacora oMaster = (InterfazBitacora)this.Master;
            System.Web.UI.WebControls.SortDirection oDireccion = (System.Web.UI.WebControls.SortDirection)ViewState["direccionsorting"];
            var result = (IQueryable<MedDAL.DAL.bitacora>)Session["resultadoquery"];
            DataTable dt = MedNeg.Utilidades.DataSetLinqOperators.CopyToDataTable(result);
            DataView dv = new DataView(dt);
            gdvDatos.DataSource = oMaster.Paging(e, ViewState["sortexpression"] == null ? "FechaEntradaSrv" : ViewState["sortexpression"].ToString(), dv, ref gdvDatos, ref oDireccion);
            ViewState["direccionsorting"] = oDireccion;
            gdvDatos.DataBind();
        }
    }
}