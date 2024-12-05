<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateAdmin.aspx.cs" Inherits="CreateAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form class="AdminCreateio" runat="server">
        <div class="col-md-12 my-4">
            <div class="card shadow">
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group mb-3">
                                <label>User Name</label>
                                <asp:TextBox ID="txtAdmin" runat="server" CssClass="form-control" oninput="restrictBackspace(event)"  onkeydown="preventDeleteBackspace(event)"  />
                            </div>

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
</asp:Content>
