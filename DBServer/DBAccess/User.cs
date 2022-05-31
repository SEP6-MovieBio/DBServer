using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DBServer.DBAccess
{
    public class User
    {        
        [JsonPropertyName("Username")]
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Special characters are not allowed")]
        [MaxLength(256, ErrorMessage = "Username is too long, please enter a shorter one")]
        public string Username { get; set; }
        
        [JsonPropertyName("Password")]
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Special characters are not allowed")]
        [MaxLength(256, ErrorMessage = "Password is too long, please enter a shorter one")]
        public string Password { get; set; }

        [JsonPropertyName("SecurityLevel")]
        public int SecurityLevel { get; set; }
        
        /*
        public User(string username, string Password, int securityLevel)
        {
            this.Username = username;
            this.Password = Password;
            this.SecurityLevel = securityLevel;
        }
        public User()
        {
            Username = "No username";
            Password = "No password";
            SecurityLevel = 0;

        } 
        */
        
    }
}