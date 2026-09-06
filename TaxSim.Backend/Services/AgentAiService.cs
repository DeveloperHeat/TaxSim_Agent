using System;
using System.Text.Json;
using System.Threading.Tasks;
using TaxSim.Backend.Models;

namespace TaxSim.Backend.Services
{
    public class AgentAiService : IAgentAiService
    {
        private readonly ILLMClient _llm;

        public AgentAiService(ILLMClient llm)
        {
            _llm = llm;
        }

        public async Task<AgentDecisionResult> EvaluateAgentDecisionAsync(TaxAgent agent, string policyText)
        {
            var prompt = BuildPrompt(agent, policyText);
            var response = await _llm.GenerateAsync(prompt);
            return ParseResponse(response);
        }

        private string BuildPrompt(TaxAgent agent, string policyText)
        {
            return $@"
You are an autonomous tax-payer agent in a simulation.

Your persona: {agent.BehavioralProfile}
Your income: {agent.Income}

The government has introduced the following tax policy:
{policyText}

Instructions:
1. Analyze the policy text based on your persona ('{agent.BehavioralProfile}').
2. Decide whether you will comply strictly or attempt to exploit loopholes / bend the rules.
3. Output your strategic intent and reasoning.

Output ONLY in valid JSON with fields:
attemptsLoophole: boolean (true if you try to exploit ambiguities or claim unauthorized exemptions/loopholes)
isCompliant: boolean (false if your behavior violates the letter or spirit of the law)
reasoning: string (explain your strategy and interpretation of the policy text)

Now decide how you will file your taxes under this policy.
";
        }

        private AgentDecisionResult ParseResponse(string rawResponse)
        {
            try
            {
                var cleanedJson = rawResponse.Trim();
                if (cleanedJson.StartsWith("```json"))
                {
                    cleanedJson = cleanedJson.Substring(7);
                }
                else if (cleanedJson.StartsWith("```"))
                {
                    cleanedJson = cleanedJson.Substring(3);
                }

                if (cleanedJson.EndsWith("```"))
                {
                    cleanedJson = cleanedJson.Substring(0, cleanedJson.Length - 3);
                }

                cleanedJson = cleanedJson.Trim();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<AgentDecisionResult>(cleanedJson, options)
                       ?? GetFallbackResult();
            }
            catch
            {
                return GetFallbackResult();
            }
        }

        private AgentDecisionResult GetFallbackResult()
        {
            return new AgentDecisionResult
            {
                AttemptsLoophole = false,
                IsCompliant = true,
                Reasoning = "Fallback: AI returned no valid JSON response."
            };
        }
    }

    public class AgentDecisionResult 
    {
        public bool AttemptsLoophole { get; set; }
        public bool IsCompliant { get; set; }
        public string Reasoning { get; set; } = string.Empty;
    }

    public interface IAgentAiService
    {
        Task<AgentDecisionResult> EvaluateAgentDecisionAsync(TaxAgent agent, string policyRule);
    }
}