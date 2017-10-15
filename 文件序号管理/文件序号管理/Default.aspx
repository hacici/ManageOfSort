<%@ Page Title="主页" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="文件序号管理._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        欢迎使用  
        <asp:Label ID="Label1" runat="server"></asp:Label>
        文件序号管理系统!
    </h2>
    <p>
        &nbsp;<asp:Label ID="Label2" runat="server"></asp:Label>
    </p>
    <p>
        您还可以找到 <a href="http://go.microsoft.com/fwlink/?LinkID=152368"
            title="MSDN ASP.NET 文档">MSDN 上有关 ASP.NET 的文档</a>。
     
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:DataServices %>" 
            SelectCommand="SELECT * FROM [SimpleDataItem]"></asp:SqlDataSource>
    </p>
</asp:Content>
