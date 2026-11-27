using System;
using System.Windows;

namespace Lab_1.Services
{
    public static class ThemeManager
    {
        private static ResourceDictionary _currentTheme;

        public static void ApplyTheme(string themeName)
        {
            string themePath;

            if (themeName.Contains("Тёмн") || themeName.ToLower().Contains("dark"))
                themePath = "pack://application:,,,/WpfResources;component/DarkTheme.xaml";
            else
                themePath = "pack://application:,,,/WpfResources;component/LightTheme.xaml";

            if (_currentTheme != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(_currentTheme);
            }

            _currentTheme = new ResourceDictionary
            {
                Source = new Uri(themePath, UriKind.Absolute)
            };
            Application.Current.Resources.MergedDictionaries.Add(_currentTheme);
        }
    }
}