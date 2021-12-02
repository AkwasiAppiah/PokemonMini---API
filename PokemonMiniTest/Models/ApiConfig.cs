using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonMiniTest.Models
{
    public class ApiConfig
    {
        public const string Api = "Api";
        public string PokemonBaseUrl { get; set;} = string.Empty;
        public int Version { get; set;} = (int)default;
    }
}
