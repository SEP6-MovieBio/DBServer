
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DBServer.DBAccess;
using DBServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace DBServer.Controllers
{
        [ApiController]
        [Route("[controller]")]
        public class MovieInfoController : ControllerBase
        {
                 private DBContext dbContext = new DBContext();

                /*
                [HttpGet]
                public List<MovieInfo> GetMovieinfo()
                {
                        return dbContext.GetMovieInfo();
                }
                */
                [HttpGet]
                [Route("movieId")]
                public async Task<ActionResult<Movie>> GetMovieById([FromQuery] int id)
                {
                        try
                        {
                                Movie movie = await dbContext.GetMovieById(id);
                                return Ok(movie);
                        }
                        catch (Exception e)
                        {
                                Console.WriteLine(e);
                                throw;
                        } 
                }
                
                
        }
}
