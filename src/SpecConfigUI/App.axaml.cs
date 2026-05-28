using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SpecConfigUI.ViewModels;
using SpecConfigUI.Views;

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
            // MainViewModel получает профильный редактор через DI
            var mainVm = _provider.GetRequiredService<MainViewModel>();
            desktop.MainWindow = new MainWindow 
            { 
                DataContext = mainVm 
            };
        }
        base.OnFrameworkInitializationCompleted();
    }
}
