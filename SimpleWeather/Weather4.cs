﻿using System;
using Newtonsoft.Json;

namespace SimpleWeather
{
    public class Weather4
    {
        public Weather4()
        {

        }
        [JsonProperty("list")]
        public WeatherList[] WeatherList{get;set;}
        [JsonProperty("city")]
        public City City { get; set; }
    }
    public class  WeatherList
    {
        [JsonProperty("main")]
        public Main MainInfo { get; set; }
        [JsonProperty("weather")]
        public CommonWeather[] CommonWeather { get; set; }
        [JsonProperty("dt_txt")]
        public string Time { get; set; }
    }
    public class City
    {
        public string Name { get; set; }
    }
}
