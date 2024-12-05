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
                    UserInfo.UserName = drc.Decrypt( dt.Rows[0]["username"].ToString());
                    UserInfo.UserType =  drc.Decrypt( dt.Rows[0]["usertype"].ToString());
                    UserInfo.IsActive =  dt.Rows[0]["isactive"].ToString();
                    UserInfo.LocationID =   dt.Rows[0]["Location"].ToString();
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