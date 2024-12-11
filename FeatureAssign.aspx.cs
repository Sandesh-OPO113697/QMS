using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FeatureAssign : System.Web.UI.Page
{
    string connectionString = UserInfo.Dnycon;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindProcessToUser();
        }

    }


    public void BindProcessToUser()
    {
        string processQuery = "SELECT Id, FeatureName FROM [dbo].[Feature_Master] where Active = 1";
        DataTable dtProcess = GetData(processQuery);
        if (dtProcess.Rows.Count > 0)
        {
            CheckBoxList1.Items.Clear();
            foreach (DataRow row in dtProcess.Rows)
            {
                ListItem item = new ListItem(row["FeatureName"].ToString(), row["Id"].ToString());
                CheckBoxList1.Items.Add(item);
            }
        }

        string userQuery = "SELECT Roleid, Role_name FROM [dbo].[Role_Master] where active = 1";
        DataTable dt = GetData(userQuery);
        //DataTable dt = DecryptDataTable(dtUsers);
        if (dt.Rows.Count > 0)
        {
            users.DataSource = dt;
            users.DataTextField = "Role_name";
            users.DataValueField = "Roleid";
            users.DataBind();
        }

        users.Items.Insert(0, new ListItem("Select User", "0"));
    }

    private DataTable DecryptDataTable(DataTable dt)
    {
        DL_Encrpt encrypter = new DL_Encrpt();
        foreach (DataRow row in dt.Rows)
        {
            if (row["Name"] != DBNull.Value)
            {
                row["Name"] = encrypter.Decrypt(row["Name"].ToString());
            }
        }
        return dt;
    }

    private DataTable GetData(string query)
    {
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }
        return dt;
    }
    protected void CreateAccountBySuper(object sender, EventArgs e)
    {
        // Validation
        if (Page.IsValid)
        {
            string userID = users.SelectedValue;
            string UserName = users.SelectedItem.Text;

            // Manually loop through the CheckBoxList items to get selected processes
            List<string> selectedProcesses = new List<string>();
            foreach (ListItem item in CheckBoxList1.Items)
            {
                if (item.Selected)
                {
                    selectedProcesses.Add(item.Value); // Add selected process ID to the list
                }
            }

            // If processes are selected
            if (selectedProcesses.Count > 0)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    foreach (string processID in selectedProcesses)
                    {
                        // Fetch Process Name based on selected Process ID
                        string processNameQuery = "SELECT FeatureName FROM [dbo].[Feature_Master] WHERE ID = @ProcessID";
                        string processName = string.Empty;

                        using (SqlCommand cmd = new SqlCommand(processNameQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@ProcessID", processID);
                            processName = cmd.ExecuteScalar().ToString(); // Get the process name for the selected ProcessID
                        }

                        if (!string.IsNullOrEmpty(processName))
                        {
                            // Check if this user-role combination already exists in the database
                            string checkExistingQuery = "SELECT COUNT(*) FROM User_Feature_Mapping WHERE feature_id = @Role_id AND Role_id = @User_id";
                            int existingCount = 0;

                            using (SqlCommand cmd = new SqlCommand(checkExistingQuery, con))
                            {
                                cmd.Parameters.AddWithValue("@User_id", userID);
                                cmd.Parameters.AddWithValue("@Role_id", processID);
                                existingCount = (int)cmd.ExecuteScalar(); // Get count of existing records
                            }

                            // If no existing record, proceed with insertion
                            if (existingCount == 0)
                            {
                                string insertQuery = "INSERT INTO User_Feature_Mapping (Role_id, RoleName, feature_id, FeatureName, CreateDate) " +
                                                     "VALUES (@User_id, @UserName, @Role_id, @Role_Name, @CreateDate)";

                                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                                {
                                    // Add parameters to the command
                                    cmd.Parameters.AddWithValue("@Role_id", processID);
                                    cmd.Parameters.AddWithValue("@Role_Name", processName); // Insert the actual Process name
                                    cmd.Parameters.AddWithValue("@User_id", userID);
                                    cmd.Parameters.AddWithValue("@UserName", UserName);
                                    cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now); // Set current date and time

                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // Optionally, handle the case where the record already exists
                                Response.Write("<script>alert('This role is already assigned to the user.');</script>");
                            }
                        }
                    }
                }

                // Optionally: Add a success message or redirect
                Response.Write("<script>alert('Roles assigned successfully!');</script>");
            }
            else
            {
                // Show error if no process is selected
                Response.Write("<script>alert('Please select at least one process.');</script>");
            }
        }
        Response.Redirect("ListProcess", false);
    }

}