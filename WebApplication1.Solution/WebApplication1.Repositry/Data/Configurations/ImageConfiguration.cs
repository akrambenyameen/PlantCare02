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
    internal class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Url).IsRequired();

            // Relationships
            builder.HasOne(i => i.Plant)
                   .WithMany(p => p.Images)
                   .HasForeignKey(i => i.PlantId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Disease)
                   .WithMany(d => d.Images)
                   .HasForeignKey(i => i.DiseaseId)
                   .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
