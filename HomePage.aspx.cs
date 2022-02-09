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

        protected void LogoutMe(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Log VALUES(@UserId, @LogStatus, @DateTimeLogged)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            userid = Session["UserID"].ToString();
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@UserId", userid);
                            cmd.Parameters.AddWithValue("@LogStatus", "Log out");
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
                                Session.Clear();
                                Session.Abandon();
                                Session.RemoveAll();
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

        protected void viewProfileBTN_Click(object sender, EventArgs e)
        {
            Response.Redirect("Profile.aspx");
        }
    }
}