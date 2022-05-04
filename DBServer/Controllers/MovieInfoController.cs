
using System.Collections.Generic;
using DBServer.DBAccess;
using Microsoft.AspNetCore.Mvc;

namespace DBServer.Controllers
{
        [ApiController]
        [Route("[controller]")]
        public class MovieInfoController : ControllerBase
        {
                [HttpGet]
                public List<MovieInfo> Get()
                {
                        DBContext dbContext = new DBContext();
                        
                        return dbContext.GetMovieInfo();
                }
        }
}
