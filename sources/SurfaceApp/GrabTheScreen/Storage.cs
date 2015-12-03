using System.Configuration;
using RestSharp;

namespace GrabTheScreen
{
    class Storage
    {
        private readonly RestClient _restClient;

        public Storage()
        {
            var ip = ConfigurationManager.AppSettings.Get("storage-ip");
            var port = ConfigurationManager.AppSettings.Get("storage-port");
            var url = string.Format("http://{0}:{1}/string-store", ip, port);
            
            _restClient = new RestClient(url);
            _restClient.Execute(new RestRequest());
        }

        public string Load(string key)
        {
            var request = new RestRequest("get", Method.GET);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "*/*");
            request.AddQueryParameter("key", key);

            IRestResponse response = _restClient.Execute(request);
            return response.Content;
        }

        public void Save(string key, string value)
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
