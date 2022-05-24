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

        public async Task<Movie> GetMovieById(int id)
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

        
        
        //Testing with object between servers
        /*
        public async Task<Movie> GetDummyMovie()
        {
            MovieReview review1 = new MovieReview("Crisseren", "Too good to be true", 8.9);
            MovieReview review2 = new MovieReview("Toothbrook", "It was OK", 10);
            MovieReview review3 = new MovieReview("Ling", "Meh", 4);

            List<MovieReview> reviews = new List<MovieReview>();
            reviews.Add(review1);
            reviews.Add(review2);
            reviews.Add(review3);
            
            List<string> genreTag = new List<string>();
            genreTag.Add("Horror");
            genreTag.Add("Action");
            genreTag.Add("Romance");
            
            List<string> directors = new List<string>();
            directors.Add("James");
            directors.Add("TheBond");
            directors.Add("Shaken");
            
            List<string> starred = new List<string>();
            starred.Add("Toothbrook");
            starred.Add("Ling");
            starred.Add("Crisseren");

            Movie dummyMovie = new Movie(1,"TestMovie",2009,"imageString","This movie good",genreTag, 12,7,4,directors,starred, reviews);
            return dummyMovie;
        }
        */
    }
}