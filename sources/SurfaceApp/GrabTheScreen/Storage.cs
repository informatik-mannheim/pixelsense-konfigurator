using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Runtime.Remoting;
using RestSharp;

namespace GrabTheScreen
{
    /// <summary>
    /// Handles communication with the REST-Server / string store. Loads string values for a given key or inserts a value for the given key.
    /// IP and Port of the server can be configured in the app.config.
    /// </summary>
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
        /// Loads the value for a given key. Raises exceptions if either the server is unreachable or the key can't be found.
        /// </summary>
        /// <param name="key">The key to return the value for.</param>
        /// <returns>The value.</returns>
        /// <exception cref="ServerException"></exception>
        /// <exception cref="KeyNotFoundException"></exception>
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
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                throw new KeyNotFoundException(string.Format("Key '{0}' not found.", key));
            }

            return response.Content;
        }

        /// <summary>
        /// Saves a tuple of key and value, both strings. Doesn't give a feedback whether the insert was successful or not.
        /// </summary>
        /// <param name="key">The key under which to insert.</param>
        /// <param name="value">The value to be inserted.</param>
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