<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="SITconnect.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
            background-color: #ECECFF;
        }
        .auto-style2 {
            width: 915px;
        }
        .submitstyle {
            width: 175px;
            height: 30px;
            background-color: white;
            text-align: center;
            border-radius: 7px;
            border-color: pink;
            margin-top:25px;
            margin-left: 280px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="auto-style1">
                <tr>
                    <td class="auto-style2">
                        <h1 style="margin-left:20px;">SITConnect Stationary e-Shop</h1>
                    </td>
                    <td>
                        <asp:Button ID="viewProfileBTN" class="submitstyle" runat="server" Text="View Profile" Width="308px" Height="54px" OnClick="viewProfileBTN_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td>
                        <asp:Button ID="logoutBTN" class="submitstyle" runat="server" Text="Log Out" Width="308px" Height="54px" OnClick="logoutBTN_Click" />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">Welcome to the e-shop! View your profile on the top left &lt;3</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
        <p>
            &nbsp;</p>
    </form>
</body>
</html>
