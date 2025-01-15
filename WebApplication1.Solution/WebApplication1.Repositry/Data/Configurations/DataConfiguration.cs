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
    public class DataConfiguration : IEntityTypeConfiguration<Core.Entities.Data>
    {
        public void Configure(EntityTypeBuilder<Core.Entities.Data> builder)
        {
            builder.HasKey(d=>d.Id);
            builder.HasOne(d => d.Plant)
              .WithOne(p => p.Data)
              .HasForeignKey<Core.Entities.Data>(d=> d.PlantId)
              .OnDelete(DeleteBehavior.NoAction)
              .IsRequired();
        }
    }
}
