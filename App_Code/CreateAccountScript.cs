using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

public class CreateAccountScript
{
    DL_Encrpt drc = new DL_Encrpt();
    Dl_Connection con = new Dl_Connection();
    public CreateAccountScript()
	{
		
	}

    public void InsertAccount(string AccountName, string SignOn , string prifix)
    {
        
        SqlCommand cmd = new SqlCommand("Sp_CreateAccount", con.Get_Connection(UserInfo.Dnycon));
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@AccountName", drc.Encrypt( AccountName));
        cmd.Parameters.AddWithValue("@SignOn", drc.Encrypt( SignOn));
        cmd.Parameters.AddWithValue("@Prefix", prifix);
        cmd.ExecuteNonQuery();

    }
   
    public void CreateAccountByScript(string AccountName)
    {
        try
        {
            string createDatabaseQuery =
                "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '" + AccountName + "') CREATE DATABASE " + AccountName + "";
            string sqlScript = File.ReadAllText(@"D:\TestDBQMS.sql");
            using (SqlConnection conn = new SqlConnection(UserInfo.Dnycon))
            {
                conn.Open();
                ExecuteQuery(conn, createDatabaseQuery);
                ExecuteQuery(conn, "USE " + AccountName + "; " + sqlScript);
            }
        }
        catch (Exception ex)
        {  
        }
    }
    private void ExecuteQuery(SqlConnection connection, string query)
    {
        using (SqlCommand cmd = new SqlCommand(query, connection))
        {
            cmd.ExecuteNonQuery();
        }
    }

}