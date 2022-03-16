using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FritidsTjanser.Models
{
    public class DALManager
    {
        //Connection string to the database. Full connection string is located in web.config
        private static string connString = ConfigurationManager.ConnectionStrings["FritidsTjansConnString"].ConnectionString;

        //Salt used to hash passwords
        private static string salt;

        //Generates a random salt to hash the passwords
        public void GenerateSalt()
        {
            using (var randomGenerator = new RNGCryptoServiceProvider())
            {
                byte[] buff = new byte[8];
                randomGenerator.GetBytes(buff);

                salt = Convert.ToBase64String(buff);
            }
        }

        //Creates a hashed password based on the password inputted and the generated salt
        public string CreateHashedPassword(string password)
        {
            byte[] pwdWithSalt = Encoding.ASCII.GetBytes(string.Concat(password, salt));
            using (var sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(pwdWithSalt));
            }
        }

        //Stores the hashed password in the database
        public void StoreHashedPassword(string username, string hashedPassword)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Users VALUES (@username, @password, @salt)", conn);
                cmd.Parameters.Add(new SqlParameter("@username", username));
                cmd.Parameters.Add(new SqlParameter("@password", hashedPassword));
                cmd.Parameters.Add(new SqlParameter("@salt", salt));
                cmd.ExecuteNonQuery();
            }
        }

        //Gets the salt from the database based on the username to compare with login
        public static string GetSaltFromDatabase(string username)
        {
            string returnedSalt = "";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Salt FROM Users WHERE Username = @username", conn);
                cmd.Parameters.Add(new SqlParameter("@username", username));
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    returnedSalt = (string)rdr[0];
                }
            }
            return returnedSalt;
        }

        //Gets hashed password from the database based on the inputted username
        public static string GetHashedPasswordFromDataBase(string username)
        {
            string returnedPwd = "";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Password FROM Users WHERE Username = @username", conn);
                cmd.Parameters.Add(new SqlParameter("@username", username));
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    returnedPwd = (string)rdr[0];
                }
            }
            return returnedPwd;
        }

        //Get all services from the database based on the searched zipcode
        public List<ServiceModel> GetServicesFromDatabase(int zipCode)
        {
            List<ServiceModel> services = new List<ServiceModel>();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Service WHERE ZipCode = @zipCode", conn);
                cmd.Parameters.Add(new SqlParameter("@zipCode", zipCode));
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    services.Add(new ServiceModel
                    {
                        Id = Convert.ToInt32(rdr["ID"]),
                        Description = rdr["Description"].ToString(),
                        DateFrom = rdr["DateFrom"].ToString(),
                        DateTo = rdr["DateTo"].ToString(),
                        ZipCode = Convert.ToInt32(rdr["ZipCode"])
                    });
                }
            }
            return services;
        }

        //Checks if the inputted password when trying to login
        public bool ValidatePassword(string password, string username)
        {
            string tempPwd = "";
            byte[] pwdWithSaltFromDB = Encoding.ASCII.GetBytes(string.Concat(password, GetSaltFromDatabase(username)));
            using (var sha256 = SHA256.Create())
            {
                tempPwd = Convert.ToBase64String(sha256.ComputeHash(pwdWithSaltFromDB));
            }
            if (tempPwd == GetHashedPasswordFromDataBase(username))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}