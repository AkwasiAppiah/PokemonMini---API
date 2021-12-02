using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonMiniTest.HTTPClientHelpers
{
    public class PokemonHTTPClientHelper : IPokemonHTTPClientHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly String _clientName;

        public PokemonHTTPClientHelper(IHttpClientFactory httpClientFactory, string clientName)
        {
            _httpClientFactory = httpClientFactory;
            _clientName = clientName;
        }

        #region Generic, Async, static HTTP functions for GET, POST, PUT, and DELETE             

        public async Task<T> GetAsync<T>(string pokemonName)
        {
            T data;
            var client = _httpClientFactory.CreateClient(_clientName);
            try
            {
                using (HttpResponseMessage response = await client.GetAsync($"pokemon-species/{pokemonName}"))
                using (HttpContent content = response.Content)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ThirdPartyApiException($"Pokemon Api failed.");
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
            var client = _httpClientFactory.CreateClient(_clientName);
            using (HttpResponseMessage response = await client.PostAsync("/", contentPost))
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
            var client = _httpClientFactory.CreateClient(_clientName);

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
            var client = _httpClientFactory.CreateClient(_clientName);

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
