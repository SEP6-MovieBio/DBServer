using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Security;


namespace DBServer.DBAccess
{
    public class DBContext
    {
        public List<MovieInfo> GetMovieInfo()
        {
            List<MovieInfo> list = new List<MovieInfo>();
            try
            {
                string connectionString;
                SqlConnection connection;
                NetworkCredential creds = new NetworkCredential(Environment.GetEnvironmentVariable("DbUsername"),Environment.GetEnvironmentVariable("DbPassword"));

                SecureString secureString = creds.SecurePassword;
                secureString.MakeReadOnly();

                connectionString = "Data Source=dbsep6.database.windows.net;initial Catalog=moviedb";
                /*$"jdbc:sqlserver://dbsep6.database.windows.net:1433;database=moviedb;user=ad@dbsep6;password={password};" +
                "encrypt=true;trustServerCertificate=false;hostNameInCertificate=*.database.windows.net;loginTimeout=30;";*/
            
                connection = new SqlConnection(connectionString, new SqlCredential(creds.UserName, secureString));
            
                
            
                SqlCommand command;
                SqlDataReader reader;
                string sql = "";
                string output = "";

                connection.Open();

                sql = "Select top(10) title, [year], director, rating, votes from moviedb.dbo.movieinfo";
            
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
    }
}