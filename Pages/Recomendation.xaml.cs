using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace KeyBoardTrainer.Pages
{
    public sealed partial class Recomendation : Page
    {
        public Recomendation()
        {
            this.InitializeComponent();
            LoadRecommendationsAsync();
        }

        private async void LoadRecommendationsAsync()
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/recommendations.txt"));
                string text = await FileIO.ReadTextAsync(file);

                var recommendations = ParseRecommendations(text);
                if (recommendations.Count == 0)
                {
                    AddRecommendationText("Немає доступних рекомендацій.");
                    return;
                }

                foreach (var rec in recommendations)
                {
                    TextBlock titleBlock = new TextBlock
                    {
                        Text = rec.Title,
                        Style = Application.Current.Resources["SubtitleTextBlockStyle"] as Style,
                        Margin = new Thickness(0, 10, 0, 5)
                    };
                    TextBlock contentBlock = new TextBlock
                    {
                        Text = rec.Content,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(0, 0, 0, 15)
                    };
                    RecommendationsPanel.Children.Add(titleBlock);
                    RecommendationsPanel.Children.Add(contentBlock);
                }
            }
            catch (Exception ex)
            {
                AddRecommendationText($"Помилка завантаження рекомендацій: {ex.Message}");
            }
        }

        private List<Recommendation> ParseRecommendations(string text)
        {
            var recommendations = new List<Recommendation>();
            var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i += 3)
            {
                if (i + 1 < lines.Length && !string.IsNullOrWhiteSpace(lines[i]) && !string.IsNullOrWhiteSpace(lines[i + 1]))
                {
                    recommendations.Add(new Recommendation
                    {
                        Title = lines[i].Trim(),
                        Content = lines[i + 1].Trim()
                    });
                }
            }

            return recommendations;
        }

        private void AddRecommendationText(string message)
        {
            TextBlock errorBlock = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 10, 0, 10)
            };
            RecommendationsPanel.Children.Add(errorBlock);
        }
    }

    public class Recommendation
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}