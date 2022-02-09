<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="SITconnect.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration</title>
    <script type="text/javascript">

        function validatePW() {
            var pwstr = document.getElementById('<%=pwTB.ClientID%>').value;

            if (pwstr.length < 12) {
                document.getElementById("pwcheckerLBL").innerHTML = "Password length must be at least 12 characters!";
                document.getElementById("pwcheckerLBL").style.color = "Red";
                return("pw_short");
            }

            else if (pwstr.search(/[0-9]/) == -1) {
                document.getElementById("pwcheckerLBL").innerHTML = "Password require at least 1 number!";
                document.getElementById("pwcheckerLBL").style.color = "Red";
                return("pw_no_number")
            }

            else if (pwstr.search(/[A-Z]/) == -1) {
                document.getElementById("pwcheckerLBL").innerHTML = "Password require at least 1 uppercase letter!";
                document.getElementById("pwcheckerLBL").style.color = "Red";
                return("pw_no_upper")
            }

            else if (pwstr.search(/[a-z]/) == -1) {
                document.getElementById("pwcheckerLBL").innerHTML = "Password require at least 1 lowercase letter!";
                document.getElementById("pwcheckerLBL").style.color = "Red";
                return("pw_no_lower")
            }

            else if (pwstr.search(/[@$!%*?&]/) == -1) {
                document.getElementById("pwcheckerLBL").innerHTML = "Password require at least 1 special character!";
                document.getElementById("pwcheckerLBL").style.color = "Red";
                return ("pw_no_specialchar")
            }

            document.getElementById("pwcheckerLBL").innerHTML = "Excellent!";
            document.getElementById("pwcheckerLBL").style.color = "Green";
        }

        function validateEmail() {
            var emailadd = document.getElementById('<%=emailTB.ClientID%>').value;

            if (emailadd.search(/^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i) == -1) {
                document.getElementById("emailcheckerLBL").innerHTML = "Invalid email address!";
                document.getElementById("emailcheckerLBL").style.color = "Red";
                return ("email_invalid")
            }
            document.getElementById("emailcheckerLBL").innerHTML = "Valid email address!";
            document.getElementById("emailcheckerLBL").style.color = "Green";
        }

    </script>

    <style type="text/css">
        .auto-style1 {
            width: 100%;
            margin-right: 0px;
        }
        .auto-style2 {
            height: 31px;
        }
        .auto-style3 {
            width: 124px;
        }
        .auto-style4 {
            height: 31px;
            width: 124px;
        }
        .auto-style5 {
            width: 175px;
        }
        .auto-style6 {
            height: 31px;
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
            <h1 class="reg">Registration</h1>
        </div>
        <table class="auto-style1">
            <tr>
                <td class="auto-style3">First name:</td>
                <td class="auto-style5">
                    <asp:TextBox ID="fnameTB" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4">Last Name:</td>
                <td class="auto-style6">
                    <asp:TextBox ID="lnameTB" runat="server"></asp:TextBox>
                </td>
                <td class="auto-style2">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">Date of Birth:</td>
                <td class="auto-style5">
                    <asp:TextBox ID="dobTB" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">Credit Card Info:</td>
                <td class="auto-style5">
                    <asp:TextBox ID="ccTB" runat="server"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">Email Address:</td>
                <td class="auto-style5">
                    <asp:TextBox ID="emailTB" runat="server" onkeyup="javascript:validateEmail()"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="emailcheckerLBL" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">Password:</td>
                <td class="auto-style5">
                    <asp:TextBox ID="pwTB" runat="server" onkeyup="javascript:validatePW()" TextMode="Password"></asp:TextBox>
                </td>
                <td>
                    <asp:Label ID="pwcheckerLBL" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style3">photo?</td>
                <td class="auto-style5">
                    <asp:FileUpload ID="pfpFU" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style5">
                    <asp:Button ID="checkpwBTN" class="submitstyle" runat="server" Text="Check Password" OnClick="checkpwBTN_Click"/>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style5">
                    <asp:Button ID="registerBTN" class="submitstyle" runat="server" Text="Register" OnClick="registerBTN_Click" />
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style5">
                    <asp:Button ID="loginPage" class="submitstyle" runat="server" Text="Go to Login Page" OnClick="loginPage_Click"/>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    </form>
    </div>
</body>
</html>
