using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using bushcraftAPI.Models;

namespace bushcraftAPI.Controllers
{
    [Route("/[controller]")]
    public class GameInfoController : Controller
    {
        private readonly GameInfo _gameInfo;

        public GameInfoController(IOptions<GameInfo> gameInfoAccessor)
        {
            _gameInfo = gameInfoAccessor.Value;
        }

        [HttpGet(Name = nameof(GetGameInfo))]
        public IActionResult GetGameInfo()
        {
            _gameInfo.Href = Url.Link(nameof(GetGameInfo), null);

            return Ok(_gameInfo);
        }
    }
}