using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CreateAdmin : System.Web.UI.Page
{
    public string ConnSTR = ConfigurationManager.ConnectionStrings["SuperDataBase"].ConnectionString;
    DL_Encrpt dec = new DL_Encrpt();
    Dl_Connection dl = new Dl_Connection();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserList.Visible = false;
            UserID.Visible = false;
            btnSubmit.Visible = false;
            NotificationDiv.Visible = false;
            if (UserInfo.UserType == "Normal")
            {
                AccountDropDown.Visible = false;
            }
            if (UserInfo.UserType == "SuperAdmin")
            {
                AccountDropDown.Visible = true;
            }
            txtAdmin.Text = "____";
            BindAccountDetails();
            if (AccountDropDown.SelectedIndex == 0)
            {
                Account_Prefix_Change(AccountDropDown, EventArgs.Empty);
            }
        }
    }


    protected void ShowUser(object sender, EventArgs e)
    {
        UserID.Visible = true;
        btnSubmit.Visible = true;
    }

    [WebMethod]
    public static string UpdateUserStatus(string name, bool isActive, int id)
    {
        Dl_Connection dl = new Dl_Connection();
        DL_Encrpt dec = new DL_Encrpt();
        string DecName = dec.Encrypt(name);

        string Connn22 = dl.getDynStrByUserID(name);
        try
        {
            using (SqlConnection conn = new SqlConnection(Connn22))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE User_Master SET isactive = @isActive WHERE id = @id", conn))
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
    private DataTable DecryptUserList(DataTable dt)
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
    private void BindUserGrid(string accountId)
    {
        try
        {
            Dl_Connection dl = new Dl_Connection();
            string cooonn = dl.GetConnectionByAccount(accountId);
            using (SqlConnection cc = new SqlConnection(cooonn))
            {
                cc.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT id, Name, usertype, isactive FROM User_Master WHERE Account_id = @AccountID AND isactive = 1 and usertype='Admin'", cc))
                {
                    cmd.Parameters.AddWithValue("@AccountID", accountId);

                    DataTable dt = new DataTable();
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dt);
                    DataTable ds = DecryptUserList(dt);
                    UserList.Visible = true;

                    if (ds.Rows.Count > 0)
                    {
                        UserGrid.DataSource = ds;
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

    protected void Account_Prefix_Change(object sender, EventArgs e)
    {
        if (AccountDropDown.SelectedIndex > 0)
        {
            string accountId = AccountDropDown.SelectedValue;
            BindUserGrid(accountId);
            try
            {

                using (SqlConnection cc = new SqlConnection(ConnSTR))
                {
                    cc.Open();
                    using (SqlCommand cmd = new SqlCommand("Get_AccountDetails", cc))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mode", "GetAccountPrifix");
                        cmd.Parameters.AddWithValue("@AccountID", accountId);

                        DataTable dt = new DataTable();
                        SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                        adpt.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {

                            string accountPrefix = dec.Decrypt(dt.Rows[0]["AccountPrefix"].ToString());
                            txtAdmin.Text = accountPrefix + "_";
                        }
                        else
                        {
                            txtAdmin.Text = string.Empty;
                            // Show message (optional): no account prefix found
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
    }

    public void ClearFeilds()
    {
        AccountDropDown.SelectedIndex = 0;
        txtPassword.Text = "";
        txtAdmin.Text = "____";
    }
    protected void CreateAdminClick(object sender, EventArgs e)
    {
        if (AccountDropDown.SelectedIndex == 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please select an account.');", true);
            return;

        }
        if (txtAdmin.Text == "____")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alertMessage", "alert('Please select an account.');", true);
            return;
        }

        string password = txtPassword.Text.Trim();
        string accountId = AccountDropDown.SelectedValue;


        try
        {
            string str = dl.GetConnectionByAccount(accountId);
            using (SqlConnection cc = new SqlConnection(str))
            {
                cc.Open();
                using (SqlCommand cmd = new SqlCommand("InsertAdmin", cc))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserName", dec.Encrypt(txtAdmin.Text.ToString()));
                    if (UserInfo.UserType == "SuperAdmin")
                    {
                        cmd.Parameters.AddWithValue("@UserType", "Admin");
                    }
                    if (UserInfo.UserType == "Normal")
                    {
                        cmd.Parameters.AddWithValue("@UserType", "QE");
                    }
                    cmd.Parameters.AddWithValue("@Password", dec.Encrypt(password));
                    cmd.Parameters.AddWithValue("@AccountID", accountId);

                    int result = cmd.ExecuteNonQuery();
                    NotificationDiv.Visible = true;
                    Notification.InnerText = " User created successfully ! " + txtAdmin.Text.ToString();
                    if (result > 0)
                    {
                        Response.Write("Admin created successfully!");
                    }
                    else
                    {
                        Response.Write("Failed to create admin.");
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            // Log error and show a message to the user
            Response.Write("Database error: " + ex.Message);
        }
        ClearFeilds();
    }

    private DataTable DecryptDataTable(DataTable dt)
    {
        DL_Encrpt encrypter = new DL_Encrpt();

        foreach (DataRow row in dt.Rows)
        {
            if (row["AccountID"] != DBNull.Value)
            {
                row["AccountID"] = row["AccountID"].ToString();
            }

            if (row["AccountName"] != DBNull.Value)
            {
                row["AccountName"] = encrypter.Decrypt(row["AccountName"].ToString());
            }


        }

        return dt;
    }
    public void BindAccountDetails()
    {
        try
        {
            using (SqlConnection cc = new SqlConnection(ConnSTR))
            {
                cc.Open();
                using (SqlCommand cmd = new SqlCommand("Get_AccountDetails", cc))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "AccounDropDawon");

                    DataTable dt = new DataTable();
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dt);
                    DataTable crc = DecryptDataTable(dt);
                    if (dt.Rows.Count > 0)
                    {
                        AccountDropDown.DataSource = crc;
                        AccountDropDown.DataTextField = "AccountName";
                        AccountDropDown.DataValueField = "AccountID";
                        AccountDropDown.DataBind();
                        AccountDropDown.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", ""));
                    }
                    else
                    {
                        AccountDropDown.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Accounts Available", ""));
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
}
