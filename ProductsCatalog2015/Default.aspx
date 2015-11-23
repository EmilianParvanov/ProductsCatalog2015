<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProductsCatalog2015._Default" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">
        function ConfirmationBox(prodName) {

            var result = confirm('Are you sure you want to delete ' + prodName + ' ???');
            if (result) {
                return true;
            }//
            else {
                return false;
            }
        }

    </script>

    <br />
    <asp:GridView runat="server" ID="gridViewProducts" ItemType="Models.ProductsDBModel.Product" AllowPaging="true"
         AllowSorting="true" PageSize="3" OnPageIndexChanging="gridViewProducts_PageIndexChanging" OnSorting="Sorting">   

        <Columns>
      <%--      <asp:BoundField DataField="id" HeaderText="id" />
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Price" HeaderText="Price" />--%>
        </Columns>

    </asp:GridView>
    <br />
    <p style="text-align:left;"><asp:Button ID="AddNewZakuskaBtn" runat="Server" Text="Add New Zakuska" OnClick="InvokeNewZakuskaView" /></p>
    <br />

    <asp:GridView ID="addNewZakuskaGrid" runat="server" 
                  AutoGenerateColumns="False"
                  Visible="false"
                  GridLines="None">
        <Columns>
            <asp:TemplateField HeaderText="Zakuska Name">
                <ItemTemplate>
                    <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Zakuska Price">
                <ItemTemplate>
                    <asp:TextBox ID="txtPrice" runat="server"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <asp:Button ID="AddNewZakuskaBtn" runat="Server" Text="Add" OnClick="AddNewZakuska" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <asp:Button ID="CancelNewZakuskaBtn" runat="Server" Text="Cancel" OnClick="CanlcelNewZakuska" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <br />
    
    <asp:GridView ID="GridVwPagingSorting" runat="server" AutoGenerateColumns="False" DataKeyNames="id"
                  Font-Names="Verdana" AllowPaging="True" AllowSorting="True" PageSize="5" Width="75%"
                  OnPageIndexChanging="PageIndexChanging" OnSorting="Sorting" onrowdatabound="GridVwPagingSorting_RowDataBound"
                  OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting" 
                  OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
             
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="id" SortExpression="id" />
                    <asp:BoundField DataField="name" HeaderText="name" SortExpression="name" />
                    <asp:BoundField DataField="price" HeaderText="price" SortExpression="price" DataFormatString="{0:F}"/>
                    <asp:CommandField ShowEditButton="true" />
                    <asp:CommandField ShowDeleteButton="true" />

                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkdelete" runat="server" OnClick="lnkdelete_Click" CausesValidation="False">Delete Product</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                    
                </Columns>
            </asp:GridView>
</asp:Content>
