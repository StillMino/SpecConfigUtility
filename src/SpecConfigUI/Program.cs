using System;
using Avalonia;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
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
            .WriteTo.Console()
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
        services.AddSingleton<IConfiguration>(config);
        services.AddSingleton<StaComDispatcher>();
        services.AddSingleton<NanoCadComAdapter>();
        services.AddSingleton<ISqlDataAccessor, SqlDataAccessor>();
        services.AddSingleton<IXmlProfileLoader, XmlProfileLoader>();
        services.AddTransient<MainViewModel>();
        
        var provider = services.BuildServiceProvider();
        
        // Простейшая конфигурация, которая работает везде:
        return AppBuilder.Configure(() => new App(provider))
            .UsePlatformDetect()
            .LogToTrace();
    }
}