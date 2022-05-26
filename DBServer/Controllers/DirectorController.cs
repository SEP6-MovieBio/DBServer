
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
        public class DirectorController : ControllerBase
        {
                 private DBContext dbContext = new DBContext();

                [HttpGet]
                [Route("top20Directors")]
                public async Task<ActionResult<List<Director>>> GetTop20Directors()
                {
                        try
                        {
                                List<Director> top20Directors = await dbContext.GetTop20Directors();

                                return Ok(top20Directors);
                        }
                        catch (Exception e)
                        {
                                Console.WriteLine(e);
                                throw;
                        } 
                }
                
                [Route("rating")]
                [HttpGet]
                public async Task<IActionResult> GetRating([FromQuery] string directorID)
                {
                        DBContext dbContext = new DBContext();
                        return new OkObjectResult(dbContext.GetDirectorRating(directorID));
                }
        }


  
        
    
}