<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateAccount.aspx.cs" Inherits="CreateAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <form id="form11" runat="server">
        <div class="alert alert-primary" id="NotificationDiv" runat="server" role="alert">
            <span class="fe fe-alert-circle fe-16 mr-2" id="Notification" runat="server"></span>

        </div>
        <div class="col-md-12 my-4">
            <div class="card shadow">
                <div class="card-body">
                    <h5 class="card-title">Welcome SuperAdmin</h5>

                    <table id="SuperAdminTable" class="table table-bordered table-hover mb-0">
                        <thead>
                            <tr>
                                <th>AccountID</th>
                                <th>AccountName</th>
                                <th>Authantication_Type</th>
                                <th>Isactive</th>
                                <th>Create_date</th>
                                <th>Activate</th>
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach (System.Data.DataRow row in GetSuperAdminData().Rows)
                               { %>
                            <tr>
                                <td><%= row["AccountID"] %></td>
                                <td><%= row["AccountName"] %></td>
                                <td><%= row["Authantication_Type"] %></td>
                                <td><%= row["Isactive"].ToString() == "1" ? "Yes" : "No" %></td>
                                <td><%= row["Create_date"] %></td>
                                <td>
                                    <div class="custom-control custom-switch">
                                        <input type="checkbox" class="custom-control-input" id="chk_<%= row["AccountID"] %>"
                                            <%= row["Isactive"].ToString() == "1" ? "checked" : "" %>
                                            onclick="updateStatus('<%= row["AccountID"] %>    ', this.checked)" />
                                        <label class="custom-control-label" for="chk_<%= row["AccountID"] %>"></label>
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
                            <div class="form-group mb-3">
                                <label for="example-palaceholder">Enter Account Name</label>
                                <asp:TextBox ID="AccountNameTextBox" runat="server" CssClass="form-control" placeholder="Account Name" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group mb-3">
                                <label for="example-placeholder">Enter Account Prefix</label>
                                <asp:TextBox ID="AccountPrefix" runat="server" CssClass="form-control" placeholder="Enter 3 Character Prefix" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group mb-3">
                                <label for="custom-select-1">Authentication Type</label>
                                <asp:DropDownList ID="AuthenticationTypeDropDown" runat="server" CssClass="custom-select">
                                    <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Domain Authentication" Value="Tea"></asp:ListItem>
                                    <asp:ListItem Text="Normal" Value="Lunch"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-2 my-4">
            <div class="card-body d-flex justify-content-center align-items-center">

                <asp:Button ID="btnSubmit" class="btn btn-primary" runat="server" Text="Submit" OnClick="CreateAccountBySuper" />


            </div>
        </div>


        <div class="modal fade" id="userListModal" tabindex="-1" role="dialog" aria-labelledby="userListModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document" style="display: flex; justify-content: center; align-items: center; min-height: 100vh;">
                <div class="modal-content" style="width: 100%; max-width: 500px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); border-radius: 8px; padding: 20px;">
                    <div class="modal-header" style="border-bottom: 1px solid #e0e0e0;">
                        <h5 class="modal-title" id="userListModalLabel" style="font-size: 1.25rem; font-weight: bold;">Ativate - Deactivate Users</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close" style="font-size: 1.5rem;">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body" style="padding: 1rem;">
                        <div id="userListContent">
                            <!-- Dynamic User List Content will be injected here -->
                        </div>
                    </div>
                    <div class="modal-footer" style="border-top: 1px solid #e0e0e0;">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal" style="padding: 0.5rem 1rem;">Cancel</button>
                        <button type="button" class="btn btn-primary" onclick="saveUserDeactivation()" style="padding: 0.5rem 1rem;">Save Changes</button>
                    </div>
                </div>
            </div>
        </div>

    </form>

    <script>

        function updateStatus(accountId, isChecked) {
            var status = isChecked ? 1 : 0;

            saveAccountStatus(accountId, status);
            // Fetch users associated with the account
            $.ajax({
                type: "POST",
                url: "SuperAdmin/CreateAccount.aspx/GetUsersByAccount",
                data: JSON.stringify({ accountId: accountId.trim() }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var users = response.d;
                    var userListHtml = '<ul class="list-group">';
                    //users.forEach(function (user) {
                            
                    //    var isChecked = user.isactive == 1 ? 'checked' : ''; // assuming '1' means active and '0' means inactive

                    //    userListHtml += `<li class="list-group-item">
                    //                        <input type="checkbox" id="user_${user.UserID}" value="${user.UserID}" ${isChecked}>
                    //                        ${user.UserName} - <strong>${user.usertype}</strong>
                    //                    </li>`;
                    //});
                    users.forEach(function (user) {
                        // Always uncheck the checkbox by not setting the 'checked' attribute
                        userListHtml += `<li class="list-group-item">
                                            <input type="checkbox" id="user_${user.UserID}" value="${user.UserID}">
                                            ${user.UserName} - <strong>${user.usertype}</strong>
                                        </li>`;
                    });
                    userListHtml += '</ul>';
                    $('#userListContent').html(userListHtml);
                    $('#userListModal').modal('show');
                },
                error: function () {
                    alert("Error fetching user list.");
                }
            });
           
                
               
            
        }

        function saveUserDeactivation() {
           
            var activeUsers = [];
            var inactiveUsers = [];
            $('#userListContent input[type="checkbox"]').each(function () {
                var userId = $(this).val();
                if ($(this).prop('checked')) {
                    activeUsers.push(userId); 
                } else {
                    inactiveUsers.push(userId); 
                }
            });
          
            // Deactivate selected users
            $.ajax({
                type: "POST",
                url: "SuperAdmin/CreateAccount.aspx/DeactivateUsers",
                data: JSON.stringify({ activeUsers: activeUsers, inactiveUsers: inactiveUsers }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function () {
                    alert("Users' statuses updated successfully.");
                    location.reload();
                },
                error: function () {
                    alert("Error updating user statuses.");
                }
            });
        }

        function saveAccountStatus(accountId, isChecked) {
            
            var status = isChecked ? 1 : 0; 

           
            $.ajax({
                type: "POST",
                url: "SuperAdmin/CreateAccount.aspx/UpdateAccountStatus", // The server-side method
                data: JSON.stringify({ accountId: accountId, isActive: status }), // Send accountId and status
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    
                },
                error: function (xhr, status, error) {
                    alert("Error updating status.");
                }
            });
        }
        
    </script>

    <%-- <script>
       
       
       
    </script>
    --%>
</asp:Content>
