using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using PokemonMiniTest.HTTPClientHelpers;
using PokemonMiniTest.Models;

namespace PokemonMiniTest.Services
{
    public class YodaTranslationService : IYodaTranslationService
    {
        private readonly IYodaHTTPClientHelper _hTTPClientHelper;

        public YodaTranslationService(IYodaHTTPClientHelper hTTPClientHelper)
        {
            //_httpClientFactory = httpClientFactory;
            _hTTPClientHelper = hTTPClientHelper;
        }
        public async Task<ServiceResult<ModelPokemon>> GetTranslatedYodaPokemonModel(ModelPokemon pokemonToTranslate)
        {
            try
            {
                var description = pokemonToTranslate.Description;

                var jsonContent = new Dictionary<string, string>()
                {
                    {"text", $"{description}"}
                };

                var content = new FormUrlEncodedContent(jsonContent);

                var yodaObject = await _hTTPClientHelper.PostAsync<TranslationAPIResponseJson>(content);

                if(yodaObject.Contents == null)
                {
                    return new ServiceResult<ModelPokemon>
                    {
                        ErrorMessage = "External API could not translate this text for some reason"
                    };
                }

                var translatedText = yodaObject.Contents.Translated;

                pokemonToTranslate.Description = translatedText;

                return new ServiceResult<ModelPokemon>()
                {
                    Data = pokemonToTranslate
                };
            }
            catch (Exception exception)
            {
                return new ServiceResult<ModelPokemon>()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = exception.Message,
                };
            }
        }
    }
}
