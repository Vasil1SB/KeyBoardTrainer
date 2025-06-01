using KeyBoardTrainer.Helpers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;



namespace KeyBoardTrainer.Pages;

public sealed partial class RegistrationPage : Page
{
    public RegistrationPage()
    {
        this.InitializeComponent();
    }
    private async void RegisterButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string username = RegUsernameTextBox.Text.Trim();
            string email = RegEmailTextBox.Text.Trim();
            string password = RegPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;
            string role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();

            // Валідація введених даних
            if (string.IsNullOrWhiteSpace(username))
            {
                await ShowErrorDialog("Будь ласка, введіть ім'я користувача");
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                await ShowErrorDialog("Будь ласка, введіть електронну пошту");
                return;
            }

            if (!IsValidEmail(email))
            {
                await ShowErrorDialog("Невірний формат електронної пошти");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                await ShowErrorDialog("Будь ласка, введіть пароль");
                return;
            }

            if (password.Length < 6)
            {
                await ShowErrorDialog("Пароль повинен містити принаймні 6 символів");
                return;
            }

            if (password != confirmPassword)
            {
                await ShowErrorDialog("Паролі не співпадають!");
                return;
            }

            // Спроба реєстрації (передаємо ролі та email)
            bool success = await UserManager.RegisterUserAsync(username, email, password, role);

            if (success)
            {
                await ShowSuccessDialog("Реєстрація пройшла успішно!");

                // Очищення полів після успішної реєстрації
                RegUsernameTextBox.Text = string.Empty;
                RegEmailTextBox.Text = string.Empty;
                RegPasswordBox.Password = string.Empty;
                ConfirmPasswordBox.Password = string.Empty;

                // Перехід на сторінку авторизації
                this.Frame.Navigate(typeof(AuthorizationPage));
            }
            else
            {
                await ShowErrorDialog("Користувач з таким ім'ям вже існує!");
            }
        }
        catch (Exception ex)
        {
            await ShowErrorDialog($"Сталася неочікувана помилка: {ex.Message}");
        }
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }


    // Допоміжні методи для відображення діалогових вікон
    private async Task ShowErrorDialog(string message)
    {
        ContentDialog dialog = new ContentDialog
        {
            Title = "Помилка",
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = this.XamlRoot
        };
        await dialog.ShowAsync();
    }

    private async Task ShowSuccessDialog(string message)
    {
        ContentDialog dialog = new ContentDialog
        {
            Title = "Успіх",
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = this.XamlRoot
        };
        await dialog.ShowAsync();
    }

}
