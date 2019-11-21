using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace PJSv2_Test1_MVC.Models
{
    public class AuthorizeSessionResponse
    {
        public string PublicKeyBase64 { get; set; }
        public string ClientToken { get; set; }
        public string Errors { get; set; }

    }
}