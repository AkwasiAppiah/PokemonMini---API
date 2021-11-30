﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using PokemonMiniTest.Models;

namespace PokemonMiniTest.Services
{
    public class YodaTranslationService : IYodaTranslationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public YodaTranslationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ServiceResult<ModelPokemon>> GetTranslatedYodaPokemonModel(ModelPokemon pokemonToTranslate)
        {

            try
            {
                // Add to config
                const string endpoint = "https://api.funtranslations.com/translate/yoda";

                var description = pokemonToTranslate.Description;

                var jsonContent = new Dictionary<string, string>
                {
                    {"text", $"{description}"}
                };

                var content = new FormUrlEncodedContent(jsonContent);

                string responseBody = "";
                HttpStatusCode statusCode = HttpStatusCode.OK;

                using(var httpClient = _httpClientFactory.CreateClient())
                {
                    var result = await httpClient.PostAsync(endpoint, content);

                    if (!result.IsSuccessStatusCode)
                    {
                        return new ServiceResult<ModelPokemon>()
                        {
                            HttpStatusCode = result.StatusCode,
                            ErrorMessage = "External Service Error",
                            Data = null
                        };
                    }
                    responseBody = await result.Content.ReadAsStringAsync();
                    statusCode = result.StatusCode;
                }

                var yodaObject = JsonSerializer.Deserialize<YodaApiResponseJson>(responseBody);

                var translatedText = yodaObject.contents.translated;

                pokemonToTranslate.Description = translatedText;


                return new ServiceResult<ModelPokemon>()
                {
                    HttpStatusCode = statusCode,
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
