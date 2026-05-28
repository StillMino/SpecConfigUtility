using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SpecConfig.Core.Models
{
    // Обёртка для отображения поля в DataGrid
    public class FieldUiModel : ObservableObject
    {
        public FieldProfile Source { get; }
        
        private string _caption;
        private string _data;
        private bool _aggregate;
        private bool _visible = true;
        private string _format;
        private bool _isModified;

        public FieldUiModel(FieldProfile source)
        {
            Source = source;
            _caption = source.Caption;
            _data = source.Data;
            _aggregate = source.Aggregate;
            _visible = source.Visible;
            _format = source.Format;
        }

        public string Caption
        {
            get => _caption;
            set => SetProperty(ref _caption, value, propertyChanged: () => IsModified = true);
        }

        public string Data
        {
            get => _data;
            set => SetProperty(ref _data, value, propertyChanged: () => IsModified = true);
        }

        public bool Aggregate
        {
            get => _aggregate;
            set => SetProperty(ref _aggregate, value, propertyChanged: () => IsModified = true);
        }

        public bool Visible
        {
            get => _visible;
            set => SetProperty(ref _visible, value, propertyChanged: () => IsModified = true);
        }

        public string Format
        {
            get => _format;
            set => SetProperty(ref _format, value, propertyChanged: () => IsModified = true);
        }

        public bool IsModified
        {
            get => _isModified;
            set => SetProperty(ref _isModified, value);
        }

        public void ApplyChanges()
        {
            Source.Caption = Caption;
            Source.Data = Data;
            Source.Aggregate = Aggregate;
            Source.Visible = Visible;
            Source.Format = Format;
            IsModified = false;
        }
    }

    // Обёртка для таблицы в профиле экспорта
    public class TableUiModel : ObservableObject
    {
        public TableProfile Source { get; }
        public ObservableCollection<FieldUiModel> Fields { get; } = new();
        
        private string _caption;
        private string _filter;

        public TableUiModel(TableProfile source)
        {
            Source = source;
            _caption = source.Caption;
            _filter = source.Filter;
            foreach (var f in source.Fields)
                Fields.Add(new FieldUiModel(f));
        }

        public string Caption
        {
            get => _caption;
            set => SetProperty(ref _caption, value);
        }

        public string Filter
        {
            get => _filter;
            set => SetProperty(ref _filter, value);
        }

        public void ApplyChanges()
        {
            Source.Caption = Caption;
            Source.Filter = Filter;
            Source.Fields.Clear();
            foreach (var f in Fields)
            {
                f.ApplyChanges();
                Source.Fields.Add(f.Source);
            }
        }
    }

    // Состояние редактора для привязки к UI
    public class EditorState : ObservableObject
    {
        private string _profilePath = "";
        private string _profileName = "";
        private bool _isExportProfile = true;
        private bool _hasChanges;
        private string _statusMessage = "Готово";
        private string _errorMessage = "";

        public string ProfilePath
        {
            get => _profilePath;
            set => SetProperty(ref _profilePath, value);
        }

        public string ProfileName
        {
            get => _profileName;
            set => SetProperty(ref _profileName, value);
        }

        public bool IsExportProfile
        {
            get => _isExportProfile;
            set => SetProperty(ref _isExportProfile, value);
        }

        public bool HasChanges
        {
            get => _hasChanges;
            set => SetProperty(ref _hasChanges, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }
    }
}
