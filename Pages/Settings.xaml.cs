using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace KeyBoardTrainer.Pages
{
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
        }

        private void ThemeRadioButtons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ThemeRadioButtons.SelectedItem is RadioButton selectedRadio)
            {
                var newTheme = selectedRadio.Tag?.ToString() == "Light" ? ElementTheme.Light : ElementTheme.Dark;

                if (App.MainWindow is Window mainWindow && mainWindow.Content is FrameworkElement rootElement)
                {
                    rootElement.RequestedTheme = newTheme;
                }
            }
        }
    }
}
