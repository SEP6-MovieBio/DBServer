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
        public async Task<ActionResult<List<Actor>>> GetTop20Directors()
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
    }
    
    
}