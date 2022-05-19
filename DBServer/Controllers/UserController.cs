using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using DBServer.DBAccess;
using Microsoft.AspNetCore.Mvc;

namespace DBServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController
    {
        [HttpPost]
        public bool ValidateLogin([FromBody] JsonElement user)
        {
            DBContext dbContext = new DBContext();
            User u = JsonSerializer.Deserialize<User>(user.GetRawText());
            return dbContext.ValidateLogin(u.username, u.hash);
        }

        [Route("[controller]/info")]
        [HttpGet]
        public UserInfo GetInfo([FromQuery] string username)
        {
            DBContext dbContext = new DBContext();
            return dbContext.GetUserInfo(username);
        }
        
    }
}