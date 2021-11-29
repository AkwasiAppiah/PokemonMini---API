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
        private readonly IHttpClientFactory _httpClientFactory;

        public ShakespeareTranslationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ServiceResult<ModelPokemon>> TranslateShakespeareAsyncTask(ModelPokemon pokemonToTranslate)
        {
            try
            {
                const string endpoint = "https://api.funtranslations.com/translate/shakespeare";

                var description = pokemonToTranslate.Description;

                var jsonContent = new Dictionary<string, string>()
                {
                    {"text", $"{description}"}
                };

                var content = new FormUrlEncodedContent(jsonContent);


                var httpClient = _httpClientFactory.CreateClient();

                var result = await httpClient.PostAsync(endpoint, content);

                if (!result.IsSuccessStatusCode)
                {
                    return new ServiceResult<ModelPokemon>()
                    {
                        HttpStatusCode = result.StatusCode,
                        ErrorMessage = "Shakespeare API failed",
                        Data = null
                    };
                }

                var responseBody = await result.Content.ReadAsStringAsync();

                var yodaObject = JsonSerializer.Deserialize<YodaApiResponseJson>(responseBody);

                var translatedText = yodaObject.contents.translated;

                var translatedYodaModel = pokemonToTranslate;

                translatedYodaModel.Description = translatedText;

                return new ServiceResult<ModelPokemon>()
                {
                    HttpStatusCode = result.StatusCode,
                    ErrorMessage = string.Empty,
                    Data = translatedYodaModel
                };
            }
            catch (Exception e)
            {
                return new ServiceResult<ModelPokemon>()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = e.Message,
                    Data = null
                };
            }
        }
    }
}