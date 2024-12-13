<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="QMS.aspx.cs" Inherits="QMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="SuperAdmin" runat="server">
        <div class="col-md-12 my-4">
            <div class="card shadow">
                <div class="card-body">
                    <h5 class="card-title">Welcome SuperAdmin</h5>
                    
                    <table id="SuperAdminTable" class="table table-bordered table-hover mb-0">
                        <thead>
                            <tr>
                               <th style="color: black;">AccountID</th>
                                <th style="color: black;">AccountName</th>
                                <th style="color: black;">Authantication_Type</th>
                                <th style="color: black;">Isactive</th>
                                <th style="color: black;">Create_date</th>
                               <%-- <th style="color: black;">Activate</th>--%>
                            </tr>
                        </thead>
                        <tbody>
                            <% foreach (System.Data.DataRow row in GetSuperAdminData().Rows) { %>
                                <tr>
                                    <td><%= row["AccountID"] %></td>
                                    <td><%= row["AccountName"] %></td>
                                    <td><%= row["Authantication_Type"] %></td>
                                    <td><%= row["Isactive"].ToString() == "1" ? "Yes" : "No" %></td>
                                    <td><%= row["Create_date"] %></td>
                                   <%-- <td>
                                         <div class="custom-control custom-switch">
                                        <input type="checkbox" class="custom-control-input" id="chk_<%= row["AccountID"] %>" 
                                            <%= row["Isactive"].ToString() == "1" ? "checked" : "" %> 
                                            onclick="updateStatus('<%= row["AccountID"] %>    ', this.checked)" />
                                        <label class="custom-control-label" for="chk_<%= row["AccountID"] %>"></label>
                                    </div>
                                    </td>--%>
                                </tr>
                            <% } %>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <div id="Admin" runat="server">
        <div class="col-md-12 my-4">
            <div class="card shadow">
                <div class="card-body">
                    <h5 class="card-title">Welcome Admin</h5>
                    <p class="card-text">Add .table-bordered for borders on all sides of the table and cells.</p>
                 
                        <asp:Table ID="SuperAdminTable" runat="server" CssClass="table table-bordered table-hover mb-0">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell>AccountID</asp:TableHeaderCell>
                                <asp:TableHeaderCell>AccountName</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Authantication_Type</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Isactive</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Create_date</asp:TableHeaderCell>
                           
                            </asp:TableHeaderRow>
                    </asp:Table>
                </div>
            </div>
        </div>




    </div>



    <div id="QE" runat="server">
        <div class="col-md-12 my-4">
            <div class="card shadow">
                <div class="card-body">
                    <h5 class="card-title">WelCome QE</h5>
                    <p class="card-text">Add .table-bordered for borders on all sides of the table and cells.</p>
                    <table class="table table-bordered table-hover mb-0">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Name</th>
                                <th>Company</th>
                                <th>Address</th>
                                <th>Date</th>
                                <th>Activate</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>3224</td>
                                <td>Keith Baird</td>
                                <td>Enim Limited</td>
                                <td>901-6206 Cras Av.</td>
                                <td>Apr 24, 2019</td>
                                <td>
                                    <div class="custom-control custom-switch">
                                        <input type="checkbox" class="custom-control-input" id="c1" checked>
                                        <label class="custom-control-label" for="c1"></label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>3218</td>
                                <td>Graham Price</td>
                                <td>Nunc Lectus Incorporated</td>
                                <td>Ap #705-5389 Id St.</td>
                                <td>May 23, 2020</td>
                                <td>
                                    <div class="custom-control custom-switch">
                                        <input type="checkbox" class="custom-control-input" id="c2">
                                        <label class="custom-control-label" for="c2"></label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>2651</td>
                                <td>Reuben Orr</td>
                                <td>Nisi Aenean Eget Limited</td>
                                <td>7425 Malesuada Rd.</td>
                                <td>Nov 4, 2019</td>
                                <td>
                                    <div class="custom-control custom-switch">
                                        <input type="checkbox" class="custom-control-input" id="c3" checked>
                                        <label class="custom-control-label" for="c3"></label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>2636</td>
                                <td>Akeem Holder</td>
                                <td>Pellentesque Associates</td>
                                <td>896 Sodales St.</td>
                                <td>Mar 27, 2020</td>
                                <td>
                                    <div class="custom-control custom-switch">
                                        <input type="checkbox" class="custom-control-input" id="c4">
                                        <label class="custom-control-label" for="c4"></label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>2757</td>
                                <td>Beau Barrera</td>
                                <td>Augue Incorporated</td>
                                <td>4583 Id St.</td>
                                <td>Jan 13, 2020</td>
                                <td>
                                    <div class="custom-control custom-switch">
                                        <input type="checkbox" class="custom-control-input" id="c5">
                                        <label class="custom-control-label" for="c5"></label>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>





    </div>


    <script>
        function updateStatus(accountId, isChecked) {
            
            var status = isChecked ? 1 : 0; 

           
            $.ajax({
                type: "POST",
                url: "SuperAdmin/QMS.aspx/UpdateAccountStatus", // The server-side method
                data: JSON.stringify({ accountId: accountId, isActive: status }), // Send accountId and status
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    location.reload(); 
                    
                },
                error: function (xhr, status, error) {
                    alert("Error updating status.");
                }
            });
        }
        
       
       
    </script>
</asp:Content>
