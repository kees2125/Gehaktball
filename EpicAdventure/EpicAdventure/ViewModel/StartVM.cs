using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicAdventure.ViewModel
{
    public class StartVM : INotifyPropertyChanged
    {
        private static string _afstand;

        public string Afstand
        {
            get { return _afstand; }
            set { _afstand = value; NotifyPropertyChanged(nameof(Afstand)); }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
