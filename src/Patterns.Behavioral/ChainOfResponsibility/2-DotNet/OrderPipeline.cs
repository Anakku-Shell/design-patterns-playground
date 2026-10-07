using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Patterns.Behavioral.ChainOfResponsibility.DotNet;

/// <summary>
/// An ASP.NET Core middleware pipeline built in memory, without a web server: each <c>app.Use</c> is a link
/// of the chain and can stop the request. Guide: §6.1.
/// </summary>
public static class OrderPipeline
{
    public static RequestDelegate Build(ICollection<string> trace)
    {
        ArgumentNullException.ThrowIfNull(trace);
        var app = new ApplicationBuilder(new ServiceCollection().BuildServiceProvider());

        app.Use(async (context, next) =>
        {
            trace.Add("correlation");
            context.Response.Headers["X-Correlation-Id"] = "abc-123";
            await next(context);
        });
        app.Use(async (context, next) =>
        {
            trace.Add("auth");
            if (!context.Request.Headers.ContainsKey("X-Customer-Id"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return; // short-circuit: next is never called, the rest of the chain never runs
            }
            await next(context);
        });
        app.Use(async (context, next) =>
        {
            trace.Add("log");
            await next(context);
        });
        app.Run(async context => // terminal: no next
        {
            trace.Add("endpoint");
            await context.Response.WriteAsync("order accepted");
        });

        return app.Build(); // one RequestDelegate: the whole chain, nested
    }
}
