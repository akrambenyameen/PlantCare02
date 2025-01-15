using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Repositries;
using WebApplication1.Repositry.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication1.Repositry
{
    public class GenericRepositry<T> : IGenericRepositry<T> where T : class
    {
        private readonly PlantCareContext DbContext;
        public GenericRepositry(PlantCareContext DbContext)
        {
            this.DbContext = DbContext;
        }

        // Get all records
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

      
        public async Task<T> GetByIdAsync(int id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        // Get an image by ID (if applicable)
        public async Task<byte[]> GetImageByIdAsync(int id)
        {
            // Fetch the entity
            var entity = await DbContext.Set<T>().FindAsync(id);
            if (entity == null)
                return null;

            // Check for the Image property
            var propertyInfo = typeof(T).GetProperty("Image");
            if (propertyInfo == null || propertyInfo.PropertyType != typeof(byte[]))
                throw new InvalidOperationException("The entity does not have an 'Image' property of type byte[]");

            // Return the image data
            return (byte[])propertyInfo.GetValue(entity);
        }

       

        public async Task<IEnumerable<object>> GetLibraryData()
        {
            if (typeof(T) != typeof(Image))
            {
                throw new InvalidOperationException("This method is only supported for the 'Image' entity type.");
            }

            var data = await DbContext.Set<Disease>()
                                      .Include(d => d.Plant)         
                                      .Select(d => new
                                   {
                                       DiseaseId = d.Id,
                                       DiseaseName = d.Name,
                                       PlantName = d.Plant.plantName,
                                       ImageId = d.Images
                                      .Select(i => i.Id)
                                      .FirstOrDefault(),  
                                       ImageUrl = d.Images
                                      .Select(i => i.Url)
                                      .FirstOrDefault()  
                                   })
                                      .Take(35)                     
                                      .ToListAsync();

            return data;

        }


        public async Task<object> GetDiseaseDataByIdAsync(int diseaseId)
        {
            var data = await DbContext.Set<Disease>()
                .Include(d => d.Images) 
                .Include(d => d.Article)
                .Where(d=>d.Id == diseaseId)
                .Select(d => new
                {
                    diseaseId=d.Id,
                    diseaseName=d.Name,
                    Symptoms = d.Article.Symptoms,
                    Cause = d.Article.Cause,
                    Images = d.Images.Select(i => new
                    {
                        imageUrl=i.Url,
                    })

                }).FirstOrDefaultAsync();
            return data?? throw new KeyNotFoundException($"Not found"); 
        }

        public async Task<IEnumerable<object>> getEviroAnalysisDataAsync()
        {
            var data = await DbContext.Set<Image>()
     .Include(i => i.Plant) // Include related Plant entity
     .Select(i => new
     {
         plantName = i.Plant.plantName, // Select plant name
         image = i.Plant.Images         // Filter for "Healthy" images
             .Where(img => img.Url.Contains("Healthy"))
             .Select(img => img.Url)
             .FirstOrDefault(),
         id = i.PlantId                 // Select plant ID
     })
     .Distinct().Take(8)
     .ToListAsync();

            return data ?? throw new KeyNotFoundException($"Not found");
        }

        public async Task<object> getAnalysisByPlantIdAsync(int plantId)
        {
            var data = await DbContext.Set<Plant>()
                .Include(d => d.Data)
                .Where(d => d.Id == plantId)
                .Select(d => new
                {
                    plantName = d.plantName,
                    humidity = d.humidity,
                    tempreture = d.tempreture,
                    soilHumidity = d.soilHumidity,
                    lightIntensity = d.lightIntensity,
                    data = d.Data.details

                }).FirstOrDefaultAsync();
            if (data is null)
                throw new KeyNotFoundException("Data Not Found");
            return data;


        }
    }


}
