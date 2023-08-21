using Serilog;
using Serilog.Events;

namespace SuperPlay.Game.Api.Extensions
{
    public static class SerilogBootstrapper
    {
        public static LoggerConfiguration GetLoggerConfiguration() => new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console();

        public static WebApplicationBuilder SetupSerilog(this WebApplicationBuilder builder)
        {
            var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .Enrich.FromLogContext()
                    .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);

            return builder;
        }
    }
}