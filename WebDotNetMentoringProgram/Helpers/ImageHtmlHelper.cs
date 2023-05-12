using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;

namespace WebDotNetMentoringProgram.Helpers
{
    public static class ImageLinkHelper
    {
        public static HtmlString ImageLink(this IHtmlHelper helper, int imageId, IHtmlContent htmlContent)
        {
            string htmlText = GetString(htmlContent);
            string htmlString = string.Format("<a href='Images/{0}'>{1}</a>", imageId, htmlText);

            return new HtmlString(htmlString);
        }

        public static string GetString(IHtmlContent content)
        {
            if (content == null)
                return string.Empty;

            using var writer = new StringWriter();
            content.WriteTo(writer, HtmlEncoder.Default);
            return writer.ToString();
        }
    }
}
