using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class QMS : System.Web.UI.Page
{
    Dl_Connection con = new Dl_Connection();
    public string LogInCOn = ConfigurationManager.ConnectionStrings["SuperDataBase"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (UserInfo.UserType == "SuperAdmin")
        {

            
            Admin.Visible = false;
            QE.Visible = false;
            SuperAdmin.Visible = true;


        }


    }

    [WebMethod]
    public static void UpdateAccountStatus(string accountId, int isActive)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SuperDataBase"].ConnectionString;

        using (SqlConnection con = new SqlConnection(connectionString))
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
            
            if (row["Create_date"] != DBNull.Value)
            {
                row["Create_date"] = row["Create_date"].ToString();
            }

        }

        return dt;
    }

    public DataTable GetSuperAdminData()
    {
        string query = "SELECT AccountID, AccountName, Authantication_Type, Isactive, Create_date FROM AccountDetails";
        DataTable dt = GetData(query);
        if (dt.Rows.Count > 0)
        {
            DataTable decryptedData2 = DecryptDataTable(dt);
            return decryptedData2;
        }

        return dt;
        return GetData(query);
    }

    private DataTable GetData(string query)
    {
        DataTable dt = new DataTable();
        using (SqlConnection con = new SqlConnection(LogInCOn))
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
    public void BindsUperAdmin()
    {

        DataTable dt = new DataTable();
        string query = "select AccountID	,AccountName,	Authantication_Type,	Isactive,	Create_date from AccountDetails";
        SqlCommand cmd = new SqlCommand(query, con.Get_Connection(LogInCOn));
        SqlDataAdapter adpt = new SqlDataAdapter(cmd);
        adpt.Fill(dt);
       
    }

   


}