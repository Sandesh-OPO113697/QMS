<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProcessAssign.aspx.cs" Inherits="RoleAssign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h1>Assign Program</h1>
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
                                <label for="custom-select-1">Select Users</label>
                                <asp:DropDownList ID="users" runat="server"
                                    AutoPostBack="true"
                                    CssClass="custom-select">
                                </asp:DropDownList>
                                
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group mb-3 ">
                                <label for="custom-select-1">Select Process</label>
                                <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                                </asp:CheckBoxList>
                              
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-2 my-4">
            <div class="card-body d-flex justify-content-center align-items-center">
                <asp:Button ID="btnSubmit" class="btn btn-primary" runat="server" Text="Map" OnClick="CreateAccountBySuper" />
            </div>
        </div>
    </form>
</asp:Content>
