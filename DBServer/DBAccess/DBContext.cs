using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading.Tasks;
using DBServer.Models;


namespace DBServer.DBAccess
{
    public class DBContext
    {
        public List<MovieInfo> GetMovieInfo()
        {
            List<MovieInfo> list = new List<MovieInfo>();
            string sql = "Select top(10) title, [year], director, rating, votes from moviedb.dbo.movieinfo";
            try
            {
                
                NetworkCredential creds = new NetworkCredential(Environment.GetEnvironmentVariable("DbUsername"),Environment.GetEnvironmentVariable("DbPassword"));
                string connectionString;
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

        public async Task<Movie> GetMovieByRandChar(int id)
        {
            Movie movie = new Movie();
            List<MovieReview> reviews = new List<MovieReview>();
            List<string> directors = new List<string>();
            
            //string sql = $"Select id, m.title, m.[year], director, rating, votes, s.star from dbo.movies m inner join dbo.movieInfo mi on mi.title like m.title and  mi.[year] = m.[year] left join dbo.starInfo s on s.movieAppearedIn like m.title where id = {id}";

            string sql = $"  select [MovieID], [title], [year], [Director], [rating], [votes] from [dbo].[movieInfo] where [MovieID] = {id}";
            try
            {
                NetworkCredential creds = new NetworkCredential(Environment.GetEnvironmentVariable("DbUsername"),Environment.GetEnvironmentVariable("DbPassword"));


                string connectionString;
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
                        reader.IsDBNull(2)? 0: reader.GetDecimal(2),
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
                NetworkCredential creds = new NetworkCredential(Environment.GetEnvironmentVariable("DbUsername"),Environment.GetEnvironmentVariable("DbPassword"));

                string connectionString;
                SqlConnection connection;
                SecureString secureString = creds.SecurePassword;
                secureString.MakeReadOnly();

                connectionString = Environment.GetEnvironmentVariable("connString");
     
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
                    if (counter==randInt)
                    {
                        directors.Add(reader.IsDBNull(3)?"Unknown":reader.GetString(3));
                        movie = new Movie(
                            reader.IsDBNull(0)?0:reader.GetInt32(0),
                            reader.IsDBNull(1)?"Unknown":reader.GetString(1),
                            reader.IsDBNull(2)? 0: reader.GetDecimal(2),
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

        private static int GetRandomInt(int max){
            Random random = new Random();
            int number = random.Next(0, max);
            return number;
        }
    }
}