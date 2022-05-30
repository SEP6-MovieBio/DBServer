﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DBServer.DBAccess;
using DBServer.Models;
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

        [Route("favoriteMovies")]
        [HttpGet]
        public async Task<IActionResult> GetFavoriteMoviesForUser([FromQuery] string username)
        {
            DBContext dbContext = new DBContext();
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
        
        [Route("Niceness")]
        [HttpGet]
        public async Task<IActionResult> PostPassHash([FromQuery] string username)
        {
            DBContext dbContext = new DBContext();

            return new OkObjectResult(dbContext.getNicenessUser(username));
        }
        
    }
}