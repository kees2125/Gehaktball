﻿using EpicAdventure.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
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
        double Lattitude;
        double Longitude;
        public static BasicGeoposition destination;
        public CoordinateView()
        {
            this.InitializeComponent();
            startRoute.IsEnabled = false;
            startRoute1.IsEnabled = false;
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        //private void newButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Menu.IsPaneOpen = !Menu.IsPaneOpen;
        //}

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        //private async void startRoute_Click(object sender, RoutedEventArgs e)
        //{
        //    string text = "";
        //    if(Degrees1.Text.Length.Equals(0))
        //    {
        //        text = "Please fill in the degrees";
        //    }
        //    var dialog = new Windows.UI.Popups.MessageDialog(text);

        //    dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
        //    dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });

        //    dialog.DefaultCommandIndex = 0;
        //    dialog.CancelCommandIndex = 1;

        //    var result = await dialog.ShowAsync();
        //}
        private void startRoute_Click(object sender, RoutedEventArgs e)
        {
            if (toggleSwitch.IsOn)
            {
                Longitude = -(int.Parse(Degrees1.Text) + (((double.Parse(Minutes1.Text)) * 60 + double.Parse(Secondes1.Text)) / 3600));
            }
            else
            {
                Longitude = int.Parse(Degrees1.Text) + (((double.Parse(Minutes1.Text)) * 60 + double.Parse(Secondes1.Text)) / 3600);
            }
            if (toggleSwitch1.IsOn)
            {
                Lattitude = -(int.Parse(Degrees2.Text) + (((double.Parse(Minutes2.Text)) * 60 + double.Parse(Secondes2.Text)) / 3600));
            }
            else
            {
                Lattitude = int.Parse(Degrees2.Text) + (((double.Parse(Minutes2.Text)) * 60 + double.Parse(Secondes2.Text)) / 3600);
            }
            //decimalDegrees.Text = convertDMS.ToString();
            //decimalDegrees2.Text = convertDMS1.ToString();
            //destination = new Geocoordinate();
            destination = new BasicGeoposition();
            destination.Latitude = Lattitude;
            destination.Longitude = Longitude;
            Frame.Navigate(typeof(StartView));
        }


       

        private void startRoute1_Click(object sender, RoutedEventArgs e)
        {
            if (toggleSwitch2.IsOn)
            {
                Longitude = -(double.Parse(decimalDegrees.Text));
            }
            else
            {
                Longitude = double.Parse(decimalDegrees.Text);
            }
            if (toggleSwitch3.IsOn)
            {
                Lattitude = -(double.Parse(decimalDegrees2.Text));
            }
            else
            {
                Lattitude = double.Parse(decimalDegrees2.Text);
            }
            destination = new BasicGeoposition();
            destination.Latitude = Lattitude;
            destination.Longitude = Longitude;
            Frame.Navigate(typeof(StartView));
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

        private void FilledCoordinatesTest1(object sender, TextChangedEventArgs e)
        {
            if (decimalDegrees.Text.Length < 2) { startRoute1.IsEnabled = false; }
            else if (decimalDegrees2.Text.Length < 2) { startRoute1.IsEnabled = false; }
            else { startRoute1.IsEnabled = true; }
        }
    }
}
