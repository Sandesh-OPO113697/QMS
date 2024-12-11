<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProcessList.aspx.cs" Inherits="ProcessList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form11" runat="server">
        <div id="Process2" runat="server">
            <div class="col-md-12 my-4">
                <div class="card shadow">
                    <div class="card-body">
                        <h5 class="card-title">Welcome Admin</h5>

                        <table id="Process" class="table table-bordered table-hover mb-0">
                            <thead>
                                <tr>
                                    <th style="color: black;">ID</th>
                                    <th style="color: black;">Process Name</th>
                                    <th style="color: black;">Location_ID</th>
                                    <th style="color: black;">Flag</th>
                                    <th style="color: black;">Created_Date</th>
                                    <th style="color: black;">Active_Status</th>

                                </tr>
                            </thead>
                            <tbody>
                                <% foreach (System.Data.DataRow row in GetProcessListData().Rows)
                                   { %>
                                <tr>
                                    <td><%= row["ID"] %></td>
                                    <td><%= row["Process"] %></td>
                                    <td><%= row["Location_ID"] %></td>
                                    <td><%= row["Flag"] %></td>
                                    <td><%= row["Created_Date"] %></td>
                                    <td>
                                        <div class="custom-control custom-switch">
                                            <input type="checkbox" class="custom-control-input" id="status_<%= row["ID"] %>"
                                                onchange="updateActiveStatus(<%= row["ID"] %>, this.checked)"
                                                <%= (row["Active_Status"].ToString() == "1" ? "checked" : "") %>>
                                            <label class="custom-control-label" for="status_<%= row["ID"] %>"></label>
                                        </div>
                                       
                                    </td>



                                </tr>
                                <% } %>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>



        <div id="ProcessAccountUser" runat="server">
            <div class="col-md-12 my-4">
                <div class="card shadow">
                    <div class="card-body">
                        <h5 class="card-title">Welcome User</h5>

                        <table id="ProcessUser" class="table table-bordered table-hover mb-0">
                            <thead>
                                <tr>
                                    <th style="color: black;">ProgramName</th>
                                    <th style="color: black;">SubProcessName</th>
                                    <th style="color: black;">UserName</th>
                                    <th style="color: black;">userid</th>
                                    
                                

                                </tr>
                            </thead>
                            <tbody>
                                <% foreach (System.Data.DataRow row in GetProcessAccountUser().Rows)
                                   { %>
                                <tr>
                                    <td><%= row["ProgramName"] %></td>
                                    <td><%= row["SubProcessName"] %></td>
                                    <td><%= row["UserName"] %></td>
                                    <td><%= row["userid"] %></td>
                                    
                                </tr>
                                <% } %>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script>
        function updateActiveStatus(id, isActive) {
            $.ajax({
                type: "POST",
                url: "Process/ProcessList.aspx/UpdateActiveStatus", // Server-side method
                data: JSON.stringify({ id: id, isActive: isActive }), // Data to send
                contentType: "application/json; charset=utf-8", // Content type
                dataType: "json", // Expected response data type
                success: function (response) {
                    console.log("Update successful: " + response.d); // Handle success response
                },
                error: function (xhr, status, error) {
                    console.error("Error occurred: " + error); // Handle error
                }
            });
        }
    </script>


</asp:Content>
