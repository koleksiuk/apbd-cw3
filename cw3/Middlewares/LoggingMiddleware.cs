using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace cw3.Middlewares
{
    public class LoggingMiddleware
    {
        private const string logPath = "log/request.log";
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var requestLog = await FormatRequest(httpContext.Request);
                
            using (var log = File.AppendText(logPath))
            {
                log.WriteLine($"{requestLog}");
                log.Close();
            }
            
            //Our code
            await _next(httpContext);
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            request.EnableBuffering();

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            // convert the byte[] into a string using UTF8 encoding...
            var bodyAsText = Encoding.UTF8.GetString(buffer);

            request.Body.Position = 0;

            return $"[{DateTime.Now}] {request.Method} {request.Path}, Body: '{bodyAsText}', QueryString: '{request.QueryString}'";
        }
    }
}
