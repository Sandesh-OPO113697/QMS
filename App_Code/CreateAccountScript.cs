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

    public void InsertAccount(string AccountName, string SignOn)
    {
        string prefix = GeneratePrefix(AccountName);
        SqlCommand cmd = new SqlCommand("Sp_CreateAccount", con.Get_Connection(UserInfo.Dnycon));
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@AccountName", drc.Encrypt( AccountName));
        cmd.Parameters.AddWithValue("@SignOn", drc.Encrypt( SignOn));
        cmd.Parameters.AddWithValue("@Prefix", drc.Encrypt( prefix));
        cmd.ExecuteNonQuery();

    }
    private string GeneratePrefix(string accountName)
    {
        string[] words = accountName.Split(new[] { ' ', '.', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);

        // Take the first letter of each word
        string prefix = string.Concat(words.Select(w => w[0]));

        // If the prefix is less than 3 characters, continue adding characters from the first word
        if (prefix.Length < 3)
        {
            string firstWord = words[0];
            prefix = firstWord.Substring(0, Math.Min(3, firstWord.Length));
            while (prefix.Length < 3)
            {
                // Add more characters from the first word if available
                int nextCharIndex = prefix.Length;
                if (nextCharIndex < firstWord.Length)
                {
                    prefix += firstWord[nextCharIndex];
                }
                else
                {
                    break; // No more characters to add
                }
            }
        }

        // Take only the first 3 characters and convert to uppercase
        return prefix.Substring(0, 3).ToUpper();
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