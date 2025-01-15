using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Api.Errors;

namespace WebApplication1.Api.Controllers
{
    //api used for rediriction page for any notfound page
    [Route("Redirect/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi =true)]//bec route must be api/controller/code ,,but we change it
    public class ErrorRedirectionController : ControllerBase
    {
        public ActionResult error (int code)
        {
            return NotFound(new ApiErrorResponse(code));
        }
    }
}
