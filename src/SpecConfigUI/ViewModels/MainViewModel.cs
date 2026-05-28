using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpecConfig.Infrastructure.Com;

namespace SpecConfigUI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly NanoCadComAdapter _cad;
    [ObservableProperty] private string _statusText = "Готово";
    [ObservableProperty] private bool _isCadConnected;

    public MainViewModel(NanoCadComAdapter cad) { _cad = cad; UpdateStatus(); }
    private void UpdateStatus() => StatusText = _cad.IsConnected ? "Подключено" : "Отключено";

    [RelayCommand]
    private async Task ConnectToCadAsync()
    {
        try { await _cad.ConnectAsync(); IsCadConnected = _cad.IsConnected; UpdateStatus(); }
        catch (Exception ex) { StatusText = $"Ошибка: {ex.Message}"; }
    }

    [RelayCommand]
    private async Task GetDocNameAsync()
    {
        if (!_cad.IsConnected) return;
        var name = await _cad.GetActiveDocumentNameAsync();
        StatusText = $"Документ: {name}";
    }
}
