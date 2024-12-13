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

public partial class UserCreation : System.Web.UI.Page
{
    DL_Encrpt drc = new DL_Encrpt();
    Dl_Connection con = new Dl_Connection();
    public string ConnSTR = ConfigurationManager.ConnectionStrings["SuperDataBase"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            NotificationDiv.Visible = false;
            txtusername.Text = "____";
            BindPrifix();
            BindLocationDetails();
            BindRoleDetails();
        }
    }
    [WebMethod]
    public static string HandleBackspace(string message)
    {
        UserCreation uc = new UserCreation();
       string Prifix= uc.ReteunProfix();
       return Prifix;

    }
    public void BindPrifix()
    {
        string accountId = UserInfo.AccountID;

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

                        string accountPrefix = drc.Decrypt(dt.Rows[0]["AccountPrefix"].ToString());
                        txtusername.Text = accountPrefix + "_";
                    }
                    else
                    {
                        txtusername.Text = string.Empty;
                        // Show message (optional): no account prefix found
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }


    public string ReteunProfix()
    {
        string accountId = UserInfo.AccountID;

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

                        string accountPrefix = drc.Decrypt(dt.Rows[0]["AccountPrefix"].ToString());

                        return accountPrefix + "_";
                       
                    }
                    else
                    {
                       
                        // Show message (optional): no account prefix found
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return"";

        }
        return "";
    }
    protected void LocationDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedLocation = LocationDropDown.SelectedValue;
        BindProcessDetails(selectedLocation);


    }


    protected void ProgramDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectedLocation = ProgramDropDown.SelectedValue;
        BindSubProcessDetails(selectedLocation);


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




    public void BindSubProcessDetails(string Process)
    {
        try
        {
            drpSubProgram.Items.Clear();
            using (SqlConnection conn = new SqlConnection(UserInfo.Dnycon))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("USP_Fill_Dropdown", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "Select_Program_Master_ProcessWise");
                    cmd.Parameters.AddWithValue("@p_username", Process);

                    DataTable dt = new DataTable();
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        drpSubProgram.DataSource = dt;
                        drpSubProgram.DataTextField = "SubProcessName";
                        drpSubProgram.DataValueField = "ID";
                        drpSubProgram.DataBind();
                        drpSubProgram.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", ""));
                    }
                    else
                    {

                        drpSubProgram.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Accounts Available", ""));

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




    public void BindRoleDetails()
    {
        try
        {

            using (SqlConnection conn = new SqlConnection(UserInfo.Dnycon))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("USP_Fill_Dropdown", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Mode", "BindRoleDetails");

                    DataTable dt = new DataTable();
                    SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                    adpt.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        drpRole.DataSource = dt;
                        drpRole.DataTextField = "Role_name";
                        drpRole.DataValueField = "Roleid";
                        drpRole.DataBind();
                        drpRole.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", ""));
                    }
                    else
                    {
                        drpRole.Items.Insert(0, new System.Web.UI.WebControls.ListItem("No Accounts Available", ""));
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



    protected void CreateUser(object sender, EventArgs e)
    {
        try
        {
            string Location = LocationDropDown.SelectedValue.ToString();
            string UserName = drc.Encrypt(txtusername.Text.ToString());
            string Program = ProgramDropDown.SelectedValue.ToString(); // Rename variable to avoid conflict
            string SubProgram = drpSubProgram.SelectedValue.ToString();
            string Password = drc.Encrypt(txtPassword.Text.ToString());
            string Rolename = drpRole.SelectedValue.ToString();

            string UName = drc.Encrypt(name.Text.ToString());

            if (LocationDropDown.SelectedIndex > 0 && ProgramDropDown.SelectedIndex > 0 && txtusername.Text != "" && txtusername.Text != null && txtPassword.Text != "" && txtPassword.Text != null && drpRole.SelectedIndex>0)
            {
                using (SqlConnection conn = new SqlConnection(UserInfo.Dnycon))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("CreateUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Location", Location);
                        cmd.Parameters.AddWithValue("@UserName", UserName); // Add parameter for UserName
                        cmd.Parameters.AddWithValue("@Program", Program); // Add parameter for selectedProgram
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@SubProcesname", SubProgram);// Add parameter for Password
                        cmd.Parameters.AddWithValue("@Role", Rolename);
                        cmd.Parameters.AddWithValue("@AccountID", UserInfo.AccountID);
                        cmd.Parameters.AddWithValue("@UserNamedrp", txtusername.Text.ToString());
                        cmd.Parameters.AddWithValue("@Name", UName);
                        cmd.Parameters.AddWithValue("@Phone", Phone.Text.ToString());
                        cmd.Parameters.AddWithValue("@Procesname", drpSubProgram.SelectedItem.Text.ToString());
                        cmd.Parameters.AddWithValue("@CreateBy", UserInfo.UserName);
                        cmd.ExecuteNonQuery();



                    }
                }
                ClearFeilds();
                NotificationDiv.Visible = true;
                Notification.InnerText = " User Is Created  ! " + txtusername.Text.ToString();

                
            }
            else
            {
                NotificationDiv.Visible = true;
                Notification.InnerText = " Select All feilds  ! " ;
            }
            

            //Response.Redirect("UserCreation", false);
        }
        catch (SqlException ex)
        {

        }
    }


    public void ClearFeilds()
    {

        LocationDropDown.SelectedIndex =0 ;
        ProgramDropDown.SelectedIndex =0;
        txtusername.Text = "" ;
        txtPassword.Text = "" ;
        drpRole.SelectedIndex = 0;
        drpSubProgram.SelectedIndex = 0;
        Phone.Text = "";
        name.Text = "";
    }
    public DataTable GetSuperAdminData()
    {
        string query = "SELECT UM.ID,UM.username,UM.usertype  as Role,(SELECT Process FROM Eval_Process WHERE ID = UM.Program_id) AS Process,LM.Location, UM.Created_Date, UM.isactive AS Active_Status FROM User_Master UM LEFT JOIN LocationMaster LM ON UM.Location = LM.ID where UM.CreateBy='"+UserInfo.UserName+"' ;";
        DataTable dt = GetData(query);
        if (dt.Rows.Count > 0)
        {
            DataTable decryptedData2 = DecryptDataTable(dt);
            return decryptedData2;
        }

        return dt;

    }

    private DataTable DecryptDataTable(DataTable dt)
    {
        DL_Encrpt encrypter = new DL_Encrpt();

        foreach (DataRow row in dt.Rows)
        {


            if (row["username"] != DBNull.Value)
            {
                row["username"] = encrypter.Decrypt(row["username"].ToString());
            }


        }

        return dt;
    }

    private DataTable GetData(string query)
    {
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(UserInfo.Dnycon))
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
}