using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
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
            UserList.Visible = false;
            NotificationDiv.Visible = false;
            BindLocationDetails();
        }
    }
    protected void UserGrid_RowEditing(object sender, GridViewEditEventArgs e)
    {
        UserGrid.EditIndex = e.NewEditIndex;
        BindSybProcess("1"); // Pass the correct process ID
    }
    protected void UserGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = UserGrid.Rows[e.RowIndex];
        int id = Convert.ToInt32(UserGrid.DataKeys[e.RowIndex].Value);
        TextBox txtSubProcessName = (TextBox)row.FindControl("txtSubProcessName");
        string updatedName = txtSubProcessName.Text;

        // Update the database
        try
        {
            using (SqlConnection conn = new SqlConnection(UserInfo.Dnycon))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE Eval_SubProcess SET SubProcessName = @SubProcessName WHERE ID = @ID", conn))
                {
                    cmd.Parameters.AddWithValue("@SubProcessName", updatedName);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                }
            }
            UserGrid.EditIndex = -1; // Exit edit mode
            BindSybProcess("1");    // Refresh GridView
        }
        catch (Exception ex)
        {
            Response.Write("Error: " + ex.Message);
        }
    }
    protected void UserGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        UserGrid.EditIndex = -1;
        BindSybProcess("1"); // Refresh GridView
    }



    protected void ProgramChangedinde(object sender, EventArgs e)
    {
        string ProcessID = ProgramDropDown.SelectedValue.ToString() ;
        if (ProgramDropDown.SelectedIndex >0)
        {
            BindSybProcess(ProcessID);
        }

    }
    [WebMethod]
    public static string UpdateUserStatus(string name, bool isActive, int id)
    {
        Dl_Connection dl = new Dl_Connection();
        DL_Encrpt dec = new DL_Encrpt();
        string DecName = dec.Encrypt(name);

       
        try
        {
            using (SqlConnection conn = new SqlConnection(UserInfo.Dnycon))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("update [Eval_SubProcess] set  IsActive=@isActive where id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@isActive", isActive ? 1 : 0);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return "User status updated successfully.";
                    }
                    else
                    {
                        return "Failed to update user status.";
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            return "Database error: " + ex.Message;
        }
    }

    private void BindSybProcess(string processID)
    {

        try
        {
            Dl_Connection dl = new Dl_Connection();
           
            using (SqlConnection cc = new SqlConnection(UserInfo.Dnycon))
            {
                cc.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT sp.ID, p.Process, sp.SubProcessName, l.Location, sp.IsActive FROM  dbo.Eval_SubProcess sp LEFT JOIN  dbo.Eval_Process p ON sp.ProcessID = p.ID LEFT JOIN dbo.LocationMaster l ON sp.LocationID = l.ID WHERE p.ID = '1' and sp.CreatedBy='"+UserInfo.UserName+"' ;", cc))
                {
                    cmd.Parameters.AddWithValue("@processID", processID);

                    DataTable dt = new DataTable();
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dt);
                   
                    UserList.Visible = true;

                    if (dt.Rows.Count > 0)
                    {
                        UserGrid.DataSource = dt;
                        UserGrid.DataBind();
                    }
                    else
                    {
                        UserList.Visible = false;
                        UserGrid.DataSource = null;
                        UserGrid.DataBind();
                        Response.Write("No active users found.");
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            Response.Write("Database error: " + ex.Message);
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
            if (LocationDropDown.SelectedIndex > 0 && ProgramDropDown.SelectedIndex > 0 && txtsubprogram.Text != "" && txtsubprogram.Text != null)
            {
                using (SqlConnection conn = new SqlConnection(UserInfo.Dnycon))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("CreateSubProcess", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Location", Location);
                        cmd.Parameters.AddWithValue("@Subprogram", Subprogram);
                        cmd.Parameters.AddWithValue("@Procesname", selectedProgram);
                        cmd.Parameters.AddWithValue("@CreatedBy", UserInfo.UserName);

                        cmd.ExecuteNonQuery();

                    }
                }
                NotificationDiv.Visible = true;
                Notification.InnerText = "Sub-Process Assign Sucessfully";
            }
            else
            {
                NotificationDiv.Visible = true;
                Notification.InnerText = "Please Select All process";
            }
            

            //Response.Redirect("ListProcess", false);
        }
        catch (SqlException ex)
        {

        }
    }
}