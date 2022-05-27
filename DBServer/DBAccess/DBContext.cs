using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using DBServer.Models;
using Microsoft.VisualBasic.CompilerServices;

namespace DBServer.DBAccess
{
    public class DBContext
    {
        private NetworkCredential creds;
        private string connectionString;

        public DBContext()
        {
            creds = new NetworkCredential(Environment.GetEnvironmentVariable("DbUsername"),
                Environment.GetEnvironmentVariable("DbPassword"));

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
                    MovieInfo movieInfo = new MovieInfo(
                        reader.IsDBNull(0) ? "Unknown" : reader.GetString(0),
                        reader.IsDBNull(1) ? 0 : reader.GetDecimal(1),
                        reader.IsDBNull(2) ? "Unknown" : reader.GetString(2),
                        reader.IsDBNull(3) ? 0 : reader.GetSqlSingle(3).Value,
                        reader.IsDBNull(4) ? 0 : reader.GetInt32(4));
                    list.Add(movieInfo);
                }

                connection.Close();

                return list;
            }
            catch (Exception e)
            {
                list.Add(new MovieInfo(e.Message.ToString(), 0, "unknown", 0, 0));
                return list;
            }
        }

        public async Task<List<Movie>> GetTop200Movies()
        {
            List<int> movieIds = new List<int>();
            List<Movie> list = new List<Movie>();
            List<MovieReview> reviews = new List<MovieReview>();
            int movieid = 0;

            string sql = "select distinct top 200 [MovieID], [rating] from [dbo].[movieInfo] order by [rating] desc";
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
                    movieid = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    movieIds.Add(movieid);
                }

                connection.Close();


                foreach (int id in movieIds)
                {
                    Movie movie = await GetMovieByID(id);
                    list.Add(movie);
                }

                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                List<Movie> errorlist = new List<Movie>();
                errorlist.Add(new Movie(movieTitle: e.Message));
                return errorlist;
            }
        }

        public async Task<Movie> GetMovieByID(int id)
        {
            Movie movie = new Movie();
            List<MovieReview> reviews = new List<MovieReview>();
            List<string> directors = new List<string>();

            //string sql = $"Select id, m.title, m.[year], director, rating, votes, s.star from dbo.movies m inner join dbo.movieInfo mi on mi.title like m.title and  mi.[year] = m.[year] left join dbo.starInfo s on s.movieAppearedIn like m.title where id = {id}";

            string sql =
                $"select [MovieID], [title], [year], [Director], [rating], [votes] from [dbo].[movieInfo] where [MovieID] = {id}";
            try
            {
                SqlConnection connection;
                SecureString secureString = creds.SecurePassword;
                secureString.MakeReadOnly();

                connectionString = Environment.GetEnvironmentVariable("connString");

                connection = new SqlConnection(connectionString, new SqlCredential(creds.UserName, secureString));

                SqlCommand command;
                SqlDataReader reader;

                connection.Open();

                command = new SqlCommand(sql, connection);

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    directors.Add(reader.IsDBNull(3) ? "Unknown" : reader.GetString(3));
                    movie = new Movie(
                        reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                        reader.IsDBNull(1) ? "Unknown" : reader.GetString(1),
                        reader.IsDBNull(2) ? 0 : Decimal.ToInt32(reader.GetDecimal(2)),
                        directors,
                        reader.IsDBNull(4) ? 0 : reader.GetSqlSingle(4).Value,
                        reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                        0,
                        reviews
                    );
                }

                connection.Close();

                return movie;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);

                return new Movie();
            }
        }

        public async Task<Movie> GetMovieByRandChar(char randchar)
        {
            Movie movie = new Movie();
            List<MovieReview> reviews = new List<MovieReview>();
            List<string> directors = new List<string>();

            //string sql = $"Select id, m.title, m.[year], director, rating, votes, s.star from dbo.movies m inner join dbo.movieInfo mi on mi.title like m.title and  mi.[year] = m.[year] left join dbo.starInfo s on s.movieAppearedIn like m.title where id = {id}";

            string rows = $"select COUNT(*) from dbo.movieInfo where [title] like '%{randchar}%'";

            string sql =
                $"select [MovieID], [title], [year], [Director], [rating], [votes] from [dbo].[movieInfo] where [title] like '%{randchar}%'";
            try
            {
                SqlConnection connection;
                SecureString secureString = creds.SecurePassword;
                secureString.MakeReadOnly();

                connection = new SqlConnection(connectionString, new SqlCredential(creds.UserName, secureString));

                SqlCommand command;
                SqlDataReader reader;

                connection.Open();

                command = new SqlCommand(rows, connection);

                reader = command.ExecuteReader();

                int rowCount = 0;

                if (reader.Read())
                {
                    rowCount = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                }

                int randInt = GetRandomInt(rowCount);

                connection.Close();

                connection.Open();

                command = new SqlCommand(sql, connection);
                reader = command.ExecuteReader();

                int counter = 0;
                while (reader.Read())
                {
                    if (counter == randInt)
                    {
                        directors.Add(reader.IsDBNull(3) ? "Unknown" : reader.GetString(3));
                        movie = new Movie(
                            reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                            reader.IsDBNull(1) ? "Unknown" : reader.GetString(1),
                            reader.IsDBNull(2) ? 0 : Decimal.ToInt32(reader.GetDecimal(2)),
                            directors,
                            reader.IsDBNull(4) ? 0 : reader.GetSqlSingle(4).Value,
                            reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                            0,
                            reviews
                        );
                    }

                    counter++;
                }


                connection.Close();

                return movie;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);

                return new Movie();
            }
        }

        private static int GetRandomInt(int max)
        {
            Random random = new Random();
            int number = random.Next(0, max);
            return number;
        }

        public List<string> GetFavoriteMovieIDs(string username)
        {
            List<string> ids = new List<string>();
            string sql = "SELECT [MovieID] FROM [dbo].[FavoriteMovies] where [User] = '" + username + "'";
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
                    ids.Add(reader.IsDBNull(0) ? "Unknown" : reader.GetInt32(0).ToString());
                }

                connection.Close();

                return ids;
            }
            catch (Exception e)
            {
                ids.Add(e.Message);
                return ids;
            }
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
            string sql =
                "SELECT [username],[phoneNumber],[phoneIsHidden],[email],[emailIsHidden],[biography]FROM moviedb.[dbo].[UserInfo] where username = '" +
                username + "'";
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
                    userInfo = new UserInfo(
                        reader.IsDBNull(1) ? "Unknown" : reader.GetString(1),
                        reader.IsDBNull(2) ? true : reader.GetBoolean(2),
                        reader.IsDBNull(3) ? "Unknown" : reader.GetString(3),
                        reader.IsDBNull(4) ? true : reader.GetBoolean(4),
                        reader.IsDBNull(3) ? "" : reader.GetString(5));
                    userInfo.Username = reader.IsDBNull(0) ? "Unknown" : reader.GetString(0);
                }

                connection.Close();

                return userInfo;
            }
            catch (Exception e)
            {
                userInfo = new UserInfo(biography: e.Message.ToString());
                return userInfo;
            }
        }

        public bool PostBiography(UserInfo userInfo)
        {
            string sql = "Update moviedb.[dbo].[UserInfo] set [biography] = '" + userInfo.Biography +
                         "' where username = '" + userInfo.Username + "'";
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
            string sql = "Update moviedb.[dbo].[Users] set [passwordHash] = '" + user.Password +
                         "' where username = '" + user.Username + "'";
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
            string sql =
                "SELECT AVG(m.rating) FROM [dbo].[starInfo] s Left Join [dbo].[movieinfo] m on s.movieID = m.MovieID where starID = " +
                starId;
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

        // TESTING
        public async Task<User> GetValidatedUser(string username, string password)
        {
            User user = new User();
            string sql =
                $"select [username], [passwordHash] from [dbo].[Users] where [username] ='{username}' and [passwordHash] ='{password}'";
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
                    user = new User();
                    user.Username = reader.IsDBNull(1) ? "Unknown" : reader.GetString(1);
                    user.Password = reader.IsDBNull(2) ? "Unknown" : reader.GetString(2);
                }

                connection.Close();

                return user;
            }
            catch (Exception e)
            {
                user = new User();
                return user;
            }
        }

        public async Task<UserInfo> PostCreateUser(UserInfo user)
        {
            //From bool to bit
            int _mailBit = 0;
            int _phoneBit = 0;

            if (user.EmailIsHidden)
            {
                _mailBit = 1;
            }

            if (user.PhoneIsHidden)
            {
                _phoneBit = 1;
            }


            CreateUser(user);

            string sql =
                $"INSERT INTO [dbo].[UserInfo](username, phoneNumber, phoneIsHidden, email, emailIsHidden, biography) VALUES ('{user.Username}', {user.PhoneNumber}, {_phoneBit},'{user.Email}',{_mailBit},'{user.Biography}');";


            try
            {
                SqlConnection connection;
                SecureString secureString = creds.SecurePassword;
                secureString.MakeReadOnly();

                connection = new SqlConnection(connectionString, new SqlCredential(creds.UserName, secureString));

                SqlCommand command;
                connection.Open();
                command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
                connection.Close();

                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new UserInfo();
            }
        }

        //Creates User from UserInfo - See PostCreateUser
        private void CreateUser(UserInfo user)
        {
            string hashPassword = Hashing.GetHashString(user.Password);
            string sqlUser =
                $"INSERT INTO [dbo].[Users](username, passwordHash) VALUES ('{user.Username}', '{hashPassword}');";
            try
            {
                SqlConnection connection;
                SecureString secureString = creds.SecurePassword;
                secureString.MakeReadOnly();

                connection = new SqlConnection(connectionString, new SqlCredential(creds.UserName, secureString));

                SqlCommand command;
                connection.Open();
                command = new SqlCommand(sqlUser, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}