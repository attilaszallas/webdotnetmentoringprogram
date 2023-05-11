using Microsoft.AspNetCore.Mvc;

namespace WebDotNetMentoringProgram.ViewComponents
{
    public class BreadCrumb : ViewComponent
    {
        private static string _breadCrumb ;

        public IViewComponentResult Invoke()
        {
            var request = HttpContext.Request;
            string controller = request.RouteValues["controller"].ToString();
            string action = request.RouteValues["action"].ToString();

            if (_breadCrumb != null)
            {
                _breadCrumb = _breadCrumb + " > ";
            }

            _breadCrumb = _breadCrumb + ( (action == "Index")
                ? controller
                : action);

            if (controller == "Home" && action == "Index")
            { 
                return Content(string.Empty);
            }

            ViewBag.BreadCrumb = _breadCrumb;

            return View();
        }
    }
}
