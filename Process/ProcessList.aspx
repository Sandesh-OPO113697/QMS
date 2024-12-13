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
                                    <th>ID</th>
                                    <th>Process Name</th>
                                    <th>Location</th>

                                    <th>Created_Date</th>
                                    <th>Active_Status</th>
                                      <th>Edit</th>

                                </tr>
                            </thead>
                            <tbody>
                                <% foreach (System.Data.DataRow row in GetProcessListData().Rows)
                                   { %>
                                <tr id="row_<%= row["ID"] %>">
                                    <td><%= row["ID"] %></td>
                                    <td id="process_name_<%= row["ID"] %>">
                                        <span class="process-name"><%= row["Process"] %></span>
                                        <input type="text" class="form-control d-none process-input" value="<%= row["Process"] %>" />
                                    </td>
                                    <td><%= row["Location"] %></td>
                                    <td><%= row["Created_Date"] %></td>
                                    <td>
                                        <div class="custom-control custom-switch">
                                            <input type="checkbox" class="custom-control-input" id="status_<%= row["ID"] %>"
                                                onchange="updateActiveStatus(<%= row["ID"] %>, this.checked)"
                                                <%= (row["Active_Status"].ToString() == "1" ? "checked" : "") %> />
                                            <label class="custom-control-label" for="status_<%= row["ID"] %>"></label>
                                        </div>
                                    </td>
                                    <td>
                                        <button class="btn btn-sm btn-primary" onclick="enableEdit(<%= row["ID"] %>)">Edit</button>
                                        <button class="btn btn-sm btn-success d-none" onclick="saveEdit(<%= row["ID"] %>)">Save</button>
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
        function enableEdit(id) {
            event.preventDefault();
            var row = $("#row_" + id);
            row.find(".process-name").addClass("d-none");
            row.find(".process-input").removeClass("d-none");
            row.find(".btn-primary").addClass("d-none");
            row.find(".btn-success").removeClass("d-none");
           
        }

        function saveEdit(id) {
           
            var row = $("#row_" + id);
            var updatedName = row.find(".process-input").val();

            $.ajax({
                type: "POST",
                url: "Process/ProcessList.aspx/UpdateProcessName",
                data: JSON.stringify({ id: id, processName: updatedName }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    row.find(".process-name").text(updatedName).removeClass("d-none");
                    row.find(".process-input").addClass("d-none");
                    row.find(".btn-primary").removeClass("d-none");
                    row.find(".btn-success").addClass("d-none");
                    console.log("Process name updated successfully");
                },
                error: function (xhr, status, error) {
                    console.error("Error occurred while updating process name: " + error);
                }
            });
        }

    </script>


</asp:Content>
