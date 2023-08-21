using Serilog;
using SuperPlay.Game.Api;
using SuperPlay.Game.Api.Extensions;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = SerilogBootstrapper.GetLoggerConfiguration().CreateLogger();

    builder.SetupSerilog();

    Log.Information("Starting app setup...");

    var startup = new Startup();
    startup.ConfigureServices(builder.Services);

    var app = builder.Build();

    app.MapGet("/", () => "Superplay game server is running...");
    Log.Information("Superplay game server is running...");

    app.KickOffGameServer();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
