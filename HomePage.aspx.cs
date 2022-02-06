using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace SITconnect
{
    
    public partial class HomePage : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        static byte[] Key = null;
        static byte[] IV = null;
        static string userid = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
            if (!IsPostBack)
            {
                Response.Cache.SetNoStore();
            }
        }

        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();

                        }
                    }
                }
            }


            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return plainText;
        }

        protected void LogoutMe(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }

        protected void logoutBTN_Click(object sender, EventArgs e)
        {
            LogoutMe(sender, e);
        }
    }
}