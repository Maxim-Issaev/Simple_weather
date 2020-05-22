using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SimpleWeather
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class CurrentWeatherPage : ContentPage
    {
        public CurrentWeatherPage()
        {
            InitializeComponent();
        }
        protected override async void  OnAppearing()
        {
            base.OnAppearing();
            await UpdateWeatherAsync();
            if (CrossSettings.Current.GetValueOrDefault("UseGPS",false)==true)
            {
                Editor1Frame.IsVisible = false;
                Label4Frame.IsVisible = true;
            }
            else
            {
                Editor1Frame.IsVisible = true;
                Label4Frame.IsVisible = false;
            }
        }
        public async Task<Weather> GetWeatherAsync() // Функиция для полученя информации о погоде
        {
            string url;
            if (CrossSettings.Current.GetValueOrDefault("UseGPS", false) == false)
                url = "http://api.openweathermap.org/data/2.5/weather?q=" + Editor1.Text;
            else
            {
                Location loacation = await GetLocationAsync();
                if (loacation == null)
                    return null;
                else
                    url = "http://api.openweathermap.org/data/2.5/weather?lat=" + Convert.ToString(loacation.Latitude) + "&lon=" + Convert.ToString(loacation.Longitude);
            }
            url+= "&appid=19cfe7ddf6b7b30677251b4ea4f0c6c0&units=metric&lang=ru";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string output;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    output = streamReader.ReadToEnd();
                }
                Weather weather = JsonConvert.DeserializeObject<Weather>(output);
                return weather;

            }
            catch (WebException error)
            {
                try
                {
                    HttpWebResponse httpWebError = (HttpWebResponse)error.Response;
                    if (httpWebError.StatusCode == HttpStatusCode.NotFound)
                    {
                        await DisplayAlert("Ошибка :(", "Не удалось найти город.", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Ошибка :(", error.Message, "Ok");
                    }
                }
                catch (NullReferenceException)
                {
                    await DisplayAlert("Ошибка :(", "Не удалось установить соединение.", "Ok");
                }
                return null;
            }

        }
        private async Task UpdateWeatherAsync() // обновление погоды и обновление погоды на giu
        {
            Indicator.IsRunning= true;
            Weather weather = await GetWeatherAsync();
            if (weather == null)
            {
                Label1.Text = "Error";
                Label2.Text = "Error";
                Label3.Text = "Error";
                Editor1.Text = "";
                Image1.Source = ImageSource.FromResource("SimpleWeather.icons.error.png");
            }
            else
            {
                Label1.Text = weather.Name;
                Label2.Text = Convert.ToInt32(weather.MainInfo.Temp).ToString() + "°C";
                Label3.Text = weather.CommonWeather[0].Description;
                Image1.Source = ImageSource.FromResource("SimpleWeather.icons." + weather.CommonWeather[0].Icon + ".png");
            }
            Indicator.IsRunning = false;

        }

        async void Button1_ClickedAsync(System.Object sender, System.EventArgs e)
        {
           await UpdateWeatherAsync();
        }
        private async Task<Location> GetLocationAsync()
        {
            try
            {
                var GPSrequest = new GeolocationRequest(GeolocationAccuracy.Lowest);
                Location location = await Geolocation.GetLocationAsync(GPSrequest).ConfigureAwait(false);
                return location;
            }
            catch (FeatureNotSupportedException )
            {
                // Handle not supported on device exception
                return null;
            }
            catch (FeatureNotEnabledException )
            {
                // Handle not enabled on device exception
                return null;
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                return null;
            }
            catch (Exception ex)
            {
                // Unable to get location
                return null;
            }
        }
    }

}
