using System;

namespace DBServer.DBAccess
{
    public class MovieInfo
    {
        public MovieInfo(string Title, decimal Year, string Director, float Rating, int Votes)
        {
            this.Title = Title;
            this.Director = Director;
            this.Year = Year;
            this.Rating = Rating;
            this.Votes = Votes;
        }
        
        public string Title { get; set; }
        
        public string Director { get; set; }
        
        public decimal Year { get; set; }
        
        public double Rating { get; set; }
        
        public int Votes { get; set; }
    }
}