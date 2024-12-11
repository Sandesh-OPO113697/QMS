using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LogIn : System.Web.UI.Page
{
    Dl_Connection dcn = new Dl_Connection();
    DL_Encrpt dcrptkeyval = new DL_Encrpt();
    Dl_Login dl_l = new Dl_Login();
    string encrptedconnctionsrtng = ConfigurationSettings.AppSettings["SuperDataBase"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    void Application_Start(object sender, EventArgs e)
    {
        RouteTable.Routes.MapPageRoute("LogInRoute", "Login", "~/Login.aspx");
    }
    protected void btn_Check_LogIn(object sender, EventArgs e)
    {
        try
        {
            string ConSTR = dcrptkeyval.Decrypt(encrptedconnctionsrtng);
            int IsFind= dl_l.CheckUserValid(txtUserName.Text.ToString(), txtPassword.Text.ToString(), ConSTR);
            string userType = IsFind == 1 ? "SuperAdmin" : "NormalUser";
            if (userType == "SuperAdmin")
            {
                Response.Redirect("Qms", false);
            }
            else
            {

                dcn.GetDynamicConnection(txtUserName.Text.ToString());
                int IsFind2 = dl_l.CheckUserAccountUser(txtUserName.Text.ToString(), txtPassword.Text.ToString(), UserInfo.Dnycon);
                string userTypeAccount = IsFind2 == 1 ? "AccountUser" : "Fail";

                dl_l.AcessUserLevel(txtUserName.Text.ToString());
                if (userTypeAccount == "AccountUser")
                {
                    Response.Redirect("ListProcess", false);
                }
                else
                {
                    UserInfo.Dnycon = ConSTR;
                    ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Enter Correct Credential.');", true);
                    return;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
}