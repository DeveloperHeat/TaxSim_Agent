using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaxSim.Backend.Models
{
    public class TaxPolicy
    {
        [Key]
        public int Id { get; set; }
        public string PolicyTitle { get; set; } = string.Empty;
        public string NaturalLanguageRule { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public ICollection<SimulationRun> SimulationRuns { get; set; } = new List<SimulationRun>();
    }

    public class TaxAgent
    {
        [Key]
        public int Id { get; set; }
        public string AgentName { get; set; } = string.Empty;
        public string BehavioralProfile { get; set; } = string.Empty;
        public decimal Income { get; set; }
        
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }

    public class SimulationRun
    {
        [Key]
        public int Id { get; set; }
        public int TaxPolicyId { get; set; }
        public TaxPolicy? TaxPolicy { get; set; }
        public DateTime RunTimestamp { get; set; } = DateTime.UtcNow;
        public decimal TotalRevenueCollected { get; set; }
        public int ComplianceFailureCount { get; set; }
        
        public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
    }

    public class AuditLog
    {
        [Key]
        public int Id { get; set; }
        public int SimulationRunId { get; set; }
        public SimulationRun? SimulationRun { get; set; }
        public int TaxAgentId { get; set; }
        public TaxAgent? TaxAgent { get; set; }
        public string ActionTaken { get; set; } = string.Empty;
        public string RiskFlag { get; set; } = string.Empty;
    }
}