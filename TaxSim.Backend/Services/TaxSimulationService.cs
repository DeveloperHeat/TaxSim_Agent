using Microsoft.EntityFrameworkCore;
using TaxSim.Backend.Data;
using TaxSim.Backend.Models;

namespace TaxSim.Backend.Services
{
    public class TaxSimulationService
    {
        private readonly TaxSimDbContext _context;
        private readonly IAgentAiService _ai;

        public TaxSimulationService(TaxSimDbContext context, IAgentAiService ai)
        {
            _context = context;
            _ai = ai;
        }

        public async Task<SimulationRun> RunSimulationAsync(int policyId)
        {
            var policy = await _context.TaxPolicies.FindAsync(policyId);
            if (policy == null)
            {
                throw new KeyNotFoundException("Policy not found");
            }
            
            var agents = await _context.TaxAgents.ToListAsync();

            decimal totalRevenue = 0;
            int failureCount = 0;

            var simulationRun = new SimulationRun
            {
                TaxPolicyId = policyId,
                RunTimestamp = DateTime.UtcNow
            };

            _context.SimulationRuns.Add(simulationRun);
            await _context.SaveChangesAsync();

            foreach (var agent in agents)
            {
                var decision = await _ai.EvaluateAgentDecisionAsync(agent, policy.NaturalLanguageRule);
                
                // Deterministic math calculation in C# prevents LLM revenue hallucinations
                decimal effectiveRate = 0.25m; // Baseline statutory rate

                if (decision.AttemptsLoophole && (agent.BehavioralProfile == "Corporate" || agent.BehavioralProfile == "Aggressive"))
                {
                    effectiveRate = 0.10m; // Exploited loophole rate
                }
                else if (!decision.IsCompliant)
                {
                    effectiveRate = 0.05m; // Non-compliant evasion rate
                }

                decimal calculatedTax = agent.Income * effectiveRate;
                bool isNotCompliant = !decision.IsCompliant || decision.AttemptsLoophole;

                if (isNotCompliant)
                {
                    failureCount++;
                } 

                totalRevenue += calculatedTax;

                _context.AuditLogs.Add(new AuditLog
                {
                    SimulationRunId = simulationRun.Id,
                    TaxAgentId = agent.Id,
                    ActionTaken = $"Assessed tax of ${calculatedTax:F2} ({effectiveRate * 100}% rate). Reasoning: {decision.Reasoning}",
                    RiskFlag = isNotCompliant ? "High Risk: Loophole exploited / Non-compliant" : "Compliant"
                });
            }

            simulationRun.TotalRevenueCollected = totalRevenue;
            simulationRun.ComplianceFailureCount = failureCount;

            await _context.SaveChangesAsync();
            return simulationRun;
        }
    }
}