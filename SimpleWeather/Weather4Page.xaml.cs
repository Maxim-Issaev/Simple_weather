using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SimpleWeather
{
    public partial class Weather4Page : ContentPage
    {
        public Weather4Page()
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
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (CrossSettings.Current.GetValueOrDefault("UseGPS", false) == true)
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
        async void Button1_ClickedAsync(System.Object sender, System.EventArgs e)
        {
            await UpdateWeatherAsync();
        }
        private void Create_Frame(string Tempeature, string Status, string Time, string Icon)
        {
            StackLayout stackLayout = new StackLayout {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    new Image {WidthRequest=50,HeightRequest=50, Source=ImageSource.FromResource("SimpleWeather.icons." + Icon + ".png")},
                    new StackLayout
                    {
                        Children =
                        {
                            new StackLayout
                            {
                                Orientation=StackOrientation.Horizontal,
                                Children =
                                {
                                    new Label {Text="Вермя:"},
                                    new Label {Text=Time}
                                }
                            },
                            new StackLayout
                            {
                                Orientation=StackOrientation.Horizontal,
                                Children =
                                {
                                    new Label {Text="Температура:"},
                                    new Label {Text=Tempeature}
                                }
                            },
                            new StackLayout
                            {
                                Orientation=StackOrientation.Horizontal,
                                Children =
                                {
                                    new Label {Text="Cтатус"},
                                    new Label {Text=Status}
                                }
                            }
                        }
                    }
                }
            };
            Frame frame = new Frame
            {
                Content = stackLayout,
                CornerRadius = 20,
                Margin = new Thickness(0,15)

            };
            WeatherStack.Children.Add(frame);
        }
        public async Task<Weather4> GetWeatherAsync()
        {
            string url = "http://api.openweathermap.org/data/2.5/forecast?";
            //string url = "http://api.openweathermap.org/data/2.5/forecast?q=Moscow&appid=19cfe7ddf6b7b30677251b4ea4f0c6c0&units=metric&lang=ru";
            if (CrossSettings.Current.GetValueOrDefault("UseGPS", false) == false)
            {
                    url +="q="+ Editor1.Text;
            }
            else
            {
                Location loacation = await GetLocationAsync();
                if (loacation == null)
                {
                    await DisplayAlert("Ошибка :(", "Не удалось получить местоположение", "Ok");
                    return null;
                }
                else
                    url += "lat="+ Convert.ToString(loacation.Latitude) + "&lon=" + Convert.ToString(loacation.Longitude);
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
                Weather4 weather = JsonConvert.DeserializeObject<Weather4>(output);
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
            Indicator.IsRunning = true;
            Weather4 weather4 = await GetWeatherAsync();
            string Temperature, Status, Icon;
            DateTime Time;
            if (weather4 != null)
            {
                Label1.Text =  weather4.City.Name;
                WeatherStack.Children.Clear();
                int ListSize = weather4.WeatherList.Length;
                for (int i=0; i<ListSize;i++)
                {
                    if (i % CrossSettings.Current.GetValueOrDefault("WeatherStep", 8) == 0)
                    {
                        Temperature = Convert.ToInt32(weather4.WeatherList[i].MainInfo.Temp).ToString() + "°C";
                        Status = weather4.WeatherList[i].CommonWeather[0].Description;
                        Time = Convert.ToDateTime(weather4.WeatherList[i].Time);
                        Icon = weather4.WeatherList[i].CommonWeather[0].Icon;
                        if (CrossSettings.Current.GetValueOrDefault("WeatherStep", 8) == 8)
                            Create_Frame(Temperature, Status, Time.ToString("dd MMMM"), Icon);
                        else
                            Create_Frame(Temperature, Status, Time.ToString("dd MMMM HH:mm"), Icon);
                    }
                }
            }
            else
            {
                Label1.Text = "Error";
            }
            Editor1.Text = "";
            Indicator.IsRunning = false;

        }
        private async Task<Location> GetLocationAsync()
        {
            try
            {
                var GPSrequest = new GeolocationRequest(GeolocationAccuracy.Lowest);
                Location location = await Geolocation.GetLocationAsync(GPSrequest).ConfigureAwait(false);
                return location;
            }
            catch (FeatureNotSupportedException)
            {
                // Handle not supported on device exception
                return null;
            }
            catch (FeatureNotEnabledException)
            {
                // Handle not enabled on device exception
                return null;
            }
            catch (PermissionException)
            {
                // Handle permission exception
                return null;
            }
            catch (Exception)
            {
                // Unable to get location
                return null;
            }
        } 
    }
}
