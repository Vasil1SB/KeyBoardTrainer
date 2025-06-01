using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI;
using Microsoft.UI.Xaml.Media;

namespace KeyBoardTrainer.Pages
{
    /// <summary>
    /// A page for keyboard training with a counter functionality.
    /// </summary>
    public sealed partial class Counter : Page
    {
        private Stopwatch stopwatch = new();
        private string currentUser = "default";
        private Dictionary<string, Button> keyMap = new();
        private bool isCapsLockOn = false;
        private bool isShiftOn = false;

        public Counter()
        {
            this.InitializeComponent();
            InitializeKeyboard();
        }

        private void InitializeKeyboard()
        {
            keyMap["1"] = Key_1;
            keyMap["2"] = Key_2;
            keyMap["3"] = Key_3;
            keyMap["4"] = Key_4;
            keyMap["5"] = Key_5;
            keyMap["6"] = Key_6;
            keyMap["7"] = Key_7;
            keyMap["8"] = Key_8;
            keyMap["9"] = Key_9;
            keyMap["0"] = Key_0;
            keyMap["-"] = Key_Minus;
            keyMap["="] = Key_Equal;
            keyMap["backspace"] = Key_Backspace;
            keyMap["tab"] = Key_Tab;
            keyMap["й"] = Key_Uk_J;
            keyMap["ц"] = Key_Uk_C;
            keyMap["у"] = Key_Uk_U;
            keyMap["к"] = Key_Uk_K;
            keyMap["е"] = Key_Uk_E;
            keyMap["н"] = Key_Uk_N;
            keyMap["г"] = Key_Uk_H;
            keyMap["ш"] = Key_Uk_Sh;
            keyMap["щ"] = Key_Uk_Sch;
            keyMap["з"] = Key_Uk_Z;
            keyMap["х"] = Key_Uk_HardSign;
            keyMap["ї"] = Key_Uk_Yi;
            keyMap["capslock"] = Key_CapsLock;
            keyMap["ф"] = Key_Uk_F;
            keyMap["ґ"] = Key_Uk_Gh;
            keyMap["і"] = Key_Uk_I;
            keyMap["в"] = Key_Uk_V;
            keyMap["а"] = Key_Uk_A;
            keyMap["п"] = Key_Uk_P;
            keyMap["р"] = Key_Uk_R;
            keyMap["о"] = Key_Uk_O;
            keyMap["л"] = Key_Uk_L;
            keyMap["д"] = Key_Uk_D;
            keyMap["ж"] = Key_Uk_Zh;
            keyMap["shift"] = Key_ShiftLeft;
            keyMap["є"] = Key_Uk_Ye;
            keyMap["я"] = Key_Uk_Ya;
            keyMap["ч"] = Key_Uk_Ch;
            keyMap["с"] = Key_Uk_S;
            keyMap["м"] = Key_Uk_M;
            keyMap["и"] = Key_Uk_Y;
            keyMap["т"] = Key_Uk_T;
            keyMap["ь"] = Key_Uk_SoftSign;
            keyMap["б"] = Key_Uk_B;
            keyMap["ю"] = Key_Uk_Yu;
            keyMap[","] = Key_Comma;
            keyMap["ctrl"] = Key_CtrlLeft;
            keyMap["alt"] = Key_AltLeft;
            keyMap[" "] = Key_Space;
            keyMap["!"] = Key_Exclamation;
            keyMap["+"] = Key_Plus;
            keyMap[":"] = Key_Colon;
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string login)
            {
                currentUser = login;
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

                button.Background = new SolidColorBrush(Color.FromArgb(255, 169, 169, 169));

                if (key == "Backspace")
                {
                    if (InputTextBox.Text.Length > 0)
                    {
                        InputTextBox.Text = InputTextBox.Text.Substring(0, InputTextBox.Text.Length - 1);
                    }
                }
                else if (key == "Tab")
                {
                    InputTextBox.Text += "\t";
                }
                else if (key == "CapsLock")
                {
                    isCapsLockOn = !isCapsLockOn;
                }
                else if (key == "Shift")
                {
                    isShiftOn = !isShiftOn;
                }
                else if (key == "Ctrl" || key == "Alt")
                {
                    // Ігноруємо Ctrl і Alt
                }
                else if (key == " ")
                {
                    InputTextBox.Text += " ";
                }
                else
                {
                    string charToAdd = key;
                    if (isCapsLockOn || isShiftOn)
                    {
                        charToAdd = charToAdd.ToUpper();
                    }
                    InputTextBox.Text += charToAdd;
                }

                if (isShiftOn)
                {
                    isShiftOn = false;
                }

                button.Background = new SolidColorBrush(Color.FromArgb(255, 211, 211, 211));

                InputTextBox.SelectionStart = InputTextBox.Text.Length;
                InputTextBox.SelectionLength = 0;
            }
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            stopwatch.Stop();
            string input = InputTextBox.Text;

            // Обчислення статистики
            int totalCharacters = input.Length; // Усі символи, включно з пробілами
            int charactersWithoutSpaces = input.Replace(" ", "").Length; // Символи без пробілів
            int wordCount = string.IsNullOrWhiteSpace(input) ? 0 : input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length; // Кількість слів

            // Форматування статистики в один рядок
            TargetTextBlock.Text = $"Символи: {totalCharacters}   Слова: {wordCount}   Без пробілів: {charactersWithoutSpaces}\n{input}";
        }
    }
}