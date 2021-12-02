using PokemonMiniTest.HTTPClientHelpers;
using PokemonMiniTest.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokemonMiniTest.Services
{
    public class ShakespeareTranslationService : IShakespeareTranslationService
    {
        private readonly IShakespeareHTTPClientHelper _hTTPClientHelper;

        public ShakespeareTranslationService(IShakespeareHTTPClientHelper hTTPClientHelper)
        {
            _hTTPClientHelper = hTTPClientHelper;
        }
        public async Task<ServiceResult<ModelPokemon>> TranslateShakespeareAsyncTask(ModelPokemon pokemonToTranslate)
        {
            try
            {
                var description = pokemonToTranslate.Description;

                var jsonContent = new Dictionary<string, string>()
                {
                    {"text", $"{description}"}
                };

                var content = new FormUrlEncodedContent(jsonContent);

                var shakespeareObject = await _hTTPClientHelper.PostAsync<TranslationAPIResponseJson>(content);


                if (shakespeareObject.Contents == null)
                {
                    return new ServiceResult<ModelPokemon>
                    {
                        ErrorMessage = "External API could not translate this text for some reason"
                    };
                }

                var translatedText = shakespeareObject.Contents.Translated;

                var translatedShakespeareModel = pokemonToTranslate;

                translatedShakespeareModel.Description = translatedText;

                return new ServiceResult<ModelPokemon>(translatedShakespeareModel);
            }
            catch (Exception e)
            {
                return new ServiceResult<ModelPokemon>(e.Message);
            }
        }
    }
}
