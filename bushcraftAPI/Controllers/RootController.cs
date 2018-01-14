using System;
using Microsoft.AspNetCore.Mvc;

namespace bushcraftAPI.Controllers
{
    [Route("/")]
    [ApiVersion("1.0")]
    public class RootController : Controller
    {
        [HttpGet(Name = nameof(GetRoot))]
        public IActionResult GetRoot()
        {
            var response = new
            {
                href = Url.Link(nameof(GetRoot), null),

                gear = new { href = Url.Link(nameof(GearController.GetGear), null) },

                gameInfo = new { href = Url.Link(nameof(GameInfoController.GetGameInfo), null)}
            };

            return Ok(response);
        }
    }
}
