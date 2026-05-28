using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SpecConfig.Infrastructure.Com;

namespace SpecConfigUI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly NanoCadComAdapter _cadAdapter;
    private readonly ProfileEditorViewModel _editorViewModel;

    [ObservableProperty] private string _statusText = "Готово";
    [ObservableProperty] private string _currentProfileName = "";
    [ObservableProperty] private bool _isCadConnected;

    public ProfileEditorViewModel EditorViewModel => _editorViewModel;

    public MainViewModel(NanoCadComAdapter cadAdapter, ProfileEditorViewModel editorViewModel)
    {
        _cadAdapter = cadAdapter;
        _editorViewModel = editorViewModel;
        UpdateStatus();
    }

    private void UpdateStatus() => StatusText = _cadAdapter.IsConnected ? "Подключено к nanoCAD" : "Отключено";

    [RelayCommand]
    private async Task ConnectToCadAsync()
    {
        try
        {
            await _cadAdapter.ConnectAsync();
            IsCadConnected = _cadAdapter.IsConnected;
            UpdateStatus();
        }
        catch (Exception ex)
        {
            StatusText = $"Ошибка: {ex.Message}";
        }
    }

    [RelayCommand]
    private async Task OpenProfileAsync(Window? window)
    {
        if (window == null) return;
        
        var storage = window.StorageProvider;
        var result = await storage.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Открыть профиль спецификации",
            FileTypeFilter = new[] 
            { 
                new FilePickerFileType("XML профили") { Patterns = new[] { "*.xml" } }
            },
            AllowMultiple = false
        });

        if (result?.Count > 0)
        {
            var path = result[0].Path.LocalPath;
            CurrentProfileName = Path.GetFileName(path);
            await EditorViewModel.LoadProfileCommand.ExecuteAsync(path);
            StatusText = $"Открыт: {CurrentProfileName}";
        }
    }

    [RelayCommand]
    private async Task GetDocNameAsync()
    {
        if (!_cadAdapter.IsConnected) return;
        var name = await _cadAdapter.GetActiveDocumentNameAsync();
        StatusText = $"Документ nanoCAD: {name}";
    }
}
