using System.Text.Json.Serialization;

namespace DBServer.Models
{
    public class MovieReview
    {

        [JsonPropertyName("ReviewID")]
        public int ReviewID { get; set; }
        
        [JsonPropertyName("MovieID")]
        public int MovieID { get; set; }
        
        [JsonPropertyName("ReviewUsername")]
        public string ReviewUsername { get; set; }

        [JsonPropertyName("ReviewDescription")]
        public string ReviewDescription { get; set; }

        [JsonPropertyName("ReviewRating")]
        public float ReviewRating { get; set; }

        
    }
}