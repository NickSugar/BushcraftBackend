using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace bushcraftAPI.Controllers
{
    [Route("/[controller]")]
    public class GearController : Controller
    {
        // GET: /<controller>/
        [HttpGet(Name = nameof(GetGear))]
        public IActionResult GetGear()
        {
            throw new NotImplementedException();
        }
    }
}
