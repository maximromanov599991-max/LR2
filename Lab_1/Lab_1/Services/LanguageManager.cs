using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace Lab_1.Services
{
    public class LanguageManager
    {
        // Singleton — единственный экземпляр класса
        private static LanguageManager _instance;
        public static LanguageManager Instance => _instance ??= new LanguageManager();

        private ResourceDictionary _currentLanguage;
        private Dictionary<string, string> _translations = new Dictionary<string, string>();
        private string _currentLanguageCode = "ru";

        private LanguageManager()
        {
            // При создании экземпляра применяем русский язык по умолчанию
            ApplyLanguage("ru");
        }

        /// <summary>
        /// Текущий язык приложения. При установке автоматически меняет словарь ресурсов.
        /// </summary>
        public string CurrentLanguage
        {
            get => _currentLanguageCode;
            set
            {
                if (_currentLanguageCode != value)
                {
                    _currentLanguageCode = value;
                    ApplyLanguage(value);
                }
            }
        }

        /// <summary>
        /// Словарь переводов текущего языка (для привязки в XAML через Translations[Key]).
        /// </summary>
        public Dictionary<string, string> Translations => _translations;

        /// <summary>
        /// Индексатор: LanguageManager.Instance["Key"]
        /// </summary>
        public string this[string key]
        {
            get
            {
                if (_translations.TryGetValue(key, out string value))
                    return value;
                return key;
            }
        }

        /// <summary>
        /// Применяет язык: меняет ResourceDictionary в Application.Resources
        /// и обновляет внутренний словарь Translations.
        /// </summary>
        public void ApplyLanguage(string languageName)
        {
            string languagePath;

            if (languageName == "ru" || languageName.Contains("Русск"))
                languagePath = "pack://application:,,,/WpfResources;component/RussianLanguage.xaml";
            else if (languageName == "en" || languageName.Contains("English") || languageName.Contains("Англ"))
                languagePath = "pack://application:,,,/WpfResources;component/EnglishLanguage.xaml";
            else
                languagePath = "pack://application:,,,/WpfResources;component/RussianLanguage.xaml";

            // Удаляем старый языковой словарь
            if (_currentLanguage != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(_currentLanguage);
            }

            // Загружаем новый
            _currentLanguage = new ResourceDictionary
            {
                Source = new Uri(languagePath, UriKind.Absolute)
            };
            Application.Current.Resources.MergedDictionaries.Add(_currentLanguage);

            // Копируем ключ-значение в наш внутренний словарь для привязок
            _translations.Clear();
            foreach (DictionaryEntry entry in _currentLanguage)
            {
                if (entry.Key is string key && entry.Value is string value)
                {
                    _translations[key] = value;
                }
            }
        }
    }
}