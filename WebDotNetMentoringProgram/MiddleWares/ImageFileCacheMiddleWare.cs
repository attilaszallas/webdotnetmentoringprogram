namespace WebDotNetMentoringProgram.MiddleWares
{
    public class ImageFileCacheMiddleWare
    {
        readonly RequestDelegate _next;

        public ImageFileCacheMiddleWare(RequestDelegate next)
        { 
            _next = next;
        }

        public async Task Invoke(HttpContext context, IConfiguration configuration)
        {
            await _next(context);

            if (context.Response.ContentType == "image/bmp")
            {
                var contextRequestPath = context.Request.Path;

                var imageCacheFilePath = configuration["ImageFileCacheFolder"] + "\\" + contextRequestPath.Value.Replace("/", "") + ".bmp";

                bool cacheFileShouldBeUsed = false;
                if (File.Exists(imageCacheFilePath))
                {
                    TimeSpan ageOfFile = DateTime.Now - File.GetLastWriteTime(imageCacheFilePath);

                    int maxAgeOfFilesInDays;
                    if (!int.TryParse(configuration["MaxAgeOfFilesInDays"], out maxAgeOfFilesInDays))
                    {
                        throw new Exception("MaxAgeOfFilesInDays parameter is not an integer.");
                    }

                    TimeSpan maxAgeOfFiles = new TimeSpan(maxAgeOfFilesInDays, 0, 0, 0);
                    TimeSpan ageOfFilesInDays = DateTime.Now - File.GetLastWriteTime(imageCacheFilePath);

                    if (ageOfFilesInDays < maxAgeOfFiles)
                    {
                        cacheFileShouldBeUsed = true;
                    }
                }

                if (cacheFileShouldBeUsed)
                {
                    using FileStream fileStream = File.OpenRead(imageCacheFilePath);
                    await fileStream.CopyToAsync(context.Response.Body);
                }
                else
                {
                    var originalBody = context.Response.Body;

                    try
                    {
                        byte[] bytes;

                        using (var memoryStream = new MemoryStream())
                        {
                            context.Response.Body = memoryStream;

                            //await _next(context);

                            memoryStream.Position = 0;

                            bytes = memoryStream.ToArray();
                        }

                        using (var fileStream = new FileStream(imageCacheFilePath, FileMode.Create, System.IO.FileAccess.Write))
                        {
                            fileStream.Write(bytes, 0, bytes.Length);
                        }
                    }
                    finally
                    {
                        context.Response.Body = originalBody;
                    }
                }
            }
        }
    }

    // if not used anywhere please remove
    public static class ImageFileCacheMiddleWareExtensions
    { 
        public static IApplicationBuilder UseImageFileCacheMiddleWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageFileCacheMiddleWare>();
        }
    }
}
