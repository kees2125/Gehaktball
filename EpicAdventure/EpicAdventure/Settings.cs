﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Globalization;
using Windows.Storage;

namespace NavCityBreda.Helpers
{
    static class Settings
    {
        public static ApplicationDataContainer LOCAL_SETTINGS = ApplicationData.Current.LocalSettings;

        static Settings()
        {
            //Define default settings here
        }

        public static async void ChangeLanguage(string lang)
        {
                ApplicationLanguages.PrimaryLanguageOverride = lang;
                await Task.Delay(TimeSpan.FromMilliseconds(100));
                App.rootFrame.Navigate(typeof(MainPage));
        }
    }
}
