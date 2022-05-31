using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DBServer.DBAccess
{
    public class UserInfo : User
    {
        
        public UserInfo( string phoneNumber = null, bool phoneIsHidden = default, string email = null, bool emailIsHidden = default, string biography = null)
        {
            this.PhoneNumber = phoneNumber;
            this.PhoneIsHidden = phoneIsHidden;
            this.Email = email;
            this.EmailIsHidden = emailIsHidden;
            this.Biography = biography;
        }
        

        /*
        public UserInfo(string username, string phoneNumber, bool phoneIsHidden, string email, bool emailIsHidden, string biography)
        {
            this.username = username;
            this.phoneNumber = phoneNumber;
            this.phoneIsHidden = phoneIsHidden;
            this.email = email;
            this.emailIsHidden = emailIsHidden;
            this.biography = biography;
        }
        */
        
        //public string username { get; set; }
        [JsonPropertyName("PhoneNumber")]
        public string PhoneNumber { get; set; }
        
        [JsonPropertyName("PhoneIsHidden")]
        public bool PhoneIsHidden { get; set; }
        
        [JsonPropertyName("Email")]
        public string Email { get; set; }
        [Required]
        [JsonPropertyName("EmailIsHidden")]
        public bool EmailIsHidden { get; set; }
        [JsonPropertyName("Biography")]
        public string Biography { get; set; }
        
    }
}