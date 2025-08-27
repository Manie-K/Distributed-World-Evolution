using Microsoft.AspNetCore.Mvc;
using SharedLibrary.LobbyDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core.Lobby
{
    [ApiController]
    [Route("api/[controller]")]
    public class LobbyController : ControllerBase
    {
        private readonly LobbyManager _lobbyManager;

        public LobbyController(LobbyManager lobbyManager)
        {
            _lobbyManager = lobbyManager;
        }

        // GET: api/lobby
        [HttpGet]
        public IActionResult GetLobbies()
        {
            var lobbies = _lobbyManager.lobbies.Values
            .OfType<Lobby>()
            .Select(l => new LobbyDto
            {
                LobbyId = l.LobbyId,
            });

            return Ok(lobbies);
        }

    }

}
