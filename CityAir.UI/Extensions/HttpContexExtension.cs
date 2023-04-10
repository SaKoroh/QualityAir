namespace CityAir.UI.Extensions
{
    public static class HttpContexExtension
    {
        public static string GetCurrentUrl(this HttpContext httpContext)
        {
            return string.Format("{0}://{1}{2}{3}", httpContext.Request.Scheme,
                httpContext.Request.Host, httpContext.Request.Path,
                httpContext.Request.QueryString);
        }
    }
}
