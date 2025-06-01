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

            // �������� �������� �����
            if (string.IsNullOrWhiteSpace(username))
            {
                await ShowErrorDialog("���� �����, ������ ��'� �����������");
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                await ShowErrorDialog("���� �����, ������ ���������� �����");
                return;
            }

            if (!IsValidEmail(email))
            {
                await ShowErrorDialog("������� ������ ���������� �����");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                await ShowErrorDialog("���� �����, ������ ������");
                return;
            }

            if (password.Length < 6)
            {
                await ShowErrorDialog("������ ������� ������ �������� 6 �������");
                return;
            }

            if (password != confirmPassword)
            {
                await ShowErrorDialog("����� �� ����������!");
                return;
            }

            // ������ ��������� (�������� ��� �� email)
            bool success = await UserManager.RegisterUserAsync(username, email, password, role);

            if (success)
            {
                await ShowSuccessDialog("��������� ������� ������!");

                // �������� ���� ���� ������ ���������
                RegUsernameTextBox.Text = string.Empty;
                RegEmailTextBox.Text = string.Empty;
                RegPasswordBox.Password = string.Empty;
                ConfirmPasswordBox.Password = string.Empty;

                // ������� �� ������� �����������
                this.Frame.Navigate(typeof(AuthorizationPage));
            }
            else
            {
                await ShowErrorDialog("���������� � ����� ��'�� ��� ����!");
            }
        }
        catch (Exception ex)
        {
            await ShowErrorDialog($"������� ����������� �������: {ex.Message}");
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


    // ������� ������ ��� ����������� ��������� ����
    private async Task ShowErrorDialog(string message)
    {
        ContentDialog dialog = new ContentDialog
        {
            Title = "�������",
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
            Title = "����",
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = this.XamlRoot
        };
        await dialog.ShowAsync();
    }

}
