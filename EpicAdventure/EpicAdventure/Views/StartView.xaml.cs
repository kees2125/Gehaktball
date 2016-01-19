using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using EpicAdventure.Views;
using EpicAdventure.ViewModel;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EpicAdventure.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartView : Page
    {
        private Compass _compass; // Our app's compass object
        public static string status="";
        public static string acuratie="";
        public static string source="";
        public static StartVM v = new StartVM();
        // This event handler writes the current compass reading to 
        // the textblocks on the app's main page.
        public static double distance = 0;
        int counter = 0;
        public String afstandstring;
        bool playing = true;
        List<BasicGeoposition> l = new List<BasicGeoposition>();
        MapIcon CurrenPosition;
        Geolocator geo;
        public static BasicGeoposition position;
        bool PositionTracking = false;
        BasicGeoposition temp;
        bool zoomSet = false;

        private async void ReadingChanged(object sender, CompassReadingChangedEventArgs e)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                CompassReading reading = e.Reading;
                String value = String.Format("{0,5:0.00}", reading.HeadingTrueNorth);
                IMAGETRANSFORM.Rotation = -double.Parse(value);
                IMAGETRANSFORM1.Rotation = -double.Parse(value);

            });
        }

        public StartView()
        {
            this.DataContext = v;
            this.InitializeComponent();
            _compass = Compass.GetDefault(); // Get the default compass object
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;

            //map !!!!!!!!!
            //this.DataContext = new MapVM();

            CurrenPosition = new MapIcon();

            App.Geo.PositionChanged += Geo_PositionChanged;
            StartTracking();
            //map !!!!!!

            // Assign an event handler for the compass reading-changed event
            if (_compass != null)
            {
                // Establish the report interval for all scenarios
                uint minReportInterval = _compass.MinimumReportInterval;
                uint reportInterval = minReportInterval > 16 ? minReportInterval : 16;
                _compass.ReportInterval = reportInterval;
                _compass.ReadingChanged += new TypedEventHandler<Compass, CompassReadingChangedEventArgs>(ReadingChanged);
            }

            image.Source = new BitmapImage(new Uri("ms-appx:///Resources/test1.png"));
            image1.Source = new BitmapImage(new Uri("ms-appx:///Resources/test2.png"));
            image2.Source = new BitmapImage(new Uri("ms-appx:///Resources/test.png"));
           
        }
        //map testsosjfdosaifdspafjisoafajfopajwpfasj

        private async void Geo_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
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
                MenuVM temps = new MenuVM();
                temps.Accuracy = args.Position.Coordinate.Accuracy.ToString() + "m";
                temps.Source = args.Position.Coordinate.PositionSource.ToString();
                temps.Status = sender.LocationStatus.ToString();
                //Accuracy = App.Geo.Position.Coordinate.Accuracy.ToString() + "m";
                //Source = App.Geo.Position.Coordinate.PositionSource.ToString();
                if (zoomSet == false)
                {
                    Map.ZoomLevel = 15;
                    Map.Center = new Geopoint(position);
                    zoomSet = true;
                }

                if (CoordinateView.Longitude1 != 0)
                {
                    distance = getDistanceFromLatLonInKm(CoordinateView.Lattitude1, CoordinateView.Longitude1, position.Latitude, position.Longitude);
                    afstandstring = Math.Round(distance * 1000) / 1000 +"";
                    afstandstring= afstandstring.Replace('.', ',');
                    StartView.v.Afstand = "Afstand: " + afstandstring + "KM";

                    if (distance < 0.020 && playing == true)
                    {
                        var dialog = new MessageDialog("You found your treasure be happy or somthing");
                        playing = false;
                        await dialog.ShowAsync();
                    }
                    
                }
                else
                {
                    StartView.v.Afstand = "Afstand:";
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
        private async void StartTracking()
        {
            // Request permission to access location
            var accessStatus = await Geolocator.RequestAccessAsync();



            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    geo = new Geolocator { ReportInterval = 1500 };

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
