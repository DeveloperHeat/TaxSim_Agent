using Microsoft.AspNetCore.Mvc;
using TaxSim.Backend.Services;
using TaxSim.Backend.Models;

namespace TaxSim.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimulationController : ControllerBase
    {
        private readonly TaxSimulationService _simulationService;

        public SimulationController(TaxSimulationService simulationService)
        {
            _simulationService = simulationService;
        }

        [HttpPost("run/{policyId}")]
        public async Task<IActionResult> RunSimulation(int policyId)
        {
            try
            {
                var result = await _simulationService.RunSimulationAsync(policyId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}