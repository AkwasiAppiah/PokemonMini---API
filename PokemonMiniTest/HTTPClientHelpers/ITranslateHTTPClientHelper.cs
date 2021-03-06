using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonMiniTest.HTTPClientHelpers
{
    public interface ITranslateHTTPClientHelper : IHTTPClientHelper
    {
        Task<T> GetAsync<T>(string url, string id);
    }
}
