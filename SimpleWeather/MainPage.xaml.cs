using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimpleWeather
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            TabPage1.Title = "Погода сейчас";
            TabPage1.IconImageSource = ImageSource.FromResource("SimpleWeather.icons.tabIcons.cloud.png");
            TabPage2.Title = "Настройки";
            TabPage2.IconImageSource = ImageSource.FromResource("SimpleWeather.icons.tabIcons.settings.png");
        }
    }
}
