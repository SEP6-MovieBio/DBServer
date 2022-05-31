using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using System.Threading.Tasks;
using DBServer.DBAccess;
using DBServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace DBServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private DBContext dbContext = new DBContext();

        
        [HttpPost]
        public bool ValidateLogin([FromBody] JsonElement user)
        {
            User u = JsonSerializer.Deserialize<User>(user.GetRawText());
            return dbContext.ValidateLogin(u.Username, u.Password);
        }

        [Route("favoriteMovies")]
        [HttpGet]
        public async Task<IActionResult> GetFavoriteMoviesForUser([FromQuery] string username)
        {
            List<string> iDs = dbContext.GetFavoriteMovieIDs(username);
            List<Movie> movies = new List<Movie>();
            foreach (string id in iDs)
            {
                movies.Add(await dbContext.GetMovieByID(Int32.Parse(id)));
            }
            return new OkObjectResult(movies);
        }

        [Route("/userInfo")]
        [HttpGet]
        public async Task<IActionResult> GetInfo([FromQuery] string username)
        {
            return new OkObjectResult(dbContext.GetUserInfo(username));
        }

        [Route("postinfo")]
        [HttpPost]
        public async Task<IActionResult> PostInfo([FromBody] JsonElement userInfo)
        {
            return new OkObjectResult(dbContext.PostBiography(JsonSerializer.Deserialize<UserInfo>(userInfo.GetRawText())));
        }
        [Route("postHash")]
        [HttpPost]
        public async Task<IActionResult> PostPassHash([FromBody] UserInfo user)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                bool successfull = await dbContext.PostPassHash(user);
                return Created($"/{successfull}", successfull);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }        
        }
        
        [Route("Niceness")]
        [HttpGet]
        public async Task<IActionResult> PostPassHash([FromQuery] string username)
        {
            DBContext dbContext = new DBContext();

            return new OkObjectResult(dbContext.getNicenessUser(username));
        }
        
        
        
        
        [Route("LoginCheck")]
        [HttpGet]
        public async Task<ActionResult<User>> GetValidatedUser([FromQuery] string username, [FromQuery] string password)
        {
            try
            {
                User user = await dbContext.GetValidatedUser(username, password);
                //return user; 
                return Ok(user);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
                // TODO Add more exceptions? 404?
            }
        }
        
        [Route("postUser")]
        [HttpPost]
        public async Task<ActionResult<UserInfo>> PostCreateUser([FromBody] UserInfo userInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                UserInfo userToBeAdded = await dbContext.PostCreateUser(userInfo);
                return Created($"/{userToBeAdded.Username}", userToBeAdded);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
        
        [Route("postFavouriteMovie")]
        [HttpPost]
        public async Task<ActionResult<string>> PostFavouriteMovie([FromQuery] string username, [FromQuery] int movieID)
        {
            Console.WriteLine($"1)String is: {username} and {movieID}");
            //Console.WriteLine($"1)String is: {listInfoJson}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await dbContext.PostFavouriteMovie(username,movieID);
                return Created($"/{username}", username);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}