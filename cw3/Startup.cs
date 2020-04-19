
using cw3.DAL;
using cw3.DAL.MsSql;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using cw3.Middlewares;

namespace cw3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IStudentDbService, StudentDbService>();
            services.AddSingleton<IEnrollmentDbService, EnrollmentDbService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                if (context.Request.Headers.ContainsKey("Index"))
                {
                    var index = context.Request.Headers["Index"].ToString();

                    if (!String.IsNullOrWhiteSpace(index))
                    {
                        var service = app.ApplicationServices.GetService<IStudentDbService>();

                        if (service.GetStudent(index) != null)
                        {
                            await next();
                            return;
                        }
                    }
                }

                context.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                await HttpResponseWritingExtensions.WriteAsync(context.Response, "Invalid Student");
            });

            app.UseMiddleware<LoggingMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });            
        }
    }
}