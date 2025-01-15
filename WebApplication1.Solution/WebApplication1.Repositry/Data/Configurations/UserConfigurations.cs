using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Core.Entities;

namespace WebApplication1.Repositry.Data.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> U)
        {
            U.HasKey(u => u.Id);

            U.Property(U => U.name)
             .HasColumnType("varchar")
             .IsRequired(true)
             .HasMaxLength(100)  // Maximum length of 15
             .HasAnnotation("MinLength", 3);  // This is not a built-in method, would require custom logic if you want it validated in the database



            U.Property(U => U.isConfirmed)
             .HasDefaultValue(false).IsRequired(false);


            U.Property(U => U.activateCode)
                .IsRequired(false);

            U.Property(U => U.forgetCode).IsRequired(false) ;

            U.Property(U => U.Email)
             .HasColumnType("varchar(100)")
             .IsRequired(true);

            U.Property(U => U.PhoneNumber)
             .IsRequired(true);


            U.Property(U => U.PasswordHash)
             .IsRequired(true);
            U
         .HasMany(u => u.tokens)      // A User has many Tokens
         .WithOne(t => t.user)         // Each Token is associated with one User
         .HasForeignKey(t => t.userId) // Foreign key in Token
         .IsRequired();


        }
    }
}
