using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace PJSv2_Test1_MVC.Models
{
    public class AuthorizeSession
    {
        [JsonProperty(PropertyName = "gateway")]
        public string Gateway { get; set; }

        [JsonProperty(PropertyName = "apiKey")]
        public string ApiKey { get; set; }

        [JsonProperty(PropertyName = "apiSecret")]
        public string ApiSecret { get; set; }

        [JsonProperty(PropertyName = "authToken")]
        public string AuthToken { get; set; }

        [JsonProperty(PropertyName = "transarmorToken")]
        public string TransarmorToken { get; set; }

        [JsonProperty(PropertyName = "zeroDollarAuth")]
        public bool ZeroDollarAuth { get; set; }

    }
}