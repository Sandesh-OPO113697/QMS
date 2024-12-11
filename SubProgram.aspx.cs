using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SubProgram : System.Web.UI.Page
{
    DL_Encrpt drc = new DL_Encrpt();
    Dl_Connection con = new Dl_Connection();
    public string ConnSTR = ConfigurationManager.ConnectionStrings["SuperDataBase"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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

    protected void LocationDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedLocation = LocationDropDown.SelectedValue;
        BindProcessDetails(selectedLocation);


    }

    public void BindProcessDetails(string location)
    {
        try
        {
            ProgramDropDown.Items.Clear();
            using (SqlConnection conn = new SqlConnection(UserInfo.Dnycon))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("USP_Fill_Dropdown", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "Select_Program_Master_locationWise");
                    cmd.Parameters.AddWithValue("@p_username", location);

                    DataTable dt = new DataTable();
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        ProgramDropDown.DataSource = dt;
                        ProgramDropDown.DataTextField = "Process";
                        ProgramDropDown.DataValueField = "ID";
                        ProgramDropDown.DataBind();
                        ProgramDropDown.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", ""));
                    }
                    else
                    {

                        ProgramDropDown.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Accounts Available", ""));

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

    protected void CreateProcess(object sender, EventArgs e)
    {
        try
        {
            string Location = LocationDropDown.SelectedValue.ToString();
            string Subprogram = txtsubprogram.Text.ToString();
            string selectedProgram = ProgramDropDown.SelectedValue.ToString(); 
            using (SqlConnection conn = new SqlConnection(UserInfo.Dnycon))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("CreateSubProcess", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Location", Location);
                    cmd.Parameters.AddWithValue("@Subprogram", Subprogram);
                    cmd.Parameters.AddWithValue("@Procesname", selectedProgram);
                   
                    cmd.ExecuteNonQuery();

                }
            }

            Response.Redirect("ListProcess", false);
        }
        catch (SqlException ex)
        {
           
        }
    }
}