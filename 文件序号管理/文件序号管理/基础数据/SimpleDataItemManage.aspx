<%@ Page Title="条目管理" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="SimpleDataItemManage.aspx.cs" Inherits="_Default"  MaintainScrollPositionOnPostback="true"  %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">

        .style1
    {
        width: 92%;
        height: 179px;
    }
    .style2
    {
            width: 131px;
            font-size: medium;
            text-align: right;
        }
    .style3
    {
        width: 131px;
        height: 26px;
            font-size: medium;
        }
    .style4
    {
        height: 26px;
            width: 730px;
        }
        .style5
        {
            width: 131px;
            height: 41px;
        }
        .style6
        {
            height: 41px;
            width: 730px;
        }
        .style7
        {
            width: 131px;
            height: 16px;
            font-size: medium;
        }
        .style8
        {
            height: 16px;
            width: 730px;
        }
        .style9
        {
            width: 131px;
            height: 8px;
        }
        .style10
        {
            height: 8px;
            width: 730px;
        }
        .style11
        {
            width: 131px;
            font-size: medium;
            text-align: right;
            height: 11px;
        }
        .style12
        {
            height: 11px;
            width: 730px;
        }
        .style13
        {
            width: 730px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table class="style1">
    <tr>
        <td class="style2">
            简单数据条目名称</td>
        <td class="style13">
            <asp:TextBox ID="TextBox_jiaoyanshi" runat="server" Width="468px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="style11">
            说&nbsp;&nbsp;&nbsp; 明</td>
        <td class="style12">
            <asp:TextBox ID="TextBox_leader1" runat="server" Height="31px" Width="468px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="style3">
            &nbsp;</td>
        <td class="style4">
            <asp:Button ID="Button1" runat="server" Text="添加新条目" onclick="Button1_Click" />
            <a href="#tips" id="clickLink"></a>
        </td> 
    </tr>
    <tr>
        <td class="style5">
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                ConnectionString="<%$ ConnectionStrings:DataServices %>" 
                SelectCommand="SELECT * FROM [SimpleDataItem] WHERE ([ItemName] = @ItemName)">
                <SelectParameters>
                    <asp:ControlParameter ControlID="TextBox_ItemName" Name="ItemName" PropertyName="Text" 
                        Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </td>
        <td class="style6">
            <asp:GridView ID="GridView1" runat="server" AllowSorting="True" 
                AutoGenerateColumns="False" DataSourceID="SqlDataSource1" Width="100%" 
                DataKeyNames="ItemName" PageSize="50">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                    <asp:BoundField DataField="ItemName" HeaderText="条目名称" 
                        SortExpression="ItemName" ReadOnly="True" />
                    <asp:BoundField DataField="ItemDescript" HeaderText="条目描述" 
                        SortExpression="ItemDescript" />
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click1" 
                                CommandArgument='<%# Eval("ItemName", "{0}") %>'>选择编辑</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
             
        </td>
    </tr>
    <tr>
        <td class="style5">
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:DataServices %>" 
                OldValuesParameterFormatString="original_{0}" 
                SelectCommand="SELECT * FROM [SimpleDataItem]" 
                ConflictDetection="CompareAllValues" 
                DeleteCommand="DELETE FROM [SimpleDataItem] WHERE [ItemName] = @original_ItemName AND (([ItemDescript] = @original_ItemDescript) OR ([ItemDescript] IS NULL AND @original_ItemDescript IS NULL))" 
                InsertCommand="INSERT INTO [SimpleDataItem] ([ItemName], [ItemDescript]) VALUES (@ItemName, @ItemDescript)" 
                UpdateCommand="UPDATE [SimpleDataItem] SET [ItemDescript] = @ItemDescript WHERE [ItemName] = @original_ItemName AND (([ItemDescript] = @original_ItemDescript) OR ([ItemDescript] IS NULL AND @original_ItemDescript IS NULL))">
                <DeleteParameters>
                    <asp:Parameter Name="original_ItemName" Type="String" />
                    <asp:Parameter Name="original_ItemDescript" Type="String" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="ItemName" Type="String" />
                    <asp:Parameter Name="ItemDescript" Type="String" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="ItemDescript" Type="String" />
                    <asp:Parameter Name="original_ItemName" Type="String" />
                    <asp:Parameter Name="original_ItemDescript" Type="String" />
                </UpdateParameters>
            </asp:SqlDataSource>
            </td>
        <td class="style6">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblInfor" runat="server" ForeColor="Red"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="style9" bgcolor="#4B6C9E">
            </td>
        <td class="style10" bgcolor="#4B6C9E">
        </td>
    </tr>
    <tr>
        <td class="style7">
            &nbsp;</td>
        <td class="style8">
            &nbsp;</td>
    </tr>
    <tr>
        <td class="style7">
            简单数据条目名称</td>
        <td class="style8">
            <asp:TextBox ID="TextBox_ItemName" runat="server" AutoPostBack="True" 
                Enabled="False" Height="18px" Width="491px">性别</asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="style7">
            条目名称</td>
        <td class="style8">
            <asp:TextBox ID="TextBox_ItemContent" runat="server" Width="493px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="style7">
            说&nbsp;&nbsp;&nbsp; 明</td>
        <td class="style8">
            <asp:TextBox ID="TextBox_Content_descript" runat="server" Height="38px" 
                Width="490px"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="style7">
            &nbsp;</td>
        <td class="style8">
            <asp:Button ID="Button2" runat="server" onclick="Button2_Click" 
                Text="添加新条目内容" />
        </td>
    </tr>
    <tr>
        <td class="style5">
            &nbsp;</td>
        <td class="style6">
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="SimpleDataID" DataSourceID="SqlDataSource3" 
                AllowSorting="True" Width="633px">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                    <asp:BoundField DataField="SimpleDataID" HeaderText="序号" 
                        InsertVisible="False" ReadOnly="True" SortExpression="SimpleDataID" 
                        Visible="False" />
                    <asp:BoundField DataField="ItemName" HeaderText="条目名称" 
                        SortExpression="ItemName" />
                    <asp:BoundField DataField="ItemContent" HeaderText="条目明细" 
                        SortExpression="ItemContent" />
                    <asp:BoundField DataField="ItemContentDescript" 
                        HeaderText="描  述" SortExpression="ItemContentDescript" />
                </Columns>
            </asp:GridView>
             
        </td>
    </tr>
    <tr>
        <td class="style5">
            <asp:SqlDataSource ID="SqlDataSource3" runat="server" 
                ConnectionString="<%$ ConnectionStrings:DataServices %>" 
                SelectCommand="SELECT * FROM [SimpleData] WHERE ([ItemName] = @ItemName2)" 
                ConflictDetection="CompareAllValues" 
                DeleteCommand="DELETE FROM [SimpleData] WHERE [SimpleDataID] = @original_SimpleDataID AND [ItemName] = @original_ItemName AND [ItemContent] = @original_ItemContent AND (([ItemContentDescript] = @original_ItemContentDescript) OR ([ItemContentDescript] IS NULL AND @original_ItemContentDescript IS NULL))" 
                InsertCommand="INSERT INTO [SimpleData] ([ItemName], [ItemContent], [ItemContentDescript]) VALUES (@ItemName, @ItemContent, @ItemContentDescript)" 
                OldValuesParameterFormatString="original_{0}" 
                UpdateCommand="UPDATE [SimpleData] SET [ItemContent] = @ItemContent, [ItemContentDescript] = @ItemContentDescript WHERE [SimpleDataID] = @original_SimpleDataID AND [ItemName] = @original_ItemName AND [ItemContent] = @original_ItemContent AND (([ItemContentDescript] = @original_ItemContentDescript) OR ([ItemContentDescript] IS NULL AND @original_ItemContentDescript IS NULL))">
                <DeleteParameters>
                    <asp:Parameter Name="original_SimpleDataID" Type="Int32" />
                    <asp:Parameter Name="original_ItemName" Type="String" />
                    <asp:Parameter Name="original_ItemContent" Type="String" />
                    <asp:Parameter Name="original_ItemContentDescript" Type="String" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="ItemName" Type="String" />
                    <asp:Parameter Name="ItemContent" Type="String" />
                    <asp:Parameter Name="ItemContentDescript" Type="String" />
                </InsertParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="TextBox_ItemName" Name="ItemName2" 
                        PropertyName="Text" Type="String" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="ItemName" Type="String" />
                    <asp:Parameter Name="ItemContent" Type="String" />
                    <asp:Parameter Name="ItemContentDescript" Type="String" />
                    <asp:Parameter Name="original_SimpleDataID" Type="Int32" />
                    <asp:Parameter Name="original_ItemName" Type="String" />
                    <asp:Parameter Name="original_ItemContent" Type="String" />
                    <asp:Parameter Name="original_ItemContentDescript" Type="String" />
                </UpdateParameters>
            </asp:SqlDataSource>
            </td>
        <td class="style6">
            <asp:Label ID="lblInfor0" runat="server" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</table>

            <a name="tips"></a>
    <br />
</asp:Content>
