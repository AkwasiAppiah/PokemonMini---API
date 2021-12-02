using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonMiniTest.HTTPClientHelpers
{
    public class TranslateHttpClientHelper : ITranslateHTTPClientHelper
    {
        IHttpClientFactory _httpClientFactory;
        String _clientName;

        public Task<T> DeleteAsync<T>(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync<T>(string url, string id)
        {
            T data;
            var client = _httpClientFactory.CreateClient(_clientName);
            try
            {
                using (HttpResponseMessage response = await client.GetAsync($"{url}/{id}"))
                using (HttpContent content = response.Content)
                {
                    //if (!response.IsSuccessStatusCode)
                    //{
                    //    throw new ThirdPartyApiException($"Pokemon Api failed.");
                    //}
                    //string d = await content.ReadAsStringAsync();
                    //if (d != null)
                    //{
                    //    data = JsonConvert.DeserializeObject<T>(d);
                    //    return (T)data;
                    //}
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            Object o = new Object();
            return (T)o;
        }

        public Task<T> GetAsync<T>(string id)
        {
            throw new NotImplementedException();
        }

        public Task<T> PostAsync<T>(HttpContent contentPost)
        {
            throw new NotImplementedException();
        }

        public Task<T> PutAsync<T>(string id, HttpContent contentPut)
        {
            throw new NotImplementedException();
        }
    }
}
