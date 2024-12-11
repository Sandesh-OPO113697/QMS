
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Program.aspx.cs" Inherits="Program" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form class="AdminCreateio" runat="server">
        <div class="col-md-12 my-4">
            <div class="card shadow">
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group mb-3">
                                <label>Location Name</label>
                                  <asp:DropDownList ID="LocationDropDown" runat="server" 
                                    
                                    AutoPostBack="true" 
                                    CssClass="custom-select">
                                </asp:DropDownList>
                            </div>

                            <div class="form-group mb-3">
                               <label>Data Retention(Days)</label>
                                <asp:TextBox ID="txtDataRetention" runat="server" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group mb-3">
                               
                                  <label>Program Name</label>
                               <asp:TextBox ID="txtProgram" runat="server" CssClass="form-control" />
                            </div>
                            
                        </div>
                        
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-2 my-4">
            <div class="card-body d-flex justify-content-center align-items-center">
                <asp:Button ID="btnSubmit" CssClass="btn btn-primary" OnClick="CreateProcess" runat="server" Text="Create Program" />
            </div>
        </div>
    </form>

  
</asp:Content>
