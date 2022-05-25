using System.Threading.Tasks;
using DBServer.DBAccess;
using Microsoft.AspNetCore.Mvc;

namespace DBServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Director
    {
        [Route("rating")]
        [HttpGet]
        public async Task<IActionResult> GetRating([FromQuery] string directorID)
        {
            DBContext dbContext = new DBContext();
            return new OkObjectResult(dbContext.GetDirectorRating(directorID));
        }
    }
}