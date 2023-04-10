using CityAir.Infrastructure;
using CityAir.UI.Features.CityAirQuality;
using Serilog;
using Serilog.Formatting.Compact;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(new RenderedCompactJsonFormatter())
    .WriteTo.Debug(outputTemplate: DateTime.Now.ToString())
    .WriteTo.File("/Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllersWithViews().AddRazorOptions(
     options =>
     {
         options.ViewLocationFormats.Add("/Features/{1}/{0}.cshtml");
     });
    builder.Services.AddRazorTemplating();
    builder.Services.AddInfrastructure();
    builder.Services.AddScoped<ICityAirViewModelFactory, CityAirViewModelFactory>();

    builder.Host.UseSerilog();

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=CityAirQuality}/{action=Index}/{id?}");

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
