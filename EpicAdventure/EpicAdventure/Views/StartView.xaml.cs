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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EpicAdventure.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartView : Page
    {
        private Compass _compass; // Our app's compass object
        public static StartVM v = new StartVM();
        // This event handler writes the current compass reading to 
        // the textblocks on the app's main page.

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

            //this.DataContext = new MapVM();

            //CurrenPosition = new MapIcon();
            
            //App.Geo.PositionChanged += Geo_PositionChanged;
            //StartTracking();

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
    }
}
