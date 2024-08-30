using LLI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LLI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options): base(options)
        {
            
        }

        public DbSet<BarangayInformation> BrgyInformation { get; set; }

    }
}
