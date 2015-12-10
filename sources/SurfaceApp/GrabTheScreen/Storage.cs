using System;
using System.Configuration;
using System.Runtime.Remoting;
using RestSharp;

namespace GrabTheScreen
{
    internal class Storage
    {
        private readonly RestClient _restClient;
        private readonly string _ip;
        private readonly string _port;

        public Storage()
        {
            _ip = ConfigurationManager.AppSettings.Get("storage-ip");
            _port = ConfigurationManager.AppSettings.Get("storage-port");
            var url = string.Format("http://{0}:{1}/string-store", _ip, _port);

            _restClient = new RestClient(url);
            _restClient.Execute(new RestRequest());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ServerException"></exception>
        public string Load(string key)
        {
            var request = new RestRequest("get", Method.GET);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "*/*");
            request.AddQueryParameter("key", key);

            var response = _restClient.Execute(request);
            
            if (response.ResponseStatus == ResponseStatus.Error)
            {
                throw new ServerException(string.Format("{0} ({1}:{2})", response.ErrorMessage, _ip, _port));
            }

            return response.Content;
        }

        public void Save(string key, string value)
        {
            var request = new RestRequest("set", Method.POST);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "*/*");

            request.AddQueryParameter("key", key);
            request.AddQueryParameter("value", value);

            var response = _restClient.Execute(request);
            if (response.ResponseStatus == ResponseStatus.Error)
            {
                Logger.Log(string.Format("{0} ({1}:{2})", response.ErrorMessage, _ip, _port));
            }
        }
    }
}