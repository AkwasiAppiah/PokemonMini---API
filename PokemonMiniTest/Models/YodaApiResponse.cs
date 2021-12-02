using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonMiniTest.Models
{

    public class TranslationAPIResponseJson
        {
            public Success Success { get; set; }
            public Contents Contents { get; set; }
        }

        public class Success
        {
            public int Total { get; set; }
        }

        public class Contents
        {
            [JsonProperty("translated")]
            public string Translated { get; set; }
            public string Text { get; set; }
            public string Translation { get; set; }
        }

}

