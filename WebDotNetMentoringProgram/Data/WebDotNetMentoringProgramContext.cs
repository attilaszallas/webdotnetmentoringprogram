using Microsoft.EntityFrameworkCore;
using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgram.Data
{
    public class WebDotNetMentoringProgramContext : DbContext
    {
        public WebDotNetMentoringProgramContext (DbContextOptions<WebDotNetMentoringProgramContext> options)
            : base(options)
        {
        }

        // please remove WebDotNetMentoringProgram.Models from DbSet
        public DbSet<Product> Products { get; set; } = default!;

        public DbSet<Category>? Categories { get; set; }

        public DbSet<Supplier>? Suppliers { get; set; }
    }
}
