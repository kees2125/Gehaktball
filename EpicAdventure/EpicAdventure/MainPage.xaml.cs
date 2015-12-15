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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EpicAdventure
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Geolocator geo;
        bool PositionTracking = false;
        BasicGeoposition temp;

        public MainPage()
        {
            this.InitializeComponent();
            StartTracking();
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            Menu.IsPaneOpen = !Menu.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPage1));
        }

        private void Geo_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {

                }
                );
        }

        private async void Geo_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                async () =>
                {
                    BasicGeoposition p = new BasicGeoposition();
                    p = args.Position.Coordinate.Point.Position;
                    if (temp.ToString() == null)
                        temp = args.Position.Coordinate.Point.Position;

                    MapPolyline mapPolyline = new MapPolyline();
                    mapPolyline.Path = new Geopath(
                        new List<BasicGeoposition>() {
                    p,temp });



                    mapPolyline.StrokeColor = Colors.Black;
                    mapPolyline.StrokeThickness = 3;
                    mapPolyline.StrokeDashed = true;
                    Map.MapElements.Add(mapPolyline);



                    await Map.TrySetViewAsync(args.Position.Coordinate.Point);
                }
                );
        }
        private async void StartTracking()
        {
            // Request permission to access location
            var accessStatus = await Geolocator.RequestAccessAsync();

            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    geo = new Geolocator { ReportInterval = 500 };

                    // Subscribe to PositionChanged event to get updated tracking positions
                    geo.PositionChanged += Geo_PositionChanged;

                    // Subscribe to StatusChanged event to get updates of location status changes
                    geo.StatusChanged += Geo_StatusChanged;

                    PositionTracking = true;

                   

                    break;

                case GeolocationAccessStatus.Denied:

                    break;

                case GeolocationAccessStatus.Unspecified:

                    break;
            }


        }

    }
}