using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DBServer.Models
{
    public class Movie
    {
        /*
        public Movie(int movieId, string movieTitle, decimal year,string imageSource, string movieDescription, List<string> genreTag, int votes, float rating, float ratingBasedOnActors, List<string> directors, List<string> starredActors, List<MovieReview> reviews)
        {
            this.MovieId = movieId;
            this.MovieTitle = movieTitle;
            this.Year = year;

             this.ImageSource = imageSource;
            this.MovieDescription = movieDescription;
            this.GenreTag = genreTag;
            this.Votes = votes;
            this.Rating = rating;
            this.RatingBasedOnActors = ratingBasedOnActors;
            this.Directors = directors;
            this.StarredActors = starredActors;
            
            this.Reviews = reviews;

        }
        */
        public Movie()
        {
            this.MovieId = 0;
            this.MovieTitle = "No title";
            this.Year = 0;
            this.Director = null;
            this.Rating = 0;
            this.Votes = 0;
            this.RatingBasedOnActors = 0;
            this.Reviews = null;
        }
        public Movie(int movieId, string movieTitle, int year,List<string> director,float rating, int votes, float ratingBasedOnActors, List<MovieReview> reviews)
        {
            this.MovieId = movieId;
            this.MovieTitle = movieTitle;
            this.Year = year;
            this.Director = director;
            this.Rating = rating;
            this.Votes = votes;
            this.RatingBasedOnActors = ratingBasedOnActors;
            this.Reviews = reviews;
        }
        
        [JsonPropertyName("movieId")]
        public int MovieId { get; set; }

        [JsonPropertyName("movieTitle")]
        public string MovieTitle { get; set; }

        [JsonPropertyName("Year")]
        public int Year { get; set; }

        /*
        [JsonPropertyName("imageSource")]
        public string ImageSource { get; set; }
        
        [JsonPropertyName("movieDescription")]
        public string MovieDescription { get; set; }
        
        [JsonPropertyName("genreTag")]
        public List<string> GenreTag { get; set; }

        */
        [JsonPropertyName("rating")]
        public float Rating { get; set; }
        
        [JsonPropertyName("votes")]
        public int Votes { get; set; }
        
        [JsonPropertyName("director")]
        public List<string> Director { get; set; }   
        
        [JsonPropertyName("ratingBasedOnActors")]
        public float RatingBasedOnActors { get; set; }

        [JsonPropertyName("reviews")]
        public List<MovieReview> Reviews { get; set; }    
        
         
        /*
        [JsonPropertyName("starredActors")]
        public List<string> StarredActors { get; set; }    
        */

        /*
        public string ToString()
        {
            // Converts the values into jsonFormat

            return "{"
                   + "\"movieId\":" + "\"" + MovieId + "\","
                   + "\"MovieTitle\":" + "\"" + MovieTitle + "\","
                   + "\"year\":" + "\"" + Year + "\","
                   + "\"ImageSource\":" + "\"" + ImageSource + "\","
                   + "\"movieDescription\":" + "\"" + MovieDescription + "\","
                   + "\"genreTag\":" + "\"" + GenreTag + "\","
                   + "\"votes\":" + "\"" + Votes + "\","
                   + "\"rating\":" + "\"" + Rating + "\","
                   + "\"ratingBasedOnActors\":" + RatingBasedOnActors + ","
                   + "\"directors\":" + Directors + ","
                   + "\"starredActors\":" + "\"" + StarredActors + "\"" +","
                   + "\"reviews\":" + Reviews
                   + "}";
        }
        */
    }
}