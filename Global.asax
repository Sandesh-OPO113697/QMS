<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {

        System.Web.Routing.RouteTable.Routes.MapPageRoute("LogInRossdsdute", "", "~/LogIn.aspx");
        System.Web.Routing.RouteTable.Routes.MapPageRoute("LogInRoute", "LogIn", "~/LogIn.aspx");
        System.Web.Routing.RouteTable.Routes.MapPageRoute("LogInRoutecfg", "QMS", "~/SuperAdmin/QMS.aspx");
        System.Web.Routing.RouteTable.Routes.MapPageRoute("Programpage", "Program", "~/Process/Program.aspx");
        System.Web.Routing.RouteTable.Routes.MapPageRoute("CreateAccount", "Account", "~/SuperAdmin/CreateAccount.aspx");
        System.Web.Routing.RouteTable.Routes.MapPageRoute("CreateAdmin", "AdminUser", "~/SuperAdmin/CreateAdmin.aspx");
        System.Web.Routing.RouteTable.Routes.MapPageRoute("CreateProcess", "ListProcess", "~/Process/ProcessList.aspx");
        
    }
    
    void Application_End(object sender, EventArgs e) 
    {
     

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
       

    }

    void Session_Start(object sender, EventArgs e) 
    {
       

    }

    void Session_End(object sender, EventArgs e) 
    {
       

    }
       
</script>
