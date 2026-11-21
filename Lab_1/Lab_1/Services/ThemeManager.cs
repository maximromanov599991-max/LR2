using System.Windows;

namespace Lab_1.Services
{
    /// <summary>
    /// Менеджер тем приложения. Отвечает за динамическую смену темы.
    /// </summary>
    public static class ThemeManager
    {
        private static ResourceDictionary _currentTheme;

        /// <summary>
        /// Применяет тему к приложению.
        /// </summary>
        /// <param name="themeName">Имя темы: "Light" или "Dark"</param>
        public static void ApplyTheme(string themeName)
        {
            // Определяем путь к файлу темы
            string themePath = themeName.ToLower() switch
            {
                "light" => "pack://application:,,,/WpfResources;component/LightTheme.xaml",
                "dark" => "pack://application:,,,/WpfResources;component/DarkTheme.xaml",
                _ => "pack://application:,,,/WpfResources;component/LightTheme.xaml"
            };

            // Удаляем старую тему, если она есть
            if (_currentTheme != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(_currentTheme);
            }

            // Загружаем и применяем новую тему
            _currentTheme = new ResourceDictionary { Source = new System.Uri(themePath, System.UriKind.Absolute) };
            Application.Current.Resources.MergedDictionaries.Add(_currentTheme);
        }
    }
}