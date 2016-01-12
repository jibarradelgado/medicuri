using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Medicuri
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if ((bool)Session["alertabitacora"] == false)
                {
                    if (DateTime.Today.Day == 1)
                    {
                        if (!ClientScript.IsStartupScriptRegistered("alert"))
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(),
                                "alert", "alertarBitacora();", true);
                        }
                    }
                    Session["alertabitacora"] = true;
                }
            }
            if ((Hashtable)Session["permisos"] == null)
            {
                Response.Redirect("Login.aspx");
            }
        }
    }
}