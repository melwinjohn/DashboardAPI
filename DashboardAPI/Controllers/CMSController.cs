using DashboardAPI.Data;
using DashboardAPI.DTOs;
using DashboardAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DashboardAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CMSController : ControllerBase
    {
        private readonly MultilingualDbContext _context;

        public CMSController(MultilingualDbContext context)
        {
            _context = context;
        }

        [HttpGet("languages")]
        public async Task<ActionResult<IEnumerable<LanguageMaster>>> GetLanguages()
        {
            return await _context.LanguageMaster.ToListAsync();
        }

        [HttpGet("keys")]
        public async Task<ActionResult<IEnumerable<CMSKey>>> GetKeys()
        {
            return await _context.CMSKey.ToListAsync();
        }

        [HttpGet("values")]
        public async Task<ActionResult<IEnumerable<CMSKeyValueDTO>>> GetValues()
        {
            return await _context.CMSKeyValue
                .Include(c => c.CMSKey)
                .Include(c => c.Language)
                .Select(c => new CMSKeyValueDTO
                {
                    ID = c.ID,
                    KeyID = c.KeyID,
                    LangID = c.LangID,
                    Value = c.Value,
                    KeyName = c.CMSKey.Name,
                    LanguageName = c.Language.Name
                })
                .ToListAsync();
        }

        [HttpPost("values")]
        public async Task<ActionResult<CMSKeyValue>> CreateValue(CMSKeyValue value)
        {
            _context.CMSKeyValue.Add(value);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetValues), new { id = value.ID }, value);
        }

        [HttpPut("values/{id}")]
        public async Task<IActionResult> UpdateValue(int id, CMSKeyValue value)
        {
            if (id != value.ID) return BadRequest();

            _context.Entry(value).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.CMSKeyValue.Any(e => e.ID == id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("values/{id}")]
        public async Task<IActionResult> DeleteValue(int id)
        {
            var value = await _context.CMSKeyValue.FindAsync(id);
            if (value == null) return NotFound();

            _context.CMSKeyValue.Remove(value);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
