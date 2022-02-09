<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITconnect.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <script src="https://www.google.com/recaptcha/api.js?render=6LcEVl4eAAAAAK7-SfdzOmP8Si3Y5Sds3xK1sdFH"></script>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
            margin-right: 0px;
        }
        .auto-style3 {
            width: 124px;
        }
        .auto-style5 {
            width: 175px;
        }
        body {
            background-color: #D6E7FF;
        }
        .reg {
            text-align: center;
            margin-top: 0px;
            padding-bottom: 20px;
        }
        .boxed {
            background-color: white;
            width: 400px;
            border: 15px solid pink;
            padding: 35px;
            margin: auto;
        }

        .submitstyle {
            width: 175px;
            height: 30px;
            background-color: white;
            text-align: center;
            border-radius: 7px;
            border-color: pink;
            margin-top:25px;
        }

    </style>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LcEVl4eAAAAAK7-SfdzOmP8Si3Y5Sds3xK1sdFH', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</head>
<body>
    <div class="boxed">
        <form id="form1" runat="server">
        <div>
            <h1 class="reg">Login</h1>
        </div>
        <table class="auto-style1">
            <tr>
                <td class="auto-style3">Email:</td>
                <td class="auto-style5">
                    <asp:TextBox ID="loginemailTB" runat="server" Width="173px"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">Password:</td>
                <td class="auto-style5">
                    <asp:TextBox ID="loginpwTB" runat="server" onkeyup="javascript:validatePW()" TextMode="Password" Width="173px"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="errormessageLBL" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style5">
                    <asp:Button ID="loginBTN" class="submitstyle" runat="server" Text="Login" Width="187px" OnClick="loginBTN_Click" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    &nbsp;</td>
                <td class="auto-style5">
                    <asp:Button ID="registerPage" class="submitstyle" runat="server" Text="Go to Register Page" Width="187px" OnClick="registerPage_Click"/>
                </td>
            </tr>            
            <tr>
                <td class="auto-style3">&nbsp;</td>                
            </tr>
            <div>
                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
            </div>
            <td>
                    <asp:Label ID="Label1" runat="server"></asp:Label>
            </td>
            <td>
                    <asp:Label ID="Label2" runat="server"></asp:Label>
            </td>
        </table>
    </form>
    </div>
</body>
</html>
