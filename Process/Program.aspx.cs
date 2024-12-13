using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Program : System.Web.UI.Page
{
    DL_Encrpt drc = new DL_Encrpt();
    Dl_Connection con = new Dl_Connection();
    public string ConnSTR = ConfigurationManager.ConnectionStrings["SuperDataBase"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            NotificationDiv.Visible = false;
            BindLocationDetails();
        }
    }

    public void BindLocationDetails()
    {
        try
        {

            using (SqlConnection conn = new SqlConnection(UserInfo.Dnycon))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("USP_Fill_Dropdown", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "Select_Location_Master");

                    DataTable dt = new DataTable();
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        LocationDropDown.DataSource = dt;
                        LocationDropDown.DataTextField = "Location";
                        LocationDropDown.DataValueField = "ID";
                        LocationDropDown.DataBind();
                        LocationDropDown.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", ""));
                    }
                    else
                    {
                        LocationDropDown.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Accounts Available", ""));
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            // Log error and show a message to the user
            Response.Write("Database error: " + ex.Message);
        }
    }
    public void ClearFeils()
    {
        LocationDropDown.SelectedIndex = 0;
        txtDataRetention.Text = "";
        txtProgram.Text = "";
    }

    protected void CreateProcess(object sender, EventArgs e)
    {
        try
        {
            string Location = LocationDropDown.SelectedValue.ToString();
            string dataRetaintion = txtDataRetention.Text.ToString();
            string Procesname = txtProgram.Text.ToString();
            if (Location != "" && dataRetaintion != ""  && LocationDropDown.SelectedIndex > 0)
            {


                using (SqlConnection conn = new SqlConnection(UserInfo.Dnycon))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("CreateProcess", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Location", Location);
                        cmd.Parameters.AddWithValue("@dataRetaintion", dataRetaintion);
                        cmd.Parameters.AddWithValue("@Procesname", Procesname);
                        cmd.Parameters.AddWithValue("@CreateBy", UserInfo.UserName);

                        cmd.ExecuteNonQuery();


                    }
                    NotificationDiv.Visible = true;
                    Notification.InnerText = " Process Is Created " + Procesname;
                }
            }
            else
            {
                NotificationDiv.Visible = true;
                Notification.InnerText = " Please Select All feilds " ;
            }

            ClearFeils();


        }
        catch (SqlException ex)
        {

        }
    }
}