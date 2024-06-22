namespace OMA.Services
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class OMASimpleAuth
    {
        private readonly RequestDelegate _next;

        public OMASimpleAuth(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            bool authenticated = CheckAuthentication(httpContext);

            if (!authenticated)
            {
                if (!httpContext.Request.Path.StartsWithSegments("/login")
                    && httpContext.Request.Method != "POST"
                    && httpContext.Request.Path != "/OsuMultiAnalyser.styles.css")
                {
                    httpContext.Request.Path = "/login";
                    httpContext.Response.Redirect("/login");
                }
            }
            return _next(httpContext);
        }

        private bool CheckAuthentication(HttpContext httpContext)
        {
            var hash = httpContext.Request.Cookies["hash"];
            if (string.IsNullOrEmpty(hash))
            {
                return false;
            }

            return OMAService.ValidateHash(hash);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseOMASimpleAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OMASimpleAuth>();
        }
    }
}
