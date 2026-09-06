using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxSim.Backend.Data;
using TaxSim.Backend.Models;

namespace TaxSim.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PoliciesController : ControllerBase
    {
        private readonly TaxSimDbContext _context;

        public PoliciesController(TaxSimDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPolicies()
        {
            var policies = await _context.TaxPolicies.ToListAsync();
            return Ok(policies);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePolicies([FromBody] TaxPolicyDto request) 
        {
            if (string.IsNullOrEmpty(request.NaturalLanguageRule)) {
                return BadRequest("Natural language rule cannot be empty.");
            }

            var policy = new TaxPolicy
            {
                PolicyTitle = request.PolicyTitle,
                NaturalLanguageRule = request.NaturalLanguageRule,
                CreatedAt = DateTime.UtcNow
            };

            _context.TaxPolicies.Add(policy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPolicyById), new { id = policy.Id }, policy);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPolicyById(int id)
        {
            var policy = await _context.TaxPolicies.FindAsync(id);

            if (policy == null)
            {
                return NotFound();
            }

            return Ok(policy);
        }
    }

    public class TaxPolicyDto
    {
        public string PolicyTitle { get; set; } = string.Empty;
        public string NaturalLanguageRule { get; set; } = string.Empty;
    }
}