using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebDotNetMentoringProgram.Helpers
{
    public static class ImageLinkHelper
    {
        public static IHtmlContent ImageLink(this HtmlHelper helper, int imageId, string text)
        {
            string htmlString = string.Format("<a href='Images/{0}'>{1}</a>", imageId, text);

            return new StringHtmlContent(htmlString);
        }
    }
}