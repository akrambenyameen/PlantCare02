//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using WebApplication1.Core.Entities;

//namespace WebApplication1.Repositry.Data.Configurations
//{
//    internal class PlantConfiguration : IEntityTypeConfiguration<Plant>
//    {
//        public void Configure(EntityTypeBuilder<Plant> P)
//        {
//            P.HasKey(p => p.Id);
//            P.Property(p => p.plantName).IsRequired().HasMaxLength(100);

//            // Relationships
//            P.HasMany(p => p.Images)
//                   .WithOne(i => i.Plant)
//                   .HasForeignKey(i => i.PlantId);

//            P.HasMany(p => p.Diseases)
//                   .WithOne(d => d.Plant)
//                   .HasForeignKey(d => d.PlantId);

//        }
//    }
//}
