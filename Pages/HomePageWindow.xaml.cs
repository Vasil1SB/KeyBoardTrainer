using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Player; // Додано для доступу до сторінки Player

namespace KeyBoardTrainer.Pages
{
    public sealed partial class HomePageWindow : Page
    {
        public HomePageWindow()
        {
            this.InitializeComponent();
            if (ContentFrame != null)
            {
                ContentFrame.Navigate(typeof(WelcomePage));
            }
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                var tag = selectedItem.Tag?.ToString();

                switch (tag)
                {
                    case "home":
                        ContentFrame.Navigate(typeof(WelcomePage));
                        break;

                    case "Recomendation":
                        ContentFrame.Navigate(typeof(Recomendation));
                        break;

                    case "Player":
                        ContentFrame.Navigate(typeof(Player.Player)); 
                        break;

                    case "Training":
                        ContentFrame.Navigate(typeof(Training));
                        break;


                    case "Counter":
                        ContentFrame.Navigate(typeof(Counter));
                        break;


                    case "NumTraining":
                        ContentFrame.Navigate(typeof(NumTraining));
                        break;

                    case "Info":
                        ContentFrame.Navigate(typeof(AboutPage));
                        break;

                    case "Settings":
                        ContentFrame.Navigate(typeof(Settings));
                        break;

                    case "Leave":
                        Frame.Navigate(typeof(Menu));
                        break;
                }
            }
        }
    }
}