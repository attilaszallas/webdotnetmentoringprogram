using Microsoft.AspNetCore.Mvc;

namespace WebDotNetMentoringProgram.ViewComponents
{
    public class BreadCrumb : ViewComponent
    {
        private static string _breadCrumb ;

        public IViewComponentResult Invoke()
        {
            if (_breadCrumb != null)
            {
                _breadCrumb = _breadCrumb + " > ";
            }

            var _routeValues = HttpContext.Request.RouteValues;

            if (_routeValues.ContainsKey("controller") && _routeValues.ContainsKey("action"))
            {
                var _controller = _routeValues["controller"].ToString();
                var _action = _routeValues["action"].ToString();

                _breadCrumb = _breadCrumb + ((_action == "Index")
                    ? _controller
                    : _action);

                if (_controller == "Home" && _action == "Index")
                {
                    return Content(string.Empty);
                }
            }
            else if (_routeValues.ContainsKey("page") && _routeValues.ContainsKey("area"))
            {
                var page = _routeValues["page"].ToString();

                _breadCrumb = _breadCrumb + page.Substring(page.LastIndexOf('/') + 1);
            }
            else
            {
                throw new ArgumentException("Request RouteValues does not containt the expected keys");
            }

            ViewBag.BreadCrumb = _breadCrumb;

            return View();
        }
    }
}
