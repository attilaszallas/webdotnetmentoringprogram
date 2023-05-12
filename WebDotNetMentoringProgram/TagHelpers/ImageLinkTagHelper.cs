using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebDotNetMentoringProgram.TagHelpers
{
    public class ImageLinkTagHelper : TagHelper
    {
        public int ImageId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            output.TagName = "a";
            output.Attributes.SetAttribute("href", "Images/" + ImageId);
        }
    }
}
