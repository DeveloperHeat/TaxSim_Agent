using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaxSim.Backend.Data
{
    public class TaxSimDbContextFactory : IDesignTimeDbContextFactory<TaxSimDbContext>
    {
        public TaxSimDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaxSimDbContext>();
            optionsBuilder.UseSqlite("Data Source=taxsim.db");

            return new TaxSimDbContext(optionsBuilder.Options);
        }
    }
}