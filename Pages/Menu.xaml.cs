using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;



namespace KeyBoardTrainer.Pages;


public sealed partial class Menu : Page
{
    public Menu()
    {
        this.InitializeComponent();
    }

    private void RegistrationButton_Click(object sender, RoutedEventArgs e)
    {
        ContentFrame.Navigate(typeof(RegistrationPage));
    }

    private void AuthorizationButton_Click(object sender, RoutedEventArgs e)
    {
        ContentFrame.Navigate(typeof(AuthorizationPage));
    }

    private void GuestButton_Click(object sender, RoutedEventArgs e)
    {
        ContentFrame.Navigate(typeof(GuestPage));
    }


    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        // Close the application
        Application.Current.Exit();
    }



}
