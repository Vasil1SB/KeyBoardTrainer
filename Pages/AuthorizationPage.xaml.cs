using KeyBoardTrainer.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;



namespace KeyBoardTrainer.Pages;


public sealed partial class AuthorizationPage : Page
{
    public AuthorizationPage()
    {
        this.InitializeComponent();
    }

    private async void AuthButton_Click(object sender, RoutedEventArgs e)
    {
        string username = UsernameTextBox.Text.Trim();
        string password = PasswordBox.Password;

        bool isValid = await UserManager.ValidateUserAsync(username, password);

        if (isValid)
        {
            ContentDialog successDialog = new ContentDialog
            {
                Title = "����",
                Content = "����������� ������� ������!",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await successDialog.ShowAsync();

            // ���������� �� ������� ������� (HomePage)
            this.Frame.Navigate(typeof(HomePageWindow));

        }
        else
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = "�������",
                Content = "������� ���� ��� ������.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            await errorDialog.ShowAsync();
        }
    }
    private void Hyperlink_Register_Click(object sender, RoutedEventArgs e)
    {
        // ���������� �� ������� ���������
        this.Frame.Navigate(typeof(RegistrationPage));
    }

}

