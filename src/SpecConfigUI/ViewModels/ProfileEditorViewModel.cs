using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SpecConfig.Core.Models;
using SpecConfig.Core.Services;

namespace SpecConfigUI.ViewModels;

public partial class ProfileEditorViewModel : ObservableObject
{
    private readonly IXmlProfileLoader _loader;
    private readonly IValidator<ExportProfile> _exportValidator;
    private readonly IValidator<SpecifierProfile> _specValidator;
    private readonly ILogger<ProfileEditorViewModel> _logger;

    [ObservableProperty] private EditorState _state = new();
    [ObservableProperty] private ObservableCollection<TableUiModel> _tables = new();
    [ObservableProperty] private ObservableCollection<FieldUiModel> _selectedTableFields = new();
    [ObservableProperty] private TableUiModel? _selectedTable;
    [ObservableProperty] private FieldUiModel? _selectedField;
    [ObservableProperty] private string _searchText = "";

    public ProfileEditorViewModel(
        IXmlProfileLoader loader,
        IValidator<ExportProfile> exportValidator,
        IValidator<SpecifierProfile> specValidator,
        ILogger<ProfileEditorViewModel> logger)
    {
        _loader = loader;
        _exportValidator = exportValidator;
        _specValidator = specValidator;
        _logger = logger;
    }

    [RelayCommand]
    private async Task LoadProfileAsync(string path)
    {
        try
        {
            State.ErrorMessage = "";
            State.ProfilePath = path;
            State.ProfileName = Path.GetFileNameWithoutExtension(path);
            State.IsExportProfile = path.Contains("Export", System.StringComparison.OrdinalIgnoreCase);

            if (State.IsExportProfile)
            {
                var profile = await _loader.LoadExportAsync(path);
                LoadExportToUi(profile);
            }
            else
            {
                var profile = await _loader.LoadSpecifierAsync(path);
                LoadSpecifierToUi(profile);
            }
            State.HasChanges = false;
            State.StatusMessage = $"Загружен: {State.ProfileName}";
            _logger.LogInformation("Loaded profile: {Path}", path);
        }
        catch (Exception ex)
        {
            State.ErrorMessage = $"Ошибка загрузки: {ex.Message}";
            State.StatusMessage = "Ошибка";
            _logger.LogError(ex, "Failed to load profile: {Path}", path);
        }
    }

    private void LoadExportToUi(ExportProfile profile)
    {
        Tables.Clear();
        foreach (var t in profile.Tables)
            Tables.Add(new TableUiModel(t));
        if (Tables.Any()) SelectedTable = Tables[0];
    }

    private void LoadSpecifierToUi(SpecifierProfile profile)
    {
        // Для спецификатора создаём виртуальную таблицу с основными полями
        Tables.Clear();
        var virtualTable = new TableUiModel(new TableProfile { Caption = "Основные поля" });
        virtualTable.Fields.Add(new FieldUiModel(new FieldProfile { Caption = "Позиция", Data = "BOM_NUMBER", Visible = true }));
        virtualTable.Fields.Add(new FieldUiModel(new FieldProfile { Caption = "Комментарий", Data = "BOM_COMMENT", Visible = true }));
        Tables.Add(virtualTable);
        SelectedTable = virtualTable;
    }

    [RelayCommand]
    private void SaveProfile(string path)
    {
        // TODO: Реализовать сохранение после реализации генератора XML
        State.StatusMessage = "Сохранение (в разработке)";
    }

    [RelayCommand]
    private void AddField()
    {
        if (SelectedTable == null) return;
        var newField = new FieldUiModel(new FieldProfile 
        { 
            Caption = "Новое поле", 
            Data = "CUSTOM_" + (SelectedTable.Fields.Count + 1),
            Visible = true 
        });
        SelectedTable.Fields.Add(newField);
        SelectedTable.Source.Fields.Add(newField.Source);
        State.HasChanges = true;
    }

    [RelayCommand]
    private void MoveFieldUp(FieldUiModel field)
    {
        if (SelectedTable == null) return;
        var idx = SelectedTable.Fields.IndexOf(field);
        if (idx > 0)
        {
            SelectedTable.Fields.Move(idx, idx - 1);
            State.HasChanges = true;
        }
    }

    [RelayCommand]
    private void MoveFieldDown(FieldUiModel field)
    {
        if (SelectedTable == null) return;
        var idx = SelectedTable.Fields.IndexOf(field);
        if (idx >= 0 && idx < SelectedTable.Fields.Count - 1)
        {
            SelectedTable.Fields.Move(idx, idx + 1);
            State.HasChanges = true;
        }
    }

    [RelayCommand]
    private void RemoveField(FieldUiModel field)
    {
        if (SelectedTable == null) return;
        SelectedTable.Fields.Remove(field);
        SelectedTable.Source.Fields.Remove(field.Source);
        State.HasChanges = true;
    }

    partial void OnSelectedTableChanged(TableUiModel? value)
    {
        SelectedTableFields.Clear();
        if (value != null)
            foreach (var f in value.Fields) SelectedTableFields.Add(f);
    }

    [RelayCommand]
    private void ApplyChanges()
    {
        if (SelectedTable != null)
        {
            SelectedTable.ApplyChanges();
            State.HasChanges = false;
            State.StatusMessage = "Изменения применены";
        }
    }

    [RelayCommand]
    private void CancelChanges()
    {
        // TODO: Перезагрузить профиль из файла или отменить изменения
        State.HasChanges = false;
        State.StatusMessage = "Изменения отменены";
    }
}
