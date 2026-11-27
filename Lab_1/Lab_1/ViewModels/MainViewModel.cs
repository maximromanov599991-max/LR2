using Lab_1.Commands;
using Lab_1.Models;
using Lab_1.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

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
                    UpdateAllTexts();
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
        public MainViewModel()
        {
            ThemeManager.ApplyTheme(_selectedTheme);
            LanguageManager.ApplyLanguage(_selectedLanguage);
            ApplyFontToApplication(_selectedFontFamily);
            ApplyFontSizeToApplication(_fontSize);

            InitializeAllData();

            // ТЕСТ: принудительно меняем язык на английский через 3 секунды
            System.Windows.Threading.DispatcherTimer timer =
                new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += (s, e) =>
            {
                timer.Stop();
                SelectedLanguage = "English";
            };
            timer.Start();

            // ... остальной код команд ...
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

        // Коллекции
        public ObservableCollection<ItemModel> SourceItems { get; set; } = new ObservableCollection<ItemModel>();
        public ObservableCollection<ItemModel> TargetItems { get; set; } = new ObservableCollection<ItemModel>();
        public ObservableCollection<ItemModel> ClipboardItems { get; set; } = new ObservableCollection<ItemModel>();
        public ObservableCollection<ColorShade> ColorShades { get; set; } = new ObservableCollection<ColorShade>();
        public ObservableCollection<TreeNode> TreeNodes { get; set; } = new ObservableCollection<TreeNode>();
        public ObservableCollection<ItemModel> DemoGridItems { get; set; } = new ObservableCollection<ItemModel>();

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

            InitializeAllData();

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

        private void InitializeAllData()
        {
            InitializeSourceItems();
            InitializeColorShades();
            InitializeTreeNodes();
            InitializeDemoGrid();
        }

        private void UpdateAllTexts()
        {
            UpdateSourceItemsTexts();
            UpdateColorShadesTexts();
            UpdateTreeNodesTexts();
            UpdateDemoGridTexts();
        }

        private void InitializeSourceItems()
        {
            SourceItems.Clear();
            SourceItems.Add(new ItemModel { Name = "Элемент 1", Description = "Тестовое описание", Status = "Активен" });
            SourceItems.Add(new ItemModel { Name = "Элемент 2", Description = "Еще одно описание", Status = "Ожидает" });
        }

        private void InitializeColorShades()
        {
            ColorShades.Clear();
            ColorShades.Add(new ColorShade { Hex = "#E3F2FD", RGB = "227, 242, 253", Name = "Основной 50" });
            ColorShades.Add(new ColorShade { Hex = "#BBDEFB", RGB = "187, 222, 251", Name = "Основной 100" });
            ColorShades.Add(new ColorShade { Hex = "#90CAF9", RGB = "144, 202, 249", Name = "Основной 200" });
            ColorShades.Add(new ColorShade { Hex = "#64B5F6", RGB = "100, 181, 246", Name = "Основной 300" });
            ColorShades.Add(new ColorShade { Hex = "#42A5F5", RGB = "66, 165, 245", Name = "Основной 400" });
            ColorShades.Add(new ColorShade { Hex = "#2196F3", RGB = "33, 150, 243", Name = "Основной 500 (Базовый)" });
            ColorShades.Add(new ColorShade { Hex = "#1E88E5", RGB = "30, 136, 229", Name = "Основной 600" });
            ColorShades.Add(new ColorShade { Hex = "#1976D2", RGB = "25, 118, 210", Name = "Основной 700" });
            ColorShades.Add(new ColorShade { Hex = "#1565C0", RGB = "21, 101, 192", Name = "Основной 800" });
            ColorShades.Add(new ColorShade { Hex = "#0D47A1", RGB = "13, 71, 161", Name = "Основной 900" });
        }

        private void InitializeTreeNodes()
        {
            TreeNodes.Clear();
            var controlsNode = new TreeNode { Name = "Элементы управления" };
            controlsNode.Children.Add(new TreeNode { Name = "Кнопки" });
            controlsNode.Children.Add(new TreeNode { Name = "Поля ввода" });

            var containersNode = new TreeNode { Name = "Контейнеры" };

            TreeNodes.Add(controlsNode);
            TreeNodes.Add(containersNode);
        }

        private void InitializeDemoGrid()
        {
            DemoGridItems.Clear();
            DemoGridItems.Add(new ItemModel { Name = "Button", Description = "Обычная кнопка", Status = "Активна" });
            DemoGridItems.Add(new ItemModel { Name = "TextBox", Description = "Поле ввода текста", Status = "Пустое" });
            DemoGridItems.Add(new ItemModel { Name = "ComboBox", Description = "Раскрывающийся список", Status = "Выбрано" });
            DemoGridItems.Add(new ItemModel { Name = "CheckBox", Description = "Флажок", Status = "Отмечен" });
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

        private void UpdateColorShadesTexts()
        {
            ColorShades.Clear();
            if (_selectedLanguage == "English")
            {
                ColorShades.Add(new ColorShade { Hex = "#E3F2FD", RGB = "227, 242, 253", Name = "Primary 50" });
                ColorShades.Add(new ColorShade { Hex = "#BBDEFB", RGB = "187, 222, 251", Name = "Primary 100" });
                ColorShades.Add(new ColorShade { Hex = "#90CAF9", RGB = "144, 202, 249", Name = "Primary 200" });
                ColorShades.Add(new ColorShade { Hex = "#64B5F6", RGB = "100, 181, 246", Name = "Primary 300" });
                ColorShades.Add(new ColorShade { Hex = "#42A5F5", RGB = "66, 165, 245", Name = "Primary 400" });
                ColorShades.Add(new ColorShade { Hex = "#2196F3", RGB = "33, 150, 243", Name = "Primary 500 (Base)" });
                ColorShades.Add(new ColorShade { Hex = "#1E88E5", RGB = "30, 136, 229", Name = "Primary 600" });
                ColorShades.Add(new ColorShade { Hex = "#1976D2", RGB = "25, 118, 210", Name = "Primary 700" });
                ColorShades.Add(new ColorShade { Hex = "#1565C0", RGB = "21, 101, 192", Name = "Primary 800" });
                ColorShades.Add(new ColorShade { Hex = "#0D47A1", RGB = "13, 71, 161", Name = "Primary 900" });
            }
            else
            {
                ColorShades.Add(new ColorShade { Hex = "#E3F2FD", RGB = "227, 242, 253", Name = "Основной 50" });
                ColorShades.Add(new ColorShade { Hex = "#BBDEFB", RGB = "187, 222, 251", Name = "Основной 100" });
                ColorShades.Add(new ColorShade { Hex = "#90CAF9", RGB = "144, 202, 249", Name = "Основной 200" });
                ColorShades.Add(new ColorShade { Hex = "#64B5F6", RGB = "100, 181, 246", Name = "Основной 300" });
                ColorShades.Add(new ColorShade { Hex = "#42A5F5", RGB = "66, 165, 245", Name = "Основной 400" });
                ColorShades.Add(new ColorShade { Hex = "#2196F3", RGB = "33, 150, 243", Name = "Основной 500 (Базовый)" });
                ColorShades.Add(new ColorShade { Hex = "#1E88E5", RGB = "30, 136, 229", Name = "Основной 600" });
                ColorShades.Add(new ColorShade { Hex = "#1976D2", RGB = "25, 118, 210", Name = "Основной 700" });
                ColorShades.Add(new ColorShade { Hex = "#1565C0", RGB = "21, 101, 192", Name = "Основной 800" });
                ColorShades.Add(new ColorShade { Hex = "#0D47A1", RGB = "13, 71, 161", Name = "Основной 900" });
            }
            OnPropertyChanged(nameof(ColorShades));
        }

        private void UpdateTreeNodesTexts()
        {
            TreeNodes.Clear();
            if (_selectedLanguage == "English")
            {
                var controlsNode = new TreeNode { Name = "Controls" };
                controlsNode.Children.Add(new TreeNode { Name = "Buttons" });
                controlsNode.Children.Add(new TreeNode { Name = "Input Fields" });

                var containersNode = new TreeNode { Name = "Containers" };

                TreeNodes.Add(controlsNode);
                TreeNodes.Add(containersNode);
            }
            else
            {
                var controlsNode = new TreeNode { Name = "Элементы управления" };
                controlsNode.Children.Add(new TreeNode { Name = "Кнопки" });
                controlsNode.Children.Add(new TreeNode { Name = "Поля ввода" });

                var containersNode = new TreeNode { Name = "Контейнеры" };

                TreeNodes.Add(controlsNode);
                TreeNodes.Add(containersNode);
            }
            OnPropertyChanged(nameof(TreeNodes));
        }

        private void UpdateDemoGridTexts()
        {
            DemoGridItems.Clear();
            if (_selectedLanguage == "English")
            {
                DemoGridItems.Add(new ItemModel { Name = "Button", Description = "Normal button", Status = "Active" });
                DemoGridItems.Add(new ItemModel { Name = "TextBox", Description = "Text input field", Status = "Empty" });
                DemoGridItems.Add(new ItemModel { Name = "ComboBox", Description = "Dropdown list", Status = "Selected" });
                DemoGridItems.Add(new ItemModel { Name = "CheckBox", Description = "Checkbox", Status = "Checked" });
            }
            else
            {
                DemoGridItems.Add(new ItemModel { Name = "Button", Description = "Обычная кнопка", Status = "Активна" });
                DemoGridItems.Add(new ItemModel { Name = "TextBox", Description = "Поле ввода текста", Status = "Пустое" });
                DemoGridItems.Add(new ItemModel { Name = "ComboBox", Description = "Раскрывающийся список", Status = "Выбрано" });
                DemoGridItems.Add(new ItemModel { Name = "CheckBox", Description = "Флажок", Status = "Отмечен" });
            }
            OnPropertyChanged(nameof(DemoGridItems));
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