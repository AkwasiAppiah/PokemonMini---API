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
                // Add to config
                const string endpoint = "https://api.funtranslations.com/translate/shakespeare";

                var description = pokemonToTranslate.Description;

                var jsonContent = new Dictionary<string, string>()
                {
                    {"text", $"{description}"}
                };

                var content = new FormUrlEncodedContent(jsonContent);

                string responseBody = "";
                HttpStatusCode statusCode = HttpStatusCode.OK;

                using (var httpClient = _httpClientFactory.CreateClient()) {

                    var result = await httpClient.PostAsync(endpoint, content);

                    if (!result.IsSuccessStatusCode)
                    {
                        return new ServiceResult<ModelPokemon>()
                        {
                            HttpStatusCode = result.StatusCode,
                            ErrorMessage = "Shakespeare API failed",
                        };
                    }

                    responseBody = await result.Content.ReadAsStringAsync();
                    statusCode = result.StatusCode;
                }

                var shakespeareObject = JsonSerializer.Deserialize<YodaApiResponseJson>(responseBody);

                var translatedText = shakespeareObject.contents.translated;

                var translatedShakespeareModel = pokemonToTranslate;

                translatedShakespeareModel.Description = translatedText;

                return new ServiceResult<ModelPokemon>()
                {
                    HttpStatusCode = statusCode,
                    Data = translatedShakespeareModel
                };
            }
            catch (Exception e)
            {
                return new ServiceResult<ModelPokemon>()
                {
                    HttpStatusCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = e.Message,
                };
            }
        }
    }
}