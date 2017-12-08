using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HiveAPI.Models;
using HiveAPI.Data;

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

        // GET: api/Matches
        [HttpGet]
        public IEnumerable<Match> GetMatches()
        {
            return _context.Matches
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .ToList();
        }

        // GET: api/Matches/5
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

            return Ok(match);
        }

        // PUT: api/Matches/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatch([FromRoute] Guid id, [FromBody] Match match)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

            return NoContent();
        }

        // POST: api/Matches
        [HttpPost]
        public async Task<IActionResult> PostMatch([FromBody] Match match)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMatch", new { id = match.Id }, match);
        }

        // DELETE: api/Matches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch([FromRoute] Guid id)
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

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return Ok(match);
        }

        [HttpGet]
        [Route("history/{userId}")]
        public async Task<IActionResult> GetMatchHistory([FromRoute] Guid userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var matches =  await _context.Matches.Where(m => m.Player1.Id.Equals(userId) || m.Player2.Id.Equals(userId))
                                                  .OrderBy(m => m.Date)
                                                  .Include(m => m.Player1)
                                                  .Include(m => m.Player2)
                                                  .ToListAsync();
            if (matches == null)
            {
                return NotFound();
            }

            return Ok(matches);
        }

        [HttpGet]
        [Route("points/{userId}")]
        public async Task<IActionResult> GetPlayerPoints([FromRoute] Guid userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var matches = await _context.Matches.Where(m => m.Player1.Id.Equals(userId) || m.Player2.Id.Equals(userId))
                                                  .OrderBy(m => m.Date)
                                                  .Include(m => m.Player1)
                                                  .Include(m => m.Player2)
                                                  .ToListAsync();

            if (matches == null)
            {
                return NotFound();
            }

            long points = 0;

            foreach (var match in matches)
            {
                points = match.Player1.Id.Equals(userId) ? points + match.Player1Points : points + match.Player2Points;
            }            

            return Ok(points);
        }

        private bool MatchExists(Guid id)
        {
            return _context.Matches.Any(e => e.Id == id);
        }
    }
}