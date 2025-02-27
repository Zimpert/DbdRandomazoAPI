using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbdRandomazoAPI.Services;
using DbdRandomazoAPI.Models;

namespace DbdRandomazoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RandomazoController : ControllerBase
    {
        private readonly DbdApiService _dbdApiService;
        private readonly Random _random;

        public RandomazoController(DbdApiService dbdApiService)
        {
            _dbdApiService = dbdApiService;
            _random = new Random();
        }

        [HttpGet("randomKiller")]
        public async Task<ActionResult<RandomazoResult>> GetRandomKiller()
        {
            try
            {
                // Get all killers
                var killers = await _dbdApiService.GetAllKillersAsync();
                if (killers == null || !killers.Any())
                    return NotFound("No killers found");

                // Get all killer perks
                var perks = await _dbdApiService.GetKillerPerksAsync();
                if (perks == null || !perks.Any())
                    return NotFound("No perks found");

                // Select random killer
                var randomKiller = killers[_random.Next(killers.Count)];

                // Select 4 random perks
                var randomPerks = perks.OrderBy(x => Guid.NewGuid()).Take(4).ToList();

                // Get addons for the selected killer
                var addons = await _dbdApiService.GetKillerAddonsAsync(randomKiller.Name);

                // Select 2 random addons if available
                var randomAddons = addons != null && addons.Any()
                    ? addons.OrderBy(x => Guid.NewGuid()).Take(2).ToList()
                    : new List<Addon>();

                var result = new RandomazoResult
                {
                    Killer = randomKiller,
                    Perks = randomPerks,
                    Addons = randomAddons
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
