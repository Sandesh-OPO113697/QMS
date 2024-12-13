<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SubProgram.aspx.cs" Inherits="SubProgram" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form class="AdminCreateio" runat="server">
        <div class="alert alert-primary" id="NotificationDiv" runat="server" role="alert">
            <span class="fe fe-alert-circle fe-16 mr-2" id="Notification" runat="server"></span>

        </div>
        <div class="col-md-12 my-4">
            <div class="card shadow">
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group mb-3">
                                <label>Location Name</label>
                                <asp:DropDownList ID="LocationDropDown" runat="server"
                                    OnSelectedIndexChanged="LocationDropDown_SelectedIndexChanged"
                                    AutoPostBack="true"
                                    CssClass="custom-select">
                                </asp:DropDownList>
                            </div>


                        </div>
                        <div class="col-md-6">
                            <div class="form-group mb-3">

                                <label>Program Name</label>
                                <asp:DropDownList ID="ProgramDropDown" runat="server"
                                    OnSelectedIndexChanged="ProgramChangedinde"
                                    AutoPostBack="true"
                                    CssClass="custom-select">
                                </asp:DropDownList>
                            </div>



                        </div>
                    </div>
                </div>
            </div>
        </div>


        <div class="col-md-12 my-4">
            <div class="card shadow">
                <div class="card-body" id="UserList" runat="server">
                    <asp:GridView ID="UserGrid" runat="server" AutoGenerateColumns="false"  DataKeyNames="ID"
                        CssClass="table table-bordered" OnRowEditing="UserGrid_RowEditing"
                        OnRowUpdating="UserGrid_RowUpdating" OnRowCancelingEdit="UserGrid_RowCancelingEdit">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="true" />
                            <asp:BoundField DataField="Process" HeaderText="Process" ReadOnly="true" />
                            <asp:BoundField DataField="Location" HeaderText="Location" ReadOnly="true" />
                            <asp:TemplateField HeaderText="SubProcessName">

                                <ItemTemplate>
                                    <asp:Label ID="lblSubProcessName" runat="server" Text='<%# Eval("SubProcessName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtSubProcessName" runat="server" Text='<%# Eval("SubProcessName") %>' CssClass="form-control"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-primary btn-sm"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Save" CssClass="btn btn-success btn-sm"></asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-secondary btn-sm"></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </div>
            </div>
        </div>
        <div class="col-md-12 my-4">
            <div class="card shadow">
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group mb-3">
                                <label>Sub Program Name</label>
                                <asp:TextBox ID="txtsubprogram" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <div class="col-md-2 my-4">
            <div class="card-body d-flex justify-content-center align-items-center">
                <asp:Button ID="btnSubmit" CssClass="btn btn-primary" OnClick="CreateProcess" runat="server" Text="Create SubProgram" />
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function updateStatus(name, id, isChecked) {
            alert(name);
            alert(id);
            alert(isChecked);
            $.ajax({
                type: "POST",
                url: "SubProgram.aspx/UpdateUserStatus",
                data: JSON.stringify({ name: name, isActive: isChecked, id: id }),
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
