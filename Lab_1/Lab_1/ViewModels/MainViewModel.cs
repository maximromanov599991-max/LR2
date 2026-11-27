using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Lab_1.Commands;
using Lab_1.Models;
using Lab_1.Services;
using Microsoft.Win32;

namespace Lab_1.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Свойства вкладки "Основное"

        private string _selectedTheme = "Светлая тема";
        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (SetProperty(ref _selectedTheme, value))
                {
                    ThemeManager.ApplyTheme(value);
                    OnPropertyChanged(nameof(CurrentThemeText));
                }
            }
        }

        private string _selectedLanguage = "ru";
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (SetProperty(ref _selectedLanguage, value))
                {
                    LanguageManager.Instance.CurrentLanguage = value;
                    OnPropertyChanged(nameof(CurrentThemeText));
                    OnPropertyChanged(nameof(CurrentFontSizeText));
                    OnPropertyChanged(nameof(Translations));
                }
            }
        }

        // Новое свойство: выбранный шрифт
        private string _selectedFontFamily = "Segoe UI";
        public string SelectedFontFamily
        {
            get => _selectedFontFamily;
            set => SetProperty(ref _selectedFontFamily, value);
        }

        // Размер шрифта — теперь он применяется к окну
        private double _fontSize = 14;
        public double FontSize
        {
            get => _fontSize;
            set
            {
                if (SetProperty(ref _fontSize, value))
                {
                    OnPropertyChanged(nameof(CurrentFontSizeText));
                }
            }
        }

        // Вычисляемое свойство для локализованного текста "Текущий размер: N"
        public string CurrentFontSizeText
            => string.Format(LanguageManager.Instance["CurrentFontSize"], FontSize);

        #endregion

        #region Свойства вкладки "Типы привязок"

        private string _bindingPropertyChanged;
        public string BindingPropertyChanged
        {
            get => _bindingPropertyChanged;
            set => SetProperty(ref _bindingPropertyChanged, value);
        }

        private string _bindingLostFocus;
        public string BindingLostFocus
        {
            get => _bindingLostFocus;
            set => SetProperty(ref _bindingLostFocus, value);
        }

        private string _bindingExplicit;
        public string BindingExplicit
        {
            get => _bindingExplicit;
            set => SetProperty(ref _bindingExplicit, value);
        }

        #endregion

        #region Коллекции для таблиц

        public ObservableCollection<ItemModel> SourceItems { get; set; } = new ObservableCollection<ItemModel>();
        public ObservableCollection<ItemModel> TargetItems { get; set; } = new ObservableCollection<ItemModel>();
        public ObservableCollection<ItemModel> ClipboardItems { get; set; } = new ObservableCollection<ItemModel>();

        private ItemModel _selectedSourceItem;
        public ItemModel SelectedSourceItem
        {
            get => _selectedSourceItem;
            set
            {
                if (SetProperty(ref _selectedSourceItem, value))
                    CommandManager.InvalidateRequerySuggested();
            }
        }

        private ItemModel _selectedTargetItem;
        public ItemModel SelectedTargetItem
        {
            get => _selectedTargetItem;
            set
            {
                if (SetProperty(ref _selectedTargetItem, value))
                    CommandManager.InvalidateRequerySuggested();
            }
        }

        #endregion

        #region Стек Undo / Redo

        private Stack<List<ItemModel>> _undoStack = new Stack<List<ItemModel>>();
        private Stack<List<ItemModel>> _redoStack = new Stack<List<ItemModel>>();

        #endregion

        #region Команды

        public RelayCommand AddCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand CopyCommand { get; }
        public RelayCommand PasteCommand { get; }
        public RelayCommand UpdateExplicitCommand { get; }

        public RelayCommand OpenCommand { get; }
        public RelayCommand SaveCommand { get; }
        public RelayCommand ExitCommand { get; }

        public RelayCommand UndoCommand { get; }
        public RelayCommand RedoCommand { get; }

        public RelayCommand AboutCommand { get; }

        #endregion

        public string CurrentThemeText => $"Текущая тема: {_selectedTheme}";

        public Dictionary<string, string> Translations => LanguageManager.Instance.Translations;

        public MainViewModel()
        {
            ThemeManager.ApplyTheme(_selectedTheme);

            SourceItems.Add(new ItemModel { Name = "Элемент 1", Description = "Тестовое описание", Status = "Активен" });
            SourceItems.Add(new ItemModel { Name = "Элемент 2", Description = "Еще одно описание", Status = "Ожидает" });

            SaveSnapshot();

            AddCommand = new RelayCommand(_ =>
            {
                SaveSnapshot();
                SourceItems.Add(new ItemModel { Name = "Новый элемент", Description = "Описание", Status = "Новый" });
                CommandManager.InvalidateRequerySuggested();
            });

            DeleteCommand = new RelayCommand(_ =>
            {
                if (SelectedSourceItem != null)
                {
                    SaveSnapshot();
                    SourceItems.Remove(SelectedSourceItem);
                    SelectedSourceItem = null;
                    CommandManager.InvalidateRequerySuggested();
                }
            }, _ => SelectedSourceItem != null);

            CopyCommand = new RelayCommand(_ =>
            {
                ClipboardItems.Clear();
                ClipboardItems.Add(new ItemModel
                {
                    Name = SelectedSourceItem.Name,
                    Description = SelectedSourceItem.Description,
                    Status = SelectedSourceItem.Status
                });
                CommandManager.InvalidateRequerySuggested();
            }, _ => SelectedSourceItem != null);

            PasteCommand = new RelayCommand(_ =>
            {
                SaveSnapshot();
                foreach (var item in ClipboardItems)
                {
                    TargetItems.Add(new ItemModel
                    {
                        Name = item.Name,
                        Description = item.Description,
                        Status = item.Status
                    });
                }
                ClipboardItems.Clear();
                CommandManager.InvalidateRequerySuggested();
            }, _ => ClipboardItems.Count > 0);

            UpdateExplicitCommand = new RelayCommand(_ =>
            {
                OnPropertyChanged(nameof(BindingExplicit));
                MessageBox.Show($"Значение обновлено явно: {BindingExplicit}");
            });

            OpenCommand = new RelayCommand(_ => ExecuteOpen());
            SaveCommand = new RelayCommand(_ => ExecuteSave());
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());

            UndoCommand = new RelayCommand(_ => ExecuteUndo(), _ => _undoStack.Count > 1);
            RedoCommand = new RelayCommand(_ => ExecuteRedo(), _ => _redoStack.Count > 0);

            AboutCommand = new RelayCommand(_ =>
            {
                MessageBox.Show(
                    "Лабораторная работа 2\n" +
                    "Тема: WPF, MVVM, привязки и команды\n" +
                    "Проект: Lab_1",
                    "О программе",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            });
        }

        #region Вспомогательные private-методы

        private void SaveSnapshot()
        {
            var snapshot = new List<ItemModel>();
            foreach (var item in SourceItems)
                snapshot.Add(new ItemModel { Name = item.Name, Description = item.Description, Status = item.Status });
            _undoStack.Push(snapshot);
            _redoStack.Clear();
            CommandManager.InvalidateRequerySuggested();
        }

        private void ExecuteUndo()
        {
            if (_undoStack.Count <= 1) return;
            var current = new List<ItemModel>();
            foreach (var item in SourceItems)
                current.Add(new ItemModel { Name = item.Name, Description = item.Description, Status = item.Status });
            _redoStack.Push(current);
            _undoStack.Pop();
            var previous = _undoStack.Peek();
            SourceItems.Clear();
            foreach (var item in previous)
                SourceItems.Add(new ItemModel { Name = item.Name, Description = item.Description, Status = item.Status });
            CommandManager.InvalidateRequerySuggested();
        }

        private void ExecuteRedo()
        {
            if (_redoStack.Count == 0) return;
            var next = _redoStack.Pop();
            _undoStack.Push(next);
            SourceItems.Clear();
            foreach (var item in next)
                SourceItems.Add(new ItemModel { Name = item.Name, Description = item.Description, Status = item.Status });
            CommandManager.InvalidateRequerySuggested();
        }

        private void ExecuteOpen()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*",
                Title = "Открыть файл данных"
            };
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    SaveSnapshot();
                    SourceItems.Clear();
                    foreach (var line in File.ReadAllLines(dialog.FileName))
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        var parts = line.Split(';');
                        if (parts.Length >= 3)
                            SourceItems.Add(new ItemModel
                            {
                                Name = parts[0].Trim(),
                                Description = parts[1].Trim(),
                                Status = parts[2].Trim()
                            });
                    }
                    CommandManager.InvalidateRequerySuggested();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка чтения файла: " + ex.Message, "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExecuteSave()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv",
                Title = "Сохранить данные",
                FileName = "data.csv"
            };
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    using (var writer = new StreamWriter(dialog.FileName))
                        foreach (var item in SourceItems)
                            writer.WriteLine($"{item.Name};{item.Description};{item.Status}");
                    MessageBox.Show("Данные сохранены успешно.", "Сохранение",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка записи файла: " + ex.Message, "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        #endregion
    }
}