using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors.Infrastructure;
using heroApi.Models;
using Microsoft.AspNetCore.Mvc;
using heroApi.Repositories;

namespace heroApi.Extensions
{
    public static class ServiceExtensions
    {
        //public static void ConfigureCors(this IServiceCollection services )
        public static void ConfigureCors(this IServiceCollection services, IConfiguration config)
        {
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.WithMethods("GET", "POST", "PUT", "DELETE");
            corsBuilder.WithOrigins((config["AllowedOrigins"] ?? "http://localhost").Split(","));
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsAllowAllPolicy", corsBuilder.Build());
            });
        }

        public static void ConfigureMySqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:DefaultConnection"];
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            services.AddDbContext<HeroContext>(o => o.UseMySql(connectionString, serverVersion));
        }

        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
        
        //it is for error handler of model validation exception when direct bind request parameter to model in controller function
        public static void ConfigureModelBindingExceptionHandling(this IServiceCollection services) 
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    ValidationProblemDetails error = actionContext.ModelState
                    .Where(e => e.Value is not null && e.Value.Errors.Count > 0)
                    .Select(e => new ValidationProblemDetails(actionContext.ModelState)).First();
            
                    string ErrorMessage = "";
                    foreach (KeyValuePair<string, string[]>  errobj in error.Errors) {
                        foreach(string s in errobj.Value) {
                            ErrorMessage = ErrorMessage + s + "\r\n";
                        }
                    }
                    return new BadRequestObjectResult(new { data = 0, error = ErrorMessage});
                };
            }); 
        }
    }
}
