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

        [Route("/favoriteMovies")]
        [HttpGet]
        public async Task<IActionResult> GetFavoriteMoviesForUser([FromQuery] string username)
        {
            DBContext dbContext = new DBContext();
            dbContext.GetFavoriteMovieIDs(username);
            return new OkObjectResult();
        }

        [Route("/userInfo")]
        [HttpGet]
        public async Task<IActionResult> GetInfo([FromQuery] string username)
        {
            DBContext dbContext = new DBContext();
            return new OkObjectResult(dbContext.GetUserInfo(username));
        }

        [Route("postinfo")]
        [HttpPost]
        public async Task<IActionResult> PostInfo([FromBody] JsonElement userInfo)
        {
            DBContext dbContext = new DBContext();

            return new OkObjectResult(dbContext.PostBiography(JsonSerializer.Deserialize<UserInfo>(userInfo.GetRawText())));
        }
        [Route("postHash")]
        [HttpPost]
        public async Task<IActionResult> PostPassHash([FromBody] JsonElement user)
        {
            DBContext dbContext = new DBContext();

            return new OkObjectResult(dbContext.PostPassHash(JsonSerializer.Deserialize<User>(user.GetRawText())));
        }
        
    }
}