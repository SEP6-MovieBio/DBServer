using System.Text.Json.Serialization;

namespace DBServer.Models
{
    public class MovieReview
    {
        public MovieReview(string reviewUsername, string reviewDescription, double reviewRating)
        {
            this.ReviewUsername = reviewUsername;
            this.ReviewDescription = reviewDescription;
            this.ReviewRating = reviewRating;
        }
        
        [JsonPropertyName("reviewUsername")]
        public string ReviewUsername { get; set; }

        [JsonPropertyName("reviewDescription")]
        public string ReviewDescription { get; set; }

        [JsonPropertyName("reviewRating")]
        public double ReviewRating { get; set; }

        
        public string ToString()
        {
            // Converts the values into jsonFormat


            return "{"
                   + "\"reviewUsername\":" + "\"" + ReviewUsername + "\","
                   + "\"reviewDescription\":" + "\"" + ReviewDescription + "\","
                   + "\"reviewRating\":" + "\"" + ReviewRating + "\","
                   + "}";
        }
    }
}