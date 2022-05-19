using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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

        [Route("/userInfo")]
        [HttpGet]
        public async Task<IActionResult> GetInfo([FromQuery] string username)
        {
            DBContext dbContext = new DBContext();
            return new OkObjectResult(dbContext.GetUserInfo(username));
        }
        
    }
}