using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProcessList : System.Web.UI.Page
{
    DL_Encrpt drc = new DL_Encrpt();
    Dl_Connection dcn = new Dl_Connection();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
           if(UserInfo.UserType=="Admin")
           {
               Process2.Visible = true;

           }
            else
           {
               Process2.Visible = false;
           }
           if (UserInfo.UserType != "Admin")
           {
               ProcessAccountUser.Visible = true;

           }
           else
           {
               ProcessAccountUser.Visible = false;
           }
        }

    }
    [WebMethod]
    public static string UpdateProcessName(int id, string processName)
    {
        using (SqlConnection con = new SqlConnection(UserInfo.Dnycon))
        {
            string query = "UPDATE Eval_Process SET Process = @processName WHERE ID = @id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@processName", processName);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        return "Process name updated successfully.";
    }



    [WebMethod]
    public static string UpdateActiveStatus(int id, bool isActive)
    {

        string status = isActive ? "1" : "0";

        using (SqlConnection con = new SqlConnection(UserInfo.Dnycon))
        {
            string query = "UPDATE Eval_Process SET Active_Status = @status WHERE ID = @id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@status", status);  // Pass 1 or 0 based on isActive
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Return status as string ("Active" or "Inactive")
        return isActive ? "Active" : "Inactive";
    }

    public DataTable GetProcessListData()
    {
        string query = "SELECT e.ID, e.Process, l.Location,e.Created_Date, e.Active_Status FROM Eval_Process e JOIN LocationMaster l ON e.Location_ID = l.ID where e.CreateBy='"+UserInfo.UserName+"';";
        DataTable dt = GetData(query);
      
        return dt;

    }

    public DataTable GetProcessAccountUser()
    {
        string query = "SELECT up.ProgramName,ep.SubProcessName,up.UserName,up.userid  FROM [dbo].[User_Program_Mapping] up JOIN [Eval_SubProcess] ep ON up.Proram_id = ep.ProcessId where up.UserName='"+UserInfo.UserName+"';";
        DataTable dt = GetData(query);

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