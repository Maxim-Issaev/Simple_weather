using System;
using Newtonsoft.Json;

namespace SimpleWeather
{
    public class Weather
    {
        [JsonProperty("weather")]
        public CommonWeather [] CommonWeather { get; set; }

        [JsonProperty("main")]
        public Main MainInfo { get; set; }

        public string Name { get; set; }
    }
    public class CommonWeather
    {
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
    public class Main
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }
    }
    public class ErrorWeather
    {
        public int code { get; set; }
    }
}