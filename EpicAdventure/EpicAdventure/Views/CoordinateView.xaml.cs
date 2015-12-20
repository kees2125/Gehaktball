using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EpicAdventure
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CoordinateView : Page
    {
        public CoordinateView()
        {
            this.InitializeComponent();
            startRoute.IsEnabled = false;
        }

        //private void newButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Menu.IsPaneOpen = !Menu.IsPaneOpen;
        //}

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void startRoute_Click(object sender, RoutedEventArgs e)
        {
            string text = "";
            if(Degrees1.Text.Length.Equals(0))
            {
                text = "Please fill in the degrees";
            }
            var dialog = new Windows.UI.Popups.MessageDialog(text);

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });
            
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            var result = await dialog.ShowAsync();
        }

        private void FilledCoordinatesTest(object sender, TextChangedEventArgs e)
        {
            if (Degrees1.Text.Length <2) { startRoute.IsEnabled = false; }
            else if (Minutes1.Text.Length <1) { startRoute.IsEnabled = false; }
            else if (Secondes1.Text.Length <2){ startRoute.IsEnabled = false; }
            else if (Degrees2.Text.Length < 2) { startRoute.IsEnabled = false; }
            else if (Minutes2.Text.Length < 1) { startRoute.IsEnabled = false; }
            else if (Secondes2.Text.Length < 2) { startRoute.IsEnabled = false; }
            else { startRoute.IsEnabled = true; }
        }
    }
}
