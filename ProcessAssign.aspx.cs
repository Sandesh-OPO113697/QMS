using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RoleAssign : System.Web.UI.Page
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
        string processQuery = @"
                SELECT 
                p.ID AS ProcessID, 
                    sp.id AS SubProcessID, 
                    p.Process AS ProcessName, 
                    sp.SubProcessName
                FROM Eval_Process p
                LEFT JOIN Eval_SubProcess sp 
                    ON p.ID = sp.processid
                WHERE p.Active_Status = 1;";
        DataTable dtProcess = GetData(processQuery);
        if (dtProcess.Rows.Count > 0)
        {
            CheckBoxList1.Items.Clear();
            //foreach (DataRow row in dtProcess.Rows)
            //{
            //    ListItem item = new ListItem(row["ProcessName"].ToString() + "    --    " + row["SubProcessName"].ToString(), row["ProcessID"].ToString());
            //    CheckBoxList1.Items.Add(item);
            //}
            foreach (DataRow row in dtProcess.Rows)
            {
                string processID = row["ProcessID"].ToString();
                string subProcessID = row["SubProcessID"].ToString();
                string processName = row["ProcessName"].ToString();
                string subProcessName = row["SubProcessName"].ToString();

                // Concatenate strings
                string displayText = processName + " -- " + subProcessName;
                string value = processID + "-" + subProcessID;

                ListItem item = new ListItem(displayText, value);
                CheckBoxList1.Items.Add(item);
            }

        }

        string userQuery = "SELECT   ID, Name FROM [dbo].[User_Master] where isactive = 1";
        DataTable dtUsers = GetData(userQuery);
        DataTable dt = DecryptDataTable(dtUsers);
        if (dt.Rows.Count > 0)
        {
            users.DataSource = dt;
            users.DataTextField = "Name";
            users.DataValueField = "ID";
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
    //protected void CreateAccountBySuper(object sender, EventArgs e)
    //{
    //    // Validation
    //    if (Page.IsValid)
    //    {
    //        string userID = users.SelectedValue;
    //        string UserName = users.SelectedItem.Text;

    //        // Manually loop through the CheckBoxList items to get selected processes
    //        List<string> selectedProcesses = new List<string>();
    //        foreach (ListItem item in CheckBoxList1.Items)
    //        {
    //            if (item.Selected)
    //            {
    //                selectedProcesses.Add(item.Value); // Add selected process ID to the list
    //            }
    //        }

    //        // If processes are selected
    //        if (selectedProcesses.Count > 0)
    //        {
    //            using (SqlConnection con = new SqlConnection(connectionString))
    //            {
    //                con.Open();
    //                foreach (string processID in selectedProcesses)
    //                {
    //                    // Fetch Process Name based on selected Process ID
    //                    string processNameQuery = "SELECT Process FROM [dbo].[Eval_Process] WHERE ID = @ProcessID";
    //                    string processName = string.Empty;

    //                    using (SqlCommand cmd = new SqlCommand(processNameQuery, con))
    //                    {
    //                        cmd.Parameters.AddWithValue("@ProcessID", processID);
    //                        processName = cmd.ExecuteScalar().ToString(); // Get the process name for the selected ProcessID
    //                    }

    //                    if (!string.IsNullOrEmpty(processName))
    //                    {
    //                        // Check if the record already exists in User_Program_Mapping table
    //                        string checkExistenceQuery = "SELECT COUNT(*) FROM User_Program_Mapping WHERE Userid = @UserID AND Proram_id = @ProcessID";
    //                        int existingCount = 0;

    //                        using (SqlCommand cmd = new SqlCommand(checkExistenceQuery, con))
    //                        {
    //                            cmd.Parameters.AddWithValue("@UserID", userID);
    //                            cmd.Parameters.AddWithValue("@ProcessID", processID);
    //                            existingCount = Convert.ToInt32(cmd.ExecuteScalar());
    //                        }

    //                        // If the record does not exist, insert it
    //                        if (existingCount == 0)
    //                        {
    //                            string insertQuery = "INSERT INTO User_Program_Mapping (Proram_id, ProgramName, Userid, UserName, CreateDate) " +
    //                                                 "VALUES (@ProcessID, @ProgramName, @UserID, @UserName, @CreateDate)";

    //                            using (SqlCommand cmd = new SqlCommand(insertQuery, con))
    //                            {
    //                                // Add parameters to the command
    //                                cmd.Parameters.AddWithValue("@ProcessID", processID);
    //                                cmd.Parameters.AddWithValue("@ProgramName", processName); // Insert the actual Process name
    //                                cmd.Parameters.AddWithValue("@UserID", userID);
    //                                cmd.Parameters.AddWithValue("@UserName", UserName);
    //                                cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now); // Set current date and time

    //                                cmd.ExecuteNonQuery();
    //                            }
    //                        }
    //                    }
    //                }
    //            }

    //            // Optionally: Add a success message or redirect
    //            Response.Write("<script>alert('Roles assigned successfully!');</script>");
    //        }
    //        else
    //        {
    //            // Show error if no process is selected
    //            Response.Write("<script>alert('Please select at least one process.');</script>");
    //        }
    //    }
    //    Response.Redirect("ListProcess", false);
    //}
    protected void CreateAccountBySuper(object sender, EventArgs e)
{
    if (Page.IsValid)
    {
        string userID = users.SelectedValue;
        string userName = users.SelectedItem.Text;

        List<string> selectedProcesses = new List<string>();
        foreach (ListItem item in CheckBoxList1.Items)
        {
            if (item.Selected)
            {
                selectedProcesses.Add(item.Value); // Add selected process-subprocess pair
            }
        }

        if (selectedProcesses.Count > 0)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                foreach (string selectedProcess in selectedProcesses)
                {
                    // Split to get ProcessID and SubProcessID
                    string[] ids = selectedProcess.Split('-');
                    string processID = ids[0];
                    string subProcessID = ids[1];

                    // Fetch Process Name
                    string processNameQuery = "SELECT Process FROM [dbo].[Eval_Process] WHERE ID = @ProcessID";
                    string processName = string.Empty;
                    using (SqlCommand cmd = new SqlCommand(processNameQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@ProcessID", processID);
                        processName = cmd.ExecuteScalar().ToString();
                    }

                    if (!string.IsNullOrEmpty(processName))
                    {
                        // Check for existence in the User_Program_Mapping table
                        string checkExistenceQuery = "SELECT COUNT(*) FROM User_Program_Mapping WHERE Userid = @UserID AND Proram_id = @ProcessID AND Sub_ProgramId = @SubProcessID";
                        int existingCount = 0;
                        using (SqlCommand cmd = new SqlCommand(checkExistenceQuery, con))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userID);
                            cmd.Parameters.AddWithValue("@ProcessID", processID);
                            cmd.Parameters.AddWithValue("@SubProcessID", subProcessID);
                            existingCount = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        // If not exists, insert into table
                        if (existingCount == 0)
                        {
                            string insertQuery = "INSERT INTO User_Program_Mapping (Proram_id, ProgramName, Sub_ProgramId, Userid, UserName, CreateDate) VALUES (@ProcessID, @ProgramName, @SubProcessID, @UserID, @UserName, @CreateDate)";
                            using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                            {
                                cmd.Parameters.AddWithValue("@ProcessID", processID);
                                cmd.Parameters.AddWithValue("@ProgramName", processName);
                                cmd.Parameters.AddWithValue("@SubProcessID", subProcessID);
                                cmd.Parameters.AddWithValue("@UserID", userID);
                                cmd.Parameters.AddWithValue("@UserName", userName);
                                cmd.Parameters.AddWithValue("@CreateDate", DateTime.Now);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            // Show success message or redirect
            Response.Write("<script>alert('Roles and SubProcesses assigned successfully!');</script>");
        }
        else
        {
            Response.Write("<script>alert('Please select at least one process.');</script>");
        }
    }
    Response.Redirect("ListProcess", false);
}


}
