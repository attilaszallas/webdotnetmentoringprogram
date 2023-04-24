namespace WebDotNetMentoringProgram.ViewModels
{
    public class ImageFileUpload
    {
        public int? CategoryId { get; set; }
        public IFormFile ImageFile { set; get; }
    }
}
