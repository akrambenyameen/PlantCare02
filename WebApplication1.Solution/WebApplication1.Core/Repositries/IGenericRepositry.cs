using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Core.Entities;

namespace WebApplication1.Core.Repositries
{
    public  interface IGenericRepositry<T> where T : class
    {
        
        Task <IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task<IEnumerable<object>> GetLibraryData();

        Task<object> GetDiseaseDataByIdAsync(int diseaseId);

        Task<IEnumerable<object>> getEviroAnalysisDataAsync();


        Task<object> getAnalysisByPlantIdAsync(int plantId);



    }
}
