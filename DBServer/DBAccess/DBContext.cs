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
                        reader.IsDBNull(0)?"Unknown":reader.GetString(0), 
                        reader.IsDBNull(1)? 0: reader.GetDecimal(1), 
                        reader.IsDBNull(2)?"Unknown":reader.GetString(2),
                        reader.IsDBNull(3)?0:reader.GetSqlSingle(3).Value, 
                        reader.IsDBNull(4)?0:reader.GetInt32(4));
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
                /*
                connection.Open();
                string ids = "(";
                string realids = "";
                foreach (int id in movieIds)
                {
                    ids += id+",";
                }
                Char[] array;
                array = ids.ToCharArray();
                array[array.Length-1] = ')';
                realids = array.ToString();
                
                string sql2 = "select [MovieID], [title], [year], [Director], [rating], [votes] from [dbo].[movieInfo] where [MovieID] in " + realids;

                command = new SqlCommand(sql2, connection);
            
                reader = command.ExecuteReader();

                while (reader.Read())
                {            
                    List<string> directors = new List<string>();
                    directors.Add(reader.IsDBNull(3)?"Unknown":reader.GetString(3));
                    Movie movie = new Movie(
                        reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                        reader.IsDBNull(1) ? "Unknown" : reader.GetString(1),
                        reader.IsDBNull(2) ? 0 : Decimal.ToInt32(reader.GetDecimal(2)),
                        directors,
                        reader.IsDBNull(4) ? 0 : reader.GetSqlSingle(4).Value,
                        reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                        0,
                        reviews
                    );
                    list.Add(movie);
                }

                connection.Close();
                */
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
                
                while(reader.Read())
                {
                    directors.Add(reader.IsDBNull(3)?"Unknown":reader.GetString(3));
                    movie = new Movie(
                        reader.IsDBNull(0)?0:reader.GetInt32(0),
                        reader.IsDBNull(1)?"Unknown":reader.GetString(1),
                        reader.IsDBNull(2)? 0: Decimal.ToInt32(reader.GetDecimal(2)),
                        directors,
                        reader.IsDBNull(4)?0:reader.GetSqlSingle(4).Value,
                        reader.IsDBNull(5)?0:reader.GetInt32(5),
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
        
        public async Task<List<Director>> GetTop20Directors()
        {
            List<int> directorIds = new List<int>();
            List<Director> list = new List<Director>();
            int directorid = 0;
            
            string sql = "select distinct top 20 [person_id] from [dbo].[directors]";
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
                    directorid = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    directorIds.Add(directorid);
                }

                connection.Close();
                
                
                foreach (int id in directorIds)
                {
                    Director director = await GetDirectorByID(id);
                    list.Add(director);
                }
                
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                List<Director> errorlist = new List<Director>();
                errorlist.Add(new Director(name: e.Message));
                return errorlist;

            }
           
        }
        
        
        public async Task<Director> GetDirectorByID(int id)
        {
            Director director = new Director();
            
            string sql = $"select [id], [name], [birth] from [dbo].[people] where [id] = {id}";
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
                
                while(reader.Read())
                {
                    director = new Director(
                        reader.IsDBNull(0)?0:reader.GetInt32(0),
                        reader.IsDBNull(1)?"Unknown":reader.GetString(1),
                        Double.Parse(GetDirectorRating(id.ToString())),
                        reader.IsDBNull(2)?0:Decimal.ToInt32(reader.GetDecimal(2))
                        );
                }

                connection.Close();
                
                return director;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);

                return new Director(name: e.Message);
            }

        }
        
        public async Task<List<Actor>> GetTop20Actors()
        {
            List<int> actorIds = new List<int>();
            List<Actor> list = new List<Actor>();
            int actorId = 0;
            
            string sql = "select distinct top 20 [person_id] from [dbo].[stars]";
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
                    actorId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    actorIds.Add(actorId);
                }

                connection.Close();
                
                
                foreach (int id in actorIds)
                {
                    Actor actor = await GetActorByID(id);
                    list.Add(actor);
                }
                
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                List<Actor> errorlist = new List<Actor>();
                errorlist.Add(new Actor(name: e.Message));
                return errorlist;

            }
           
        }
        
        public async Task<Actor> GetActorByID(int id)
        {
            Actor actor = new Actor();
            
            string sql = $"select [id], [name], [birth] from [dbo].[people] where [id] = {id}";
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
                
                while(reader.Read())
                {
                    actor = new Actor(
                        reader.IsDBNull(0)?0:reader.GetInt32(0),
                        reader.IsDBNull(1)?"Unknown":reader.GetString(1),
                        Double.Parse(GetStarRating(id.ToString())),
                        reader.IsDBNull(2)?0:Decimal.ToInt32(reader.GetDecimal(2))
                    );
                }

                connection.Close();
                
                return actor;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);

                return new Actor(name: e.Message);
            }

        }
        
        public async Task<List<Actor>> SearchTop10Actors(string searchText)
        {
            List<int> actorIds = new List<int>();
            List<Actor> list = new List<Actor>();
            int actorId = 0;
            
            string sql = $"select distinct top 10 [person_id] from [dbo].[stars] s Left Join dbo.people p on p.id = s.person_id where p.id = s.person_id and p.name like '%{searchText}%'";
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
                    actorId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    actorIds.Add(actorId);
                }

                connection.Close();
                
                
                foreach (int id in actorIds)
                {
                    Actor actor = await GetActorByID(id);
                    list.Add(actor);
                }
                
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                List<Actor> errorlist = new List<Actor>();
                errorlist.Add(new Actor(name: e.Message));
                return errorlist;

            }
           
        }
        
        public async Task<List<Director>> SearchTop10Directors(string searchText)
        {
            List<int> directorIds = new List<int>();
            List<Director> list = new List<Director>();
            int directorId = 0;
            
            string sql = $"select distinct top 10 [person_id] from [dbo].[directors] d Left Join dbo.people p on p.id = d.person_id where p.id = d.person_id and p.name like '%{searchText}%'";
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
                    directorId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    directorIds.Add(directorId);
                }

                connection.Close();
                
                
                foreach (int id in directorIds)
                {
                    Director director = await GetDirectorByID(id);
                    list.Add(director);
                }
                
                return list;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                List<Director> errorlist = new List<Director>();
                errorlist.Add(new Director(name: e.Message));
                return errorlist;

            }
           
        }
        
        public async Task<List<Movie>> SearchTop10Movies(string searchText)
        {
            List<int> movieIds = new List<int>();
            List<Movie> list = new List<Movie>();
            List<MovieReview> reviews = new List<MovieReview>();
            int movieid = 0;
            
            string sql = $"select distinct top 10 [id] from [dbo].[movies] where title like '%{searchText}%'";
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
        
        public async Task<Movie> GetMovieByRandChar(char randchar)
            {
            Movie movie = new Movie();
            List<MovieReview> reviews = new List<MovieReview>();
            List<string> directors = new List<string>();
            
            //string sql = $"Select id, m.title, m.[year], director, rating, votes, s.star from dbo.movies m inner join dbo.movieInfo mi on mi.title like m.title and  mi.[year] = m.[year] left join dbo.starInfo s on s.movieAppearedIn like m.title where id = {id}";

            string rows = $"select COUNT(*) from dbo.movieInfo where [title] like '%{randchar}%'";
            
            string sql = $"select [MovieID], [title], [year], [Director], [rating], [votes] from [dbo].[movieInfo] where [title] like '%{randchar}%'";
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
                        directors.Add(reader.IsDBNull(3)?"Unknown":reader.GetString(3));
                        movie = new Movie(
                            reader.IsDBNull(0)?0:reader.GetInt32(0),
                            reader.IsDBNull(1)?"Unknown":reader.GetString(1),
                            reader.IsDBNull(2)? 0: Decimal.ToInt32(reader.GetDecimal(2)),
                            directors,
                            reader.IsDBNull(4)?0:reader.GetSqlSingle(4).Value,
                            reader.IsDBNull(5)?0:reader.GetInt32(5),
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
                    ids.Add(reader.IsDBNull(0)?"Unknown":reader.GetInt32(0).ToString());
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

        public string GetDirectorRating(string directorId)
        {
            string rating = "";
            string sql = "SELECT AVG(m.rating) FROM [dbo].[movieInfo] m where DirectorID = " + directorId;
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

        public List<string> GetStarsInMovie(int movieid)
        {
            List<string> ids = new List<string>();
            
            string sql = "SELECT [StarID] FROM [dbo].[starInfo] where movieID = " + movieid;
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
                    ids.Add(reader.IsDBNull(0)?"Unknown":reader.GetInt32(0).ToString());
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
    }
}