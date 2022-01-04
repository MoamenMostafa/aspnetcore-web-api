using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using my_books.Data.ViewModels;
using System.Net;

namespace my_books.Exceptions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureBuiltInExceptionHandler(this IApplicationBuilder applicationBuilder, ILoggerFactory loggerFactory)
        {
            applicationBuilder.UseExceptionHandler(appError => {
                appError.Run(async context =>
                {
                    var logger = loggerFactory.CreateLogger("ConfigureBuiltInExceptionHandler");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeatures = context.Features.Get<IExceptionHandlerFeature>();
                    var contextRequest = context.Features.Get<IHttpRequestFeature>();
                    if(contextFeatures != null)
                    {
                        var errorVMString = new ErrorVM()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeatures.Error.Message,
                            Path = contextRequest.Path
                        }.ToString();
                        logger.LogError(errorVMString);
                        await context.Response.WriteAsync(errorVMString);
                    }
                });
            });
        }
    
        public static void ConfigureCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
