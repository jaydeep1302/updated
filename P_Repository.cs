using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using PJSv2_Test1_MVC.Models;


namespace PJSv2_Test1_MVC.Repository
{
    public class P_Repository
    {
        private readonly string apiKey = string.Empty;
        private readonly string apiSecret = string.Empty;
        private readonly string paymentJsSecret = string.Empty;
        private readonly string payeezyUrl = string.Empty;

        public P_Repository()
        {
            this.payeezyUrl = "https://cert.api.firstdata.com/paymentjs/v2";
            this.apiKey = "aRRMPmVZ1YcYg1TYPDTShbF7MTHyINy1";
            this.apiSecret = "	fcaccf88badd97b8e436035f074c5a1139dfb0528722dad7ad7c651fc5fb8527";
            this.paymentJsSecret = "AoMv7RIpmiZqGoGS";
            //this.apiSecret = assets.get_asset("PayeezyApiSecret");
        }



        private string PaymentEndpoint => "v1/transactions";

        private string TransactionEndpoint => "v1/transactions/";

        public string CalculateHMAC(string data, string secret)
        {
            //HMAC SHA256 signed with Payment.js Api Secret
            HMAC hmacSha256 = new HMACSHA256(Encoding.UTF8.GetBytes(secret));

            byte[] hmac2Hex = hmacSha256.ComputeHash(Encoding.UTF8.GetBytes(data));

            string base64String = Convert.ToBase64String(hmac2Hex);
            return base64String;
        }

        public AuthorizeSessionResponse AuthorizeSession(AuthorizeSession message)
        {
            return this.CallPayeezyApi(message, "/merchant/authorize-session");
        }


        private AuthorizeSessionResponse CallPayeezyApi(dynamic request, string url)
        {
            AuthorizeSessionResponse model = new AuthorizeSessionResponse();
            string jsonString = JsonConvert.SerializeObject(request);

            Random random = new Random();
            string nonce = (random.Next(0, 1000000)).ToString();

            DateTime date = DateTime.UtcNow;
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan span = date - epoch;
            string time = ((long)span.TotalMilliseconds).ToString();


            string hashData = this.apiKey + nonce + time + jsonString;

            string message = this.CalculateHMAC(hashData, this.paymentJsSecret);


            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(this.payeezyUrl + url);

            webRequest.Method = "POST";
            webRequest.Accept = "*/*";
            webRequest.ContentLength = jsonString.Length;
            webRequest.ContentType = "application/json";
            webRequest.Headers.Add("api-key", this.apiKey);
            webRequest.Headers.Add("Nonce", nonce);
            webRequest.Headers.Add("Timestamp", time);
            webRequest.Headers.Add("Message-Signature", message);




            var writer = new StreamWriter(webRequest.GetRequestStream());
            writer.Write(jsonString);
            writer.Close();



            try
            {
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader responseStream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        var stringData = responseStream.ReadToEnd();


                        for (int i = 0; i < webResponse.Headers.Count; ++i)
                        {
                            if (webResponse.Headers.Keys[i] == "Client-Token")
                            {
                                model.ClientToken = webResponse.Headers[i];
                            }
                        }



                        model.PublicKeyBase64 = JsonConvert.DeserializeObject<payeezyAuthorize>(stringData).publicKeyBase64;

                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (HttpWebResponse errorResponse = (HttpWebResponse)ex.Response)
                    {
                        using (StreamReader reader = new StreamReader(errorResponse.GetResponseStream()))
                        {
                            string remoteEx = reader.ReadToEnd();


                            model.Errors = remoteEx;
                        }
                    }
                }
            }
            return model;
        }

        public string ByteArrayToHexString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }

        public class payeezyAuthorize
        {
            public string publicKeyBase64 { get; set; }
        }
    }
}
