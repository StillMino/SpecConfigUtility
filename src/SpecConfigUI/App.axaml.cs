using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SpecConfigUI.ViewModels;

namespace SpecConfigUI;

public partial class App : Application
{
    private readonly IServiceProvider _provider;

    public App(IServiceProvider provider)
    {
        _provider = provider;
    }

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var vm = _provider.GetRequiredService<MainViewModel>();
            desktop.MainWindow = new Views.MainWindow { DataContext = vm };
        }
        base.OnFrameworkInitializationCompleted();
    }
}