using Lab_1.Commands;
using Lab_1.Models;
using Lab_1.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Lab_1.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _selectedTheme = "Light";
        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (SetProperty(ref _selectedTheme, value))
                {
                    // Применяем тему при изменении
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
                    // Меняем язык
                    LanguageManager.Instance.CurrentLanguage = value;
                    OnPropertyChanged(nameof(CurrentThemeText));
                    OnPropertyChanged(nameof(Translations));
                }
            }
        }

        private double _fontSize = 14;
        public double FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }

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

        public ObservableCollection<ItemModel> SourceItems { get; set; } = new ObservableCollection<ItemModel>();
        public ObservableCollection<ItemModel> TargetItems { get; set; } = new ObservableCollection<ItemModel>();
        public ObservableCollection<ItemModel> ClipboardItems { get; set; } = new ObservableCollection<ItemModel>();

        public ItemModel SelectedSourceItem { get; set; }
        public ItemModel SelectedTargetItem { get; set; }

        public RelayCommand AddCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand CopyCommand { get; }
        public RelayCommand PasteCommand { get; }
        public RelayCommand UpdateExplicitCommand { get; }

        /// <summary>
        /// Текст текущей темы для отображения.
        /// </summary>
        public string CurrentThemeText
        {
            get
            {
                string themeKey = _selectedTheme == "Light" ? "LightTheme" : "DarkTheme";
                string themeName = LanguageManager.Instance[themeKey];
                return string.Format(LanguageManager.Instance["CurrentTheme"], themeName);
            }
        }

        /// <summary>
        /// Словарь переводов для привязки в XAML.
        /// </summary>
        public Dictionary<string, string> Translations => LanguageManager.Instance.Translations;

        public MainViewModel()
        {
            // Применяем начальную тему
            ThemeManager.ApplyTheme(_selectedTheme);

            SourceItems.Add(new ItemModel { Name = "Элемент 1", Description = "Тестовое описание", Status = "Активен" });
            SourceItems.Add(new ItemModel { Name = "Элемент 2", Description = "Еще одно описание", Status = "Ожидает" });

            AddCommand = new RelayCommand(_ =>
                SourceItems.Add(new ItemModel { Name = "Новый элемент", Description = "Описание", Status = "Новый" }));

            DeleteCommand = new RelayCommand(_ =>
            {
                if (SelectedSourceItem != null)
                    SourceItems.Remove(SelectedSourceItem);
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
            }, _ => SelectedSourceItem != null);

            PasteCommand = new RelayCommand(_ =>
            {
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
            });

            UpdateExplicitCommand = new RelayCommand(_ =>
            {
                OnPropertyChanged(nameof(BindingExplicit));
                MessageBox.Show($"Значение обновлено явно: {BindingExplicit}");
            });
        }
    }
}