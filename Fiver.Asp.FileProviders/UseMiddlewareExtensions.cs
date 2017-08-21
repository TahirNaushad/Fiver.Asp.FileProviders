using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Fiver.Asp.FileProviders
{
    public static class UseMiddlewareExtensions
    {
        public static IApplicationBuilder UseHelloFileProvider(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HelloFileProviderMiddleware>();
        }
    }

    //public class HelloFileProviderMiddleware
    //{
    //    private readonly RequestDelegate next;
    //    private readonly IFileProvider fileProvider;

    //    public HelloFileProviderMiddleware(
    //        RequestDelegate next,
    //        IFileProvider fileProvider)
    //    {
    //        this.next = next;
    //        this.fileProvider = fileProvider;
    //    }

    //    public async Task Invoke(HttpContext context)
    //    {
    //        var output = new StringBuilder("");

    //        IDirectoryContents dir = this.fileProvider.GetDirectoryContents("");
    //        foreach (IFileInfo item in dir)
    //        {
    //            output.AppendLine(item.Name);
    //        }

    //        await context.Response.WriteAsync(output.ToString());
    //    }
    //}

    public class HelloFileProviderMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IFileProvider fileProvider;

        public HelloFileProviderMiddleware(
            RequestDelegate next,
            IFileProvider fileProvider)
        {
            this.next = next;
            this.fileProvider = fileProvider;
        }

        public async Task Invoke(HttpContext context)
        {
            IFileInfo file = this.fileProvider.GetFileInfo("Startup.cs");

            using (var stream = file.CreateReadStream())
            using (var reader = new StreamReader(stream))
            {
                var output = await reader.ReadToEndAsync();
                await context.Response.WriteAsync(output.ToString());
            }
        }
    }
}
