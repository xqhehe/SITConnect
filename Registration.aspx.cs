using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITconnect
{
    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        static String verificationcode;
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }
        private int checkPassword(string password)
        {
            int score = 0;

            // pw: length
            if (password.Length < 12)
            {
                return 1;
            }
            else
            {
                score = 1;
            }
            // pw: upper chars
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            // pw: lower chars
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            // pw: numerals
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            // pw: special chars
            if (Regex.IsMatch(password, "[@$!%*?&]"))
            {
                score++;
            }

            return score;
        }

        protected void checkpwBTN_Click(object sender, EventArgs e)
        {
            // Extract data from textbox
            int scores = checkPassword(pwTB.Text);
            string status = "";
            switch (scores)
            {
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Excellent";
                    break;
                default:
                    break;
            }
            pwcheckerLBL.Text = "Status : " + status;
            if (scores < 4)
            {
                pwcheckerLBL.ForeColor = Color.Red;
                return;
            }
            pwcheckerLBL.ForeColor = Color.Green;
        }

        protected void registerBTN_Click(object sender, EventArgs e)
        {
            //string pwd = get value from your Textbox
            string pwd = pwTB.Text.ToString().Trim();
            //Generate random "salt"
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];
            //Fills array of bytes with a cryptographically strong sequence of random values.
            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);
            SHA512Managed hashing = new SHA512Managed();
            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
            finalHash = Convert.ToBase64String(hashWithSalt);
            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;
            createAccount();
            Response.Redirect("Login.aspx", false);
        }

        protected void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@FirstName, @LastName, @DOB, @Email, @CreditCard, @PasswordHash, @PasswordSalt, @DateTimeRegistered, @EmailVerified, @VerificationCode, @IV, @Key, @Locked, @DateTimeLocked)"))
                {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", fnameTB.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", lnameTB.Text.Trim());
                            cmd.Parameters.AddWithValue("@DOB", dobTB.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", emailTB.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreditCard", Convert.ToBase64String(encryptData(ccTB.Text.Trim())));
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@DateTimeRegistered", DateTime.Now);
                            cmd.Parameters.AddWithValue("@EmailVerified", DBNull.Value);
                            cmd.Parameters.AddWithValue("@VerificationCode", DBNull.Value);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@Locked", 0);
                            cmd.Parameters.AddWithValue("@DateTimeLocked", DBNull.Value);
                            cmd.Connection = con;
                            try
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();
                            } 
                            catch (Exception ex) {
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
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);


                //Encrypt
                //cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
                //cipherString = Convert.ToBase64String(cipherText);
                //Console.WriteLine("Encrypted Text: " + cipherString);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally {
            
            }
            return cipherText;
        }

        protected void loginPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}