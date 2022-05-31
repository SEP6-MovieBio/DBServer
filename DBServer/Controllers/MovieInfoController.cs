
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
                [Route("searchTop10Movies")]
                public async Task<ActionResult<List<Movie>>> SearchTop10Movies([FromQuery] string searchText)
                {
                        try
                        {
                                List<Movie> top200Movies = await dbContext.SearchTop10Movies(searchText);

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
                [Route("ratingByActor")]
                public async Task<ActionResult<double>> GetMovieRatingByActorRating([FromQuery] int movieid)
                {
                        try
                        {
                                Movie movie = await dbContext.GetMovieByID(movieid);


                                List<string> StarIDs = dbContext.GetStarsInMovie(movieid);

                                double rating = 0;
                                foreach (string id in StarIDs)
                                { 
                                     rating += Double.Parse(dbContext.GetStarRating(id));
                                }

                                rating = rating / StarIDs.Count;
                                
                                return Ok(rating);
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
                        Console.WriteLine("POST");

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
                [HttpPatch]
                [Route("UpdateMovieReview")]
                public async Task<ActionResult<MovieReview>> PatchMovieReview([FromBody] MovieReview review)
                {
                        Console.WriteLine("PATCHG");
                        if (!ModelState.IsValid)
                        {
                                return BadRequest(ModelState);
                        }

                        try
                        {
                                MovieReview reviewToBeUpdated = await dbContext.PatchMovieReview(review);
                                return Created($"/{reviewToBeUpdated.ReviewID}", reviewToBeUpdated);
                        }
                        catch (Exception e)
                        {
                                Console.WriteLine(e);
                                return StatusCode(500, e.Message);
                        }
                }
                [Route("MovieRatingsByDecade")]
                [HttpGet]
                public async Task<IActionResult> GetMovieRatingsByDecade()
                {
                        return new OkObjectResult(await dbContext.GetMovieRatingByDecade());
                }

        }
}
