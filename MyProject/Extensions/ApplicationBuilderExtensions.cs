using Microsoft.AspNetCore.Diagnostics;
using MyProject.Exceptions;
using static System.Net.Mime.MediaTypeNames;

namespace MyProject.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                if (context.RequestServices.GetService<IProblemDetailsService>() is { } problemDetailsService)
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exceptionType = exceptionHandlerFeature?.Error;

                    if (exceptionType is NotFoundException notFoundException)
                    {
                        context.Response.ContentType = Text.Plain;
                        context.Response.StatusCode = StatusCodes.Status404NotFound;
                        await problemDetailsService.WriteAsync(new ProblemDetailsContext
                        {
                            HttpContext = context,
                            ProblemDetails =
                            {
                                Title = "Item Not Found",
                                Detail = notFoundException.Message
                            }
                        });
                        await context.Response.CompleteAsync();
                    }
                }
            });
        });

        return app;
    }
}