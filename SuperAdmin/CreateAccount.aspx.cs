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

public partial class CreateAccount : System.Web.UI.Page
{
    DL_Encrpt drc = new DL_Encrpt();
    Dl_Connection dcn = new Dl_Connection();
    CreateAccountScript ac = new CreateAccountScript();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public void ClearFeilds()
    {
        AccountNameTextBox.Text = "";
        AuthenticationTypeDropDown.SelectedIndex = 0;
    }

    [WebMethod]
    public static void UpdateAccountStatus(string accountId, int isActive)
    {

        Dl_Connection dcn = new Dl_Connection();

        using (SqlConnection con = new SqlConnection(UserInfo.Dnycon))
        {
            string query = "UPDATE AccountDetails SET Isactive = @IsActive WHERE AccountID = @AccountID";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@AccountID", accountId);
            cmd.Parameters.AddWithValue("@IsActive", isActive);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }


    public DataTable GetSuperAdminData()
    {
        string query = "SELECT AccountID, AccountName, Authantication_Type, Isactive, Create_date FROM AccountDetails";
        DataTable dt= GetData(query);
        if (dt.Rows.Count>0)
        {
            DataTable decryptedData2 = DecryptDataTable(dt);
            return decryptedData2;
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
            if (row["Authantication_Type"] != DBNull.Value)
            {
                row["Authantication_Type"] = encrypter.Decrypt(row["Authantication_Type"].ToString());
            }
            if (row["Isactive"] != DBNull.Value)
            {
                row["Isactive"] = row["Isactive"].ToString();
            }
            if (row["Create_date"] != DBNull.Value)
            {
                row["Create_date"] = row["Create_date"].ToString();
            }
           
        }

        return dt;
    }


    public  DataTable DecryptDataEmployee(DataTable dt)
    {
        DL_Encrpt encrypter = new DL_Encrpt();

        foreach (DataRow row in dt.Rows)
        {
            if (row["EMPID"] != DBNull.Value)
            {
                row["EMPID"] = row["EMPID"].ToString();
            }

            if (row["Name"] != DBNull.Value)
            {
                row["Name"] = encrypter.Decrypt(row["Name"].ToString());
            }
        

        }

        return dt;
    }

    protected void CreateAccountBySuper(object sender, EventArgs e)
    {
        string AccountName = AccountNameTextBox.Text.ToString();
        string SignOn = AuthenticationTypeDropDown.SelectedItem.Text.ToString();
        if (AuthenticationTypeDropDown.SelectedIndex > 0)
        {
            CreateAccountScr(AccountName, SignOn);
        }
        else
        {

        }
        ClearFeilds();

    }


    [WebMethod]
    public static object GetUsersByAccount(string accountId)
    {
        CreateAccount ac = new CreateAccount();
        Dl_Connection cl = new Dl_Connection();
        string connString = cl.GetConnectionByAccount(accountId);
        using (SqlConnection conn = new SqlConnection(connString))
        {
            conn.Open();
            string query2 = "SELECT Name, Username, EMPID, Account_id,usertype , isactive FROM User_Master WHERE Account_id = @AccountId";
            SqlCommand cmd = new SqlCommand(query2, conn);
            cmd.Parameters.AddWithValue("@AccountId", accountId);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            DataTable dtt = ac.DecryptDataEmployee(dt);
            var users = new System.Collections.Generic.List<object>();

            foreach (DataRow row in dtt.Rows)
            {
                users.Add(new
                {
                    UserID =  row["EMPID"],
                    UserName = row["Name"],
                    AccountID = row["Account_id"],
                    usertype = row["usertype"],
                    isactive = row["isactive"]

                });
            }

           
            return users;
        }
    }

    [WebMethod]
    public static void DeactivateUsers(System.Collections.Generic.List<string> activeUsers, System.Collections.Generic.List<string> inactiveUsers)
    {
        Dl_Connection cl = new Dl_Connection();
        DL_Encrpt drc = new DL_Encrpt();
        string DynSTR = string.Empty;
        string firstActiveUser = activeUsers != null && activeUsers.Count > 0 ? activeUsers[0] : "No active user";
        string firstInactiveUser = inactiveUsers != null && inactiveUsers.Count > 0 ? inactiveUsers[0] : "No inactive user";

        if ( firstInactiveUser != "No inactive user")
        {
            string UserID = drc.Decrypt(firstInactiveUser);
            DynSTR = cl.getDynStrByUserID(UserID);

            
        }
        if ((firstActiveUser != "No active user") )
        {
            string UserID = drc.Decrypt(firstActiveUser);
            DynSTR = cl.getDynStrByUserID(UserID);
        }

        using (SqlConnection conn = new SqlConnection(DynSTR))
        {
            conn.Open();
            if (activeUsers.Count > 0)
            {
                string queryActivate = "UPDATE User_Master SET IsActive = 1 WHERE EMPID = @EmpId";
                foreach (var empId in activeUsers)
                {
                    SqlCommand cmd = new SqlCommand(queryActivate, conn);
                    cmd.Parameters.AddWithValue("@EmpId", empId);
                    cmd.ExecuteNonQuery();
                }
            }

            if (inactiveUsers.Count > 0)
            {
                string queryDeactivate = "UPDATE User_Master SET IsActive = 0 WHERE EMPID = @EmpId";
                foreach (var empId in inactiveUsers)
                {
                    SqlCommand cmd = new SqlCommand(queryDeactivate, conn);
                    cmd.Parameters.AddWithValue("@EmpId", empId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    

    public class User
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
    }


    public void CreateAccountScr(string AccountName, string SignOn)
    {
        ac.InsertAccount(AccountName, SignOn);
        ac.CreateAccountByScript(AccountName);
    }

}