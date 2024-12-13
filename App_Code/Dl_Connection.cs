using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class Dl_Connection
{
    DL_Encrpt dec = new DL_Encrpt();
    public Dl_Connection()
    {

    }


    public SqlConnection Get_Connection(string Connection)
    {
        SqlConnection con = new SqlConnection(Connection);
        if (con.State == System.Data.ConnectionState.Open)
        {
            con.Close();
        }
        else
        {
            con.Open();
        }
        return con;
    }
    public string GetConnectionByAccount(string accountid)
    {
        string baseConnString = ConfigurationManager.ConnectionStrings["SuperDataBase"].ConnectionString;
        string query = "SELECT Account_DB_IP, Account_db_Name, Account_User_ID, Account_Password FROM AccountDetails WHERE AccountID = @AccountID";
        string connString = string.Empty;

        using (SqlConnection conn = new SqlConnection(baseConnString))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@AccountID", accountid);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string accountDbName = dec.Decrypt(reader["Account_db_Name"].ToString());
                    string accountUserId = dec.Decrypt(reader["Account_User_ID"].ToString());
                    string accountDbPassword = dec.Decrypt(reader["Account_Password"].ToString());
                    string Account_DB_IP = dec.Decrypt(reader["Account_DB_IP"].ToString());


                    connString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};",
                                              Account_DB_IP,
                                               accountDbName,
                                               accountUserId,
                                               accountDbPassword);

                }
            }
        }

        return connString;
    }
    public string getDynStrByUserID(string UserID)
    {
        string prefix = string.Empty;
        if (!string.IsNullOrEmpty(UserID) && UserID.Length >= 3)
        {
            prefix = UserID.Substring(0, 3); // Extract first 3 characters
        }
        string RncPrifix = dec.Encrypt(prefix);
        string connString = string.Empty;
        string baseConnString = ConfigurationManager.ConnectionStrings["SuperDataBase"].ConnectionString;

        string query = "SELECT Account_DB_IP, Account_db_Name, Account_User_ID, Account_Password FROM AccountDetails WHERE AccountPrefix = @Prifix";

        using (SqlConnection conn = new SqlConnection(baseConnString))
        {
            conn.Open();

            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Prifix", RncPrifix); // Use raw prefix instead of encrypted one

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string accountDbName = dec.Decrypt(reader["Account_db_Name"].ToString());
                        string accountUserId = dec.Decrypt(reader["Account_User_ID"].ToString());
                        string accountDbPassword = dec.Decrypt(reader["Account_Password"].ToString());
                        string accountDbIp = dec.Decrypt(reader["Account_DB_IP"].ToString());

                        connString = string.Format(
                            "Data Source={0};Initial Catalog={1};User ID={2};Password={3};",
                            accountDbIp,
                            accountDbName,
                            accountUserId,
                            accountDbPassword
                        );
                    }
                }
            }
        }

        return connString;
    }

    public void GetDynamicConnection(string userName)
    {
         string prefix =string.Empty;
        if (!string.IsNullOrEmpty(userName) && userName.Length >= 3)
        {
            prefix = userName.Substring(0, 3); 
        }
        string RncPrifix = dec.Encrypt(prefix);
        string baseConnString = ConfigurationManager.ConnectionStrings["SuperDataBase"].ConnectionString;
        string query = "SELECT  Account_DB_IP , Account_db_Name, Account_User_ID, Account_Password FROM AccountDetails WHERE AccountPrefix = @Prifix";
        using (SqlConnection conn = new SqlConnection(baseConnString))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Prifix", RncPrifix); 

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string accountDbName = dec.Decrypt(reader["Account_db_Name"].ToString());
                    string accountUserId = dec.Decrypt(reader["Account_User_ID"].ToString());
                    string accountDbPassword = dec.Decrypt(reader["Account_Password"].ToString());
                    string Account_DB_IP = dec.Decrypt(reader["Account_DB_IP"].ToString());



                    string connString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};",
                                                Account_DB_IP,
                                                 accountDbName,
                                                 accountUserId,
                                                 accountDbPassword);
                    UserInfo.Dnycon = connString;
                }
            }
        }

    }


}