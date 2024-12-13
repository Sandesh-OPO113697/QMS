<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UserCreation.aspx.cs" Inherits="UserCreation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form class="AdminCreateio" runat="server">

        <div class="col-md-12 my-4">
            <div class="card shadow">
                <div class="card-body">
                    <h5 class="card-title">Welcome </h5>
                    <div class="alert alert-primary" id="NotificationDiv" runat="server" role="alert">
                        <span class="fe fe-alert-circle fe-16 mr-2" id="Notification" runat="server"></span>

                    </div>

                    <table id="SuperAdminTable" class="table table-bordered table-hover mb-0">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th >UserName</th>
                                  <th >Role</th>
                                  <th >Process</th>
                                  
                                <th >Location</th>
                                <th >Active_Status</th>
                                <th >Created_Date</th>
                                <th >Activate</th>
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach (System.Data.DataRow row in GetSuperAdminData().Rows)
                               { %>
                            <tr>
                                <td><%= row["ID"] %></td>
                                <td><%= row["username"] %></td>
                                <td><%= row["Role"] %></td>
                                <td><%= row["Process"] %></td>
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
                                <label>Sub Program Name</label>
                                <asp:DropDownList ID="drpSubProgram" runat="server"
                                    CssClass="custom-select">
                                </asp:DropDownList>
                            </div>

                            <div class="form-group mb-2">
                                <label>Password</label>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                            </div>
                            <div class="form-group mb-2">
                                <label>User Name</label>
                                <asp:TextBox ID="name" runat="server" CssClass="form-control" />
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
                                <label>User ID</label>
                                <asp:TextBox ID="txtusername" runat="server" CssClass="form-control" oninput="restrictBackspace(event)" onkeydown="preventDeleteBackspace(event)" />
                            </div>

                            <div class="form-group mb-2">
                                <label>Role Name</label>
                                <asp:DropDownList ID="drpRole" runat="server"
                                    CssClass="custom-select">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group mb-2">
                                <label>Phone Number</label>
                                <asp:TextBox ID="Phone" runat="server" CssClass="form-control" TextMode="Number" oninput="validatePhone(this)" />
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
    <script type="text/javascript">
        function restrictBackspace(event) {
            event.preventDefault();
            var textbox = document.getElementById('<%= txtusername.ClientID %>');
            var length = textbox.value.length;
            if (length < 4) {
                $.ajax({
                    type: "POST",
                    url: 'UserCreation.aspx/HandleBackspace',  // URL of the method in the Page's code-behind
                    data: JSON.stringify({ message: 'Backspace key was pressed' }), // Data to send
                    contentType: "application/json; charset=utf-8",  // Set content type for JSON
                    dataType: "json",  // Expect JSON response
                    success: function(response) {
                        var newUsername = response.d; 
                
                        document.getElementById('<%= txtusername.ClientID %>').value = newUsername;
                    },
                    error: function(xhr, status, error) {
                        console.error('Error: ' + error);
                    }
                });

            }            
        }

        function validatePhone(input) {
            // Remove non-digit characters
            input.value = input.value.replace(/\D/g, '');
        
            // If input length exceeds 10 digits, limit it and show an alert
            if (input.value.length > 10) {
                input.value = input.value.substring(0, 10);
                alert("Phone number can only be 10 digits.");
            }
        }
    </script>

</asp:Content>
