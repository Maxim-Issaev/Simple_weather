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
using Xamarin.Forms;

namespace SimpleWeather
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateWeather();
        }
        public Weather GetWeather() // Функиция для полученя информации о погоде
        {
           string url = "http://api.openweathermap.org/data/2.5/weather?q="+Editor1.Text+"&appid=19cfe7ddf6b7b30677251b4ea4f0c6c0&units=metric&lang=ru";
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
                        DisplayAlert("Ошибка :(", "Не удалось найти город.", "Ok");
                    }
                    else
                    {
                        DisplayAlert("Ошибка :(", error.Message, "Ok");
                    }
                }
                catch (NullReferenceException)
                {
                    DisplayAlert("Ошибка :(", "Не удалось установить соединение.", "Ok");
                }
                return null;
            }
  
        }
        private void UpdateWeather() // обновление погоды и обновление погоды на giu
        {
            Weather weather = GetWeather();
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
            
        }

        void Button1_Clicked(System.Object sender, System.EventArgs e)
        {
            UpdateWeather();
        }
    }

}
