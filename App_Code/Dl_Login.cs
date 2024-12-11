using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;


public class Dl_Login
{
    DL_Encrpt drc = new DL_Encrpt();
    Dl_Connection con = new Dl_Connection();
    public Dl_Login()
    {

    }
    public int CheckUserValid(string UserName, string Password, string LogInCOn)
    {
        try
        {
            using (SqlConnection cc = con.Get_Connection(LogInCOn))
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Check_LogIn", cc);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Empcode", drc.Encrypt(UserName));
                cmd.Parameters.AddWithValue("@Password", drc.Encrypt(Password));
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    UserInfo.UserName = drc.Decrypt(dt.Rows[0]["username"].ToString());
                    UserInfo.UserType = drc.Decrypt(dt.Rows[0]["usertype"].ToString());
                    UserInfo.IsActive = dt.Rows[0]["isactive"].ToString();
                    UserInfo.LocationID = dt.Rows[0]["Location"].ToString();
                    UserInfo.UserType = "SuperAdmin";
                    UserInfo.Dnycon = LogInCOn;
                    return 1;
                }
                else
                {
                    UserInfo.UserType = "Normal";
                    return 0;
                }
            }
        }
        catch (SqlException sqlEx)
        {
            return 0;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
    public void AcessUserLevel(string userName)
    {
        // Connection string (update with your own connection details)
        string connectionString = UserInfo.Dnycon;


        string query = @"
                SELECT 
                    upm.Proram_id as Program_id,
                    upm.ProgramName as ProgramName,
                    upm.Sub_ProgramId as Sub_ProgramId,
                    (SELECT SubProcessName FROM [dbo].[Eval_SubProcess] WHERE id = upm.Sub_ProgramId) AS SubProcessName,
                    upm.Userid as Userid,
                    urm.Role_Name AS UserRoleName,
                    ufm.FeatureName as FeatureName
                FROM 
                    [dbo].[User_Program_Mapping] upm
                JOIN 
                    [dbo].[User_Role_Mapping] urm 
                    ON upm.Userid = urm.User_Id
                LEFT JOIN 
                    [dbo].[User_Feature_Mapping] ufm
                    ON urm.Role_Name = ufm.RoleName 
                WHERE 
                    urm.UserName = @UserName
                GROUP BY 
                    upm.Proram_id, 
                    upm.ProgramName, 
                    upm.Userid, 
                    urm.Role_Name, 
                    upm.Sub_ProgramId, 
                    ufm.FeatureName";


        DataTable dt = new DataTable();

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create a SQL command
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add the parameter to the command
                    command.Parameters.AddWithValue("@UserName", userName);

                    // Open the connection
                    connection.Open();

                    // Execute the query and load the result into the DataTable
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
            }



            var distinctProcesses = dt.AsEnumerable()
                                                    .GroupBy(row => new
                                                    {
                                                        ProcessId = row["Program_id"],
                                                        ProcessName = row["ProgramName"],
                                                        SubProcessId = row["Sub_ProgramId"],
                                                        SubProcessName = row["SubProcessName"],
                                                        UserId = row["Userid"],
                                                        UserRoleName = row["UserRoleName"]
                                                    })
                                                    .Select(g => new
                                                    {
                                                        ProcessId = g.Key.ProcessId,
                                                        ProcessName = g.Key.ProcessName,
                                                        SubProcessId = g.Key.SubProcessId,
                                                        SubProcessName = g.Key.SubProcessName,
                                                        UserId = g.Key.UserId,
                                                        UserRoleName = g.Key.UserRoleName
                                                    }).ToList();


            List<string> distinctFeatureNames = dt.AsEnumerable()
                .Select(row => row["FeatureName"].ToString())
                .Distinct()
                .ToList();

            if (distinctProcesses.Any())
            {
                var firstProcess = distinctProcesses.First(); // Gets the first distinct process entry

                UserAcesslevel.Proram_id = firstProcess.ProcessId.ToString();
                UserAcesslevel.ProgramName = firstProcess.ProcessName.ToString();
                UserAcesslevel.Sub_ProgramId = firstProcess.SubProcessId.ToString();
                UserAcesslevel.SubProcessName = firstProcess.SubProcessName.ToString();
                UserAcesslevel.Userid = firstProcess.UserId.ToString();
                UserAcesslevel.UserRoleName = firstProcess.UserRoleName.ToString();
            }
            UserAcesslevel.DistinctFeatureNames = distinctFeatureNames;


        }
        catch (Exception ex)
        {
            // Handle any errors that may occur
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    public int CheckUserAccountUser(string UserName, string Password, string LogInCOn)
    {
        try
        {
            using (SqlConnection cc = con.Get_Connection(LogInCOn))
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Sp_Check_LogIn", cc);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Empcode", drc.Encrypt(UserName));
                cmd.Parameters.AddWithValue("@Password", drc.Encrypt(Password));
                SqlDataAdapter adpt = new SqlDataAdapter(cmd);
                adpt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    UserInfo.UserName = drc.Decrypt(dt.Rows[0]["Name"].ToString());
                    UserInfo.UserType = dt.Rows[0]["usertype"].ToString();
                    UserInfo.IsActive = dt.Rows[0]["isactive"].ToString();
                    UserInfo.LocationID = dt.Rows[0]["Location"].ToString();
                    UserInfo.AccountID = dt.Rows[0]["Account_id"].ToString();
                    

                    UserInfo.Dnycon = LogInCOn;
                    return 1;
                }
                else
                {
                    UserInfo.UserType = "failed";
                    return 0;
                }
            }
        }
        catch (SqlException sqlEx)
        {
            return 0;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
}