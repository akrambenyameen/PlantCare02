using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace WebApplication1.Repositry.Data
{
    public class PlantCareContext : IdentityDbContext<User,IdentityRole<int>,int>
    {
        public PlantCareContext(DbContextOptions<PlantCareContext> options ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure IdentityUserLogin
            modelBuilder.Entity<IdentityUserLogin<int>>()
                .HasKey(ul => ul.UserId); // Set the UserId as the primary key

            // Configure IdentityUserClaim
            modelBuilder.Entity<IdentityUserClaim<int>>()
                .HasKey(uc => uc.Id); // Set the Id as the primary key

            // Configure IdentityUserRole
            modelBuilder.Entity<IdentityUserRole<int>>()
                .HasKey(ur => new { ur.UserId, ur.RoleId }); // Composite key for IdentityUserRole

            // Configure IdentityUserToken
            modelBuilder.Entity<IdentityUserToken<int>>()
                .HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name }); // Composite key for IdentityUserToken

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Disease> Diseases { get; set; }

        public DbSet<Core.Entities.Data> Data { get; set; }
    }
}
