
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
                [Route("top200Movies")]
                public async Task<ActionResult<List<Movie>>> GetTop200Movies()
                {
                        try
                        {
                                List<Movie> top200Movies = await dbContext.GetTop200Movies();

                                return Ok(top200Movies);
                        }
                        catch (Exception e)
                        {
                                Console.WriteLine(e);
                                throw;
                        } 
                }
                
                [HttpGet]
                [Route("RandomChar")]
                public async Task<ActionResult<Movie>> GetMovieByRandChar([FromQuery] char randChar)
                {
                        try
                        {
                                Movie movie = await dbContext.GetMovieByRandChar(randChar);
                                return Ok(movie);
                        }
                        catch (Exception e)
                        {
                                Console.WriteLine(e);
                                throw;
                        } 
                }
                
                [HttpGet]
                [Route("MovieID")]
                public async Task<ActionResult<Movie>> GetMovieById([FromQuery] int id)
                {
                        try
                        {
                                Movie movie = await dbContext.GetMovieByID(id);
                                return Ok(movie);
                        }
                        catch (Exception e)
                        {
                                Console.WriteLine(e);
                                throw;
                        } 
                }
                
                [HttpPost]
                [Route("MovieReview")]
                public async Task<ActionResult<MovieReview>> GetMovieReviewByMovieID([FromBody] MovieReview review)
                {
                        if (!ModelState.IsValid)
                        {
                                return BadRequest(ModelState);
                        }

                        try
                        {
                                MovieReview reviewToBeAdded = await dbContext.PostReview(review);
                                return Created($"/{reviewToBeAdded.ReviewID}", reviewToBeAdded);
                        }
                        catch (Exception e)
                        {
                                Console.WriteLine(e);
                                return StatusCode(500, e.Message);
                        }
                }

        }
}
