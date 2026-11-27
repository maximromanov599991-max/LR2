using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Lab_1.Commands;
using Lab_1.Models;
using Lab_1.Services;

namespace Lab_1.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _selectedTheme = "Светлая тема";
        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (SetProperty(ref _selectedTheme, value))
                {
                    ThemeManager.ApplyTheme(value);
                }
            }
        }

        private string _selectedLanguage = "Русский";
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (SetProperty(ref _selectedLanguage, value))
                {
                    LanguageManager.ApplyLanguage(value);
                    UpdateSourceItemsTexts();
                }
            }
        }

        private string _selectedFontFamily = "Segoe UI";
        public string SelectedFontFamily
        {
            get => _selectedFontFamily;
            set
            {
                if (SetProperty(ref _selectedFontFamily, value))
                {
                    ApplyFontToApplication(value);
                }
            }
        }

        private double _fontSize = 14;
        public double FontSize
        {
            get => _fontSize;
            set
            {
                if (SetProperty(ref _fontSize, value))
                {
                    ApplyFontSizeToApplication(value);
                }
            }
        }

        // Свойства для демонстрации привязок
        private string _bindingTwoWay;
        public string BindingTwoWay
        {
            get => _bindingTwoWay;
            set => SetProperty(ref _bindingTwoWay, value);
        }

        private string _bindingOneWay = "Только для чтения из ViewModel";
        public string BindingOneWay
        {
            get => _bindingOneWay;
            set => SetProperty(ref _bindingOneWay, value);
        }

        private string _bindingOneTime = "Значение при загрузке";
        public string BindingOneTime
        {
            get => _bindingOneTime;
            set => SetProperty(ref _bindingOneTime, value);
        }

        private string _bindingOneWayToSource;
        public string BindingOneWayToSource
        {
            get => _bindingOneWayToSource;
            set => SetProperty(ref _bindingOneWayToSource, value);
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

        public MainViewModel()
        {
            ThemeManager.ApplyTheme(_selectedTheme);
            LanguageManager.ApplyLanguage(_selectedLanguage);
            ApplyFontToApplication(_selectedFontFamily);
            ApplyFontSizeToApplication(_fontSize);

            InitializeSourceItems();

            AddCommand = new RelayCommand(_ =>
            {
                var newItem = new ItemModel();
                if (_selectedLanguage == "English")
                {
                    newItem.Name = "New item";
                    newItem.Description = "Description";
                    newItem.Status = "New";
                }
                else
                {
                    newItem.Name = "Новый элемент";
                    newItem.Description = "Описание";
                    newItem.Status = "Новый";
                }
                SourceItems.Add(newItem);
            });

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

        private void InitializeSourceItems()
        {
            SourceItems.Clear();
            SourceItems.Add(new ItemModel { Name = "Элемент 1", Description = "Тестовое описание", Status = "Активен" });
            SourceItems.Add(new ItemModel { Name = "Элемент 2", Description = "Еще одно описание", Status = "Ожидает" });
        }

        private void UpdateSourceItemsTexts()
        {
            foreach (var item in SourceItems)
            {
                if (_selectedLanguage == "English")
                {
                    if (item.Name == "Элемент 1") item.Name = "Item 1";
                    else if (item.Name == "Элемент 2") item.Name = "Item 2";
                    else if (item.Name == "Новый элемент") item.Name = "New item";

                    if (item.Description == "Тестовое описание") item.Description = "Test description";
                    else if (item.Description == "Еще одно описание") item.Description = "Another description";
                    else if (item.Description == "Описание") item.Description = "Description";

                    if (item.Status == "Активен") item.Status = "Active";
                    else if (item.Status == "Ожидает") item.Status = "Pending";
                    else if (item.Status == "Новый") item.Status = "New";
                }
                else
                {
                    if (item.Name == "Item 1") item.Name = "Элемент 1";
                    else if (item.Name == "Item 2") item.Name = "Элемент 2";
                    else if (item.Name == "New item") item.Name = "Новый элемент";

                    if (item.Description == "Test description") item.Description = "Тестовое описание";
                    else if (item.Description == "Another description") item.Description = "Еще одно описание";
                    else if (item.Description == "Description") item.Description = "Описание";

                    if (item.Status == "Active") item.Status = "Активен";
                    else if (item.Status == "Pending") item.Status = "Ожидает";
                    else if (item.Status == "New") item.Status = "Новый";
                }
            }
            OnPropertyChanged(nameof(SourceItems));
        }

        private void ApplyFontToApplication(string fontFamilyName)
        {
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.FontFamily = new FontFamily(fontFamilyName);
            }
        }

        private void ApplyFontSizeToApplication(double fontSize)
        {
            if (Application.Current.MainWindow != null)
            {
                Application.Current.MainWindow.FontSize = fontSize;
            }
        }
    }
}