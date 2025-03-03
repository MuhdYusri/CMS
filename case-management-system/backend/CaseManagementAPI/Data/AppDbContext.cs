using Microsoft.EntityFrameworkCore;
using CaseManagementAPI.Models;

namespace CaseManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CustomerCase> CustomerCases { get; set; }
    }
}
