
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserCreation.aspx.cs" Inherits="UserCreation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
      <form class="AdminCreateio" runat="server">

         <div class="col-md-12 my-4">
            <div class="card shadow">
                <div class="card-body">
                    <h5 class="card-title">Welcome SuperAdmin</h5>
                   
                     <table id="SuperAdminTable" class="table table-bordered table-hover mb-0">
                        <thead>
                            <tr>
                               <th >ID</th>
                                <th style="color: black;">UserName</th>
                                <th style="color: black;">Location</th>
                                <th style="color: black;">Active_Status</th>
                                <th style="color: black;">Created_Date</th>
                                <th style="color: black;">Activate</th>
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach (System.Data.DataRow row in GetSuperAdminData().Rows) { %>
                                <tr>
                                    <td><%= row["ID"] %></td>
                                    <td><%= row["username"] %></td>
                                    <td><%= row["Location"] %></td>
                                    <td><%= row["Active_Status"].ToString() == "1" ? "Yes" : "No" %></td>
                                    <td><%= row["Created_Date"] %></td>
                                    <td>
                                         <div class="custom-control custom-switch">
                                        <input type="checkbox" class="custom-control-input" id="chk_<%= row["ID"] %>" 
                                            <%= row["Active_Status"].ToString() == "1" ? "checked" : "" %> 
                                            onclick="updateStatus('<%= row["ID"] %>    ', this.checked)" />
                                        <label class="custom-control-label" for="chk_<%= row["ID"] %>"></label>
                                    </div>
                                    </td>
                                </tr>
                            <% } %>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
        <div class="col-md-12 my-4">
            <div class="card shadow">
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            

                            <div class="form-group mb-2">
                                <label>Location Name</label>
                                <asp:DropDownList ID="LocationDropDown" runat="server" 
                                     OnSelectedIndexChanged="LocationDropDown_SelectedIndexChanged"
                                    AutoPostBack="true" 
                                    CssClass="custom-select">
                                </asp:DropDownList>
                            </div>


                             <div class="form-group mb-2">
                                <label>SubProgram Name</label>
                                <asp:DropDownList ID="drpSubProgram" runat="server" 
                                    
                                 
                                    CssClass="custom-select">
                                </asp:DropDownList>
                            </div>

                              <div class="form-group mb-2">
                                <label>Password</label>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                            </div>


                         

                        </div>
                        <div class="col-md-6">
                              <div class="form-group mb-2">
                                <label>Program Name</label>
                                <asp:DropDownList ID="ProgramDropDown" runat="server" 
                                        OnSelectedIndexChanged="ProgramDropDown_SelectedIndexChanged"
                                      AutoPostBack="true" 
                                    CssClass="custom-select">
                                </asp:DropDownList>
                            </div>

                               <div class="form-group mb-2">
                                <label>User Name</label>
                                <asp:TextBox ID="txtusername" runat="server" CssClass="form-control" oninput="restrictBackspace(event)"  onkeydown="preventDeleteBackspace(event)"  />
                            </div>

                               <div class="form-group mb-2">
                                <label>Role Name</label>
                                <asp:DropDownList ID="drpRole" runat="server" 
                                    
                                 
                                    CssClass="custom-select">
                                </asp:DropDownList>
                            </div>

                           
                        </div>
                       

                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-2 my-4">
            <div class="card-body d-flex justify-content-center align-items-center">
                <asp:Button ID="btnSubmit" OnClick="CreateUser" CssClass="btn btn-primary" runat="server" Text="Create User*" />
            </div>
        </div>
    </form>

   
</asp:Content>
