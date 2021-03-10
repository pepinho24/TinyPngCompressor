<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Compress" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://unpkg.com/@progress/kendo-theme-bootstrap/dist/all.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" />
        <asp:Label Text="The path to the Docs repo folder:" ID="Label1" AssociatedControlID="tbSourcePath" runat="server" />

        <asp:TextBox runat="server" CssClass="k-textbox" ID="tbSourcePath" Width="600px" Placeholder="D:\Work\ajax-docs\"></asp:TextBox>
        <asp:RequiredFieldValidator ErrorMessage="Please enter a valid Path" ControlToValidate="tbSourcePath" runat="server"  />
        <br />
        <br />
        <asp:Label Text="TinyPNG API" ID="Label2" AssociatedControlID="tbTinyPngKey" runat="server" />
        <asp:TextBox runat="server" CssClass="k-textbox" ID="tbTinyPngKey" Width="600px" placeholder="Enter your TinyPNG API here"></asp:TextBox>
        <br />
        <br />
        <asp:Button runat="server" CssClass="k-button k-primary" ID="RadButton1" Text="Compress" OnClick="RadButton1_Click" AutoPostBack="true" />
        <hr />
        <asp:GridView runat="server" ID="RadGrid1"></asp:GridView>
      
    </form>
</body>
</html>
