using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using EpicAdventure.Views;
using EpicAdventure.Helpers;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EpicAdventure
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        

        public MainPage()
        {
            this.InitializeComponent();
            Frame.Navigated += Frame_Navigated;
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            Frame.Navigate(typeof(StartView));
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if(Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }

        private void GPSRefresh_Tapped(object sender, TappedRoutedEventArgs e)
        {
            App.Geo.ForceRefresh();
        }

        private void NavList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavView.IsPaneOpen = false;

            if (NavListMap.IsSelected)
                Frame.Navigate(typeof(MapView));
            else if (NavListSetCoordinates.IsSelected)
                Frame.Navigate(typeof(CoordinateView));
            else if (NavListHome.IsSelected)
                Frame.Navigate(typeof(StartView));
        }

        private void MySplitviewPane_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.Cumulative.Translation.X < -20)
            {
                NavView.IsPaneOpen = false;
            }
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            //Dirty Hack
            string pagename = e.SourcePageType.ToString().Split('.').Last();

            NavList.SelectedIndex = -1;

            switch (pagename.ToLower())
            {
                default:
                //case "helpview":
                //    PageTitle.Text = Util.Loader.GetString("PageTitleHelp");
                //    NavListHelp.IsSelected = true;
                //    break;
                //case "settingsview":
                //    PageTitle.Text = Util.Loader.GetString("PageTitleSettings");
                //    NavListSettings.IsSelected = true;
                //    break;
                case "mapview":
                    //NavListSetCoordinates.IsSelected = true;
                    PageTitle.Text = "Map";
                    break;
                case "coordinateview":
                    PageTitle.Text = "Coordinates";
                    //NavListSetCoordinates.IsSelected = true;
                    //PageTitle.Text = Util.Loader.GetString("PageTitleRouteDetail");
                    break;
                //case "routeview":
                //    PageTitle.Text = Util.Loader.GetString("PageTitleRoute");
                //    NavListRoute.IsSelected = true;
                //    break;
                case "startview":
                    PageTitle.Text = "Home";
                    //PageTitle.Text = Util.Loader.GetString("PageTitleWaypoint");
                    break;
            }

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                Frame.CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private void Grid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.Cumulative.Translation.X > 20)
            {
                NavView.IsPaneOpen = true;
            }
        }

        private void NavList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            NavView.IsPaneOpen = false;
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            NavView.IsPaneOpen = !NavView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(CoordinateView));
        }

        

    }
}