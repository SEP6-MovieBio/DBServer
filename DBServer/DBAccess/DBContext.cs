using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Security;


namespace DBServer.DBAccess
{
    public class DBContext
    {
        private NetworkCredential creds;
        private string connectionString;
        public DBContext()
        {
            creds = new NetworkCredential(Environment.GetEnvironmentVariable("DbUsername"),Environment.GetEnvironmentVariable("DbPassword"));
            
            connectionString = Environment.GetEnvironmentVariable("connString");
            
        }

        public List<MovieInfo> GetMovieInfo()
        {
            List<MovieInfo> list = new List<MovieInfo>();
            string sql = "Select top(10) title, [year], director, rating, votes from moviedb.dbo.movieinfo";
            try
            {
                
                SqlConnection connection;
                SecureString secureString = creds.SecurePassword;
                secureString.MakeReadOnly();
                

                connection = new SqlConnection(connectionString, new SqlCredential(creds.UserName, secureString));
            
                
            
                SqlCommand command;
                SqlDataReader reader;

                connection.Open();

                
            
                command = new SqlCommand(sql, connection);
            
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    MovieInfo movieInfo = new MovieInfo(reader.IsDBNull(0)?"Unknown":reader.GetString(0), reader.IsDBNull(1)? 0: reader.GetDecimal(1), reader.IsDBNull(2)?"Unknown":reader.GetString(2),
                        reader.IsDBNull(3)?0:reader.GetSqlSingle(3).Value, reader.IsDBNull(4)?0:reader.GetInt32(4));
                    list.Add(movieInfo);
                }

                connection.Close();
                
                return list;
            }
            catch (Exception e)
            {
                list.Add(new MovieInfo(e.Message.ToString(),0,"unknown",0,0));
                return list;

            }
           
        }

        public List<Movie> GetFavoriteMovieIDs(string username)
        {
            
        }

        public bool ValidateLogin(string usernameToBeValidated, string hash)
        {
            string sql = "Select passwordHash from moviedb.dbo.Users where username = '" + usernameToBeValidated + "'";
            try
            {
                
                
                SqlConnection connection;
                SecureString secureString = creds.SecurePassword;
                secureString.MakeReadOnly();
                
               

                connection = new SqlConnection(connectionString, new SqlCredential(creds.UserName, secureString));
            
                
            
                SqlCommand command;
                SqlDataReader reader;
                bool isCorrectPass = false;
                connection.Open();

                
            
                command = new SqlCommand(sql, connection);
            
                reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        isCorrectPass = reader.GetString(0) == hash;
                    }
                    
                }

                connection.Close();
                
                return isCorrectPass;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;

            }
            
        }

        public UserInfo GetUserInfo(string username)
        {
            UserInfo userInfo = new UserInfo();
            string sql = "SELECT [username],[phoneNumber],[phoneIsHidden],[email],[emailIsHidden],[biography]FROM moviedb.[dbo].[UserInfo] where username = '" + username + "'";
            try
            {
                
                
                
                SqlConnection connection;
                SecureString secureString = creds.SecurePassword;
                secureString.MakeReadOnly();

                

                connection = new SqlConnection(connectionString, new SqlCredential(creds.UserName, secureString));
            
                
            
                SqlCommand command;
                SqlDataReader reader;

                connection.Open();

                
            
                command = new SqlCommand(sql, connection);
            
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    userInfo = new UserInfo(reader.IsDBNull(0)?"Unknown":reader.GetString(0), reader.IsDBNull(1)? "Unknown": reader.GetString(1), reader.IsDBNull(2)? true:reader.GetBoolean(2),
                        reader.IsDBNull(3)?"Unknown":reader.GetString(3), reader.IsDBNull(4)?true:reader.GetBoolean(4),reader.IsDBNull(3)?"":reader.GetString(5));
                    
                }

                connection.Close();
                
                return userInfo;
            }
            catch (Exception e)
            {
                userInfo = new UserInfo(biography:e.Message.ToString());
                return userInfo;

            }
        }

        public bool PostBiography(UserInfo userInfo)
        {
            string sql = "Update moviedb.[dbo].[UserInfo] set [biography] = '"+ userInfo.biography + "' where username = '" + userInfo.username + "'";
            try
            {
                
                
                SqlConnection connection;
                SecureString secureString = creds.SecurePassword;
                secureString.MakeReadOnly();


                connection = new SqlConnection(connectionString, new SqlCredential(creds.UserName, secureString));
            
                
            
                SqlCommand command;
                SqlDataReader reader;
                bool isCorrectPass = false;
                connection.Open();

                
            
                command = new SqlCommand(sql, connection);
            
                command.ExecuteNonQuery();
                
                

                connection.Close();
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;

            }
        }
        public bool PostPassHash(User user)
        {
            string sql = "Update moviedb.[dbo].[Users] set [passwordHash] = '"+ user.hash + "' where username = '" + user.username + "'";
            try
            {
                
                
                SqlConnection connection;
                SecureString secureString = creds.SecurePassword;
                secureString.MakeReadOnly();


                connection = new SqlConnection(connectionString, new SqlCredential(creds.UserName, secureString));
            
                
            
                SqlCommand command;
                SqlDataReader reader;
                bool isCorrectPass = false;
                connection.Open();

                
            
                command = new SqlCommand(sql, connection);
            
                command.ExecuteNonQuery();
                
                

                connection.Close();
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;

            }
        }
        public string GetStarRating(string starId)
        {
            string rating = "";
            string sql = "SELECT AVG(m.rating) FROM [dbo].[starInfo] s Left Join [dbo].[movieinfo] m on s.movieID = m.MovieID where starID = " + starId;
            try
            {
                
                SqlConnection connection;
                SecureString secureString = creds.SecurePassword;
                secureString.MakeReadOnly();
                

                connection = new SqlConnection(connectionString, new SqlCredential(creds.UserName, secureString));
            
                
            
                SqlCommand command;
                SqlDataReader reader;

                connection.Open();

                
            
                command = new SqlCommand(sql, connection);
            
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    rating = reader.IsDBNull(0) ? "Unknown" : reader.GetSqlDouble(0).Value.ToString();
                }
                

                connection.Close();
                
                return rating;
            }
            catch (Exception e)
            {
                return e.Message.ToString();
                

            }
           
        }
    }
}