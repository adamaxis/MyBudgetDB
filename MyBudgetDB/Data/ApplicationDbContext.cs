using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyBudgetDB.Models;

namespace MyBudgetDB.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly AppSecrets _DbInfo;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IOptions<AppSecrets> DbInfo)
            : base(options)
        {
            _DbInfo = DbInfo.Value ?? throw new ArgumentException(nameof(DbInfo));
        }

        // Added for unit test
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<UserBudget> Budgets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => 
            options.UseSqlServer($"{_DbInfo.Database};User ID={_DbInfo.User};Password={_DbInfo.Password};{_DbInfo.Options};");
    }
}
