using System;
using System.Windows;

namespace Lab_1.Services
{
    /// <summary>
    /// Менеджер локализации. Динамически переключает ResourceDictionary с переводами.
    /// </summary>
    public static class LanguageManager
    {
        private static ResourceDictionary _currentLanguage;

        /// <summary>
        /// Применяет язык к приложению.
        /// </summary>
        /// <param name="languageName">Название языка: "ru" или "en" (или русское/английское название)</param>
        public static void ApplyLanguage(string languageName)
        {
            string languagePath;

            if (languageName == "ru" || languageName.Contains("Русск"))
                languagePath = "pack://application:,,,/WpfResources;component/RussianLanguage.xaml";
            else if (languageName == "en" || languageName.Contains("English") || languageName.Contains("Англ"))
                languagePath = "pack://application:,,,/WpfResources;component/EnglishLanguage.xaml";
            else
                languagePath = "pack://application:,,,/WpfResources;component/RussianLanguage.xaml";

            // Удаляем старый язык
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
        }
    }
}