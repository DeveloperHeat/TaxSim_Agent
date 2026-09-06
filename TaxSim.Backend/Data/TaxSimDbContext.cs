using Microsoft.EntityFrameworkCore;
using TaxSim.Backend.Models;

namespace TaxSim.Backend.Data
{
    public class TaxSimDbContext : DbContext
    {
        public TaxSimDbContext(DbContextOptions<TaxSimDbContext> options) : base(options) { }

        public DbSet<TaxPolicy> TaxPolicies => Set<TaxPolicy>();
        public DbSet<TaxAgent> TaxAgents => Set<TaxAgent>();
        public DbSet<SimulationRun> SimulationRuns => Set<SimulationRun>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    }
}