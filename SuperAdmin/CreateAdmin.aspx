<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateAdmin.aspx.cs" Inherits="CreateAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <form class="AdminCreateio" runat="server">
         <div class="alert alert-primary" id="NotificationDiv"  runat="server"  role="alert">
          <span class="fe fe-alert-circle fe-16 mr-2" id="Notification" runat="server"  ></span> 

         </div>
        <div class="col-md-12 my-4">
            <div class="card shadow">
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group mb-3">
                                <label>Account Name</label>
                                <asp:DropDownList ID="AccountDropDown" runat="server"
                                    OnSelectedIndexChanged="Account_Prefix_Change"
                                    AutoPostBack="true"
                                    CssClass="custom-select">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-6">

                            <div class="card-body d-flex justify-content-center align-items-center">
                                <asp:Button ID="UserButton" CssClass="btn btn-primary" runat="server" Text="Add user" OnClick="ShowUser" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-12 my-4">
            <div class="card shadow">
                <div class="card-body" id="UserList" runat="server">
                    <asp:GridView ID="UserGrid" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="ID" />
                            <asp:BoundField DataField="Name" HeaderText="Name" />
                            <asp:BoundField DataField="usertype" HeaderText="User Type" />


                            <asp:TemplateField HeaderText="Active Status">
                                <ItemTemplate>

                                    <div class="custom-control custom-switch">
                                        <input type="checkbox" class="custom-control-input" id="chk_<%# Eval("id") %>"
                                            <%# Convert.ToBoolean(Eval("isactive")) ? "checked" : "" %>
                                            onclick="updateStatus('<%# Eval("Name") %>    ', '<%# Eval("id") %>    ', this.checked)" />
                                        <label class="custom-control-label" for="chk_<%# Eval("id") %>"></label>
                                    </div>

                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="col-md-12 my-4">
            <div class="card shadow">
                <div class="card-body" id="UserID" runat="server">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group mb-3">
                                <label>User Name</label>
                                <asp:TextBox ID="txtAdmin" runat="server" CssClass="form-control" oninput="restrictBackspace(event)" onkeydown="preventDeleteBackspace(event)" />
                            </div>


                        </div>
                        <div class="col-md-6">
                            <div class="form-group mb-3">
                                <label>Password</label>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-2 my-4">
            <div class="card-body d-flex justify-content-center align-items-center">
                <asp:Button ID="btnSubmit" CssClass="btn btn-primary" runat="server" Text="Create Admin*" OnClick="CreateAdminClick" />
            </div>
        </div>
    </form>


    <script type="text/javascript">
        function restrictBackspace(event) {
            var textbox = document.getElementById('<%= txtAdmin.ClientID %>');
            var length = textbox.value.length;

            if (length == 3) {


                if (textbox.value.endsWith('__')) {

                    dropdown.selectedIndex = 0;
                    __doPostBack('<%= AccountDropDown.UniqueID %>', '');
                    textbox.value = textbox.value + '____'; // Add extra underscores
                } else {
                    textbox.value = textbox.value + '_';
                }
                if (textbox.value.endsWith('__')) {

                    var textbox = document.getElementById('<%= txtAdmin.ClientID %>');
                var length = textbox.value.length;
                var underscoreCount = (textbox.value.match(/_/g) || []).length;

                if (underscoreCount == 2) {
                    textbox.value = '____';
                    var dropdown = document.getElementById('<%= AccountDropDown.ClientID %>');
                    dropdown.selectedIndex = 0;
                    __doPostBack('<%= AccountDropDown.UniqueID %>', '');
                }
            }

        }
        if (length == 0) {
            textbox.value = textbox.value + '____';
            var dropdown = document.getElementById('<%= AccountDropDown.ClientID %>');
                    dropdown.selectedIndex = 0;
                    __doPostBack('<%= AccountDropDown.UniqueID %>', '');

                    }
                }


    </script>

    <script type="text/javascript">
        function updateStatus(name,id, isChecked) {
           
            $.ajax({
                type: "POST",
                url: "SuperAdmin/CreateAdmin.aspx/UpdateUserStatus",
                data: JSON.stringify({ name: name, isActive: isChecked , id:id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    alert(response.d); // Display success or failure message
                },
                error: function (error) {
                    console.error("Error updating status:", error);
                    alert("Failed to update status.");
                }
            });
        }
    </script>
</asp:Content>
