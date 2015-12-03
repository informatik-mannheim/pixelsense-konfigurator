using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using RestSharp;

namespace GrabTheScreen
{
    class Storage
    {
        private string URL;
        private RestClient _restClient;

        public Storage()
        {
            String ip = ConfigurationManager.AppSettings.Get("storage-ip");
            String port = ConfigurationManager.AppSettings.Get("storage-port");
            URL = String.Format("http://{0}:{1}/string-store", ip, port);
            
            _restClient = new RestClient(URL);
        }

        public String Load(String key)
        {
            var request = new RestRequest("get", Method.GET);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "*/*");
            request.AddQueryParameter("key", key);

            IRestResponse response = _restClient.Execute(request);
            return response.Content;
        }

        public void Save(String key, String value)
        {
            var request = new RestRequest("set", Method.POST);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "*/*");

            request.AddQueryParameter("key", key);
            request.AddQueryParameter("value", value);

            _restClient.Execute(request);
        }
    }
}
