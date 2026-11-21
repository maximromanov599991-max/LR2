using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lab_1.Services
{
    /// <summary>
    /// Менеджер локализации. Хранит переводы и уведомляет об изменении языка.
    /// </summary>
    public class LanguageManager : INotifyPropertyChanged
    {
        private static LanguageManager _instance;
        public static LanguageManager Instance => _instance ??= new LanguageManager();

        private string _currentLanguage = "ru";
        private Dictionary<string, Dictionary<string, string>> _translations;

        public event PropertyChangedEventHandler PropertyChanged;

        private LanguageManager()
        {
            LoadTranslations();
        }

        /// <summary>
        /// Загружает переводы для всех языков.
        /// </summary>
        private void LoadTranslations()
        {
            _translations = new Dictionary<string, Dictionary<string, string>>
            {
                ["ru"] = new Dictionary<string, string>
                {
                    ["AppTitle"] = "Лабораторная работа 2 - WPF MVVM",
                    ["TabMain"] = "Основное",
                    ["TabPalette"] = "Цветовая палитра",
                    ["TabControls"] = "Элементы управления",
                    ["TabLayouts"] = "Контейнеры компоновки",
                    ["TabTables"] = "Таблицы и Команды",
                    ["TabBindings"] = "Типы привязок",
                    ["PersonalizationSettings"] = "Настройки персонализации",
                    ["ThemeLabel"] = "Тема приложения:",
                    ["LanguageLabel"] = "Язык приложения:",
                    ["FontSettings"] = "Настройки шрифтов",
                    ["FontLabel"] = "Шрифт:",
                    ["FontSizeLabel"] = "Размер шрифта:",
                    ["CurrentTheme"] = "Текущая тема: {0}",
                    ["MainColors"] = "Основные цвета",
                    ["Primary"] = "Основной",
                    ["Accent"] = "Акцентный",
                    ["Success"] = "Успех",
                    ["Error"] = "Ошибка",
                    ["Info"] = "Информация",
                    ["ColorPalette"] = "Палитра основного цвета (10 оттенков)",
                    ["Buttons"] = "Кнопки",
                    ["NormalButton"] = "Обычная",
                    ["SuccessButton"] = "Успех",
                    ["DeleteButton"] = "Удалить",
                    ["ToggleButton"] = "Переключатель",
                    ["InputFields"] = "Поля ввода и списки",
                    ["TextElements"] = "Текстовые элементы",
                    ["TreeViewDataGrid"] = "TreeView и DataGrid",
                    ["ProgressBar"] = "ProgressBar",
                    ["ProgressText"] = "Прогресс: 65%",
                    ["StackPanelVertical"] = "StackPanel (Вертикальный)",
                    ["StackPanelHorizontal"] = "StackPanel (Горизонтальный)",
                    ["WrapPanel"] = "WrapPanel (Перенос элементов)",
                    ["DockPanel"] = "DockPanel (Пристыковка к краям)",
                    ["Grid"] = "Grid (Табличная структура)",
                    ["Canvas"] = "Canvas (Абсолютное позиционирование)",
                    ["SourceTable"] = "Исходная таблица (Source)",
                    ["TargetTable"] = "Целевая таблица (Target)",
                    ["Add"] = "Добавить",
                    ["Delete"] = "Удалить",
                    ["Copy"] = "Копировать",
                    ["Paste"] = "Вставить",
                    ["NameColumn"] = "Название",
                    ["DescriptionColumn"] = "Описание",
                    ["StatusColumn"] = "Статус",
                    ["BindingDemo"] = "Демонстрация режимов обновления источника (UpdateSourceTrigger)",
                    ["PropertyChanged"] = "1. PropertyChanged (Обновление при каждом символе)",
                    ["LostFocus"] = "2. LostFocus (Обновление при потере фокуса)",
                    ["Explicit"] = "3. Explicit (Явное обновление по кнопке)",
                    ["UpdateExplicit"] = "Обновить явно",
                    ["LightTheme"] = "Светлая тема",
                    ["DarkTheme"] = "Тёмная тема",
                    ["Russian"] = "Русский",
                    ["English"] = "English"
                },
                ["en"] = new Dictionary<string, string>
                {
                    ["AppTitle"] = "Lab Work 2 - WPF MVVM",
                    ["TabMain"] = "Main",
                    ["TabPalette"] = "Color Palette",
                    ["TabControls"] = "Controls",
                    ["TabLayouts"] = "Layout Containers",
                    ["TabTables"] = "Tables and Commands",
                    ["TabBindings"] = "Binding Types",
                    ["PersonalizationSettings"] = "Personalization Settings",
                    ["ThemeLabel"] = "Application Theme:",
                    ["LanguageLabel"] = "Application Language:",
                    ["FontSettings"] = "Font Settings",
                    ["FontLabel"] = "Font:",
                    ["FontSizeLabel"] = "Font Size:",
                    ["CurrentTheme"] = "Current Theme: {0}",
                    ["MainColors"] = "Main Colors",
                    ["Primary"] = "Primary",
                    ["Accent"] = "Accent",
                    ["Success"] = "Success",
                    ["Error"] = "Error",
                    ["Info"] = "Info",
                    ["ColorPalette"] = "Primary Color Palette (10 shades)",
                    ["Buttons"] = "Buttons",
                    ["NormalButton"] = "Normal",
                    ["SuccessButton"] = "Success",
                    ["DeleteButton"] = "Delete",
                    ["ToggleButton"] = "Toggle",
                    ["InputFields"] = "Input Fields and Lists",
                    ["TextElements"] = "Text Elements",
                    ["TreeViewDataGrid"] = "TreeView and DataGrid",
                    ["ProgressBar"] = "ProgressBar",
                    ["ProgressText"] = "Progress: 65%",
                    ["StackPanelVertical"] = "StackPanel (Vertical)",
                    ["StackPanelHorizontal"] = "StackPanel (Horizontal)",
                    ["WrapPanel"] = "WrapPanel (Wrap)",
                    ["DockPanel"] = "DockPanel (Dock)",
                    ["Grid"] = "Grid (Table)",
                    ["Canvas"] = "Canvas (Absolute)",
                    ["SourceTable"] = "Source Table",
                    ["TargetTable"] = "Target Table",
                    ["Add"] = "Add",
                    ["Delete"] = "Delete",
                    ["Copy"] = "Copy",
                    ["Paste"] = "Paste",
                    ["NameColumn"] = "Name",
                    ["DescriptionColumn"] = "Description",
                    ["StatusColumn"] = "Status",
                    ["BindingDemo"] = "UpdateSourceTrigger Modes Demo",
                    ["PropertyChanged"] = "1. PropertyChanged (Update on each character)",
                    ["LostFocus"] = "2. LostFocus (Update on focus loss)",
                    ["Explicit"] = "3. Explicit (Manual update by button)",
                    ["UpdateExplicit"] = "Update Explicitly",
                    ["LightTheme"] = "Light Theme",
                    ["DarkTheme"] = "Dark Theme",
                    ["Russian"] = "Russian",
                    ["English"] = "English"
                }
            };
        }

        /// <summary>
        /// Текущий язык приложения.
        /// </summary>
        public string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    OnPropertyChanged();
                    OnPropertyChanged("Translations");
                }
            }
        }

        /// <summary>
        /// Получает перевод по ключу.
        /// </summary>
        public string this[string key]
        {
            get
            {
                if (_translations.ContainsKey(_currentLanguage) &&
                    _translations[_currentLanguage].ContainsKey(key))
                {
                    return _translations[_currentLanguage][key];
                }
                return key;
            }
        }

        /// <summary>
        /// Возвращает словарь переводов для текущего языка (для привязки).
        /// </summary>
        public Dictionary<string, string> Translations
        {
            get => _translations.ContainsKey(_currentLanguage)
                ? _translations[_currentLanguage]
                : new Dictionary<string, string>();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}