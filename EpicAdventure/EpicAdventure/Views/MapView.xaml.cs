using EpicAdventure.ViewModel;
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
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Core;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EpicAdventure.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapView : Page
    {
        MapIcon CurrenPosition;
        Geolocator geo;
        bool PositionTracking = false;
        BasicGeoposition temp;
        public MapView()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            this.DataContext = new MapVM();

            CurrenPosition = new MapIcon();
            
            App.Geo.PositionChanged += Geo_PositionChanged;
            StartTracking();
        }

        private void Geo_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                DrawCurrenPosition(new Geopoint(args.Position.Coordinate.Point.Position));
                drawline();
            });
        }

        private void DrawCurrenPosition(Geopoint p)
        {
          
            CurrenPosition.Location = p;
            CurrenPosition.NormalizedAnchorPoint = new Point(0.5, 1.0);
            CurrenPosition.Title = "Current Position";
            CurrenPosition.ZIndex = 999;
            Map.MapElements.Remove(CurrenPosition);
            Map.MapElements.Add(CurrenPosition);

        }
        private void Geo_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {

                }
                );
        }

        private async void drawline()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                async () =>
                {
                    BasicGeoposition p = new BasicGeoposition();
                    p = args.Position.Coordinate.Point.Position;
                    if (temp.ToString() == null)
                        temp = args.Position.Coordinate.Point.Position;

                    MapPolyline mapPolyline = new MapPolyline();
                    var l = new List<BasicGeoposition>();
                    l.Add(p);
                    l.Add(temp);
                    mapPolyline.Path = new Geopath(l);



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
                    geo = new Geolocator { ReportInterval = 100 };

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
