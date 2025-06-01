using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Storage;
using Microsoft.UI.Xaml.Media;
using KeyBoardTrainer.Helpers; // Додаємо простір імен для UserManager

namespace KeyBoardTrainer.Pages
{
    /// <summary>
    /// A page for number training with a numeric keypad.
    /// </summary>
    public sealed partial class NumTraining : Page
    {
        private List<string> tasks = new();
        private string currentTask = "";
        private Stopwatch stopwatch = new();
        private string currentUser = "default";
        private Dictionary<string, Button> keyMap = new();

        public NumTraining()
        {
            this.InitializeComponent();
            InitializeKeyboard();
            LoadTasksFromTxtAsync();
        }

        private void InitializeKeyboard()
        {
            keyMap["-"] = Key_Minus;
            keyMap["7"] = Key_7;
            keyMap["8"] = Key_8;
            keyMap["9"] = Key_9;
            keyMap["+"] = Key_Plus;
            keyMap["4"] = Key_4;
            keyMap["5"] = Key_5;
            keyMap["6"] = Key_6;
            keyMap["1"] = Key_1;
            keyMap["2"] = Key_2;
            keyMap["3"] = Key_3;
            keyMap["Enter"] = Key_Enter;
            keyMap["0"] = Key_0;
            keyMap[","] = Key_Comma;
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string login)
            {
                currentUser = login;
            }
        }

        private async void LoadTasksFromTxtAsync()
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/numberSequences.txt"));
                string text = await FileIO.ReadTextAsync(file);

                tasks = new List<string>(text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries));

                for (int i = 0; i < tasks.Count; i++)
                {
                    TaskSelector.Items.Add($"Завдання {i + 1}");
                }

                if (tasks.Count > 0)
                    TaskSelector.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                TargetTextBlock.Text = $"Помилка завантаження завдань: {ex.Message}";
            }
        }

        private void TaskSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = TaskSelector.SelectedIndex;
            if (index >= 0 && index < tasks.Count)
            {
                currentTask = tasks[index];
                InputTextBox.Text = string.Empty;
                ResultTextBlock.Text = string.Empty;
                MistakeTextBlock.Text = string.Empty;
                SpeedTextBlock.Text = string.Empty;
                AccuracyTextBlock.Text = string.Empty;
                TimeTextBlock.Text = string.Empty;
                TargetTextBlock.Text = currentTask;
                stopwatch.Reset();
            }
        }

        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!stopwatch.IsRunning && InputTextBox.Text.Length > 0)
                stopwatch.Start();
        }

        private void InputTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
        }

        private void Key_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string key = button.Content?.ToString();
                if (string.IsNullOrEmpty(key))
                    return;

                button.Background = new SolidColorBrush(Microsoft.UI.Colors.LightGray);

                if (key == "Enter")
                {
                    CheckButton_Click(sender, e);
                }
                else
                {
                    InputTextBox.Text += key;
                }

                button.Background = key == "0" ? new SolidColorBrush(Microsoft.UI.Colors.Green) :
                                   new SolidColorBrush(Microsoft.UI.Colors.LightGray);

                InputTextBox.SelectionStart = InputTextBox.Text.Length;
                InputTextBox.SelectionLength = 0;
            }
        }

        private async void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentTask)) return;

            stopwatch.Stop();
            string input = InputTextBox.Text;

            int correct = 0;
            int mistakes = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (i < currentTask.Length && input[i] == currentTask[i])
                    correct++;
                else
                    mistakes++;
            }

            double minutes = stopwatch.Elapsed.TotalMinutes;
            int speed = minutes > 0 ? (int)(input.Length / minutes) : 0;
            double accuracy = currentTask.Length > 0 ? (double)correct / currentTask.Length * 100 : 0;

            ResultTextBlock.Text = $"Правильних символів: {correct} з {currentTask.Length}";
            MistakeTextBlock.Text = $"Кількість помилок: {mistakes}";
            SpeedTextBlock.Text = $"Швидкість: {speed} символів/хв";
            AccuracyTextBlock.Text = $"Точність: {accuracy:F1}%";
            TimeTextBlock.Text = $"Час: {stopwatch.Elapsed.TotalSeconds:F1} сек";

            // Збереження статистики в основний файл
            try
            {
                await UserManager.UpdateUserStatisticsAsync(speed, accuracy);
            }
            catch (Exception ex)
            {
                ResultTextBlock.Text = $"Помилка збереження статистики: {ex.Message}";
            }
        }
    }
}