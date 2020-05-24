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
            if (Device.RuntimePlatform == Device.iOS)
            {
                ContentPage1.BackgroundColor = Color.FromRgb(242, 242, 247);
                Stack1.BackgroundColor = Color.FromRgb(249, 249, 248);
            }
            if (Device.RuntimePlatform==Device.Android)
            {
                Stack1.IsVisible = false;
            }
            Switch1.Toggled += Switch1_Toggled;
            Switch1.IsToggled = CrossSettings.Current.GetValueOrDefault("UseGPS", false);
            int item = CrossSettings.Current.GetValueOrDefault("WeatherStep", 8);
            if (item == 8)
                Picker1.SelectedIndex = 0;
            else if (item == 4)
                Picker1.SelectedIndex = 1;
            else if (item == 2)
                Picker1.SelectedIndex = 2;
            else if (item == 1)
                Picker1.SelectedIndex = 3;
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

        void Picker1_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            int item = Picker1.SelectedIndex;
            if (item == 0)
                CrossSettings.Current.AddOrUpdateValue("WeatherStep", 8);
            else if (item==1)
                 CrossSettings.Current.AddOrUpdateValue("WeatherStep", 4);
            else if (item == 2 )
                CrossSettings.Current.AddOrUpdateValue("WeatherStep", 2);
            else if (item==3)
                CrossSettings.Current.AddOrUpdateValue("WeatherStep", 1);

        }
    }
}
