namespace InventariumAPI.Middleware;

public static class ErrorHandlingExtention
{
    public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        => builder.UseMiddleware<ErrorHandling>();
}
