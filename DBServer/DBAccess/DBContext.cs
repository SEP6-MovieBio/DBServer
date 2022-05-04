﻿using System;
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
            string connectionString;
            SqlConnection connection;
            NetworkCredential creds = new NetworkCredential(Environment.GetEnvironmentVariable("DbUsername"), Environment.GetEnvironmentVariable("DbPassword"));

            SecureString secureString = creds.SecurePassword;
            secureString.MakeReadOnly();

            connectionString = "Data Source=dbsep6.database.windows.net;initial Catalog=moviedb";
                /*$"jdbc:sqlserver://dbsep6.database.windows.net:1433;database=moviedb;user=ad@dbsep6;password={password};" +
                "encrypt=true;trustServerCertificate=false;hostNameInCertificate=*.database.windows.net;loginTimeout=30;";*/
            
            connection = new SqlConnection(connectionString, new SqlCredential(creds.UserName, secureString));
            
            List<MovieInfo> list = new List<MovieInfo>();
            
            SqlCommand command;
            SqlDataReader reader;
            string sql = "";
            string output = "";

            connection.Open();

            sql = "Select title, [year], director, rating, votes from moviedb.dbo.movieinfo";
            
            command = new SqlCommand(sql, connection);
            
            reader = command.ExecuteReader();

            while (reader.Read())
            {
                MovieInfo movieInfo = new MovieInfo(reader.GetString(0), reader.GetDecimal(1), reader.GetString(2),
                    reader.GetDouble(3), reader.GetInt32(4));
                list.Add(movieInfo);
            }

            connection.Close();

            return list;
        }
    }
}