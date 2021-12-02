using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokemonMiniTest.HTTPClientHelpers
{
    public interface IHTTPClientHelper
    {
        Task<T> DeleteAsync<T>(string id);
        Task<T> GetAsync<T>(string id);
        Task<T> PostAsync<T>(HttpContent contentPost);
        Task<T> PutAsync<T>(string id, HttpContent contentPut);
    }
}
