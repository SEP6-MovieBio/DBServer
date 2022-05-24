using System.Threading.Tasks;
using DBServer.DBAccess;
using Microsoft.AspNetCore.Mvc;

namespace DBServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActorController
    {
        [Route("rating")]
        [HttpGet]
        public async Task<IActionResult> GetInfo([FromQuery] string actorID)
        {
            DBContext dbContext = new DBContext();
            return new OkObjectResult(dbContext.GetStarRating(actorID));
        }
    }
}