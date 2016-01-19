using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Core;

namespace EpicAdventure.ViewModel
{
    public class MenuVM : INotifyPropertyChanged
    {
        CoreDispatcher dispatcher;
        private string _Accuracy;
        private string _Source;
        private string _Status;
        public MenuVM()
        {
            dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
            App.Geo.StatusChanged += Geo_StatusChanged;
            App.Geo.PositionChanged += Geo_PositionChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Geo_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
               
                Status = App.Geo.Status.ToString();
                

            });
            
        }

        private void Geo_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Accuracy = App.Geo.Position.Coordinate.Accuracy.ToString() + "m";
                Source = App.Geo.Position.Coordinate.PositionSource.ToString();
               // NotifyPropertyChanged(nameof(Source));
               // NotifyPropertyChanged(nameof(Accuracy));
            });
        }

        public string Status
        {
            get
            {
                return _Status;
            }
            set 
            {
                _Status = value; NotifyPropertyChanged(nameof(Status));
            }
        }

        public string Source
        {
            get
            {
                if (App.Geo.Connected)
                    //  return App.Geo.Position.Coordinate.PositionSource.ToString();
                    return _Source;
                else
                    return "N/A";
            }
            set
            {
                _Source = value; NotifyPropertyChanged(nameof(Source));
            }
        }

        public string Accuracy
        {
            get
            {

                if (App.Geo.Connected )
                    //  return App.Geo.Position.Coordinate.Accuracy.ToString() + "m";
                    return _Accuracy;
                else
                    return "0m";
            }
            set
            {
                _Accuracy = value; NotifyPropertyChanged(nameof(Accuracy));
            }
        }
    }
}
