using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace KeyBoardTrainer.Pages
{
    public sealed partial class GuestPage : Page
    {
        public GuestPage()
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

                    case "Training":
                        ContentFrame.Navigate(typeof(Training));
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
