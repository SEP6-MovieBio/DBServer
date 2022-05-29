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
    public class ActorController : ControllerBase
    {
        private DBContext dbContext = new DBContext();
        
        [Route("rating")]
        [HttpGet]
        public async Task<IActionResult> GetRating([FromQuery] string actorID)
        {
            return new OkObjectResult(dbContext.GetStarRating(actorID));
        }
        
        
        [HttpGet]
        [Route("top20Actors")]
        public async Task<ActionResult<List<Actor>>> GetTop20Actors()
        {
            try
            {
                List<Actor> top20Actors = await dbContext.GetTop20Actors();

                return Ok(top20Actors);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            } 
        }
        
        [HttpGet]
                [Route("searchTop10Actors")]
                public async Task<ActionResult<List<Actor>>> SearchTop10Actors([FromQuery] string searchText)
                {
                    try
                    {
                        List<Actor> top20Actors = await dbContext.SearchTop10Actors(searchText);
        
                        return Ok(top20Actors);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    } 
                }
    }
    
    
}