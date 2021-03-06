﻿using EpicAdventure.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
        public static BasicGeoposition destination;
        public static double Lattitude1;
        public static double Longitude1;
        public CoordinateView()
        {
            this.InitializeComponent();
            startRoute.IsEnabled = false;
            startRoute1.IsEnabled = false;
            readcordinates();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }
        private async void readcordinates()
        {
            try
            {
                Windows.Storage.StorageFolder storageFolder =
                Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile =
                    await storageFolder.GetFileAsync("sample.txt");
                string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
                string[] text2 = text.Split('/');
                decimalDegrees.Text = text2[0];
                decimalDegrees2.Text = text2[1];
            }
            catch
            {

            }
           
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
             Secondes1.Text= Secondes1.Text.Replace(',', '.');
             Secondes1.Text= Secondes2.Text.Replace(',', '.');
            if (toggleSwitch.IsOn)
            {
                Lattitude1 = -(int.Parse(Degrees1.Text) + (((double.Parse(Minutes1.Text)) * 60 + double.Parse(Secondes1.Text)) / 3600));
            }
            else
            {
                Lattitude1 = int.Parse(Degrees1.Text) + (((double.Parse(Minutes1.Text)) * 60 + double.Parse(Secondes1.Text)) / 3600);
            }
            if (toggleSwitch1.IsOn)
            {
                Longitude1 = -(int.Parse(Degrees2.Text) + (((double.Parse(Minutes2.Text)) * 60 + double.Parse(Secondes2.Text)) / 3600));
            }
            else
            {
                Longitude1 = int.Parse(Degrees2.Text) + (((double.Parse(Minutes2.Text)) * 60 + double.Parse(Secondes2.Text)) / 3600);
            }
            Frame.Navigate(typeof(StartView));
        }

        
        public double deg2rad(double deg)
        {
            return deg * (Math.PI / 180);
        }
        public double getDistanceFromLatLonInKm(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = deg2rad((lat2 - lat1));  // deg2rad below
            var dLon = deg2rad(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
              ;
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c; // Distance in km
            return d;
        }


        private async void startRoute1_Click(object sender, RoutedEventArgs e)
        {
            decimalDegrees.Text = decimalDegrees.Text.Replace(',', '.');
            decimalDegrees2.Text = decimalDegrees2.Text.Replace(',', '.');
            if (toggleSwitch2.IsOn)
            {
                Lattitude1 = -(double.Parse(decimalDegrees.Text));
            }
            else
            {
                Lattitude1 = double.Parse(decimalDegrees.Text);
            }
            if (toggleSwitch3.IsOn)
            {
                Longitude1 = -(double.Parse(decimalDegrees2.Text));
            }
            else
            {
                Longitude1 = double.Parse(decimalDegrees2.Text);
            }
            Frame.Navigate(typeof(StartView));

            Windows.Storage.StorageFolder storageFolder =
            Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile =
                await storageFolder.CreateFileAsync("sample.txt",
                    Windows.Storage.CreationCollisionOption.ReplaceExisting);

            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, Lattitude1 + "/" + Longitude1);


        }

        private void FilledCoordinatesTest(object sender, TextChangedEventArgs e)
        {
            if (Degrees1.Text.Length <1) {startRoute.IsEnabled = false;}
            else if (Minutes1.Text.Length <1) { startRoute.IsEnabled = false; }
            else if (Secondes1.Text.Length <2){ startRoute.IsEnabled = false; }
            else if (Degrees2.Text.Length < 1) { startRoute.IsEnabled = false; }
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
