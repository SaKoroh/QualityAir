using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CityAir.UI.Features.Error
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        public IActionResult Error()
        {
            var expectionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError(expectionDetails.Error.Message, expectionDetails);
            return View("/Features/Error/Index.cshtml");
        }
    }
}
