using System.Collections.ObjectModel;
using System.Windows;
using Lab_1.Commands;
using Lab_1.Models;

namespace Lab_1.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // Свойства для вкладки "Основное"
        private string _selectedTheme = "Светлая тема";
        public string SelectedTheme
        {
            get => _selectedTheme;
            set => SetProperty(ref _selectedTheme, value);
        }

        private double _fontSize = 14;
        public double FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }

        // Свойства для вкладки "Типы привязок"
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

        // Коллекции для таблиц
        public ObservableCollection<ItemModel> SourceItems { get; set; } = new ObservableCollection<ItemModel>();
        public ObservableCollection<ItemModel> TargetItems { get; set; } = new ObservableCollection<ItemModel>();
        public ObservableCollection<ItemModel> ClipboardItems { get; set; } = new ObservableCollection<ItemModel>();

        public ItemModel SelectedSourceItem { get; set; }
        public ItemModel SelectedTargetItem { get; set; }

        // Команды
        public RelayCommand AddCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand CopyCommand { get; }
        public RelayCommand PasteCommand { get; }
        public RelayCommand UpdateExplicitCommand { get; }

        public MainViewModel()
        {
            // Начальное заполнение таблицы
            SourceItems.Add(new ItemModel { Name = "Элемент 1", Description = "Тестовое описание", Status = "Активен" });
            SourceItems.Add(new ItemModel { Name = "Элемент 2", Description = "Еще одно описание", Status = "Ожидает" });

            // Инициализация команд
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