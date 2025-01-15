using Microsoft.AspNetCore.Mvc;
using WebApplication1.Api.Controllers;
using WebApplication1.Api.Errors;
using WebApplication1.Core.Entities;
using WebApplication1.Core.Repositries;

public class libraryController : ApiBaseController
{
    private readonly IGenericRepositry<Image> repositry;

    public libraryController(IGenericRepositry<Image> repositry)
    {
        this.repositry = repositry;
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetLibraryData()
    {
        try
        {
            var data = await repositry.GetLibraryData();
            return Ok(data);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiErrorResponse(401, "An error occurred"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
        }
  
    }

    [HttpGet("{id}")]
    public async Task <IActionResult> GetLibiraryDataById(int id)
    {
        var result = await repositry.GetDiseaseDataByIdAsync(id);
        if(result == null)
            return BadRequest(new ApiErrorResponse(401,"An Error Occured"));
        return Ok(result);
    }
}


