using Microsoft.AspNetCore.Mvc;

namespace SalaryCounter.Api.Controller
{
    /// <summary>
    ///     Base controller
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BaseController : Microsoft.AspNetCore.Mvc.Controller
    {
    }
}