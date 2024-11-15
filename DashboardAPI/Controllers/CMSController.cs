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

        [HttpGet("value")]
        public async Task<ActionResult<string>> GetValueByKeyAndLang([FromQuery] int keyId, [FromQuery] int langId)
        {
            try
            {
                // Validate input
                if (keyId <= 0 || langId <= 0)
                {
                    return BadRequest("KeyID and LangID must be positive integers");
                }

                // Check if the key exists
                var keyExists = await _context.CMSKey.AnyAsync(x => x.ID == keyId);
                if (!keyExists)
                {
                    return NotFound($"KeyID {keyId} does not exist");
                }

                // Check if the language exists
                var langExists = await _context.LanguageMaster.AnyAsync(x => x.ID == langId);
                if (!langExists)
                {
                    return NotFound($"LangID {langId} does not exist");
                }

                // Get the value
                var value = await _context.CMSKeyValue
                    .Where(x => x.KeyID == keyId && x.LangID == langId)
                    .Select(x => x.Value)
                    .FirstOrDefaultAsync();

                if (value == null)
                {
                    return NotFound($"No translation found for KeyID: {keyId} and LangID: {langId}");
                }

                return Ok(value);
            }
            catch (Exception ex)
            {
                // Log the error
                return StatusCode(500, "An error occurred while retrieving the value");
            }
        }
    }
}
