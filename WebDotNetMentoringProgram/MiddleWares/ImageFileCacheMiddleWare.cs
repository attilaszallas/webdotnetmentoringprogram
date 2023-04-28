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

                    using (var fileStream = new FileStream(configuration["ImageFileCacheFolder"] + "\\" + contextRequestPath.Value.Replace("/","") + ".bmp", FileMode.Create, System.IO.FileAccess.Write))
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

    public static class ImageFileCacheMiddleWareExtensions
    { 
        public static IApplicationBuilder UseImageFileCacheMiddleWare(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ImageFileCacheMiddleWare>();
        }
    }
}
