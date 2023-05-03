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

                var imageCacheFolder = configuration["ImageFileCacheFolder"];

                var imageCacheFilePath = imageCacheFolder + "\\" + contextRequestPath.Value.Replace("/", "") + ".bmp";

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
                    int maxCount;
                    if (!int.TryParse(configuration["MaxImageFileCacheCount"], out maxCount))
                    {
                        throw new Exception("MaxImageFileCacheCount parameter is not an integer.");
                    }
                                  
                    var fileCount = (from file in Directory.EnumerateFiles(imageCacheFolder, "*.bmp", SearchOption.AllDirectories)
                                     select file).Count();

                    if (fileCount < maxCount)
                    {
                        var originalBody = context.Response.Body;

                        try
                        {
                            byte[] bytes;

                            using (var memoryStream = new MemoryStream())
                            {
                                context.Response.Body = memoryStream;

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
