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
                        SqlConnection conn = new SqlConnection(MYDBConnectionString);
                        String sql = "SELECT Locked, DateTimeLocked from Account where Email='" + loginemailTB.Text + "'";
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = sql;
                        cmd.Connection = conn;
                        SqlDataAdapter adapt = new SqlDataAdapter();
                        adapt.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        adapt.Fill(ds);
                        bool lockstatus;
                        DateTime datetimelock = DateTime.Now; ;
                        lockstatus = Convert.ToBoolean(ds.Tables[0].Rows[0]["Locked"].ToString());
                        if (lockstatus == false)
                        {
                            LoginMe(sender, e);
                        }
                        else
                        {
                            Label2.Text = "Account locked temporarily (15 Minutes) after 3 Invalid attempts. Your account will be unlocked automatically after 15 minutes.";
                        }
                        if (lockstatus == true)
                        {
                            datetimelock = Convert.ToDateTime(ds.Tables[0].Rows[0]["DateTimeLocked"].ToString());
                            datetimelock = Convert.ToDateTime(datetimelock.ToString("dd/MM/yyyy HH:mm:ss"));
                            DateTime cdatetime = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                            TimeSpan ts = cdatetime.Subtract(datetimelock);
                            if (ts.TotalMinutes >= 1)
                            {
                                unlockUser();
                                LoginMe(sender, e);
                            }
                        }
                    }
                    else
                    {
                        // Lock Account
                        SqlConnection conn = new SqlConnection(MYDBConnectionString);
                        String sql = "SELECT Locked, DateTimeLocked from Account where Email='" + loginemailTB.Text + "'";
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = sql;
                        cmd.Connection = conn;
                        SqlDataAdapter adapt = new SqlDataAdapter();
                        adapt.SelectCommand = cmd;
                        DataSet ds = new DataSet();
                        adapt.Fill(ds);
                        bool lockstatus;
                        DateTime datetimelock = DateTime.Now;
                        lockstatus = Convert.ToBoolean(ds.Tables[0].Rows[0]["Locked"].ToString());
                        if (lockstatus == true)
                        {
                            datetimelock = Convert.ToDateTime(ds.Tables[0].Rows[0]["DateTimeLocked"].ToString());
                            datetimelock = Convert.ToDateTime(datetimelock.ToString("dd/MM/yyyy HH:mm:ss"));
                            DateTime cdatetime = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                            TimeSpan ts = cdatetime.Subtract(datetimelock);
                            //Int32 minutelocked = Convert.ToInt32(ts.TotalMinutes);
                            //Int32 pendingminutes = 15 - minutelocked;
                            if (ts.TotalMinutes >= 15)
                            {
                                unlockUser();
                            }
                            else
                            {
                                Label2.Text = "Account locked temporarily (15 Minutes) after 3 Invalid attempts. Your account will be unlocked automatically after 15 minutes.";
                            }
                        }
                        else
                        {
                            int attemptcount;
                            if (Session["InvalidLoginAttempt"] != null)
                            {
                                attemptcount = Convert.ToInt16(Session["InvalidLoginAttempt"].ToString());
                                if (attemptcount == 2)
                                {
                                    updateLockStatus();
                                    Label2.Text = "Account locked temporarily (15 Minutes) after 3 Invalid attempts. Your account will be unlocked automatically after 15 minutes.";
                                }
                                else
                                {

                                    attemptcount += 1;
                                    Session["InvalidLoginAttempt"] = attemptcount;
                                    Label2.Text = "Invalid email or password. You have " + (3 - attemptcount) + " remaining to login";
                                }
                            }
                            else
                            {
                                attemptcount = 1;
                                Session["InvalidLoginAttempt"] = attemptcount;
                                Label2.Text = "Invalid email or password. You have " + (3 - attemptcount) + " remaining to login";
                            }

                        }

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

                try
                {
                    using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO Log VALUES(@UserId, @LogStatus, @DateTimeLogged)"))
                        {
                            using (SqlDataAdapter sda = new SqlDataAdapter())
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@UserId", loginemailTB.Text.Trim());
                                cmd.Parameters.AddWithValue("@LogStatus", "Log in");
                                cmd.Parameters.AddWithValue("@DateTimeLogged", DateTime.Now.ToString());
                                cmd.Connection = con;
                                try
                                {
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(ex.ToString());
                                }
                                finally
                                {
                                    con.Close();
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

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

        void updateLockStatus()
        {
            SqlConnection conn = new SqlConnection(MYDBConnectionString);
            string format = "MM/dd/yyyy HH:mm:ss";
            String updateData = "UPDATE Account set Locked=1, DateTimeLocked=' " + DateTime.Now.ToString(format) + "' where Email = '" + loginemailTB.Text + "'";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = updateData;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        void unlockUser()
        {
            SqlConnection conn = new SqlConnection(MYDBConnectionString);
            String updateData = "Update Account set Locked=0, DateTimeLocked= NULL where Email = '" + loginemailTB.Text + "'";
            
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = updateData;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close ();
            }
            
        }
    }
}
