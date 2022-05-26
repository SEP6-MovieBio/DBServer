namespace DBServer.Models
{
    public class Actor
    {
        public int id { get; set; }
        public  string Name { get; set; }
        public  double Rating { get; set; }
        public  int Age { get; set; }
        public  string KnownFor { get; set; }
        public string Picture { get; set; }
    
        public Actor(int id = 0, string name = null, double rating = 0, int age = 0, string knownFor = null, string picture = null)
        {
            this.id = id;
            Name = name;
            Rating = rating;
            Age = age;
            KnownFor = knownFor;
            Picture = picture;
        }
    }
}