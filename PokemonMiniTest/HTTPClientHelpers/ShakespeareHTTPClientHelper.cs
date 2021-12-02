using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonMiniTest.HTTPClientHelpers
{
    public class ShakespeareHTTPClientHelper : IShakespeareHTTPClientHelper
    {
        IHttpClientFactory httpClientFactory;
        HttpClient client;
        String ClientName;

        public ShakespeareHTTPClientHelper(IHttpClientFactory httpClientFactory, string ClientName)
        {
            this.httpClientFactory = httpClientFactory;
            this.ClientName = ClientName;
        }

        #region Generic, Async, static HTTP functions for GET, POST, PUT, and DELETE             

        public async Task<T> GetAsync<T>(string id)
        {
            T data;
            client = httpClientFactory.CreateClient(ClientName);
            try
            {
                using (HttpResponseMessage response = await client.GetAsync(id))
                using (HttpContent content = response.Content)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ThirdPartyApiException($"API at this address {id} failed.");
                    }
                    string d = await content.ReadAsStringAsync();
                    if (d != null)
                    {
                        data = JsonConvert.DeserializeObject<T>(d);
                        return (T)data;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            Object o = new Object();
            return (T)o;
        }

        public async Task<T> PostAsync<T>(HttpContent contentPost)
        {
            T data;
            //string url = "https://api.funtranslations.com/translate/shakespeare";
            client = httpClientFactory.CreateClient(ClientName);
            using (HttpResponseMessage response = await client.PostAsync(client.BaseAddress, contentPost))
            using (HttpContent content = response.Content)
            {

                if (!response.IsSuccessStatusCode)
                {
                    throw new ThirdPartyApiException($"API at this address {client.BaseAddress} failed.");
                }

                string d = await content.ReadAsStringAsync();
                if (d != null)
                {
                    data = JsonConvert.DeserializeObject<T>(d);
                    return (T)data;
                }
            }
            Object o = new Object();
            return (T)o;
        }

        public async Task<T> PutAsync<T>(string url, HttpContent contentPut)
        {
            T data;
            client = httpClientFactory.CreateClient(ClientName);

            using (HttpResponseMessage response = await client.PutAsync(url, contentPut))
            using (HttpContent content = response.Content)
            {
                string d = await content.ReadAsStringAsync();
                if (d != null)
                {
                    data = JsonConvert.DeserializeObject<T>(d);
                    return (T)data;
                }
            }
            Object o = new Object();
            return (T)o;
        }

        public async Task<T> DeleteAsync<T>(string url)
        {
            T newT;
            client = httpClientFactory.CreateClient(ClientName);

            using (HttpResponseMessage response = await client.DeleteAsync(url))
            using (HttpContent content = response.Content)
            {
                string data = await content.ReadAsStringAsync();
                if (data != null)
                {
                    newT = JsonConvert.DeserializeObject<T>(data);
                    return newT;
                }
            }
            Object o = new Object();
            return (T)o;
        }

        public class ThirdPartyApiException : Exception
        {
            public ThirdPartyApiException() : base() { }
            public ThirdPartyApiException(string message) : base(message) { }
            public ThirdPartyApiException(string message, Exception inner) : base(message, inner) { }

            // A constructor is needed for serialization when an
            // exception propagates from a remoting server to the client.
            protected ThirdPartyApiException(System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
        #endregion
    }
}
