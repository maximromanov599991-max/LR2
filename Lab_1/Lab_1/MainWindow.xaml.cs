using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Lab_1.ViewModels;

namespace Lab_1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик кнопки "Обновить явно" для демонстрации UpdateSourceTrigger=Explicit.
        /// Принудительно передаёт значение из TextBox в ViewModel через UpdateSource().
        /// </summary>
        private void UpdateExplicit_Click(object sender, RoutedEventArgs e)
        {
            // Получаем BindingExpression для свойства Text у TextBox
            BindingExpression bindingExpression = ExplicitTextBox.GetBindingExpression(TextBox.TextProperty);

            // Принудительно передаём значение из TextBox в источник привязки (ViewModel)
            bindingExpression.UpdateSource();

            // Получаем ViewModel из DataContext окна
            MainViewModel viewModel = DataContext as MainViewModel;

            // Показываем актуальное значение в MessageBox
            if (viewModel != null)
            {
                MessageBox.Show(
                    $"Значение обновлено явно: {viewModel.BindingExplicit}",
                    "Явное обновление",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
    }
}