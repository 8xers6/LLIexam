using Microsoft.EntityFrameworkCore;

namespace LLI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options): base(options)
        {
            
        }



    }
}
