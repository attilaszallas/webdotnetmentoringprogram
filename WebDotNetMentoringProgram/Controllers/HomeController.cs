using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using WebDotNetMentoringProgram.Models;

namespace WebDotNetMentoringProgram.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult CustomError()
		{
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>().Error;

			string? ExceptionMessage = exceptionDetails.Message;
			string? ExceptionStackTrace = exceptionDetails.StackTrace;

			// Logging the unhandled exception as Error
            _logger.LogError($"Unhandled exception occurred while processing your request. \n Message: {ExceptionMessage} \n StackTrace: {ExceptionStackTrace} ");

			if (exceptionDetails != null)
			{
				return View(new CustomErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ThrownException = exceptionDetails });
			}
			else
			{
				return View();
			}
		}
	}
}