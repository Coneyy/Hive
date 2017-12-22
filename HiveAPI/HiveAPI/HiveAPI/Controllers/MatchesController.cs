using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HiveAPI.Models;
using HiveAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace HiveAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Matches")]

    
    public class MatchesController : Controller
    {
        private readonly HiveApiContext _context;

        public MatchesController(HiveApiContext context)
        {
            _context = context;
        }
     
        [HttpGet]
        public async Task<IEnumerable<MatchDto>> GetMatches()
        {
            return await _context.Matches
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .Select(m=> new MatchDto(m))
                .ToListAsync();
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatch([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var match = await _context.Matches.SingleOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound();
            }

            return Ok(new MatchDto(match));
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] Match match)
        {
            if (id != match.Id)
            {
                return BadRequest();
            }

            _context.Entry(match).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Match match)
        {
            match.Player1 = await _context.Users.SingleOrDefaultAsync(u => u.Email.Equals(match.Player1.Email));
            match.Player2 = await _context.Users.SingleOrDefaultAsync(u => u.Email.Equals(match.Player2.Email));

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMatch", new { id = match.Id }, new MatchDto(match));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var match = await _context.Matches.SingleOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return Ok(match);
        }

        [Authorize]
        [HttpGet]
        [Route("history/{userId}")]
        public async Task<IActionResult> GetMatchHistory([FromRoute] Guid userId)
        {

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id.Equals(userId));
            if (user == null)
            {
                return NotFound("Użytkownik o podanym Id nie istnieje");
            }


            var matches =  await _context.Matches.Where(m => m.Player1.Id.Equals(userId) || m.Player2.Id.Equals(userId))
                                                  .OrderBy(m => m.Date)
                                                  .Include(m => m.Player1)
                                                  .Include(m => m.Player2)
                                                  .Select(m => new MatchDto(m))
                                                  .ToListAsync();
            if (matches == null)
            {
                return NoContent();
            }

            return Ok(matches);
        }

        [Authorize]
        [HttpGet]
        [Route("points/{userId}")]
        public async Task<IActionResult> GetPlayerPoints([FromRoute] Guid userId)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id.Equals(userId));
            if (user == null)
            {
                return NotFound("Użytkownik o podanym Id nie istnieje");
            }

            var matches = await _context.Matches.Where(m => m.Player1.Id.Equals(userId) || m.Player2.Id.Equals(userId))
                                                  .OrderBy(m => m.Date)
                                                  .Include(m => m.Player1)
                                                  .Include(m => m.Player2)
                                                  .ToListAsync();
            long points = 0;
            if (matches == null)
            {
                return Ok(points);
            }            

            foreach (var match in matches)
            {
                points = match.Player1.Id.Equals(userId) ? points + match.Player1Points : points + match.Player2Points;
            }            

            return Ok(points);
        }

        [Authorize]
        [HttpGet]
        [Route("highscores")]
        public async Task<IActionResult> GetHighscores()
        {

            var players = await _context.Matches.Select(m => m.Player1).Distinct().ToListAsync();
            players.AddRange(_context.Matches.Select(m => m.Player2).Distinct().ToList());
            players = players.Distinct().ToList();
            var highscores = new Dictionary<User, int>();
            players.ForEach(p => highscores.Add(p, 0));

            await _context.Matches.ForEachAsync(m =>
                                               {
                                                   highscores[m.Player1] +=(int) m.Player1Points;
                                                   highscores[m.Player2] += (int)m.Player2Points;
                                               });
            if (!highscores.Any())
            {
                return NoContent();
            }

            var highscoresDto = new Dictionary<UserDto, int>();
            
            foreach (var kvp in highscores) {
                highscoresDto.Add(new UserDto(kvp.Key), kvp.Value);
            }

            return Ok(highscoresDto.Select( hs => new { player = hs.Key, points = hs.Value }).OrderByDescending(o => o.points));
        }

        private bool MatchExists(Guid id)
        {
            return _context.Matches.Any(e => e.Id == id);
        }
    }
}