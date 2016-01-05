using EpicAdventure.ViewModel;
using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
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
        public static double distance = 0;
        List<BasicGeoposition> l = new List<BasicGeoposition>();
        MapIcon CurrenPosition;
        Geolocator geo;
        public static BasicGeoposition position;
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
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
            {
                DrawCurrenPosition(new Geopoint(args.Position.Coordinate.Point.Position));
                BasicGeoposition p = new BasicGeoposition();
                p = args.Position.Coordinate.Point.Position;
                position = p;
                MapPolyline mapPolyline = new MapPolyline();
                
                l.Add(p);
                mapPolyline.Path = new Geopath(l);
                temp = args.Position.Coordinate.Point.Position;

                mapPolyline.StrokeColor = Colors.Black;
                mapPolyline.StrokeThickness = 3;
                mapPolyline.StrokeDashed = true;
                Map.MapElements.Remove(mapPolyline);
                Map.MapElements.Add(mapPolyline);

                if(CoordinateView.destination.Longitude != null)
                {
                    distance = getDistanceFromLatLonInKm(position.Latitude, position.Longitude, CoordinateView.destination.Latitude, CoordinateView.destination.Longitude);
                }

                await Map.TrySetViewAsync(args.Position.Coordinate.Point);
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

        private void Geo_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {

                }
                );
        }

        //private async void Geo_positionChanged(Geolocator sender, PositionChangedEventArgs args)
        //{
        //    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
        //        async () =>
        //        {
        //            BasicGeoposition p = new BasicGeoposition();
        //            p = args.Position.Coordinate.Point.Position;
        //            if (temp.ToString() == null)
        //                temp = args.Position.Coordinate.Point.Position;

        //            MapPolyline mapPolyline = new MapPolyline();
        //            var l = new List<BasicGeoposition>();
        //            l.Add(p);
        //            l.Add(temp);
        //            mapPolyline.Path = new Geopath(l);



        //            mapPolyline.StrokeColor = Colors.Black;
        //            mapPolyline.StrokeThickness = 3;
        //            mapPolyline.StrokeDashed = true;
        //            Map.MapElements.Add(mapPolyline);



        //            await Map.TrySetViewAsync(args.Position.Coordinate.Point);
        //        }
        //        );
        //}
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
