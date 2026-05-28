using System;
using Avalonia;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SpecConfig.Core.Models;
using SpecConfig.Core.Services;
using SpecConfig.Infrastructure.Com;
using SpecConfig.Infrastructure.Sql;
using SpecConfig.Infrastructure.Xml;
using SpecConfigUI.ViewModels;

namespace SpecConfigUI;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.File("logs/SpecConfig_.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Information("SpecConfig Utility starting...");
        
        try 
        { 
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args); 
        }
        catch (Exception ex) 
        { 
            Log.Fatal(ex, "App crashed"); 
            Console.WriteLine("CRASH: " + ex);
        }
        finally 
        { 
            Log.CloseAndFlush(); 
        }
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        var services = new ServiceCollection();
        
        // Configuration
        services.AddSingleton<IConfiguration>(config);
        
        // Infrastructure services
        services.AddSingleton<StaComDispatcher>();
        services.AddSingleton<NanoCadComAdapter>();
        services.AddSingleton<ISqlDataAccessor, SqlDataAccessor>();
        services.AddSingleton<IXmlProfileLoader, XmlProfileLoader>();
        
        // Validators
        services.AddSingleton<IValidator<ExportProfile>, ExportProfileValidator>();
        services.AddSingleton<IValidator<SpecifierProfile>, SpecifierProfileValidator>();
        
        // Logging for ViewModels
        services.AddLogging(cfg => cfg.AddDebug().AddConsole().SetMinimumLevel(LogLevel.Debug));
        
        // ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<ProfileEditorViewModel>();
        
        var provider = services.BuildServiceProvider();
        
        return AppBuilder.Configure(() => new App(provider))
            .UsePlatformDetect()
            .LogToTrace();
    }
}
