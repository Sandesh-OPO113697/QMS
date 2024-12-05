using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        VisibilityNavBar();
    }

    public void VisibilityNavBar()
    {
        if (UserInfo.UserType == "SuperAdmin")
        {
            table.Visible = false;
            Upload.Visible = false;
            UserReport.Visible = false;
            QAForm.Visible = false;
            ViewUser.Visible = false;
            Create.Visible = true;
            Reports.Visible = true; 
        }


        if (UserInfo.UserType == "Admin")
        {
            table.Visible = false;
            UserReport.Visible = true;
            QAForm.Visible = false;
            ViewUser.Visible = false;
            table.Visible = true;
            Upload.Visible = true;
            Create.Visible = false;
            UserReport.Visible = false;
        }

        if (UserInfo.UserType == "QE")
        {
            
            Create.Visible = false;
            Reports.Visible = false;
            table.Visible = false;
            Upload.Visible = false;
            UserReport.Visible = false;
            QAForm.Visible = true;
            ViewUser.Visible = true;
            Create.Visible = false;
            UserReport.Visible = true;
        }
       
    }
}
