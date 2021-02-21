using System.Net;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using SalaryCounter.Model.Dto;

namespace SalaryCounter.Api.Controller
{
    /// <summary>
    ///     Base controller
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [SwaggerDefaultResponse]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorDto), Description = "General error")]
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(ErrorDto), Description = "Not found")]
    public class BaseController : Microsoft.AspNetCore.Mvc.Controller
    {
    }
}