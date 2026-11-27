using System;
using System.Windows;

namespace Lab_1.Services
{
    public static class LanguageManager
    {
        private static ResourceDictionary _currentLanguage;

        public static void ApplyLanguage(string languageName)
        {
            string languagePath;

            if (languageName == "ru" || languageName.Contains("Русск"))
                languagePath = "pack://application:,,,/WpfResources;component/RussianLanguage.xaml";
            else if (languageName == "en" || languageName.Contains("English") || languageName.Contains("Англ"))
                languagePath = "pack://application:,,,/WpfResources;component/EnglishLanguage.xaml";
            else
                languagePath = "pack://application:,,,/WpfResources;component/RussianLanguage.xaml";

            if (_currentLanguage != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(_currentLanguage);
            }

            _currentLanguage = new ResourceDictionary
            {
                Source = new Uri(languagePath, UriKind.Absolute)
            };
            Application.Current.Resources.MergedDictionaries.Add(_currentLanguage);
        }
    }
}