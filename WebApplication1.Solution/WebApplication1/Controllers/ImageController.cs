using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Api.Errors;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Repositries;
using WebApplication1.Repositry.Data;

namespace WebApplication1.Api.Controllers
{

    public class ImageController : ApiBaseController
    {
        private readonly PlantCareContext context;
        private readonly IGenericRepositry<Image> genericRepositry;

        public ImageController(PlantCareContext context, IGenericRepositry<Image> genericRepositry)
        {
            this.context = context;
            this.genericRepositry = genericRepositry;
        }

        //#region get-images
        //[HttpGet("get-images")]
        //public IActionResult GetImages()
        //{
        //    // Example: Fetch images from database
        //    var images = context.Images.Select(img => new { img.Id, img.Url }).ToList();

        //    // Return as JSON response
        //    return Ok(images);
        //}
        //#endregion

        //#region get-image-by-id
        //[HttpGet("image/{id}")]
        //public async Task<IActionResult> GetImage(int id)
        //{
        //    var image = await genericRepositry.GetByIdAsync(id);  // Retrieve the image entity
        //    if (image == null)
        //        return NotFound();

        //    // Assuming 'image.Url' contains the URL where the image is accessible
        //    return Ok(new { Url = image.Url });
        //}
        //#endregion

    }
}
