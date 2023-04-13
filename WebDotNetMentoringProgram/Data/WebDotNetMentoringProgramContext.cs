using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public DbSet<WebDotNetMentoringProgram.Models.Product> Products { get; set; } = default!;

        public DbSet<WebDotNetMentoringProgram.Models.Category>? Categories { get; set; }

        public DbSet<WebDotNetMentoringProgram.Models.Supplier>? Suppliers { get; set; }
    }
}
