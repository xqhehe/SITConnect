<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verification.aspx.cs" Inherits="SITconnect.Verification" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Account Verification</title>
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
</head>
<body>
    <div class="boxed">
        <form id="form1" runat="server">
        <div>
            <h1 class="reg">Account Verification</h1>
        </div>
        <table class="auto-style1">
            <tr>
                <td class="auto-style3">Verification Code:</td>
                <td class="auto-style5">
                    <asp:TextBox ID="loginemailTB" runat="server" Width="173px"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
          
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style5">
                    <asp:Button ID="verifyBTN" class="submitstyle" runat="server" Text="Verify" Width="187px" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <div>
                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
            </div>
        </table>
    </form>
    </div>
</body>
</html>
