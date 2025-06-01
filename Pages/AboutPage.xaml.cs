using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Text;
using System.Threading.Tasks;
using KeyBoardTrainer.Helpers;
using System.Diagnostics;
using System.Linq;

namespace KeyBoardTrainer.Pages
{
    public sealed partial class AboutPage : Page
    {
        private string currentUser = "default";

        public AboutPage()
        {
            this.InitializeComponent();
            Debug.WriteLine("AboutPage initialized");
        }

        protected override async void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Debug.WriteLine("OnNavigatedTo called");

            if (e.Parameter is string login)
            {
                currentUser = login;
                Debug.WriteLine($"Current user set to: {currentUser}");
            }
            else
            {
                Debug.WriteLine("No login parameter provided");
            }

            if (currentUser == "default")
            {
                Frame.Navigate(typeof(WelcomePage));
                return;
            }

            UserManager.SetCurrentUser(currentUser);
            await LoadStatisticsAsync();
        }

        private async void LoadResultsButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("LoadResultsButton_Click called");
            await LoadStatisticsAsync();
        }

        private async Task LoadStatisticsAsync()
        {
            try
            {
                Debug.WriteLine("LoadStatisticsAsync started");

                if (ResultsTextBox == null)
                {
                    Debug.WriteLine("ResultsTextBox is null");
                    return;
                }

                var stats = await UserManager.GetUserStatisticsAsync();

                if (stats.ExercisesCompleted == 0)
                {
                    ResultsTextBox.Text = "Немає виконаних вправ.";
                    Debug.WriteLine("No statistics available");
                    return;
                }

                StringBuilder displayText = new StringBuilder();
                displayText.AppendLine($"Користувач: {UserManager.CurrentUser.Username}");
                displayText.AppendLine($"Виконано вправ: {stats.ExercisesCompleted}");
                displayText.AppendLine($"Середня швидкість: {stats.AverageWPM:F1} симв/хв");
                displayText.AppendLine($"Середня точність: {stats.AverageAccuracy:F1}%");
                displayText.AppendLine("\nІсторія вправ:");

                foreach (var result in stats.ExerciseHistory.OrderByDescending(r => r.Date))
                {
                    displayText.AppendLine($"Дата: {result.Date:yyyy-MM-dd HH:mm:ss}, Швидкість: {result.WPM:F1} симв/хв, Точність: {result.Accuracy:F1}%");
                }

                ResultsTextBox.Text = displayText.ToString();
                Debug.WriteLine("Statistics loaded successfully");
            }
            catch (Exception ex)
            {
                ResultsTextBox.Text = $"Помилка завантаження статистики: {ex.Message}";
                Debug.WriteLine($"Error loading statistics: {ex}");
            }
        }
    }
}
