namespace DBServer.DBAccess
{
    public class UserInfo
    {
        public UserInfo(string username = null, string phoneNumber = null, bool phoneIsHidden = default, string email = null, bool emailIsHidden = default, string biography = null)
        {
            this.username = username;
            this.phoneNumber = phoneNumber;
            this.phoneIsHidden = phoneIsHidden;
            this.email = email;
            this.emailIsHidden = emailIsHidden;
            this.biography = biography;
        }

        public string username { get; set; }
        public string phoneNumber { get; set; }
        public bool phoneIsHidden { get; set; }
        public string email { get; set; }
        public bool emailIsHidden { get; set; }
        public string biography { get; set; }
    }
}