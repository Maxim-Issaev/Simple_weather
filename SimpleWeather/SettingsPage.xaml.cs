using System;
using System.Collections.Generic;
using Plugin.Settings;
using Xamarin.Forms;

namespace SimpleWeather
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            Switch1.Toggled += Switch1_Toggled;
            Switch1.IsToggled = CrossSettings.Current.GetValueOrDefault("UseGPS",false);
        }
        private void Switch1_Toggled(object sender, ToggledEventArgs e)
        {
            if (Switch1.IsToggled)
            {
                CrossSettings.Current.AddOrUpdateValue("UseGPS", true);
            }
            else
            {
                CrossSettings.Current.AddOrUpdateValue("UseGPS", false);
            }

        }
    }
}
