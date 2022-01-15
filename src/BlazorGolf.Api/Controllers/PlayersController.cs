using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BlazorGolf.Core.Models;
using FluentValidation;
using BlazorGolf.Api.Services;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;

namespace BlazorGolf.Api.Controllers
{
    [ApiController]
    [Route("api/players")]
    [EnableCors("AllowEveryone")]
    public class PlayersController : ControllerBase
    {
        private readonly ILogger<PlayersController> _logger;

        public PlayersController(ILogger<PlayersController> logger)
        {
            _logger = logger;
        }

        // GET: api/players 
        [HttpGet(Name = "GetPlayers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation($"GetPlayers called.");
            var players = await RetrievePlayers();
            _logger.LogInformation("GetCourses returning");
            return Ok(players);
        }

        private async Task<IEnumerable<Player>> RetrievePlayers()
        {
            var players = new List<Player>() {
                new Player() { FirstName = "Pete", LastName = "Skelly" },
                new Player() { FirstName = "Krise", LastName = "Sheedy" }
            };

            await Task.Delay(2000);
            return players;
        }
   }
}