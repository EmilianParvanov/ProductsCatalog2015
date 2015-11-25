<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProductsCatalog2015._Default" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <script type="text/javascript">
        $(function () {
            $.ajax({
                type: "POST",
                url:'<%= ResolveUrl("Default.aspx/GetProducts") %>',
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess
            });
        });

        function OnSuccess(response) {
            var xmlDoc = $.parseXML(response.d);
            var xml = $(xmlDoc);
            var products = xml.find("Table");
            var row = $("[id*=gvProducts] tr:last-child").clone(true);
            $("[id*=gvProducts] tr").not($("[id*=gvProducts] tr:first-child")).remove();
            $.each(products, function () {
                var product = $(this);
                AppendRow(row, $(this).find("id").text(), $(this).find("name").text(), $(this).find("price").text())
                row = $("[id*=gvProducts] tr:last-child").clone(true);
            });
        }
        function AppendRow(row, id, name, price) {
            $(".id", row).find("span").html(id);
            //non-editable
            $(".name", row).find("span").html(name);
            $(".name", row).find("input").val(name);

            $(".price", row).find("span").html(price);
            $(".price", row).find("input").val(price);
            $("[id*=gvProducts]").append(row);
        }
    </script>

    <script type="text/javascript">
        $("body").on("click", "[id*=gvProducts] .Delete", function () {
            if (confirm("Are you sure?")) {
                var row = $(this).closest("tr");
                var id = row.find("span").html();
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("Default.aspx/DeleteProduct") %>',
                    data: '{id: ' + id + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        row.remove();
                    }
                });
            }

            return false;
        });
    </script>
    
    <script type="text/javascript">
        $("body").on("click", "[id*=gvProducts] .Edit", function () {
            var row = $(this).closest("tr");
            $("td", row).each(function () {
                if ($(this).find("input").length > 0) {
                    $(this).find("input").show();
                    $(this).find("span").hide();
                }
            });
            row.find(".Update").show();
            row.find(".Cancel").show();
            row.find(".Delete").hide();
            $(this).hide();
            return false;
        });
    </script>

    <script type="text/javascript">
        //Update.
        $("body").on("click", "[id*=gvProducts] .Update", function () {
            var row = $(this).closest("tr");
            $("td", row).each(function () {
                if ($(this).find("input").length > 0) {
                    var span = $(this).find("span");
                    var input = $(this).find("input");
                    span.html(input.val());
                    span.show();
                    input.hide();
                }
            });
            row.find(".Edit").show();
            row.find(".Delete").show();
            row.find(".Cancel").hide();
            $(this).hide();

            var id = row.find(".id").find("span").html();
            var name = row.find(".name").find("span").html();
            var price = row.find(".price").find("span").html();
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("Default.aspx/UpdateProduct") %>',
                data: '{id: ' + id + ', name: "' + name + '", price: "' + price + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json"
            });

            return false;
        });
    </script>

    <script type="text/javascript">
        $("body").on("click", "[id*=btnAdd]", function () {
            //debugger;
            var txtName = $("[id*=txtName]");
            var txtPrice = $("[id*=txtPrice]");
            $.ajax({
                type: "POST",
                url: '<%= ResolveUrl("Default.aspx/InsertProduct") %>',
                data: '{name: "' + txtName.val() + '", price: "' + txtPrice.val() + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var row = $("[id*=gvProducts] tr:last-child").clone(true);
                    AppendRow(row, response.d, txtName.val(), txtPrice.val());
                    txtName.val("");
                    txtPrice.val("");
                }
            });
            return false;
        });
    </script>

    <script type="text/javascript">
        $("body").on("click", "[id*=btnClean]", function Clean() {
            var txtName = $("[id*=txtName]");
            var txtPrice = $("[id*=txtPrice]");
            txtName.val("");
            txtPrice.val("");

            return false;
            });
    </script>


    <br />

    <asp:Button runat="server" ID="InvokeNewZakuskaInputFields" OnClick="InvokeNewZakuskaInputFieldsBtnFunc" Text="Add New Zakuska" />

    <br />

    <div  ID="InvokeNewZakuskaInputFieldsView" runat="server">
        <table>
            <tr>
                <td style="width: 150px">
                    Name:<br />
                    <asp:TextBox ID="txtName" runat="server" Width="140" />
                </td>
                <td style="width: 150px">
                    Price:<br />
                    <asp:TextBox ID="txtPrice" runat="server" Width="140" />
                </td>
                <td style="width: 30px">
                    <br />
                    <asp:Button ID="btnAdd" runat="server" Text="Add"/>
                </td>
                <td style="width: 45px">
                    <br />
                    <asp:Button ID="btnClean" runat="server" Text="Clean"/>
                </td>
                <td style="width: 45px">
                    <br />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="CancelNewZakuska"/>
                </td>

            </tr>
        </table>
    </div>


    <asp:GridView runat="server" ID="gridViewProducts" ItemType="Models.ProductsDBModel.Product" AllowPaging="true"
         AllowSorting="true" PageSize="3" OnPageIndexChanging="gridViewProducts_PageIndexChanging" OnSorting="Sorting">   

        <Columns>
      <%--      <asp:BoundField DataField="id" HeaderText="id" />
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Price" HeaderText="Price" />--%>
        </Columns>

    </asp:GridView>
   
    <br />
    
    <asp:GridView ID="GridVwPagingSorting" runat="server" AutoGenerateColumns="False" DataKeyNames="id"
        Font-Names="Verdana" AllowPaging="True" AllowSorting="True" PageSize="5" Width="75%"
        OnPageIndexChanging="PageIndexChanging" OnSorting="Sorting"
        OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowDeleting="GridView1_RowDeleting"
        OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating">
        <Columns>
            <asp:BoundField DataField="id" HeaderText="id" SortExpression="id" />
            <asp:BoundField DataField="name" HeaderText="name" SortExpression="name" />
            <asp:BoundField DataField="price" HeaderText="price" SortExpression="price" DataFormatString="{0:F}" />
            <asp:CommandField ShowEditButton="true" />
            <asp:CommandField ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>

    <br />

    <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:TemplateField HeaderText="id" ItemStyle-Width="110px" ItemStyle-CssClass="id">
                <ItemTemplate>
                    <asp:Label Text='<%# Eval("id") %>' runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="name" ItemStyle-Width="150px" ItemStyle-CssClass="name">
                <ItemTemplate>
                    <asp:Label Text='<%# Eval("name") %>' runat="server" />
                    <asp:TextBox Text='<%# Eval("name") %>' runat="server" Style="display: none" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="price" ItemStyle-Width="150px" ItemStyle-CssClass="price">
                <ItemTemplate>
                    <asp:Label Text='<# Eval("price") %>' runat="server" />
                    <asp:TextBox Text='<# Eval("price") %>' runat="server" Style="display: none" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton Text="Edit" runat="server" CssClass="Edit" />
                    <asp:LinkButton Text="Update" runat="server" CssClass="Update" Style="display: none" />
                    <asp:LinkButton Text="Cancel" runat="server" CssClass="Cancel" Style="display: none" />
                    <asp:LinkButton Text="Delete" runat="server" CssClass="Delete" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
