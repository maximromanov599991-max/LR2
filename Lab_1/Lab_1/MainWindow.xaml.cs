using System.Windows;
using System.Windows.Input;
using Lab_1.ViewModels;

namespace Lab_1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private MainViewModel VM => DataContext as MainViewModel;

        private void Copy_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            VM?.CopyCommand.Execute(null);
        }

        private void Copy_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = VM?.SelectedSourceItem != null;
        }

        private void Paste_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            VM?.PasteCommand.Execute(null);
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            VM?.AddCommand.Execute(null);
        }

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            VM?.DeleteCommand.Execute(null);
        }

        private void Delete_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = VM?.SelectedSourceItem != null;
        }
    }
}