namespace DBServer.Models
{
    public class Director
    {
        public int id { get; set; }
        public  string Name { get; set; }
        public  double Rating { get; set; }
        public  int Age { get; set; }
        public  string TopDirectedMovie { get; set; }
        public string Picture { get; set; }
    
        public Director(int id = 0, string name = null, double rating = 0, int age = 0, string topDirectedMovie = null, string picture = null)
        {
            this.id = id;
            Name = name;
            Rating = rating;
            Age = age;
            TopDirectedMovie = topDirectedMovie;
            Picture = picture;
        }
    }
}