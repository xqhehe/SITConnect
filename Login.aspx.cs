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
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace SITconnect
{
    public partial class Login : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        static string errorMsg = "";
        
        public string success { get; set; }
        public List<string> ErrorMessage { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Response.Cache.SetNoStore();
            }
        }

        protected void loginBTN_Click(object sender, EventArgs e)
        {
            
            //Response.Write("<script>window.alert('before getDBHash.')</script>");         
            string pwd = loginpwTB.Text.ToString().Trim();
            string userid = loginemailTB.Text.ToString().Trim();

            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);

            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = pwd + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);

                    if (userHash.Equals(dbHash))
                    {
                        Session["UserID"] = userid;
                        
                        LoginMe(sender, e);
                    }
                    else
                    {
                        errorMsg = "User ID or password is not valid. Please try again.";
                        errormessageLBL.Text = errorMsg;
                        // Response.Redirect("Login.aspx", false);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
        }

        private string getDBSalt(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;
        }

        private string getDBHash(string userid)
        {
            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return h;
        }

        protected void LoginMe(object sender, EventArgs e)
        {
            if (ValidateCaptcha())
            {
                Session["LoggedIn"] = loginemailTB.Text.Trim();
                // create a new guid and save into the session
                string guid = Guid.NewGuid().ToString();
                Session["AuthToken"] = guid;
                // create a new cookie with the created guid value
                Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                // Check for username and password
                Response.Redirect("HomePage.aspx", false);
            }
        }

        protected void registerPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registration.aspx");
        }

        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6LcEVl4eAAAAAMtMx0VAayxRohZIJDoLN3vFvfYK &response=" + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream())) {
                        string jsonResponse = readStream.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        Login jsonObject = js.Deserialize<Login>(jsonResponse);

                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected string decryptData(byte[] cipherText)
        {

            string plainText = null;
            //byte[] cipherText = Convert.FromBase64String(cipherString);

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();

                //Decrypt
                //byte[] decryptedText = decryptTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);
                //decryptedString = Encoding.UTF8.GetString(decryptedText);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return plainText;
        }
    }
}
