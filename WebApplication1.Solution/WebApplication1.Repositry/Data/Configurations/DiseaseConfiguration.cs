using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApplication1.Core.Entities;

namespace WebApplication1.Repositry.Data.Configurations
{
    public class DiseaseConfiguration : IEntityTypeConfiguration<Disease>
    {
        public void Configure(EntityTypeBuilder<Disease> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name).IsRequired().HasMaxLength(100);

            // Relationships
            builder.HasOne(d => d.Plant)
                   .WithMany(p => p.Diseases)
                   .HasForeignKey(d => d.PlantId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.Article)
                   .WithOne(a => a.Disease)
                   .HasForeignKey<Disease>(d => d.ArticleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
