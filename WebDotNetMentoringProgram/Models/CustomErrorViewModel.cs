namespace WebDotNetMentoringProgram.Models
{
    public class CustomErrorViewModel
    {
        public string? RequestId { get; set; }

        public Exception? ThrownException { get; set; }
    }
}
